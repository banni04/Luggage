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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Luggage
{
    public partial class homeadmin : Form
    {
        
        public homeadmin()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            homepage homepage = new homepage();
            homepage.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            showinfo showinfo = new showinfo();
            showinfo.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dropoff dropoff = new dropoff();
            dropoff.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            history history = new history();
            history.Show();
            this.Hide();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            pickup pickup = new pickup();
            pickup.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            customermember customermember = new customermember();
            customermember.Show();
            this.Hide();
        }
    }
}
