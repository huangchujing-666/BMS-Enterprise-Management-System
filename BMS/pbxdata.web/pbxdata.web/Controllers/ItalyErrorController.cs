using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class ItalyErrorController : BaseController
    {
        //
        // GET: /ItalyError/

        public ActionResult ShowError()
        {
            int menuId = int.Parse(Request.QueryString["menuId"]);
            PublicHelpController pub = new PublicHelpController();
            //判断是否有该功能权限
            if (!pub.isFunPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId), menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            string[] str= pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId),menuId,funName.selectName);
            bll.ItalyErrorbll bll = new bll.ItalyErrorbll();
            int pageRowsCount;
            DataTable dt = bll.GetItaly(null, null, null, 0, 10, out pageRowsCount);
            ViewData["pageCount"] = pageRowsCount % 10 == 0 ? pageRowsCount / 10 : pageRowsCount / 10+1;
            ViewData["pageRowsCount"] = pageRowsCount;
            ViewData["menuId"] = menuId;
            ChangeTableColumnName(dt, str);
            return View(dt);
        }
        public ActionResult PageIndexChange()
        {
            string[] str = new string[10];//查询条件
            int ii;
            if (!int.TryParse(Request.Form["pageIndex"], out ii) || !int.TryParse(Request.Form["pgCount"], out ii))
                return Content("");
            int pageIndex = int.Parse(Request.Form["pageIndex"]);
            int pgCount = int.Parse(Request.Form["pgCount"]);
            str[0] = Request.Form["beginTime"] == null ? "" : Request.Form["beginTime"];//开始时间
            if (string.IsNullOrWhiteSpace(Request.Form["endTime"]))//结束时间
            {
                str[1] = "";
            }
            else
            {
                str[1] = Request.Form["endTime"] + " 23:59:59";
            }
            DateTime? createTime;
            if (str[0] == "")
            {
                createTime = null;
            }
            else
            {
                createTime = DateTime.Parse(str[0]);
            }
            DateTime? createTime1;
            if (str[1] == "")
            {
                createTime1 = null;
            }
            else
            {
                createTime1 = DateTime.Parse(str[1]);
            }
            str[2] = Request.Form["Italy"] == null ? "" : Request.Form["Italy"];//数据源code
            str[3] = Request.Form["menuId"] == null ? "" : Request.Form["menuId"];
            bll.ItalyErrorbll bll = new bll.ItalyErrorbll();
            Exception ex;
            int pageRowsCount;
            //获取显示的字段列表
            PublicHelpController pub = new PublicHelpController();
            string[] strr = pub.getFiledPermisson(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId),int.Parse( str[3]), funName.selectName);
            DataTable table = bll.GetItaly(str[2], createTime, createTime1, pgCount * (pageIndex - 1), pgCount, out pageRowsCount);

            ChangeTableColumnName(table, strr);
            //DataTable table = bll.GetProductOrder(pgCount * (pageIndex - 1), pgCount, str, out ex);
            string html = "";
            foreach (DataRow dr in table.Rows)
            {
                html += "<tr>";
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    html += "<td>";
                    html += dr[i];
                    html += "</td>";
                }
                html += "</tr>";
            }
            //html += "<script>$('#pageCount').html('" + (count % 10 == 0 ? count / 10 : count / 10 + 1) + "');$('#pageRowsCount').html('" + count + "'); </script>";
            html += "╋" + (pageRowsCount % pgCount == 0 ? pageRowsCount / pgCount : pageRowsCount / pgCount + 1) + "╋" + pageRowsCount;
            ViewData["menuId"] = str[3];
            return Content(html);
        }
        /// <summary>
        /// 改变datatable列名
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="str"></param>
        private void ChangeTableColumnName(DataTable dt,string[] str)
        {
            try
            {
                List<DataColumn> list = new List<DataColumn>();
                foreach (DataColumn c in dt.Columns)
                {
                    if (str.Contains(c.ColumnName))
                    {
                        if (c.ColumnName == "Id")
                        {
                            c.ColumnName = "编号";
                        }
                        else if (c.ColumnName == "ItalyCode")
                        {
                            c.ColumnName = "供应商编号";
                        }
                        else if (c.ColumnName == "createTime")
                        {
                            c.ColumnName = "时间";
                        }
                        else if (c.ColumnName == "msg")
                        {
                            c.ColumnName = "错误详情";
                        }
                        else if (c.ColumnName == "methed")
                        {
                            c.ColumnName = "错误方法";
                        }
                        else if (c.ColumnName == "def1")
                        {
                            
                        }
                        else if (c.ColumnName == "def2")
                        {
                            
                        }
                        else if (c.ColumnName == "def3")
                        {

                        }
                        else if (c.ColumnName == "def4")
                        {

                        }
                        else if (c.ColumnName == "def5")
                        {

                        }
                    }
                    else
                    {
                        list.Add(c);
                    }
                }
                foreach (var c in list)
                {
                    dt.Columns.Remove(c);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}
