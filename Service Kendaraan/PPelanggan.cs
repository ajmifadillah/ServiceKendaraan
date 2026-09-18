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
        string idpelanggan = "";
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
            if (e.RowIndex < 0)
                return;

            int baris = e.RowIndex;
            idpelanggan = dataGridView1.Rows[baris].Cells[0].Value?.ToString();

            if (string.IsNullOrEmpty(idpelanggan))
                return;
            DB.crud($"SELECT * FROM pelanggan WHERE id_pelanggan = '{idpelanggan}'");

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                DataRow brs = DB.ds.Tables[0].Rows[0];

                label5.Text = brs["id_pelanggan"].ToString();
                txtnama.Text = brs["nama"].ToString();
                txtnohp.Text = brs["no_hp"].ToString();
                txtalamat.Text = brs["alamat"].ToString();
                
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idpelanggan))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DB.crud($@"
            UPDATE pelanggan SET
            nama = '{txtnama.Text}',
            no_hp = '{txtnohp.Text}',
            alamat = '{txtalamat.Text}'
            WHERE id_pelanggan = '{idpelanggan}'
            ");

            MessageBox.Show("Data berhasil diupdate.");

            tampildata();
            idpelanggan = "";
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

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idpelanggan))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DialogResult setuju = MessageBox.Show(
                "Apakah mau menghapus transaksi " + idpelanggan + "?",
                "Pemberitahuan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (setuju == DialogResult.Yes)
            {
                DB.crud($"DELETE FROM pelanggan WHERE id_pelanggan = '{idpelanggan}'");
                MessageBox.Show("Data berhasil dihapus.");
                tampildata();

                idpelanggan = "";
                label5.Text = "";
                txtnama.Text = "";
                txtnohp.Text = "";
                txtalamat.Text = "";
                
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    }

