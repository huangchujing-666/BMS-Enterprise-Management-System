using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Top.Api.Domain;

namespace pbxdata.web.Controllers
{
    public class ShopOrderController : BaseController
    {
        //
        // GET: /ShopOrder/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult getShopOrder()
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


        #region 获取店铺订单列表
        /// <summary>
        /// 获取店铺订单列表
        /// </summary>
        /// <returns></returns>
        public string getData()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);
            List<model.porder> list = new List<model.porder>();
            StringBuilder s = new StringBuilder();
            bll.shoporderbll shoporderbll = new bll.shoporderbll();
            string[] ssName = shoporderbll.getDataName("porder");
            string[] ssName1 = shoporderbll.getDataName("orderdetails");
            string[] ssName2 = shoporderbll.getDataName("productstock");
            string[] ssName3 = shoporderbll.getDataName("activeShopOrder");
            string[] ssName4 = shoporderbll.getDataName("activeShop");
            string[] ssName5 = shoporderbll.getDataName("orderComments");
            string[] ssName6 = shoporderbll.getDataName("OrderExpress");

            List<string> listName = new List<string>();
            for (int i = 0; i < ssName.Length; i++)
            {
                if (!listName.Contains(helpcommon.ParmPerportys.stringToLower(ssName[i])))
                {
                    listName.Add(helpcommon.ParmPerportys.stringToLower(ssName[i]));
                }

            }
            for (int i = 0; i < ssName1.Length; i++)
            {
                if (!listName.Contains(helpcommon.ParmPerportys.stringToLower(ssName1[i])))
                {
                    listName.Add(helpcommon.ParmPerportys.stringToLower(ssName1[i]));
                }
            }
            for (int i = 0; i < ssName2.Length; i++)
            {
                if (!listName.Contains(helpcommon.ParmPerportys.stringToLower(ssName2[i])))
                {
                    listName.Add(helpcommon.ParmPerportys.stringToLower(ssName2[i]));
                }
            }
            for (int i = 0; i < ssName3.Length; i++)
            {
                if (!listName.Contains(helpcommon.ParmPerportys.stringToLower(ssName3[i])))
                {
                    listName.Add(helpcommon.ParmPerportys.stringToLower(ssName3[i]));
                }
            }
            for (int i = 0; i < ssName4.Length; i++)
            {
                if (!listName.Contains(helpcommon.ParmPerportys.stringToLower(ssName4[i])))
                {
                    listName.Add(helpcommon.ParmPerportys.stringToLower(ssName4[i]));
                }
            }
            for (int i = 0; i < ssName5.Length; i++)
            {
                if (!listName.Contains(helpcommon.ParmPerportys.stringToLower(ssName5[i])))
                {
                    listName.Add(helpcommon.ParmPerportys.stringToLower(ssName5[i]));
                }
            }
            for (int i = 0; i < ssName6.Length; i++)
            {
                if (!listName.Contains(helpcommon.ParmPerportys.stringToLower(ssName6[i])))
                {
                    listName.Add(helpcommon.ParmPerportys.stringToLower(ssName6[i]));
                }
            }
            List<string> l1 = new List<string>();
            l1 = ssName.ToList();
            for (int i = ssName.Length - 1; i >= 0; i--)
            {
                l1.RemoveAt(i);
            }
            ssName = l1.ToArray();
            ssName = listName.ToArray();


            #region 查询
            string orderId = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["orderId"]);
            string scode = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["scode"]);
            string brand = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["brand"]);
            string type = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["type"]);
            string status = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["status"]);
            string shopname = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["shopname"]);
            string servicecustom = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["servicecustom"]);
            string buynick = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["buynick"]);
            string price1 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["price1"]);
            string price2 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["price2"]);
            string pprice1 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["pprice1"]);
            string pprice2 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["pprice2"]);
            string ordertime1 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["ordertime1"]);
            string ordertime2 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["ordertime2"]);
            string paytime1 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["paytime1"]);
            string paytime2 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["paytime2"]);
            string sendtime1 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["sendtime1"]);
            string sendtime2 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["sendtime2"]);
            string sucesstime1 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["sucesstime1"]);
            string sucesstime2 = helpcommon.ParmPerportys.GetStrParms(Request.QueryString["sucesstime2"]);
            #endregion

            //DataTable dt = shoporderbll.getData();
            DataTable dt = shoporderbll.getData(pageIndex, pageSize, orderId, scode, brand, type, status, shopname, servicecustom, buynick, price1, price2, pprice1, pprice2, ordertime1, ordertime2, paytime1, paytime2, sendtime1, sendtime2, sucesstime1, sucesstime2);
            int count = shoporderbll.getDataCount(orderId, scode, brand, type, status, shopname, servicecustom, buynick, price1, price2, pprice1, pprice2, ordertime1, ordertime2, paytime1, paytime2, sendtime1, sendtime2, sucesstime1, sucesstime2);
            int pageCount = count / pageSize;
            pageCount = count % pageSize > 0 ? pageCount + 1 : pageCount;


            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            string[] ssDataTableName = new string[ss.Length];
            for (int i = 0; i < ss.Length; i++)
            {
                ss[i] = helpcommon.ParmPerportys.stringToLower(ss[i]);
            }

            List<string> listDataTableName = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (ss.Contains(helpcommon.ParmPerportys.stringToLower(dt.Columns[i].ColumnName)))
                    listDataTableName.Add(helpcommon.ParmPerportys.stringToLower(dt.Columns[i].ColumnName));
                //ssDataTableName[i] = helpcommon.ParmPerportys.stringToLower(dt.Columns[i].ColumnName);
            }
            ss = listDataTableName.ToArray();




            #region TABLE添加
            s.Append("<tr>");
            s.Append("<th colspan='50' class='mytableadd'>");
            s.Append("<div style='padding-top: 20px;'>");
            if (ph.isFunPermisson(roleId, menuId, funName.addName))
            {
                s.Append("<a href='#'  onclick='javascript: showDiv()' >添加</a>");
            }
            s.Append("</div>");
            s.Append("</th>");
            s.Append("</tr>");
            #endregion

            #region TABLE表头
            s.Append("<tr>");

            #region porder
            for (int z = 0; z < ss.Length; z++)
            {
                if (ssName.Contains(helpcommon.ParmPerportys.stringToLower(ss[z])))
                {

                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderid")
                    {
                        s.Append("<td>"); s.Append("订单编号"); s.Append("</td>");
                    }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "productid")
                    { s.Append("<td>"); s.Append("商品ID"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "shopname")
                    { s.Append("<td>"); s.Append("店铺名称"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderprice")
                    { s.Append("<td>"); s.Append("价格"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ordertime")
                    { s.Append("<td>"); s.Append("下单时间"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderpaytime")
                    { s.Append("<td>"); s.Append("付款时间"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderedittime")
                    { s.Append("<td>"); s.Append("更改付款时间"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ordernick")
                    { s.Append("<td>"); s.Append("昵称"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "paystate")
                    { s.Append("<td>"); s.Append("支付状态"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderstate")
                    { s.Append("<td>"); s.Append("订单状态(店铺)"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderstate1")
                    { s.Append("<td>"); s.Append("订单状态(源头)"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "customserverid")
                    { s.Append("<td>"); s.Append("客服"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "userid")
                    { s.Append("<td>"); s.Append("操作人"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "def1")
                    { s.Append("<td>"); s.Append("默认1"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "def2")
                    { s.Append("<td>"); s.Append("默认2"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "def3")
                    { s.Append("<td>"); s.Append("默认3"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "def4")
                    { s.Append("<td>"); s.Append("默认4"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "def5")
                    { s.Append("<td>"); s.Append("默认5"); s.Append("</td>"); }




                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "detailsprice")
                    { s.Append("<td>"); s.Append("价格"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderimg")
                    { s.Append("<td>"); s.Append("图片"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ordercolor")
                    { s.Append("<td>"); s.Append("颜色"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "detailsname")
                    { s.Append("<td>"); s.Append("标题"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderchildenid")
                    { s.Append("<td>"); s.Append("子订单号"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "orderscode")
                    { s.Append("<td>"); s.Append("货号"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "detailssum")
                    { s.Append("<td>"); s.Append("数量"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ordersendtime")
                    { s.Append("<td>"); s.Append("发货时间"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ordersucesstime")
                    { s.Append("<td>"); s.Append("结束时间"); s.Append("</td>"); }



                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "cat")
                    { s.Append("<td>"); s.Append("品牌"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "cat2")
                    { s.Append("<td>"); s.Append("类别"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "clolor")
                    { s.Append("<td>"); s.Append("颜色"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "pricee")
                    { s.Append("<td>"); s.Append("成本价"); s.Append("</td>"); }

                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "acname")
                    { s.Append("<td>"); s.Append("活动名称"); s.Append("</td>"); }

                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ocremark")
                    { s.Append("<td>"); s.Append("备注"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "occomment")
                    { s.Append("<td>"); s.Append("评价"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ocbanner")
                    { s.Append("<td>"); s.Append("旗帜"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ocpostprice")
                    { s.Append("<td>"); s.Append("邮费"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "ocotherprice")
                    { s.Append("<td>"); s.Append("其他价格"); s.Append("</td>"); }



                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "expressno")
                    { s.Append("<td>"); s.Append("运单号"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "customname")
                    { s.Append("<td>"); s.Append("姓名"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "customcity1")
                    { s.Append("<td>"); s.Append("省份"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "customcity2")
                    { s.Append("<td>"); s.Append("城市"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "customcity3")
                    { s.Append("<td>"); s.Append("县/街"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "customaddress")
                    { s.Append("<td>"); s.Append("地址"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "expressname")
                    { s.Append("<td>"); s.Append("快递公司"); s.Append("</td>"); }
                    if (helpcommon.ParmPerportys.stringToLower(ss[z]) == "customphone")
                    { s.Append("<td>"); s.Append("电话"); s.Append("</td>"); }


                }
            }
            #endregion

            //#region orderdetails
            //for (int z = 0; z < ssName1.Length; z++)
            //{
            //    if (ss.Contains(helpcommon.ParmPerportys.stringToLower(ssName1[z])))
            //    {
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "detailsprice")
            //        { s.Append("<td>"); s.Append("价格"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "orderimg")
            //        { s.Append("<td>"); s.Append("图片"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "ordercolor")
            //        { s.Append("<td>"); s.Append("颜色"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "detailsname")
            //        { s.Append("<td>"); s.Append("标题"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "orderchildenid")
            //        { s.Append("<td>"); s.Append("子订单号"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "orderscode")
            //        { s.Append("<td>"); s.Append("货号"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "detailssum")
            //        { s.Append("<td>"); s.Append("数量"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "ordersendtime")
            //        { s.Append("<td>"); s.Append("发货时间"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName1[z]) == "ordersucesstime")
            //        { s.Append("<td>"); s.Append("结束时间"); s.Append("</td>"); }

            //    }
            //}
            //#endregion

            //#region productstock
            //for (int z = 0; z < ssName2.Length; z++)
            //{
            //    if (ss.Contains(helpcommon.ParmPerportys.stringToLower(ssName2[z])))
            //    {
            //        if (helpcommon.ParmPerportys.stringToLower(ssName2[z]) == "cat")
            //        { s.Append("<td>"); s.Append("品牌"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName2[z]) == "cat2")
            //        { s.Append("<td>"); s.Append("类别"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName2[z]) == "clolor")
            //        { s.Append("<td>"); s.Append("颜色"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName2[z]) == "pricee")
            //        { s.Append("<td>"); s.Append("成本价"); s.Append("</td>"); }
            //    }
            //}
            //#endregion

            //#region activeShopOrder
            //for (int z = 0; z < ssName3.Length; z++)
            //{
            //    if (ss.Contains(helpcommon.ParmPerportys.stringToLower(ssName3[z])))
            //    {

            //    }
            //}
            //#endregion

            //#region activeShop
            //for (int z = 0; z < ssName4.Length; z++)
            //{
            //    if (ss.Contains(helpcommon.ParmPerportys.stringToLower(ssName4[z])))
            //    {
            //        if (helpcommon.ParmPerportys.stringToLower(ssName4[z]) == "acname")
            //        { s.Append("<td>"); s.Append("活动名称"); s.Append("</td>"); }
            //    }
            //}
            //#endregion

            //#region orderComments
            //for (int z = 0; z < ssName5.Length; z++)
            //{
            //    if (ss.Contains(helpcommon.ParmPerportys.stringToLower(ssName5[z])))
            //    {
            //        if (helpcommon.ParmPerportys.stringToLower(ssName5[z]) == "ocremark")
            //        { s.Append("<td>"); s.Append("备注"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName5[z]) == "occomment")
            //        {s.Append("<td>"); s.Append("评价"); s.Append("</td>");}
            //        if (helpcommon.ParmPerportys.stringToLower(ssName5[z]) == "ocbanner")
            //        { s.Append("<td>"); s.Append("旗帜"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName5[z]) == "ocpostprice")
            //        {s.Append("<td>"); s.Append("邮费"); s.Append("</td>");}
            //        if (helpcommon.ParmPerportys.stringToLower(ssName5[z]) == "ocotherprice")
            //        { s.Append("<td>"); s.Append("其他价格"); s.Append("</td>"); }
            //    }
            //}
            //#endregion

            //#region OrderExpress
            //for (int z = 0; z < ssName6.Length; z++)
            //{
            //    if (ss.Contains(helpcommon.ParmPerportys.stringToLower(ssName6[z])))
            //    {
            //        if (helpcommon.ParmPerportys.stringToLower(ssName6[z]) == "expressno")
            //        { s.Append("<td>"); s.Append("运单号"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName6[z]) == "customname")
            //        {s.Append("<td>"); s.Append("姓名"); s.Append("</td>");}
            //        if (helpcommon.ParmPerportys.stringToLower(ssName6[z]) == "customaddress")
            //        { s.Append("<td>"); s.Append("地址"); s.Append("</td>"); }
            //        if (helpcommon.ParmPerportys.stringToLower(ssName6[z]) == "customname")
            //        {s.Append("<td>"); s.Append("快递公司"); s.Append("</td>");}
            //        if (helpcommon.ParmPerportys.stringToLower(ssName6[z]) == "customphone")
            //        { s.Append("<td>"); s.Append("电话"); s.Append("</td>"); }
            //    }
            //}
            //#endregion


            s.Append("<td>编辑</td><td>删除</td>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < ss.Length; j++)
                {
                    if (ss[j].ToLower() == "id")
                    {
                        s.Append("<td>");
                        s.Append("<label id='lblId'>" + dt.Rows[i][ss[j]].ToString() + "</label>");
                        s.Append("</td>");
                    }
                    else
                    {
                        s.Append("<td>");
                        if (ss[j].ToLower() == "paystate")
                        {
                            int keyPayStatus = helpcommon.ParmPerportys.GetNumParms(dt.Rows[i][ss[j]].ToString());
                            switch (keyPayStatus)
                            {
                                case 0:
                                    s.Append("未支付");
                                    break;
                                case 1:
                                    s.Append("已支付");
                                    break;
                            }
                        }
                        else if (ss[j].ToLower() == "orderstate" || ss[j].ToLower() == "orderstate1")
                        {
                            int keyOrderStatus = helpcommon.ParmPerportys.GetNumParms(dt.Rows[i][ss[j]].ToString());
                            switch (keyOrderStatus)
                            {
                                case 1:
                                    s.Append("等待付款");
                                    break;
                                case 2:
                                    s.Append("等待发货");
                                    break;
                                case 3:
                                    s.Append("部分发货");
                                    break;
                                case 4:
                                    s.Append("等待收货");
                                    break;
                                case 6:
                                    s.Append("交易成功");
                                    break;
                                case 7:
                                    s.Append("交易关闭");
                                    break;
                                case 8:
                                    s.Append("交易被淘宝关闭");
                                    break;
                                default:
                                    s.Append("其他状态");
                                    break;
                            }
                        }
                        else if (ss[j].ToLower() == "orderimg")
                        {
                            s.Append("<img onmouseover='show(\"" + dt.Rows[i][ss[j]].ToString() + "\", \"mydiv1\")' onmouseout='hide(\"mydiv1\")' src='" + dt.Rows[i][ss[j]].ToString() + "' alt='' width='40' height='40' />");
                        }
                        else
                        {
                            s.Append(dt.Rows[i][ss[j]].ToString());
                        }

                        s.Append("</td>");
                    }
                }

                #region 编辑
                s.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    s.Append("<a href='#' onclick='javascript: showDivEdit();orderEdit(\"" + dt.Rows[i]["orderchildenid"].ToString() + "\")'>编辑</a>");
                }
                else
                {
                    s.Append("<a href='#'>无编辑权限</a>");
                }
                s.Append("</td>");
                #endregion

                #region 删除
                s.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='del(" + dt.Rows[i][0].ToString() + ")'>删除</a>");
                }
                else
                {
                    s.Append("<a href='#'>无删除权限</a>");
                }

                s.Append("</td>");
                #endregion


                s.Append("</tr>");

            }
            #endregion

            #region 分页
            s.Append("-----");
            s.Append(pageCount + "-----" + count);
            #endregion

            shoporderbll = null;

            return s.ToString();
        }
        #endregion

        #region 添加天猫订单
        /// <summary>
        /// 添加天猫订单
        /// </summary>
        /// <returns></returns>
        public string getTmallOrder()
        {
            string s = string.Empty;

            List<Trade> list = new List<Trade>();
            List<Trade> listAdd = new List<Trade>();
            tmall.orderapp orderTmall = new tmall.orderapp();
            bll.shoporderbll shoporderBll = new bll.shoporderbll();
            DateTime sdate1 = DateTime.Parse(shoporderBll.getDataTime());
            sdate1 = sdate1.AddDays(-sdate1.Day);
            string date3 = sdate1.ToString("yyyy-MM-dd");
            string date1 = shoporderBll.getDataTime();
            string date2 = DateTime.Now.ToString("yyyy-MM-dd");

            int count = userInfo.TaoAppUserList.Count;
            for (int i = 0; i < count; i++)
            {
                orderTmall.Sessionkey = userInfo.TaoAppUserList[i].refreshToken;
                list.AddRange(orderTmall.getSoldTrades(date1, date2));


            }

            for (int i = 0; i < list.Count; i++)
            {
                s += orderTmall.Sessionkey + "|||" + list[i].Tid + "---";
            }
            s += orderTmall.Sessionkey + "MMMMM";
            //return s;

            DataTable dtTime = shoporderBll.getData(date3, date2);
            string[] ss = new string[dtTime.Rows.Count];
            for (int i = 0; i < dtTime.Rows.Count; i++)
            {
                ss[i] = dtTime.Rows[i]["OrderId"].ToString();
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (!ss.Contains(list[i].Tid.ToString()))
                {
                    listAdd.Add(list[i]);
                }
            }

            s += ss.Length + "---" + listAdd.Count;
            if (listAdd.Count < 1)
            {
                return "暂无新增订单!";
            }


            #region 添加
            DataTable dt = new DataTable();
            dt.Columns.Add("OrderId", typeof(String));
            dt.Columns.Add("ShopName", typeof(String));
            dt.Columns.Add("OrderPrice", typeof(String));
            dt.Columns.Add("OrderTime", typeof(String));
            dt.Columns.Add("OrderPayTime", typeof(String));
            dt.Columns.Add("OrderEditTime", typeof(String));
            dt.Columns.Add("OrderSendTime", typeof(String));
            dt.Columns.Add("OrderSucessTime", typeof(String));
            dt.Columns.Add("OrderNick", typeof(String));
            dt.Columns.Add("PayState", typeof(Int64));
            dt.Columns.Add("OrderState", typeof(Int64));
            dt.Columns.Add("OrderState1", typeof(Int64));
            dt.Columns.Add("CustomServerId", typeof(Int64));
            dt.Columns.Add("UserId", typeof(Int64));
            dt.Columns.Add("Def1", typeof(String));
            dt.Columns.Add("Def2", typeof(String));
            dt.Columns.Add("Def3", typeof(String));
            dt.Columns.Add("Def4", typeof(String));
            dt.Columns.Add("Def5", typeof(String));
            for (int i = 0; i < listAdd.Count; i++)
            {
                DataRow drow = dt.NewRow();

                drow["OrderId"] = listAdd[i].Tid;
                drow["ShopName"] = listAdd[i].SellerNick;
                drow["OrderPrice"] = listAdd[i].Payment;
                drow["OrderTime"] = listAdd[i].Created;
                drow["OrderPayTime"] = listAdd[i].PayTime;
                drow["OrderEditTime"] = listAdd[i].Modified;
                drow["OrderSendTime"] = listAdd[i].ConsignTime;
                drow["OrderSucessTime"] = listAdd[i].EndTime;
                drow["OrderNick"] = listAdd[i].BuyerNick;
                drow["PayState"] = 0;  //支付订单状态(0未支付，1已支付)

                //TRADE_NO_CREATE_PAY(没有创建支付宝交易) ---0
                //WAIT_BUYER_PAY(等待买家付款)  ---1
                //WAIT_SELLER_SEND_GOODS(等待卖家发货,即:买家已付款)  ---2
                //SELLER_CONSIGNED_PART（卖家部分发货）  ---3
                //WAIT_BUYER_CONFIRM_GOODS(等待买家确认收货,即:卖家已发货)  ---4
                //TRADE_BUYER_SIGNED(买家已签收,货到付款专用)  ---5
                //TRADE_FINISHED(交易成功)  ---6
                //TRADE_CLOSED(交易关闭)  ---7
                //TRADE_CLOSED_BY_TAOBAO(交易被淘宝关闭)  ---8
                //ALL_WAIT_PAY(包含：WAIT_BUYER_PAY、TRADE_NO_CREATE_PAY)  ---9
                //ALL_CLOSED(包含：TRADE_CLOSED、TRADE_CLOSED_BY_TAOBAO)  ---10
                //WAIT_PRE_AUTH_CONFIRM(余额宝0元购合约中) ---11
                //其他  ---100
                string sValue = listAdd[i].Status;
                switch (sValue)
                {
                    case "TRADE_NO_CREATE_PAY":
                        drow["OrderState"] = 0;
                        drow["OrderState1"] = 0;
                        break;
                    case "WAIT_BUYER_PAY":
                        drow["OrderState"] = 1;
                        drow["OrderState1"] = 1;
                        break;
                    case "WAIT_SELLER_SEND_GOODS":
                        drow["OrderState"] = 2;
                        drow["OrderState1"] = 2;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "SELLER_CONSIGNED_PART":
                        drow["OrderState"] = 3;
                        drow["OrderState1"] = 3;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "WAIT_BUYER_CONFIRM_GOODS":
                        drow["OrderState"] = 4;
                        drow["OrderState1"] = 4;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_BUYER_SIGNED":
                        drow["OrderState"] = 5;
                        drow["OrderState1"] = 5;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_FINISHED":
                        drow["OrderState"] = 6;
                        drow["OrderState1"] = 6;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_CLOSED":
                        drow["OrderState"] = 7;
                        drow["OrderState1"] = 7;
                        break;
                    case "TRADE_CLOSED_BY_TAOBAO":
                        drow["OrderState"] = 8;
                        drow["OrderState1"] = 8;
                        break;
                    case "ALL_WAIT_PAY":
                        drow["OrderState"] = 9;
                        drow["OrderState1"] = 9;
                        break;
                    case "ALL_CLOSED":
                        drow["OrderState"] = 10;
                        drow["OrderState1"] = 10;
                        break;
                    case "WAIT_PRE_AUTH_CONFIRM":
                        drow["OrderState"] = 11;
                        drow["OrderState1"] = 11;
                        break;
                    default:
                        drow["OrderState"] = 100;
                        drow["OrderState1"] = 100;
                        break;
                }


                //drow["PayState"] = 0;
                //drow["OrderState"] = 0;
                //drow["OrderState1"] = 0;
                drow["CustomServerId"] = 0;
                drow["UserId"] = userInfo.User.Id;
                drow["Def1"] = "";
                drow["Def2"] = "";
                drow["Def3"] = "";
                drow["Def4"] = "";
                drow["Def5"] = "";

                dt.Rows.Add(drow);
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            SqlBulkCopy sbc = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepIdentity, transaction);
            sbc.BatchSize = dt.Rows.Count;
            sbc.DestinationTableName = "porder";

            try
            {
                sbc.WriteToServer(dt);
                transaction.Commit();


                s = "添加成功!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                s = "添加失败!";
            }

            #endregion


            return s;
        }
        #endregion

        #region 更新天猫订单
        public string strParent = string.Empty;
        /// <summary>
        /// 更新天猫订单（三个月以内增量订单）
        /// </summary>
        /// <returns></returns>
        public string updateTmallOrder()
        {
            getParentThread();
            Thread.Sleep(5000);
            return strParent;
        }

        /// <summary>
        /// 更新父订单
        /// </summary>
        /// <param name="list"></param>
        public void updateParentOrder(List<Trade> mylist)
        {
            List<Trade> list = new List<Trade>();

            string s = string.Empty;
            ///主订单插入
            List<Trade> listParentInsert = new List<Trade>();
            ///主订单更新
            List<Trade> listParentUpdate = new List<Trade>();
            ///子订单插入
            List<Trade> listChildInsert = new List<Trade>();
            ///子订单更新
            List<Trade> listChildUpdate = new List<Trade>();

            tmall.orderapp orderTmall = new tmall.orderapp();
            bll.shoporderbll shoporderBll = new bll.shoporderbll();



            string dtime1 = DateTime.Now.ToString("yyyy-MM-dd");
            string dtime2 = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            string[] ssChild = shoporderBll.getDataChildOrderId(dtime2, dtime1);  //某段时间内的子订单
            string[] ssParent = shoporderBll.getOrderId(dtime2, dtime1); //某段时间内的主订单

            #region 去重
            for (int i = 0; i < mylist.Count; i++)  //外循环是循环的次数
            {
                for (int j = mylist.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {

                    if (mylist[i].Tid == mylist[j].Tid)
                    {
                        mylist.RemoveAt(j);
                    }
                }
            }
            #endregion


            list.AddRange(mylist);


            for (int i = 0; i < list.Count; i++)
            {
                #region 主订单
                if (ssParent.Contains(list[i].Tid.ToString())) //主订单
                {
                    //update
                    if (!listParentUpdate.Contains(list[i]))
                    {
                        listParentUpdate.Add(list[i]);
                    }
                }
                else
                {
                    //insert
                    if (!listParentInsert.Contains(list[i]))
                    {
                        listParentInsert.Add(list[i]);
                    }
                }
                #endregion

                #region 子订单
                List<Order> listOrderCount = new List<Order>();
                listOrderCount = list[i].Orders;
                if (listOrderCount.Count > 1) //子订单>1
                {
                    for (int n = 0; n < listOrderCount.Count; n++)
                    {
                        if (ssChild.Contains(listOrderCount[n].Oid.ToString()))
                        {
                            //update
                            if (!listChildUpdate.Contains(list[i]))
                            {
                                listChildUpdate.Add(list[i]);
                            }
                        }
                        else
                        {
                            //insert
                            if (!listChildInsert.Contains(list[i]))
                            {
                                listChildInsert.Add(list[i]);
                            }
                        }
                    }

                }
                else //子订单=1
                {
                    if (ssChild.Contains(list[i].Orders[0].Oid.ToString()))
                    {
                        //update
                        if (!listChildUpdate.Contains(list[i]))
                        {
                            listChildUpdate.Add(list[i]);
                        }
                    }
                    else
                    {
                        //insert
                        if (!listChildInsert.Contains(list[i]))
                        {
                            listChildInsert.Add(list[i]);
                        }
                    }

                }
                #endregion

            }


            #region 主订单插入前去重
            for (int w = listParentInsert.Count-1; w >=0 ; w--)
            {
                if (ssParent.Contains(listParentInsert[w].Tid.ToString()))
                {
                    listParentUpdate.Add(listParentInsert[w]);
                    listParentInsert.RemoveAt(w);
                }

                DateTime dateCreate = helpcommon.ParmPerportys.GetDateTimeParms(listParentInsert[w].Created);
                DateTime dateEnd = DateTime.Now;
                int months = (dateEnd.Year - dateCreate.Year) * 12 + (dateEnd.Month - dateCreate.Month); //获取两个时间相差月份
                if (months > 2)
                {
                    listParentUpdate.Add(listParentInsert[w]);
                    listParentInsert.RemoveAt(w);
                }
            }
            #endregion
            #region 主订单添加
            DataTable dtParent = new DataTable();
            dtParent.Columns.Add("OrderId", typeof(String));
            dtParent.Columns.Add("ShopName", typeof(String));
            dtParent.Columns.Add("OrderPrice", typeof(String));
            dtParent.Columns.Add("OrderTime", typeof(String));
            dtParent.Columns.Add("OrderPayTime", typeof(String));
            dtParent.Columns.Add("OrderEditTime", typeof(String));
            dtParent.Columns.Add("OrderSendtParentime", typeof(String));
            dtParent.Columns.Add("OrderSucessTime", typeof(String));
            dtParent.Columns.Add("OrderNick", typeof(String));
            dtParent.Columns.Add("PayState", typeof(Int64));
            dtParent.Columns.Add("OrderState", typeof(Int64));
            dtParent.Columns.Add("OrderState1", typeof(Int64));
            dtParent.Columns.Add("CustomServerId", typeof(Int64));
            dtParent.Columns.Add("UserId", typeof(Int64));
            dtParent.Columns.Add("Def1", typeof(String));
            dtParent.Columns.Add("Def2", typeof(String));
            dtParent.Columns.Add("Def3", typeof(String));
            dtParent.Columns.Add("Def4", typeof(String));
            dtParent.Columns.Add("Def5", typeof(String));
            for (int i = 0; i < listParentInsert.Count; i++)
            {
                DataRow drow = dtParent.NewRow();

                drow["OrderId"] = listParentInsert[i].Tid;
                drow["ShopName"] = listParentInsert[i].SellerNick;
                drow["OrderPrice"] = listParentInsert[i].Payment;
                drow["OrderTime"] = listParentInsert[i].Created;
                drow["OrderPayTime"] = listParentInsert[i].PayTime;
                drow["OrderEditTime"] = listParentInsert[i].Modified;
                drow["OrderSendtParentime"] = listParentInsert[i].ConsignTime;
                drow["OrderSucessTime"] = listParentInsert[i].EndTime;
                drow["OrderNick"] = listParentInsert[i].BuyerNick;
                drow["PayState"] = 0;  //支付订单状态(0未支付，1已支付)

                //TRADE_NO_CREATE_PAY(没有创建支付宝交易) ---0
                //WAIT_BUYER_PAY(等待买家付款)  ---1
                //WAIT_SELLER_SEND_GOODS(等待卖家发货,即:买家已付款)  ---2
                //SELLER_CONSIGNED_PART（卖家部分发货）  ---3
                //WAIT_BUYER_CONFIRM_GOODS(等待买家确认收货,即:卖家已发货)  ---4
                //TRADE_BUYER_SIGNED(买家已签收,货到付款专用)  ---5
                //TRADE_FINISHED(交易成功)  ---6
                //TRADE_CLOSED(交易关闭)  ---7
                //TRADE_CLOSED_BY_TAOBAO(交易被淘宝关闭)  ---8
                //ALL_WAIT_PAY(包含：WAIT_BUYER_PAY、TRADE_NO_CREATE_PAY)  ---9
                //ALL_CLOSED(包含：TRADE_CLOSED、TRADE_CLOSED_BY_TAOBAO)  ---10
                //WAIT_PRE_AUTH_CONFIRM(余额宝0元购合约中) ---11
                //其他  ---100
                string sValue = listParentInsert[i].Status;
                switch (sValue)
                {
                    case "TRADE_NO_CREATE_PAY":
                        drow["OrderState"] = 0;
                        drow["OrderState1"] = 0;
                        break;
                    case "WAIT_BUYER_PAY":
                        drow["OrderState"] = 1;
                        drow["OrderState1"] = 1;
                        break;
                    case "WAIT_SELLER_SEND_GOODS":
                        drow["OrderState"] = 2;
                        drow["OrderState1"] = 2;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "SELLER_CONSIGNED_PART":
                        drow["OrderState"] = 3;
                        drow["OrderState1"] = 3;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "WAIT_BUYER_CONFIRM_GOODS":
                        drow["OrderState"] = 4;
                        drow["OrderState1"] = 4;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_BUYER_SIGNED":
                        drow["OrderState"] = 5;
                        drow["OrderState1"] = 5;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_FINISHED":
                        drow["OrderState"] = 6;
                        drow["OrderState1"] = 6;
                        drow["PayState"] = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_CLOSED":
                        drow["OrderState"] = 7;
                        drow["OrderState1"] = 7;
                        break;
                    case "TRADE_CLOSED_BY_TAOBAO":
                        drow["OrderState"] = 8;
                        drow["OrderState1"] = 8;
                        break;
                    case "ALL_WAIT_PAY":
                        drow["OrderState"] = 9;
                        drow["OrderState1"] = 9;
                        break;
                    case "ALL_CLOSED":
                        drow["OrderState"] = 10;
                        drow["OrderState1"] = 10;
                        break;
                    case "WAIT_PRE_AUTH_CONFIRM":
                        drow["OrderState"] = 11;
                        drow["OrderState1"] = 11;
                        break;
                    default:
                        drow["OrderState"] = 100;
                        drow["OrderState1"] = 100;
                        break;
                }


                //drow["PayState"] = 0;
                //drow["OrderState"] = 0;
                //drow["OrderState1"] = 0;
                drow["CustomServerId"] = 0;
                drow["UserId"] = userInfo.User.Id;
                drow["Def1"] = "";
                drow["Def2"] = "";
                drow["Def3"] = "";
                drow["Def4"] = "";
                drow["Def5"] = "";

                dtParent.Rows.Add(drow);
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            SqlBulkCopy sbc = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepIdentity, transaction);
            sbc.BatchSize = dtParent.Rows.Count;
            sbc.DestinationTableName = "porder";

            try
            {
                sbc.WriteToServer(dtParent);
                transaction.Commit();

                s = "添加成功!";
                strParent = "添加成功!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                s = "添加失败!";
                strParent = "添加失败!";
            }
            #endregion


            #region 主订单更新
            for (int i = 0; i < listParentUpdate.Count; i++)
            {
                int payState = 0;
                int orderState = 0;

                string sValue = listParentUpdate[i].Status;
                switch (sValue)
                {
                    case "TRADE_NO_CREATE_PAY":
                        orderState = 0;
                        break;
                    case "WAIT_BUYER_PAY":
                        orderState = 1;
                        break;
                    case "WAIT_SELLER_SEND_GOODS":
                        orderState = 2;
                        payState = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "SELLER_CONSIGNED_PART":
                        orderState = 3;
                        payState = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "WAIT_BUYER_CONFIRM_GOODS":
                        orderState = 4;
                        payState = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_BUYER_SIGNED":
                        orderState = 5;
                        payState = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_FINISHED":
                        orderState = 6;
                        payState = 1;  //支付订单状态(0未支付，1已支付)
                        break;
                    case "TRADE_CLOSED":
                        orderState = 7;
                        break;
                    case "TRADE_CLOSED_BY_TAOBAO":
                        orderState = 8;
                        break;
                    case "ALL_WAIT_PAY":
                        orderState = 9;
                        break;
                    case "ALL_CLOSED":
                        orderState = 10;
                        break;
                    case "WAIT_PRE_AUTH_CONFIRM":
                        orderState = 11;
                        break;
                    default:
                        orderState = 100;
                        break;
                }

                s = shoporderBll.updateTmallOrder(
                    listParentUpdate[i].Tid.ToString(),
                    helpcommon.ParmPerportys.GetDecimalParms(listParentUpdate[i].Payment),
                    helpcommon.ParmPerportys.GetDateTimeParms(listParentUpdate[i].Created),
                    helpcommon.ParmPerportys.GetDateTimeParms(listParentUpdate[i].PayTime),
                    helpcommon.ParmPerportys.GetDateTimeParms(listParentUpdate[i].Modified),
                    helpcommon.ParmPerportys.GetDateTimeParms(listParentUpdate[i].ConsignTime),
                    helpcommon.ParmPerportys.GetDateTimeParms(listParentUpdate[i].EndTime),
                    payState, orderState, orderState);
            }

            if (s == "更新失败")
            {
                strParent = s;
            }
            else
            {
                strParent = "更新成功！";
            }

            #endregion


            #region 子订单插入前去重
            for (int w = listChildInsert.Count - 1; w >= 0; w--)
            {
                if (ssChild.Contains(listChildInsert[w].Tid.ToString()))
                {
                    listChildUpdate.Add(listChildInsert[w]);
                    listChildInsert.RemoveAt(w);
                }

                DateTime dateCreate = helpcommon.ParmPerportys.GetDateTimeParms(listChildInsert[w].Created);
                DateTime dateEnd = DateTime.Now;
                int months = (dateEnd.Year - dateCreate.Year) * 12 + (dateEnd.Month - dateCreate.Month); //获取两个时间相差月份
                if (months>2)
                {
                    listChildUpdate.Add(listChildInsert[w]);
                    listChildInsert.RemoveAt(w);
                }


            }
            #endregion
            #region 子订单添加
            DataTable dt = new DataTable();

            dt.Columns.Add("OrderId", typeof(String));
            dt.Columns.Add("OrderChildenId", typeof(String));
            dt.Columns.Add("ProductId", typeof(String));
            dt.Columns.Add("DetailsName", typeof(String));
            dt.Columns.Add("OrderScode", typeof(String));
            dt.Columns.Add("OrderColor", typeof(String));
            dt.Columns.Add("OrderImg", typeof(String));
            dt.Columns.Add("DetailsSum", typeof(Int64));
            dt.Columns.Add("DetailsPrice", typeof(String));
            dt.Columns.Add("OrderTime", typeof(String));
            dt.Columns.Add("OrderPayTime", typeof(String));
            dt.Columns.Add("OrderEditTime", typeof(String));
            dt.Columns.Add("OrderSendTime", typeof(String));
            dt.Columns.Add("OrderSucessTime", typeof(String));
            dt.Columns.Add("Def1", typeof(String));
            dt.Columns.Add("Def2", typeof(String));
            dt.Columns.Add("Def3", typeof(String));
            dt.Columns.Add("Def4", typeof(String));
            dt.Columns.Add("Def5", typeof(String));

            for (int i = 0; i < listChildInsert.Count; i++)
            {
                List<Order> listChildOrders = new List<Order>();
                listChildOrders.AddRange(listChildInsert[i].Orders);
                for (int j = 0; j < listChildOrders.Count; j++)
                {
                    DataRow drow = dt.NewRow();

                    drow["OrderId"] = listChildInsert[i].Tid.ToString();
                    drow["OrderChildenId"] = listChildOrders[j].Oid.ToString();
                    drow["ProductId"] = listChildOrders[j].NumIid.ToString();
                    drow["DetailsName"] = listChildOrders[j].Title;
                    drow["OrderScode"] = listChildOrders[j].OuterSkuId;
                    drow["OrderColor"] = listChildOrders[j].SkuPropertiesName;
                    drow["OrderImg"] = listChildOrders[j].PicPath;
                    drow["DetailsSum"] = listChildOrders[j].Num;
                    drow["DetailsPrice"] = listChildOrders[j].Payment;
                    drow["OrderTime"] = listChildInsert[i].Created;
                    drow["OrderPayTime"] = listChildInsert[i].PayTime;
                    drow["OrderEditTime"] = listChildInsert[i].Modified;
                    drow["OrderSendTime"] = listChildOrders[j].ConsignTime;
                    drow["OrderSucessTime"] = listChildOrders[j].EndTime;
                    drow["Def1"] = "";
                    drow["Def2"] = "";
                    drow["Def3"] = "";
                    drow["Def4"] = "";
                    drow["Def5"] = "";

                    dt.Rows.Add(drow);
                }
            }

            SqlConnection conChild = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            conChild.Open();
            SqlTransaction transactionChild = conChild.BeginTransaction();
            SqlBulkCopy sbcChild = new SqlBulkCopy(conChild, SqlBulkCopyOptions.KeepIdentity, transactionChild);
            sbcChild.BatchSize = dt.Rows.Count;
            sbcChild.DestinationTableName = "OrderDetails";

            try
            {
                if (dt.Rows.Count < 1)
                {
                    strParent = "暂未新增订单!";
                }
                else
                {
                    sbcChild.WriteToServer(dt);
                    transactionChild.Commit();

                    strParent = "添加成功!";
                }
            }
            catch (Exception ex)
            {
                transactionChild.Rollback();

                strParent = ex.Message + "添加失败!";
            }

            #endregion


            #region 子订单更新
            for (int i = 0; i < listChildUpdate.Count; i++)
            {
                List<Order> listChildOrders = new List<Order>();
                listChildOrders.AddRange(listChildUpdate[i].Orders);
                for (int m = 0; m < listChildOrders.Count; m++)
                {
                    s = shoporderBll.updateChildTmallOrder(
                    listChildOrders[m].Oid.ToString(),
                    helpcommon.ParmPerportys.GetDecimalParms(listChildOrders[m].Payment),
                    helpcommon.ParmPerportys.GetDateTimeParms(listChildOrders[m].ConsignTime),
                    helpcommon.ParmPerportys.GetDateTimeParms(listChildOrders[m].EndTime));
                }

                if (s == "更新失败")
                {
                    strParent = s;
                }
                else
                {
                    strParent = "更新成功";
                }
            }
            #endregion


        }

        /// <summary>
        /// 一段时间内所有的父订单量
        /// </summary>
        public List<Trade> listAllTrade = new List<Trade>();

        /// <summary>
        /// 三个月的订单更新平均拆分为10天
        /// </summary>
        public void getParentThread()
        {
            tmall.orderapp orderTmall = new tmall.orderapp();
            List<Trade> list = new List<Trade>();

            DateTime d3 = DateTime.Now;
            DateTime d2 = DateTime.Now; //最新时间
            DateTime d1 = DateTime.Now;//最早时间
            Thread[] td = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                lock (this)
                {
                    d2 = d3.AddDays(-i * 9);
                    d1 = d3.AddDays(-(i + 1) * 9);
                    string date1 = d1.ToString("yyyy-MM-dd") + " 00:00:00";//最早时间
                    string date2 = d2.ToString("yyyy-MM-dd") + " 23:59:59";//最新时间
                    OrderParms pv = new OrderParms(date1, date2);
                    pv.Data1 = date1;
                    pv.Data2 = date2;
                    pv.reParentMsg += pv_reParentMsg;
                    pv.nofity();
                    td[i] = new Thread(new ParameterizedThreadStart(refundTrade));
                    td[i].Start(pv);
                }

            }

            for (int j = 0; j < 10; j++)
            {
                if (td[j].ThreadState == ThreadState.Stopped)
                {
                    td[j].Abort();
                }
            }

            updateParentOrder(listAllTrade);
        }

        /// <summary>
        /// 返回每个平均时间段的订单量
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <returns></returns>
        List<Trade> pv_reParentMsg(string data1, string data2)
        {
            List<Trade> list = new List<Trade>();
            tmall.orderapp orderTmall = new tmall.orderapp();


            int count = userInfo.TaoAppUserList.Count;
            for (int m = 0; m < count; m++)
            {
                orderTmall.Sessionkey = userInfo.TaoAppUserList[m].refreshToken;
                list.AddRange(orderTmall.getSoldIncrements(data1, data2));
            }
            listAllTrade.AddRange(list);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void refundTrade(object obj)
        {
            OrderParms o = (OrderParms)obj;
            pv_reParentMsg(o.Data1, o.Data2);
        }

        #endregion


        #region 订单编辑
        /// <summary>
        /// 根据订单ID,商品scode获取此商品相关信息
        /// </summary>
        /// <returns></returns>
        public string orderEdit()
        {
            //string s = string.Empty;
            int menuId = Request.Form["MenuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["MenuId"]) : 0;
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            PublicHelpController ph = new PublicHelpController();
            //int roleId = helpcommon.ParmPerportys.GetNumParms(Request.Form["roleId"]);
            //int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string orderchildenid = helpcommon.ParmPerportys.GetStrParms(Request.Form["orderchildenid"]);
            //string scode = helpcommon.ParmPerportys.GetStrParms(Request.Form["scode"]);
            StringBuilder s = new StringBuilder();
            //PublicHelpController ph = new PublicHelpController();
            //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.updateName);
            string Pricea = "", Priceb = "", Pricec = "", Priced = "", Pricee = "", CustomerName = "";
            bll.shoporderbll shoporderBll = new bll.shoporderbll();
            DataTable dt = shoporderBll.orderEdit(orderchildenid);
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectProductPrice);
            for (int i = 0; i < ss.Length; i++)
            {
                if (ss[i].Contains("Pricea"))
                {
                    Pricea = dt.Rows[0]["Pricea"].ToString();
                }
                if (ss[i].Contains("Priceb"))
                {
                    Priceb = dt.Rows[0]["Priceb"].ToString();
                }
                if (ss[i].Contains("Pricec"))
                {
                    Pricec = dt.Rows[0]["Pricec"].ToString();
                }
                if (ss[i].Contains("Priced"))
                {
                    Priced = dt.Rows[0]["Priced"].ToString();
                }
                if (ss[i].Contains("Pricee"))
                {
                    Pricee = dt.Rows[0]["Pricee"].ToString();
                }
            }
            if (dt.Rows[0]["CustomServerId"].ToString() != "0")
            {
                CustomerName = dt.Rows[0]["CustomerName"].ToString();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                s.Append("<div style='width: 100%; height: 40px; float: left; line-height: 40px; font-weight: bolder; text-indent: 2em'>订单基本信息</div><div style='width: 620px; margin: auto;'>");
                s.Append("<table><tr><td><div class='div'><span class='span'>店铺名称：</span><input class='iptCss' disabled='disabled' id='orderShop1' value=\"" + dt.Rows[0]["ShopName"].ToString() + "\" /></div></td>");
                s.Append("<td><div class='div'><span class='span'>订单编号：</span><input class='iptCss' type='text' id='orderId1' name='orderId1' disabled='disabled' value=\"" + dt.Rows[0]["OrderId"].ToString() + "\" /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>销售方式：</span><select id='orderXSFS1'><option value=''></option><option value='刷单'>刷单</option><option value='正常销售'>正常销售</option></select></div></td>");
                s.Append("<td><div class='div'><span class='span'>所属客服：</span><input class='iptCss' disabled='disabled' type='text' id='orderSSKF1' name='orderSSKF1' value=\"" + CustomerName + "\" /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>买家昵称：</span><input class='iptCss' disabled='disabled' type='text' id='orderMJNC1' name='orderMJNC1' value=\"" + dt.Rows[0]["OrderNick"].ToString() + "\" /></div></td>");
                s.Append("<td><div class='div'><span class='span'>快递公司：</span><input class='iptCss' type='text' id='CourierCompanies1' name='CourierCompanies1' value=\"" + dt.Rows[0]["ExpressName"].ToString() + "\" /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>快递编号：</span><input class='iptCss' type='text' id='CourierNo1' name='CourierNo1' value=\"" + dt.Rows[0]["ExpressNo"].ToString() + "\" /></div></td>");
                s.Append("<td rowspan='3'><div class='div'><span class='span'>备注：</span><textarea class='iptCss' id='orderRemark' name='orderRemark' style='height: 75px'>" + dt.Rows[0]["ocRemark"].ToString() + " </textarea></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>邮费：</span><input class='iptCss' type='text' id='orderPostage1' name='orderPostage1' value=\"" + dt.Rows[0]["ocPostPrice"].ToString() + "\" /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>其他费用：</span><input class='iptCss' type='text' id='orderOtherMoney1' name='orderOtherMoney1' value=\"" + dt.Rows[0]["ocOtherPrice"].ToString() + "\" /></div></td></tr></table></div>");
                s.Append("<div style='width: 100%; height: 40px; float: left; line-height: 40px; font-weight: bolder; text-indent: 2em'>成本及费用</div><div style='width: 620px; margin: auto;'>");
                s.Append("<table><tr><td><div class='div'><span class='span'>货号：</span><input class='iptCss' id='orderScode1'  value=\"" + dt.Rows[0]["OrderScode"].ToString() + "\"  /></div></td>");
                s.Append("<td><div class='div'><span class='span'>品牌：</span><input class='iptCss' id='orderBrand1' disabled='disabled' value=\"" + dt.Rows[0]["Cat"].ToString() + "\"  /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>类别：</span><input class='iptCss' id='orderType1' disabled='disabled' value=\"" + dt.Rows[0]["Cat2"].ToString() + "\"  /></div></td>");
                s.Append("<td><div class='div'><span class='span'>颜色：</span><input class='iptCss' id='orderColor1' disabled='disabled'  value=\"" + dt.Rows[0]["Clolor"].ToString() + "\"  /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>订单来源：</span><input class='iptCss' disabled='disabled' id='orderForm1'/></div></td>");
                s.Append("<td><div class='div'><span class='span'>成交金额：</span><input class='iptCss' disabled='disabled' id='orderSucessPrice1'  value=\"" + dt.Rows[0]["DetailsPrice"].ToString() + "\"  /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>吊牌价：</span><input class='iptCss' disabled='disabled' id='orderCostPrice1'  value=\"" + Pricea + "\"  /></div></td>");
                s.Append("<td><div class='div'><span class='span'>零售价：</span><input class='iptCss' disabled='disabled' id='orderCostPrice1'  value=\"" + Priceb + "\"  /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>VIP价：</span><input class='iptCss' disabled='disabled' id='orderCostPrice1'  value=\"" + Pricec + "\"  /></div></td>");
                s.Append("<td><div class='div'><span class='span'>批发价：</span><input class='iptCss' disabled='disabled' id='orderCostPrice1'  value=\"" + Priced + "\"  /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>成本价：</span><input class='iptCss' disabled='disabled' id='orderCostPrice1'  value=\"" + Pricee + "\"  /></div></td>");
                s.Append("<td rowspan='3'><div class='div'><span class='span'>缩略图：</span><img id='OrderImg' src=\"" + dt.Rows[0]["OrderImg"].ToString() + "\" style='width:40px;height:40px' /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>平台扣点：</span><input class='iptCss' disabled='disabled' type='text' id='orderPTKD1' name='orderPTKD1'  value='' /></div></td></tr>");
                s.Append("<tr><td><div class='div'><span class='span'>出货仓库：</span><select id='orderCHCK1' ><option value=''></option><option value='香港仓库'>香港仓库</option><option value='深圳仓库'>深圳仓库</option><option value='退货仓库'>退货仓库</option><option value='顺丰仓库'>顺丰仓库</option></select></div></td></tr></table></div>");
                //s.Append("<div style='width: 100%; height: 40px; float: left; line-height: 40px; font-weight: bolder; text-indent: 2em'>订单基本信息</div><div>");
                //s.Append("<div class='div'><span class='span'>店铺名称：</span><input class='iptCss' disabled='disabled' id='orderShop1' value="+dt.Rows[0]["ShopName"].ToString()+" /></div>");
                //s.Append("<div class='div'><span class='span'>订单编号：</span><input class='iptCss' type='text' id='orderId1' name='orderId1' disabled='disabled' value=" + dt.Rows[0]["OrderId"].ToString() + " /></div>");
                //s.Append("<div class='div'><span class='span'>销售方式：</span><select id='orderXSFS1' style='width: 250px;'><option value=''></option><option value='刷单'>刷单</option><option value='正常销售'>正常销售</option></select></div>");
                //s.Append("<div class='div'><span class='span'>所属客服：</span><select id='orderSSKF1' style='width: 250px;'></select></div>");
                //s.Append("<div class='div'><span class='span'>买家昵称：</span><input class='iptCss' disabled='disabled' type='text' id='orderMJNC1' name='orderMJNC1' value=" + dt.Rows[0]["OrderNick"].ToString() + " /></div>");
                //s.Append("<div class='div'><span class='span'>快递公司：</span><input class='iptCss' type='text' id='CourierCompanies1' name='CourierCompanies1' value=" + dt.Rows[0]["ExpressName"].ToString() + " /></div>");
                //s.Append("<div class='div'><span class='span'>快递编号：</span><input class='iptCss' type='text' id='CourierNo1' name='CourierNo1' value=" + dt.Rows[0]["ExpressNo"].ToString() + " /></div>");
                //s.Append("<div class='div'><span class='span'>邮费：</span><input class='iptCss' type='text' id='orderPostage1' name='orderPostage1' value=" + dt.Rows[0]["ocPostPrice"].ToString() + " /></div>");
                //s.Append("<div class='div'><span class='span'>其他费用：</span><input class='iptCss' type='text' id='orderOtherMoney1' name='orderOtherMoney1' value=" + dt.Rows[0]["ocOtherPrice"].ToString() + " /></div>");
                //s.Append("<div class='div'><span class='span'>备注：</span><textarea class='iptCss' id='orderRemark' name='orderRemark'>" + dt.Rows[0]["ocRemark"].ToString() + " </textarea></div>");
                //s.Append("</div>");
                //s.Append("<div style='width: 100%; height: 40px; float: left; line-height: 40px; font-weight: bolder; text-indent: 2em'>成本及费用</div><div>");
                //s.Append("<div class='div'><span class='span'>货号：</span><input class='iptCss' id='orderScode1'  value=" + dt.Rows[0]["OrderScode"].ToString() + "  /></div>");
                //s.Append("<div class='div'><span class='span'>品牌：</span><input class='iptCss' id='orderBrand1'  value=" + dt.Rows[0]["Cat"].ToString() + "  /></></div>");
                //s.Append("<div class='div'><span class='span'>类别：</span><input class='iptCss' id='orderType1'  value=" + dt.Rows[0]["Cat2"].ToString() + "  /></div>");
                //s.Append("<div class='div'><span class='span'>颜色：</span><input class='iptCss' id='orderColor1'  value=" + dt.Rows[0]["Clolor"].ToString() + "  /></div>");
                //s.Append("<div class='div'><span class='span'>订单来源：</span><input class='iptCss' disabled='disabled' id='orderForm1'/></div>");
                //s.Append("<div class='div'><span class='span'>成交金额：</span><input class='iptCss' disabled='disabled' id='orderSucessPrice1'  value=" + dt.Rows[0]["DetailsPrice"].ToString() + "  /></div>");
                //s.Append("<div class='div'><span class='span'>成本价：</span><input class='iptCss' disabled='disabled' id='orderCostPrice1'  value=" + dt.Rows[0]["Pricee"].ToString() + "  /></div>");
                //s.Append("<div class='div'><span class='span'>平台扣点：</span><input class='iptCss' disabled='disabled' type='text' id='orderPTKD1' name='orderPTKD1'  value=" + dt.Rows[0]["costPrice"].ToString() + "  /></div>");
                //s.Append("<div class='div'><span class='span'>出货仓库：</span><select id='orderCHCK1' style='width: 250px;'><option value=''></option><option value='香港仓库'>香港仓库</option><option value='深圳仓库'>深圳仓库</option><option value='退货仓库'>退货仓库</option><option value='顺丰仓库'>顺丰仓库</option></select></div>");
                //s.Append("</div>");

                //s.Append("{\"orderProperty\":[");

                //s.Append("{\"shopName\":\"" + dt.Rows[0]["ShopName"].ToString() + "\"},");//店铺名称
                //s.Append("{\"orderId\":\"" + dt.Rows[0]["OrderId"].ToString() + "\"},");//订单编号
                //s.Append("{\"saleMethod\":\"" + "" + "\"},");//销售方式------------暂未用到
                //s.Append("{\"CustomServerId\":\"" + dt.Rows[0]["CustomServerId"].ToString() + "\"},");//所属客服
                //s.Append("{\"buyNick\":\"" + dt.Rows[0]["OrderNick"].ToString() + "\"},");//买家昵称
                //s.Append("{\"expressName\":\"" + dt.Rows[0]["ExpressName"].ToString() + "\"},");//快递公司
                //s.Append("{\"expressNo\":\"" + dt.Rows[0]["ExpressNo"].ToString() + "\"},");//快递编号
                //s.Append("{\"postPrice\":\"" + dt.Rows[0]["ocPostPrice"].ToString() + "\"},");//邮费
                //s.Append("{\"otherPrice\":\"" + dt.Rows[0]["ocOtherPrice"].ToString() + "\"},");//其他价格
                //s.Append("{\"remark\":\"" + dt.Rows[0]["ocRemark"].ToString() + "\"},"); //备注

                //s.Append("{\"scode\":\"" + dt.Rows[0]["Scode"].ToString() + "\"},"); //货号
                //s.Append("{\"brand\":\"" + dt.Rows[0]["Cat"].ToString() + "\"},");//品牌
                //s.Append("{\"type\":\"" + dt.Rows[0]["Cat2"].ToString() + "\"},");//类别
                //s.Append("{\"color\":\"" + dt.Rows[0]["Clolor"].ToString() + "\"},");//颜色
                //s.Append("{\"orderFrom\":\"" + "" + "\"},");//订单来源------------暂未用到
                //s.Append("{\"price\":\"" + dt.Rows[0]["DetailsPrice"].ToString() + "\"},");//成交价
                //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectProductPrice);
                //for (int i = 0; i < ss.Length; i++)
                //{
                //    if (ss[i].Contains("Pricea"))
                //    {
                //        Pricea = dt.Rows[0]["Pricea"].ToString();
                //    }
                //    if (ss[i].Contains("Priceb"))
                //    {
                //        Priceb = dt.Rows[0]["Priceb"].ToString();
                //    }
                //    if (ss[i].Contains("Pricec"))
                //    {
                //        Pricec = dt.Rows[0]["Pricec"].ToString();
                //    }
                //    if (ss[i].Contains("Priced"))
                //    {
                //        Priced = dt.Rows[0]["Priced"].ToString();
                //    }
                //    if (ss[i].Contains("Pricee"))
                //    {
                //        Pricee = dt.Rows[0]["Pricee"].ToString();
                //    }
                //}
                //s.Append("{\"orderPricea\":\"" + Pricea + "\"},");//--吊牌价
                //s.Append("{\"orderPriceb\":\"" + Priceb + "\"},");//--零售价
                //s.Append("{\"orderPricec\":\"" + Pricec + "\"},");//--VIP价
                //s.Append("{\"orderPriced\":\"" + Priced + "\"},");//--批发价
                //s.Append("{\"orderPricee\":\"" + Pricee + "\"},");//成本价  
                //s.Append("{\"plantPrice\":\"" + "" + "\"},");//平台扣点------------暂未用到
                //s.Append("{\"productSource\":\"" + "" + "\"},");//出库源头------------暂未用到
                //s.Append("{\"image\":\"" + dt.Rows[0]["OrderImg"].ToString() + "\"},");//根据货号缩略图
                //s.Append("{\"CustomerName\":\"" + dt.Rows[0]["CustomerName"].ToString() + "\"}");//客服名称
                //s.Append("]}");

            }
            else
            {
                s.Append("{\"error\":\"错误\"}");
            }


            return s.ToString();
        }

        /// <summary>
        /// 根据子订单ID获取此商品相关信息
        /// </summary>
        public string GetCustomerDDlist(string Name)
        {
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            try
            {
                if (Name != "")
                {
                    s += "<option selected='selected' value='-1'>" + Name + "</option>";
                }
                else
                {
                    s += "<option value=''>请选择</option>";
                    string sql = @"select * from users where personaId='4'";
                    dt1 = DbHelperSQL.Query(sql).Tables[0];
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i]["userRealName"].ToString() == Name)
                        {
                            s += "<option selected='selected' value='" + dt1.Rows[i]["Id"] + "'>" + dt1.Rows[i]["userRealName"] + "</option>";
                        }
                        else
                        {
                            s += "<option value='" + dt1.Rows[i]["Id"] + "'>" + dt1.Rows[i]["userRealName"] + "</option>";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }

            return s;
        }
        /// <summary>
        /// 根据子订单ID修改订单相关信息
        /// </summary>
        /// <returns></returns>
        public string orderUpdate()
        {
            string s = string.Empty;
            string a = Request.Form["orderSSKF1"].ToString();
            string CustomerId = Request.Form["orderSSKF1"].ToString() == "" ? userInfo.User.Id.ToString() : "-1";
            string orderchildenid = Request.Form["orderchildenid"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("orderid1", Request.Form["orderid1"].ToString());
            Dic.Add("orderXSFS1", Request.Form["orderXSFS1"].ToString());
            Dic.Add("orderSSKF1", CustomerId);
            Dic.Add("CourierCompanies1", Request.Form["CourierCompanies1"].ToString());
            Dic.Add("CourierNo1", Request.Form["CourierNo1"].ToString());
            Dic.Add("orderPostage1", Request.Form["orderPostage1"].ToString());
            Dic.Add("orderOtherMoney1", Request.Form["orderOtherMoney1"].ToString());
            Dic.Add("orderRemark", Request.Form["orderRemark"].ToString());
            Dic.Add("orderScode1", Request.Form["orderScode1"].ToString());
            Dic.Add("orderCHCK1", Request.Form["orderCHCK1"].ToString());
            Dic.Add("UserId", userInfo.User.Id.ToString());

            //string orderXSFS1 = Request.Form["orderXSFS1"].ToString();
            //string orderSSKF1 = Request.Form["orderSSKF1"].ToString();
            //string CourierCompanies1 = Request.Form["CourierCompanies1"].ToString();
            //string CourierNo1 = Request.Form["CourierNo1"].ToString();
            //string orderPostage1 = Request.Form["orderPostage1"].ToString();
            //string orderOtherMoney1 = Request.Form["orderOtherMoney1"].ToString();
            //string orderRemark = Request.Form["orderRemark"].ToString();
            //string orderScode1 = Request.Form["orderScode1"].ToString();
            //string orderCHCK1 = Request.Form["orderCHCK1"].ToString();
            bll.shoporderbll shoporderBll = new bll.shoporderbll();

            return shoporderBll.orderUpdate(orderchildenid, Dic);
        }
        #endregion


        #region 获取订单物流信息(这里获取的是用户地址,电话等信息)
        /// <summary>
        /// 获取订单物流信息(这里获取的是用户地址,电话等信息)
        /// </summary>
        /// <returns></returns>
        public string getLogisticsMsg()
        {
            string s = string.Empty;
            List<Shipping> listAdd = new List<Shipping>();
            tmall.orderdetails od = new tmall.orderdetails();
            for (int i = 0; i < userInfo.TaoAppUserList.Count; i++)
            {
                od.Sessionkey = userInfo.TaoAppUserList[i].refreshToken;
                listAdd.AddRange(od.getAllLogistics(DateTime.Now.AddMonths(-3).ToString(), DateTime.Now.ToString()));
            }

            #region 去重
            for (int i = 0; i < listAdd.Count; i++)  //外循环是循环的次数
            {
                for (int j = listAdd.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {

                    if (listAdd[i].Tid == listAdd[j].Tid)
                    {
                        listAdd.RemoveAt(j);
                    }
                }
            }
            #endregion

            bll.shoporderbll shoporderBll = new bll.shoporderbll();
            string[] Express = shoporderBll.getExpressOrderId();
            for (int i = listAdd.Count - 1; i >= 0; i--)
            {
                if (Express.Contains(listAdd[i].Tid.ToString()))
                {
                    listAdd.Remove(listAdd[i]);
                }
            }


            #region 添加
            DataTable dt = new DataTable();
            //dt.Columns.Add("Id", typeof(Int32));
            dt.Columns.Add("OrderId", typeof(String));
            dt.Columns.Add("OrderChildenId", typeof(String));
            dt.Columns.Add("ExpressNo", typeof(String));
            dt.Columns.Add("ExpressName", typeof(String));
            dt.Columns.Add("ExpressPrice", typeof(String));
            dt.Columns.Add("ExpressFollow", typeof(String));
            dt.Columns.Add("CustomName", typeof(String));
            dt.Columns.Add("CustomPhone", typeof(String));
            dt.Columns.Add("CustomZipCode", typeof(String));
            dt.Columns.Add("CustomAddress", typeof(String));
            dt.Columns.Add("CustomCity1", typeof(String));
            dt.Columns.Add("CustomCity2", typeof(String));
            dt.Columns.Add("CustomCity3", typeof(String));
            dt.Columns.Add("Def1", typeof(String));
            dt.Columns.Add("Def2", typeof(String));
            dt.Columns.Add("Def3", typeof(String));
            dt.Columns.Add("Def4", typeof(String));
            dt.Columns.Add("Def5", typeof(String));
            for (int i = 0; i < listAdd.Count; i++)
            {
                DataRow drow = dt.NewRow();

                //drow["Id"] = "";
                drow["OrderId"] = listAdd[i].Tid;
                drow["OrderChildenId"] = "";
                drow["ExpressNo"] = "";
                drow["ExpressName"] = "";
                drow["ExpressPrice"] = "";
                drow["ExpressFollow"] = "";
                drow["CustomName"] = listAdd[i].ReceiverName; //姓名
                drow["CustomPhone"] = listAdd[i].ReceiverMobile; //电话
                drow["CustomZipCode"] = listAdd[i].Location.Zip; //邮编
                drow["CustomAddress"] = listAdd[i].Location.Address;//地址
                drow["CustomCity1"] = listAdd[i].Location.State; //省份
                drow["CustomCity2"] = listAdd[i].Location.City; //城市
                drow["CustomCity3"] = listAdd[i].Location.District; //县级
                drow["Def1"] = "";
                drow["Def2"] = "";
                drow["Def3"] = "";
                drow["Def4"] = "";
                drow["Def5"] = "";

                dt.Rows.Add(drow);
            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            SqlBulkCopy sbc = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepIdentity, transaction);
            sbc.ColumnMappings.Add("OrderId", "OrderId");
            sbc.ColumnMappings.Add("ExpressNo", "ExpressNo");
            sbc.ColumnMappings.Add("CustomName", "CustomName");
            sbc.ColumnMappings.Add("CustomPhone", "CustomPhone");
            sbc.ColumnMappings.Add("CustomZipCode", "CustomZipCode");
            sbc.ColumnMappings.Add("CustomAddress", "CustomAddress");
            sbc.ColumnMappings.Add("CustomCity1", "CustomCity1");
            sbc.ColumnMappings.Add("CustomCity2", "CustomCity2");
            sbc.ColumnMappings.Add("CustomCity3", "CustomCity3");
            sbc.BatchSize = dt.Rows.Count;
            sbc.DestinationTableName = "OrderExpress";

            try
            {
                sbc.WriteToServer(dt);
                transaction.Commit();


                s = "添加成功!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                s = ex.Message + "添加失败!";
            }

            #endregion


            return s;
        }
        #endregion

        #region 获取物流运单号
        /// <summary>
        /// 获取物流运单号
        /// </summary>
        /// <returns></returns>
        public string getLogisticsNo()
        {
            string s = string.Empty;
            List<Shipping> listAdd = new List<Shipping>();
            tmall.orderdetails od = new tmall.orderdetails();
            for (int i = 0; i < userInfo.TaoAppUserList.Count; i++)
            {
                od.Sessionkey = userInfo.TaoAppUserList[i].refreshToken;
                listAdd.AddRange(od.getAllLogisticsNo(DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"));
            }
            s = listAdd.Count.ToString();
            bll.shoporderbll shoporderBll = new bll.shoporderbll();
            string[] Express = shoporderBll.getExpressOrderId();
            for (int i = listAdd.Count - 1; i >= 0; i--)
            {
                if (!Express.Contains(listAdd[i].Tid.ToString()))
                {
                    listAdd.Remove(listAdd[i]);
                }
            }

            string sqlText = string.Empty;
            for (int i = 0; i < listAdd.Count; i++)
            {
                sqlText += "update OrderExpress set ExpressNo='" + listAdd[i].OutSid + "',ExpressName='" + listAdd[i].CompanyName + "' where OrderId='" + listAdd[i].Tid.ToString() + "';";
            }

            s = shoporderBll.updateExpressNo(sqlText);

            shoporderBll = null;
            listAdd = null;
            return s;
        }
        #endregion

        #region 获取订单备注信息(2个月订单信息更新)
        List<Trade> listRemark = new List<Trade>();

        /// <summary>
        /// 获取订单备注信息
        /// </summary>
        /// <returns></returns>
        public string getRemarkMsg()
        {
            string s = string.Empty;
            string sqlText = string.Empty;

            //bll.shoporderbll shoporderBll = new bll.shoporderbll();
            //string[] ss = shoporderBll.getCommentOrderRemarkId();//备注订单
            //string[] ssParent = shoporderBll.getOrderId(DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));  //主订单

            DateTime d3 = DateTime.Now;
            DateTime d2 = DateTime.Now; //最新时间
            DateTime d1 = DateTime.Now;//最早时间
            Thread[] td = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                lock (this)
                {
                    d2 = d3.AddDays(-i * 6);
                    d1 = d3.AddDays(-(i + 1) * 6);
                    string date1 = d1.ToString("yyyy-MM-dd") + " 00:00:00";//最早时间
                    string date2 = d2.ToString("yyyy-MM-dd") + " 23:59:59";//最新时间
                    OrderParms pv = new OrderParms(date1, date2);
                    pv.Data1 = date1;
                    pv.Data2 = date2;
                    pv.reRemarkMsg += pv_reRemarkMsg;
                    pv.nofityRemark();
                    td[i] = new Thread(new ParameterizedThreadStart(refundRemark));
                    td[i].Start(pv);
                }

            }

            for (int j = 0; j < 10; j++)
            {
                if (td[j].ThreadState == ThreadState.Stopped)
                {
                    td[j].Abort();
                }
            }

            updateOrderRemark(listRemark);

            return s;
        }

        public string updateOrderRemark(List<Trade> listAdd)
        {
            List<Trade> list = listAdd;

            string s = string.Empty;
            string sqlText = string.Empty;
            bll.shoporderbll shoporderBll = new bll.shoporderbll();
            string[] ss = shoporderBll.getCommentOrderRemarkId();//备注订单
            string[] ssParent = shoporderBll.getOrderId(DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));  //主订单]

            int f = 0;
            foreach (Trade item in list)
            {
                if (item != null && (!string.IsNullOrWhiteSpace(item.SellerMemo) || !string.IsNullOrWhiteSpace(item.SellerFlag.ToString())) && !string.IsNullOrWhiteSpace(item.Tid.ToString()))
                {
                    if (ss.Contains(ssParent[f].ToString()))
                    {
                        //update
                        sqlText += string.Format("update orderComments set ocRemark='{0}',ocBanner='{2}' where ocOrderId='{1}'", item.SellerMemo, item.Tid.ToString(), item.SellerFlag.ToString());
                    }
                    else
                    {
                        //add
                        sqlText += string.Format("insert orderComments(ocRemark,ocBanner,ocOrderId) values('{0}','{2}','{1}')", item.SellerMemo, item.Tid.ToString(), item.SellerFlag.ToString());
                    }
                }
                f++;
            }

            s = shoporderBll.updateExpressNo(sqlText);

            return s;
        }

        List<Trade> pv_reRemarkMsg(string data1, string data2)
        {
            bll.shoporderbll shoporderBll = new bll.shoporderbll();
            //string[] ssParent = shoporderBll.getOrderId(data1, data2);  //主订单
            string[] ssParentPbx = shoporderBll.getShopOrderId(data1, data2, "攀比轩旗舰店");
            string[] ssParentFb = shoporderBll.getShopOrderId(data1, data2, "佛罗伦斯海外专营店");
            tmall.orderdetails od = new tmall.orderdetails();
            for (int m = 0; m < userInfo.TaoAppUserList.Count; m++)
            {
                od.Sessionkey = userInfo.TaoAppUserList[m].refreshToken;
                if (userInfo.TaoAppUserList[m].refreshToken == "61007278ecd41be4ead2e3859a0f4169a1d18d3002122dc1969004137")
                {
                    for (int i = 0; i < ssParentPbx.Length; i++)
                    {
                        Trade td = od.getRemarkMsg(ssParentPbx[i].ToString());

                        listRemark.Add(td);
                    }
                }
                if (userInfo.TaoAppUserList[m].tbUserNick == "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799")
                {
                    for (int i = 0; i < ssParentFb.Length; i++)
                    {
                        Trade td = od.getRemarkMsg(ssParentFb[i].ToString());

                        listRemark.Add(td);
                    }
                }


                //for (int i = 0; i < ssParent.Length; i++)
                //{
                //    Trade td = od.getRemarkMsg(ssParent[i].ToString());

                //    listRemark.Add(td);
                //}
            }



            shoporderBll = null;

            return listRemark;
        }

        private void refundRemark(object obj)
        {
            OrderParms o = (OrderParms)obj;
            pv_reRemarkMsg(o.Data1, o.Data2);
        }
        #endregion

    }


    public delegate List<Trade> OrderHandler(string data1, string data2);
    public class OrderParms
    {

        public OrderParms(string data1, string data2)
        {
            this._data1 = data1;
            this._data2 = data2;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        private string _data1;

        public string Data1
        {
            get { return _data1; }
            set { _data1 = value; }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        private string _data2;

        public string Data2
        {
            get { return _data2; }
            set { _data2 = value; }
        }


        /// <summary>
        /// 需更新数据
        /// </summary>
        private List<string> _listData;

        public List<string> ListData
        {
            get { return _listData; }
            set { _listData = value; }
        }

        /// <summary>
        /// 需更新数据集合
        /// </summary>
        private List<Trade> _listTradeData;

        public List<Trade> ListTradeData
        {
            get { return _listTradeData; }
            set { _listTradeData = value; }
        }

        /// <summary>
        /// 主订单
        /// </summary>
        public event OrderHandler reParentMsg;
        /// <summary>
        /// 子订单
        /// </summary>
        public event OrderHandler reChildeMsg;

        public event OrderHandler reRemarkMsg;

        /// <summary>
        /// 主订单
        /// </summary>
        public void nofity()
        {
            if (reParentMsg != null)
            {
                reParentMsg(Data1, Data2);
            }
        }

        /// <summary>
        /// 子订单
        /// </summary>
        public void nofityChilde()
        {
            if (reChildeMsg != null)
            {
                reChildeMsg(Data1, Data2);
            }
        }

        public void nofityRemark()
        {
            if (reRemarkMsg != null)
            {
                reRemarkMsg(Data1, Data2);
            }
        }



    }

}