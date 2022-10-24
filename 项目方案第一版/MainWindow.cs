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
        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度
        public MainWindow()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            setTag(this);
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
        private void setControls(float newx, float newy, Control cons)
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
        private string SelectPath()

        {
            string path = string.Empty;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Files (*.xls)|*.xls|(*.xlsx)|*.xlsx"//如果需要筛选txt文件（"Files (*.txt)|*.txt"）
                //Filter = "Files (全部文件)|*.*"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                path = openFileDialog.FileName;
            }
            return path;
        }
        public static DataTable ReadExcel(string filePath)
        {
            try
            {
                //创建连接，引用协议
                string strConn;
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel8.0;HDR=False;IMEX=1'";
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + filePath + ";Extended Properties='Excel 12.0; HDR=NO;IMEX=1'";//此连接可以操作.xls与.xlsx文件
                OleDbConnection OleConn = new OleDbConnection(strConn);
                OleConn.Open();
                String sql = "SELECT* FROM[Sheet1$]";//可是更改Sheet名称，比如sheet2，等等 
                OleDbDataAdapter OleDaExcel = new OleDbDataAdapter(sql, OleConn);
                DataSet OleDsExcle = new DataSet();
                OleDaExcel.Fill(OleDsExcle, "Sheet1");
                OleConn.Close();
                return OleDsExcle.Tables["Sheet1"];
            }
            catch (Exception err)
            {
                MessageBox.Show("数据绑定Excel失败!失败原因：" + err.Message, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            导入文件 form2 = new 导入文件();
            form2.ShowDialog();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }//窗口大小自动调整

        private void button3_Click(object sender, EventArgs e)//进路数据导入
        {
            Manager.Load_file_jinluinfo(this.dataGridView2);

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
            }
            else
            {
                校验结果 form3 = new 校验结果();
                timer1.Enabled = false;
                Thread.Sleep(300);
                form3.ShowDialog();
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            string a;
            if (e.KeyValue == 13)
            {
                for (int i = 3; i <dataGridView2.RowCount; i++)
                {
                    if (i > 0)
                    {
                        //a = this.dataGridView2.Rows[i].Cells[3].Value.ToString().Trim();
                        a = Convert.ToString(dataGridView2[3, i].Value);
                        if (textBox2.Text.ToString().Trim() == a)
                        {
                            dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.FromName("Skyblue");
                        }
                        if(textBox2.Text=="")
                        {
                            dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.FromName("White");
                        }
                    }
                }
                //dataGridView2.Rows[3].DefaultCellStyle.BackColor = Color.FromName("Skyblue");
            }
        }
    }
}
