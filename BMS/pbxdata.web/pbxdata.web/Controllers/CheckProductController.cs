using Maticsoft.DBUtility;
using pbxdata.dal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
//
namespace pbxdata.web.Controllers
{
    public class CheckProductController : BaseController
    {
        //
        // GET: /CheckProduct/
        ProductHelper phb = new ProductHelper();
        public ActionResult CheckProduct()
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();
                string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
                ViewBag.ShowSearch = SearchList(ss, menuId);
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
        /// 判断搜索条件
        /// </summary>
        /// <param name="Id">条件</param>
        /// <returns></returns>
        public string SearchList(string[] ss, int menuId)
        {
            PublicHelpController ph = new PublicHelpController();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            StringBuilder s = new StringBuilder(200);
            //string s = string.Empty;
            //s.Append("<ul>");
            foreach (var temp in ss)
            {
                if (temp == "Style")
                {
                    
                 s.Append("<span class='spanProperty'><span class='spanPropertyName'>款号：</span><input type='text' id='txtStyle'  class='spanPropertyValue'/></span>");
                    //s.Append("<li>款号:<input type='text' id='txtStyle' /></li>");

                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Scode")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>货号：</span><input type='text' id='txtScode'  class='spanPropertyValue'/></span>");
                    //s.Append("<li>货号:<input type='text' id='txtScode' /></li>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Descript" && temp == "Cdescript")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>名称：</span><input type='text' id='txtName'  class='spanPropertyValue'/></span>");
                    //s.Append("<li>名称:<input type='text' id='txtName' /></li>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat1")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>季节：</span><input type='text' id='txtCat1'  class='spanPropertyValue'/></span>");
                    //s.Append("<li>季节：<input type='text' id='txtCat1' style='width: 80px' /></li>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Clolor")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>颜色：</span><input type='text' id='txtColor'  class='spanPropertyValue'/></span>");
                    //s.Append("<li>颜色：<input type='text' id='txtColor' style='width: 50px' /></li>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>品牌：</span><input type='text' id='txtCat' onclick='GetDropToBrand(this.id)'  class='spanPropertyValue'/></span>");
                   // s.Append("<li><span class='spanContainer'spanContainer>品牌:<input type='text' id='txtCat' /><select onchange='ddl1(this[selectedIndex].value,\"\")'>" + ProductStyleDDlist() + "</select></span></li>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat2")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>类别：</span><input type='text' id='txtCat2' onclick='GetDropToType(this.id)'  class='spanPropertyValue'/></span>");
                    //s.Append("<li><span class='spanContainer'>类别：<input type='text' id='txtCat2' /><select onchange='ddl1(this[selectedIndex].value,\"2\")'>" + ProductTypeDDlist() + "</select></span></li>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Balance")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>库存：</span><input type='text' id='txtBalance'  class='onlyNum'  style='width: 50px;height:24px;' />-<input type='text' id='txtBalance2'  class='onlyNum'  style='width: 50px;height:24px;' /></span>");
                    //s.Append("<li>库存：<input type='text' id='txtBalance' class='onlyNum' style='width: 50px' />-<input type='text' id='txtBalance2' class='onlyNum' style='width: 50px' /></li>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Imagefile")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>是否有图片：</span><select id='selImagefile' class='spanPropertyValue'><option value=''>请选择</option><option value='0'>没有</option><option value='1'>有</option></select></span>");
                   // s.Append("<li>是否有图片：<select id='selImagefile'><option value=''>请选择</option><option value='0'>没有</option><option value='1'>有</option></select></li>");
                }
            };
            s.Append("<span class='spanProperty'><span class='spanPropertyName'>商品信息图：</span><select id='selSciqProductId' class='spanPropertyValue'><option value=''>请选择</option><option value='3'>不合格</option><option value='1'>合格</option><option value='2'>未标记</option></select></span>");
            //s.Append("<li>商品信息图：<select id='selSciqProductId'><option value=''>请选择</option><option value='3'>不合格</option><option value='1'>合格</option><option value='2'>未标记</option></select></li>");
            if (ph.isFunPermisson(roleId, menuId, funName.HSCheck))
            {
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>商检资料：</span><select id='selZLStatus' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='1'>合格</option><option  value='0'>不合格</option></select></span>");
                //s.Append("<li>商检资料：<select id='selZLStatus'><option value='' selected='selected' >请选择</option><option value='1'>合格</option><option  value='0'>不合格</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>商检：</span><select id='selSJstatus' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='0'>失败</option><option  value='1'>成功</option><option  value='2'>未上传</option><option  value='3'>上传成功</option></select></span>");
                //s.Append("<li>商检：<select id='selSJstatus'><option value='' selected='selected' >请选择</option><option value='0'>失败</option><option  value='1'>成功</option><option  value='2'>未上传</option><option  value='3'>上传成功</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>商检审核：</span><select id='selRegStatus' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='10'>通过</option><option  value='20'>不通过</option></select></span>");
                //s.Append("<li>商检审核：<select id='selRegStatus'><option value='' selected='selected' >请选择</option><option value='10'>通过</option><option  value='20'>不通过</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>联邦报备：</span><select id='selBBCask' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='1'>成功</option><option  value='0'>失败</option><option  value='2'>未报备</option></select></span>");
                //s.Append("<li>联邦报备：<select id='selBBCask'><option value='' selected='selected' >请选择</option><option value='1'>成功</option><option  value='0'>失败</option><option  value='2'>未报备</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>(判断是否有值)HS编码：</span><select id='selSciqHSNo' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></span>");
                //s.Append("<li><span style='color:red'>(判断是否有值)</span>HS编码：<select id='selSciqHSNo'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>原产国：</span><select id='selSciqAssemCountry' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></span>");
                //s.Append("<li>原产国：<select id='selSciqAssemCountry'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>计量单位：</span><select id='selSQtyUnit' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></span>");
                //s.Append("<li>计量单位：<select id='selSQtyUnit'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>毛重：</span><select id='selSDef15' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></span>");
                //s.Append("<li>毛重：<select id='selSDef15'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></li>");
                s.Append("<span class='spanProperty'><span class='spanPropertyName'>净重：</span><select id='selSDef16' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></span>");
                //s.Append("<li>净重：<select id='selSDef16'><option value='' selected='selected' >请选择</option><option value='1'>有</option><option  value='0'>没有</option></select></li>");
            }
            //s.Append("</ul><br />");
            s.Append("<div class='clearfix'></div>");
            s.Append(" <input type='button' value=' 查  询 ' class='spanPropertySearch' style='margin-left: 2px' onclick='SearchbyScode()' />");
            if (ph.isFunPermisson(roleId, menuId, funName.HSCheck))
            {
                s.Append(" <input type='button' id='btn1Check' class='spanPropertySearch' onclick='CeateSJProductReport()' value='1.生成报文' />");  //1
                s.Append(" <input type='button' id='btn2Check' class='spanPropertySearch' onclick='ProductReport()' value='2.上传报文' />");//2
                s.Append(" <input type='button' id='btn3Check' class='spanPropertySearch' onclick='getSJProductResult()' value='3.下载报文回执' />");//3
                s.Append(" <input type='button' id='btn5Check' class='spanPropertySearch' onclick='getSJProductResult2()' value='5.下载审核回执' />");//5
                s.Append(" <input type='button' id='btn6Check' class='spanPropertySearch' onclick='ProductReportResult2()' value='6.读取审核回执' />");//6
                s.Append(" <input type='button' id='btn7Check' class='spanPropertySearch' onclick='BBCProductReport()' value='联邦商品报备' />");//7
                
            }
            return s.ToString();

        }
        /// <summary>
        /// 条件搜索主货号下表
        /// </summary>
        /// <returns></returns>
        public string OnSeach()//string lists, string menuId, string page, string Selpages
        {
            //Style:'" + Style + "',Scode:'" + Scode + "',Cat:'" + Cat + "',Cat1:'" + Cat1 + "',Cat2:'" + Cat2 + "',Balance:'" + Balance + "',Balance2:'" + Balance2 + "',
            //Color:'" + Color + "',SJstatus:'" + SJstatus + "',RegStatus:'" + RegStatus + "',BBCask:'" + BBCask + "',ZLStatus:'" + ZLStatus + "',Imagefile:'" + Imagefile + "',SciqHSNo:'" + SciqHSNo + "'，
            //SciqAssemCountry:'" + SciqAssemCountry + "',SQtyUnit:'" + SQtyUnit + "',SDef15:'" + SDef15 + "',SDef16:'" + SDef16 + "',SciqProductId:'" + SciqProductId + "'}";
            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Style = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}", "");
            string Scode = helpcommon.StrSplit.StrSplitData(ss[1], ':')[1].Replace("'", "").Replace("}", "");
            string Cat = helpcommon.StrSplit.StrSplitData(ss[2], ':')[1].Replace("'", "").Replace("}", "");
            string Cat1 = helpcommon.StrSplit.StrSplitData(ss[3], ':')[1].Replace("'", "").Replace("}", "");
            string Cat2 = helpcommon.StrSplit.StrSplitData(ss[4], ':')[1].Replace("'", "").Replace("}", "");
            string Balance = helpcommon.StrSplit.StrSplitData(ss[5], ':')[1].Replace("'", "").Replace("}", "");
            string Balance2 = helpcommon.StrSplit.StrSplitData(ss[6], ':')[1].Replace("'", "").Replace("}", "");
            string Color = helpcommon.StrSplit.StrSplitData(ss[7], ':')[1].Replace("'", "").Replace("}", "");
            string SJstatus = helpcommon.StrSplit.StrSplitData(ss[8], ':')[1].Replace("'", "").Replace("}", "");
            string RegStatus = helpcommon.StrSplit.StrSplitData(ss[9], ':')[1].Replace("'", "").Replace("}", "");
            string BBCask = helpcommon.StrSplit.StrSplitData(ss[10], ':')[1].Replace("'", "").Replace("}", "");
            string ZLStatus = helpcommon.StrSplit.StrSplitData(ss[11], ':')[1].Replace("'", "").Replace("}", "");
            string Imagefile = helpcommon.StrSplit.StrSplitData(ss[12], ':')[1].Replace("'", "").Replace("}", "");
            string SciqHSNo = helpcommon.StrSplit.StrSplitData(ss[13], ':')[1].Replace("'", "").Replace("}", "");
            string SciqAssemCountry = helpcommon.StrSplit.StrSplitData(ss[14], ':')[1].Replace("'", "").Replace("}", "");
            string SQtyUnit = helpcommon.StrSplit.StrSplitData(ss[15], ':')[1].Replace("'", "").Replace("}", "");
            string SDef15 = helpcommon.StrSplit.StrSplitData(ss[16], ':')[1].Replace("'", "").Replace("}", "");
            string SDef16 = helpcommon.StrSplit.StrSplitData(ss[17], ':')[1].Replace("'", "").Replace("}", "");
            string SciqProductId = helpcommon.StrSplit.StrSplitData(ss[18], ':')[1].Replace("'", "").Replace("}", "");

            DataTable dt = new DataTable();
            bll.CheckProductbll bll = new bll.CheckProductbll();
            //string[] SearchInfo = lists.Split(',');
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Style", Style);
            Dic.Add("Scode", Scode);
            Dic.Add("Cat", Cat);
            Dic.Add("Cat1", Cat1);
            Dic.Add("Cat2", Cat2);
            Dic.Add("MinBalance", Balance);
            Dic.Add("MaxBalance", Balance2);
            Dic.Add("Color", Color);
            Dic.Add("SJstatus", SJstatus);
            Dic.Add("RegStatus", RegStatus);
            Dic.Add("BBCask", BBCask);
            Dic.Add("ZLStatus", ZLStatus);
            Dic.Add("Imagefile", Imagefile);
            Dic.Add("ciqHSNo", SciqHSNo);
            Dic.Add("ciqAssemCountry", SciqAssemCountry);
            Dic.Add("QtyUnit", SQtyUnit);
            Dic.Add("Def15", SDef15);
            Dic.Add("Def16", SDef16);
            Dic.Add("ciqProductId", SciqProductId);
            string counts;
            //if (page == "0")
            //{
            //    dt = bll.SearchProduct(Dic, 1, Convert.ToInt32(Selpages), out counts);
            //}
            //else if (page == "-1")
            //{
            //    dt = bll.SearchProduct(Dic, 200, Convert.ToInt32(Selpages), out counts);
            //}
            //else
            //{
                dt = bll.SearchProduct(Dic, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), out counts);
            //}
                int pageCount = Convert.ToInt32(counts) % Convert.ToInt32(pageSize) == 0 ? Convert.ToInt32(counts) / Convert.ToInt32(pageSize) : Convert.ToInt32(counts) / Convert.ToInt32(pageSize);
                return getDataProduct(dt, menuId) + "-----" + pageCount + "-----" + counts;
        }
        /// <summary>
        /// Product界面下表主货号
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataProduct(DataTable dt, string FmenuId)
        {

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            string[] ssName = { "Imagefile", "ciqProductId", "Style", "Model", "Scode", "Descript", "Cdescript", "Cat", "Cat1", "Cat2", "Clolor", "Size", "Balance", "SJstatus", "RegStatus", "BBCask" };
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            #region TABLE表头
            s.Append("<tr><th><input type='checkbox' id='selectallbox' onchange='selectbox()' /></th><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {

                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");

                    if (ssName[z] == "Imagefile")
                        s.Append("缩略图");
                    if (ssName[z] == "ciqProductId")
                        s.Append("商品信息图");
                    if (ssName[z] == "Style")
                        s.Append("款号");
                    if (ssName[z] == "Model")
                        s.Append("型号");
                    if (ssName[z] == "Scode")
                        s.Append("货品编号");
                    if (ssName[z] == "Descript")
                        s.Append("英文名称");
                    if (ssName[z] == "Cdescript")
                        s.Append("中文名称");
                    if (ssName[z] == "Cat")
                        s.Append("品牌");
                    if (ssName[z] == "Cat1")
                        s.Append("季节");
                    if (ssName[z] == "Cat2")
                        s.Append("类别");
                    if (ssName[z] == "Clolor")
                        s.Append("颜色");
                    if (ssName[z] == "Size")
                        s.Append("尺寸");
                    if (ssName[z] == "Loc")
                        s.Append("店铺");
                    if (ssName[z] == "Balance")
                        s.Append("库存");
                    if (ssName[z] == "SJstatus")
                        s.Append("商检");
                    if (ssName[z] == "RegStatus")
                        s.Append("商检审核");
                    if (ssName[z] == "BBCask")
                        s.Append("联邦报备");
                    s.Append("</th>");
                }
            }
            //s.Append("<th>商检</th>");
            s.Append("<th>编辑</th>");
            #endregion

            #region 商检编辑
            if (ph.isFunPermisson(roleId, menuId, funName.EditProductCheck))
            {
                s.Append("<th>商品信息报备</th><th>读取商品回执</th><th>读取商品回执2</th><th>联邦商品报备</th>");
            }
            #endregion



            //#region 删除
            //if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            //{
            //    s.Append("<th>删除</th>");
            //}
            //#endregion

            s.Append("</tr>");


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
                    s.Append("<tr><td><input type='checkbox' class='box' title='" + dt.Rows[i]["Scode"] + "'></td><td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        if (ss.Contains(ssName[j]))
                        {
                            if (ssName[j] == "Cat2")
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i]["TypeName"].ToString());
                                s.Append("</td>");
                            }
                            else if (ssName[j] == "ciqProductId")
                            {
                                string item = dt.Rows[i]["ciqProductId"].ToString() == "3" ? "不合格" : dt.Rows[i]["ciqProductId"].ToString() == "1" ? "合格" : "未标记";
                                s.Append("<td>");
                                s.Append(item);
                                s.Append("</td>");
                            }
                            else if (ssName[j] == "Imagefile")
                            {
                                if (dt.Rows[i][ssName[j]].ToString() != "")
                                {
                                    s.Append("<td><img src='" + dt.Rows[i][ssName[j]].ToString() + "' style='width:60px;height:60px'/></td>");
                                }
                                else
                                {
                                    s.Append("<td></td>");
                                }
                            }
                            else if (ssName[j] == "SJstatus")
                            {
                                s.Append("<td>");
                                if (dt.Rows[i][ssName[j]].ToString() == "0")
                                {
                                    s.Append("<span  style='color:red'>失败</span>");
                                }
                                else if (dt.Rows[i][ssName[j]].ToString() == "1")
                                {
                                    s.Append("成功");
                                }
                                else if (dt.Rows[i][ssName[j]].ToString() == "2" || dt.Rows[i][ssName[j]].ToString() == "")
                                {
                                    s.Append("未上传");
                                }
                                else if (dt.Rows[i][ssName[j]].ToString() == "3")
                                {
                                    s.Append("上传成功");
                                }
                                s.Append("</td>");
                            }
                            else if (ssName[j] == "RegStatus")
                            {
                                s.Append("<td>");
                                if (dt.Rows[i][ssName[j]].ToString() == "10")
                                {
                                    s.Append("通过");
                                }
                                else if (dt.Rows[i][ssName[j]].ToString() == "20")
                                {
                                    s.Append("<span  style='color:red'>" + dt.Rows[i]["RegNotes"].ToString() + "</span>");
                                }

                                s.Append("</td>");
                            }
                            else if (ssName[j] == "BBCask")
                            {
                                s.Append("<td>");

                                if (dt.Rows[i][ssName[j]].ToString() == "0")
                                {
                                    s.Append("<span  style='color:red'>" + dt.Rows[i]["BBCerrorMessage"].ToString() + "</span>");
                                }
                                else if (dt.Rows[i][ssName[j]].ToString() == "1")
                                {
                                    s.Append("成功");
                                }
                                else
                                {
                                    s.Append("未报备");
                                }
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
                    s.Append("<td>");
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<a href='#' onclick='EditProduct(\"" + dt.Rows[i]["Scode"].ToString() + "\",\"edit\")'>编辑</a>");
                    }

                    s.Append("</td>");
                    #endregion


                    #region 商检编辑
                    if (ph.isFunPermisson(roleId, menuId, funName.EditProductCheck))
                    {
                        s.Append("<td><a href='#' onclick='ProductReport(\"" + dt.Rows[i]["Scode"] + "\")'>商品信息报备</a></td>");
                        s.Append("<td><a href='#' onclick='ProductReportResult(\"" + dt.Rows[i]["Scode"] + "\")'>读取商品回执</a></td>");
                        s.Append("<td><a href='#' onclick='ProductReportResult2(\"" + dt.Rows[i]["Scode"] + "\")'>读取商品回执2</a></td>");
                        s.Append("<td><a href='#' onclick='BBCProductReport(\"" + dt.Rows[i]["Scode"] + "\")'>联邦商品报备</a></td>");
                    }
                    #endregion
                    s.Append("</tr>");
                }
            }
            
            #endregion
            return s.ToString();
        }

        /// <summary>
        /// 获取指定商品信息进行编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string EditProduct()
        {
            //ViewBag.asas = id;

            //string id = Request.QueryString["id"];

            string scode = Request.QueryString["scode"];
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
            PublicHelpController ph = new PublicHelpController();

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string src = "";
            string strsql = @"select * from product where scode='" + scode + "'";
            dt1 = DbHelperSQL.Query(@"select * from scodepic where scode='" + scode + "' and Left(Def1,1)='2' and Def4='1' order by Left(Def1,2)").Tables[0];
            dt = DbHelperSQL.Query(strsql).Tables[0];
            string a =  dt.Columns[0].ToString();
            string s = "";

            if (Request.QueryString["type"].ToString() == "edit")
            {
                s = string.Format(@"<table id='tabProperty'>
                    <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><div style='position:relative'><div id='divThumbnail' onclick='DeleteThumbnail()'>×</div><img src='{11}' style='width:120px;' /></div></td></tr>
                    <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' type='text' value='{2}'  /></td></tr>
                    <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                        <td>季节：<input id=txtCat1E type='text' value='{4}' /></td></tr>
                    <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                        <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
                    <tr><td>预警库存：<input id='txtRolevelE' type='text' value='{9}' disabled='disabled'  /></td>
                        <td>类别：{5}<input type='hidden' id='hidId'' value='{10}' /></td></tr>
                    <tr><td>商品信息图：<select id='txtciqProductId'  style='width:150px'>{12}</select></td>
                        <td>规格型号：<input id='txtciqSpec' type='text' value='{13}'  /></td>
                        <td>图片是否合格:{17}</td></tr>
                    <tr><td>商品HS编码：<input id='txtciqHSNo' type='text' value='{14}' onfocus='ShowHSNum()' /></td>
                        <td><div style='position:relative'>原产国/地区：<input type='text' id='txtciqAssemCountry' oninput='GetCountry()' value='{22}' ><select id='selciqAssemCountry' style='width:150px' onchange='GetCountryddl()'>{15}</selected><div></td>
                        <td>商品序号：<input type='text' value='{16}' disabled='disabled' /></td></tr>
                   <tr><td>条形码：<input id='txtDef10' type='text' value='{18}'  /></td>
                       <td>计量单位: {19}</td>
                       <td>毛重：<input id='txtDef15' type='text' value='{20}'  /></td></tr>
                   <tr><td>净重：<input id='txtDef16' type='text' value='{21}'  /></td>
                       <td></td>
                       <td></td></tr>
           </table>", dt.Rows[0]["Scode"].ToString()
                , dt.Rows[0]["Descript"].ToString()
                , dt.Rows[0]["Cdescript"].ToString()
                , dt.Rows[0]["Cat"].ToString()
                , dt.Rows[0]["Cat1"].ToString()
                , GetDDlist(dt.Rows[0]["Cat2"].ToString())
                , dt.Rows[0]["Clolor"].ToString()
                , dt.Rows[0]["Style"].ToString()
                , dt.Rows[0]["Size"].ToString()
                , dt.Rows[0]["Rolevel"].ToString()
                , "1"
                , dt.Rows[0]["Imagefile"].ToString()
                , ProductInfoPic(dt.Rows[0]["ciqProductId"].ToString())
                , dt.Rows[0]["ciqSpec"].ToString()
                , dt.Rows[0]["ciqHSNo"].ToString()
                , GetCountrylist("", dt.Rows[0]["ciqAssemCountry"].ToString())
                , dt.Rows[0]["ciqProductNo"].ToString()
                , PicCheckDDlList(dt.Rows[0]["Def9"].ToString())
                , dt.Rows[0]["Def10"].ToString()
                , apiUnitDDlist(dt.Rows[0]["QtyUnit"].ToString(), Request.QueryString["type"].ToString())
                , dt.Rows[0]["Def15"].ToString()
                , dt.Rows[0]["Def16"].ToString()
                , dt.Rows[0]["ciqAssemCountry"].ToString()
             );
            }

            //string json = JsonConvert.SerializeObject(dt);
            return s;

        }

        /// <summary>
        /// 根据货号更新商品信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateProduct()
        {
            bll.ProductBll pro = new bll.ProductBll();
            model.product Promm = new product();
            string ComAttrvalues = Request.QueryString["ComAttrvalues"];
            string AttrId = Request.QueryString["AttrId"];
            string PropertyId = Request.QueryString["PropertyId"];
            Promm.Scode = Request.QueryString["Scode"];
            Promm.Cdescript = Request.QueryString["Cdescript"];
            Promm.Rolevel = Convert.ToInt32(Request.QueryString["Rolevel"]);
            Promm.Cat1 = Request.QueryString["Cat1"];
            Promm.Cat2 = Request.QueryString["Cat2"];
            Promm.ciqProductId = Request.QueryString["ciqProductId"];
            Promm.ciqHSNo = Request.QueryString["ciqHSNo"];
            Promm.ciqSpec = Request.QueryString["ciqSpec"];
            Promm.ciqAssemCountry = Request.QueryString["ciqAssemCountry"];
            Promm.Def9 = Request.QueryString["Def9"];
            Promm.Def10 = Request.QueryString["Def10"];
            Promm.Imagefile = Request.QueryString["Imagefile"].ToString();
            Promm.QtyUnit = Request.QueryString["QtyUnit"].ToString();
            Promm.Def15 = Request.QueryString["Def15"].ToString();
            Promm.Def16 = Request.QueryString["Def16"].ToString();
            return pro.UpdateProduct(Promm, AttrId, ComAttrvalues, PropertyId, userInfo.User.Id);
        }
        /// <summary>
        /// 获取国家下拉列表
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="Country"></param>
        /// <returns></returns>
        public string GetCountrylist(string countryName, string Country)
        {

            string s = string.Empty;
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            s += "<option value=''>请选择</option>";
            if (countryName != "")
            {

                //var q = from c in context.customsCountry where c.countryName.Contains(countryName) || c.countryNo.Contains(countryName) select c;
                var q = from c in context.customsCountry where c.countryName.Contains(countryName) || c.countryNo.Contains(countryName) || context.fun_getPY(c.countryName).Contains(countryName) select c;
                dt = LinqToDataTable.LINQToDataTable(q.OrderBy(a => a.countryNo));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += "<option value='" + dt.Rows[i]["countryNo"] + "'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>";
                }
            }
            else
            {
                dt = DbHelperSQL.Query(@"select * from customsCountry order by countryNo").Tables[0];
                if (Country == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += "<option value='" + dt.Rows[i]["countryNo"] + "'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>";
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Country.Contains(dt.Rows[i]["countryNo"].ToString()))
                        {
                            s += "<option value='" + dt.Rows[i]["countryNo"] + "' selected='selected'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>";
                        }
                        else
                        {
                            s += "<option value='" + dt.Rows[i]["countryNo"] + "'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>";
                        }

                    }
                }
            }


            return s;
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        /// <returns></returns>
        public string apiUnitDDlist(string unitNo, string type)
        {

            string s = string.Empty;
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query(@"select * from apiUnit").Tables[0];
            s += "<select id='selapiUnit' style='width:150px'>";

            if (type == "edit")
            {
                s += "<option value=''>请选择</option>";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (unitNo == dt1.Rows[i]["unitNo"].ToString())
                    {
                        s += "<option value='" + dt1.Rows[i]["unitNo"].ToString() + "' selected='selected' >" + dt1.Rows[i]["unitName"].ToString() + "</option>";
                    }
                    else
                    {
                        s += "<option value='" + dt1.Rows[i]["unitNo"].ToString() + "'>" + dt1.Rows[i]["unitName"].ToString() + "</option>";
                    }

                }
            }
            else
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (unitNo == dt1.Rows[i]["unitNo"].ToString())
                    {
                        s += "<option value='" + dt1.Rows[i]["unitNo"].ToString() + "' selected='selected' >" + dt1.Rows[i]["unitName"].ToString() + "</option>";
                        break;
                    }
                }
            }
            s += "</select>";
            return s;

        }
        /// <summary>
        /// 获取图片是否合格下拉框
        /// </summary>
        /// 
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
        /// 获取商品类别并构造下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDDlist(string TypeNo)
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            bll.Attributebll attrbll = new bll.Attributebll();
            dt = attrbll.GetTypeDDlist(userInfo.User.Id.ToString());
            s += "<select id='ddlType' style='width:150px' onchange='ddl(this[selectedIndex].value)' >";

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
        /// 品牌下拉列表
        /// </summary>
        /// <returns></returns>
        public string ProductStyleDDlist()
        {
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            bll.Attributebll attrbll = new bll.Attributebll();
            dt1 = attrbll.GetCatDDlist(userInfo.User.Id.ToString());

            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s += "<option value='" + dt1.Rows[i][1].ToString() + "'>" + dt1.Rows[i][0].ToString() + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 类别下拉列表
        /// </summary>
        /// <returns></returns>
        public string ProductTypeDDlist()
        {
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            bll.Attributebll attrbll = new bll.Attributebll();
            dt1 = attrbll.GetTypeDDlist(userInfo.User.Id.ToString());

            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s += "<option value='" + dt1.Rows[i][1].ToString() + "'>" + dt1.Rows[i][0].ToString() + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 商品信息图
        /// </summary>
        /// <returns></returns>
        public string ProductInfoPic(string item)
        {
            string s = string.Empty;
            if (item == "")
            {
                s += "<option value='' selected='selected'>请选择</option>";
            }
            else
            {
                s += "<option value=''>请选择</option>";
            }
            if (item == "1")
            {
                s += "<option value='1' selected='selected'>合格</option>";
            }
            else
            {
                s += "<option value='1'>合格</option>";
            }
            if (item == "3")
            {
                s += "<option value='3' selected='selected'>不合格</option>";
            }
            else
            {
                s += "<option value='3'>不合格</option>";
            }
            return s;
        }

        #region  类别 品牌下拉列表

        public string Brand()
        {
            string sql = Request.Form["sql"].ToString();
            string data = Request.Form["value"].ToString();
            string tsql;
            if (sql == "1")
            {
                tsql = "select a.BrandName,BrandAbridge from brand a left join BrandConfigPersion b on a.BrandAbridge=b.BrandId where b.CustomerId=" + userInfo.User.Id + " order by a.BrandName";
            }
            else
            {
                tsql = "select a.BrandName,BrandAbridge from brand a left join BrandConfigPersion b on a.BrandAbridge=b.BrandId where b.CustomerId=" + userInfo.User.Id + " and  a.BrandName like '%" + data + "%' order by a.BrandName";
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
                tsql = "select TypeName,TypeNo from producttype a left join PersonaTypeConfit b on a.TypeNo=b.TypeId where b.CustomerId=" + userInfo.User.Id + " order by TypeName";
            }
            else
            {
                tsql = "select TypeName,TypeNo from producttype a left join PersonaTypeConfit b on a.TypeNo=b.TypeId where b.CustomerId=" + userInfo.User.Id + "  and a.TypeName like '%" + data + "%' order by TypeName";
            }
            return phb.GetDropList1(tsql);
        }

        #endregion
    }
}
