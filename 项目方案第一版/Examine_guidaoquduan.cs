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
        private static DataTable guidaoquduan_table;
        private static string []guidao;
        private static string a;

        public static DataTable start_exam(string name)
        {
            Jinlu_table = DataSets[name].Tables[0];
            guidaoquduan_table= DataSets["站内轨道区段信息表"].Tables[0];
            string[] guidao = new string[Jinlu_table.Rows.Count];
            string[] bijiao = new string[guidaoquduan_table.Rows.Count];
            string[] changdu = new string[guidaoquduan_table.Rows.Count];
            for (int i= 3,j=0; i < Jinlu_table.Rows.Count; i++,j++)
                guidao[j] = Convert.ToString(Jinlu_table.Rows[i][11]);//进路表
            for (int i=4,j = 0; i < guidaoquduan_table.Rows.Count; i++, j++)
                bijiao[j] = Convert.ToString(guidaoquduan_table.Rows[i][1]);//比较表
            for (int i =4, j = 0; i < guidaoquduan_table.Rows.Count; i++, j++)
                changdu[j] = Convert.ToString(guidaoquduan_table.Rows[i][3]);//比较表
            string[] GD = guidao[0].Split(',');
            string[] GN = GD[0].Split('\\');       
            //guidao = Jinlu_table.Rows[3][11].ToString();
            //string[] GD = guidao.Split(',');
            //string[] GN = GD[0].Split('\\');
            MessageBox.Show(Jinlu_table.TableName, Jinlu_table.Rows[0][0].ToString());
            return guidaoquduan_table;
        }      
    }
}
 