using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luggage
{
    public partial class pickup : Form
    {

        private string myconnection = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";

        // เชื่อมต่อฐานข้อมูล MySQL
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }


        public pickup()
        {
            InitializeComponent();
        }

        

        private void showLuggage(string idToSearch)
        {
            DataTable dataTable = new DataTable();
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM luggageinfo WHERE cusid = @cusid";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@cusid", idToSearch);
                        using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(selectCommand))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Date conversion logic
                        if (row["dropoff"] is DateTime dropoffDate && dropoffDate.Year >= 2560)
                        {
                            row["dropoff"] = dropoffDate.AddYears(-543);
                        }

                        if (row["pickup"] is DateTime pickupDate && pickupDate.Year >= 2560)
                        {
                            row["pickup"] = pickupDate.AddYears(-543);
                        }
                    }

                    dataLuggage.DataSource = dataTable;
                    dataLuggage.Columns["dropoff"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dataLuggage.Columns["pickup"].DefaultCellStyle.Format = "dd/MM/yyyy";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            //using (MySqlConnection conn = databaseConnection())
            //{
            //    string query = "SELECT * FROM luggageinfo WHERE cusid = @cusid";

            //    using (MySqlCommand cmd = new MySqlCommand(query, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@cusid", idToSearch);

            //        try
            //        {
            //            conn.Open();
            //            using (MySqlDataReader reader = cmd.ExecuteReader())
            //            {
            //                if (reader.Read())
            //                {
            //                    // Assuming the columns are in specific indices
            //                    label1.Text = reader.GetString(1); // Replace index 1 with actual column index
            //                    label2.Text = reader.GetString(2); // Replace index 2 with actual column index
            //                    label3.Text = reader.GetString(3);
            //                    label4.Text = reader.GetString(4);
            //                    label5.Text = reader.GetString(5);

            //                    label20.Text = reader.GetString(20);
            //                    label21.Text = reader.GetString(21);
            //                    label22.Text = reader.GetString(22);
            //                    label10.Text = reader.GetString(24);
            //                    label11.Text = reader.GetString(25);


            //                    // Assuming date columns are at index 8 and 9
            //                    DateTime date1 = reader.GetDateTime(8);
            //                    DateTime date2 = reader.GetDateTime(9);

            //                    // Convert to Buddhist Era (พ.ศ.)
            //                    int buddhistYear1 = date1.Year;
            //                    int buddhistYear2 = date2.Year;

            //                    //// Format date with Buddhist Era
            //                    //label8.Text = date1.ToString("d/M/") + buddhistYear1 + date1.ToString(" HH:mm:ss");
            //                    //label9.Text = date2.ToString("d/M/") + buddhistYear2 + date2.ToString(" HH:mm:ss");

            //                    // Format date with Buddhist Era
            //                    label8.Text = date1.ToString("d/M/") + buddhistYear1;
            //                    label9.Text = date2.ToString("d/M/") + buddhistYear1;

            //                    // Get counts and days difference
            //                    int regularCount = reader.GetInt32(6);
            //                    int specialCount = reader.GetInt32(7);
            //                    int daysDifference = reader.GetInt32(14);
            //                    int subtotal = reader.GetInt32(15);
            //                    int tax = reader.GetInt32(16);
            //                    int discount = reader.GetInt32(17);
            //                    int total = reader.GetInt32(18);

            //                    // Display counts and days difference in different labels
            //                    label6.Text = regularCount.ToString();
            //                    label7.Text = specialCount.ToString();
            //                    label14.Text = daysDifference.ToString();
            //                    label15.Text = Decimal.Truncate(subtotal).ToString("C0", new CultureInfo("th-TH"));
            //                    label16.Text = Decimal.Truncate(tax).ToString("C0", new CultureInfo("th-TH"));
            //                    label17.Text = Decimal.Truncate(discount).ToString("C0", new CultureInfo("th-TH"));
            //                    label18.Text = Decimal.Truncate(total).ToString("C0", new CultureInfo("th-TH"));



            //                }
            //            }
            //        }
            //        catch (MySqlException ex)
            //        {
            //            MessageBox.Show("MySQL Error: " + ex.Message);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Unexpected Error: " + ex.Message);
            //        }
            //    }
            //}
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // เรียกใช้ showLuggage เพื่อแสดงข้อมูล
            string idToSearch = txtId.Text; // หรือจะใช้ TextBox ที่เก็บ ID ที่ต้องการค้นหาก็ได้
            showLuggage(idToSearch);
        }

        private void UpdateDayIn(string connectionString, DateTime newDayInDate, string id, string status)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE luggageinfo SET realpickup = @realpickup, status = @status WHERE cusid = @cusid";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add("@realpickup", MySqlDbType.DateTime).Value = newDayInDate;
                    command.Parameters.Add("@status", MySqlDbType.String).Value = status;
                    command.Parameters.Add("@cusid", MySqlDbType.String).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "Image Files (.jpg; *.jpeg; *.png; *.gif)|.jpg; *.jpeg; *.png; *.gif";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.Multiselect = false;
                DialogResult result = openFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {


                    string imagePath = openFileDialog1.FileName;
                    //byte[] imagebyte = File.ReadAllBytes(imagePath);
                    //fileimage(imagebyte);
                    pictureBox1.Image = Image.FromFile(imagePath);
                    pictureBox1.Tag = Path.GetFileName(imagePath);

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "Image Files (.jpg; *.jpeg; *.png; *.gif)|.jpg; *.jpeg; *.png; *.gif";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.Multiselect = false;
                DialogResult result = openFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {


                    string imagePath = openFileDialog1.FileName;
                    //byte[] imagebyte = File.ReadAllBytes(imagePath);
                    //fileimage(imagebyte);
                    pictureBox2.Image = Image.FromFile(imagePath);
                    pictureBox2.Tag = Path.GetFileName(imagePath);

                }
            }
        }

        private void ADDImageIntoDatabase(string connectionString, Image image, Image identiimagetwo, string cusid)
        {
            byte[] imageData = ImageToByteArray(image);
            byte[] imageDataiden = ImageToByteArray(identiimagetwo);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE luggageinfo SET imagetwo = @imagetwo, identiimagetwo = @identiimagetwo WHERE cusid = @cusid";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@cusid", MySqlDbType.String).Value = cusid;
                    command.Parameters.Add("@imagetwo", MySqlDbType.VarBinary).Value = imageData;
                    command.Parameters.Add("@identiimagetwo", MySqlDbType.VarBinary).Value = imageDataiden;

                    command.ExecuteNonQuery();
                }

            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //string currentDayInDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            //DateTime parsedDateTime = DateTime.ParseExact(currentDayInDate, "MM/dd/yyyy HH:mm:ss", null);
            DateTime currentDayInDate = DateTime.Now;

            // หรือระบุวันที่ที่คุณต้องการ
            string id = txtId.Text; // รหัสที่ต้องการอัปเดต
            string status = "already picked up";//status


            try
            {
                // เรียกใช้ฟังก์ชันเพื่ออัปเดตข้อมูล
                UpdateDayIn(myconnection, currentDayInDate, id, status);

                // เพิ่มรูปภาพลงในฐานข้อมูล
                ADDImageIntoDatabase(myconnection, pictureBox1.Image, pictureBox2.Image, txtId.Text);

                // แสดงข้อความแจ้งเตือนเมื่อบันทึกข้อมูลสำเร็จ
                MessageBox.Show("ข้อมูลได้ถูกบันทึกเข้าตาราง luggageinfo เรียบร้อยแล้ว",
                                "การบันทึกสำเร็จ",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message,
                                "ข้อผิดพลาด",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }


            string cusid = txtId.Text.Trim(); // ใช้ Trim() เพื่อลบช่องว่างรอบๆ

            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();

                // ตรวจสอบว่ามีแถวที่ตรงกับ cusid ใน luggageinfo
                string checkQuery = "SELECT COUNT(*) FROM luggageinfo WHERE cusid = @cusid";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@cusid", cusid);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("CUSID not found in luggageinfo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // ตรวจสอบค่าว่างในข้อมูล
                string checkDataQuery = "SELECT * FROM luggageinfo WHERE cusid = @cusid";
                using (MySqlCommand checkDataCmd = new MySqlCommand(checkDataQuery, conn))
                {
                    checkDataCmd.Parameters.AddWithValue("@cusid", cusid);

                    using (MySqlDataReader reader = checkDataCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool hasInvalidData =
                                reader.IsDBNull(reader.GetOrdinal("namectm")) || string.IsNullOrWhiteSpace(reader["namectm"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("tel")) || string.IsNullOrWhiteSpace(reader["tel"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("service")) || string.IsNullOrWhiteSpace(reader["service"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("location")) || string.IsNullOrWhiteSpace(reader["location"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("locationtwo")) || string.IsNullOrWhiteSpace(reader["locationtwo"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("regular")) || string.IsNullOrWhiteSpace(reader["regular"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("special")) || string.IsNullOrWhiteSpace(reader["special"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("dropoff")) || string.IsNullOrWhiteSpace(reader["dropoff"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("pickup")) || string.IsNullOrWhiteSpace(reader["pickup"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("realdropoff")) || string.IsNullOrWhiteSpace(reader["realdropoff"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("realpickup")) || string.IsNullOrWhiteSpace(reader["realpickup"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("image")) || string.IsNullOrWhiteSpace(reader["image"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("imagetwo")) || string.IsNullOrWhiteSpace(reader["imagetwo"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("duration")) || string.IsNullOrWhiteSpace(reader["duration"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("subtotal")) || string.IsNullOrWhiteSpace(reader["subtotal"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("tax")) || string.IsNullOrWhiteSpace(reader["tax"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("discount")) || string.IsNullOrWhiteSpace(reader["discount"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("total")) || string.IsNullOrWhiteSpace(reader["total"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("receipt")) || string.IsNullOrWhiteSpace(reader["receipt"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("status")) || string.IsNullOrWhiteSpace(reader["status"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("email")) || string.IsNullOrWhiteSpace(reader["email"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("username")) || string.IsNullOrWhiteSpace(reader["username"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("cusid")) || string.IsNullOrWhiteSpace(reader["cusid"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("idenno")) || string.IsNullOrWhiteSpace(reader["idenno"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("address")) || string.IsNullOrWhiteSpace(reader["address"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("identiimage")) || string.IsNullOrWhiteSpace(reader["identiimage"].ToString()) ||
                                reader.IsDBNull(reader.GetOrdinal("identiimagetwo")) || string.IsNullOrWhiteSpace(reader["identiimagetwo"].ToString());

                            if (hasInvalidData)
                            {
                                MessageBox.Show("Data contains NULL or empty values. Cannot copy to abouthistory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("CUSID not found in luggageinfo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                // ตรวจสอบว่ามีข้อมูลที่ตรงกับ cusid ใน abouthistory หรือไม่
                string checkAbouthistoryQuery = "SELECT COUNT(*) FROM abouthistory WHERE cusid = @cusid";
                using (MySqlCommand checkAbouthistoryCmd = new MySqlCommand(checkAbouthistoryQuery, conn))
                {
                    checkAbouthistoryCmd.Parameters.AddWithValue("@cusid", cusid);

                    int abouthistoryCount = Convert.ToInt32(checkAbouthistoryCmd.ExecuteScalar());
                    if (abouthistoryCount > 0)
                    {
                        MessageBox.Show("CUSID already exists in abouthistory. Data will not be copied again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // คัดลอกข้อมูลจาก luggageinfo ไปยัง abouthistory
                string insertQuery = @"
            INSERT INTO abouthistory (namectm, tel, service, location, locationtwo, regular, special, dropoff, pickup, realdropoff, realpickup, image, imagetwo, duration, subtotal, tax, discount, total, receipt, status, email, username, cusid, idenno, address, identiimage, identiimagetwo)
            SELECT namectm, tel, service, location, locationtwo, regular, special, dropoff, pickup, realdropoff, realpickup, image, imagetwo, duration, subtotal, tax, discount, total, receipt, status, email, username, cusid, idenno, address, identiimage, identiimagetwo
            FROM luggageinfo
            WHERE cusid = @cusid";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@cusid", cusid);

                    try
                    {
                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // หลังจากคัดลอกข้อมูลสำเร็จ ให้ลบข้อมูลจากตาราง luggageinfo
                            string deleteQuery = "DELETE FROM luggageinfo WHERE cusid = @cusid";
                            using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn))
                            {
                                deleteCmd.Parameters.AddWithValue("@cusid", cusid);

                                deleteCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Data copied into abouthistory table and deleted successfully from luggageinfo.");

              

                            homeadmin homeadmin = new homeadmin();
                            homeadmin.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("No rows copied. Please check the data and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            homeadmin homeadmin = new homeadmin();
            homeadmin.Show();
            this.Hide();
        }

        
    }
}
