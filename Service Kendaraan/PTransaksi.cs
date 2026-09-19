using System;
using System.Data;
using System.Windows.Forms;

namespace Service_Kendaraan
{
    public partial class PTransaksi : Form
    {
        public PTransaksi()
        {
            InitializeComponent();

            // DataGridView
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;

            // Event DataGridView
            dataGridView1.CellClick -= dataGridView1_CellClick;
            dataGridView1.CellClick += dataGridView1_CellClick;

            // Event tombol
            guna2Button2.Click -= guna2Button2_Click;
            guna2Button2.Click += guna2Button2_Click;

            guna2Button3.Click -= guna2Button3_Click_2;
            guna2Button3.Click += guna2Button3_Click_2;

            // Event customer
            cmbid.SelectedIndexChanged -= cmbid_SelectedIndexChanged;
            cmbid.SelectedIndexChanged += cmbid_SelectedIndexChanged;

            // Event jasa
            cmbjasa.SelectedIndexChanged -= cmbjasa_SelectedIndexChanged;
            cmbjasa.SelectedIndexChanged += cmbjasa_SelectedIndexChanged;
        }

        // =========================================================
        // FORM LOAD
        // =========================================================

        private void PTransaksi_Load(object sender, EventArgs e)
        {
            AturKolomGrid();

            LoadPelanggan();
            LoadJenisService();
            LoadJasa();

            cmbkendaraan.DataSource = null;
            cmbkendaraan.Items.Clear();

            BersihkanForm();
            TampilkanDataTransaksi();
        }

        // =========================================================
        // ATUR DATAGRIDVIEW
        // =========================================================

        private void AturKolomGrid()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("no_transaksi", "No Transaksi");
            dataGridView1.Columns.Add("tanggal", "Tanggal");
            dataGridView1.Columns.Add("pelanggan", "Pelanggan");
            dataGridView1.Columns.Add("kendaraan", "Kendaraan");
            dataGridView1.Columns.Add("jenis_service", "Jenis Service");
            dataGridView1.Columns.Add("jasa", "Jasa");
            dataGridView1.Columns.Add("jumlah", "Jumlah");
            dataGridView1.Columns.Add("harga_service", "Harga Service");
            dataGridView1.Columns.Add("harga_jasa", "Harga Jasa");
            dataGridView1.Columns.Add("subtotal", "Subtotal");
        }

        // =========================================================
        // LOAD PELANGGAN
        // =========================================================

        private void LoadPelanggan()
        {
            try
            {
                DB.crud(
                    "SELECT id_pelanggan, nama " +
                    "FROM pelanggan " +
                    "ORDER BY nama"
                );

                if (DB.ds.Tables.Count > 0)
                {
                    cmbid.DataSource = DB.ds.Tables[0];
                    cmbid.DisplayMember = "nama";
                    cmbid.ValueMember = "id_pelanggan";
                    cmbid.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat pelanggan:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================================================
        // LOAD KENDARAAN BERDASARKAN PELANGGAN
        // =========================================================

        private void cmbid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbid.SelectedIndex == -1 ||
                    cmbid.SelectedValue == null ||
                    cmbid.SelectedValue is DataRowView)
                {
                    cmbkendaraan.DataSource = null;
                    cmbkendaraan.Items.Clear();
                    return;
                }

                int idPelanggan;

                if (!int.TryParse(
                    cmbid.SelectedValue.ToString(),
                    out idPelanggan))
                {
                    return;
                }

                LoadKendaraan(idPelanggan);
            }
            catch
            {
                cmbkendaraan.DataSource = null;
                cmbkendaraan.Items.Clear();
            }
        }

        private void LoadKendaraan(int idPelanggan)
        {
            try
            {
                DB.crud(
                    "SELECT id_kendaraan, " +
                    "CONCAT(plat_nomor, ' - ', tipe) AS tampilan " +
                    "FROM kendaraan " +
                    "WHERE id_pelanggan = " + idPelanggan + " " +
                    "ORDER BY id_kendaraan"
                );

                if (DB.ds.Tables.Count > 0)
                {
                    cmbkendaraan.DataSource = DB.ds.Tables[0];
                    cmbkendaraan.DisplayMember = "tampilan";
                    cmbkendaraan.ValueMember = "id_kendaraan";
                    cmbkendaraan.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat kendaraan:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================================================
        // LOAD JENIS SERVICE
        // =========================================================

        private void LoadJenisService()
        {
            try
            {
                DB.crud(
                    "SELECT id_jenis, nama_service, harga, stok " +
                    "FROM jenis_service " +
                    "ORDER BY nama_service"
                );

                if (DB.ds.Tables.Count > 0)
                {
                    cmbservice.DataSource = DB.ds.Tables[0];
                    cmbservice.DisplayMember = "nama_service";
                    cmbservice.ValueMember = "id_jenis";
                    cmbservice.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat jenis service:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================================================
        // LOAD JASA
        // =========================================================

        private void LoadJasa()
        {
            try
            {
                DB.crud(
                    "SELECT id_jasa, nama_jasa, harga_jasa " +
                    "FROM jasa " +
                    "ORDER BY nama_jasa"
                );

                if (DB.ds.Tables.Count > 0)
                {
                    cmbjasa.DataSource = DB.ds.Tables[0];
                    cmbjasa.DisplayMember = "nama_jasa";
                    cmbjasa.ValueMember = "id_jasa";
                    cmbjasa.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal memuat jasa:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================================================
        // HARGA SERVICE
        // =========================================================

        private void cmbservice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbservice.SelectedIndex == -1 ||
                    cmbservice.SelectedValue == null ||
                    cmbservice.SelectedValue is DataRowView)
                {
                    txtbiaya.Text = "0";
                    HitungSubtotal();
                    return;
                }

                int idJenis;

                if (!int.TryParse(
                    cmbservice.SelectedValue.ToString(),
                    out idJenis))
                {
                    txtbiaya.Text = "0";
                    HitungSubtotal();
                    return;
                }

                DB.crud(
                    "SELECT harga FROM jenis_service " +
                    "WHERE id_jenis = " + idJenis
                );

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    decimal harga = Convert.ToDecimal(
                        DB.ds.Tables[0].Rows[0]["harga"]
                    );

                    txtbiaya.Text = harga.ToString("N0");
                }
                else
                {
                    txtbiaya.Text = "0";
                }
            }
            catch
            {
                txtbiaya.Text = "0";
            }

            HitungSubtotal();
        }

        // =========================================================
        // HARGA JASA
        // =========================================================

        private void cmbjasa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbjasa.SelectedIndex == -1 ||
                    cmbjasa.SelectedValue == null ||
                    cmbjasa.SelectedValue is DataRowView)
                {
                    txtbiayajasa.Text = "0";
                    HitungSubtotal();
                    return;
                }

                int idJasa;

                if (!int.TryParse(
                    cmbjasa.SelectedValue.ToString(),
                    out idJasa))
                {
                    txtbiayajasa.Text = "0";
                    HitungSubtotal();
                    return;
                }

                DB.crud(
                    "SELECT harga_jasa FROM jasa " +
                    "WHERE id_jasa = " + idJasa
                );

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    decimal harga = Convert.ToDecimal(
                        DB.ds.Tables[0].Rows[0]["harga_jasa"]
                    );

                    txtbiayajasa.Text = harga.ToString("N0");
                }
                else
                {
                    txtbiayajasa.Text = "0";
                }
            }
            catch
            {
                txtbiayajasa.Text = "0";
            }

            HitungSubtotal();
        }

        // =========================================================
        // JUMLAH
        // =========================================================

        private void txtjumlah_TextChanged(object sender, EventArgs e)
        {
            HitungSubtotal();
        }

        // =========================================================
        // HITUNG SUBTOTAL
        // =========================================================

        private void HitungSubtotal()
        {
            try
            {
                decimal hargaService = AmbilAngka(txtbiaya.Text);
                decimal hargaJasa = AmbilAngka(txtbiayajasa.Text);

                int jumlah = 0;

                int.TryParse(txtjumlah.Text, out jumlah);

                decimal subtotal =
                    (hargaService + hargaJasa) * jumlah;

                txtsubtotal.Text = subtotal.ToString("N0");
            }
            catch
            {
                txtsubtotal.Text = "0";
            }
        }

        // =========================================================
        // HITUNG TOTAL
        // =========================================================

        private void HitungTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string noTransaksi =
                    Convert.ToString(row.Cells["no_transaksi"].Value);

                if (noTransaksi == "BARU")
                {
                    total += AmbilAngka(
                        Convert.ToString(
                            row.Cells["subtotal"].Value
                        )
                    );
                }
            }

            txttotal.Text = total.ToString("N0");
        }

        // =========================================================
        // TAMBAH SERVICE
        // =========================================================

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            if (cmbid.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Pilih pelanggan terlebih dahulu."
                );
                return;
            }

            if (cmbkendaraan.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Pilih kendaraan terlebih dahulu."
                );
                return;
            }

            if (cmbservice.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Pilih jenis service terlebih dahulu."
                );
                return;
            }

            int jumlah;

            if (!int.TryParse(txtjumlah.Text, out jumlah) ||
                jumlah <= 0)
            {
                MessageBox.Show(
                    "Jumlah harus lebih dari 0."
                );
                return;
            }

            int stok = AmbilStokService();

            if (jumlah > stok)
            {
                MessageBox.Show(
                    "Stok service tidak mencukupi.\n" +
                    "Stok tersedia: " + stok
                );
                return;
            }

            string pelanggan = cmbid.Text;
            string kendaraan = cmbkendaraan.Text;
            string service = cmbservice.Text;
            string jasa = cmbjasa.Text;

            decimal hargaService =
                AmbilAngka(txtbiaya.Text);

            decimal hargaJasa =
                AmbilAngka(txtbiayajasa.Text);

            decimal subtotal =
                AmbilAngka(txtsubtotal.Text);

            int rowIndex =
                dataGridView1.Rows.Add();

            DataGridViewRow row =
                dataGridView1.Rows[rowIndex];

            row.Cells["no_transaksi"].Value = "BARU";

            row.Cells["tanggal"].Value =
                DateTime.Now.ToString("dd-MM-yyyy");

            row.Cells["pelanggan"].Value =
                pelanggan;

            row.Cells["kendaraan"].Value =
                kendaraan;

            row.Cells["jenis_service"].Value =
                service;

            row.Cells["jasa"].Value =
                jasa;

            row.Cells["jumlah"].Value =
                jumlah;

            row.Cells["harga_service"].Value =
                hargaService.ToString("N0");

            row.Cells["harga_jasa"].Value =
                hargaJasa.ToString("N0");

            row.Cells["subtotal"].Value =
                subtotal.ToString("N0");

            row.Tag = null;

            HitungTotal();

            BersihkanInput();
        }

        // =========================================================
        // AMBIL STOK SERVICE
        // =========================================================

        private int AmbilStokService()
        {
            try
            {
                if (cmbservice.SelectedIndex == -1 ||
                    cmbservice.SelectedValue == null ||
                    cmbservice.SelectedValue is DataRowView)
                {
                    return 0;
                }

                int idJenis;

                if (!int.TryParse(
                    cmbservice.SelectedValue.ToString(),
                    out idJenis))
                {
                    return 0;
                }

                DB.crud(
                    "SELECT stok FROM jenis_service " +
                    "WHERE id_jenis = " + idJenis
                );

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(
                        DB.ds.Tables[0].Rows[0]["stok"]
                    );
                }
            }
            catch
            {
            }

            return 0;
        }

        // =========================================================
        // SIMPAN TRANSAKSI
        // =========================================================

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow firstNewRow = null;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string no =
                        Convert.ToString(
                            row.Cells["no_transaksi"].Value
                        );

                    if (no == "BARU")
                    {
                        firstNewRow = row;
                        break;
                    }
                }

                if (firstNewRow == null)
                {
                    MessageBox.Show(
                        "Belum ada service yang ditambahkan.",
                        "Informasi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                string pelanggan =
                    Convert.ToString(
                        firstNewRow.Cells["pelanggan"].Value
                    );

                string kendaraan =
                    Convert.ToString(
                        firstNewRow.Cells["kendaraan"].Value
                    );

                int idPelanggan =
                    AmbilIdDariText(pelanggan);

                if (idPelanggan <= 0)
                {
                    MessageBox.Show(
                        "Data pelanggan tidak ditemukan."
                    );
                    return;
                }

                int idKendaraan =
                    AmbilIdKendaraan(
                        kendaraan,
                        idPelanggan
                    );

                if (idKendaraan <= 0)
                {
                    MessageBox.Show(
                        "Data kendaraan tidak ditemukan."
                    );
                    return;
                }

                // =================================================
                // SIMPAN HEADER TRANSAKSI
                // =================================================

                decimal total =
                    AmbilAngka(txttotal.Text);

                string sqlHeader =
                    "INSERT INTO transaksi " +
                    "(tanggal, pelanggan, kendaraan, total) " +
                    "VALUES " +
                    "(CURDATE(), " +
                    "'" + EscapeSql(pelanggan) + "', " +
                    "'" + EscapeSql(kendaraan) + "', " +
                    total.ToString(System.Globalization.CultureInfo.InvariantCulture) +
                    ")";

                DB.crud(sqlHeader);

                // =================================================
                // AMBIL NOMOR TRANSAKSI
                // =================================================

                DB.crud(
                    "SELECT LAST_INSERT_ID() AS no_transaksi"
                );

                if (DB.ds.Tables.Count == 0 ||
                    DB.ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show(
                        "Nomor transaksi tidak berhasil dibuat."
                    );
                    return;
                }

                int noTransaksi =
                    Convert.ToInt32(
                        DB.ds.Tables[0]
                          .Rows[0]["no_transaksi"]
                    );

                // =================================================
                // SIMPAN SEMUA DETAIL
                // =================================================

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string no =
                        Convert.ToString(
                            row.Cells["no_transaksi"].Value
                        );

                    if (no != "BARU")
                        continue;

                    string jenisService =
                        Convert.ToString(
                            row.Cells["jenis_service"].Value
                        );

                    string jasa =
                        Convert.ToString(
                            row.Cells["jasa"].Value
                        );

                    int jumlah = 0;

                    int.TryParse(
                        Convert.ToString(
                            row.Cells["jumlah"].Value
                        ),
                        out jumlah
                    );

                    decimal hargaService =
                        AmbilAngka(
                            Convert.ToString(
                                row.Cells["harga_service"].Value
                            )
                        );

                    decimal hargaJasa =
                        AmbilAngka(
                            Convert.ToString(
                                row.Cells["harga_jasa"].Value
                            )
                        );

                    decimal subtotal =
                        AmbilAngka(
                            Convert.ToString(
                                row.Cells["subtotal"].Value
                            )
                        );

                    // Ambil ID jenis service
                    int idJenis =
                        AmbilIdJenisService(
                            jenisService
                        );

                    // Ambil ID jasa
                    int idJasa = 0;

                    if (!string.IsNullOrWhiteSpace(jasa) &&
                        jasa != "-" &&
                        jasa != "System.Data.DataRowView")
                    {
                        idJasa =
                            AmbilIdJasa(jasa);
                    }

                    if (idJenis <= 0)
                    {
                        MessageBox.Show(
                            "Jenis service tidak ditemukan: " +
                            jenisService
                        );
                        return;
                    }

                    string sqlDetail =
                        "INSERT INTO detail_transaksi " +
                        "(no_transaksi, jenis_service, jasa, jumlah, " +
                        "harga_service, harga_jasa, subtotal) VALUES (" +

                        noTransaksi + ", " +

                        idJenis + ", " +

                        (idJasa > 0
                            ? idJasa.ToString()
                            : "NULL") + ", " +

                        jumlah + ", " +

                        hargaService.ToString(
                            System.Globalization.CultureInfo.InvariantCulture
                        ) + ", " +

                        hargaJasa.ToString(
                            System.Globalization.CultureInfo.InvariantCulture
                        ) + ", " +

                        subtotal.ToString(
                            System.Globalization.CultureInfo.InvariantCulture
                        ) +

                        ")";

                    DB.crud(sqlDetail);

                    // =============================================
                    // KURANGI STOK
                    // =============================================

                    DB.crud(
                        "UPDATE jenis_service " +
                        "SET stok = stok - " + jumlah + " " +
                        "WHERE id_jenis = " + idJenis
                    );
                }

                // =================================================
                // FORMAT NOMOR UNTUK DITAMPILKAN DI STRUK
                // =================================================

                string nomorCetak =
                    "TRX-" +
                    noTransaksi +
                    "-" +
                    DateTime.Now.ToString("ddMMyyyy");

                // =================================================
                // BUKA STRUK
                // =================================================

                Fstruk struk =
                    new Fstruk(
                        noTransaksi,
                        nomorCetak
                    );

                struk.ShowDialog();

                // =================================================
                // RESET
                // =================================================

                BersihkanForm();

                TampilkanDataTransaksi();

                MessageBox.Show(
                    "Transaksi berhasil disimpan.",
                    "Berhasil",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menyimpan transaksi:\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================================================
        // AMBIL ID PELANGGAN
        // =========================================================

        private int AmbilIdDariText(string nama)
        {
            try
            {
                DB.crud(
                    "SELECT id_pelanggan " +
                    "FROM pelanggan " +
                    "WHERE nama = '" +
                    EscapeSql(nama) +
                    "' LIMIT 1"
                );

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(
                        DB.ds.Tables[0]
                          .Rows[0]["id_pelanggan"]
                    );
                }
            }
            catch
            {
            }

            return 0;
        }

        // =========================================================
        // AMBIL ID KENDARAAN
        // =========================================================

        private int AmbilIdKendaraan(
            string text,
            int idPelanggan)
        {
            try
            {
                string plat = text;

                if (text.Contains(" - "))
                {
                    plat =
                        text.Split(
                            new string[] { " - " },
                            StringSplitOptions.None
                        )[0];
                }

                DB.crud(
                    "SELECT id_kendaraan " +
                    "FROM kendaraan " +
                    "WHERE plat_nomor = '" +
                    EscapeSql(plat) +
                    "' " +
                    "AND id_pelanggan = " +
                    idPelanggan +
                    " LIMIT 1"
                );

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(
                        DB.ds.Tables[0]
                          .Rows[0]["id_kendaraan"]
                    );
                }
            }
            catch
            {
            }

            return 0;
        }

        // =========================================================
        // AMBIL ID JENIS SERVICE
        // =========================================================

        private int AmbilIdJenisService(string nama)
        {
            try
            {
                DB.crud(
                    "SELECT id_jenis " +
                    "FROM jenis_service " +
                    "WHERE nama_service = '" +
                    EscapeSql(nama) +
                    "' LIMIT 1"
                );

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(
                        DB.ds.Tables[0]
                          .Rows[0]["id_jenis"]
                    );
                }
            }
            catch
            {
            }

            return 0;
        }

        // =========================================================
        // AMBIL ID JASA
        // =========================================================

        private int AmbilIdJasa(string nama)
        {
            try
            {
                DB.crud(
                    "SELECT id_jasa " +
                    "FROM jasa " +
                    "WHERE nama_jasa = '" +
                    EscapeSql(nama) +
                    "' LIMIT 1"
                );

                if (DB.ds.Tables.Count > 0 &&
                    DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(
                        DB.ds.Tables[0]
                          .Rows[0]["id_jasa"]
                    );
                }
            }
            catch
            {
            }

            return 0;
        }

        // =========================================================
        // TAMPILKAN DATA TRANSAKSI
        // =========================================================

        private void TampilkanDataTransaksi()
        {
            try
            {
                dataGridView1.Rows.Clear();

                DB.crud(
                    "SELECT " +
                    "t.no_transaksi, " +
                    "t.tanggal, " +
                    "t.pelanggan, " +
                    "t.kendaraan, " +
                    "js.nama_service AS jenis_service, " +
                    "IFNULL(j.nama_jasa, '-') AS jasa, " +
                    "d.jumlah, " +
                    "d.harga_service, " +
                    "d.harga_jasa, " +
                    "d.subtotal, " +
                    "d.id_detail " +

                    "FROM transaksi t " +

                    "INNER JOIN detail_transaksi d " +
                    "ON t.no_transaksi = d.no_transaksi " +

                    "INNER JOIN jenis_service js " +
                    "ON d.jenis_service = js.id_jenis " +

                    "LEFT JOIN jasa j " +
                    "ON d.jasa = j.id_jasa " +

                    "ORDER BY t.no_transaksi DESC, d.id_detail ASC"
                );

                if (DB.ds.Tables.Count == 0)
                    return;

                foreach (DataRow dr in DB.ds.Tables[0].Rows)
                {
                    int index =
                        dataGridView1.Rows.Add();

                    DataGridViewRow row =
                        dataGridView1.Rows[index];

                    row.Cells["no_transaksi"].Value =
                        dr["no_transaksi"];

                    row.Cells["tanggal"].Value =
                        Convert.ToDateTime(
                            dr["tanggal"]
                        ).ToString("dd-MM-yyyy");

                    row.Cells["pelanggan"].Value =
                        dr["pelanggan"];

                    row.Cells["kendaraan"].Value =
                        dr["kendaraan"];

                    row.Cells["jenis_service"].Value =
                        dr["jenis_service"];

                    row.Cells["jasa"].Value =
                        dr["jasa"];

                    row.Cells["jumlah"].Value =
                        dr["jumlah"];

                    row.Cells["harga_service"].Value =
                        Convert.ToDecimal(
                            dr["harga_service"]
                        ).ToString("N0");

                    row.Cells["harga_jasa"].Value =
                        Convert.ToDecimal(
                            dr["harga_jasa"]
                        ).ToString("N0");

                    row.Cells["subtotal"].Value =
                        Convert.ToDecimal(
                            dr["subtotal"]
                        ).ToString("N0");

                    row.Tag =
                        Convert.ToInt32(
                            dr["id_detail"]
                        );
                }

                HitungTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menampilkan transaksi:\n" +
                    ex.Message
                );
            }
        }

        // =========================================================
        // CELL CLICK
        // =========================================================

        private void dataGridView1_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row =
                dataGridView1.Rows[e.RowIndex];

            try
            {
                cmbid.Text =
                    Convert.ToString(
                        row.Cells["pelanggan"].Value
                    );

                cmbkendaraan.Text =
                    Convert.ToString(
                        row.Cells["kendaraan"].Value
                    );

                cmbservice.Text =
                    Convert.ToString(
                        row.Cells["jenis_service"].Value
                    );

                string jasa =
                    Convert.ToString(
                        row.Cells["jasa"].Value
                    );

                if (jasa == "-")
                    cmbjasa.SelectedIndex = -1;
                else
                    cmbjasa.Text = jasa;

                txtjumlah.Text =
                    Convert.ToString(
                        row.Cells["jumlah"].Value
                    );

                txtbiaya.Text =
                    Convert.ToString(
                        row.Cells["harga_service"].Value
                    );

                txtbiayajasa.Text =
                    Convert.ToString(
                        row.Cells["harga_jasa"].Value
                    );

                txtsubtotal.Text =
                    Convert.ToString(
                        row.Cells["subtotal"].Value
                    );
            }
            catch
            {
            }
        }

        // =========================================================
        // EDIT
        // =========================================================

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(
                    "Pilih data terlebih dahulu."
                );
                return;
            }

            DataGridViewRow row =
                dataGridView1.CurrentRow;

            if (row.IsNewRow)
                return;

            row.Cells["pelanggan"].Value =
                cmbid.Text;

            row.Cells["kendaraan"].Value =
                cmbkendaraan.Text;

            row.Cells["jenis_service"].Value =
                cmbservice.Text;

            row.Cells["jasa"].Value =
                string.IsNullOrWhiteSpace(cmbjasa.Text)
                    ? "-"
                    : cmbjasa.Text;

            row.Cells["jumlah"].Value =
                txtjumlah.Text;

            row.Cells["harga_service"].Value =
                txtbiaya.Text;

            row.Cells["harga_jasa"].Value =
                txtbiayajasa.Text;

            row.Cells["subtotal"].Value =
                txtsubtotal.Text;

            // Kalau data lama, update detail
            if (row.Tag != null)
            {
                try
                {
                    int idDetail =
                        Convert.ToInt32(row.Tag);

                    int idJenis =
                        AmbilIdJenisService(
                            cmbservice.Text
                        );

                    int idJasa = 0;

                    if (!string.IsNullOrWhiteSpace(cmbjasa.Text) &&
                        cmbjasa.Text != "-")
                    {
                        idJasa =
                            AmbilIdJasa(cmbjasa.Text);
                    }

                    int jumlah = 0;

                    int.TryParse(
                        txtjumlah.Text,
                        out jumlah
                    );

                    decimal hargaService =
                        AmbilAngka(txtbiaya.Text);

                    decimal hargaJasa =
                        AmbilAngka(txtbiayajasa.Text);

                    decimal subtotal =
                        AmbilAngka(txtsubtotal.Text);

                    DB.crud(
                        "UPDATE detail_transaksi SET " +
                        "jenis_service = " + idJenis + ", " +
                        "jasa = " +
                        (idJasa > 0
                            ? idJasa.ToString()
                            : "NULL") + ", " +
                        "jumlah = " + jumlah + ", " +
                        "harga_service = " +
                        hargaService.ToString(
                            System.Globalization.CultureInfo.InvariantCulture
                        ) + ", " +
                        "harga_jasa = " +
                        hargaJasa.ToString(
                            System.Globalization.CultureInfo.InvariantCulture
                        ) + ", " +
                        "subtotal = " +
                        subtotal.ToString(
                            System.Globalization.CultureInfo.InvariantCulture
                        ) +
                        " WHERE id_detail = " +
                        idDetail
                    );

                    MessageBox.Show(
                        "Data berhasil diubah."
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Gagal mengubah data:\n" +
                        ex.Message
                    );
                }
            }

            HitungTotal();
            BersihkanInput();
        }

        // =========================================================
        // HAPUS
        // =========================================================

        private void guna2Button3_Click_2(
            object sender,
            EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(
                    "Pilih data terlebih dahulu."
                );
                return;
            }

            DataGridViewRow row =
                dataGridView1.CurrentRow;

            if (row.IsNewRow)
                return;

            DialogResult result =
                MessageBox.Show(
                    "Yakin ingin menghapus data ini?",
                    "Konfirmasi",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result != DialogResult.Yes)
                return;

            try
            {
                // Data yang belum disimpan
                if (row.Tag == null)
                {
                    dataGridView1.Rows.Remove(row);
                    HitungTotal();
                    BersihkanInput();
                    return;
                }

                int idDetail =
                    Convert.ToInt32(row.Tag);

                int jumlah =
                    Convert.ToInt32(
                        row.Cells["jumlah"].Value
                    );

                string jenisService =
                    Convert.ToString(
                        row.Cells["jenis_service"].Value
                    );

                int idJenis =
                    AmbilIdJenisService(
                        jenisService
                    );

                // Kembalikan stok
                DB.crud(
                    "UPDATE jenis_service " +
                    "SET stok = stok + " + jumlah + " " +
                    "WHERE id_jenis = " + idJenis
                );

                // Hapus detail
                DB.crud(
                    "DELETE FROM detail_transaksi " +
                    "WHERE id_detail = " + idDetail
                );

                // Hapus transaksi jika sudah tidak punya detail
                string noTransaksi =
                    Convert.ToString(
                        row.Cells["no_transaksi"].Value
                    );

                DB.crud(
                    "DELETE FROM transaksi " +
                    "WHERE no_transaksi = " +
                    noTransaksi +
                    " AND NOT EXISTS (" +
                    "SELECT 1 FROM detail_transaksi " +
                    "WHERE detail_transaksi.no_transaksi = transaksi.no_transaksi" +
                    ")"
                );

                MessageBox.Show(
                    "Data berhasil dihapus."
                );

                TampilkanDataTransaksi();
                BersihkanForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Gagal menghapus data:\n" +
                    ex.Message
                );
            }
        }

        // =========================================================
        // SEARCH
        // =========================================================

        private void txtcari_TextChanged(
            object sender,
            EventArgs e)
        {
            string keyword =
                txtcari.Text.Trim().ToLower();

            foreach (DataGridViewRow row
                in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                bool ditemukan = false;

                foreach (DataGridViewCell cell
                    in row.Cells)
                {
                    string value =
                        Convert.ToString(cell.Value)
                        .ToLower();

                    if (value.Contains(keyword))
                    {
                        ditemukan = true;
                        break;
                    }
                }

                row.Visible = ditemukan;
            }
        }

        // =========================================================
        // BERSIHKAN INPUT
        // =========================================================

        private void BersihkanInput()
        {
            cmbservice.SelectedIndex = -1;
            cmbjasa.SelectedIndex = -1;

            txtjumlah.Text = "1";
            txtbiaya.Text = "0";
            txtbiayajasa.Text = "0";
            txtsubtotal.Text = "0";
        }

        // =========================================================
        // BERSIHKAN FORM
        // =========================================================

        private void BersihkanForm()
        {
            cmbid.SelectedIndex = -1;

            cmbkendaraan.DataSource = null;
            cmbkendaraan.Items.Clear();

            cmbservice.SelectedIndex = -1;
            cmbjasa.SelectedIndex = -1;

            txtjumlah.Text = "1";
            txtbiaya.Text = "0";
            txtbiayajasa.Text = "0";
            txtsubtotal.Text = "0";
            txttotal.Text = "0";
        }

        // =========================================================
        // KONVERSI ANGKA
        // =========================================================

        private decimal AmbilAngka(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            string angka =
                text.Replace(".", "")
                    .Replace(",", "")
                    .Replace("Rp", "")
                    .Trim();

            decimal hasil;

            if (decimal.TryParse(
                angka,
                out hasil))
            {
                return hasil;
            }

            return 0;
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
        // EVENT TAMBAHAN
        // =========================================================

        private void dataGridView1_CellClick_1(
            object sender,
            DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }

        private void guna2Button3_Click_1(
            object sender,
            EventArgs e)
        {
            TampilkanDataTransaksi();
        }

        private void guna2Button3_Click(
            object sender,
            EventArgs e)
        {
            TampilkanDataTransaksi();
        }
    }
}