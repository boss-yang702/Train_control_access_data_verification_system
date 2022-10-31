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
    internal class guidaoquduan
    {
        private static DataTable  ds;
        public static void fengzhuang(string[] quduanming, string[] changdu)
        {
            //string[] quduanming = new string[ds.Rows.Count];
            //string[] changdu=new string[ds.Rows.Count];
            ds = Manager.DataSets["站内轨道区段信息表"].Tables[0];
            for (int i = 4; i <= ds.Rows.Count; i++)
            {
                if (quduanming[i] != quduanming[i - 1])
                {
                    quduanming[i] = Convert.ToString(ds.Rows[i][1]);
                    changdu[i] = Convert.ToString(ds.Rows[i][3]);
                }
            }
        }
        public static void fenge(DataTable name)
        { 
           
        }
    }
}
