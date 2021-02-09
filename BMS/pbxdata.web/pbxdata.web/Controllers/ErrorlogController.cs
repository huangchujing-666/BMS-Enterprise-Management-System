using pbxdata.bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace pbxdata.web.Controllers
{
    public class ErrorlogController : BaseController
    {
        //
        // GET: /Errollog/
        string mess = "";
        /// <summary>
        /// 显示异常信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowErrorlog()
        {
            if (Session["count"] == null)
                Session["count"] = 20;
            if (Session["pageIndex"] == null)
                Session["pageIndex"] = 1;
            bll.Errorlogbll errorlog = new bll.Errorlogbll();
            int menuId = int.Parse(Request.QueryString["menuId"]);
            PublicHelpController pub = new PublicHelpController();
            if (!pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["IsDelete"] = pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.deleteName);
            ViewData["show"] = pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.selectName);
            ViewData["menuId"] = menuId;
            ViewData["backups"] = pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.backups);
            ViewData["recover"] = pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.recover);
            int listCount;
            int count = int.Parse(Session["count"].ToString());
            List<model.errorlog> list = errorlog.GetListErrorlog(0, count, 1, out listCount, out mess);
            if (mess != "")
            {
                return Content(mess);
            }
            ViewBag.pageProCount = listCount % count == 0 ? listCount / count : listCount / count + 1;
            return View(list);

        }
        public ActionResult PageIndexChange(FormCollection fr, string Id)
        {
            string mess = "";
            Errorlogbll errorlogbll = new Errorlogbll();
            List<model.errorlog> li = null;
            int operation = int.Parse(fr["operation"]);//显示错误信息的类型
            int menuId = int.Parse(Request.Form["menuId"]);
            int count = int.Parse(Session["count"].ToString());//分页单页面数据个数
            //int pageIndex = int.Parse(Session["pageIndex"]!=null?Session["pageIndex"].ToString():"1");
            int listCount = 0;
            string[] str = Id.Split('-');
            PublicHelpController pub = new PublicHelpController();
            string[] strr = pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.selectName);
            bool IsDelete = pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.deleteName);
            int pageIndex = int.Parse(str[1]);
            li = errorlogbll.GetListErrorlog(0, 0, operation, out listCount, out mess);
            switch (str[0])
            {
                case "1":
                    pageIndex = 1;
                    break;
                case "2":
                    pageIndex = pageIndex - 1;
                    break;
                case "3":
                    pageIndex = pageIndex + 1;
                    break;
                case "4":
                    pageIndex = pageIndex = listCount % count > 0 ? listCount / count + 1 : listCount / count;
                    break;
                case "5":
                    pageIndex = int.Parse(str[1]);
                    break;
            }
            if (pageIndex < 1)
            {
                return Content("");
            }
            else if (pageIndex > (listCount % count > 0 ? listCount / count + 1 : listCount / count))
            {
                return Content("");
            }
            else
            {
                Session["pageIndex"] = pageIndex;
            }
            li = errorlogbll.GetListErrorlog(count * (pageIndex - 1), count, operation, out listCount, out mess);
            ViewBag.pageCount = listCount % count > 0 ? listCount / count + 1 : listCount / count;
            string html = "";
            int i = count * (pageIndex - 1) + 1;
            foreach (model.errorlog err in li)
            {

                html += "<tr id='" + err.Id + "'><td>" + (i++) + "</td>";
                if (strr.Contains("ErrorMsg"))
                {
                    if (err.ErrorMsg.Length > 30)
                    {
                        html += "<td  onmousemove='mouseOver(this)' class='curren' onmouseout='mouseOut(this)'>" + err.ErrorMsg.Substring(0, 30) + "...<div><p>" + err.ErrorMsg + "</p></div></td>";
                    }
                    else 
                    html += "<td>" + err.ErrorMsg + "</td>";
                }
                if (strr.Contains("errorSrc"))
                {
                    if (err.errorSrc.Length > 30)
                    {
                        html += "<td  onmousemove='mouseOver(this)' class='curren' onmouseout='mouseOut(this)'>" + err.errorSrc.Substring(0, 30) + "...<div><p>" + err.errorSrc + "</p></div></td>";
                    }
                    else 
                    html += "<td>" + err.errorSrc + "</td>";
                }
                if (strr.Contains("errorMsgDetails"))
                {
                    if (err.errorMsgDetails.Length > 30)
                    {
                        html += "<td  onmousemove='mouseOver(this)' class='curren' onmouseout='mouseOut(this)'>" + err.errorMsgDetails.Substring(0, 30) + "...<div><p>" + err.errorMsgDetails + "</p></div></td>";
                    }
                    else 
                    html += "<td>" + err.errorMsgDetails + "</td>";
                }
                if (strr.Contains("UserId"))
                {
                    html += "<td>" + err.UserId + "</td>";
                }
                if (strr.Contains("errorTime"))
                {
                    html += "<td>" + (err.errorTime != null ? err.errorTime.ToString() : "") + "</td>";
                }
                if (IsDelete)
                {
                    html += " <td><a href='#' onclick='IsYesUpdate('" + err.Id + "')'>删除</a></td>";
                }
                else
                {
                    html += " <td>无权限</td>";
                }
                html += "</tr>";
                //@Html.ActionLink("修改", "UpdateCustom/" + cus.Id)
                //@Html.ActionLink("删除", "DeleteCustom/" + cus.Id)
            }
            return Content(html + "+++" + (listCount % count > 0 ? listCount / count + 1 : listCount / count));
        }
        public ContentResult DeleteErrorlog(int Id)
        {
            string mess = "";
            Errorlogbll errorlogbll = new Errorlogbll();

            if (errorlogbll.DeleteErrorlog(Id, out mess))
            {
                return Content("true");
            }
            else if (mess.Equals("该条信息已经不存在"))
            {
                return Content("noE");
            }
            else
            {
                return Content(mess + "<a href='javascript:history.go(-1)'>返回</a>");
            }
        }
        ///// <summary>
        ///// 修改客户信息
        ///// </summary>
        ///// <param name="Id">客户id</param>
        ///// <returns></returns>
        //public ActionResult UpdateErrorlog(int Id)
        //{
        //    string mess = "";
        //    Errorlogbll errorlogbll = new bll.Errorlogbll("ConnectionString", out mess);
        //    //判断是否有错误信息
        //    if (!string.IsNullOrEmpty(mess))
        //    {
        //        return Content(mess);
        //    }
        //   // model.custom cus = errorlogbll(Id);//找到需要修改实体

        //    if (cus != null)
        //    {
        //        return View("InsertOrUpdateView", cus);
        //    }
        //    else
        //    {
        //        return Content("该信息已不存在！<a href='/Custom/ShowCustom'>刷新</a> <a href='javascript:history.go(-1)'>返回</a>");
        //    }
        //}
        /// <summary>
        /// 页面搜索事件
        /// </summary>
        /// <param name="fr"></param>
        /// <returns></returns>
        public string Json(List<model.errorlog> list)
        {
            string json = "{";
            foreach (model.errorlog err in list)
            {
                json += "{\"Id\":\"" + err.Id + "\",\"ErrorMsg\"}";

            }
            json += "}";
            return json;
        }
        /// <summary>
        /// 数据备份
        /// </summary>
        /// <param name="fr"></param>
        public string DateBackups(FormCollection fr)
        {
            bll.Errorlogbll errorlog = new bll.Errorlogbll();
            string path = "E:\\fabu\\ExcelFile\\database";
            if (!string.IsNullOrWhiteSpace(path))
            {
                mess = "";
                string DBPath;//备份成功后的文件在服务器的完全路径
                bool bo = errorlog.DateBackups(path, out mess, out DBPath);
                if (bo)
                {
                    model.errorlog err = new model.errorlog()
                    {
                        ErrorMsg = "备份数据库",
                        errorMsgDetails = "数据库备份",
                        errorSrc = "ErrorloController.cs->DateBackups()",
                        errorTime = DateTime.Now,
                        UserId = userInfo.User.Id,
                        operation = 2
                    };
                    errorlog.InsertErrorlog(err);

                    FileInfo file = new FileInfo(Server.MapPath("~/ErrorlogPath.txt"));
                    FileStream sf = file.Open(FileMode.OpenOrCreate);

                    StreamReader sr = new StreamReader(sf, System.Text.Encoding.UTF8);
                    string str = sr.ReadToEnd();
                    sr.Close();
                    str = DBPath.Replace(".bak", "") + "\r\n" + str;
                    StreamWriter sw = new StreamWriter(file.Open(FileMode.OpenOrCreate), System.Text.Encoding.UTF8);
                    sw.Write(str);
                    sw.Close();

                    return bo.ToString() + "|" + DBPath;
                }
                else
                {
                    model.errorlog err = new model.errorlog()
                    {
                        ErrorMsg = "备份数据库",
                        errorMsgDetails = mess,
                        errorSrc = "ErrorloController.cs->DateBackups()",
                        errorTime = DateTime.Now,
                        UserId = userInfo.User.Id,
                        operation = 2
                    };
                    errorlog.InsertErrorlog(err);
                    return mess;
                }
            }
            //string st=  Server.MapPath("~/ErrorlogPath.txt");
            //FileInfo file = new FileInfo(st);
            // FileStream sf = file.Open(FileMode.OpenOrCreate);
            // StreamReader sr = new StreamReader(sf);
            // string str = sr.ReadLine();
            return "请输入文件路径！";
        }
        [HttpGet]
        public string GetDBPath()
        {
            FileInfo file = new FileInfo(Server.MapPath("~/ErrorlogPath.txt"));
            FileStream sf = file.Open(FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(sf);
            string str1 = sr.ReadLine();
            string str = str1 == null ? null : str1.Replace("\r\n", "");
            sr.Close();
            sf.Close();
            return str;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="id">文件名称</param>
        /// <returns></returns>
        public ActionResult Down(string id)
        {
            //string path = "http://192.168.1.124:50003/dataBase/" + id;
            //string path = Server.MapPath("~/database/" + id + ".bak");
            string path = "E:/fabu/ExcelFile/database/" + id + ".bak";
            bool a = System.IO.File.Exists(path);

            //FileInfo file = new FileInfo(path);
            //FileStream fs = file.Open(FileMode.Open, FileAccess.Read);
            //StreamReader sr = new StreamReader(fs);
            //byte[] by = new byte[file.Length];
            //int index = fs.Read(by, 0, (int)file.Length);
            //sr.Close();
            if (a)
                return File(path, "application/octet-stream", id + ".bak");
            return Content("该文件已经不存在！");
        }
        public ActionResult Recover(string id)
        {
            bll.Errorlogbll errorlog = new bll.Errorlogbll();
            string path = "E:/fabu/ExcelFile/database/" + id + ".bak";
            if (errorlog.Recover(path, out mess))
            {
                model.errorlog err = new model.errorlog()
                {
                    ErrorMsg = "备份数据库",
                    errorMsgDetails = "数据库恢复",
                    errorSrc = "ErrorloController.cs->Recover()",
                    errorTime = DateTime.Now,
                    UserId = userInfo.User.Id,
                    operation = 2
                };
                errorlog.InsertErrorlog(err);
                return Content("True");
            }
            model.errorlog errr = new model.errorlog()
            {
                ErrorMsg = "备份数恢复",
                errorMsgDetails = mess,
                errorSrc = "ErrorloController.cs->Recover()",
                errorTime = DateTime.Now,
                UserId = userInfo.User.Id,
                operation = 2
            };
            errorlog.InsertErrorlog(errr);
            return Content(mess);
        }
    }
}
