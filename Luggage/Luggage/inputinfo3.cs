using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Luggage
{
    public partial class inputinfo3 : Form
    {

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }


        //private string username;

        //public inputinfo3(string username)
        //{
        //    InitializeComponent();
        //    this.username = username;
        //}

        private string _username;
        public string username_ii3
        {
            get { return _username; }
            set { _username = value; }
            //username1
        }

        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
            //username1
        }

        private int _processid;
        public int processid
        {
            get { return _processid; }
            set { _processid = value; }
            //username1
        }



       

        private int regularCount = 0;
        private int specialCount = 0;
        private int daysDifference = 0;

        public inputinfo3()
        {
            InitializeComponent();
        }

       

        private void button2_Click(object sender, EventArgs e) //ปุ่ม save
        {
            // ตรวจสอบว่ามีข้อมูลใน cname, ctel, cemail, ciden, caddress หรือไม่
            if (string.IsNullOrEmpty(cname.Text) ||
                string.IsNullOrEmpty(ctel.Text) ||
                string.IsNullOrEmpty(cemail.Text) ||
                string.IsNullOrEmpty(ciden.Text) ||
                string.IsNullOrEmpty(caddress.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // ตรวจสอบว่า cname ต้องเป็นตัวอักษรภาษาอังกฤษหรือภาษาไทยและช่องว่างเท่านั้น
            if (!System.Text.RegularExpressions.Regex.IsMatch(cname.Text, @"^[a-zA-Zก-๙\s]+$"))
            {
                MessageBox.Show("Name must contain only English or Thai letters and spaces, no special characters.");
                return;
            }

            // ตรวจสอบว่า ctel ต้องเป็นตัวเลขที่มี 10 ตัว
            if (!System.Text.RegularExpressions.Regex.IsMatch(ctel.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Telephone number must be a 10-digit number.");
                return;
            }

            // ตรวจสอบว่า cemail ต้องเป็นอีเมลที่ถูกต้อง
            if (!System.Text.RegularExpressions.Regex.IsMatch(cemail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Email must be in a valid format.");
                return;
            }

            // ตรวจสอบว่า ciden ต้องเป็นตัวเลขที่มี 13 ตัว
            if (!System.Text.RegularExpressions.Regex.IsMatch(ciden.Text, @"^\d{13}$"))
            {
                MessageBox.Show("Identification number must be a 13-digit number.");
                return;
            }

            // ตรวจสอบว่า caddress มีเฉพาะตัวอักษรภาษาอังกฤษหรือภาษาไทย ตัวเลข ช่องว่าง และเครื่องหมายพิเศษที่กำหนด (/ - , . &)
            if (!System.Text.RegularExpressions.Regex.IsMatch(caddress.Text, @"^[a-zA-Zก-๙0-9\s/\-,.&]+$"))
            {
                MessageBox.Show("Address can only contain English or Thai letters, numbers, spaces, and specific symbols (/, -, , , ., &).");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();

                    // ตรวจสอบว่าข้อมูลซ้ำหรือไม่
                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM customer_info 
                WHERE 
                    (tel = @tel AND username != @username) OR 
                    (email = @email AND username != @username) OR 
                    (idenno = @idenno AND username != @username) OR 
                    (address = @address AND username != @username)";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@tel", ctel.Text);
                        checkCmd.Parameters.AddWithValue("@email", cemail.Text);
                        checkCmd.Parameters.AddWithValue("@idenno", ciden.Text);
                        checkCmd.Parameters.AddWithValue("@address", caddress.Text);
                        checkCmd.Parameters.AddWithValue("@username", username_ii3); // username ปัจจุบัน

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Duplicate data found: Telephone, Email, ID Number, or Address already exists.");
                            return;
                        }
                    }

                    // เริ่มต้นการทำงานของ Transaction
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        // อัพเดทข้อมูลในตาราง customer_info
                        string customerQuery = "UPDATE customer_info SET name = @name, email = @email, idenno = @idenno, address = @address, tel = @tel WHERE username = @username";
                        using (MySqlCommand customerCmd = new MySqlCommand(customerQuery, conn, transaction))
                        {
                            customerCmd.Parameters.AddWithValue("@name", cname.Text);
                            customerCmd.Parameters.AddWithValue("@email", cemail.Text);
                            customerCmd.Parameters.AddWithValue("@idenno", ciden.Text);
                            customerCmd.Parameters.AddWithValue("@address", caddress.Text);
                            customerCmd.Parameters.AddWithValue("@tel", ctel.Text);
                            customerCmd.Parameters.AddWithValue("@username", username_ii3);

                            int rowsAffected = customerCmd.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                // หากไม่มีแถวที่ตรงกับ username
                                MessageBox.Show("No matching username found to update.");
                                transaction.Rollback(); // ยกเลิกการทำงานของ Transaction
                                return;
                            }
                        }

                        // ยืนยันการทำงานของ Transaction
                        transaction.Commit();

                        MessageBox.Show("Data updated successfully");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }


        }

        



        private void inputinfo3_Load(object sender, EventArgs e)
        {
            // สร้าง Label สำหรับแสดงข้อมูลชื่อผู้ใช้
            labelUsername = new Label();
            labelUsername.Font = new Font("Dubai", 16);
            labelUsername.Location = new Point(880, 36);  // เลื่อนตำแหน่ง Label ไปทางขวา

            // กำหนดขนาดของ Label
            labelUsername.Width = 150;  // กำหนดความกว้างที่เหมาะสม
            labelUsername.Height = 30;  // ขนาด Label

            // ปรับสีพื้นหลังของ Label
            labelUsername.BackColor = Color.Black;  // สีพื้นหลังของ Label

            // ปรับสีข้อความของ Label
            labelUsername.ForeColor = Color.White;  // สีข้อความของ Label

            // เพิ่มช่องว่างทางขวา (ถ้าจำเป็น)
            labelUsername.Padding = new Padding(10, 0, 10, 0);

            // เพิ่ม Label ไปที่ Controls ของฟอร์ม
            this.Controls.Add(labelUsername);

            // ใช้ Paint Event เพื่อวาดข้อความเอง (ไม่ตั้งค่า Text ของ Label)
            labelUsername.Paint += (sender2, e2) =>
            {
                // ข้อความที่จะแสดง (ไม่ใช้ labelUsername.Text แต่ใช้ username_ii โดยตรง)
                string username = username_ii3;

                // คำนวณความกว้างของข้อความ
                SizeF textSize = e2.Graphics.MeasureString(username, labelUsername.Font);

                // คำนวณตำแหน่งข้อความให้เริ่มจากขวา
                int textX = labelUsername.Width - labelUsername.Padding.Right - (int)textSize.Width;
                int textY = labelUsername.Padding.Top;

                // วาดข้อความเอง
                e2.Graphics.DrawString(username, labelUsername.Font, new SolidBrush(labelUsername.ForeColor), new PointF(textX, textY));
            };

            //// แสดงชื่อผู้ใช้ใน TextBox เมื่อฟอร์มโหลด
            //labelUsername.Text = username_ii3;

            if (processid <= 0)
            {
                MessageBox.Show("Invalid Process ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadProcessData();
            LoadCustomerData();
        }


        private void LoadProcessData()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT * FROM process WHERE ID = @processid";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@processid", processid);

                    try
                    {
                        conn.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            label1.Text = reader.GetString(3); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            label7.Text = reader.GetString(4); // ตรวจสอบหมายเลข index ให้ถูกต้อง

                            DateTime date1 = reader.GetDateTime(5); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            DateTime date2 = reader.GetDateTime(6); // ตรวจสอบหมายเลข index ให้ถูกต้อง

                            int buddhistYear1 = date1.Year;
                            int buddhistYear2 = date2.Year;

                            label2.Text = date1.ToString("d/M/") + buddhistYear1 + date1.ToString(" HH:mm:ss");
                            label3.Text = date2.ToString("d/M/") + buddhistYear2 + date2.ToString(" HH:mm:ss");

                            regularCount = reader.GetInt32(7); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            specialCount = reader.GetInt32(8); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            daysDifference = reader.GetInt32(9); // ตรวจสอบหมายเลข index ให้ถูกต้อง

                            UpdateSummary();
                        }
                        else
                        {
                            MessageBox.Show("No data found for the provided Process ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("MySQL Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unexpected Error: " + ex.Message);
                    }
                }
            }
        }

        private void LoadCustomerData()
        {
            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT * FROM customer_info WHERE username = @username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username_ii3);

                    try
                    {
                        conn.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            cname.Text = reader.GetString(4); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            ctel.Text = reader.GetString(3); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            cemail.Text = reader.GetString(5); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            ciden.Text = reader.GetString(6); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                            caddress.Text = reader.GetString(7); // ตรวจสอบหมายเลข index ให้ถูกต้อง
                        }
                        else
                        {
                            MessageBox.Show("No customer info found for the provided username.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("MySQL Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unexpected Error: " + ex.Message);
                    }
                }
            }
        }

        int totalPrice = 0;
        private void UpdateSummary()
        {
            // สมมุติว่าราคา regular และ special มีค่าคงที่
            const int regularPricePerDay = 100; // ราคา per day ของ regular
            const int specialPricePerDay = 200; // ราคา per day ของ special

            // คำนวณราคาสำหรับ regular และ special
            int totalRegularPrice = regularCount * regularPricePerDay * daysDifference;
            int totalSpecialPrice = specialCount * specialPricePerDay * daysDifference;

            // คำนวณราคาทั้งหมด
            totalPrice = totalRegularPrice + totalSpecialPrice;

            // สร้างข้อความสำหรับ TextBox summarize
            string summaryMessage = $"{regularCount} Regular Luggage x {daysDifference} days = {totalRegularPrice.ToString("C0", new CultureInfo("th-TH"))}";
            label5.Text = summaryMessage;

            // สร้างข้อความสำหรับ TextBox summarize2
            string summaryMessage2 = $"{specialCount} Special Luggage x {daysDifference} days = {totalSpecialPrice.ToString("C0", new CultureInfo("th-TH"))}";
            label6.Text = summaryMessage2;

            // แสดงผลรวมราคาใน TextBox textBoxTotalPrice
            label4.Text = totalPrice.ToString("C0", new CultureInfo("th-TH")); // Format to number without decimals
                                           
        }



        private void button4_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่ามี Process ID ก่อนทำการลบ
            if (processid <= 0)
            {
                MessageBox.Show("Invalid Process ID.");
                return;
            }

            // เปิดการเชื่อมต่อกับฐานข้อมูล
            using (MySqlConnection conn = databaseConnection())
            {
                string deleteQuery = "DELETE FROM process WHERE ID = @processid";

                using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@processid", processid);

                    try
                    {
                        conn.Open();
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data successfully deleted from the process table.");

                            // เปิดฟอร์ม inputinfo และส่งค่า username
                            inputinfo inputinfoForm = new inputinfo();
                            inputinfoForm.username_ii = username_ii3; // ส่งค่า username
                            inputinfoForm.Show(); // แสดงฟอร์ม inputinfo

                            // ปิดฟอร์ม inputinfo3
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No data found with the provided ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("MySQL Error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unexpected Error: " + ex.Message);
                    }
                }
            }



        }

        private void button5_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่ามีข้อมูลใน cname, ctel, cemail, ciden, caddress หรือไม่
            if (string.IsNullOrEmpty(cname.Text) ||
                string.IsNullOrEmpty(ctel.Text) ||
                string.IsNullOrEmpty(cemail.Text) ||
                string.IsNullOrEmpty(ciden.Text) ||
                string.IsNullOrEmpty(caddress.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // ตรวจสอบว่า cname ต้องเป็นตัวอักษรภาษาอังกฤษหรือภาษาไทยและช่องว่างเท่านั้น
            if (!System.Text.RegularExpressions.Regex.IsMatch(cname.Text, @"^[a-zA-Zก-๙\s]+$"))
            {
                MessageBox.Show("Name must contain only English or Thai letters and spaces, no special characters.");
                return;
            }

            // ตรวจสอบว่า ctel ต้องเป็นตัวเลขที่มี 10 ตัว
            if (!System.Text.RegularExpressions.Regex.IsMatch(ctel.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Telephone number must be a 10-digit number.");
                return;
            }

            // ตรวจสอบว่า cemail ต้องเป็นอีเมลที่ถูกต้อง
            if (!System.Text.RegularExpressions.Regex.IsMatch(cemail.Text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Email must be in a valid format.");
                return;
            }

            // ตรวจสอบว่า ciden ต้องเป็นตัวเลขที่มี 13 ตัว
            if (!System.Text.RegularExpressions.Regex.IsMatch(ciden.Text, @"^\d{13}$"))
            {
                MessageBox.Show("Identification number must be a 13-digit number.");
                return;
            }

            // ตรวจสอบว่า caddress มีเฉพาะตัวอักษรภาษาอังกฤษหรือภาษาไทย ตัวเลข ช่องว่าง และเครื่องหมายพิเศษที่กำหนด (/ - , . &)
            if (!System.Text.RegularExpressions.Regex.IsMatch(caddress.Text, @"^[a-zA-Zก-๙0-9\s/\-,.&]+$"))
            {
                MessageBox.Show("Address can only contain English or Thai letters, numbers, spaces, and specific symbols (/, -, , , ., &).");
                return;
            }

            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();

                    // ตรวจสอบว่าข้อมูลซ้ำหรือไม่
                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM customer_info 
                WHERE 
                    (tel = @tel AND username != @username) OR 
                    (email = @email AND username != @username) OR 
                    (idenno = @idenno AND username != @username) OR 
                    (address = @address AND username != @username)";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@tel", ctel.Text);
                        checkCmd.Parameters.AddWithValue("@email", cemail.Text);
                        checkCmd.Parameters.AddWithValue("@idenno", ciden.Text);
                        checkCmd.Parameters.AddWithValue("@address", caddress.Text);
                        checkCmd.Parameters.AddWithValue("@username", username_ii3); // username ปัจจุบัน

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Duplicate data found: Telephone, Email, ID Number, or Address already exists.");
                            return;
                        }
                    }

                    // เริ่มต้นการทำงานของ Transaction
                    using (MySqlTransaction transaction = conn.BeginTransaction())
                    {
                        // อัพเดทข้อมูลในตาราง customer_info
                        string customerQuery = "UPDATE customer_info SET name = @name, email = @email, idenno = @idenno, address = @address, tel = @tel WHERE username = @username";
                        using (MySqlCommand customerCmd = new MySqlCommand(customerQuery, conn, transaction))
                        {
                            customerCmd.Parameters.AddWithValue("@name", cname.Text);
                            customerCmd.Parameters.AddWithValue("@email", cemail.Text);
                            customerCmd.Parameters.AddWithValue("@idenno", ciden.Text);
                            customerCmd.Parameters.AddWithValue("@address", caddress.Text);
                            customerCmd.Parameters.AddWithValue("@tel", ctel.Text);
                            customerCmd.Parameters.AddWithValue("@username", username_ii3);

                            int rowsAffected = customerCmd.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                // หากไม่มีแถวที่ตรงกับ username
                                MessageBox.Show("No matching username found to update.");
                                transaction.Rollback(); // ยกเลิกการทำงานของ Transaction
                                return;
                            }
                        }

                        // ยืนยันการทำงานของ Transaction
                        transaction.Commit();

                        MessageBox.Show("Data updated successfully");
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            inputinfo5 inputinfo5 = new inputinfo5();
            inputinfo5.username_qr = username_ii3;
            inputinfo5.processid_qr = processid;
            inputinfo5.name_qr = cname.Text;
            inputinfo5.tel_qr = ctel.Text;
            inputinfo5.email_qr = cemail.Text;
            inputinfo5.iden_qr = ciden.Text;
            inputinfo5.address_qr = caddress.Text;

            inputinfo5.Show();
            this.Hide();
        }

       
    }
}



