﻿using System;
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
    public partial class 背景颜色 : Form
    {
        public 背景颜色()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            label1.Text = "LightSalmon";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            label1.Text = "SkyBlue";

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            label1.Text = "PaleGreen";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
