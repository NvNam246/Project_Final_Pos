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
using System.Xml.Linq;

namespace Pos_restraurant
{
    public partial class AddMenuFood : Form
    {
        function fn = new function();
        string query;
        public AddMenuFood()
        {
            InitializeComponent();
        }
        public void clearAll()
        {
            txtFoodname.Clear();
            txtCategory.SelectedIndex = -1;
            txtDescription.Clear();
            txtPrice.Clear();

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtFoodname.Text != "" && txtPrice.Text != "" && txtCategory.Text != "" && txtDescription.Text != "")
            {
                string foodname = txtFoodname.Text;
                string price = txtPrice.Text;
                string category = txtCategory.Text;
                string description = txtDescription.Text;

                query = "insert into [Food] (Name,Price,Category,Description) values ('" + foodname + "','" + price + "','" + category + "','" + description + "')";
                fn.setData(query, "Food Added");


                AddMenuFood_Load(this, null);
                clearAll();
            }
            else
            {
                MessageBox.Show("Please fill in all the required information.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AddMenuFood_Load(object sender, EventArgs e)
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

        private void AddMenuFood_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void txtCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtFoodname_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
