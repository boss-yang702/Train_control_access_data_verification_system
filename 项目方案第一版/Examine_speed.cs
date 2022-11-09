using System;
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
        static 始终端信号机表 start_end_sheet;
        static DataGridViewColumn dvc_jinlu;
        //开始检验，获得结果,传入的DataGridView中标注错误，警告信息，如果错误则标红，信息不足则标黄
        public static void start_exam(string name, DataGridView dv)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                result += "主窗体没有进路表\r\n";
                return;
            }
            if (name.Contains("所站"))
             {
                v1 = 100;
                v2 = 80;
                v3 = 160;
            }
            //在进路数据表的DataGridView中找到与应答器有关的列,缺少直接返回
            DataGridViewColumn dvc_sd = dv_find_colunm(dv, "始端信号机");
            DataGridViewColumn dvc_zd = dv_find_colunm(dv, "终端信号机名称");
            DataGridViewColumn dvc_dc = dv_find_colunm(dv, "道岔");
            DataGridViewColumn dvc_xlsd = dv_find_colunm(dv, "线路速度");
            dvc_jinlu = dv_find_colunm(dv, "进路");
            if (dvc_sd == null || dvc_zd == null || dvc_dc == null || dvc_xlsd == null) 
            {
                MessageBox.Show("请重新检查进路表信息是否完整");
                return;
            }
            try
            {//检查信息表是否完整
                start_end_sheet=new 始终端信号机表(Manager.DataSets["怀衡线怀化南至衡阳东站始终端信号机信息表"]);
                swis = new Switches(DataSets["怀衡线怀化南至衡阳东站道岔信息表"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("缺少始终端信号机信息表或道岔信息表");
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
                //提取进路信息表信息
                string sd = mytrim(dvc_dc.DataGridView[dvc_sd.Name, row].Value.ToString());
                string zd = mytrim(dvc_dc.DataGridView[dvc_zd.Name, row].Value.ToString());
                string[] dcs = mytrim(dvc_dc.DataGridView[dvc_dc.Name, row].Value.ToString()).Split(',');
                string[] xlsds = mytrim(dvc_dc.DataGridView[dvc_xlsd.Name, row].Value.ToString()).Split(',');

                List<int> Poss = Get_Poss(sd,zd,dcs);
                if (Poss.Contains(0)) //有信号机位置缺失，信息不足
                {
                    indicate_warning(dvc_xlsd, row);
                    continue; 
                };
                List<int> Roads = Get_Roads(Poss);
                int[] speeds= Get_Speed1(sd,zd, Roads.Count);
                if(speeds==null)//没有速度信息
                {
                    indicate_warning(dvc_xlsd,row);
                    continue;
                }
                //生成的数量不一致，直接标红
                if (Roads.Count != xlsds.Count())
                {
                    indicate_error(dvc_xlsd, row);
                    continue;
                }
                
                if (sd == "XF" || sd == "X" || sd == "XH" || sd == "XHF" 
                    || zd == "SF" || zd == "S" ||zd == "SHF" || zd == "SH" || zd == "SY" || zd == "SYF")
                {
                    for(int i = 0; i < xlsds.Count() ; i++)
                    {
                        string[] sd_cd = xlsds[i].Split('/');//s[0]为速度 s[1]为长度
                        if (sd_cd[0] == speeds[i].ToString() && Math.Abs(Convert.ToInt32(sd_cd[1])- Roads[i])<=1)
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
                        if (sd_cd[0] == speeds[i].ToString() && Math.Abs(Convert.ToInt32(sd_cd[1]) - Roads[xlsds.Count()-i-1]) <= 1)
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
        
        
        static List<int> Get_Poss(string sd,string zd,string[] dcs)
        {
            //List<int> roads = new List<int>();
            List<int> Poss = new List<int>();
            foreach(string dc in dcs)
            {
                if (Regex.IsMatch(dc, @"\(\d+/\d+\)"))    //（1/3）双反位道岔
                {
                    List<int> temp = current_station.Mymatch(dc);
                    if (temp==null) continue;
                    foreach (int a in temp)
                    {
                        Poss.Add(a);
                    }
                }
                else if (Regex.IsMatch(dc, @"\(\d+\)"))//(3)
                {
                    int s = Convert.ToInt32(Regex.Replace(dc,@"[()]",""));
                    if (current_station.data[s].dir == "正线")
                    {
                        Poss.Add(current_station.data[s].pos);
                    }
                }
                else
                {
                    continue;
                }
            }
            Poss.Add(start_end_sheet[current_station.Station_name, sd]);
            Poss.Add(start_end_sheet[current_station.Station_name, zd]);

            return Poss;
        }
        static List<int> Get_Roads(List<int> Poss)
        {
            List<int> roads=new List<int>();
            //从小到大排序，每相邻两个做差值得到两个点之间的距离，加到roads中去
            Poss.Sort();
            for (int i = 0; i < Poss.Count - 1; i++)
            {
                int a = Poss[i];
                int b = Poss[i + 1];
                int D_value = b - a;
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
        //static int[] Get_Beginning2Start(string station_name,string sd,string zd)
        //{
        //    int[] ints = new int[2];
        //    DataTable dt = Manager.DataSets["怀衡线怀化南至衡阳东站始终端信号机信息表"].Tables[0];
        //     for (int col = 0; col < dt.Columns.Count; col++)
        //       {
        //            if (dt.Rows[0][col].ToString().Contains(station_name))
        //            {

        //                for (int i = 1; i<dt.Rows.Count; i++)
        //                {
        //                    if (dt.Rows[i][col].ToString() == sd)
        //                    {
        //                        ints[0] = Get_mile(dt.Rows[i][col + 1].ToString());
        //                    }
        //                    if (dt.Rows[i][col].ToString() == zd)
        //                    {
        //                        ints[1] = Get_mile(dt.Rows[i][col + 1].ToString());
        //                    }
        //                }
        //            }
        //     }


        //    return ints;
        //}
        /// <summary>
        /// 获取该条进路每一段速度
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="zd"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        static int v1=200;
        static int v2=45;
        static int v3=160;
        static int[] Get_Speed1(string sd,string zd,int count)
        {
            int[] speeds = new int[count];
            //speed从列车所在地开始生成，由近及远
            if (sd == "X"||sd=="XH"||sd =="XY" || sd == "S" || sd == "SH" || sd == "SY"||Regex.IsMatch(sd,@"\bD+")||Regex.IsMatch(sd, @"\bD+"))
            {
                //正向接车
                switch (count)
                { 
                    case 1: 
                        speeds[0] = v1;
                        break;
                    case 2:
                        speeds[0] = v1;
                        speeds[1] = v2;
                        break;
                    case 3:
                        speeds[0] = v1;
                        speeds[1] = v2;
                        speeds[2] = v3;
                        break;
                    case 4:
                        speeds[0] = v1;
                        speeds[1] = v2;
                        speeds[2] = v3;
                        speeds[3] = v2;
                        break;
                    case 5:
                        speeds[0] = v1;
                        speeds[1] = v2;
                        speeds[2] = v3;
                        speeds[3] = v2;
                        speeds[4] = v1;
                        break;
                    case 6:
                        speeds[0] = v1;
                        speeds[1] = v2;
                        speeds[2] = v3;
                        speeds[3] = v2;
                        speeds[4] = v1;
                        speeds[5] = v2;
                        break;
                }
                    
            }
            else if (sd == "XF" || sd == "SF"||sd == "XYF" || sd == "SYF"||sd == "XHF" || sd == "SHF")
            {
                //反向接车
                switch (count)
                { 
                    case 1: 
                        speeds[0] = v3;
                        break;
                    case 2:
                        speeds[0] = v3;
                        speeds[1] = v2;
                        break;
                    case 3:
                        speeds[0] = v3;
                        speeds[1] = v2;
                        speeds[2] = v1;
                        break;
                    case 4:
                        speeds[0] = v3;
                        speeds[1] = v2;
                        speeds[2] = v1;
                        speeds[3] = v2;
                        break;
                    case 5:
                        speeds[0] = v3;
                        speeds[1] = v2;
                        speeds[2] = v1;
                        speeds[3] = v2;
                        speeds[4] = v3;
                        break;
                    case 6:
                        speeds[0] = v3;
                        speeds[1] = v2;
                        speeds[2] = v1;
                        speeds[3] = v2;
                        speeds[4] = v3;
                        speeds[5] = v2; 
                        break;
                }
            }
            else if (zd == "XF" || zd == "SF"||zd == "XYF" || zd == "SYF"||zd == "XHF" || zd == "SHF")
            {
                //正向发车
                switch (count)
                {
                    case 1:
                        speeds[0] = v1;
                        break;
                    case 2:
                        speeds[0] = v2;
                        speeds[1] = v1;
                        break;
                    case 3:
                        speeds[0] = v3;
                        speeds[1] = v2;
                        speeds[2] = v1;
                        break;
                    case 4:
                        speeds[0] = v2;
                        speeds[1] = v3;
                        speeds[2] = v2;
                        speeds[3] = v1;
                        break;
                    case 5:
                        speeds[0] = v1;
                        speeds[1] = v2;
                        speeds[2] = v3;
                        speeds[3] = v2;
                        speeds[4] = v1;
                        break;
                    case 6:

                        speeds[0] = v2;
                        speeds[1] = v1;
                        speeds[2] = v2;
                        speeds[3] = v3;
                        speeds[4] = v2;
                        speeds[5] = v1;
                        break;
                }

            }
            else if (zd == "X" || zd == "S"||zd == "XY" || zd == "SY"||zd == "XH" || zd == "SH")
            {
                //反向发车
                switch (count)
                {
                    case 1:
                        speeds[0] = v3;
                        break;
                    case 2:
                        speeds[0] = v2;
                        speeds[1] = v3;
                        break;
                    case 3:
                        speeds[0] = v1;
                        speeds[1] = v2;
                        speeds[2] = v3;
                        break;
                    case 4:
                        speeds[0] = v2;
                        speeds[1] = v1;
                        speeds[2] = v2;
                        speeds[3] = v3;
                        break;
                    case 5:
                        speeds[0] = v3;
                        speeds[1] = v2;
                        speeds[2] = v1;
                        speeds[3] = v2;
                        speeds[4] = v3;
                        break;
                    case 6:
                        speeds[0] = v2;
                        speeds[1] = v3;
                        speeds[2] = v2;
                        speeds[3] = v1;
                        speeds[4] = v2;
                        speeds[5] = v3;
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
