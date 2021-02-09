using pbxdata.bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class OpenProductController : BaseController
    {
        //
        // GET: /OpenProduct/




        public ActionResult ProductOpen()
        {
            ProductStockBLL psb = new ProductStockBLL();
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
            Savemenuid = menuId;
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ProductHelper php = new ProductHelper();
            
            ViewData["DrpBrand"] =php.GetBrandDDlist(userInfo.User.Id);

            ViewData["DropType"] = php.GetTypeDDlist(userInfo.User.Id);
            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["Vencode"] = php.GetVencodeDDlist();
            ViewData["Shop"] = php.GetShopDDlist();
            return View();
        }
        public static int PageSum = 0; //记录查询的页码
        public static int PageSumByStyle = 0;//记录款号查询的页码
        public static int Savemenuid = 0;//记录菜单编号
        public string GetAllProduct()
        {
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();
            ProductStockBLL psb = new ProductStockBLL();
            #region 查询条件

            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件

            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//货号
            string Scode = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string PriceMin = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小价格
            string PriceMax = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大价格
            string Season = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//季节
            string Type = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//品牌
            string Brand = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小库存
            string StockMin = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大库存
            string StockMax = helpcommon.StrSplit.StrSplitData(orderParamss[8], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小时间
            string Descript = helpcommon.StrSplit.StrSplitData(orderParamss[9], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大时间
            string MinTime = helpcommon.StrSplit.StrSplitData(orderParamss[10], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别
            string MaxTime = helpcommon.StrSplit.StrSplitData(orderParamss[11], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//供应商
            string Vencode = helpcommon.StrSplit.StrSplitData(orderParamss[12], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//有图/无图
            string ImageFile = helpcommon.StrSplit.StrSplitData(orderParamss[13], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//有图/无图


            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            PriceMax = PriceMax == "\'\'" ? string.Empty : PriceMax;
            PriceMin = PriceMin == "\'\'" ? string.Empty : PriceMin;
            Season = Season == "\'\'" ? string.Empty : Season;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            StockMax = StockMax == "\'\'" ? string.Empty : StockMax;
            StockMin = StockMin == "\'\'" ? string.Empty : StockMin;
            MinTime = MinTime == "\'\'" ? string.Empty : MinTime;
            MaxTime = MaxTime == "\'\'" ? string.Empty : MaxTime;
            Type = Type == "\'\'" ? string.Empty : Type;
            Vencode = Vencode == "\'\'" ? string.Empty : Vencode;
            ImageFile = ImageFile == "\'\'" ? string.Empty : ImageFile;
            Descript = Descript == "\'\'" ? string.Empty : Descript;

            dic.Add("Scode", Scode);
            dic.Add("Style", Style);
            dic.Add("PriceMax", PriceMax);
            dic.Add("PriceMin", PriceMin);
            dic.Add("Season", Season);
            dic.Add("Brand", Brand);
            dic.Add("StockMax", StockMax);
            dic.Add("StockMin", StockMin);
            dic.Add("TimeMin", MinTime);
            dic.Add("TimeMax", MaxTime);
            dic.Add("Type", Type);
            dic.Add("Vencode", Vencode);
            dic.Add("Imagefile", ImageFile);
            dic.Add("Descript", Descript);
            dic.Add("ShopId", "");//店铺Id 选货处使用
            dic.Add("isCheckProduct", "");//是否为选货查询 选货处使用
            dic.Add("CustomerId", userInfo.User.Id.ToString());//用户Id 

            #endregion
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("productstock");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段
            bool Storage = ph.isFunPermisson(roleId, menuId, funName.Storage); //入库权限
            bool Marker = ph.isFunPermisson(roleId, menuId, funName.Marker);//商品标记权限（备注）

            int Count = 0;//数据总个数
            psb.SearchShowProductStock(dic, out Count);
            int PageCount = Count % pageSize > 0 ? Count / pageSize + 1 : Count / pageSize;//数据总页数

            int MinId = pageSize;
            int MaxId = pageSize * (pageIndex - 1);

            DataTable dt = psb.SearchShowProductStock(dic, MinId, MaxId);
 
            #region  Table表头排序
            string idNuber = "";

            idNuber = allTableName[0];
            for (int i = 0; i < allTableName.Length; i++)   //排序将款号挪到第一列
            {
                if (allTableName[i] == "Style")
                {
                    string styleNuber = allTableName[i];
                    allTableName[0] = styleNuber;
                    allTableName[i] = idNuber;
                }
            }
            string Value = "";
            Value = allTableName[1];
            for (int i = 0; i < allTableName.Length; i++)   //排序将货号挪到第二列
            {
                if (allTableName[i] == "Scode")
                {
                    string temp = allTableName[i];
                    allTableName[1] = Scode;
                    allTableName[i] = Value;
                }
            }
            string Model = "";
            Model = allTableName[2];
            for (int i = 0; i < allTableName.Length; i++)   //排序将型号挪到第三列
            {
                if (allTableName[i] == "Model")
                {
                    string temp = allTableName[i];
                    allTableName[2] = Scode;
                    allTableName[i] = Model;
                }
            }
            string IdNumber = "";
            IdNumber = s[0];
            for (int n = 0; n < s.Length; n++)   //排序将款号挪到第一列
            {
                if (s[n] == "Style")
                {
                    string styleNuber = s[n];
                    s[0] = styleNuber;
                    s[n] = IdNumber;
                }
            }
            string test = "";
            test = s[1];
            for (int n = 0; n < s.Length; n++)   //排序将货号挪到第二列
            {
                if (s[n] == "Scode")
                {
                    string styleNuber = s[n];
                    s[1] = styleNuber;
                    s[n] = test;
                }
            }
            string ModelS = "";
            ModelS = s[2];
            for (int n = 0; n < s.Length; n++)   //排序将货号挪到第二列
            {
                if (s[n] == "Model")
                {
                    string styleNuber = s[n];
                    s[2] = styleNuber;
                    s[n] = ModelS;
                }
            }
            if (s.Contains("Imagefile"))
            {
                string temp = s[3];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Imagefile")
                    {
                        s[i] = temp;
                        s[3] = "Imagefile";
                    }

                }
            }
            #endregion
            StringBuilder Alltable = new StringBuilder();

            #region Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'><tr style='text-align:center;'>");
            Alltable.Append("<td>序号</td>");
            for (int i = 0; i < s.Length; i++)
            {
                if (allTableName.Contains(s[i]))
                {
                    Alltable.Append("<th>");
                    if (s[i] == "Id")
                        Alltable.Append("编号");
                    if (s[i] == "Scode")
                        Alltable.Append("货号");
                    if (s[i] == "Bcode")
                        Alltable.Append("条码1");
                    if (s[i] == "Bcode2")
                        Alltable.Append("条码2");
                    if (s[i] == "Descript")
                        Alltable.Append("英文描述");
                    if (s[i] == "Cdescript")
                        Alltable.Append("中文描述");
                    if (s[i] == "Unit")
                        Alltable.Append("单位");
                    if (s[i] == "Currency")
                        Alltable.Append("货币");
                    if (s[i] == "Cat")
                        Alltable.Append("品牌");
                    if (s[i] == "Cat1")
                        Alltable.Append("季节");
                    if (s[i] == "Cat2")
                        Alltable.Append("类别");
                    if (s[i] == "Clolor")
                        Alltable.Append("颜色");
                    if (s[i] == "Size")
                        Alltable.Append("尺寸");
                    if (s[i] == "Style")
                        Alltable.Append("款号");
                    if (s[i] == "Pricea")
                        Alltable.Append("吊牌价");
                    if (s[i] == "Priceb")
                        Alltable.Append("零售价");
                    if (s[i] == "Pricec")
                        Alltable.Append("VIP价");
                    if (s[i] == "Priced")
                        Alltable.Append("批发价");
                    if (s[i] == "Pricee")
                        Alltable.Append("成本价");
                    if (s[i] == "Disca")
                        Alltable.Append("折扣1");
                    if (s[i] == "Discb")
                        Alltable.Append("折扣2");
                    if (s[i] == "Discc")
                        Alltable.Append("折扣3");
                    if (s[i] == "Discd")
                        Alltable.Append("折扣4");
                    if (s[i] == "Disce")
                        Alltable.Append("折扣5");
                    if (s[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (s[i] == "Model")
                        Alltable.Append("型号");
                    if (s[i] == "Rolevel")
                        Alltable.Append("预警库存");
                    if (s[i] == "Roamt")
                        Alltable.Append("最少订货量");
                    if (s[i] == "Stopsales")
                        Alltable.Append("停售库存");
                    if (s[i] == "Loc")
                        Alltable.Append("店铺");
                    if (s[i] == "Balance")
                        Alltable.Append("供应商库存");
                    if (s[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (s[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (s[i] == "UserId")
                        Alltable.Append("操作人");
                    if (s[i] == "PrevStock")
                        Alltable.Append("上一次库存");
                    if (s[i] == "Def2")
                        Alltable.Append("是否次品");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("是否开放");
                    if (s[i] == "Def5")
                        Alltable.Append("默认5");
                    if (s[i] == "Def6")
                        Alltable.Append("默认6");
                    if (s[i] == "Def7")
                        Alltable.Append("默认7");
                    if (s[i] == "Def8")
                        Alltable.Append("默认8");
                    if (s[i] == "Def9")
                        Alltable.Append("默认9");
                    if (s[i] == "Def10")
                        Alltable.Append("默认10");
                    if (s[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
            }
            Alltable.Append("<td><input type='checkbox' id='CheckAll' onchange='checkall()'/>操作</td>");
            Alltable.Append("</tr>");
            #endregion

            #region Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + (i + pageSize * (pageIndex - 1) + 1) + "</td>");
                int DefCountBalance = psb.GetDefctiveRemarkBalanceCount(psb.SelectVenNameByVencode(dt.Rows[i]["Vencode"].ToString()), dt.Rows[i]["Scode"].ToString());
                for (int j = 0; j < s.Length; j++)
                {

                    if (allTableName.Contains(s[j]))
                    {
                        if (s[j] == "Imagefile")
                        {
                            if (dt.Rows[i]["ImagePath"] != null && dt.Rows[i]["ImagePath"].ToString() != "")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("<img style='height:50px;width:50px;' onerror='errorImg(this)' src='" + dt.Rows[i]["ImagePath"] + "' />");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Def2")
                        {
                            string IsState = dt.Rows[i]["Def2"] == null ? "0" : dt.Rows[i]["Def2"].ToString();
                            if (IsState == "1")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("是");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("否");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Balance")
                        {
                            int scount = int.Parse(dt.Rows[i]["Balance"].ToString()) - DefCountBalance;
                            Alltable.Append("<td>");
                            Alltable.Append(scount);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Def4")
                        {
                            string def4 = dt.Rows[i]["Def4"] == null ? "2" : dt.Rows[i]["Def4"].ToString();
                            if (def4 != "1")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("已开放");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("未开放");
                                Alltable.Append("</td>");
                            }
                        }
                        else
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i][s[j]]);
                            Alltable.Append("</td>");
                        }


                    }
                }
                Alltable.Append("<td>");
                Alltable.Append("<input type='checkbox' class='check' vencode=\"" + dt.Rows[i]["Vencode"].ToString() + "\" title=\"" + dt.Rows[i]["Scode"] + "\"/>");
                Alltable.Append("</td>");
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
        /// 选择开放
        /// </summary>
        /// <returns></returns>
        public string OpenOperation()
        {
            ProductStockBLL psb = new ProductStockBLL();
            string list = Request.Form["list"].ToString();
            string[] str = list.Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("Scode");
            dt.Columns.Add("Vencode");
            for (int i = 0; i < str.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Scode"] = str[i].Split('❤')[0];
                dr["Vencode"] = str[i].Split('❤')[1];
                dt.Rows.Add(dr);
            }
            return psb.ProductOpenScode(dt, "2");
        }
        /// <summary>
        /// 选择暂停
        /// </summary>
        /// <returns></returns>
        public string CloseOperation()
        {
            ProductStockBLL psb = new ProductStockBLL();
            string list = Request.Form["list"].ToString();
            string[] str = list.Split(',');
            DataTable dt = new DataTable();
            dt.Columns.Add("Scode");
            dt.Columns.Add("Vencode");
            for (int i = 0; i < str.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Scode"] = str[i].Split('❤')[0];
                dr["Vencode"] = str[i].Split('❤')[1];
                dt.Rows.Add(dr);
            }
            return psb.ProductOpenScode(dt, "1");
        }
        /// <summary>
        /// 批量开放
        /// </summary>
        /// <returns></returns>
        public string OpenOperationAll()
        {
            ProductStockBLL psb = new ProductStockBLL();
            string[] str = new string[17];
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString().Trim();
            string style = Request.Form["style"] == null ? "" : Request.Form["style"].ToString().Trim();
            string price = Request.Form["price"] == null ? "" : Request.Form["price"].ToString().Trim();
            string price1 = Request.Form["price1"] == null ? "" : Request.Form["price1"].ToString().Trim();
            string cat = Request.Form["Cat1"] == null ? "" : Request.Form["Cat1"].ToString().Trim();
            string brand = Request.Form["brand"] == null ? "" : Request.Form["brand"].ToString().Trim();//品牌
            string stcok = Request.Form["stcok"] == null ? "" : Request.Form["stcok"].ToString().Trim();
            string stcok1 = Request.Form["stcok1"] == null ? "" : Request.Form["stcok1"].ToString().Trim();
            string time = Request.Form["time"] == null ? "" : Request.Form["time"].ToString().Trim();
            string time1 = Request.Form["time1"] == null ? "" : Request.Form["time1"].ToString().Trim();
            string CatLb = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString().Trim();//类别
            string Vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString().Trim();//供应商
            str[0] = scode;
            str[1] = style;
            str[2] = price;
            str[3] = price1;
            str[4] = cat;//季节
            str[5] = stcok;
            str[6] = stcok1;
            str[7] = brand; //品牌
            str[8] = time;
            str[9] = time1;
            str[10] = CatLb;//类别
            str[11] = Vencode;//供应商
            str[12] = userInfo.User.Id.ToString();
            str[13] = "";
            str[14] = "";
            str[15] = Request.Form["Imagefile"] == null ? "" : Request.Form["Imagefile"].ToString();
            str[16] = Request.Form["Descript"] == null ? "" : Request.Form["Descript"].ToString();
            DataTable dt = psb.GetProductOpenScode(str);
            return psb.ProductOpenScode(dt, "2");
        }
        /// <summary>
        /// 批量关闭
        /// </summary>
        /// <returns></returns>
        public string CloseOperationAll()
        {
            ProductStockBLL psb = new ProductStockBLL();
            string[] str = new string[17];
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString().Trim();
            string style = Request.Form["style"] == null ? "" : Request.Form["style"].ToString().Trim();
            string price = Request.Form["price"] == null ? "" : Request.Form["price"].ToString().Trim();
            string price1 = Request.Form["price1"] == null ? "" : Request.Form["price1"].ToString().Trim();
            string cat = Request.Form["Cat1"] == null ? "" : Request.Form["Cat1"].ToString().Trim();
            string brand = Request.Form["brand"] == null ? "" : Request.Form["brand"].ToString().Trim();//品牌
            string stcok = Request.Form["stcok"] == null ? "" : Request.Form["stcok"].ToString().Trim();
            string stcok1 = Request.Form["stcok1"] == null ? "" : Request.Form["stcok1"].ToString().Trim();
            string time = Request.Form["time"] == null ? "" : Request.Form["time"].ToString().Trim();
            string time1 = Request.Form["time1"] == null ? "" : Request.Form["time1"].ToString().Trim();
            string CatLb = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString().Trim();//类别
            string Vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString().Trim();//供应商
            str[0] = scode;
            str[1] = style;
            str[2] = price;
            str[3] = price1;
            str[4] = cat;//季节
            str[5] = stcok;
            str[6] = stcok1;
            str[7] = brand; //品牌
            str[8] = time;
            str[9] = time1;
            str[10] = CatLb;//类别
            str[11] = Vencode;//供应商
            str[12] = userInfo.User.Id.ToString();
            str[13] = "";
            str[14] = "";
            str[15] = Request.Form["Imagefile"] == null ? "" : Request.Form["Imagefile"].ToString();
            str[16] = Request.Form["Descript"] == null ? "" : Request.Form["Descript"].ToString();
            DataTable dt = psb.GetProductOpenScode(str);
            return psb.ProductOpenScode(dt, "1");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int BrandOpenProductSaveMenuId;
        public ActionResult BrandOpenProduct()
        {
            ProductStockBLL psb = new ProductStockBLL();
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
            BrandOpenProductSaveMenuId = menuId;
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ProductHelper php = new ProductHelper();
            ViewData["Vencode"] = php.GetVencodeDDlist() ;
            return View();
        }
        /// <summary>
        /// 数据源
        /// </summary>
        /// <returns></returns>
        public string ShowProductSource() 
        {
            ProductStockBLL psb=new ProductStockBLL();
            List<model.productsource> list = psb.GetProductSourceAll();
            StringBuilder alltable = new StringBuilder();
            alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'><tr style='text-align:center;'>");
            alltable.Append("<th>编号</th>");
            alltable.Append("<th>数据源名</th>");
            alltable.Append("<th>已开放</th>");
            alltable.Append("<th>未开放</th>");
            alltable.Append("<th>操作</th>");
            alltable.Append("<th>细节</th>");
            alltable.Append("</tr>");
            for (int i = 0; i < list.Count; i++) 
            {
                alltable.Append("<tr style='text-align:center;'>");
                alltable.Append("<td>"+list[i].Id+"</td>");
                alltable.Append("<td>" + list[i].sourceName + "</td>");
                alltable.Append("<td>"+psb.GetProductStockOpenCount(list[i].SourceCode,"2")+"</td>");
                alltable.Append("<td>" + psb.GetProductStockOpenCount(list[i].SourceCode, "1") + "</td>");
                alltable.Append("<td><a href='#' onclick='OpenAll(\"" + list[i].SourceCode + "\",2)' >开放数据源</a>丨丨<a href='#' onclick='OpenAll(\"" + list[i].SourceCode + "\",1)'>暂停数据源</a></td>");
                alltable.Append("<td><a href='#' onclick='QueryBrand(\"" + list[i].SourceCode + "\")'>查看品牌</a>丨丨<a href='#' onclick='QueryType(\"" + list[i].SourceCode + "\")'>查看类别</a></td>");
                alltable.Append("</tr>");
            }
            return alltable.ToString();
        }
        /// <summary>
        /// 显示品牌配置
        /// </summary>
        /// <returns></returns>
        public string GetBrandVen()
        {
            ProductStockBLL psb = new ProductStockBLL();
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            StringBuilder allpage = new StringBuilder();
            string[] str = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            char [] cr=new char [] {'A', 'B', 'C','D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};
            for (int i = 0; i < str.Length; i++) 
            {
                allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' type='checkbox' id='" + str[i] + "' onchange='CheckAll(\"" + str[i] + "\")' />" + str[i] + "</div>");
                List<model.BrandVen> list = psb.GetBrandByVencode(vencode);
                list = list.Where(a => a.BrandNameVen.StartsWith(str[i])).ToList();
                for (int j = 0; j < list.Count; j++) 
                {
                    allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' name='" + str[i] + "' id='" + list[j].BrandAbridge + "'>" + list[j].BrandNameVen + "</label></div>");
                }
                allpage.Append("</div><div style='clear:both'></div>");
            }
            List<model.BrandVen> listall = psb.GetBrandByVencode(vencode);
            for (int i = 0; i < str.Length; i++) 
            {
                listall = listall.Where(a => a.BrandNameVen.StartsWith(str[i]) == false).ToList();
            }
            allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' type='checkbox' id='else' onchange='CheckAll(else)' />其他</div>");
            for (int i = 0; i < listall.Count; i++) 
            {
                allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' name='else' id='" + listall[i].BrandAbridge + "'>" + listall[i].BrandNameVen + "</label></div>");
            }
            return allpage.ToString();
        }
        /// <summary>
        /// 显示类别
        /// </summary>
        /// <returns></returns>
        public string GetProductVen() 
        {
            ProductStockBLL psb = new ProductStockBLL();
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            StringBuilder allpage = new StringBuilder();
            List<model.productbigtype> listBigType = psb.GetProductBigTypeVencode();
            for (int i = 0; i < listBigType.Count; i++) 
            {
                allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' type='checkbox' id='" + listBigType[i].Id + "' onchange='CheckAll(\"" + listBigType[i].Id + "\")' />" + listBigType[i].bigtypeName + "</div>");
                List<model.producttypeVen> list = psb.GetProductTypeVencode(vencode);
                list = list.Where(a => a.BigId == listBigType[i].Id).ToList();
                for (int j = 0; j < list.Count; j++) 
                {
                    allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' name='" + listBigType[i].Id + "' id='" + list[j].TypeNo + "'>" + list[j].TypeName + "</label></div>");
                }
                allpage.Append("</div><div style='clear:both'></div>");
            }
            return  allpage.ToString();
        }
        /// <summary>
        /// 数据源库存开放或者关闭
        /// </summary>
        /// <returns></returns>
        public string OpenProduct() 
        {
            ProductStockBLL psb = new ProductStockBLL();
            string Dates = Request.Form["OpenArry"] == null ? "" : Request.Form["OpenArry"].ToString();//品牌缩写或者类别编号
            string [] Arry = Dates.Split(',');
            string vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();//数据源编号
            string openstate = Request.Form["OpenState"] == null ? "" : Request.Form["OpenState"].ToString();//1-暂停  2-开放
            string opentype = Request.Form["OpenType"] == null ? "" : Request.Form["OpenType"].ToString();//开放类型  Brand-开放品牌   ProductType-开放类别
            if (opentype == "Brand") 
            {
                bool result = psb.OpenPorudctBrand(Arry, openstate, vencode);
                if (result)
                {
                    return "操作成功！";
                }
                else 
                {
                    return "操作失败！";
                }
            }
            else if (opentype == "ProductType")
            {
                bool result = psb.OpenPorudctType(Arry, openstate, vencode);
                if (result)
                {
                    return "操作成功！";
                }
                else
                {
                    return "操作失败！";
                }
            }
            else 
            {
                return "操作失败";
            }
        }
        /// <summary>
        /// 开放或者暂停整个数据源
        /// </summary>
        /// <returns></returns>
        public string OpenAll() 
        {
            ProductStockBLL psb=new ProductStockBLL();
            string vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string openstate = Request.Form["OpenState"] == null ? "" : Request.Form["OpenState"].ToString();
            if (psb.OpenAll(vencode, openstate))
            {
                return "操作成功";
            }
            else 
            {
                return "操作失败";
            }
        }
    }
}
