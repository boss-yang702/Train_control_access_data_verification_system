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
        public static DataSet Ds1;
        public static void Func(DataGridView dv)
        {
            Ds1 = DataSets[DsNames["线路速度表"]];
        }       
    }
}
