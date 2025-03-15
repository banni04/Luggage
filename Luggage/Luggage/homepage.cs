using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;  //ชุดคำสั่งที่จำเป็นในการทำงานของโปรแกรม
using System.Windows.Forms;

namespace Luggage
{
    public partial class homepage : Form
    {
        
        public homepage()
        {
            InitializeComponent();  //สร้างอัตโนมัติเพื่อกำหนดค่า ตั้งค่าคอนโทรลเลอร์ต่างๆบนฟอร์ม

        }

        //ฟังก์ชันปุ่มต่างๆเมื่อกดก็จะไปหน้า...
        private void button1_Click(object sender, EventArgs e)
        {
            service service = new service();
            service.Show();
            this.Hide();
        }

        

        

        private void button4_Click(object sender, EventArgs e)
        {
            mysignin mysignin = new mysignin();
            mysignin.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tracking tracking = new tracking();
            tracking.Show();
            this.Hide();
        }

        

        
    }
}
