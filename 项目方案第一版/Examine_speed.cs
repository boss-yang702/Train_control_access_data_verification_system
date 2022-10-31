using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace 项目方案第一版
{
    internal class Examine_speed : Manager
    {
        private static DataTable Jinlu_table;
        public static void start_exam(string name)
        {
            Jinlu_table = DataSets[name].Tables[0];
        }
    }
}
