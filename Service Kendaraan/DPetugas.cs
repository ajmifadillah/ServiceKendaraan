using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service_Kendaraan
{
    public partial class DPetugas : Form
    {
        public DPetugas()
        {
            InitializeComponent();
        }
        public string IDLogin;
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            PTransaksi Hal3 = new PTransaksi() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            PPelanggan Hal3 = new PPelanggan() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            dashboardpetugas Hal3 = new dashboardpetugas() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Form1 Fr = new Form1();
            Fr.Show();
            this.Hide();
        }

        private void pnlkonten_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            PKendaraan Hal3 = new PKendaraan() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }
    }
}
