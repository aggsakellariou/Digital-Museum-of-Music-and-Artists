namespace Digital_Museum_of_Music_and_Artists
{
    partial class CafeTicket
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTicket = new System.Windows.Forms.Label();
            this.labelTicketTitle = new System.Windows.Forms.Label();
            this.labelMoney = new System.Windows.Forms.Label();
            this.labelMoneyTitle = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelUsernameTitle = new System.Windows.Forms.Label();
            this.pictureBoxInfo = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxCafeTicket = new System.Windows.Forms.PictureBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCafeTicket)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.buttonBack);
            this.panel2.Location = new System.Drawing.Point(859, 522);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(110, 100);
            this.panel2.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Map";
            // 
            // buttonBack
            // 
            this.buttonBack.BackColor = System.Drawing.Color.White;
            this.buttonBack.BackgroundImage = global::Digital_Museum_of_Music_and_Artists.Properties.Resources._9022584_map_trifold_duotone_icon;
            this.buttonBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonBack.ForeColor = System.Drawing.Color.Transparent;
            this.buttonBack.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.buttonBack.Location = new System.Drawing.Point(3, 30);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(100, 70);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.UseVisualStyleBackColor = false;
            this.buttonBack.Click += new System.EventHandler(this.ButtonBack_Click);
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.labelTemperature.Location = new System.Drawing.Point(904, 64);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(64, 29);
            this.labelTemperature.TabIndex = 24;
            this.labelTemperature.Text = "25°C";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelTicket);
            this.panel3.Controls.Add(this.labelTicketTitle);
            this.panel3.Controls.Add(this.labelMoney);
            this.panel3.Controls.Add(this.labelMoneyTitle);
            this.panel3.Controls.Add(this.labelUsername);
            this.panel3.Controls.Add(this.labelUsernameTitle);
            this.panel3.Controls.Add(this.pictureBoxInfo);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(189, 131);
            this.panel3.TabIndex = 38;
            // 
            // labelTicket
            // 
            this.labelTicket.AutoSize = true;
            this.labelTicket.Location = new System.Drawing.Point(122, 92);
            this.labelTicket.Name = "labelTicket";
            this.labelTicket.Size = new System.Drawing.Size(37, 13);
            this.labelTicket.TabIndex = 28;
            this.labelTicket.Text = "Ticket";
            // 
            // labelTicketTitle
            // 
            this.labelTicketTitle.AutoSize = true;
            this.labelTicketTitle.Location = new System.Drawing.Point(122, 79);
            this.labelTicketTitle.Name = "labelTicketTitle";
            this.labelTicketTitle.Size = new System.Drawing.Size(40, 13);
            this.labelTicketTitle.TabIndex = 32;
            this.labelTicketTitle.Text = "Ticket:";
            // 
            // labelMoney
            // 
            this.labelMoney.AutoSize = true;
            this.labelMoney.Location = new System.Drawing.Point(122, 57);
            this.labelMoney.Name = "labelMoney";
            this.labelMoney.Size = new System.Drawing.Size(39, 13);
            this.labelMoney.TabIndex = 29;
            this.labelMoney.Text = "Money";
            // 
            // labelMoneyTitle
            // 
            this.labelMoneyTitle.AutoSize = true;
            this.labelMoneyTitle.Location = new System.Drawing.Point(122, 44);
            this.labelMoneyTitle.Name = "labelMoneyTitle";
            this.labelMoneyTitle.Size = new System.Drawing.Size(42, 13);
            this.labelMoneyTitle.TabIndex = 31;
            this.labelMoneyTitle.Text = "Money:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(94, 10);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(55, 13);
            this.labelUsername.TabIndex = 27;
            this.labelUsername.Text = "Username";
            // 
            // labelUsernameTitle
            // 
            this.labelUsernameTitle.AutoSize = true;
            this.labelUsernameTitle.Location = new System.Drawing.Point(39, 10);
            this.labelUsernameTitle.Name = "labelUsernameTitle";
            this.labelUsernameTitle.Size = new System.Drawing.Size(58, 13);
            this.labelUsernameTitle.TabIndex = 30;
            this.labelUsernameTitle.Text = "Username:";
            // 
            // pictureBoxInfo
            // 
            this.pictureBoxInfo.Image = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.id_info;
            this.pictureBoxInfo.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxInfo.Name = "pictureBoxInfo";
            this.pictureBoxInfo.Size = new System.Drawing.Size(189, 124);
            this.pictureBoxInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxInfo.TabIndex = 34;
            this.pictureBoxInfo.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Image = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.temperatureIcon;
            this.pictureBox1.Location = new System.Drawing.Point(858, 37);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 25;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBoxCafeTicket
            // 
            this.pictureBoxCafeTicket.BackColor = System.Drawing.Color.White;
            this.pictureBoxCafeTicket.Image = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.CafeTicket1;
            this.pictureBoxCafeTicket.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxCafeTicket.Name = "pictureBoxCafeTicket";
            this.pictureBoxCafeTicket.Size = new System.Drawing.Size(957, 581);
            this.pictureBoxCafeTicket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCafeTicket.TabIndex = 2;
            this.pictureBoxCafeTicket.TabStop = false;
            // 
            // helpProvider
            // 
            this.helpProvider.HelpNamespace = "C:\\\\Users\\\\aggel\\\\Desktop\\\\On-Line help.chm";
            // 
            // CafeTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(981, 634);
            this.Controls.Add(this.labelTemperature);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pictureBoxCafeTicket);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CafeTicket";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CafeTicket";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCafeTicket)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.PictureBox pictureBoxCafeTicket;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTicket;
        private System.Windows.Forms.Label labelTicketTitle;
        private System.Windows.Forms.Label labelMoney;
        private System.Windows.Forms.Label labelMoneyTitle;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelUsernameTitle;
        private System.Windows.Forms.PictureBox pictureBoxInfo;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}