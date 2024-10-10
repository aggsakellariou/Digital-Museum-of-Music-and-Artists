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
    public partial class Cafe : Form
    {
        //
        // Initialization
        //
        private readonly UserRole currentUserRole;
        private readonly string username;
        private readonly UserTicket currentUserTicket;
        private int money;
        public Cafe(UserRole role, string username, UserTicket currentUserTicket, int money)
        {
            InitializeComponent();
            this.username = username;
            this.currentUserRole = role;
            this.currentUserTicket = currentUserTicket;
            this.money = money;

            numericUpDownCofe.ValueChanged += NumericUpDownQuantity_ValueChanged;
            numericUpDownSnack.ValueChanged += NumericUpDownQuantity_ValueChanged;
            UpdateTotalCost();
        }
        // methods
        private void NumericUpDownQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (currentUserRole == UserRole.Customer)
            {
                UpdateTotalCost();
            }
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
        private void Map()
        {
            CafeTicket cafeTicket = new CafeTicket(currentUserRole, username, currentUserTicket, money);
            cafeTicket.Show();
            this.Close();
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

            if(currentUserRole == UserRole.Employee)
            {
                DialogResult result = MessageBox.Show($"Are you sure about the order?", "Order", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    Map();
                }
                else
                {
                    return;
                }
            }
            else if (totalCost <= money)
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
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            CafeTicket cafeTicket = new CafeTicket(currentUserRole, username, currentUserTicket, money);
            cafeTicket.Show();
            this.Close();
        }
    }
}
