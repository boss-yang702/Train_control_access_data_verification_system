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

namespace 项目方案第一版
{
    public partial class MainWindow : Form
    {
        public static DataTable main_dt;
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
        
        //导入文件按钮
        private void button3_Click(object sender, EventArgs e)//进路数据导入
        {
            
            Manager.Load_file(this.dataGridView2,textBox1);
            
            //string path;
            //path = SelectPath();
            //string filename = System.IO.Path.GetFileName(path);//文件名  “Default.aspx”
            //textBox1.Text = filename;
            //DataTable dt = ReadExcel(path); ;//存放Excel表的内容到DataTable中
            //dataGridView2.DataSource = dt;
            //textBox2.Text = dataGridView2.Rows[3].Cells[3].Value.ToString();
            //dataGridView2.Rows[3].Cells[2].Style.BackColor = Color.FromName("Red");
            //dataGridView2.Rows[3].DefaultCellStyle.BackColor = Color.FromName("Skyblue");
        }
        //进度条
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    if (progressBar1.Value < progressBar1.Maximum)
        //    {
        //        progressBar1.Value++;
        //    }
        //    else
        //    {
        //        校验结果 form3 = new 校验结果();
        //        timer1.Enabled = false;
        //        Thread.Sleep(300);
        //        form3.ShowDialog();
        //        progressBar1.Value = 0;
        //    }
        //}
        //开始检验按钮
        private void button1_Click(object sender, EventArgs e)
        {
            //progressBar1.Value = 0;
            //progressBar1.Minimum = 0;
            //progressBar1.Maximum = 25;
            //timer1.Enabled = true;
            //Examine_guidaoquduan.start_exam(textBox1.Text);
            //校验结果 result_window = new 校验结果();
            
            //result_window.dataGridView1.DataSource = dataGridView2.DataSource;
            //result_window.Show();
            //Examine_yingdaqi.start_exam(textBox1.Text,result_window.dataGridView1);

            //Examine_speed.start_exam(textBox1.Text, result_window.dataGridView1);
           


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
            Manager.Haseload();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            
        }
    }
}
