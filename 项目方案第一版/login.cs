using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 项目方案第一版
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string userName = this.textBox1.Text;
            string userPassword = this.textBox2.Text;
            if (userName.Equals("") || userPassword.Equals(""))
            {
                MessageBox.Show("用户名或密码不能为空！");
            }

            else
            {
                //用户名和密码验证正确，提示成功，并执行跳转界面。
                if (userName.Equals("123") && userPassword.Equals("123"))
                {
                    MessageBox.Show("登录成功！");
                    //跳转主界面
                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                    this.Close();
                }
                //用户名和密码验证错误，提示错误。
                else
                {
                    MessageBox.Show("用户名或密码错误！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
