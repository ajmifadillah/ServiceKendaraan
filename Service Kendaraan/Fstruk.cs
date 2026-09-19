using System;
using System.Data;
using System.Windows.Forms;

namespace Service_Kendaraan
{
    public partial class Fstruk : Form
    {
        private int noTransaksi;
        private string nomorCetak;

        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Fstruk(int noTransaksi, string nomorCetak)
        {
            InitializeComponent();

            this.noTransaksi = noTransaksi;
            this.nomorCetak = nomorCetak;

            TampilkanStruk();
        }

        // =========================================================
        // TAMPILKAN STRUK
        // =========================================================

        private void TampilkanStruk()
        {
            try
            {
                // =================================================
                // 1. AMBIL DATA TRANSAKSI
                // =================================================

                DB.crud(@"
                    SELECT
                        no_transaksi,
                        tanggal,
                        pelanggan,
                        kendaraan,
                        total
                    FROM transaksi
                    WHERE no_transaksi = " + noTransaksi);

                if (DB.ds.Tables.Count == 0 ||
                    DB.ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show(
                        "Data transaksi tidak ditemukan.",
                        "Peringatan",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                DataRow transaksi =
                    DB.ds.Tables[0].Rows[0];


                // =================================================
                // 2. NOMOR TRANSAKSI
                // =================================================

                lblnotransaksi.Text =
                    nomorCetak;


                // =================================================
                // 3. TANGGAL
                // =================================================

                DateTime tanggal =
                    Convert.ToDateTime(
                        transaksi["tanggal"]
                    );

                lbltanggal.Text =
                    tanggal.ToString("dd/MM/yyyy");


                // =================================================
                // 4. DATA PELANGGAN
                // =================================================

                string pelanggan =
                    transaksi["pelanggan"].ToString();

                string idPelanggan = "";

                /*
                 * Pada transaksi kita, kolom pelanggan
                 * menyimpan nama pelanggan.
                 *
                 * Jadi kita cari berdasarkan nama.
                 */

                DB.crud(@"
                    SELECT
                        nama,
                        no_hp
                    FROM pelanggan
                    WHERE nama = '" +
                    EscapeSql(pelanggan) +
                    "' LIMIT 1");

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dataPelanggan =
                        DB.ds.Tables[0].Rows[0];

                    lblnama.Text =
                        dataPelanggan["nama"].ToString();

                    lblnohp.Text =
                        dataPelanggan["no_hp"].ToString();
                }
                else
                {
                    lblnama.Text =
                        pelanggan;

                    lblnohp.Text =
                        "-";
                }


                // =================================================
                // 5. DATA KENDARAAN
                // =================================================

                string kendaraan =
                    transaksi["kendaraan"].ToString();


                // =================================================
                // 6. AMBIL SEMUA DETAIL TRANSAKSI
                // =================================================

                DB.crud(@"
                    SELECT
                        js.nama_service AS jenis_service,
                        IFNULL(j.nama_jasa, '-') AS jasa,
                        d.jumlah,
                        d.harga_service,
                        d.harga_jasa,
                        d.subtotal
                    FROM detail_transaksi d

                    INNER JOIN jenis_service js
                        ON d.jenis_service = js.id_jenis

                    LEFT JOIN jasa j
                        ON d.jasa = j.id_jasa

                    WHERE d.no_transaksi = " +
                    noTransaksi + @"

                    ORDER BY d.id_detail ASC
                ");

                if (DB.ds.Tables.Count == 0 ||
                    DB.ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show(
                        "Detail transaksi tidak ditemukan.",
                        "Peringatan",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }


                // =================================================
                // 7. BUAT TAMPILAN DETAIL
                // =================================================

                string semuaService = "";
                string semuaHarga = "";
                string semuaJumlah = "";

                foreach (DataRow detail
                    in DB.ds.Tables[0].Rows)
                {
                    // =============================================
                    // JENIS SERVICE
                    // =============================================

                    string jenisService =
                        detail["jenis_service"].ToString();


                    // =============================================
                    // JASA
                    // =============================================

                    string jasa =
                        detail["jasa"].ToString();


                    // =============================================
                    // JUMLAH
                    // =============================================

                    string jumlah =
                        detail["jumlah"].ToString();


                    // =============================================
                    // HARGA SERVICE
                    // =============================================

                    decimal hargaService = 0;

                    decimal.TryParse(
                        detail["harga_service"].ToString(),
                        out hargaService
                    );


                    // =============================================
                    // HARGA JASA
                    // =============================================

                    decimal hargaJasa = 0;

                    decimal.TryParse(
                        detail["harga_jasa"].ToString(),
                        out hargaJasa
                    );


                    // =============================================
                    // NAMA SERVICE
                    // =============================================

                    semuaService +=
                        jenisService +
                        Environment.NewLine;


                    // =============================================
                    // NAMA JASA
                    // =============================================

                    if (!string.IsNullOrWhiteSpace(jasa) &&
                        jasa != "-")
                    {
                        semuaService +=
                            "  " +
                            jasa +
                            Environment.NewLine;
                    }


                    // Jarak antar item
                    semuaService +=
                        Environment.NewLine;


                    // =============================================
                    // HARGA
                    // =============================================

                    semuaHarga +=
                        "Rp " +
                        hargaService.ToString("N0") +
                        Environment.NewLine;

                    if (hargaJasa > 0)
                    {
                        semuaHarga +=
                            "Rp " +
                            hargaJasa.ToString("N0") +
                            Environment.NewLine;
                    }

                    semuaHarga +=
                        Environment.NewLine;


                    // =============================================
                    // JUMLAH
                    // =============================================

                    semuaJumlah +=
                        jumlah +
                        Environment.NewLine +
                        Environment.NewLine;
                }


                // =================================================
                // 8. TAMPILKAN DETAIL KE LABEL
                // =================================================

                lbljenis.Text =
                    semuaService.TrimEnd();

                lblharga.Text =
                    semuaHarga.TrimEnd();

                lbljumlah.Text =
                    semuaJumlah.TrimEnd();


                // =================================================
                // 9. TOTAL TRANSAKSI
                // =================================================

                decimal total = 0;

                decimal.TryParse(
                    transaksi["total"].ToString(),
                    out total
                );

                lbltotal.Text =
                    "Rp " +
                    total.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menampilkan struk:\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================================================
        // ESCAPE SQL
        // =========================================================

        private string EscapeSql(string text)
        {
            if (text == null)
                return "";

            return text.Replace("'", "''");
        }

        // =========================================================
        // PICTURE BOX
        // =========================================================

        private void pictureBox2_Click(object sender,EventArgs e)
        {
            DPetugas Fr = new DPetugas();
            Fr.Show();
            this.Hide();
        }
    }
}