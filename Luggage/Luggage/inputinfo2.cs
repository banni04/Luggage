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
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.DirectoryServices.ActiveDirectory;

namespace Luggage
{
    public partial class inputinfo2 : Form
    {
        private int numberreg = 0;
        private int numberspec = 0;
        private int daysDifference;


        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        // สร้าง dictionary เพื่อเก็บข้อมูลห้างสรรพสินค้าตามประเภท
        private Dictionary<string, List<string>> storeOptions = new Dictionary<string, List<string>>()
        {

            { "Airport", new List<string> { "Suvarnnabhumi Airport (BKK)", "Don Mueang Airport (DMK)" } },
            { "Shopping Mall", new List<string> { "MBK Center", "Terminal 21 Bangkok", "Central World", "Siam Paragon", "ICONSIAM" } }
        };


        public inputinfo2()
        {
            InitializeComponent();


            // ตั้งค่า MinDate ของ DateTimePicker
            dateTimePicker1.MinDate = DateTime.Today;
            // ตั้งค่า MinDate ของ DateTimePicker
            dateTimePicker3.MinDate = DateTime.Today;
            //// กำหนดค่าที่จะเลือกใน ComboBox2
            //comboBox2.DataSource = null;
            //comboBox2.Items.Clear();

            // ผูกเหตุการณ์ SelectedIndexChanged ของ ComboBox1
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            //// ซ่อน ComboBox2 และ TextBox1 เริ่มต้น
            //comboBox2.Visible = false;
            //textBox1.Visible = false;

            

            // ผูกเหตุการณ์ SelectedIndexChanged ของ ComboBox3
            comboBox3.SelectedIndexChanged += ComboBox3_SelectedIndexChanged;



            //// ซ่อน ComboBox4 และ TextBox2 เริ่มต้น
            //comboBox4.Visible = false;
            //textBox2.Visible = false;

            // เชื่อมโยงเหตุการณ์ ValueChanged ของ DateTimePicker กับฟังก์ชันที่คำนวณจำนวนวัน
            dateTimePicker1.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);
            dateTimePicker2.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);
            dateTimePicker3.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);
            dateTimePicker4.ValueChanged += new EventHandler(DateTimePickers_ValueChanged);

            // ตั้งค่าเริ่มต้นของ daysDifference
            daysDifference = 0;
            label11.Text = daysDifference.ToString();




            // อัปเดตข้อความใน TextBox summarize เมื่อโหลดฟอร์ม
            UpdateSummary();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = comboBox1.SelectedItem?.ToString();

            if (selectedValue == "Hotel" || selectedValue == "Home/Condo/Airbnb")
            {
                // ซ่อน ComboBox2 และแสดง TextBox1
                comboBox2.Visible = false;
                textBox1.Visible = true;
                label2.Visible = true;  // ทำให้ label2 มองเห็นได้เมื่อเลือก "Hotel" หรือ "Home/Condo/Airbnb"
                label4.Visible = false;
            }
            else
            {
                // ซ่อน TextBox1 และแสดง ComboBox2
                textBox1.Visible = false;
                comboBox2.Visible = true;


                label4.Visible = true;
                // ซ่อน label2 เมื่อเลือก "Airport" หรือ "Shopping Mall"
                label2.Visible = false;

                // ตั้งค่าของ ComboBox2 ตามการเลือกที่เหลือ
                comboBox2.Items.Clear();
                switch (selectedValue)
                {
                    case "Airport":
                        comboBox2.Items.AddRange(new string[] { "Suvarnnabhumi Airport (BKK)", "Don Mueang Airport (DMK)" });
                        break;
                    case "Shopping Mall":
                        comboBox2.Items.AddRange(new string[] { "MBK Center", "Terminal 21 Bangkok", "Central World", "Siam Paragon", "ICONSIAM" });
                        break;
                }
            }
            // อัพเดท Labels
            UpdateTextBoxes();


        }


        private void ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedValue = comboBox3.SelectedItem?.ToString();

            if (selectedValue == "Hotel" || selectedValue == "Home/Condo/Airbnb")
            {
                // ซ่อน ComboBox4 และแสดง TextBox2
                comboBox4.Visible = false;
                textBox2.Visible = true;
                label3.Visible = true;  // ทำให้ label3 มองเห็นได้เมื่อเลือก "Hotel" หรือ "Home/Condo/Airbnb"
                label6.Visible = false;
            }
            else
            {
                // ซ่อน TextBox2 และแสดง ComboBox4
                textBox2.Visible = false;
                comboBox4.Visible = true;

                label6.Visible = true;
                // ซ่อน label3 เมื่อเลือก "Airport" หรือ "Shopping Mall"
                label3.Visible = false;

                // ตั้งค่าของ ComboBox4 ตามการเลือกที่เหลือ
                comboBox4.Items.Clear();
                switch (selectedValue)
                {
                    case "Airport":
                        comboBox4.Items.AddRange(new string[] { "Suvarnnabhumi Airport (BKK)", "Don Mueang Airport (DMK)" });
                        break;
                    case "Shopping Mall":
                        comboBox4.Items.AddRange(new string[] { "MBK Center", "Terminal 21 Bangkok", "Central World", "Siam Paragon", "ICONSIAM" });
                        break;
                }
            }
            // อัพเดท Labels
            UpdateTextBoxes();

        }

        //private bool isUpdating = false;

        private void DateTimePickers_ValueChanged(object sender, EventArgs e)
        {
            //    if (isUpdating) return; // ข้ามการทำงานหากกำลังอัพเดต

            //    isUpdating = true; // เริ่มการอัพเดต

            // ตรวจสอบว่าค่าของ dateTimePicker1 เปลี่ยนแปลง
            if (sender == dateTimePicker1)
            {
                // ตรวจสอบว่าค่าของ dateTimePicker3 ไม่ต่ำกว่าค่าของ dateTimePicker1
                if (dateTimePicker3.Value < dateTimePicker1.Value)
                {
                    // แจ้งเตือนผู้ใช้
                    MessageBox.Show("วันที่ pickup ต้องไม่น้อยกว่าวันที่ dropoff",
                                    "วันที่ไม่ถูกต้อง",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    // ตั้งค่าของ dateTimePicker3 ให้ตรงกับ dateTimePicker1
                    dateTimePicker3.Value = dateTimePicker1.Value;
                }
            }
            else if (sender == dateTimePicker3)
            {
                // ตรวจสอบว่าค่าของ dateTimePicker3 ไม่ต่ำกว่าค่าของ dateTimePicker1
                if (dateTimePicker3.Value < dateTimePicker1.Value)
                {
                    // แจ้งเตือนผู้ใช้
                    MessageBox.Show("วันที่ pickup ต้องไม่น้อยกว่าวันที่ dropoff",
                                    "วันที่ไม่ถูกต้อง",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    // ตั้งค่าของ dateTimePicker3 ให้ตรงกับ dateTimePicker1
                    dateTimePicker3.Value = dateTimePicker1.Value;
                }
            }

            // คำนวณจำนวนวันระหว่างวันที่ใน DateTimePicker
            DateTime startDate = dateTimePicker1.Value.Date;  // ใช้วันที่
            DateTime endDate = dateTimePicker3.Value.Date;    // ใช้วันที่


            // ดึงเวลาออกจาก dateTimePicker3 และ dateTimePicker4
            DateTime dateTime1 = dateTimePicker2.Value; // ได้ค่า DateTime
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

            // อัปเดต label11 ด้วย daysDifference
            label11.Text = daysDifference.ToString();

            // ตรวจสอบความแตกต่างของเวลา
            TimeSpan timeDifference = time2 - time1;

            // ตรวจสอบว่า dateTimePicker1 และ dateTimePicker3 มีค่าเท่ากันหรือไม่
            if (dateTimePicker1.Value == dateTimePicker3.Value)
            {
                if (timeDifference.TotalHours < 1)
                {
                    MessageBox.Show("ความแตกต่างระหว่างเวลาต้องมากกว่า 60 นาที", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // กำหนดเวลาใน dateTimePicker4 ให้อยู่หลัง dateTimePicker2 60 นาที
                    DateTime newDateTimePicker4Value = dateTimePicker2.Value.AddMinutes(60);
                    dateTimePicker4.Value = newDateTimePicker4Value;
                }
            }

        }


        private void UpdateTextBoxes()
        {
            string locationorigin = comboBox1.Text;
            string locationorigin2 = comboBox2.Text;
            string locationorigintt = textBox1.Text;

            string locationdestination = comboBox3.Text;
            string locationdestination2 = comboBox4.Text;
            string locationdestinationtt = textBox2.Text;
            
           


            string datedropoffAdded = dateTimePicker1.Value.ToShortDateString();
            string datepickupAdded = dateTimePicker3.Value.ToShortDateString();
            string timeDropoff = dateTimePicker2.Value.ToString("HH:mm");
            string timePickup = dateTimePicker4.Value.ToString("HH:mm");

            // แสดงข้อมูลใน label
            label4.Text = locationorigin2;
            label6.Text = locationdestination2;
            label2.Text = locationorigintt;
            label3.Text = locationdestinationtt;
            label5.Text = datedropoffAdded + " " + timeDropoff + ":00";
            label7.Text = datepickupAdded + " " + timePickup + ":00";
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
            label10.Text = totalPrice.ToString("C"); // Format to currency
        }


        private string _username;
        public string username_ii2
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
            //usename1
        }


        private void inputinfo2_Load(object sender, EventArgs e)
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
                string username = username_ii2;

                // คำนวณความกว้างของข้อความ
                SizeF textSize = e2.Graphics.MeasureString(username, labelUsername.Font);

                // คำนวณตำแหน่งข้อความให้เริ่มจากขวา
                int textX = labelUsername.Width - labelUsername.Padding.Right - (int)textSize.Width;
                int textY = labelUsername.Padding.Top;

                // วาดข้อความเอง
                e2.Graphics.DrawString(username, labelUsername.Font, new SolidBrush(labelUsername.ForeColor), new PointF(textX, textY));
            };

            //// แสดงชื่อผู้ใช้ใน TextBox เมื่อฟอร์มโหลด
            //labelUsername.Text = username_ii2;

            // ตั้งค่า MinDate ของ DateTimePicker
            dateTimePicker1.MinDate = DateTime.Today;
            // ตั้งค่า MinDate ของ DateTimePicker
            dateTimePicker3.MinDate = DateTime.Today;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            UpdateTextBoxes();
        }


        int maxid = 0;
        private void button8_Click(object sender, EventArgs e)
        {

            // แสดงชื่อผู้ใช้ใน TextBox เมื่อฟอร์มโหลด
            string username = username_ii2;

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT MAX(ID) FROM process";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {


                    try
                    {
                        conn.Open();
                        maxid = (int)cmd.ExecuteScalar();
                        
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

            string service = "luggagedelivery";

            string locationorigin = comboBox1.Text;
            string locationorigin2 = comboBox2.Text;
            string locationorigintt = textBox1.Text;

            string locationdestination = comboBox3.Text;
            string locationdestination2 = comboBox4.Text;
            string locationdestinationtt = textBox2.Text;

            string sumlocation = $"{locationorigin}, {locationorigintt}";
            string sumlocation2 = $"{locationorigin}, {locationorigin2}";
            string sumlocationtwo = $"{locationdestination}, {locationdestinationtt}";
            string sumlocationtwo2 = $"{locationdestination}, {locationdestination2}";

            // กำหนดค่าของพารามิเตอร์ location ตามเงื่อนไข
            string location;

            if (!string.IsNullOrEmpty(locationorigintt))
            {
                location = sumlocation; // ใช้ sumlocation ถ้า textBox1 มีค่า
            }
            else if (!string.IsNullOrEmpty(locationorigin2))
            {
                location = sumlocation2; // ใช้ sumlocation2 ถ้า textBox1 ว่าง แต่ comboBox2 มีค่า
            }
            else
            {
                location = null; // ถ้าไม่มีค่าในทั้งสอง
            }

            // กำหนดค่าของพารามิเตอร์ locationtwo ตามเงื่อนไข
            string locationtwo;

            if (!string.IsNullOrEmpty(locationdestinationtt))
            {
                locationtwo = sumlocationtwo; // ใช้ sumlocationtwo ถ้า textBox2 มีค่า
            }
            else if (!string.IsNullOrEmpty(locationdestination2))
            {
                locationtwo = sumlocationtwo2; // ใช้ sumlocationtwo2 ถ้า textBox2 ว่าง แต่ comboBox4 มีค่า
            }
            else
            {
                locationtwo = null; // ถ้าไม่มีค่าในทั้งสอง
            }


            // ตรวจสอบว่า location และ locationtwo ห้ามมีตัวอักษรภาษาไทย
            if (System.Text.RegularExpressions.Regex.IsMatch(location, @"[ก-๙]") ||
                System.Text.RegularExpressions.Regex.IsMatch(locationtwo, @"[ก-๙]"))
            {
                MessageBox.Show("Both NameOrigin and NameDestination must not contain Thai characters.");
                return;
            }




            double tt = Convert.ToDouble(totalPrice);
            string datetextbox1 = label5.Text;
            string datetextbox2 = label7.Text;

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
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@locationtwo", locationtwo);
                    cmd.Parameters.AddWithValue("@dtdropoff", dropoff);
                    cmd.Parameters.AddWithValue("@dtpickup", pickup);
                    cmd.Parameters.AddWithValue("@qttregular", regular.Text);
                    cmd.Parameters.AddWithValue("@qttspecial", special.Text);
                    cmd.Parameters.AddWithValue("@duration", daysDifference);
                    cmd.Parameters.AddWithValue("@total", tt);
                    cmd.Parameters.AddWithValue("@processid", maxid + 1);

                    try
                    {
                        conn.Open(); // เปิดการเชื่อมต่อ
                        cmd.ExecuteNonQuery(); // เรียกใช้ ExecuteNonQuery เพื่อแทรกข้อมูล
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


            


            // ตรวจสอบว่ามีค่าใน textBoxUsername
            string username2 = username;

            // เปิดฟอร์ม inputinfo6 และส่ง username
            inputinfo6 inputinfo6Form = new inputinfo6();
            inputinfo6Form.username_ii3 = username2;
            inputinfo6Form.processid = GetMaxProcessId(); // ส่งค่า ProcessId ล่าสุด
            inputinfo6Form.id = maxid + 1;
            inputinfo6Form.Show();
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
            if (numberreg > 0) // ตรวจสอบก่อนลดค่า
            {
                numberreg--;
                regular.Text = numberreg.ToString();
                CheckLabels(); // อัปเดตสถานะของปุ่ม
                CalculatePrice(); // คำนวณราคาใหม่
                UpdateSummary(); // อัปเดตข้อความ
            }
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
            if (numberspec > 0) // ตรวจสอบก่อนลดค่า
            {
                numberspec--;
                special.Text = numberspec.ToString();
                CheckLabels(); // อัปเดตสถานะของปุ่ม
                CalculatePrice(); // คำนวณราคาใหม่
                UpdateSummary(); // อัปเดตข้อความ
            }
        }

        private void label11_TextChanged(object sender, EventArgs e)
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

            if (!int.TryParse(label11.Text, out int daysDifference))
            {
                daysDifference = 0; // กำหนดค่าเริ่มต้นถ้าไม่สามารถแปลงได้
            }

            // สมมุติว่าราคา regular และ special มีค่าคงที่
            const int regularPricePerDay = 300; // ราคา per day ของ regular
            const int specialPricePerDay = 600; // ราคา per day ของ special

            // คำนวณราคาสำหรับ regular และ special
            int totalRegularPrice = regularCount * regularPricePerDay * daysDifference;
            int totalSpecialPrice = specialCount * specialPricePerDay * daysDifference;

            // คำนวณราคาทั้งหมด
            totalPrice = totalRegularPrice + totalSpecialPrice;


            // สร้างข้อความสำหรับ TextBox summarize
            string summaryMessage = $"{regularCount} Regular Luggage x {daysDifference} days = {totalRegularPrice.ToString("C0", new CultureInfo("th-TH"))}";
            label8.Text = summaryMessage;

            // สร้างข้อความสำหรับ TextBox summarize2
            string summaryMessage2 = $"{specialCount} Special Luggage x {daysDifference} days = {totalSpecialPrice.ToString("C0", new CultureInfo("th-TH"))}";
            label9.Text = summaryMessage2;

            // แสดงผลรวมราคาใน TextBox textBoxTotalPrice
            label10.Text = totalPrice.ToString("C0", new CultureInfo("th-TH")); // Format to currency
        }

        
    }
}
