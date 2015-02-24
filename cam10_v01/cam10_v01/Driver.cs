// --------------------------------------------------------------------------------
// ASCOM Camera driver for cam10 v.0.1
// Edit Log:
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 16-feb-2015  VSS 0.1     Initial release
// --------------------------------------------------------------------------------

#define Camera

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;

// work with files
using System.IO;
// class savings
using System.Xml.Serialization; 

namespace ASCOM.cam10_v01
{
    /// <summary>
    /// ASCOM Camera Driver for cam10_v01.
    /// </summary>
    [Guid("23790512-dbde-4359-8ef9-a90fda6da6bc")]
    [ClassInterface(ClassInterfaceType.None)]

    /// <summary>
    /// Class for saving settings (serializing)
    /// </summary>
    public class iniSettingsClass
    {
        public short gain;
        public short offset;
        public short blevel;
        public bool onTop;
        public bool autoOffset;

        public iniSettingsClass()
        {
            gain = 0;
            offset = 0;
            blevel = 0;
            onTop = false;
            autoOffset = false;
        }
    }

    public class Camera : ICameraV2
    { 
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.cam10_v01.Camera";
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "Cam10 v.0.1 ASCOM Driver";

        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        internal static bool traceState;

        /// <summary>
        /// Form, handle gain/offset settings
        /// </summary>
        private camSettings settingsForm;

        /// <summary>
        /// FilePath to settings XML (save gain/offset value)
        /// </summary>
        private string settingFilePath;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        private TraceLogger tl;

        //Imports cam10ll01.dll functions
        [DllImport("cam10ll01.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern bool cameraConnect();
        [DllImport("cam10ll01.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern bool cameraDisconnect();
        [DllImport("cam10ll01.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern bool cameraIsConnected();
        [DllImport("cam10ll01.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern bool cameraStartExposure(double Duration, int gain, int offset, bool autoOffset, int blevel);
        [DllImport("cam10ll01.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern int cameraGetCameraState();
        [DllImport("cam10ll01.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern bool cameraGetImageReady();
        [DllImport("cam10ll01.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        static extern uint cameraGetImage();

        /// <summary>
        /// Initializes a new instance of the <see cref="cam10_v01"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Camera()
        {
            // Read device configuration from the ASCOM Profile store
            ReadProfile();
            //Get path to Common Files directory
            string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE").ToString();
            if (arch.IndexOf("86") != -1) settingFilePath = Environment.ExpandEnvironmentVariables("%CommonProgramFiles%\\ASCOM\\Camera\\cam10\\cam10_settings.xml");
            else settingFilePath = Environment.ExpandEnvironmentVariables("%CommonProgramFiles(x86)%\\ASCOM\\Camera\\cam10\\cam10_settings.xml");
            //Init debug logger
            tl = new TraceLogger("", "cam10_v01");
            tl.Enabled = traceState;
            tl.LogMessage("Camera", "Starting initialisation");
            // Initialise connected to false
            connectedState = false;
            //Initialise util object
            utilities = new Util();
            // Initialise astro utilities object
            astroUtilities = new AstroUtils(); 
            //New form for gain/offset settings
            settingsForm = new camSettings();
            //extract gain, offset settings
            tl.LogMessage("Camera", "Reading gain/offset/blevel settings from file " + settingFilePath);
            tl.LogMessage("Camera", "Also in Win with UAC check /Users/AppData/Local/VirtualStore/Program Files (x86)/Common Files/ASCOM/Camera/cam10/ dir");
            if (File.Exists(settingFilePath))
            {
                try
                {
                    using (Stream stream = new FileStream(settingFilePath, FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(iniSettingsClass));

                        iniSettingsClass iniSettings = (iniSettingsClass)serializer.Deserialize(stream);
                        settingsForm.gain = iniSettings.gain;
                        settingsForm.offset = iniSettings.offset;
                        settingsForm.blevel = iniSettings.blevel;
                        settingsForm.onTop = iniSettings.onTop;
                        settingsForm.autoOffset = iniSettings.autoOffset;                            
                    }
                }
                catch
                {                    
                    settingsForm.gain = 0;
                    settingsForm.offset = 0;
                    settingsForm.blevel = 0;
                    settingsForm.onTop = false;
                    settingsForm.autoOffset = false;
                }
            }
            tl.LogMessage("Camera", "Read gain/offset/blevel settings from file; gain=" + settingsForm.gain.ToString() + " offset=" + settingsForm.offset.ToString() + " autoOffset" + settingsForm.autoOffset.ToString() + " blevel=" + settingsForm.blevel.ToString() + " onTop="+settingsForm.onTop.ToString());
            tl.LogMessage("Camera", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE ICameraV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // Persist device configuration values to the ASCOM Profile store
                    WriteProfile(); 
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            tl.LogMessage("Action", "Not implemented");
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            tl.LogMessage("CommandBlind", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            tl.LogMessage("CommandBool", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            tl.LogMessage("CommandString", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the tracelogger, settings form and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
            settingsForm.Dispose();
        }

        public bool Connected
        {
            get
            {
                tl.LogMessage("Connected Get", IsConnected.ToString());
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected Set", value.ToString());
                if (value == IsConnected)
                    return;

                if (value)
                {
                    tl.LogMessage("Connected Set", "Connecting to camera, call cameraConnect from cam10ll01.dll");
                    if (cameraConnect() == false)
                    {
                        tl.LogMessage("Connected Set", "Cant connect to cam10");
                        throw new ASCOM.NotConnectedException("Cant connect to cam10");
                    }
                    tl.LogMessage("Connected Set", "connectedState=true");
                    connectedState = true;
                    settingsForm.Show();                   
                }
                else
                {
                    tl.LogMessage("Connected Set", "Disconnecting from camera, call cameraConnect from cam10ll01.dll");
                    if (cameraDisconnect() == false)
                    {
                        tl.LogMessage("Connected Set", "Cant disconnect cam10");
                        throw new ASCOM.NotConnectedException("Cant disconnect cam10");
                    }
                    tl.LogMessage("Connected Set", "connectedState=false");
                    connectedState = false;
                    settingsForm.Hide();
                }
            }
        }

        public string Description
        {
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;                
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                tl.LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                tl.LogMessage("Name Get", "cam10");
                return "cam10";
            }
        }

        #endregion

        #region ICamera Implementation

        // Constants to define the ccd pixel dimenstions
        private const int ccdWidth = 1280; 
        private const int ccdHeight = 1024;
        // Constant for the pixel physical dimension um
        private const double pixelSize = 5.2;

        // Initialise variables to hold values required for functionality tested by Conform
        private int cameraNumX = ccdWidth; 
        private int cameraNumY = ccdHeight;
        private int cameraStartX = 0;
        private int cameraStartY = 0;
        private short cameraBinX = 1;
        private short cameraBinY = 1;
        private DateTime exposureStart = DateTime.MinValue;
        private double cameraLastExposureDuration = 0.0;

        private bool cameraImageReady = false;
        private Array cameraImageArray;

        public void AbortExposure()
        {
            tl.LogMessage("AbortExposure", "Not implemented");
            throw new MethodNotImplementedException("AbortExposure");
        }

        public short BayerOffsetX
        {
            get
            {
                tl.LogMessage("BayerOffsetX Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", false);
            }
        }

        public short BayerOffsetY
        {
            get
            {
                tl.LogMessage("BayerOffsetY Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("BayerOffsetX", true);
            }
        }

        public short BinX
        {
            get
            {
                tl.LogMessage("BinX Get", cameraBinX.ToString());
                return cameraBinX;
            }
            set
            {
                tl.LogMessage("BinX Set", value.ToString());
                if ((value < 1) || (value > this.MaxBinX))
                {
                    tl.LogMessage("BinX Set", "InvalidValueException BinX must be in range [1;MaxBinX]");
                    throw new ASCOM.InvalidValueException("BinX", value.ToString(),"BinX must be in range [1;MaxBinX]");
                } 
                cameraStartX=(cameraStartX * cameraBinX) / value;
                cameraNumX = (cameraNumX * cameraBinX) / value;
                cameraBinX = cameraBinY = value;
            }
        }

        public short BinY
        {
            get
            {
                tl.LogMessage("BinY Get", cameraBinY.ToString());
                return cameraBinY;
            }
            set
            {
                tl.LogMessage("BinY Set", value.ToString());
                if ((value < 1) || (value > this.MaxBinY))
                {
                    tl.LogMessage("BinY Set", "InvalidValueException BinY must be in range [1;MaxBinY]");
                    throw new ASCOM.InvalidValueException("BinY", value.ToString(), "BinY must be in range [1;MaxBinY]");
                }
                cameraStartY = (cameraStartY * cameraBinY) / value;
                cameraNumY = (cameraNumY * cameraBinY) / value;
                cameraBinY = cameraBinX = value;
            }
        }

        public double CCDTemperature
        {
            get
            {
                tl.LogMessage("CCDTemperature Get Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CCDTemperature", false);
            }
        }

        public CameraStates CameraState
        {
            get
            {
                tl.LogMessage("CameraState Get", "Call cameraGetCameraState from cam10ll01.dll");
                switch ((short)cameraGetCameraState())
                {
                    case 0:
                        {
                            tl.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString());
                            return CameraStates.cameraIdle;
                        }
                    case 1:
                        {
                            tl.LogMessage("CameraState Get", CameraStates.cameraWaiting.ToString());
                            return CameraStates.cameraWaiting;
                        }
                    case 2:
                        {
                            tl.LogMessage("CameraState Get", CameraStates.cameraExposing.ToString());
                            return CameraStates.cameraExposing;
                        }
                    case 3:
                        {
                            tl.LogMessage("CameraState Get", CameraStates.cameraReading.ToString());
                            return CameraStates.cameraReading;
                        }
                    case 4:
                        {
                            tl.LogMessage("CameraState Get", CameraStates.cameraDownload.ToString());
                            return CameraStates.cameraDownload;
                        }
                    default:
                        {
                            tl.LogMessage("CameraState Get", CameraStates.cameraError.ToString());
                            return CameraStates.cameraError;
                        }
                }                                
            }
        }

        public int CameraXSize
        {
            get
            {
                tl.LogMessage("CameraXSize Get", ccdWidth.ToString());
                return ccdWidth;
            }
        }

        public int CameraYSize
        {
            get
            {
                tl.LogMessage("CameraYSize Get", ccdHeight.ToString());
                return ccdHeight;
            }
        }

        public bool CanAbortExposure
        {
            get
            {
                tl.LogMessage("CanAbortExposure Get", false.ToString());
                return false;
            }
        }

        public bool CanAsymmetricBin
        {
            get
            {
                tl.LogMessage("CanAsymmetricBin Get", false.ToString());
                return false;
            }
        }

        public bool CanFastReadout
        {
            get
            {
                tl.LogMessage("CanFastReadout Get", false.ToString());
                return false;
            }
        }

        public bool CanGetCoolerPower
        {
            get
            {
                tl.LogMessage("CanGetCoolerPower Get", false.ToString());
                return false;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                tl.LogMessage("CanPulseGuide Get", false.ToString());
                return false;
            }
        }

        public bool CanSetCCDTemperature
        {
            get
            {
                tl.LogMessage("CanSetCCDTemperature Get", false.ToString());
                return false;
            }
        }

        public bool CanStopExposure
        {
            get
            {
                tl.LogMessage("CanStopExposure Get", false.ToString());
                return false;
            }
        }

        public bool CoolerOn
        {
            get
            {
                tl.LogMessage("CoolerOn Get Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CoolerOn", false);
            }
            set
            {
                tl.LogMessage("CoolerOn Set Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CoolerOn", true);
            }
        }

        public double CoolerPower
        {
            get
            {
                tl.LogMessage("CoolerPower Get Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("CoolerPower", false);
            }
        }

        public double ElectronsPerADU
        {
            get
            {
                tl.LogMessage("ElectronsPerADU Get Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ElectronsPerADU", false);
            }
        }

        public double ExposureMax
        {
            get
            {
                tl.LogMessage("ExposureMax Get Get", "2.0");
                return 2.0;
            }
        }

        public double ExposureMin
        {
            get
            {
                tl.LogMessage("ExposureMin Get", "0.0");
                return 0.0;
            }
        }

        public double ExposureResolution
        {
            get
            {
                tl.LogMessage("ExposureResolution Get", "0.001");
                return 0.001;
            }
        }

        public bool FastReadout
        {
            get
            {
                tl.LogMessage("FastReadout Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("FastReadout", false);
            }
            set
            {
                tl.LogMessage("FastReadout Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("FastReadout", true);
            }
        }

        public double FullWellCapacity
        {
            get
            {
                tl.LogMessage("FullWellCapacity Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("FullWellCapacity", false);
            }
        }

        public short Gain
        {
            get
            {
                tl.LogMessage("Gain Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Gain", false);
            }
            set
            {
                tl.LogMessage("Gain Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Gain", true);
            }
        }

        public short GainMax
        {
            get
            {
                tl.LogMessage("GainMax Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GainMax", false);
            }
        }

        public short GainMin
        {
            get
            {
                tl.LogMessage("GainMin Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GainMin", true);
            }
        }

        public ArrayList Gains
        {
            get
            {
                tl.LogMessage("Gains Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("Gains", true);
            }
        }

        public bool HasShutter
        {
            get
            {
                tl.LogMessage("HasShutter Get", false.ToString());
                return false;
            }
        }

        public double HeatSinkTemperature
        {
            get
            {
                tl.LogMessage("HeatSinkTemperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("HeatSinkTemperature", false);
            }
        }

        public object ImageArray
        {
            get
            {
                if (!cameraImageReady)
                {
                    tl.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!");
                }

                uint imagepoint;
                //Get image pointer
                tl.LogMessage("ImageArray Get", "Call cameraGetImage from cam10ll01.dll");
                imagepoint = cameraGetImage();
                unsafe
                {
                    ushort* zeropixelpoint, pixelpoint;
                    //Set pixelpointers
                    zeropixelpoint = pixelpoint = (ushort*)imagepoint;
                    //Create image array
                    cameraImageArray = Array.CreateInstance(typeof(int), cameraNumX * cameraNumY);
                    int i, j, bini, binj, k=0, binSumm=0;

                    if (cameraBinX == 1)
                    {
                        for (j = cameraStartY; j < (cameraStartY + cameraNumY); j++)
                            for (i = cameraStartX; i < (cameraStartX + cameraNumX); i++)
                            {
                                pixelpoint = (ushort*)(zeropixelpoint + (j*(ccdWidth) + i));
                                cameraImageArray.SetValue(*pixelpoint, k);
                                k++;
                            }
                    }
                    else
                    {
                        for (j=cameraStartY*cameraBinY; j<(cameraStartY + cameraNumY)*cameraBinY;j=j+cameraBinY)
                            for (i = cameraStartX * cameraBinX; i < (cameraStartX + cameraNumX) * cameraBinX; i = i + cameraBinX)
                            {
                                binSumm = 0;
                                for (binj = 0; binj < cameraBinY; binj++)
                                    for (bini = 0; bini < cameraBinX; bini++)
                                    {
                                        pixelpoint = (ushort*)(zeropixelpoint + ( (j+binj) * (ccdWidth) + (i+bini) ));
                                        binSumm += *pixelpoint;
                                    }
                                cameraImageArray.SetValue(binSumm, k);
                                k++;
                            }
                    }                    
                }                                   
                return cameraImageArray;
            }
        }

        public object ImageArrayVariant
        {
            get
            {
                if (!cameraImageReady)
                {
                    tl.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!");
                }
                tl.LogMessage("ImageArrayVariant Get", "Call ImageArray method");
                return this.ImageArray;
            }
        }

        public bool ImageReady
        {
            get
            {
                tl.LogMessage("ImageReady Get", "Call cameraGetImageReady from cam10ll01.dll");
                cameraImageReady = cameraGetImageReady();
                tl.LogMessage("ImageReady Get", cameraImageReady.ToString());
                return cameraImageReady;
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                tl.LogMessage("IsPulseGuiding Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("IsPulseGuiding", false);
            }
        }

        public double LastExposureDuration
        {
            get
            {
                if (!cameraImageReady)
                {
                    tl.LogMessage("LastExposureDuration Get", "Throwing InvalidOperationException because of a call to LastExposureDuration before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to LastExposureDuration before the first image has been taken!");
                }
                tl.LogMessage("LastExposureDuration Get", cameraLastExposureDuration.ToString());
                return cameraLastExposureDuration;
            }
        }

        public string LastExposureStartTime
        {
            get
            {
                if (!cameraImageReady)
                {
                    tl.LogMessage("LastExposureStartTime Get", "Throwing InvalidOperationException because of a call to LastExposureStartTime before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to LastExposureStartTime before the first image has been taken!");
                }
                string exposureStartString = exposureStart.ToString("yyyy-MM-ddTHH:mm:ss");
                tl.LogMessage("LastExposureStartTime Get", exposureStartString.ToString());
                return exposureStartString;
            }
        }

        public int MaxADU
        {
            get
            {
                tl.LogMessage("MaxADU Get", (256 * cameraBinX * cameraBinX - 1).ToString());
                return 256*cameraBinX*cameraBinX-1;
            }
        }

        public short MaxBinX
        {
            get
            {
                tl.LogMessage("MaxBinX Get", "4");
                return 4;
            }
        }

        public short MaxBinY
        {
            get
            {
                tl.LogMessage("MaxBinY Get", "4");
                return 4;
            }
        }

        public int NumX
        {
            get
            {
                tl.LogMessage("NumX Get", cameraNumX.ToString());
                return cameraNumX;
            }
            set
            {
                tl.LogMessage("NumX set", value.ToString());
                if ((value < 1) || (value > (ccdWidth / cameraBinX)))
                {
                    tl.LogMessage("NumX set", "InvalidValueException NumX must be in range [1;ccdWidth/cameraBinX]");
                    throw new InvalidValueException("NumX Set", value.ToString(), "NumX must be in range [1;ccdWidth/cameraBinX]");
                }
                cameraNumX = value;                
            }
        }

        public int NumY
        {
            get
            {
                tl.LogMessage("NumY Get", cameraNumY.ToString());
                return cameraNumY;
            }
            set
            {
                tl.LogMessage("NumY set", value.ToString());
                if ((value < 1) || (value > (ccdHeight / cameraBinY)))
                {
                    tl.LogMessage("NumY set", "InvalidValueException NumY must be in range [1;ccdHeight/cameraBinY]");
                    throw new InvalidValueException("NumY Set", value.ToString(), "NumY must be in range [1;ccdHeight/cameraBinY]");
                }                
                cameraNumY = value;
            }
        }

        public short PercentCompleted
        {
            get
            {
                tl.LogMessage("PercentCompleted Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("PercentCompleted", false);
            }
        }

        public double PixelSizeX
        {
            get
            {
                tl.LogMessage("PixelSizeX Get", pixelSize.ToString());
                return pixelSize;
            }
        }

        public double PixelSizeY
        {
            get
            {
                tl.LogMessage("PixelSizeY Get", pixelSize.ToString());
                return pixelSize;
            }
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            tl.LogMessage("PulseGuide", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("PulseGuide");
        }

        public short ReadoutMode
        {
            get
            {
                tl.LogMessage("ReadoutMode Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ReadoutMode", false);
            }
            set
            {
                tl.LogMessage("ReadoutMode Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ReadoutMode", true);
            }
        }

        public ArrayList ReadoutModes
        {
            get
            {
                tl.LogMessage("ReadoutModes Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ReadoutModes", false);
            }
        }

        public string SensorName
        {
            get
            {
                tl.LogMessage("SensorName Get", "MT9M001C12STM");
                return "MT9M001C12STM";
            }
        }

        public SensorType SensorType
        {
            get
            {
                tl.LogMessage("SensorType Get", "SensorType.Monochrome");
                return SensorType.Monochrome;
            }
        }

        public double SetCCDTemperature
        {
            get
            {
                tl.LogMessage("SetCCDTemperature Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", false);
            }
            set
            {
                tl.LogMessage("SetCCDTemperature Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SetCCDTemperature", true);
            }
        }

        public void StartExposure(double Duration, bool Light)
        {
            //check exposure parameters
            tl.LogMessage("StartExposure","Duration="+Duration.ToString()+" Light="+Light.ToString());
            if ((Duration < ExposureMin) || (Duration > ExposureMax))
            {
                tl.LogMessage("StartExposure", "InvalidValueException Duration must be in range [MinExposure;MaxExposure]");
                throw new InvalidValueException("StartExposure", Duration.ToString(), "Duration must be in range [MinExposure;MaxExposure]");
            }
            if ((cameraStartX + cameraNumX) > (ccdWidth / cameraBinX))
            {
                tl.LogMessage("StartExposure", "InvalidValueException (cameraStartX + cameraNumX) must be < (ccdWidth / cameraBinX)");
                throw new InvalidValueException("StartExposure", (cameraStartX + cameraNumX).ToString(), "(cameraStartX + cameraNumX) must be < (ccdWidth / cameraBinX)");
            }
            if ((cameraStartY + cameraNumY) > (ccdHeight / cameraBinY))
            {
                tl.LogMessage("StartExposure", "InvalidValueException (cameraStartY + cameraNumY) must be < (ccdHeight / cameraBinY)");
                throw new InvalidValueException("StartExposure", (cameraStartY + cameraNumY).ToString(), "(cameraStartY + cameraNumY) must be < (ccdHeight / cameraBinY)");
            }
            //save gain, offset settings
            tl.LogMessage("StartExposure", "Saving gain/offset/blevel settings to "+settingFilePath);
            tl.LogMessage("StartExposure", "Also in Win with UAC check /Users/AppData/Local/VirtualStore/Program Files (x86)/Common Files/ASCOM/Camera/cam10/ dir");
            tl.LogMessage("StartExposure", "Saving gain/offset/blevel settings to file; gain=" + settingsForm.gain.ToString() + " offset=" + settingsForm.offset.ToString() + " blevel=" + settingsForm.blevel.ToString() + " onTop=" + settingsForm.onTop.ToString());
            iniSettingsClass iniSettings = new iniSettingsClass();
            iniSettings.gain = settingsForm.gain;
            iniSettings.offset = settingsForm.offset;
            iniSettings.blevel = settingsForm.blevel;
            iniSettings.onTop = settingsForm.onTop;
            iniSettings.autoOffset = settingsForm.autoOffset;
            using (Stream writer = new FileStream(settingFilePath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(iniSettingsClass));
                serializer.Serialize(writer, iniSettings);
            }
            //Save parameters
            cameraLastExposureDuration = Duration;
            exposureStart = DateTime.Now;
            //start exposure
            tl.LogMessage("StartExposure", "Call cameraStartExposure from cam10ll01.dll, args: ");
            tl.LogMessage("StartExposure",  " Duration=" + Duration.ToString() +                                             
                                            " gain=" + settingsForm.gain.ToString() +
                                            " offset=" + settingsForm.offset.ToString() +
                                            " autoOffset=" + settingsForm.autoOffset.ToString() +
                                            " blevel=" + settingsForm.blevel.ToString());
            cameraStartExposure(Duration, settingsForm.gain, settingsForm.offset, settingsForm.autoOffset, settingsForm.blevel);
        }

        public int StartX
        {
            get
            {
                tl.LogMessage("StartX Get", cameraStartX.ToString());
                return cameraStartX;
            }
            set
            {
                tl.LogMessage("StartX Set", value.ToString());
                if ((value < 0) || (value >= (ccdWidth / cameraBinX)))
                {
                    tl.LogMessage("StartX Set", "InvalidValueException StartX must be in range [0;ccdWidth/cameraBinX)");
                    throw new InvalidValueException("StartX Set", value.ToString(), "StartX must be in range [0;ccdWidth/cameraBinX)");
                }               
                cameraStartX = value;
            }
        }

        public int StartY
        {
            get
            {
                tl.LogMessage("StartY Get", cameraStartY.ToString());
                return cameraStartY;
            }
            set
            {
                tl.LogMessage("StartY set", value.ToString());
                if ((value < 0) || (value >= (ccdHeight / cameraBinY)))
                {
                    tl.LogMessage("StartY Set", "InvalidValueException StartY must be in range [0;ccdHeight/cameraBinY)");
                    throw new InvalidValueException("StartY Set", value.ToString(), "StartY must be in range [0;ccdHeight/cameraBinY)");
                }               
                cameraStartY = value;
            }
        }

        public void StopExposure()
        {
            tl.LogMessage("StopExposure", "Not implemented");
            throw new MethodNotImplementedException("StopExposure");
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Camera";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                tl.LogMessage("IsConnected Get", "Call cameraIsConnected from cam10ll01.dll");
                connectedState = cameraIsConnected();
                tl.LogMessage("IsConnected Get", "connectedState=" + connectedState.ToString());
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                tl.LogMessage("CheckConnected", "connectedState=false" + message);
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString());
            }
        }

        #endregion

    }
}
