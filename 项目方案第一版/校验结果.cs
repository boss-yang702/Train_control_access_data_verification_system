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

namespace 项目方案第一版
{
    public partial class 校验结果 : Form
    {
        public 校验结果()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 校验结果_Load(object sender, EventArgs e)
        {
            string a;
            DataTable b = MainWindow.dt;
            textBox1.Text = MainWindow.at;
            textBox1.Text += "校验表";
            dataGridView1.DataSource = b;
            dataGridView1.Rows[3].Cells[3].Style.BackColor = Color.FromName("Red");
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
