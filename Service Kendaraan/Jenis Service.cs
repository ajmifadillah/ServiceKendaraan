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
    public partial class Jenis_Service : Form
    {
        string idjenis = "";
        public Jenis_Service()
        {
            InitializeComponent();
        } 
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from jenis_service");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_jenis"];
                string nm = "" + baris["nama_service"];
                string np = "" + baris["harga"];
                string st = "" + baris["stok"];
                dataGridView1.Rows.Add(idp, nm, np, st);
            }
        }
        public void bersih()
        {
            txtnama.Text = "";
            txtharga.Text = "";
            txtstok.Text = "";
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB.crud($"insert into jenis_service values (null, '{txtnama.Text}','{txtharga.Text}','{txtstok.Text}')");
            tampildata();
            bersih();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from jenis_service where nama_service like '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_jenis"];
                string nm = "" + baris["nama_service"];
                string np = "" + baris["harga"];
                dataGridView1.Rows.Add(idp, nm, np);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int baris = e.RowIndex;
            idjenis = dataGridView1.Rows[baris].Cells[0].Value?.ToString();

            if (string.IsNullOrEmpty(idjenis))
                return;
            DB.crud($"SELECT * FROM jenis_service WHERE id_jenis = '{idjenis}'");

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                DataRow brs = DB.ds.Tables[0].Rows[0];

                label5.Text = brs["id_jenis"].ToString();
                txtnama.Text = brs["nama_service"].ToString();
                txtharga.Text = brs["harga"].ToString();
                txtstok.Text = brs["stok"].ToString();

            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idjenis))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DB.crud($@"
            UPDATE jenis_service SET
            nama_service = '{txtnama.Text}',
            harga = '{txtharga.Text}',
            stok = '{txtstok.Text}'
            WHERE id_jenis = '{idjenis}'
            ");

            MessageBox.Show("Data berhasil diupdate.");

            tampildata();
            idjenis = "";
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void Jenis_Service_Load(object sender, EventArgs e)
        {
            tampildata();
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idjenis))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DialogResult setuju = MessageBox.Show(
                "Apakah mau menghapus transaksi " + idjenis + "?",
                "Pemberitahuan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (setuju == DialogResult.Yes)
            {
                DB.crud($"DELETE FROM jenis_service WHERE id_jenis = '{idjenis}'");
                MessageBox.Show("Data berhasil dihapus.");
                tampildata();

                idjenis = "";
                label5.Text = "";
                txtnama.Text = "";
                txtharga.Text = "";
                txtstok.Text = "";

            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
