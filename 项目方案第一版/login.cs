using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';   //显示输入
            }
            else
            {
                textBox2.PasswordChar = '*';   //显示*
            }
    }

        private void label3_Click(object sender, EventArgs e)
        {
            string str = Directory.GetCurrentDirectory();
            string path1 = @str+ "\\登录账号密码.txt";  //打开D盘下的log.txt文件
            System.Diagnostics.Process.Start(path1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
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
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
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
        }
    }
}
