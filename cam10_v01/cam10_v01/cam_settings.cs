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
    public partial class cam_settings : Form
    {
        const short MinGain = 0;
        const short MaxGain = 63;
        const short MinOffset = -63;
        const short MaxOffset = 63;

        public cam_settings()
        {
            InitializeComponent(); 
        }

        private short p_gain = 0;
        private short p_offset = 0;

        public short gain
        {
            get
            {
                return p_gain;           
            }
            set
            {
                if ((value >= MinGain) && (value <= MaxGain))
                {
                    p_gain = value;
                    this.GainTextBox.Text = p_gain.ToString();
                    this.GainTrackBar.Value = p_gain;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, gain");
            }
        }
        public short offset
        {
            get
            {
                return p_offset;
            }
            set
            {
                if ((value >= MinOffset) && (value <= MaxOffset))
                {
                    p_offset = value;
                    this.OffsetTextBox.Text = p_offset.ToString();
                    this.OffsetTrackBar.Value = p_offset;
                }
                else throw new ASCOM.InvalidValueException("cam_settings, offset");
            }
        }

        private void GainTrackBar_Scroll(object sender, EventArgs e)
        {
            GainTextBox.Text = GainTrackBar.Value.ToString();
        }

        private void OffsetTrackBar_Scroll(object sender, EventArgs e)
        {
            OffsetTextBox.Text = OffsetTrackBar.Value.ToString();
        }

        private void GainTextBox_TextChanged(object sender, EventArgs e)
        {
            bool ConvRes;
            short ValNum;
            //Settings are correct?
            ConvRes = short.TryParse(GainTextBox.Text, out ValNum);
            if ((ConvRes == false) || (ValNum < MinGain) || (ValNum > MaxGain))
            {
                GainTrackBar.Value = gain = MinGain;
                GainTextBox.Text = MinGain.ToString();
                return;
            }
            GainTrackBar.Value = gain = short.Parse(GainTextBox.Text);
        }

        private void OffsetTextBox_TextChanged(object sender, EventArgs e)
        {
            bool ConvRes;
            short ValNum;
            //Settings are correct?
            ConvRes = short.TryParse(OffsetTextBox.Text, out ValNum);
            if ((ConvRes == false) || (ValNum < MinOffset) || (ValNum > MaxOffset))
            {
                OffsetTrackBar.Value = offset = MinOffset;
                OffsetTextBox.Text = MinOffset.ToString();
                return;
            }
            OffsetTrackBar.Value = offset = short.Parse(OffsetTextBox.Text);
        }

        private void OnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OnTopCheckBox.Checked) this.TopMost = true;
            else this.TopMost = false;
        }
    }
}
