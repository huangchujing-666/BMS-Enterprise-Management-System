using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using pbxdata.bll;
namespace pbxdata.web.Controllers
{
    public class ActivityController : BaseController
    {
        //
        // GET: /Activity/
        ActivityBLL ab = new ActivityBLL();
        ProductHelper php = new ProductHelper();



        #region 销售计划
        public ActionResult SalesPlant()
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
            SalesMenuId = menuId;
            // 查询
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["Type"] = php.GetTypeDDlist(userInfo.User.Id);
            ViewData["Brand"] = php.GetBrandDDlist(userInfo.User.Id);
            ViewData["Shop"] = php.GetShopByUserDDlist(userInfo.User.Id);
            return View();
        }
        /// <summary>
        /// 生成申请编号
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
        /// 创建活动
        /// </summary>
        /// <returns></returns>
        public string InsertSalesPlant()
        {
            string[] str = new string[8];
            str[0] = Request.Form["PlaseNo"] == null ? "" : Request.Form["PlaseNo"].ToString();
            str[1] = Request.Form["ActivityName"] == null ? "" : Request.Form["ActivityName"].ToString();
            str[2] = Request.Form["Loc"] == null ? "" : Request.Form["Loc"].ToString();
            str[3] = Request.Form["Brand"] == null ? "" : Request.Form["Brand"].ToString().Trim(',');
            str[4] = DateTime.Now.ToString();
            str[5] = Request.Form["SalesTime"] == null ? "" : Request.Form["SalesTime"].ToString();
            str[6] = Request.Form["StopTime"] == null ? "" : Request.Form["StopTime"].ToString();
            str[7] = userInfo.User.Id.ToString();
            return ab.InsertSalesPlant(str);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public static int SalesMenuId;
        public static int SalesPage = 0;
        public string GetSalesPlant()
        {

            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();

            #region 查询条件
            string Params = Request.Form["params"] == null ? "" : Request.Form["params"].ToString();

            string[] ShopParamss = helpcommon.StrSplit.StrSplitData(Params, ','); //参数集合

            string PlaseNo = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string ActivitName = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string ShState = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string Brand = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号

            PlaseNo = PlaseNo == "\'\'" ? string.Empty : PlaseNo;
            ActivitName = ActivitName == "\'\'" ? string.Empty : ActivitName;
            ShState = ShState == "\'\'" ? string.Empty : ShState;
            Brand = Brand == "\'\'" ? string.Empty : Brand;

            string[] strProduct = new string[6];
            strProduct[0] = PlaseNo;
            strProduct[1] = ActivitName;
            strProduct[2] = ShState;
            strProduct[3] = userInfo.User.Id.ToString();
            strProduct[4] = Brand;

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("SalesPlant");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段

            int count = ab.GetSalesPlantPageCount(strProduct);
            int PageSum = count % pageSize > 0 ? count / pageSize + 1 : count / pageSize;//页码

            int MinId = (pageIndex - 1) * pageSize;
            int MaxId = (pageIndex - 1) * pageSize + pageSize;

            DataTable dt = ab.GetSalesPlantPage(strProduct, MinId, MaxId);


            #endregion

            StringBuilder Alltable = new StringBuilder();

            #region  Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < allTableName.Length; i++)
            {
                if (s.Contains(allTableName[i]))
                {
                    Alltable.Append("<th>");
                    if (allTableName[i] == "Id")
                        Alltable.Append("编号");
                    if (allTableName[i] == "PlaseNo")
                        Alltable.Append("申请编号");
                    if (allTableName[i] == "ActivityName")
                        Alltable.Append("活动名称");
                    if (allTableName[i] == "Loc")
                        Alltable.Append("店铺");
                    if (allTableName[i] == "Brand")
                        Alltable.Append("品牌");
                    if (allTableName[i] == "UserId")
                        Alltable.Append("负责人");
                    if (allTableName[i] == "CountMoney")
                        Alltable.Append("总金额");
                    if (allTableName[i] == "CountScode")
                        Alltable.Append("总货物");
                    if (allTableName[i] == "ApplyTime")
                        Alltable.Append("报备日期");
                    if (allTableName[i] == "SalesTime")
                        Alltable.Append("开放日期");
                    if (allTableName[i] == "StopTime")
                        Alltable.Append("截至日期");
                    if (allTableName[i] == "AuditState")
                        Alltable.Append("审核状态");
                    if (allTableName[i] == "LockTime")
                        Alltable.Append("锁定时间");
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
                    Alltable.Append("</th>");
                }
            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #endregion
            
            #region  Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["RowId"] + "</td>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {
                        if (allTableName[j] == "Brand")
                        {
                            string BrandName = "";
                            string[] strBrand = dt.Rows[i][allTableName[j]].ToString().Split(',');//将品牌进行分割
                            for (int k = 0; k < strBrand.Length; k++)
                            {
                                string name = ab.GetBrandNameByBrands(strBrand[k]);//将分割后的品牌缩写变成全称
                                BrandName += "," + name;
                            }
                            BrandName = BrandName.Trim(',');
                            Alltable.Append("<td>" + BrandName + "</td>");
                        }
                        else if (allTableName[j] == "AuditState")
                        {
                            if (dt.Rows[i][allTableName[j]].ToString() == "1")
                            {
                                Alltable.Append("<td style='color:blue;'>已通过</td>");
                            }
                            else if (dt.Rows[i][allTableName[j]].ToString() == "2")
                            {
                                Alltable.Append("<td style='color:red;'>不通过</td>");
                            }
                            else
                            {
                                Alltable.Append("<td style='color:red;'>未审核</td>");
                            }
                        }
                        else if (allTableName[j] == "Loc")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["ShopName"] + "</td>");
                        }
                        else if (allTableName[j] == "UserId")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["userRealName"] + "</td>");
                        }
                        else
                        {
                            Alltable.Append("<td>" + dt.Rows[i][allTableName[j]] + "</td>");
                        }
                    }
                }

                if (ph.isFunPermisson(roleId, menuId, funName.checkOrderShow))
                {
                    if (dt.Rows[i]["AuditState"].ToString() == "0")
                    {
                        Alltable.Append("<td><a href='#' onclick='Pass(\"" + dt.Rows[i]["PlaseNo"] + "\")'>通过</a>丨丨<a href='#' onclick='NoPass(\"" + dt.Rows[i]["PlaseNo"] + "\")'>不通过</a></td>");
                    }
                    else
                    {
                        Alltable.Append("<td>回退</td>");
                    }
                }
                else
                {
                    Alltable.Append("<td><a href='#' onclick='AddProduct(\"" + dt.Rows[i]["PlaseNo"] + "\")'>增加商品</a>||");
                    Alltable.Append("<a href='#' onclick='LookProduct(\"" + dt.Rows[i]["PlaseNo"] + "\",\"" + menuId + "\")'>查看商品</a></td>");
                }
                Alltable.Append("</tr>");
            }
            Alltable.Append("</table>");
            #endregion

            #region 分页
            Alltable.Append("-----");
            Alltable.Append(PageSum + "-----" + count);
            #endregion
            return Alltable.ToString();
        }
        /// <summary>
        /// 得到可选商品
        /// </summary>
        /// <returns></returns>
        public static int ProductPage = 0;
        public static string PlaseNoQuest = "";
        public ActionResult AddProduct()
        {
            PlaseNoQuest = Request.QueryString["PlaseNo"] == null ? "" : Request.QueryString["PlaseNo"].ToString();
            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["Type"] = php.GetTypeDDlist(userInfo.User.Id);
            ViewData["Brand"] = php.GetBrandDDlist(userInfo.User.Id);
            ViewData["Shop"] = php.GetShopByUserDDlist(userInfo.User.Id);
            return View();
        }
        /// <summary>
        /// 显示可挑选商品
        /// </summary>
        /// <returns></returns>
        public string GetProduct()
        {
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件
            string Scode = helpcommon.StrSplit.StrSplitData(orderParamss[13], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//货号
            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[13], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string Type = helpcommon.StrSplit.StrSplitData(orderParamss[13], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别

            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            Type = Type == "\'\'" ? string.Empty : Type;

            string[] str = new string[5];
            str[0] = PlaseNoQuest;
            str[1] = "";
            str[2] = "";
            str[3] = "";
            str[4] = "";
            DataTable dtBrand = ab.GetSalesPlantPage(str, 0, 10);//查询出当前申请编号的信息 取得所有品牌
            string[] strbrand = dtBrand.Rows[0]["Brand"].ToString().Split(',');
            string[] strProduct = new string[4];
            string Cat2 = "";
            for (int i = 0; i < strbrand.Length; i++)
            {
                string temp = ",'" + strbrand[i] + "'";
                Cat2 = Cat2 + temp;
                Cat2 = Cat2.Trim(',');
            }
            strProduct[0] = Type;
            strProduct[1] = Cat2;
            strProduct[2] = Scode;
            strProduct[3] = Style;

            int count = ab.GetProductBybrandCount(strProduct);
            
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            int PageSum = count % pageSize > 0 ? count / pageSize + 1 : count / pageSize;//页码

            int minid = (pageIndex - 1) * pageSize;
            int maxid = (pageIndex - 1) * pageSize + pageSize;
            DataTable dt = ab.GetProductBybrand(strProduct, minid, maxid);
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("product");
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.AddProduct);
            StringBuilder Alltable = new StringBuilder();

            #region  Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < allTableName.Length; i++)
            {
                if (s.Contains(allTableName[i]))
                {
                    Alltable.Append("<th>");
                    if (allTableName[i] == "Id")
                        Alltable.Append("编号");
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
                    if (allTableName[i] == "Disca")
                        Alltable.Append("折扣1");
                    if (allTableName[i] == "Discb")
                        Alltable.Append("折扣2");
                    if (allTableName[i] == "Discc")
                        Alltable.Append("折扣3");
                    if (allTableName[i] == "Discd")
                        Alltable.Append("折扣4");
                    if (allTableName[i] == "Disce")
                        Alltable.Append("折扣5");
                    if (allTableName[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (allTableName[i] == "Model")
                        Alltable.Append("型号");
                    if (allTableName[i] == "Rolevel")
                        Alltable.Append("预警库存");
                    if (allTableName[i] == "Roamt")
                        Alltable.Append("最少订货量");
                    if (allTableName[i] == "Stopsales")
                        Alltable.Append("停售库存");
                    if (allTableName[i] == "Loc")
                        Alltable.Append("店铺");
                    if (allTableName[i] == "Balance")
                        Alltable.Append("供应商库存");
                    if (allTableName[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (allTableName[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (allTableName[i] == "UserId")
                        Alltable.Append("操作人");
                    if (allTableName[i] == "PrevStock")
                        Alltable.Append("上一次库存");
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
            Alltable.Append("<th><label style='width:100%;height:100%;display:block;padding-top:15px;'>全选<input type='checkbox' id='CheckAll' onclick='CheckAll()'/></label></th>");
            Alltable.Append("</tr>");
            #endregion

            #region  Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["RowId"] + "</td>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {
                        if (allTableName[j] == "Cat")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["BrandName"] + "</td>");
                        }
                        else if (allTableName[j] == "Cat2")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                        }
                        else if (allTableName[j] == "Balance")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["Stock"] + "</td>");
                        }
                        else if (allTableName[j] == "Imagefile")
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
                            Alltable.Append("<td>" + dt.Rows[i][allTableName[j]] + "</td>");
                        }
                    }
                }
                Alltable.Append("<td><label style='width:100%;height:100%;display:block;padding-top:15px;'><input type='checkbox' class='Check' Scode='" + dt.Rows[i]["Scode"] + "' /></label></td>");
                Alltable.Append("</tr>");
            }
            Alltable.Append("</table>");
            #endregion

            #region Table分页
            Alltable.Append("-----");
            Alltable.Append(PageSum + "-----" + count);
            #endregion

            return Alltable.ToString();
        }
        /// <summary>
        /// 确认添加商品
        /// </summary>
        /// <returns></returns>
        public string OkCheck()
        {
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string[] scodes = scode.Trim(',').Split('❤');
            DataTable dt = ab.GetInsertTable(scodes);
            dt.Columns.Add("PlaseNo");
            dt.Columns.Add("UserId");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["PlaseNo"] = PlaseNoQuest;
                dt.Rows[i]["UserId"] = userInfo.User.Id;
            }
            bool result = ab.InsertPlantProductStock(dt);
            if (result)
            {
                DataTable dtcount = ab.StatisticsSalesMoney(PlaseNoQuest);
                if (ab.UpdateSalesPlantCount(dtcount.Rows[0]["CountMoney"].ToString(), dtcount.Rows[0]["CountBalance"].ToString(), PlaseNoQuest))
                {
                    return "添加成功！";
                }
                else
                {
                    return "添加成功，数据统计失败！";
                }
            }
            else
            {
                return "添加失败！";
            }
        }
        /// <summary>
        /// 查看商品 弹窗方式
        /// </summary>
        public static string PlaseNoLook = "";
        public ActionResult PlantProducts()
        {
            PlaseNoLook = Request.QueryString["PlaseNo"] == null ? "" : Request.QueryString["PlaseNo"].ToString();
            ViewBag.PlaseNo = PlaseNoLook;
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["MenuId"]);
            ViewData["myMenuId"] = menuId;
            
            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["Type"] = php.GetTypeDDlist(userInfo.User.Id);
            ViewData["Brand"] = php.GetBrandDDlist(userInfo.User.Id);
            ViewData["Shop"] = php.GetShopByUserDDlist(userInfo.User.Id);
            return View();
        }
        /// <summary>
        /// 查看商品 弹窗
        /// </summary>
        /// <returns></returns>
        /// 
        public string GetPlantProduct()
        {
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();

            #region 查询条件
            string Params = Request.Form["params"] == null ? "" : Request.Form["params"].ToString();

            string[] ShopParamss = helpcommon.StrSplit.StrSplitData(Params, ','); //参数集合

            string Scode = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string Style = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string Cat = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string Cat2 = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string PlaseNo = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号

            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            Cat = Cat == "\'\'" ? string.Empty : Cat;
            Cat2 = Cat2 == "\'\'" ? string.Empty : Cat2;
            PlaseNo = PlaseNo == "\'\'" ? string.Empty : PlaseNo;

            string[] strProduct = new string[6];
            strProduct[0] = Cat;
            strProduct[1] = Cat2;
            strProduct[2] = Scode;
            strProduct[3] = Style;
            strProduct[4] = userInfo.User.Id.ToString();
            strProduct[5] = PlaseNo;


            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("PlantProductStock");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.LookShop);//获得当前权限字段
            #endregion
            int count = ab.GetPlantProductCount(strProduct);
            int PageCount = count % pageSize > 0 ? count / pageSize + 1 : count / pageSize;

            int MinId = pageSize * (pageIndex - 1);
            int MaxId = pageSize * (pageIndex - 1) + pageSize;
            DataTable dt = ab.GetPlantProduct(strProduct, MinId, MaxId);

            StringBuilder Alltable = new StringBuilder();

            #region Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < allTableName.Length; i++)
            {
                if (s.Contains(allTableName[i]))
                {
                    Alltable.Append("<th>");
                    if (allTableName[i] == "Id")
                        Alltable.Append("编号");
                    if (allTableName[i] == "PlaseNo")
                        Alltable.Append("申请编号");
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
                    if (allTableName[i] == "LocPrice")
                        Alltable.Append("店铺价格");
                    if (allTableName[i] == "ActivityPrice")
                        Alltable.Append("活动价格");
                    if (allTableName[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (allTableName[i] == "Model")
                        Alltable.Append("型号");
                    if (allTableName[i] == "Loc")
                        Alltable.Append("店铺");
                    if (allTableName[i] == "Balance")
                        Alltable.Append("销售库存");
                    if (allTableName[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (allTableName[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (allTableName[i] == "UserId")
                        Alltable.Append("操作人");
                    if (allTableName[i] == "Def1")
                        Alltable.Append("上一次库存");
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
            Alltable.Append("<th>系统库存</th>");
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #endregion

            #region  Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["RowId"] + "</td>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {
                        if (allTableName[j] == "Cat")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["BrandName"] + "</td>");
                        }
                        else if (allTableName[j] == "Cat2")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                        }
                        else if (allTableName[j] == "Imagefile")
                        {
                            if (dt.Rows[i]["Imagefile"].ToString() != "" && dt.Rows[i]["Imagefile"] != null)
                            {
                                Alltable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["Imagefile"] + "' width='50' height='50'/></td>");
                            }
                            else
                            {
                                Alltable.Append("<td></td>");
                            }
                        }
                        else if (allTableName[j] == "LocPrice") { Alltable.Append("<td style='color:red;'>" + dt.Rows[i]["LocPrice"] + "</td>"); }
                        else if (allTableName[j] == "ActivityPrice") { Alltable.Append("<td style='color:blue;'>" + dt.Rows[i]["ActivityPrice"] + "</td>"); }
                        else
                        {
                            Alltable.Append("<td>" + dt.Rows[i][allTableName[j]] + "</td>");
                        }
                    }
                }
                Alltable.Append("<td>" + dt.Rows[i]["Stock"] + "</td>");
                Alltable.Append("<td><a href='#' onclick='UpdatePrice(\"" + dt.Rows[i]["Scode"] + "\",\"" + dt.Rows[i]["PlaseNo"] + "\")'>修改</a></td>");
                Alltable.Append("</tr>");
            }
            Alltable.Append("</table>");
            #endregion

            #region 分页
            Alltable.Append("-----");
            Alltable.Append(PageCount + "-----" + count);
            #endregion
            return Alltable.ToString();
        }
        #endregion



        #region 销售商品
        public ActionResult PlantProduct()
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
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["Type"] = php.GetTypeDDlist(userInfo.User.Id);
            ViewData["Brand"] = php.GetBrandDDlist(userInfo.User.Id);
            ViewData["Shop"] = php.GetShopByUserDDlist(userInfo.User.Id);
            return View();
        }
        /// <summary>
        /// 查看销售商品所有
        /// </summary>
        /// <returns></returns>
        public string GetPlantProducts()
        {

            usersbll usb=new usersbll();
            PublicHelpController ph=new PublicHelpController();

            #region 查询条件
            string Params = Request.Form["params"] == null ? "" : Request.Form["params"].ToString();

            string[] ShopParamss = helpcommon.StrSplit.StrSplitData(Params, ','); //参数集合
            
            string Scode = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//货号
            string Style = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string Cat = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//品牌
            string Cat2 = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别
            string PlaseNo = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//申请编号

            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            Cat = Cat == "\'\'" ? string.Empty : Cat;
            Cat2 = Cat2 == "\'\'" ? string.Empty : Cat2;
            PlaseNo = PlaseNo == "\'\'" ? string.Empty : PlaseNo;

            string[] strProduct = new string[6];
            strProduct[0] = Cat;
            strProduct[1] = Cat2;
            strProduct[2] = Scode;
            strProduct[3] = Style;
            strProduct[4] = userInfo.User.Id.ToString();
            strProduct[5] = PlaseNo;


            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("PlantProductStock");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段
            #endregion 

            int count = ab.GetPlantProductCount(strProduct);
            int PageCount = count % pageSize > 0 ? count / pageSize + 1 : count / pageSize;

            int MinId = pageSize * (pageIndex-1);
            int MaxId = pageSize * (pageIndex-1) + pageSize;
            DataTable dt = ab.GetPlantProduct(strProduct, MinId, MaxId);
            
            StringBuilder Alltable = new StringBuilder();

            #region Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < allTableName.Length; i++)
            {
                if (s.Contains(allTableName[i]))
                {
                    Alltable.Append("<th>");
                    if (allTableName[i] == "Id")
                        Alltable.Append("编号");
                    if (allTableName[i] == "PlaseNo")
                        Alltable.Append("申请编号");
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
                    if (allTableName[i] == "LocPrice")
                        Alltable.Append("店铺价格");
                    if (allTableName[i] == "ActivityPrice")
                        Alltable.Append("活动价格");
                    if (allTableName[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (allTableName[i] == "Model")
                        Alltable.Append("型号");
                    if (allTableName[i] == "Loc")
                        Alltable.Append("店铺");
                    if (allTableName[i] == "Balance")
                        Alltable.Append("销售库存");
                    if (allTableName[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (allTableName[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (allTableName[i] == "UserId")
                        Alltable.Append("操作人");
                    if (allTableName[i] == "Def1")
                        Alltable.Append("上一次库存");
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
            Alltable.Append("<th>系统库存</th>");
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #endregion

            #region  Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["RowId"] + "</td>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {
                        if (allTableName[j] == "Cat")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["BrandName"] + "</td>");
                        }
                        else if (allTableName[j] == "Cat2")
                        {
                            Alltable.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                        }
                        else if (allTableName[j] == "Imagefile")
                        {
                            if (dt.Rows[i]["Imagefile"].ToString() != "" && dt.Rows[i]["Imagefile"] != null)
                            {
                                Alltable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["Imagefile"] + "' width='50' height='50'/></td>");
                            }
                            else
                            {
                                Alltable.Append("<td></td>");
                            }
                        }
                        else if (allTableName[j] == "LocPrice") { Alltable.Append("<td style='color:red;'>" + dt.Rows[i]["LocPrice"] + "</td>"); }
                        else if (allTableName[j] == "ActivityPrice") { Alltable.Append("<td style='color:blue;'>" + dt.Rows[i]["ActivityPrice"] + "</td>"); }
                        else
                        {
                            Alltable.Append("<td>" + dt.Rows[i][allTableName[j]] + "</td>");
                        }
                    }
                }
                Alltable.Append("<td>" + dt.Rows[i]["Stock"] + "</td>");
                Alltable.Append("<td><a href='#' onclick='UpdatePrice(\"" + dt.Rows[i]["Scode"] + "\",\"" + dt.Rows[i]["PlaseNo"] + "\")'>修改</a></td>");
                Alltable.Append("</tr>");
            }
            Alltable.Append("</table>");
            #endregion

            #region 分页
            Alltable.Append("-----");
            Alltable.Append(PageCount + "-----" + count);
            #endregion
            return Alltable.ToString();
        }
        /// <summary>
        /// 得到当前货号的价格
        /// </summary>
        /// <returns></returns>
        public string GetPlantProductPrice()
        {
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string plaseNo = Request.Form["PlaseNo"] == null ? "" : Request.Form["PlaseNo"].ToString();
            return ab.GetPlantProductPrice(scode, plaseNo);
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <returns></returns>
        public string UpdatePlantProductPrice()
        {
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string plaseNo = Request.Form["PlaseNo"] == null ? "" : Request.Form["PlaseNo"].ToString();
            string locprice = Request.Form["locprice"] == null ? "" : Request.Form["locprice"].ToString();
            string activitprice = Request.Form["activitprice"] == null ? "" : Request.Form["activitprice"].ToString();
            string Balance = Request.Form["Balance"] == null ? "" : Request.Form["Balance"].ToString();
            return ab.UpdatePlantProductPrice(scode, plaseNo, locprice, activitprice, Balance);
        }
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        public string UpdateSalesState()
        {
            string state = Request.Form["State"] == null ? "" : Request.Form["State"].ToString();
            string plaseno = Request.Form["PlaseNo"] == null ? "" : Request.Form["PlaseNo"].ToString();
            return ab.UpdateSalesState(state, plaseno);
        }
        #endregion
        
        

    }
}
