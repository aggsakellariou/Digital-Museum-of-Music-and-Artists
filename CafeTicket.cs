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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class CafeTicket : Form
    {
        //
        // Initialization
        //
        private RoomState roomState;
        private readonly Dictionary<string, Point[]> roomMapping = new Dictionary<string, Point[]>
        {
            { "Ticket", new Point[] { new Point(124, 195), new Point(124, 271), new Point(175, 295), new Point(226, 271), new Point(226, 199), new Point(171, 171) } },
            { "Cafe", new Point[] { new Point(257, 188), new Point(257, 213), new Point(276, 221), new Point(328, 196), new Point(328, 170), new Point(313, 162) } },
            { "Control Panel", new Point[] { new Point(654, 62), new Point(633, 73), new Point(633, 121), new Point(654, 111) } },
        };
        private readonly ToolTip roomToolTip = new ToolTip();
        private readonly Dictionary<string, (Type formType, UserRole role, string username, string currentRoom, UserTicket ticket, int money)> roomFormMapping;
        private readonly string currentRoom = "CafeTicket";
        private readonly string username;
        private readonly UserRole currentUserRole;
        private readonly UserTicket currentUserTicket;
        private readonly int money;
        private string hoveredRoom = null;

        public CafeTicket(UserRole role, string username, UserTicket ticket, int money)
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
                { "Cafe", (typeof(Cafe), role, username, currentRoom, currentUserTicket, money) },
                { "Ticket", (typeof(Ticket), role, username, currentRoom, currentUserTicket, money) },
            };

            ConfigureUIBasedOnRole();
            pictureBoxCafeTicket.Click += PictureBoxCafeTicket_Click;
            pictureBoxCafeTicket.MouseMove += PictureBoxCafeTicket_MouseMove;
            pictureBoxCafeTicket.MouseLeave += PictureBoxCafeTicket_MouseLeave;
            pictureBoxCafeTicket.Paint += PictureBoxCafeTicket_Paint;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            LoadRoomState();
        }
        //
        // Map for Rooms
        //
        private void OpenExistingRoomForm(string room)
        {
            if (roomFormMapping.TryGetValue(room, out var roomFormInfo))
            {
                Type roomFormType = roomFormInfo.formType;
                UserRole role = roomFormInfo.role;
                string username = roomFormInfo.username;
                UserTicket ticket = roomFormInfo.ticket;
                int money = roomFormInfo.money;

                if (role == UserRole.Employee && room == "Ticket")
                {
                    _ = MessageBox.Show("Employees have access to all rooms. No need to buy a ticket.", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (role == UserRole.Employee && room == "Cafe")
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
                else if (role == UserRole.Customer && room == "Control Panel")
                {
                    using (Welcome welcomeForm = new Welcome(username, room, "controlPanel"))
                    {
                        welcomeForm.ShowDialog();
                    }
                    return;
                }
                else if (role == UserRole.Customer && room == "Cafe")
                {
                    using (Welcome welcomeForm = new Welcome(username, room, "cafe"))
                    {
                        welcomeForm.ShowDialog();
                    }
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
                else if (role == UserRole.Customer && room == "Ticket")
                {
                    using (Welcome welcomeForm = new Welcome(username, room, "ticket"))
                    {
                        welcomeForm.ShowDialog();
                    }
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
        private void PictureBoxCafeTicket_Click(object sender, EventArgs e)
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
        private void PictureBoxCafeTicket_MouseMove(object sender, MouseEventArgs e)
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
                pictureBoxCafeTicket.Invalidate();
            }
        }
        private void PictureBoxCafeTicket_MouseLeave(object sender, EventArgs e)
        {
            hoveredRoom = null;
            Cursor = Cursors.Default;
            HideRoomToolTip();
            pictureBoxCafeTicket.Invalidate();
        }
        private void ShowRoomToolTip(string room)
        {
            Point mousePosition = pictureBoxCafeTicket.PointToClient(Cursor.Position);
            roomToolTip.Show(room, pictureBoxCafeTicket, mousePosition.X, mousePosition.Y - 20);
        }
        private void HideRoomToolTip()
        {
            roomToolTip.Hide(pictureBoxCafeTicket);
        }
        private void PictureBoxCafeTicket_Paint(object sender, PaintEventArgs e)
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
                if (File.Exists("roomStateCafeTicket.dat"))
                {
                    using (FileStream fs = new FileStream("roomStateCafeTicket.dat", FileMode.Open))
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
                pictureBoxCafeTicket.Image = Properties.Resources.CafeTicket1;
            }
            else
            {
                pictureBoxCafeTicket.Image = Properties.Resources.CafeTicketOff;
            }
        }
        /*private void SaveRoomState()
        {
            try
            {
                roomState.IsLightOn = true;

                using (FileStream fs = new FileStream("roomStateCafeTicket.dat", FileMode.Create))
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