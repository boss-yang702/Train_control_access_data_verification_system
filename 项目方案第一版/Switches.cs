using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace 项目方案第一版
{
    internal class Switches
    {
        DataSet ds;
        
         public struct rowdata
        {
            
            public int pos;
            public string dir;

        }
         public class Single_Station_Switchs
        {
            public string Station_name;
            public Dictionary<int, rowdata> data;
            /// <summary>
            /// 找到有效的反位道岔位置
            /// </summary>
            /// <param name="dc"></param>
            /// <returns></returns>
            public List<int> Mymatch(string dc)
            {
                
                string[] ss= Regex.Replace(dc, @"[()]", "").Split('/');
                int[] nums=new int[2];
                nums[0] = Convert.ToInt32(ss[0]);
                nums[1] = Convert.ToInt32(ss[1]);
                List<int> pos = new List<int>();
                try
                {
                    if (data[nums[0]].dir == "正线" && data[nums[1]].dir == "正线")//两端都是干线
                    {

                        pos.Add(data[nums[0]].pos);
                        pos.Add(data[nums[1]].pos);
                        return pos;
                    }
                    else if (data[nums[0]].dir == "侧线" && data[nums[1]].dir == "正线")//一段干线，一段侧线,返回正线上道岔的位置
                    {
                        pos.Add(data[nums[1]].pos);

                    }
                    else if (data[nums[0]].dir == "正线" && data[nums[1]].dir == "侧线")
                    {
                        pos.Add(data[nums[0]].pos);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch { pos.Add(0); }
                return pos;
            }
            
            
        }
        List<Single_Station_Switchs> All;

        //构造函数
        public  Switches(DataSet ds)
        {
            this.ds = ds;
            Create();
            
        }
        public Single_Station_Switchs Get_one_station(string name)
        {
            int index1 = name.IndexOf('站');
            name = name.Substring(0, index1);
            foreach(Single_Station_Switchs t in All)
            {
                if (t.Station_name == name)
                {
                    return t;
                }
            }
            //找不到返回空站
            Single_Station_Switchs tt = new Single_Station_Switchs();
            tt.Station_name = " ";
            return tt;
        }
        //创建字典列表
        void Create()
        {
            string curren_name=string.Empty;
            DataTable dt = ds.Tables[0];
            All = new List<Single_Station_Switchs>();
            Single_Station_Switchs current=null;
            //current.data = null;
            for (int row = 2; row < dt.Rows.Count; row++)
            {
                DataRow dr = dt.Rows[row];
                string name = dr[1].ToString();
                if (curren_name != name)
                {
                    //名字不相等，创建一个新的站的信息加到列表中
                    curren_name = name;
                    current = new Single_Station_Switchs();
                    current.data = new Dictionary<int, rowdata>();
                    current.Station_name = name;
                    All.Add(current);
                }
                rowdata temp = new rowdata();
                int number = Convert.ToInt32(mytrim(dr[2].ToString()));
                temp.pos = Manager.Get_mile(mytrim(dr[3].ToString()));
                temp.dir = mytrim(dr[4].ToString());
                current.data.Add(number,temp);
            }
        }

        /// <summary>
        /// 输入字符串里程数 K106+233;返回106233
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
       
        /// <summary>
        /// 去除不可见字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string mytrim(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
    }
}
