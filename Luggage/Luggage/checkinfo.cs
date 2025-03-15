using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luggage
{
    public partial class checkinfo : Form
    {

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        public checkinfo()
        {
            InitializeComponent();
        }

        private string _cusid; //รับค่าcusid จากหน้าtrackingมา
        public string cusid
        {
            get { return _cusid; }
            set { _cusid = value; }
            
        }


        //ฟังก์ชันปุ่มบ้านเมื่อกดก็จะไปหน้า...
        private void button2_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }


        // ฟังก์ชันเมื่อโหลดหน้าcheckinfoขึ้น
        private void checkinfo_Load(object sender, EventArgs e)
        {
            // สร้าง DataTable สำหรับเก็บข้อมูล
            DataTable dataTable = new DataTable();

            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;"; //เปิดการเชื่อมต่อกับฐานข้อมูล

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // ดึงข้อมูลจากตาราง luggageinfo ทั้งหมดจาก cusid ที่กรอก มาแสดงที่ datagridview
                    string selectQuery = "SELECT * FROM luggageinfo WHERE cusid = @cusid";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@cusid", _cusid);
                        using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(selectCommand))
                        {
                            // เติมข้อมูลลงใน DataTable
                            dataAdapter.Fill(dataTable);
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }


        }

        //ฟังก์ชันเมื่อคลิกที่แถวในคอลัมน์ receipt ใน datagridview แล้วจะเปิดไฟล์ใบเสร็จ pdf ขึ้นมา

        private void dataLuggage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ตรวจสอบว่าแถวที่คลิกอยู่ภายในขอบเขตของ DataGridView มั้ย
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // ดึงข้อมูล DataGridView จาก sender และตรวจสอบว่ามันเป็น DataGridView จริงๆ
                if (sender is DataGridView dgv)
                {
                    // ตรวจสอบว่าแถวที่คลิกอยู่ในคอลัมน์ 'receipt' มั้ย
                    if (dgv.Columns[e.ColumnIndex].Name == "receipt")
                    {
                        // ดึงค่าจากแถวที่คลิก
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


        

        

        
    }
}
