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
    public partial class Jasa : Form
    {
        string idjasa = "";
        public Jasa()
        {
            InitializeComponent();
        }
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from jasa");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_jasa"];
                string nm = "" + baris["nama_jasa"];
                string np = "" + baris["harga_jasa"];
                dataGridView1.Rows.Add(idp, nm, np);
            }
        }
        public void bersih()
        {
            txtnama.Text = "";
            txtharga.Text = "";
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB.crud($"insert into jasa values (null, '{txtnama.Text}','{txtharga.Text}')");
            tampildata();
            bersih();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idjasa))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DB.crud($@"
            UPDATE jasa SET
            nama_jasa = '{txtnama.Text}',
            harga = '{txtharga.Text}'
            WHERE id_jenis = '{idjasa}'
            ");

            MessageBox.Show("Data berhasil diupdate.");

            tampildata();
            idjasa = "";
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idjasa))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DialogResult setuju = MessageBox.Show(
                "Apakah mau menghapus transaksi " + idjasa + "?",
                "Pemberitahuan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (setuju == DialogResult.Yes)
            {
                DB.crud($"DELETE FROM jasa WHERE id_jasa = '{idjasa}'");
                MessageBox.Show("Data berhasil dihapus.");
                tampildata();

                idjasa = "";
                label5.Text = "";
                txtnama.Text = "";
                txtharga.Text = "";

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int baris = e.RowIndex;
            idjasa = dataGridView1.Rows[baris].Cells[0].Value?.ToString();

            if (string.IsNullOrEmpty(idjasa))
                return;
            DB.crud($"SELECT * FROM jasa WHERE id_jasa = '{idjasa}'");

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                DataRow brs = DB.ds.Tables[0].Rows[0];

                label5.Text = brs["id_jasa"].ToString();
                txtnama.Text = brs["nama_jasa"].ToString();
                txtharga.Text = brs["harga_jasa"].ToString();

            }
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from jasa where nama_jasa like '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["id_jasa"];
                string nm = "" + baris["nama_jasa"];
                string np = "" + baris["harga_jasa"];
                dataGridView1.Rows.Add(idp, nm, np);
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Jasa_Load(object sender, EventArgs e)
        {
            tampildata();
        }
    }
}
