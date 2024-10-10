using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class MediaPlayerForm : Form
    {
        public MediaPlayerForm(string mediaFileName)
        {
            InitializeComponent();
            ShowMedia(mediaFileName);
        }

        private void ShowMedia(string mediaFileName)
        {
            if (mediaFileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
            {
                axWindowsMediaPlayer1.URL = mediaFileName;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            // Handle other media types if needed
        }
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            this.Close();
        }
    }
}
