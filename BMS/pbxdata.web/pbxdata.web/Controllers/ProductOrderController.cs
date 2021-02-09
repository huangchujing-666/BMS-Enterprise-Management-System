
using pbxdata.bll;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class ProductOrderController : Controller
    {
        //d
        // GET: /ProductOrder/

        public ActionResult Index()
        {
            
            return View();
        }
        /// <summary>
        ///  显示所有订单
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductOrderShow()
        {
            #region
            //model.apiOrderModel api = new model.apiOrderModel()
            //{
            //    orderId = "3215464",
            //    deliveryPrice = 132,
            //    buyNameAddress = "深圳",
            //    cityId = "罗湖",
            //    createTime = DateTime.Now,
            //    district = "国贸"
            //    ,
            //    favorablePrice = 231,
            //    invoiceTitle = "dfda",
            //    invoiceType = "jdfakd",
            //    isPay = 0,
            //    itemPrice = 34233,
            //    orderMsg = "发的噶",
            //    orderPrice = 23423,
            //    orderStatus = 1,
            //    paidPrice = 2342,
            //    payOuterId = "11000"
            //    ,
            //    payTime = DateTime.Now,
            //    phone = "12345666660",
            //    postcode = "444444",
            //    provinceId = "32",
            //    realName = "张三",
            //    taxPrice = 2342,
            //    apiOrderDetails = new model.apiOrderDetailsModel[] { new model.apiOrderDetailsModel() { orderId = "3215464", detailsColor="Red", detailsDeliveryPrice="123", detailsEditTime=DateTime.Now,
            //    detailsImg="sdfa", detailsItemPrice=12313, detailsOrderId="123123", detailsPayTime=DateTime.Now, detailsSaleCount=1, detailsScode="396382741000F", detailsSendTime=DateTime.Now, detailsStatus=1,
            //    detailsSucessTime=DateTime.Now, detailsTaxPrice=123, detailsTime=DateTime.Now}
            //    },
            //    apiOrderDiscount = new model.apiOrderDiscountModel[] { new model.apiOrderDiscountModel() { orderId = "3215464" } },
            //    apiOrderPayDetails = new model.apiOrderPayDetailsModel[] { new model.apiOrderPayDetailsModel() { orderId = "3215464", payTime = DateTime.Now } }

            //};
            //System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(model.apiOrderModel));
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //ser.WriteObject(ms, api);
            //string jsonString = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            //jsonString = jsonString.Replace("_","");
            //GetPage("http://localhost:10155/api/AddAllOrder?userName='sa'&userPwd='123'", jsonString);
            #endregion
            string[] str = new string[10];//查询条件
            str[0] = Request.Form["beginTime"] == null ? "" : Request.Form["beginTime"];//开始时间
            str[1] = Request.Form["endTime"] == null ? "" : Request.Form["endTime"];//结束时间
            str[2] = Request.Form["yhm"] == null ? "" : Request.Form["yhm"];//用户名
            str[3] = Request.Form["Brand"] == null ? "" : Request.Form["Brand"];//品牌
            str[4] = Request.Form["Type"] == null ? "" : Request.Form["Type"];//类别
            str[5] = Request.Form["state"] == null ? "" : Request.Form["state"];//状态
            str[6] = Request.Form["pricemin"] == null ? "" : Request.Form["pricemin"];//最小价格
            str[7] = Request.Form["pricemax"] == null ? "" : Request.Form["pricemax"];//最大价格
            str[8] = Request.Form["Scode"] == null ? "" : Request.Form["Scode"];//货号
            str[9] = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"];//单号
            bll.ProductOrderbll bll = new bll.ProductOrderbll();
            Exception ex;
            DataTable table = bll.GetProductOrder(0, 10, str, out ex);
            ViewData["pageCount"] = bll.GetProductOrderCount(str); ;
            ViewData["pageRowsCount"] = table != null ? table.Rows.Count : 0;
            if (table != null)
            {
                #region
                foreach (DataColumn c in table.Columns)
                {
                    if (c.ColumnName == "nid") 
                    {
                        c.ColumnName = "编号";
                    }
                    if (c.ColumnName == "orderId")
                    {
                        c.ColumnName = "订单编号";
                    }
                    if (c.ColumnName == "productScode")
                    {
                        c.ColumnName = "货号";
                    }
                    if (c.ColumnName == "CAT") 
                    {
                        c.ColumnName = "品牌";
                    }
                    if (c.ColumnName == "CAT2") 
                    {
                        c.ColumnName = "类别";
                    }
                    if (c.ColumnName == "SIZE")
                    {
                        c.ColumnName = "尺码";
                    }
                    if (c.ColumnName == "Color")
                    {
                        c.ColumnName = "颜色";
                    }
                    if (c.ColumnName == "PRICEE")
                    {
                        c.ColumnName = "批发价";
                    }
                    if (c.ColumnName == "PRICED")
                    {
                        c.ColumnName = "成本价";
                    }
                    if (c.ColumnName == "orderNum")
                    {
                        c.ColumnName = "数量";
                    }
                    if (c.ColumnName == "orderCreateTime")
                    {
                        c.ColumnName = "订单创建时间";
                    }
                    if (c.ColumnName == "UserName")
                    {
                        c.ColumnName = "用户名";
                    }
                    if (c.ColumnName == "reserved")
                    {
                        c.ColumnName = "订单状态";
                    }
                    
                }
                List<SelectListItem> drplist = new List<SelectListItem>();//品牌下拉框
                BrandBLL bb = new BrandBLL();
                List<BrandModel> brm = bb.SelectBrand();
                foreach (var temp in brm)
                {
                    SelectListItem sl = new SelectListItem();
                    sl.Text = temp.BrandName;
                    sl.Value = temp.BrandAbridge;
                    drplist.Add(sl);
                }
                ViewData["DrpBrand"] = drplist;
                ProductTypeBLL ptb = new ProductTypeBLL();
                List<SelectListItem> drplisttype = new List<SelectListItem>();//类别下拉框
                List<ProductTypeModel> listType = ptb.GetProductType();
                foreach (var temp in listType)
                {
                    SelectListItem si = new SelectListItem();
                    si.Text = temp.TypeName;
                    si.Value = temp.TypeNo;
                    drplisttype.Add(si);
                }
                ViewData["DropType"] = drplisttype;
                #endregion
                return View(table);
            }
            else
            {
                return View("../ErrorMsg/Index");
            }
        }
        public string GetPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = System.Text.Encoding.GetEncoding("gb2312");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }
        public ActionResult PageIndexChange()
        {
            string[] str=new string[10];//查询条件
            int ii;
            if (!int.TryParse(Request.Form["pageIndex"], out ii) || !int.TryParse(Request.Form["pgCount"], out ii))
                return Content("");
            int pageIndex = int.Parse(Request.Form["pageIndex"]);
            int pgCount = int.Parse(Request.Form["pgCount"]);
            str[0] = Request.Form["beginTime"] == null ? "" : Request.Form["beginTime"];//开始时间
            if (Request.Form["endTime"] == "")//结束时间
            {
                str[1] = "";
            }
            else 
            {
                str[1] = Request.Form["endTime"] + "   23:59:59";
            }
            str[2] = Request.Form["yhm"] == null ? "" : Request.Form["yhm"];//用户名
            str[3] = Request.Form["Brand"] == null ? "" : Request.Form["Brand"];//品牌
            str[4] = Request.Form["Type"] == null ? "" : Request.Form["Type"];//类别
            str[5] = Request.Form["state"] == null ? "" : Request.Form["state"];//状态
            str[6] = Request.Form["pricemin"] == null ? "" : Request.Form["pricemin"];//最小价格
            str[7] = Request.Form["pricemax"] == null ? "" : Request.Form["pricemax"];//最大价格
            str[8] = Request.Form["Scode"] == null ? "" : Request.Form["Scode"];//货号
            str[9] = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"];//单号
            bll.ProductOrderbll bll = new bll.ProductOrderbll();
            Exception ex;
            int count=bll.GetProductOrderCount(str);
            DataTable table = bll.GetProductOrder(pgCount * (pageIndex - 1), pgCount, str, out ex);
            string html = "";
            foreach (DataRow dr in table.Rows)
            {
                html += "<tr>";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    html += "<td>";
                    if (table.Columns[i].ColumnName == "reserved")
                    {
                        if (dr[i] == null || dr[i].ToString() == "" || dr[i].ToString() != "1")
                        {
                            html += "入单";
                        }
                        else
                            html += "取消";
                    }
                    else
                    {
                        html += dr[i];
                    }
                    html += "</td>";
                }
                html += "</tr>";
            }
            //html += "<script>$('#pageCount').html('" + (count % 10 == 0 ? count / 10 : count / 10 + 1) + "');$('#pageRowsCount').html('" + count + "'); </script>";
            html += "╋" + (count % pgCount == 0 ? count / pgCount : count / pgCount + 1) + "╋" + count;
            return Content(html);
        }

    }
}
