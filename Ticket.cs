using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Digital_Museum_of_Music_and_Artists
{
    public partial class Ticket : Form
    {
        //
        //Initialization
        //
        private readonly UserRole currentUserRole;
        private readonly string username;
        private UserTicket currentUserTicket;
        private int money;

        public Ticket(UserRole role, string username, UserTicket currentUserTicket, int money)
        {
            InitializeComponent();
            this.username = username;
            this.currentUserRole = role;
            this.currentUserTicket = currentUserTicket;
            this.money = money;
        }
        //
        // Buttons
        //
        private void ButtonRegular_Click(object sender, EventArgs e)
        {
            if (currentUserTicket.HasFlag(UserTicket.Regular))
            {
                MessageBox.Show("You have already purchased a Regular ticket.", "Ticket Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (currentUserTicket.HasFlag(UserTicket.VIP))
                {
                    DialogResult result = MessageBox.Show("You changed your ticket from VIP to Regular. We returned you 10 euros.", "Ticket Change", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        currentUserTicket &= ~UserTicket.VIP;
                        money += 10;
                        currentUserTicket |= UserTicket.Regular;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (money >= 10)
                    {
                        DialogResult result = MessageBox.Show("Are you sure about the purchase?", "Purchase", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            currentUserTicket |= UserTicket.Regular;
                            money -= 10;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ButtonVIP_Click(object sender, EventArgs e)
        {
            if (currentUserTicket.HasFlag(UserTicket.VIP))
            {
                MessageBox.Show("You have already purchased a VIP ticket", "Ticket Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (currentUserTicket.HasFlag(UserTicket.Regular))
                {
                    DialogResult result = MessageBox.Show("You changed your ticket from Regular to VIP. We took 10 euros from you.", "Ticket Change", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        currentUserTicket &= ~UserTicket.Regular;
                        money -= 10;
                        currentUserTicket |= UserTicket.VIP;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (money >= 20)
                    {
                        DialogResult result = MessageBox.Show("Are you sure about the purchase?", "Purchase", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            currentUserTicket |= UserTicket.VIP;
                            money -= 20;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        _ = MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void ButtonEventA_Click(object sender, EventArgs e)
        {
            UpdateTicket(UserTicket.EventA, 50);
        }
        private void ButtonEventB_Click(object sender, EventArgs e)
        {
            UpdateTicket(UserTicket.EventB, 50);
        }
        private void ButtonDJ_Click(object sender, EventArgs e)
        {
            UpdateTicket(UserTicket.DJ, 60);
        }
        private void ButtonAll_Click(object sender, EventArgs e)
        {
            if (currentUserTicket.HasFlag(UserTicket.Regular) ||
                currentUserTicket == UserTicket.EventA ||
                currentUserTicket == UserTicket.EventB ||
                currentUserTicket == UserTicket.DJ)
            {
                MessageBox.Show("You have already bought a ticket from the package.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (money >= 170)
                {
                    DialogResult result = MessageBox.Show("Are you sure about the purchase?", "Purchase", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        currentUserTicket |= UserTicket.Regular;
                        currentUserTicket |= UserTicket.EventA;
                        currentUserTicket |= UserTicket.EventB;
                        currentUserTicket |= UserTicket.DJ;
                        money -= 150;
                        Map();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    _ = MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void ButtonBack_Click(object sender, EventArgs e)
        {
            Map();
        }
        //
        // Methods
        //
        private void UpdateTicket(UserTicket newTicket, int cost)
        {
            if (currentUserTicket.HasFlag(newTicket))
            {
                MessageBox.Show($"You have already purchased a {newTicket} ticket.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (money >= cost)
                {
                    DialogResult result = MessageBox.Show("Are you sure about the purchase?", "Purchase", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        currentUserTicket |= newTicket;
                        money -= cost;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    _ = MessageBox.Show("Insufficient balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void Map()
        {
            CafeTicket cafeTicket = new CafeTicket(currentUserRole, username, currentUserTicket, money);
            cafeTicket.Show();
            this.Close();
        }
    }
}
