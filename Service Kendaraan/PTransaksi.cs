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
                string by = "" + baris["biaya"];
                dataGridView1.Rows.Add(idp, nm, np, al, js, by);
            }
        }
        public void bersih()
        {
            cmbid.SelectedIndex = -1;
            cmbkendaraan.SelectedIndex = -1;
            cmbservice.SelectedIndex = -1;
            txtbiaya.Text = "";
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
            DB.crud($"insert into transaksi values (null,null, '{cmbid.Text}','{cmbkendaraan.Text}','{cmbservice.Text}','{txtbiaya.Text}')");
            tampildata();
            bersih();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = e.RowIndex;
            int kolom = e.ColumnIndex;
            if (kolom == 6)
            {
                string idt = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DB.crud($"select * from transaksi where no_transaksi = '{idt}'");
                foreach (DataRow brs in DB.ds.Tables[0].Rows)
                {
                    string idta = "" + brs["no_transaksi"];
                    string nm = "" + brs["pelanggan"];
                    string np = "" + brs["kendaraan"];
                    string al = "" + brs["jenis_service"];
                    string by = "" + brs["biaya"];
                    label7.Text = idta;
                    cmbid.Text = nm;
                    cmbkendaraan.Text = np;
                    cmbservice.Text = al;
                    txtbiaya.Text = by;
                }
            }
            if (kolom == 7)
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
            string by = "" + txtbiaya.Text;
            DB.crud($"update transaksi SET pelanggan = '{nu}', kendaraan = '{un}', jenis_service = '{ps}',  biaya = '{by}' where no_transaksi = '{label7.Text}'");
            bersih();
            tampildata();
        }
    }
}
