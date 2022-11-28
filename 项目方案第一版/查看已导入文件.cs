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
    public partial class 查看已导入文件 : Form
    {
        public 查看已导入文件()
        {
            InitializeComponent();
            label3.Show();
            label4.Show();

        }

        private void 查看已导入文件_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach(string item in Manager.DataSets.Keys)
            {
                listBox1.Items.Add(item);               
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {

        }
        int Current_index = 0;
        DataSet ds;
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            label3.Hide();
            label4.Hide();
            ds = Manager.DataSets[listBox1.SelectedItem.ToString()];
            Current_index = 0;
            dataGridView1.DataSource = ds.Tables[Current_index];
            label1.Text = ds.DataSetName;
           
        }

        private void 查看已导入文件_KeyUp(object sender, KeyEventArgs e)
        {
            int keyvalue = e.KeyValue;
            
        }

        void Nexttable()
        {

            try
            {
                if (ds != null)
                {
                    if (Current_index < ds.Tables.Count - 1)
                    {
                        Current_index++;
                        dataGridView1.DataSource = ds.Tables[Current_index];
                        label2.Text = ds.Tables[Current_index].TableName;
                    }
                }
            }
            catch
            {
                return;
            }

            
        }
        void Lasttable()
        {
            try
            {
                if (Current_index > 0)
                {
                    Current_index--;
                    dataGridView1.DataSource = ds.Tables[Current_index];
                    label2.Text = ds.Tables[Current_index].TableName;
                }
            }
            catch
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lasttable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Nexttable();
            }
            catch
            {
                return;
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null) return;
            Manager.DataSets.Remove(listBox1.SelectedItem.ToString());
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void 导入文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            导入文件 form2 = new 导入文件();
            form2.ShowDialog();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            查看已导入文件_Load(sender, e);
        }
    }
}
