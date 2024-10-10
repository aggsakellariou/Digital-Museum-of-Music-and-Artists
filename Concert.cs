using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class Concert : Form
    {
        //
        // Initialization
        //
        private RoomState roomState;
        private readonly Dictionary<string, Point[]> roomMapping = new Dictionary<string, Point[]>
        {
            { "Control Panel", new Point[] { new Point(452, 175), new Point(482, 191), new Point(544, 160), new Point(516, 143) } },
        };
        private readonly ToolTip roomToolTip = new ToolTip();
        private readonly Dictionary<string, (Type formType, UserRole role, string username, string currentRoom, UserTicket ticket, int money)> roomFormMapping;
        private readonly string currentRoom = "Concert";
        private readonly string username;
        private readonly UserRole currentUserRole;
        private readonly UserTicket currentUserTicket;
        private readonly int money;
        private string hoveredRoom = null;
        private readonly SoundPlayer soundPlayer = new SoundPlayer();

        public Concert(UserRole role, string username, UserTicket ticket, int money)
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
            pictureBoxConcert.Click += PictureBoxConcert_Click;
            pictureBoxConcert.MouseMove += PictureBoxConcert_MouseMove;
            pictureBoxConcert.MouseLeave += PictureBoxConcert_MouseLeave;
            pictureBoxConcert.Paint += PictureBoxConcert_Paint;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            LoadRoomState();
            InitializeMousic("Pop2_Dua_Lipa_Houdini.wav");
        }
        //
        // Map for Rooms
        //
        private void InitializeMousic(string selectedSong)
        {
            soundPlayer.SoundLocation = selectedSong;
            soundPlayer.Play();
        }
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
        private void PictureBoxConcert_Click(object sender, EventArgs e)
        {
            if (e is MouseEventArgs mouseEventArgs)
            {
                foreach (var kvp in roomMapping)
                {
                    if (IsPointInPolygon(mouseEventArgs.Location, kvp.Value))
                    {
                        OpenExistingRoomForm(kvp.Key);
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
        private void PictureBoxConcert_MouseMove(object sender, MouseEventArgs e)
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
                pictureBoxConcert.Invalidate();
            }
        }
        private void PictureBoxConcert_MouseLeave(object sender, EventArgs e)
        {
            hoveredRoom = null;
            Cursor = Cursors.Default;
            HideRoomToolTip();
            pictureBoxConcert.Invalidate();
        }
        private void ShowRoomToolTip(string room)
        {
            Point mousePosition = pictureBoxConcert.PointToClient(Cursor.Position);
            roomToolTip.Show(room, pictureBoxConcert, mousePosition.X, mousePosition.Y - 20);
        }
        private void HideRoomToolTip()
        {
            roomToolTip.Hide(pictureBoxConcert);
        }
        private void PictureBoxConcert_Paint(object sender, PaintEventArgs e)
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
            soundPlayer.Stop();
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
                if (File.Exists("roomStateConcert.dat"))
                {
                    using (FileStream fs = new FileStream("roomStateConcert.dat", FileMode.Open))
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
                pictureBoxConcert.Image = Properties.Resources.Concert1;
            }
            else
            {
                pictureBoxConcert.Image = Properties.Resources.ConcertOff;
            }
        }
        private void SaveRoomState()
        {
            try
            {
                roomState.IsLightOn = true;

                using (FileStream fs = new FileStream("roomStateConcert.dat", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, roomState);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving room state: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}