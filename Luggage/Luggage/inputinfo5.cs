using Saladpuk.PromptPay.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;
using ZXing;
using MySql.Data.MySqlClient;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MySqlX.XDevAPI.Relational;
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Fonts;
using System.Diagnostics;
using System.Globalization; 

namespace Luggage
{
    public partial class inputinfo5 : Form
    {

        private MySqlConnection databaseConnection()
        {
            string connectionstring = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            return conn;
        }

        private int processidqr;
        public int processid_qr
        {
            get { return processidqr; }
            set
            {
                processidqr = value;
                label2.Text = value.ToString(); // แปลง int เป็น string ที่นี่
            }
        }

        private string _username;
        public string username_qr
        {
            get { return _username; }
            set { _username = value; label3.Text = value; }
            //username1
        }

        private string _name;
        public string name_qr
        {
            get { return _name; }
            set { _name = value; label4.Text = value; }
            //username1
        }

        private string _tel;
        public string tel_qr
        {
            get { return _tel; }
            set { _tel = value; label5.Text = value; }
            //username1
        }

        private string _email;
        public string email_qr
        {
            get { return _email; }
            set { _email = value; label6.Text = value; }
            //username1
        }

        private string _idenno;
        public string iden_qr
        {
            get { return _idenno; }
            set { _idenno = value; label7.Text = value; }
            //username1
        }

        private string _address;
        public string address_qr
        {
            get { return _address; }
            set { _address = value; label8.Text = value; }
            //username1
        }





        double vat = 0;//tax
        double discount = 0;//discount
        double total = 0;//total
        int sum = 0;//subtotal

        string cusid;//cusid
        string status = "already paid but not dropped off yet";//status

        public inputinfo5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("we apologize that your request was canceled due to non-payment, please submit a new request.");

            using (MySqlConnection conn = databaseConnection())
            {
                // คำสั่ง SQL สำหรับการลบข้อมูล
                string deleteQuery = "DELETE FROM process WHERE ID = @processid";

                using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@processid", processidqr);

                    try
                    {
                        conn.Open();
                        int rowsAffected = deleteCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data successfully deleted from the process table.");

                            // เปิดฟอร์ม homepage และซ่อนฟอร์มปัจจุบัน
                            homepage homepageForm = new homepage();
                            homepageForm.Show();
                            this.Hide();
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
        private void inputinfo5_Load(object sender, EventArgs e)
        {
         
            

            //int sum = 0; // กำหนดค่าเริ่มต้นให้กับ sum

            using (MySqlConnection conn = databaseConnection())
            {
                string query = "SELECT * FROM process WHERE ID = @processid"; // ใช้ ID แทน processid

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@processid", processidqr);

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
            label1.Text = $"฿{total.ToString("N0")}"; // แสดงผลรวมใน label

            // สร้าง QR Code
            string qr = PPay.DynamicQR.MobileNumber("0963522258").Amount(total).CreateCreditTransferQrCode();
            BarcodeWriter barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 250,
                    Height = 250,
                    PureBarcode = true
                },
            };
            Bitmap barCodeBitmap = barcodeWriter.Write(qr);
            picqr.Image = barCodeBitmap;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //string tick = DateTime.Now.Ticks.ToString(); // ตัวเลขสุ่มสำหรับเลขใบเสร็จและชื่อไฟล์ pdf

            //string username = "";
            //string location = "";
            //string locationtwo = "";
            //string service = "";
            //int regular = 0;
            //int special = 0;
            //int duration = 0;
            //DateTime dropoff = DateTime.MinValue;
            //DateTime pickup = DateTime.MinValue;

            //using (MySqlConnection conn = databaseConnection())
            //{
            //    string query = "SELECT * FROM process WHERE ID = @processid"; // ใช้ ID แทน processid

            //    using (MySqlCommand cmd = new MySqlCommand(query, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@processid", processidqr);

            //        try
            //        {
            //            conn.Open();
            //            MySqlDataReader reader = cmd.ExecuteReader();
            //            if (reader.Read()) // ใช้ if แทน while เพราะเราคาดว่ามีข้อมูลแค่หนึ่งแถว
            //            {
            //                username = reader.GetString(1);
            //                service = reader.GetString(2);
            //                location = reader.GetString(3);
            //                locationtwo = reader.GetString(4);
            //                regular = reader.GetInt32(7);
            //                special = reader.GetInt32(8);
            //                duration = reader.GetInt32(9);
            //                dropoff = reader.GetDateTime(5);
            //                pickup = reader.GetDateTime(6);
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








            //    // วันที่ปริ้นใบเสร็จในรูปแบบปีพุทธศักราช
            //    string charge_date = DateTime.Now.ToString("dd/MM/") + (DateTime.Now.Year).ToString();

            //    string charge_time = DateTime.Now.ToString("HH:mm:ss");//เวลาที่ปริ้นใบเสร็จ



            //    //ใช้ pdfsharp สร้างpdf โหลดใน nuget
            //    PdfDocument document = new PdfDocument();
            //    PdfSharp.Pdf.PdfPage page = document.AddPage();
            //    XGraphics gfx = XGraphics.FromPdfPage(page);

            //    //ฟ้อนต์ตัวอักษร
            //    XFont font = new XFont("Tahoma", 12, XFontStyleEx.Regular);
            //    XTextFormatter tf = new XTextFormatter(gfx);




            ////เนื้อหาในpdf

            //DrawImage(gfx, "C:\\Users\\MikiBunnie\\Downloads\\logo.png", 0, 0, 600, 115);      // logo  //โลโก้เอามาจากไฟล์ในเครื่อง


            //gfx.DrawString($"No. : {tick}", font, XBrushes.Black, new XPoint(400, 150)); //เลขที่ใบเสร็จ
            //gfx.DrawString($"Charge Date : {charge_date}", font, XBrushes.Black, new XPoint(400, 170)); //วันที่ปริ้นใบเสร็จ
            //gfx.DrawString($"Charge time : {charge_time}", font, XBrushes.Black, new XPoint(400, 190));






            //// ตรวจสอบค่าของ service
            //if (service == "luggagestorage")
            //{
            //    // วาดข้อความเมื่อ service เป็น "luggagestorage"
            //    gfx.DrawString($"Service : {service}", font, XBrushes.Black, new XPoint(33, 150));
            //    gfx.DrawString($"Location : {location}", font, XBrushes.Black, new XPoint(33, 170));
            //}
            //else
            //{
            //    // วาดข้อความเมื่อ service เป็น "luggagedelivery"
            //    gfx.DrawString($"Service : {service}", font, XBrushes.Black, new XPoint(33, 150));
            //    gfx.DrawString($"LocationOrigin : {location}", font, XBrushes.Black, new XPoint(33, 170));
            //    gfx.DrawString($"LocationDestination : {locationtwo}", font, XBrushes.Black, new XPoint(33, 190));
            //}






            //gfx.DrawString($"Customer Name : {_name}", font, XBrushes.Black, new XPoint(33, 210));                      //ชื่อลูกค้า
            //gfx.DrawString($"โทร : {_tel}", font, XBrushes.Black, new XPoint(33, 230));
            //gfx.DrawString($"Email Address : {_email}", font, XBrushes.Black, new XPoint(33, 250));                         //อีเมลลูกค้า
            //gfx.DrawString($"Booking ID : {cusid}", font, XBrushes.Black, new XPoint(33, 270));
            //gfx.DrawString($"Dropoff : {dropoff}", font, XBrushes.Black, new XPoint(33, 290));
            //gfx.DrawString($"Pickup : {pickup}", font, XBrushes.Black, new XPoint(33, 310));


            //// วาดเส้นกรอบด้านบน
            //gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(33, 330), new XPoint(570, 330));

            //// วาดเส้นกรอบด้านซ้าย
            //gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(33, 330), new XPoint(33, 730));

            //// วาดเส้นกรอบด้านล่าง
            //gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(33, 730), new XPoint(570, 730));

            //// วาดเส้นกรอบด้านขวา
            //gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(570, 330), new XPoint(570, 730));

            //// วาดเส้นแนวนอนในตาราง
            //gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(33, 370), new XPoint(570, 370));
            //gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(33, 600), new XPoint(570, 600));


            //////ตาราง 1บรรทัดคือ1เส้น
            ////gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(40, 250), new XPoint(570, 250));
            ////gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(40, 350), new XPoint(40, 700)); //เส้นกรอบบน
            ////gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(40, 700), new XPoint(570, 700));
            ////gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(570, 250), new XPoint(570, 700));
            ////gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(40, 280), new XPoint(570, 280)); //เส้นแนวนอน
            ////gfx.DrawLine(new XPen(XColor.FromName("Black")), new XPoint(40, 600), new XPoint(570, 600));

            ////รายละเอียดในตาราง
            //int space = 0;
            //int psr = 100 * duration * regular;
            //int pss = 200 * duration * special;
            //int pdr = 300 * duration * regular;
            //int pds = 600 * duration * special;

            //// วาดหัวตาราง
            //gfx.DrawString($"Description", font, XBrushes.Black, new XPoint(110, 355));
            //gfx.DrawString($"Quantity", font, XBrushes.Black, new XPoint(370, 355));
            //gfx.DrawString($"Total", font, XBrushes.Black, new XPoint(500, 355));


            //// ตรวจสอบ service
            //if (service == "luggagestorage")
            //{
            //    // เงื่อนไขสำหรับ luggagestorage
            //    if (special == 0)
            //    {
            //        gfx.DrawString($"Regular Luggage (100 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 400 + space));
            //        gfx.DrawString($"{regular}", font, XBrushes.Black, new XPoint(390, 400 + space));
            //        gfx.DrawString($"฿{psr.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 400 + space));
            //    }
            //    else if (regular == 0)
            //    {
            //        gfx.DrawString($"Special Luggage (200 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 400 + space));
            //        gfx.DrawString($"{special}", font, XBrushes.Black, new XPoint(390, 400 + space));
            //        gfx.DrawString($"฿{pss.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 400 + space));
            //    }
            //    else
            //    {
            //        gfx.DrawString($"Regular Luggage (100 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 400 + space));
            //        gfx.DrawString($"{regular}", font, XBrushes.Black, new XPoint(390, 400 + space));
            //        gfx.DrawString($"฿{psr.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 400 + space));

            //        gfx.DrawString($"Special Luggage (200 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 430 + space));
            //        gfx.DrawString($"{special}", font, XBrushes.Black, new XPoint(390, 430 + space));
            //        gfx.DrawString($"฿{pss.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 430 + space));

            //    }
            //}
            //else if (service == "luggagedelivery")
            //{
            //    // เงื่อนไขสำหรับ luggagedelivery
            //    if (special == 0)
            //    {
            //        gfx.DrawString($"Regular Luggage (300 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 400 + space));
            //        gfx.DrawString($"{regular}", font, XBrushes.Black, new XPoint(390, 400 + space));
            //        gfx.DrawString($"฿{pdr.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 400 + space));
            //    }
            //    else if (regular == 0)
            //    {
            //        gfx.DrawString($"Special Luggage (600 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 400 + space));
            //        gfx.DrawString($"{special}", font, XBrushes.Black, new XPoint(390, 400 + space));
            //        gfx.DrawString($"฿{pds.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 400 + space));
            //    }
            //    else
            //    {
            //        gfx.DrawString($"Regular Luggage (300 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 400 + space));
            //        gfx.DrawString($"{regular}", font, XBrushes.Black, new XPoint(390, 400 + space));
            //        gfx.DrawString($"฿{pdr.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 400 + space));

            //        gfx.DrawString($"Special Luggage (600 THB/Bag per Day)", font, XBrushes.Black, new XPoint(60, 430 + space));
            //        gfx.DrawString($"{special}", font, XBrushes.Black, new XPoint(390, 430 + space));
            //        gfx.DrawString($"฿{pds.ToString("N0")}", font, XBrushes.Black, new XPoint(510, 430 + space));
            //    }

            //}




            //gfx.DrawString($"Sub Total", font, XBrushes.Black, new XPoint(70, 620));
            //gfx.DrawString($"฿{sum.ToString("N0")}", font, XBrushes.Black, new XPoint(500, 620));
            //gfx.DrawString($"Vat 7%", font, XBrushes.Black, new XPoint(70, 650));
            //gfx.DrawString($"฿{vat.ToString("N0")}", font, XBrushes.Black, new XPoint(500, 650));

            //gfx.DrawString($"Discount 3%", font, XBrushes.Black, new XPoint(70, 680));
            //gfx.DrawString($"฿{discount.ToString("N0")}", font, XBrushes.Black, new XPoint(500, 680));

            //gfx.DrawString($"Total", font, XBrushes.Black, new XPoint(70, 710));
            //gfx.DrawString($"฿{total.ToString("N0")}", font, XBrushes.Black, new XPoint(500, 710));


            //gfx.DrawString($"Payment", font, XBrushes.Black, new XPoint(33, 760));
            //gfx.DrawString($"QR code Promptpay", font, XBrushes.Black, new XPoint(33, 780));

            //DrawImage(gfx, "C:\\Users\\MikiBunnie\\Downloads\\contactgb.png", 0, 800, 600, 45);

            ////เซฟไฟล์ลงเครื่อง
            //string filename = @"d:\bill_guardianbuddy\" + tick + ".pdf";
            //    document.Save(filename);

            //using (MySqlConnection conn = databaseConnection())
            //{
            //    string query = "INSERT INTO luggageinfo (namectm, tel, service, location, locationtwo, regular, special, dropoff, pickup, duration, subtotal, tax, discount, total, receipt, status, email, username, cusid, idenno, address) " +
            //                   "VALUES (@namectm, @tel, @service, @location, @locationtwo, @regular, @special, @dtdropoff, @dtpickup, @duration, @subtotal, @tax, @discount, @total, @receipt, @status, @email, @username, @cusid, @idenno, @address)";

            //    using (MySqlCommand cmd = new MySqlCommand(query, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@namectm", _name);
            //        cmd.Parameters.AddWithValue("@tel", _tel);
            //        cmd.Parameters.AddWithValue("@service", service);
            //        cmd.Parameters.AddWithValue("@location", location);
            //        cmd.Parameters.AddWithValue("@locationtwo", locationtwo);
            //        cmd.Parameters.AddWithValue("@regular", regular);
            //        cmd.Parameters.AddWithValue("@special", special);
            //        cmd.Parameters.AddWithValue("@dtdropoff", dropoff);
            //        cmd.Parameters.AddWithValue("@dtpickup", pickup);
            //        cmd.Parameters.AddWithValue("@duration", duration);
            //        cmd.Parameters.AddWithValue("@subtotal", sum);
            //        cmd.Parameters.AddWithValue("@tax", vat);
            //        cmd.Parameters.AddWithValue("@discount", discount);
            //        cmd.Parameters.AddWithValue("@total", total);
            //        cmd.Parameters.AddWithValue("@status", status);
            //        cmd.Parameters.AddWithValue("@email", _email);
            //        cmd.Parameters.AddWithValue("@username", username);
            //        cmd.Parameters.AddWithValue("@cusid", cusid);
            //        cmd.Parameters.AddWithValue("@receipt", filename);
            //        cmd.Parameters.AddWithValue("@idenno", _idenno);
            //        cmd.Parameters.AddWithValue("@address", _address);




            //        try
            //        {
            //            conn.Open();
            //            cmd.ExecuteNonQuery();
            //            MessageBox.Show("Data inserted successfully into luggageinfo table");
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

            //////เก็บบิลเข้าดาต้าเบส

            //Process.Start(filename);


            //// เปิดฟอร์ม inputinfo4 และส่ง username
            //inputinfo4 inputinfo4Form = new inputinfo4();
            //inputinfo4Form.username_ii4 = label3.Text;
            //inputinfo4Form.cusid_ii4 = label2.Text;
            //inputinfo4Form.Show();

            //this.Hide(); 

            // เปิดฟอร์ม inputinfo4 และซ่อนฟอร์มปัจจุบัน
            
            inputinfo4 inputinfo4 = new inputinfo4();
            inputinfo4.username_ii4 = label3.Text;
            inputinfo4.cusid_ii4 = label2.Text;
            inputinfo4.name_qr = label4.Text;
            inputinfo4.tel_qr = label5.Text;
            inputinfo4.email_qr = label6.Text;
            inputinfo4.iden_qr = label7.Text;
            inputinfo4.address_qr = label8.Text;



            inputinfo4.Show();
            this.Hide();


        }

        public class CustomFontResolver : IFontResolver
        {
            public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
            {
                // เปลี่ยนเส้นทางไปยังฟอนต์ที่คุณติดตั้งในเครื่อง
                if (familyName == "Tahoma")
                {
                    string fontPath = @"C:\Windows\Fonts\Tahoma.otf"; // เช็คไฟล์ฟอนต์ .ttf
                    if (!File.Exists(fontPath))
                    {
                        throw new Exception("Font file not found.");
                    }
                    return new FontResolverInfo(fontPath);
                }
                return null; // ไม่สามารถค้นพบฟอนต์
            }

            public byte[] GetFont(string faceName)
            {
                return null; // ไม่โหลดฟอนต์
            }
        }

        void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }
    }
 
}
