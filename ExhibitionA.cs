using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Digital_Museum_of_Music_and_Artists.Properties;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class ExhibitionA : Form
    {
        //
        // Initialization
        //
        private RoomState roomState;
        private readonly Dictionary<string, Point[]> roomMapping = new Dictionary<string, Point[]>
        {
            {"The_Classical", new Point[] { new Point(404, 320), new Point(404, 378), new Point(468, 345), new Point(468, 288) } },

            {"Portrait of Bach", new Point[] { new Point(141, 207), new Point(141, 237), new Point(175, 221), new Point(175, 190) } },
            {"Portrait of Beethoven", new Point[] { new Point(318, 120), new Point(318, 148), new Point(350, 132), new Point(350, 104) } },
            {"The Revolutionary Beethoven", new Point[] { new Point(418, 104), new Point(418, 131), new Point(450, 149), new Point(450, 120) } },
            {"Beethoven's family", new Point[] { new Point(498, 144), new Point(498, 174), new Point(530, 190), new Point(530, 160) } },
            {"Beethoven vs Steibelt", new Point[] { new Point(577, 183), new Point(577, 213), new Point(609, 228), new Point(609, 199) } },
            {"Bach playing the Organ", new Point[] { new Point(657, 223), new Point(657, 252), new Point(690, 269), new Point(690, 240) } },
            {"Bach's family", new Point[] { new Point(736, 262), new Point(736, 293), new Point(769, 309), new Point(769, 278) } },
            {"G Minor", new Point[] { new Point(815, 301), new Point(815, 332), new Point(848, 348), new Point(848, 317) } },

            {"Control Panel", new Point[] { new Point(593, 340), new Point(595, 396), new Point(627, 397), new Point(627, 346) } },
            {"Exhibition B", new Point[] { new Point(139, 369), new Point(139, 390), new Point(180, 390), new Point(170, 385), new Point(182, 380), new Point(160, 368), new Point(149, 374) } },
        };
        private readonly ToolTip roomToolTip = new ToolTip();
        private readonly Dictionary<string, (Type formType, UserRole role, string username, string currentRoom, UserTicket ticket, int money)> roomFormMapping;
        private readonly string currentRoom = "ExhibitionA";
        private readonly string username;
        private readonly UserRole currentUserRole;
        private readonly UserTicket currentUserTicket;
        private readonly int money;
        private string hoveredRoom = null;

        private int currentRoomIndex = 0;

        public ExhibitionA(UserRole role, string username, UserTicket ticket, int money)
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
                { "Exhibition B", (typeof(ExhibitionB), role, username, currentRoom, currentUserTicket, money) },
            };

            ConfigureUIBasedOnRole();
            pictureBoxExhibitionA.Click += PictureBoxExhibitionA_Click;
            pictureBoxExhibitionA.MouseMove += PictureBoxExhibitionA_MouseMove;
            pictureBoxExhibitionA.MouseLeave += PictureBoxExhibitionA_MouseLeave;
            pictureBoxExhibitionA.Paint += PictureBoxExhibitionA_Paint;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            LoadRoomState();

            KeyPreview = true;
            KeyDown += ExhibitionA_KeyDown;
        }

        private void ExhibitionA_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    currentRoomIndex = (currentRoomIndex - 1 + roomMapping.Count) % roomMapping.Count;
                    break;

                case Keys.Right:
                    currentRoomIndex = (currentRoomIndex + 1) % roomMapping.Count;
                    break;
            }
            UpdateCurrentRoom();
        }

        private void UpdateCurrentRoom()
        {
            var rooms = roomMapping.Keys.ToList();
            string newRoom = rooms[currentRoomIndex];
            ShowMedia(newRoom);
            pictureBoxExhibitionA.Invalidate();
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
                    else if (room == "Exhibition B")
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
        private void PictureBoxExhibitionA_Click(object sender, EventArgs e)
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
                    case "The_Classical":
                        videoPath = Path.Combine(executablePath, "Resources", "The_Classical.mp4");
                        break;
                    case "Control Panel":
                        OpenExistingRoomForm("Control Panel");
                        return;
                    case "Exhibition B":
                        OpenExistingRoomForm("Exhibition B");
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
        private void PictureBoxExhibitionA_MouseMove(object sender, MouseEventArgs e)
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
                pictureBoxExhibitionA.Invalidate();
            }
        }
        private void PictureBoxExhibitionA_MouseLeave(object sender, EventArgs e)
        {
            hoveredRoom = null;
            Cursor = Cursors.Default;
            HideRoomToolTip();
            pictureBoxExhibitionA.Invalidate();
        }
        private void ShowRoomToolTip(string room)
        {
            Point mousePosition = pictureBoxExhibitionA.PointToClient(Cursor.Position);
            roomToolTip.Show(room, pictureBoxExhibitionA, mousePosition.X, mousePosition.Y - 20);
        }
        private void HideRoomToolTip()
        {
            roomToolTip.Hide(pictureBoxExhibitionA);
        }
        private void PictureBoxExhibitionA_Paint(object sender, PaintEventArgs e)
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
        //Buttons
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
                if (File.Exists("roomStateExhibitionA.dat"))
                {
                    using (FileStream fs = new FileStream("roomStateExhibitionA.dat", FileMode.Open))
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
                pictureBoxExhibitionA.Image = Properties.Resources.ExhibitionA2;
            }
            else
            {
                pictureBoxExhibitionA.Image = Properties.Resources.ExhibitionA2Off;
            }
        }
        /*private void SaveRoomState()
        {
            try
            {
                roomState.IsLightOn = true;

                using (FileStream fs = new FileStream("roomStateExhibitionA.dat", FileMode.Create))
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
