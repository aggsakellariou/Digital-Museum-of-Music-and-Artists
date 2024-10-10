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
    public partial class Pictures : Form
    {
        //
        //Initialization
        //
        private readonly List<string> pictureNavigation;
        private int currentImageIndex = 0;
        public Pictures(string mediaFileName)
        {
            InitializeComponent();
            pictureNavigation = new List<string> { "Portrait of Bach", "Portrait of Beethoven", "The Revolutionary Beethoven", "Beethoven's family", "Beethoven vs Steibelt", "Bach playing the Organ", "Bach's family", "G Minor" };
            
            currentImageIndex = pictureNavigation.IndexOf(mediaFileName);
            ShowImage(pictureNavigation[currentImageIndex]);
        }

        private void ShowImage(string mediaFileName)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject(mediaFileName);
        }
        //
        //Buttons
        //
        private void ButtonNext_Click(object sender, EventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % pictureNavigation.Count;
            ShowImage(pictureNavigation[currentImageIndex]);
        }
        private void ButtonPrevious_Click(object sender, EventArgs e)
        {
            currentImageIndex = (currentImageIndex - 1 + pictureNavigation.Count) % pictureNavigation.Count;
            ShowImage(pictureNavigation[currentImageIndex]);
        }
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
