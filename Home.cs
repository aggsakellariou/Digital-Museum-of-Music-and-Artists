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
    public partial class Home : Form
    {
        //
        //Initialization
        //
        public Home()
        {
            InitializeComponent();
            textBoxEmployee.UseSystemPasswordChar = true;

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(buttonHelp, "Help");
        }
        //
        //Buttons
        //
        private void ButtonEmployee_Click(object sender, EventArgs e)
        {
            string password = textBoxEmployee.Text;
            int money = 0;
            if (password == "employee")
            {
                string username = "Employee";
                Map map = new Map(UserRole.Employee, username, UserTicket.Employee, money);
                map.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ButtonCustomer_Click(object sender, EventArgs e)
        {
            string username = textBoxCustomer.Text;
            int money = 200;
            if (!string.IsNullOrWhiteSpace(username) && username.Length >= 6 && username.Length <= 12)
            {
                using (Welcome welcomeForm = new Welcome(username, "Museum", "welcomeMuseum"))
                {
                    welcomeForm.ShowDialog();
                }
                Map map = new Map(UserRole.Customer, username, UserTicket.None, money);
                map.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username must be greater than or equal to 6\nand less than or equal to 12.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "file://C:\\Users\\aggel\\Desktop\\On-Line help.chm");
        }
        //
        //Methods
        //
        private void TextBoxEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEmployee.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void TextBoxCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonCustomer.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
