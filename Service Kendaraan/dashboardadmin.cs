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
    public partial class dashboardadmin : Form
    {
        public dashboardadmin()
        {
            InitializeComponent();
        }
        private void HitungTotalPelanggan()
        {
            DB.crud("SELECT COUNT(*) FROM pelanggan");


            int total = Convert.ToInt32(DB.ds.Tables[0].Rows[0][0]);


            label4.Text = total.ToString();
        }
        private void HitungTotalKendaraan()
        {
            DB.crud("SELECT COUNT(*) FROM kendaraan");


            int total = Convert.ToInt32(DB.ds.Tables[0].Rows[0][0]);


            label7.Text = total.ToString();
        }
        private void HitungTotalTransaksi()
        {
            DB.crud("SELECT COUNT(*) FROM transaksi");


            int total = Convert.ToInt32(DB.ds.Tables[0].Rows[0][0]);


            label10.Text = total.ToString();
        }
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from pelanggan");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_pelanggan"];
                string nm = "" + baris["nama"];
                string np = "" + baris["no_hp"];
                string al = "" + baris["alamat"];
                dataGridView1.Rows.Add(idp, nm, np, al);
            }
        }
        public void Tampildata()
        {
            
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void dashboardadmin_Load(object sender, EventArgs e)
        {
            HitungTotalPelanggan();
            HitungTotalKendaraan();
            HitungTotalTransaksi();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
