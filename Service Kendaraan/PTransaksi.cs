using System;
using System.Data;
using System.Windows.Forms;

namespace Service_Kendaraan
{
    public partial class PTransaksi : Form
    {
        string idTransaksi = "";

        public PTransaksi()
        {
            InitializeComponent();
        }

        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("SELECT * FROM transaksi");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["no_transaksi"];
                string nm = "" + baris["tanggal"];
                string np = "" + baris["pelanggan"];
                string al = "" + baris["kendaraan"];
                string js = "" + baris["jenis_service"];
                string jm = "" + baris["jumlah_service"];
                string by = "" + baris["harga"];
                string tl = "" + baris["total"];
                dataGridView1.Rows.Add(idp,nm,np,al,js,jm,by,tl);
            }
        }

        
        public void bersih()
        {
            cmbid.SelectedIndex = -1;
            cmbkendaraan.Items.Clear();
            cmbkendaraan.SelectedIndex = -1;
            cmbservice.SelectedIndex = -1;
            txtjumlah.Text = "";
            txtbiaya.Text = "";
            txttotal.Text = "";
            label7.Text = "";
        }

       
        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            tampildata();
        }

       
        private void PTransaksi_Load(object sender, EventArgs e)
        {
            tampildata();
        }

        
        private void guna2ComboBox3_DropDown(object sender, EventArgs e)
        {
            cmbid.Items.Clear();
            DB.crud("SELECT * FROM pelanggan");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idb = "" + baris["id_pelanggan"];
                string nm = "" + baris["nama"];
                cmbid.Items.Add(idb + " - " + nm);
            }
        }

        
        private void cmbid_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbkendaraan.Items.Clear();
            cmbkendaraan.SelectedIndex = -1;
        }

        
        private void guna2ComboBox1_DropDown_1(object sender, EventArgs e)
        {
            cmbkendaraan.Items.Clear();
            if (cmbid.Text == "")
            {
                MessageBox.Show("Silakan pilih pelanggan terlebih dahulu.");
                return;
            }
            string idpelanggan = cmbid.Text.Split('-')[0].Trim();
            DB.crud($@"SELECT * FROM kendaraan WHERE id_pelanggan = '{idpelanggan}'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string plat = "" + baris["plat_nomor"];
                string tipe = "" + baris["tipe"];
                cmbkendaraan.Items.Add(plat + " - " + tipe);
            }
            if (cmbkendaraan.Items.Count == 0)
            {
                MessageBox.Show("Pelanggan ini belum memiliki kendaraan.");
            }
        }

       
        private void guna2ComboBox2_DropDown(object sender, EventArgs e)
        {
            cmbservice.Items.Clear();
            DB.crud("SELECT * FROM jenis_service");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string namaService = "" + baris["nama_service"];
                cmbservice.Items.Add(namaService);
            }
        }

       
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (cmbid.Text == "")
            {
                MessageBox.Show("Silakan pilih pelanggan.");
                return;
            }

            if (cmbkendaraan.Text == "")
            {
                MessageBox.Show("Silakan pilih kendaraan.");
                return;
            }

            if (cmbservice.Text == "")
            {
                MessageBox.Show("Silakan pilih jenis service.");
                return;
            }

            if (!int.TryParse(txtjumlah.Text,out int jumlah))
            {
                MessageBox.Show("Jumlah service harus berupa angka.");
                return;
            }

            if (jumlah <= 0)
            {
                MessageBox.Show("Jumlah service harus lebih dari 0.");
                return;
            }

            DB.crud($@"SELECT stok FROM jenis_service WHERE nama_service = '{cmbservice.Text}'");
            if (DB.ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Jenis service tidak ditemukan.");
                return;
            }

            int stok = Convert.ToInt32(DB.ds.Tables[0].Rows[0]["stok"]);

            if (stok < jumlah)
            {
                MessageBox.Show("Stok service tidak mencukupi.\n" + "Stok tersedia: " + stok);
                return;
            }

            DB.crud($@"INSERT INTO transaksi VALUES (null,null,'{cmbid.Text}','{cmbkendaraan.Text}','{cmbservice.Text}','{jumlah}','{txtbiaya.Text}','{txttotal.Text}')");

            DB.crud($@"UPDATE jenis_service
                       SET stok = stok - {jumlah}
                       WHERE nama_service = '{cmbservice.Text}'");

            MessageBox.Show("Transaksi berhasil disimpan.");
            
            DB.crud(@"SELECT no_transaksi FROM transaksi ORDER BY no_transaksi DESC LIMIT 1");
            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                string noTransaksi = DB.ds.Tables[0].Rows[0]["no_transaksi"].ToString();

                Fstruk struk = new Fstruk(
                    noTransaksi,
                    cmbid.Text,
                    cmbservice.Text,
                    txtjumlah.Text,
                    txtbiaya.Text,
                    txttotal.Text
                );

                struk.ShowDialog();
            }

            tampildata();
            bersih();

        }

       
        private void dataGridView1_CellClick(object sender,DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int baris = e.RowIndex;

            idTransaksi = dataGridView1.Rows[baris].Cells[0].Value?.ToString();

            if (string.IsNullOrEmpty(idTransaksi))
                return;

            DB.crud(
                $"SELECT * FROM transaksi " + $"WHERE no_transaksi = '{idTransaksi}'"
            );

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                DataRow brs = DB.ds.Tables[0].Rows[0];

                label7.Text = brs["no_transaksi"].ToString();
                cmbid.Text = brs["pelanggan"].ToString();
                cmbkendaraan.Text = brs["kendaraan"].ToString();
                cmbservice.Text = brs["jenis_service"].ToString();
                txtjumlah.Text = brs["jumlah_service"].ToString();
                txtbiaya.Text = brs["harga"].ToString();
                txttotal.Text = brs["total"].ToString();
            }
        }

       
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idTransaksi))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            if (cmbid.Text == "" || cmbkendaraan.Text == "" || cmbservice.Text == "")
            {
                MessageBox.Show("Data transaksi belum lengkap.");
                return;
            }

            if (!int.TryParse(txtjumlah.Text,out int jumlahBaru))
            {
                MessageBox.Show("Jumlah service harus berupa angka.");
                return;
            }

            if (jumlahBaru <= 0)
            {
                MessageBox.Show("Jumlah service harus lebih dari 0.");
                return;
            }

            DB.crud($@"SELECT jenis_service, jumlah_service FROM transaksi WHERE no_transaksi = '{idTransaksi}'");

            if (DB.ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Data transaksi tidak ditemukan.");
                return;
            }

            string serviceLama = DB.ds.Tables[0].Rows[0]["jenis_service"].ToString();

            int jumlahLama = Convert.ToInt32(DB.ds.Tables[0].Rows[0]["jumlah_service"]);

            DB.crud($@"UPDATE jenis_service SET stok = stok + {jumlahLama} WHERE nama_service = '{serviceLama}'");

            DB.crud($@"SELECT stok FROM jenis_service WHERE nama_service = '{cmbservice.Text}'");

            if (DB.ds.Tables[0].Rows.Count == 0)
            {
                
                DB.crud($@"UPDATE jenis_service SET stok = stok - {jumlahLama} WHERE nama_service = '{serviceLama}'");

                MessageBox.Show("Jenis service tidak ditemukan.");
                return;
            }

            int stokSekarang = Convert.ToInt32(DB.ds.Tables[0].Rows[0]["stok"] );

            if (stokSekarang < jumlahBaru)
            {
               
                DB.crud($@"UPDATE jenis_service
                           SET stok = stok - {jumlahLama}
                           WHERE nama_service = '{serviceLama}'");

                MessageBox.Show(
                    "Stok service tidak mencukupi.\n" +
                    "Stok tersedia: " + stokSekarang
                );

                return;
            }

            
            DB.crud($@"UPDATE jenis_service
                       SET stok = stok - {jumlahBaru}
                       WHERE nama_service = '{cmbservice.Text}'");

            
            DB.crud($@"UPDATE transaksi SET
                       pelanggan = '{cmbid.Text}',
                       kendaraan = '{cmbkendaraan.Text}',
                       jenis_service = '{cmbservice.Text}',
                       jumlah_service = '{jumlahBaru}',
                       harga = '{txtbiaya.Text}',
                       total = '{txttotal.Text}'
                       WHERE no_transaksi = '{idTransaksi}'");

            MessageBox.Show(
                "Data berhasil diupdate."
            );

            tampildata();
            bersih();

            idTransaksi = "";
        }

        
        private void harga()
        {
            if (cmbservice.Text == "")
                return;

            DB.crud(
                $"SELECT * FROM jenis_service " +
                $"WHERE nama_service = '{cmbservice.Text}'"
            );

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                string harga =
                    DB.ds.Tables[0].Rows[0]
                    ["harga"].ToString();

                txtbiaya.Text = harga;
            }
        }

        
        private void cmbservice_SelectedIndexChanged(object sender, EventArgs e)
        {
            harga();
        }

        
        private void txtjumlah_TextChanged(object sender,EventArgs e)
        {
            if (int.TryParse(txtjumlah.Text,out int qty) && int.TryParse(txtbiaya.Text,out int harga))
            {
                int total = harga * qty;

                txttotal.Text = total.ToString();
            }
            else
            {
                txttotal.Text = "";
            }
        }

        
        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($@"SELECT * FROM transaksi WHERE pelanggan LIKE '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idp = "" + baris["no_transaksi"];
                string nm = "" + baris["tanggal"];
                string np = "" + baris["pelanggan"];
                string al = "" + baris["kendaraan"];
                string js = "" + baris["jenis_service"];
                string jm = "" + baris["jumlah_service"];
                string by = "" + baris["harga"];
                string tl = "" + baris["total"];
                dataGridView1.Rows.Add(idp,nm,np,al,js,jm,by,tl);
            }
        }

        
        private void guna2Button3_Click_2(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrEmpty(idTransaksi))
            {
                MessageBox.Show(
                    "Silakan pilih data terlebih dahulu."
                );

                return;
            }

            DialogResult setuju =
                MessageBox.Show(
                    "Apakah mau menghapus transaksi " +
                    idTransaksi + "?",
                    "Pemberitahuan",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (setuju == DialogResult.Yes)
            {
                
                DB.crud($@"SELECT jenis_service, jumlah_service
                           FROM transaksi
                           WHERE no_transaksi = '{idTransaksi}'");

                if (DB.ds.Tables[0].Rows.Count > 0)
                {
                    string namaService =
                        DB.ds.Tables[0].Rows[0]
                        ["jenis_service"].ToString();

                    int jumlah =
                        Convert.ToInt32(
                            DB.ds.Tables[0].Rows[0]
                            ["jumlah_service"]
                        );

                    
                    DB.crud($@"UPDATE jenis_service
                               SET stok = stok + {jumlah}
                               WHERE nama_service = '{namaService}'");
                }

               
                DB.crud(
                    $"DELETE FROM transaksi " +
                    $"WHERE no_transaksi = '{idTransaksi}'"
                );

                MessageBox.Show(
                    "Data berhasil dihapus."
                );

                tampildata();
                bersih();

                idTransaksi = "";
            }
        }

        
        private void guna2ComboBox1_DropDown(object sender, EventArgs e)
        {
        }

        private void txtbiaya_TextChanged(object sender,EventArgs e)
        {
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}