

### 数据导入存储模块

**在项目中新建了一个Manager类，里面存放了所有与数据读取存放等的代码。**

#### 数据导入

​		在项目中添加引用Microsoft.Office.Interop.Excel，使用 System.Data.OleDb模块在内存中开辟空间作为数据库，根据给出的文件路径对数据进行读取。**通过使用try catrth语句分别使用不同的oledb连接字符串，支持Excel 2003 和Excel 2007以上版本文件格式的读取。**

​		一个Excel文件对应一个oldeb连接，针对每一个文件实例化一个**DataSet**，对象读取之后获得所有sheet，根据每一张sheet单独实例化一个**DataTable**对象，然后把Excel文件所有sheet一一对应的DataTable添加的之前创建好的**DataSet.Tables**中，**这样一个Excel文件对应一个Dataset，里面存放所有该文件的所有Sheet，放在Tables中。并将这个DataSet返回。**

```C#
public static DataSet ImportExcel(string filePath)
        {
            DataSet ds = null;
            OleDbConnection OleConn;

            string strConn = string.Empty;
            string sheetName = string.Empty;

            try
            {
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
```

#### 数据存放

在Manager类中定一个一个字典构建起Excel文件的名字信息与DataSet的字典对应关系，将文件导入字典，通过表名便可访问Dataset eg：DataSets[string name]。![image-20221130120324113](/Users/challenger/Library/Application Support/typora-user-images/image-20221130120324113.png)

**虽然在代码中可以通过表名索引到该文件的对应的DataSet，但为了防止文件名不规范可能会导致错误，所以在导入时参考数据都是一一对应，应答器位置表，线路数据表等在程序内部都有固定的名字。**

![image-20221130120804683](/Users/challenger/Library/Application Support/typora-user-images/image-20221130120804683.png)

**在程序内部这些表名已经定死，可视作用下标获取DataSet,因此不会导致文件名不规范而产生bug**

```C#
 				private void button15_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox4, ref dss[1]);
            dss[1].DataSetName = "怀衡线怀化南至衡阳东站道岔信息表";
        }
        private void button16_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox3, ref dss[2]);
            dss[2].DataSetName = "怀衡线怀化南至衡阳东站始终端信号机信息表";

        }
        private void button17_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox2, ref dss[3]);
            dss[3].DataSetName = "怀衡线怀化南至衡阳东站线路数据表";
        }
        private void button18_Click(object sender, EventArgs e)
        {
            Manager.Load_file(textBox6, ref dss[4]);
            dss[4].DataSetName = "站内轨道区段信息表";

        }
```

### 应答器检验模块

##### 参考数据与待检验数据完整性检查

```C#
//开始检验，获得结果,传入的DataGridView中标注错误，警告信息，如果错误则标红，信息不足则标黄
        public static void start_exam(string name,   DataGridView dv)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("进路表名为空！");
                return;
            }
          
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
```

​	在该函数中，检验数据完整的情况下会新建一个 **Responder_pos**类的对象，该类为自定的类，里面是应答器相关的数据以及检验过程中的各个函数,实例化 **Responder_pos**类时，将应答器位置表对应的DataSet作为该类的构造函数的实际参数，调用Createdic()，将应答器位置表中的关键信息提取出来，放在该类中的字典中。

​	该字典Key为应答器编号(string),Value是位置里程数(int)。通过索引器重载可以很方便的拿到应答器编号对应的位置。

```C#
 class Responder_pos
    {
        public DataSet ds;
        public Dictionary<string, int> res_pos =new Dictionary<string, int>();
         public Responder_pos(DataSet ds)
        {
            this.ds = ds;
            //构建索引字典
            Createdic();
        }
        /// <summary>
        /// 遍历所有所有datatable，将编号对应的位置转化为里程数，存放到字典中，应答器编号与位置唯一对应
        /// </summary>
        private void Createdic()
        {
            for(int count = 0; count < ds.Tables.Count; count++)
            {
                DataTable dt = ds.Tables[count];
                for(int row=2; row<dt.Rows.Count; row++)
                {
                    string bh = dt.Rows[row][2].ToString();
                    if (string.IsNullOrWhiteSpace(bh)) break;
                    bh = Regex.Replace(bh, @"\s+", "");
                    string lc=dt.Rows[row][3].ToString();
                    lc = Regex.Replace(lc, @"\s+", "");
                    int pos = Get_mile(lc);
                    res_pos.Add(bh, pos);
                }
            }
        }
        //索引器重载通过应答器编号索引得到应答器位置
        public int this[string bh]
        {
            get
            {//字典中没有该值，异常返回-1
                try
                {
                    int temp = res_pos[bh];
                    return res_pos[bh];
                }
                catch(Exception ex)
                {
                    return - 1;
                }
                
            }
        }
```

##### 经过应答器列检验主体主体代码

检验逻辑已在前文有详细介绍，此处不再赘述，根据检验结果对检验结果中的DataGridView改变颜色。

```C#
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
```

### 线路速度检验模块

**线路速度模块代码结构与应答器检验模块类似，在此不再赘述**

```C#
static void  Exam_begin(DataGridViewColumn dvc_sd,DataGridViewColumn dvc_zd,
            DataGridViewColumn dvc_dc,DataGridViewColumn dvc_xlsd)
        {

            for(int row = 3; row < dvc_sd.DataGridView.RowCount; row++)
            {
                //提取进路信息表信息
                string sd = mytrim(dvc_dc.DataGridView[dvc_sd.Name, row].Value.ToString());
                string zd = mytrim(dvc_dc.DataGridView[dvc_zd.Name, row].Value.ToString());
                string[] dcs = mytrim(dvc_dc.DataGridView[dvc_dc.Name, row].Value.ToString()).Split(',');
                string[] xlsds = mytrim(dvc_dc.DataGridView[dvc_xlsd.Name, row].Value.ToString()).Split(',');

                List<int> Poss = Get_Poss(sd,zd,dcs);
                if (Poss.Contains(0)) //有信号机位置缺失，信息不足
                {
                    indicate_warning(dvc_xlsd, row);
                    continue; 
                };
                List<int> Roads = Get_Roads(Poss);
                int[] speeds;
                if (sta_name.Contains("所站"))
                {
                    v1 = 100;
                    v2 = 80;
                    v3 = 160;
                speeds= Get_Speed2(sd,zd, Roads.Count);
                }else
                {
                    speeds = Get_Speed1(sd, zd, Roads.Count);
                }
                if (speeds==null)//没有速度信息
                {
                    indicate_warning(dvc_xlsd,row);
                    continue;
                }
                //生成的数量不一致，直接标红
                if (Roads.Count != xlsds.Count())
                {
                    indicate_error(dvc_xlsd, row);
                    continue;
                }
                
                if (sd == "XF" || sd == "X" || sd == "XH" || sd == "XHF" 
                    || zd == "SF" || zd == "S" ||zd == "SHF" || zd == "SH" || zd == "SY" || zd == "SYF")
                {
                    for(int i = 0; i < xlsds.Count() ; i++)
                    {
                        string[] sd_cd = xlsds[i].Split('/');//s[0]为速度 s[1]为长度
                        if (sd_cd[0] == speeds[i].ToString() && Math.Abs(Convert.ToInt32(sd_cd[1])- Roads[i])<=1)
                        {
                            continue;
                        }
                        else
                        {
                            indicate_error(dvc_xlsd, row);
                        }
                    }
                    indicate_correct(dvc_xlsd, row);
                }
                else//生成的距离次序与列车运行方向相反
                {
                    for (int i = 0; i < xlsds.Count(); i++)
                    {
                        string[] sd_cd = xlsds[i].Split('/');//s[0]为速度 s[1]为长度
                        if (sd_cd[0] == speeds[i].ToString() && Math.Abs(Convert.ToInt32(sd_cd[1]) - Roads[xlsds.Count()-i-1]) <= 1)
                        {
                            continue;
                        }
                        else
                        {
                            indicate_error(dvc_xlsd, row);
                        }
                    }
                    indicate_correct(dvc_xlsd, row);
                }

            }
        }
        
```

