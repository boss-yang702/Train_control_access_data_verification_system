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


namespace 项目方案第一版
{
    internal   class Manager
    {
        //一个文件对应一个Dataset 该Dataset有个名字与该下标对应，构建这个字典，便可以通过名字得到下标再寻找Dataset
        public static Dictionary<string, int> DsNames = new Dictionary<string, int>();

        //项目所有数据集,统一方便管理
        public static List<DataSet> DataSets =new List<DataSet>();


        

        /// <summary>
        /// 输入文件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>返回一个包含该文件的DateSet 里面有所有Sheet对应的Datatable</returns>
        public static DataSet ImportExcel(string filePath)
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
        public static void Load_file_jinluinfo(DataGridView dav,TextBox tb)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格|*.xls";
            //文件绝对路径
            string strPath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPath = ofd.FileName;
                string filename = System.IO.Path.GetFileName(strPath);//文件名  “Default.aspx”
                tb.Text = filename;
                int index2 = filename.LastIndexOf('表');
                string DsName = filename.Substring(0, index2+1);
                DataSet ds = ImportExcel(strPath);
                ds.DataSetName = DsName;
                DsNames.Add(DsName, DataSets.Count);
                Manager.DataSets.Add(ds);
                dav.DataSource = Manager.DataSets[DsNames[DsName]].Tables[0];

            }

        }

        /// <summary>
        /// 导入线路数据表加载到内存中,并将文件名加入字典中与DataSets下标对应 eg:"线路数据表":1
        /// </summary>

        public static void Load_file_xianlu_info()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格|*.xls";
            //文件绝对路径
            string strPath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPath = ofd.FileName;
                int index1 = ofd.FileName.LastIndexOf('站');
                int index2 = ofd.FileName.LastIndexOf('表');
                string DsName=ofd.FileName.Substring(index1+1, index2-index1);
                DataSet ds = ImportExcel(strPath);
                ds.DataSetName = DsName;
                DsNames.Add(DsName, DataSets.Count);
                DataSets.Add(ds);
            } 
        }

        /// <summary>
        /// 导入道岔信息表加载到内存中,并将文件名加入字典中与DataSets下标对应 eg:"道岔信息表":2
        /// </summary>
        public static void Load_file_daocha_info()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格|*.xls";
            //文件绝对路径
            string strPath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPath = ofd.FileName;
                int index1 = ofd.FileName.LastIndexOf('站');
                int index2 = ofd.FileName.LastIndexOf('表');
                string DsName = ofd.FileName.Substring(index1 + 1, index2 - index1);
                DataSet ds = ImportExcel(strPath);
                ds.DataSetName = DsName;
                DsNames.Add(DsName, DataSets.Count);
                DataSets.Add(ds);
                

            }

        }

        /// <summary>
        /// 导入应答器位置表加载到内存中,并将文件名加入字典中与DataSets下标对应 eg:"应答器位置表":3
        /// </summary>
        public static void Load_file_yindaqi_info()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格|*.xls";
            //文件绝对路径
            string strPath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPath = ofd.FileName;
                int index1 = ofd.FileName.LastIndexOf('站');
                int index2 = ofd.FileName.LastIndexOf('表');
                string DsName = ofd.FileName.Substring(index1 + 1, index2 - index1);
                DataSet ds = ImportExcel(strPath);
                ds.DataSetName = DsName;
                DsNames.Add(DsName, DataSets.Count);
                DataSets.Add(ds);

            }

        }

        /// <summary>
        /// 导入辅助信息表加载到内存中,并将文件名加入字典中与DataSets下标对应 eg:"辅助信息表":4
        /// </summary>
        public static void Load_file_fuzhu_info()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "表格|*.xls";
            //文件绝对路径
            string strPath = string.Empty;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strPath = ofd.FileName;
                int index1 = ofd.FileName.LastIndexOf('站');
                int index2 = ofd.FileName.LastIndexOf('表');
                string DsName = ofd.FileName.Substring(index1 + 1, index2 - index1);
                DataSet ds = ImportExcel(strPath);
                ds.DataSetName = DsName;
                DsNames.Add(DsName, DataSets.Count);
                DataSets.Add(ds);
            }

        }

    }
}
