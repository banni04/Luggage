using MySql.Data.MySqlClient;
using PdfSharp.Pdf.Content.Objects;
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
    public partial class signup : Form
    {

        private MySqlConnection databaseConnection() //เชื่อมต่อฐานข้อมูล
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        public signup()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //ฟังก์ชันปุ่มconfirm รับข้อมูลจาก...
        {
            string name = username.Text;
            string password1 = password.Text;
            string telephone = tel.Text;

            // ตรวจสอบว่ามีข้อมูลหรือไม่
            if (string.IsNullOrEmpty(username.Text) ||
                string.IsNullOrEmpty(password.Text) ||
                string.IsNullOrEmpty(tel.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // ตรวจสอบ username ให้เป็นตัวอักษรภาษาไทย/อังกฤษ และตัวเลข ห้ามมีอักขระพิเศษ
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z0-9ก-๙]+$"))
            {
                MessageBox.Show("Username must only contain letters (Thai or English) and numbers, no special characters allowed.");
                return;
            }

            // ตรวจสอบว่า password ต้องมีอย่างน้อย 8 อักขระ
            if (password1.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.");
                return;
            }

            // ตรวจสอบว่า tel ต้องเป็นตัวเลขที่มี 10 ตัว
            if (!System.Text.RegularExpressions.Regex.IsMatch(telephone, @"^\d{10}$"))
            {
                MessageBox.Show("Telephone number must be a 10-digit number.");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();

                    // ตรวจสอบว่ามี username หรือ tel ซ้ำในฐานข้อมูล
                    string checkQuery = "SELECT COUNT(*) FROM customer_info WHERE username = @u OR tel = @t";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@u", name);
                        checkCmd.Parameters.AddWithValue("@t", telephone);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Username or Telephone number already exists.");
                            return;
                        }
                    }

                    // ถ้าไม่มีข้อมูลซ้ำ ให้เพิ่มข้อมูลใหม่
                    string query = "INSERT INTO customer_info (username, password, tel) VALUES (@u, @p, @t)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", name);
                        cmd.Parameters.AddWithValue("@p", password1);
                        cmd.Parameters.AddWithValue("@t", telephone);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data inserted successfully");

                        // ไปหน้า service
                        service service = new service();
                        service.Show();
                        this.Hide();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            service service = new service();
            service.Show();
            this.Hide();
        }
    }
}
