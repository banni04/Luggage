namespace Luggage
{
    partial class request
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(request));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cname = new System.Windows.Forms.TextBox();
            this.ctel = new System.Windows.Forms.TextBox();
            this.cusername = new System.Windows.Forms.TextBox();
            this.cpassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Dubai", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(28, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "HOME";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(930, 539);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "EDIT";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cname
            // 
            this.cname.Location = new System.Drawing.Point(152, 231);
            this.cname.Name = "cname";
            this.cname.Size = new System.Drawing.Size(100, 20);
            this.cname.TabIndex = 3;
            // 
            // ctel
            // 
            this.ctel.Location = new System.Drawing.Point(366, 231);
            this.ctel.Name = "ctel";
            this.ctel.Size = new System.Drawing.Size(100, 20);
            this.ctel.TabIndex = 4;
            // 
            // cusername
            // 
            this.cusername.Location = new System.Drawing.Point(152, 363);
            this.cusername.Name = "cusername";
            this.cusername.Size = new System.Drawing.Size(100, 20);
            this.cusername.TabIndex = 5;
            // 
            // cpassword
            // 
            this.cpassword.Location = new System.Drawing.Point(152, 418);
            this.cpassword.Name = "cpassword";
            this.cpassword.Size = new System.Drawing.Size(100, 20);
            this.cpassword.TabIndex = 6;
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(910, 30);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(100, 20);
            this.textBoxUsername.TabIndex = 7;
            this.textBoxUsername.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // request
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1084, 661);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.cpassword);
            this.Controls.Add(this.cusername);
            this.Controls.Add(this.ctel);
            this.Controls.Add(this.cname);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "request";
            this.Text = "request";
            this.Load += new System.EventHandler(this.request_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox cname;
        private System.Windows.Forms.TextBox ctel;
        private System.Windows.Forms.TextBox cusername;
        private System.Windows.Forms.TextBox cpassword;
        private System.Windows.Forms.TextBox textBoxUsername;
    }
}