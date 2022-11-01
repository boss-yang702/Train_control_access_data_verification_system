using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.ComponentModel;

namespace 项目方案第一版
{
    internal abstract class Examine_yingdaqi : Manager
    {

        //进路数据表table
        protected static DataTable Jinlu_table;
        public static string result;
       
        private static DataSet yingdaqi_ds;
        static  Responder_pos re;
        //开始检验，获得结果,传入的DataGridView中标注错误，警告信息，如果错误则标红，信息不足则标黄
        public static void start_exam(string name,  DataGridView dv)
        {
            if (name == "")
            {
                result += "没有进路表\r\n";
                return;
            }
            Jinlu_table = DataSets[name].Tables[0];
            main_dv= dv;

            re = new Responder_pos(DataSets["应答器位置表"]);
            //在进路数据表的DataGridView中找到与应答器有关的列,缺少直接返回
            DataGridViewColumn dv_bh = dv_find_colunm(main_dv, "应答器编号");
            DataGridViewColumn dv_jg = dv_find_colunm(main_dv, "经过应答器");
            if (dv_bh == null || dv_jg == null) return;
            Responder_pos.Exam_dataset_info(dv_bh, dv_jg,ref yingdaqi_ds);

            Exam_begin(dv_bh, dv_jg,re.ds);
        }


        /// <summary>
        /// 开始检查每个单元详细数据，并在DataGridView中标注结果
        /// </summary>
        static void Exam_begin(DataGridViewColumn dv_bh, DataGridViewColumn dv_jg,DataSet ds)
        {

            for (int i = 3; i < dv_bh.DataGridView.RowCount; i++)
            {
                string bh = Regex.Replace(main_dv[dv_bh.Name, i].Value.ToString().Trim(), "[ \n\r]", "", RegexOptions.IgnoreCase);
                string[] jl = Regex.Replace(main_dv[dv_jg.Name, i].Value.ToString().ToString().Trim(), "[ \n\r]", "", RegexOptions.IgnoreCase).Split(',');
                if (jl[0] == "-")
                {
                    indicate_correct(dv_jg, i);
                    continue;//经过应答器为-，则不检验，跳过
                }
                int start_pos = Get_yingdaqi_pos(bh,ds);
                if(start_pos== -1)
                {
                    Responder_pos.indicate_warning(dv_jg, i);
                    Examine_yingdaqi.result=Examine_yingdaqi.result+"没有" + bh + "应答器位置";
                }
                string bh_pre = Regex.Match(bh, @"\d+-\d+-\d+-", RegexOptions.IgnoreCase).Value;//105-3-04- 
                foreach (string j in jl)//有多个应答器单元编号/链接距离组合
                {
                    string[] js = j.Split('/');//分别得到后缀前半部分的编号 和 距离
                    string des_pre = bh_pre + js[0];//目标编号前缀 105-3-04-075

                    List<string> dess = re.Get_des_yingdaqibianhaos(des_pre,ds);//得到所有目标编号-1 -2 -3等
                    List<int> des_positions = new List<int>();//每个目标应答器的位置List
                    foreach (string bianhao in dess)//每个目标应答器有-1 -2 -3 等位置
                    {
                        des_positions.Add(Get_yingdaqi_pos(bianhao,ds));
                    }
                    int result = re.Compare(start_pos,Convert.ToInt32(js[1]), des_positions, 2);
                    if (result == 1)
                    {
                        Responder_pos.indicate_correct(dv_jg,i);
                        continue;//标注绿色
                    }
                       
                    else if (result == 0) Responder_pos.indicate_error(dv_jg, i);//错误把这个单元格表红
                    else Responder_pos.indicate_warning(dv_jg, i);//信息不足则标黄
                }
              
            }
        }


















        /// <summary>
        /// 输入源位置 偏移距离 目标位置范围，允许误差范围
        /// </summary>
        /// <param name="source"></param>
        /// <param name="deviation"></param>
        /// <param name="des_positions"></param>
        /// <param name="error_range"></param>
        /// <returns>错误返回0 正确返回1 信息不足返回-1</returns>
        static int Compare(int source, int deviation, List<int> des_positions,int error_range)
        {
            if (des_positions.Count == 0|| des_positions.Count == 1)
                return -1;//只有一个应答器，没有数据
            int right = des_positions.Max()+error_range;
            int left = des_positions.Min()-error_range;
            int obj = source + deviation;
            if(left<=obj && obj<=right)
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
        static List<string> Get_des_yingdaqibianhaos(string bh_pre,DataSet ds)
        {
            List<string> dess = new List<string>();
            int bh_index = dt_find_column_index(ds.Tables[0], "应答器编号") ;
            foreach( DataTable dt in ds.Tables) 
            {
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    string temp1 = Regex.Replace(dt.Rows[i][bh_index].ToString().Trim(), "[\n\r ]", "", RegexOptions.IgnoreCase);
                    if (temp1.Contains(bh_pre))
                    {
                        dess.Add(temp1); 
                    }
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
        static int Get_yingdaqi_pos(string bh, DataSet ds)
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
        static int Get_mile(string s)
        {
            if (s.Equals("")) return 0;
            string ss = Regex.Replace(s, "[a - zA-Z]", "", RegexOptions.IgnoreCase);//去除字母
            string[] SA = ss.Split('+');//分割+
            int number = Convert.ToInt32(SA[0]) * 1000 + Convert.ToInt32(SA[1]);//计算里程
            return number;
        }

        /// <summary>
        /// 找到colname对应的列，返回其列下标index
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colname"></param>
        /// <returns></returns>
        static int dt_find_column_index(DataTable dt,string colname)
        {
            for(int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Rows[1][i].ToString().Trim() == colname)
                {
                    return i;
                }
            }
            return -1;
        }
        //在传入的DataGridview中找到需要的数据的数据列，并返回；
        static DataGridViewColumn dv_find_colunm(DataGridView dv,string colname)
        {
            for(int i = 0; i < dv.ColumnCount; i++)
            {
                string temp = Regex.Replace( dv[i, 1].Value.ToString().Trim(),"[\n ]","",RegexOptions.IgnoreCase);
                if (temp == colname)
                {
                    return dv.Columns[i];
                }
            }
            MessageBox.Show("该进路信息表格缺少" + colname + "信息列");
            return null;
        }
        /// <summary>
        /// 在Manager.DataSets里面检查导入的数据信息是否拥有，如果缺少文件则直接将对应列标黄
        /// </summary>
        public static void Exam_dataset_info(DataGridViewColumn dv_bh, DataGridViewColumn dv_jg,ref DataSet ds)
        {
            //找到则赋值给全局变量yindaqi_ds，没找到就在dv_jg这一列标黄
            if (!find_ds("应答器位置表",ref ds))
            {
                indicate_warning(dv_jg);//直接在datagridview标黄某一列
            }
            
        }
        public static void indicate_correct(DataGridViewColumn col,int a)
        {
            col.DataGridView[col.Index, a].Style.BackColor = Color.Green;
        }
        private static void indicate_error(DataGridViewColumn col, int a)
        {
            col.DataGridView[col.Index, a].Style.BackColor = Color.Red;
        }

        private static void indicate_warning( DataGridViewColumn col)
        {
            col.DefaultCellStyle.BackColor = Color.Yellow;
        }
        private static void indicate_warning( DataGridViewColumn col,int a)
        {
            if (col.DefaultCellStyle.BackColor == Color.Red) return;//已经是红色则不改，黄色优先级低于红色
            col.DataGridView[col.Index, a].Style.BackColor = Color.Yellow;
        }
        /// <summary>
        /// 输入信息类型 (如应答器位置) ,一个Dataset  
        /// </summary>
        /// <param name="type"></param>
        /// <returns>找到到该信息表格数据集 ，赋值给该DataSet,返回ture 没找到则会返回false</returns>
        private static bool find_ds(string type, ref DataSet ds)
        {
            string[] keys=DataSets.Keys.ToArray();
            foreach(string it in keys)
            {
                if (it.Contains(type))
                {
                    ds = DataSets[it];
                    return true;
                }
            }
            result += "缺少" + "type" + "表\n";
            return false;
        }


    }
}
