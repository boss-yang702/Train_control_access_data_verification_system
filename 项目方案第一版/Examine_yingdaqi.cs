using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Windows.Forms;

namespace 项目方案第一版
{
    internal abstract class Examine_yingdaqi : Manager
    {
        
        //进路数据表table
        protected static DataTable Jinlu_table;
        public static void start_exam(string name)
        {
            Jinlu_table = DataSets[name].Tables[0];
            //DataTable dt = DataSets["进路信息表"]
            
            
        }


    }
}
