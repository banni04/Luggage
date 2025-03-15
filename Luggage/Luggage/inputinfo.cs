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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


/* C:\Users\MikiBunnie\source\repos */


namespace Luggage
{
    public partial class inputinfo : Form
    {
        private int numberreg = 0;
        private int numberspec = 0; //สร้างตัวแปรไว้ใช้คำนวณ
        private int daysDifference;

        private MySqlConnection databaseConnection() //เชื่อมต่อฐานข้อมูล
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

       

        private string _username;
        public string username_ii //เก็บค่าและส่งค่า
        {
            get { return _username; }
            set { _username = value; }
            //usename1
        }


        private string _id;
        public string id_ii
        {
            get { return _id; }
            set { _id = value; }
            //username1
        }




        public inputinfo()
        {
            InitializeComponent();
            // ตั้งค่า MinDate ของ DateTimePicker
            dateTimePicker1.MinDate = DateTime.Today; //ตั้งค่าDateTimePicker กำหนด MinDate ให้เป็นวันที่ปัจจุบัน และเชื่อมโยงเหตุการณ์ ValueChanged กับฟังก์ชันที่ชื่อว่า DateTimePickers_ValueChanged เพื่อให้สามารถคำนวณจำนวนวันและเวลาได้
            // ตั้งค่า MinDate ของ DateTimePicker
            dateTimePicker2.MinDate = DateTime.Today;

            // เชื่อมโยงเหตุการณ์ ValueChanged ของ DateTimePicker กับฟังก์ชันที่คำนวณจำนวนวัน
            dateTimePicker1.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);
            dateTimePicker2.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);
            dateTimePicker3.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);
            dateTimePicker4.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);


            // ตั้งค่าเริ่มต้นของ daysDifference
            daysDifference = 0;
            textBoxDaysDifference.Text = daysDifference.ToString();

            

            // อัปเดตข้อความใน label summarize เมื่อโหลดฟอร์ม
            UpdateSummary();
        }

        //ฟังก์ชัน...คำนวณจำนวนวันและเวลา
        private void DateTimePickers_ValueChanged(object sender, EventArgs e)
        {

            // คำนวณจำนวนวันระหว่างวันที่ใน DateTimePicker
            DateTime startDate = dateTimePicker1.Value.Date;  // ใช้วันที่
            DateTime endDate = dateTimePicker2.Value.Date;    // ใช้วันที่

            // ดึงเวลาออกจาก dateTimePicker3 และ dateTimePicker4
            DateTime dateTime1 = dateTimePicker3.Value; // ได้ค่า DateTime
            DateTime dateTime2 = dateTimePicker4.Value; // ได้ค่า DateTime

            // ตัดวินาทีออก
            TimeSpan time1 = new TimeSpan(dateTime1.Hour, dateTime1.Minute, 0);
            TimeSpan time2 = new TimeSpan(dateTime2.Hour, dateTime2.Minute, 0);

            // ตรวจสอบเวลาใน dateTimePicker4 ว่าข้ามวันหรือไม่
            if (time2 < TimeSpan.FromHours(24))
            {
                // เพิ่มวันให้กับ endDate ถ้าเวลาใน dateTimePicker4 น้อยกว่า 00:00
                endDate = endDate.AddDays(1);
            }

            // คำนวณความแตกต่างระหว่างวันที่
            TimeSpan dateDifference = endDate - startDate;

            // เช็คเวลาใน dateTimePicker3 กับ dateTimePicker4
            if (time1 > time2)
            {
                // เวลาใน dateTimePicker3 มากกว่าเวลาใน dateTimePicker4
                daysDifference = dateDifference.Days - 1;
            }
            else
            {
                // เวลาใน dateTimePicker3 น้อยกว่าหรือเท่ากับเวลาใน dateTimePicker4
                daysDifference = dateDifference.Days;
            }

            // ตรวจสอบกรณี daysDifference < 0
            if (daysDifference < 0)
            {
                daysDifference = 0; // ความแตกต่างวันไม่ควรเป็นลบ
            }

            // อัปเดตค่าที่ label
            textBoxDaysDifference.Text = daysDifference.ToString();

            //if (daysDifference > 1)
            //{
            //    // กำหนดเวลาใน dateTimePicker4 ให้เท่ากับ dateTimePicker3
            //    dateTimePicker4.Value = dateTimePicker3.Value;
            //}

            //// เพิ่มการตรวจสอบความแตกต่างของเวลา
            //TimeSpan timeDifference = time2 - time1;

            //if (timeDifference.TotalMinutes < 60)
            //{


            //    MessageBox.Show("ความแตกต่างระหว่างเวลาใน DateTimePicker ต้องมากกว่า 60 นาที", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);



            //    // กำหนดเวลาใน dateTimePicker4 ให้อยู่หลัง dateTimePicker3 60 นาที
            //    DateTime newDateTimePicker4Value = dateTimePicker3.Value.AddMinutes(60);
            //    dateTimePicker4.Value = newDateTimePicker4Value;
            //}

            // ตรวจสอบความแตกต่างของเวลา
            TimeSpan timeDifference = time2 - time1;

            //// เพิ่ม MessageBox เพื่อดีบักค่าต่าง ๆ
            //MessageBox.Show($"time1: {time1}\ntime2: {time2}\ntimeDifference: {timeDifference}", "Debug Info");

            // ตรวจสอบว่า dateTimePicker1 และ dateTimePicker2 มีค่าเท่ากันหรือไม่
            if (dateTimePicker1.Value == dateTimePicker2.Value)
            {
                if (timeDifference.TotalHours < 1)
                {
                    MessageBox.Show("ความแตกต่างระหว่างเวลาต้องมากกว่า 60 นาที", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // กำหนดเวลาใน dateTimePicker4 ให้อยู่หลัง dateTimePicker3 60 นาที
                    DateTime newDateTimePicker4Value = dateTimePicker3.Value.AddMinutes(60);
                    dateTimePicker4.Value = newDateTimePicker4Value;
                }
            }            // ตรวจสอบว่าค่าของ dateTimePicker2 ไม่ต่ำกว่าค่าของ dateTimePicker1
            else if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                // แจ้งเตือนผู้ใช้
                MessageBox.Show("วันที่ pickup ต้องไม่น้อยกว่าวันที่ dropoff",
                                "วันที่ไม่ถูกต้อง",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                // ตั้งค่าของ dateTimePicker2 ให้ตรงกับ dateTimePicker1
                dateTimePicker2.Value = dateTimePicker1.Value;
            }

        }


        private void UpdateLabels()
        {
            string locationName = comboBox1.Text;
            string datedropoffAdded = dateTimePicker1.Value.ToShortDateString();
            string datepickupAdded = dateTimePicker2.Value.ToShortDateString();
            string timeDropoff = dateTimePicker3.Value.ToString("HH:mm");
            string timePickup = dateTimePicker4.Value.ToString("HH:mm");

            // แสดงข้อมูลใน TextBoxes
            label1.Text = locationName;
            label2.Text = datedropoffAdded + " " +  timeDropoff + ":00";
            label3.Text = datepickupAdded  + " " + timePickup + ":00";

        }

        private void CheckLabels()
        {
            bool regularHasValue = HasValue(regular);
            bool specialHasValue = HasValue(special);
            if (regularHasValue == false && specialHasValue == false)
            {
                minusregular.Enabled = false;
                minusspecial.Enabled = false;
            }
            else if (regularHasValue == true && specialHasValue == false)
            {
                minusregular.Enabled = true;
                minusspecial.Enabled = false;
            }
            else if (regularHasValue == false && specialHasValue == true)
            {
                minusregular.Enabled = false;
                minusspecial.Enabled = true;
            }
            else
            {
                minusregular.Enabled = true;
                minusspecial.Enabled = true;
            }
        }

        private bool HasValue(System.Windows.Forms.Label label)
        {
            // Check if label is not null and has non-empty text
            return (label != null && label.Text != "0");
        }

        private void CalculatePrice()
        {
            // ราคา
            const int regularPrice = 100;
            const int specialPrice = 200;

            // คำนวณราคา
            int totalRegularPrice = numberreg * regularPrice;
            int totalSpecialPrice = numberspec * specialPrice;

            // ผลรวมราคา
            int totalPrice = totalRegularPrice + totalSpecialPrice;

            // แสดงราคาใน TextBox
            summarize.Text = totalPrice.ToString("C"); // Format to currency
        }

        


        private void button5_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        

        private void inputinfo_Load(object sender, EventArgs e)
        {
            CheckLabels();

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
                string username = username_ii;

                // คำนวณความกว้างของข้อความ
                SizeF textSize = e2.Graphics.MeasureString(username, labelUsername.Font);

                // คำนวณตำแหน่งข้อความให้เริ่มจากขวา
                int textX = labelUsername.Width - labelUsername.Padding.Right - (int)textSize.Width;
                int textY = labelUsername.Padding.Top;

                // วาดข้อความเอง
                e2.Graphics.DrawString(username, labelUsername.Font, new SolidBrush(labelUsername.ForeColor), new PointF(textX, textY));
            };


            //// แสดงชื่อผู้ใช้ใน TextBox เมื่อฟอร์มโหลด
            //labelUsername.Text = username_ii;



        }


        int maxid = 0;
        private void button3_Click(object sender, EventArgs e)  //ปุ่ม confirm
        {

            
            string username = username_ii;

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT MAX(ID) FROM process";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    

                    try
                    {
                        conn.Open();
                        maxid = (int)cmd.ExecuteScalar();
                        MessageBox.Show("Data inserted successfully into process table");
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


          


            string service = "luggagestorage";
            string servicelocation = "not available";


            double tt = Convert.ToDouble(totalPrice);
            string datetextbox1 = label2.Text;
            string datetextbox2 = label3.Text;

            DateTime dropoff = DateTime.ParseExact(datetextbox1, "d/M/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime pickup = DateTime.ParseExact(datetextbox2, "d/M/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "INSERT INTO process (username, service, location, locationtwo, datetimedropoff, datetimepickup, qttregular, qttspecial, duration, total, processid) " +
                               "VALUES (@username, @service, @location, @locationtwo, @dtdropoff, @dtpickup, @qttregular, @qttspecial, @duration, @total, @processid)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@service", service);
                    cmd.Parameters.AddWithValue("@location", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@locationtwo", servicelocation);
                    cmd.Parameters.AddWithValue("@dtdropoff", dropoff);
                    cmd.Parameters.AddWithValue("@dtpickup", pickup);
                    cmd.Parameters.AddWithValue("@qttregular", regular.Text);
                    cmd.Parameters.AddWithValue("@qttspecial", special.Text);
                    cmd.Parameters.AddWithValue("@duration", textBoxDaysDifference.Text);
                    cmd.Parameters.AddWithValue("@total", tt);
                    cmd.Parameters.AddWithValue("@processid", maxid + 1);

                    try
                    {
                        conn.Open(); // เปิดการเชื่อมต่อ
                        cmd.ExecuteNonQuery(); // เรียกใช้ ExecuteNonQuery เพื่อแทรกข้อมูล
                       
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



            // ตรวจสอบว่ามีค่าใน textBoxUsername
            string username1 = username;

            // เปิดฟอร์ม inputinfo3 และส่ง username
            inputinfo3 inputinfo3Form = new inputinfo3();
            inputinfo3Form.username_ii3 = username1;
            inputinfo3Form.processid = GetMaxProcessId(); // ส่งค่า ProcessId ล่าสุด
            inputinfo3Form.id = maxid+1;
            inputinfo3Form.Show();
            this.Hide(); // ซ่อนฟอร์ม inputinfo
        }

        private int GetMaxProcessId()
        {
            int maxId = 0;
            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT MAX(ID) FROM process";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        maxId = Convert.ToInt32(cmd.ExecuteScalar());
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
            return maxId;
        }

        private void plusregular_Click(object sender, EventArgs e)
        {
            numberreg++;
            regular.Text = numberreg.ToString();
            CheckLabels();
            CalculatePrice(); // คำนวณราคาใหม่
            UpdateSummary(); // อัปเดตข้อความ
        }

        private void minusregular_Click(object sender, EventArgs e)
        {
            numberreg--;
            regular.Text = numberreg.ToString();
            CheckLabels();
            CalculatePrice(); // คำนวณราคาใหม่
            UpdateSummary(); // อัปเดตข้อความ
        }

        private void plusspecial_Click(object sender, EventArgs e)
        {
            numberspec++;
            special.Text = numberspec.ToString();
            CheckLabels();
            CalculatePrice(); // คำนวณราคาใหม่
            UpdateSummary(); // อัปเดตข้อความ
        }

        private void minusspecial_Click(object sender, EventArgs e)
        {
            numberspec--;
            special.Text = numberspec.ToString();
            CheckLabels();
            CalculatePrice(); // คำนวณราคาใหม่
            UpdateSummary(); // อัปเดตข้อความ
        }

        

        private void textBoxDaysDifference_TextChanged(object sender, EventArgs e)
        {
            UpdateSummary();
        }

        private void textBoxTotalPrice_TextChanged(object sender, EventArgs e)
        {
            UpdateSummary();
        }

        int totalPrice = 0;

        

        private void UpdateSummary()
        {
            // รับค่าจาก Label และ TextBox
            if (!int.TryParse(regular.Text, out int regularCount))
            {
                regularCount = 0; // กำหนดค่าเริ่มต้นถ้าไม่สามารถแปลงได้
            }

            if (!int.TryParse(special.Text, out int specialCount))
            {
                specialCount = 0; // กำหนดค่าเริ่มต้นถ้าไม่สามารถแปลงได้
            }

            if (!int.TryParse(textBoxDaysDifference.Text, out int daysDifference))
            {
                daysDifference = 0; // กำหนดค่าเริ่มต้นถ้าไม่สามารถแปลงได้
            }

            // สมมุติว่าราคา regular และ special มีค่าคงที่
            const int regularPricePerDay = 100; // ราคา per day ของ regular
            const int specialPricePerDay = 200; // ราคา per day ของ special

            // คำนวณราคาสำหรับ regular และ special
            int totalRegularPrice = regularCount * regularPricePerDay * daysDifference;
            int totalSpecialPrice = specialCount * specialPricePerDay * daysDifference;

            // คำนวณราคาทั้งหมด
            totalPrice = totalRegularPrice + totalSpecialPrice;


            // สร้างข้อความสำหรับ Label summarize
            string summaryMessage = $"{regularCount} Regular Luggage x {daysDifference} days = {totalRegularPrice.ToString("C0", new CultureInfo("th-TH"))}";
            summarize.Text = summaryMessage;

            // สร้างข้อความสำหรับ Label summarize2
            string summaryMessage2 = $"{specialCount} Special Luggage x {daysDifference} days = {totalSpecialPrice.ToString("C0", new CultureInfo("th-TH"))}";
            summarize2.Text = summaryMessage2;

            // แสดงผลรวมราคาใน Label labelTotalPrice
            labelTotalPrice.Text = totalPrice.ToString("C0", new CultureInfo("th-TH")); // Format to currency
        }

       


        //private void ChangeLabelText()
        //{
        //    regular.Text = "UpdatedValue"; // ตัวอย่างการเปลี่ยนแปลงค่า Label
        //    UpdateSummary(); // เรียกใช้งานฟังก์ชันเพื่ออัปเดต TextBox
        //}
    }
}