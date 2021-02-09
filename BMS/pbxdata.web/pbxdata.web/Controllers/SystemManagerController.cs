using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace pbxdata.web.Controllers
{
    public class SystemManagerController : BaseController
    {
        //
        // GET: /SystemManager/

        /// <summary>
        /// 供应商管理
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductsourcesManager()
        {
            int menuId = int.Parse(Request.QueryString["menuId"]);
            PublicHelpController pub = new PublicHelpController();
            #region 权限查看
            if (!pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.addName))
            {
                ViewData["Insert"] = "true";
            }
            else
            {
                ViewData["Insert"] = "false";
            }
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.updateName))
            {
                ViewData["menuId"] = menuId;
                ViewData["Update"] = "true";
            }
            else
            {
                ViewData["Update"] = "false";
            }
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.deleteName))
            {
                ViewData["Delete"] = "true";
            }
            else
            {
                ViewData["Delete"] = "false";
            }
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.start))
            {
                ViewData["Start"] = "true";
            }
            else
            {
                ViewData["Start"] = "false";
            }
            #endregion
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();


            ViewData["Head"] = GetData(menuId, funName.selectName);
            List<model.productsource> list = systembll.GetProductsource(userInfo.User.Id);
            return View(list);
        }
        /// <summary>
        /// 获取数据源显示字段
        /// </summary>
        /// <param name="FunName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetDataConfig(string FunName, int menuId)
        {
            PublicHelpController pub = new PublicHelpController();
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            string[] temp = systembll.getDataName("productsourceConfig");
            string[] str = pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, FunName);
            Dictionary<string, string> lis = new Dictionary<string, string>();
            foreach (string t in temp)
            {
                if (str.Contains(t))
                {
                    if (t == "Id")
                    {
                        lis.Add("Id", "编号");
                    }
                    if (t == "SourceCode")
                    {
                        lis.Add("SourceCode", "供应商编号");
                    }
                    if (t == "SourcesAddress")
                    {
                        lis.Add("SourcesAddress", "服务器地址");
                    }
                    if (t == "UserId")
                    {
                        lis.Add("UserId", "登陆名");
                    }
                    if (t == "UserPwd")
                    {
                        lis.Add("UserPwd", "密码");
                    }
                    if (t == "DataSources")
                    {
                        lis.Add("DataSources", "数据库名称");
                    }
                    if (t == "DataSourcesLevel")
                    {
                        lis.Add("DataSourcesLevel", "数据库等级");
                    }
                    if (t == "Def3")
                    {
                        lis.Add("DataSourcesLevel", "更新程序启动Ip");
                    }
                }
            }
            return lis;
        }
        public Dictionary<string, string> GetData(int menuId, string FunName)
        {
            PublicHelpController pub = new PublicHelpController();
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            string[] temp = systembll.getDataName("productsource");
            string[] str = pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, FunName);
            Dictionary<string, string> lis = new Dictionary<string, string>();
            foreach (string t in temp)
            {
                if (str.Contains(t))
                {
                    if (t == "SourceCode")
                    {
                        lis.Add("SourceCode", "供应商编码");
                    }
                    if (t == "sourceName")
                    {
                        lis.Add("sourceName", "供应商名称");
                    }
                    if (t == "sourcePhone")
                    {
                        lis.Add("sourcePhone", "供应商电话");
                    }
                    if (t == "SourceManage")
                    {
                        lis.Add("SourceManage", "供应商联系人");
                    }
                    if (t == "SourceLevel")
                    {
                        lis.Add("SourceLevel", "供应商等级");
                    }
                    if (t == "UserName1")
                    {
                        lis.Add("UserName1", "供应商联系人");
                    }
                    if (t == "Def2")
                    {
                        lis.Add("Def2", "更新程序路径");
                    }
                    if (t == "Def3")
                    {
                        lis.Add("Def3", "发货地");
                    }
                    if (t == "UserId")
                    {
                        lis.Add("UserId", "操作人");
                    }
                }
            }
            return lis;
        }
        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Boolean DeleteProductsources(string Id)
        {
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            bool bol = systembll.DeleteProductsources(Id, userInfo.User.UserId);
            return bol;
        }
        //添加供应商页面
        public ActionResult InsertProductsoures()
        {
            ViewData["IsUpdate"] = "false";
            pbxdata.model.productsource pro = new model.productsource();
            pro.UserId = userInfo.User.UserId;
            pro.UserName1 = userInfo.User.userName;
            return View(pro);
        }
        //供应商添加事件
        [HttpPost]
        public ActionResult InsertProductsoures1(FormCollection fr)
        {

            bool isUpdate = bool.Parse(fr["IsUpdate"]);

            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            if (isUpdate)
            {
                pbxdata.model.productsource prop = systembll.GetProductsourece(fr["SourceCode"]);
                prop.SourceCode = fr["SourceCode"] == "" ? prop.SourceCode : fr["SourceCode"];
                prop.sourceName = fr["sourceName"] == "" ? prop.sourceName : fr["sourceName"];
                prop.sourcePhone = fr["sourcePhone"] == "" ? prop.sourcePhone : fr["sourcePhone"];
                prop.SourceManage = fr["SourceManage"] == "" ? prop.SourceManage : fr["SourceManage"];
                prop.SourceLevel = fr["SourceLevel"] == "" ? prop.SourceLevel : fr["SourceLevel"];
                prop.Def2 = fr["Def2"] == "" ? prop.Def2 : fr["Def2"];
                prop.Def3 = fr["Def3"] == "" ? prop.Def3 : fr["Def3"];
                prop.UserId = userInfo.User.Id;

                bool bol = systembll.UpdateProductsource(prop, userInfo.User.Id);
                if (bol)
                {
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
            else
            {
                model.productsource pro = new model.productsource();
                pro.SourceCode = fr["SourceCode"];
                pro.sourceName = fr["sourceName"];
                pro.sourcePhone = fr["sourcePhone"];
                pro.SourceManage = fr["SourceManage"];
                pro.SourceLevel = fr["SourceLevel"];
                pro.Def2 = fr["Def2"];
                pro.Def3 = fr["Def3"];
                pro.UserId = userInfo.User.Id;

                //pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
                bool bol;
                bool success = systembll.InsertProductsources(pro, out bol);
                if (!success)
                {
                    if (bol)
                    {
                        return Content("NoE");
                    }
                    else
                    {
                        return Content("保存失败！");
                    }
                }
                return Content("true");
            }
        }
        [HttpGet]
        public ActionResult UpdateProductsoures()
        {
            string Id = Request.QueryString["Id"];
            int menuId = int.Parse(Request.QueryString["menuId"]);
            Dictionary<string, string> dic = GetData(menuId, funName.updateName);//获取权限
            ViewData["dic"] = dic;
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            pbxdata.model.productsource pro = systembll.GetProductsourece(Id);
            pro.UserName1 = userInfo.User.userName;
            //PublicHelpController pub = new PublicHelpController();
            ViewData["IsUpdate"] = "true";
            ViewData["Id"] = pro.Id;
            return View("InsertProductsoures", pro);
        }

        /// <summary>
        /// 显示所有数据源信息
        /// </summary>
        /// <returns></returns>
        public ActionResult DataSourcesConfig()
        {
            string str = Request.QueryString["menuId"];
            ViewData["menuId"] = str;
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            PublicHelpController pub = new PublicHelpController();
            bool bol = pub.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), int.Parse(str), funName.selectName);
            #region 权限查看
            if (!bol)
            {
                return View("../NoPermisson/Index");
            }
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), int.Parse(str), funName.addName))
            {
                ViewData["Insert"] = "true";
            }
            else
            {
                ViewData["Insert"] = "false";
            }
            //if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), int.Parse(str), funName.updateName))
            //{
            //    ViewData["Update"] = "true";
            //}
            //else
            //{
            //    ViewData["Update"] = "false";
            //}
            //if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), int.Parse(str), funName.deleteName))
            //{
            //    ViewData["Delete"] = "true";
            //}
            //else
            //{
            //    ViewData["Delete"] = "false";
            //}
            //if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), int.Parse(str), funName.deleteName))
            //{
            //    ViewData["Delete"] = "true";
            //}
            //else
            //{
            //    ViewData["Delete"] = "false";
            //}
            #endregion
            DataTable dt = systembll.GetProductsourceConfig(userInfo.User.UserId);
            string[] strt = pub.getFiledPermisson(int.Parse(userInfo.User.personaId.ToString()), int.Parse(str), funName.selectName);
            if (dt == null)
            {
                return View("../ErrorMsg/Index");
            }
            List<DataColumn> list = new List<DataColumn>();
            foreach (DataColumn dc in dt.Columns)
            {
                if (strt.Contains(dc.ColumnName))
                {
                    if (dc.ColumnName == "Id")
                        dc.ColumnName = "编号";
                    if (dc.ColumnName == "SourceCode")
                        dc.ColumnName = "数据源编号";
                    if (dc.ColumnName == "SourcesAddress")
                        dc.ColumnName = "服务器地址";
                    if (dc.ColumnName == "UserId")
                        dc.ColumnName = "登陆名";
                    if (dc.ColumnName == "UserPwd")
                        dc.ColumnName = "密码";
                    if (dc.ColumnName == "DataSources")
                        dc.ColumnName = "数据库名称";
                    if (dc.ColumnName == "DataSourcesLevel")
                        dc.ColumnName = "数据源等级";
                    if (dc.ColumnName == "TimeStart")
                        dc.ColumnName = "启动时间间隔(分)";
                    if (dc.ColumnName == "Def2")
                        dc.ColumnName = "更新已启动(分)";
                    if (dc.ColumnName == "Def1")
                        dc.ColumnName = "更新状态";
                    if (dc.ColumnName == "Def3")
                        dc.ColumnName = "更新程序运行Ip";
                }
                else
                {
                    if (dc.ColumnName == "Id")
                        dc.ColumnName = "编号";
                    else if (dc.ColumnName == "sourceName" && strt.Contains("SourceCode"))
                    {
                        dc.ColumnName = "数据源名称";
                    }
                    else
                        list.Add(dc);
                    //dt.Columns.Remove(dc);
                }
            }
            foreach (var v in list)
            {
                dt.Columns.Remove(v);
            }
            dt.Columns.Add(new DataColumn("操作"));
            bool IsUpdate = pub.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), int.Parse(str), funName.updateName);
            bool IsDelete = pub.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), int.Parse(str), funName.deleteName);
            bool IsLog = pub.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), int.Parse(str), funName.Log);
            foreach (DataRow dr in dt.Rows)
            {
                if (IsUpdate)
                    dr["操作"] += "修改";
                if (IsDelete)
                    dr["操作"] += "删除";
                if (IsLog)
                    dr["操作"] += "该数据源更新日志";
            }
            return View(dt);
        }
        /// <summary>
        /// 查询当前供应商信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SourcesUpdataLog()
        {
            string Id = Request.QueryString[0];
            int menuId = int.Parse(Request.QueryString[1]);
            PublicHelpController pub = new PublicHelpController();
            #region 权限查看
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.deleteName))
            {
                ViewData["Delete"] = "true";
            }
            else
            {
                ViewData["Delete"] = "false";
            }
            #endregion
            bll.SystemConfigManagerbll system = new bll.SystemConfigManagerbll();
            int count;
            DataTable dt = system.GetProGetProductsourceUpdateLog(0, 10, Id, "1", null, null, out count, userInfo.User.UserId);
            ViewData["pageProCount"] = count % 10 == 0 ? count / 10 : count / 10 + 1;
            ViewData["pageRowsCount"] = count;
            ViewData["menuId"] = menuId;
            ViewData["sourceCode"] = Id;
            bool isShowid;
            dt = GetRenewalLogShow(dt, menuId,funName.Log, out isShowid);
            ViewData["IsShowId"] = isShowid;
            return View(dt);
        }
        /// <summary>
        /// 当前供应商日志查询分页
        /// </summary>
        /// <returns></returns>
        public ActionResult PageIndexChange1()
        {
            if (!IsIntType(Request.Form["pageIndex"]) || !IsIntType(Request.Form["pgCount"]))
                return Content("");
            int pageIndex = int.Parse(Request.Form["pageIndex"]);
            int pgCount = int.Parse(Request.Form["pgCount"]);
            int menuId = int.Parse(Request.Form["menuId"]);
            string log = Request.Form["log"];
            string sourceCode = Request.Form["sourceCode"];
            string beginTime = Request.Form["beginTime"];
            string endTime = Request.Form["endTime"];
            bll.SystemConfigManagerbll system = new bll.SystemConfigManagerbll();
            int count;
            DataTable dt = system.GetProGetProductsourceUpdateLog(pgCount * (pageIndex - 1), pgCount,sourceCode,log,beginTime,endTime, out count, userInfo.User.UserId);
            bool isShowId;
            dt = GetRenewalLogShow(dt, menuId,funName.Log, out isShowId);
            PublicHelpController pub = new PublicHelpController();
            bool IsDelete = false;
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.Log))
            {
                IsDelete = true;
            }
            string html = "";
            foreach (DataRow dr in dt.Rows)
            {
                html += "<tr id='" + dr["编号"] + "'>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "编号")
                    {
                        if (isShowId)
                        {
                            html += "<td>";
                            html += dr[i];
                            html += "</td>";
                        }
                    }
                    else
                    {
                        if (dr[i].ToString().Length > 25)
                        {
                            string str = dr[i].ToString();
                            html += "<td  class='curren'  onmouseout='mouseOut(this)' onmousemove='mouseOver(this)'>";
                            html += str.Substring(0, 25) + "...";
                            html += "<div>" + str + "</div>";
                        }
                        else
                        {
                            html += "<td>";
                            html += dr[i];
                        }
                        html += "</td>";
                    }
                }
                if (IsDelete)
                    html += "<td><a href='#' onclick=\"onDelete('" + dr["编号"] + "')\">删除</a></td>";
                else
                    html += "<td>无权限</td>";
                html += "</tr>";
            }
            //html += "<script>$('#pageCount').html('" + (count % 10 == 0 ? count / 10 : count / 10 + 1) + "');$('#pageRowsCount').html('" + count + "'); </script>";
            html += "╋" + (count % pgCount == 0 ? count / pgCount : count / pgCount + 1) + "╋" + count;
            return Content(html);
        }
        /// <summary>
        /// 删除数据源信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteDataSources(int Id)
        {
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            bool bol = systembll.DeleteProductsourceConfig(Id, userInfo.User.Id);
            if (!bol)
            {
                return Content("修改失败！");
            }
            else
            {
                return Content("true");
            }

        }
        /// <summary>
        /// 修改数据源信息
        /// </summary>
        /// <param name="Id">数据源编号</param>
        /// <returns></returns>
        public ActionResult UpdateDataSources()
        {
            int Id = int.Parse(Request.QueryString[0]);


            int menuId = int.Parse(Request.QueryString[1]);
            Dictionary<string, string> dic = GetDataConfig(funName.updateName, menuId);//获取权限
            ViewData["dic"] = dic;
            bll.SystemConfigManagerbll system = new bll.SystemConfigManagerbll();
            model.productsourceConfig pro = system.GetProductsourceConfig(Id);
            ViewData["dic"] = dic;
            ViewData["Id"] = pro.Id;
            ViewData["IsUpdate"] = "true";
            if (pro != null)
            {
                return View("InsertDataSources", pro);
            }
            else
            {
                return Content("访问的信息已经不存在！");
            }
        }
        /// <summary>
        /// 添加数据源信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertDataSources()
        {
            ViewData["IsUpdate"] = "false";
            bll.SystemConfigManagerbll system = new bll.SystemConfigManagerbll();
            Dictionary<string, string> dic = system.GetProductsource1(userInfo.User.Id);
            if (dic == null)
            {
                return View("../ErrorMsg/Index");
            }
            if (dic.Count == 0)
            {
                return Content("未找到供应商信息 请先添加供应商!");
            }
            ViewData["dicScode"] = dic;
            return View();
        }
        /// <summary>
        /// 添加修改数据源信息保存界面
        /// </summary>
        /// <param name="fr"></param>
        /// <returns></returns>
        public ActionResult InsertDataSourcesPost(FormCollection fr)
        {
            string IsUpdate = fr["IsUpdate"];
            bll.SystemConfigManagerbll system = new bll.SystemConfigManagerbll();
            if (IsUpdate == "true")
            {
                model.productsourceConfig pro = system.GetProductsourceConfig(int.Parse(fr["Id"]));
                pro.SourceCode = fr["SourceCode"] == "" ? pro.SourceCode : fr["SourceCode"];
                pro.SourcesAddress = fr["SourcesAddress"] == "" ? pro.SourcesAddress : fr["SourcesAddress"];
                pro.UserId = fr["UserId"] == "" ? pro.UserId : fr["UserId"];
                pro.UserPwd = fr["UserPwd"] == "" ? pro.UserPwd : fr["UserPwd"];
                pro.DataSources = fr["DataSources"] == "" ? pro.DataSources : fr["DataSources"];
                pro.DataSourcesLevel = fr["DataSourcesLevel"] == "" ? pro.DataSourcesLevel : fr["DataSourcesLevel"];
                pro.TimeStart = fr["TimeStart"] == "" ? pro.TimeStart : int.Parse(fr["TimeStart"]);
                bool bol = system.UpdateProducesourceConfig(pro, userInfo.User.Id);
                if (bol)
                    return Content("true");
                else
                    return Content("false");
            }
            else
            {
                model.productsourceConfig pro = new model.productsourceConfig();
                pro.SourceCode = fr["SourceCode"];
                pro.SourcesAddress = fr["SourcesAddress"];
                pro.UserId = fr["UserId"];
                pro.UserPwd = fr["UserPwd"];
                pro.DataSources = fr["DataSources"];
                pro.DataSourcesLevel = fr["DataSourcesLevel"];
                pro.TimeStart = fr["TimeStart"] == "" ? 0 : int.Parse(fr["TimeStart"]);
                bool isExist;
                bool bol = system.InsertProductsourceConfig(pro, userInfo.User.Id, out isExist);
                if (bol)
                    return Content("true");
                else if (isExist)
                    return Content("Exist");
                else
                    return Content("false");
            }
        }
        public ActionResult RenewalLogShow()
        {
            int menuId = int.Parse(Request.QueryString["menuId"]);
            PublicHelpController pub = new PublicHelpController();
            #region 权限查看
            if (!pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.deleteName))
            {
                ViewData["Delete"] = "true";
            }
            else
            {
                ViewData["Delete"] = "false";
            }
            #endregion
            bll.SystemConfigManagerbll system = new bll.SystemConfigManagerbll();
            int count;
            DataTable dt = system.GetProGetProductsourceUpdateLog(0, 10,"1",null,null, out count, userInfo.User.UserId);
            ViewData["pageProCount"] = count % 10 == 0 ? count / 10 : count / 10 + 1;
            ViewData["pageRowsCount"] = count;
            ViewData["menuId"] = menuId;
            bool isShowid;
            dt = GetRenewalLogShow(dt, menuId,funName.selectName, out isShowid);
            ViewData["IsShowId"] = isShowid;
            return View(dt);
        }
        private DataTable GetRenewalLogShow(DataTable dt, int menuId,string FunName, out bool IsShowId)
        {
            IsShowId = true;//是否显示Id列
            PublicHelpController pub = new PublicHelpController();
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            string[] str = pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, FunName);
            //Dictionary<string,string> st = GetRenewalLogShowData(menuId, funName.selectName);
            List<DataColumn> list = new List<DataColumn>();
            foreach (DataColumn t in dt.Columns)
            {
                if (str.Contains(t.ColumnName))
                {
                    if (t.ColumnName == "Id")
                    {
                        t.ColumnName = "编号";
                    }
                    if (t.ColumnName == "ErrorMsg")
                    {
                        t.ColumnName = "错误信息";
                    }
                    if (t.ColumnName == "errorSrc")
                    {
                        t.ColumnName = "错误路径";
                    }
                    if (t.ColumnName == "errorMsgDetails")
                    {
                        t.ColumnName = "错误详情";
                    }
                    if (t.ColumnName == "UserId")
                    {
                        t.ColumnName = "操作人编号";
                    }
                    if (t.ColumnName == "errorTime")
                    {
                        t.ColumnName = "日志时间";
                    }
                    if (t.ColumnName == "operation")
                    {
                        t.ColumnName = "数据读取日志";
                    }
                    if (t.ColumnName == "Def2")
                    {
                        t.ColumnName = "错误行号";
                    }
                    if (t.ColumnName == "Def3")
                    {
                        t.ColumnName = "供应商编号";
                    }
                    if (t.ColumnName == "Def4")
                    {
                        t.ColumnName = "当前供应商第几次更新";
                    }
                }
                else if (t.ColumnName == "UserName")
                {
                    t.ColumnName = "操作人";
                }
                else if (t.ColumnName == "sourceName")
                {
                    t.ColumnName = "供应商";
                }
                else if (t.ColumnName == "Id")
                {
                    t.ColumnName = "编号";
                    IsShowId = false;
                }
                else
                {
                    list.Add(t);
                }
            }
            foreach (DataColumn d in list)
            {
                dt.Columns.Remove(d);
            }
            return dt;
        }
        /// <summary>
        /// 返回有权限字段数据库名称 中文名称
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="FunName"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetRenewalLogShowData(int menuId, string FunName)
        {
            PublicHelpController pub = new PublicHelpController();
            pbxdata.bll.SystemConfigManagerbll systembll = new bll.SystemConfigManagerbll();
            string[] temp = systembll.getDataName("errorlog");
            string[] str = pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, FunName);
            Dictionary<string, string> lis = new Dictionary<string, string>();
            foreach (string t in temp)
            {
                if (str.Contains(t))
                {
                    if (t == "Id")
                    {
                        lis.Add("Id", "编号");
                    }
                    if (t == "ErrorMsg")
                    {
                        lis.Add("ErrorMsg", "错误信息");
                    }
                    if (t == "errorSrc")
                    {
                        lis.Add("errorSrc", "错误路径");
                    }
                    if (t == "errorMsgDetails")
                    {
                        lis.Add("errorMsgDetails", "错误详情");
                    }
                    if (t == "UserId")
                    {
                        lis.Add("UserId", "操作人编号");
                    }
                    if (t == "errorTime")
                    {
                        lis.Add("errorTime", "日志时间");
                    }
                    if (t == "operation")
                    {
                        lis.Add("operation", "数据读取日志");
                    }
                    if (t == "Def2")
                    {
                        lis.Add("Def2", "错误行号");
                    }
                    if (t == "Def3")
                    {
                        lis.Add("Def3", "供应商编号");
                    }
                    if (t == "Def4")
                    {
                        lis.Add("Def4", "当前供应商第几次更新");
                    }
                }
                else if (t == "UserName")
                {
                    lis.Add("UserName", "操作人");
                }
                else if (t == "sourceName")
                {
                    lis.Add("sourceName", "供应商");
                }
            }
            return lis;
        }
        public ActionResult PageIndexChange()
        {
            if (!IsIntType(Request.Form["pageIndex"]) || !IsIntType(Request.Form["pgCount"]))
                return Content("");
            int pageIndex = int.Parse(Request.Form["pageIndex"]);
            int pgCount = int.Parse(Request.Form["pgCount"]);
            int menuId = int.Parse(Request.Form["menuId"]);
            string log = Request.Form["log"];
            string beginTime = Request.Form["beginTime"];
            string endTime = Request.Form["endTime"];
            bll.SystemConfigManagerbll system = new bll.SystemConfigManagerbll();
            int count;
            DataTable dt = system.GetProGetProductsourceUpdateLog(pgCount * (pageIndex - 1), pgCount, log, beginTime, endTime, out count, userInfo.User.UserId);
            bool isShowId;
            dt = GetRenewalLogShow(dt, menuId,funName.selectName, out isShowId);
            PublicHelpController pub = new PublicHelpController();
            bool IsDelete = false;
            if (pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.deleteName))
            {
                IsDelete = true;
            }
            string html = "";
            foreach (DataRow dr in dt.Rows)
            {
                html += "<tr id='" + dr["编号"] + "'>";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName == "编号")
                    {
                        if (isShowId)
                        {
                            html += "<td>";
                            html += dr[i];
                            html += "</td>";
                        }
                    }
                    else
                    {
                        if (dr[i].ToString().Length > 20)
                        {
                            string str = dr[i].ToString();
                            html += "<td  class='curren'  onmouseout='mouseOut(this)' onmousemove='mouseOver(this)'>";
                            html += str.Substring(0, 20) + "...";
                            html += "<div>" + str + "</div>";
                        }
                        else
                        {
                            html += "<td>";
                            html += dr[i];
                        }
                        html += "</td>";
                    }
                }
                if (IsDelete)
                    html += "<td><a href='#' onclick=\"onDelete('" + dr["编号"] + "')\">删除</a></td>";
                else
                    html += "<td>无权限</td>";
                html += "</tr>";
            }
            //html += "<script>$('#pageCount').html('" + (count % 10 == 0 ? count / 10 : count / 10 + 1) + "');$('#pageRowsCount').html('" + count + "'); </script>";
            html += "╋" + (count % pgCount == 0 ? count / pgCount : count / pgCount + 1) + "╋" + count;
            return Content(html);
        }
        /// <summary>
        /// 判断是否为int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool IsIntType(object obj)
        {
            try
            {
                int i = int.Parse(obj.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
