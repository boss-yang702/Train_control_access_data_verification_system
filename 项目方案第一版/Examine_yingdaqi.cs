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
using System.Reflection.Emit;

namespace 项目方案第一版
{
    internal abstract class Examine_yingdaqi : Manager
    {

        public static string result; 
        static  Responder_pos re;
        //开始检验，获得结果,传入的DataGridView中标注错误，警告信息，如果错误则标红，信息不足则标黄
        public static void start_exam(string name,   DataGridView dv)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("进路表名为空！");
                return;
            }
            //Jinlu_table = DataSets[name].Tables[0];
            
            //在进路数据表的DataGridView中找到与应答器有关的列,缺少直接返回
            DataGridViewColumn dvc_bh = dv_find_colunm(dv, "应答器编号");
            DataGridViewColumn dvc_jg = dv_find_colunm(dv, "经过应答器");
            if (dvc_bh == null || dvc_jg == null) return;
            try
            {
                re = new Responder_pos(DataSets["怀衡线怀化南至衡阳东站应答器位置表"]);
            }
            catch(Exception ex)
            {
                MessageBox.Show("没有导入应答器位置表！");
                indicate_warning(dvc_jg);//直接在datagridview标黄某一列
                return;
            }
            Exam_begin(dvc_bh, dvc_jg);
          
        }

        /// <summary>
        /// 开始检查每个单元详细数据，并在DataGridView中标注结果
        /// </summary>
        static void Exam_begin(DataGridViewColumn dvc_bh, DataGridViewColumn dvc_jg)
        {

            for (int row = 3; row < dvc_bh.DataGridView.RowCount; row++)
            {
                string bh = Regex.Replace(dvc_bh.DataGridView[dvc_bh.Name, row].Value.ToString(),@"\s+","");
                string[] jl = Regex.Replace(dvc_bh.DataGridView[dvc_jg.Name, row].Value.ToString(),@"\s+","").Split(',');
                if (jl[0] == "-")
                {
                    indicate_correct(dvc_jg, row);
                    continue;//经过应答器为-，则不检验，跳过
                }
                int start_pos = re[bh];
                if(start_pos== -1)
                {
                    
                    //result+="没有" + bh + "应答器位置";
                    //MessageBox.Show("没有" + bh + "应答器位置");
                    indicate_warning(dvc_jg,row);
                    continue;
                }
                string bh_pre = Regex.Match(bh, @"\d+-\d+-\d+-").Value;//105-3-04- 
                
                string sd = dvc_bh.DataGridView[5,row].Value.ToString();
                if (Regex.IsMatch(sd, @"\bX+"))
                {
                    foreach (string combination in jl)//有多个应答器单元编号/链接距离组合
                    {
                        string[] js = combination.Split('/');//分别得到后缀前半部分的编号 和 距离 075/156
                        string des_pre = bh_pre + js[0];//目标编号前缀 105-3-04-075
                                                        //得到所有目标编号-1 -2 -3等 通过编号得到每个目标应答器的位置再加到List中
                        List<int> des_positions = re.Get_des_yingdaqibianhaos(des_pre);//
                                                                                       //将起始位置，偏移距离，目标应答器位置组，误差范围传入比较，返回结果
                        int deviation = Convert.ToInt32(js[1]);
                        
                        int result = re.Compare(start_pos, deviation, des_positions, 2);
                        //根据结果在datagridview中标注颜色
                        if (result == 1)
                        {
                            indicate_correct(dvc_jg, row);
                           
                        }
                        else if (result == 0) indicate_error(dvc_jg, row);//错误把这个单元格表红
                        else indicate_warning(dvc_jg, row);//信息不足则标黄
                        start_pos+=deviation;
                    }
                }
                else
                {
                    foreach (string combination in jl)//有多个应答器单元编号/链接距离组合
                    {
                        string[] js = combination.Split('/');//分别得到后缀前半部分的编号 和 距离 075/156
                        string des_pre = bh_pre + js[0];//目标编号前缀 105-3-04-075
                                                        //得到所有目标编号-1 -2 -3等 通过编号得到每个目标应答器的位置再加到List中
                        List<int> des_positions = re.Get_des_yingdaqibianhaos(des_pre);//
                                                                                       //将起始位置，偏移距离，目标应答器位置组，误差范围传入比较，返回结果
                        int deviation = Convert.ToInt32(js[1]);
                        int result = re.Compare(start_pos, (-deviation), des_positions, 2);
                        //根据结果在datagridview中标注颜色
                        if (result == 1)
                        {
                            indicate_correct(dvc_jg, row);
                            
                        }
                        else if (result == 0) indicate_error(dvc_jg, row);//错误把这个单元格表红
                        else indicate_warning(dvc_jg, row);//信息不足则标黄
                        start_pos-=deviation;
                    }
                }
            }
        }
        public static string GetResult()
        {
            return result;
        }

    }
}
