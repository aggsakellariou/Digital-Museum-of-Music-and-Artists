using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class EventA : Form
    {
        //
        // Initialization
        //
        private RoomState roomState;
        private readonly Dictionary<string, Point[]> roomMapping = new Dictionary<string, Point[]>
        {
            {"Coldplay - A Sky Full Of Stars", new Point[] { new Point(611, 79), new Point(611, 152), new Point(709, 202), new Point(709, 128) } },
            {"Control Panel", new Point[] { new Point(83, 298), new Point(83, 367), new Point(113, 367), new Point(113, 298) } },
        };
        private readonly ToolTip roomToolTip = new ToolTip();
        private readonly Dictionary<string, (Type formType, UserRole role, string username, string currentRoom, UserTicket ticket, int money)> roomFormMapping;
        private readonly string currentRoom = "EventA";
        private readonly string username;
        private readonly UserRole currentUserRole;
        private readonly UserTicket currentUserTicket;
        private readonly int money;
        private string hoveredRoom = null;
        private bool video;

        public EventA(UserRole role, string username, UserTicket ticket, int money)
        {
            InitializeComponent();
            this.username = username;
            this.currentUserRole = role;
            this.currentUserTicket = ticket;
            this.money = money;
            labelUsername.Text = username;
            labelMoney.Text = $"{money} €";
            LabelChange(ticket);

            this.roomFormMapping = new Dictionary<string, (Type, UserRole, string, string, UserTicket, int)>
            {
                { "Control Panel", (typeof(ControlPanel), role, username, currentRoom, currentUserTicket, money) },
            };

            ConfigureUIBasedOnRole();
            pictureBoxEventA.Click += PictureBoxEventA_Click;
            pictureBoxEventA.MouseMove += PictureBoxEventA_MouseMove;
            pictureBoxEventA.MouseLeave += PictureBoxEventA_MouseLeave;
            pictureBoxEventA.Paint += PictureBoxEventA_Paint;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            LoadRoomState();
        }
        //
        // Map for Rooms
        //
        private void OpenExistingRoomForm(string room)
        {
            Form existingForm = Application.OpenForms.OfType<Form>().FirstOrDefault(f => f.Text == $"Room: {room}");

            if (existingForm != null)
            {
                existingForm.BringToFront();
            }
            else
            {
                if (roomFormMapping.TryGetValue(room, out var roomFormInfo))
                {
                    Type roomFormType = roomFormInfo.formType;
                    UserRole role = roomFormInfo.role;
                    string username = roomFormInfo.username;
                    UserTicket ticket = roomFormInfo.ticket;
                    int money = roomFormInfo.money;

                    if (room == "Control Panel")
                    {
                        ControlPanel controlPanel = new ControlPanel(role, username, currentRoom, currentUserTicket, money);
                        controlPanel.Show();
                        this.Close();
                    }
                }
                else
                {
                    Console.WriteLine($"No form type mapped for room: {room}");
                }
            }
        }
        private void PictureBoxEventA_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                foreach (var kvp in roomMapping)
                {
                    if (IsPointInPolygon(mouseEventArgs.Location, kvp.Value))
                    {
                        ShowMedia(kvp.Key);
                        return;
                    }
                }
                Console.WriteLine("Keys in roomMapping:");
                foreach (var key in roomMapping.Keys)
                {
                    Console.WriteLine(key);
                }
            }
        }
        private void ShowMedia(string mediaFileName)
        {
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                string videoPath;

                switch (mediaFileName)
                {
                    case "Coldplay - A Sky Full Of Stars":
                        if (video)
                        {
                            videoPath = Path.Combine(executablePath, "Resources", "Coldplay_A_Sky_Full_Of_Stars.mp4");
                            break;
                        }
                        else
                        {
                            String room = null;
                            using (Welcome welcomeForm = new Welcome(username, room, "denyVideo"))
                            {
                                welcomeForm.ShowDialog();
                            }
                            return;
                        }
                    default:
                        OpenExistingRoomForm("Control Panel");
                        return;
                }

                if (File.Exists(videoPath))
                {
                    Console.WriteLine($"File found: {videoPath}");
                    MediaPlayerForm mediaPlayerForm = new MediaPlayerForm(videoPath);
                    mediaPlayerForm.ShowDialog();
                }
                else
                {
                    Console.WriteLine($"File not found: {videoPath}");
                    MessageBox.Show("Video file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsPointInPolygon(PointF point, IEnumerable<Point> polygon)
        {
            int count = polygon.Count();
            PointF[] polygonArray = polygon.Select(p => new PointF(p.X, p.Y)).ToArray();

            bool isInside = false;
            int j = count - 1;

            for (int i = 0; i < count; i++)
            {
                if ((polygonArray[i].Y < point.Y && polygonArray[j].Y >= point.Y ||
                     polygonArray[j].Y < point.Y && polygonArray[i].Y >= point.Y) &&
                     (polygonArray[i].X <= point.X || polygonArray[j].X <= point.X))
                {
                    isInside ^= (polygonArray[i].X + (point.Y - polygonArray[i].Y) / (polygonArray[j].Y - polygonArray[i].Y) * (polygonArray[j].X - polygonArray[i].X) < point.X);
                }
                j = i;
            }

            return isInside;
        }
        private void PictureBoxEventA_MouseMove(object sender, MouseEventArgs e)
        {
            string previouslyHoveredRoom = hoveredRoom;
            bool foundHoveredRoom = false;
            hoveredRoom = null;

            foreach (var kvp in roomMapping)
            {
                if (IsPointInPolygon(e.Location, kvp.Value))
                {
                    hoveredRoom = kvp.Key;
                    foundHoveredRoom = true;
                    Cursor = Cursors.Hand;
                    ShowRoomToolTip(hoveredRoom);
                    break;
                }
            }
            if (!foundHoveredRoom)
            {
                Cursor = Cursors.Default;
                HideRoomToolTip();
            }

            if (hoveredRoom != previouslyHoveredRoom)
            {
                pictureBoxEventA.Invalidate();
            }
        }
        private void PictureBoxEventA_MouseLeave(object sender, EventArgs e)
        {
            hoveredRoom = null;
            Cursor = Cursors.Default;
            HideRoomToolTip();
            pictureBoxEventA.Invalidate();
        }
        private void ShowRoomToolTip(string room)
        {
            Point mousePosition = pictureBoxEventA.PointToClient(Cursor.Position);
            roomToolTip.Show(room, pictureBoxEventA, mousePosition.X, mousePosition.Y - 20);
        }
        private void HideRoomToolTip()
        {
            roomToolTip.Hide(pictureBoxEventA);
        }
        private void PictureBoxEventA_Paint(object sender, PaintEventArgs e)
        {
            if (hoveredRoom != null)
            {
                if (roomMapping.TryGetValue(hoveredRoom, out var roomPoints))
                {
                    using (Pen pen = new Pen(Color.Red, 2))
                    {
                        e.Graphics.DrawPolygon(pen, roomPoints);
                    }
                }
            }
        }
        private void LabelChange(UserTicket ticket)
        {
            if ((ticket & UserTicket.VIP) != 0)
            {
                labelTicket.Text = "VIP";
            }
            else if ((ticket & UserTicket.Regular) != 0)
            {
                labelTicket.Text = "Regular";
            }
            else
            {
                labelTicket.Text = "None";
            }
        }
        //
        // Users
        //
        private void ConfigureUIBasedOnRole()
        {
            if (currentUserRole == UserRole.Employee)
            {
                pictureBoxInfo.Image = Properties.Resources.id_info_employee;
                labelMoney.Hide();
                labelTicket.Hide();
                labelUsername.Hide();
                labelUsernameTitle.Hide();
                labelMoneyTitle.Hide();
                labelTicketTitle.Hide();
                panelMobile.Hide();
            }
            else
            {
                pictureBoxInfo.Image = Properties.Resources.id_info;
            }
        }
        //
        //Buttons
        //
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            //SaveRoomState();
            Map map = new Map(currentUserRole, username, currentUserTicket, money);
            map.Show();
            this.Close();
        }
        private void ButtonCafe_Click(object sender, EventArgs e)
        {
            CafeMobile cafe = new CafeMobile(currentUserRole, username, currentRoom, currentUserTicket, money);
            cafe.Show();
            this.Close();
        }
        //
        // Room State
        //
        private void LoadRoomState()
        {
            try
            {
                if (File.Exists("roomStateEventA.dat"))
                {
                    using (FileStream fs = new FileStream("roomStateEventA.dat", FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        roomState = (RoomState)formatter.Deserialize(fs);
                    }
                }
                else
                {
                    roomState = new RoomState();
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
            labelTemperature.Text = roomState.Temperature + "°C";
            if (roomState.IsLightOn)
            {
                pictureBoxEventA.Image = Properties.Resources.EventA1;
            }
            else
            {
                pictureBoxEventA.Image = Properties.Resources.EventAOff;
            }
            if (roomState.IsVideoPlaying)
            {
                video = true;
            }
            else
            {
                video = false;
            }
        }
        /*private void SaveRoomState()
        {
            try
            {
                roomState.IsLightOn = true;
                roomState.IsVideoPlaying = true;

                using (FileStream fs = new FileStream("roomStateEventA.dat", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, roomState);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving room state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
    }
}