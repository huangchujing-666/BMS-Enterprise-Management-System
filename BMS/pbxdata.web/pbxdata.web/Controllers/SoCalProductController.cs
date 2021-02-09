 using Aliyun.OpenServices.OpenStorageService;
using Maticsoft.DBUtility;
using pbxdata.bll;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
namespace pbxdata.web.Controllers
{
    public class SoCalProductController : BaseController
    {
        //
        // GET: /SoCalProduct/



        ProductHelper php = new ProductHelper();



        #region 现货挑选
        public static int Savemenuid;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectSoCalProduct()
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
                Savemenuid = menuId;
            }
            // 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["menuId"] = Savemenuid;
            #region   显示当前登陆人
            ProductHelper phb = new ProductHelper();
            ProductTypeBLL ptb = new ProductTypeBLL();
            string userName = userInfo.User.userName;
            DataTable dtPersion = ptb.GetPersonaIdByUserName(userName);//通过当前用户查找当前用户所属角色
            string Name = dtPersion.Rows[0][1].ToString();//用户名
            ViewData["UserName"] = Name;
            #endregion
            #region  显示当前登陆人的店铺
            ViewData["ShopName"] = phb.GetShopByUserDDlist(userInfo.User.Id);
            #endregion
            //   页码下拉框
            ViewData["PageSum"] = phb.GetPageDDlist();
            ViewData["Type"] = phb.GetTypeDDlist(userInfo.User.Id);
            ViewData["Brand"] = phb.GetBrandDDlist(userInfo.User.Id);
            return View();
        }
        /// <summary>
        /// 显示所有可挑选商品
        /// </summary>
        /// <returns></returns>
        public string ShowCheckProduct()
        {
            SoCalProductBll scp = new SoCalProductBll();
            ProductStockBLL psb = new ProductStockBLL();
            shopbll sbl = new shopbll();
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();

            #region 查询条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合

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
            string Imagefiel = helpcommon.StrSplit.StrSplitData(orderParamss[11], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//有图/无图

            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            PriceMax = PriceMax == "\'\'" ? string.Empty : PriceMax;
            PriceMin = PriceMin == "\'\'" ? string.Empty : PriceMin;
            Season = Season == "\'\'" ? string.Empty : Season;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            StockMax = StockMax == "\'\'" ? string.Empty : StockMax;
            StockMin = StockMin == "\'\'" ? string.Empty : StockMin;
            TimeMin = TimeMin == "\'\'" ? string.Empty : TimeMin;
            TimeMax = TimeMax == "\'\'" ? string.Empty : TimeMax;
            Type = Type == "\'\'" ? string.Empty : Type;
            Imagefiel = Imagefiel == "\'\'" ? string.Empty : Imagefiel;

            string[] str = new string[15];
            str[0] = Scode;
            str[1] = Style;
            str[2] = PriceMin;
            str[3] = PriceMax;
            str[4] = Season;
            str[5] = Type;
            str[6] = Brand;
            str[7] = StockMin;
            str[8] = StockMax;
            str[9] = TimeMin;
            str[10] = TimeMax;
            str[11] = "";
            str[12] = userInfo.User.Id.ToString();
            str[13] = "";
            str[14] = Imagefiel;

            #endregion 
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("productstock");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段
            bool Storage = ph.isFunPermisson(roleId, menuId, funName.Storage); //入库权限
            bool Marker = ph.isFunPermisson(roleId, menuId, funName.Marker);//商品标记权限（备注）

            int Count = scp.GetSoCalProductCount(str) ;//数据总个数
            int PageCount = Count % pageSize > 0 ? Count / pageSize + 1 : Count / pageSize;//数据总页数

            int MinId = pageSize * (pageIndex - 1);
            int MaxId = pageSize * (pageIndex - 1) + pageSize;

            DataTable dt = scp.GetSoCalProduct(str, MinId, MaxId);

            StringBuilder Alltable = new StringBuilder();
            #region Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                #region//判断头
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
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("默认4");
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
                #endregion
            }
            Alltable.Append("<th>店铺</th>");
            Alltable.Append("<th>操作</th>");
            if (ph.isFunPermisson(roleId, menuId, funName.CheckProduct))
            {
                Alltable.Append("<th>选货<input type='checkbox' id='CheckAll' onchange='CheckAll()'></th>");
            }
            #endregion
            #region   Table内容
            Alltable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + (i + (pageIndex-1) * pageSize + 1) + "</td>");
                for (int j = 0; j < s.Length; j++)
                {

                    if (allTableName.Contains(s[j]))
                    {
                        if (s[j] == "Imagefile")
                        {
                            if (dt.Rows[i]["Imagefile"].ToString() == "")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i]["Imagefile"].ToString());
                                Alltable.Append("</td>");
                            }
                            else
                            {

                                Alltable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["ImagePath"] + "' width='50' height='50'/></td>");
                            }
                        }
                        else if (s[j] == "Cat")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["BrandName"].ToString());
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Cat2")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["TypeName"].ToString());
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
                DataTable dtShopName = scp.GetShopByScodeSoCal(dt.Rows[i]["Scode"].ToString(), userInfo.User.Id.ToString());
                string shopname = "";
                if (dtShopName.Rows.Count > 0)
                {
                    for (int zc = 0; zc < dtShopName.Rows.Count; zc++)
                    {
                        shopname += "," + dtShopName.Rows[zc]["ShopName"].ToString();
                    }
                    shopname = shopname.Trim(',');
                }
                Alltable.Append("<td ><label style='color:blue;'>" + shopname + "</label></td>");
                Alltable.Append("<td><a href='#' onclick='LookAtrrbutt(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Vencode"].ToString().Trim() + "\")'>属性</a></td>");
                if (ph.isFunPermisson(roleId, menuId, funName.CheckProduct))
                {
                    Alltable.Append("<td><label style='width:100%;height:100%;display:block;padding-top:2px;'><input type='checkbox' class='Check' Scode='" + dt.Rows[i]["Scode"] + "' Vencode='" + dt.Rows[i]["Vencode"] + "'></label></td>");
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
        /// 保存选货
        /// </summary>
        /// <returns></returns>
        public string SaveCheckProduct()
        {
            ShopProductBll spb = new ShopProductBll();
            ProductStockBLL psb = new ProductStockBLL();
            SoCalProductBll scb = new SoCalProductBll();
            ProductTypeBLL ptb = new ProductTypeBLL();
            string result = "";//返回成功结果
            string sbResult = "";//返回失败结果
            int resultCount = 0;//成功的数量
            int SbCount = 0;//失败的数量
            string str = Request.Form["Scodes"] == null ? "" : Request.Form["Scodes"].ToString();

            string str1 = Request.Form["Vencodes"] == null ? "" : Request.Form["Vencodes"].ToString();
            string[] Scode = str.Split(',');
            string[] Vencode = str1.Split(',');
            string StrNoInsertScode = "";
            string shopid = Request.Form["ShopId"] == null ? "" : Request.Form["ShopId"].ToString();//得到店铺
            if (Scode.Length > 0)//如果有选中的才做操作
            {
                for (int i = 0; i < Scode.Length - 1; i++)
                {
                    if (scb.GetSoCalByScodeAndShopId(Scode[i + 1].ToString(), shopid))//查询当前货号  在当前店铺是否已经分配库存
                    {
                        StrNoInsertScode += "," + Scode[i + 1].ToString();
                        sbResult = "当前货品已存在您的店铺当中：" + StrNoInsertScode;
                    }
                    else //为分配库存则添加
                    {
                        DataTable dt = scb.GetSoCalProductByScodeAndVencode(Scode[i + 1].ToString(), Vencode[i + 1].ToString());
                        dt.Columns.Add("UpdateDatetime");
                        dt.Columns.Add("UserId");
                        dt.Columns.Add("ShopId");
                        dt.Columns.Add("SaleState");
                        dt.Rows[0]["UpdateDatetime"] = DateTime.Now.ToString();
                        dt.Rows[0]["UserId"] = userInfo.User.Id.ToString();
                        dt.Rows[0]["ShopId"] = shopid;
                        dt.Rows[0]["SaleState"] = 2;//销售状态  默认为 不开放销售   0  未开放  1 开放 2  选货状态
                        dt.Rows[0]["Def8"] = 1;//销售状态  默认为 不开放销售   0  未开放  1 开放 2  选货状态
                        if (scb.InsertSoCaloutsideProduct(dt))
                        {
                            resultCount++;
                            result = "有" + resultCount + "条货品选货成功！";
                        }
                        else
                        {
                            SbCount++;
                            result = "有" + SbCount + "条货品选货失败！";
                        }
                    }
                }
            }
            else
            {
                result = "您还未选中货品！";
            }
            return result + sbResult;
        }
        /// <summary>
        /// 查看属性
        /// </summary>
        /// <returns></returns>
        public string LookAttrByCheck()
        {
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            SoCalProductBll scb = new SoCalProductBll();
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            DataTable dt = scb.LookSoCalPrdouctAttr(scode, vencode);
            string property = GetTypeNo(scode, dt.Rows[0]["Cat2"].ToString());
            string s = string.Format(@"<table id='tabProperty'>
            <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><img src='{11}' style='width:120px;' /></td></tr>
            <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' disabled='disabled' type='text' value='{2}' /></td></tr>
            <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                <td>季节：<input type='text' value='{4}' disabled='disabled' /></td></tr>
            <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
            <tr><td>预警库存：<input id='txtRolevelE' disabled='disabled' type='text' value='{9}' /></td><td>类别：<input type='text' disabled='disabled' id='hidId'' value='{5}' /></td></tr>
        </table>", dt.Rows[0]["Scode"].ToString()
                             , dt.Rows[0]["Descript"].ToString()
                             , dt.Rows[0]["Cdescript"].ToString()
                             , dt.Rows[0]["Cat"].ToString()
                             , dt.Rows[0]["Cat1"].ToString()
                             , dt.Rows[0]["Cat2"].ToString()
                             , dt.Rows[0]["Clolor"].ToString()
                             , dt.Rows[0]["Style"].ToString()
                             , dt.Rows[0]["Size"].ToString()
                             , dt.Rows[0]["Rolevel"].ToString()
                             , dt.Rows[0]["Id"].ToString()
                             , dt.Rows[0]["Imagefile"].ToString()

                        );
            StringBuilder alltableprice = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            int menuId = Savemenuid;
            string[] Spermission = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            alltableprice.Append("<table class='mytable' style='font-size:12px;min-width:300px;'>");
            alltableprice.Append("<tr>");
            alltableprice.Append("<th>来源</th>");
            if (Spermission.Contains("Pricea"))
            {
                alltableprice.Append("<th>吊牌价</th>");
            }
            if (Spermission.Contains("Priceb"))
            {
                alltableprice.Append("<th>零售价</th>");
            }
            if (Spermission.Contains("Pricec"))
            {
                alltableprice.Append("<th>供货价</th>");
            }
            if (Spermission.Contains("Priced"))
            {
                alltableprice.Append("<th>批发价</th>");
            }
            if (Spermission.Contains("Pricee"))
            {
                alltableprice.Append("<th>成本价</th>");
            }
            alltableprice.Append("<th>库存</th>");
            alltableprice.Append("</tr>");
            alltableprice.Append("<tr>");
            alltableprice.Append("<td>" + dt.Rows[0]["sourceName"] + "</td>");
            if (Spermission.Contains("Pricea"))
            {
                alltableprice.Append("<td>");
                alltableprice.Append(dt.Rows[0]["Pricea"]);
                alltableprice.Append("</td>");

            }
            if (Spermission.Contains("Priceb"))
            {

                alltableprice.Append("<td>");
                alltableprice.Append(dt.Rows[0]["Priceb"]);
                alltableprice.Append("</td>");
            }
            if (Spermission.Contains("Pricec"))
            {


                alltableprice.Append("<td>");
                alltableprice.Append(dt.Rows[0]["Pricec"]);
                alltableprice.Append("</td>");

            }
            if (Spermission.Contains("Priced"))
            {

                alltableprice.Append("<td>");
                alltableprice.Append(dt.Rows[0]["Priced"]);
                alltableprice.Append("</td>");

            }
            if (Spermission.Contains("Pricee"))
            {

                alltableprice.Append("<td>");
                alltableprice.Append(dt.Rows[0]["Pricee"]);
                alltableprice.Append("</td>");

            }

            if (Spermission.Contains("Balance"))
            {
                alltableprice.Append("<td>");
                alltableprice.Append(dt.Rows[0]["Balance"]);
                alltableprice.Append("</td>");
            }

            alltableprice.Append("</tr>");
            alltableprice.Append("</table>");
            return s.ToString() + "❤" + alltableprice.ToString() + "❤" + property;
        }
        #region 图片处理
        /// <summary>
        /// 图片存到阿里云--主货号表
        /// </summary>
        string rootName = "images/";//正式用
        aliyun.ControlAliyun CAliyun = new aliyun.ControlAliyun();
        public bool UploadScodePic()
        {

            bool flag = false;
            try
            {
                int i = 1;
                DataTable dtpath = new DataTable();
                DataTable dtcount = new DataTable();
                string srcs = Request.QueryString["Arrysrc"];
                string scode = Request.QueryString["scode"];
                string[] arrysrc;
                string filename = "";
                arrysrc = srcs.Split(',');
                string Picurl;
                string path = CAliyun.GetObjectlistScode(scode.Replace("*", "_").Replace(".", "-"));//得到货号文件夹路径
                string pathsql = @"select * from SoCalProduct where Scode='" + scode + "'";
                dtpath = DbHelperSQL.Query(pathsql).Tables[0];
                string newpath = rootName + dtpath.Rows[0]["Cat"] + "/" + dtpath.Rows[0]["Cat2"] + "/" + dtpath.Rows[0]["Style"].ToString() + "/" + dtpath.Rows[0]["Scode"].ToString() + "/";
                dtcount = DbHelperSQL.Query(@"select count(*) from scodepic where scode='" + scode + "' ").Tables[0];
                i = Convert.ToInt32(dtcount.Rows[0][0]) + 1;//判断数据中对应货号存放图片的个数
                //string num = dtcount.Rows[dtcount.Rows.Count - 1]["scodePicSrc"].ToString();
                //i = Convert.ToInt32(num.Substring(num.LastIndexOf('/') + 1).Split('_')[0]) + 1;//判断数据中对应货号存放图片的个数
                if (CAliyun.ScodeExist(scode.Replace("*", "_").Replace(".", "-")))//判断该货号文件夹是否存在
                {
                    foreach (var temp in arrysrc)
                    {
                        filename = scode.Replace(".", "-") + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[temp.Split('.').Length - 1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.pbxluxury.com/" + path + i + "_" + filename;

                        string sql = @"insert into scodepic values ('" + scode + "','" + Picurl.Replace("*", "_").Replace("*", "_").Replace("*", "_") + "','1','1','1','" + userInfo.User.Id + "','1','1','1','1','" + getFile.Substring(0, getFile.LastIndexOf('.')) + "')";
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
                    newpath = newpath.Replace("*", "_").Replace("*", "_").Replace("*", "_").Replace(".", "-");
                    CAliyun.CreateEmptyFolder(newpath);//创建款号文件夹
                    foreach (var temp in arrysrc)
                    {
                        filename = scode.Replace(".", "-") + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[temp.Split('.').Length - 1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.pbxluxury.com/" + newpath + i + "_" + filename;
                        string sql = @"insert into scodepic values ('" + scode + "','" + Picurl.Replace("*", "_").Replace("*", "_").Replace("*", "_") + "','1','1','1','" + userInfo.User.Id + "','1','1','1','1','" + getFile.Substring(0, getFile.LastIndexOf('.')) + "')";
                        int n = DbHelperSQL.ExecuteSql(sql);
                        if (n != -1)
                        {
                            PutObject(getFile, newpath + i + "_" + filename);//存入新上传图片

                        }
                        i++;
                    }

                }
                flag = true;
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string updatetime = @"update product set Def1='" + date + "',Def9='0' where Scode='" + scode + "'";
                DbHelperSQL.ExecuteSql(updatetime);

            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 图片存到阿里云--主款号表
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="objkey"></param>
        /// 
        const string accessId = "X9foTnzzxHCk6gK7";
        const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
        const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
        string bucketName = "best-bms";
        public void PutObject(string filename, string objkey)
        {
            OssClient client = new OssClient(endpoint, accessId, accessKey);

            //定义文件流
            var objStream = new System.IO.FileStream(Server.MapPath("~/UploadImage/") + filename, System.IO.FileMode.OpenOrCreate);
            //定义 object 描述
            var objMetadata = new ObjectMetadata();
            //执行 put 请求，并且返回对象的MD5摘要。
            var putResult = client.PutObject(bucketName, objkey.Replace("*", "_").Replace("*", "_").Replace("*", "_"), objStream, objMetadata);
        }
        #endregion

        /// <summary>
        /// 上传excel文件到服务器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UploadExcel()
        {
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                // Response.Output.WriteLine("<script language='javascript'>parent.UploadCallback('fase|请选择文件！')</script>");
                return "请选择文件！";

            }
            if (!Request.Files[0].FileName.Contains(".xls"))
            {
                // Response.Output.WriteLine("<script language='javascript'>parent.UploadCallback('fase|请选择文件！')</script>");
                return "请选择excel文件！";

            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                //Response.Output.WriteLine("<script language='javascript'>parent.UploadCallback('fase|上传文件失败！')</script>");
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
                    if (scb.SelectSoCalProductByTimeAndScodeAndVencode(dtTable.Rows[i]["Scode"].ToString(), dtTable.Rows[i]["Lastgrnd"].ToString(), dtTable.Rows[i]["Vencode"].ToString())) //如果已有重复
                    {
                        scb.UpdateSoCalProductByFile(dtTable.Rows[i]["Scode"].ToString(), DateTime.Now.ToShortDateString(), dtTable.Rows[i]["Vencode"].ToString(), dtTable.Rows[i]["Balance"].ToString());
                        dtIsInTable.Add(dtTable.Rows[i]);
                    }
                }
                foreach (DataRow r in dtIsInTable)
                {
                    dtTable.Rows.Remove(r);
                }
                if (scb.InsertSoCalProduct(dtTable))
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    return "<script>alert('上传成功！');</script>";
                }
                else
                {
                    return "<script>alert('上传失败！');</script>";
                }
            }


        }
        #endregion



        #region 合作店铺
        public static int CheckProductSaveMenuId = 0;
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SoCalCollaboration()
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
            CheckProductSaveMenuId = menuId;
            #region 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            return View();
            #endregion
        }
        /// <summary>
        /// 页面第一次加载，显示当前客户拥有的店铺
        /// </summary>
        /// <returns></returns>
        public string CollaborationShopShow()
        {
            shopbll sbl = new shopbll();
            DataTable dt = sbl.SelectCollaborationShop(userInfo.User.Id.ToString());
            StringBuilder alltable = new StringBuilder();
            alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'>");
            alltable.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                alltable.Append("<th>");
                if (dt.Columns[i].ColumnName == "row_nb")
                    alltable.Append("序号");
                if (dt.Columns[i].ColumnName == "Id")
                    alltable.Append("店铺编号");
                if (dt.Columns[i].ColumnName == "ShopName")
                    alltable.Append("店铺名称");
                if (dt.Columns[i].ColumnName == "ShopState")
                    alltable.Append("店铺状态");
                if (dt.Columns[i].ColumnName == "Def1")
                    alltable.Append("联系电话");
                if (dt.Columns[i].ColumnName == "ShoptypeName")
                    alltable.Append("店铺类型");
                if (dt.Columns[i].ColumnName == "userRealName")
                    alltable.Append("店铺负责人");
                alltable.Append("</th>");
            }
            alltable.Append("<th>操作</th>");
            alltable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                alltable.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName == "ShopState")
                    {
                        alltable.Append("<td>");
                        if (dt.Rows[i]["ShopState"].ToString() == "1")
                        {
                            alltable.Append("已启用");
                        }
                        else
                        {
                            alltable.Append("未启用");

                        }
                        alltable.Append("</td>");
                    }
                    else
                    {
                        alltable.Append("<td>");
                        alltable.Append(dt.Rows[i][dt.Columns[j].ColumnName]);
                        alltable.Append("</td>");
                    }
                }

                alltable.Append("<td><a href='#' onclick='Instruction(\"" + dt.Rows[i]["Id"] + "\",\"" + dt.Rows[i]["ShopName"] + "\")'>仓库</a></td>");
                alltable.Append("</tr>");
            }
            alltable.Append("</table>");
            return alltable.ToString();
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <returns></returns>
        public static string ShopName;
        public ActionResult SoCalWareHouse()
        {
            string ShopName = Request.QueryString["shopname"].ToString();
            ViewData["shopname"] = ShopName;
            ViewData["MenuId"] = Request.QueryString["MenuId"].ToString();
            string shopid = Request.QueryString["shopid"].ToString();
            ViewData["DrpBrand"] = php.GetBrandDDlist(userInfo.User.Id);
            ViewData["DropType"] = php.GetTypeDDlist(userInfo.User.Id);
            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["ShopId"] = shopid;
            return View();
        }
        /// <summary>
        /// 跳转仓库
        /// </summary>
        /// <returns></returns>
        public string SoCalWareHouseShow()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = CheckProductSaveMenuId;
            int MenuId = int.Parse(Request.Form["MenuId"].ToString());
            PublicHelpController ph = new PublicHelpController();
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("SoCaloutsideProduct");
            StringBuilder alltable = new StringBuilder();
            #region    款号栏 搜索条件
            if (s.Contains("Style"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>款号</span><input type='text' id='StyleTop'  class='spanPropertyValue'/></span>");
            }
            if (s.Contains("Pricea"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最小价格</span><input type='text' id='priceMin' class='spanPropertyValue'></span>");

                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最大价格</span><input type='text' id='priceMax' class='spanPropertyValue'></span>");
            }
            if (s.Contains("Cat2"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>类别</span><select id='Cat2' class='spanPropertyValue'>");
                alltable.Append(php.GetTypeDDlist(userInfo.User.Id));
                alltable.Append("</select></span>");
            }
            if (s.Contains("Balance1"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最小库存</span><input type='text' class='spanPropertyValue' id='BalanceMin'></span>");
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最大库存</span><input type='text' class='spanPropertyValue' id='BalanceMax'></span>");

            }
            if (s.Contains("Cat"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>品牌</span><select id='Cat' class='spanPropertyValue'>");
                alltable.Append(php.GetBrandDDlist(userInfo.User.Id));
                alltable.Append("</select></span>");
            }
            if (s.Contains("Imagefile"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>图片状态</span><select id='Imagefile' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>有</option><option value='0'>无</option></select></span>");
            }

            if (ph.isFunPermisson(roleId, menuId, funName.SalesState))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>销售状态</span><select id='salesState' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>已开放</option><option value='0'>未开放</option><option value='2'>已选货</option></select></span></br>");
                alltable.Append("<input type='button' class='spanPropertySearch'  value='开放销售' onclick='OpenSale()'/>");
                alltable.Append("<input type='button' class='spanPropertySearch'   value='取消销售' onclick='CancelSale()'/>");
                alltable.Append("<input type='button' class='spanPropertySearch'  value='批量删除' onclick='DeleteAll()'/>&nbsp;&nbsp;");
            }
            alltable.Append("<input type='button' value='查询' class='spanPropertySearch' onclick='SoCalSearchStyle()'/> ");
            #endregion
            #region  货号栏  搜索条件
            StringBuilder alltableDown = new StringBuilder();
            if (s.Contains("Style"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>款号</span><input type='text' id='Style'  class='spanPropertyValue'/></span>");
            }
            if (s.Contains("Scode"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>货号</span><input type='text' id='ScodeDown'  class='spanPropertyValue'/></span>");
            }
            if (s.Contains("Cat"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>品牌</span><select id='CatDown' class='spanPropertyValue'>");
                alltableDown.Append(php.GetBrandDDlist(userInfo.User.Id));
                alltableDown.Append("</select></span>");
            }

            if (s.Contains("Cat2"))
            {

                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>类别</span><select id='Cat2Down' class='spanPropertyValue'>");
                alltableDown.Append(php.GetTypeDDlist(userInfo.User.Id));
                alltableDown.Append("</select></span>");
            }
            if (s.Contains("Pricea"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最小价格</span><input type='text' id='PriceMinDown' class='spanPropertyValue'></span>");
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最大价格</span><input type='text' id='PriceMaxDown' class='spanPropertyValue'></span>");
            }
            if (s.Contains("Balance1"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最小库存</span><input type='text' class='spanPropertyValue' id='StcokMinDown'></span>");
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最大库存</span><input type='text' class='spanPropertyValue' id='StcokMaxDown'></span>");
            }
            if (s.Contains("Lastgrnd"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>时间</span><input type='text' class='spanPropertyValue' id='MinTimeDown' onfocus=\"WdatePicker({isShowWeek:true,onpicked:function() {$dp.$(\'d122_1\').value=$dp.cal.getP(\'W\',\'W\');$dp.$(\'d122_2\').value=$dp.cal.getP(\'W\',\'WW\');}})\" /></span>");
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>时间</span><input type='text' class='spanPropertyValue' id='MaxTimeDown' onfocus=\"WdatePicker({isShowWeek:true,onpicked:function() {$dp.$(\'d122_1\').value=$dp.cal.getP(\'W\',\'W\');$dp.$(\'d122_2\').value=$dp.cal.getP(\'W\',\'WW\');}})\" /></span>");
            }
            if (s.Contains("Imagefile"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>图片状态</span><select id='Imagefiledown' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>有</option><option value='0'>无</option></select></span>");
            }
            alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>销售状态</span><select id='salesStatedown' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>已开放</option><option value='0'>未开放</option><option value='2'>已选货</option><option value='3'>已改价格</option><option value='5'>未改价格</option><option value='4'>已改库存</option><option value='6'>未改库存</option></select></span>");
            alltableDown.Append("<input type='button' class='spanPropertySearch' value='查询' onclick='ScCalSearchScode()'/>");
            #endregion
            return alltable.ToString() + "❤" + alltableDown.ToString();
        }
        /// <summary>
        /// 仓库款号查询   
        /// </summary>
        /// <returns></returns>
        public string SoCalSearch()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = CheckProductSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            string[] str = new string[20];
            str[0] = Request.Form["style"] == null ? "" : Request.Form["style"].ToString().Trim();
            str[1] = Request.Form["priceMin"] == null ? "" : Request.Form["priceMin"].ToString().Trim();
            str[2] = Request.Form["priceMax"] == null ? "" : Request.Form["priceMax"].ToString().Trim();
            str[3] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString().Trim();
            str[4] = Request.Form["BalanceMin"] == null ? "" : Request.Form["BalanceMin"].ToString().Trim();
            str[5] = Request.Form["BalanceMax"] == null ? "" : Request.Form["BalanceMax"].ToString().Trim();
            str[6] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString().Trim();
            str[7] = Request.Form["Imagefile"] == null ? "" : Request.Form["Imagefile"].ToString().Trim();
            str[8] = Request.Form["salesState"] == null ? "" : Request.Form["salesState"].ToString().Trim();
            str[9] = Request.Form["ShopId"].ToString();
            bool isPage = Request.Form["IsPage"] == null ? false : bool.Parse(Request.Form["IsPage"].ToString());//是否为翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());//判断操作
            int page = Request.Form["Page"] == null ? 10 : int.Parse(Request.Form["Page"].ToString());//当前页码显示多少条数据 默认10
            int ThisPage = Request.Form["ThisPage"] == null ? 0 : int.Parse(Request.Form["ThisPage"].ToString())-1;
            shopbll sbl = new shopbll();
            SoCalProductBll scb = new SoCalProductBll();
            int Count = scb.GetSoCalWarehouseByStyleCount(str);//得到货物个数
            int PageSum = Count % page > 0 ? Count / page + 1 : Count / page;//总页数
            int PageIndex = php.PageIndex(ThisPage, PageSum, Index);//得到当前需要的页码
            StringBuilder alltable = new StringBuilder();
            DataTable dt = scb.GetSoCalWarehouseByStyle(str, PageIndex * page, PageIndex * page + page);
            alltable.Append("<table id='Warehouse' class='mytable' rules='all'>");
            alltable.Append("<tr>");
            alltable.Append("<th><input type='checkbox' id='Checkall' onchange='CheckAll()'/></th>");
            dt.Columns.Remove("Pricea");
            dt.Columns.Remove("Cat2");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                #region   行标题
                alltable.Append("<th>");
                if (dt.Columns[i].ColumnName == "nid")
                    alltable.Append("序号");
                if (dt.Columns[i].ColumnName == "style")
                    alltable.Append("款号");
                if (dt.Columns[i].ColumnName == "Cat")
                    alltable.Append("品牌");
                if (dt.Columns[i].ColumnName == "TypeName")
                    alltable.Append("类别");
                if (dt.Columns[i].ColumnName == "Rolevel")
                    alltable.Append("预警库存");
                if (dt.Columns[i].ColumnName == "Balance")
                    alltable.Append("店铺库存");
                if (dt.Columns[i].ColumnName == "stylePicSrc")
                    alltable.Append("缩略图");
                if (dt.Columns[i].ColumnName == "ShowState")
                    alltable.Append("销售状态");
                alltable.Append("</th>");
                #endregion
            }
            alltable.Append("<th>操作</th>");
            alltable.Append("</tr>");
            if (dt.Rows.Count > 0)
            {
                #region  内容
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    alltable.Append("<tr>");
                    alltable.Append("<td><input type='checkbox' id='Choose' styleName='" + dt.Rows[i]["Style"] + "' shopid='" + str[9] + "'/></td>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "ShowState")
                        {
                            if (dt.Rows[i]["ShowState"].ToString().Equals("0"))
                            {
                                alltable.Append("<td style='color:Green;'>");
                                alltable.Append("未开放");
                                alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("1"))
                            {
                                alltable.Append("<td style='color:red;'>");
                                alltable.Append("已开放");
                                alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("2"))
                            {
                                alltable.Append("<td style='color:blue;'>");
                                alltable.Append("已选货");
                                alltable.Append("</td>");
                            }
                        }
                        else if (dt.Columns[j].ColumnName == "stylePicSrc")
                        {
                            alltable.Append("<td>");
                            if (dt.Rows[i][dt.Columns[j].ColumnName].ToString() != "")
                            {
                                alltable.Append("<img src='" + dt.Rows[i][dt.Columns[j].ColumnName] + "'style='width:50px;height:50px;' />");
                            }
                            alltable.Append("</td>");
                        }
                        else
                        {
                            alltable.Append("<td>");
                            alltable.Append(dt.Rows[i][dt.Columns[j].ColumnName]);
                            alltable.Append("</td>");
                        }
                    }
                    alltable.Append("<td>");
                    if (ph.isFunPermisson(roleId, menuId, funName.LookPice))
                    {
                        alltable.Append("<a href='#' onclick='LookAttr(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\")'>查看图片</a>&nbsp;");
                    }
                    if (ph.isFunPermisson(roleId, menuId, funName.LookShop))
                    {
                        alltable.Append("<a href='#' onclick='LookShop(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\")'>查看商品</a>&nbsp;");
                    }
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        alltable.Append("<a href='#' onclick='Delete(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\")'>删除</a>&nbsp;");
                    }
                    alltable.Append("</td>");
                    alltable.Append("</tr>");
                }
                #endregion
            }
            alltable.Append("</table>");
            int thisPage = PageIndex + 1;
            return alltable.ToString() + "❤" + Count + "❤" + PageSum + "❤" + thisPage;
        }  
        /// <summary>
        /// 仓库货号查询
        /// </summary>
        /// <returns></returns>
        public string ScCalSearchScode()
        {
            #region  查询条件
            string[] str = new string[20];
            str[1] = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();//货号
            str[2] = Request.Form["style"] == null ? "" : Request.Form["style"].ToString();//款号
            str[3] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();//品牌
            str[5] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();//类别
            str[6] = Request.Form["BalanceMin"] == null ? "" : Request.Form["BalanceMin"].ToString();//最小库存
            str[7] = Request.Form["BalanceMax"] == null ? "" : Request.Form["BalanceMax"].ToString();//最大库存
            str[8] = Request.Form["priceMin"] == null ? "" : Request.Form["priceMin"].ToString();//最小价格
            str[9] = Request.Form["priceMax"] == null ? "" : Request.Form["priceMax"].ToString();//最大价格
            str[10] = Request.Form["minTime"] == null ? "" : Request.Form["minTime"].ToString();//最小时间
            str[11] = Request.Form["maxTime"] == null ? "" : Request.Form["maxTime"].ToString();//最大时间
            str[12] = Request.Form["ShopId"].ToString(); ;//店铺编号
            str[13] = Request.Form["salesState"] == null ? "" : Request["salesState"].ToString();//销售状态
            str[14] = Request.Form["Imagefiledown"] == null ? "" : Request.Form["Imagefiledown"].ToString();
            bool isPage = Request.Form["IsPage"] == null ? false : bool.Parse(Request.Form["IsPage"].ToString());//是否为翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());//判断操作
            int page = Request.Form["Page"] == null ? 10 : int.Parse(Request.Form["Page"].ToString());//当前页码显示多少条数据 默认10条
            #endregion
            SoCalProductBll scb = new SoCalProductBll();
            shopbll sbl = new shopbll();
            int Count = scb.GetSoCalWarehouseBySocdeCount(str);//数据总个数
            int PageSum = Count % page > 0 ? Count / page + 1 : Count / page;//总页数
            int ThisPage = Request.Form["ThisPage"] == null ? 0 : int.Parse(Request.Form["ThisPage"].ToString())-1;//当前页码
            int PageIndex = php.PageIndex(ThisPage, PageSum, Index);//当前需要分页的页码
            int menuId = CheckProductSaveMenuId;
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            PublicHelpController ph = new PublicHelpController();
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("outsideProduct");
            StringBuilder Alltable = new StringBuilder();
            DataTable dt = scb.GetSoCalWarehouseBySocde(str, PageIndex * page, PageIndex * page + page);
            Alltable.Append("<table id='Warehouse' class='mytable' rules='all'>");
            Alltable.Append("<tr>");
            Alltable.Append("<th>序号</th>");
            #region 表头排序
            if (s.Contains("Imagefile"))
            {
                string temp;
                temp = s[2];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Imagefile")
                    {
                        s[i] = temp;
                        s[2] = "Imagefile";
                    }
                }
            }
            if (s.Contains("Style"))
            {
                string temp;
                temp = s[3];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Style")
                    {
                        s[i] = temp;
                        s[3] = "Style";
                    }
                }
            }
            s[2] = "Style";
            s[3] = "Imagefile";
            #endregion
            for (int i = 0; i < s.Length; i++)
            {
                #region//判断头
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
                        Alltable.Append("供货价");
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
                        Alltable.Append("店铺库存");
                    if (s[i] == "Balance2")
                        Alltable.Append("销售库存");
                    if (s[i] == "Balance3")
                        Alltable.Append("退货库存");
                    if (s[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (s[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (s[i] == "UserId")
                        Alltable.Append("操作人");
                    if (s[i] == "ShowState")
                        Alltable.Append("销售状态");
                    if (s[i] == "Def2")
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("退货库存");
                    if (s[i] == "Def5")
                        Alltable.Append("操作人");
                    if (s[i] == "Def6")
                        Alltable.Append("操作时间");
                    if (s[i] == "Def7")
                        Alltable.Append("价格状态");
                    if (s[i] == "Def8")
                        Alltable.Append("库存状态");
                    if (s[i] == "Def9")
                        Alltable.Append("默认9");
                    if (s[i] == "Def10")
                        Alltable.Append("默认10");
                    if (s[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
                #endregion
            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            string RoleType = sbl.GetUserTypeByPersonaId(roleId.ToString());
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string showState = sbl.GetShowStateByScodeAndLoc(dt.Rows[i]["Scode"].ToString(), str[12]);
                #region  内容
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["row_nb"] + "</td>");
                for (int j = 0; j < s.Length; j++)
                {

                    if (allTableName.Contains(s[j]))
                    {
                        if (s[j] == "Def5")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(sbl.GetUserRealNameByUserId(dt.Rows[i]["Def5"].ToString()));
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "ShowState")
                        {
                            if (dt.Rows[i]["ShowState"].ToString().Equals("0"))
                            {
                                Alltable.Append("<td style='color:green;'>");
                                Alltable.Append("未开放");
                                Alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("1"))
                            {
                                Alltable.Append("<td style='color:red;'>");
                                Alltable.Append("已开放");
                                Alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("2"))
                            {
                                Alltable.Append("<td style='color:blue;'>");
                                Alltable.Append("已选货");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Imagefile")
                        {
                            Alltable.Append("<td>");
                            if (dt.Rows[i]["Imagefile"].ToString() != "")
                            {
                                Alltable.Append("<img src='" + dt.Rows[i]["Imagefile"] + "' style='height:50px;width:50px;'/>");
                            }
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Loc")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(ShopName);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Def7")
                        {
                            if (dt.Rows[i]["Def7"].ToString() == "1")
                            {
                                Alltable.Append("<td style='color:green;'>");
                                Alltable.Append("已改");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td style='color:red;'>");
                                Alltable.Append("未改");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Def8")
                        {
                            if (dt.Rows[i]["Def8"].ToString() == "1")
                            {
                                Alltable.Append("<td style='color:green;'>");
                                Alltable.Append("已改");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td style='color:red;'>");
                                Alltable.Append("未改");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Priceb")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Pricec")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Priced")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Pricee")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Balance")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
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
                if (ph.isFunPermisson(roleId, menuId, funName.LookAttr))
                {
                    Alltable.Append("<a href='#' onclick='LookAtrrbutt(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Vencode"].ToString().Trim() + "\")'>属性</a>&nbsp;");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    Alltable.Append("<a href='#' onclick='DeleteScode(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\")'>删除</a>&nbsp;&nbsp;");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.SubtractBalance))
                {
                    Alltable.Append("<a href='#' onclick='Subtract(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Vencode"].ToString().Trim() + "\")'>库存</a>&nbsp;");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.UpdatePrice))
                {
                    Alltable.Append("<a href='#' onclick='UpdatePricea(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\",\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Vencode"].ToString().Trim() + "\")'>价格</a>&nbsp;");
                }
                Alltable.Append("</td>");
                Alltable.Append("</tr>");
                #endregion
            }
            Alltable.Append("</table>");
            int thisPage = PageIndex + 1;
            return Alltable.ToString() + "❤" + Count + "❤" + PageSum + "❤" + thisPage;
        }
        /// <summary>
        /// 查看属性
        /// </summary>
        /// <returns></returns>
        public string GetShopProductByScodeAndShopIdVencode()
        {
            shopbll sbl = new shopbll();
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            string shopid = Request.Form["ShopId"].ToString();
            SoCalProductBll scb = new SoCalProductBll();
            DataTable dt = scb.GetSoCaloutsideProductScodeAndVencode(scode, vencode);
            StringBuilder alltableprice = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            int menuId = CheckProductSaveMenuId;
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                string s = string.Format(@"<table id='tabProperty'>
            <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><img src='{11}' style='width:120px;' /></td></tr>
            <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE'  type='text' value='{2}' /></td></tr>
            <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                <td>季节：<input type='text' value='{4}' disabled='disabled' /></td></tr>
            <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
            <tr><td>预警库存：<input id='txtRolevelE' disabled='disabled' type='text' value='{9}' /></td><td>类别：<input type='text' disabled='disabled' id='hidId'' value='{5}' /></td></tr>
        </table>", dt.Rows[0]["Scode"].ToString()
                                 , dt.Rows[0]["Descript"].ToString()
                                 , dt.Rows[0]["Cdescript"].ToString()
                                 , dt.Rows[0]["Cat"].ToString()
                                 , dt.Rows[0]["Cat1"].ToString()
                                 , dt.Rows[0]["Cat2"].ToString()
                                 , dt.Rows[0]["Clolor"].ToString()
                                 , dt.Rows[0]["Style"].ToString()
                                 , dt.Rows[0]["Size"].ToString()
                                 , dt.Rows[0]["Rolevel"].ToString()
                                 , dt.Rows[0]["Id"].ToString()
                                 , dt.Rows[0]["Imagefile"].ToString()

                            );
                string RoleType = sbl.GetUserTypeByPersonaId(roleId.ToString());
                string showState = sbl.GetShowStateByScodeAndLoc(scode, Request.Form["ShopId"].ToString());
                string[] Spermission = ph.getFiledPermisson(roleId, menuId, funName.selectName);
                alltableprice.Append("<table class='mytable' style='font-size:12px;min-width:300px;'>");
                alltableprice.Append("<tr>");
                alltableprice.Append("<th>来源</th>");
                if (Spermission.Contains("Pricea"))
                {
                    alltableprice.Append("<th>吊牌价</th>");
                }
                if (Spermission.Contains("Priceb"))
                {
                    alltableprice.Append("<th>零售价</th>");
                }
                if (Spermission.Contains("Pricec"))
                {
                    alltableprice.Append("<th>供货价</th>");
                }
                if (Spermission.Contains("Priced"))
                {
                    alltableprice.Append("<th>批发价</th>");
                }
                if (Spermission.Contains("Pricee"))
                {
                    alltableprice.Append("<th>成本价</th>");
                }
                alltableprice.Append("<th>库存</th>");
                alltableprice.Append("</tr>");
                alltableprice.Append("<tr>");
                alltableprice.Append("<td>" + dt.Rows[0]["Vencode"] + "</td>");
                if (Spermission.Contains("Pricea"))
                {

                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Pricea"]);
                    alltableprice.Append("</td>");

                }
                if (Spermission.Contains("Priceb"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Priceb"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (Spermission.Contains("Pricec"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Pricec"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (Spermission.Contains("Priced"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Priced"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (Spermission.Contains("Pricee"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Pricee"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (showState == "1" || RoleType != "Customer")
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Balance"]);
                    alltableprice.Append("</td>");
                }
                else
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append("*****");
                    alltableprice.Append("</td>");
                }
                alltableprice.Append("</tr>");
                alltableprice.Append("</table>");
                StringBuilder CaoZuo = new StringBuilder();
                CaoZuo.Append("<input type='button' name='Close' value='关 闭' onclick='Close()' style='margin-left: 50px' />");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    CaoZuo.Append("<input type='button' name='Close' value='保存' onclick='SelectMainPic()' style='margin-left: 50px' />");
                }
                return s.ToString() + "❤" + alltableprice.ToString() + "❤" + CaoZuo;
            }
            else
            {
                string s = string.Format(@"<table id='tabProperty'>
            <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><img src='{11}' style='width:120px;' /></td></tr>
            <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' disabled='disabled' type='text' value='{2}' /></td></tr>
            <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                <td>季节：<input type='text' value='{4}' disabled='disabled' /></td></tr>
            <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
            <tr><td>预警库存：<input id='txtRolevelE' disabled='disabled' type='text' value='{9}' /></td><td>类别：<input type='text' disabled='disabled' id='hidId'' value='{5}' /></td></tr>
        </table>", dt.Rows[0]["Scode"].ToString()
                                , dt.Rows[0]["Descript"].ToString()
                                , dt.Rows[0]["Cdescript"].ToString()
                                , dt.Rows[0]["Cat"].ToString()
                                , dt.Rows[0]["Cat1"].ToString()
                                , dt.Rows[0]["Cat2"].ToString()
                                , dt.Rows[0]["Clolor"].ToString()
                                , dt.Rows[0]["Style"].ToString()
                                , dt.Rows[0]["Size"].ToString()
                                , dt.Rows[0]["Rolevel"].ToString()
                                , dt.Rows[0]["Id"].ToString()
                                , dt.Rows[0]["Imagefile"].ToString()

                           );

                string[] Spermission = ph.getFiledPermisson(roleId, menuId, funName.selectName);
                string RoleType = sbl.GetUserTypeByPersonaId(roleId.ToString());
                string showState = sbl.GetShowStateByScodeAndLoc(scode, shopid);
                alltableprice.Append("<table class='mytable' style='font-size:12px;min-width:300px;'>");
                alltableprice.Append("<tr>");
                alltableprice.Append("<th>来源</th>");
                if (Spermission.Contains("Pricea"))
                {
                    alltableprice.Append("<th>吊牌价</th>");
                }
                if (Spermission.Contains("Priceb"))
                {
                    alltableprice.Append("<th>零售价</th>");
                }
                if (Spermission.Contains("Pricec"))
                {
                    alltableprice.Append("<th>供货价</th>");
                }
                if (Spermission.Contains("Priced"))
                {
                    alltableprice.Append("<th>批发价</th>");
                }
                if (Spermission.Contains("Pricee"))
                {
                    alltableprice.Append("<th>成本价</th>");
                }
                alltableprice.Append("<th>库存</th>");
                alltableprice.Append("</tr>");
                alltableprice.Append("<tr>");
                alltableprice.Append("<td>" + dt.Rows[0]["Vencode"] + "</td>");
                if (Spermission.Contains("Pricea"))
                {

                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Pricea"]);
                    alltableprice.Append("</td>");

                }
                if (Spermission.Contains("Priceb"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Priceb"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (Spermission.Contains("Pricec"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Pricec"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (Spermission.Contains("Priced"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Priced"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (Spermission.Contains("Pricee"))
                {
                    if (showState == "1" || RoleType != "Customer")
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append(dt.Rows[0]["Pricee"]);
                        alltableprice.Append("</td>");
                    }
                    else
                    {
                        alltableprice.Append("<td>");
                        alltableprice.Append("*****");
                        alltableprice.Append("</td>");
                    }
                }
                if (showState == "1" || RoleType != "Customer")
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Balance"]);
                    alltableprice.Append("</td>");
                }
                else
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append("*****");
                    alltableprice.Append("</td>");
                }
                alltableprice.Append("</tr>");
                alltableprice.Append("</table>");
                StringBuilder CaoZuo = new StringBuilder();
                CaoZuo.Append("<input type='button' name='Close' value='关 闭' onclick='Close()' style='margin-left: 50px' />");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    CaoZuo.Append("<input type='button' name='Close' value='保存' onclick='SelectMainPic()' style='margin-left: 50px' />");
                }
                return s.ToString() + "❤" + alltableprice.ToString() + "❤" + CaoZuo;
            }
        }

        #region 图片处理
        ///  <summary>
        /// 显示配图
        /// </summary>
        public string ShowPeiPic()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string style = Request.QueryString["style"];
            string sql = @"select * from stylepic where style='" + style + "' and Left(Def2,1)='2' Order by RIGHT(Def2,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        /// <summary>
        /// 显示主图-款号表
        /// </summary>
        /// <returns></returns>
        public string ShowMainPicStyle()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string style = Request.QueryString["style"];
            string sql = @"select * from stylepic where style='" + style + "' and Left(Def1,1)='2' Order by RIGHT(Def1,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        /// <summary>
        /// 显示主图-货号表
        /// </summary>
        /// <returns></returns>
        public string ShowMainPic()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string scode = Request.QueryString["scode"];
            string sql = @"select * from scodepic where scode='" + scode + "' and Left(Def1,1)='2' Order by RIGHT(Def1,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["scodePicSrc"].ToString() + "@10q." + dt1.Rows[i]["scodePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        ///  <summary>
        /// 显示细节图     
        /// </summary>
        public string ShowDetailPic()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string style = Request.QueryString["style"];
            string sql = @"select * from stylepic where style='" + style + "' and Left(Def3,1)='2'  Order by RIGHT(Def3,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];

            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        #endregion

        /// <summary>
        /// 获取拉列表商品类别显示相应属性  ---于查看属性相关
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTypeNo(string scode, string typeno)
        {
            ProductBll pro = new ProductBll();
            string s = string.Empty;
            DataTable dt = new DataTable();
            dt = pro.GetPropertyByTypeNo(scode, typeno);
            if (dt.Rows.Count > 0)
            {
                s += "<table style='text-align:right;'>";//--hidUpdateType判断是Insert 还是Update
                if (dt.Rows.Count % 3 == 0)
                {
                    s += "<tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += "<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\" title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>";
                        if ((i + 1) % 3 == 0)
                            s += "</tr>";
                    }
                }
                else if (dt.Rows.Count % 3 == 1)
                {
                    s += "<tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += "<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\"  title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>";
                        if ((i + 1) % 3 == 0)
                            s += "</tr>";
                    }
                    s += "<td style='width:237px'></td><td style='width:238px'></td></tr>";
                }
                else
                {
                    s += "<tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += "<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\"  title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>";
                        if ((i + 1) % 3 == 0)
                            s += "</tr>";
                    }
                    s += "<td style='width:238px'></td></tr>";
                }
                s += "</table>";
            }
            return s;
        }
        /// <summary>
        /// 修改当前数据源 当前店铺 对应货号的 库存
        /// </summary>
        /// <returns></returns>
        public string UpdateBalanceByAndShopIdVencode()
        {
            SoCalProductBll scb = new SoCalProductBll();
            int balance = Request.Form["Balance"] == null ? 0 : int.Parse(Request.Form["Balance"].ToString());
            string vencode = Request.Form["Vencod"] == null ? "" : Request.Form["Vencod"].ToString();
            string shopid = Request.Form["ShopId"].ToString();
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            int countBalance = scb.GetSoCalProductBalanceCountByScodeAndShopId(scode, shopid, vencode);//当前货号的库存
            string count = scb.GetSoCalProductSumBalanceByScode(scode).ToString();//当前货号的总库存
            int sum = scb.GetSocalSumBalanceByScode(scode);//当前货号已分配的库存  
            int SurplusBalance = int.Parse(count.ToString()) - sum;//剩余库存
            DataTable dt = scb.GetSoCalProductShowStateByScodeAndShopId(scode, shopid, vencode);//查询当前店铺该货号的状态 
            int thisBalance = int.Parse(dt.Rows[0]["Balance"].ToString());
            int KxGkc = sum - thisBalance;//出当前店铺当前货号 以外的库存
            if (balance + KxGkc > int.Parse(count.ToString()))
            {
                return "货号" + scode + "剩余库存不足:" + balance + "件";
            }
            else
            {
                scb.UpdateSoCalBalanceByScode("1", balance.ToString(), userInfo.User.Id.ToString(), DateTime.Now.ToString(), shopid, vencode, scode);//修改价格  时将状态改为以修改价格  
                if (dt.Rows.Count > 0)//不管有没有修改成功 都判断
                {
                    if (dt.Rows[0]["Def7"].ToString() == "1" && dt.Rows[0]["Def8"].ToString() == "1")//如果两个都已经修改 则修改为未开放
                    {
                        scb.SalesState(scode, "0", shopid, userInfo.User.Id.ToString(), DateTime.Now.ToString());
                        return "修改成功！";
                    }
                }
                else
                {
                    return "修改成功！";
                }
                return "修改成功！";
            }

        }
        /// <summary>
        /// 打开库存修改页面  //已完成
        /// </summary>
        /// <returns></returns>
        public string PageShowRk()
        {

            ProductStockBLL psb = new ProductStockBLL();
            ShopProductBll spb = new ShopProductBll();
            SoCalProductBll scb = new SoCalProductBll();
            string shopid = Request.Form["ShopId"].ToString();
            string ShopName = spb.GetShopNameByShopId(shopid);
            string Scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string venName = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string vencode = venName;
            int sum = scb.GetSocalSumBalanceByScode(Scode);//当前货号已分配的库存  
            string count = scb.GetSoCalProductSumBalanceByScode(Scode).ToString();//当前货号的总库存
            int SurplusBalance = int.Parse(count.ToString()) - sum;//剩余库存
            int countBalance = int.Parse(count.ToString());
            DataTable dt = scb.GetSoCalShopShowByVencodeAndScode(0, 10, Scode, vencode);
            int countShopBalance = scb.GetSoCalShopShowByVencodeAndScodeCount(Scode, vencode);//共多少条记录
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
            alltable.Append("</tr>");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ShopName"].ToString().Equals(ShopName))
                    {
                        alltable.Append("<tr style='background-color:gray;font-weight:bold;'>");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName != "Loc")
                            {
                                alltable.Append("<td>");
                                alltable.Append(dt.Rows[i][j]);
                                alltable.Append("</td>");
                            }

                        }
                        alltable.Append("</tr>");
                    }
                    else
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
                        alltable.Append("</tr>");
                    }

                }
            }
            alltable.Append("</table>");
            return Scode + "❤" + psb.GetSourname(venName).ToString() + "❤" + count + "❤" + alltable + "❤" + countShopBalance + "❤" + PageSum + "❤" + SurplusBalance;
        }
        /// <summary>
        /// 打开修改价格页面显示价格
        /// </summary>
        /// <returns></returns>
        public string GetPriceByScodeAndShopId()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string shopid = Request.Form["ShopId"].ToString();
            DataTable dt = scb.GetSoCalProductShowStateByScodeAndShopId(scode, shopid, vencode);//查询当前店铺该货号的状态
            string str = dt.Rows[0]["pricea"].ToString() + "❤" + dt.Rows[0]["priceb"].ToString() + "❤" + dt.Rows[0]["pricec"].ToString() + "❤" + dt.Rows[0]["priced"].ToString() + "❤" + dt.Rows[0]["pricee"].ToString();
            return str;
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <returns></returns>
        public string UpdateSoCalPriceByStyleAll()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string price = Request.Form["pricec"] == null ? "" : Request.Form["pricec"].ToString();
            string Updateprice = Request.Form["UpatePricec"] == null ? "" : Request.Form["UpatePricec"].ToString();
            string style = Request.Form["style"] == null ? "" : Request.Form["style"].ToString();
            string Scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            string vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string shopid = Request.Form["ShopId"].ToString();
            bool reslut = scb.UpdateSoCalPriceByStyleAll(style, shopid, price, Updateprice);//将同款好下 相同价格的也进行修改
            bool updatereslut = scb.UpdateSoCalPriceByScode("1", Updateprice, userInfo.User.Id.ToString(), DateTime.Now.ToString(), shopid, vencode, Scode);//改变当前货号的价格状态和价格
            if (updatereslut == true)
            {
                DataTable dt = scb.GetSoCalProductShowStateByScodeAndShopId(Scode, shopid, vencode);//查询当前店铺该货号的状态
                if (dt.Rows.Count > 0)
                {
                    string def7 = dt.Rows[0]["Def7"] == null ? "" : dt.Rows[0]["Def7"].ToString();
                    string def8 = dt.Rows[0]["Def8"] == null ? "" : dt.Rows[0]["Def8"].ToString();
                    if (def7 == "1" && def8 == "1")//如果两个都已经修改 则修改为未开放
                    {
                        scb.SalesState(Scode, "0", shopid, userInfo.User.Id.ToString(), DateTime.Now.ToString());
                    }
                }
                return "修改成功";
            }
            else
            {
                return "修改失败";
            }

        }
        /// <summary>
        ///开放销售
        /// </summary>
        /// <returns></returns>
        public string OpenSale()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string styles = Request.Form["styles"] == null ? "" : Request.Form["styles"].ToString();
            string[] str = styles.Split('❤');
            string Scode = "";
            string shopid = Request.Form["ShopId"].ToString();
            for (int i = 0; i < str.Length - 1; i++)
            {

                DataTable dt = scb.GetSoCalScodeByStyle(str[i].ToString(), shopid);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string state = "0";

                    if (dt.Rows[j]["ShowState"].ToString() == "0") //判断当前货号是否为未开放状态
                    {
                        state = "1";  //是 则改为开放
                        scb.SalesState(dt.Rows[j]["Scode"].ToString(), state, shopid, userInfo.User.Id.ToString(), DateTime.Now.ToString());

                    }
                    else
                    {
                        if (dt.Rows[j]["ShowState"].ToString() != "1")
                        {
                            Scode += dt.Rows[j]["Scode"].ToString() + "\n";
                            if (i == str.Length - 1)
                            {
                                Scode = "开放失败！";
                            }
                        }
                    }
                }

            }
            return "开放成功！";
        }
        /// <summary>
        /// 取消销售
        /// </summary>
        /// <returns></returns>
        public string CancelSale()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string styles = Request.Form["styles"] == null ? "" : Request.Form["styles"].ToString();
            string[] str = styles.Split('❤');
            string Scode = "";
            string shopid = Request.Form["ShopId"].ToString();
            for (int i = 0; i < str.Length - 1; i++)
            {

                DataTable dt = scb.GetSoCalScodeByStyle(str[i].ToString(), shopid);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string state = "0";

                    if (dt.Rows[j]["ShowState"].ToString() == "1") //判断当前货号是否为未开放状态
                    {
                        state = "0";  //是 则改为开放
                        scb.SalesState(dt.Rows[j]["Scode"].ToString(), state, shopid, userInfo.User.Id.ToString(), DateTime.Now.ToString());
                    }
                    else
                    {
                        if (dt.Rows[j]["ShowState"].ToString() != "0")
                        {
                            Scode += dt.Rows[j]["Scode"].ToString() + "\n";
                            if (i == str.Length - 1)
                            {
                                Scode = "开放失败！";
                            }
                        }
                    }
                }

            }
            return "已暂停销售！";
        }
        #endregion



        #region 下单
        /// <summary>
        /// 货号下单
        /// </summary>
        /// <returns></returns>
        public string PlaceAnOrderScode()
        {
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            string style = Request.Form["style"] == null ? "" : Request.Form["style"].ToString();
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            return "";
        }
        
        /// <summary>
        /// 获取商品类别并构造下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDDlist(string TypeNo)
        {
            Attributebll attrbll = new Attributebll();
            string s = string.Empty;
            DataTable dt = new DataTable();
            dt = attrbll.GetTypeDDlist(userInfo.User.Id.ToString());
            s += "<select id='ddlType' style='width:150px' onchange='ddl(this[selectedIndex].value)'>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i][1].ToString() == TypeNo)
                {
                    s += "<option value='" + dt.Rows[i][1].ToString() + "' selected='selected' >" + dt.Rows[i][0].ToString() + "</option>";
                }
                else
                {
                    s += "<option value='" + dt.Rows[i][1].ToString() + "'>" + dt.Rows[i][0].ToString() + "</option>";
                }


            }
            s += "</select>";
            return s;
        }
        /// <summary>
        /// 获取图片是否合格下拉框
        /// </summary>
        public string PicCheckDDlList(string state)
        {
            string s = "";
            if (state == "0")
            {
                s = "<select id='PicChecklist'><option value=''>请选择</option><option value='0' selected='selected' >未检查</option><option  value='1'>合格</option><option value='2'>不合格</option></select>";
            }
            else if (state == "1")
            {
                s = "<select id='PicChecklist'><option value=''>请选择</option><option value='0'>未检查</option><option  value='1' selected='selected' >合格</option><option value='2'>不合格</option></select>";
            }
            else if (state == "2")
            {
                s = "<select id='PicChecklist'><option value=''>请选择</option><option value='0'>未检查</option><option  value='1'>合格</option><option value='2' selected='selected' >不合格</option></select>";
            }
            else
            {
                s = "<select id='PicChecklist'><option value='' selected='selected' >请选择</option><option value='0'>未检查</option><option  value='1'>合格</option><option value='2'>不合格</option></select>";
            }

            return s;
        }
        /// <summary>
        /// 确认上传图片
        /// </summary>
        /// <returns></returns>
        public string uploadifyUpload()
        {
            var file = Request.Files["Filedata"];
            var style = Request.QueryString["style"];
            bool blean = false;
            string uploadPath = Server.MapPath("~/UploadImage/");
            string url = "/UploadImage/" + file.FileName;
            if (file != null)
            {
                file.SaveAs(uploadPath + file.FileName);
                return url;
            }
            return blean.ToString();
        }
        /// <summary>
        /// -----修改商品信息
        /// </summary>
        /// <returns></returns>
        public string UpdateSoCalProduct()
        {
            string Scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string Descript = Request.Form["Descript"] == null ? "" : Request.Form["Descript"].ToString();
            string shopid = Request.Form["ShopId"].ToString();
            SoCalProductBll scb = new SoCalProductBll();
            scb.UpdateSoCaloutsideProduct(Scode, Descript, shopid);
            return "";
        }
        
        public static int PlaceAnOrderMenuId = 0;
        public ActionResult PlaceAnOrder()
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
                PlaceAnOrderMenuId = menuId;
            }
            //查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["menuId"] = Savemenuid;
            ViewData["Brand"] = php.GetBrandDDlist(userInfo.User.Id);
            ViewData["Type"] = php.GetTypeDDlist(userInfo.User.Id);
            return View();
        }
        /// <summary>
        /// 显示所有可下单货物
        /// </summary>
        /// <returns></returns>
        public static List<Order> ScodeList = new List<Order>() { new Order { Count = "", Price = "", Scode = "" } };
        public string PlaceAnOrderShow()
        {
            shopbll sb = new shopbll();
            DataTable dtShop = sb.GetShopNameByUserId(userInfo.User.Id.ToString());
            SoCalProductBll scb = new SoCalProductBll();
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();
            #region 查询条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合

            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//货号
            string Scode = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string PriceMin = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小价格
            string PriceMax = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大价格
            string Brand = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//季节
            string StockMin = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//品牌
            string StockMax = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小库存
            string Type = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大库存
            string Imagefile = helpcommon.StrSplit.StrSplitData(orderParamss[8], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别
            string Season = helpcommon.StrSplit.StrSplitData(orderParamss[9], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//有图/无图

            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            PriceMin = PriceMin == "\'\'" ? string.Empty : PriceMin;
            PriceMax = PriceMax == "\'\'" ? string.Empty : PriceMax;
            Season = Season == "\'\'" ? string.Empty : Season;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            StockMin = StockMin == "\'\'" ? string.Empty : StockMin;
            StockMax = StockMax == "\'\'" ? string.Empty : StockMax;
            Type = Type == "\'\'" ? string.Empty : Type;
            Imagefile = Imagefile == "\'\'" ? string.Empty : Imagefile;

            
            string[] str = new string[20];
            str[0] = Scode;//货号
            str[1] = Season;//季节
            str[2] = PriceMin;//最小价格
            str[3] = PriceMax;//最大价格
            str[4] = Brand;//品牌
            str[5] = StockMin;//最小库存
            str[6] = StockMax;//最大库存
            str[7] = Type;//类别
            str[8] = Style;//款号
            str[9] = userInfo.User.Id.ToString();//用户编号
            str[10] = Imagefile;//是否有无图片
            str[11] = dtShop.Rows[0]["Id"].ToString();//店铺编号


            #endregion
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);//分页
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);//页码个数

            string[] s = ph.getFiledPermisson(roleId, PlaceAnOrderMenuId, funName.selectName);
            string[] allTableName = usb.getDataName("SoCaloutsideProduct");

            int Count = scb.GetPlaceAnOrder(str);//数据总个数
            int PageCount = Count % pageSize > 0 ? Count / pageSize + 1 : Count / pageSize;//数据总页数

            int MinId = pageSize * (pageIndex - 1);
            int MaxId = pageSize * (pageIndex - 1) + pageSize;

            DataTable dt = scb.GetPlaceAnOrder(str, MinId, MaxId);

            StringBuilder Alltable = new StringBuilder();

            #region Table表头排序

            if (s.Contains("Imagefile"))
            {
                string temp;
                temp = s[2];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Imagefile")
                    {
                        s[i] = temp;
                        s[2] = "Imagefile";
                    }
                }
            }
            if (s.Contains("Style"))
            {
                string temp;
                temp = s[3];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Style")
                    {
                        s[i] = temp;
                        s[3] = "Style";
                    }
                }
            }
            s[2] = "Style";
            s[3] = "Imagefile";
            #endregion

            #region Table表头
            Alltable.Append("<table id='Warehouse' class='mytable' rules='all'>");
            Alltable.Append("<tr>");
            Alltable.Append("<th>序号</th>");
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
                        Alltable.Append("供货价");
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
                        Alltable.Append("店铺库存");
                    if (s[i] == "Balance2")
                        Alltable.Append("销售库存");
                    if (s[i] == "Balance3")
                        Alltable.Append("退货库存");
                    if (s[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (s[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (s[i] == "UserId")
                        Alltable.Append("操作人");
                    if (s[i] == "ShowState")
                        Alltable.Append("销售状态");
                    if (s[i] == "Def2")
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("退货库存");
                    if (s[i] == "Def5")
                        Alltable.Append("操作人");
                    if (s[i] == "Def6")
                        Alltable.Append("操作时间");
                    if (s[i] == "Def7")
                        Alltable.Append("价格状态");
                    if (s[i] == "Def8")
                        Alltable.Append("库存状态");
                    if (s[i] == "Def9")
                        Alltable.Append("默认9");
                    if (s[i] == "Def10")
                        Alltable.Append("默认10");
                    if (s[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("<th>全选<input type='checkbox' id='CheckAll' onchange='CheckAll()'></th>");
            Alltable.Append("</tr>");


            #endregion

            #region Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["row_Id"] + "</td>");
                for (int j = 0; j < s.Length; j++)
                {

                    if (allTableName.Contains(s[j]))
                    {
                        if (s[j] == "Imagefile")
                        {
                            Alltable.Append("<td>");
                            if (dt.Rows[i]["ImagePath"].ToString() != "")
                            {
                                Alltable.Append("<img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["ImagePath"] + "' width='50' height='50'/>");
                            }
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
                Alltable.Append("<td>");
                string[] strScode = new string[ScodeList.Count];
                for (int ct = 0; ct < ScodeList.Count; ct++)
                {
                    strScode[ct] = ScodeList[ct].Scode;
                }
                if (strScode.Contains(dt.Rows[i]["Scode"].ToString()))
                {
                    Alltable.Append("<td><label style='width:100%;height:100%;display:block;padding-top:2px;'><input type='checkbox' class='Check' checked='checked' Scode='" + dt.Rows[i]["Scode"] + "'/></label></td>");
                }
                else
                {
                    Alltable.Append("<td><label style='width:100%;height:100%;display:block;padding-top:2px;'><input type='checkbox' class='Check'  Scode='" + dt.Rows[i]["Scode"] + "'/></label></td>");
                }
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
        /// 显示订单物品  
        /// </summary>
        /// <returns></returns>
        public static string OrderId = "";
        public string OrderShow()
        {
            DateTime time = DateTime.Now;
            OrderId = time.ToString("yyyyMMddhhmmss");
            string random = "";
            Random rd = new Random();
            random = rd.Next(10000, 99999).ToString();
            OrderId += random;
            string Scodes = Request.Form["Scodes"] == null ? "" : Request.Form["Scodes"].ToString();
            string[] strScodes = Scodes.Split('❤');
            SoCalProductBll scb = new SoCalProductBll();
            shopbll sb = new shopbll();
            StringBuilder AllTable = new StringBuilder();
            AllTable.Append("<table class='mytable' style='font-size:12px;'>");
            AllTable.Append("<tr>");
            AllTable.Append("<th>货号</th>");
            AllTable.Append("<th>品牌</th>");
            AllTable.Append("<th>类别</th>");
            AllTable.Append("<th>颜色</th>");
            AllTable.Append("<th>尺码</th>");
            AllTable.Append("<th>价格</th>");
            AllTable.Append("<th>库存</th>");
            AllTable.Append("<th>下单数量</th>");
            AllTable.Append("</tr>");
            DataTable dtShop = sb.GetShopNameByUserId(userInfo.User.Id.ToString());
            for (int i = 0; i < strScodes.Length - 1; i++)
            {
                DataTable dt = scb.GetSoCaloutsideProductByScodeAndShopId(strScodes[i].ToString(), dtShop.Rows[0]["Id"].ToString());
                AllTable.Append("<tr>");
                AllTable.Append("<td>" + strScodes[i] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Cat"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Cat2"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Clolor"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Size"] + "</td>");
                AllTable.Append("<td class='price' price='" + dt.Rows[0]["pricec"] + "'>" + dt.Rows[0]["pricec"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Balance"] + "</td>");
                int count = 0;
                for (int j = 0; j < ScodeList.Count; j++)
                {
                    if (strScodes[i] == ScodeList[j].Scode)
                    {
                        count = int.Parse(ScodeList[j].Count.ToString());
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                AllTable.Append("<td><input type='text' style='width:50px;' class='count' value='" + count + "' /></td>");
                AllTable.Append("</tr>");
            }
            AllTable.Append("</table>");
            return AllTable.ToString() + "❤" + OrderId;
        }
        /// <summary>
        /// //确认下单
        /// </summary>
        /// <returns></returns>
        public string OkOrder()
        {
            shopbll sb = new shopbll();
            SoCalProductBll scb = new SoCalProductBll();
            DataTable dtShop = sb.GetShopNameByUserId(userInfo.User.Id.ToString());
            string shop = dtShop.Rows[0]["Id"].ToString();//店铺编号
            string time = DateTime.Now.ToString();
            string Scodes = Request.Form["Scodes"] == null ? "" : Request.Form["Scodes"].ToString();
            string Counts = Request.Form["strCount"] == null ? "" : Request.Form["strCount"].ToString();
            string Moneys = Request.Form["strmoney"] == null ? "" : Request.Form["strmoney"].ToString();
            string[] StrScode = Scodes.Split('❤');
            string[] strCount = Counts.Split('❤');
            string[] strMoney = Moneys.Split('❤');
            string[] str = new string[7];
            bool result = false;
            if (ScodeList.Count > 1)  //添加到购物车再下单
            {
                for (int i = 0; i < ScodeList.Count; i++)
                {
                    str[0] = OrderId;
                    str[1] = ScodeList[i].Scode;
                    str[2] = time;
                    str[3] = ScodeList[i].Count;
                    str[4] = ScodeList[i].Price;
                    str[5] = shop;
                    str[6] = userInfo.User.Id.ToString();
                    if (scb.InsertSoCalOrder(str))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else   //不用购物车 直接下单
            {
                for (int i = 0; i < StrScode.Length; i++)
                {
                    if (StrScode[i] != "")
                    {
                        str[0] = OrderId;
                        str[1] = StrScode[i];
                        str[2] = time;
                        str[3] = strCount[i];
                        str[4] = strMoney[i];
                        str[5] = shop;
                        str[6] = userInfo.User.Id.ToString();
                        if (scb.InsertSoCalOrder(str))
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            if (result == true)
            {
                OrderId = "";
                ScodeList.Clear();
                return "订单已提交！";
            }
            else
            {
                return "订单提交失败！";
            }

        }
        /// <summary>
        /// 继续下单  保存到购物车
        /// </summary>
        public void GoOnOrder()
        {
            string Scodes = Request.Form["Scodes"] == null ? "" : Request.Form["Scodes"].ToString();
            string Counts = Request.Form["strCount"] == null ? "" : Request.Form["strCount"].ToString();
            string Moneys = Request.Form["strmoney"] == null ? "" : Request.Form["strmoney"].ToString();
            string[] StrScode = Scodes.Split('❤');
            string[] strCount = Counts.Split('❤');
            string[] strMoney = Moneys.Split('❤');
            string[] temp = new string[ScodeList.Count];
            for (int j = 0; j < ScodeList.Count; j++)
            {
                temp[j] = ScodeList[j].Scode;
            }
            for (int i = 0; i < StrScode.Length - 1; i++)
            {
                if (!temp.Contains(StrScode[i]))
                {
                    ScodeList.Add(new Order()
                    {
                        Count = strCount[i],
                        Scode = StrScode[i],
                        Price = strMoney[i]
                    });
                }
            }
        }
        /// <summary>
        /// 购物车
        /// </summary>
        /// <returns></returns>
        public string ShopCar()
        {
            SoCalProductBll scb = new SoCalProductBll();
            shopbll sb = new shopbll();
            StringBuilder AllTable = new StringBuilder();
            AllTable.Append("<table class='mytable' style='font-size:12px;'>");
            AllTable.Append("<tr>");
            AllTable.Append("<th>货号</th>");
            AllTable.Append("<th>品牌</th>");
            AllTable.Append("<th>类别</th>");
            AllTable.Append("<th>颜色</th>");
            AllTable.Append("<th>尺码</th>");
            AllTable.Append("<th>价格</th>");
            AllTable.Append("<th>库存</th>");
            AllTable.Append("<th>下单数量</th>");
            AllTable.Append("<th></th>");
            AllTable.Append("</tr>");
            DataTable dtShop = sb.GetShopNameByUserId(userInfo.User.Id.ToString());
            if (ScodeList[0].Scode == "")
            {
                ScodeList.RemoveAt(0);
            }
            for (int i = 0; i < ScodeList.Count; i++)
            {
                DataTable dt = scb.GetSoCaloutsideProductByScodeAndShopId(ScodeList[i].Scode.ToString(), dtShop.Rows[0]["Id"].ToString());
                AllTable.Append("<tr>");
                AllTable.Append("<td>" + ScodeList[i].Scode + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Cat"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Cat2"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Clolor"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Size"] + "</td>");
                AllTable.Append("<td class='price' price='" + dt.Rows[0]["pricec"] + "'>" + dt.Rows[0]["pricec"] + "</td>");
                AllTable.Append("<td>" + dt.Rows[0]["Balance"] + "</td>");
                AllTable.Append("<td><input type='text' style='width:50px;' value='" + ScodeList[i].Count + "' class='count' /></td>");
                AllTable.Append("<td><a href='#' onclick='Deleteshop(\"" + ScodeList[i].Scode + "\")'>X</a></td>");
                AllTable.Append("</tr>");
            }
            AllTable.Append("</table>");
            return AllTable.ToString() + "❤" + OrderId;
        }
        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <returns></returns>
        public string ClearShopCar()
        {
            ScodeList.Clear();
            if (ScodeList.Count > 0)
            {
                return "清空失败！";
            }
            else
            {
                return "已清空！";
            }
        }
        /// <summary>
        /// 删除购物车中某条数据
        /// </summary>
        /// <returns></returns>
        public string DeleteShop()
        {
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            for (int i = 0; i < ScodeList.Count; i++)
            {
                if (ScodeList[i].Scode == scode)
                {
                    ScodeList.RemoveAt(i);
                    break;
                }
            }
            return "删除成功！";

        }
        /// <summary>
        /// 保存订单信息
        /// </summary>
        public class Order
        {
            public string Scode { get; set; }
            public string Price { get; set; }
            public string Count { get; set; }
        }
        #endregion



        #region 现货订单详情
        public static int SaveOrderMenuId = 0;
        public static int OrderPage = 0;
        public ActionResult SoCalOrder()
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
                SaveOrderMenuId = menuId;
            }
            #region    加载下拉框

            ViewData["PageSum"] = php.GetPageDDlist();
            ViewData["Brand"] = php.GetBrandDDlist(userInfo.User.Id);
            ViewData["Type"] = php.GetTypeDDlist(userInfo.User.Id);
            #endregion
            //查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            StringBuilder Ok = new StringBuilder();
            if (ph.isFunPermisson(roleId, menuId, funName.CustomerOk))
            {
                Ok.Append("<input class='spanPropertySearch' type='button' value='确认订单' onclick='CustomerOk()'/>");
            }
            if (ph.isFunPermisson(roleId, menuId, funName.VencodeOk))
            {
                Ok.Append("<input class='spanPropertySearch' type='button' value='确认订单' onclick='VencodeOk()'/>");
            }
            if (ph.isFunPermisson(roleId, menuId, funName.Js))
            {
                Ok.Append("<input class='spanPropertySearch' type='button' value='确认结算' onclick='JsOk()'/>");
            }
            ViewBag.Ok = Ok;
            return View();
        }
        /// <summary>
        /// 查看订单
        /// </summary>
        /// <returns></returns>
        public string GetSoCalOrder()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string[] str = new string[10];
            str[0] = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"].ToString();
            str[1] = Request.Form["JsState"] == null ? "" : Request.Form["JsState"].ToString();
            str[2] = Request.Form["Customer"] == null ? "" : Request.Form["Customer"].ToString();
            str[3] = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            str[4] = userInfo.User.Id.ToString();
            str[5] = Request.Form["MinTime"] == null ? "" : Request.Form["MinTime"].ToString();
            str[6] = Request.Form["MaxTime"] == null ? "" : Request.Form["MaxTime"].ToString();
            int page = Request.Form["page"] == null ? 10 : int.Parse(Request.Form["page"].ToString());
            int count = scb.GetSoCalOrder(str);
            int PageSum = count % page > 0 ? count / page + 1 : count / page;//页码
            bool IsPage = Request.Form["isPage"] == null ? false : true;//为false 表示加载  为true则表示翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());
            StringBuilder Alltable = new StringBuilder();
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        OrderPage = 0;
                        break;
                    case 1:         //上一页
                        if (OrderPage - 1 >= 0)
                        {
                            OrderPage = OrderPage - 1;
                        }
                        else
                        {
                            OrderPage = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["ReturnPage"] == null ? 0 : int.Parse(Request.Form["ReturnPage"].ToString());
                        OrderPage = ReturnPage - 1;
                        break;
                    case 3:        //下页
                        if (OrderPage + 1 <= PageSum - 1)
                        {
                            OrderPage = OrderPage + 1;
                        }
                        else
                        {
                            OrderPage = PageSum - 1;
                        }
                        break;
                    case 4:        //末页
                        OrderPage = PageSum - 1;
                        break;
                }
                #endregion
            }
            else
            {
                OrderPage = 0;
            }
            DataTable dt = scb.GetSoCalOrder(str, OrderPage * page, OrderPage * page + page);
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            Alltable.Append("<th>订单编号</th>");
            Alltable.Append("<th>总数量</th>");
            Alltable.Append("<th>总价格</th>");
            Alltable.Append("<th>是否结算</th>");
            Alltable.Append("<th>客户确认</th>");
            Alltable.Append("<th>供应商确认</th>");
            Alltable.Append("<th>下单时间</th>");
            Alltable.Append("<th>店铺</th>");
            Alltable.Append("<th>操作</th>");
            Alltable.Append("<th>全选<input type='checkbox' id='CheckAll' onchange='CheckAll()'></th>");
            string[] s = new string[] { "row_id", "OrderId", "scodesum", "AllPrice", "SettlementState", "CustomerState", "SuppliersState", "OrderTime", "Loc" };
            #region   内容
            Alltable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j] == "SettlementState")
                    {
                        if (dt.Rows[i][s[j]].ToString() == "0")
                        {
                            Alltable.Append("<td style='color:red;'>未结算</td>");
                        }
                        else
                        {
                            Alltable.Append("<td style='color:green;'>已结算</td>");
                        }
                    }
                    else if (s[j] == "CustomerState")
                    {
                        if (dt.Rows[i][s[j]].ToString() == "0")
                        {
                            Alltable.Append("<td style='color:red;'>未确认</td>");
                        }
                        else
                        {
                            Alltable.Append("<td style='color:green;'>已确认</td>");
                        }
                    }
                    else if (s[j] == "SuppliersState")
                    {
                        if (dt.Rows[i][s[j]].ToString() == "0")
                        {
                            Alltable.Append("<td style='color:red;'>未确认</td>");
                        }
                        else
                        {
                            Alltable.Append("<td style='color:green;'>已确认</td>");
                        }
                    }
                    else if (s[j] == "Loc")
                    {
                        ShopProductBll spb = new ShopProductBll();
                        string ShopName = spb.GetShopNameByShopId(dt.Rows[i]["Loc"].ToString());
                        Alltable.Append("<td>" + ShopName + "</td>");
                    }
                    else
                    {
                        Alltable.Append("<td>" + dt.Rows[i][s[j]] + "</td>");
                    }
                }
                Alltable.Append("<td><a href='#' onclick='LookOrder(\"" + dt.Rows[i]["OrderId"].ToString() + "\")'>订单详细</a></td>");
                Alltable.Append("<td><label style='width:100%;height:100%;display:block;padding-top:2px;'><input type='checkbox' class='Check' OrderId='" + dt.Rows[i]["OrderId"].ToString() + "' /></label></td>");
                Alltable.Append("<tr>");
            }
            #endregion
            Alltable.Append("</table>");
            int thispage = 0;
            if (IsPage == true)
            {
                thispage = OrderPage + 1;
            }
            else
            {
                thispage = 1;
            }

            return Alltable.ToString() + "❤" + count + "❤" + PageSum + "❤" + thispage.ToString();
        }
        /// <summary>
        /// 订单详细信息
        /// </summary>
        /// <returns></returns>
        public static int OrderPageDetailed = 0;
        public string GetSoCalOrderDetailed()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string[] str = new string[13];
            str[0] = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"].ToString();
            str[1] = Request.Form["JsState"] == null ? "" : Request.Form["JsState"].ToString();
            str[2] = Request.Form["Customer"] == null ? "" : Request.Form["Customer"].ToString();
            str[3] = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            str[4] = userInfo.User.Id.ToString();
            str[5] = Request.Form["MinTime"] == null ? "" : Request.Form["MinTime"].ToString();
            str[6] = Request.Form["MaxTime"] == null ? "" : Request.Form["MaxTime"].ToString();
            str[7] = Request.Form["OrderScode"] == null ? "" : Request.Form["OrderScode"].ToString();
            str[8] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();
            str[9] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();
            int page = Request.Form["page"] == null ? 10 : int.Parse(Request.Form["page"].ToString());
            int count = scb.GetSoCalrOrderDetailed(str);
            int PageSum = count % page > 0 ? count / page + 1 : count / page;//页码
            bool IsPage = Request.Form["isPage"] == null ? false : true;//为false 表示加载  为true则表示翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());
            StringBuilder Alltable = new StringBuilder();
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        OrderPageDetailed = 0;
                        break;
                    case 1:         //上一页
                        if (OrderPageDetailed - 1 >= 0)
                        {
                            OrderPageDetailed = OrderPageDetailed - 1;
                        }
                        else
                        {
                            OrderPageDetailed = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["ReturnPage"] == null ? 0 : int.Parse(Request.Form["ReturnPage"].ToString());
                        OrderPageDetailed = ReturnPage - 1;
                        break;
                    case 3:        //下页
                        if (OrderPageDetailed + 1 <= PageSum - 1)
                        {
                            OrderPageDetailed = OrderPageDetailed + 1;
                        }
                        else
                        {
                            OrderPageDetailed = PageSum - 1;
                        }
                        break;
                    case 4:        //末页
                        OrderPageDetailed = PageSum - 1;
                        break;
                }
                #endregion
            }
            else
            {
                OrderPage = 0;
            }
            DataTable dt = scb.GetSoCalrOrderDetailed(str, OrderPage * page, OrderPage * page + page);
            int menuId = SaveOrderMenuId;
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            PublicHelpController ph = new PublicHelpController();
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);

            for (int i = 0; i < s.Length; i++)
            {
                string temp = "";
                if (s[i] == "OrderId")
                {
                    temp = s[0];
                    s[0] = "OrderId";
                    s[i] = temp;
                    break;
                }
            }
            for (int i = 0; i < s.Length; i++)
            {
                string temp = "";
                if (s[i] == "OrderScode")
                {
                    temp = s[1];
                    s[1] = "OrderScode";
                    s[i] = temp;
                    break;
                }
            }
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("SoCalOrder");
            string[] allTableName1 = usb.getDataName("SoCalProduct");
            Alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'><tr style='text-align:center;'>");
            Alltable.Append("<tr>");
            Alltable.Append("<th style='min-width:70px;'>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {

                if (allTableName.Contains(s[i]) || allTableName1.Contains(s[i]))
                {
                    #region 表头
                    Alltable.Append("<th style='min-width:70px;'>");
                    if (s[i] == "OrderId")
                        Alltable.Append("订单编号");
                    if (s[i] == "OrderScode")
                        Alltable.Append("货号");
                    if (s[i] == "OrderTime")
                        Alltable.Append("下单时间");
                    if (s[i] == "SettlementState")
                        Alltable.Append("结算状态");
                    if (s[i] == "CustomerState")
                        Alltable.Append("客户确认");
                    if (s[i] == "SuppliersState")
                        Alltable.Append("供应商确认");
                    if (s[i] == "ScodeSum")
                        Alltable.Append("数量");
                    if (s[i] == "ScodePrice")
                        Alltable.Append("价格");
                    if (s[i] == "Loc")
                        Alltable.Append("店铺");
                    if (s[i] == "UserId")
                        Alltable.Append("用户");
                    if (s[i] == "Def1")
                        Alltable.Append("默认1");
                    if (s[i] == "Def2")
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("默认4");
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
                    if (s[i] == "Cat")
                        Alltable.Append("品牌");
                    if (s[i] == "Cat2")
                        Alltable.Append("类别");
                    if (s[i] == "Clolor")
                        Alltable.Append("颜色");
                    if (s[i] == "Size")
                        Alltable.Append("尺码");
                    if (s[i] == "Imagefile")
                        Alltable.Append("图片");
                    Alltable.Append("</th>");
                    #endregion
                }

            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #region   内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["row_no"] + "</td>");
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j] == "SettlementState")
                    {
                        if (dt.Rows[i][s[j]].ToString() == "0")
                        {
                            Alltable.Append("<td style='color:red;'>未结算</td>");
                        }
                        else
                        {
                            Alltable.Append("<td style='color:green;'>已结算</td>");
                        }
                    }
                    else if (s[j] == "CustomerState")
                    {
                        if (dt.Rows[i][s[j]].ToString() == "0")
                        {
                            Alltable.Append("<td style='color:red;'>未确认</td>");
                        }
                        else
                        {
                            Alltable.Append("<td style='color:green;'>已确认</td>");
                        }
                    }
                    else if (s[j] == "SuppliersState")
                    {
                        if (dt.Rows[i][s[j]].ToString() == "0")
                        {
                            Alltable.Append("<td style='color:red;'>未确认</td>");
                        }
                        else
                        {
                            Alltable.Append("<td style='color:green;'>已确认</td>");
                        }
                    }
                    else if (s[j] == "Imagefile")
                    {
                        if (dt.Rows[i][s[j]].ToString() == null)
                        {
                            Alltable.Append("<td>" + dt.Rows[i][s[j]] + "</td>");
                        }
                        else
                        {
                            Alltable.Append("<td><img src='" + dt.Rows[i][s[j]] + "'/></td>");
                        }
                    }
                    else if (s[j] == "ScodeSum")
                    {
                        Alltable.Append("<td class='ScodeSum' OrderId='" + dt.Rows[i]["OrderId"] + "' OrderScode='" + dt.Rows[i]["OrderScode"] + "'>" + dt.Rows[i][s[j]] + "</td>");
                    }
                    else if (s[j] == "Loc")
                    {

                        ShopProductBll spb = new ShopProductBll();
                        string ShopName = spb.GetShopNameByShopId(dt.Rows[i]["Loc"].ToString());
                        Alltable.Append("<td>" + ShopName + "</td>");
                    }
                    else
                    {
                        Alltable.Append("<td>" + dt.Rows[i][s[j]] + "</td>");
                    }
                }
                Alltable.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.CustomerOk))
                {
                    Alltable.Append("<a href='#' class='CustomeUpdateSum' >修改数量</a>&nbsp;");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.VencodeOk))
                {
                    Alltable.Append("<a href='#' class='VencodeUpdateSum' >修改数量</a>");
                }
                Alltable.Append("</td>");
                Alltable.Append("<tr>");
            }
            #endregion
            Alltable.Append("</table>");
            int thispage = 0;
            if (IsPage == true)
            {
                thispage = OrderPageDetailed + 1;
            }
            else
            {
                thispage = 1;
            }
            return Alltable.ToString() + "❤" + count + "❤" + PageSum + "❤" + thispage.ToString();
        }
        /// <summary>
        /// 客户确认
        /// </summary>
        /// <returns></returns>
        public string CustomerOk()
        {
            string Orders = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"].ToString();
            string[] strOrder = Orders.Split(',');
            SoCalProductBll scb = new SoCalProductBll();
            for (int i = 0; i < strOrder.Length - 1; i++)
            {
                scb.OrderOkState("1", "CustomerState", strOrder[i]);
            }
            return "已成功确认订单！";
        }
        /// <summary>
        /// 供应商确认
        /// </summary>
        /// <returns></returns>
        public string Vencode()
        {
            string Orders = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"].ToString();
            string[] strOrder = Orders.Split(',');
            SoCalProductBll scb = new SoCalProductBll();
            for (int i = 0; i < strOrder.Length - 1; i++)
            {
                scb.OrderOkState("1", "SuppliersState", strOrder[i]);
            }
            return "已成功确认订单！";
        }
        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public string Js()
        {
            string Orders = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"].ToString();
            string[] strOrder = Orders.Split(',');
            SoCalProductBll scb = new SoCalProductBll();
            for (int i = 0; i < strOrder.Length - 1; i++)
            {
                scb.OrderOkState("1", "SettlementState", strOrder[i]);
            }
            return "已成功确认结算！";
        }
        /// <summary>
        /// 客户修改数量
        /// </summary>
        public void CustomeUpdateSumUpdateSum()
        {
            string OrderId = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"].ToString();
            string OrderScode = Request.Form["OrderScode"] == null ? "" : Request.Form["OrderScode"].ToString();
            string Num = Request.Form["Num"] == null ? "" : Request.Form["Num"].ToString();
            SoCalProductBll scb = new SoCalProductBll();
            DataTable dt = scb.GetSoCalOrderByOrderIdAndScode(OrderId, OrderScode);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Def1"].ToString() != "1")
                {
                    scb.UpdateOrderSum(OrderId, OrderScode, Num, "Def1", "1");
                }
            }
        }
        /// <summary>
        /// 供应商修改数量
        /// </summary>
        public void VencodeUpdateSumUpdateSum()
        {
            string OrderId = Request.Form["OrderId"] == null ? "" : Request.Form["OrderId"].ToString();
            string OrderScode = Request.Form["OrderScode"] == null ? "" : Request.Form["OrderScode"].ToString();
            string Num = Request.Form["Num"] == null ? "" : Request.Form["Num"].ToString();
            SoCalProductBll scb = new SoCalProductBll();
            scb.UpdateOrderSum(OrderId, OrderScode, Num, "Def2", "1");

        }
        //*********************************6.30库存汇总功能
        public static int MergerSaveMenuId;//保存库存汇总菜单编号
        public static int MergerPage;
        public ActionResult StockMerge()
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
                MergerSaveMenuId = menuId;
            }
            // 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["menuId"] = Savemenuid;
            SelectListItem sli = new SelectListItem()
            {
                Text = "请选择",
                Value = ""
            };
            List<SelectListItem> drplist = new List<SelectListItem>();//品牌下拉框
            BrandBLL bb = new BrandBLL();
            ProductHelper pdh = new ProductHelper();
            DataTable brm = pdh.ProductStyleDDlist(userInfo.User.Id.ToString());
            drplist.Add(sli);
            for (int i = 0; i < brm.Rows.Count; i++)
            {
                SelectListItem sl = new SelectListItem();
                sl.Text = brm.Rows[i]["BrandName"].ToString();
                sl.Value = brm.Rows[i]["BrandAbridge"].ToString();
                drplist.Add(sl);
            }
            ViewData["DrpBrand"] = drplist;
            ProductTypeBLL ptb = new ProductTypeBLL();
            List<SelectListItem> drplisttype = new List<SelectListItem>();//类别下拉框
            DataTable listType = pdh.ProductTypeDDlist(userInfo.User.Id.ToString());

            drplisttype.Add(sli);
            for (int i = 0; i < listType.Rows.Count; i++)
            {
                SelectListItem si = new SelectListItem();
                si.Text = listType.Rows[i]["TypeName"].ToString();
                si.Value = listType.Rows[i]["TypeNo"].ToString();
                drplisttype.Add(si);
            }
            List<SelectListItem> list = new List<SelectListItem>();//页码下拉框
            SelectListItem sl0 = new SelectListItem();
            sl0.Text = "10"; sl0.Value = "10";
            SelectListItem sl1 = new SelectListItem();
            sl1.Text = "5"; sl1.Value = "5";
            SelectListItem sl2 = new SelectListItem();
            sl2.Text = "20"; sl2.Value = "20";
            SelectListItem sl3 = new SelectListItem();
            sl3.Text = "50"; sl3.Value = "50";
            list.Add(sl0); list.Add(sl1); list.Add(sl2); list.Add(sl3);
            ViewData["PageSum"] = list;
            ViewData["DropType"] = drplisttype;
            return View();
        }
        /// <summary>
        /// 汇总货号查询
        /// </summary>
        /// <returns></returns>
        public string ProductStockMergeScodePage()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string customer = userInfo.User.Id.ToString();
            string[] str = new string[10];
            str[0] = Request.Form["Style"] == null ? "" : Request.Form["Style"].ToString();
            str[1] = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            str[2] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();
            str[3] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();
            str[4] = Request.Form["minPrice"] == null ? "" : Request.Form["minPrice"].ToString();
            str[5] = Request.Form["maxPrice"] == null ? "" : Request.Form["maxPrice"].ToString();
            str[6] = Request.Form["minBalance"] == null ? "" : Request.Form["minBalance"].ToString();
            str[7] = Request.Form["maxBalance"] == null ? "" : Request.Form["maxBalance"].ToString();
            int page = Request.Form["page"] == null ? 10 : int.Parse(Request.Form["page"].ToString());
            int count = scb.ProductStockMergeScodeCount(str, customer);
            int PageSum = count % page > 0 ? count / page + 1 : count / page;//页码
            bool IsPage = Request.Form["isPage"] == null ? false : true;//为false 表示加载  为true则表示翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        MergerPage = 0;
                        break;
                    case 1:         //上一页
                        if (MergerPage - 1 >= 0)
                        {
                            MergerPage = MergerPage - 1;
                        }
                        else
                        {
                            MergerPage = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["PageReturn"] == null ? 0 : int.Parse(Request.Form["PageReturn"].ToString());
                        MergerPage = ReturnPage - 1;
                        break;
                    case 3:        //下页
                        if (MergerPage + 1 <= PageSum - 1)
                        {
                            MergerPage = MergerPage + 1;
                        }
                        else
                        {
                            MergerPage = PageSum - 1;
                        }
                        break;
                    case 4:        //末页
                        MergerPage = PageSum - 1;
                        break;
                }
                #endregion
            }
            else
            {
                MergerPage = 0;
            }
            str[8] = (page * MergerPage).ToString();
            str[9] = ((page * MergerPage) + page).ToString();
            DataTable dt = scb.ProductStockMergeScodePage(str, customer);
            int Page = Request.Form["Page"] == null ? 5 : helpcommon.ParmPerportys.GetNumParms(Request.Form["Page"]);
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = MergerSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("distinctProductStock");
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            #region  排序
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
                    string Scode = allTableName[i];
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
                    string Scode = allTableName[i];
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
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                #region//判断头
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
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("默认4");
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
                #endregion
            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            #region //判断内容
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
                    Alltable.Append("<td><a href='#' onclick='LookAttr(\"" + dt.Rows[i]["Scode"] + "\")'>查看属性</a></td>");
                    Alltable.Append("</tr>");
                }
            }
            #endregion
            Alltable.Append("</table>");
            int thispage = 0;
            if (IsPage == true)
            {
                thispage = MergerPage + 1;
            }
            else
            {
                thispage = 1;
            }
            return Alltable.ToString() + "❤" + PageSum + "❤" + count + "❤" + thispage;
        }
        /// <summary>
        /// 汇总款号查询
        /// </summary>
        /// <returns></returns>
        public static int StyleMergerPage;
        public string ProductStockMergeStylePage()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string customer = userInfo.User.Id.ToString();
            string[] str = new string[10];
            str[0] = Request.Form["Style"] == null ? "" : Request.Form["Style"].ToString();
            str[1] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();
            str[2] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();
            str[3] = Request.Form["minPrice"] == null ? "" : Request.Form["minPrice"].ToString();
            str[4] = Request.Form["maxPrice"] == null ? "" : Request.Form["maxPrice"].ToString();
            str[5] = Request.Form["minBalance"] == null ? "" : Request.Form["minBalance"].ToString();
            str[6] = Request.Form["maxBalance"] == null ? "" : Request.Form["maxBalance"].ToString();
            int page = Request.Form["page"] == null ? 10 : int.Parse(Request.Form["page"].ToString());
            int count = scb.ProductStockMergeStyleCount(str, customer);
            int PageSum = count % page > 0 ? count / page + 1 : count / page;//页码
            bool IsPage = Request.Form["isPage"] == null ? false : true;//为false 表示加载  为true则表示翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        StyleMergerPage = 0;
                        break;
                    case 1:         //上一页
                        if (StyleMergerPage - 1 >= 0)
                        {
                            StyleMergerPage = StyleMergerPage - 1;
                        }
                        else
                        {
                            StyleMergerPage = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["PageReturn"] == null ? 0 : int.Parse(Request.Form["PageReturn"].ToString());
                        StyleMergerPage = ReturnPage - 1;
                        break;
                    case 3:        //下页
                        if (StyleMergerPage + 1 <= PageSum - 1)
                        {
                            StyleMergerPage = StyleMergerPage + 1;
                        }
                        else
                        {
                            StyleMergerPage = PageSum - 1;
                        }
                        break;
                    case 4:        //末页
                        StyleMergerPage = PageSum - 1;
                        break;
                }
                #endregion
            }
            else
            {
                MergerPage = 0;
            }
            str[7] = (page * StyleMergerPage).ToString();
            str[8] = ((page * StyleMergerPage) + page).ToString();
            DataTable dt = scb.ProductStockMergeStylePage(str, customer);
            int Page = Request.Form["Page"] == null ? 5 : helpcommon.ParmPerportys.GetNumParms(Request.Form["Page"]);
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = MergerSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("distinctProductStock");
            string[] s = new string[] { "Style", "Cat", "Cat2", "Pricea", "Balance" };
            #region  排序
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
                    string Scode = allTableName[i];
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
                    string Scode = allTableName[i];
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
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                #region//判断头
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
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("默认4");
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
                #endregion
            }
            //if (ph.isFunPermisson(roleId, menuId, funName.Storage))
            //{
            Alltable.Append("<th>操作</th>");
            //}
            Alltable.Append("</tr>");
            #region //判断内容
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
                            if (s[j] == "Cat")
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
                    Alltable.Append("<td><a href='#' onclick='LookProduct(\"" + dt.Rows[i]["Style"] + "\")'>查看商品</a></td>");
                    Alltable.Append("</tr>");
                }
            }
            #endregion
            Alltable.Append("</table>");
            int thispage = 0;
            if (IsPage == true)
            {
                thispage = MergerPage + 1;
            }
            else
            {
                thispage = 1;
            }
            return Alltable.ToString() + "❤" + PageSum + "❤" + count + "❤" + thispage;
        }
        /// <summary>
        /// 查看属性
        /// </summary>
        /// <returns></returns>
        public string GetdistinctProductStockByScod()
        {
            SoCalProductBll scb = new SoCalProductBll();
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            DataTable dt = scb.GetdistinctProductStockByScod(scode);
            string property = GetTypeNo(scode, dt.Rows[0]["Cat2"].ToString());
            string s = string.Format(@"<table id='tabProperty'>
            <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><img src='{11}' style='width:120px;' /></td></tr>
            <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' disabled='disabled' type='text' value='{2}' /></td></tr>
            <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                <td>季节：<input type='text' value='{4}' disabled='disabled' /></td></tr>
            <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
            <tr><td>预警库存：<input id='txtRolevelE' disabled='disabled' type='text' value='{9}' /></td><td>类别：<input type='text' disabled='disabled' id='hidId'' value='{5}' /></td></tr>
        </table>", dt.Rows[0]["Scode"].ToString()
                             , dt.Rows[0]["Descript"].ToString()
                             , dt.Rows[0]["Cdescript"].ToString()
                             , dt.Rows[0]["BrandName"].ToString()
                             , dt.Rows[0]["Cat1"].ToString()
                             , dt.Rows[0]["TypeName"].ToString()
                             , dt.Rows[0]["Clolor"].ToString()
                             , dt.Rows[0]["Style"].ToString()
                             , dt.Rows[0]["Size"].ToString()
                             , dt.Rows[0]["Rolevel"].ToString()
                             , dt.Rows[0]["Id"].ToString()
                             , dt.Rows[0]["ImagePath"].ToString()

                        );
            #region
            StringBuilder alltableprice = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            int menuId = MergerSaveMenuId;
            string[] Spermission = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            
            #endregion
            return s.ToString() + "❤" + alltableprice.ToString() + "❤" + property;
        }
        #endregion
    }
}
