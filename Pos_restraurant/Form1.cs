using System;
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
    public partial class Form1 : Form
    {
        function fn = new function();
        string query;
        public static string LoggedInUser = "";
        public static string UserRole = "";
        public static string UserName = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            query = "select Username,Password,Role from [User] where Username = '" + txtUsername.Text + "' and Password ='" + txtPassword.Text + "'";
            DataSet ds = fn.GetData(query);


            if (ds.Tables[0].Rows.Count != 0)
            {
                lblloginError.Visible = false;
                LoggedInUser = txtUsername.Text;
                UserRole = ds.Tables[0].Rows[0]["role"].ToString().ToLower();


                staff staff = new staff();
                this.Hide();
                staff.Show();
            }
            else
            {
                lblloginError.Visible = true;
                txtPassword.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
