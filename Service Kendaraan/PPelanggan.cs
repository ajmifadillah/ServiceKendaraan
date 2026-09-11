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
    public partial class PPelanggan : Form
    {
        public PPelanggan()
        {
            InitializeComponent();
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
        public void bersih()
        {
            txtnama.Text = "";
            txtnohp.Text = "";
            txtalamat.Text = "";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB.crud($"insert into pelanggan values (null, '{txtnama.Text}','{txtnohp.Text}','{txtalamat.Text}')");
            tampildata();
            bersih();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = e.RowIndex;
            int kolom = e.ColumnIndex;
            if (kolom == 4)
            {
                string idp = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DB.crud($"select * from pelanggan where id_pelanggan = '{idp}'");
                foreach (DataRow brs in DB.ds.Tables[0].Rows)
                {
                    string idpe = "" + brs["id_pelanggan"];
                    string nm = "" + brs["nama"];
                    string np = "" + brs["no_hp"];
                    string al = "" + brs["alamat"];
                    label5.Text = idpe;
                    txtnama.Text = nm;
                    txtnohp.Text = np;
                    txtalamat.Text = al;
                }
            }
            if (kolom == 5)
            {
                string idp = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DialogResult setuju = MessageBox.Show("Apakah mau dihapus? " + idp, "pemberitahuan,",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
                if (setuju == DialogResult.Yes)
                {
                    DB.crud($"delete from pelanggan where id_pelanggan = '{idp}'");

                }
                tampildata();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string nm = "" + txtnama.Text;
            string np = "" + txtnohp.Text;
            string al = "" + txtalamat.Text;
            DB.crud($"update pelanggan SET nama = '{nm}', no_hp = '{np}', alamat = '{al}' where id_pelanggan = '{label5.Text}'");
            bersih();
            tampildata();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from pelanggan where nama like '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_pelanggan"];
                string nm = "" + baris["nama"];
                string np = "" + baris["no_hp"];
                string al = "" + baris["alamat"];
                dataGridView1.Rows.Add(idp, nm, np, al);
            }
        }

        private void PPelanggan_Load(object sender, EventArgs e)
        {
            tampildata();
        }
    }
}
