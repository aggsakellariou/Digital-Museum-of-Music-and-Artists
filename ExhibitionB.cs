using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class ExhibitionB : Form
    {
        //
        // Initialization
        //
        private RoomState roomState;
        private readonly Dictionary<string, Point[]> roomMapping = new Dictionary<string, Point[]>
        {
            {"Beethoven ", new Point[] { new Point(143, 183), new Point(143, 243), new Point(207, 211), new Point(207, 151) } },
            {"Bach ", new Point[] { new Point(295, 104), new Point(295, 162), new Point(359, 130), new Point(359, 73) } },
            {"Beethoven", new Point[] { new Point(660, 201), new Point(660, 259), new Point(724, 290), new Point(724, 232) } },
            {"Bach", new Point[] { new Point(763, 248), new Point(763, 307), new Point(828, 338), new Point(828, 280) } },

            {"Control Panel", new Point[] { new Point(549, 465), new Point(549, 515), new Point(578, 515), new Point(578, 465) } },
            {"Exhibition A", new Point[] { new Point(558, 160), new Point(580, 172), new Point(605, 160), new Point(616, 165), new Point(616, 145), new Point(575, 145), new Point(581, 150) } },
        };
        private readonly ToolTip roomToolTip = new ToolTip();
        private readonly Dictionary<string, (Type formType, UserRole role, string username, string currentRoom, UserTicket ticket, int money)> roomFormMapping;
        private readonly string currentRoom = "ExhibitionB";
        private readonly string username;
        private readonly UserRole currentUserRole;
        private readonly UserTicket currentUserTicket;
        private readonly int money;
        private string hoveredRoom = null;

        public ExhibitionB(UserRole role, string username, UserTicket ticket, int money)
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
                { "Exhibition A", (typeof(ExhibitionA), role, username, currentRoom, currentUserTicket, money) },
            };

            ConfigureUIBasedOnRole();
            pictureBoxExhibitionB.Click += PictureBoxExhibitionB_Click;
            pictureBoxExhibitionB.MouseMove += PictureBoxExhibitionB_MouseMove;
            pictureBoxExhibitionB.MouseLeave += PictureBoxExhibitionB_MouseLeave;
            pictureBoxExhibitionB.Paint += PictureBoxExhibitionB_Paint;
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

                    if (role == UserRole.Customer && room == "Control Panel")
                    {
                        using (Welcome welcomeForm = new Welcome(username, room, "controlPanel"))
                        {
                            welcomeForm.ShowDialog();
                        }
                        return;
                    }
                    else if (room == "Exhibition A")
                    {
                        this.Close();
                        ConstructorInfo constructor = roomFormType.GetConstructor(new Type[] { typeof(UserRole), typeof(string), typeof(UserTicket), typeof(int) });

                        if (constructor != null)
                        {
                            object[] parameters = { role, username, ticket, money };
                            Form roomForm = (Form)constructor.Invoke(parameters);
                            roomForm.Text = $"Room: {room}";
                            roomForm.Show();
                        }
                        else
                        {
                            Console.WriteLine($"Constructor not found for {roomFormType.Name}");
                        }
                    }
                    else
                    {
                        this.Close();
                        ConstructorInfo constructor = roomFormType.GetConstructor(new Type[] { typeof(UserRole), typeof(string), typeof(string), typeof(UserTicket), typeof(int) });

                        if (constructor != null)
                        {
                            object[] parameters = { role, username, currentRoom, ticket, money };
                            Form roomForm = (Form)constructor.Invoke(parameters);
                            roomForm.Text = $"Room: {room}";
                            roomForm.Show();
                        }
                        else
                        {
                            Console.WriteLine($"Constructor not found for {roomFormType.Name}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"No form type mapped for room: {room}");
                }
            }
        }
        private void PictureBoxExhibitionB_Click(object sender, EventArgs e)
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
                    case "Beethoven":
                    case "Beethoven ":
                        videoPath = Path.Combine(executablePath, "Resources", "Beethoven's genius explained in 3 minutes (1080p).mp4");
                        break;
                    case "Bach":
                    case "Bach ":
                        videoPath = Path.Combine(executablePath, "Resources", "Untitled-video-bach.mp4");
                        break;
                    case "Control Panel":
                        OpenExistingRoomForm("Control Panel");
                        return;
                    case "Exhibition A":
                        OpenExistingRoomForm("Exhibition A");
                        return;
                    default:
                        Pictures pictures = new Pictures(mediaFileName);
                        pictures.ShowDialog();
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
        private void PictureBoxExhibitionB_MouseMove(object sender, MouseEventArgs e)
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
                pictureBoxExhibitionB.Invalidate();
            }
        }
        private void PictureBoxExhibitionB_MouseLeave(object sender, EventArgs e)
        {
            hoveredRoom = null;
            Cursor = Cursors.Default;
            HideRoomToolTip();
            pictureBoxExhibitionB.Invalidate();
        }
        private void ShowRoomToolTip(string room)
        {
            Point mousePosition = pictureBoxExhibitionB.PointToClient(Cursor.Position);
            roomToolTip.Show(room, pictureBoxExhibitionB, mousePosition.X, mousePosition.Y - 20);
        }
        private void HideRoomToolTip()
        {
            roomToolTip.Hide(pictureBoxExhibitionB);
        }
        private void PictureBoxExhibitionB_Paint(object sender, PaintEventArgs e)
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
            }
            else
            {
                pictureBoxInfo.Image = Properties.Resources.id_info;
            }
        }
        //
        // Buttons
        //
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            //SaveRoomState();
            Map map = new Map(currentUserRole, username, currentUserTicket, money);
            map.Show();
            this.Close();
        }
        //
        // Room State
        //
        private void LoadRoomState()
        {
            try
            {
                if (File.Exists("roomStateExhibitionB.dat"))
                {
                    using (FileStream fs = new FileStream("roomStateExhibitionB.dat", FileMode.Open))
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
                pictureBoxExhibitionB.Image = Properties.Resources.ExhibitionB;
            }
            else
            {
                pictureBoxExhibitionB.Image = Properties.Resources.ExhibitionBOff;
            }
        }
        /*private void SaveRoomState()
        {
            try
            {
                roomState.IsLightOn = true;

                using (FileStream fs = new FileStream("roomStateExhibitionB.dat", FileMode.Create))
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