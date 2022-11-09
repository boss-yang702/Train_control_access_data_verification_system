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
        }

        private void 查看已导入文件_Load(object sender, EventArgs e)
        {
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
            if (Current_index < ds.Tables.Count-1)
            {
                Current_index++;
                dataGridView1.DataSource=ds.Tables[Current_index];
                label2.Text = ds.Tables[Current_index].TableName;
            }
            
        }
        void Lasttable()
        {
            if (Current_index > 0)
            {
                Current_index--;
                dataGridView1.DataSource = ds.Tables[Current_index];
                label2.Text = ds.Tables[Current_index].TableName;
            }

        }

       
    }
}
