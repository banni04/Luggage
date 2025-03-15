using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Luggage
{
    public partial class history : Form
    {
        
        
        public history()
        {
            InitializeComponent();
            LoadData();
          

        }

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            homeadmin homeadmin = new homeadmin();
            homeadmin.Show();
            this.Hide();
        }

        
        private void history_Load(object sender, EventArgs e)
        {
            LoadData();
            
            FillDGV("");
            //CalculateTotalSum();
            //CalculateTotalSumService();
            //CalculatePopularLocations();

            // เพิ่มประเภทการดูข้อมูล
            comboBoxViewType.Items.Add("Daily");
            comboBoxViewType.Items.Add("Monthly");
            comboBoxViewType.Items.Add("Yearly");


            // เพิ่มวัน
            for (int i = 1; i <= 31; i++)
            {
                comboBoxDay.Items.Add(i);
            }

            // เพิ่มเดือน
            for (int i = 1; i <= 12; i++)
            {
                comboBoxMonth.Items.Add(i);
            }

            // เพิ่มปี
            for (int i = 2020; i <= 2024; i++)
            {
                comboBoxYear.Items.Add(i);
            }
        }

        private void CheckData()
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=luggage;User ID=root;Password=;";
            string query = string.Empty;
            string selectedDate = string.Empty; // ประกาศที่นี่
            int month = 0; // ประกาศที่นี่
            int year = 0; // ประกาศที่นี่

            if (comboBoxViewType.SelectedItem == null)
            {
                MessageBox.Show("Please select a view type.");
                return;
            }

            string viewType = comboBoxViewType.SelectedItem.ToString();

            if (viewType == "Daily" && comboBoxDay.SelectedItem != null && comboBoxMonth.SelectedItem != null && comboBoxYear.SelectedItem != null)
            {
                int day = Convert.ToInt32(comboBoxDay.SelectedItem);
                month = Convert.ToInt32(comboBoxMonth.SelectedItem);
                year = Convert.ToInt32(comboBoxYear.SelectedItem);

                selectedDate = new DateTime(year, month, day).ToString("yyyy-MM-dd");
                query = "SELECT * FROM abouthistory WHERE DATE(realpickup) = @date"; // แทนที่ your_date_column
            }
            else if (viewType == "Monthly" && comboBoxMonth.SelectedItem != null && comboBoxYear.SelectedItem != null)
            {
                month = Convert.ToInt32(comboBoxMonth.SelectedItem);
                year = Convert.ToInt32(comboBoxYear.SelectedItem);

                query = "SELECT * FROM abouthistory WHERE MONTH(realpickup) = @month AND YEAR(realpickup) = @year"; // แทนที่ your_date_column
            }
            else if (viewType == "Yearly" && comboBoxYear.SelectedItem != null)
            {
                year = Convert.ToInt32(comboBoxYear.SelectedItem);
                query = "SELECT * FROM abouthistory WHERE YEAR(realpickup) = @year"; // แทนที่ your_date_column
            }
            else
            {
                MessageBox.Show("Please select valid date components.");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(query, connection);

                    if (viewType == "Daily")
                    {
                        command.Parameters.AddWithValue("@date", selectedDate);
                    }
                    else if (viewType == "Monthly")
                    {
                        command.Parameters.AddWithValue("@month", month);
                        command.Parameters.AddWithValue("@year", year);
                    }
                    else if (viewType == "Yearly")
                    {
                        command.Parameters.AddWithValue("@year", year);
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable(); // ย้ายออกจาก while
                        dataTable.Load(reader); // โหลดข้อมูลจาก reader ลงใน DataTable

                        if (dataTable.Rows.Count > 0)
                        {
                            dataLuggage.DataSource = dataTable; // แสดงใน DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No data found for the selected criteria.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }

        private void dataLuggage_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่ามีการเลือกแถวอยู่หรือไม่
            if (dataLuggage.CurrentRow == null)
            {
                MessageBox.Show("ยังไม่มีการเลือกแถว.");
                return;
            }

            try
            {
                // ตรวจสอบข้อมูลภาพในเซลล์ที่คอลัมน์ที่ 12
                var imgCellValue12 = dataLuggage.CurrentRow.Cells[12].Value;
                if (imgCellValue12 != DBNull.Value && imgCellValue12 != null)
                {
                    Byte[] img12 = (Byte[])imgCellValue12;

                    if (img12.Length > 0) // ตรวจสอบความยาวของข้อมูลภาพ
                    {
                        using (MemoryStream ms = new MemoryStream(img12))
                        {
                            try
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                            }
                            catch (ArgumentException)
                            {
                                pictureBox1.Image = null; // ล้างภาพใน PictureBox1 ถ้าไม่สามารถสร้างภาพได้
                            }
                        }
                    }
                    else
                    {
                        pictureBox1.Image = null; // ล้างภาพใน PictureBox1 ถ้าข้อมูลเป็นอาร์เรย์ว่าง
                    }
                }
                else
                {
                    pictureBox1.Image = null; // ล้างภาพใน PictureBox1 ถ้าไม่มีข้อมูล
                }

                // ตรวจสอบข้อมูลภาพในเซลล์ที่คอลัมน์ที่ 13
                var imgCellValue13 = dataLuggage.CurrentRow.Cells[13].Value;
                if (imgCellValue13 != DBNull.Value && imgCellValue13 != null)
                {
                    Byte[] img13 = (Byte[])imgCellValue13;

                    if (img13.Length > 0) // ตรวจสอบความยาวของข้อมูลภาพ
                    {
                        using (MemoryStream ms = new MemoryStream(img13))
                        {
                            try
                            {
                                pictureBox2.Image = Image.FromStream(ms);
                            }
                            catch (ArgumentException)
                            {
                                pictureBox2.Image = null; // ล้างภาพใน PictureBox2 ถ้าไม่สามารถสร้างภาพได้
                            }
                        }
                    }
                    else
                    {
                        pictureBox2.Image = null; // ล้างภาพใน PictureBox2 ถ้าข้อมูลเป็นอาร์เรย์ว่าง
                    }
                }
                else
                {
                    pictureBox2.Image = null; // ล้างภาพใน PictureBox2 ถ้าไม่มีข้อมูล
                }

                // ตรวจสอบข้อมูลภาพในแถวที่คอลัมน์ที่ 26
                var imgCellValue26 = dataLuggage.CurrentRow.Cells[26].Value;
                if (imgCellValue26 != DBNull.Value && imgCellValue26 != null)
                {
                    Byte[] img26 = (Byte[])imgCellValue26;

                    if (img26.Length > 0) // ตรวจสอบความยาวของข้อมูลภาพ
                    {
                        using (MemoryStream ms = new MemoryStream(img26))
                        {
                            try
                            {
                                pictureBox3.Image = Image.FromStream(ms); //ถ้าตรวจสอบว่ามีภาพก็จะแสดงที่picturebox
                            }
                            catch (ArgumentException)
                            {
                                pictureBox3.Image = null; // ล้างภาพใน PictureBox1 ถ้าไม่สามารถสร้างภาพได้
                            }
                        }
                    }
                    else
                    {
                        pictureBox3.Image = null; // ล้างภาพใน PictureBox1 ถ้าข้อมูลเป็นอาร์เรย์ว่าง
                    }
                }
                else
                {
                    pictureBox3.Image = null; // ล้างภาพใน PictureBox1 ถ้าไม่มีข้อมูล
                }

                // ตรวจสอบข้อมูลภาพในแถวที่คอลัมน์ที่ 27
                var imgCellValue27 = dataLuggage.CurrentRow.Cells[27].Value;
                if (imgCellValue27 != DBNull.Value && imgCellValue27 != null)
                {
                    Byte[] img27 = (Byte[])imgCellValue27;

                    if (img27.Length > 0) // ตรวจสอบความยาวของข้อมูลภาพ
                    {
                        using (MemoryStream ms = new MemoryStream(img27))
                        {
                            try
                            {
                                pictureBox4.Image = Image.FromStream(ms);
                            }
                            catch (ArgumentException)
                            {
                                pictureBox4.Image = null; // ล้างภาพใน PictureBox2 ถ้าไม่สามารถสร้างภาพได้
                            }
                        }
                    }
                    else
                    {
                        pictureBox4.Image = null; // ล้างภาพใน PictureBox2 ถ้าข้อมูลเป็นอาร์เรย์ว่าง
                    }
                }
                else
                {
                    pictureBox4.Image = null; // ล้างภาพใน PictureBox2 ถ้าไม่มีข้อมูล
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}");
            }
        }

        

        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    string query = @"
            SELECT *
            FROM abouthistory
            WHERE namectm IS NOT NULL
              AND tel IS NOT NULL
              AND service IS NOT NULL
              AND location IS NOT NULL
              AND locationtwo IS NOT NULL
              AND regular IS NOT NULL
              AND special IS NOT NULL
              AND dropoff IS NOT NULL AND dropoff != '0000-00-00 00:00:00'
              AND pickup IS NOT NULL AND pickup != '0000-00-00 00:00:00'
              AND realdropoff IS NOT NULL AND realdropoff != '0000-00-00 00:00:00'
              AND realpickup IS NOT NULL AND realpickup != '0000-00-00 00:00:00'
              AND image IS NOT NULL
              AND imagetwo IS NOT NULL
              AND duration IS NOT NULL
              AND subtotal IS NOT NULL
              AND tax IS NOT NULL
              AND discount IS NOT NULL
              AND total IS NOT NULL
              AND receipt IS NOT NULL
              AND status IS NOT NULL
              AND email IS NOT NULL
              AND username IS NOT NULL
              AND cusid IS NOT NULL";

                    conn.Open();
                    DataTable dataTable = new DataTable();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    // แปลงวันที่ใน DataTable
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // แปลงคอลัมน์วันที่แรก
                        if (row["dropoff"] != DBNull.Value)
                        {
                            DateTime firstDateValue = Convert.ToDateTime(row["dropoff"]);
                            int year = firstDateValue.Year;
                            if (year >= 2560) // แปลงเฉพาะปีพุทธศักราช
                            {
                                firstDateValue = firstDateValue.AddYears(-543); // แปลงเป็นปีคริสต์ศักราช
                            }
                            row["dropoff"] = firstDateValue; // อัพเดทค่าใน DataRow
                        }

                        // แปลงคอลัมน์วันที่ที่สอง
                        if (row["pickup"] != DBNull.Value)
                        {
                            DateTime secondDateValue = Convert.ToDateTime(row["pickup"]);
                            int year = secondDateValue.Year;
                            if (year >= 2560) // แปลงเฉพาะปีพุทธศักราช
                            {
                                secondDateValue = secondDateValue.AddYears(-543); // แปลงเป็นปีคริสต์ศักราช
                            }
                            row["pickup"] = secondDateValue; // อัพเดทค่าใน DataRow
                        }
                    }

                    // ตั้งค่า DataGridView ให้แสดงข้อมูลจาก DataTable
                    dataLuggage.DataSource = dataTable;

                    // ตั้งค่าฟอร์แมตวันที่ใน DataGridView
                    dataLuggage.Columns["dropoff"].DefaultCellStyle.Format = "dd/MM/yyyy"; // ปรับเปลี่ยนให้ตรงกับชื่อคอลัมน์
                    dataLuggage.Columns["pickup"].DefaultCellStyle.Format = "dd/MM/yyyy"; // ปรับเปลี่ยนให้ตรงกับชื่อคอลัมน์

                    // เพิ่มคอลัมน์สำหรับแสดงภาพใน DataTable
                    dataTable.Columns.Add("Pic", typeof(Image));
                    dataTable.Columns.Add("Pictwo", typeof(Image));

                    // แปลงข้อมูล BLOB เป็นภาพ
                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["image"] != DBNull.Value)
                        {
                            row["Pic"] = ConvertBlobToImage(row["image"] as byte[]);
                        }

                        if (row["imagetwo"] != DBNull.Value)
                        {
                            row["Pictwo"] = ConvertBlobToImage(row["imagetwo"] as byte[]);
                        }
                    }

                    // ซ่อนคอลัมน์ BLOB ถ้าจำเป็น
                    if (dataLuggage.Columns.Contains("image"))
                    {
                        dataLuggage.Columns["image"].Visible = false;
                    }

                    if (dataLuggage.Columns.Contains("imagetwo"))
                    {
                        dataLuggage.Columns["imagetwo"].Visible = false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("ข้อผิดพลาด MySQL: " + ex.Message, "ข้อผิดพลาดฐานข้อมูล", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ข้อผิดพลาดที่ไม่คาดคิด: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private Image ConvertBlobToImage(byte[] blobData)
        {
            if (blobData == null || blobData.Length == 0)
            {
                // หากไม่มีข้อมูล ให้ส่งกลับเป็นภาพดีฟอลต์
                return CreateDefaultImage();
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(blobData))
                {
                    // ตรวจสอบขนาดของ MemoryStream ก่อนสร้างภาพ
                    if (ms.Length == 0)
                    {
                        MessageBox.Show("MemoryStream is empty.", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return CreateDefaultImage();
                    }

                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error converting BLOB to image: " + ex.Message, "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return CreateDefaultImage();
            }
        }


        private Image CreateDefaultImage()
        {
            Bitmap bitmap = new Bitmap(100, 100); // ขนาดของภาพดีฟอลต์
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.LightGray); // สีพื้นหลัง
                using (System.Drawing.Font font = new System.Drawing.Font("Arial", 12))
                {
                    g.DrawString("No Image", font, Brushes.Black, new PointF(10, 40));
                }
            }
            return bitmap;
        }

        //public void CalculateTotalSum()
        //{
        //    decimal totalSum = 0;


        //    // ตรวจสอบว่ามีข้อมูลใน DataGridView หรือไม่
        //    foreach (DataGridViewRow row in dataLuggage.Rows)
        //    {
        //        // ตรวจสอบให้แน่ใจว่าแถวไม่ใช่แถวที่ถูกสร้างขึ้น
        //        if (row.Cells["total"].Value != null) // เปลี่ยน "total" เป็นชื่อคอลัมน์ที่ถูกต้อง
        //        {
        //            totalSum += Convert.ToDecimal(row.Cells["total"].Value);
        //        }


        //    }

        //    //// แสดงผลรวมที่คำนวณได้
        //    //MessageBox.Show($"Total Sum: {totalSum}"); // เพิ่มเพื่อแสดงค่าผลรวม



        //    // อัปเดต label1
        //    label1.Text = $"{totalSum.ToString("C0", new CultureInfo("th-TH"))}"; // แสดงผลรวมใน label1

        //}

        //public void CalculateTotalSumService()
        //{
        //    string connectionString = "Server=127.0.0.1;Port=3306;Database=luggage;User ID=root;Password=;";
        //    string query = @"
        //SELECT 
        //    SUM(CASE WHEN service = 'luggagestorage' THEN total ELSE 0 END) AS TotalLuggageStorage,
        //    SUM(CASE WHEN service = 'luggagedelivery' THEN total ELSE 0 END) AS TotalLuggageDelivery
        //FROM abouthistory";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            MySqlCommand command = new MySqlCommand(query, connection);
        //            connection.Open();
        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    object storageTotal = reader["TotalLuggageStorage"];
        //                    object deliveryTotal = reader["TotalLuggageDelivery"];

        //                    label2.Text = storageTotal != DBNull.Value
        //                        ? Convert.ToDecimal(storageTotal).ToString("C0", new CultureInfo("th-TH"))
        //                        : "No data for Luggage Storage";

        //                    label3.Text = deliveryTotal != DBNull.Value
        //                        ? Convert.ToDecimal(deliveryTotal).ToString("C0", new CultureInfo("th-TH"))
        //                        : "No data for Luggage Delivery";
        //                }
        //                else
        //                {
        //                    label2.Text = "No data for Luggage Storage";
        //                    label3.Text = "No data for Luggage Delivery";
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception
        //            MessageBox.Show($"An error occurred: {ex.Message}");
        //        }
        //    }
        //}

        //public void CalculatePopularLocations()
        //{
        //    string connectionString = "Server=127.0.0.1;Port=3306;Database=luggage;User ID=root;Password=;";

        //    string queryLocation = @"
        //SELECT location
        //FROM abouthistory
        //GROUP BY location
        //ORDER BY COUNT(location) DESC
        //LIMIT 1";

        //    string queryLocationTwo = @"
        //SELECT locationtwo
        //FROM abouthistory
        //GROUP BY locationtwo
        //ORDER BY COUNT(locationtwo) DESC
        //LIMIT 1";

        //    string queryService = @"
        //SELECT service
        //FROM abouthistory
        //GROUP BY service
        //ORDER BY COUNT(service) DESC
        //LIMIT 1";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();

        //            // Query สำหรับ location
        //            MySqlCommand commandLocation = new MySqlCommand(queryLocation, connection);
        //            var location = commandLocation.ExecuteScalar()?.ToString();
        //            label5.Text = location ?? "No data for location";

        //            // Query สำหรับ locationtwo
        //            MySqlCommand commandLocationTwo = new MySqlCommand(queryLocationTwo, connection);
        //            var locationTwo = commandLocationTwo.ExecuteScalar()?.ToString();
        //            label6.Text = locationTwo ?? "No data for locationtwo";

        //            // Query สำหรับ service
        //            MySqlCommand commandService = new MySqlCommand(queryService, connection);
        //            var service = commandService.ExecuteScalar()?.ToString();
        //            label7.Text = service ?? "No data for service";
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception
        //            MessageBox.Show($"An error occurred: {ex.Message}");
        //        }
        //    }
        //}

        //private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
        public void CalculateTotalSum()
        {
            decimal totalSum = 0;
            decimal totalSumLuggageStorage = 0;
            decimal totalSumLuggageDelivery = 0;

            Dictionary<string, int> locationCount = new Dictionary<string, int>();
            Dictionary<string, int> locationTwoCount = new Dictionary<string, int>();
            Dictionary<string, int> serviceCount = new Dictionary<string, int>();

            // ตรวจสอบว่ามีข้อมูลใน DataGridView หรือไม่
            foreach (DataGridViewRow row in dataLuggage.Rows)
            {
                if (row.Cells["total"].Value != null)
                {
                    decimal total = Convert.ToDecimal(row.Cells["total"].Value);
                    totalSum += total;

                    // คำนวณผลรวมสำหรับ Luggage Storage และ Luggage Delivery
                    string service = row.Cells["service"].Value?.ToString();
                    if (service == "luggagestorage")
                    {
                        totalSumLuggageStorage += total;
                    }
                    else if (service == "luggagedelivery")
                    {
                        totalSumLuggageDelivery += total;
                    }

                    // คำนวณจำนวน location และ locationtwo
                    string location = row.Cells["location"].Value?.ToString();
                    if (location != null)
                    {
                        if (locationCount.ContainsKey(location))
                        {
                            locationCount[location]++;
                        }
                        else
                        {
                            locationCount[location] = 1;
                        }
                    }

                    string locationTwo = row.Cells["locationtwo"].Value?.ToString();
                    if (locationTwo != null)
                    {
                        if (locationTwoCount.ContainsKey(locationTwo))
                        {
                            locationTwoCount[locationTwo]++;
                        }
                        else
                        {
                            locationTwoCount[locationTwo] = 1;
                        }
                    }

                    // คำนวณจำนวน service
                    if (service != null)
                    {
                        if (serviceCount.ContainsKey(service))
                        {
                            serviceCount[service]++;
                        }
                        else
                        {
                            serviceCount[service] = 1;
                        }
                    }
                }
            }

            // อัปเดต label1, label2 และ label3
            label1.Text = $"{totalSum.ToString("C0", new CultureInfo("th-TH"))}"; // แสดงผลรวมใน label1
            label2.Text = $"{totalSumLuggageStorage.ToString("C0", new CultureInfo("th-TH"))}"; // แสดงผลรวม Luggage Storage
            label3.Text = $"{totalSumLuggageDelivery.ToString("C0", new CultureInfo("th-TH"))}"; // แสดงผลรวม Luggage Delivery

            // อัปเดต label5, label6 และ label7
            var locationResult = GetMaxItem(locationCount, "location");
            var locationTwoResult = GetMaxItem(locationTwoCount, "locationtwo");
            var serviceResult = GetMaxItem(serviceCount, "service");

            label5.Text = locationResult; // ตั้งค่าให้ label5
                                          // ตรวจสอบว่า locationTwoResult เป็น "not available" หรือไม่
            if (locationTwoResult != "not available")
            {
                label6.Text = locationTwoResult; // ตั้งค่าให้ label6
            }
            else
            {
                label6.Text = ""; // เคลียร์ label6 ถ้าเป็น "not available"
            }

            label7.Text = serviceResult; // ตั้งค่าให้ label7

        }

        private string GetMaxItem(Dictionary<string, int> countDict, string labelName)
        {
            if (countDict.Count > 0)
            {
                var maxEntry = countDict.OrderByDescending(x => x.Value).ToList();
                var maxCount = maxEntry.First().Value;
                var maxItems = maxEntry.Where(x => x.Value == maxCount).Select(x => x.Key).ToList();

                // ตรวจสอบว่ามีหลายค่าที่เกิดขึ้นมากที่สุดหรือไม่
                if (maxItems.Count > 1)
                {
                    MessageBox.Show($"แจ้งเตือน: มีหลายค่าใน {labelName} ที่มีจำนวนเท่ากัน: {string.Join(", ", maxItems)}");
                    return $"ไม่สามารถหาความนิยมได้"; // คืนค่าที่ไม่สามารถหาความนิยมได้
                }
                else
                {
                    return maxItems.First(); // คืนค่าที่เกิดขึ้นมากที่สุด
                }
            }
            else
            {
                return $"No data for {labelName}"; // คืนค่าเมื่อไม่มีข้อมูล
            }
        }

        //private void RetrieveAdditionalData()
        //{
        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();

        //            string queryLocation = @"
        //    SELECT location
        //    FROM abouthistory
        //    GROUP BY location
        //    ORDER BY COUNT(location) DESC
        //    LIMIT 1";

        //            string queryLocationTwo = @"
        //    SELECT locationtwo
        //    FROM abouthistory
        //    GROUP BY locationtwo
        //    ORDER BY COUNT(locationtwo) DESC
        //    LIMIT 1";

        //            string queryService = @"
        //    SELECT service
        //    FROM abouthistory
        //    GROUP BY service
        //    ORDER BY COUNT(service) DESC
        //    LIMIT 1";

        //            // Query สำหรับ location
        //            MySqlCommand commandLocation = new MySqlCommand(queryLocation, connection);
        //            var location = commandLocation.ExecuteScalar()?.ToString();
        //            label5.Text = location ?? "No data for location";

        //            // Query สำหรับ locationtwo
        //            MySqlCommand commandLocationTwo = new MySqlCommand(queryLocationTwo, connection);
        //            var locationTwo = commandLocationTwo.ExecuteScalar()?.ToString();
        //            label6.Text = locationTwo ?? "No data for locationtwo";

        //            // Query สำหรับ service
        //            MySqlCommand commandService = new MySqlCommand(queryService, connection);
        //            var service = commandService.ExecuteScalar()?.ToString();
        //            label7.Text = service ?? "No data for service";
        //        }
        //        catch (Exception ex)
        //        {
        //            // จัดการข้อผิดพลาดที่เกิดขึ้น
        //            MessageBox.Show($"Error: {ex.Message}");
        //        }
        //    }
        //}

        public void FillDGV(string valueToSearch)
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();

                    // การค้นหาในหลายคอลัมน์
                    string query = @"
                SELECT * 
                FROM abouthistory 
                WHERE CONCAT_WS(' ', namectm, tel, service, location, locationtwo, dropoff, pickup, realdropoff, realpickup, email, username, idenno, address) 
                LIKE @searchValue";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@searchValue", "%" + valueToSearch + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataLuggage.DataSource = table;

                    //// แจ้งเตือนว่าฟังก์ชันถูกเรียกและ DataGridView ได้รับการอัปเดต
                    //MessageBox.Show("DataGridView updated!", "Update Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (dataLuggage.Columns.Count > 0)
                    {
                        foreach (DataGridViewColumn column in dataLuggage.Columns)
                        {
                            if (column is DataGridViewImageColumn)
                            {
                                (column as DataGridViewImageColumn).ImageLayout = DataGridViewImageCellLayout.Stretch;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        private MySqlConnection connection = new MySqlConnection("datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;");

        

        public void ClearFields()
        {
            textBox3.Text = "";
            textBox2.Text = "";
          
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;

            // เคลียร์ข้อความใน Label
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            
            label5.Text = "";
            label6.Text = "";
            label7.Text = "";
        }

        

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            FillDGV(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่า ID ถูกกรอกหรือไม่
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Please enter an ID.");
                return;
            }

            try
            {
                // สร้างการเชื่อมต่อกับฐานข้อมูล
                using (MySqlConnection connection = databaseConnection())
                {
                    connection.Open();

                    // สร้างคำสั่ง SQL
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM abouthistory WHERE ID = @id", connection))
                    {
                        // เพิ่มพารามิเตอร์
                        cmd.Parameters.AddWithValue("@id", textBox3.Text);

                        // สร้าง MySqlDataAdapter และ DataTable
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            // ตรวจสอบว่ามีข้อมูลหรือไม่
                            if (table.Rows.Count <= 0)
                            {
                                MessageBox.Show("No Data Found");
                                ClearFields();
                            }
                            else
                            {
                                // ตั้งค่า DataSource ของ DataGridView
                                dataLuggage.DataSource = table;

                                // การตั้งค่า PictureBox1 และ PictureBox2
                                if (table.Rows[0][12] != DBNull.Value)
                                {
                                    byte[] img1 = (byte[])table.Rows[0][12];
                                    using (MemoryStream ms1 = new MemoryStream(img1))
                                    {
                                        try
                                        {
                                            pictureBox1.Image = Image.FromStream(ms1);
                                        }
                                        catch (ArgumentException)
                                        {
                                            pictureBox1.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                        }
                                    }
                                }
                                else
                                {
                                    pictureBox1.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                }

                                if (table.Rows[0][13] != DBNull.Value)
                                {
                                    byte[] img2 = (byte[])table.Rows[0][13];
                                    using (MemoryStream ms2 = new MemoryStream(img2))
                                    {
                                        try
                                        {
                                            pictureBox2.Image = Image.FromStream(ms2);
                                        }
                                        catch (ArgumentException)
                                        {
                                            pictureBox2.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                        }
                                    }
                                }
                                else
                                {
                                    pictureBox2.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                }

                                if (table.Rows[0][26] != DBNull.Value)
                                {
                                    byte[] img2 = (byte[])table.Rows[0][26];
                                    using (MemoryStream ms2 = new MemoryStream(img2))
                                    {
                                        try
                                        {
                                            pictureBox3.Image = Image.FromStream(ms2);
                                        }
                                        catch (ArgumentException)
                                        {
                                            pictureBox3.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                        }
                                    }
                                }
                                else
                                {
                                    pictureBox3.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                }

                                if (table.Rows[0][27] != DBNull.Value)
                                {
                                    byte[] img2 = (byte[])table.Rows[0][27];
                                    using (MemoryStream ms2 = new MemoryStream(img2))
                                    {
                                        try
                                        {
                                            pictureBox4.Image = Image.FromStream(ms2);
                                        }
                                        catch (ArgumentException)
                                        {
                                            pictureBox4.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                        }
                                    }
                                }
                                else
                                {
                                    pictureBox4.Image = null; // หรือใช้ภาพเริ่มต้นถ้าต้องการ
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }



        private void dataLuggage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ตรวจสอบว่าเซลล์ที่คลิกอยู่ภายในขอบเขตของ DataGridView หรือไม่
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // ดึงข้อมูล DataGridView จาก sender และตรวจสอบว่ามันเป็น DataGridView จริงๆ
                if (sender is DataGridView dgv)
                {
                    // ตรวจสอบว่าเซลล์ที่คลิกอยู่ในคอลัมน์ 'receipt' หรือไม่
                    if (dgv.Columns[e.ColumnIndex].Name == "receipt")
                    {
                        // ดึงค่าจากเซลล์ที่คลิก
                        var filePath = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as string;

                        // ตรวจสอบว่าไฟล์พาธไม่เป็น null หรือเป็นสตริงว่าง
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            // ตรวจสอบว่าไฟล์มีอยู่จริงหรือไม่
                            if (System.IO.File.Exists(filePath))
                            {
                                try
                                {
                                    // เปิดไฟล์ด้วยโปรแกรมดู PDF ที่ตั้งค่าเป็นค่าเริ่มต้น
                                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                                    {
                                        FileName = filePath,
                                        UseShellExecute = true
                                    });
                                }
                                catch (Exception ex)
                                {
                                    // จัดการข้อผิดพลาดที่อาจเกิดขึ้น
                                    MessageBox.Show($"ไม่สามารถเปิดไฟล์: {filePath}\nข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                // แสดงข้อความเตือนเมื่อไฟล์ไม่พบ
                                MessageBox.Show($"ไม่พบไฟล์: {filePath}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("ค่าของเซลล์ไม่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void RefreshData()
        {
            // สร้างการเชื่อมต่อฐานข้อมูล
            using (MySqlConnection conn = databaseConnection())
            {
                // สร้าง MySqlDataAdapter สำหรับดึงข้อมูล
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM abouthistory", conn);
                DataTable dataTable = new DataTable();

                // เติมข้อมูลจากฐานข้อมูล
                adapter.Fill(dataTable);

                // ตั้งค่า DataSource สำหรับ DataGridView
                dataLuggage.DataSource = dataTable;
            }

        }


        private void button4_Click(object sender, EventArgs e)
        {
            RefreshData(); // เรียกใช้ฟังก์ชัน RefreshData เพื่อรีเฟรชข้อมูล
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // อัปเดต DataGridView (สมมติว่ามีฟังก์ชันที่ใช้ในการโหลดข้อมูลใหม่)
            FillDGV(""); // หรือฟังก์ชันที่คุณใช้ในการโหลดข้อมูล

         
            CheckData();
            // คำนวณผลรวม
            CalculateTotalSum();

        }
    }

}
