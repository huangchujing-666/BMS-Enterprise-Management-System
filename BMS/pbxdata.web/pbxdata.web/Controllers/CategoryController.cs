using Maticsoft.DBUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;
using System.IO;
using System.Net;
using System.Drawing;
using Aliyun.OpenServices.OpenStorageService;
using System.Data.SqlClient;
using pbxdata.bll;

namespace pbxdata.web.Controllers
{
    public class CategoryController : BaseController
    {
        //
        // GET: /Category/
        string url = "http://gw.api.taobao.com/router/rest ";
        string appkey = "21738994";//--值在taoAppUser表中
        string appsecret = "94bbf1b464e4983726c97b4461dfb448";
        string sessionKey = "61007278ecd41be4ead2e3859a0f4169a1d18d3002122dc1969004137";//--攀比轩
        string Access_token = "6100827dfda310ce6104ffd3c18960a110c1700c4a50f4c1969004137";//--攀比轩
        string sessionKey1 = "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799";//--佛罗伦斯
        string Access_token1 = "6101a06da22d1a1cc93f331ff3b4333bfaee0a71ea7146f1954282799";//--佛罗伦斯
        bll.Attributebll attrbll = new bll.Attributebll();
        const string accessId = "X9foTnzzxHCk6gK7";
        const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
        const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
        string bucketName = "best-bms";

        public ActionResult TBAddGoods()
        {
            //OssClient client = new OssClient(endpoint, accessId, accessKey);
            //var imgs = client.GetObject(bucketName, @"images/BL/L0100/357331DBCAN/357331DBCAN5811F/0_357331DBCAN5811F.jpg");
            //var objStream = imgs.Content;
            DeleteDir("");
            string style = Request.QueryString["style"];
            string ShopId = Request.QueryString["ShopId"];
            ViewBag.ShopId = ShopId;
            ViewBag.style = style;
            return View();


        }
        public ActionResult Index()
        {
            // string accessId = "X9foTnzzxHCk6gK7";
            // string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            // string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            //string bucketName = "best-bms";
            //Aliyun.OpenServices.OpenStorageService.OssClient client = new Aliyun.OpenServices.OpenStorageService.OssClient(endpoint, accessId, accessKey);
            //client.DeleteObject(bucketName, @"images/ZILLI/M0100/ZILLI00/ZILLI00/0_ZILLI00.jpg");
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }
        /// <summary>
        /// 商品上货
        /// </summary>
        public string addProduct()
        {
            string error = "";
            try
            {
                if (Request.Form["ShopId"].ToString() == "2")
                {
                    sessionKey = "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799";//--佛罗伦斯
                    Access_token = "6101a06da22d1a1cc93f331ff3b4333bfaee0a71ea7146f1954282799";//--佛罗伦斯
                }

                ITopClient client = new DefaultTopClient(url, appkey, appsecret);
                ItemAddRequest req = new ItemAddRequest();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string StyleNum = Request.Form["StyleNum"];
                string TBNumber = "";
                string name = "";
                string Props = "";
                string sql = @"select a.*,c.Imagefile as images,b.Def1 as cid,d.stylePicSrc,d.productDescript  from productstock a 
  left join producttype b on a.Cat2=b.TypeNo 
  left join product c on a.Scode=c.Scode 
  left join styledescript d on a.Style=d.style where a.Style='" + StyleNum + "'";
                dt = DbHelperSQL.Query(sql).Tables[0];
                string co = Request.Form["Colors[]"];
                string[] color = Request.Form["Colors[]"].Split(',');
                string[] AttrProps = Request.Form["Props[]"].Split(',');
                foreach (string item in AttrProps)
                {
                    Props += item + ";";
                }
                Props = Props.Trim(';');
                long cid = long.Parse(dt.Rows[0]["cid"].ToString());
                req.Cid = cid;                                                //--子类目
                req.Type = Request.Form["Type"].ToString();                             //--天猫
                req.StuffStatus = "new";               //--新旧程度
                req.Title = Request.Form["Title"].ToString();                                //--宝贝标题 不能超过30个字
                req.Desc = Request.Form["Desc"].ToString();          //--宝贝描述 不少于5个字               
                req.LocationState = Request.Form["State"].ToString();                     //--省份
                req.LocationCity = Request.Form["City"].ToString();                      //--城市
                req.ApproveStatus = Request.Form["Status"].ToString();                   //--上货后状态 默认Instock 在仓库中
                req.Props = Props;
                req.FreightPayer = Request.Form["Payer"].ToString();                    //--运费承担 默认卖家
                req.HasInvoice = true;
                req.AuctionPoint = long.Parse(Request.Form["Point"]);         //--积分返点 默认5（0.5%）
                req.Num = long.Parse(Request.Form["Num"]);
                req.Price = Request.Form["Price"];
                if (Request.Form["ShopId"].ToString() == "2")
                {
                    req.HasInvoice = false;//--佛罗伦斯   没发票 还要设置体积重量
                    req.ItemWeight = "1";
                    req.ItemSize = "bulk:1";
                }
                FileItem fi = new FileItem(Server.MapPath("~/image/NOIMAGE.jpg"));
                if (dt.Rows[0]["stylePicSrc"].ToString() != "")
                {
                    using (var webClient = new WebClient())
                    {
                        //string path = @"http://best-bms.pbxluxury.com/images/BL/L0100/357331DBCAN/357331DBCAN5811F/0_357331DBCAN5811F.jpg";
                        string path = dt.Rows[0]["stylePicSrc"].ToString();
                        var obj = webClient.DownloadData(path);
                        using (var stream = new MemoryStream(obj))
                        {
                            var img = Image.FromStream(stream);
                            name = path.Substring(path.LastIndexOf('/') + 1);
                            img.Save(Server.MapPath("~/UploadImage/" + name));
                        }
                    }
                    fi = new FileItem(Server.MapPath("~/UploadImage/" + name)); ;

                }
                req.Image = fi;//--主图
                req.ProductId = GetProductId(cid, Props, Request.Form["Brand"], StyleNum, fi, out error);
                ItemAddResponse response = client.Execute(req, sessionKey);
                string json = JsonConvert.SerializeObject(response);
                JObject jsonObj = JObject.Parse(json);
                if (jsonObj["ErrCode"].ToString() != "")
                {
                    error = jsonObj["SubErrMsg"].ToString();
                    //TBNumber = long.Parse(System.Text.RegularExpressions.Regex.Replace(jsonObj["SubErrMsg"].ToString(), @"[^\d]*", ""));
                }
                //if (jsonObj["SubErrMsg"].ToString().Contains())  
                TBNumber = jsonObj["Item"]["NumIid"].ToString();

                //添加商品图片
                #region
                if (TBNumber != "")
                {
                    DeleteDir("");
                    string imgsql = @"select *,right(Def1,1) as postion from stylepic where style='" + StyleNum + "' and Left(Def1,1)='2' and Def4='1' order by right(Def1,1)";
                    dt1 = DbHelperSQL.Query(imgsql).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            ItemImgUploadRequest reqImg = new ItemImgUploadRequest();
                            reqImg.NumIid = long.Parse(TBNumber);
                            using (var webClient = new WebClient())
                            {
                                string path = dt1.Rows[i]["stylePicSrc"].ToString();
                                var obj = webClient.DownloadData(path);
                                using (var stream = new MemoryStream(obj))
                                {
                                    var img = Image.FromStream(stream);
                                    name = path.Substring(path.LastIndexOf('/') + 1);
                                    img.Save(Server.MapPath("~/UploadImage/" + name));
                                }
                            }
                            FileItem fis = new FileItem(Server.MapPath("~/UploadImage/" + name));
                            reqImg.Image = fis;
                            reqImg.Position = long.Parse(dt1.Rows[i]["postion"].ToString());
                            ItemImgUploadResponse responseImg = client.Execute(reqImg, sessionKey);
                        }
                    }
                }
                #endregion

                //添加SKU
                #region
                if (TBNumber != "")
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ItemSkuAddRequest reqsku = new ItemSkuAddRequest();
                        reqsku.NumIid = long.Parse(TBNumber);
                        reqsku.Properties = "1627207:" + color[i];
                        reqsku.Quantity = long.Parse(dt.Rows[i]["Balance"].ToString());
                        reqsku.Price = dt.Rows[i]["Pricea"].ToString();
                        reqsku.OuterId = dt.Rows[i]["Scode"].ToString();
                        ItemSkuAddResponse responsesku = client.Execute(reqsku, sessionKey);
                    }
                    ItemGetRequest reqGet = new ItemGetRequest();
                    reqGet.Fields = "num_iid,input_str,title,pic_url,price,num,list_time,delist_time,approve_status,nick";
                    reqGet.NumIid = long.Parse(TBNumber);
                    ItemGetResponse responseGet = client.Execute(reqGet, sessionKey);
                    string jsonget = JsonConvert.SerializeObject(responseGet);
                    JObject jsonObjget = JObject.Parse(jsonget);
                    SqlParameter[] parameters = {
                    new SqlParameter("@ProductReMarkShopId", SqlDbType.BigInt,8),
                    new SqlParameter("@ProductStyle", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ProductTitle", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ProductImg", SqlDbType.NVarChar,500),
                    new SqlParameter("@ProductTaoBaoPrice", SqlDbType.Money,8),
                    new SqlParameter("@ProductYJStock", SqlDbType.Int,4),
                    new SqlParameter("@ProductSJ", SqlDbType.Int,4),
                    new SqlParameter("@ProductState", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductShopName", SqlDbType.NVarChar,50)};
                    parameters[0].Value = jsonObjget["Item"]["NumIid"].ToString();  //商品ID
                    parameters[1].Value = jsonObjget["Item"]["InputStr"].ToString(); //款号
                    parameters[2].Value = jsonObjget["Item"]["Title"].ToString(); //标题
                    parameters[3].Value = jsonObjget["Item"]["PicUrl"].ToString(); //图片路径
                    parameters[4].Value = jsonObjget["Item"]["Price"].ToString(); //淘宝价
                    parameters[5].Value = 1; //预警库存
                    parameters[6].Value = Convert.ToInt32(jsonObjget["Item"]["Num"].ToString()); //上架数
                    parameters[7].Value = jsonObjget["Item"]["ApproveStatus"].ToString(); //状态
                    parameters[8].Value = jsonObjget["Item"]["Nick"].ToString(); //店铺名称
                    string insertsql = @"insert into tbProductReMark (ProductReMarkId,ProductStyle,ProductTitle,ProductImg,ProductTaoBaoPrice,ProductYJStock,ProductSJ,ProductState,ProductShopName) values (@ProductReMarkShopId,@ProductStyle,@ProductTitle,@ProductImg,@ProductTaoBaoPrice,@ProductYJStock,@ProductSJ,@ProductState,@ProductShopName) ";
                    int n = DbHelperSQL.ExecuteSql(insertsql, parameters);

                }
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("fileInfo is null or not exists"))
                {
                    error = "图片错误!(不存在该款缩略图或者没有获取到图片)";
                }
                return error;
            }
            return "上货成功!";
        }
        /// <summary>
        /// 清空文件夹的图片
        /// </summary>
        public void DeleteDir(string aimPath)
        {
            try
            {

                aimPath = Server.MapPath("~/UploadImage/");
                long a = 0;


                if (Directory.Exists(aimPath))
                {
                    DirectoryInfo di = new DirectoryInfo(aimPath);
                    // 检查目标目录存放文件达到500M就删除所有图片
                    foreach (var i in di.GetFiles())
                    {
                        a += i.Length;
                    }
                    if (a > 100000000)//超过100M时候自动删除UploadImage中所有文件
                    {
                        // 检查目标目录是否以目录分割字符结束如果不是则添加之
                        if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                            aimPath += Path.DirectorySeparatorChar;
                        // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                        // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
                        // string[] fileList = Directory.GetFiles(aimPath);
                        string[] fileList = Directory.GetFileSystemEntries(aimPath);
                        // 遍历所有的文件和目录
                        foreach (string file in fileList)
                        {

                            // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                            if (Directory.Exists(file))
                            {
                                DeleteDir(aimPath + Path.GetFileName(file));
                            }
                            //否则直接Delete文件
                            else
                            {
                                System.IO.File.Delete(aimPath + Path.GetFileName(file));
                            }
                        }
                    }


                }
                else
                {
                    Directory.CreateDirectory(aimPath);
                }

            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 获取productID
        /// </summary>
        ///---cid 淘宝编码,Date 淘宝上市时间,Zhidi 淘宝质地,styleName 淘宝款式,brandName 淘宝品牌名,style 款号,image 产品主图片 这些必填字段
        public long GetProductId(long cid, string Props, string brandName, string style, FileItem fi, out string error)
        {
            long productid = 0;
            error = "0";
            JObject jsonObj = new JObject();
            try
            {
                ITopClient client = new DefaultTopClient(url, appkey, appsecret);
                ProductAddRequest req = new ProductAddRequest();
                req.Cid = cid;
                req.Props = Props;
                //req.CustomerProps = "20000:" + brandName + ";13021751:" + style;
                req.CustomerProps = "20000:" + brandName + ";13021751:test12";
                //string name = "";
                //FileInfo file = new FileInfo(@"C:\Users\Administrator\Desktop\33547\33547LIBAJF-3.jpg");
                //using (var webClient = new WebClient())
                //{
                //    string path = image;
                //    var obj = webClient.DownloadData(path);
                //    using (var stream = new MemoryStream(obj))
                //    {
                //        var img = Image.FromStream(stream);
                //        name = path.Substring(path.LastIndexOf('/') + 1);
                //        img.Save(Server.MapPath("~/UploadImage/" + name));
                //    }
                //}
                //FileItem fi = new FileItem(Server.MapPath("~/UploadImage/" + name));
                req.Image = fi;
                req.Name = style;
                req.Desc = "这是一个测试";
                ProductAddResponse response = client.Execute(req, sessionKey);
                string json = JsonConvert.SerializeObject(response);
                jsonObj = JObject.Parse(json);
                string a = jsonObj["Product"].ToString();
                if (jsonObj["Product"].ToString() != "")
                {
                    productid = long.Parse(jsonObj["Product"]["ProductId"].ToString());
                }
                else
                {
                    if (jsonObj["SubErrMsg"].ToString().Contains("您没有该品牌的授权"))
                    {
                        error = jsonObj["SubErrMsg"].ToString();
                        return productid;
                    }
                    else
                    {
                        error = jsonObj["SubErrMsg"].ToString();
                        productid = long.Parse(System.Text.RegularExpressions.Regex.Replace(jsonObj["SubErrMsg"].ToString(), @"[^\d]*", ""));
                    }

                }
                return productid;
            }
            catch (Exception e)
            {
                error = jsonObj["ErrorCode"].ToString();
                return productid;
            }


        }
        /// <summary>
        /// 返回淘宝商品信息
        /// </summary>
        public string rtnProductInfo()
        {
            //string s = "";
            StringBuilder s = new StringBuilder(200);
            int count = 0;
            decimal aveprice = 0;
            string image = "";
            string style = Request.QueryString["style"];
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select a.*,c.TBBrandName,c.vid,d.Imagefile as Images,e.Def1 as cid  from productstock a left join brand b on a.Cat=b.BrandAbridge left join TBBrand c on b.Def1=c.vid left join product d
 on a.Scode=d.Scode left join producttype e on a.Cat2=e.TypeNo where a.Style='" + style + "'").Tables[0];
            ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            ItempropsGetRequest req = new ItempropsGetRequest();
            req.Fields = "pid,name,must,multi,prop_values";
            req.Cid = long.Parse(dt.Rows[0]["cid"].ToString());
            req.Type = 2L;
            ItempropsGetResponse response = client.Execute(req);
            string json = JsonConvert.SerializeObject(response);
            JObject jsonObj = JObject.Parse(json);
            if (dt.Rows.Count != 0)
            {
                s.Append("<table id='tabProductInfo'><tr><th>颜色</th><th>商家编码</th><th>缩略图</th><th>数量</th><th>价格</th><th>颜色</th><th>英文名</th><th>季节</th><th>尺寸</th></tr>");
                string Colorlist = GetColorlist(jsonObj);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    image = "";
                    if (dt.Rows[i]["Images"].ToString() != "")
                    {

                        image = "<img src='" + dt.Rows[i]["Images"].ToString() + "' style='width:40px;height:30px' />";
                    }
                    s.Append("<tr><td>" + Colorlist + "</td><td>" + dt.Rows[i]["Scode"] + "</td><td>" + image + "</td><td>" + dt.Rows[i]["Balance"] + "</td><td>" + dt.Rows[i]["Pricea"] + "</td><td>" + dt.Rows[i]["Clolor"] + "</td><td>" + dt.Rows[i]["Descript"] + "</td><td>" + dt.Rows[i]["Cat1"] + "</td><td>" + dt.Rows[i]["Size"] + "</td></tr>");
                    count += Convert.ToInt32(dt.Rows[i]["Balance"]);
                    aveprice += Convert.ToDecimal(dt.Rows[i]["Pricea"]);
                }
                s.Append("</table>");
            }
            aveprice = aveprice / dt.Rows.Count;
            s.Append("-*-" + dt.Rows[0]["TBBrandName"] + "-*-" + count.ToString() + "-*-" + aveprice.ToString() + "-*-" + ShowProperty(jsonObj, dt.Rows[0]["cid"].ToString()));
            return s.ToString();
        }
        /// <summary>
        /// 获取淘宝颜色
        /// </summary>
        public string GetColorlist(JObject jsonObj)
        {
            //string s = "";
            StringBuilder s = new StringBuilder(200);
            s.Append("<select class='selColor'>");
            int a = jsonObj["ItemProps"].Count();
            for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
            {
                if (jsonObj["ItemProps"][i]["Pid"].ToString() == "1627207")
                {
                    for (int j = 0; j < jsonObj["ItemProps"][i]["PropValues"].Count(); j++)
                    {
                        s.Append("<option value='" + jsonObj["ItemProps"][i]["PropValues"][j]["Vid"] + "'>" + jsonObj["ItemProps"][i]["PropValues"][j]["Name"] + "</option>");
                    }
                    break;
                }
            }
            s.Append("</select>");
            return s.ToString();
        }
        ///// <summary>
        ///// 获取淘宝属性以及属性值
        ///// </summary>
        public string ShowProperty(JObject jsonObj, string cid)
        {
            //string s = "";
            StringBuilder s = new StringBuilder(200);
            //ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            //ItempropsGetRequest req = new ItempropsGetRequest();
            //req.Fields = "pid,name,must,multi,prop_values";
            //req.Cid = long.Parse(cid);
            //req.Type = 2L;
            //ItempropsGetResponse response = client.Execute(req);
            //string json = JsonConvert.SerializeObject(response);
            //JObject jsonObj = JObject.Parse(json);
            DataTable dt1 = new DataTable();
            string sql = @"select * from TBProductProperty where parent_cid='" + cid + "'";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            for (int n = 0; n < dt1.Rows.Count; n++)
            {
                for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
                {
                    if (jsonObj["ItemProps"][i]["Pid"].ToString() == "20000" || jsonObj["ItemProps"][i]["Pid"].ToString() == "13021751")
                        continue;

                    if (jsonObj["ItemProps"][i]["Pid"].ToString() == dt1.Rows[n]["vid"].ToString())
                    {
                        s.Append("<div class='divinner'>" + dt1.Rows[n]["TBPropertyName"].ToString() + ":<select class='sel' name='sel" + dt1.Rows[n]["vid"].ToString() + "'><option value=''>请选择</option>");

                        for (int j = 0; j < jsonObj["ItemProps"][i]["PropValues"].Count(); j++)
                        {
                            s.Append("<option value='" + jsonObj["ItemProps"][i]["PropValues"][j]["Vid"] + "'>" + jsonObj["ItemProps"][i]["PropValues"][j]["Name"] + "</option>");
                        }
                        s.Append("</select></div>");
                        break;
                    }
                }
            }
            return s.ToString();
        }

        ///// <summary>
        ///// 获取淘宝上市日期
        ///// </summary>
        //public string GetDatelist(JObject jsonObj)
        //{
        //    string s = "";
        //    int a = jsonObj["ItemProps"].Count();
        //    for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
        //    {
        //        if (jsonObj["ItemProps"][i]["Pid"].ToString() == "8560225")
        //        {
        //            for (int j = 0; j < jsonObj["ItemProps"][i]["PropValues"].Count(); j++)
        //            {
        //                s += "<option value='" + jsonObj["ItemProps"][i]["PropValues"][j]["Vid"] + "'>" + jsonObj["ItemProps"][i]["PropValues"][j]["Name"] + "</option>";
        //            }
        //            break;
        //        }
        //    }

        //    return s;
        //}
        ///// <summary>
        ///// 获取淘宝质地
        ///// </summary>
        //public string GetZhidilist(JObject jsonObj)
        //{
        //    string s = "";
        //    int a = jsonObj["ItemProps"].Count();
        //    for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
        //    {
        //        if (jsonObj["ItemProps"][i]["Pid"].ToString() == "20021")
        //        {
        //            for (int j = 0; j < jsonObj["ItemProps"][i]["PropValues"].Count(); j++)
        //            {
        //                s += "<option value='" + jsonObj["ItemProps"][i]["PropValues"][j]["Vid"] + "'>" + jsonObj["ItemProps"][i]["PropValues"][j]["Name"] + "</option>";
        //            }
        //            break;
        //        }
        //    }

        //    return s;
        //}
        ///// <summary>
        ///// 获取淘宝款式
        ///// </summary>
        //public string GetStylelist(JObject jsonObj)
        //{
        //    string s = "";
        //    int a = jsonObj["ItemProps"].Count();
        //    for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
        //    {
        //        if (jsonObj["ItemProps"][i]["Pid"].ToString() == "122276315")
        //        {
        //            for (int j = 0; j < jsonObj["ItemProps"][i]["PropValues"].Count(); j++)
        //            {
        //                s += "<option value='" + jsonObj["ItemProps"][i]["PropValues"][j]["Vid"] + "'>" + jsonObj["ItemProps"][i]["PropValues"][j]["Name"] + "</option>";
        //            }
        //            break;
        //        }
        //    }

        //    return s;
        //}
        ///// <summary>
        ///// 获取淘宝流行款式
        ///// </summary>
        //public string GetPopStylelist(JObject jsonObj)
        //{
        //    string s = "";
        //    int a = jsonObj["ItemProps"].Count();
        //    for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
        //    {
        //        if (jsonObj["ItemProps"][i]["Pid"].ToString() == "137424339")
        //        {
        //            for (int j = 0; j < jsonObj["ItemProps"][i]["PropValues"].Count(); j++)
        //            {
        //                s += "<option value='" + jsonObj["ItemProps"][i]["PropValues"][j]["Vid"] + "'>" + jsonObj["ItemProps"][i]["PropValues"][j]["Name"] + "</option>";
        //            }
        //            break;
        //        }
        //    }

        //    return s;
        //}
        public string Test1(Int64 cid)
        {

            //req.Title = "test"; //标题
            //req.Price = "1100.00"; //价格
            //req.Num = 30;  //数量
            //req.Cid = long.Parse("121434005");  //叶子类目ID
            ////req.Props = props; //商品属性列表
            //req.LocationCity = "香港"; //城市
            //req.LocationState = "九龙"; //省份
            //req.Type = "fixed"; //发布类型。可选值:fixed(一口价),auction(拍卖)。
            //req.StuffStatus = "new"; //新旧程度。可选值：new(新)，second(二手)，unused(闲置)
            //req.Desc = "这是一个测试的宝贝，所以没有什么好描述的。"; //宝贝描述。
            ////item.InputPids = inputPids; //用户自行输入的类目属性ID串
            //req.InputPids = "13021751";
            ////item.InputStr = inputStr; //用户自行输入的子属性名和属性值
            //req.InputStr = "test";
            //req.ApproveStatus = "instock"; //商品上传后的状态。可选值:onsale(出售中),instock(仓库中);默认值:onsale
            //req.AuctionPoint = 5; //商品的积分返点比例。如:5,表示:返点比例0.5%. 注意：返点比例必须是>0的整数，而且最大是90,即为9%
            //ItemAddResponse response = client.Execute(req, sessionKey);
            ITopClient client = new DefaultTopClient(url, appkey, appsecret);
            ItempropsGetRequest req = new ItempropsGetRequest();
            req.Fields = "pid,name,must,multi,prop_values";
            req.Cid = cid;
            req.Type = 2L;
            ItempropsGetResponse response = client.Execute(req);
            string json = JsonConvert.SerializeObject(response);
            JObject jsonObj = JObject.Parse(json);
            int a = jsonObj["ItemProps"].Count();
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
            {
                s.Append("<div>insert into TBProductProperty(TBPropertyName,vid,parent_cid) values('" + jsonObj["ItemProps"][i]["Name"] + "','" + jsonObj["ItemProps"][i]["Pid"] + "','" + cid.ToString() + "')</div>");
                //DbHelperSQL.ExecuteSql(sql);
            }
            return s.ToString();
        }
        /// <summary>
        /// 显示淘宝类别
        /// </summary>
        public ActionResult TBProductTypeShow()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }
        /// <summary>
        /// 淘宝类别表查询
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="menuId"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string SearchTBType()//string menuId, string Name, string page, string Selpages
        {
            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Name = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}", "");
            string counts = string.Empty;
            DataTable dt = new DataTable();
            dt = attrbll.SearchTBType(Name, pageIndex, pageSize, out counts);
            int pageCount=Convert.ToInt32(counts)%Convert.ToInt32(pageSize)==0?Convert.ToInt32(counts)/Convert.ToInt32(pageSize):Convert.ToInt32(counts)/Convert.ToInt32(pageSize);
            return getDataTBType(dt, menuId) + "-----" + pageCount + "-----" + counts;
        }
        /// <summary>
        /// 显示淘宝类别别表表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataTBType(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            s.Append("<th>淘宝类别</th>");
            s.Append("<th>淘宝编号</th>");
            s.Append("<th>属性</th>");
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                s.Append("<th>编辑</th>");
            }
            if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            {
                s.Append("<th>删除</th>");
            }

            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr><td>" + n + "</td>");
                    s.Append("<td>" + dt.Rows[i]["TBtypeName"] + "</td>");
                    s.Append("<td>" + dt.Rows[i]["cid"] + "</td>");

                    #region 查看属性
                    s.Append("<td>");
                    s.Append("<a href='#' onclick='CheckTBProperty(\"" + dt.Rows[i]["cid"] + "\")'>查看属性</a>");
                    s.Append("</td>");
                    #endregion

                    #region 编辑
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<td><a href='#' onclick='Edit(\"" + dt.Rows[i]["TBtypeName"] + "\",\"" + dt.Rows[i]["cid"] + "\",\"" + dt.Rows[i]["Id"] + "\")'>编辑</a></td>");
                    }
                    #endregion

                    #region 删除
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        s.Append("<td><a href='#' onclick='DeleteTBtype(\"" + dt.Rows[i]["TBtypeName"] + "\",\"" + dt.Rows[i]["cid"] + "\",\"" + dt.Rows[i]["Id"] + "\")'>删除</a></td>");
                    }
                    #endregion

                    s.Append("</tr>");
                }
            }
            #endregion


            return s.ToString();
        }
        /// <summary>
        /// 更新淘宝类别
        /// </summary>
        public string UpdateTBType()
        {
            string TypeName = Request.QueryString["TypeName"];
            string cid = Request.QueryString["cid"];
            string Id = Request.QueryString["Id"];
            return attrbll.UpdateTBType(TypeName, cid, Id, userInfo.User.Id);
        }
        /// <summary>
        /// 删除淘宝类别
        /// </summary>
        public string DeleteTBtype()
        {
            string TBtypeName = Request.QueryString["TBTypeName"];
            string cid = Request.QueryString["cid"];
            string Id = Request.QueryString["Id"];
            string s = "";
            string Updatesql = @"delete from TBProducttype where TBtypeName='" + TBtypeName + "' and cid='" + cid + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(Updatesql)) == -1)
            {
                s = "修改失败!";
            }
            return s;
        }
        /// <summary>
        /// 添加淘宝类别
        /// </summary>
        public string AddTBType()
        {
            string TypeName = Request.QueryString["TypeName"];
            string cid = Request.QueryString["cid"];
            return attrbll.AddTBType(TypeName, cid, userInfo.User.Id);
        }
        /// <summary>
        /// 根据类别编号获得属性
        /// </summary>
        public string GetProperty()
        {
            string cid = Request.QueryString["cid"];
            DataTable dt1 = new DataTable();
            StringBuilder s = new StringBuilder();
            string sql = @"select * from TBProductProperty where parent_cid='" + cid + "'";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            s.Append("<tr><th>编号</th>");
            s.Append("<th style='width:160px'>淘宝属性</th>");
            s.Append("<th style='width:160px'>淘宝编号</th>");
            s.Append("<th>编辑</th>");
            s.Append("<th>删除</th>");
            s.Append("</tr>");
            if (dt1.Rows.Count > 0)
            {

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr><td>" + n + "</td><td class='txtTBPropertyName'><label>" + dt1.Rows[i]["TBPropertyName"] + "</label></td><td class='txtTypevid'><label>" + dt1.Rows[i]["vid"] + "</label></td><td><a href='#' class='Update' title='" + dt1.Rows[i]["Id"] + "' >修改</a></td><td><a href='#' onclick='DeleteTBProperty(\"" + dt1.Rows[i]["Id"] + "\")' >删除</a></td></tr>");
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 更新淘宝属性
        /// </summary>
        public string UpdateTBProperty()
        {
            string PropertyName = Request.QueryString["PropertyName"];
            string vid = Request.QueryString["vid"];
            string Id = Request.QueryString["Id"];
            return attrbll.UpdateTBProperty(PropertyName, vid, Id, userInfo.User.Id);
        }
        /// <summary>
        /// 添加淘宝属性
        /// </summary>
        public string AddTBProperty()
        {
            string TBPropertyName = Request.QueryString["TBPropertyName"];
            string vid = Request.QueryString["vid"];
            string parent_cid = Request.QueryString["parentcid"];
            return attrbll.AddTBProperty(TBPropertyName, vid, parent_cid, userInfo.User.Id);
        }
        /// <summary>
        /// 删除淘宝属性
        /// </summary>
        public string DeleteTBProperty()
        {
            string Id = Request.QueryString["Id"];
            return attrbll.DeleteTBProperty(Id, userInfo.User.Id);
            //string s = "";
            //string Deletesql = @"delete from TBProductProperty where Id='" + Id + "'";
            //if (Convert.ToInt32(DbHelperSQL.GetSingle(Deletesql)) == -1)
            //{
            //    s = "删除失败!";
            //}
            //return s;
        }
        /// <summary>
        /// 供应商类别
        /// </summary>
        public ActionResult VenProducttype()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }


        }
        /// <summary>
        /// 供应商类别搜索
        /// </summary>
        public string VenProducttypeSearch(string lists, string menuId, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("SearchName", SearchInfo[0]);  //--本地类别
                Dic.Add("SearchVenName", SearchInfo[1]); //--供应商类别
                Dic.Add("SearchVencode", SearchInfo[2]);//--供应商
                Dic.Add("bangd", SearchInfo[3]);//--是否绑定
                string counts;
                dt = attrbll.VenProducttypeSearch(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabVenProducttype(dt, menuId) + "-*-" + counts; ;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
        /// <summary>
        /// 供应商类别搜索
        /// </summary>
        public string GetTabVenProducttype(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            //string sql = @"select * from(select cat2 from ItalyPorductStock group by cat2) a left join producttypeVen b on a.Cat2=b.TypeNo";
            //string[] ssName = attrbll.getDataName(sql);
            string[] ssName = { "Cat2", "Vencode2", "BigId", "TypeNo", "TypeName" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            //DataTable dt = DbHelperSQL.Query(strsql).Tables[0];
            PublicHelpController ph = new PublicHelpController();
            //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            string[] ss = { "Cat2", "Vencode2", "BigId", "TypeNo", "TypeName" };
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "Cat2")
                        s.Append("供应商类别名称");
                    if (ssName[z] == "Vencode2")
                        s.Append("供应商");
                    if (ssName[z] == "BigId")
                        s.Append("大类别名称");
                    if (ssName[z] == "TypeNo")
                        s.Append("类别编号");
                    if (ssName[z] == "TypeName")
                        s.Append("类别名称");

                    s.Append("</th>");
                }
            }
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                s.Append("<th>编辑</th>");
            };
            #region 删除
            if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            {
                s.Append("<th>删除</th>");
            }
            #endregion
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='text-align:center; color:red;font-size:12px;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr><td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        if (ss.Contains(ssName[j]))
                        {
                            if (ssName[j] == "BigId")
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i]["bigtypeName"].ToString());
                                s.Append("</td>");
                            }
                            else if (ssName[j] == "Vencode2")
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i]["sourceName"].ToString());
                                s.Append("</td>");
                            }
                            else
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i][ssName[j]].ToString());
                                s.Append("</td>");
                            }
                        }
                    }

                    #region 编辑
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        string itemId = dt.Rows[i]["Id"].ToString() == null || dt.Rows[i]["Id"].ToString() == "" ? "0" : dt.Rows[i]["Id"].ToString();
                        //s.Append("<td><a href='#' onclick='EditAttribute(\"" + dt.Rows[i]["Id"].ToString() + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\",\"" + dt.Rows[i]["TypeNameVen"].ToString() + "\",\"" + dt.Rows[i]["Vencode"].ToString() + "\")'>编辑</a></td>");
                        s.Append("<td><a href='#' onclick='EditAttribute(\"" + itemId + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\",\"" + dt.Rows[i]["Cat2"].ToString() + "\",\"" + dt.Rows[i]["Vencode2"].ToString() + "\")'>编辑</a></td>");
                    }
                    #endregion

                    #region 删除
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        string itemId = dt.Rows[i]["Id"].ToString() == null || dt.Rows[i]["Id"].ToString() == "" ? "0" : dt.Rows[i]["Id"].ToString();
                        //s.Append("<td><a href='#' onclick='DeleteType(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a></td>");
                        if (itemId == "0")
                        {
                            s.Append("<td></td>");
                        }
                        else
                        {
                            s.Append("<td><a href='#' onclick='DeleteType(\"" + itemId + "\")'>删除</a></td>");
                        }

                    }
                    #endregion

                    s.Append("</tr>");
                }
            }
            #endregion
            return s.ToString();
        }
        /// <summary>
        /// 类别搜索
        /// </summary>
        public string SearchProducttype()
        {
            string TypeName = Request.QueryString["TypeName"].ToString();
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            dt = attrbll.SearchProducttype(TypeName);
            //string sql = @"select * from producttype where TypeName like '%" + TypeName + "%'";
            //dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                s.Append("<option value=''>请选择</option>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (TypeName == dt.Rows[i]["TypeName"].ToString())
                    {
                        s.Append("<option value='" + dt.Rows[i]["TypeNo"] + "' selected='selected' >" + dt.Rows[i]["TypeName"] + "</option>");
                    }
                    else
                    {
                        s.Append("<option value='" + dt.Rows[i]["TypeNo"] + "'>" + dt.Rows[i]["TypeName"] + "</option>");
                    }

                }
            }
            return s.ToString();
        }
        ///// <summary>
        ///// 添加供应商类别
        ///// </summary>
        //public string AddProducttypeVen()
        //{

        //    string TypeNameVen = Request.QueryString["TypeNameVen"].ToString();
        //    string Vencode = Request.QueryString["Vencode"].ToString();
        //    string TypeNo = Request.QueryString["TypeNo"].ToString();
        //    string s = string.Empty;
        //    string insertsql = "";
        //    DataTable dt = new DataTable();
        //    string sql = @"select * from producttype where TypeNo='" + TypeNo + "'";
        //    dt = DbHelperSQL.Query(sql).Tables[0];
        //    string checksql = @"select * from producttypeVen where TypeNameVen='" + TypeNameVen + "' and Vencode='" + Vencode + "' and TypeNo='" + TypeNo + "'";
        //    if (DbHelperSQL.Query(checksql).Tables[0].Rows.Count == 0)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            insertsql = @"insert into producttypeVen values('" + dt.Rows[0]["BigId"] + "','" + dt.Rows[0]["TypeName"] + "','" + TypeNo + "','" + TypeNameVen + "','" + Vencode + "','','" + userInfo.User.Id + "','','','','','')";
        //        }
        //        return DbHelperSQL.ExecuteSql(insertsql) > -1 ? "添加成功!" : "添加失败!";
        //    }
        //    else
        //    {
        //        return "该供应商类别已经存在!";
        //    }
        //}
        /// <summary>
        /// 编辑供应商类别
        /// </summary>
        public string UpdateProducttypeVen()
        {
            string Id = Request.QueryString["Id"].ToString();
            string TypeNameVen = Request.QueryString["TypeNameVen"].ToString();
            string Vencode = Request.QueryString["Vencode"].ToString();
            string TypeNo = Request.QueryString["TypeNo"].ToString();
            DataTable dt = new DataTable();
            return attrbll.UpdateProducttypeVen(Id, TypeNameVen, Vencode, TypeNo, userInfo.User.Id);
        }
        /// <summary>
        /// 删除供应商类别
        /// </summary>
        public string deleteProducttypeVen()
        {
            string Id = Request.QueryString["Id"].ToString();
            return attrbll.deleteProducttypeVen(Id, userInfo.User.Id);
        }
        /// <summary>
        /// 获取供应商下拉表
        /// </summary>
        public string GetVencodelist()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            string SourceCode = Request.QueryString["SourceCode"].ToString();
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query(@"select * from productsource").Tables[0];
            s.Append("<option value='' >请选择</option>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (SourceCode == dt1.Rows[i]["SourceCode"].ToString())
                {
                    s.Append("<option value='" + dt1.Rows[i]["SourceCode"] + "' selected='selected' >" + dt1.Rows[i]["sourceName"] + "</option>");
                }
                else
                {
                    s.Append("<option value='" + dt1.Rows[i]["SourceCode"] + "' >" + dt1.Rows[i]["sourceName"] + "</option>");                }

            }
            return s.ToString();

        }
        //****************     7.6
        /// <summary>
        /// 加载显示权限类别
        /// </summary>
        /// <returns></returns>
        public string TypeNo()
        {
            ProductHelper pdh = new ProductHelper();
            List<SelectListItem> drplisttype = new List<SelectListItem>();//类别下拉框
            DataTable listType = pdh.ProductTypeDDlist();
            StringBuilder str = new StringBuilder();
            str.Append("<option value=''>请选择</option>");
            for (int i = 0; i < listType.Rows.Count; i++)
            {
                str.Append("<option value='" + listType.Rows[i]["TypeNo"] + "'>" + listType.Rows[i]["TypeName"] + "</option>");
            }
            return str.ToString();
        }
        /// <summary>
        /// 确认添加对应类别
        /// </summary>
        /// <returns></returns>
        public string InsertProducttypeVenExcel()
        {
            //"typeno": typeno,
            //            "typeName": typeName,
            //            "typeNameVen": typeNameVen,
            string typeno = Request.Form["typeno"] == null ? "" : Request.Form["typeno"].ToString();
            string typename = Request.Form["typeName"] == null ? "" : Request.Form["typeName"].ToString();
            string typenameven = Request.Form["typeNameVen"] == null ? "" : Request.Form["typeNameVen"].ToString();
            string Vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string result = attrbll.InsertProducttypeVenExcel(typeno, typename, typenameven,Vencode);
            return result;
        }
        /// <summary>
        /// 查询类别编号
        /// </summary>
        /// <returns></returns>
        public static int TypeNoPage;
        public string SerachTypeNo()
        {
            string customer = userInfo.User.Id.ToString();
            string[] str = new string[11];
            str[0] = Request.Form["TypeName"] == null ? "" : Request.Form["TypeName"].ToString();
            str[1] = Request.Form["TypeNameVen"] == null ? "" : Request.Form["TypeNameVen"].ToString();
            int page = Request.Form["page"] == null ? 10 : int.Parse(Request.Form["page"].ToString());
            int count = attrbll.GetProductTypeExcelCount(str);
            int PageSum = count % page > 0 ? count / page + 1 : count / page;//页码
            bool IsPage = Request.Form["isPage"] == null ? false : true;//为false 表示加载  为true则表示翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        TypeNoPage = 0;
                        break;
                    case 1:         //上一页
                        if (TypeNoPage - 1 >= 0)
                        {
                            TypeNoPage = TypeNoPage - 1;
                        }
                        else
                        {
                            TypeNoPage = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["PageReturn"] == null ? 0 : int.Parse(Request.Form["PageReturn"].ToString());
                        TypeNoPage = ReturnPage - 1;
                        break;
                    case 3:        //下页
                        if (TypeNoPage + 1 <= PageSum - 1)
                        {
                            TypeNoPage = TypeNoPage + 1;
                        }
                        else
                        {
                            TypeNoPage = PageSum - 1;
                        }
                        break;
                    case 4:        //末页
                        TypeNoPage = PageSum - 1;
                        break;
                }
                #endregion
            }
            else
            {
                TypeNoPage = 0;
            }
            int minid = TypeNoPage * page;
            int maxid = TypeNoPage * page + page;
            DataTable dt = attrbll.GetProductTypeExcel(str, minid.ToString(), maxid.ToString());
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            //int menuId = OrderDetailsSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            //string[] allTableName = usb.getDataName("productstockOrder");
            //string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder Alltable = new StringBuilder();
            Alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'><tr style='text-align:center;'>");

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ToString() != "Id")
                {
                    if (dt.Columns[i].ToString() == "RowId")
                        Alltable.Append("<th>序号</th>");
                    if (dt.Columns[i].ToString() == "TypeName")
                        Alltable.Append("<th>类别名称</th>");
                    if (dt.Columns[i].ToString() == "TypeNo")
                        Alltable.Append("<th>类别编号</th>");
                    if (dt.Columns[i].ToString() == "TypeNameVen")
                        Alltable.Append("<th>供应商名称</th>");
                    if (dt.Columns[i].ToString() == "Vencode")
                        Alltable.Append("<th>来源</th>");
                }

            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            if (dt.Rows.Count<=0||dt==null)
            {
                Alltable.Append("<tr><td colspan='50' style='text-align:center; color:red;font-size:12px;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alltable.Append("<tr>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ToString() != "Id")
                        {
                            Alltable.Append("<td>" + dt.Rows[i][j] + "</td>");
                        }
                    }
                    Alltable.Append("<td><a href='#' onclick='DeleteTypeVen(" + dt.Rows[i]["Id"] + ")'>删除</a></td>");
                    Alltable.Append("</tr>");
                }
            }   
            Alltable.Append("</table>");
            int thispage = 0;
            if (IsPage == true)
            {
                thispage = TypeNoPage + 1;
            }
            else
            {
                thispage = 1;
            }
            return Alltable.ToString() + "❤" + PageSum + "❤" + count + "❤" + thispage;
        }
        /// <summary>
        /// 类别搜索
        /// </summary>
        /// <returns></returns>
        public string TypeNameChange()
        {
            ProductHelper pdh = new ProductHelper();
            string typename = Request.Form["typename"] == null ? "" : Request.Form["typename"].ToString();

            List<SelectListItem> drplisttype = new List<SelectListItem>();//类别下拉框
            DataTable listType = pdh.ProductTypeDDlist(userInfo.User.Id.ToString());
            if (typename != "")
            {
                DataRow[] dr = listType.Select("TypeName LIKE '%" + typename + "%'");
                StringBuilder str = new StringBuilder();
                str.Append("<option value=''>请选择</option>");
                for (int i = 0; i < dr.Length; i++)
                {
                    str.Append("<option value='" + dr[i][1].ToString() + "'>" + dr[i][0].ToString() + "</option>");
                }
                return str.ToString();
            }
            else
            {
                StringBuilder str = new StringBuilder();
                str.Append("<option value=''>请选择</option>");
                for (int i = 0; i < listType.Rows.Count; i++)
                {
                    str.Append("<option value='" + listType.Rows[i]["TypeNo"] + "'>" + listType.Rows[i]["TypeName"] + "</option>");
                }
                return str.ToString();
            }
        }
        /// <summary>
        /// 删除对应的类别
        /// </summary>
        /// <returns></returns>
        public string DeleteTypeExcel()
        {
            string id = Request.Form["Id"] == null ? "" : Request.Form["Id"].ToString();
            return attrbll.DeleteTypeExcel(id);
        }
        /// <summary>
        /// 获取所有供应商下拉列表
        /// </summary>
        /// <returns></returns>
        public string VencodeList()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query(@"select * from productsource").Tables[0];
            s.Append("<option value='' selected='selected' >请选择</option>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s.Append("<option value='" + dt1.Rows[i]["SourceCode"] + "' >" + dt1.Rows[i]["sourceName"] + "</option>");
            }
            return s.ToString();
        }
    }
}
