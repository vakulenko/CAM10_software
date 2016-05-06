using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.cam10_v05
{
    public partial class camSettings : Form
    {
        const short minGain = 0;
        const short maxGain = 15;
        const short minOffset = -63;
        const short maxOffset = 63;
        const short minBlevel = 0;
        const short maxBlevel = 25;
        const short minHistStretchBits = 8;
        const short maxHistStretchBits = 16;

        const short CameraStatusOperational = 0;
        const short CameraStatusWarning = 1;
        const short CameraStatusFailed = 2;

        public camSettings()
        {
            InitializeComponent();
        }

        private short pGain = 0;
        private short pOffset = 0;
        private short pBlevel = 0;
        private short pHistStretchBits = 8;

        public short gain
        {
            get
            {
                return pGain;
            }
            set
            {
                if ((value >= minGain) && (value <= maxGain))
                {
                    pGain = value;
                    this.gainNumUpDown.Value = pGain;
                    this.gainTrackBar.Value = pGain;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, gain");
            }
        }
        public short offset
        {
            get
            {
                return pOffset;
            }
            set
            {
                if ((value >= minOffset) && (value <= maxOffset))
                {
                    pOffset = value;
                    this.offsetNumUpDown.Value = pOffset;
                    this.offsetTrackBar.Value = pOffset;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, offset");
            }
        }

        public bool enableControl
        {
            set
            {
                panel.Enabled = value;
            }
        }

        public short blevel
        {
            get
            {
                return pBlevel;
            }
            set
            {
                if ((value >= minBlevel) && (value <= maxBlevel))
                {
                    pBlevel = value;
                    this.blevelNumUpDown.Value = pBlevel;
                    this.blevelTrackBar.Value = pBlevel;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, blevel");
            }
        }

        public bool onTop
        {
            get
            {
                return onTopCheckBox.Checked;
            }
            set
            {
                onTopCheckBox.Checked = value;
            }
        }

        public short histStretch
        {
            get
            {
                return pHistStretchBits;
            }
            set
            {
                if ((value >= minHistStretchBits) && (value <= maxHistStretchBits))
                {
                    pHistStretchBits = value;
                    this.histStretchUpDown.Value = value;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, histStretch");
            }
        }

        public int cameraError
        {
            set
            {
                switch (value)
                {
                    case CameraStatusOperational:
                        {
                            this.BackColor = SystemColors.Control;
                            this.cameraStatusLabel.Text = "Camera status: operational";
                            break;
                        };
                    case CameraStatusWarning:
                        {
                            this.BackColor = System.Drawing.Color.Yellow;
                            this.cameraStatusLabel.Text = "Camera status: warning";
                            break;
                        };
                    default:
                        {
                            this.BackColor = System.Drawing.Color.Yellow;
                            this.cameraStatusLabel.Text = "Camera status: failed";
                            break;
                        };
                }
            }
        }

        public bool autoOffset
        {
            get
            {
                return autoOffsetCheckBox.Checked;
            }
            set
            {
                autoOffsetCheckBox.Checked = value;
                offsetTrackBar.Enabled = offsetNumUpDown.Enabled = !value;
            }
        }

        public bool overscan
        {
            get
            {
                return OverscanCheckBox.Checked;
            }
            set 
            {
                OverscanCheckBox.Checked = value;
            }
        }

        private void GainTrackBar_Scroll(object sender, EventArgs e)
        {
            gainNumUpDown.Value = gainTrackBar.Value;
        }

        private void OffsetTrackBar_Scroll(object sender, EventArgs e)
        {
            offsetNumUpDown.Value = offsetTrackBar.Value;
        }

        private void BlevelTrackBar_Scroll(object sender, EventArgs e)
        {
            blevelNumUpDown.Value = blevelTrackBar.Value;
        }

        private void OnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onTopCheckBox.Checked) this.TopMost = true;
            else this.TopMost = false;
        }

        private void gainNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            gainTrackBar.Value = gain = (short)gainNumUpDown.Value;
        }

        private void offsetNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            offsetTrackBar.Value = offset = (short)offsetNumUpDown.Value;
        }

        private void blevelNumUpDown_ValueChanged(object sender, EventArgs e)
        {
            blevelTrackBar.Value = blevel = (short)blevelNumUpDown.Value;
        }

        private void camSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void autoOffsetCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            offsetTrackBar.Enabled = offsetNumUpDown.Enabled = !autoOffsetCheckBox.Checked;
        }

        private void histStretchUpDown_ValueChanged(object sender, EventArgs e)
        {
            pHistStretchBits = (short)histStretchUpDown.Value;
        }
    }
}
