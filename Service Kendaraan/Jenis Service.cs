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
            DB.crud($"insert into jenis_service values (null, '{txtnama.Text}','{txtharga.Text}')");
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
            int baris = e.RowIndex;
            int kolom = e.ColumnIndex;
            if (kolom == 3)
            {
                string idr = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DB.crud($"select * from jenis_service where id_jenis = '{idr}'");
                foreach (DataRow brs in DB.ds.Tables[0].Rows)
                {
                    string idro = "" + brs["id_jenis"];
                    string nr = "" + brs["nama_service"];
                    string hr = "" + brs["harga"];
                    label5.Text = idro;
                    txtnama.Text = nr;
                    txtharga.Text = hr;
                }
            }
            if (kolom == 4)
            {
                string idr = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DialogResult setuju = MessageBox.Show("Apakah mau dihapus? " + idr, "pemberitahuan,",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
                if (setuju == DialogResult.Yes)
                {
                    DB.crud($"delete from jenis_service where id_jenis = '{idr}'");

                }
                tampildata();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string nu = "" + txtnama.Text;
            string un = "" + txtharga.Text;
            DB.crud($"update jenis_service SET nama_service = '{nu}', harga = '{un}' where id_jenis = '{label5.Text}'");
            bersih();
            tampildata();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }
    }
}
