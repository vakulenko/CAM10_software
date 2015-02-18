namespace ASCOM.cam10_v01
{
    partial class camSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gainTrackBar = new System.Windows.Forms.TrackBar();
            this.offsetTrackBar = new System.Windows.Forms.TrackBar();
            this.minMaxOffsetLabel = new System.Windows.Forms.Label();
            this.minMaxGainLabel = new System.Windows.Forms.Label();
            this.offsetLabel = new System.Windows.Forms.Label();
            this.offsetTextBox = new System.Windows.Forms.TextBox();
            this.gainTextBox = new System.Windows.Forms.TextBox();
            this.gainLabel = new System.Windows.Forms.Label();
            this.onTopCheckBox = new System.Windows.Forms.CheckBox();
            this.blevelLabel = new System.Windows.Forms.Label();
            this.minMaxBlevelLabel = new System.Windows.Forms.Label();
            this.blevelTextBox = new System.Windows.Forms.TextBox();
            this.blevelTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.gainTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blevelTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // gainTrackBar
            // 
            this.gainTrackBar.Location = new System.Drawing.Point(6, 25);
            this.gainTrackBar.Maximum = 63;
            this.gainTrackBar.Name = "gainTrackBar";
            this.gainTrackBar.Size = new System.Drawing.Size(219, 37);
            this.gainTrackBar.TabIndex = 36;
            this.gainTrackBar.Scroll += new System.EventHandler(this.GainTrackBar_Scroll);
            // 
            // offsetTrackBar
            // 
            this.offsetTrackBar.Location = new System.Drawing.Point(6, 91);
            this.offsetTrackBar.Maximum = 63;
            this.offsetTrackBar.Minimum = -63;
            this.offsetTrackBar.Name = "offsetTrackBar";
            this.offsetTrackBar.Size = new System.Drawing.Size(219, 37);
            this.offsetTrackBar.TabIndex = 35;
            this.offsetTrackBar.Scroll += new System.EventHandler(this.OffsetTrackBar_Scroll);
            // 
            // minMaxOffsetLabel
            // 
            this.minMaxOffsetLabel.AutoSize = true;
            this.minMaxOffsetLabel.Location = new System.Drawing.Point(47, 73);
            this.minMaxOffsetLabel.Name = "minMaxOffsetLabel";
            this.minMaxOffsetLabel.Size = new System.Drawing.Size(40, 13);
            this.minMaxOffsetLabel.TabIndex = 34;
            this.minMaxOffsetLabel.Text = "-63..63";
            // 
            // minMaxGainLabel
            // 
            this.minMaxGainLabel.AutoSize = true;
            this.minMaxGainLabel.Location = new System.Drawing.Point(47, 7);
            this.minMaxGainLabel.Name = "minMaxGainLabel";
            this.minMaxGainLabel.Size = new System.Drawing.Size(31, 13);
            this.minMaxGainLabel.TabIndex = 33;
            this.minMaxGainLabel.Text = "0..63";
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(6, 73);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(35, 13);
            this.offsetLabel.TabIndex = 32;
            this.offsetLabel.Text = "Offset";
            // 
            // offsetTextBox
            // 
            this.offsetTextBox.Location = new System.Drawing.Point(106, 69);
            this.offsetTextBox.Name = "offsetTextBox";
            this.offsetTextBox.Size = new System.Drawing.Size(51, 20);
            this.offsetTextBox.TabIndex = 31;
            this.offsetTextBox.Text = "0";
            this.offsetTextBox.TextChanged += new System.EventHandler(this.OffsetTextBox_TextChanged);
            // 
            // gainTextBox
            // 
            this.gainTextBox.Location = new System.Drawing.Point(106, 4);
            this.gainTextBox.Name = "gainTextBox";
            this.gainTextBox.Size = new System.Drawing.Size(51, 20);
            this.gainTextBox.TabIndex = 30;
            this.gainTextBox.Text = "0";
            this.gainTextBox.TextChanged += new System.EventHandler(this.GainTextBox_TextChanged);
            // 
            // gainLabel
            // 
            this.gainLabel.AutoSize = true;
            this.gainLabel.Location = new System.Drawing.Point(6, 7);
            this.gainLabel.Name = "gainLabel";
            this.gainLabel.Size = new System.Drawing.Size(29, 13);
            this.gainLabel.TabIndex = 29;
            this.gainLabel.Text = "Gain";
            // 
            // onTopCheckBox
            // 
            this.onTopCheckBox.AutoSize = true;
            this.onTopCheckBox.Location = new System.Drawing.Point(163, 7);
            this.onTopCheckBox.Name = "onTopCheckBox";
            this.onTopCheckBox.Size = new System.Drawing.Size(62, 17);
            this.onTopCheckBox.TabIndex = 37;
            this.onTopCheckBox.Text = "On Top";
            this.onTopCheckBox.UseVisualStyleBackColor = true;
            this.onTopCheckBox.CheckedChanged += new System.EventHandler(this.OnTopCheckBox_CheckedChanged);
            // 
            // blevelLabel
            // 
            this.blevelLabel.AutoSize = true;
            this.blevelLabel.Location = new System.Drawing.Point(6, 138);
            this.blevelLabel.Name = "blevelLabel";
            this.blevelLabel.Size = new System.Drawing.Size(36, 13);
            this.blevelLabel.TabIndex = 38;
            this.blevelLabel.Text = "Blevel";
            // 
            // minMaxBlevelLabel
            // 
            this.minMaxBlevelLabel.AutoSize = true;
            this.minMaxBlevelLabel.Location = new System.Drawing.Point(47, 138);
            this.minMaxBlevelLabel.Name = "minMaxBlevelLabel";
            this.minMaxBlevelLabel.Size = new System.Drawing.Size(37, 13);
            this.minMaxBlevelLabel.TabIndex = 39;
            this.minMaxBlevelLabel.Text = "0..255";
            // 
            // blevelTextBox
            // 
            this.blevelTextBox.Location = new System.Drawing.Point(106, 135);
            this.blevelTextBox.Name = "blevelTextBox";
            this.blevelTextBox.Size = new System.Drawing.Size(51, 20);
            this.blevelTextBox.TabIndex = 40;
            this.blevelTextBox.Text = "0";
            this.blevelTextBox.TextChanged += new System.EventHandler(this.blevelTextBox_TextChanged);
            // 
            // blevelTrackBar
            // 
            this.blevelTrackBar.Location = new System.Drawing.Point(6, 158);
            this.blevelTrackBar.Maximum = 255;
            this.blevelTrackBar.Name = "blevelTrackBar";
            this.blevelTrackBar.Size = new System.Drawing.Size(219, 37);
            this.blevelTrackBar.TabIndex = 41;
            this.blevelTrackBar.Scroll += new System.EventHandler(this.BlevelTrackBar_Scroll);
            // 
            // camSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 202);
            this.ControlBox = false;
            this.Controls.Add(this.blevelTrackBar);
            this.Controls.Add(this.blevelTextBox);
            this.Controls.Add(this.minMaxBlevelLabel);
            this.Controls.Add(this.blevelLabel);
            this.Controls.Add(this.onTopCheckBox);
            this.Controls.Add(this.gainTrackBar);
            this.Controls.Add(this.offsetTrackBar);
            this.Controls.Add(this.minMaxOffsetLabel);
            this.Controls.Add(this.minMaxGainLabel);
            this.Controls.Add(this.offsetLabel);
            this.Controls.Add(this.offsetTextBox);
            this.Controls.Add(this.gainTextBox);
            this.Controls.Add(this.gainLabel);
            this.MaximumSize = new System.Drawing.Size(238, 231);
            this.MinimumSize = new System.Drawing.Size(238, 231);
            this.Name = "camSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "CAM10_settings";
            ((System.ComponentModel.ISupportInitialize)(this.gainTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blevelTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar gainTrackBar;
        private System.Windows.Forms.TrackBar offsetTrackBar;
        private System.Windows.Forms.Label minMaxOffsetLabel;
        private System.Windows.Forms.Label minMaxGainLabel;
        private System.Windows.Forms.Label offsetLabel;
        public System.Windows.Forms.TextBox offsetTextBox;
        public System.Windows.Forms.TextBox gainTextBox;
        private System.Windows.Forms.Label gainLabel;
        private System.Windows.Forms.CheckBox onTopCheckBox;
        private System.Windows.Forms.Label blevelLabel;
        private System.Windows.Forms.Label minMaxBlevelLabel;
        public System.Windows.Forms.TextBox blevelTextBox;
        private System.Windows.Forms.TrackBar blevelTrackBar;
    }
}