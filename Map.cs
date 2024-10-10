using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public enum UserRole
{
    Customer,
    Employee
}
[Flags]
public enum UserTicket
{
    None = 0,
    Regular = 1,
    VIP = 2,
    Employee = 4,
    EventA = 8,
    EventB = 16,
    DJ = 32
}

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class Map : Form
    {
        //
        // Initialization
        //
        private readonly UserRole currentUserRole;
        private readonly UserTicket currentUserTicket;
        private readonly Dictionary<string, (Type formType, UserRole role, string username, UserTicket ticket, int money)> roomFormMapping;
        private string hoveredRoom = null;
       
        private readonly List<string> roomNavigationOrder;       
        private int currentRoomIndex;
        private string selectedRoom;
        private bool tabNavigationMode = false;

        public Map(UserRole role, string username, UserTicket ticket, int money)
        {
            InitializeComponent();

            this.currentUserRole = role;
            this.currentUserTicket = ticket;
            labelUsername.Text = username;
            labelMoney.Text = $"{money} €";
            LabelChange(ticket);

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(buttonHelp, "Help");

            // Initialize roomNavigationOrder with the order of rooms you want
            roomNavigationOrder = new List<string> { "", "DJ", "Concert", "Event A", "Event B", "Exhibition A", "Cafe Ticket" };
            currentRoomIndex = 0;
            selectedRoom = roomNavigationOrder[currentRoomIndex];
            tabNavigationMode = false;

            // Initialize roomFormMapping
            roomFormMapping = new Dictionary<string, (Type, UserRole, string, UserTicket, int)>
            {
                { "DJ", (typeof(DJ), currentUserRole, labelUsername.Text, currentUserTicket, money) },
                { "Concert", (typeof(Concert), currentUserRole, labelUsername.Text, currentUserTicket, money) },
                { "Event A", (typeof(EventA), currentUserRole, labelUsername.Text, currentUserTicket, money) },
                { "Event B", (typeof(EventB), currentUserRole, labelUsername.Text, currentUserTicket, money) },
                { "Exhibition A", (typeof(ExhibitionA), currentUserRole, labelUsername.Text, currentUserTicket, money) },
                { "Cafe Ticket", (typeof(CafeTicket), currentUserRole, labelUsername.Text, currentUserTicket, money) },
            };
            
            ConfigureUIBasedOnRole();
            pictureBoxBig.Click += PictureBoxBig_Click;
            pictureBoxBig.MouseMove += PictureBoxBig_MouseMove;
            pictureBoxBig.MouseLeave += PictureBoxBig_MouseLeave;           
            this.PreviewKeyDown += Map_PreviewKeyDown;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
        private readonly Dictionary<string, Point[]> roomMapping = new Dictionary<string, Point[]>
        {
            { "Concert", new Point[] { new Point(134, 277), new Point(266, 345), new Point(488, 234), new Point(488, 197), new Point(454, 180), new Point(454, 143), new Point(355, 94), new Point(355, 127), new Point(134, 238) } },
            { "DJ", new Point[] { new Point(686, 344), new Point(776, 390), new Point(870, 344), new Point(870, 305), new Point(778, 259), new Point(686, 303) } },
            { "Event B", new Point[] { new Point(516, 109), new Point(613, 159), new Point(742, 93), new Point(742, 53), new Point(645, 5), new Point(516, 68) } },
            { "Event A", new Point[] { new Point(646, 207), new Point(741, 256), new Point(872, 192), new Point(872, 151), new Point(774, 103), new Point(646, 166) } },
            { "Exhibition A", new Point[] { new Point(554, 430), new Point(736, 523), new Point(846, 466), new Point(846, 432), new Point(663, 339), new Point(554, 394) } },
            /*{ "ExhibitionB", new Point[] { new Point(350, 507), new Point(531, 598), new Point(643, 543), new Point(643, 507), new Point(461, 415), new Point(350, 470) } },*/
            { "Cafe Ticket", new Point[] { new Point(338, 368), new Point(425, 413), new Point(699, 275), new Point(622, 237), new Point(589, 251), new Point(530, 221), new Point(366, 303), new Point(420, 328) } },
        };
        private readonly ToolTip roomToolTip = new ToolTip();
        //
        // Map for Rooms
        //
        private void OpenExistingRoomForm(string room)
        {
            // If the form is not open, create a new instance and show it
            if (roomFormMapping.TryGetValue(room, out var roomFormInfo))
            {
                Type roomFormType = roomFormInfo.formType;
                UserRole role = roomFormInfo.role;
                string username = roomFormInfo.username;
                UserTicket ticket = roomFormInfo.ticket;
                int money = roomFormInfo.money;
                ConstructorInfo constructor = roomFormType.GetConstructor(new Type[] { typeof(UserRole), typeof(string), typeof(UserTicket), typeof(int) });

                if (role == UserRole.Customer)
                {
                    if (room == "Concert")
                    {
                        if ((ticket & (UserTicket.Regular | UserTicket.VIP)) == 0)
                        {
                            Welcome(username, room, "denyConcert");
                            return;
                        }
                        else if ((ticket & UserTicket.Regular) != 0)
                        {
                            Welcome(username, room, "welcome");
                        }
                        else if ((ticket & UserTicket.VIP) != 0)
                        {
                            Welcome(username, room, "welcomeVIP");
                        }
                    }
                    else if (room == "Event A")
                    {
                        if ((ticket & UserTicket.EventA) == 0)
                        {
                            Welcome(username, room, "denyEvent");
                            return;
                        }
                        else if ((ticket & UserTicket.EventA) != 0)
                        {
                            Welcome(username, room, "welcome");
                        }
                    }  
                    else if (room == "Event B")
                    {
                        if ((ticket & UserTicket.EventB) == 0)
                        {
                            Welcome(username, room, "denyEvent");
                            return;
                        }
                        else if ((ticket & UserTicket.EventB) != 0)
                        {
                            Welcome(username, room, "welcome");
                        }
                    }
                    else if (room == "DJ")
                    {
                        if ((ticket & UserTicket.DJ) == 0)
                        {
                            Welcome(username, room, "denyEvent");
                            return;
                        }
                        else if ((ticket & UserTicket.DJ) != 0)
                        {
                            Welcome(username, room, "welcome");
                        }
                    }
                    else
                    {
                        Welcome(username, room, "welcome");
                    }
                }

                if (constructor != null)
                {
                    object[] parameters = { role, username, ticket, money };
                    Form roomForm = (Form)constructor.Invoke(parameters);
                    roomForm.Text = $"Room: {room}";
                    roomForm.Show();
                    this.Close();
                }
                else
                {
                    Console.WriteLine($"Constructor not found for {roomFormType.Name}");
                }
            }
            else
            {
                Console.WriteLine($"No form type mapped for room: {room}");
            }
        }
        private void Welcome(string username, string room, string welcomeType)
        {
            using (Welcome welcomeForm = new Welcome(username, room, welcomeType))
            {
                welcomeForm.ShowDialog();
            }
        }       
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                currentRoomIndex = (currentRoomIndex + 1) % roomNavigationOrder.Count;
                selectedRoom = roomNavigationOrder[currentRoomIndex];
                ShowRoomToolTip(selectedRoom);
                pictureBoxBig.Invalidate();
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                OpenExistingRoomForm(selectedRoom);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void PictureBoxBig_Click(object sender, EventArgs e)
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
        private void PictureBoxBig_MouseMove(object sender, MouseEventArgs e)
        {
            if (!tabNavigationMode)
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
                    roomToolTip.Hide(pictureBoxBig);
                }

                if (hoveredRoom != previouslyHoveredRoom)
                {
                    pictureBoxBig.Invalidate();
                }
            }
        }
        private void PictureBoxBig_MouseLeave(object sender, EventArgs e)
        {
            if (!tabNavigationMode)
            {
                currentRoomIndex = 0;
                selectedRoom = roomNavigationOrder[currentRoomIndex];
                hoveredRoom = null;
                Cursor = Cursors.Default;
                roomToolTip.Hide(pictureBoxBig);
                pictureBoxBig.Invalidate();
            }
        }        
        private void Map_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                tabNavigationMode = true;
                hoveredRoom = null;
                pictureBoxBig.Invalidate();
                e.IsInputKey = true;
            }
        }
        private void ShowRoomToolTip(string room)
        {
            Point mousePosition = pictureBoxBig.PointToClient(Cursor.Position);
            roomToolTip.Show(room, pictureBoxBig, mousePosition.X, mousePosition.Y - 20);
        }
        private void PictureBoxBig_Paint(object sender, PaintEventArgs e)
        {
            if (hoveredRoom != null && roomMapping.TryGetValue(hoveredRoom, out var hoveredRoomPoints))
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawPolygon(pen, hoveredRoomPoints);
                }
                ShowRoomToolTip(selectedRoom);
            }
            else if (roomMapping.TryGetValue(selectedRoom, out var selectedRoomPoints))
            {
                using (Pen pen = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawPolygon(pen, selectedRoomPoints);
                }
            }
            else
            {
                roomToolTip.Hide(pictureBoxBig);
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
        private void ButtonLogOut_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "file://C:\\Users\\aggel\\Desktop\\On-Line help.chm");
        }
    }
}