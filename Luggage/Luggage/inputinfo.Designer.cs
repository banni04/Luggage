namespace Luggage
{
    partial class inputinfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(inputinfo));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button5 = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.textBoxDaysDifference = new System.Windows.Forms.TextBox();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.special = new System.Windows.Forms.Label();
            this.regular = new System.Windows.Forms.Label();
            this.minusspecial = new System.Windows.Forms.Button();
            this.plusspecial = new System.Windows.Forms.Button();
            this.minusregular = new System.Windows.Forms.Button();
            this.plusregular = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.summarize = new System.Windows.Forms.Label();
            this.summarize2 = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelTotalPrice = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.ItemHeight = 27;
            this.comboBox1.Items.AddRange(new object[] {
            "Suvarnabhumi Airport (BKK)",
            "Don Mueang International Airport (DMK)",
            "MBK Center",
            "Terminal 21 Bangkok",
            "Central World",
            "Siam Paragon",
            "ICONSIAM",
            "Terminal 21 Pattaya",
            "Central Pattaya",
            "Central Samui",
            "Central Airport Plaza Chiang Mai"});
            this.comboBox1.Location = new System.Drawing.Point(275, 79);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(263, 35);
            this.comboBox1.TabIndex = 19;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            this.dateTimePicker1.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(159, 232);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 35);
            this.dateTimePicker1.TabIndex = 20;
            this.dateTimePicker1.Value = new System.DateTime(2024, 10, 10, 0, 0, 0, 0);
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Transparent;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(27, 21);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(93, 69);
            this.button5.TabIndex = 22;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "dd/MM/yyyy";
            this.dateTimePicker2.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Location = new System.Drawing.Point(159, 338);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 35);
            this.dateTimePicker2.TabIndex = 23;
            this.dateTimePicker2.Value = new System.DateTime(2024, 10, 9, 0, 0, 0, 0);
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.CustomFormat = "HH:mm";
            this.dateTimePicker3.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(425, 232);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.ShowUpDown = true;
            this.dateTimePicker3.Size = new System.Drawing.Size(83, 35);
            this.dateTimePicker3.TabIndex = 24;
            this.dateTimePicker3.ValueChanged += new System.EventHandler(this.dateTimePicker3_ValueChanged);
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.CustomFormat = "HH:mm";
            this.dateTimePicker4.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Location = new System.Drawing.Point(425, 338);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.ShowUpDown = true;
            this.dateTimePicker4.Size = new System.Drawing.Size(83, 35);
            this.dateTimePicker4.TabIndex = 25;
            this.dateTimePicker4.ValueChanged += new System.EventHandler(this.dateTimePicker4_ValueChanged);
            // 
            // textBoxDaysDifference
            // 
            this.textBoxDaysDifference.Location = new System.Drawing.Point(1055, 647);
            this.textBoxDaysDifference.Multiline = true;
            this.textBoxDaysDifference.Name = "textBoxDaysDifference";
            this.textBoxDaysDifference.Size = new System.Drawing.Size(17, 38);
            this.textBoxDaysDifference.TabIndex = 29;
            this.textBoxDaysDifference.Visible = false;
            this.textBoxDaysDifference.TextChanged += new System.EventHandler(this.textBoxDaysDifference_TextChanged);
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(949, 163);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(65, 23);
            this.buttonCalculate.TabIndex = 30;
            this.buttonCalculate.Text = "button3";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Visible = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(751, 644);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(254, 44);
            this.button3.TabIndex = 32;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.special);
            this.panel1.Controls.Add(this.regular);
            this.panel1.Controls.Add(this.minusspecial);
            this.panel1.Controls.Add(this.plusspecial);
            this.panel1.Controls.Add(this.minusregular);
            this.panel1.Controls.Add(this.plusregular);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.dateTimePicker4);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.dateTimePicker3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(41, 226);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 467);
            this.panel1.TabIndex = 33;
            // 
            // special
            // 
            this.special.AutoSize = true;
            this.special.BackColor = System.Drawing.Color.White;
            this.special.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.special.Font = new System.Drawing.Font("Dubai", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.special.Location = new System.Drawing.Point(441, 734);
            this.special.Name = "special";
            this.special.Size = new System.Drawing.Size(35, 45);
            this.special.TabIndex = 28;
            this.special.Text = "0";
            // 
            // regular
            // 
            this.regular.AutoSize = true;
            this.regular.BackColor = System.Drawing.Color.White;
            this.regular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.regular.Font = new System.Drawing.Font("Dubai", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regular.Location = new System.Drawing.Point(441, 568);
            this.regular.Name = "regular";
            this.regular.Size = new System.Drawing.Size(35, 45);
            this.regular.TabIndex = 27;
            this.regular.Text = "0";
            // 
            // minusspecial
            // 
            this.minusspecial.BackColor = System.Drawing.Color.White;
            this.minusspecial.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.minusspecial.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.minusspecial.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.minusspecial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minusspecial.Font = new System.Drawing.Font("Dubai", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minusspecial.Location = new System.Drawing.Point(498, 757);
            this.minusspecial.Name = "minusspecial";
            this.minusspecial.Size = new System.Drawing.Size(42, 48);
            this.minusspecial.TabIndex = 26;
            this.minusspecial.Text = "-";
            this.minusspecial.UseVisualStyleBackColor = false;
            this.minusspecial.Click += new System.EventHandler(this.minusspecial_Click);
            // 
            // plusspecial
            // 
            this.plusspecial.BackColor = System.Drawing.Color.White;
            this.plusspecial.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.plusspecial.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.plusspecial.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.plusspecial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusspecial.Font = new System.Drawing.Font("Dubai", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plusspecial.Location = new System.Drawing.Point(498, 705);
            this.plusspecial.Name = "plusspecial";
            this.plusspecial.Size = new System.Drawing.Size(42, 46);
            this.plusspecial.TabIndex = 25;
            this.plusspecial.Text = "+";
            this.plusspecial.UseVisualStyleBackColor = false;
            this.plusspecial.Click += new System.EventHandler(this.plusspecial_Click);
            // 
            // minusregular
            // 
            this.minusregular.BackColor = System.Drawing.Color.White;
            this.minusregular.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.minusregular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.minusregular.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.minusregular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minusregular.Font = new System.Drawing.Font("Dubai", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minusregular.Location = new System.Drawing.Point(498, 592);
            this.minusregular.Name = "minusregular";
            this.minusregular.Size = new System.Drawing.Size(42, 45);
            this.minusregular.TabIndex = 24;
            this.minusregular.Text = "-";
            this.minusregular.UseVisualStyleBackColor = false;
            this.minusregular.Click += new System.EventHandler(this.minusregular_Click);
            // 
            // plusregular
            // 
            this.plusregular.BackColor = System.Drawing.Color.White;
            this.plusregular.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.plusregular.FlatAppearance.BorderSize = 0;
            this.plusregular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.plusregular.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.plusregular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plusregular.Font = new System.Drawing.Font("Dubai", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plusregular.Location = new System.Drawing.Point(498, 537);
            this.plusregular.Name = "plusregular";
            this.plusregular.Size = new System.Drawing.Size(42, 38);
            this.plusregular.TabIndex = 23;
            this.plusregular.Text = "+";
            this.plusregular.UseVisualStyleBackColor = false;
            this.plusregular.Click += new System.EventHandler(this.plusregular_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(0, -23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(615, 900);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Dubai", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(838, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 22);
            this.label1.TabIndex = 37;
            this.label1.Text = "1111";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Dubai", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(822, 346);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 22);
            this.label2.TabIndex = 38;
            this.label2.Text = "1111";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Dubai", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(822, 387);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 22);
            this.label3.TabIndex = 39;
            this.label3.Text = "11111";
            // 
            // summarize
            // 
            this.summarize.AutoSize = true;
            this.summarize.BackColor = System.Drawing.Color.Transparent;
            this.summarize.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summarize.Location = new System.Drawing.Point(755, 466);
            this.summarize.Name = "summarize";
            this.summarize.Size = new System.Drawing.Size(0, 27);
            this.summarize.TabIndex = 40;
            // 
            // summarize2
            // 
            this.summarize2.AutoSize = true;
            this.summarize2.BackColor = System.Drawing.Color.Transparent;
            this.summarize2.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.summarize2.Location = new System.Drawing.Point(755, 516);
            this.summarize2.Name = "summarize2";
            this.summarize2.Size = new System.Drawing.Size(0, 27);
            this.summarize2.TabIndex = 41;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.BackColor = System.Drawing.Color.Black;
            this.labelUsername.Font = new System.Drawing.Font("Dubai", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsername.ForeColor = System.Drawing.Color.White;
            this.labelUsername.Location = new System.Drawing.Point(969, 39);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelUsername.Size = new System.Drawing.Size(0, 32);
            this.labelUsername.TabIndex = 42;
            this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTotalPrice
            // 
            this.labelTotalPrice.AutoSize = true;
            this.labelTotalPrice.BackColor = System.Drawing.Color.Transparent;
            this.labelTotalPrice.Font = new System.Drawing.Font("Dubai", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotalPrice.ForeColor = System.Drawing.Color.Black;
            this.labelTotalPrice.Location = new System.Drawing.Point(944, 585);
            this.labelTotalPrice.Name = "labelTotalPrice";
            this.labelTotalPrice.Size = new System.Drawing.Size(48, 27);
            this.labelTotalPrice.TabIndex = 43;
            this.labelTotalPrice.Text = "1111";
            // 
            // inputinfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1094, 699);
            this.Controls.Add(this.labelTotalPrice);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.summarize2);
            this.Controls.Add(this.summarize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBoxDaysDifference);
            this.Controls.Add(this.button5);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "inputinfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inputinfo";
            this.Load += new System.EventHandler(this.inputinfo_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.TextBox textBoxDaysDifference;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label special;
        private System.Windows.Forms.Label regular;
        private System.Windows.Forms.Button minusspecial;
        private System.Windows.Forms.Button plusspecial;
        private System.Windows.Forms.Button minusregular;
        private System.Windows.Forms.Button plusregular;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label summarize;
        private System.Windows.Forms.Label summarize2;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelTotalPrice;
    }
}