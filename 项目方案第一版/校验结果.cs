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
using static 项目方案第一版.guidaoquduan;

namespace 项目方案第一版
{
    public partial class 校验结果 : Form
    {

        public 校验结果()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Maximized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox9.Text = null;
            for (int i = 0; i < MainWindow.strings1.Length - 1; i++)
            {
                this.textBox9.Text = this.textBox9.Text + MainWindow.strings1[i]+ "\r\n"+ "\r\n";
            }
            for (int i = 0; i < MainWindow.strings4.Length - 1; i++)
            {
                this.textBox2.Text = this.textBox2.Text + MainWindow.strings4[i] + "\r\n" + "\r\n";
            }



            //int M = 3;
            //guidaoquduan.fengzhuang_jinluxinxibiao(Manager.chengzhanming_1);
            //textBox2.Text = (guidaoquduan.xinxi[x].hangshu +" "+ guidaoquduan.xinxi[x].changdu+" "+guidaoquduan.xinxi[x].zaipin+" "+guidaoquduan.xinxi[x].xinhaoji+" " + guidaoquduan.xinxi[x].mingcheng_quduan + " " + guidaoquduan.xinxi[x].daochaweizhi);
            //guidaoquduan.fengzhuang_duizhao();
            //fengzhuang_zaipinbiao();
            //textBox3.Text = (guidaoquduan.zaipinduizhao[2000].zaipin);
            //textBox2.Text = (guidaoquduan.zaipinduizhao[2000].quduan);

            //for (int x = 0; x < duizhao.Length; x++)
            //     if (duizhao[x].chezhanming == xinxi[M].chezhanming && duizhao[x].mingcheng_quduan == xinxi[M].mingcheng_quduan && duizhao[x].daocha_weizhi == xinxi[M].daochaweizhi)
            //     {

            //         textBox9.Text = (guidaoquduan.duizhao[x].chezhanming);
            //         textBox6.Text = (guidaoquduan.xinxi[M].chezhanming);
            //         textBox2.Text = (guidaoquduan.xinxi[M].mingcheng_quduan);
            //         textBox10.Text = (guidaoquduan.duizhao[x].mingcheng_quduan);
            //         textBox8.Text = (guidaoquduan.xinxi[M].daochaweizhi);
            //         textBox11.Text = (guidaoquduan.duizhao[x].daocha_weizhi);
            //         textBox3.Text = (guidaoquduan.xinxi[M].zaipin);
            //         textBox4.Text = (guidaoquduan.xinxi[M].xinhaoji);
            //         textBox5.Text = (Convert.ToString(guidaoquduan.xinxi[M].changdu));
            //         textBox14.Text = (Convert.ToString(guidaoquduan.duizhao[x].changdu));
            //         textBox7.Text = (Convert.ToString(guidaoquduan.xinxi[M].hangshu-2));

            //     }
            //DataTable b = MainWindow.dt;
            //textBox1.Text = MainWindow.at;
            //textBox1.Text += "校验表";
            //dataGridView1.DataSource = b;
            ////dataGridView1.Rows[3].Cells[3].Style.BackColor = Color.FromName("Red");
        }

        private void 校验结果_Load(object sender, EventArgs e)
        {
           
            //DataTable b = MainWindow.dt;
            //textBox1.Text = MainWindow.at;
            //textBox1.Text += "校验表";
            //dataGridView1.DataSource = b;
            //dataGridView1.Rows[3].Cells[3].Style.BackColor = Color.FromName("Red");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            Manager.Search(dataGridView1, textBox15, e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 校验结果_Load_1(object sender, EventArgs e)
        {

        }

        //private void 校验结果_Load(object sender, EventArgs e)
        //{

        //}
    }
}
