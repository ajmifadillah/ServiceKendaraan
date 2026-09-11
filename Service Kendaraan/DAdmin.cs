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
    public partial class DAdmin : Form
    {
        public DAdmin()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            Form1 Fr = new Form1();
            Fr.Show();
            this.Hide();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            user Hal3 = new user() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            role Hal3 = new role() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DPelanggan Hal3 = new DPelanggan() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            AKendaraan Hal3 = new AKendaraan() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            Jenis_Service Hal3 = new Jenis_Service() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            dashboardadmin Hal3 = new dashboardadmin() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            ATransaksi Hal3 = new ATransaksi() { TopLevel = false, TopMost = true };
            KF.UntukFormMenu(Hal3, pnlkonten);
        }
    }
}
