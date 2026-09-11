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
    public partial class PTransaksi : Form
    {
        public PTransaksi()
        {
            InitializeComponent();
        }
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from transaksi");
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
                dataGridView1.Rows.Add(idp, nm, np, al, js, jm, by, tl);
            }
        }
        public void bersih()
        {
            cmbid.SelectedIndex = -1;
            cmbkendaraan.SelectedIndex = -1;
            cmbservice.SelectedIndex = -1;
            txtjumlah.Text = "";
            txtbiaya.Text = "";
            txttotal.Text = "";
        }
        private void guna2ComboBox1_DropDown(object sender, EventArgs e)
        {
           
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
            DB.crud("Select * from pelanggan");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idb = "" + baris["id_pelanggan"];
                string nm = "" + baris["nama"];
                cmbid.Items.Add(idb + " - " + nm);
            }
        }

        private void guna2ComboBox2_DropDown(object sender, EventArgs e)
        {
            cmbservice.Items.Clear();
            DB.crud("Select * from jenis_service");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idb = "" + baris["nama_service"];
                cmbservice.Items.Add(idb);
            }
        }

        private void guna2ComboBox1_DropDown_1(object sender, EventArgs e)
        {
            cmbkendaraan.Items.Clear();
            DB.crud("Select * from kendaraan");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idb = "" + baris["plat_nomor"];
                string nm = "" + baris["tipe"];
                cmbkendaraan.Items.Add(idb + " - " + nm);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            DB.crud($"INSERT INTO transaksi VALUES (null,null," +
                $"'{cmbid.Text}','{cmbkendaraan.Text}','{cmbservice.Text}'," +
                $"{txtjumlah.Text},'{txtbiaya.Text}','{txttotal.Text}')");

           
            DB.crud("SELECT * FROM jenis_service");
            foreach (DataRow item in DB.ds.Tables[0].Rows)
            {
                string ID = item["id_jenis"].ToString();
                label7.Text = ID;

                DB.crud($"UPDATE jenis_service SET stok = stok - {txtjumlah.Text} WHERE id_jenis = '{ID}'");
            }

            tampildata();
            bersih();
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = e.RowIndex;
            int kolom = e.ColumnIndex;
            if (kolom == 8)
            {
                string idt = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DB.crud($"select * from transaksi where no_transaksi = '{idt}'");
                foreach (DataRow brs in DB.ds.Tables[0].Rows)
                {
                    string idta = "" + brs["no_transaksi"];
                    string nm = "" + brs["pelanggan"];
                    string np = "" + brs["kendaraan"];
                    string al = "" + brs["jenis_service"];
                    string js = "" + brs["jumlah_service"];
                    string by = "" + brs["harga"];
                    string tl = "" + brs["total"];
                    label7.Text = idta;
                    cmbid.Text = nm;
                    cmbkendaraan.Text = np;
                    cmbservice.Text = al;
                    txtjumlah.Text = js;
                    txttotal.Text = tl;
                    txtbiaya.Text = by;
                }
            }
            if (kolom == 9)
            {
                string idt = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DialogResult setuju = MessageBox.Show("Apakah mau dihapus? " + idt, "pemberitahuan,",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
                if (setuju == DialogResult.Yes)
                {
                    DB.crud($"delete from transaksi where no_transaksi = '{idt}'");

                }
                tampildata();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string nu = "" + cmbid.Text;
            string un = "" + cmbkendaraan.Text;
            string ps = "" + cmbservice.Text;
            string js = "" + txtjumlah.Text;
            string by = "" + txtbiaya.Text;
            string tt = "" + txttotal.Text;
            DB.crud($"update transaksi SET pelanggan = '{nu}', kendaraan = '{un}', jenis_service = '{ps}', jumlah_service = '{js}',  harga = '{by}', total = '{tt}'  where no_transaksi = '{label7.Text}'");
            bersih();
            tampildata();
        }

        private void txtbiaya_TextChanged(object sender, EventArgs e)
        {

        }
        private void harga()
        {
            DB.crud($"SELECT * FROM jenis_service WHERE nama_service = '{cmbservice.Text}'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string harga = baris["harga"].ToString();
                txtbiaya.Text = harga;
            }

        }
        private void cmbservice_SelectedIndexChanged(object sender, EventArgs e)
        {
            harga();
        }

        private void txtjumlah_TextChanged(object sender, EventArgs e)
        {
            if (txtjumlah.Text != "" && txtbiaya.Text != "")
            {
                int hr = Convert.ToInt32(txtbiaya.Text);
                int qty = Convert.ToInt32(txtjumlah.Text);
                int total = hr * qty;
                txttotal.Text = Convert.ToString(total);  // total harga tiket
            }
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from transaksi where pelanggan like '%{guna2TextBox4.Text}%'");
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
                dataGridView1.Rows.Add(idp, nm, np, al, js, jm, by, tl);
            }
        }
    }
}
