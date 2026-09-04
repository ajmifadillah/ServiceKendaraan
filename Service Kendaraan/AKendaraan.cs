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
    public partial class AKendaraan : Form
    {
        public AKendaraan()
        {
            InitializeComponent();
        }
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from kendaraan");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_kendaraan"];
                string nm = "" + baris["plat_nomor"];
                string np = "" + baris["merek"];
                string al = "" + baris["tipe"];
                dataGridView1.Rows.Add(idp, nm, np, al);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from kendaraan where plat_nomor like '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_kendaraan"];
                string nm = "" + baris["plat_nomor"];
                string np = "" + baris["merek"];
                string al = "" + baris["tipe"];
                dataGridView1.Rows.Add(idp, nm, np, al);
            }
        }
    }
}
