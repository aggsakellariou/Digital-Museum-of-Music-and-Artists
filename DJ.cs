using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class DJ : Form
    {
        //
        //Initialization
        //
        private DJState djState;
        private readonly Dictionary<string, Point[]> roomMapping = new Dictionary<string, Point[]>
        {
            { "DJ Deck", new Point[] { new Point(478, 231), new Point(518, 249), new Point(536, 239), new Point(536, 203), new Point(495, 183), new Point(478, 193) } },
            { "Control Panel", new Point[] { new Point(807, 270), new Point(807, 352), new Point(847, 352), new Point(847, 270) } },
            { "Karaoke", new Point[] { new Point(559, 69), new Point(559, 160), new Point(659, 211), new Point(659, 119) } },
        };
        private readonly ToolTip roomToolTip = new ToolTip();
        private readonly Dictionary<string, (Type formType, UserRole role, string username, string currentRoom, UserTicket ticket, int money)> roomFormMapping;
        private readonly string currentRoom = "DJ";
        private readonly string username;
        private readonly UserRole currentUserRole;
        private readonly UserTicket currentUserTicket;
        private LightState lightState;
        private KaraokeState karaokeState;
        private readonly int money;
        private string hoveredRoom = null;

        public DJ(UserRole role, string username, UserTicket ticket, int money)
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
                { "DJ Deck", (typeof(DJDeck), role, username, currentRoom, currentUserTicket, money) },
            };

            ConfigureUIBasedOnRole();
            pictureBoxDJ.Click += PictureBoxDJ_Click;
            pictureBoxDJ.MouseMove += PictureBoxDJ_MouseMove;
            pictureBoxDJ.MouseLeave += PictureBoxDJ_MouseLeave;
            pictureBoxDJ.Paint += PictureBoxDJ_Paint;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            LoadRoomState();
        }
        //
        // Map for deck
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

                    if (room == "DJ Deck" && role == UserRole.Employee)
                    {
                        _ = MessageBox.Show("Employees are not allowed in the DJ Deck.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (room == "DJ Deck" && role == UserRole.Customer)
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
        private void PictureBoxDJ_Click(object sender, EventArgs e)
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
            if (mediaFileName == "Karaoke")
            {
                string room = "";
                if (currentUserRole == UserRole.Customer && karaokeState == KaraokeState.On)
                {
                    string executablePath = AppDomain.CurrentDomain.BaseDirectory;
                    string videoPath = Path.Combine(executablePath, "Resources", "Dua_Lipa-Houdini(Karaoke-edit).mp4");
                    MediaPlayerForm mediaPlayerForm = new MediaPlayerForm(videoPath);
                    mediaPlayerForm.ShowDialog();
                }
                else if (currentUserRole == UserRole.Customer && karaokeState == KaraokeState.Off)
                {
                    using (Welcome welcomeForm = new Welcome(username, room, "denyKaraoke"))
                    {
                        welcomeForm.ShowDialog();
                    }
                }
                else
                {
                    _ = MessageBox.Show("Employees are not allowed in the Karaoke.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (mediaFileName.Equals("Control Panel", StringComparison.OrdinalIgnoreCase))
            {
                OpenExistingRoomForm("Control Panel");
            }
            else if (mediaFileName.Equals("DJ Deck", StringComparison.OrdinalIgnoreCase))
            {
                OpenExistingRoomForm("DJ Deck");
            }
            else
            {
                MessageBox.Show($"Unsupported media format: {mediaFileName}");
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
        private void PictureBoxDJ_MouseMove(object sender, MouseEventArgs e)
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
                pictureBoxDJ.Invalidate();
            }
        }
        private void PictureBoxDJ_MouseLeave(object sender, EventArgs e)
        {
            hoveredRoom = null;
            Cursor = Cursors.Default;
            HideRoomToolTip();
            pictureBoxDJ.Invalidate();
        }
        private void ShowRoomToolTip(string room)
        {
            Point mousePosition = pictureBoxDJ.PointToClient(Cursor.Position);
            roomToolTip.Show(room, pictureBoxDJ, mousePosition.X, mousePosition.Y - 20);
        }
        private void HideRoomToolTip()
        {
            roomToolTip.Hide(pictureBoxDJ);
        }
        private void PictureBoxDJ_Paint(object sender, PaintEventArgs e)
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
            SaveRoomState();
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
                if (File.Exists("roomStateDJ.dat"))
                {
                    using (FileStream fs = new FileStream("roomStateDJ.dat", FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        djState = (DJState)formatter.Deserialize(fs);
                    }
                }
                else
                {
                    djState = new DJState();
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
            karaokeState = djState.KaraokeOn;
            lightState = djState.ColorLight;
            labelTemperature.Text = djState.Temperature + "°C";
            if (lightState == LightState.None)
            {
                if (djState.IsLightOn)
                {
                    pictureBoxDJ.Image = Properties.Resources.DJ1;
                }
                else
                {
                    pictureBoxDJ.Image = Properties.Resources.DJOff;
                }
            }
            else if (lightState == LightState.Red)
            {
                pictureBoxDJ.Image = Properties.Resources.DJRed;
            }
            else if (lightState == LightState.Green)
            {
                pictureBoxDJ.Image = Properties.Resources.DJGreen;
            }
            else if (lightState == LightState.Blue)
            {
                pictureBoxDJ.Image = Properties.Resources.DJBlue;
            }
            else
            {
                MessageBox.Show("light error");
            }
        }
        private void SaveRoomState()
        {
            try
            {
                djState.KaraokeOn = KaraokeState.Off;
                djState.ColorLight = LightState.None;
                djState.IsLightOn = true;

                using (FileStream fs = new FileStream("roomStateDJ.dat", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, djState);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving room state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
