using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Luggage
{
    public partial class request : Form
    {

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }


        private string username;

        public request(string username)
        {
            InitializeComponent();
            this.username = username;
        }

       

        public request()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*if (!int.TryParse(cdaywant.Text, out int days))
            {
                MessageBox.Show("Invalid number of days entered.");
                return;
            }*/

            if (!int.TryParse(cpassword.Text, out int amount))
            {
                MessageBox.Show("Invalid number of luggage entered.");
                return;
            }


            //decimal cpay = days * 25.00m + amount * 10.00m;

            DateTime day_in = DateTime.Now;
            //DateTime day_out = day_in.AddDays(days);

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "UPDATE luggage_information " +
                               "SET name = @name, tel = @tel, luggage_type = @type, luggage_color = @color, amount_luggage = @amount, amountday_want_care = @dayW, " +
                               "amount_pay = @pay, day_in = @di, day_out = @do " +
                               "WHERE id = @id"; // ปรับแต่งให้เหมาะสมกับ schema ของตารางของคุณ

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    //cmd.Parameters.AddWithValue("@id", idTextBox.Text); // แทน idTextBox.Text ด้วย primary key ของข้อมูลที่ต้องการแก้ไข
                    cmd.Parameters.AddWithValue("@name", cname.Text);
                    cmd.Parameters.AddWithValue("@tel", ctel.Text);
                    cmd.Parameters.AddWithValue("@type", cusername.Text);
                    //cmd.Parameters.AddWithValue("@color", ccolor.Text);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    //cmd.Parameters.AddWithValue("@dayW", days);
                    //cmd.Parameters.AddWithValue("@pay", cpay);
                    cmd.Parameters.AddWithValue("@di", day_in);
                    //cmd.Parameters.AddWithValue("@do", day_out);

                    try
                    {
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data updated successfully");
                            homepage homepage = new homepage();
                            homepage.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("No rows updated");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void request_Load_1(object sender, EventArgs e)
        {
            // แสดงชื่อผู้ใช้ใน TextBox เมื่อฟอร์มโหลด
            textBoxUsername.Text = username;
        }
    }
}
