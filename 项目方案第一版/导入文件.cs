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
        private DataSet ds;
        private int Current_index;
        public 导入文件()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds_ = ds;
            Manager.Load_file(textBox1,ref ds);
            if (ds != ds_)
            {
                Current_index = 0;
                dataGridView1.DataSource = ds.Tables[Current_index];
                label3.Text = ds.Tables[Current_index].TableName;
            }
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
            string DsName = textBox2.Text;
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

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Current_index = (Current_index - 1 + ds.Tables.Count) % ds.Tables.Count;
            this.dataGridView1.DataSource = ds.Tables[Current_index];
            label3.Text = ds.Tables[Current_index].TableName;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Current_index = (Current_index +1) % ds.Tables.Count;
            this.dataGridView1.DataSource = ds.Tables[Current_index];
            label3.Text = ds.Tables[Current_index].TableName;
        }
    }
}
