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
    public partial class 信息表显示 : Form
    {
        private DataSet ds;
        int Current_index = 0;
        public 信息表显示(DataSet ds)
        {
            InitializeComponent();
            this.ds = ds;
            this.dataGridView1.DataSource = ds.Tables[Current_index];
            label1.Text = ds.Tables[Current_index].TableName;
            label2.Text = ds.DataSetName;
            this.Text = ds.DataSetName;
        }

        private void 信息表显示_Load(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Current_index=(Current_index-1+ds.Tables.Count)%ds.Tables.Count;
            this.dataGridView1.DataSource = ds.Tables[Current_index];
            label1.Text = ds.Tables[Current_index].TableName;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Current_index = (Current_index + 1) % ds.Tables.Count;
            this.dataGridView1.DataSource = ds.Tables[Current_index];
            label1.Text = ds.Tables[Current_index].TableName;

        }
    }
}
