using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace 项目方案第一版
{
     class Responder_pos
    {
        public DataSet ds;
        public Dictionary<string, int> res_pos =new Dictionary<string, int>();
         public Responder_pos(DataSet ds)
        {
            this.ds = ds;
            //构建索引字典
            Createdic();
            


        }
        /// <summary>
        /// 遍历所有所有datatable，将编号对应的位置转化为里程数，存放到字典中，应答器编号与位置唯一对应
        /// </summary>
        private void Createdic()
        {
            for(int count = 0; count < ds.Tables.Count; count++)
            {
                DataTable dt = ds.Tables[count];
                for(int row=2; row<dt.Rows.Count; row++)
                {
                    string bh = dt.Rows[row][2].ToString();
                    if (string.IsNullOrWhiteSpace(bh)) break;
                    bh = Regex.Replace(bh, @"\s+", "");
                    string lc=dt.Rows[row][3].ToString();
                    lc = Regex.Replace(lc, @"\s+", "");
                    int pos = Get_mile(lc);
                    res_pos.Add(bh, pos);
                }
            }
        }
        //索引器重载通过应答器编号索引得到应答器位置
        public int this[string bh]
        {
            get
            {//字典中没有该值，异常返回-1
                try
                {
                    int temp = res_pos[bh];
                    return res_pos[bh];
                }
                catch(Exception ex)
                {
                    return - 1;
                }
                
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
 
        /// <summary>
        /// 输入源位置 偏移距离 目标位置范围，允许误差范围
        /// </summary>
        /// <param name="source"></param>
        /// <param name="deviation"></param>
        /// <param name="des_positions"></param>
        /// <param name="error_range"></param>
        /// <returns>错误返回0 正确返回1 信息不足返回-1</returns>
        public int Compare(int source, int deviation, List<int> des_positions, int error_range)
        {
            if (des_positions.Count == 0 || des_positions.Count == 1)
                return -1;//只有一个应答器，没有数据
            int right = des_positions.Max() + error_range;
            int left = des_positions.Min() - error_range;
            int obj = source + deviation;
            if (left <= obj && obj <= right)
            {
                return 1;//true
            }
            return 0;//false
        }
        /// <summary>
        /// 输入应答器编号前缀，返回完整的该应答器编号字符串列表
        /// </summary>
        /// <param name="bh_pre"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        public  List<int> Get_des_yingdaqibianhaos(string bh_pre)
        {
            List<int> dess = new List<int>();
            foreach (string item in res_pos.Keys)
            {
                if (item.Contains(bh_pre))
                {
                    dess.Add(res_pos[item]);
                }
            }
            return dess;
        }
        /// <summary>
        /// 输入一个应答器编号，输入一个应答器数据集
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns>返回这个应答器编号对应的里程数，找不到则返回-1</returns>
        public int Get_yingdaqi_pos(string bh, DataSet ds)
        {
            int bh_index = dt_find_column_index(ds.Tables[0], "应答器编号");
            int licheng_index = dt_find_column_index(ds.Tables[0], "里程");
            foreach (DataTable dt in ds.Tables)
            {
                for (int i = 2; i < dt.Rows.Count; i++)
                {
                    string temp1 = Regex.Replace(dt.Rows[i][bh_index].ToString().Trim(), "[\n\r ]", "", RegexOptions.IgnoreCase);
                    if (bh == temp1)
                    {
                        string temp = Regex.Replace(dt.Rows[i][licheng_index].ToString().Trim(), "[\n\r ]", "", RegexOptions.IgnoreCase);
                        return Get_mile(temp);
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// 输入里程数，K15+500
        /// </summary>
        /// <param name="s"></param>
        /// <returns>返回15000+500=15500</returns>
        

        /// <summary>
        /// 找到colname对应的列，返回其列下标index
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colname"></param>
        /// <returns></returns>
        public int dt_find_column_index(DataTable dt, string colname)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Rows[1][i].ToString().Trim() == colname)
                {
                    return i;
                }
            }
            return -1;
        }
        //在传入的DataGridview中找到需要的数据的数据列，并返回；
       public DataGridViewColumn dv_find_colunm(DataGridView dv, string colname)
        {
            for (int i = 0; i < dv.ColumnCount; i++)
            {
                string temp = Regex.Replace(dv[i, 1].Value.ToString().Trim(), "[\n ]", "", RegexOptions.IgnoreCase);
                if (temp == colname)
                {
                    return dv.Columns[i];
                }
            }
            MessageBox.Show("该进路信息表格缺少" + colname + "信息列");
            return null;
        }

 
        /// <summary>
        /// 输入信息类型 (如应答器位置) ,一个Dataset  
        /// </summary>
        /// <param name="type"></param>
        /// <returns>找到到该信息表格数据集 ，赋值给该DataSet,返回ture 没找到则会返回false</returns>
        public static bool find_ds(string type, ref DataSet ds)
        {
            string[] keys = Manager.DataSets.Keys.ToArray();
            foreach (string it in keys)
            {
                if (it.Contains(type))
                {
                    ds = Manager.DataSets[it];
                    return true;
                }
            }
            //result += "缺少" + "type" + "表\n";
            return false;
        }
    }
}
