using Microsoft.Office.Interop.Excel;
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
        public static string[] strings=new string[2000];
        public static string[] strings3 = new string[2000];


        public static void start_exam(string name, DataGridView dv)
        {

            int z = 0;
            int d = 0;
            string[] guidao3 = new string[2000];
            string[] guidao4 = new string[2000];

            for (int qq = 0; qq <strings.Length; qq++)
            {
                strings[qq] = "";
                strings3[qq] = "";
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
            try
            {

                guidaoquduan.fengzhuang_xinhaoji(name);
                guidaoquduan.fengzhuang_jinluxinxibiao(name);
                guidaoquduan.fengzhuang_xinhaoji(name);
                guidaoquduan.fengzhuang_zaipin();
                guidaoquduan.fengzhuang_zaipin1();
            }
            catch
            {
                MessageBox.Show("进路数据表");
                return;
            }
            int uuu = 0;
            for (int x = 0; x < guidaoquduan.duizhao.Length; x++)
            {
                if (guidaoquduan.duizhao[x].chezhanming == guidaoquduan.xinxi[0].chezhanming)
                {
                    guidao3[uuu] = guidaoquduan.duizhao[x].mingcheng_quduan;
                    uuu++;
                }
            }
            for (int M = 0; M < guidaoquduan.xinxi.Length; M++)
            {
                for (int x = 0; x < guidaoquduan.duizhao.Length; x++)
                {
                    if (guidaoquduan.duizhao[x].chezhanming == guidaoquduan.xinxi[M].chezhanming)
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
                                    strings[z] = "序号为" + kkkk + "，" + "进路为" + guidaoquduan.xinxi[M].jinluming + "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan + "的长度应由" + guidaoquduan.xinxi[M].changdu + "改为" + guidaoquduan.duizhao[o].changdu;
                                    z++;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (MainWindow.att)
                {
                    if (guidaoquduan.zaipin[M].chezhanming != null)
                    {
                        if (guidaoquduan.zaipin[M].zaipin != "缺少信息"&&guidaoquduan.zaipin[M].zaipin != "缺少信息1")
                        {
                            if (guidaoquduan.xinxi[M].zaipin != guidaoquduan.zaipin[M].zaipin)
                            {
                                int kkkk = guidaoquduan.xinxi[M].hangshu - 2;
                                dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("Red");
                                strings[z] = "序号为" + kkkk + "，" + "进路为" + guidaoquduan.xinxi[M].jinluming + "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan + "的载频应由" + guidaoquduan.xinxi[M].zaipin + "改为" + guidaoquduan.zaipin[M].zaipin;
                                z++;

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
                        strings[z] = "序号为" + kkkk + "，" + "进路为" + guidaoquduan.xinxi[M].jinluming + "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan + "的信号机类型应由" + guidaoquduan.xinxi[M].xinhaoji + "改为" + guidaoquduan.xinhaoji[M].leixing;
                        z++;
                    }
                }
                int hhh = guidao3.Count(p => p == guidaoquduan.xinxi[M].mingcheng_quduan);
                if (hhh == 0)
                {
                    try
                    {
                        if (guidaoquduan.xinxi[M].mingcheng_quduan != "")
                        {
                            if (dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor != Color.FromName("Red"))
                            {
                                dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("yellow");
                            }
                            int kkkk = guidaoquduan.xinxi[M].hangshu - 2;
                            strings3[d] = "序号为" + kkkk + "，" + "进路为" + guidaoquduan.xinxi[M].jinluming + "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan + "的长度信息缺失";
                            d++;
                        }
                    }
                    catch
                    { 
                        
                    }
                }
                if (MainWindow.att)
                {
                    //int ccc = guidao4.Count(p => p == guidaoquduan.xinxi[M].mingcheng_quduan);
                    //if (ccc == 0)

                    try
                    {
                        if (guidaoquduan.zaipin[M].zaipin != "缺少信息1"&&guidaoquduan.zaipin[M].zaipin == "缺少信息"&& guidaoquduan.zaipin[M].zaipin!="正确")
                        {
                            if (dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor != Color.FromName("Red"))
                            {
                                dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("yellow");
                            }
                            int kkkk = guidaoquduan.xinxi[M].hangshu - 2;
                            strings3[d] = "序号为" + kkkk + "，" + "进路为" + guidaoquduan.xinxi[M].jinluming + "的轨道区段" + guidaoquduan.xinxi[M].mingcheng_quduan + "的载频信息缺失";
                            d++;
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }
                try
                {
                    if (dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor != Color.FromName("Red") && dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor != Color.FromName("Yellow") && guidaoquduan.xinxi[M].mingcheng_quduan != "")
                    {
                        dv.Rows[guidaoquduan.xinxi[M].hangshu].Cells[11].Style.BackColor = Color.FromName("Green");
                    }
                }
                catch
                {
                    continue;
                }
            }
        }      
    }
}
 