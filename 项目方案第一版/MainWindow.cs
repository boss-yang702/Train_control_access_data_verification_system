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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Threading;
using Microsoft.Office.Interop.Excel;
using Font = System.Drawing.Font;
using System.Xml.Linq;

namespace 项目方案第一版
{
    public partial class MainWindow : Form
    {

        public static string[] strings1 = new string[2000];
        public static string[] strings4 = new string[2000];
        public static string at;
        public static bool att;


        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度
        public MainWindow()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            setTag(this);
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            导入文件 form2 = new 导入文件();
            form2.ShowDialog();

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }//窗口大小自动调整
        
        private void button3_Click(object sender, EventArgs e)//进路数据导入
        {

            Manager.Load_file(this.dataGridView2,textBox1);
        }
        //进度条
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
            }
            else
            {
                校验结果 aaa = new 校验结果();
                timer1.Enabled = false;
                Thread.Sleep(300);
                aaa.dataGridView1.DataSource = dataGridView2.DataSource;
                if (aaa.dataGridView1.DataSource != null)
                {
                    aaa.label5.Hide();
                    aaa.label6.Hide();
                }
                aaa.Show();
                if (checkedListBox1.GetItemChecked(3))
                {
                    att = true;
                }
                else
                {
                    att = false;
                }

                if (checkedListBox1.GetItemChecked(0) && !checkedListBox1.GetItemChecked(1) && !checkedListBox1.GetItemChecked(2))
                {
                    Examine_yingdaqi.start_exam(textBox1.Text, aaa.dataGridView1);
                }
                if (checkedListBox1.GetItemChecked(1)&& !checkedListBox1.GetItemChecked(0)&& !checkedListBox1.GetItemChecked(2))
                {
                    Examine_guidaoquduan.start_exam(textBox1.Text, aaa.dataGridView1);
                    for (int pp = 0; pp < strings1.Length; pp++)
                    {
                        strings1[pp] = "";
                        strings4[pp] = "";
                    }
                    strings1 = (string[])Examine_guidaoquduan.strings.Clone();
                    strings4 = (string[])Examine_guidaoquduan.strings3.Clone();
                }
                if (checkedListBox1.GetItemChecked(2)&&!checkedListBox1.GetItemChecked(0) && !checkedListBox1.GetItemChecked(1))
                {
                    Examine_speed.start_exam(textBox1.Text, aaa.dataGridView1);
                }
                if (checkedListBox1.GetItemChecked(0) && checkedListBox1.GetItemChecked(1) && !checkedListBox1.GetItemChecked(2))
                {
                    Examine_yingdaqi.start_exam(textBox1.Text, aaa.dataGridView1);
                    Examine_guidaoquduan.start_exam(textBox1.Text, aaa.dataGridView1);
                    for (int pp = 0; pp < strings1.Length; pp++)
                    {
                        strings1[pp] = "";
                    }
                    strings1 = (string[])Examine_guidaoquduan.strings.Clone();
                    strings4 = (string[])Examine_guidaoquduan.strings3.Clone();

                }
                if (checkedListBox1.GetItemChecked(0) && checkedListBox1.GetItemChecked(1) && checkedListBox1.GetItemChecked(2))
                {
                    Examine_yingdaqi.start_exam(textBox1.Text, aaa.dataGridView1);
                    Examine_guidaoquduan.start_exam(textBox1.Text, aaa.dataGridView1);
                    for (int pp = 0; pp < strings1.Length; pp++)
                    {
                        strings1[pp] = "";
                    }
                    strings1 = (string[])Examine_guidaoquduan.strings.Clone();
                    strings4 = (string[])Examine_guidaoquduan.strings3.Clone();

                    Examine_speed.start_exam(textBox1.Text, aaa.dataGridView1);
                }
                if (checkedListBox1.GetItemChecked(0) && checkedListBox1.GetItemChecked(2) && !checkedListBox1.GetItemChecked(1))
                {
                    Examine_yingdaqi.start_exam(textBox1.Text, aaa.dataGridView1);
                    Examine_speed.start_exam(textBox1.Text, aaa.dataGridView1);


                }
                if (checkedListBox1.GetItemChecked(1) && checkedListBox1.GetItemChecked(2) && !checkedListBox1.GetItemChecked(0))
                {
                    Examine_guidaoquduan.start_exam(textBox1.Text, aaa.dataGridView1);
                    for (int pp = 0; pp < strings1.Length; pp++)
                    {
                        strings1[pp] = "";
                    }
                    strings1 = (string[])Examine_guidaoquduan.strings.Clone();
                    strings4 = (string[])Examine_guidaoquduan.strings3.Clone();

                    Examine_speed.start_exam(textBox1.Text, aaa.dataGridView1);
                }
                if(!checkedListBox1.GetItemChecked(0) && !checkedListBox1.GetItemChecked(1) && !checkedListBox1.GetItemChecked(2))
                {
                    MessageBox.Show("请勾选校验数据");
                }
                progressBar1.Value = 0;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 25;
            timer1.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)//关闭进路校验窗口
        {
            this.Close();
        }

       private void textBox2_KeyDown(object sender, KeyEventArgs e)//进路数据表进路的搜索功能
        {
            Manager.Search(dataGridView2, textBox2, e);
        }
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)//自适应窗体内控件大小
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }

        private void qqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            label3.Show();

            label6.Show();

        }

        private void 查看导入的具体数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            查看已导入文件 ccc = new 查看已导入文件();
            ccc.ShowDialog();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            //Manager.Haseload();           
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void 使用说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = Directory.GetCurrentDirectory();
            string path1 = @str + "\\软件使用说明.doc";  //打开D盘下的log.txt文件
            System.Diagnostics.Process.Start(path1);
        }

        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            修改密码 bbb = new 修改密码();
            bbb.ShowDialog();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (dataGridView2.DataSource != null)
            {
                label3.Hide();

                label6.Hide();
            }

        }

        private void 改变背景颜色ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 保存文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("保存成功");
        }

        private void 版本解释ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = Directory.GetCurrentDirectory();
            string path1 = @str + "\\解释权.txt";  //打开D盘下的log.txt文件
            System.Diagnostics.Process.Start(path1);
        }

        private void checkAllItemEvent(ItemCheckEventArgs e, CheckedListBox box)
        {
            if (e.Index == 4)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    checkedListBox1.SetItemChecked(0, true);
                    checkedListBox1.SetItemChecked(1, true);
                    checkedListBox1.SetItemChecked(2, true);
                    checkedListBox1.SetItemChecked(3, true);
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    checkedListBox1.SetItemChecked(0, false);
                    checkedListBox1.SetItemChecked(1, false);
                    checkedListBox1.SetItemChecked(2, false);
                    checkedListBox1.SetItemChecked(3, false);
                }
            }
        } 

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            checkAllItemEvent(e, checkedListBox1);
        }
    }
}
