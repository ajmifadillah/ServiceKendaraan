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
    public partial class role : Form
    {
        public role()
        {
            InitializeComponent();
        }
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from role");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idr = "" + baris["id_role"];
                string nr = "" + baris["nama_role"];
                dataGridView1.Rows.Add(idr, nr);
            }
        }
        public void bersih()
        {
            txtrole.Text = "";
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB.crud($"insert into role values (null, '{txtrole.Text}')");


            tampildata();
            bersih();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int baris = e.RowIndex;
            int kolom = e.ColumnIndex;
            if (kolom == 2)
            {
                string idr = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DB.crud($"select * from role where id_role = '{idr}'");
                foreach (DataRow brs in DB.ds.Tables[0].Rows)
                {
                    string idro = "" + brs["id_role"];
                    string nr = "" + brs["nama_role"];
                    label7.Text = idro;
                    txtrole.Text = nr;
                }
            }
            if (kolom == 3)
            {
                string idr = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DialogResult setuju = MessageBox.Show("Apakah mau dihapus? " + idr, "pemberitahuan,",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
                if (setuju == DialogResult.Yes)
                {
                    DB.crud($"delete from role where id_role = '{idr}'");

                }
                tampildata();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string nr = "" + txtrole.Text;
            DB.crud($"update role SET nama_role = '{nr}' where id_role = '{label7.Text}'");
            bersih();
            tampildata();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            tampildata();
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from role where nama_role like '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idr = "" + baris["id_role"];
                string nr = "" + baris["nama_role"];
                dataGridView1.Rows.Add(idr, nr);
            }
        }
    }
}
