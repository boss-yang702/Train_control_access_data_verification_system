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
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Office.Core;

namespace 项目方案第一版
{
    public partial class 导入文件 : Form
    {
        private DataSet ds;
        private int Current_index;
        public 导入文件()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Manager.Load_file(textBox1,ref ds);
            
            
                Current_index = 0;
                dataGridView1.DataSource = ds.Tables[Current_index];
                label3.Text = ds.Tables[Current_index].TableName;
            
            //if (dataGridView1.DataSource != null)
            //{
            //    label2.Hide();
            //    label4.Hide();
            //    label5.Hide();

            //}
            //else
            //{
            //    label2.Show();
            //    label4.Show();
            //    label5.Show();

            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ds == null) return;
            string DsName = textBox1.Text;
            ds.DataSetName = DsName;
            if (Manager.DataSets.ContainsKey(DsName))
            {
                DialogResult MsgBoxResult;//设置对话框的返回值
                MsgBoxResult = System.Windows.Forms.MessageBox.Show("该文件已导入，是否覆盖？", "这会替换系统中原本的数据集，不覆盖则会显示原本数据", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
                if (MsgBoxResult.ToString() == "Yes")//如果对话框的返回值是YES（按"Y"按钮）
                {
                    //选择了Yes，继续
                    Manager.DataSets.Remove(DsName);
                    Manager.DataSets.Add(DsName, ds);
                    MessageBox.Show("导入成功！");
                   
                }
                if (MsgBoxResult.ToString() == "No")//如果对话框的返回值是NO（按"N"按钮）
                {
                    //选择了No，继续

                    return;
                    
                }
            }
            else
            {//没有重复直接导入
                Manager.DataSets.Add(DsName, ds);
                MessageBox.Show("导入成功！");

            }
            //label2.Show();
            //label4.Show();
            //label5.Show();
            Thread.Sleep(200);

            textBox1.Text = null;
            dataGridView1.DataSource = null;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                Current_index = (Current_index - 1 + ds.Tables.Count) % ds.Tables.Count;
                this.dataGridView1.DataSource = ds.Tables[Current_index];
                label3.Text = ds.Tables[Current_index].TableName;
            }
            catch
            {
                return;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                Current_index = (Current_index + 1) % ds.Tables.Count;
                this.dataGridView1.DataSource = ds.Tables[Current_index];
                label3.Text = ds.Tables[Current_index].TableName;
            }
            catch
            {
                return;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        public  void Select_Directory()
        {

            string path_;
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择存放线路数据表，道岔信息表等文件夹";
            if (dilog.ShowDialog() == DialogResult.OK)
            {
                path_ = dilog.SelectedPath;
                string[] strPaths = Directory.GetFiles(path_, "*.XLS");
                string[] strPaths_ = Directory.GetFiles(path_, "*.xlsx");
                strPaths = (strPaths.Concat(strPaths_)).ToArray();
                foreach (string path in strPaths)
                {
                    string filename = System.IO.Path.GetFileName(path);
                    if (filename.Contains("应答器"))
                    {
                        dss[0] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox5.Text = DsName;
                        dss[0].DataSetName = "怀衡线怀化南至衡阳东站应答器位置表";
                    }
                    if (filename.Contains("道岔"))
                    {
                        dss[1] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox4.Text = DsName;
                        dss[1].DataSetName = "怀衡线怀化南至衡阳东站道岔信息表";
                    }
                    if (filename.Contains("始终端信号机"))
                    {
                        dss[2] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox3.Text = DsName;
                        dss[2].DataSetName = "怀衡线怀化南至衡阳东站始终端信号机信息表";
                    }
                    if (filename.Contains("线路数据"))
                    {
                        dss[3] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox2.Text = DsName;
                        dss[3].DataSetName = "怀衡线怀化南至衡阳东站线路数据表";
                    }
                    if (filename.Contains("轨道区段"))
                    {
                        dss[4] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox6.Text = DsName;
                        dss[4].DataSetName = "站内轨道区段信息表";
                    }
                }
            }
        }
        public void Load_file()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "所有文件|*.*";
            ofd.Multiselect = true;
            //文件绝对路径
            string[] strPaths;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPaths = ofd.FileNames;

                foreach(string path in strPaths)
                {
                    string filename = System.IO.Path.GetFileName(path);
                    if (filename.Contains("应答器"))
                    {
                        dss[0] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox5.Text = DsName;
                        dss[0].DataSetName = "怀衡线怀化南至衡阳东站应答器位置表";

                    }
                    if (filename.Contains("道岔"))
                    {
                        dss[1] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox4.Text = DsName;
                        dss[1].DataSetName = "怀衡线怀化南至衡阳东站道岔信息表";

                    }
                    if (filename.Contains("始终端信号机"))
                    {
                        dss[2] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox3.Text = DsName;
                        dss[2].DataSetName = "怀衡线怀化南至衡阳东站始终端信号机信息表";

                    }
                    if (filename.Contains("线路数据"))
                    {
                        dss[3] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox2.Text = DsName;
                        dss[3].DataSetName = "怀衡线怀化南至衡阳东站线路数据表";
                    }
                    if (filename.Contains("轨道区段"))
                    {
                        dss[4] = Manager.ImportExcel(path);
                        int index2 = filename.LastIndexOf('表');
                        string DsName = filename.Substring(0, index2 + 1);
                        textBox6.Text = DsName;
                        dss[4].DataSetName = "站内轨道区段信息表";
                    }
                }
                
            }
        }
         static DataSet[] dss = new DataSet[5];
        private void button6_Click(object sender, EventArgs e)
        {
            Load_file();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach( DataSet ds in dss)
            {
                if (ds!=null)
                {
                    try
                    {
                        Manager.DataSets.Add(ds.DataSetName, ds);
                    }
                    catch
                    {
                        DialogResult MsgBoxResult;//设置对话框的返回值
                        MsgBoxResult = System.Windows.Forms.MessageBox.Show(ds.DataSetName+"已导入，是否覆盖？", "这会替换系统中原本的数据集，不覆盖则会显示原本数据", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
                        if (MsgBoxResult.ToString() == "Yes")//如果对话框的返回值是YES（按"Y"按钮）
                        {
                            Manager.DataSets.Remove(ds.DataSetName);
                            Manager.DataSets.Add(ds.DataSetName, ds);
                        }

                    }
                }
            }
            MessageBox.Show("导入成功！");
            Thread.Sleep(200);
            dataGridView1.DataSource = null;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Select_Directory();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dss[0] == null) return;
            ds = dss[0];
            textBox1.Text = ds.DataSetName;
            Current_index = 0;
            dataGridView1.DataSource = ds.Tables[Current_index];
            label3.Text = ds.Tables[Current_index].TableName;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dss[1] == null) return;
            ds = dss[1];
            textBox1.Text = ds.DataSetName;
            Current_index = 0;
            dataGridView1.DataSource = ds.Tables[Current_index];
            label3.Text = ds.Tables[Current_index].TableName;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dss[2] == null) return;
            ds = dss[2];
            textBox1.Text = ds.DataSetName;
            Current_index = 0;
            dataGridView1.DataSource = ds.Tables[Current_index];
            label3.Text = ds.Tables[Current_index].TableName;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dss[3] == null) return;

            ds = dss[3];
            textBox1.Text = ds.DataSetName;
            Current_index = 0;
            dataGridView1.DataSource = ds.Tables[Current_index];
            label3.Text = ds.Tables[Current_index].TableName;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (dss[4] == null) return;

            ds = dss[4];
            textBox1.Text = ds.DataSetName;
            Current_index = 0;
            dataGridView1.DataSource = ds.Tables[Current_index];
            label3.Text = ds.Tables[Current_index].TableName;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null)
            {
                label2.Hide();
            }
            if (dataGridView1.DataSource == null)
            {
                label2.Show();
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox5, ref dss[0]);
            dss[0].DataSetName = "怀衡线怀化南至衡阳东站应答器位置表";

        }

        private void button19_Click(object sender, EventArgs e)
        {
            Acknowledge(dss[0]);
        }
        void Acknowledge(DataSet ds)
        {
            if (ds == null) return;
            try
            {
                Manager.DataSets.Add(ds.DataSetName, ds);
                MessageBox.Show("导入成功！");
            }
            catch
            {
                DialogResult MsgBoxResult;//设置对话框的返回值
                MsgBoxResult = System.Windows.Forms.MessageBox.Show(ds.DataSetName + "已导入，是否覆盖？", "这会替换系统中原本的数据集，不覆盖则会显示原本数据", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
                if (MsgBoxResult.ToString() == "Yes")//如果对话框的返回值是YES（按"Y"按钮）
                {
                    Manager.DataSets.Remove(ds.DataSetName);
                    Manager.DataSets.Add(ds.DataSetName, ds);
                    MessageBox.Show("导入成功！");

                }

            }
            
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox4, ref dss[1]);
            dss[1].DataSetName = "怀衡线怀化南至衡阳东站道岔信息表";

        }

        private void button20_Click(object sender, EventArgs e)
        {
            Acknowledge(dss[1]);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox3, ref dss[2]);
            dss[2].DataSetName = "怀衡线怀化南至衡阳东站始终端信号机信息表";

        }

        private void button21_Click(object sender, EventArgs e)
        {
            Acknowledge(dss[2]);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox2, ref dss[3]);
            dss[3].DataSetName = "怀衡线怀化南至衡阳东站线路数据表";

        }

        private void button22_Click(object sender, EventArgs e)
        {
            Acknowledge(dss[3]);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox6, ref dss[4]);
            dss[4].DataSetName = "站内轨道区段信息表";

        }

        private void button23_Click(object sender, EventArgs e)
        {
            Acknowledge(dss[4]);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
