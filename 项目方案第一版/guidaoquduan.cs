using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace 项目方案第一版
{

    internal class guidaoquduan
    {
        private static DataTable db;
        private static DataTable dn;
        private static DataTable dz;
        private static DataTable dr;
        private static DataTable dm;
        private static DataTable dk;
        private static DataTable dt;
        private static DataTable dp;
        public static xinxi_guidao[] xinxi = new xinxi_guidao[1000];
        public static duizhaobiao[] duizhao = new duizhaobiao[677];
        public static zaipinxinxi[] zaipinduizhao = new zaipinxinxi[2500];
        public static xinhaojileixing[] xinhaoji = new xinhaojileixing[1000];
        public static zaipinbiao[] zaipin = new zaipinbiao[1000];
        private static DataTable ds;

        public struct zaipinbiao
        {
            public string chezhanming;
            public string quduan;
            public string zaipin;
            public string jinluleixing;
            public string shiduanxinhaoji;
            public string zhongduanxinhaoji;
            public string state1;
            public string state2;
            public int hangshu;
        }
        public struct zaipinxinxi
        {
            public string zaipin;
            public string quduan;
            public string chezhanming;
        }
        public struct xinhaojileixing
        {
            public string leixing;
            public string gudaoming;
            public int hangshu;
        }
        public struct xinxi_guidao
        {
            public string chezhanming;
            public int changdu;
            public string zaipin;
            public string xinhaoji;
            public string mingcheng_quduan;
            public string daochaweizhi;
            public int hangshu;
            public string jinluming;
        }
        public struct duizhaobiao
        {
            public string chezhanming;
            public string mingcheng_quduan;
            public string daocha_weizhi;
            public double changdu;
        }
        static string mytrim(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
        public static void fengzhuang_duizhao()
        {

                int x = 3;
                ds = Manager.DataSets["站内轨道区段信息表"].Tables[0];
                for (int i = 4, j = 0; i <= ds.Rows.Count; i++, j++)
                {

                    try
                    {
                        int o = Convert.ToString(ds.Rows[i][1]).IndexOf("-");
                        int pp = mytrim(Convert.ToString(ds.Rows[x][1])).IndexOf("邵");
                        if (pp == -1)
                        {
                            duizhao[j].chezhanming = mytrim(Convert.ToString(ds.Rows[x][1])).Substring(0, 2);
                        }
                        if (pp != -1)
                        {
                            duizhao[j].chezhanming = mytrim(Convert.ToString(ds.Rows[x][1])).Substring(0, 3).Replace("站", "");
                        }
                        duizhao[j].changdu = Convert.ToDouble(ds.Rows[i][3]);
                        duizhao[j].mingcheng_quduan = mytrim(Convert.ToString(ds.Rows[i][1]));
                        int weizhi0 = Convert.ToString(ds.Rows[i][2]).IndexOf("定");
                        int weizhi1 = Convert.ToString(ds.Rows[i][2]).IndexOf("反");
                        if (o == -1)
                        {
                            if (weizhi0 == -1 && weizhi1 == -1)
                            {
                                duizhao[j].daocha_weizhi = "无道岔";
                            }
                            if (weizhi0 == -1 && weizhi1 != -1)
                            {
                                duizhao[j].daocha_weizhi = "反位";
                            }
                            if (weizhi0 != -1 && weizhi1 == -1)
                            {
                                duizhao[j].daocha_weizhi = "全定位";
                            }
                        }
                        if (o != -1)
                        {
                            string aa = Convert.ToString(ds.Rows[i][2]);
                            duizhao[j].daocha_weizhi = aa;
                            if (weizhi0 != -1 && weizhi1 != -1)
                            {
                                string kkk = mytrim(Convert.ToString(ds.Rows[i][2]).Replace("#道岔", "").Replace("#", "").Replace(" ", "").Replace(",", "").Replace("、", "").Replace("，", ""));
                                string[] zzz = kkk.Split('位');
                                string str1 = System.Text.RegularExpressions.Regex.Replace(zzz[0], @"[^0-9]+", "");
                                string str2 = System.Text.RegularExpressions.Regex.Replace(zzz[1], @"[^0-9]+", "");
                                string sss;

                                sss = kkk.Replace(str1, "").Replace(str2, "");
                                string[] u = sss.Split('位');
                                if (Convert.ToInt32(str1) > Convert.ToInt32(str2))
                                {
                                    duizhao[j].daocha_weizhi = str2 + u[1] + "位" + str1 + u[0] + "位";
                                }
                                else
                                {
                                    duizhao[j].daocha_weizhi = kkk;
                                }
                            }
                            if (weizhi0 == -1 && weizhi1 != -1)
                            {
                                int xx = Regex.Matches(aa, "反").Count;
                                if (xx == 2)
                                    duizhao[j].daocha_weizhi = "全反位";
                                if (xx == 1)
                                    duizhao[j].daocha_weizhi = mytrim(Convert.ToString(ds.Rows[i][2]).Replace("#道岔", "").Replace("#", "").Replace("、", "").Replace(",", "").Replace(" ", ""));
                            }
                            if (weizhi0 != -1 && weizhi1 == -1)
                            {
                                int xx=Regex.Matches(aa,"定").Count;
                                int cc = Regex.Matches(aa, "#").Count;
                            //int xx = Regex.Matches(aa, "定").Count;
                                if(xx==2)
                                duizhao[j].daocha_weizhi = "全定位";
                                if (xx == 1)
                                { 
                                    if(cc==2)
                                    duizhao[j].daocha_weizhi = "全定位";
                                if (cc == 1)
                                {
                                    duizhao[j].daocha_weizhi = mytrim(Convert.ToString(ds.Rows[i][2]).Replace("#道岔", "").Replace("#", "").Replace("、", "").Replace(",", "").Replace(" ", ""));
                                }
                                }
                                //duizhao[j].daocha_weizhi = mytrim(Convert.ToString(ds.Rows[i][2]).Replace("#道岔", "").Replace("#", "").Replace("、", "").Replace(",", "").Replace(" ", ""));
                            }
                        }
                    }
                    catch
                    {
                        x = i;
                        j = j - 1;
                        continue;
                    }
                }
            
        }
        public static void fengzhuang_jinluxinxibiao(string name)
        {
            int n = 0;
            for (int ppp = 0; ppp < 1000; ppp++)
            {
                xinxi[ppp].mingcheng_quduan="";
            }
            dt = Manager.DataSets[name].Tables[0];
            string[] guidao = new string[dt.Rows.Count];
            string[] panduandingfan = new string[dt.Rows.Count];
            string[] panduanjinlu = new string[dt.Rows.Count];
            for (int i = 3, j = 0; i < dt.Rows.Count; i++, j++)
            {
                panduanjinlu[j] = mytrim(Convert.ToString(dt.Rows[i][3]));
                panduandingfan[j] = mytrim(Convert.ToString(dt.Rows[i][9]));
                string[] daocha = panduandingfan[j].Split(',');
                guidao[j] = mytrim(Convert.ToString(dt.Rows[i][11]));//进路表 
                string[] GD = guidao[j].Split(',');

                for (int c = 0; c < GD.Length; c++)
                {
                    int uu,mm;
                    string[] GN = GD[c].Split('\\');
                    xinxi[n].hangshu = i;
                    zaipin[n].hangshu = i;
                    uu = mytrim(name).IndexOf("邵");
                    mm= mytrim(name).IndexOf("南");
                    if (uu == -1)
                    {
                        xinxi[n].chezhanming = mytrim(name.Substring(0, 2));
                    }
                    if (uu != -1 && mm == -1)
                    {
                        xinxi[n].chezhanming = mytrim(name.Substring(0, 3).Replace("怀", ""));
                        zaipin[n].chezhanming= mytrim(name.Substring(0, 3).Replace("怀", ""));
                    }
                    else 
                    {
                        xinxi[n].chezhanming = mytrim(name.Substring(0, 2));
                        zaipin[n].chezhanming= mytrim(name.Substring(0, 2));
                    }
                    try
                    {
                        xinxi[n].changdu = Convert.ToInt32(GN[0]);
                        xinxi[n].zaipin = GN[1];
                        
                        zaipin[n].quduan = GN[3];//载频
                        if (mytrim(Convert.ToString(dt.Rows[i][4])).IndexOf("发") != -1)
                        {
                            zaipin[n].jinluleixing = "发车";
                        }
                        else
                        {
                            zaipin[n].jinluleixing = "接车";
                        }
                        zaipin[n].shiduanxinhaoji = Convert.ToString(dt.Rows[i][5]);
                        zaipin[n].zhongduanxinhaoji = Convert.ToString(dt.Rows[i][7]);
                        xinxi[n].xinhaoji = GN[2];
                        xinxi[n].mingcheng_quduan = GN[3];
                        xinxi[n].jinluming = panduanjinlu[j];
                    }
                    catch
                    {
                        continue;
                    }

                    int k = Convert.ToString(panduandingfan[j]).IndexOf("(");
                    int m = Convert.ToString(xinxi[n].mingcheng_quduan).IndexOf("D");
                    

                    if (k == -1 && m != -1)
                    {
                        xinxi[n].daochaweizhi = "全定位";
                    }
                    if (m == -1)
                    {
                        xinxi[n].daochaweizhi = "无道岔";
                    }
                    if (k != -1 && m != -1)
                    {
                        string[] aaa = new string[30];
                        int jj = 0;
                        string[] bbb = new string[30];
                        int vv=0;
                        string[] zzz = new string[30];
                        int zz = 0;
                        string Str = mytrim(xinxi[n].mingcheng_quduan.Replace("DG", ""));
                        int q = Convert.ToString(Str).IndexOf("-");
                        if (q == -1)
                        {
                            for (int l = 0; l < daocha.Length; l++)
                            {
                                string yyy = daocha[l];
                                int h = yyy.IndexOf("(");
                                if (h != -1)
                                {
                                    string oo = yyy.Replace("(", "").Replace(")", "");
                                    string[] shuzi = oo.Split('/');
                                        for (int r = 0; r < shuzi.Length; r++)
                                        {
                                            aaa[jj] = shuzi[r];
                                            jj++;
                                        }                                  
                                }
                            }
                            for (int tt = 0; tt < aaa.Length; tt++)
                            {
                                if (Str == aaa[tt])
                                {
                                    xinxi[n].daochaweizhi = "反位";
                                    break;
                                }
                                else
                                {
                                    xinxi[n].daochaweizhi = "全定位";
                                }
                            }
                        }
                        if (q != -1)
                        {
                            for (int l = 0; l < daocha.Length; l++)
                            {
                                string yyy = daocha[l];
                                int h = yyy.IndexOf("(");
                                if (h != -1)
                                {
                                    string oo = yyy.Replace("(", "").Replace(")", "");
                                    string[] shuzi = oo.Split('/');
                                    for (int r = 0; r < shuzi.Length; r++)
                                    {
                                        bbb[vv] = shuzi[r];
                                        vv++;
                                    }
                                }
                                if (h == -1)
                                {
                                    string[] shuzi = yyy.Split('/');
                                    for (int r = 0; r < shuzi.Length; r++)
                                    {
                                        zzz[zz] = shuzi[r];
                                        zz++;
                                    }
                                }
                            }                           
                            string[] daocha2 = Convert.ToString(Str).Split('-');
                            string str1 = null;
                            string str2 = null;
                            for (int l = 0; l < bbb.Length; l++)
                            {
                                if (daocha2[0] == bbb[l])
                                {
                                    str1 = "反位";
                                    break;
                                }
                            }
                            for (int l = 0; l < bbb.Length; l++)
                            {
                                if (daocha2[1] == bbb[l])
                                {
                                    str2 = "反位";
                                    break;
                                }
                            }
                            for (int l = 0; l < zzz.Length; l++)
                            {
                                if (daocha2[0] == zzz[l])
                                {
                                    str1 = "定位";
                                    break;
                                }
                            }
                            for (int l = 0; l < zzz.Length; l++)
                            {
                                if (daocha2[1] == zzz[l])
                                {
                                    str2 = "定位";
                                    break;
                                }
                            }
                            if (str1 == "反位" && str2 == "反位")
                                {
                                    //xinxi[n].daochaweizhi = daocha2[0]+ "反位，" + daocha2[1]+"反位";
                                    xinxi[n].daochaweizhi = ("全反位");
                                }
                                if (str1 == "反位" && str2 == null)
                                {
                                    xinxi[n].daochaweizhi = daocha2[0] + "反位";
                                }
                                if (str1 == null && str2 == "反位")
                                {
                                    xinxi[n].daochaweizhi = daocha2[1] + "反位";
                                }
                                if (str1 == "定位" && str2 == "反位")
                                {
                                    xinxi[n].daochaweizhi = daocha2[0] + "定位" + daocha2[1] + "反位";
                                }
                                if (str1 == "反位" && str2 == "定位")
                                {
                                    xinxi[n].daochaweizhi = daocha2[0] + "反位" + daocha2[1] + "定位";
                                }
                                if (str1 == "定位" && str2 == "定位")
                                {
                                    //xinxi[n].daochaweizhi = daocha2[0]+ "反位，" + daocha2[1]+"反位";
                                    xinxi[n].daochaweizhi = ("全定位");
                                }
                            
                        }
                    }
                    n = n + 1;
                }
            }
        }

        public static void fengzhuang_zaipinbiao()
        {
            int j = 0;
            int k, m, s;
            db = Manager.DataSets["怀衡线怀化南至衡阳东站线路数据表"].Tables[0];
            dn = Manager.DataSets["怀衡线怀化南至衡阳东站线路数据表"].Tables[1];
            dz = Manager.DataSets["怀衡线怀化南至衡阳东站线路数据表"].Tables[2];
            dr = Manager.DataSets["怀衡线怀化南至衡阳东站线路数据表"].Tables[3];

            for (int i = 3; i < db.Rows.Count - 3; i++)
            {
                int mm, uu;
                string name ;
                zaipinduizhao[j].zaipin = mytrim(Convert.ToString(db.Rows[i][7]));
                zaipinduizhao[j].quduan = mytrim(Convert.ToString(db.Rows[i][6]));
                name= mytrim(Convert.ToString(db.Rows[i][1]));
                uu = mytrim(name).IndexOf("邵");
                mm = mytrim(name).IndexOf("南");
                if (uu == -1)
                {
                    zaipinduizhao[j].chezhanming = mytrim(name.Substring(0, 2));
                }
                if (uu != -1 && mm == -1)
                {
                    zaipinduizhao[j].chezhanming = mytrim(name.Substring(0, 3).Replace("怀", ""));
                }
                else
                {
                    zaipinduizhao[j].chezhanming = mytrim(name.Substring(0, 2));
                }
                j++;

            }
            k = j;
            for (int b = 3; b < dn.Rows.Count - 3; b++)
            {
                int mm, uu;
                string name ;
                zaipinduizhao[k].zaipin = mytrim(Convert.ToString(dn.Rows[b][7]));
                zaipinduizhao[k].quduan = mytrim(Convert.ToString(dn.Rows[b][6]));
                name = mytrim(Convert.ToString(db.Rows[b][1]));
                uu = mytrim(name).IndexOf("邵");
                mm = mytrim(name).IndexOf("南");

                //zaipinduizhao[k].chezhanming = mytrim(Convert.ToString(dn.Rows[b][1]));
                if (uu == -1)
                {
                    zaipinduizhao[k].chezhanming = mytrim(name.Substring(0, 2));
                }
                if (uu != -1 && mm == -1)
                {
                    zaipinduizhao[k].chezhanming = mytrim(name.Substring(0, 3).Replace("怀", ""));
                }
                else
                {
                    zaipinduizhao[k].chezhanming = mytrim(name.Substring(0, 2));
                }
                k++;
            }
            m = k;
            for (int z = 3; z < dz.Rows.Count - 3; z++)
            {
                int mm, uu;
                string name;
                zaipinduizhao[m].zaipin = mytrim(Convert.ToString(dz.Rows[z][7]));
                zaipinduizhao[m].quduan = mytrim(Convert.ToString(dz.Rows[z][6]));
                name = mytrim(Convert.ToString(dz.Rows[z][1]));
                uu = mytrim(name).IndexOf("邵");
                mm = mytrim(name).IndexOf("南");
                if (uu == -1)
                {
                    zaipinduizhao[m].chezhanming = mytrim(name.Substring(0, 2));
                }
                if (uu != -1 && mm == -1)
                {
                    zaipinduizhao[m].chezhanming = mytrim(name.Substring(0, 3).Replace("怀", ""));
                }
                else
                {
                    zaipinduizhao[m].chezhanming = mytrim(name.Substring(0, 2));
                }
                m++;
            }

            for (int w = 3, v = 1648; w < dr.Rows.Count - 3; w++, v++)
            {
                int mm, uu;
                string name;

                zaipinduizhao[v].zaipin = mytrim(Convert.ToString(dr.Rows[w][7]));
                zaipinduizhao[v].quduan = mytrim(Convert.ToString(dr.Rows[w][6]));
                //zaipinduizhao[v].chezhanming = mytrim(Convert.ToString(db.Rows[w][1]));
                name = mytrim(Convert.ToString(dr.Rows[w][1]));
                uu = mytrim(name).IndexOf("邵");
                mm = mytrim(name).IndexOf("南");
                if (uu == -1)
                {
                    zaipinduizhao[v].chezhanming = mytrim(name.Substring(0, 2));
                }
                if (uu != -1 && mm == -1)
                {
                    zaipinduizhao[v].chezhanming = mytrim(name.Substring(0, 3).Replace("怀", ""));
                }
                else
                {
                    zaipinduizhao[v].chezhanming = mytrim(name.Substring(0, 2));
                }

            }

        }
        public static void fengzhuang_xinhaoji(string name)
        {
            int n = 0;
            dm = Manager.DataSets[name].Tables[0];
            string[] xinhaoji1 = new string[dm.Rows.Count];
            string[] jiefache = new string[dm.Rows.Count];

            for (int i = 3, j = 0; i < dm.Rows.Count; i++, j++)
            {
                xinhaoji1[j] = mytrim(Convert.ToString(dm.Rows[i][11]));//进路表 
                jiefache[j]= mytrim(Convert.ToString(dm.Rows[i][4]));

                string[] GD = xinhaoji1[j].Split(',');

                for (int c = 0; c < GD.Length; c++)
                {
                    try
                    {
                        int y;
                        string[] GN = GD[c].Split('\\');
                        xinhaoji[n].gudaoming = GN[3];
                        xinhaoji[n].hangshu = i;
                        y = jiefache[j].IndexOf("接");
                        if (c == GD.Length - 1)
                        {
                            if (y == -1)
                            {
                                xinhaoji[n].leixing = "出站口";
                            }
                            else
                            {
                                xinhaoji[n].leixing = "出站信号机";
                            }
                        }
                        else
                        {
                            xinhaoji[n].leixing = "没有信号机";
                        }
                        n++;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
        public static void fengzhuang_zaipin()
        {
            dp = Manager.DataSets["怀衡线怀化南至衡阳东站始终端信号机信息表"].Tables[0];
            string chezhanming=null ;

            for (int j = 0; j < 36; j=j+3)
            {
                int uu;
                uu = Convert.ToString(dp.Rows[0][j]).Substring(0, 3).IndexOf("邵");
                if (uu == -1)
                {
                    chezhanming = Convert.ToString(dp.Rows[0][j]).Substring(0, 2);
                }
                if (uu != -1)
                {
                    chezhanming = Convert.ToString(dp.Rows[0][j]).Substring(0, 3).Replace("怀","");

                }
                if (zaipin[0].chezhanming== chezhanming)
                  {
                      for (int k = 0; k < zaipin.Length; k++)
                      {
                        for (int i = 0; i < 30; i++)
                        {
                            if (Convert.ToString(dp.Rows[i][j]) == zaipin[k].shiduanxinhaoji)
                            {
                                zaipin[k].state1 = (Convert.ToString(dp.Rows[i][j + 1]));
                                break;
                            }
                            else
                            {
                                zaipin[k].state1 = null;
                                continue;
                            }
                        }
                        for (int i = 0; i < 30; i++)
                        {
                            if (Convert.ToString(dp.Rows[i][j]) == zaipin[k].zhongduanxinhaoji)
                            {
                                zaipin[k].state2 = (Convert.ToString(dp.Rows[i][j + 1]));
                                break;
                            }
                            else
                            {
                                zaipin[k].state2 = null;
                                continue;
                            }
                        }

                    }
                }
             }
        }
        public static void fengzhuang_zaipin1()
        {
            for (int i = 0; i < zaipin.Length; i++)
            {
                if (zaipin[i].chezhanming != null)
                {
                    if (zaipin[i].state1 == zaipin[i].state2 && zaipin[i].state1 != null && zaipin[i].state2 != null)
                    {
                        for (int j = 0; j < zaipinduizhao.Length; j++)
                        {
                            if (zaipinduizhao[j].chezhanming == zaipin[i].chezhanming)
                            {
                                if (zaipinduizhao[j].quduan == zaipin[i].quduan)
                                {
                                    zaipin[i].zaipin = zaipinduizhao[j].zaipin;
                                    break;
                                }
                                else
                                {
                                    zaipin[i].zaipin = "缺少信息";
                                    continue;
                                }
                            }
                        }
                    }
                    if (zaipin[i].state1 != zaipin[i].state2&& zaipin[i].state1 !=null&& zaipin[i].state2 !=null)
                    {
                        if (zaipin[i].jinluleixing == "发车")
                        {
                            zaipin[i].zaipin = "0";
                        }
                        if (zaipin[i].jinluleixing == "接车")
                        {
                            if (zaipin[i].hangshu == zaipin[i + 2].hangshu)
                            {
                                zaipin[i].zaipin = "0";
                            }
                            if (zaipin[i].hangshu != zaipin[i + 2].hangshu)
                            {
                                for (int k = 0; k < zaipinduizhao.Length; k++)
                                {
                                    if (zaipinduizhao[k].chezhanming == zaipin[i].chezhanming)
                                    {
                                        if (zaipinduizhao[k].quduan == zaipin[i].quduan)
                                        {
                                            zaipin[i].zaipin = zaipinduizhao[k].zaipin;
                                            break;
                                        }
                                        else
                                        {
                                            zaipin[i].zaipin = "缺少信息";
                                            continue;
                                        }
                                    }
                                }

                            }
                        }
                    }
                    if (zaipin[i].state1 == null || zaipin[i].state2 == null)
                    {
                        zaipin[i].zaipin = "缺少信息";
                    }
                }
            }
        }
    }
}


