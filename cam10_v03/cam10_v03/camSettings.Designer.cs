namespace ASCOM.cam10_v03
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
            this.gainLabel = new System.Windows.Forms.Label();
            this.onTopCheckBox = new System.Windows.Forms.CheckBox();
            this.blevelLabel = new System.Windows.Forms.Label();
            this.minMaxBlevelLabel = new System.Windows.Forms.Label();
            this.blevelTrackBar = new System.Windows.Forms.TrackBar();
            this.gainNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.offsetNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.blevelNumUpDown = new System.Windows.Forms.NumericUpDown();
            this.autoOffsetCheckBox = new System.Windows.Forms.CheckBox();
            this.panel = new System.Windows.Forms.Panel();
            this.cameraStatusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gainTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blevelTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetNumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blevelNumUpDown)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gainTrackBar
            // 
            this.gainTrackBar.Location = new System.Drawing.Point(3, 22);
            this.gainTrackBar.Maximum = 63;
            this.gainTrackBar.Name = "gainTrackBar";
            this.gainTrackBar.Size = new System.Drawing.Size(219, 37);
            this.gainTrackBar.TabIndex = 36;
            this.gainTrackBar.Scroll += new System.EventHandler(this.GainTrackBar_Scroll);
            // 
            // offsetTrackBar
            // 
            this.offsetTrackBar.Location = new System.Drawing.Point(3, 88);
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
            this.minMaxOffsetLabel.Location = new System.Drawing.Point(44, 70);
            this.minMaxOffsetLabel.Name = "minMaxOffsetLabel";
            this.minMaxOffsetLabel.Size = new System.Drawing.Size(40, 13);
            this.minMaxOffsetLabel.TabIndex = 34;
            this.minMaxOffsetLabel.Text = "-63..63";
            // 
            // minMaxGainLabel
            // 
            this.minMaxGainLabel.AutoSize = true;
            this.minMaxGainLabel.Location = new System.Drawing.Point(44, 4);
            this.minMaxGainLabel.Name = "minMaxGainLabel";
            this.minMaxGainLabel.Size = new System.Drawing.Size(31, 13);
            this.minMaxGainLabel.TabIndex = 33;
            this.minMaxGainLabel.Text = "0..63";
            // 
            // offsetLabel
            // 
            this.offsetLabel.AutoSize = true;
            this.offsetLabel.Location = new System.Drawing.Point(3, 70);
            this.offsetLabel.Name = "offsetLabel";
            this.offsetLabel.Size = new System.Drawing.Size(35, 13);
            this.offsetLabel.TabIndex = 32;
            this.offsetLabel.Text = "Offset";
            // 
            // gainLabel
            // 
            this.gainLabel.AutoSize = true;
            this.gainLabel.Location = new System.Drawing.Point(3, 4);
            this.gainLabel.Name = "gainLabel";
            this.gainLabel.Size = new System.Drawing.Size(29, 13);
            this.gainLabel.TabIndex = 29;
            this.gainLabel.Text = "Gain";
            // 
            // onTopCheckBox
            // 
            this.onTopCheckBox.AutoSize = true;
            this.onTopCheckBox.Location = new System.Drawing.Point(160, 4);
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
            this.blevelLabel.Location = new System.Drawing.Point(3, 135);
            this.blevelLabel.Name = "blevelLabel";
            this.blevelLabel.Size = new System.Drawing.Size(36, 13);
            this.blevelLabel.TabIndex = 38;
            this.blevelLabel.Text = "Blevel";
            // 
            // minMaxBlevelLabel
            // 
            this.minMaxBlevelLabel.AutoSize = true;
            this.minMaxBlevelLabel.Location = new System.Drawing.Point(44, 135);
            this.minMaxBlevelLabel.Name = "minMaxBlevelLabel";
            this.minMaxBlevelLabel.Size = new System.Drawing.Size(37, 13);
            this.minMaxBlevelLabel.TabIndex = 39;
            this.minMaxBlevelLabel.Text = "0..255";
            // 
            // blevelTrackBar
            // 
            this.blevelTrackBar.Location = new System.Drawing.Point(3, 155);
            this.blevelTrackBar.Maximum = 255;
            this.blevelTrackBar.Name = "blevelTrackBar";
            this.blevelTrackBar.Size = new System.Drawing.Size(219, 37);
            this.blevelTrackBar.TabIndex = 41;
            this.blevelTrackBar.Scroll += new System.EventHandler(this.BlevelTrackBar_Scroll);
            // 
            // gainNumUpDown
            // 
            this.gainNumUpDown.Location = new System.Drawing.Point(90, 2);
            this.gainNumUpDown.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.gainNumUpDown.Name = "gainNumUpDown";
            this.gainNumUpDown.Size = new System.Drawing.Size(51, 20);
            this.gainNumUpDown.TabIndex = 42;
            this.gainNumUpDown.ValueChanged += new System.EventHandler(this.gainNumUpDown_ValueChanged);
            // 
            // offsetNumUpDown
            // 
            this.offsetNumUpDown.Location = new System.Drawing.Point(90, 68);
            this.offsetNumUpDown.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.offsetNumUpDown.Minimum = new decimal(new int[] {
            63,
            0,
            0,
            -2147483648});
            this.offsetNumUpDown.Name = "offsetNumUpDown";
            this.offsetNumUpDown.Size = new System.Drawing.Size(51, 20);
            this.offsetNumUpDown.TabIndex = 43;
            this.offsetNumUpDown.ValueChanged += new System.EventHandler(this.offsetNumUpDown_ValueChanged);
            // 
            // blevelNumUpDown
            // 
            this.blevelNumUpDown.Location = new System.Drawing.Point(90, 133);
            this.blevelNumUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.blevelNumUpDown.Name = "blevelNumUpDown";
            this.blevelNumUpDown.Size = new System.Drawing.Size(51, 20);
            this.blevelNumUpDown.TabIndex = 44;
            this.blevelNumUpDown.ValueChanged += new System.EventHandler(this.blevelNumUpDown_ValueChanged);
            // 
            // autoOffsetCheckBox
            // 
            this.autoOffsetCheckBox.AutoSize = true;
            this.autoOffsetCheckBox.Location = new System.Drawing.Point(160, 70);
            this.autoOffsetCheckBox.Name = "autoOffsetCheckBox";
            this.autoOffsetCheckBox.Size = new System.Drawing.Size(48, 17);
            this.autoOffsetCheckBox.TabIndex = 45;
            this.autoOffsetCheckBox.Text = "Auto";
            this.autoOffsetCheckBox.UseVisualStyleBackColor = true;
            this.autoOffsetCheckBox.CheckedChanged += new System.EventHandler(this.autoOffsetCheckBox_CheckedChanged);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.cameraStatusLabel);
            this.panel.Controls.Add(this.gainLabel);
            this.panel.Controls.Add(this.autoOffsetCheckBox);
            this.panel.Controls.Add(this.offsetLabel);
            this.panel.Controls.Add(this.blevelNumUpDown);
            this.panel.Controls.Add(this.minMaxGainLabel);
            this.panel.Controls.Add(this.offsetNumUpDown);
            this.panel.Controls.Add(this.minMaxOffsetLabel);
            this.panel.Controls.Add(this.gainNumUpDown);
            this.panel.Controls.Add(this.offsetTrackBar);
            this.panel.Controls.Add(this.blevelTrackBar);
            this.panel.Controls.Add(this.gainTrackBar);
            this.panel.Controls.Add(this.minMaxBlevelLabel);
            this.panel.Controls.Add(this.onTopCheckBox);
            this.panel.Controls.Add(this.blevelLabel);
            this.panel.Location = new System.Drawing.Point(2, 1);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(228, 211);
            this.panel.TabIndex = 46;
            // 
            // cameraStatusLabel
            // 
            this.cameraStatusLabel.AutoSize = true;
            this.cameraStatusLabel.Location = new System.Drawing.Point(3, 189);
            this.cameraStatusLabel.Name = "cameraStatusLabel";
            this.cameraStatusLabel.Size = new System.Drawing.Size(132, 13);
            this.cameraStatusLabel.TabIndex = 46;
            this.cameraStatusLabel.Text = "Camera status: operational";
            // 
            // camSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 215);
            this.Controls.Add(this.panel);
            this.Name = "camSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "CAM10_settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.camSettings_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gainTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blevelTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gainNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetNumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blevelNumUpDown)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar gainTrackBar;
        private System.Windows.Forms.TrackBar offsetTrackBar;
        private System.Windows.Forms.Label minMaxOffsetLabel;
        private System.Windows.Forms.Label minMaxGainLabel;
        private System.Windows.Forms.Label offsetLabel;
        private System.Windows.Forms.Label gainLabel;
        private System.Windows.Forms.CheckBox onTopCheckBox;
        private System.Windows.Forms.Label blevelLabel;
        private System.Windows.Forms.Label minMaxBlevelLabel;
        private System.Windows.Forms.TrackBar blevelTrackBar;
        private System.Windows.Forms.NumericUpDown gainNumUpDown;
        private System.Windows.Forms.NumericUpDown offsetNumUpDown;
        private System.Windows.Forms.NumericUpDown blevelNumUpDown;
        private System.Windows.Forms.CheckBox autoOffsetCheckBox;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label cameraStatusLabel;
    }
}