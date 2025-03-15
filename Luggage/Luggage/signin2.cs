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
    public partial class signin2 : Form
    {
        public signin2()
        {
            InitializeComponent();
        }


        private MySqlConnection databaseConnection() //เชื่อมต่อฐานข้อมูล
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string username1 = username.Text.Trim();
            string password1 = password.Text.Trim();

            if (username.Text == "ADMIN" && password.Text == "123")
            {
                MessageBox.Show("Admin log in successfully");
                homeadmin homeadmin = new homeadmin();
                homeadmin.Show();
                this.Hide();
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();

                string query = "SELECT * FROM customer_info WHERE username = @username AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username1); // ใช้ username1 ที่เก็บค่าจาก TextBox
                cmd.Parameters.AddWithValue("@password", password1); // ใช้ password1 ที่เก็บค่าจาก TextBox

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    MessageBox.Show("log in successfully"); //เหมือนกับsign in แต่
                    // ไปฟอร์ม inputinfo2 และส่งค่า username1
                    inputinfo2 inputinfo2Form = new inputinfo2();
                    inputinfo2Form.username_ii2 = username1;
                    inputinfo2Form.Show();
                    this.Hide(); // ซ่อนฟอร์มล็อกอิน
                }
                else
                {
                    MessageBox.Show("Incorrect username or password."); //ถ้าไม่มีข้อมูลก็ขึ้นแจ้งเตือน
                }

                reader.Close();
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            signup signup = new signup();
            signup.Show();
            this.Hide();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            service service = new service();
            service.Show();
            this.Hide();
        }

        
    }
}
