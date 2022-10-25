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

namespace 项目方案第一版
{
    internal abstract class Examine_yingdaqi : Manager
    {

        //进路数据表table
        protected static DataTable Jinlu_table;
        private static DataSet yingdaqi_ds;
        private static string result;
        private static DataGridView main_dv;
        private static DataColumn[] dv_relevent_columns;

        //开始检验，获得结果,传入的DataGridView中标注错误，警告信息，如果错误则标红，信息不足则标黄
        public static void start_exam(string name,DataGridView dv)
        {
            Jinlu_table = DataSets[name].Tables[0];
            main_dv = dv;
            //在DataGridView中找到与应答器有关的列,缺少直接返回
            DataGridViewColumn dv_bh = dv_find_colunm(dv, "应答器编号");
            DataGridViewColumn dv_jg = dv_find_colunm(dv, "经过应答器");
            if (dv_bh == null || dv_jg == null) return;

            Exam_dataset_info(dv_bh,dv_jg);

            DataTable dt1 = yingdaqi_ds.Tables[0];
            DataTable dt2=yingdaqi_ds.Tables[1];
            Exam_begin(dv_bh, dv_jg, dt1, dt2);
        }

        /// <summary>
        /// 开始检查每个单元详细数据，并在DataGridView中标注结果
        /// </summary>
        static void Exam_begin(DataGridViewColumn dv_bh, DataGridViewColumn dv_jg, DataTable dt1,DataTable dt2)
        {
            
            for (int i = 1; i < dv_bh.DataGridView.RowCount; i ++)
            {
                string bh = main_dv[dv_bh.Name, i].ToString().Trim();
                string jl = main_dv[dv_jg.Name, i].ToString().Trim();
                int start_pos = Get_licheng(bh,dt1,dt2);
                Match bh_pre = Regex.Match(bh, "[a - zA-Z]");
            }
        }
        /// <summary>
        /// 输入两张表，一张上行，一张下行，输入一个应答器编号，
        /// </summary>
        /// <param name="bh"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns>返回这个应答器编号对应的里程数，找不到则返回-1</returns>
        static int Get_licheng(string bh, DataTable dt1, DataTable dt2)
        {
            int bh_index = dt_find(dt1, "应答器编号");
            int licheng_index = dt_find(dt1, "历程");
            //第一张表
            for(int i = 0; i < dt1.Rows.Count; i++)
            {
                if(bh == dt1.Rows[i][bh_index].ToString().Trim())
                {
                    string temp = dt1.Rows[i][licheng_index].ToString().Trim();
                    return Get_mile(temp);
                }
                
            }
            //第二张表
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                if (bh == dt2.Rows[i][bh_index].ToString().Trim())
                {
                    string temp = dt2.Rows[i][licheng_index].ToString().Trim();
                    return Get_mile(temp);
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
            string ss = Regex.Replace(s, "[a - zA-Z]", "", RegexOptions.IgnoreCase);
            string[] SA = ss.Split('+');
            int number = Convert.ToInt32(SA[0]) * 1000 + Convert.ToInt32(SA[1]);
            return number;
        }
        static int dt_find(DataTable dt,string colname)
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
        //在传入的DataGridview中找到【应答器编号，经过应答器】的数据列的下标，并返回；
        static DataGridViewColumn dv_find_colunm(DataGridView dv,string colname)
        {
            for(int i = 0; i < dv.ColumnCount; i++)
            {
                string temp = dv[1, i].Value.ToString().Trim();
                if (temp == colname)
                {
                    return dv.Columns[i];
                }
            }
            MessageBox.Show("该进路信息表格没有" + colname + "信息");
            return null;
        }
        /// <summary>
        /// 检查导入的数据信息是否充足，如果缺少文件则直接将对应列标黄
        /// </summary>
        public static void Exam_dataset_info(DataGridViewColumn dv_bh, DataGridViewColumn dv_jg)
        {
            //找到则赋值给全局变量yindaqi_ds，没找到就在dv_jg这一列标黄
            if (!find_ds("应答器位置表",yingdaqi_ds))
            {
                indicate_warning(dv_jg);//直接在datagridview标黄某一列
               
            }
            
        }

        private static void indicate_error()
        {

        }
        public static string GetResult()
        {
            return result;
        }
        private static void indicate_warning(DataGridViewColumn col)
        {
            col.DefaultCellStyle.BackColor = Color.Yellow;
        }
        /// <summary>
        /// 输入信息类型 (如应答器位置) ,一个Dataset  
        /// </summary>
        /// <param name="type"></param>
        /// <returns>找到到该信息表格数据集 ，赋值给该DataSet,返回ture 没找到则会返回false</returns>
        private static bool find_ds(string type,DataSet ds)
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
            result += "缺少" + "type" + "表";
            return false;
        }


    }
}
