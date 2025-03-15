using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luggage
{
    public partial class dropoff : Form
    {
        private string myconnection = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";

        // เชื่อมต่อฐานข้อมูล MySQL
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }


        public dropoff()
        {
            InitializeComponent();
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            homeadmin homeadmin = new homeadmin();
            homeadmin.Show();
            this.Hide();
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
        

        private void button3_Click(object sender, EventArgs e)
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

                string query = "UPDATE luggageinfo SET realdropoff = @realdropoff, status = @status WHERE cusid = @cusid";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add("@realdropoff", MySqlDbType.DateTime).Value = newDayInDate;
                    command.Parameters.Add("@status", MySqlDbType.String).Value = status;
                    command.Parameters.Add("@cusid", MySqlDbType.String).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //string currentDayInDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            //DateTime parsedDateTime = DateTime.ParseExact(currentDayInDate, "MM/dd/yyyy HH:mm:ss", null);
            DateTime currentDayInDate = DateTime.Now;

            // หรือระบุวันที่ที่คุณต้องการ
            string id = txtId.Text; // รหัสที่ต้องการอัปเดต
            string status = "already dropped off";//status


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

                // เปลี่ยนไปยังฟอร์ม homeadmin
                homeadmin homeadmin = new homeadmin();
                homeadmin.Show(); // แสดงฟอร์มใหม่
                this.Hide(); // ซ่อนฟอร์มปัจจุบัน
            }
            catch (Exception ex)
            {
                // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message,
                                "ข้อผิดพลาด",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }


        private void button5_Click(object sender, EventArgs e)
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
        private void ADDImageIntoDatabase(string connectionString, Image image, Image identiimage, string cusid)
        {
            byte[] imageData = ImageToByteArray(image);
            byte[] imageDataiden = ImageToByteArray(identiimage);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE luggageinfo SET image = @image, identiimage = @identiimage WHERE cusid = @cusid";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@cusid", MySqlDbType.String).Value = cusid;
                    command.Parameters.Add("@image", MySqlDbType.VarBinary).Value = imageData;
                    command.Parameters.Add("@identiimage", MySqlDbType.VarBinary).Value = imageDataiden;

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

       

        private void button1_Click(object sender, EventArgs e)
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
    }
}
