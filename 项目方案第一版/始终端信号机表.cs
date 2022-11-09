using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace 项目方案第一版
{
    internal class 始终端信号机表
    {
        
        Dictionary<string,Dictionary<string,int>> stationlist=  new Dictionary<string, Dictionary<string, int>>();
        public 始终端信号机表(DataSet ds)
        {
           
            DataTable dt = ds.Tables[0];
            for(int col = 0; col < dt.Columns.Count; col++)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0][col].ToString()))
                {
                    Dictionary<string, int> one_Station_Annunciator = new Dictionary<string, int>();
                   
                    string station_name=dt.Rows[0][col].ToString();
                    for(int row = 1; row < dt.Rows.Count;row++)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[row][col].ToString())) break;
                        string name=dt.Rows[row][col].ToString();
                        string lc = dt.Rows[row][col + 1].ToString();
                        int pos = Manager.Get_mile(lc);
                        one_Station_Annunciator.Add(name, pos);
                    }
                    stationlist.Add(station_name, one_Station_Annunciator);
                }
            }
        }

        public int this[string station_name, string name]
        {
            get
            {
                foreach(string na in stationlist.Keys)
                {
                    if (na.Contains(station_name) || station_name.Contains(na))
                    {
                        try
                        {
                            int pos = stationlist[na][name];
                            return pos;
                        }
                        catch
                        {
                            return 0;
                        }
                    }
                }
                return 0;
            }

            
           
        }

        private int Get_mile(string s)
        {
            if (s.Equals("-")) return -1;
            string ss = Regex.Replace(s, "[a - zA-Z]", "", RegexOptions.IgnoreCase);//去除字母
            string[] SA = ss.Split('+');//分割+
            int number = Convert.ToInt32(SA[0]) * 1000 + Convert.ToInt32(SA[1]);//计算里程
            return number;
        }
    }
}
