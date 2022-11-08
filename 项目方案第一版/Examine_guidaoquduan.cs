﻿using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace 项目方案第一版
{
    internal class Examine_guidaoquduan:Manager
    {
        public static string[] strings=new string[1000];

        public static void start_exam(string name, DataGridView dv)
        {

            int z = 0;
            for (int qq = 0; qq <strings.Length; qq++)
            {
                strings[qq] = "";
            }
            try
            {
                guidaoquduan.fengzhuang_duizhao();
            }
            catch
            {
                MessageBox.Show("请导入站内轨道区段信息表");
                return;
            }           
            try
            {
                guidaoquduan.fengzhuang_zaipinbiao();
            }
            catch
            {
                MessageBox.Show("请导入怀衡线怀化南至衡阳东站线路数据表");
                return;
            }
            guidaoquduan.fengzhuang_xinhaoji(name);
            guidaoquduan.fengzhuang_jinluxinxibiao(name);
            guidaoquduan.fengzhuang_xinhaoji(name);
            for (int M = 0; M < guidaoquduan.xinxi.Length; M++)
            {
                for (int x = 0; x < guidaoquduan.duizhao.Length; x++)
                {
                    if (guidaoquduan.duizhao[x].chezhanming == guidaoquduan.xinxi[M].chezhanming )
                    {
                        if (guidaoquduan.duizhao[x].mingcheng_quduan == guidaoquduan.xinxi[M].mingcheng_quduan)
                        {
                            if (guidaoquduan.duizhao[x].daocha_weizhi == guidaoquduan.xinxi[M].daochaweizhi)
                            {
                                int o = x;//dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("Green");
                                if (guidaoquduan.xinxi[M].changdu > guidaoquduan.duizhao[o].changdu + 3 || guidaoquduan.xinxi[M].changdu < guidaoquduan.duizhao[o].changdu - 3)
                                {
                                    int kkkk = guidaoquduan.xinxi[M].hangshu - 2;
                                    dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("Red");
                                    strings[z]= "第"+ kkkk+"行"+ "该进路"+ guidaoquduan.xinxi[M].jinluming+ "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan+"的长度应由"+ guidaoquduan.xinxi[M].changdu+"改为"+guidaoquduan.duizhao[o].changdu;
                                    z++;
                                    break;
                                }
                            }
                        }
                    }
                }
                for (int ii = 0; ii < guidaoquduan.zaipinduizhao.Length; ii++)
                {
                    if (guidaoquduan.xinxi[M].chezhanming == guidaoquduan.zaipinduizhao[ii].chezhanming)
                    {
                        if (guidaoquduan.xinxi[M].mingcheng_quduan == guidaoquduan.zaipinduizhao[ii].quduan)
                        {
                            if (guidaoquduan.xinxi[M].zaipin != guidaoquduan.zaipinduizhao[ii].zaipin)
                            {
                                int kkkk = guidaoquduan.xinxi[M].hangshu - 2;
                                dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("Red");
                                strings[z] = "第" + kkkk + "行" + "该进路" + guidaoquduan.xinxi[M].jinluming + "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan + "的载频应由" + guidaoquduan.xinxi[M].zaipin + "改为" + guidaoquduan.zaipinduizhao[ii].zaipin;
                                z++;
                                break;
                            }
                        }
                    }
                }
                if (guidaoquduan.xinxi[M].mingcheng_quduan == guidaoquduan.xinhaoji[M].gudaoming)
                {
                    if (guidaoquduan.xinxi[M].xinhaoji != guidaoquduan.xinhaoji[M].leixing)
                    {
                        int kkkk = guidaoquduan.xinxi[M].hangshu - 2;
                        dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("Red");
                        strings[z] = "第" + kkkk + "行" + "该进路" + guidaoquduan.xinxi[M].jinluming + "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan + "的信号机类型应由" + guidaoquduan.xinxi[M].xinhaoji + "改为" + guidaoquduan.xinhaoji[M].leixing;
                        z++;
                    }
                }

            }
        }      
    }
}
 