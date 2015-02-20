using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.cam10_v01
{
    public partial class camSettings : Form
    {
        const short minGain = 0;
        const short maxGain = 63;
        const short minOffset = -63;
        const short maxOffset = 63;
        const short minBlevel = 0;
        const short maxBlevel = 255;

        public camSettings()
        {
            InitializeComponent(); 
        }

        private short pGain = 0;
        private short pOffset = 0;
        private short pBlevel = 0;
        private bool pOnTop = false;

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
                    this.gainTextBox.Text = pGain.ToString();
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
                    this.offsetTextBox.Text = pOffset.ToString();
                    this.offsetTrackBar.Value = pOffset;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, offset");
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
                    this.blevelTextBox.Text = pBlevel.ToString();
                    this.blevelTrackBar.Value = pBlevel;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, offset");
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

        private void GainTrackBar_Scroll(object sender, EventArgs e)
        {
            gainTextBox.Text = gainTrackBar.Value.ToString();
        }

        private void OffsetTrackBar_Scroll(object sender, EventArgs e)
        {
            offsetTextBox.Text = offsetTrackBar.Value.ToString();
        }

        private void BlevelTrackBar_Scroll(object sender, EventArgs e)
        {
            blevelTextBox.Text = blevelTrackBar.Value.ToString();
        }

        private void GainTextBox_TextChanged(object sender, EventArgs e)
        {
            bool ConvRes;
            short ValNum;
            //Settings are correct?
            ConvRes = short.TryParse(gainTextBox.Text, out ValNum);
            if ((ConvRes == false) || (ValNum < minGain) || (ValNum > maxGain))
            {
                gainTrackBar.Value = gain = minGain;
                gainTextBox.Text = minGain.ToString();
                return;
            }
            gainTrackBar.Value = gain = short.Parse(gainTextBox.Text);
        }

        private void OffsetTextBox_TextChanged(object sender, EventArgs e)
        {
            bool ConvRes;
            short ValNum;
            //Settings are correct?
            ConvRes = short.TryParse(offsetTextBox.Text, out ValNum);
            if ((ConvRes == false) || (ValNum < minOffset) || (ValNum > maxOffset))
            {
                offsetTrackBar.Value = offset = minOffset;
                offsetTextBox.Text = minOffset.ToString();
                return;
            }
            offsetTrackBar.Value = offset = short.Parse(offsetTextBox.Text);
        }

        private void blevelTextBox_TextChanged(object sender, EventArgs e)
        {
            bool ConvRes;
            short ValNum;
            //Settings are correct?
            ConvRes = short.TryParse(blevelTextBox.Text, out ValNum);
            if ((ConvRes == false) || (ValNum < minBlevel) || (ValNum > maxBlevel))
            {
                blevelTrackBar.Value = blevel = minBlevel;
                blevelTextBox.Text = minBlevel.ToString();
                return;
            }
            blevelTrackBar.Value = pBlevel = short.Parse(blevelTextBox.Text);
        }

        private void OnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onTopCheckBox.Checked) this.TopMost = true;
            else this.TopMost = false;
        }
    }
}
