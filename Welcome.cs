using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class Welcome : Form
    {
        //
        // Initialization
        //
        public Welcome(string username, string room, string stage)
        {
            InitializeComponent();

            label1.Text = GetWelcomeMessage(username, room, stage);
            pictureBox1.Image = GetStageImage(stage);
            SetButtonText(stage);
        }
        //
        // Buttons
        //
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //
        // Appearance
        //
        private string GetWelcomeMessage(string username, string room, string stage)
        {
            switch (stage)
            {
                case "welcome":
                    return $"Welcome, {username}\nto {room}!";
                case "welcomeVIP":
                    return $"Welcome, {username}\nto the VIP section\nof the {room}!";
                case "controlPanel":
                    return $"Hi, Customers\nare not allowed\nin the ControlPanel!";
                case "cafe":
                    return $"Hello, here is the\nmenu.";
                case "ticket":
                    return $"Hello, here is\nthe ticket price list.";
                case "denyConcert":
                    return $"Hi, you need to buy\na ticket first.";
                case "denyEvent":
                    return $"Hi, you need to book\nthe {room} room to\nenter.";
                case "denyKaraoke":
                    return $"Hi, you need to enable\nthe karaoke mode\nfrom the DJDeck.";
                case "denyVideo":
                    return $"Hi, the video is\ndisabled. You can\nenable the Video from\nthe Control Panel.";
                case "welcomeMuseum":
                    return $"Welcome to the\nDigital Museum of\nMusic and Artists.";
                default:
                    return $"Welcome, {username}\nto {room}!";
            }
        }
        private Image GetStageImage(string stage)
        {
            switch (stage)
            {
                case "welcome":
                case "welcomeVIP":
                case "welcomeMuseum":
                    return Properties.Resources.man;
                case "cafe":
                    return Properties.Resources.ticketLady;
                case "ticket":
                    return Properties.Resources.cafeGuy;
                case "controlPanel":
                case "denyConcert":
                case "denyKaraoke":
                case "denyEvent":
                case "denyVideo":
                    return Properties.Resources.technician;
                default:
                    return Properties.Resources.technician;
            }
        }
        private void SetButtonText(string stage)
        {
            switch (stage)
            {
                case "welcome":
                case "welcomeVIP":
                case "welcomeMuseum":
                    buttonOK.Text = "Enter";
                    break;
                case "controlPanel":
                case "denyConcert":
                case "denyKaraoke":
                case "denyEvent":
                case "denyVideo":
                    buttonOK.Text = "OK";
                    break;
                case "cafe":
                    buttonOK.Text = "Show Menu";
                    break;
                case "ticket":
                    buttonOK.Text = "Show Tickets";
                    break;
                default:
                    buttonOK.Text = "OK";
                    break;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 50;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            }
        }
    }

}
