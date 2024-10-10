using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class CafeMobile : Form
    {
        //
        // Initialization
        //
        private readonly string room;
        private readonly UserRole currentUserRole;
        private readonly string username;
        private readonly UserTicket currentUserTicket;
        private int money;

        public CafeMobile(UserRole role, string username, string room, UserTicket currentUserTicket, int money)
        {
            InitializeComponent();
            this.room = room;
            this.username = username;
            this.currentUserRole = role;
            this.currentUserTicket = currentUserTicket;
            this.money = money;

            numericUpDownCofe.ValueChanged += NumericUpDownQuantity_ValueChanged;
            numericUpDownSnack.ValueChanged += NumericUpDownQuantity_ValueChanged;
            UpdateTotalCost();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TransparencyKey = Color.Magenta;
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
        //
        // Methods
        //
        private void NumericUpDownQuantity_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotalCost();
        }
        private void UpdateTotalCost()
        {
            int cafeQuantity = (int)numericUpDownCofe.Value;
            int snackQuantity = (int)numericUpDownSnack.Value;
            int cafePrice = 5;
            int snackPrice = 8;
            int cafeCost = cafeQuantity * cafePrice;
            int snackCost = snackQuantity * snackPrice;
            int totalCost = cafeCost + snackCost;

            labelCafeCost.Text = $"{cafeCost} €";
            labelSnackCost.Text = $"{snackCost} €";
            labelTotalCost.Text = $"{totalCost} €";
        }
        // paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 113;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            }
        }
        private void Map()
        {
            if (room == "EventA")
            {
                EventA eventA = new EventA(currentUserRole, username, currentUserTicket, money);
                eventA.Show();
                this.Close();
            }
            else if (room == "EventB")
            {
                EventB eventB = new EventB(currentUserRole, username, currentUserTicket, money);
                eventB.Show();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
        //
        // Buttons
        //
        private void ButtonPurchase_Click(object sender, EventArgs e)
        {
            int cafeQuantity = (int)numericUpDownCofe.Value;
            int snackQuantity = (int)numericUpDownSnack.Value;
            int regularTicketPrice = 5;
            int vipTicketPrice = 8;

            int totalCost = (cafeQuantity * regularTicketPrice) + (snackQuantity * vipTicketPrice);

            if (totalCost > 0)
            {
                if (totalCost <= money)
                {
                    DialogResult result = MessageBox.Show($"Are you sure about the purchase?", "Purchase", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        money -= totalCost;
                        Map();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    _ = MessageBox.Show("Insufficient funds to purchase product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You need to select at least one item to purchase.");
            }
              
        }
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Map();
        }
    }
}
