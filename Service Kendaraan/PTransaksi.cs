using System;
using System.Data;
using System.Windows.Forms;
namespace Service_Kendaraan
{
    public partial class PTransaksi : Form
    {
        private int idPelangganTerpilih = 0;
        public PTransaksi()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.CellClick -= dataGridView1_CellClick;
            dataGridView1.CellClick += dataGridView1_CellClick;
            cmbid.TextChanged -= cmbid_TextChanged;
            cmbid.TextChanged += cmbid_TextChanged;
            cmbid.SelectedIndexChanged -= cmbid_SelectedIndexChanged;
            cmbid.SelectedIndexChanged += cmbid_SelectedIndexChanged;
            cmbservice.SelectedIndexChanged -= cmbservice_SelectedIndexChanged;
            cmbservice.SelectedIndexChanged += cmbservice_SelectedIndexChanged;
            cmbjasa.SelectedIndexChanged -= cmbjasa_SelectedIndexChanged;
            cmbjasa.SelectedIndexChanged += cmbjasa_SelectedIndexChanged;
            txtjumlah.TextChanged -= txtjumlah_TextChanged;
            txtjumlah.TextChanged += txtjumlah_TextChanged;
            guna2Button2.Click -= guna2Button2_Click;
            guna2Button2.Click += guna2Button2_Click;
            guna2Button3.Click -= guna2Button3_Click_2;
            guna2Button3.Click += guna2Button3_Click_2;
        }
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
        private void LoadPelanggan()
        {
            try
            {
                DB.crud("SELECT id_pelanggan, nama " + "FROM pelanggan " + "ORDER BY nama");
                if (DB.ds.Tables.Count == 0 || DB.ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Data pelanggan tidak ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DataTable dt = DB.ds.Tables[0];
                cmbid.DataSource = null;
                cmbid.Items.Clear();
                cmbid.DropDownStyle = ComboBoxStyle.DropDown;
                cmbid.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbid.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection daftarPelanggan = new AutoCompleteStringCollection();
                foreach (DataRow row in dt.Rows)
                {
                    string id = row["id_pelanggan"].ToString();
                    string nama = row["nama"].ToString();
                    string tampilan = id + " - " + nama;
                    cmbid.Items.Add(tampilan);
                    if (!string.IsNullOrWhiteSpace(nama))
                    {
                        daftarPelanggan.Add(nama);
                    }
                }
                cmbid.AutoCompleteCustomSource = daftarPelanggan;
                cmbid.SelectedIndex = -1;
                cmbid.Text = "";
                idPelangganTerpilih = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat pelanggan:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmbid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string teks = cmbid.Text.Trim();
                if (string.IsNullOrWhiteSpace(teks))
                {
                    idPelangganTerpilih = 0;
                    cmbkendaraan.DataSource = null;
                    cmbkendaraan.Items.Clear();
                    return;
                }
                int id = CariIdPelanggan(teks);
                if (id > 0)
                {
                    if (idPelangganTerpilih != id)
                    {
                        idPelangganTerpilih = id;
                        LoadKendaraan(id);
                    }
                }
            }
            catch
            {
                idPelangganTerpilih = 0;
            }
        }
        private void cmbid_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbid.SelectedIndex == -1) return;
                string teks = cmbid.Text.Trim();
                int id = CariIdPelanggan(teks);
                if (id > 0)
                {
                    idPelangganTerpilih = id;
                    LoadKendaraan(id);
                }
            }
            catch
            {
                idPelangganTerpilih = 0;
            }
        }
        private int CariIdPelanggan(string teks)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(teks))
                    return 0;
                if (teks.Contains(" - "))
                {
                    string bagianID = teks.Split(new string[]
                            {
                                " - "
                            }, StringSplitOptions.None)[0];
                    int id;
                    if (int.TryParse(bagianID, out id))
                    {
                        return id;
                    }
                }
                DB.crud("SELECT id_pelanggan " + "FROM pelanggan " + "WHERE nama = '" + EscapeSql(teks) + "' " + "LIMIT 1");
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(DB.ds.Tables[0].Rows[0]["id_pelanggan"]);
                }
            }
            catch
            {
            }
            return 0;
        }
        private void LoadKendaraan(int idPelanggan)
        {
            try
            {
                DB.crud("SELECT " + "id_kendaraan, " + "CONCAT(plat_nomor, ' - ', tipe) AS tampilan " + "FROM kendaraan " + "WHERE id_pelanggan = " + idPelanggan + " ORDER BY id_kendaraan");
                if (DB.ds.Tables.Count == 0) return;
                if (DB.ds.Tables[0].Rows.Count == 0)
                {
                    cmbkendaraan.DataSource = null;
                    cmbkendaraan.Items.Clear();
                    return;
                }
                cmbkendaraan.DataSource = null;
                cmbkendaraan.DataSource = DB.ds.Tables[0];
                cmbkendaraan.DisplayMember = "tampilan";
                cmbkendaraan.ValueMember = "id_kendaraan";
                cmbkendaraan.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat kendaraan:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadJenisService()
        {
            try
            {
                DB.crud("SELECT " + "id_jenis, " + "nama_service, " + "harga, " + "stok " + "FROM jenis_service " + "ORDER BY nama_service");
                if (DB.ds.Tables.Count > 0)
                {
                    cmbservice.DataSource = null;
                    cmbservice.DataSource = DB.ds.Tables[0];
                    cmbservice.DisplayMember = "nama_service";
                    cmbservice.ValueMember = "id_jenis";
                    cmbservice.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat jenis service:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadJasa()
        {
            try
            {
                DB.crud("SELECT " + "id_jasa, " + "nama_jasa, " + "harga_jasa " + "FROM jasa " + "ORDER BY nama_jasa");
                if (DB.ds.Tables.Count > 0)
                {
                    cmbjasa.DataSource = null;
                    cmbjasa.DataSource = DB.ds.Tables[0];
                    cmbjasa.DisplayMember = "nama_jasa";
                    cmbjasa.ValueMember = "id_jasa";
                    cmbjasa.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat jasa:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmbservice_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbservice.SelectedIndex == -1 || cmbservice.SelectedValue == null || cmbservice.SelectedValue is DataRowView)
                {
                    txtbiaya.Text = "0";
                    HitungSubtotal();
                    return;
                }
                int idJenis;
                if (!int.TryParse(cmbservice.SelectedValue.ToString(), out idJenis))
                {
                    txtbiaya.Text = "0";
                    HitungSubtotal();
                    return;
                }
                DB.crud("SELECT harga " + "FROM jenis_service " + "WHERE id_jenis = " + idJenis);
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    decimal harga = Convert.ToDecimal(DB.ds.Tables[0].Rows[0]["harga"]);
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
        private void cmbjasa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbjasa.SelectedIndex == -1 || cmbjasa.SelectedValue == null || cmbjasa.SelectedValue is DataRowView)
                {
                    txtbiayajasa.Text = "0";
                    HitungSubtotal();
                    return;
                }
                int idJasa;
                if (!int.TryParse(cmbjasa.SelectedValue.ToString(), out idJasa))
                {
                    txtbiayajasa.Text = "0";
                    HitungSubtotal();
                    return;
                }
                DB.crud("SELECT harga_jasa " + "FROM jasa " + "WHERE id_jasa = " + idJasa);
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    decimal harga = Convert.ToDecimal(DB.ds.Tables[0].Rows[0]["harga_jasa"]);
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
        private void txtjumlah_TextChanged(object sender, EventArgs e)
        {
            HitungSubtotal();
        }
        private void HitungSubtotal()
        {
            try
            {
                decimal hargaService = AmbilAngka(txtbiaya.Text);
                decimal hargaJasa = AmbilAngka(txtbiayajasa.Text);
                int jumlah = 0;
                int.TryParse(txtjumlah.Text, out jumlah);
                decimal subtotal = (hargaService * jumlah) + hargaJasa;
                txtsubtotal.Text = subtotal.ToString("N0");
            }
            catch
            {
                txtsubtotal.Text = "0";
            }
        }
        private void HitungTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                string no = Convert.ToString(row.Cells["no_transaksi"].Value);
                if (no == "BARU")
                {
                    total += AmbilAngka(Convert.ToString(row.Cells["subtotal"].Value));
                }
            }
            txttotal.Text = total.ToString("N0");
        }
        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            if (idPelangganTerpilih <= 0)
            {
                MessageBox.Show("Pilih pelanggan terlebih dahulu.");
                return;
            }
            if (cmbkendaraan.SelectedIndex == -1)
            {
                MessageBox.Show("Pilih kendaraan terlebih dahulu.");
                return;
            }
            if (cmbservice.SelectedIndex == -1)
            {
                MessageBox.Show("Pilih jenis service terlebih dahulu.");
                return;
            }
            int jumlah;
            if (!int.TryParse(txtjumlah.Text, out jumlah) || jumlah <= 0)
            {
                MessageBox.Show("Jumlah harus lebih dari 0.");
                return;
            }
            int stok = AmbilStokService();
            if (jumlah > stok)
            {
                MessageBox.Show("Stok service tidak mencukupi.\n\n" + "Stok tersedia: " + stok);
                return;
            }
            string pelanggan = cmbid.Text;
            string kendaraan = cmbkendaraan.Text;
            string service = cmbservice.Text;
            string jasa = string.IsNullOrWhiteSpace(cmbjasa.Text) ? "-" : cmbjasa.Text;
            decimal hargaService = AmbilAngka(txtbiaya.Text);
            decimal hargaJasa = AmbilAngka(txtbiayajasa.Text);
            decimal subtotal = AmbilAngka(txtsubtotal.Text);
            int rowIndex = dataGridView1.Rows.Add();
            DataGridViewRow row = dataGridView1.Rows[rowIndex];
            row.Cells["no_transaksi"].Value = "BARU";
            row.Cells["tanggal"].Value = DateTime.Now.ToString("dd-MM-yyyy");
            row.Cells["pelanggan"].Value = pelanggan;
            row.Cells["kendaraan"].Value = kendaraan;
            row.Cells["jenis_service"].Value = service;
            row.Cells["jasa"].Value = jasa;
            row.Cells["jumlah"].Value = jumlah;
            row.Cells["harga_service"].Value = hargaService.ToString("N0");
            row.Cells["harga_jasa"].Value = hargaJasa.ToString("N0");
            row.Cells["subtotal"].Value = subtotal.ToString("N0");
            row.Tag = null;
            HitungTotal();
            BersihkanInput();
        }
        private int AmbilStokService()
        {
            try
            {
                if (cmbservice.SelectedIndex == -1 || cmbservice.SelectedValue == null || cmbservice.SelectedValue is DataRowView)
                {
                    return 0;
                }
                int idJenis;
                if (!int.TryParse(cmbservice.SelectedValue.ToString(), out idJenis))
                {
                    return 0;
                }
                DB.crud("SELECT stok " + "FROM jenis_service " + "WHERE id_jenis = " + idJenis);
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(DB.ds.Tables[0].Rows[0]["stok"]);
                }
            }
            catch
            {
            }
            return 0;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow firstNewRow = null;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    string no = Convert.ToString(row.Cells["no_transaksi"].Value);
                    if (no == "BARU")
                    {
                        firstNewRow = row;
                        break;
                    }
                }
                if (firstNewRow == null)
                {
                    MessageBox.Show("Belum ada service yang ditambahkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int idPelanggan = idPelangganTerpilih;
                if (idPelanggan <= 0)
                {
                    MessageBox.Show("Pelanggan belum dipilih.");
                    return;
                }
                string pelanggan = cmbid.Text;
                string kendaraan = Convert.ToString(firstNewRow.Cells["kendaraan"].Value);
                int idKendaraan = AmbilIdKendaraan(kendaraan, idPelanggan);
                if (idKendaraan <= 0)
                {
                    MessageBox.Show("Data kendaraan tidak ditemukan.");
                    return;
                }
                decimal total = AmbilAngka(txttotal.Text);
                string sqlHeader = "INSERT INTO transaksi " + "(tanggal, pelanggan, kendaraan, total) " + "VALUES (" + "CURDATE(), " + "'" + EscapeSql(pelanggan) + "', " + "'" + EscapeSql(kendaraan) + "', " + total.ToString(System.Globalization.CultureInfo.InvariantCulture
                    ) + ")";
                DB.crud(sqlHeader);
                DB.crud("SELECT LAST_INSERT_ID() AS no_transaksi");
                if (DB.ds.Tables.Count == 0 || DB.ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Nomor transaksi tidak berhasil dibuat.");
                    return;
                }
                int noTransaksi = Convert.ToInt32(DB.ds.Tables[0].Rows[0]["no_transaksi"]);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    string no = Convert.ToString(row.Cells["no_transaksi"].Value);
                    if (no != "BARU") continue;
                    string jenisService = Convert.ToString(row.Cells["jenis_service"].Value);
                    string jasa = Convert.ToString(row.Cells["jasa"].Value);
                    int jumlah = 0;
                    int.TryParse(Convert.ToString(row.Cells["jumlah"].Value), out jumlah);
                    decimal hargaService = AmbilAngka(Convert.ToString(row.Cells["harga_service"].Value));
                    decimal hargaJasa = AmbilAngka(Convert.ToString(row.Cells["harga_jasa"].Value));
                    decimal subtotal = AmbilAngka(Convert.ToString(row.Cells["subtotal"].Value));
                    int idJenis = AmbilIdJenisService(jenisService);
                    if (idJenis <= 0)
                    {
                        MessageBox.Show("Jenis service tidak ditemukan:\n" + jenisService);
                        return;
                    }
                    int idJasa = 0;
                    if (!string.IsNullOrWhiteSpace(jasa) && jasa != "-")
                    {
                        idJasa = AmbilIdJasa(jasa);
                    }
                    string sqlDetail = "INSERT INTO detail_transaksi " + "(no_transaksi, jenis_service, jasa, jumlah, " + "harga_service, harga_jasa, subtotal) " + "VALUES (" + noTransaksi + ", " + idJenis + ", " + (idJasa > 0
                            ? idJasa.ToString() : "NULL") + ", " + jumlah + ", " + hargaService.ToString(System.Globalization.CultureInfo.InvariantCulture
                        ) + ", " + hargaJasa.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", " + subtotal.ToString(System.Globalization.CultureInfo.InvariantCulture
                        ) + ")";
                    DB.crud(sqlDetail);
                    DB.crud("UPDATE jenis_service " + "SET stok = stok - " + jumlah + " WHERE id_jenis = " + idJenis);
                }
                string nomorCetak = "TRX-" + noTransaksi + "-" + DateTime.Now.ToString("ddMMyyyy");
                Fstruk struk = new Fstruk(noTransaksi, nomorCetak);
                struk.ShowDialog();
                BersihkanForm();
                TampilkanDataTransaksi();
                MessageBox.Show("Transaksi berhasil disimpan.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menyimpan transaksi:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int AmbilIdKendaraan(string text, int idPelanggan)
        {
            try
            {
                string plat = text;
                if (text.Contains(" - "))
                {
                    plat = text.Split(new string[]
                            {
                                " - "
                            }, StringSplitOptions.None)[0];
                }
                DB.crud("SELECT id_kendaraan " + "FROM kendaraan " + "WHERE plat_nomor = '" + EscapeSql(plat) + "' " + "AND id_pelanggan = " + idPelanggan + " LIMIT 1"
                );
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(DB.ds.Tables[0].Rows[0]["id_kendaraan"]);
                }
            }
            catch
            {
            }
            return 0;
        }
        private int AmbilIdJenisService(string nama)
        {
            try
            {
                DB.crud("SELECT id_jenis " + "FROM jenis_service " + "WHERE nama_service = '" + EscapeSql(nama) + "' LIMIT 1");
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(DB.ds.Tables[0].Rows[0]["id_jenis"]);
                }
            }
            catch
            {
            }
            return 0;
        }
        private int AmbilIdJasa(string nama)
        {
            try
            {
                DB.crud("SELECT id_jasa " + "FROM jasa " + "WHERE nama_jasa = '" + EscapeSql(nama) + "' LIMIT 1");
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(DB.ds.Tables[0].Rows[0]["id_jasa"]);
                }
            }
            catch
            {
            }
            return 0;
        }
        private void TampilkanDataTransaksi()
        {
            try
            {
                dataGridView1.Rows.Clear();
                DB.crud("SELECT " + "t.no_transaksi, " + "t.tanggal, " + "t.pelanggan, " + "t.kendaraan, " + "js.nama_service AS jenis_service, " + "IFNULL(j.nama_jasa, '-') AS jasa, " + "d.jumlah, " + "d.harga_service, " + "d.harga_jasa, " + "d.subtotal, " + "d.id_detail " + "FROM transaksi t " + "INNER JOIN detail_transaksi d " + "ON t.no_transaksi = d.no_transaksi " + "INNER JOIN jenis_service js " + "ON d.jenis_service = js.id_jenis " + "LEFT JOIN jasa j " + "ON d.jasa = j.id_jasa " + "ORDER BY " + "t.no_transaksi DESC, " + "d.id_detail ASC"
                );
                if (DB.ds.Tables.Count == 0) return;
                foreach (DataRow dr in DB.ds.Tables[0].Rows)
                {
                    int index = dataGridView1.Rows.Add();
                    DataGridViewRow row = dataGridView1.Rows[index];
                    row.Cells["no_transaksi"].Value = dr["no_transaksi"];
                    row.Cells["tanggal"].Value = Convert.ToDateTime(dr["tanggal"]).ToString("dd-MM-yyyy");
                    row.Cells["pelanggan"].Value = dr["pelanggan"];
                    row.Cells["kendaraan"].Value = dr["kendaraan"];
                    row.Cells["jenis_service"].Value = dr["jenis_service"];
                    row.Cells["jasa"].Value = dr["jasa"];
                    row.Cells["jumlah"].Value = dr["jumlah"];
                    row.Cells["harga_service"].Value = Convert.ToDecimal(dr["harga_service"]).ToString("N0");
                    row.Cells["harga_jasa"].Value = Convert.ToDecimal(dr["harga_jasa"]).ToString("N0");
                    row.Cells["subtotal"].Value = Convert.ToDecimal(dr["subtotal"]).ToString("N0");
                    row.Tag = Convert.ToInt32(dr["id_detail"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan transaksi:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            try
            {
                string pelanggan = Convert.ToString(row.Cells["pelanggan"].Value);
                string kendaraan = Convert.ToString(row.Cells["kendaraan"].Value);
                string service = Convert.ToString(row.Cells["jenis_service"].Value);
                string jasa = Convert.ToString(row.Cells["jasa"].Value);
                int id = CariIdPelanggan(pelanggan);
                idPelangganTerpilih = id;
                if (id > 0)
                {
                    SetPelangganTerpilih(id);
                    LoadKendaraan(id);
                    cmbkendaraan.Text = kendaraan;
                }
                else
                {
                    cmbid.Text = pelanggan;
                }
                cmbservice.Text = service;
                if (jasa == "-")
                {
                    cmbjasa.SelectedIndex = -1;
                }
                else
                {
                    cmbjasa.Text = jasa;
                }
                txtjumlah.Text = Convert.ToString(row.Cells["jumlah"].Value);
                txtbiaya.Text = Convert.ToString(row.Cells["harga_service"].Value);
                txtbiayajasa.Text = Convert.ToString(row.Cells["harga_jasa"].Value);
                txtsubtotal.Text = Convert.ToString(row.Cells["subtotal"].Value);
            }
            catch
            {
            }
        }
        private void SetPelangganTerpilih(int idPelanggan)
        {
            try
            {
                DB.crud("SELECT id_pelanggan, nama " + "FROM pelanggan " + "WHERE id_pelanggan = " + idPelanggan + " LIMIT 1");
                if (DB.ds.Tables.Count > 0 && DB.ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = DB.ds.Tables[0].Rows[0];
                    string tampilan = row["id_pelanggan"].ToString() + " - " + row["nama"].ToString();
                    cmbid.Text = tampilan;
                    idPelangganTerpilih = idPelanggan;
                }
            }
            catch
            {
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu.");
                return;
            }
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (row.IsNewRow) return;
            if (idPelangganTerpilih <= 0)
            {
                MessageBox.Show("Pilih pelanggan terlebih dahulu.");
                return;
            }
            row.Cells["pelanggan"].Value = cmbid.Text;
            row.Cells["kendaraan"].Value = cmbkendaraan.Text;
            row.Cells["jenis_service"].Value = cmbservice.Text;
            row.Cells["jasa"].Value = string.IsNullOrWhiteSpace(cmbjasa.Text) ? "-" : cmbjasa.Text;
            row.Cells["jumlah"].Value = txtjumlah.Text;
            row.Cells["harga_service"].Value = txtbiaya.Text;
            row.Cells["harga_jasa"].Value = txtbiayajasa.Text;
            row.Cells["subtotal"].Value = txtsubtotal.Text;
            if (row.Tag != null)
            {
                try
                {
                    int idDetail = Convert.ToInt32(row.Tag);
                    int idJenis = AmbilIdJenisService(cmbservice.Text);
                    int idJasa = 0;
                    if (!string.IsNullOrWhiteSpace(cmbjasa.Text) && cmbjasa.Text != "-")
                    {
                        idJasa = AmbilIdJasa(cmbjasa.Text);
                    }
                    int jumlah = 0;
                    int.TryParse(txtjumlah.Text, out jumlah);
                    decimal hargaService = AmbilAngka(txtbiaya.Text);
                    decimal hargaJasa = AmbilAngka(txtbiayajasa.Text);
                    decimal subtotal = AmbilAngka(txtsubtotal.Text);
                    DB.crud("UPDATE detail_transaksi SET " + "jenis_service = " + idJenis + ", jasa = " + (idJasa > 0 ? idJasa.ToString() : "NULL") + ", jumlah = " + jumlah + ", harga_service = " + hargaService.ToString(System.Globalization.CultureInfo.InvariantCulture
                        ) + ", harga_jasa = " + hargaJasa.ToString(System.Globalization.CultureInfo.InvariantCulture) + ", subtotal = " + subtotal.ToString(System.Globalization.CultureInfo.InvariantCulture
                        ) + " WHERE id_detail = " + idDetail);
                    MessageBox.Show("Data berhasil diubah.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengubah data:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            HitungTotal();
            BersihkanInput();
        }
        private void guna2Button3_Click_2(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Pilih data terlebih dahulu.");
                return;
            }
            DataGridViewRow row = dataGridView1.CurrentRow;
            if (row.IsNewRow) return;
            DialogResult hasil = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (hasil != DialogResult.Yes) return;
            try
            {
                if (row.Tag == null)
                {
                    dataGridView1.Rows.Remove(row);
                    HitungTotal();
                    BersihkanInput();
                    return;
                }
                int idDetail = Convert.ToInt32(row.Tag);
                int jumlah = Convert.ToInt32(row.Cells["jumlah"].Value);
                string jenisService = Convert.ToString(row.Cells["jenis_service"].Value);
                int idJenis = AmbilIdJenisService(jenisService);
                DB.crud("UPDATE jenis_service " + "SET stok = stok + " + jumlah + " WHERE id_jenis = " + idJenis);
                DB.crud("DELETE FROM detail_transaksi " + "WHERE id_detail = " + idDetail);
                string noTransaksi = Convert.ToString(row.Cells["no_transaksi"].Value);
                DB.crud("DELETE FROM transaksi " + "WHERE no_transaksi = " + noTransaksi + " AND NOT EXISTS (" + "SELECT 1 FROM detail_transaksi " + "WHERE detail_transaksi.no_transaksi = " + "transaksi.no_transaksi)"
                );
                MessageBox.Show("Data berhasil dihapus.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TampilkanDataTransaksi();
                BersihkanForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus data:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtcari_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtcari.Text.Trim().ToLower();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                bool ditemukan = false;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    string value = Convert.ToString(cell.Value).ToLower();
                    if (value.Contains(keyword))
                    {
                        ditemukan = true;
                        break;
                    }
                }
                row.Visible = ditemukan;
            }
        }
        private void BersihkanInput()
        {
            cmbservice.SelectedIndex = -1;
            cmbjasa.SelectedIndex = -1;
            txtjumlah.Text = "1";
            txtbiaya.Text = "0";
            txtbiayajasa.Text = "0";
            txtsubtotal.Text = "0";
        }
        private void BersihkanForm()
        {
            cmbid.SelectedIndex = -1;
            cmbid.Text = "";
            idPelangganTerpilih = 0;
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
        private decimal AmbilAngka(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;
            string angka = text.Replace(".", "").Replace(",", "").Replace("Rp", "").Trim();
            decimal hasil;
            if (decimal.TryParse(angka, out hasil))
            {
                return hasil;
            }
            return 0;
        }
        private string EscapeSql(string text)
        {
            if (text == null)
                return "";
            return text.Replace("'", "''");
        }
        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            TampilkanDataTransaksi();
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            TampilkanDataTransaksi();
        }
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }
        private void cmbid_DropDownStyleChanged(object sender, EventArgs e)
        {
        }
        private void cmbid_DropDown(object sender, EventArgs e)
        {
        }
    }
}
