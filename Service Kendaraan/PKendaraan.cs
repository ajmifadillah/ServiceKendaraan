using System;
using System.Data;
using System.Windows.Forms;

namespace Service_Kendaraan
{
    public partial class PKendaraan : Form
    {
        string idkendaraan = "";

        public PKendaraan()
        {
            InitializeComponent();
        }


        private void LoadPelanggan()
        {
            DB.crud("SELECT id_pelanggan, nama FROM pelanggan ORDER BY nama");

            cmbPelanggan.DataSource = DB.ds.Tables[0];
            cmbPelanggan.DisplayMember = "nama";
            cmbPelanggan.ValueMember = "id_pelanggan";
            cmbPelanggan.SelectedIndex = -1;
        }


        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud(@"SELECT 
                        k.id_kendaraan,
                        k.plat_nomor,
                        k.merek,
                        k.tipe,
                        p.nama
                      FROM kendaraan k
                      LEFT JOIN pelanggan p 
                        ON k.id_pelanggan = p.id_pelanggan
                      ORDER BY k.id_kendaraan");

            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string id = "" + baris["id_kendaraan"];
                string plat = "" + baris["plat_nomor"];
                string merek = "" + baris["merek"];
                string tipe = "" + baris["tipe"];
                string pelanggan = "" + baris["nama"];

                dataGridView1.Rows.Add(id, plat, merek, tipe, pelanggan);

            }
        }


        public void bersih()
        {
            txtplat.Text = "";
            txtmerek.Text = "";
            txttipe.Text = "";
            cmbPelanggan.SelectedIndex = -1;
            label7.Text = "";
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (cmbPelanggan.SelectedIndex == -1)
            {
                MessageBox.Show("Silakan pilih pelanggan terlebih dahulu.");
                return;
            }

            if (txtplat.Text == "" ||
                txtmerek.Text == "" ||
                txttipe.Text == "")
            {
                MessageBox.Show("Data kendaraan belum lengkap.");
                return;
            }

            string idpelanggan = cmbPelanggan.SelectedValue.ToString();

            DB.crud($@"INSERT INTO kendaraan
                       (id_pelanggan, plat_nomor, merek, tipe)
                       VALUES
                       ('{idpelanggan}',
                        '{txtplat.Text}',
                        '{txtmerek.Text}',
                        '{txttipe.Text}')");

            MessageBox.Show("Data berhasil disimpan.");

            tampildata();
            bersih();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int baris = e.RowIndex;

            idkendaraan =
            dataGridView1.Rows[baris].Cells[0].Value?.ToString();

            if (string.IsNullOrEmpty(idkendaraan))
                return;

            DB.crud(
                $"SELECT * FROM kendaraan " +
                $"WHERE id_kendaraan = '{idkendaraan}'"
            );

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                DataRow brs = DB.ds.Tables[0].Rows[0];

                label7.Text = brs["id_kendaraan"].ToString();
                txtplat.Text = brs["plat_nomor"].ToString();
                txtmerek.Text = brs["merek"].ToString();
                txttipe.Text = brs["tipe"].ToString();
                if (brs["id_pelanggan"] != DBNull.Value)
                {
                    cmbPelanggan.SelectedValue = brs["id_pelanggan"].ToString();
                }
                else
                {
                    cmbPelanggan.SelectedIndex = -1;
                }
            }
        }


        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idkendaraan))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            if (cmbPelanggan.SelectedIndex == -1)
            {
                MessageBox.Show("Silakan pilih pelanggan terlebih dahulu.");
                return;
            }

            if (txtplat.Text == "" || txtmerek.Text == "" || txttipe.Text == "")
            {
                MessageBox.Show("Data kendaraan belum lengkap.");
                return;
            }

            string idpelanggan = cmbPelanggan.SelectedValue.ToString();

            DB.crud($@"UPDATE kendaraan SET
                       id_pelanggan = '{idpelanggan}',
                       plat_nomor = '{txtplat.Text}',
                       merek = '{txtmerek.Text}',
                       tipe = '{txttipe.Text}'
                       WHERE id_kendaraan = '{idkendaraan}'");

            MessageBox.Show("Data berhasil diupdate." );
            tampildata();
            bersih();
            idkendaraan = "";
        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }


        private void guna2TextBox4_TextChanged(object sender,EventArgs e)
        {
            dataGridView1.Rows.Clear();

            DB.crud($@"SELECT
                        k.id_kendaraan,
                        k.plat_nomor,
                        k.merek,
                        k.tipe,
                        p.nama
                       FROM kendaraan k
                       LEFT JOIN pelanggan p
                         ON k.id_pelanggan = p.id_pelanggan
                       WHERE k.plat_nomor LIKE '%{guna2TextBox4.Text}%'
                       ORDER BY k.id_kendaraan");

            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string id = "" + baris["id_kendaraan"];
                string plat = "" + baris["plat_nomor"];
                string merek = "" + baris["merek"];
                string tipe = "" + baris["tipe"];
                string pelanggan = "" + baris["nama"];
                dataGridView1.Rows.Add(id,plat,merek,tipe,pelanggan);
               
            }
        }


        private void PKendaraan_Load(object sender,EventArgs e)
        {
            LoadPelanggan();
            tampildata();
        }


        private void guna2Button3_Click_1(object sender,EventArgs e)
        {
            if (string.IsNullOrEmpty(idkendaraan))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DialogResult setuju =
                MessageBox.Show("Apakah mau menghapus kendaraan " + idkendaraan + "?","Pemberitahuan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (setuju == DialogResult.Yes)
            {
                DB.crud(
                    $"DELETE FROM kendaraan " +
                    $"WHERE id_kendaraan = '{idkendaraan}'"
                );

                MessageBox.Show("Data berhasil dihapus.");
                tampildata();
                bersih();
                idkendaraan = "";
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}