using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Pos_restraurant
{
    public partial class staff : Form
    {
        private function fn = new function();
        private Label lblTotal;
        private RichTextBox rtbInvoice;            
        private PrintDocument printDoc;

        private bool voucherApplied = false;
        private decimal discountPercent = 0;
        public staff()
        {
            InitializeComponent();

            lblTotal = new Label()
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Text = "Total: €0.00"
            };
            this.Controls.Add(lblTotal);

            this.dgvMenu.CellClick += dgvMenu_CellClick;
            this.dgvInvoice.CellClick += dgvInvoice_CellClick;
        }

        private void LoadWeather()
        {
            try
            {
               
                string city = "Rzeszow"; 
                string apiKey = "6b1d71e66534bc1e64eb04354cbda1fa";
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

                string json = new WebClient().DownloadString(url);
                JObject data = JObject.Parse(json);

                string temp = data["main"]["temp"].ToString();
                string desc = data["weather"][0]["description"].ToString();

                txtWeather.Text = $"Location: {city}\r\nTemp: {temp}°C\r\nStatus: {desc}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Weather Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtWeather.Text = "Unable to load weather data.";
            }
        }

        private void staff_Load(object sender, EventArgs e)
        {
            LoadWeather();
            dgvInvoice.Columns.Clear();
            dgvInvoice.Columns.Add("ID", "ID");
            dgvInvoice.Columns.Add("Name", "Name");
            dgvInvoice.Columns.Add("Price", "Price");
            dgvInvoice.Columns.Add("Quantity", "Quantity");

            LoadAllItems();

            if (Form1.UserRole == "admin")
            {
                btnAdmin.Visible = true;  
                pictureBoxAdmin.Visible = true;
            }
            else
            {
                btnAdmin.Visible = false; 
                pictureBoxAdmin.Visible = false;
            }
        }

        private void LoadAllItems()
        {
            string query = "SELECT FoodID, Name, Price, Category, Description FROM Food";
            DataSet ds = fn.GetData(query);
            dgvMenu.DataSource = ds.Tables[0];
        }

        private void dgvMenu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvMenu.Rows[e.RowIndex];
            if (row.Cells["FoodID"].Value == null || row.Cells["Name"].Value == null || row.Cells["Price"].Value == null) return;

            string id = row.Cells["FoodID"].Value.ToString();
            string name = row.Cells["Name"].Value.ToString();
            decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
            int quantity = 1;

            bool found = false;
            foreach (DataGridViewRow invRow in dgvInvoice.Rows)
            {
                if (invRow.Cells["ID"].Value != null && invRow.Cells["ID"].Value.ToString() == id)
                {
                    int currentQty = Convert.ToInt32(invRow.Cells["Quantity"].Value);
                    invRow.Cells["Quantity"].Value = currentQty + 1;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                dgvInvoice.Rows.Add(id, name, price.ToString("F2"), quantity);
            }

            UpdateTotal();
        }

        private void dgvInvoice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvInvoice.Rows[e.RowIndex];
            if (row.Cells["Quantity"].Value == null) return;

            int currentQty = Convert.ToInt32(row.Cells["Quantity"].Value);
            if (currentQty > 1)
            {
                row.Cells["Quantity"].Value = currentQty - 1;
            }
            else
            {
                dgvInvoice.Rows.RemoveAt(e.RowIndex);
            }

            UpdateTotal();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllItems();
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            FilterCategory("Food");
        }

        private void btnDrink_Click(object sender, EventArgs e)
        {
            FilterCategory("Drink");
        }

        private void btnBeverage_Click(object sender, EventArgs e)
        {
            FilterCategory("Beverage");
        }

        private void FilterCategory(string category)
        {
            string query = $"SELECT FoodID, Name, Price, Category, Description FROM Food WHERE Category = '{category}'";
            DataSet ds = fn.GetData(query);
            dgvMenu.DataSource = ds.Tables[0];
        }
        public void ApplyVoucher(decimal discount, int voucherType)
        {
            if (!voucherApplied)
            {
                discountPercent = discount;
                voucherApplied = true;
                UpdateTotal();
            }
            else if (voucherApplied && discountPercent == discount)
            {
                
                discountPercent = 0;
                voucherApplied = false;
                UpdateTotal();
            }
        }

        private void btnAddVoucher_Click(object sender, EventArgs e)
        {
            btndeleteVoucher.Visible = true;
            btnAddVoucher.Visible = false;
            if (voucherApplied)
            {
                MessageBox.Show("A voucher has already been applied!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            VoucherForm voucherForm = new VoucherForm();
            if (voucherForm.ShowDialog() == DialogResult.OK)
            {
                discountPercent = voucherForm.SelectedDiscount; 
                voucherApplied = true;
                UpdateTotal(); 
                MessageBox.Show($"Voucher {discountPercent * 100}% applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

           
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
           
        }

       


        private void UpdateTotal()
        {
            decimal originalTotal = 0;
            foreach (DataGridViewRow row in dgvInvoice.Rows)
            {
                if (row.Cells["Price"].Value != null && row.Cells["Quantity"].Value != null)
                {
                    decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                    int qty = Convert.ToInt32(row.Cells["Quantity"].Value);
                    originalTotal += price * qty;
                }
            }
            if (voucherApplied)
            {
                decimal discountAmount = originalTotal * discountPercent;
                decimal finalTotal = originalTotal - discountAmount;
                lblTotal.Text = $"Total: €{originalTotal:F2} (-{discountPercent * 100}%) => €{finalTotal:F2}";
            }
            else
            {
                lblTotal.Text = $"Total: €{originalTotal:F2}";
            }
        }

        private void dgvMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to logout?", "Logout Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                
                this.Hide();
                Form1 form = new Form1();
                form.Show();


            }
        }

        private void staff_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btndeleteVoucher_Click(object sender, EventArgs e)
        {
            if (voucherApplied)
            {
                voucherApplied = false;
                discountPercent = 0;
                UpdateTotal();
                MessageBox.Show("Voucher has been removed.", "Voucher Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No voucher is currently applied.", "No Voucher", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btndeleteVoucher.Visible = false;
            btnAddVoucher.Visible = true;
        }

        private void panelButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Please double-check your invoice and confirm.",
        "Payment Confirmation",
        MessageBoxButtons.OKCancel,
        MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                MessageBox.Show(
                    "Thank you for your payment!",
                    "Payment Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Hide(); // Close the current form

                // Create and show a new instance of staff form
                staff newStaffForm = new staff();
                newStaffForm.Show(); // Show the new, refreshed form
            }
        }

        

        private void dgvInvoice_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
