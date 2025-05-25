using System;
using System.Data;
using System.Windows.Forms;

namespace Pos_restraurant
{
    public partial class AdminForm : Form
    {
        private function fn = new function();

        public AdminForm()
        {
            InitializeComponent();
            this.Load += AdminForm_Load;
            cbSortBy.SelectedIndexChanged += cbSortBy_SelectedIndexChanged;
            btnLogout.Click += btnLogout_Click;
          
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadStatistics();
            LoadEmployees();
            LoadMenuItems();
            if (cbSortBy.Items.Count > 0)
                cbSortBy.SelectedIndex = 0;
        }

        private void LoadStatistics()
        {
            dgvStatsList.DataSource = fn.GetData("SELECT FoodID, Name, Price, Category, Description FROM Food").Tables[0];
            dgvStatsRank.DataSource = fn.GetData("SELECT TOP 10 Name, Price FROM Food ORDER BY Price DESC").Tables[0];
        }

        private void cbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvStatsList.DataSource is DataTable dt)
            {
                string sortExpr = cbSortBy.SelectedItem.ToString() == "Price" ? "Price DESC" : "Name ASC";
                dt.DefaultView.Sort = sortExpr;
                dgvStatsList.DataSource = dt.DefaultView.ToTable();
            }
        }

        private void LoadEmployees()
        {
            dgvEmployees.DataSource = fn.GetData("SELECT UserID,Name, Username, Role FROM [User]").Tables[0];
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            AddStaff addstaff = new AddStaff();
            addstaff.Show();
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedIDStaff)) 
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this employee?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM [User] WHERE UserID = '" + selectedIDStaff + "'";
                    fn.setData(query, "Employee Deleted");

                }
            }
            else
            {
                MessageBox.Show("Please select an employee first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadMenuItems()
        {
            dgvMenuItems.DataSource = fn.GetData("SELECT FoodID, Name, Price, Category, Description FROM Food").Tables[0];
        }

        private void btnAddMenuItem_Click(object sender, EventArgs e)
        {
            AddMenuFood addfood = new AddMenuFood();
            addfood.Show();
        }

        private void btnDeleteMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedIDFood))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this food?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM [Food] WHERE FoodID = '" + selectedIDFood + "'";
                    fn.setData(query, "Food Deleted");

                }
            }
            else
            {
                MessageBox.Show("Please select an food first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void cbSortBy_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void LoadAllItems()
        {
            string query = "SELECT UserID,Name, Username, Role FROM [User]";
            DataSet ds = fn.GetData(query);
            dgvEmployees.DataSource = ds.Tables[0];
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllItems();
        }

       
        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        string selectedIDStaff = "";
        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >=0)
            {
                DataGridViewRow row = dgvEmployees.Rows[e.RowIndex];
                selectedIDStaff = row.Cells["UserID"].Value.ToString();
            }

        }
        string selectedIDFood = "";

        private void dgvMenuItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row1 = dgvMenuItems.Rows[e.RowIndex];
                selectedIDFood = row1.Cells["FoodID"].Value.ToString();
            }
        }

        private void dgvMenuItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadAllFoodItems()
        {
            string query = "SELECT FoodID,Name, Price, Category FROM [Food]";
            DataSet ds = fn.GetData(query);
            dgvMenuItems.DataSource = ds.Tables[0];
        }
       
        private void btnRefreshfood_Click(object sender, EventArgs e)
        {
            LoadAllFoodItems();
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void dgvStatsList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
