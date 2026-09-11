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
    public partial class PKendaraan : Form
    {
        public PKendaraan()
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
        public void bersih()
        {
            txtplat.Text = "";
            txtmerek.Text = "";
            txttipe.Text = "";
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB.crud($"insert into kendaraan values (null, '{txtplat.Text}','{txtmerek.Text}','{txttipe.Text}')");
            tampildata();
            bersih();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = e.RowIndex;
            int kolom = e.ColumnIndex;
            if (kolom == 4)
            {
                string idk = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DB.crud($"select * from kendaraan where id_kendaraan = '{idk}'");
                foreach (DataRow brs in DB.ds.Tables[0].Rows)
                {
                    string idpe = "" + brs["id_kendaraan"];
                    string nm = "" + brs["plat_nomor"];
                    string np = "" + brs["merek"];
                    string al = "" + brs["tipe"];
                    label7.Text = idpe;
                    txtplat.Text = nm;
                    txtmerek.Text = np;
                    txttipe.Text = al;
                }
            }
            if (kolom == 5)
            {
                string idk = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DialogResult setuju = MessageBox.Show("Apakah mau dihapus? " + idk, "pemberitahuan,",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
                if (setuju == DialogResult.Yes)
                {
                    DB.crud($"delete from kendaraan where id_kendaraan = '{idk}'");

                }
                tampildata();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string nu = "" + txtplat.Text;
            string un = "" + txtmerek.Text;
            string ps = "" + txttipe.Text;
            DB.crud($"update kendaraan SET plat_nomor = '{nu}', merek = '{un}', tipe = '{ps}' where id_kendaraan = '{label7.Text}'");
            bersih();
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

        private void PKendaraan_Load(object sender, EventArgs e)
        {
            tampildata();
        }
    }
}
