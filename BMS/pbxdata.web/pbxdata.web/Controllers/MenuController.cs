using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class MenuController : BaseController
    {
        //
        // GET: /Menu/

        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult getdata()
        {
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("roleId",SqlDbType.Int,4)
            };
            ipara[0].Value = userInfo.User.personaId;
            //List<model.menu> list = menuBll.getMenu(ipara, "menuSelectF");
            List<model.menu> list = menuBll.getMenu(ipara, "menuSelectFList");
            
            return View(list);
        }


        /// <summary>
        /// 编辑菜单列表
        /// </summary>
        /// <returns></returns>
        public ActionResult editdata(int id)
        {
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("id",SqlDbType.Int,4)
            };
            ipara[0].Value = id;
            List<model.menu> list = menuBll.getMenu(ipara, "menuEdit");
            model.menu menuMd = new model.menu();

            menuMd = list.Where(c => c.Id == id).ToList()[0];

            return View(menuMd);
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <returns></returns>
        public string updatedata()
        {
            string s = string.Empty;
            string menuName = Request.Form["menuName"].ToString();
            string MenuSrc = Request.Form["MenuSrc"].ToString();

            int MenuLevel = helpcommon.ParmPerportys.GetNumParms(Request.Form["MenuLevel"].ToString());
            int MenuIndex = helpcommon.ParmPerportys.GetNumParms(Request.Form["MenuIndex"].ToString());
            int UserId = 1;
            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["Id"].ToString());
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                
                new SqlParameter("menuName",SqlDbType.NVarChar,20),
                new SqlParameter("MenuSrc",SqlDbType.NVarChar,500),
                new SqlParameter("MenuLevel",SqlDbType.Int,4),
                new SqlParameter("MenuIndex",SqlDbType.Int,4),
                new SqlParameter("UserId",SqlDbType.Int,4),
                new SqlParameter("Id",SqlDbType.Int,4)
            };
            ipara[0].Value = menuName;
            ipara[1].Value = MenuSrc;
            ipara[2].Value = MenuLevel;
            ipara[3].Value = MenuIndex;
            ipara[4].Value = UserId;
            ipara[5].Value = id;

            s = menuBll.Update(ipara, "menuUpdate");

            return s;
        }


        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        public string deldata()
        {
            string s = string.Empty;

            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["id"]);
            bll.menubll menuBll = new bll.menubll();
            s = menuBll.del(id);
            menuBll = null;

            return s;
        }

        /// <summary>
        /// 删除子菜单
        /// </summary>
        /// <returns></returns>
        public string delSubMenu(int id)
        {
            string s = string.Empty;
            int ids = helpcommon.ParmPerportys.GetNumParms(id);
            bll.menubll menubll = new bll.menubll();
            IDataParameter[] ipara=new IDataParameter[]{
             new SqlParameter("Id",SqlDbType.Int,4),
              new SqlParameter("PersondId",SqlDbType.Int,4)
            };
            ipara[0].Value = ids;
            ipara[1].Value = userInfo.User.personaId;
            s = menubll.Delete(ipara, "DeleteSubMenu");        
            menubll = null;
            return s;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <returns></returns>
        public string adddata(FormCollection collection)
        {
            string s = string.Empty;
            string menuName = Request.Form["menuName"].ToString();
            string MenuSrc = Request.Form["MenuSrc"].ToString();

            int MenuLevel = helpcommon.ParmPerportys.GetNumParms(Request.Form["MenuLevel"].ToString());
            int MenuIndex = helpcommon.ParmPerportys.GetNumParms(Request.Form["MenuIndex"].ToString());
            int UserId = 1;
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("menuName",SqlDbType.NVarChar,20),
                new SqlParameter("MenuSrc",SqlDbType.NVarChar,500),
                new SqlParameter("MenuLevel",SqlDbType.Int,4),
                new SqlParameter("MenuIndex",SqlDbType.Int,4),
                new SqlParameter("UserId",SqlDbType.Int,4)
            };
            ipara[0].Value = menuName;
            ipara[1].Value = MenuSrc;
            ipara[2].Value = MenuLevel;
            ipara[3].Value = MenuIndex;
            ipara[4].Value = UserId;

            s = menuBll.Add(ipara, "menuAdd");

            return s;
        }


        /// <summary>
        /// 查看菜单详情(子菜单)
        /// </summary>
        /// <returns></returns>
        public ActionResult menuDetails(int id)
        {
            ViewData["MenuLevel"] = id;
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("roleId",SqlDbType.Int,4),
                new SqlParameter("menuid",SqlDbType.Int,4)
            };
            ipara[0].Value = userInfo.User.personaId;
            ipara[1].Value = id;
            List<model.menu> list = menuBll.getMenu(ipara, "menuSelectC");

            return View(list);
        }

    }
}
