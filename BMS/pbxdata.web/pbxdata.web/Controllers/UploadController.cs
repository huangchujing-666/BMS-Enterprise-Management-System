using Aliyun.OpenServices.OpenStorageService;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using pbxdata.model;
using pbxdata.idal;
using System.Data.SqlClient;
using System.Xml;
using System.Net;
using System.Text;

namespace pbxdata.web.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/
        //DotNet.Utilities.FTPHelper FTPHelper = new DotNet.Utilities.FTPHelper("", "", "X9foTnzzxHCk6gK7", "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz");
        aliyun.ControlAliyun CAliyun = new aliyun.ControlAliyun();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadPics()
        {
            txtwrite();
            return View();
        }
        public void txtwrite()
        {
            //FTPHelper.Upload(@"E:\工作图片\Test\05V32RJ50F");

        }
        public void UploadPictures()
        {
            DirectoryInfo theFolder = new DirectoryInfo(@"E:\工作图片\Test\05V32RJ50F");
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            DataTable dtpath = new DataTable();
            DataTable dtcount = new DataTable();
            //遍历文件夹
            foreach (DirectoryInfo temp in dirInfo)//dir所有文件夹名
            {
                //string Picurl;
                //string scode = temp.ToString();
                //string path = CAliyun.GetObjectlistScode(scode);//得到货号文件夹路径
                //string pathsql = @"select * from product where Scode='" + scode.Replace("_","*") + "'";
                //dtpath = DbHelperSQL.Query(pathsql).Tables[0];
                //string newpath = "images/" + dtpath.Rows[0]["Cat"] + "/" + dtpath.Rows[0]["Cat2"] + "/" + dtpath.Rows[0]["Style"] + "/" + dtpath.Rows[0]["Scode"] + "/";
                //dtcount = DbHelperSQL.Query(@"select count(*) from scodepic where scode='" + scode + "'").Tables[0];
                //int i = Convert.ToInt32(dtcount.Rows[0][0]);//判断数据中对应货号存放图片的个数
                FileInfo[] fileInfo = temp.GetFiles();//文件名
                foreach (FileInfo NextFile in fileInfo)  //遍历文件
                {
                    FileStream sFile = new FileStream(@"E:\工作图片\Test\05V32RJ50F\" + NextFile, FileMode.Open);
                }
            }

        }
        public void PutObject(string filename, string objkey)
        {
            //初始化 OSSClient
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            string bucketName = "best-bms";
            OssClient client = new OssClient(endpoint, accessId, accessKey);

            //定义文件流
            var objStream = new System.IO.FileStream(Server.MapPath("~/UploadImage/") + filename, System.IO.FileMode.OpenOrCreate);

            //定义 object 描述
            var objMetadata = new ObjectMetadata();
            //执行 put 请求，并且返回对象的MD5摘要。
            var putResult = client.PutObject(bucketName, objkey, objStream, objMetadata);
        }
        /// <summary>
        /// 图片存到阿里云--主货号表
        /// </summary>
        public bool UploadScodePic()
        {
            bool flag = false;
            try
            {
                DataTable dtpath = new DataTable();
                DataTable dtcount = new DataTable();
                string srcs = Request.QueryString["Arrysrc"];
                string scode = Request.QueryString["scode"];
                string[] arrysrc;
                arrysrc = srcs.Split(',');
                string Picurl;
                string path = CAliyun.GetObjectlistScode(scode);//得到货号文件夹路径
                string pathsql = @"select * from product where Scode='" + scode.Replace("_", "*") + "'";
                dtpath = DbHelperSQL.Query(pathsql).Tables[0];
                string newpath = "images/" + dtpath.Rows[0]["Cat"] + "/" + dtpath.Rows[0]["Cat2"] + "/" + dtpath.Rows[0]["Style"] + "/" + dtpath.Rows[0]["Scode"] + "/";
                dtcount = DbHelperSQL.Query(@"select count(*) from scodepic where scode='" + scode + "'").Tables[0];
                int i = Convert.ToInt32(dtcount.Rows[0][0]);//判断数据中对应货号存放图片的个数

                if (CAliyun.ScodeExist(scode.Replace("*", "_")))//判断该货号文件夹是否存在
                {
                    foreach (var temp in arrysrc)
                    {
                        string filename = scode + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.oss-cn-shenzhen.aliyuncs.com/" + path + i + "_" + filename;

                        string sql = @"insert into scodepic values ('" + scode.Replace("_", "*") + "','" + Picurl + "','1','1','1','1','1','1','1','1','1')";
                        int n = DbHelperSQL.ExecuteSql(sql);
                        if (n != -1)
                        {
                            PutObject(getFile, path + i + "_" + filename);//存入新上传图片
                        }
                        i++;

                    }
                }
                else
                {
                    CAliyun.CreateEmptyFolder(newpath);//创建款号文件夹
                    foreach (var temp in arrysrc)
                    {
                        string filename = scode + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.oss-cn-shenzhen.aliyuncs.com/" + newpath + i + "_" + filename;
                        string sql = @"insert into scodepic values ('" + scode.Replace("_", "*") + "','" + Picurl + "','1','1','1','1','1','1','1','1','1')";
                        int n = DbHelperSQL.ExecuteSql(sql);
                        if (n != -1)
                        {
                            PutObject(getFile, newpath + i + "_" + filename);//存入新上传图片
                        }
                        i++;
                    }

                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        public string GetExcell()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            DataTable dt = new DataTable();
            string sql = "";
            dt = GetData(@"D:\20150803.xls", "Sheet2$").Tables[0];
            List<string> list = new List<string>();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    list.Add(dt.Rows[i][0].ToString().Trim());

            //}
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() != "")
                {
                    //string QtyUnit = DbHelperSQL.Query(@"select unitNo from apiUnit where unitName='" + dt.Rows[i][6] + "'").Tables[0].Rows[0]["unitNo"].ToString();
                    //sql += "update product set ciqHSNo='" + dt.Rows[i][1] + "',Cdescript='" + dt.Rows[i][2] + "',ciqAssemCountry='" + dt.Rows[i][3] + "',Def15='" + dt.Rows[i][5] + "',Def16='" + dt.Rows[i][4] + "',QtyUnit='" + QtyUnit + "' where Scode='" + dt.Rows[i][0] + "';<br />";
                    //sql += "insert into product(Scode) values('" + dt.Rows[i][0] + "1111111111');<br />";
                    if (dt.Rows[i][4].ToString() == "")
                    {
                        sql += "insert into AreaTable(Region_id,Parent_id,Region_name) values (" + dt.Rows[i][1] + "," + dt.Rows[i][2] + ",'" + dt.Rows[i][3] + "');<br />";

                    }
                    else
                    {
                        sql += "insert into AreaTable(Region_id,Parent_id,Region_name,Post_code) values (" + dt.Rows[i][1] + "," + dt.Rows[i][2] + ",'" + dt.Rows[i][3] + "'," + dt.Rows[i][4] + ");<br />";
                    }

                }

                //sql += "insert into productCustomsResult(productScode,RegStatus) values('" + dt.Rows[i][0] + "','10')<br />";  
            }
            //DbHelperSQL.ExecuteSql(sql);ciqAssemCountry

            return sql;
        }
        public DataSet GetData(string filename, string workTableName)
        {
            DataSet ds;
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Extended Properties=Excel 8.0;" + "data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [" + workTableName + "]";
            try
            {
                Open(myConn);//打开数据库
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                ds = new DataSet();
                myCommand.Fill(ds);
                myCommand.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Close(myConn);
            }
        }
        private void Open(OleDbConnection con)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
        private void Close(OleDbConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        public string Excell11(DataTable dt)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
          
            app.Visible = false;
            app.UserControl = true;
            Microsoft.Office.Interop.Excel.Workbooks workbooks = app.Workbooks;
            Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(Server.MapPath("~/Uploadxls/test.xls")); //加载模板
            Microsoft.Office.Interop.Excel.Sheets sheets = workbook.Sheets;
            Microsoft.Office.Interop.Excel._Worksheet worksheet = (Microsoft.Office.Interop.Excel._Worksheet)sheets.get_Item(1); //第一个工作薄。
            if (worksheet == null)
            {
                return "";  //工作薄中没有工作表.
            }

            for (int i = 1; i <= dt.Rows.Count; i++)
            {

                int row_ = 1 + i;  //Excel模板上表头和标题行占了2行,根据实际模板需要修改;
                int dt_row = i - 1; //dataTable的行是从0开始的。 
                worksheet.Cells[row_, 1] = i.ToString();
                //worksheet.Cells[row_, 2] = dt.Rows[dt_row][""].ToString();
                //worksheet.Cells[row_, 3] = dt.Rows[dt_row][""].ToString();
                for (int j = 1; j < dt.Columns.Count; j++)
                {
                    string item = dt.Rows[dt_row][j].ToString();
                    worksheet.Cells[row_, j + 1] = item;
                }

            }


            string savaPath = "~/Uploadxls/T1_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            string uploadPath = "Uploadxls/T1_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            workbook.SaveAs(Server.MapPath(savaPath), Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            workbook.Close();
            string Host = Request.Url.Host + ":" + Request.Url.Port;
            return "http://" + Host + "/" + savaPath.Replace("~/", "");
        }
        public string mytest()
        {
            try
            {
                string s = string.Empty;
                //FileStream fs = new FileStream(Server.MapPath("~/Uploadxls/test.txt"), FileMode.OpenOrCreate);
                //StreamWriter sw = new StreamWriter(fs);
                ////开始写入
                //sw.Write("Hello World!!!!");
                ////清空缓冲区
                //sw.Flush();
                ////关闭流
                //sw.Close();
                //fs.Close();
                DataTable dt = new DataTable();
                pbxdatasourceDataContext con = new pbxdatasourceDataContext();
                var q = con.custom.Where(a => a.Id > 0);
                dt = LinqToDataTable.LINQToDataTable(q);
                s = Excell11(dt);
                return s;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
            
        }
        public string GetGeneralContent(string strUrl)
        {
            string strMsg = string.Empty;
            strUrl = @"http://www.farfetch.com/cn/shopping/women/carven--item-11087090.aspx?storeid=9298&ffref=lp_2_3_";
            try
            {
                WebRequest request = WebRequest.Create(strUrl);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                strMsg = reader.ReadToEnd();


                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            { }
            return strMsg;
        }
        public void CreateTXT()
        {
            string txt = Request.Form["test"].ToString();
            FileInfo file = new FileInfo(@"D:\FileTest\test.txt");
            if (!file.Exists)
            {
                FileStream fs1 = new FileStream(@"D:\FileTest\test.txt", FileMode.Create, FileAccess.Write);//创建写入文
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine("123|456|789");
                sw.Close();
                fs1.Close();
            }
            else
            {
                FileStream fs1 = new FileStream(@"D:\FileTest\test.txt", FileMode.Open, FileAccess.Write);//创建写入文
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(txt);
                sw.Close();
                fs1.Close();
            }


        }
        public string ReturnInfo()
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.apiOrderDetails
                    join t in context.apiOrder on c.orderId equals t.orderId
                    into tt
                    from ss in tt.DefaultIfEmpty()
                    select new
                    {
                        OrderId = c.orderId,            //--订单号
                        paidPrice = ss.paidPrice,     //--  实际支付价格  
                        isPay = ss.isPay,//--是否支付
                        detailsSaleCount = c.detailsSaleCount,//--交易数量
                        createTime = ss.createTime,//--创建时间
                    };

            DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime tomorrow = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));

            var Count = q.Where(a => a.isPay == 1).GroupBy(a => a.OrderId).Count();

            var SumPrice = q.Where(a => a.isPay == 1).Sum(a => a.paidPrice);
            SumPrice = SumPrice == null ? 0 : SumPrice;

            var BalaceCount = q.Where(a => a.isPay == 1).Sum(a => a.detailsSaleCount);
            BalaceCount = BalaceCount == null ? 0 : BalaceCount;

            var SumPrice1 = q.Where(a => a.isPay == 2).Sum(a => a.paidPrice);
            SumPrice1 = SumPrice1 == null ? 0 : SumPrice1;

            var BalaceCount1 = q.Where(a => a.isPay == 2).Sum(a => a.detailsSaleCount);
            BalaceCount1 = BalaceCount1 == null ? 0 : BalaceCount1;

            var tCount = q.Where(a => a.isPay == 1 && a.createTime < tomorrow && a.createTime > today).GroupBy(a => a.OrderId).Count();

            var tSumPrice = q.Where(a => a.isPay == 1 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.paidPrice);
            tSumPrice = tSumPrice == null ? 0 : tSumPrice;

            var tBalaceCount = q.Where(a => a.isPay == 1 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.detailsSaleCount);
            tBalaceCount = tBalaceCount == null ? 0 : tBalaceCount;

            var tSumPrice1 = q.Where(a => a.isPay == 2 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.paidPrice);
            tSumPrice1 = tSumPrice1 == null ? 0 : tSumPrice1;
            var tBalaceCount1 = q.Where(a => a.isPay == 2 && a.createTime < tomorrow && a.createTime > today).Sum(a => a.detailsSaleCount);
            tBalaceCount1 = tBalaceCount1 == null ? 0 : tBalaceCount1;
            s += "<ul class='ulCount'>";
            s += "<li>今日支付笔数:<span>" + tCount + "</span>.</li>";
            s += "<li>今日支付总金额:<span>" + Convert.ToDecimal(tSumPrice).ToString("f2") + "</span>￥</li>";
            s += "<li>今日销售总件:<span>" + tBalaceCount + "</span>.</li>";
            s += "<li>今日未支付笔数:<span>" + tBalaceCount1 + "</span>.</li>";
            s += "<li>今日未支付金额:<span>" + Convert.ToDecimal(tSumPrice1).ToString("f2") + "</span>￥.</li>";
            s += "</ul>";
            s += "<div class='clearfix'></div>";
            s += "<ul class='ulCount'>";
            s += "<li>支付笔数:<span>" + Count + "</span>.</li>";
            s += "<li>支付总金额:<span>" + Convert.ToDecimal(SumPrice).ToString("f2") + "</span>￥.</li>";
            s += "<li>销售总件:<span>" + BalaceCount + "</span>.</li>";
            s += "<li>未支付笔数:<span>" + BalaceCount1 + "</span>.</li>";
            s += "<li>未支付金额:<span>" + Convert.ToDecimal(SumPrice1).ToString("f2") + "</span>￥.</li>";
            s += "</ul>";

            //s = "[{\"Count\":" + Count + ",\"SumPrice\":" + SumPrice + ",\"BalaceCount\":" + BalaceCount + ",\"SumPrice1\":" + SumPrice1 + ",\"BalaceCount1\":" + BalaceCount1 + ",\"tCount\":" + tCount + ",\"tSumPrice\":" + tSumPrice + ",\"tBalaceCount\":" + tBalaceCount + ",\"tSumPrice1\":" + tSumPrice1 + ",\"tBalaceCount1\":" + tBalaceCount1 + "}]";
            return s;
        }

        public string ReturnProduct()
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var sql = @"select top 20 BrandName,SUM(Balance) as balance from productstock a left join brand b on a.Cat=b.BrandAbridge where BrandName is not null group by BrandName order by balance desc";
            var sql1 = @"select top 20 TypeName,SUM(Balance) as balance from productstock a left join producttype b on a.Cat2=b.TypeNo where TypeName is not null group by TypeName order by balance desc";
            //var q = from c in context.productstock
            //        group c by new { c.Cat2 } into g
            //        select new
            //        {
            //            Cat2 = g.Key.Cat2,
            //            CountBa = g.Sum(a => a.Balance),
            //        };
            //var qCat2 = from c in q.OrderByDescending(a => a.CountBa)
            //            join p in context.producttype on c.Cat2 equals p.TypeNo
            //             into pp
            //            from ppp in pp.DefaultIfEmpty()

            //            select new
            //            {
            //                TypeName = ppp.TypeName,
            //                CountBa = c.CountBa,
            //            };
            //DataTable dt3 = LinqToDataTable.LINQToDataTable(qCat2.Where(a => a.TypeName != null));
            var Brand = context.product.GroupBy(a => a.Cat2).Count();
            var Balance = context.productstock.Sum(a => a.Balance);
            var Imagefile = context.product.Where(a => a.Imagefile != "" && a.Imagefile != null).Count();
            var TypeName = context.product.GroupBy(a => a.Cat).Count();
            var Style = context.product.GroupBy(a => a.Style).Count();
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            s += "<ul class='ulCount'>";
            s += "<li>总品牌数:<span>" + Brand + "</span>.</li>";
            s += "<li>总类别数:<span>" + TypeName + "</span>.</li>";
            s += "<li>总款数:<span>" + Style + "</span>.</li>";
            s += "<li>总件数:<span>" + Balance + "</span>.</li>";
            s += "<li>有图片sku数:<span>" + Imagefile + "</span>.</li>";
            s += "</ul>";
            s += "-*-";
            for (int i = 0; i < 20; i++)
            {
                int n = i + 1;
                s += n + "." + dt.Rows[i]["BrandName"].ToString() + "<br />";
            }
            s += "-*-";
            for (int i = 0; i < 20; i++)
            {
                int n = i + 1;
                s += n + "." + dt1.Rows[i]["TypeName"].ToString() + "<br />";
            }


            return s;
        }

        public string GetProvice()
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string RegionName = Request.Form["RegionName"].ToString();
            var q = context.AreaTable.Where(a => a.Parent_id == 0);
            dt = LinqToDataTable.LINQToDataTable(q);
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (RegionName == dt.Rows[i]["Region_name"].ToString())
                {
                    s += "<option value='" + dt.Rows[i]["Region_id"] + "' selected='selected' >" + dt.Rows[i]["Region_name"] + "</option>";
                }
                else
                {
                    s += "<option value='" + dt.Rows[i]["Region_id"] + "'>" + dt.Rows[i]["Region_name"] + "</option>";
                }
            }
            return s;
        }

        public string GetCity()
        {
            string s = string.Empty;
            int Parent_id = Convert.ToInt32(Request.Form["RegionId"].ToString());
            DataTable dt = new DataTable();
            string RegionName = Request.Form["RegionName"].ToString();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = context.AreaTable.Where(a => a.Parent_id == Parent_id);
            dt = LinqToDataTable.LINQToDataTable(q);
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (RegionName == dt.Rows[i]["Region_name"].ToString())
                {
                    s += "<option value='" + dt.Rows[i]["Region_id"] + "' selected='selected' >" + dt.Rows[i]["Region_name"] + "</option>";
                }
                else
                {
                    s += "<option value='" + dt.Rows[i]["Region_id"] + "'>" + dt.Rows[i]["Region_name"] + "</option>";
                }
            }
            return s;
        }

        public string GetDistrict()
        {
            string s = string.Empty;
            int Parent_id = Convert.ToInt32(Request.Form["RegionId"].ToString());
            DataTable dt = new DataTable();
            string RegionName = Request.Form["RegionName"].ToString();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = context.AreaTable.Where(a => a.Parent_id == Parent_id);
            dt = LinqToDataTable.LINQToDataTable(q);
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (RegionName == dt.Rows[i]["Region_name"].ToString())
                {
                    s += "<option value='" + dt.Rows[i]["Region_id"] + "' selected='selected' >" + dt.Rows[i]["Region_name"] + "</option>";
                }
                else
                {
                    s += "<option value='" + dt.Rows[i]["Region_id"] + "'>" + dt.Rows[i]["Region_name"] + "</option>";
                }
            }
            return s;
        }
       


        public string UploadExcel()
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            HttpPostedFileBase file1 = Request.Files[0];
            string newFile = DateTime.Now.ToString("yyyyMMddHHmmss");
            string filePath = newFile + file1.FileName;
            file1.SaveAs(Server.MapPath("~/Uploadxls/" + filePath));
            dt = GetData(Server.MapPath("~/Uploadxls/" + filePath), "Sheet1$").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string ss = dt.Columns[0].ColumnName;
                if (dt.Rows[i][0].ToString() != "")
                {
                    s += i + "," + dt.Rows[i][0] + "<br />";
                }
            }
            return s;
        }
       
    }

    

}
