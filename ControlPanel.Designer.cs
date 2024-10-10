namespace Digital_Museum_of_Music_and_Artists
{
    partial class ControlPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.buttonDecrease = new System.Windows.Forms.Button();
            this.buttonIncrease = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Light = new System.Windows.Forms.Label();
            this.pictureBoxLight = new System.Windows.Forms.PictureBox();
            this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.buttonBack = new System.Windows.Forms.Button();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.controlPanelFinal;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel1.Controls.Add(this.labelTemperature);
            this.panel1.Controls.Add(this.buttonDecrease);
            this.panel1.Controls.Add(this.buttonIncrease);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Light);
            this.panel1.Controls.Add(this.pictureBoxLight);
            this.panel1.Controls.Add(this.pictureBoxVideo);
            this.panel1.Controls.Add(this.buttonBack);
            this.panel1.Location = new System.Drawing.Point(0, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 355);
            this.panel1.TabIndex = 15;
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.labelTemperature.Location = new System.Drawing.Point(357, 143);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(51, 20);
            this.labelTemperature.TabIndex = 22;
            this.labelTemperature.Text = "label1";
            // 
            // buttonDecrease
            // 
            this.buttonDecrease.BackgroundImage = global::Digital_Museum_of_Music_and_Artists.Properties.Resources._3994355_arrow_bottom_down_downward_navigation_icon;
            this.buttonDecrease.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonDecrease.Location = new System.Drawing.Point(353, 190);
            this.buttonDecrease.Name = "buttonDecrease";
            this.buttonDecrease.Size = new System.Drawing.Size(55, 55);
            this.buttonDecrease.TabIndex = 21;
            this.buttonDecrease.UseVisualStyleBackColor = true;
            this.buttonDecrease.Click += new System.EventHandler(this.ButtonDecrease_Click);
            // 
            // buttonIncrease
            // 
            this.buttonIncrease.BackgroundImage = global::Digital_Museum_of_Music_and_Artists.Properties.Resources._3994413_above_arrow_navigation_top_up_icon;
            this.buttonIncrease.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonIncrease.Location = new System.Drawing.Point(353, 64);
            this.buttonIncrease.Name = "buttonIncrease";
            this.buttonIncrease.Size = new System.Drawing.Size(55, 55);
            this.buttonIncrease.TabIndex = 20;
            this.buttonIncrease.UseVisualStyleBackColor = true;
            this.buttonIncrease.Click += new System.EventHandler(this.ButtonIncrease_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(540, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Video";
            // 
            // Light
            // 
            this.Light.AutoSize = true;
            this.Light.Location = new System.Drawing.Point(450, 232);
            this.Light.Name = "Light";
            this.Light.Size = new System.Drawing.Size(35, 13);
            this.Light.TabIndex = 18;
            this.Light.Text = "Lights";
            // 
            // pictureBoxLight
            // 
            this.pictureBoxLight.BackgroundImage = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.OffButton;
            this.pictureBoxLight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxLight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxLight.Location = new System.Drawing.Point(431, 54);
            this.pictureBoxLight.Name = "pictureBoxLight";
            this.pictureBoxLight.Size = new System.Drawing.Size(75, 200);
            this.pictureBoxLight.TabIndex = 17;
            this.pictureBoxLight.TabStop = false;
            this.pictureBoxLight.Click += new System.EventHandler(this.PictureBoxLight_Click);
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.BackgroundImage = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.OnButton;
            this.pictureBoxVideo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxVideo.Image = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.OnButton;
            this.pictureBoxVideo.Location = new System.Drawing.Point(519, 54);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(75, 200);
            this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxVideo.TabIndex = 16;
            this.pictureBoxVideo.TabStop = false;
            this.pictureBoxVideo.Click += new System.EventHandler(this.PictureBoxVideo_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.buttonBack.FlatAppearance.BorderSize = 2;
            this.buttonBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBack.Location = new System.Drawing.Point(533, 314);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 26);
            this.buttonBack.TabIndex = 4;
            this.buttonBack.Text = "Apply";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // helpProvider
            // 
            this.helpProvider.HelpNamespace = "C:\\\\Users\\\\aggel\\\\Desktop\\\\On-Line help.chm";
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(630, 351);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ControlPanel";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ControlPanel";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxLight;
        private System.Windows.Forms.PictureBox pictureBoxVideo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Light;
        private System.Windows.Forms.Button buttonDecrease;
        private System.Windows.Forms.Button buttonIncrease;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}