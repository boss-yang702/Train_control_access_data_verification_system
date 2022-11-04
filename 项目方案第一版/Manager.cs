using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace 项目方案第一版
{
    internal  class Manager
    {
        //构建起Excel文件的名字信息与DataSet的字典对应关系，将文件导入字典，通过表名便可访问Dataset eg：DataSets[string name]
        public static Dictionary<string, DataSet> DataSets =new Dictionary<string, DataSet>();


        /// <summary>
        /// 输入文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>返回一个包含该文件的DateSet 里面有所有Sheet对应的Datatable</returns>
        private static DataSet ImportExcel(string filePath)
        {
            DataSet ds = null;
            OleDbConnection OleConn;

            string strConn = string.Empty;
            string sheetName = string.Empty;

            try
            {
                //string strConn;
                ////strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel8.0;HDR=False;IMEX=1'";
                //strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + filePath + ";Extended Properties='Excel 12.0; HDR=NO;IMEX=1'";//此连接可以操作.xls与.xlsx文件
                //OleDbConnection OleConn = new OleDbConnection(strConn);
                //OleConn.Open();
                

                // Excel 2003 版本连接字符串
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel8.0;HDR=False;IMEX=1'";
                 OleConn = new OleDbConnection(strConn);
                OleConn.Open();
            }
            catch
            {
                // Excel 2007 以上版本连接字符串
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data source=" + filePath + ";Extended Properties='Excel 12.0; HDR=NO;IMEX=1'";
                 OleConn = new OleDbConnection(strConn);
                OleConn.Open();
            }

            //获取所有的 sheet 表
            DataTable dtSheetName = OleConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
            List<string> sheetnames = new List<string>();
            foreach (DataRow row in dtSheetName.Rows)
            {
                string name = (string)row["TABLE_NAME"];
                string tempName = name;
                if (tempName.IndexOf(" ") > -1 || tempName.IndexOf("　") > -1)
                {
                    tempName = tempName.Replace(" ", "_");
                    tempName = tempName.Replace("　", "_");
                    tempName = tempName.Replace("'", "");
                }
                if (!tempName.EndsWith("$"))
                {
                    continue;
                }
                sheetnames.Add(tempName);

            }
            ds = new DataSet();

            //导入所有Sheet
            foreach (string it in sheetnames)
            {
                DataTable dt = new DataTable(it.Substring(0, it.Length - 1));

                OleDbDataAdapter oleda = new OleDbDataAdapter("select * from [" + it + "]", OleConn);

                oleda.Fill(dt);

                ds.Tables.Add(dt);
               
            }

            //关闭连接，释放资源
            OleConn.Close();
            OleConn.Dispose();

            return ds;
        }


        /// <summary>
        /// 传入一个datagridview,通过这个datagridview导出一个Excel表格文件，可选择文件保存地址
        /// </summary>
        /// <param name="dataGridView1"></param>
        public static void Export(DataGridView dataGridView1)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "excel文件|*.XLS";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                if (app == null)
                {
                    MessageBox.Show("没有excel应用程序");
                    return;
                }
                Microsoft.Office.Interop.Excel.Workbooks workbooks = app.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1];

                //写入标题
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                //写入数据
                for (int r = 0; r < dataGridView1.RowCount; r++)
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
                    }
                }
                worksheet.Columns.AutoFit();
                MessageBox.Show("保存成功");
                workbook.Saved = true;
                workbook.SaveCopyAs(saveFileDialog.FileName);
                app.Quit();
            }
        }


        /// <summary>
        /// 打开文件对话框，导入进路信息表并显示在加载进来的DataGridview中,并将文件名加入字典中与DataSets下标对应 eg:"进路信息表":0
        /// </summary>
        /// <param name="dav"></param>
        public static void Load_file(DataGridView dav,TextBox tb)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格|*.xls|所有文件|*.*";
            //文件绝对路径
            string strPath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPath = ofd.FileName;
                string filename = System.IO.Path.GetFileName(strPath);//文件名  “Default.aspx”
                int index2 = filename.LastIndexOf('表');
                string DsName = filename.Substring(0, index2+1);
                DataSet ds = ImportExcel(strPath);
                ds.DataSetName = DsName;
                if (DataSets.ContainsKey(DsName))
                {
                    DialogResult MsgBoxResult;//设置对话框的返回值
                    MsgBoxResult = System.Windows.Forms.MessageBox.Show("该文件已导入，是否覆盖？", "这会替换系统中原本的数据集，不覆盖则会显示原本数据", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
                    if (MsgBoxResult.ToString() == "Yes")//如果对话框的返回值是YES（按"Y"按钮）
                    {
                        DataSets.Remove(DsName);
                        DataSets.Add(DsName, ds);
                        dav.DataSource = DataSets[DsName].Tables[0];
                        tb.Text = DsName;
                        //main_dv.Dispose();
                        //main_dv.DataSource = ds.Copy();
                        //选择了Yes，继续
                    }
                    if ( MsgBoxResult.ToString() == "No")//如果对话框的返回值是NO（按"N"按钮）
                    {
                        dav.DataSource = DataSets[DsName].Tables[0];
                        //main_dv.Dispose();
                        //main_dv.DataSource = ds.Copy();
                        tb.Text = DsName;
                        //选择了No，继续
                    }
                    
                    return;
                }
                DataSets.Add(DsName, ds);
                dav.DataSource = DataSets[DsName].Tables[0];
                //main_dv.Dispose();
                //main_dv.DataSource = ds.Copy();
                tb.Text = DsName;
            }

        }

        /// <summary>
        /// 导入线路数据表加载到静态存储区中,建立字典
        /// </summary>

        public static void Load_file(TextBox tb1,TextBox tb2,ref DataSet _ds)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格|*.xls|所有文件|*.*";
            //文件绝对路径
            string strPath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPath = ofd.FileName;
                string filename = System.IO.Path.GetFileName(strPath);//文件名  “Default.aspx” 
                int index1  = filename.LastIndexOf("东站");
                int index2 = filename.LastIndexOf('表');
                string DsName=filename.Substring(index1+2, index2-index1-1);
                tb1.Text = filename.Substring(0, index1 + 2);
                tb2.Text = DsName;
                DataSet ds = ImportExcel(strPath);
                //ds.DataSetName = DsName;
                _ds = ds;
            } 
        }


        /// <summary>
        /// 搜索进路栏，将搜索的ROWS标注颜色
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="tb"></param>
        /// <param name="e"></param>
        public static void Search(DataGridView dv ,TextBox tb, KeyEventArgs e)
        {
            string a;
            if (e.KeyValue == 13)
            {
                for (int i = 3; i < dv.RowCount; i++)
                {
                    if (i > 0)
                    {
                        //a = this.dataGridView2.Rows[i].Cells[3].Value.ToString().Trim();
                        a = Convert.ToString(dv[3, i].Value);
                        if (tb.Text.ToString().Trim() == a)
                        {
                            dv.Rows[i].DefaultCellStyle.BackColor = Color.FromName("Skyblue");
                        }
                        if (tb.Text == "")
                        {
                            dv.Rows[i].DefaultCellStyle.BackColor = Color.FromName("White");
                        }
                    }
                }
                //dataGridView2.Rows[3].DefaultCellStyle.BackColor = Color.FromName("Skyblue");
            }
        }

        /// <summary>
        /// 找到colname对应的列，返回其列下标index
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colname"></param>
        /// <returns></returns>

        //在传入的DataGridview中找到【应答器编号，经过应答器】的数据列，并返回；
        protected static DataGridViewColumn dv_find_colunm(DataGridView dv, string colname)
        {
            for (int i = 0; i < dv.ColumnCount; i++)
            {
                string temp = Regex.Replace(dv[i, 1].Value.ToString().Trim(), "[\n ]", "", RegexOptions.IgnoreCase);
                if (temp == colname)
                {
                    return dv.Columns[i];
                }
            }
            MessageBox.Show("该进路信息表格缺少" + colname + "信息列");
            return null;
        }

        protected static void indicate_correct(DataGridViewColumn col, int a)
        {
            if (col.DataGridView[col.Index, a].Style.BackColor == Color.Yellow ||
                col.DataGridView[col.Index, a].Style.BackColor == Color.Red) return;//绿色优先级最低
            col.DataGridView[col.Index, a].Style.BackColor = Color.Green;
        }
        protected static void indicate_error(DataGridViewColumn col, int a)
        {
            col.DataGridView[col.Index, a].Style.BackColor = Color.Red;
        }
        
        protected static void indicate_warning(DataGridViewColumn col)
        {
            if (col.DefaultCellStyle.BackColor == Color.Red) return;
            col.DefaultCellStyle.BackColor = Color.Yellow;
        }
        protected static void indicate_warning(DataGridViewColumn col, int a)
        {
            if (col.DefaultCellStyle.BackColor == Color.Red) return;//已经是红色则不改，黄色优先级低于红色
            col.DataGridView[col.Index, a].Style.BackColor = Color.Yellow;
        }

        public static string mytrim(string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
    }

}
