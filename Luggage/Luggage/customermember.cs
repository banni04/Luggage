using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Luggage
{
    public partial class customermember : Form
    {
        public customermember()
        {
            InitializeComponent();
        }

        // เชื่อมต่อฐานข้อมูล MySQL
        private MySqlConnection databaseConnection()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        private void showMember()
        {
            MySqlConnection conn = databaseConnection();
            DataSet ds = new DataSet();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM customer_info";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();

            dataMember.DataSource = ds.Tables[0].DefaultView;
        }

        private MySqlDataAdapter adapter;
        private MySqlCommandBuilder commandBuilder;
        private DataSet dataSet;

        private object updateLock = new object();
        private bool isUpdating = false;  // ตัวแปรเพื่อป้องกันการเรียกซ้ำ


        private void customermember_Load(object sender, EventArgs e)
        {
            showMember();
            dataMember.CurrentCellDirtyStateChanged += new EventHandler(dataMember_CurrentCellDirtyStateChanged);
            FillDGV("");        // ตรวจสอบว่าฟังก์ชันนี้มีอยู่และทำงานได้
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshData(); // เรียกใช้ฟังก์ชัน RefreshData เพื่อรีเฟรชข้อมูล
        }

        private void RefreshData()
        {
            // สร้างการเชื่อมต่อฐานข้อมูล
            using (MySqlConnection conn = databaseConnection())
            {
                // สร้าง MySqlDataAdapter สำหรับดึงข้อมูล
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM customer_info", conn);
                DataTable dataTable = new DataTable();

                // เติมข้อมูลจากฐานข้อมูล
                adapter.Fill(dataTable);

                // ตั้งค่า DataSource สำหรับ DataGridView
                dataMember.DataSource = dataTable;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // บันทึกการเปลี่ยนแปลงที่มีใน DataGridView
            SaveChanges();
        }

        private void SaveChanges()
        {
            lock (updateLock)
            {
                if (isUpdating) return; // ข้ามการทำงานหากกำลังอัพเดต
                isUpdating = true;

                bool anyUpdate = false; // ใช้ติดตามว่ามีการอัปเดตสำเร็จหรือไม่
                bool hasError = false;  // ใช้ติดตามว่ามีข้อผิดพลาดหรือไม่

                foreach (DataGridViewRow row in dataMember.Rows)
                {
                    if (row.IsNewRow) continue;

                    int id = Convert.ToInt32(row.Cells["id"].Value);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name != "id" && cell.Value != null)
                        {
                            string columnName = cell.OwningColumn.Name;
                            object newValue = cell.Value;

                            // ตรวจสอบว่าค่าที่แก้ไขถูกต้องก่อนทำการอัพเดต
                            if (cell.Tag != null && cell.Tag.ToString() == "Error")
                            {
                                MessageBox.Show($"ค่าที่แก้ไขในคอลัมน์ {columnName} มีข้อผิดพลาด", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                hasError = true;
                                break; // ออกจาก loop ถ้ามีข้อผิดพลาด
                            }

                            // อัพเดตฐานข้อมูล และตรวจสอบว่าการอัพเดตสำเร็จหรือไม่
                            bool updateResult = UpdateDatabase(id, columnName, newValue);
                            if (!updateResult)
                            {
                                hasError = true; // พบข้อผิดพลาด เช่น ข้อมูลซ้ำ
                                break; // หยุดกระบวนการทันที
                            }

                            anyUpdate = true; // อัพเดตสำเร็จ
                        }
                    }

                    if (hasError) break; // หยุดการประมวลผลหากพบข้อผิดพลาด
                }

                isUpdating = false;

                // แสดงการแจ้งเตือนตามสถานะ
                if (hasError)
                {
                    MessageBox.Show("ไม่สามารถอัพเดตข้อมูลได้เนื่องจากข้อผิดพลาด", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (anyUpdate)
                {
                    MessageBox.Show("ข้อมูลทั้งหมดได้รับการอัพเดตเรียบร้อยแล้ว", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("ไม่มีการเปลี่ยนแปลงข้อมูล", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }



        private bool UpdateDatabase(int id, string columnName, object newValue)
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=luggage;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // ตรวจสอบข้อมูลซ้ำในฟิลด์ที่กำหนด
                    if (columnName == "username" || columnName == "tel" || columnName == "email" || columnName == "idenno" || columnName == "address")
                    {
                        string checkQuery = $@"
                    SELECT COUNT(*) 
                    FROM customer_info 
                    WHERE {columnName} = @value AND id != @id";
                        using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@value", newValue);
                            checkCmd.Parameters.AddWithValue("@id", id);

                            int duplicateCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                            if (duplicateCount > 0)
                            {
                                MessageBox.Show($"The {columnName} '{newValue}' already exists in the database.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false; // การอัพเดตล้มเหลวเพราะค่าซ้ำ
                            }
                        }
                    }

                    // อัพเดตฐานข้อมูล
                    string sql = $"UPDATE customer_info SET {columnName} = @value WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@value", newValue);
                        cmd.Parameters.AddWithValue("@id", id);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            Console.WriteLine($"Successfully updated row with ID {id}");
                            return true; // การอัพเดตสำเร็จ
                        }
                        else
                        {
                            Console.WriteLine($"No rows affected for ID {id}");
                            return false; // ไม่มีการอัพเดต
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // การอัพเดตล้มเหลว
                }
            }
        }


        private void dataMember_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // ตรวจสอบว่าเซลล์มีการเปลี่ยนแปลง
            if (dataMember.IsCurrentCellDirty)
            {
                // บังคับให้เซลล์ที่กำลังใช้งานบันทึกการเปลี่ยนแปลง
                dataMember.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataMember_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // ตรวจสอบคอลัมน์ namectm
            if (dataMember.Columns[e.ColumnIndex].Name == "name")
            {
                string inputValue = e.FormattedValue.ToString();
                if (!IsEnglishOrThaiAlphabet(inputValue))
                {
                    MessageBox.Show("กรุณากรอกชื่อให้เป็นตัวอักษรภาษาอังกฤษหรือไทยและมีช่องว่างได้เท่านั้น", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
                }
                else
                {
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
                }
            }

            // ตรวจสอบคอลัมน์ tel
            if (dataMember.Columns[e.ColumnIndex].Name == "tel")
            {
                string inputValue = e.FormattedValue.ToString();
                if (!IsValidPhoneNumber(inputValue))
                {
                    MessageBox.Show("หมายเลขโทรศัพท์ต้องเป็นตัวเลข 10 หลัก", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
                }
                else
                {
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
                }
            }

            // ตรวจสอบคอลัมน์ email
            if (dataMember.Columns[e.ColumnIndex].Name == "email")
            {
                string inputValue = e.FormattedValue.ToString();
                if (!IsValidEmail(inputValue))
                {
                    MessageBox.Show("กรุณากรอกอีเมลที่ถูกต้อง", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
                }
                else
                {
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
                }
            }

            // ตรวจสอบคอลัมน์ idenno
            if (dataMember.Columns[e.ColumnIndex].Name == "idenno")
            {
                string inputValue = e.FormattedValue.ToString();
                if (!IsValidIdenno(inputValue))
                {
                    MessageBox.Show("กรุณากรอก idenno ที่ถูกต้อง (ตัวเลข 13 หลัก)", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
                }
                else
                {
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
                }
            }

            // ตรวจสอบคอลัมน์ address
            if (dataMember.Columns[e.ColumnIndex].Name == "address")
            {
                string inputValue = e.FormattedValue.ToString();
                if (!IsValidAddress(inputValue))
                {
                    MessageBox.Show("ที่อยู่ต้องประกอบด้วยตัวอักษรภาษาอังกฤษหรือภาษาไทย ตัวเลข ช่องว่าง และเครื่องหมายพิเศษที่กำหนด (/ - , . &)", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
                }
                else
                {
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
                }
            }

            // ตรวจสอบคอลัมน์ username
            if (dataMember.Columns[e.ColumnIndex].Name == "username")
            {
                string inputValue = e.FormattedValue.ToString();
                if (!IsValidUsername(inputValue))
                {
                    MessageBox.Show("Username must only contain letters (Thai or English) and numbers, no special characters allowed.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
                }
                else
                {
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
                }
            }

            // ตรวจสอบคอลัมน์ password
            if (dataMember.Columns[e.ColumnIndex].Name == "password")
            {
                string inputValue = e.FormattedValue.ToString();
                if (!IsValidPassword(inputValue))
                {
                    MessageBox.Show("Password must be at least 8 characters long.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
                }
                else
                {
                    dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
                }
            }




            //// ตรวจสอบคอลัมน์ caddress
            //if (dataMember.Columns[e.ColumnIndex].Name == "address")
            //{
            //    string inputValue = e.FormattedValue.ToString();
            //    if (IsValidAddress(inputValue))
            //    {
            //        MessageBox.Show("กรุณากรอก address โดยไม่ใช้อักขระพิเศษบางตัว เช่น @ _ # !", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        e.Cancel = true; // ยกเลิกการแก้ไขเซลล์
            //        dataMember[e.ColumnIndex, e.RowIndex].Tag = "Error"; // ตั้งค่า Tag ว่ามีข้อผิดพลาด
            //    }
            //    else
            //    {
            //        dataMember[e.ColumnIndex, e.RowIndex].Tag = null; // ลบข้อผิดพลาด
            //    }
            //}
        }

        // ฟังก์ชันตรวจสอบเฉพาะตัวอักษรภาษาอังกฤษหรือไทยและช่องว่าง
        private bool IsEnglishOrThaiAlphabet(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Zก-๙\s]+$");
        }


        // ฟังก์ชันตรวจสอบหมายเลขโทรศัพท์
        private bool IsValidPhoneNumber(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d{10}$");
        }

        // ฟังก์ชันตรวจสอบอีเมล
        private bool IsValidEmail(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        // ฟังก์ชันตรวจสอบ idenno
        private bool IsValidIdenno(string input)
        {
            return long.TryParse(input, out long result) && input.Length == 13;
        }

        // ฟังก์ชันตรวจสอบ address
        private bool IsValidAddress(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Zก-๙0-9\s/\-,.&]+$");
        }

        // ฟังก์ชันตรวจสอบ username
        private bool IsValidUsername(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9ก-๙]+$");
        }

        // ฟังก์ชันตรวจสอบ password
        private bool IsValidPassword(string input)
        {
            return input.Length >= 8;
        }


        //// ฟังก์ชันตรวจสอบเฉพาะตัวอักษรภาษาอังกฤษหรือไทย ตัวเลข ช่องว่าง และเครื่องหมายพิเศษ (/ - , . &)
        //private bool IsValidAddress(string input)
        //{
        //    return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Zก-๙0-9\s/\-,.&]+$");
        //}



        private void button3_Click(object sender, EventArgs e)
        {
            homeadmin homeadmin = new homeadmin();
            homeadmin.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
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
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM customer_info WHERE ID = @id", connection))
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
                                dataMember.DataSource = table;

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

        private void button5_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        public void ClearFields()
        {
            textBox1.Text = "";

            textBox2.Text = "";
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
                FROM customer_info
                WHERE CONCAT_WS(' ', username, password, tel, name, email, idenno, address) 
                LIKE @searchValue";

                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.Parameters.AddWithValue("@searchValue", "%" + valueToSearch + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataMember.DataSource = table;

                    if (dataMember.Columns.Count > 0)
                    {
                        foreach (DataGridViewColumn column in dataMember.Columns)
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            FillDGV(textBox2.Text);
        }
    }
}
