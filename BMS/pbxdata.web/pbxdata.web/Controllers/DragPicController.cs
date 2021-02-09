using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class DragPicController : Controller
    {
        //
        // GET: /DragPic/

        public ActionResult PeituExchange()
        {
            ViewData["style"] = Request.QueryString["style"].ToString();
            return View();
        }
        /// <summary>
        /// 获取配图
        /// </summary>
        /// <returns></returns>
        public string GetPeituPic()
        {
            string s = string.Empty;
            string style = Request.Form["style"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def2,1)='2' and Def4='1' order by CONVERT(int,substring(def2,3,LEN(def2)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["stylePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存配图顺序
        /// </summary>
        /// <returns></returns>
        public string SavePeitu()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update stylepic set def2='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除配图
        /// </summary>
        /// <returns></returns>
        public string DeletePeituPic()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update stylepic set Def2='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }


        public ActionResult XiJieExchange()
        {
            ViewData["style"] = Request.QueryString["style"].ToString();
            return View();
        }

        /// <summary>
        /// 获取细节图
        /// </summary>
        /// <returns></returns>
        public string GetXiJiePic()
        {
            string s = string.Empty;
            string style = Request.Form["style"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def3,1)='2' and Def4='1' order by CONVERT(int,substring(def3,3,LEN(def3)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["stylePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存细节图顺序
        /// </summary>
        /// <returns></returns>
        public string SaveXiJietu()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update stylepic set def3='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除细节图
        /// </summary>
        /// <returns></returns>
        public string DeleteXiJiePic()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update stylepic set Def3='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }


        public ActionResult ZhutuExchange()
        {
            ViewData["style"] = Request.QueryString["style"].ToString();
            return View();
        }

        /// <summary>
        /// 获取主图--款号表
        /// </summary>
        /// <returns></returns>
        public string GetZhutuPic()
        {
            string s = string.Empty;
            string style = Request.Form["style"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def1,1)='2' and Def4='1' order by CONVERT(int,substring(def1,3,LEN(def1)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["stylePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存主图顺序--款号表
        /// </summary>
        /// <returns></returns>
        public string SaveZhutu()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update stylepic set def1='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除主图--款号表
        /// </summary>
        /// <returns></returns>
        public string DeleteZhutuPic()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update stylepic set Def1='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }

        /// <summary>
        /// 获取主图--货号表
        /// </summary>
        /// <returns></returns>
        public string GetZhutuPic1()
        {
            string s = string.Empty;
            string scode = Request.Form["scode"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from scodepic where scode='" + scode + "' and Left(Def1,1)='2' and Def4='1' order by CONVERT(int,substring(def1,3,LEN(def1)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["scodePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存主图顺序--货号表
        /// </summary>
        /// <returns></returns>
        public string SaveZhutu1()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update scodepic set def1='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除主图--货号表
        /// </summary>
        /// <returns></returns>
        public string DeleteZhutuPic1()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update scodepic set Def1='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }

        /// <summary>
        /// 对调图片顺序
        /// </summary>
        /// <returns></returns>
        public string Exchange()
        {
            string handle = Request.Form["handle"].ToString();
            string oNear = Request.Form["oNear"].ToString();
            string type = Request.Form["type"].ToString();
            DataTable dt = new DataTable();
            string Item = DbHelperSQL.Query(@"select * from stylepic where Id='" + handle + "'").Tables[0].Rows[0][type].ToString();
            string Item2 = DbHelperSQL.Query(@"select * from stylepic where Id='" + oNear + "'").Tables[0].Rows[0][type].ToString();
            DbHelperSQL.ExecuteSql(@"update stylepic set " + type + "='" + Item2 + "' where Id='" + handle + "';update stylepic set " + type + "='" + Item + "' where Id='" + oNear + "';");
            return "";
        }

       
    }
}
