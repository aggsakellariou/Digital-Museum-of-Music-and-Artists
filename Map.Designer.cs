namespace Digital_Museum_of_Music_and_Artists
{
    partial class Map
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
            this.labelTicketTitle = new System.Windows.Forms.Label();
            this.labelMoneyTitle = new System.Windows.Forms.Label();
            this.labelUsernameTitle = new System.Windows.Forms.Label();
            this.buttonLogOut = new System.Windows.Forms.Button();
            this.labelTicket = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelMoney = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.pictureBoxBig = new System.Windows.Forms.PictureBox();
            this.pictureBoxInfo = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTicketTitle
            // 
            this.labelTicketTitle.AutoSize = true;
            this.labelTicketTitle.Location = new System.Drawing.Point(131, 91);
            this.labelTicketTitle.Name = "labelTicketTitle";
            this.labelTicketTitle.Size = new System.Drawing.Size(40, 13);
            this.labelTicketTitle.TabIndex = 0;
            this.labelTicketTitle.Text = "Ticket:";
            // 
            // labelMoneyTitle
            // 
            this.labelMoneyTitle.AutoSize = true;
            this.labelMoneyTitle.Location = new System.Drawing.Point(131, 56);
            this.labelMoneyTitle.Name = "labelMoneyTitle";
            this.labelMoneyTitle.Size = new System.Drawing.Size(42, 13);
            this.labelMoneyTitle.TabIndex = 0;
            this.labelMoneyTitle.Text = "Money:";
            // 
            // labelUsernameTitle
            // 
            this.labelUsernameTitle.AutoSize = true;
            this.labelUsernameTitle.Location = new System.Drawing.Point(48, 22);
            this.labelUsernameTitle.Name = "labelUsernameTitle";
            this.labelUsernameTitle.Size = new System.Drawing.Size(58, 13);
            this.labelUsernameTitle.TabIndex = 0;
            this.labelUsernameTitle.Text = "Username:";
            // 
            // buttonLogOut
            // 
            this.buttonLogOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonLogOut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonLogOut.FlatAppearance.BorderSize = 2;
            this.buttonLogOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.buttonLogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLogOut.Location = new System.Drawing.Point(909, 595);
            this.buttonLogOut.Name = "buttonLogOut";
            this.buttonLogOut.Size = new System.Drawing.Size(60, 27);
            this.buttonLogOut.TabIndex = 2;
            this.buttonLogOut.Text = "Log Out";
            this.buttonLogOut.UseVisualStyleBackColor = true;
            this.buttonLogOut.Click += new System.EventHandler(this.ButtonLogOut_Click);
            // 
            // labelTicket
            // 
            this.labelTicket.AutoSize = true;
            this.labelTicket.Location = new System.Drawing.Point(131, 104);
            this.labelTicket.Name = "labelTicket";
            this.labelTicket.Size = new System.Drawing.Size(37, 13);
            this.labelTicket.TabIndex = 0;
            this.labelTicket.Text = "Ticket";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(103, 22);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(55, 13);
            this.labelUsername.TabIndex = 0;
            this.labelUsername.Text = "Username";
            // 
            // labelMoney
            // 
            this.labelMoney.AutoSize = true;
            this.labelMoney.Location = new System.Drawing.Point(131, 69);
            this.labelMoney.Name = "labelMoney";
            this.labelMoney.Size = new System.Drawing.Size(39, 13);
            this.labelMoney.TabIndex = 0;
            this.labelMoney.Text = "Money";
            // 
            // helpProvider
            // 
            this.helpProvider.HelpNamespace = "C:\\\\Users\\\\aggel\\\\Desktop\\\\On-Line help.chm";
            // 
            // buttonHelp
            // 
            this.buttonHelp.BackgroundImage = global::Digital_Museum_of_Music_and_Artists.Properties.Resources._6138084_help_info_information_support_alert_icon;
            this.buttonHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonHelp.FlatAppearance.BorderSize = 0;
            this.buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHelp.Location = new System.Drawing.Point(929, 12);
            this.buttonHelp.Name = "buttonHelp";
            this.helpProvider.SetShowHelp(this.buttonHelp, true);
            this.buttonHelp.Size = new System.Drawing.Size(40, 40);
            this.buttonHelp.TabIndex = 3;
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            // 
            // pictureBoxBig
            // 
            this.helpProvider.SetHelpKeyword(this.pictureBoxBig, "F1");
            this.pictureBoxBig.Image = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.Map1;
            this.pictureBoxBig.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxBig.Name = "pictureBoxBig";
            this.helpProvider.SetShowHelp(this.pictureBoxBig, true);
            this.pictureBoxBig.Size = new System.Drawing.Size(957, 608);
            this.pictureBoxBig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxBig.TabIndex = 0;
            this.pictureBoxBig.TabStop = false;
            this.pictureBoxBig.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBoxBig_Paint);
            // 
            // pictureBoxInfo
            // 
            this.pictureBoxInfo.Image = global::Digital_Museum_of_Music_and_Artists.Properties.Resources.id_info;
            this.pictureBoxInfo.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxInfo.Name = "pictureBoxInfo";
            this.pictureBoxInfo.Size = new System.Drawing.Size(189, 124);
            this.pictureBoxInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxInfo.TabIndex = 9;
            this.pictureBoxInfo.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(51, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(981, 634);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelTicket);
            this.Controls.Add(this.labelMoney);
            this.Controls.Add(this.labelUsernameTitle);
            this.Controls.Add(this.labelMoneyTitle);
            this.Controls.Add(this.labelTicketTitle);
            this.Controls.Add(this.buttonLogOut);
            this.Controls.Add(this.pictureBoxInfo);
            this.Controls.Add(this.pictureBoxBig);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Map";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Map";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxBig;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelMoney;
        private System.Windows.Forms.Label labelTicket;
        private System.Windows.Forms.Button buttonLogOut;
        private System.Windows.Forms.Label labelTicketTitle;
        private System.Windows.Forms.Label labelMoneyTitle;
        private System.Windows.Forms.Label labelUsernameTitle;
        private System.Windows.Forms.PictureBox pictureBoxInfo;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.Button button1;
    }
}