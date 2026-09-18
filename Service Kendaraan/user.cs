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
    public partial class user : Form
    {
        string iduser = "";
        public user()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        public void tampildata()
        {
            dataGridView1.Rows.Clear();
            DB.crud("select * from user");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idu = "" + baris["id_user"];
                string nm = "" + baris["nama"];
                string un = "" + baris["username"];
                string ps = "" + baris["password"];
                string rl = "" + baris["role"];
                dataGridView1.Rows.Add(idu, nm, un, ps, rl);
            }
        }
        public void bersih()
        {
            cmbrole.SelectedIndex = -1;
            txtnama.Text = "";
            txtpass.Text = "";
            txtuser.Text = "";
        }
        public void role()
        {
            DB.crud($"select * from role where nama_role = '{cmbrole.Text}'");
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB.crud($"insert into user values (null, '{txtnama.Text}','{txtuser.Text}','{txtpass.Text}','{cmbrole.Text}')");


            tampildata();
            bersih();
        }

        private void cmbrole_SelectedIndexChanged(object sender, EventArgs e)
        {
            role();
        }

        private void user_Load(object sender, EventArgs e)
        {
            tampildata();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int baris = e.RowIndex;
            iduser = dataGridView1.Rows[baris].Cells[0].Value?.ToString();

            if (string.IsNullOrEmpty(iduser))
                return;
            DB.crud($"SELECT * FROM user WHERE id_user = '{iduser}'");

            if (DB.ds.Tables[0].Rows.Count > 0)
            {
                DataRow brs = DB.ds.Tables[0].Rows[0];

                label7.Text = brs["id_user"].ToString();
                txtnama.Text = brs["nama"].ToString();
                txtuser.Text = brs["username"].ToString();
                txtpass.Text = brs["password"].ToString();
                cmbrole.Text = brs["role"].ToString();

            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(iduser))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DB.crud($@"
            UPDATE user SET
            nama = '{txtnama.Text}',
            username = '{txtuser.Text}',
            password = '{txtpass.Text}',
            role = '{cmbrole.Text}'
            WHERE id_user = '{iduser}'
            ");

            MessageBox.Show("Data berhasil diupdate.");

            tampildata();
            iduser = "";
        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            DB.crud($"select * from user where nama like '%{guna2TextBox4.Text}%'");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idu = "" + baris["id_user"];
                string nm = "" + baris["nama"];
                string un = "" + baris["username"];
                string ps = "" + baris["password"];
                string rl = "" + baris["id_role"];
                dataGridView1.Rows.Add(idu, nm, un, ps, rl);
            }
        }

        private void cmbrole_DropDown(object sender, EventArgs e)
        {
            cmbrole.Items.Clear();
            DB.crud("Select * from role");
            foreach (DataRow baris in DB.ds.Tables[0].Rows)
            {
                string idb = "" + baris["nama_role"];
                cmbrole.Items.Add(idb);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(iduser))
            {
                MessageBox.Show("Silakan pilih data terlebih dahulu.");
                return;
            }

            DialogResult setuju = MessageBox.Show(
                "Apakah mau menghapus transaksi " + iduser + "?",
                "Pemberitahuan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (setuju == DialogResult.Yes)
            {
                DB.crud($"DELETE FROM user WHERE id_user = '{iduser}'");
                MessageBox.Show("Data berhasil dihapus.");
                tampildata();

                iduser = "";
                label7.Text = "";
                txtnama.Text = "";
                txtuser.Text = "";
                txtpass.Text = "";
                cmbrole.Text = "";

            }
        }
    }
}
