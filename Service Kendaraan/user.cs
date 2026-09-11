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
            int baris = e.RowIndex;
            int kolom = e.ColumnIndex;
            if (kolom == 5)
            {
                string idu = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DB.crud($"select * from user where id_user = '{idu}'");
                foreach (DataRow brs in DB.ds.Tables[0].Rows)
                {
                    string idus = "" + brs["id_user"];
                    string nu = "" + brs["nama"];
                    string un = "" + brs["username"];
                    string ps = "" + brs["password"];
                    string ir = "" + brs["id_role"];
                    label7.Text = idus;
                    txtnama.Text = nu;
                    txtuser.Text = un;
                    txtpass.Text = ps;
                    cmbrole.Text = ir;
                }
            }
            if (kolom == 6)
            {
                string idu = dataGridView1.Rows[baris].Cells[0].Value.ToString();
                DialogResult setuju = MessageBox.Show("Apakah mau dihapus? " + idu, "pemberitahuan,",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);
                if (setuju == DialogResult.Yes)
                {
                    DB.crud($"delete from user where id_user = '{idu}'");

                }
                tampildata();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string nu = "" + txtnama.Text;
            string un = "" + txtuser.Text;
            string ps = "" + txtpass.Text;
            string ir = "" + cmbrole.Text;
            DB.crud($"update user SET nama = '{nu}', username = '{un}', password = '{ps}', id_role = '{ir}' where id_user = '{label7.Text}'");
            bersih();
            tampildata();
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
    }
}
