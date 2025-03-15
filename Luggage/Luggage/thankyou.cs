using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Org.BouncyCastle.Asn1.Ocsp;
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using static Luggage.inputinfo5;
using System.Xml.Linq;
using ZXing;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using Org.BouncyCastle.Asn1.Tsp;

namespace Luggage
{

    public partial class thankyou : Form
    {
        private List<char> availableChars = new List<char> { 'ส', 'ว', 'ม', 'ท', 'ด', 'ี', 'ค', 'ร', 'ั', 'บ' };
        private int currentCharIndex = 0;
        private int correctCharCount = 0;
       

        // ตัวแปร Stopwatch, WPM, และ Timer
        private Stopwatch stopwatch = new Stopwatch();
        private List<bool> correctCharFlags;  // รายการที่เก็บว่าแต่ละตัวอักษรถูกต้องหรือไม่
        private List<bool> incorrectCharFlags;  // เก็บว่าแต่ละตำแหน่งพิมพ์ผิดหรือไม่
       
        
        
        private double accuracy = 0.0;  // ตัวแปรเก็บความแม่นยำ
        private Label wpmLabel;
        private Label timeLabel;
        private Label accuracyLabel;
        private TextBox inputTextBox;
        private Button resetButton;
        private Button exitButton;
        private Timer gameTimer;
        private bool gameStarted = false;
        private bool isGameOver = false;
        private Label charToTypeLabel; // เพิ่ม Label ใหม่เพื่อแสดงตัวอักษรที่ต้องพิมพ์

        public thankyou()
        {
            // กำหนดให้ทุกตัวอักษรใน availableChars เริ่มต้นเป็น false (ยังไม่ได้พิมพ์ถูก หรือผิด)
            incorrectCharFlags = new List<bool>(new bool[availableChars.Count]);
            correctCharFlags = new List<bool>(new bool[availableChars.Count]);

            this.Text = "เกมพิมพ์ตัวอักษร";
            this.Size = new Size(1015, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label instructionsLabel = new Label
            {
                Text = "พิมพ์ตัวอักษรที่แสดง:",
                Location = new Point(20, 20),
                Width = 300,
                Font = new Font("Tahoma", 12)
            };
            this.Controls.Add(instructionsLabel);

            inputTextBox = new TextBox
            {
                Location = new Point(20, 120),
                Width = 600,
                Font = new Font("Tahoma", 14)
            };
            this.Controls.Add(inputTextBox);

            charToTypeLabel = new Label
            {
                Text = $"{availableChars[currentCharIndex]}",
                Location = new Point(20, 60),
                Width = 200,
                Font = new Font("Tahoma", 16)
            };
            this.Controls.Add(charToTypeLabel);

            wpmLabel = new Label
            {
                Text = "WPM: 0",
                Location = new Point(20, 160),
                Width = 200,
                Font = new Font("Tahoma", 12)
            };
            this.Controls.Add(wpmLabel);

            timeLabel = new Label
            {
                Text = "Time: 0s",
                Location = new Point(20, 200),
                Width = 200,
                Font = new Font("Tahoma", 12)
            };
            this.Controls.Add(timeLabel);

            accuracyLabel = new Label
            {
                Text = "Accuracy: 0.00%",
                Location = new Point(20, 240),
                Width = 200,
                Font = new Font("Tahoma", 12)
            };
            this.Controls.Add(accuracyLabel);

            inputTextBox.TextChanged += InputTextBox_TextChanged;

            gameTimer = new Timer { Interval = 1000 }; // ตั้งเวลาให้ update ทุก 1 วินาที
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start(); // เริ่ม gameTimer ทันที
        }



        //private void InputTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    string userInput = inputTextBox.Text;

        //    if (!gameStarted && userInput.Length > 0)
        //    {
        //        stopwatch.Start(); // เริ่มจับเวลาเมื่อเริ่มพิมพ์
        //        gameStarted = true;
        //    }

        //    if (currentCharIndex < availableChars.Count)
        //    {
        //        if (userInput.Length == 1)
        //        {
        //            // ถ้าผู้ใช้พิมพ์ตัวอักษรถูก
        //            if (userInput[0] == availableChars[currentCharIndex])
        //            {
        //                // ถ้าพิมพ์ถูกในตำแหน่งที่ยังไม่ได้พิมพ์ผิด
        //                if (!incorrectCharFlags[currentCharIndex] && !correctCharFlags[currentCharIndex])
        //                {
        //                    correctCharFlags[currentCharIndex] = true;  // บันทึกว่าได้พิมพ์ถูกต้องแล้ว
        //                    correctCharCount++;  // เพิ่มจำนวนตัวอักษรที่พิมพ์ถูกต้อง
        //                }
        //                // ถ้าผู้ใช้พิมพ์ถูกต้องในตำแหน่งที่พิมพ์ผิดไปก่อนหน้า
        //                if (incorrectCharFlags[currentCharIndex] && !correctCharFlags[currentCharIndex])
        //                {
        //                    // ไม่คำนวณความแม่นยำในตำแหน่งที่เคยพิมพ์ผิด
        //                    correctCharFlags[currentCharIndex] = true;    // ตั้งค่าเป็นพิมพ์ถูกต้อง
        //                                                                  // ความแม่นยำจะไม่เพิ่มในตำแหน่งนี้
        //                }

        //                // อัปเดตข้อความใน label ถ้ามีการพิมพ์ตัวอักษรถูก
        //                currentCharIndex++;  // ไปที่ตัวอักษรถัดไป
        //                inputTextBox.Clear(); // ล้างข้อความใน TextBox

        //                if (currentCharIndex < availableChars.Count)
        //                {
        //                    charToTypeLabel.Text = $"{availableChars[currentCharIndex]}";
        //                }
        //                else
        //                {
        //                    isGameOver = true;
        //                    charToTypeLabel.Text = "พิมพ์ครบแล้ว!"; // แสดงข้อความเมื่อพิมพ์ครบ
        //                    ShowThankYouMessage(); // เรียกแสดงข้อความขอบคุณ
        //                }

        //                // คำนวณความแม่นยำเมื่อพิมพ์ตัวอักษรถูก
        //                if (correctCharCount == currentCharIndex) // คำนวณเมื่อครบ
        //                {
        //                    accuracy = ((double)correctCharCount / availableChars.Count) * 100;
        //                    accuracyLabel.Text = $"Accuracy: {accuracy:F2}%";
        //                }
        //            }
        //            else // ถ้าพิมพ์ผิด
        //            {
        //                // ถ้าพิมพ์ผิดในตำแหน่งที่ยังไม่ได้พิมพ์ผิดไปก่อนหน้านี้
        //                if (!incorrectCharFlags[currentCharIndex] && !correctCharFlags[currentCharIndex])
        //                {
        //                    incorrectCharFlags[currentCharIndex] = true; // เก็บว่าในตำแหน่งนี้พิมพ์ผิด
        //                }

        //                accuracyLabel.Text = $"Accuracy: {accuracy:F2}% (ผิด! ตัวอักษรปัจจุบัน: {availableChars[currentCharIndex]})";

        //                // ความแม่นยำไม่เปลี่ยนแปลงจนกว่าจะพิมพ์ถูกต้อง
        //            }
        //        }
        //    }
        //}

       





        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver)
            {
                return;
            }

            // การคำนวณเวลาที่ผ่านไป
            int elapsedSeconds = (int)Math.Floor(stopwatch.Elapsed.TotalSeconds);

            if (gameStarted && !isGameOver)
            {
                timeLabel.Text = $"Time: {elapsedSeconds}s"; // อัปเดตเวลาใน timeLabel
            }

            // เพิ่มการ log ค่าความแม่นยำ
            if (currentCharIndex > 0 && currentCharIndex <= availableChars.Count)
            {
                accuracy = ((double)correctCharCount / availableChars.Count) * 100;
                accuracyLabel.Text = $"Accuracy: {accuracy:F2}%";
                Console.WriteLine($"Game Timer Tick - Accuracy: {accuracy:F2}%");
            }

            this.Invalidate();
        }


        private void ShowThankYouMessage()
        {
            MessageBox.Show("ขอบคุณที่เล่นเกม! คุณพิมพ์ครบแล้ว");
            inputTextBox.Enabled = false; // ปิดการพิมพ์
            ShowResetButton(); // แสดงปุ่มเริ่มเกมใหม่
            ShowExitButton(); // แสดงปุ่มออกจากเกม
            isGameOver = true;
        }

        private void ShowResetButton()
        {
            resetButton = new Button
            {
                Text = "เริ่มเกมใหม่",
                Location = new Point(350, 500),
                Width = 100,
                Height = 40
            };
            resetButton.Click += (sender, e) => ResetGame();
            this.Controls.Add(resetButton);
        }

        private void ShowExitButton()
        {
            exitButton = new Button
            {
                Text = "ออกจากเกม",
                Location = new Point(460, 500),
                Width = 100,
                Height = 40
            };
            exitButton.Click += (sender, e) => this.Close();
            this.Controls.Add(exitButton);
        }


        private void ResetGame()
        {
            isGameOver = false;
            stopwatch.Reset(); // รีเซ็ตตัวจับเวลา
            gameStarted = false; // ตั้งค่าเกมยังไม่เริ่ม
            inputTextBox.Enabled = true; // เปิดใช้งาน TextBox
            inputTextBox.Clear(); // ล้างข้อความใน TextBox

            // รีเซ็ต UI และข้อความ
            timeLabel.Text = "Time: 0s";  // อัพเดตเวลาที่แสดงใน label
            wpmLabel.Text = "WPM: 0.00";
            accuracyLabel.Text = "Accuracy: 0.00%";

            // รีเซ็ตค่าต่างๆ
            currentCharIndex = 0;
            correctCharCount = 0;  // รีเซ็ตจำนวนตัวอักษรที่พิมพ์ถูกต้อง
            accuracy = 0.0;  // รีเซ็ตความแม่นยำให้เป็น 0

            // รีเซ็ต correctCharFlags และ incorrectCharFlags เพื่อให้คำนวณใหม่
            correctCharFlags = new List<bool>(new bool[availableChars.Count]);
            incorrectCharFlags = new List<bool>(new bool[availableChars.Count]);

            // รีเซ็ตเกม โดยไม่มีการจับเวลาหรือคำนวณอะไร
            charToTypeLabel.Text = $"{availableChars[currentCharIndex]}"; // แสดงตัวอักษรแรก

            // รีเซ็ตเกมใหม่
            inputTextBox.Focus(); // ใส่ focus ที่ TextBox เพื่อให้ผู้เล่นเริ่มพิมพ์ได้ทันที

            this.Invalidate();  // อัปเดต UI
        }

        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            string userInput = inputTextBox.Text;

            // เริ่มจับเวลาหลังจากเริ่มพิมพ์
            if (!gameStarted && userInput.Length > 0)
            {
                stopwatch.Start(); // เริ่มจับเวลาเมื่อเริ่มพิมพ์
                gameStarted = true; // ตั้งค่าเกมเริ่มต้น
            }

            if (currentCharIndex < availableChars.Count)
            {
                if (userInput.Length == 1)
                {
                    // ถ้าผู้ใช้พิมพ์ตัวอักษรถูก
                    if (userInput[0] == availableChars[currentCharIndex])
                    {
                        if (!incorrectCharFlags[currentCharIndex] && !correctCharFlags[currentCharIndex])
                        {
                            correctCharFlags[currentCharIndex] = true;  // บันทึกว่าได้พิมพ์ถูกต้องแล้ว
                            correctCharCount++;  // เพิ่มจำนวนตัวอักษรที่พิมพ์ถูกต้อง
                        }
                        currentCharIndex++;  // ไปที่ตัวอักษรถัดไป
                        inputTextBox.Clear(); // ล้างข้อความใน TextBox

                        if (currentCharIndex < availableChars.Count)
                        {
                            charToTypeLabel.Text = $"{availableChars[currentCharIndex]}";
                        }
                        else
                        {
                            isGameOver = true;
                            charToTypeLabel.Text = "พิมพ์ครบแล้ว!";

                            // คำนวณความแม่นยำเมื่อพิมพ์ตัวอักษรถูก
                            accuracy = ((double)correctCharCount / availableChars.Count) * 100;
                            accuracyLabel.Text = $"Accuracy: {accuracy:F2}%";
                            ShowThankYouMessage(); // เรียกแสดงข้อความขอบคุณ
                        }

                        // คำนวณความแม่นยำเมื่อพิมพ์ตัวอักษรถูก
                        accuracy = ((double)correctCharCount / availableChars.Count) * 100;
                        accuracyLabel.Text = $"Accuracy: {accuracy:F2}%";
                    }
                    else // ถ้าพิมพ์ผิด
                    {
                        if (!incorrectCharFlags[currentCharIndex] && !correctCharFlags[currentCharIndex])
                        {
                            incorrectCharFlags[currentCharIndex] = true; // เก็บว่าในตำแหน่งนี้พิมพ์ผิด
                        }

                        accuracyLabel.Text = $"Accuracy: {accuracy:F2}% (ผิด! ตัวอักษรปัจจุบัน: {availableChars[currentCharIndex]})";
                    }
                }
            }
        }







        private double CalculateWPM(int correctChars, double timeTaken)
        {
            double wordsTyped = correctChars / 5.0;
            double wpm = (wordsTyped / timeTaken) * 60;
            return wpm;
        }
    }








    //        private List<string> sentences = new List<string>
    //{
    //    "สุนัขจิ้งจอกสีน้ำตาลนะ"
    //};

    //        private int currentSentenceIndex = 0;
    //        private Stopwatch stopwatch;
    //        private int correctChars = 0;
    //        private int typedChars = 0;
    //        private int characterX = 0;  // ตำแหน่ง X ของตัวละคร
    //        private int characterSpeed = 35;  // ความเร็วในการเคลื่อนที่
    //        private int stopPosition = 760; // จุดที่ตัวละครต้องหยุด (ตำแหน่ง X ที่กำหนด)
    //        private Image characterImage;
    //        private bool gameStarted = false; // ใช้ตรวจสอบว่าเกมเริ่มต้นแล้วหรือยัง
    //        private bool isGameOver = false;  // เช็คสถานะเกมว่าเสร็จสิ้นแล้วหรือยัง
    //        private bool isMessageShown = false;  // ตัวแปร Flag เพื่อป้องกันไม่ให้แสดง MessageBox ซ้ำ
    //        private bool isTimeAlertShown = false;  // ตัวแปรเพื่อป้องกันไม่ให้แสดงข้อความแจ้งเตือนเวลา 31 วินาทีซ้ำ
    //        private double accuracy = 0.0;  // ตัวแปรเก็บความแม่นยำ

    //        private Label wpmLabel;
    //        private Label timeLabel;
    //        private Label accuracyLabel;
    //        private TextBox inputTextBox;
    //        private Button resetButton;  // ปุ่ม Reset สำหรับเริ่มเกมใหม่
    //        private Button exitButton;  // ปุ่มออกจากเกม
    //        private Timer gameTimer; // ประกาศ gameTimer

    //        public thankyou()
    //        {
    //            InitializeComponent();

    //            // เปิดใช้งาน Double-Buffering เพื่อลดการกระพริบ
    //            this.DoubleBuffered = true;

    //            // โหลดภาพตัวละคร
    //            characterImage = Image.FromFile("C:\\Users\\MikiBunnie\\OneDrive\\Pictures\\yellowcar.png");

    //            // สร้าง Stopwatch
    //            stopwatch = new Stopwatch();

    //            // ตั้งค่าฟอร์ม
    //            this.Text = "Typing Game with Animation";
    //            this.Size = new Size(1015, 650);
    //            this.StartPosition = FormStartPosition.CenterScreen;

    //            // สร้าง UI Components
    //            Label instructionsLabel = new Label
    //            {
    //                Text = "พิมพ์ข้อความที่แสดงในช่องด้านล่าง:",
    //                Location = new Point(20, 20),
    //                Width = 300,
    //                Font = new Font("Tahoma", 12)
    //            };
    //            this.Controls.Add(instructionsLabel);

    //            Label sentenceLabel = new Label
    //            {
    //                Text = sentences[currentSentenceIndex],
    //                Location = new Point(20, 60),
    //                Width = 600,
    //                Height = 40,
    //                Font = new Font("Tahoma", 14, FontStyle.Bold)
    //            };
    //            this.Controls.Add(sentenceLabel);

    //            inputTextBox = new TextBox
    //            {
    //                Location = new Point(20, 120),
    //                Width = 600,
    //                Font = new Font("Tahoma", 14)
    //            };
    //            this.Controls.Add(inputTextBox);

    //            wpmLabel = new Label
    //            {
    //                Text = "WPM: 0",
    //                Location = new Point(20, 160),
    //                Width = 200,
    //                Font = new Font("Tahoma", 12)
    //            };
    //            this.Controls.Add(wpmLabel);

    //            timeLabel = new Label
    //            {
    //                Text = "Time: 0s",
    //                Location = new Point(20, 200),
    //                Width = 200,
    //                Font = new Font("Tahoma", 12)
    //            };
    //            this.Controls.Add(timeLabel);

    //            accuracyLabel = new Label
    //            {
    //                Text = "Accuracy: 0.00%",
    //                Location = new Point(20, 240),
    //                Width = 200,
    //                Font = new Font("Tahoma", 12)
    //            };
    //            this.Controls.Add(accuracyLabel);

    //            // เมื่อพิมพ์ใน TextBox
    //            inputTextBox.TextChanged += (sender, e) =>
    //            {
    //                if (!gameStarted && inputTextBox.Text.Length > 0)
    //                {
    //                    stopwatch.Start();  // เริ่มจับเวลาเมื่อพิมพ์ตัวแรก
    //                    gameStarted = true; // ตั้งค่าให้เริ่มเกม
    //                }

    //                string userInput = inputTextBox.Text;

    //                if (userInput.Length == typedChars + 1)
    //                {
    //                    if (userInput[typedChars] == sentences[currentSentenceIndex][typedChars])
    //                    {
    //                        typedChars++;
    //                        correctChars++;

    //                        characterX += characterSpeed;

    //                        int elapsedSeconds = (int)Math.Floor(stopwatch.Elapsed.TotalSeconds);

    //                        if (typedChars == sentences[currentSentenceIndex].Length)
    //                        {
    //                            stopwatch.Stop();
    //                            characterX = 0;

    //                            double wpm = CalculateWPM(correctChars, elapsedSeconds);
    //                            wpmLabel.Text = $"WPM: {wpm:F2}";
    //                            timeLabel.Text = $"Time: {elapsedSeconds}s";

    //                            accuracy = (double)correctChars / sentences[currentSentenceIndex].Length * 100;
    //                            accuracyLabel.Text = $"Accuracy: {accuracy:F2}%";

    //                            ShowGameOverMessage();  // เรียกแสดงข้อความเกมเสร็จสิ้น
    //                        }
    //                    }
    //                }

    //                this.Invalidate();
    //            };

    //            // สร้าง gameTimer
    //            gameTimer = new Timer();
    //            gameTimer.Interval = 1000; // 1 วินาที
    //            gameTimer.Tick += (sender, e) =>
    //            {
    //                if (isGameOver || isMessageShown)
    //                {
    //                    return;
    //                }

    //                int elapsedSeconds = (int)Math.Floor(stopwatch.Elapsed.TotalSeconds);

    //                if (elapsedSeconds >= 31 && characterX < stopPosition && !isTimeAlertShown && !isGameOver)
    //                {
    //                    stopwatch.Stop();
    //                    characterX = 0;
    //                    gameTimer.Stop(); // หยุดจับเวลา

    //                    // แสดงข้อความ "ลองใหม่" เฉพาะเมื่อยังไม่แสดง
    //                    if (!isTimeAlertShown)
    //                    {
    //                        MessageBox.Show("ลองใหม่! ตัวละครยังไม่ถึงจุดที่กำหนดภายในเวลาที่กำหนด!");
    //                        isTimeAlertShown = true;  // ตั้งค่าว่าแสดงข้อความแล้ว
    //                    }

    //                    inputTextBox.Enabled = false;  // ปิด TextBox
    //                    ShowResetButton();  // แสดงปุ่มรีเซ็ต
    //                    ShowExitButton();  // แสดงปุ่มออกจากเกม
    //                    isGameOver = true; // เปลี่ยนสถานะเกมว่าเสร็จแล้ว
    //                }

    //                if (gameStarted && !isGameOver)
    //                {
    //                    timeLabel.Text = $"Time: {elapsedSeconds}s";
    //                }

    //                this.Invalidate();
    //            };
    //            gameTimer.Start();
    //        }

    //        // คำนวณ WPM
    //        private double CalculateWPM(int correctChars, double timeTaken)
    //        {
    //            double wordsTyped = correctChars / 5.0;
    //            double wpm = (wordsTyped / timeTaken) * 60;
    //            return wpm;
    //        }

    //        // วาดกราฟิก
    //        protected override void OnPaint(PaintEventArgs e)
    //        {
    //            base.OnPaint(e);

    //            int characterWidth = 200;
    //            int characterHeight = 130;

    //            e.Graphics.DrawImage(characterImage, characterX, 260, characterWidth, characterHeight);
    //            e.Graphics.DrawString(sentences[currentSentenceIndex], new Font("Arial", 14), Brushes.Black, new Point(20, 60));
    //        }

    //        // รีเซ็ตเกม
    //        private void ResetGame()
    //        {
    //            isMessageShown = false; // รีเซ็ตตัวแปรให้ไม่แสดงข้อความซ้ำ
    //            isGameOver = false;
    //            isTimeAlertShown = false; // รีเซ็ตตัวแปร Flag ของการแสดงข้อความแจ้งเตือนเวลา

    //            currentSentenceIndex = 0;
    //            characterX = 0;
    //            typedChars = 0;
    //            correctChars = 0;
    //            accuracy = 0.0;

    //            stopwatch.Reset(); // รีเซ็ตตัวจับเวลา
    //            gameStarted = false; // รีเซ็ตสถานะเกม
    //            inputTextBox.Enabled = true; // เปิดใช้งาน TextBox
    //            inputTextBox.Clear(); // ล้างข้อความใน TextBox

    //            timeLabel.Text = "Time: 0s";  // อัพเดตเวลาที่แสดงใน label
    //            wpmLabel.Text = "WPM: 0.00";
    //            accuracyLabel.Text = "Accuracy: 0.00%";

    //            // ลบปุ่ม "ออกจากเกม" ถ้ามี
    //            if (exitButton != null)
    //            {
    //                this.Controls.Remove(exitButton);
    //                exitButton = null;
    //            }

    //            // ลบปุ่ม "รีเซ็ต" ถ้ามี
    //            if (resetButton != null)
    //            {
    //                this.Controls.Remove(resetButton);
    //                resetButton = null;
    //            }

    //            gameTimer.Start();  // เริ่มจับเวลาใหม่หลังจากการรีเซ็ต
    //        }

    //        // แสดงปุ่มรีเซ็ต
    //        private void ShowResetButton()
    //        {
    //            resetButton = new Button
    //            {
    //                Text = "เริ่มเกมใหม่",
    //                Location = new Point(350, 500),
    //                Width = 100,
    //                Height = 40
    //            };
    //            resetButton.Click += (sender, e) =>
    //            {
    //                ResetGame();
    //            };

    //            this.Controls.Add(resetButton);
    //        }

    //        // แสดงปุ่มออกจากเกม
    //        private void ShowExitButton()
    //        {
    //            exitButton = new Button
    //            {
    //                Text = "ออกจากเกม",
    //                Location = new Point(460, 500),
    //                Width = 100,
    //                Height = 40
    //            };
    //            exitButton.Click += (sender, e) =>
    //            {
    //                this.Close();
    //            };

    //            this.Controls.Add(exitButton);
    //        }

    //        // แสดงข้อความเกมเสร็จสิ้น
    //        private void ShowGameOverMessage()
    //        {
    //            MessageBox.Show("เกมจบแล้ว! คุณพิมพ์ข้อความได้ครบแล้ว");
    //            inputTextBox.Enabled = false;  // ปิด TextBox
    //            ShowResetButton();
    //            ShowExitButton();
    //        }








}


