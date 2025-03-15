using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using PdfSharp.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace Luggage
{
    public partial class tracking : Form
    {

        private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";

        public tracking()
        {
            InitializeComponent();
        }

        //ฟังก์ชันเมื่อกดปุ่มก็จะทำการ...
        private void button1_Click(object sender, EventArgs e)
        {
            string cusid = textBox1.Text; // รับค่าจากtextbox มาตรวจสอบ ถ้าว่างก็จะขึ้นแจ้งเตือน...

            if (string.IsNullOrEmpty(cusid))
            {
                MessageBox.Show("กรุณากรอก cusid");
                return;
            }

            // ตรวจสอบว่า cusid มีอยู่ในฐานข้อมูลก่อนเปิดฟอร์ม checkinfo ขึ้น
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;"; //เชื่อมต่อกับฐานข้อมูล
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM luggageinfo WHERE cusid = @cusid"; //นับจำนวนแถวในตาราง luggageinfo ซึ่งมีเงื่อนไขว่าคอลัมน์ cusid ต้องตรงกับค่าที่กรอก 
                    using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@cusid", cusid);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count == 0) //ถ้าจำนวนแถวในตารางเป็น0 จะขึ้นแจ้งเตือน...
                        {
                            MessageBox.Show("กรุณากรอก cusid ที่ถูกต้อง"); 
                            return;
                        }

                        // เปิดฟอร์ม checkinfo และส่งค่า cusid
                        checkinfo checkinfo = new checkinfo();
                        checkinfo.cusid = cusid;
                        checkinfo.Show();
                        this.Hide(); // ซ่อนฟอร์ม tracking
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
                }
            }

        }

        //ฟังก์ชันปุ่มบ้านเมื่อกดก็จะไปหน้า...
        private void button2_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }
    }
}
