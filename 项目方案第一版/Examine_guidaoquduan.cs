using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 项目方案第一版
{
    internal class Examine_guidaoquduan:Manager
    {
        //主窗体中的进路数据表
        private static DataTable Jinlu_table;
        public static void start_exam(string name)
        {
            Jinlu_table = DataSets[name].Tables[0];
            MessageBox.Show(Jinlu_table.TableName, Jinlu_table.Rows[0][0].ToString());
            
        }
    }
}
