namespace Luggage
{
    partial class checkinfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(checkinfo));
            this.dataLuggage = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataLuggage)).BeginInit();
            this.SuspendLayout();
            // 
            // dataLuggage
            // 
            this.dataLuggage.AllowUserToOrderColumns = true;
            this.dataLuggage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataLuggage.Location = new System.Drawing.Point(40, 202);
            this.dataLuggage.Margin = new System.Windows.Forms.Padding(2);
            this.dataLuggage.Name = "dataLuggage";
            this.dataLuggage.RowHeadersWidth = 51;
            this.dataLuggage.RowTemplate.Height = 24;
            this.dataLuggage.Size = new System.Drawing.Size(1017, 140);
            this.dataLuggage.TabIndex = 4;
            this.dataLuggage.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataLuggage_CellClick);
       
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateBlue;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(21, 25);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 67);
            this.button2.TabIndex = 5;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkinfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1094, 699);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataLuggage);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "checkinfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Checkinfo";
            this.Load += new System.EventHandler(this.checkinfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataLuggage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataLuggage;
        private System.Windows.Forms.Button button2;
    }
}