using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;


namespace 项目方案第一版
{
    public partial class 导入文件 : Form
    {
        public 导入文件()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Manager.Load_file_daocha_info(textBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Manager.Load_file_xianlu_info(textBox2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Manager.Load_file_yindaqi_info(textBox3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
