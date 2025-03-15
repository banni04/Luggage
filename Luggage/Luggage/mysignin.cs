using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luggage
{
    public partial class mysignin : Form
    {
        public mysignin()
        {
            InitializeComponent();
        }

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }


        private void button2_Click(object sender, EventArgs e)
        {

            //string username1 = username.Text.Trim();
            //string password1 = password.Text.Trim();

            if (username.Text == "ADMIN" && password.Text == "12345678")
            {
                MessageBox.Show("Admin log in successfully");
                homeadmin homeadmin = new homeadmin();
                homeadmin.Show();
                this.Hide();
                return;
            }
            else
            {
                // หาก username หรือ password ไม่ถูกต้อง
                MessageBox.Show("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง");
            }



        }
    }
}
