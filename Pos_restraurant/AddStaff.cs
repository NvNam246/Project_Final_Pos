using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pos_restraurant
{
    
    public partial class AddStaff : Form
    {
        function fn = new function();
        string query;
        public AddStaff()
        {
            InitializeComponent();
        }

        private void AddStaff_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel? The Data will not be saved !", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }

        public void clearAll()
        {
            txtName.Clear();
            txtRole.SelectedIndex = -1;
            txtUsername.Clear();
            txtPassword.Clear();
            
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtUsername.Text != "" && txtPassword.Text != "" && txtRole.Text != "")
            {
                string name = txtName.Text;
                string username = txtUsername.Text;
                string password = txtPassword.Text;
                string role = txtRole.Text;
               
                query = "insert into [User] (Name,Username,Password,Role) values ('" + name + "','" + username + "','" + password + "','" + role + "')";
                fn.setData(query, "Employee Added");


                AddStaff_Load(this, null);
                clearAll();
            }
            else
            {
                MessageBox.Show("Please fill in all the required information.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
    }
}
