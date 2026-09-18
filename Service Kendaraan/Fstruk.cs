using System;
using System.Data;
using System.Windows.Forms;

namespace Service_Kendaraan
{
    public partial class Fstruk : Form
    {
        public Fstruk(
            string noTransaksi,
            string pelanggan,
            string service,
            string jumlah,
            string harga,
            string total)
        {
            InitializeComponent();

            string nomorTransaksi = "TRX-" + noTransaksi + "-" + DateTime.Now.ToString("ddMMyyyy");

            lblnotransaksi.Text = nomorTransaksi;

            string idPelanggan = pelanggan.Split('-')[0].Trim();

            DB.crud($@"SELECT * FROM pelanggan WHERE id_pelanggan = '{idPelanggan}'");

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                DataRow data = DB.ds.Tables[0].Rows[0];
                lblnama.Text = data["nama"].ToString();
                lblnohp.Text = data["no_hp"].ToString();
            }
            else
            {
                lblnama.Text = pelanggan;
                lblnohp.Text = "-";
            }

            lbljenis.Text = service;
            lblharga.Text = "Rp " + Convert.ToInt32(harga).ToString("N0");
            lbljumlah.Text = jumlah;
            lbltotal.Text = "Rp " + Convert.ToInt32(total).ToString("N0");
            lbltanggal.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DPetugas Fr = new DPetugas();
            Fr.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Shapes6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Shapes4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Shapes8_Click(object sender, EventArgs e)
        {

        }
    }
}