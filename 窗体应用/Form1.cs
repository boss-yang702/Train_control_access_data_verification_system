using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 窗体应用
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            DialogResult f=frm2.ShowDialog();
            
            if (f == DialogResult.OK)
            {
                MessageBox.Show("窗体对话框中选择了是");
            }
            else if(f==DialogResult.Cancel)
            {
                MessageBox.Show("取消了");
            }
            else
            {
                MessageBox.Show("YSE");
            }
            
        }
    }
}
