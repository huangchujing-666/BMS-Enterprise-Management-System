using pbxdata.bll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pbxdata.model;
using Maticsoft.DBUtility;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using System.Net;
using System.Reflection;
namespace pbxdata.web.Controllers
{
    public class ProductStockController : BaseController
    {
        ProductStockBLL psb = new ProductStockBLL();
        ProductHelper phb = new ProductHelper();



        #region   库存管理
        /// <summary>
        /// 首次运行加载的视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowProductStock()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }
            }
            #region 查询
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            #endregion
            ViewData["Vencode"] = phb.GetVencodeDDlist();//供应商
            ViewData["PageSum"] = phb.GetPageDDlist();//页码
            ViewData["Shop"] = phb.GetShopDDlist();//店铺下拉列表
            
            return View();

        }
        /// <summary>
        /// 货号查询
        /// </summary>
        /// <returns></returns>
        public string Search()
        {
            PublicHelpController ph = new PublicHelpController();
            ProductHelper proh = new ProductHelper();
            usersbll usb = new usersbll();

            #region 查询条件


            string orderParams = Request.Form["params"] ?? string.Empty; //参数
           


            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件

            string Scode = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//货号
            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string PriceMin = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小价格
            string PriceMax = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大价格
            string Season = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//季节
            string Brand = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//品牌
            string StockMin = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小库存
            string StockMax = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大库存
            string TimeMin = helpcommon.StrSplit.StrSplitData(orderParamss[8], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小时间
            string TimeMax = helpcommon.StrSplit.StrSplitData(orderParamss[9], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大时间
            string Type = helpcommon.StrSplit.StrSplitData(orderParamss[10], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别
            string Vencode = helpcommon.StrSplit.StrSplitData(orderParamss[11], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//供应商
            string Ddescript = helpcommon.StrSplit.StrSplitData(orderParamss[12], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//描述
            string Imagefiel = helpcommon.StrSplit.StrSplitData(orderParamss[13], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;// 有图/无图

            string Order = helpcommon.StrSplit.StrSplitData(orderParamss[14], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//排序方式  asc升序  desc降序
            string OrderValue = helpcommon.StrSplit.StrSplitData(orderParamss[15], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//排序字段

            Scode = Scode == "\'\'" ? string.Empty : Scode.Trim();
            Style = Style == "\'\'" ? string.Empty : Style.Trim();
            PriceMax = PriceMax == "\'\'" ? string.Empty : PriceMax;
            PriceMin = PriceMin == "\'\'" ? string.Empty : PriceMin;
            Season = Season == "\'\'" ? string.Empty : Season;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            StockMax = StockMax == "\'\'" ? string.Empty : StockMax;
            StockMin = StockMin == "\'\'" ? string.Empty : StockMin;
            TimeMin = TimeMin == "\'\'" ? string.Empty : TimeMin;
            TimeMax = TimeMax == "\'\'" ? string.Empty : TimeMax;
            Type = Type == "\'\'" ? string.Empty : Type;
            Vencode = Vencode == "\'\'" ? string.Empty : Vencode;
            Imagefiel = Imagefiel == "\'\'" ? string.Empty : Imagefiel;
            Ddescript = Ddescript == "\'\'" ? string.Empty : Ddescript;
            Order = Order == "\'\'" ? string.Empty : Order;
            OrderValue = OrderValue == "\'\'" ? string.Empty : OrderValue;

            dic.Add("Scode", Scode);
            dic.Add("Style", Style);
            dic.Add("PriceMax", PriceMax);
            dic.Add("PriceMin", PriceMin);
            dic.Add("Season", Season);
            dic.Add("Brand", Brand);
            dic.Add("StockMax", StockMax);
            dic.Add("StockMin", StockMin);
            dic.Add("TimeMin", TimeMin);
            dic.Add("TimeMax", TimeMax);
            dic.Add("Type", Type);
            dic.Add("Vencode", Vencode);
            dic.Add("Imagefile", Imagefiel);
            dic.Add("Descript", Ddescript);
            dic.Add("ShopId", "");//店铺Id 选货处使用
            dic.Add("isCheckProduct", "");//是否为选货查询 选货处使用
            dic.Add("CustomerId", userInfo.User.Id.ToString());//用户Id 

            dic.Add("orderValue", OrderValue);
            dic.Add("order", Order);
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            //string[] allTableName = usb.getDataName("productstock");//当前列表所有字段

            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段

            bool Storage = ph.isFunPermisson(roleId, menuId, funName.Storage); //入库权限
            bool Marker = ph.isFunPermisson(roleId, menuId, funName.Marker);//商品标记权限（备注）

            int Count = 0;//数据总个数
            psb.SearchShowProductStock(dic, out Count);
            int PageCount = Count % pageSize > 0 ? Count / pageSize + 1 : Count / pageSize;//数据总页数

            int MinId = pageSize;
            int MaxId = pageSize * (pageIndex - 1);

            DataTable dt = psb.SearchShowProductStock(dic, MinId, MaxId);

            StringBuilder Alltable = new StringBuilder();

            #endregion

            #region  表头排序
            s = proh.TableHeader(s, 3, "Cat");
            s = proh.TableHeader(s, 2, "Imagefile");
            s = proh.TableHeader(s, 1, "Scode");
            s = proh.TableHeader(s, 0, "Style");
            #endregion

            #region Table表头
            string by = Order == "desc" ? "↓" : "↑";

            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                #region
                switch (s[i])
                {
                    case "Id":
                        Alltable.Append("<th>编号<lable></th>");
                        break;
                    case "Scode":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"scode\",this)'>货号</th>");
                        break;
                    case "Bcode":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Bcode\",this)'>条码</th>");
                        break;
                    case "Bcode2":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Bcode2\",this)'>条码2</th>");
                        break;
                    case "Descript":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Descript\",this)'>英文描述</th>");
                        break;
                    case "Cdescript":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Cdescript\",this)'>中文描述</th>");
                        break;
                    case "Unit":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Unit\",this)'>单位</th>");
                        break;
                    case "Currency":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Currency\",this)'>货币</th>");
                        break;
                    case "Cat":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Cat\",this)'>品牌</th>");
                        break;
                    case "Cat1":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Cat1\",this)'>季节</th>");
                        break;
                    case "Cat2":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Cat2\",this)'>类别</th>");
                        break;
                    case "Clolor":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Clolor\",this)'>颜色</th>");
                        break;
                    case "Size":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Size\",this)'>尺码</th>");
                        break;
                    case "Style":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Style\",this)'>款号</th>");
                        break;
                    case "Pricea":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Pricea\",this)'>吊牌价</th>");
                        break;
                    case "Priceb":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Priceb\",this)'>零售价</th>");
                        break;
                    case "Pricec":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Pricec\",this)'>VIP价</th>");
                        break;
                    case "Priced":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Priced\",this)'>批发价</th>");
                        break;
                    case "Pricee":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Pricee\",this)'>成本价</th>");
                        break;
                    case "Disca":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Disca\",this)'>折扣1</th>");
                        break;
                    case "Discb":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Discb\",this)'>折扣2</th>");
                        break;
                    case "Discc":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Discc\",this)'>折扣3</th>");
                        break;
                    case "Discd":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Discd\",this)'>折扣4</th>");
                        break;
                    case "Disce":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Disce\",this)'>折扣5</th>");
                        break;
                    case "Vencode":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Vencode\",this)'>供应商</th>");
                        break;
                    case "Model":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Model\",this)'>型号</th>");
                        break;
                    case "Rolevel":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Rolevel\",this)'>预警库存</th>");
                        break;
                    case "Roamt":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Roamt\",this)'>最少订货量</th>");
                        break;
                    case "Stopsales":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Stopsales\",this)'>停售库存</th>");
                        break;
                    case "Loc":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Loc\",this)'>店铺</th>");
                        break;
                    case "Balance":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"balance\",this)'>库存</th>");
                        break;
                    case "Lastgrnd":
                        Alltable.Append("<th title='" + Order + "' onclick='OrderBy(\"Lastgrnd\",this)'>交货日期</th>");
                        break;
                    case "Imagefile":
                        Alltable.Append("<th>图片</th>");
                        break;
                    case "UserId":
                        Alltable.Append("<th>用户编号</th>");
                        break;
                    case "PrevStock":
                        Alltable.Append("<th>上一次库存</th>");
                        break;
                    case "Def2":
                        Alltable.Append("<th>默认2</th>");
                        break;
                    case "Def3":
                        Alltable.Append("<th>默认3</th>");
                        break;
                    case "Def4":
                        Alltable.Append("<th>默认4</th>");
                        break;
                    case "Def5":
                        Alltable.Append("<th>默认5</th>");
                        break;
                    case "Def6":
                        Alltable.Append("<th>默认6</th>");
                        break;
                    case "Def7":
                        Alltable.Append("<th>默认7</th>");
                        break;
                    case "Def8":
                        Alltable.Append("<th>默认8</th>");
                        break;
                    case "Def9":
                        Alltable.Append("<th>默认9</th>");
                        break;
                    case "Def10":
                        Alltable.Append("<th>默认10</th>");
                        break;
                    case "Def11":
                        Alltable.Append("<th>默认11</th>");
                        break;
                }
                #endregion
            }

            //Alltable.Append("<th>标记库存</th>");
            if (Marker && Storage)  //如果有操作权限 就加入操作列
            {
                Alltable.Append("<th>操作</th>");
            }
            Alltable.Append("</tr>");
            #endregion

            #region Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region  内容
                //Alltable.Append("<tr>");
                //Alltable.Append("<td>" + (i + pageSize * (pageIndex - 1) + 1) + "</td>");
                ////int DefCountBalance = psb.GetDefctiveRemarkBalanceCount(dt.Rows[i]["Vencode"].ToString(), dt.Rows[i]["Scode"].ToString());//标记库存
                //for (int j = 0; j < s.Length; j++)
                //{

                //    if (allTableName.Contains(s[j]))
                //    {
                //        if (s[j] == "Imagefile")
                //        {
                //            if (dt.Rows[i]["ImagePath"] != null && dt.Rows[i]["ImagePath"].ToString() != "")
                //            {
                //                Alltable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["ImagePath"] + "' width='50' height='50'/></td>");
                //            }
                //            else
                //            {
                //                Alltable.Append("<td>");
                //                Alltable.Append("");
                //                Alltable.Append("</td>");
                //            }
                //        }
                //        #region  是否次品
                //        //else if (s[j] == "Def2")//是否次品
                //        //{
                //        //    string IsState = dt.Rows[i]["Def2"] == null ? "0" : dt.Rows[i]["Def2"].ToString();
                //        //    if (IsState == "1")
                //        //    {
                //        //        Alltable.Append("<td>");
                //        //        Alltable.Append("是");
                //        //        Alltable.Append("</td>");
                //        //    }
                //        //    else
                //        //    {
                //        //        Alltable.Append("<td>");
                //        //        Alltable.Append("否");
                //        //        Alltable.Append("</td>");
                //        //    }
                //        //}
                //        #endregion
                //        else if (s[j] == "Balance")
                //        {
                //            //int scount = int.Parse(dt.Rows[i]["Balance"].ToString()) - DefCountBalance;
                //            Alltable.Append("<td>");
                //            Alltable.Append(dt.Rows[i]["Balance"].ToString());
                //            Alltable.Append("</td>");
                //        }
                //        else if (s[j] == "Cat")
                //        {
                //            Alltable.Append("<td>");
                //            Alltable.Append(dt.Rows[i]["BrandName"].ToString());
                //            Alltable.Append("</td>");
                //        }
                //        else if (s[j] == "Cat2")
                //        {
                //            Alltable.Append("<td>");
                //            Alltable.Append(dt.Rows[i]["TypeName"].ToString());
                //            Alltable.Append("</td>");
                //        }
                //        else if (s[j] == "Vencode")
                //        {

                //            Alltable.Append("<td>");
                //            Alltable.Append(dt.Rows[i]["sourceName"].ToString());
                //            Alltable.Append("</td>");
                //        }
                //        else
                //        {
                //            Alltable.Append("<td>");
                //            Alltable.Append(dt.Rows[i][s[j]]);
                //            Alltable.Append("</td>");
                //        }


                //    }

                //}
                ////Alltable.Append("<td>" + DefCountBalance.ToString() + "</td>");
                //if (Storage==true && !Marker==true)//如果有操作功能就加入操作权限列
                //{
                //    Alltable.Append("<td>");
                //    if (Storage)
                //    {
                //        Alltable.Append("<a href='#' onclick=storage('" + dt.Rows[i]["Scode"] + "','" + dt.Rows[i]["Vencode"] + "')>入库</a>|||");
                //    }
                //    if (Marker)
                //    {
                //        Alltable.Append("<a href='#' onclick=Marker('" + dt.Rows[i]["Scode"] + "','" + dt.Rows[i]["Vencode"] + "')>标记</a>|||");
                //    }
                //    Alltable.Append("</td>");
                //}

                //Alltable.Append("</tr>");


                #endregion
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + (i + pageSize * (pageIndex - 1) + 1) + "</td>");
                for (int j = 0; j < s.Length; j++)
                {
                    Alltable.Append("<td>");
                    if (s[j] == "Cat")  //判断如果为品牌字段 则取品牌名称
                    {
                        Alltable.Append(dt.Rows[i]["BrandName"].ToString());
                    }
                    else if (s[j] == "Cat2")  //判断如果为类别字段 则取类别名称
                    {
                        Alltable.Append(dt.Rows[i]["TypeName"].ToString());
                    }
                    else if (s[j] == "Vencode")  //判断如果为供应商字段 则取供应商名称
                    {
                        Alltable.Append(dt.Rows[i]["sourceName"].ToString());
                    }
                    else if (s[j] == "Imagefile") //判断如果为图片字段 则取图片路径
                    {
                        if (dt.Rows[i]["ImagePath"] != null && dt.Rows[i]["ImagePath"].ToString() != "")
                        {
                            Alltable.Append("<img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["ImagePath"] + "' width='50' height='50'/>");
                        }
                        else
                        {
                            Alltable.Append("");
                        }
                    }
                    else
                    {
                        Alltable.Append(dt.Rows[i][s[j]].ToString());
                    }
                    Alltable.Append("</td>");
                }
                if (Storage == true && !Marker == true)//如果有操作功能就加入操作权限列
                {
                    Alltable.Append("<td>");
                    if (Storage)
                    {
                        Alltable.Append("<a href='#' onclick=storage('" + dt.Rows[i]["Scode"] + "','" + dt.Rows[i]["Vencode"] + "')>入库</a>|||");
                    }
                    if (Marker)
                    {
                        Alltable.Append("<a href='#' onclick=Marker('" + dt.Rows[i]["Scode"] + "','" + dt.Rows[i]["Vencode"] + "')>标记</a>|||");
                    }
                    Alltable.Append("</td>");
                }

                Alltable.Append("</tr>");
            }
            Alltable.Append("</table>");
            #endregion

            #region 分页
            Alltable.Append("-----");
            Alltable.Append(PageCount + "-----" + Count);
            #endregion

            return Alltable.ToString();
        }
        /// <summary>
        /// 款号查询
        /// </summary>
        /// <returns></returns>
        public ActionResult PageIndexChageByStyle()
        {
            #region 取出条件
            ProductStockBLL psb = new ProductStockBLL();
            string[] str = new string[12];
            string style = Request.Form["style"] == null ? "" : Request.Form["style"].ToString().Trim();
            string price = Request.Form["price"] == null ? "" : Request.Form["price"].ToString().Trim();
            string price1 = Request.Form["price1"] == null ? "" : Request.Form["price1"].ToString().Trim();
            string cat = Request.Form["cat"] == null ? "" : Request.Form["cat"].ToString().Trim();
            string stcok = Request.Form["stcok"] == null ? "" : Request.Form["stcok"].ToString().Trim();
            string stcok1 = Request.Form["stcok1"] == null ? "" : Request.Form["stcok1"].ToString().Trim();
            string cat2 = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString().Trim();
            string Vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString().Trim();
            str[0] = "";
            str[1] = style;
            str[2] = price;
            str[3] = price1;
            str[4] = cat;
            str[5] = stcok;
            str[6] = stcok1;
            str[7] = cat2;
            str[8] = "";
            str[9] = "";
            str[10] = Vencode;
            str[11] = userInfo.User.Id.ToString();
            int Page = Request.Form["Page"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["Page"]);
            #endregion


            int Index = int.Parse(Request.Form["change"].ToString());//分页类型
            int count;
            int ThisPage = int.Parse(Request.Form["retrunPage"].ToString()) - 1;//当前页码
            DataTable dtcount = psb.ProductStcokByStyle(str, out count);
            int PageCount = count % Page > 0 ? count / Page + 1 : count / Page;
            int PageIndex = phb.PageIndex(ThisPage, PageCount, Index);//得到需要的页码
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int MenuId = int.Parse(Request.Form["MenuId"].ToString());//菜单编号
            PublicHelpController ph = new PublicHelpController();
            string[] s = ph.getFiledPermisson(roleId, MenuId, funName.selectName);
            DataTable dt = psb.ProductStcokByStyle(str, PageIndex * Page, PageIndex * Page + Page);
            usersbll usb = new usersbll();


            StringBuilder Alltable = new StringBuilder();
            Alltable.Append("<table id='StcokTable' class='mytableTop' rules='all'><tr>");
            Alltable.Append("<th>序号</th>");
            string[] ig = new string[dt.Columns.Count];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ig[j] = dt.Columns[j].ColumnName;
            }
            for (int j = 0; j < ig.Length; j++)
            {
                #region//判断头
                switch (ig[j])
                {
                    case "Id":
                        Alltable.Append("<th>");
                        Alltable.Append("编号");
                        Alltable.Append("</th>");
                        break;
                    case "Scode":
                        Alltable.Append("<th>");
                        Alltable.Append("货号");
                        Alltable.Append("</th>");
                        break;
                    case "Bcode":
                        Alltable.Append("<th>");
                        Alltable.Append("条码");
                        Alltable.Append("</th>");
                        break;
                    case "Bcode2":
                        Alltable.Append("<th>");
                        Alltable.Append("条码2");
                        Alltable.Append("</th>");
                        break;
                    case "Descript":
                        Alltable.Append("<th>");
                        Alltable.Append("英文描述");
                        Alltable.Append("</th>");
                        break;
                    case "Cdescript":
                        Alltable.Append("<th>");
                        Alltable.Append("中文描述");
                        Alltable.Append("</th>");
                        break;
                    case "Unit":
                        Alltable.Append("<th>");
                        Alltable.Append("单位");
                        Alltable.Append("</th>");
                        break;
                    case "Currency":
                        Alltable.Append("<th>");
                        Alltable.Append("货币");
                        Alltable.Append("</th>");
                        break;
                    case "Cat":
                        Alltable.Append("<th>");
                        Alltable.Append("品牌");
                        Alltable.Append("</th>");
                        break;
                    case "Cat1":
                        Alltable.Append("<th>");
                        Alltable.Append("季节");
                        Alltable.Append("</th>");
                        break;
                    case "Cat2":
                        Alltable.Append("<th>");
                        Alltable.Append("类别");
                        Alltable.Append("</th>");
                        break;
                    case "Clolor":
                        Alltable.Append("<th>");
                        Alltable.Append("颜色");
                        Alltable.Append("</th>");
                        break;
                    case "Size":
                        Alltable.Append("<th>");
                        Alltable.Append("尺码");
                        Alltable.Append("</th>");
                        break;
                    case "Style":
                        Alltable.Append("<th>");
                        Alltable.Append("款号");
                        Alltable.Append("</th>");
                        break;
                    case "Pricea":
                        Alltable.Append("<th>");
                        Alltable.Append("吊牌价");
                        Alltable.Append("</th>");
                        break;
                    case "Priceb":
                        Alltable.Append("<th>");
                        Alltable.Append("零售价");
                        Alltable.Append("</th>");
                        break;
                    case "Pricec":
                        Alltable.Append("<th>");
                        Alltable.Append("VIP价");
                        Alltable.Append("</th>");
                        break;
                    case "Priced":
                        Alltable.Append("<th>");
                        Alltable.Append("批发价");
                        Alltable.Append("</th>");
                        break;
                    case "Pricee":
                        Alltable.Append("<th>");
                        Alltable.Append("成本价");
                        Alltable.Append("</th>");
                        break;
                    case "Disca":
                        Alltable.Append("<th>");
                        Alltable.Append("折扣1");
                        Alltable.Append("</th>");
                        break;
                    case "Discb":
                        Alltable.Append("<th>");
                        Alltable.Append("折扣2");
                        Alltable.Append("</th>");
                        break;
                    case "Discc":
                        Alltable.Append("<th>");
                        Alltable.Append("折扣3");
                        Alltable.Append("<th>");
                        break;
                    case "Discd":
                        Alltable.Append("<th>");
                        Alltable.Append("折扣4");
                        Alltable.Append("</th>");
                        break;
                    case "Disce":
                        Alltable.Append("<th>");
                        Alltable.Append("折扣5");
                        Alltable.Append("</th>");
                        break;
                    case "Vencode":
                        Alltable.Append("<th>");
                        Alltable.Append("供应商");
                        Alltable.Append("</th>");
                        break;
                    case "Model":
                        Alltable.Append("<th>");
                        Alltable.Append("型号");
                        Alltable.Append("</th>");
                        break;
                    case "Rolevel":
                        Alltable.Append("<th>");
                        Alltable.Append("预警库存");
                        Alltable.Append("</th>");
                        break;
                    case "Roamt":
                        Alltable.Append("<th>");
                        Alltable.Append("最少订货量");
                        Alltable.Append("</th>");
                        break;
                    case "Stopsales":
                        Alltable.Append("<th>");
                        Alltable.Append("停售库存");
                        Alltable.Append("</th>");
                        break;
                    case "Loc":
                        Alltable.Append("<th>");
                        Alltable.Append("店铺");
                        Alltable.Append("</th>");
                        break;
                    case "Balance":
                        Alltable.Append("<th>");
                        Alltable.Append("库存");
                        Alltable.Append("</th>");
                        break;
                    case "Lastgrnd":
                        Alltable.Append("<th>");
                        Alltable.Append("交货日期");
                        Alltable.Append("</th>");
                        break;
                    case "Imagefile":
                        Alltable.Append("<th>");
                        Alltable.Append("图片");
                        Alltable.Append("</th>");
                        break;
                    case "UserId":
                        Alltable.Append("<th>");
                        Alltable.Append("用户编号");
                        Alltable.Append("</th>");
                        break;
                    case "PrevStock":
                        Alltable.Append("<th>");
                        Alltable.Append("上一次库存");
                        Alltable.Append("</th>");
                        break;
                    case "Def2":
                        Alltable.Append("<th>");
                        Alltable.Append("默认2");
                        Alltable.Append("</th>");
                        break;
                    case "Def3":
                        Alltable.Append("<th>");
                        Alltable.Append("默认3");
                        Alltable.Append("</th>");
                        break;
                    case "Def4":
                        Alltable.Append("<th>");
                        Alltable.Append("默认4");
                        Alltable.Append("</th>");
                        break;
                    case "Def5":
                        Alltable.Append("<th>");
                        Alltable.Append("默认5");
                        Alltable.Append("</th>");
                        break;
                    case "Def6":
                        Alltable.Append("<th>");
                        Alltable.Append("默认6");
                        Alltable.Append("</th>");
                        break;
                    case "Def7":
                        Alltable.Append("<th>");
                        Alltable.Append("默认7");
                        Alltable.Append("</th>");
                        break;
                    case "Def8":
                        Alltable.Append("<th>");
                        Alltable.Append("默认8");
                        Alltable.Append("</th>");
                        break;
                    case "Def9":
                        Alltable.Append("<th>");
                        Alltable.Append("默认9");
                        Alltable.Append("</th>");
                        break;
                    case "Def10":
                        Alltable.Append("<th>");
                        Alltable.Append("默认10");
                        Alltable.Append("</th>");
                        break;
                    case "Def11":
                        Alltable.Append("<th>");
                        Alltable.Append("默认11");
                        Alltable.Append("</th>");
                        break;
                }


                #endregion
            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #region 判断内容
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                Alltable.Append("<tr>");
                for (int j = 0; j < ig.Length; j++)
                {
                    Alltable.Append("<td>");
                    Alltable.Append(dt.Rows[k][ig[j]]);
                    Alltable.Append("</td>");
                }
                Alltable.Append("<td><a href='#'onclick='LookOver(\"" + dt.Rows[k]["Style"] + "\")'>查看商品</a></td>");
                Alltable.Append("</tr>");
            }
            #endregion
            Alltable.Append("</table>");
            int page = PageIndex + 1;
            if (page >= PageCount)
            {
                page = PageCount;
            }
            return Json(Alltable.ToString() + "❤" + PageCount + "❤" + count + "❤" + page);
        }
        /// <summary>
        /// Excel操作 --Scode
        /// </summary>
        /// <returns></returns>
        public string Execl()
        {
            #region  导出excel文件
            #region    取值
            ProductStockBLL psb = new ProductStockBLL();
            string[] str = new string[14];
            str[0] = Request.Form["scode"].ToString().Trim();
            str[1] = Request.Form["style"].ToString().Trim();
            str[2] = Request.Form["price"].ToString().Trim();
            str[3] = Request.Form["price1"].ToString().Trim();
            str[4] = Request.Form["cat"].ToString().Trim();//季节
            str[5] = Request.Form["stcok"].ToString().Trim();
            str[6] = Request.Form["stcok1"].ToString().Trim();
            str[7] = Request.Form["brand"].ToString().Trim(); //品牌
            str[8] = Request.Form["time"].ToString().Trim();
            str[9] = Request.Form["time1"].ToString().Trim();
            str[10] = Request.Form["CatLb"].ToString().Trim();//类别
            str[11] = Request.Form["Vencode"].ToString().Trim();//供应商//供应商
            str[12] = Request.Form["orderValue"].ToString().Trim();
            str[13] = Request.Form["order"].ToString().Trim();
            #endregion


            PublicHelpController ph = new PublicHelpController();
            DataTable dt = psb.Excel(str);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + dt.Rows.Count + "件";
            fileName += ".xls";


            #region 头信息 表头内容
            List<pbxdata.web.Controllers.ProductHelper.KeyValues> list = new List<pbxdata.web.Controllers.ProductHelper.KeyValues>() 
            {
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="BrandName",Value="品牌"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="TypeName",Value="类别"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Style",Value="款号"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Cdescript",Value="中文名称"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Descript",Value="英文名称"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Model",Value="型号"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Scode",Value="货号"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Bcode",Value="条形码"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Pricea",Value="吊牌价"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Priceb",Value="零售价"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Pricec",Value="VIP价"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Priced",Value="批发价"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Pricee",Value="成本价"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="sourceName",Value="供应商"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Cat1",Value="季节"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Clolor",Value="颜色"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Size",Value="尺寸"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="ciqAssemCountry",Value="产地"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Balance",Value="库存"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="ciqSpec",Value="规格型号"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="ciqHSNo",Value="HS编码"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Def15",Value="毛重"},
                new pbxdata.web.Controllers.ProductHelper.KeyValues(){ Key="Def15",Value="净重"},
            };
            #endregion
            try
            {
                //string savaPath = "~/Uploadxls/T1_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //Server.MapPath(savaPath);
                dataTableToCsv(dt, Server.MapPath("~/Uploadxls/" + fileName + ""), list);
                string UploadPath = "http://" + Request.Url.Authority + "/" + ("~/Uploadxls/" + fileName + "").Replace("~/", "");
                return UploadPath;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
                throw;
            }
            #endregion
        }
        /// <summary> 
        /// Excel文档 --Scode
        /// </summary>
        /// <param name="table"></param>
        /// <param name="file"></param>
        private void dataTableToCsv(DataTable table, string file, List<pbxdata.web.Controllers.ProductHelper.KeyValues> List)
        {
            string title = "";
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.UTF8);
            for (int i = 0; i < List.Count; i++)
            {
                title += List[i].Value + "\t";
            }
            string[] strColumName = new string[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                strColumName[i] = table.Columns[i].ColumnName;
            }
            title = title.Substring(0, title.Length - 1) + "\n";
            sw.Write(title);
            for (int j = 0; j < table.Rows.Count; j++)
            {
                string line = "";
                for (int i = 0; i < List.Count; i++)
                {
                    if (strColumName.Contains(List[i].Key))
                    {
                        line += "" + table.Rows[j][List[i].Key].ToString().Trim() + "\t";
                    }
                    else
                    {
                        line += "\t";
                    }
                }
                line = line.Substring(0, line.Length - 1) + "\n";
                sw.Write(line);
            }
            sw.Close();
            fs.Close();
            //dataTableToCsv(dt, @"c:\1.xls"); //调用函数

            //System.Diagnostics.Process.Start(@"c:\1.xls");  //打开excel文件
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ExcelStyle() 
        {
            string [] str=new string[9];
            str[0] = Request.Form["Style"] == null ? "" : Request.Form["Style"].ToString();
            str[1] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();
            str[2] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();
            str[3] = Request.Form["PriceMinBig"] == null ? "" : Request.Form["PriceMinBig"].ToString();
            str[4] = Request.Form["PriceMaxBig"] == null ? "" : Request.Form["PriceMaxBig"].ToString();
            str[5] = Request.Form["StcokMinBig"] == null ? "" : Request.Form["StcokMinBig"].ToString();
            str[6] = Request.Form["StcokMaxBig"] == null ? "" : Request.Form["StcokMaxBig"].ToString();
            str[7] = ""; //Request.Form[""] == null ? "" : Request.Form[""].ToString();
            str[8] = userInfo.User.Id.ToString() ;
            DataTable dt = psb.OutPutExcelStyle(str);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss-") + dt.Rows.Count + "款";
            fileName += ".xls";

            List<pbxdata.web.Controllers.ProductHelper.KeyValues> list = new List<ProductHelper.KeyValues>() 
            {
                new ProductHelper.KeyValues{ Key="Style", Value="款号"},
                new ProductHelper.KeyValues{ Key="Pricea", Value="吊牌价"},
                new ProductHelper.KeyValues{ Key="Cat1", Value="季节"},
                new ProductHelper.KeyValues{ Key="Cat", Value="品牌"},
                new ProductHelper.KeyValues{ Key="Cat2", Value="类别"},
                new ProductHelper.KeyValues{ Key="Balance", Value="总库存"},
            };
            try
            {
                dataTableToCsv(dt, Server.MapPath("~/Uploadxls/" + fileName + ""), list);
                string UploadPath = "http://" + Request.Url.Authority + "/" + ("~/Uploadxls/" + fileName + "").Replace("~/", "");
                return UploadPath;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
                throw;
            }
        }


        #region 入库界面
        /// <summary>
        /// 打开入库界面
        /// </summary>
        /// <returns></returns>
        public static string SaveScode;
        public static string Savevencode;
        public static int countBalance;//总库存
        public string PageShowRk()
        {
            string Scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string venName = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            SaveScode = Scode;
            Savevencode = venName;
            int sum = spb.BalanceByScode(SaveScode);//当前货号已分配的库存  
            string count = psb.CountByScode(Scode, venName).ToString();//当前货号的总库存
            int SurplusBalance = int.Parse(count.ToString()) - sum;//剩余库存
            countBalance = int.Parse(count.ToString());
            DataTable dt = spb.BalanceAndShopShow(0, 10, Savevencode, SaveScode);
            int countShopBalance = spb.BalanceAndShopShowCount(Savevencode, SaveScode);//共多少条记录
            int PageSum = countShopBalance % 10 == 0 ? countShopBalance / 10 : countShopBalance / 10 + 1;//共多少页
            StringBuilder alltable = new StringBuilder();
            alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'>");
            alltable.Append("<tr style='text-align:center;'>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName != "Loc")
                {
                    alltable.Append("<th>");
                    if (dt.Columns[i].ColumnName == "Id")
                        alltable.Append("序号");
                    if (dt.Columns[i].ColumnName == "ShopName")
                        alltable.Append("店铺名称");
                    if (dt.Columns[i].ColumnName == "Balance")
                        alltable.Append("库存");
                    if (dt.Columns[i].ColumnName == "userRealName")
                        alltable.Append("操作人");
                    if (dt.Columns[i].ColumnName == "Def6")
                        alltable.Append("操作时间");
                    if (dt.Columns[i].ColumnName == "Scode")
                        alltable.Append("货号");
                    alltable.Append("</th>");
                }
            }
            alltable.Append("<th>操作</th>");
            alltable.Append("</tr>");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    alltable.Append("<tr>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName != "Loc")
                        {
                            alltable.Append("<td>");
                            alltable.Append(dt.Rows[i][j]);
                            alltable.Append("</td>");
                        }
                    }
                    alltable.Append("<td><a href='#' onclick='UpdateShopBalance(\"" + dt.Rows[i]["Scode"] + "\",\"" + dt.Rows[i]["Loc"] + "\",\"" + dt.Rows[i]["ShopName"] + "\",\"" + dt.Rows[i]["Balance"] + "\")'>修改库存</a></td>");
                    alltable.Append("</tr>");

                }
            }
            alltable.Append("</table>");
            return Scode + "❤" + venName + "❤" + count + "❤" + alltable + "❤" + countShopBalance + "❤" + PageSum + "❤" + SurplusBalance;
        }
        /// <summary>
        /// 确认分配库存
        /// </summary>
        /// <returns></returns>
        ShopProductBll spb = new ShopProductBll();
        ProductTypeBLL ptb = new ProductTypeBLL();
        public string SaveBalance()
        {
            int balance = Request.Form["Balance"] == null ? 0 : int.Parse(Request.Form["Balance"].ToString());//得到库存
            string shopid = Request.Form["ShopId"] == null ? "" : Request.Form["ShopId"].ToString();//得到店铺
            int sum = spb.BalanceByScode(SaveScode);
            if (sum + balance > countBalance)//如果当前分配库存加上已分配库存大于总库存 则不能分配
            {
                return "当前货物可分配库存不足" + balance + "件";
            }
            else    //否则进行分配
            {
                if (spb.DataByScodeAndShopId(SaveScode, shopid))//查询当前货号  在当前店铺是否已经分配库存
                {
                    return spb.UpdateBalanceByShopidAndScode(SaveScode, shopid, balance.ToString(), DateTime.Now.ToString(), Savevencode);//已分配则修改
                }
                else //为分配库存则添加
                {
                    DataTable dt = psb.SelectDataByScodeAndVencode(Savevencode, SaveScode);
                    dt.Columns.Add("UpdateDatetime");
                    dt.Columns.Add("Balance1");
                    dt.Columns.Add("UserId");
                    dt.Columns.Add("ShopId");
                    dt.Columns.Add("SaleState");
                    dt.Rows[0]["UpdateDatetime"] = DateTime.Now.ToString();
                    string UserId = userInfo.User.Id.ToString();//当前角色的Id
                    dt.Rows[0]["UserId"] = int.Parse(UserId);
                    dt.Rows[0]["Balance1"] = balance.ToString();
                    dt.Rows[0]["ShopId"] = shopid;
                    dt.Rows[0]["SaleState"] = 0;//销售状态  默认为 不开放销售
                    if (spb.InsertBalance(dt))
                    {
                        return "分配成功！";
                    }
                    else
                    {
                        return "分配错误";
                    }
                }
            }
        }
        /// <summary>
        /// 页面显示列表  分页
        /// </summary>
        /// <returns></returns>
        public static int page = 0;
        public string PageChageIndexShow()
        {

            int Index = int.Parse(Request.Form["Index"].ToString());
            int countShopBalance = spb.BalanceAndShopShowCount(Savevencode, SaveScode);//共多少条记录
            int PageSum = countShopBalance % 10 == 0 ? countShopBalance / 10 : countShopBalance / 10 + 1;//共多少页
            switch (Index)
            {
                case 0:          //首页
                    page = 0;
                    break;
                case 1:          //上页 
                    if (page - 1 >= 0)
                    {
                        page = page - 1;
                    }
                    else
                    {
                        page = 0;
                    }
                    break;
                case 2:          //跳页
                    int tiaopage = int.Parse(Request.Form["PageReturn"].ToString());
                    if (tiaopage > PageSum - 1)
                    {
                        page = PageSum - 1;
                    }
                    else
                    {
                        if (tiaopage == 0)
                        {
                            page = 0;
                        }
                        else
                        {
                            page = tiaopage - 1;
                        }
                    }
                    break;
                case 3:           //下页
                    if (page + 1 <= PageSum - 1)
                    {
                        page = page + 1;
                    }
                    else
                    {
                        page = PageSum - 1;
                    }
                    break;
                case 4:           //末页
                    page = PageSum - 1;
                    break;
            }
            int sum = spb.BalanceByScode(SaveScode);//当前货号已分配的库存  
            string count = psb.CountByScode(SaveScode, Savevencode).ToString();//当前货号的总库存
            int SurplusBalance = int.Parse(count.ToString()) - sum;//剩余库存
            DataTable dt = spb.BalanceAndShopShow(page * 10 + 1, page * 10 + 10, Savevencode, SaveScode);
            StringBuilder alltable = new StringBuilder();
            alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'>");
            alltable.Append("<tr style='text-align:center;'>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName != "Loc")
                {
                    alltable.Append("<th>");
                    if (dt.Columns[i].ColumnName == "Id")
                        alltable.Append("序号");
                    if (dt.Columns[i].ColumnName == "ShopName")
                        alltable.Append("店铺名称");
                    if (dt.Columns[i].ColumnName == "Balance")
                        alltable.Append("库存");
                    if (dt.Columns[i].ColumnName == "userRealName")
                        alltable.Append("操作人");
                    if (dt.Columns[i].ColumnName == "Def6")
                        alltable.Append("操作时间");
                    if (dt.Columns[i].ColumnName == "Scode")
                        alltable.Append("货号");
                    alltable.Append("</th>");
                }
            }
            alltable.Append("<th>操作</th>");
            alltable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                alltable.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName != "Loc")
                    {
                        alltable.Append("<td>");
                        alltable.Append(dt.Rows[i][j]);
                        alltable.Append("</td>");
                    }
                }
                alltable.Append("<td><a href='#' onclick='UpdateShopBalance(\"" + dt.Rows[i]["Scode"] + "\",\"" + dt.Rows[i]["Loc"] + "\",\"" + dt.Rows[i]["ShopName"] + "\",\"" + dt.Rows[i]["Balance"] + "\")'>修改库存</a></td>");
                alltable.Append("</tr>");
            }
            int thisPage = page + 1;
            alltable.Append("</table>");
            return alltable + "❤" + thisPage + "❤" + PageSum + "❤" + countShopBalance + "❤" + SurplusBalance;
        }
        /// <summary>
        /// 修改库存 ----修改个数  不叠加
        /// </summary>
        /// <returns></returns>
        public string UpdateBalanceByShopId()
        {
            string scode = Request.Form["scode"].ToString();
            string shopid = Request.Form["shopid"].ToString();
            string balance = Request.Form["balance"].ToString();
            string ThisBalance = Request.Form["ThisBalance"].ToString();
            int sum = spb.BalanceByScode(scode);
            sum = sum - int.Parse(ThisBalance.ToString());
            if (sum + int.Parse(balance.ToString()) > countBalance)//如果当前分配库存加上已分配库存大于总库存 则不能分配
            {
                return "当前货物可分配库存不足" + balance + "件";
            }
            else
            {
                return spb.UpdateBalanceByShopid(scode, shopid, balance.ToString(), DateTime.Now.ToString(), userInfo.User.Id.ToString(), Savevencode);//修改
            }
        }

        #endregion



        /// <summary>
        /// 标记
        /// </summary>
        /// <returns></returns>
        public string Marker()
        {
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            string venName = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            int Sum = Request.Form["Sum"] == null ? 0 : int.Parse(Request.Form["Sum"].ToString());
            string Remark = Request.Form["Remark"] == null ? "" : Request.Form["Remark"].ToString();
            string MaxScodeMarker = psb.GetMaxScodeMarker(scode);//通过货号得到当前货号的最大标记货号
            int ThisScodeBalance = psb.CountByScode(scode, venName);//根据货号和数据源查询到当前货号的总库存
            int ThisMarkerBalance = psb.GetDefctiveRemarkBalanceCount(venName, scode);//得到当前货号已被标记库存
            MaxScodeMarker = MaxScodeMarker.Substring(MaxScodeMarker.LastIndexOf('_') + 1, MaxScodeMarker.Length - MaxScodeMarker.LastIndexOf('_') - 1);//截取得到最后一次添加的数量int RemarkCount
            int RemarkCount;
            if (MaxScodeMarker != "")
            {
                RemarkCount = int.Parse(MaxScodeMarker);
            }
            else
            {
                RemarkCount = 0;
            }
            string ScodeRemark;
            if (RemarkCount >= 9)
            {
                ScodeRemark = scode + "_" + (RemarkCount + 1).ToString();
            }
            else
            {
                ScodeRemark = scode + "_0" + (RemarkCount + 1).ToString();
            }
            List<model.productstock> list = psb.SelectProduct(scode, venName);//查出当前数据的所有信息
            string result = string.Empty;
            if (ThisScodeBalance - (ThisMarkerBalance + Sum) >= 0)//判断总库存  已标记库存  正在标记库存
            {
                result = psb.InsertDefectiveRemark(list, Remark, ScodeRemark, Sum);
            }
            else
            {
                result = "剩余库存小于被标记库存！";
            }
            return result;
        }


        #endregion



        #region  数据源数据更新
        ProductStockConfigBll pscb = new ProductStockConfigBll();
        /// <summary>
        /// 数据更新
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateData()
        {

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }
            }
            #region 查询
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            return View();
            #endregion
        }
        /// <summary>
        /// 页面第一次显示（源表数据更新） 
        /// </summary>
        /// <returns></returns>
        public string PageFristShow()
        {

            DataTable dt = pscb.SelectAll();
            int MenuId = int.Parse(Request.Form["MenuId"].ToString());//菜单编号
            #region     库存更新
            PublicHelpController ph = new PublicHelpController();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            StringBuilder AllTable = new StringBuilder();
            AllTable.Append("<tr>");
            dt.Columns.Remove("ProcssId");
            dt.Columns.Remove("Def3");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                AllTable.Append("<th>");
                if (dt.Columns[i].ColumnName == "Id")
                    AllTable.Append("编号");
                if (dt.Columns[i].ColumnName == "SourceCode")
                    AllTable.Append("数据源编号");
                if (dt.Columns[i].ColumnName == "DataSources")
                    AllTable.Append("数据源名称");
                if (dt.Columns[i].ColumnName == "TableName")
                    AllTable.Append("表名");
                if (dt.Columns[i].ColumnName == "UpdateState")
                    AllTable.Append("更新状态");
                if (dt.Columns[i].ColumnName == "StartTime")
                    AllTable.Append("开始时间");
                if (dt.Columns[i].ColumnName == "SetTime")
                    AllTable.Append("时间间隔");
                if (dt.Columns[i].ColumnName == "Thread")
                    AllTable.Append("线程个数");
                if (dt.Columns[i].ColumnName == "Def4")
                    AllTable.Append("运行地址");
                AllTable.Append("</th>");
            }
            AllTable.Append("<th>更新时长</th>");
            AllTable.Append("<th>设置线程</th>");
            AllTable.Append("<th>设置间隔</th>");
            AllTable.Append("<th>操作</th>");
            AllTable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int ProcssId = pscb.ScProcssIdBySourcesCode(dt.Rows[i]["SourceCode"].ToString());
                string SoucesCode = dt.Rows[i]["SourceCode"].ToString();
                AllTable.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    AllTable.Append("<td>");
                    AllTable.Append(dt.Rows[i][j]);
                    AllTable.Append("</td>");
                    AllTable.Append("</td>");
                }
                DateTime thistime = DateTime.Now;
                string time = "";
                if (dt.Rows[i]["StartTime"].ToString() == "0")  //如果没有开始时间   那么就没有更新时长
                {
                    time = "0";
                }
                else
                {
                    DateTime starttime = DateTime.Parse(dt.Rows[i]["StartTime"].ToString());
                    time = (thistime - starttime).Minutes.ToString();
                }
                AllTable.Append("<td>" + time + "</td>");
                if (ProcssId != 0)
                {
                    AllTable.Append("<td>设置线程</td>");
                    AllTable.Append("<td>设置</td>");
                }
                else
                {
                    AllTable.Append("<td><a  href='#'  onclick='SetThread(\"" + dt.Rows[i]["SourceCode"] + "\")'>设置线程</a></td>");
                    AllTable.Append("<td><a  href='#'  onclick='SetTimeOut(\"" + dt.Rows[i]["SourceCode"] + "\")'>设置</a></td>");
                }
                if (ph.isFunPermisson(roleId, MenuId, funName.StartUpdate))
                {
                    AllTable.Append("<td>");
                    if (ProcssId != 0)
                    {
                        AllTable.Append("<a href='#'   onclick='StopUpdate(\"" + dt.Rows[i]["SourceCode"] + "\")'>停止运行</a>");
                    }
                    else
                    {
                        AllTable.Append("<a href='#'   onclick='StartUpdate(\"" + dt.Rows[i]["TableName"] + "\",\"" + dt.Rows[i]["SetTime"] + "\",\"" + dt.Rows[i]["SourceCode"] + "\",\"" + dt.Rows[i]["Thread"] + "\")'>启动更新</a>");
                    }
                    AllTable.Append("</td>");
                }
                else
                {
                    AllTable.Append("<td>");
                    AllTable.Append("<a href='#'>无操作权限</a>");
                    AllTable.Append("</td>");
                }
                AllTable.Append("</tr>");
            }
            return AllTable.ToString();
            #endregion
        }
        /// <summary>
        /// 开始更新不同数据源更新库存   
        /// </summary> 
        /// <returns></returns>
        public string StartUpdate()
        {
            string TableName = Request.Form["TableName"].ToString();
            string sourceCode = Request.Form["sourceCode"].ToString();
            string Time = Request.Form["SetTime"].ToString();
            string threadCount = Request.Form["ThreadCount"].ToString();//控制线程个数

            if (sourceCode == "1")
            {
                int ProcesssId = pscb.ScProcssIdBySourcesCode(sourceCode);
                if (ProcesssId == 0)
                {
                    Process pro = new Process();
                    //pro.StartInfo.FileName = @"D:\Project\UpdataSource\ProductStockUpdate\ProductStockUpdate.exe";//服务器
                    pro.StartInfo.FileName = @"F:\project\test\ProductStockUpdate\ProductStockUpdate\bin\Debug\ProductStockUpdate.exe"; //本地
                    pro.StartInfo.Arguments = "true " + TableName + "  " + threadCount + "";
                    pro.StartInfo.UseShellExecute = true;
                    pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    pro.Start();
                    pscb.UpdateStartTime(sourceCode, DateTime.Now.ToString(), pro.Id);
                    return "更新开始";
                }
                else
                {
                    return "程序已经启动";
                }
            }
            else if (sourceCode == "3")
            {
                int ProcesssId = pscb.ScProcssIdBySourcesCode(sourceCode);
                if (ProcesssId == 0)
                {
                    Process pro = new Process();
                    //pro.StartInfo.FileName = @"D:\Project\UpdataSource\ProductStockUpdate\ProductStockUpdate.exe";//服务器
                    pro.StartInfo.FileName = @"D:\更新程序\ItalySourceUpdate\ItalySourceUpdate\bin\Debug\ItalySourceUpdate.exe"; //本地
                    pro.StartInfo.Arguments = "true " + TableName + "  " + threadCount + "";
                    pro.StartInfo.UseShellExecute = true;
                    pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    pro.Start();
                    pscb.UpdateStartTime(sourceCode, DateTime.Now.ToString(), pro.Id);
                    return "更新开始";
                }
                else
                {
                    return "程序已经启动";
                }
            }
            else
            {
                return "启动失败！";
            }

        }
        /// <summary>
        /// 停止更新    ///查询条件需要更改
        /// </summary>
        /// <returns></returns>
        public string StopUpdate()
        {
            string sourceCode = Request.Form["sourceCode"].ToString();
            int ProcessId = pscb.ScProcssIdBySourcesCode(sourceCode);
            Process[] processes = Process.GetProcesses();
            if (ProcessId != 0)
            {
                foreach (Process pr in processes)
                {
                    if (pr.Id == ProcessId)
                    {
                        pr.Kill();
                        pr.WaitForExit();
                        break;
                    }

                }
                pscb.UpdateStateByConfig(sourceCode);
                return "已停止更新";
            }
            else
            {
                return "还未启动程序";
            }

        }
        /// <summary>
        /// 设置更新时长---数据更新
        /// </summary>
        public string SetTimeOut()
        {
            string sourceCode = Request.Form["sourceCode"].ToString();
            string setTime = Request.Form["SetTime"].ToString();
            try
            {
                pscb.UpdateSetTime(setTime, sourceCode);
                return "设置成功";
            }
            catch (Exception)
            {
                return "设置失败";
                throw;
            }
        }
        /// <summary>
        /// 设置更新线程个数
        /// </summary>
        /// <returns></returns>
        public string UpdateThread()
        {
            string SourceCode = Request.Form["SourceCode"].ToString();
            string ThreadCount = Request.Form["ThreadCount"].ToString();
            try
            {
                if (pscb.UpdateThreadStock(ThreadCount, SourceCode))
                {
                    return "设置成功！";
                }
                else
                {
                    return "设置错误";
                }
            }
            catch (Exception)
            {
                return "设置错误";
            }
        }
        #endregion



        #region  商品更新
        /// <summary>
        /// 商品数据更新
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdataShopData()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }

            }
            #region 查询
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            return View();
            #endregion
        }
        /// <summary>
        /// 页面第一次显示（商品数据更新
        /// </summary>
        /// <returns></returns>
        public string PageFristShowShop()
        {
            DataTable dt = pscb.SelectShopUpdateAll();
            PublicHelpController ph = new PublicHelpController();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            DataSet ds = new DataSet();
            StringBuilder AllTable = new StringBuilder();
            AllTable.Append("<tr>");
            AllTable.Append("<th>功能说明</th>");
            AllTable.Append("<th>开始时间</th>");
            AllTable.Append("<th>更新状态</th>");
            AllTable.Append("<th>更新时长(分钟)</th>");
            AllTable.Append("<th>时间间隔（分钟）</th>");
            AllTable.Append("<th>线程个数</th>");
            AllTable.Append("<th>设置间隔</th>");
            AllTable.Append("<th>设置线程</th>");
            AllTable.Append("<th>操作</th>");
            AllTable.Append("</tr>");
            AllTable.Append("<tr>");
            AllTable.Append("<td>商品更新</td>");
            AllTable.Append("<td>" + dt.Rows[0]["StartTime"] + "</td>");
            int ProcessId = pscb.ScProcssIdByDef(dt.Rows[0]["Def3"].ToString());
            if (ProcessId != 0)
            {

                AllTable.Append("<td>正在更新</td>");   //说明正在更新
            }
            else
            {
                AllTable.Append("<td>未启动</td>");

            }
            if (dt.Rows[0]["StartTime"].ToString().Length > 10)
            {
                DateTime thistime = DateTime.Now;
                DateTime satartime = DateTime.Parse(dt.Rows[0]["StartTime"].ToString());
                AllTable.Append("<td>" + (thistime - satartime).Minutes + "</td>");
            }
            else
            {
                AllTable.Append("<td>0</td>");
            }
            AllTable.Append("<td>" + dt.Rows[0]["SetTime"].ToString() + "</td>");

            AllTable.Append("<td>" + dt.Rows[0]["Thread"] + "</td>");
            if (ProcessId != 0)
            {
                AllTable.Append("<td>不可操作</td>");
            }
            else
            {
                AllTable.Append("<td><a  href='#' onclick='SetTimeOut(\"" + dt.Rows[0]["Def3"] + "\")'>设置</a></td>");
            }
            if (ProcessId != 0)
            {
                AllTable.Append("<td>不可操作</td>");
            }
            else
            {
                AllTable.Append("<td><a  href='#' onclick='SetThread(\"" + dt.Rows[0]["Def3"] + "\")'>设置</a></td>");
            }

            if (ProcessId != 0)
            {
                AllTable.Append("<td><a  href='#' onclick='StopShopBalance(\"" + dt.Rows[0]["Def3"] + "\")'>停止运行</a></td>");
            }
            else
            {
                AllTable.Append("<td><a  href='#' onclick='ShopBalance(\"" + dt.Rows[0]["Def3"] + "\",\"" + dt.Rows[0]["Thread"] + "\")'>启动更新</a></td>");
            }
            AllTable.Append("</tr>");
            return AllTable.ToString();
        }
        /// <summary>
        /// 开始更新更新商品库存/
        /// </summary>
        /// <returns></returns>
        public string ShopBalance()
        {
            string Def3 = Request.Form["Def3"].ToString();
            string threadcount = Request.Form["Thread"].ToString();//控制线程个数
            int ProcessId = pscb.ScProcssIdByDef(Def3);
            if (ProcessId == 0)
            {
                Process pro = new Process();
                pro.StartInfo.FileName = @"D:\ShopBalance\ShopBalance\bin\Debug\ShopBalance.exe";  //本地
                //pro.StartInfo.FileName = @"D:\Project\UpdataSource\ProductUpdate\ShopBalance.exe"; //服务器
                pro.StartInfo.Arguments = "true " + threadcount + "";
                pro.StartInfo.UseShellExecute = true;
                pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                pro.Start();
                pscb.UpdateStartTimeShop(Def3, DateTime.Now.ToString(), pro.Id);
            }
            else
            {
                return "程序已启动";
            }
            return "开始执行";
        }
        /// <summary>
        /// 停止更新商品库存
        /// </summary>
        /// <returns></returns>
        public string StopShopBalance()
        {
            string Def3 = Request.Form["Def3"].ToString();
            int ProcessId = pscb.ScProcssIdByDef(Def3);
            Process[] processes = Process.GetProcesses();
            if (ProcessId != 0)
            {
                foreach (Process pr in processes)
                {
                    if (pr.Id == ProcessId || pr.ProcessName == "ShopBalance")
                    {
                        pscb.UpdateStateByConfigShop(Def3);
                        pr.Kill();
                        pr.WaitForExit();
                        break;
                    }

                }
                return "已停止更新";
            }
            else
            {
                pscb.UpdateStateByConfigShop(Def3);
                return "未启动程序";
            }

        }
        /// <summary>
        /// 设置更新时长
        /// </summary>
        public string SetTimeOutShop()
        {
            string Def3 = Request.Form["Id"].ToString();
            string setTime = Request.Form["SetTime"].ToString();
            try
            {
                pscb.UpdateSetTimeShop(setTime, Def3);
                return "设置成功";
            }
            catch (Exception)
            {
                return "设置失败";
                throw;
            }
        }
        /// <summary>
        /// 设置更新线程
        /// </summary>
        /// <returns></returns>
        public string SetThread()
        {
            string Def3 = Request.Form["Def3"].ToString();
            string Thread = Request.Form["Thread"].ToString();
            try
            {
                if (pscb.UpdateThread(Thread, Def3))
                {
                    return "设置成功！";
                }
                else
                {
                    return "设置错误！";
                }
            }
            catch (Exception)
            {
                return "设置错误！";
                throw;
            }
        }
        /// <summary>
        /// 判断当前程序是否在运行
        /// </summary>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        public string ThisProcess(int ProcessId)
        {
            Process[] processes = Process.GetProcesses();
            string Result = "";
            foreach (Process pro in processes)
            {
                if (pro.Id == ProcessId)
                {
                    Result = "正在运行";
                    break;
                }
                else
                {
                    Result = "未启动";
                }
            }
            return Result;
        }

        #endregion



        #region  订单管理

        public static int OrderSaveMenuId;
        /// <summary>
        /// 加载订单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OderGoods()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }

            }
            OrderSaveMenuId = menuId;
            // 查询
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["DrpBrand"] = phb.GetBrandDDlist(userInfo.User.Id);//品牌
            ViewData["PageSum"] = phb.GetPageDDlist();//页码
            return View();
        }
        /// <summary>
        /// 生成订单编号
        /// </summary>
        /// <returns></returns>
        public string OrderNo()
        {
            string OrderId;
            DateTime time = DateTime.Now;
            OrderId = time.ToString("yyyyMMddhhmmss");
            string random = "";
            Random rd = new Random();
            random = rd.Next(10000, 99999).ToString();
            OrderId += random;
            return OrderId;
        }
        /// <summary>
        /// 新增订单
        /// </summary>
        /// <returns></returns>
        public string InsertOrderGoods()
        {
            string[] str = new string[5];
            str[0] = Request.Form["OrderNo"] == null ? "" : Request.Form["OrderNo"].ToString();
            str[1] = Request.Form["OrderUser"] == null ? "" : Request.Form["OrderUser"].ToString();
            str[2] = Request.Form["OrderTime"] == null ? "" : Request.Form["OrderTime"].ToString();
            str[3] = Request.Form["DrpBrand"] == null ? "" : Request.Form["DrpBrand"].ToString();
            str[4] = Request.Form["ExpectTime"] == null ? "" : Request.Form["ExpectTime"].ToString();
            if (psb.InsertOrder(str))
            {
                return "添加成功！";
            }
            else
            {
                return "添加失败！";
            }

        }
        /// <summary>
        /// 显示所有订单
        /// </summary>
        /// <returns></returns>
        public string GetOrderGoods()
        {
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            StringBuilder alltable = new StringBuilder();

            #region 查询条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件
            string OrderId = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //款号
            string Brand = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //款号
            string MinTime = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //款号
            string MaxTime = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //款号


            OrderId = OrderId == "\'\'" ? string.Empty : OrderId;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            MinTime = MinTime == "\'\'" ? string.Empty : MinTime;
            MaxTime = MaxTime == "\'\'" ? string.Empty : MaxTime;


            dic.Add("OrderId", OrderId);
            dic.Add("Brand", Brand);
            dic.Add("MinTime", MinTime);
            dic.Add("MaxTime", MaxTime);

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("OrderGoods");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段

            int Count = psb.GetOrderGoodsCount(dic);//数据总个数
            int pageCount = Count / pageSize;//数据总页码
            pageCount = Count % pageSize > 0 ? pageCount + 1 : pageCount;

            int MinId = (pageIndex - 1) * pageSize;
            int MaxId = (pageIndex - 1) * pageSize + pageSize;
            DataTable dt = psb.GetOrderGoods(dic, MinId, MaxId);

            #endregion

            #region  Table表头
            alltable.Append("<table id='Warehouse' class='mytable' rules='all'><tr>");
            alltable.Append("<th>序号</th>");

            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case "OrderGoodsNo":
                        alltable.Append("<th>订单编号</th>");
                        break;
                    case "OrderUser":
                        alltable.Append("<th>订货人</th>");
                        break;
                    case "Brand":
                        alltable.Append("<th>品牌</th>");
                        break;
                    case "OrderTime":
                        alltable.Append("<th>下单时间</th>");
                        break;
                    case "ImportTime":
                        alltable.Append("<th>导入时间</th>");
                        break;
                    case "ExpectTime":
                        alltable.Append("<th>预期到货日期</th>");
                        break;
                    case "CountScode":
                        alltable.Append("<th>总件数</th>");
                        break;
                    case "CountStyle":
                        alltable.Append("<th>总款数</th>");
                        break;
                    case "Def1":
                        alltable.Append("<th>默认1</th>");
                        break;
                    case "Def2":
                        alltable.Append("<th>默认2</th>");
                        break;
                    case "Def3":
                        alltable.Append("<th>默认3</th>");
                        break;
                    case "Def4":
                        alltable.Append("<th>默认4</th>");
                        break;
                    case "Def5":
                        alltable.Append("<th>默认5</th>");
                        break;
                }
            }
            alltable.Append("<th>操作</th>");
            alltable.Append("</tr>");
            #endregion

            #region  Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                alltable.Append("<tr>");
                alltable.Append("<td>" + dt.Rows[i]["rowid"] + "</td>");
                for (int j = 0; j < s.Length; j++)
                {
                    alltable.Append("<td>");
                    if (s[j] == "Brand")
                    {
                        alltable.Append(dt.Rows[i]["BrandName"].ToString());
                    }
                    else
                    {
                        alltable.Append(dt.Rows[i][s[j]].ToString());
                    }
                    alltable.Append("</td>");
                }
                //alltable.Append("<td><a href='#' onclick='ImportData(\"" + dt.Rows[i]["OrderGoodsNo"] + "\")'>导入货物</a></td>");
                alltable.Append("<td><form action='/ProductStock/UploadExcel?OrderNo=" + dt.Rows[i]["OrderGoodsNo"] + "&Vencode=" + dt.Rows[i]["BrandName"].ToString() + "' data-ajax='true' data-ajax-method='post' enctype='multipart/form-data' id=\"" + dt.Rows[i]["OrderGoodsNo"] + "\" method='post'><input type='file' id='fileName' name='fileName' value='导入订单' class='sty' accept='.xls'><a href='#' onclick='TestSubmit(\"" + dt.Rows[i]["OrderGoodsNo"] + "\")'>导入货物</a></form></td>");
                alltable.Append("</tr>");
            }
            alltable.Append("</table>");
            #endregion

            #region Table分页
            alltable.Append("-----");
            alltable.Append(pageCount + "-----" + Count);
            #endregion

            return alltable.ToString();
        }
        /// <summary>
        /// 上传excel文件到服务器,加入到现货信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UploadExcel()
        {
            string OrderId = Request.Form["OrderNo"] == null ? "" : Request.Form["OrderNo"].ToString();
            string OrderNo = Request.QueryString["OrderNo"] == null ? "" : Request.QueryString["OrderNo"].ToString();
            string Vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string Ven = Request.QueryString["Vencode"] == null ? "" : Request.QueryString["Vencode"].ToString();
            if (Request.Files.Count == 0)
            {
                return "请选择文件！";

            }
            if (!Request.Files[0].FileName.Contains(".xls"))
            {
                return "请选择excel文件！";

            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                return "上传文件失败！";
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase file1 = Request.Files[0];
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                //服务器上的UpLoadFile文件夹必须有读写权限　　　
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss");
                string filePath = @"D:\Files\" + newFile + file1.FileName;
                file.SaveAs(filePath);
                string check = @"select * from excelFilePath where UserId=" + userInfo.User.Id + "";
                string sql = string.Empty;
                if (DbHelperSQL.Query(check).Tables[0].Rows.Count > 0)
                {
                    sql = @"update excelFilePath set FilePath='" + filePath + "' where UserId='" + userInfo.User.Id + "'";
                }
                else
                {
                    sql = @"insert into excelFilePath (FilePath,UserId) values ('" + filePath + "','" + userInfo.User.Id + "')";
                }
                DbHelperSQL.ExecuteSql(sql);
                ExcelOperationbll excel = new ExcelOperationbll();
                Exception ex;
                DataTable dt = excel.GetTablesName(filePath, out ex);
                //得到工作表名
                List<string> list = new List<string>();
                foreach (DataRow r in dt.Rows)
                {
                    list.Add(r["Table_Name"].ToString());
                }
                DataSet ds = excel.GetData(filePath, list[0], out ex);
                DataTable dtTable = ds.Tables[0];
                List<DataRow> dtIsInTable = new List<DataRow>();
                SoCalProductBll scb = new SoCalProductBll();
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    if (psb.IsProductStockOrder(dtTable.Rows[i]["Scode"].ToString(), OrderNo)) //如果已有重复
                    {
                        psb.UpdateProductStockOrder(dtTable.Rows[i]["Scode"].ToString(), dtTable.Rows[i]["Balance"].ToString(), OrderNo, dtTable.Rows[i]["Pricea"].ToString(), dtTable.Rows[i]["Pricee"].ToString());
                        dtIsInTable.Add(dtTable.Rows[i]);
                    }
                }
                foreach (DataRow r in dtIsInTable)
                {
                    dtTable.Rows.Remove(r);
                }
                if (psb.InsertProductStockOrder(dtTable, OrderNo, Ven))
                {
                    string time = DateTime.Now.ToString();
                    DataTable dtOrder = psb.ProductStockOrderStatistics(OrderNo);
                    psb.UpdateOrderGoodsImportTime(OrderNo, time, dtOrder.Rows[0][0].ToString(), dtOrder.Rows[0][1].ToString());
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    return "<script>alert('上传成功！');OrderPage(2);</script>";
                }
                else
                {
                    return "<script>alert('上传失败！');OrderPage(2);</script>";
                }
            }


        }


        #endregion



        #region  订单详情
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        public static int OrderDetailsSaveMenuId;
        public ActionResult OrderDetails()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }

            }
            OrderDetailsSaveMenuId = menuId;
            // 查询
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["PageSum"] = phb.GetPageDDlist();//页码
            return View();
        }
        /// <summary>
        /// 查询所有订单详情
        /// </summary>
        public static int OrderPage = 0;
        public string ProductStockOrder()
        {
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            #region 查询条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件
            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //款号
            string Scode = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //货号
            string PriceMin = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //最小价格
            string PriceMax = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //最大价格
            string Cat1 = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //季节
            string Type = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //类别
            string Brand = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //品牌
            string StockMin = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //最小库存
            string StockMax = helpcommon.StrSplit.StrSplitData(orderParamss[8], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //最大库存
            string OrderId = helpcommon.StrSplit.StrSplitData(orderParamss[9], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty; //最大库存

            Style = Style == "\'\'" ? string.Empty : Style;
            Scode = Scode == "\'\'" ? string.Empty : Scode;
            PriceMax = PriceMax == "\'\'" ? string.Empty : PriceMax;
            PriceMin = PriceMin == "\'\'" ? string.Empty : PriceMin;
            Cat1 = Cat1 == "\'\'" ? string.Empty : Cat1;
            Type = Type == "\'\'" ? string.Empty : Type;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            StockMax = StockMax == "\'\'" ? string.Empty : StockMax;
            StockMin = StockMin == "\'\'" ? string.Empty : StockMin;
            OrderId = OrderId == "\'\'" ? string.Empty : OrderId;


            dic.Add("Style", Style);
            dic.Add("Scode", Scode);
            dic.Add("PriceMax", PriceMax);
            dic.Add("PriceMin", PriceMin);
            dic.Add("Cat1", Cat1);
            dic.Add("Type", Type);
            dic.Add("Brand", Brand);
            dic.Add("StockMax", StockMax);
            dic.Add("StockMin", StockMin);
            dic.Add("OrderId", OrderId);
            #endregion

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段
            string[] allTableName = usb.getDataName("ProductStockOrder");

            int Count = 0;//当前数据总数
            string CustomerId = userInfo.User.Id.ToString();
            Count = psb.ProductStockOrderPageCount(dic, CustomerId);

            int pageCount = Count / pageSize;//数据总页码
            pageCount = Count % pageSize > 0 ? pageCount + 1 : pageCount;
            int MinId = (pageIndex - 1) * pageSize;
            int MaxId = (pageIndex - 1) * pageSize + pageSize;
            dic.Add("MinId", MinId.ToString());
            dic.Add("MaxId", MaxId.ToString());
            DataTable dt = psb.ProductStockOrderPage(dic, CustomerId);//订单详情数据
            dt.Columns.Remove("Id");
            StringBuilder Alltable = new StringBuilder();


            #region  表头排序
            string IdNumber = "";
            IdNumber = s[0];
            for (int n = 0; n < s.Length; n++)   //排序将款号挪到第一列
            {
                if (s[n] == "OrderId")
                {
                    string styleNuber = s[n];
                    s[0] = styleNuber;
                    s[n] = IdNumber;
                }
            }
            string style = "";
            style = s[1];
            for (int n = 0; n < s.Length; n++)   //排序将款号挪到第一列
            {
                if (s[n] == "Style")
                {
                    string styleNuber = s[n];
                    s[1] = styleNuber;
                    s[n] = style;
                }
            }
            string test = "";
            test = s[2];
            for (int n = 0; n < s.Length; n++)   //排序将货号挪到第二列
            {
                if (s[n] == "Scode")
                {
                    string styleNuber = s[n];
                    s[2] = styleNuber;
                    s[n] = test;
                }
            }
            string ModelS = "";
            ModelS = s[3];
            for (int n = 0; n < s.Length; n++)   //排序将货号挪到第二列
            {
                if (s[n] == "Model")
                {
                    string styleNuber = s[n];
                    s[3] = styleNuber;
                    s[n] = ModelS;
                }
            }
            if (s.Contains("Imagefile"))
            {
                string temp = s[4];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Imagefile")
                    {
                        s[i] = temp;
                        s[4] = "Imagefile";
                    }

                }
            }
            if (s.Contains("Cat"))
            {
                string temp = s[5];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Cat")
                    {
                        s[i] = temp;
                        s[4] = "Cat";
                    }

                }
            }
            #endregion

            #region Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                #region//判断头
                switch (s[i])
                {
                    case "OrderId":
                        Alltable.Append("<th>订单编号</th>");
                        break;
                    case "Scode":
                        Alltable.Append("<th>货号</th>");
                        break;
                    case "Bcode":
                        Alltable.Append("<th>条码</th>");
                        break;
                    case "Bcode2":
                        Alltable.Append("<th>条码2</th>");
                        break;
                    case "Descript":
                        Alltable.Append("<th>英文描述</th>");
                        break;
                    case "Cdescript":
                        Alltable.Append("<th>中文描述</th>");
                        break;
                    case "Unit":
                        Alltable.Append("<th>单位</th>");
                        break;
                    case "Currency":
                        Alltable.Append("<th>货币</th>");
                        break;
                    case "Cat":
                        Alltable.Append("<th>品牌</th>");
                        break;
                    case "Cat1":
                        Alltable.Append("<th>季节</th>");
                        break;
                    case "Cat2":
                        Alltable.Append("<th>类别</th>");
                        break;
                    case "Clolor":
                        Alltable.Append("<th>颜色</th>");
                        break;
                    case "Size":
                        Alltable.Append("<th>尺码</th>");
                        break;
                    case "Style":
                        Alltable.Append("<th>款号</th>");
                        break;
                    case "Pricea":
                        Alltable.Append("<th>吊牌价</th>");
                        break;
                    case "Priceb":
                        Alltable.Append("<th>零售价</th>");
                        break;
                    case "Pricec":
                        Alltable.Append("<th>VIP价</th>");
                        break;
                    case "Priced":
                        Alltable.Append("<th>批发价</th>");
                        break;
                    case "Pricee":
                        Alltable.Append("<th>成本价</th>");
                        break;
                    case "Disca":
                        Alltable.Append("<th>折扣1</th>");
                        break;
                    case "Discb":
                        Alltable.Append("<th>折扣2</th>");
                        break;
                    case "Discc":
                        Alltable.Append("<th>折扣3</th>");
                        break;
                    case "Discd":
                        Alltable.Append("<th>折扣4</th>");
                        break;
                    case "Disce":
                        Alltable.Append("<th>折扣5</th>");
                        break;
                    case "Vencode":
                        Alltable.Append("<th>供应商</th>");
                        break;
                    case "Model":
                        Alltable.Append("<th>型号</th>");
                        break;
                    case "Rolevel":
                        Alltable.Append("<th>预警库存</th>");
                        break;
                    case "Roamt":
                        Alltable.Append("<th>最少订货量</th>");
                        break;
                    case "Stopsales":
                        Alltable.Append("<th>停售库存</th>");
                        break;
                    case "Loc":
                        Alltable.Append("<th>店铺</th>");
                        break;
                    case "Balance":
                        Alltable.Append("<th>库存</th>");
                        break;
                    case "Lastgrnd":
                        Alltable.Append("<th>交货日期</th>");
                        break;
                    case "Imagefile":
                        Alltable.Append("<th>图片</th>");
                        break;
                    case "UserId":
                        Alltable.Append("<th>用户编号</th>");
                        break;
                    case "PrevStock":
                        Alltable.Append("<th>上一次库存</th>");
                        break;
                    case "Def2":
                        Alltable.Append("<th>默认2</th>");
                        break;
                    case "Def3":
                        Alltable.Append("<th>默认3</th>");
                        break;
                    case "Def4":
                        Alltable.Append("<th>默认4</th>");
                        break;
                    case "Def5":
                        Alltable.Append("<th>默认5</th>");
                        break;
                    case "Def6":
                        Alltable.Append("<th>默认6</th>");
                        break;
                    case "Def7":
                        Alltable.Append("<th>默认7</th>");
                        break;
                    case "Def8":
                        Alltable.Append("<th>默认8</th>");
                        break;
                    case "Def9":
                        Alltable.Append("<th>默认9</th>");
                        break;
                    case "Def10":
                        Alltable.Append("<th>默认10</th>");
                        break;
                    case "Def11":
                        Alltable.Append("<th>默认11</th>");
                        break;
                }
                #endregion
            }
            Alltable.Append("</tr>");

            #endregion

            #region Table内容

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alltable.Append("<tr>");
                    Alltable.Append("<td>" + dt.Rows[i]["rowId"] + "</td>");
                    for (int j = 0; j < s.Length; j++)
                    {

                        if (allTableName.Contains(s[j]))
                        {
                            if (s[j] == "Imagefile")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("<img style='height:50px;width:50px;' src='" + dt.Rows[i]["ImagePath"] + "' />");
                                Alltable.Append("</td>");
                            }
                            else if (s[j] == "Cat")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i]["BrandName"]);
                                Alltable.Append("</td>");
                            }
                            else if (s[j] == "Cat2")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i]["TypeName"]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                        }
                    }
                    //Alltable.Append("<td><a href='#' onclick='LookAttr(\"" + dt.Rows[i]["Scode"] + "\")'>查看属性</a></td>");
                    Alltable.Append("</tr>");
                }
            }
            Alltable.Append("</table>");
            #endregion

            #region 分页
            Alltable.Append("-----");
            Alltable.Append(pageCount + "-----" + Count);
            #endregion

            return Alltable.ToString();
        }


        #endregion



        #region  标记商品
        /// <summary>
        /// 标记商品页面    一页两分页
        /// </summary>
        /// <returns></returns>
        public static int DefectiveRemarkMenuId;
        public ActionResult DefectiveRemark()
        {

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }

            }
            DefectiveRemarkMenuId = menuId;
            // 查询
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["PageSum"] = phb.GetPageDDlist();
            return View();
        }
        //保存页码
        public static int DefectiveRemarkPage;
        /// <summary>
        /// 得到标记商品
        /// </summary>
        /// <returns></returns>
        public string GetDefectiveRemark()
        {
            string[] str = new string[5];
            str[0] = Request.Form["ScodeMarker"] == null ? "" : Request.Form["ScodeMarker"].ToString();
            str[1] = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            str[2] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();
            str[3] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();
            str[4] = Request.Form["Style"] == null ? "" : Request.Form["Style"].ToString();
            //每页显示的个数
            int Page = Request.Form["Page"] == null ? 10 : int.Parse(Request.Form["Page"].ToString());
            //是否为分页操作
            bool IsPage = Request.Form["IsPage"] == null ? false : bool.Parse(Request.Form["IsPage"].ToString());
            //判断分页的操作
            int Index = Request.Form["Index"] == null ? 2 : int.Parse(Request.Form["Index"].ToString());
            //得到查询的数据总个数
            int Count = psb.GetDefectiveRemarkCount(str);
            //得到查询数据的页数
            int PageCount = Count % Page > 0 ? Count / Page + 1 : Count / Page;
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        DefectiveRemarkPage = 0;
                        break;
                    case 1:         //上一页
                        if (DefectiveRemarkPage - 1 >= 0)
                        {
                            DefectiveRemarkPage = DefectiveRemarkPage - 1;
                        }
                        else
                        {
                            DefectiveRemarkPage = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["ReturnPage"] == null ? 0 : int.Parse(Request.Form["ReturnPage"].ToString());
                        DefectiveRemarkPage = ReturnPage - 1;
                        if (DefectiveRemarkPage < 0)
                        {
                            DefectiveRemarkPage = 0;
                        }
                        break;
                    case 3:        //下页
                        if (DefectiveRemarkPage + 1 <= PageCount - 1)
                        {
                            DefectiveRemarkPage = DefectiveRemarkPage + 1;
                        }
                        else
                        {
                            DefectiveRemarkPage = PageCount - 1;
                        }
                        break;
                    case 4:        //末页
                        DefectiveRemarkPage = PageCount - 1;
                        break;
                }
                #endregion
            }
            else
            {
                DefectiveRemarkPage = 0;
            }
            //得到查询的数据
            DataTable dt = psb.GetDefectiveRemark(str, DefectiveRemarkPage * Page, DefectiveRemarkPage * Page + Page);
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = DefectiveRemarkMenuId;
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("DefectiveRemark");
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder Alltable = new StringBuilder();
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            #region 表头
            for (int i = 0; i < allTableName.Length; i++)
            {
                if (s.Contains(allTableName[i]))
                {
                    Alltable.Append("<th>");
                    if (allTableName[i] == "ScodeMarKer")
                        Alltable.Append("标记编号");
                    if (allTableName[i] == "Scode")
                        Alltable.Append("货号");
                    if (allTableName[i] == "Bcode")
                        Alltable.Append("条码1");
                    if (allTableName[i] == "Bcode2")
                        Alltable.Append("条码2");
                    if (allTableName[i] == "Descript")
                        Alltable.Append("英文描述");
                    if (allTableName[i] == "Cdescript")
                        Alltable.Append("中文描述");
                    if (allTableName[i] == "Unit")
                        Alltable.Append("单位");
                    if (allTableName[i] == "Currency")
                        Alltable.Append("货币");
                    if (allTableName[i] == "Cat")
                        Alltable.Append("品牌");
                    if (allTableName[i] == "Cat1")
                        Alltable.Append("季节");
                    if (allTableName[i] == "Cat2")
                        Alltable.Append("类别");
                    if (allTableName[i] == "Clolor")
                        Alltable.Append("颜色");
                    if (allTableName[i] == "Size")
                        Alltable.Append("尺寸");
                    if (allTableName[i] == "Style")
                        Alltable.Append("款号");
                    if (allTableName[i] == "Pricea")
                        Alltable.Append("吊牌价");
                    if (allTableName[i] == "Priceb")
                        Alltable.Append("零售价");
                    if (allTableName[i] == "Pricec")
                        Alltable.Append("VIP价");
                    if (allTableName[i] == "Priced")
                        Alltable.Append("批发价");
                    if (allTableName[i] == "Pricee")
                        Alltable.Append("成本价");
                    if (allTableName[i] == "ProductRemark")
                        Alltable.Append("标记备注");
                    if (allTableName[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (allTableName[i] == "Model")
                        Alltable.Append("型号");
                    if (allTableName[i] == "Loc")
                        Alltable.Append("店铺");
                    if (allTableName[i] == "Balance")
                        Alltable.Append("标记库存");
                    if (allTableName[i] == "Lastgrnd")
                        Alltable.Append("标记时间");
                    if (allTableName[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (allTableName[i] == "UserId")
                        Alltable.Append("操作人");
                    if (allTableName[i] == "Def1")
                        Alltable.Append("默认1");
                    if (allTableName[i] == "Def2")
                        Alltable.Append("默认2");
                    if (allTableName[i] == "Def3")
                        Alltable.Append("默认3");
                    if (allTableName[i] == "Def4")
                        Alltable.Append("默认4");
                    if (allTableName[i] == "Def5")
                        Alltable.Append("默认5");
                    if (allTableName[i] == "Def6")
                        Alltable.Append("默认6");
                    if (allTableName[i] == "Def7")
                        Alltable.Append("默认7");
                    if (allTableName[i] == "Def8")
                        Alltable.Append("默认8");
                    if (allTableName[i] == "Def9")
                        Alltable.Append("默认9");
                    if (allTableName[i] == "Def10")
                        Alltable.Append("默认10");
                    if (allTableName[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
            }
            #endregion
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #region 内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["RowId"] + "</td>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {
                        if (allTableName[j] == "Cat")//判断如果为品牌
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["BrandName"] + "</td>");
                        }
                        else if (allTableName[j] == "Cat2")//判断如果为类别
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                        }
                        else if (allTableName[j] == "Imagefile") //判断如果为图片
                        {
                            if (dt.Rows[i]["Imagefile"].ToString() != "" && dt.Rows[i]["Imagefile"] != null)
                            {
                                Alltable.Append("<td><img src='" + dt.Rows[i]["Imagefile"] + "'style='width:50px;height:50px;' /></td>");
                            }
                            else
                            {
                                Alltable.Append("<td></td>");
                            }
                        }
                        else
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i][allTableName[j]]);
                            Alltable.Append("</td>");
                        }
                    }
                }
                Alltable.Append("<td>");
                Alltable.Append("<a href='#' onclick='UpdatePrice(\"" + dt.Rows[i]["ScodeMarKer"] + "\")'>修改价格</a>||||");
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    Alltable.Append("<a href='#' onclick='DeleteScodeMarker(\"" + dt.Rows[i]["ScodeMarKer"] + "\")'>取消标记</a>");
                }
                Alltable.Append("</td>");
                Alltable.Append("</tr>");
            }
            #endregion
            Alltable.Append("</table>");
            int thispage = 0;//当前页码
            if (DefectiveRemarkPage + 1 <= PageCount)
            {
                thispage = DefectiveRemarkPage + 1;
            }
            else
            {
                thispage = DefectiveRemarkPage;
            }
            return Alltable.ToString() + "❤" + Count + "❤" + PageCount + "❤" + thispage.ToString();
        }


        public static int MergeDefectiveRemarkPage;
        /// <summary>
        /// 货号合并
        /// </summary>
        /// <returns></returns>
        public string GetMergeDefectiveRemark()
        {
            string[] str = new string[5];
            str[0] = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            str[1] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();
            str[2] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();
            str[3] = Request.Form["Style"] == null ? "" : Request.Form["Style"].ToString();
            //每页显示的个数
            int Page = Request.Form["Page"] == null ? 10 : int.Parse(Request.Form["Page"].ToString());
            //是否为分页操作
            bool IsPage = Request.Form["IsPage"] == null ? false : bool.Parse(Request.Form["IsPage"].ToString());
            //判断分页的操作
            int Index = Request.Form["Index"] == null ? 2 : int.Parse(Request.Form["Index"].ToString());
            //得到查询的数据总个数
            int Count = psb.GetMergeDefectiveRemarkCount(str);
            //得到查询数据的页数
            int PageCount = Count % Page > 0 ? Count / Page + 1 : Count / Page;
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        MergeDefectiveRemarkPage = 0;
                        break;
                    case 1:         //上一页
                        if (MergeDefectiveRemarkPage - 1 >= 0)
                        {
                            MergeDefectiveRemarkPage = MergeDefectiveRemarkPage - 1;
                        }
                        else
                        {
                            MergeDefectiveRemarkPage = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["ReturnPage"] == null ? 0 : int.Parse(Request.Form["ReturnPage"].ToString());
                        MergeDefectiveRemarkPage = ReturnPage - 1;
                        if (MergeDefectiveRemarkPage < 0)
                        {
                            MergeDefectiveRemarkPage = 0;
                        }
                        break;
                    case 3:        //下页
                        if (MergeDefectiveRemarkPage + 1 <= PageCount - 1)
                        {
                            MergeDefectiveRemarkPage = MergeDefectiveRemarkPage + 1;
                        }
                        else
                        {
                            MergeDefectiveRemarkPage = PageCount - 1;
                        }
                        break;
                    case 4:        //末页
                        MergeDefectiveRemarkPage = PageCount - 1;
                        break;
                }
                #endregion
            }
            else
            {
                MergeDefectiveRemarkPage = 0;
            }
            //得到查询的数据
            DataTable dt = psb.GetMergeDefectiveRemark(str, MergeDefectiveRemarkPage * Page, MergeDefectiveRemarkPage * Page + Page);
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = DefectiveRemarkMenuId;
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("DefectiveRemark");
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.MergeselectName);
            StringBuilder Alltable = new StringBuilder();
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            #region 表头
            for (int i = 0; i < allTableName.Length; i++)
            {
                if (s.Contains(allTableName[i]))
                {
                    Alltable.Append("<th>");
                    if (allTableName[i] == "ScodeMarKer")
                        Alltable.Append("标记编号");
                    if (allTableName[i] == "Scode")
                        Alltable.Append("货号");
                    if (allTableName[i] == "Bcode")
                        Alltable.Append("条码1");
                    if (allTableName[i] == "Bcode2")
                        Alltable.Append("条码2");
                    if (allTableName[i] == "Descript")
                        Alltable.Append("英文描述");
                    if (allTableName[i] == "Cdescript")
                        Alltable.Append("中文描述");
                    if (allTableName[i] == "Unit")
                        Alltable.Append("单位");
                    if (allTableName[i] == "Currency")
                        Alltable.Append("货币");
                    if (allTableName[i] == "Cat")
                        Alltable.Append("品牌");
                    if (allTableName[i] == "Cat1")
                        Alltable.Append("季节");
                    if (allTableName[i] == "Cat2")
                        Alltable.Append("类别");
                    if (allTableName[i] == "Clolor")
                        Alltable.Append("颜色");
                    if (allTableName[i] == "Size")
                        Alltable.Append("尺寸");
                    if (allTableName[i] == "Style")
                        Alltable.Append("款号");
                    if (allTableName[i] == "Pricea")
                        Alltable.Append("吊牌价");
                    if (allTableName[i] == "Priceb")
                        Alltable.Append("零售价");
                    if (allTableName[i] == "Pricec")
                        Alltable.Append("VIP价");
                    if (allTableName[i] == "Priced")
                        Alltable.Append("批发价");
                    if (allTableName[i] == "Pricee")
                        Alltable.Append("成本价");
                    if (allTableName[i] == "ProductRemark")
                        Alltable.Append("标记备注");
                    if (allTableName[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (allTableName[i] == "Model")
                        Alltable.Append("型号");
                    if (allTableName[i] == "Loc")
                        Alltable.Append("店铺");
                    if (allTableName[i] == "Balance")
                        Alltable.Append("标记库存");
                    if (allTableName[i] == "Lastgrnd")
                        Alltable.Append("标记时间");
                    if (allTableName[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (allTableName[i] == "UserId")
                        Alltable.Append("操作人");
                    if (allTableName[i] == "Def1")
                        Alltable.Append("默认1");
                    if (allTableName[i] == "Def2")
                        Alltable.Append("默认2");
                    if (allTableName[i] == "Def3")
                        Alltable.Append("默认3");
                    if (allTableName[i] == "Def4")
                        Alltable.Append("默认4");
                    if (allTableName[i] == "Def5")
                        Alltable.Append("默认5");
                    if (allTableName[i] == "Def6")
                        Alltable.Append("默认6");
                    if (allTableName[i] == "Def7")
                        Alltable.Append("默认7");
                    if (allTableName[i] == "Def8")
                        Alltable.Append("默认8");
                    if (allTableName[i] == "Def9")
                        Alltable.Append("默认9");
                    if (allTableName[i] == "Def10")
                        Alltable.Append("默认10");
                    if (allTableName[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
            }
            #endregion
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #region 内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["rowId"] + "</td>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {
                        if (allTableName[j] == "Cat")//判断如果为品牌
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["BrandName"] + "</td>");
                        }
                        else if (allTableName[j] == "Cat2")//判断如果为类别
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                        }
                        else if (allTableName[j] == "Imagefile") //判断如果为图片
                        {
                            if (dt.Rows[i]["Imagefile"].ToString() != "" && dt.Rows[i]["Imagefile"] != null)
                            {
                                Alltable.Append("<td><img src='" + dt.Rows[i]["Imagefile"] + "'style='width:50px;height:50px;' /></td>");
                            }
                            else
                            {
                                Alltable.Append("<td></td>");
                            }
                        }
                        else if (allTableName[j] == "Balance")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["balance2"]);
                            Alltable.Append("</td>");
                        }
                        else
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i][allTableName[j]]);
                            Alltable.Append("</td>");
                        }
                    }
                }
                Alltable.Append("<td><a href='#' onclick='Look(\"" + dt.Rows[i]["Scode"] + "\")'>查看</a></td>");
                Alltable.Append("</tr>");
            }
            #endregion
            Alltable.Append("</table>");
            int thispage = 0;//当前页码
            if (MergeDefectiveRemarkPage + 1 <= PageCount)
            {
                thispage = MergeDefectiveRemarkPage + 1;
            }
            else
            {
                thispage = MergeDefectiveRemarkPage;
            }
            return Alltable.ToString() + "❤" + Count + "❤" + PageCount + "❤" + thispage.ToString();
        }
        /// <summary>
        /// 打开修改价格界面
        /// </summary>
        /// <returns></returns>
        public string GetDefectiveRemarkByScodeMarker()
        {
            string scodemarker = Request.Form["scodemarker"] == null ? "" : Request.Form["scodemarker"].ToString();
            List<model.DefectiveRemark> list = psb.GetDefectiveRemarkByScodeMarker(scodemarker);
            return list[0].Pricea + "❤" + list[0].Priceb + "❤" + list[0].Pricec + "❤" + list[0].Priced + "❤" + list[0].Pricee + "❤" + list[0].ProductRemark + "❤" + list[0].Scode;
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <returns></returns>
        public string UpdatePrice()
        {
            decimal[] str = new decimal[4];
            str[0] = Request.Form["pricea"] == null ? 0 : decimal.Parse(Request.Form["pricea"].ToString());
            str[1] = Request.Form["priceb"] == null ? 0 : decimal.Parse(Request.Form["priceb"].ToString());
            str[2] = Request.Form["pricec"] == null ? 0 : decimal.Parse(Request.Form["pricec"].ToString());
            str[3] = Request.Form["priced"] == null ? 0 : decimal.Parse(Request.Form["priced"].ToString());
            string scodemarker = Request.Form["scodeMarker"] == null ? "" : Request.Form["scodeMarker"].ToString();
            return psb.UpdatePrice(str, scodemarker);
        }
        /// <summary>
        /// 取消标记
        /// </summary>
        /// <returns></returns>
        public string DeleteScodeMarker()
        {
            string ScodeMarker = Request.Form["ScodeMarker"] == null ? "" : Request.Form["ScodeMarker"].ToString();
            return psb.DeleteScodeMarker(ScodeMarker);
        }
        #endregion



        #region  类别 品牌下拉列表

        public string Brand() 
        {
            string sql = Request.Form["sql"].ToString();
            string data = Request.Form["value"].ToString();
            string tsql;
            if (sql == "1")
            {
                tsql = "select BrandName,BrandAbridge from Brand";
            }
            else 
            {
                tsql = "select TOP 10 BrandName,BrandAbridge from Brand where BrandName like '%" + data + "%'";
            }
            return phb.GetDropList(tsql); 
        }

        public string Type() 
        {
            string sql = Request.Form["sql"].ToString();
            string data = Request.Form["value"].ToString();
            string tsql;
            if (sql == "1")
            {
                tsql = "select TypeNo,TypeName from producttype";
            }
            else 
            {
                tsql = "select top 10 TypeNo,TypeName from producttype where TypeName like '%" + data + "%'";
            }
            return phb.GetDropList1(tsql);
        }

        #endregion


    }
}