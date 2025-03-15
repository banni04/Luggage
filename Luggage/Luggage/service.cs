using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luggage
{
    public partial class service : Form
    {
        

        public service()
        {
            InitializeComponent();
        }


        //ฟังก์ชันปุ่มต่างๆเมื่อคลิกก็จะไปหน้า...
        private void button1_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            signin signin = new signin();
            signin.Show();
            this.Hide();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            signin2 signin2 = new signin2();
            signin2.Show();
            this.Hide();
        }
    }
}
