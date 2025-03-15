using MySql.Data.MySqlClient;
using PdfSharp.Charting;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using MySql.Data.MySqlClient;


namespace Luggage
{
    public partial class showinfo : Form
    {
        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
        private MySqlConnection connection = new MySqlConnection("datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;");

        // ฟังก์ชันที่ใช้สร้าง MySqlConnection จาก connectionString
        private MySqlConnection CreateConnection()
        {
            return new MySqlConnection(connectionString);
        }


        public showinfo()
        {
            InitializeComponent();


            // ใช้ CreateConnection แทนการกำหนดค่าให้ connectionString
            MySqlConnection connection = CreateConnection();
            LoadData();

        }

        private MySqlConnection databaseConnection()
        {
            return new MySqlConnection(connectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            homeadmin homeadmin = new homeadmin();
            homeadmin.Show();
            this.Hide();
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    string query = @"
                    SELECT *
                    FROM luggageinfo
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
                        if (row["dropoff"] is DateTime firstDateValue)
                        {
                            int year = firstDateValue.Year;
                            if (year >= 2560) // แปลงเฉพาะปีพุทธศักราช
                            {
                                firstDateValue = firstDateValue.AddYears(-543); // แปลงเป็นปีคริสต์ศักราช
                            }
                            row["dropoff"] = firstDateValue; // อัพเดทค่าใน DataRow
                        }

                        // แปลงคอลัมน์วันที่ที่สอง
                        if (row["pickup"] is DateTime secondDateValue)
                        {
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
                        row["Pic"] = ConvertBlobToImage(row["image"] as byte[]);
                        row["Pictwo"] = ConvertBlobToImage(row["imagetwo"] as byte[]);
                    }

                    // ตั้งค่าข้อมูล DataGridView
                    dataLuggage.DataSource = dataTable;

                    //// เพิ่มคอลัมน์สำหรับการแสดงภาพ
                    //EnsureImageColumns();

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

        

       
        private MySqlDataAdapter adapter;
        private MySqlCommandBuilder commandBuilder;
        private DataSet dataSet;

        private object updateLock = new object();
        private bool isUpdating = false;  // ตัวแปรเพื่อป้องกันการเรียกซ้ำ

        private void Form7_Load(object sender, EventArgs e)
        {
            //// กำหนดเหตุการณ์ที่ต้องจัดการ
       
            //dataLuggage.CurrentCellDirtyStateChanged += new EventHandler(dataLuggage_CurrentCellDirtyStateChanged);
   

            CreateConnection(); // ตรวจสอบว่าฟังก์ชันนี้มีอยู่และทำงานได้
            LoadData();         // ตรวจสอบว่าฟังก์ชันนี้มีอยู่และทำงานได้
            /*Updatedata(); */      // โหลดข้อมูลจากฐานข้อมูล
            FillDGV("");        // ตรวจสอบว่าฟังก์ชันนี้มีอยู่และทำงานได้

        }
        



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
                FROM luggageinfo 
                WHERE CONCAT_WS(' ', namectm, tel, service, location, locationtwo, dropoff, pickup, realdropoff, realpickup, email, username, idenno, address) 
                LIKE @searchValue";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@searchValue", "%" + valueToSearch + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataLuggage.DataSource = table;

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

                



                // ตรวจสอบว่ามีข้อมูลในเซลล์สำหรับข้อความหรือไม่
                var textCellValue = dataLuggage.CurrentRow.Cells[0].Value;
                if (textCellValue == DBNull.Value || textCellValue == null)
                {
                    textBox1.Text = string.Empty;
                }
                else
                {
                    textBox1.Text = textCellValue.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}");
            }
        }




        //public void ExecMyQuery(MySqlCommand cmd, string myMsg)
        //{
        //    try
        //    {
        //        connection.Open();
        //        if (cmd.ExecuteNonQuery() == 1)
        //        {
        //            MessageBox.Show(myMsg);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Query Not Executed");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}");
        //    }
        //    finally
        //    {
        //        connection.Close();
        //        FillDGV(""); // เรียก FillDGV() ที่นี่
        //    }
        //}




        //private void button5_Click(object sender, EventArgs e)
        //{
        //    // สร้างคำสั่ง SQL
        //    MySqlCommand cmd = new MySqlCommand("DELETE FROM luggageinfo WHERE ID = @id", connection);

        //    // เพิ่มพารามิเตอร์
            
        //    cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = textBox1.Text; // ใช้ TextBox1 สำหรับ ID

        //    // เรียกใช้งาน ExecMyQuery เพื่อดำเนินการคำสั่ง
        //    ExecMyQuery(cmd, "Data Deleted");

        //    ClearFields();
        //}

        

        //private void UpdateDatabase(int id, string columnName, object newValue)
        //{
        //    string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";

        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            string sql = $"UPDATE luggageinfo SET {columnName} = @value WHERE id = @id";
        //            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@value", newValue);
        //                cmd.Parameters.AddWithValue("@id", id);

        //                conn.Open();
        //                int rows = cmd.ExecuteNonQuery();

        //                if (rows > 0)
        //                {
        //                    Console.WriteLine($"Successfully updated row with ID {id}");
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"No rows affected for ID {id}");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
        //        }
        //    }
        //}


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            FillDGV(textBox3.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // ตรวจสอบว่า ID ถูกกรอกหรือไม่
            if (string.IsNullOrWhiteSpace(textBox1.Text))
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
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM luggageinfo WHERE ID = @id", connection))
                    {
                        // เพิ่มพารามิเตอร์
                        cmd.Parameters.AddWithValue("@id", textBox1.Text);

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


        private void button7_Click(object sender, EventArgs e)
        {
            ClearFields();
        }


        public void ClearFields()
        {
            textBox3.Text = "";
         
            textBox1.Text = "";
            pictureBox1.Image = null;
           
            pictureBox3.Image = null;
          
        }

       

        //private void dataLuggage_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        //{
        //    // ตรวจสอบว่าเซลล์มีการเปลี่ยนแปลง
        //    if (dataLuggage.IsCurrentCellDirty)
        //    {
        //        // บังคับให้เซลล์ที่กำลังใช้งานบันทึกการเปลี่ยนแปลง
        //        dataLuggage.CommitEdit(DataGridViewDataErrorContexts.Commit);
        //    }
        //}

        //private void button9_Click(object sender, EventArgs e)
        //{
        //    // บันทึกการเปลี่ยนแปลงที่มีใน DataGridView
        //    SaveChanges();
        //}

        //private void SaveChanges()
        //{
        //    lock (updateLock)
        //    {
        //        if (isUpdating) return;  // ข้ามการทำงานหากกำลังอัพเดต
        //        isUpdating = true;

        //        bool anyUpdate = false;

        //        foreach (DataGridViewRow row in dataLuggage.Rows)
        //        {
        //            if (row.IsNewRow) continue;

        //            int id = Convert.ToInt32(row.Cells["id"].Value);
        //            bool hasError = false;

        //            foreach (DataGridViewCell cell in row.Cells)
        //            {
        //                if (cell.OwningColumn.Name != "id" && cell.Value != null)
        //                {
        //                    string columnName = cell.OwningColumn.Name;
        //                    object newValue = cell.Value;

        //                    // ตรวจสอบว่าค่าที่แก้ไขถูกต้องก่อนทำการอัพเดต
        //                    if (cell.Tag != null && cell.Tag.ToString() == "Error")
        //                    {
        //                        MessageBox.Show($"ค่าที่แก้ไขในคอลัมน์ {columnName} มีข้อผิดพลาด", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        hasError = true;
        //                        break; // ออกจาก loop ถ้ามีข้อผิดพลาด
        //                    }

        //                    // อัพเดตฐานข้อมูล
        //                    UpdateDatabase(id, columnName, newValue);
        //                    anyUpdate = true; // แจ้งว่ามีการอัพเดตเกิดขึ้น
        //                }
        //            }

        //            if (hasError) break; // ถ้ามีข้อผิดพลาดให้ข้ามไปที่แถวถัดไป
        //        }

        //        isUpdating = false;

        //        // แสดงการแจ้งเตือนหลังจากการอัพเดตเสร็จสิ้น
        //        if (anyUpdate)
        //        {
        //            MessageBox.Show("ข้อมูลทั้งหมดได้รับการอัพเดตเรียบร้อยแล้ว", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            MessageBox.Show("ไม่มีการเปลี่ยนแปลงข้อมูล", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //}

        //private void dataLuggage_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        //{
        //    //// ตรวจสอบคอลัมน์ namectm
        //    //if (dataLuggage.Columns[e.ColumnIndex].Name == "namectm")
        //    //{
        //    //    string inputValue = e.FormattedValue.ToString();
        //    //    if (!IsEnglishAlphabet(inputValue))
        //    //    {
        //    //        MessageBox.Show("กรุณากรอกชื่อให้เป็นตัวอักษรภาษาอังกฤษเท่านั้น", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //        e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
        //    //        dataLuggage[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
        //    //    }
        //    //    else
        //    //    {
        //    //        dataLuggage[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
        //    //    }
        //    //}

        //    // ตรวจสอบคอลัมน์ tel
        //    if (dataLuggage.Columns[e.ColumnIndex].Name == "tel")
        //    {
        //        string inputValue = e.FormattedValue.ToString();
        //        if (!IsValidPhoneNumber(inputValue))
        //        {
        //            MessageBox.Show("หมายเลขโทรศัพท์ต้องเป็นตัวเลข 10 หลัก", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
        //            dataLuggage[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
        //        }
        //        else
        //        {
        //            dataLuggage[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
        //        }
        //    }

        //    // ตรวจสอบคอลัมน์ email
        //    if (dataLuggage.Columns[e.ColumnIndex].Name == "email")
        //    {
        //        string inputValue = e.FormattedValue.ToString();
        //        if (!IsValidEmail(inputValue))
        //        {
        //            MessageBox.Show("กรุณากรอกอีเมลที่ถูกต้อง", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
        //            dataLuggage[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
        //        }
        //        else
        //        {
        //            dataLuggage[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
        //        }
        //    }

        //    // ตรวจสอบคอลัมน์ idenno
        //    if (dataLuggage.Columns[e.ColumnIndex].Name == "idenno")
        //    {
        //        string inputValue = e.FormattedValue.ToString();
        //        if (!IsValidIdenno(inputValue))
        //        {
        //            MessageBox.Show("กรุณากรอก idenno ที่ถูกต้อง (ตัวเลข 13 หลัก)", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
        //            dataLuggage[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
        //        }
        //        else
        //        {
        //            dataLuggage[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
        //        }
        //    }

        //    //// ตรวจสอบคอลัมน์ caddress
        //    //if (dataLuggage.Columns[e.ColumnIndex].Name == "address")
        //    //{
        //    //    string inputValue = e.FormattedValue.ToString();
        //    //    if (ContainsThaiCharacters(inputValue))
        //    //    {
        //    //        MessageBox.Show("กรุณากรอก address โดยไม่ใช้ภาษาไทย", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    //        e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
        //    //        dataLuggage[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
        //    //    }
        //    //    else
        //    //    {
        //    //        dataLuggage[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
        //    //    }
        //    //}
        //}

        //// ฟังก์ชันตรวจสอบตัวอักษรภาษาอังกฤษ
        //private bool IsEnglishAlphabet(string input)
        //{
        //    return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]+$");
        //}

        //// ฟังก์ชันตรวจสอบหมายเลขโทรศัพท์
        //private bool IsValidPhoneNumber(string input)
        //{
        //    return System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d{10}$");
        //}

        //// ฟังก์ชันตรวจสอบอีเมล
        //private bool IsValidEmail(string input)
        //{
        //    return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        //}

        //// ฟังก์ชันตรวจสอบ idenno
        //private bool IsValidIdenno(string input)
        //{
        //    return long.TryParse(input, out long result) && input.Length == 13;
        //}

        //// ฟังก์ชันตรวจสอบว่า address มีอักขระภาษาไทยหรือไม่
        //private bool ContainsThaiCharacters(string input)
        //{
        //    return System.Text.RegularExpressions.Regex.IsMatch(input, @"[\u0E00-\u0E7F]");
        //}

        private void RefreshData()
        {
            // สร้างการเชื่อมต่อฐานข้อมูล
            using (MySqlConnection conn = databaseConnection())
            {
                // สร้าง MySqlDataAdapter สำหรับดึงข้อมูล
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM luggageinfo", conn);
                DataTable dataTable = new DataTable();

                // เติมข้อมูลจากฐานข้อมูล
                adapter.Fill(dataTable);

                // ตั้งค่า DataSource สำหรับ DataGridView
                dataLuggage.DataSource = dataTable;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshData(); // เรียกใช้ฟังก์ชัน RefreshData เพื่อรีเฟรชข้อมูล
        }

        
    }
}


