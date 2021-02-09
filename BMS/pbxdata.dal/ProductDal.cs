using Maticsoft.DBUtility;
using pbxdata.idal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public partial class ProductDal : dataoperating
    {

        //pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext("data source=192.168.0.124;initial catalog=pbxDB;user id=sa;password=bms123456");
        public string connctionstring
        {
            get { return PubConstant.ConnectionString; }
        }
        Errorlogdal dal = new Errorlogdal();

        /// <summary>
        /// 取得所有商品信息
        /// </summary>
        /// <param name="ipara">存储过程参数</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public List<model.product> GetProductList(IDataParameter[] ipara, string procName)
        {
            List<model.product> list = new List<model.product>();
            DataTable dt = Select(ipara, procName);
            list = DataTableToList(dt.Rows);
            return list;
        }

        /// <summary>
        /// 搜索条件商品信息--根据货号
        /// </summary>
        /// <returns></returns>
        public DataTable GetSearchList(Dictionary<string, string> Dic, int page, int pages, out string counts, out string Balcounts,int isReset)
        {
            DataTable dt = new DataTable();
            int Minnid = pages * (page - 1);
            int Maxnid = pages * (page);
            //查询数据
            IDataParameter[] Scode = new IDataParameter[]{
               new SqlParameter("style",Dic["Style"]),
               new SqlParameter("Scode",Dic["Scode"]),
               new SqlParameter("Name",Dic["Name"]),
               new SqlParameter("Cat",Dic["Cat"]),
               new SqlParameter("Cat1",Dic["Cat1"]),
               new SqlParameter("Cat2",Dic["Cat2"]),
               new SqlParameter("MinPricea",Dic["MinPricea"]),
               new SqlParameter("MaxPricea",Dic["MaxPricea"]),
               new SqlParameter("MinBalance",Dic["MinBalance"]),
               new SqlParameter("MaxBalance",Dic["MaxBalance"]),
               new SqlParameter("MinLastgrnd",Dic["MinLastgrnd"]),
               new SqlParameter("MaxLastgrnd",Dic["MaxLastgrnd"]),
               new SqlParameter("AttrState",Dic["AttrState"]),
               new SqlParameter("PicState",Dic["PicState"]),
               new SqlParameter("Page",Minnid.ToString()),
               new SqlParameter("PageCount",Maxnid.ToString()),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("MinPictime",Dic["MinPictime"]),
               new SqlParameter("MaxPictime",Dic["MaxPictime"]),
               new SqlParameter("CustomerId",Dic["CustomerId"]),
               new SqlParameter("UserId",Dic["UserId"]),
               new SqlParameter("Def9",Dic["Def9"]),
               new SqlParameter("Color",Dic["Color"]),
               new SqlParameter("Def11",Dic["Def11"]),
               new SqlParameter("ciqProductId",Dic["ciqProductId"]),
               new SqlParameter("countssql",SqlDbType.NVarChar,4000),           //总行数sql
               new SqlParameter("stocksql",SqlDbType.NVarChar,4000),            //总库存sql
               new SqlParameter("isReset",isReset)                              //本次操作是查询还是分页
            };

            //原来的（SelectSumBalanceByScode库存存储过程，SelectbyScode商品存储过程，SelectbyScodeCount总条数存储过程）
            DataSet ds = Select(Scode, "SelectbyScode", "");                    //返回商品数据

            #region 查看本次操作是重新查询还是翻页
            if (isReset == 1)
            {
                dt = ds.Tables[2];                                              //商品列表
                DataTable dt1 = ds.Tables[0];                                   //总行数
                DataTable dt2 = ds.Tables[1];                                   //总库存

                counts = dt1.Rows[0][0].ToString();                             //返回查询的所有数量
                Balcounts = dt2.Rows[0][0].ToString();                          //返回查询的总库存
            }
            else
            {
                dt = ds.Tables[0];                                              //商品列表
                counts = "0";
                Balcounts = "0";
            }
            #endregion

            return dt;
        }
        /// <summary>
        /// 查询product表 根据款号
        /// </summary>
        /// <param name="Dic"></param>
        /// <param name="page"></param>
        /// <param name="pages"></param>
        /// <param name="counts"></param>
        /// <param name="Balcounts"></param>
        /// <returns></returns>
        public DataTable GetSearchList1(Dictionary<string, string> Dic, int page, int pages, out string counts, out string Balcounts, int isReset)
        {
            DataTable dt = new DataTable();
            int Minnid = pages * (page - 1);
            int Maxnid = pages * (page);

            #region 参数
            IDataParameter[] Style = new IDataParameter[]{
               new SqlParameter("style",Dic["Style"]),
               new SqlParameter("Cat",Dic["Cat"]),
               new SqlParameter("Cat2",Dic["Cat2"]),
               new SqlParameter("MinPricea",Dic["MinPricea"]),
               new SqlParameter("MaxPricea",Dic["MaxPricea"]),
               new SqlParameter("MinBalance",Dic["MinBalance"]),
               new SqlParameter("MaxBalance",Dic["MaxBalance"]),
               new SqlParameter("Page",Minnid.ToString()),
               new SqlParameter("PageCount",Maxnid.ToString()),
               new SqlParameter("sqlhead",""),
               new SqlParameter("sqlbody",""),
               new SqlParameter("sqlrump",""),
               new SqlParameter("sql",""),
               new SqlParameter("StyPic",Dic["PicExist"]),
               new SqlParameter("CustomerId",Dic["CustomerId"]),
               new SqlParameter("shopName",Dic["ShopName"]),
               new SqlParameter("countssql",SqlDbType.NVarChar,4000),           //总行数sql
               new SqlParameter("stocksql",SqlDbType.NVarChar,4000),            //总库存sql
               new SqlParameter("isReset",isReset)                              //本次操作是查询还是分页
            };
            #endregion


            //原来的（SelectSumBalanceByStyle库存存储过程，SelectbyStyle商品数据存储过程，SelectbyStyleCount商品总条数存储过程）
            DataSet ds = Select(Style, "SelectbyStyle", "");                    //返回商品数据

            #region 查看本次操作是重新查询还是翻页
            if (isReset == 1)
            {
                dt = ds.Tables[2];                                              //商品列表
                DataTable dt1 = ds.Tables[0];                                   //总行数
                DataTable dt2 = ds.Tables[1];                                   //总库存

                counts = dt1.Rows[0][0].ToString();                             //返回查询的所有数量
                Balcounts = dt2.Rows[0][0].ToString();                          //返回查询的总库存
            }
            else
            {
                dt = ds.Tables[0];                                              //商品列表
                counts = "0";
                Balcounts = "0";
            }
            #endregion

            return dt;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="Id">商品id</param>
        /// <returns></returns>
        public bool UpdateProduct(model.product mm, string AttrId, string ComAttrvalues, string PropertyId, int UserId)
        {
            try
            {
                IDataParameter[] pro = new IDataParameter[]{
                  new SqlParameter("Cdescript",mm.Cdescript),
                  new SqlParameter("Cat1",mm.Cat1),
                  new SqlParameter("Cat2",mm.Cat2),
                  new SqlParameter("Rolevel",mm.Rolevel),
                  new SqlParameter("Scode",mm.Scode),
                  new SqlParameter("ciqProductId",mm.ciqProductId),
                  new SqlParameter("ciqSpec",mm.ciqSpec),
                  new SqlParameter("ciqHSNo",mm.ciqHSNo),
                  new SqlParameter("ciqAssemCountry",mm.ciqAssemCountry),
                  new SqlParameter("Def9",mm.Def9),
                  new SqlParameter("Def10",mm.Def10),
                  new SqlParameter("sql",""),
                  new SqlParameter("Imagefile",mm.Imagefile),
                  new SqlParameter("QtyUnit",mm.QtyUnit),
                  new SqlParameter("Def15",mm.Def15),
                  new SqlParameter("Def16",mm.Def16),
                };
                string n = Update(pro, "UpdateProductinfo");//根据货号更新product表的信息

                Dictionary<string, string> dic = new Dictionary<string, string>();
                if (AttrId != "")
                {
                    string[] ArryComAttrvalues, ArryAttrId, ArryPropertyId;
                    ArryAttrId = AttrId.Split(',');
                    ArryComAttrvalues = ComAttrvalues.Split(',');
                    ArryPropertyId = PropertyId.Split(',');
                    for (int i = 0; i < ArryAttrId.Length; i++)
                    {
                        string check = @"select * from productPorpertyValue where Id='" + ArryAttrId[i] + "' ";//判断该属性是否存在

                        if (DbHelperSQL.Query(check).Tables[0].Rows.Count == 0)//不存在则根据货号以及类别添加属性以及属性值 存在则跟新属性值
                        {
                            IDataParameter[] pro1 = new IDataParameter[]{
                                   new SqlParameter("Cat2",mm.Cat2),
                                   new SqlParameter("PropertyId ",ArryPropertyId[i]),
                                   new SqlParameter("Scode",mm.Scode),
                                   new SqlParameter("PropertyValue",ArryComAttrvalues[i]),
                                   new SqlParameter("PropertyIndex","1"),
                                   new SqlParameter("UserId",UserId),
                                 };
                            string m = Add(pro1, "InsertPropertyValue");

                        }
                        else
                        {
                            IDataParameter[] pro1 = new IDataParameter[]{
                                   new SqlParameter("PropertyValue",ArryComAttrvalues[i]),
                                   new SqlParameter("Id",ArryAttrId[i]),
                                   new SqlParameter("sql",""),
                                 };
                            string m = Update(pro1, "UpdatePropertyValue");

                        }
                    }
                }
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateProduct()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "通过货号修改商品信息->" + mm.Scode,
                    UserId = UserId,
                });
                return true;
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateProduct()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return false;
            }
        }
        /// <summary>
        /// 获取拉列表商品类别显示相应属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetPropertyByTypeNo(string Scode, string TypeNo)
        {
            DataTable dt = new DataTable();
            try
            {

                IDataParameter[] ipr = new IDataParameter[]{
                    new SqlParameter("Scode",Scode),
                    new SqlParameter("TypeNo",TypeNo),
              
            };
                dt = Select(ipr, "GetProductAttr");//通过改变类别来显示该类别存在哪些属性 并返回给页面显示
                return dt;

            }
            catch (Exception ex)
            {
                return dt;
            }
        }
        /// <summary>
        /// 获取Hs列表
        /// </summary>货品绑定hs编码 通过hs编码或者名称模糊查询hs编码
        public DataTable SearchHSinfo(string HSNumber, string TypeName)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = from c in context.HSInfomation select c;
            if (HSNumber != "")
            {
                q = q.Where(a => a.HSNumber.Contains(HSNumber));
            }
            if (TypeName != "")
            {
                q = q.Where(a => a.TypeName.Contains(TypeName));
            }
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;

        }

        /// <summary>
        /// 存储编辑信息 Product主款号表
        /// </summary>通过款号对styledescript进行修改 存储信息为产品描述
        public int Updatestyledescript(string style, string productDescript, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var check = context.styledescript.Where(a => a.style == style).FirstOrDefault();
                if (check != null)//判断是否已存描述信息  存在则update  不存在则insert
                {
                    var q = context.styledescript.Where(a => a.style == style);
                    foreach (var item in q)
                    {
                        item.productDescript = productDescript;//描述
                        item.UserId = UserId;//操作人
                    }
                    context.SubmitChanges();
                }
                else
                {
                    model.styledescript m = new model.styledescript()
                    {
                        style = style,//款号
                        productDescript = productDescript,//描述
                        UserId = UserId,//操作人
                    };
                    context.styledescript.InsertOnSubmit(m);
                    context.SubmitChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
            }


        }
        public bool Insert(IDataParameter[] param)
        {
            string sqlStr = "insert into product  " +
                "values(@Scode,'@Bcode,@Bcode2,@Descript,@Cdescript,@Unit,@Currency,@Cat," +
                "@Cat1,@Cat2,@Clolor,@Size,@Style,@Pricea,@Priceb,@Pricec,@Priced,@Pricee,@Disca,@Discb,@Discc,@Discd,@Disce," +
                "@Vencode,@Model,@Rolevel,@Roamt,@Stopsales,@Loc,@Balance,@Lastgrnd,@Imagefile," +
                "@Def,null,null,null,null,null," +
                "null,null,null,null,null)";
            int returnR = DbHelperSQL.ExecuteSql(sqlStr);
            int returnRowCount = DbHelperSQL.ExecuteSql(sqlStr);
            return returnRowCount > 0 ? true : false;
        }
        ////添加测试数据
        private void Add()
        {
            //DbHelperSQL.ExecuteSql("insert into persona values('Uzi',1,1,null,null,null,null,null)");
            //DbHelperSQL.ExecuteSql("insert into users values(1,'omgCool','123456','Uzi',1,'18211474147','深圳罗湖','913893239@qq.com',1,2,1,null,null,null,null,null)");
            //DbHelperSQL.ExecuteSql("insert into productsource values('111111','逗比','0755-1122145','Uzi','V1',1,null,null,null,null,null)");
            int i = 0;
            while (i < 5)
            {
                i++;
                DbHelperSQL.ExecuteSql("insert into product  values('123','321','321','啊啊啊啊啊','OGM','件','RMB','OOO','冬季','TTY','红色','XL','YOUh',123,123,123,123,123,3,3,2,1,3,111111,'TTTT',10,10,5,'阿里巴巴',500,'2015-02-11',null,null,null,null,null,null,null,null,null,null,null,null)");
            }
        }
        /// <summary>
        /// 将打datatable转换为list集合
        /// </summary>
        /// <param name="dataRow">datatable rows</param>
        /// <returns></returns>
        private List<model.product> DataTableToList(DataRowCollection dataRow)
        {
            List<model.product> list = new List<model.product>();
            foreach (DataRow row in dataRow)
            {
                model.product pro = new model.product();
                if (row["Id"] != null)
                {
                    pro.Id = int.Parse(row["Id"].ToString());
                }
                if (row["Scode"] != null)
                {
                    pro.Scode = row["Scode"].ToString();
                }
                if (row["Bcode"] != null)
                {
                    pro.Bcode = row["Bcode"].ToString();
                }
                if (row["Bcode2"] != null)
                {
                    pro.Bcode2 = row["Bcode2"].ToString();
                }
                if (row["Descript"] != null)
                {
                    pro.Descript = row["Descript"].ToString();
                }
                if (row["Cdescript"] != null)
                {
                    pro.Cdescript = row["Cdescript"].ToString();
                }
                if (row["Unit"] != null)
                {
                    pro.Unit = row["Unit"].ToString();
                }
                if (row["Currency"] != null)
                {
                    pro.Currency = row["Currency"].ToString();
                }
                if (row["Cat"] != null)
                {
                    pro.Cat = row["Cat"].ToString();
                }
                if (row["Cat1"] != null)
                {
                    pro.Cat1 = row["Cat1"].ToString();
                }
                if (row["Cat2"] != null)
                {
                    pro.Cat2 = row["Cat2"].ToString();
                }
                if (row["Clolor"] != null)
                {
                    pro.Clolor = row["Clolor"].ToString();
                }
                if (row["Size"] != null)
                {
                    pro.Size = row["Size"].ToString();
                }
                if (row["Style"] != null)
                {
                    pro.Style = row["Style"].ToString();
                }
                if (row["Pricea"] != null)
                {
                    pro.Pricea = decimal.Parse(row["Pricea"].ToString());
                }
                if (row["Priceb"] != null)
                {
                    pro.Priceb = decimal.Parse(row["Priceb"].ToString());
                }
                if (row["Pricec"] != null)
                {
                    pro.Pricec = decimal.Parse(row["Pricec"].ToString());
                }
                if (row["Priced"] != null)
                {
                    pro.Priced = decimal.Parse(row["Priced"].ToString());
                }
                if (row["Pricee"] != null)
                {
                    pro.Pricee = decimal.Parse(row["Pricee"].ToString());
                }
                if (row["Disca"] != null)
                {
                    pro.Disca = decimal.Parse(row["Disca"].ToString());
                }
                if (row["Discb"] != null)
                {
                    pro.Discb = decimal.Parse(row["Discb"].ToString());
                }
                if (row["Discc"] != null)
                {
                    pro.Discc = decimal.Parse(row["Discc"].ToString());
                }
                if (row["Discd"] != null)
                {
                    pro.Discd = decimal.Parse(row["Discd"].ToString());
                }
                if (row["Disce"] != null)
                {
                    pro.Disce = decimal.Parse(row["Disce"].ToString());
                }
                if (row["Vencode"] != null)
                {
                    pro.Vencode = row["Vencode"].ToString();
                }
                if (row["Model"] != null)
                {
                    pro.Model = row["Model"].ToString();
                }
                if (row["Rolevel"] != null)
                {
                    pro.Rolevel = int.Parse(row["Rolevel"].ToString());
                }
                if (row["Roamt"] != null)
                {
                    pro.Roamt = int.Parse(row["Roamt"].ToString());
                }
                if (row["Stopsales"] != null)
                {
                    pro.Stopsales = int.Parse(row["Stopsales"].ToString());
                }
                if (row["Loc"] != null)
                {
                    pro.Loc = row["Loc"].ToString();
                }
                if (row["Balance"] != null)
                {
                    pro.Balance = int.Parse(row["Balance"].ToString());
                }
                if (row["Lastgrnd"] != null)
                {

                    string datae = row["Lastgrnd"].ToString();
                    //pro.Lastgrnd = DateTime.Parse(row["Lastgrnd"].ToString());
                    pro.Lastgrnd = GetDateTimeParms(datae);
                }
                //if (row["Imagefile"] != null && typeof(System.DBNull) != row["Imagefile"].GetType())
                //{
                //    pro.Imagefile = (byte[])row["Imagefile"];
                //}
                if (row["Imagefile"] != null)
                {
                    pro.Imagefile = "";
                }
                if (row["Def1"] != null)
                {
                    pro.Def1 = row["Def1"].ToString();
                }
                if (row["Def2"] != null)
                {
                    pro.Def2 = row["Def2"].ToString();
                }
                if (row["Def3"] != null)
                {
                    pro.Def3 = int.Parse(row["Def3"].ToString());
                }
                if (row["ciqProductId"] != null)
                {
                    pro.ciqProductId = row["ciqProductId"].ToString();
                }
                if (row["ciqSpec"] != null)
                {
                    pro.ciqSpec = row["ciqSpec"].ToString();
                }
                if (row["ciqHSNo"] != null)
                {
                    pro.ciqHSNo = row["ciqHSNo"].ToString();
                }
                if (row["ciqAssemCountry"] != null)
                {
                    pro.ciqAssemCountry = row["ciqAssemCountry"].ToString();
                }
                if (row["ciqProductNo"] != null)
                {
                    pro.ciqProductNo = Convert.ToInt32(row["ciqProductNo"].ToString());
                }
                if (row["Def9"] != null)
                {
                    pro.Def9 = row["Def9"].ToString();
                }
                if (row["Def10"] != null)
                {
                    pro.Def10 = row["Def10"].ToString();
                }
                if (row["Def11"] != null)
                {
                    pro.Def11 = row["Def11"].ToString();
                }
                list.Add(pro);
            }
            return list;
        }


        /// <summary>
        /// 获取byte[]参数进行过滤
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public byte[] GetByteParms(object parms)
        {
            byte[] bt = new byte[65536]; ;
            if (parms == null || parms.ToString() == "")
            {
                bt = null;
            }
            else { bt = (byte[])parms as byte[]; }
            return bt;
        }

        /// <summary>
        /// 获取DateTime参数进行过滤
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static DateTime GetDateTimeParms(object parms)
        {
            DateTime time = new DateTime(0);
            if (parms != null)
                DateTime.TryParse(parms.ToString().Trim(), out time);
            return time;
        }


        public DataTable GetShopNameByStyle(string style)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("style",style),
            };
            return Select(ipr, "GetShopNameByStyle");
        }
    }
}
