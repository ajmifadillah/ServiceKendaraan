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
    public partial class ATransaksi : Form
    {
        public ATransaksi()
        {
            InitializeComponent();
        }
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from transaksi");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["no_transaksi"];
                string nm = "" + baris["tanggal"];
                string np = "" + baris["pelanggan"];
                string al = "" + baris["kendaraan"];
                string js = "" + baris["jenis_service"];
                string jm = "" + baris["jumlah_service"];
                string by = "" + baris["harga"];
                string tl = "" + baris["total"];
                dataGridView1.Rows.Add(idp, nm, np, al, js, jm, by, tl);
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from transaksi where pelanggan like '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["no_transaksi"];
                string nm = "" + baris["tanggal"];
                string np = "" + baris["pelanggan"];
                string al = "" + baris["kendaraan"];
                string js = "" + baris["jenis_service"];
                string jm = "" + baris["jumlah_service"];
                string by = "" + baris["harga"];
                string tl = "" + baris["total"];
                dataGridView1.Rows.Add(idp, nm, np, al, js, jm, by, tl);
            }
        }
    }
}
