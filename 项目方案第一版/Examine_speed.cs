﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace 项目方案第一版
{
    internal  class Examine_speed : Manager
    {
        
        public static string result;
        static Switches.Single_Station_Switchs current_station;
         static Switches swis;
        //开始检验，获得结果,传入的DataGridView中标注错误，警告信息，如果错误则标红，信息不足则标黄
        public static void start_exam(string name, DataGridView dv)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                result += "主窗体没有进路表\r\n";
                return;
            }

            //在进路数据表的DataGridView中找到与应答器有关的列,缺少直接返回
            DataGridViewColumn dvc_sd = dv_find_colunm(dv, "始端信号机");
            DataGridViewColumn dvc_zd = dv_find_colunm(dv, "终端信号机名称");
            DataGridViewColumn dvc_dc = dv_find_colunm(dv, "道岔");
            DataGridViewColumn dvc_xlsd = dv_find_colunm(dv, "线路速度");
  
            if (dvc_sd == null || dvc_zd == null||dvc_dc == null || dvc_xlsd == null) return;
            try
            {
                string s=Manager.DataSets["怀衡线怀化南至衡阳东站始终端信号机信息表"].ToString();
                swis = new Switches(DataSets["怀衡线怀化南至衡阳东站道岔信息表"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                indicate_warning(dvc_sd);//直接在datagridview标黄某一列
                indicate_warning(dvc_zd);//直接在datagridview标黄某一列
                indicate_warning(dvc_dc);//直接在datagridview标黄某一列
                indicate_warning(dvc_xlsd);//直接在datagridview标黄某一列
                return;
            }
            current_station= swis.Get_one_station(name);
            Exam_begin(dvc_sd, dvc_zd, dvc_dc, dvc_xlsd);
        }


        static void  Exam_begin(DataGridViewColumn dvc_sd,DataGridViewColumn dvc_zd,
            DataGridViewColumn dvc_dc,DataGridViewColumn dvc_xlsd)
        {
            for(int row = 3; row < dvc_sd.DataGridView.RowCount; row++)
            {
                string sd = mytrim(dvc_dc.DataGridView[dvc_sd.Name, row].Value.ToString());
                string zd = mytrim(dvc_dc.DataGridView[dvc_zd.Name, row].Value.ToString());
                string[] dcs = mytrim(dvc_dc.DataGridView[dvc_dc.Name, row].Value.ToString()).Split(',');
                string[] xlsds = mytrim(dvc_dc.DataGridView[dvc_xlsd.Name, row].Value.ToString()).Split(',');
                List<int> Miles = Get_roads(sd,zd,dcs);
                int[] speeds= Get_Speed1(sd,zd,Miles.Count);

                //生成的数量不一致，直接标红
                if (Miles.Count != xlsds.Count())
                {
                    indicate_error(dvc_xlsd, row);
                    continue;
                }
                
                if (sd == "XF" || sd == "X" || zd == "SF" || zd == "S")
                {
                    for(int i = 0; i < xlsds.Count() ; i++)
                    {
                        string[] sd_cd = xlsds[i].Split('/');//s[0]为速度 s[1]为长度
                        if (sd_cd[0] == speeds[i].ToString() && Math.Abs(Convert.ToInt32(sd_cd[1])- Miles[i])<=1)
                        {
                            continue;
                        }
                        else
                        {
                            indicate_error(dvc_xlsd, row);
                        }
                    }
                    indicate_correct(dvc_xlsd, row);
                }
                else//生成的距离次序与列车运行方向相反
                {
                    for (int i = 0; i < xlsds.Count(); i++)
                    {
                        string[] sd_cd = xlsds[i].Split('/');//s[0]为速度 s[1]为长度
                        if (sd_cd[0] == speeds[i].ToString() && Math.Abs(Convert.ToInt32(sd_cd[1]) - Miles[xlsds.Count()-i-1]) <= 1)
                        {
                            continue;
                        }
                        else
                        {
                            indicate_error(dvc_xlsd, row);
                        }
                    }
                    indicate_correct(dvc_xlsd, row);
                }

            }
        }
        
        private static int Get_mile(string s)
        {
            if (s.Equals("-")) return -1;
            string ss = Regex.Replace(s, "[a - zA-Z]", "", RegexOptions.IgnoreCase);//去除字母
            string[] SA = ss.Split('+');//分割+
            int number = Convert.ToInt32(SA[0]) * 1000 + Convert.ToInt32(SA[1]);//计算里程
            return number;
        }
        static List<int> Get_roads(string sd,string zd,string[] dcs)
        {
            List<int> roads = new List<int>();
            List<int> Pos = new List<int>();
            foreach(string dc in dcs)
            {
                if (Regex.IsMatch(dc, @"\(\d+/\d+\)"))    //（1/3）(3)双反位道岔
                {
                    List<int> temp = current_station.Mymatch(dc);
                    if (temp.Count == 0) continue;
                    foreach (int a in temp)
                    {
                        Pos.Add(a);
                    }
                }
                else if (Regex.IsMatch(dc, @"\(\d+\)"))
                {
                    int s = Convert.ToInt32(Regex.Replace(dc,@"[()]",""));
                    if (current_station.data[s].dir == "正线")
                    {
                        Pos.Add(current_station.data[s].pos);
                    }
                }
                else
                {
                    continue;
                }
            }
            int[] start_end = Get_Beginning2Start(current_station.Station_name, sd, zd);
            
            Pos.Add(start_end[0]);
            Pos.Add(start_end[1]);
            
            //从小到大排序，每相邻两个做差值得到两个点之间的距离，加到Miles中去
            Pos.Sort();
            for(int i = 0; i < Pos.Count-1; i++)
            {
                int a = Pos[i];
                int b = Pos[i + 1];
                int D_value = b-a;
                roads.Add(D_value);
            }
            return roads;
        }
        /// <summary>
        /// 获取始终端信号机位置
        /// </summary>
        /// <param name="station_name"></param>
        /// <param name="sd"></param>
        /// <param name="zd"></param>
        /// <returns></returns>
        static int[] Get_Beginning2Start(string station_name,string sd,string zd)
        {
            int[] ints = new int[2];
            DataTable dt = Manager.DataSets["怀衡线怀化南至衡阳东站始终端信号机信息表"].Tables[0];
             for (int col = 0; col < dt.Columns.Count; col++)
               {
                    if (dt.Rows[0][col].ToString().Contains(station_name))
                    {
                        
                        for (int i = 1; i<dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][col].ToString() == sd)
                            {
                                ints[0] = Get_mile(dt.Rows[i][col + 1].ToString());
                            }
                            if (dt.Rows[i][col].ToString() == zd)
                            {
                                ints[1] = Get_mile(dt.Rows[i][col + 1].ToString());
                            }
                        }
                    }
             }
            
           
            return ints;
        }
        /// <summary>
        /// 获取该条进路每一段速度
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="zd"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        static int[] Get_Speed1(string sd,string zd,int count)
        {
            int[] speeds = new int[count];
            //speed从列车所在地开始生成，由近及远
            if (sd == "X"||sd=="XH"||sd =="XY" || sd == "S" || sd == "SH" || sd == "SY")
            {
                //正向接车
                switch (count)
                { 
                    case 1: 
                        speeds[0] = 200;
                        break;
                    case 2:
                        speeds[0] = 200;
                        speeds[1] = 45;
                        break;
                    case 3:
                        speeds[0] = 200;
                        speeds[1] = 45;
                        speeds[2] = 160;
                        break;
                    case 4:
                        speeds[0] = 200;
                        speeds[1] = 45;
                        speeds[2] = 160;
                        speeds[3] = 45;
                        break;
                    case 5:
                        speeds[0] = 200;
                        speeds[1] = 45;
                        speeds[2] = 160;
                        speeds[3] = 45;
                        speeds[4] = 200;
                        break;
                    case 6:
                        speeds[0] = 200;
                        speeds[1] = 45;
                        speeds[2] = 160;
                        speeds[3] = 45;
                        speeds[4] = 200;
                        speeds[5] = 45;
                        break;
                }
                    
            }
            else if (sd == "XF" || sd == "SF"||sd == "XYF" || sd == "SYF"||sd == "XHF" || sd == "SHF")
            {
                //反向接车
                switch (count)
                { 
                    case 1: 
                        speeds[0] = 160;
                        break;
                    case 2:
                        speeds[0] = 160;
                        speeds[1] = 45;
                        break;
                    case 3:
                        speeds[0] = 160;
                        speeds[1] = 45;
                        speeds[2] = 200;
                        break;
                    case 4:
                        speeds[0] = 160;
                        speeds[1] = 45;
                        speeds[2] = 200;
                        speeds[3] = 45;
                        break;
                    case 5:
                        speeds[0] = 160;
                        speeds[1] = 45;
                        speeds[2] = 200;
                        speeds[3] = 45;
                        speeds[4] = 160;
                        break;
                    case 6:
                        speeds[0] = 160;
                        speeds[1] = 45;
                        speeds[2] = 200;
                        speeds[3] = 45;
                        speeds[4] = 160;
                        speeds[5] = 45;
                        break;
                }
            }
            else if (zd == "XF" || zd == "SF"||zd == "XYF" || zd == "SYF"||zd == "XHF" || zd == "SHF")
            {
                //正向发车
                switch (count)
                {
                    case 1:
                        speeds[0] = 200;
                        break;
                    case 2:
                        speeds[0] = 45;
                        speeds[1] = 200;
                        break;
                    case 3:
                        speeds[0] = 160;
                        speeds[1] = 45;
                        speeds[2] = 200;
                        break;
                    case 4:
                        speeds[0] = 45;
                        speeds[1] = 160;
                        speeds[2] = 45;
                        speeds[3] = 200;
                        break;
                    case 5:
                        speeds[0] = 200;
                        speeds[1] = 45;
                        speeds[2] = 160;
                        speeds[3] = 45;
                        speeds[4] = 200;
                        break;
                    case 6:

                        speeds[0] = 45;
                        speeds[1] = 200;
                        speeds[2] = 45;
                        speeds[3] = 160;
                        speeds[4] = 45;
                        speeds[5] = 200;
                        break;
                }

            }
            else if (zd == "X" || zd == "S"||zd == "XY" || zd == "SY"||zd == "XH" || zd == "SH")
            {
                //反向发车
                switch (count)
                {
                    case 1:
                        speeds[0] = 160;
                        break;
                    case 2:
                        speeds[0] = 45;
                        speeds[1] = 160;
                        break;
                    case 3:
                        speeds[0] = 200;
                        speeds[1] = 45;
                        speeds[2] = 160;
                        break;
                    case 4:
                        speeds[0] = 45;
                        speeds[1] = 200;
                        speeds[2] = 45;
                        speeds[3] = 160;
                        break;
                    case 5:
                        speeds[0] = 160;
                        speeds[1] = 45;
                        speeds[2] = 200;
                        speeds[3] = 45;
                        speeds[4] = 160;
                        break;
                    case 6:
                        speeds[0] = 45;
                        speeds[1] = 160;
                        speeds[2] = 45;
                        speeds[3] = 200;
                        speeds[4] = 45;
                        speeds[5] = 160;
                        break;
                }
            }
            else//信号机始端名称错误
            {
                return null;
            }
            return speeds;
            
        }

        
    }
}
