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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DB.crud($"SELECT * FROM user WHERE username = '{txtuser.Text}' AND password = '{txtpass.Text}'");

            int baris = DB.ds.Tables[0].Rows.Count;

            if (baris == 1)
            {
                DataRow row = DB.ds.Tables[0].Rows[0];
                string roleUser = row["role"].ToString();

                if (roleUser == "Admin")
                {
                    DAdmin Fa = new DAdmin();
                    Fa.Show();
                }
                else
                {
                    DataRow brs = DB.ds.Tables[0].Rows[0];
                    string id = "" + brs["id_user"];

                    DPetugas F1 = new DPetugas();
                    F1.IDLogin = id;
                    F1.Show();
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("salah");
            }
        }
    }
}
