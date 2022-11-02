using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 项目方案第一版
{
    internal class Switches
    {
        DataSet ds;
        class Switchs
        {
            string Station_name;
            struct rowdata{
                int number;
                int pos;
                string dir;
            }
        }
        public  Switches(DataSet ds)
        {
            this.ds = ds;
            Create();
        }

        //创建字典列表
        void Create()
        {
            //list.Add(new Dictionary<int, int>());
            //list[0].
        }
    }
}
