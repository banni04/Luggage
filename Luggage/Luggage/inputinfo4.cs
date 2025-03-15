using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace Luggage
{
    public partial class inputinfo4 : Form
    {

        //private string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";


        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        

        private string _name;
        public string name_qr
        {
            get { return _name; }
            set { _name = value; }
            //username1
        }

        private string _tel;
        public string tel_qr
        {
            get { return _tel; }
            set { _tel = value; }
            //username1
        }

        private string _email;
        public string email_qr
        {
            get { return _email; }
            set { _email = value; }
            //username1
        }

        private string _idenno;
        public string iden_qr
        {
            get { return _idenno; }
            set { _idenno = value; }
            //username1
        }

        private string _address;
        public string address_qr
        {
            get { return _address; }
            set { _address = value; }
            //username1
        }





        double vat = 0;//tax
        double discount = 0;//discount
        double total = 0;//total
        int sum = 0;//subtotal

        string cusid;//cusid
        string status = "already paid but not dropped off yet";//status


        public inputinfo4()
        {
            InitializeComponent();
        }

       

        private string username;

        //public inputinfo4(string username)
        //{
        //    InitializeComponent();
        //    this.username = username;
        //}

        private string _username;
        public string username_ii4
        {
            get { return _username; }
            set { _username = value; }
            //username1
        }

        private string _cusid;
        public string cusid_ii4
        {
            get { return _cusid; }
            set { _cusid = value; }
          
        }


       

        private void inputinfo4_Load(object sender, EventArgs e)
        {


            // แสดงชื่อผู้ใช้ใน TextBox เมื่อฟอร์มโหลด
            labelUsername.Text = username_ii4;


            cusid = "GB_" + _cusid;
            label2.Text = cusid;

            //int sum = 0; // กำหนดค่าเริ่มต้นให้กับ sum

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT * FROM process WHERE ID = @processid"; // ใช้ ID แทน processid

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@processid", _cusid);

                    try
                    {
                        conn.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read()) // ใช้ if แทน while เพราะเราคาดว่ามีข้อมูลแค่หนึ่งแถว
                        {
                            sum = reader.GetInt32(10);
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

            // คำนวณ VAT, Discount และ Total
            vat = (7 * sum) / 100;
            discount = (3 * sum) / 100;
            total = sum + vat - discount;
          
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //สร้างเอกสาร PDF
            Document document = new Document();
            Random random = new Random();
            int receiptNumber = random.Next(100000, 999999); // สุ่มเลขใบเสร็จ
            string pdfPath = $@"d:\bill_guardianbuddy\receipt_{receiptNumber}.pdf"; // ตั้งชื่อไฟล์ให้ถูกต้อง
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

            // เปิดเอกสาร
            document.Open();

            // โหลดฟอนต์
            BaseFont bf = BaseFont.CreateFont(@"C:\Users\MikiBunnie\AppData\Local\Microsoft\Windows\Fonts\THSarabunNew.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(bf, 18);



            // ดึงข้อมูล

            string username = "";
            string location = "";
            string locationtwo = "";
            string service = "";
            int regular = 0;
            int special = 0;
            int duration = 0;

            DateTime dropoff = DateTime.MinValue;
            DateTime pickup = DateTime.MinValue;



            // วันที่ปริ้นใบเสร็จในรูปแบบปีพุทธศักราช
            string charge_date = DateTime.Now.ToString("dd/MM/") + (DateTime.Now.Year).ToString(); // เพิ่ม 543 สำหรับปีพุทธศักราช
            string charge_time = DateTime.Now.ToString("HH:mm:ss"); // เวลาที่ปริ้นใบเสร็จ

           
            // เพิ่มโลโก้แรก
            string logoPath = @"C:\Users\MikiBunnie\Downloads\GB_IMAGE\Receiptlogo.png";
            Image logo = Image.GetInstance(logoPath);
            logo.ScaleToFit(600, 115); // ปรับขนาดโลโก้
            logo.SetAbsolutePosition(0, document.PageSize.Height - 110); // ปรับตำแหน่งโลโก้
            document.Add(logo); // เพิ่มโลโก้ลงในเอกสาร PDF

            // เพิ่มโลโก้ที่สอง
            string logoPath2 = @"C:\Users\MikiBunnie\Downloads\GB_IMAGE\Receiptcontact.png"; // เส้นทางโลโก้ที่สอง
            Image logo2 = Image.GetInstance(logoPath2);
            logo2.ScaleToFit(600, 115); // ปรับขนาดโลโก้ที่สอง
            logo2.SetAbsolutePosition(0, document.PageSize.Height - 850); // ปรับตำแหน่งโลโก้ที่สอง
            document.Add(logo2); // เพิ่มโลโก้ที่สองลงในเอกสาร PDF

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT * FROM process WHERE ID = @processid"; // ใช้ ID แทน processid

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@processid", _cusid);

                    try
                    {
                        conn.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read()) // ใช้ if แทน while เพราะเราคาดว่ามีข้อมูลแค่หนึ่งแถว
                        {
                            username = reader.GetString(1);
                            service = reader.GetString(2);
                            location = reader.GetString(3);
                            locationtwo = reader.GetString(4);
                            regular = reader.GetInt32(7);
                            special = reader.GetInt32(8);
                            duration = reader.GetInt32(9);
                            dropoff = reader.GetDateTime(5);
                            pickup = reader.GetDateTime(6);
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



            ////เนื้อหาในpdf
            // เพิ่มรายละเอียดใบเสร็จ
         
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"เลขที่ใบเสร็จ : {receiptNumber}", font), 410, 710, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"วันที่พิมพ์ใบเสร็จ : {charge_date}", font), 410, 690, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"เวลาพิมพ์ใบเสร็จ : {charge_time}", font), 410, 670, 0);
            
          

            //// รายการสินค้า
            //document.Add(new Paragraph("รายละเอียด:", font));
            //string[] descriptions = { "บริการปกติ", "บริการพิเศษ" };
            //float[] prices = { 100 * duration * regular, 200 * duration * special };
            //float totalAmount = 0;

            // วาดกรอบสำหรับรายการสินค้า
            float itemStartY = document.Top - 340; // จุดเริ่มต้น Y สำหรับกรอบ
            float itemBoxHeight = 350; // ความสูงคงที่
            PdfContentByte cb = writer.DirectContent;
            cb.SetLineWidth(1f);

            // กำหนดตำแหน่งและขนาดกรอบ
            float startX = 36; // ตำแหน่ง X ของกรอบ
            float startY = itemStartY; // ตำแหน่ง Y เริ่มต้นของกรอบ
            float boxWidth = document.PageSize.Width - 72; // ความกว้างของกรอบ
            float boxHeight = itemBoxHeight; // ความสูงของกรอบ

            // วาดกรอบโดยใช้เส้น
            cb.MoveTo(startX, startY); // เริ่มจากมุมซ้ายบน
            cb.LineTo(startX + boxWidth, startY); // วาดไปมุมขวาบน
            cb.LineTo(startX + boxWidth, startY - boxHeight); // วาดไปมุมขวาล่าง
            cb.LineTo(startX, startY - boxHeight); // วาดไปมุมซ้ายล่าง
            cb.LineTo(startX, startY); // วาดกลับมาที่มุมซ้ายบน
            cb.Stroke(); // วาดกรอบทั้งหมด

            // วาดเส้นขีดแนวนอนเพิ่มอีก 2 เส้น
            float firstLineY = startY - 40; // ตำแหน่ง Y สำหรับเส้นขีดแรก
            float secondLineY = startY - 210; // ตำแหน่ง Y สำหรับเส้นขีดที่สอง


            // กำหนดความยาวของเส้นแนวตั้ง
            float verticalLineLength = 210; // ความยาวที่ต้องการ

            // กำหนดตำแหน่ง X สำหรับเส้นแนวตั้ง
            float verticalLineX = startX + 270;

            // วาดเส้นแนวตั้งจากกรอบด้านบนไปยังกรอบด้านล่าง
            cb.MoveTo(verticalLineX, startY); // เริ่มที่มุมซ้ายบนของกรอบ
            cb.LineTo(verticalLineX, startY - verticalLineLength); // วาดลงไปตามความยาวที่กำหนด
            cb.Stroke(); // วาดเส้นแนวตั้ง

            // กำหนดตำแหน่ง X สำหรับเส้นแนวตั้งที่สอง
            float verticalLineX2 = startX + 370;

            // วาดเส้นแนวตั้งจากกรอบด้านบนไปยังกรอบด้านล่าง
            cb.MoveTo(verticalLineX2, startY); // เริ่มที่มุมซ้ายบนของกรอบ
            cb.LineTo(verticalLineX2, startY - verticalLineLength); // วาดลงไปตามความยาวที่กำหนด
            cb.Stroke(); // วาดเส้นแนวตั้งที่สอง



            cb.MoveTo(startX, firstLineY); // เริ่มจากมุมซ้าย
            cb.LineTo(startX + boxWidth, firstLineY); // วาดไปมุมขวา
            cb.Stroke(); // วาดเส้นแรก

            cb.MoveTo(startX, secondLineY); // เริ่มจากมุมซ้าย
            cb.LineTo(startX + boxWidth, secondLineY); // วาดไปมุมขวา
            cb.Stroke(); // วาดเส้นที่สอง



            //// วาดรายละเอียดในตาราง
            //float itemY = itemStartY - 15; // ตั้งค่าตำแหน่ง Y เริ่มต้นสำหรับรายการสินค้า
            //for (int i = 0; i < descriptions.Length; i++)
            //{
            //    document.Add(new Paragraph($"{descriptions[i]}: จำนวน {regular} - รวม: {prices[i]} บาท", font));
            //    totalAmount += prices[i];
            //    itemY -= 15; // ลดตำแหน่ง Y สำหรับรายการถัดไป
            ////}

            //// เพิ่มยอดรวม
            //document.Add(new Paragraph(" ", font)); // เพิ่มบรรทัดว่าง
            //document.Add(new Paragraph($"ยอดรวม: {totalAmount} บาท", font));



            // ตรวจสอบค่าของ service
            if (service == "luggagestorage")
            {
                // วาดข้อความเมื่อ service เป็น "luggagestorage"
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"บริการ : {service}", font), 38, 550, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สถานที่รับฝากสัมภาระ : {location}", font), 38, 530, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ชื่อผู้ใช้ : {username}", font), 38, 510, 0);
            }
            else
            {
                // วาดข้อความเมื่อ service เป็น "luggagedelivery"
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"บริการ : {service}", font), 38, 550, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"จัดส่งจาก : {location}", font), 38, 530, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"จัดส่งถึง : {locationtwo}", font), 38, 510, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ชื่อผู้ใช้ : {username}", font), 38, 490, 0);
            }


            // กำหนดตำแหน่ง
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ชื่อลูกค้า : {_name}", font), 38, 710, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"เบอร์โทร : {_tel}", font), 38, 690, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"เลขบัตรประจำตัวประชาชน : {_idenno}", font), 38, 670, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ที่อยู่ลูกค้า : {_address}", font), 38, 650, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ที่อยู่อีเมล : {_email}", font), 38, 630, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"รหัสการจอง : {cusid}", font), 38, 610, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"วันและเวลาที่ฝาก : {dropoff}", font), 38, 590, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"วันและเวลาที่รับ : {pickup}", font), 38, 570, 0);





            //รายละเอียดในตาราง
            int space = 0;
            int psr = 100 * duration * regular;
            int pss = 200 * duration * special;
            int pdr = 300 * duration * regular;
            int pds = 600 * duration * special;

            // วาดหัวตาราง
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"รายการ", font), 153, 440, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"จำนวน", font), 338, 440, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"จำนวนเงิน", font), 460, 440, 0);

            // ตรวจสอบ service


            if (service == "luggagestorage")
            {
                // เงื่อนไขสำหรับ luggagestorage
                if (special == 0)
                {
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระปกติ (100 THB ต่อชิ้นต่อวัน)", font), 70, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{regular}", font), 353, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{psr.ToString("N0")}", font), 530, 370, 0);

                }
                else if (regular == 0)
                {
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระพิเศษ (200 THB ต่อชิ้นต่อวัน)", font), 70, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{special}", font), 353, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{pss.ToString("N0")}", font), 530, 370, 0);

                }
                else
                {
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระปกติ (100 THB ต่อชิ้นต่อวัน)", font), 70, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{regular}", font), 353, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{psr.ToString("N0")}", font), 530, 370, 0);


                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระพิเศษ (200 THB ต่อชิ้นต่อวัน)", font), 70, 320, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{special}", font), 353, 320, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{pss.ToString("N0")}", font), 530, 320, 0);

                }
            }
            else if (service == "luggagedelivery")
            {
                // เงื่อนไขสำหรับ luggagedelivery
                if (special == 0)
                {
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระปกติ (300 THB ต่อชิ้นต่อวัน)", font), 70, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{regular}", font), 353, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{pdr.ToString("N0")}", font), 530, 370, 0);

                }
                else if (regular == 0)
                {
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระพิเศษ (600 THB ต่อชิ้นต่อวัน)", font), 70, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{special}", font), 353, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{pds.ToString("N0")}", font), 530, 370, 0);

                }
                else
                {
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระปกติ (300 THB ต่อชิ้นต่อวัน)", font), 70, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{regular}", font), 353, 370, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{pdr.ToString("N0")}", font), 530, 370, 0);


                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"สัมภาระพิเศษ (600 THB ต่อชิ้นต่อวัน)", font), 70, 320, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"{special}", font), 353, 320, 0);
                    ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{pds.ToString("N0")}", font), 530, 320, 0);

                }
            }

            // เพิ่มยอดรวมและรายละเอียดการชำระเงิน
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"มูลค่ารวมก่อนเสียภาษี", font), 70, 235, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{sum.ToString("N0")}", font), 530, 235, 0);


            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ภาษีมูลค่าเพิ่ม (VAT) 7%", font), 70, 200, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{vat.ToString("N0")}", font), 530, 200, 0);


            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ส่วนลด 3%", font), 70, 165, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{discount.ToString("N0")}", font), 530, 165, 0);


            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"ยอดรวม", font), 70, 130, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_RIGHT, new Phrase($"฿{total.ToString("N0")}", font), 530, 130, 0);


            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"การชำระเงิน", font), 38, 85, 0);


            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase($"QR code Promptpay", font), 38, 65, 0);





            using (MySqlConnection conn = databaseConnection())
            {
                string query = "INSERT INTO luggageinfo (namectm, tel, service, location, locationtwo, regular, special, dropoff, pickup, duration, subtotal, tax, discount, total, receipt, status, email, username, cusid, idenno, address) " +
                               "VALUES (@namectm, @tel, @service, @location, @locationtwo, @regular, @special, @dtdropoff, @dtpickup, @duration, @subtotal, @tax, @discount, @total, @receipt, @status, @email, @username, @cusid, @idenno, @address)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@namectm", _name);
                    cmd.Parameters.AddWithValue("@tel", _tel);
                    cmd.Parameters.AddWithValue("@service", service);
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@locationtwo", locationtwo);
                    cmd.Parameters.AddWithValue("@regular", regular);
                    cmd.Parameters.AddWithValue("@special", special);
                    cmd.Parameters.AddWithValue("@dtdropoff", dropoff);
                    cmd.Parameters.AddWithValue("@dtpickup", pickup);
                    cmd.Parameters.AddWithValue("@duration", duration);
                    cmd.Parameters.AddWithValue("@subtotal", sum);
                    cmd.Parameters.AddWithValue("@tax", vat);
                    cmd.Parameters.AddWithValue("@discount", discount);
                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@email", _email);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@cusid", cusid);
                    cmd.Parameters.AddWithValue("@receipt", pdfPath);
                    cmd.Parameters.AddWithValue("@idenno", _idenno);
                    cmd.Parameters.AddWithValue("@address", _address);




                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data inserted successfully into luggageinfo table");
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

            // ปิดเอกสาร
            document.Close();

            // แสดงข้อความเมื่อสร้าง PDF เสร็จ
            MessageBox.Show("PDF สร้างเรียบร้อยแล้วที่ " + pdfPath);

            // เปิด PDF ที่สร้างขึ้น
            Process.Start(pdfPath);

            // เปิดฟอร์ม homepage 

            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();


        }
    }
}
