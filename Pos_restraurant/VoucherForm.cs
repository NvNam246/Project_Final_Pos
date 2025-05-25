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

    public partial class VoucherForm : Form
    {
        public decimal SelectedDiscount { get; private set; }
        public VoucherForm()
        {
            InitializeComponent();
        }

        private staff mainForm;

        private void btn10_Click(object sender, EventArgs e)
        {
            SelectedDiscount = 0.1m;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn20_Click(object sender, EventArgs e)
        {
            SelectedDiscount = 0.2m;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn30_Click(object sender, EventArgs e)
        {
            SelectedDiscount = 0.3m;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
