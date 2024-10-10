using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class ControlPanel : Form
    {
        //
        //Initialization
        //
        private RoomState roomState;
        private DJState djState;
        private string fileName;
        private readonly string room;
        private readonly UserRole currentUserRole;
        private readonly string username;
        private readonly UserTicket currentUserTicket;
        private readonly int money;
        private int currentTemperature;

        private bool isImageVideoVisible;
        private bool isImageLightVisible;
        public ControlPanel(UserRole role, string username, string room, UserTicket currentUserTicket, int money)
        {
            InitializeComponent();
            this.room = room;
            this.username = username;
            this.currentUserRole = role;
            this.currentUserTicket = currentUserTicket;
            this.money = money;

            LoadRoomState();
            pictureBoxLight.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxVideo.SizeMode = PictureBoxSizeMode.Zoom;
            UpdateTemperatureLabel();

            this.TransparencyKey = Color.Magenta;
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
        //
        // Room State
        //
        private void SaveRoomState(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    if (room == "DJ")
                    {
                        djState.IsLightOn = isImageLightVisible;
                        djState.Temperature = currentTemperature;
                        djState.IsVideoPlaying = isImageVideoVisible;

                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, djState);
                    }
                    else
                    {
                        roomState.IsLightOn = isImageLightVisible;
                        roomState.Temperature = currentTemperature;
                        roomState.IsVideoPlaying = isImageVideoVisible;

                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, roomState);
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving room state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadRoomState()
        {
            if (room == "CafeTicket")
            {
                fileName = "roomStateCafeTicket.dat";
            }
            else if (room == "Concert")
            {
                fileName = "roomStateConcert.dat";
            }
            else if (room == "DJ")
            {
                fileName = "roomStateDJ.dat";
            }
            else if (room == "EventA")
            {
                fileName = "roomStateEventA.dat";
            }
            else if (room == "EventB")
            {
                fileName = "roomStateEventB.dat";
            }
            else if (room == "ExhibitionA")
            {
                fileName = "roomStateExhibitionA.dat";
            }
            else if (room == "ExhibitionB")
            {
                fileName = "roomStateExhibitionB.dat";
            }
            else
            {
                MessageBox.Show($"Error Loading room state:", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                if (File.Exists(fileName))
                {
                    if (room == "DJ")
                    {
                        using (FileStream fs = new FileStream(fileName, FileMode.Open))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            djState = (DJState)formatter.Deserialize(fs);
                        }
                    }
                    else
                    {
                        using (FileStream fs = new FileStream(fileName, FileMode.Open))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            roomState = (RoomState)formatter.Deserialize(fs);
                        }
                    }
                }
                else
                {
                    if (room == "DJ")
                    {
                        djState = new DJState();
                    }
                    else
                    {
                        roomState = new RoomState();
                    }
                }
                ApplyRoomState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading room state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ApplyRoomState()
        {
            if (room == "DJ")
            {
                currentTemperature = djState.Temperature;
                labelTemperature.Text = djState.Temperature.ToString();

                isImageLightVisible = djState.IsLightOn;
                pictureBoxLight.Image = isImageLightVisible ? Properties.Resources.OnButton : Properties.Resources.OffButton;
                
                label2.Hide();
                pictureBoxVideo.Hide();
            }
            else
            {
                currentTemperature = roomState.Temperature;
                labelTemperature.Text = roomState.Temperature.ToString();

                isImageLightVisible = roomState.IsLightOn;
                pictureBoxLight.Image = isImageLightVisible ? Properties.Resources.OnButton : Properties.Resources.OffButton;

                isImageVideoVisible = roomState.IsVideoPlaying;
                if (room == "EventA" || room == "EventB")
                {
                    pictureBoxVideo.Image = isImageVideoVisible ? Properties.Resources.OnButton : Properties.Resources.OffButton;
                }
                else
                {
                    label2.Hide();
                    pictureBoxVideo.Hide();
                }
            }
        }
        private void SaveAndCloseRoom(string fileName)
        {
            SaveRoomState(fileName);
            this.Close();
        }
        //paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 42;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            }
        }
        //label
        private void UpdateTemperatureLabel()
        {
            labelTemperature.Text = $"{currentTemperature}°C";
        }
        //
        //buttons
        //
        private void PictureBoxLight_Click(object sender, EventArgs e)
        {
            isImageLightVisible = !isImageLightVisible;
            pictureBoxLight.Image = isImageLightVisible ? Properties.Resources.OnButton : Properties.Resources.OffButton;
        }
        private void PictureBoxVideo_Click(object sender, EventArgs e)
        {
            isImageVideoVisible = !isImageVideoVisible;
            pictureBoxVideo.Image = isImageVideoVisible ? Properties.Resources.OnButton : Properties.Resources.OffButton;
        }
        private void ButtonIncrease_Click(object sender, EventArgs e)
        {
            currentTemperature++;
            UpdateTemperatureLabel();
        }
        private void ButtonDecrease_Click(object sender, EventArgs e)
        {
            currentTemperature--;
            UpdateTemperatureLabel();
        }
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            if (room == "CafeTicket")
            {
                SaveAndCloseRoom("roomStateCafeTicket.dat");
                CafeTicket cafeTicket = new CafeTicket(currentUserRole, username, currentUserTicket, money);
                cafeTicket.Show();
            }
            else if (room == "Concert")
            {
                SaveAndCloseRoom("roomStateConcert.dat");
                Concert concert = new Concert(currentUserRole, username, currentUserTicket, money);
                concert.Show();
            }
            else if (room == "DJ")
            {
                SaveAndCloseRoom("roomStateDJ.dat");
                DJ dj = new DJ(currentUserRole, username, currentUserTicket, money);
                dj.Show();
            }
            else if (room == "EventA")
            {
                SaveAndCloseRoom("roomStateEventA.dat");
                EventA eventA = new EventA(currentUserRole, username, currentUserTicket, money);
                eventA.Show();
            }
            else if (room == "EventB")
            {
                SaveAndCloseRoom("roomStateEventB.dat");
                EventB eventB = new EventB(currentUserRole, username, currentUserTicket, money);
                eventB.Show();
            }
            else if (room == "ExhibitionA")
            {
                SaveAndCloseRoom("roomStateExhibitionA.dat");
                ExhibitionA exhibitionA = new ExhibitionA(currentUserRole, username, currentUserTicket, money);
                exhibitionA.Show();
            }
            else if (room == "ExhibitionB")
            {
                SaveAndCloseRoom("roomStateExhibitionB.dat");
                ExhibitionB exhibitionB = new ExhibitionB(currentUserRole, username, currentUserTicket, money);
                exhibitionB.Show();
            }
            else
            {
                MessageBox.Show("Saving error");
            }
        }
    }
}