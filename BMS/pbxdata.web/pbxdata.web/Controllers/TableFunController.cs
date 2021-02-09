using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class TableFunController : BaseController
    {
        //
        // GET: /TableFun/

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult getMenu()
        {
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                //new SqlParameter("MenuId",SqlDbType.NVarChar,20)
            };
            //ipara[0].Value = "张三";
            List<model.menu> list = menuBll.getMenu(ipara, "menuChild");

            return View(list);
        }

        /// <summary>
        /// 获取功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult getFun(int id)
        {
            ViewData["MenuId"] = id;
            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("MenuId",SqlDbType.NVarChar,20)
            };
            ipara[0].Value = id;
            List<model.funpermisson> list = funPermissonBll.getFun(ipara, "funPermissonSelect");

            return View(list);
        }


        /// <summary>
        /// 添加功能
        /// </summary>
        /// <returns></returns>
        public string adddata(FormCollection collection)
        {
            string s = string.Empty;
            string funName = Request.Form["funName"].ToString();
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"].ToString());

            int funIndex = helpcommon.ParmPerportys.GetNumParms(Request.Form["funIndex"].ToString());
            int UserId = userInfo.User.Id;
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("funName",SqlDbType.NVarChar,20),
                new SqlParameter("menuId",SqlDbType.Int,4),
                new SqlParameter("funIndex",SqlDbType.Int,4),
                new SqlParameter("UserId",SqlDbType.Int,4)
            };
            ipara[0].Value = funName;
            ipara[1].Value = menuId;
            ipara[2].Value = funIndex;
            ipara[3].Value = UserId;

            s = menuBll.Add(ipara, "funPermissonAdd");

            return s;
        }


        /// <summary>
        /// 编辑功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult editFun(int id,int MenuId)
        {
            ViewData["MenuId"] = MenuId;
            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("id",SqlDbType.Int,4)
            };
            ipara[0].Value = id;
            //List<model.funpermisson> list = funPermissonBll.getFun(ipara, "funpermissonEdit");
            model.funpermisson funpermissonMd = new model.funpermisson();
            funpermissonMd = funPermissonBll.getFun(ipara, "funpermissonEdit")[0];
            //funpermissonMd = list.Where(c => c.Id == id).ToList()[0];

            return View(funpermissonMd);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        public string Delete()
        {
            int id=helpcommon.ParmPerportys.GetNumParms(Request.Form["Id"]);
            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            string s = funPermissonBll.del(id);
            funPermissonBll = null;

            return s;
        }

        /// <summary>
        /// 更新功能
        /// </summary>
        /// <returns></returns>
        public string updateFun()
        {
            //{ id: id, MenuId: MenuId, FunName: FunName, FunIndex: FunIndex }
            string s = string.Empty;
            string MenuId = Request.Form["MenuId"].ToString();
            string FunName = Request.Form["FunName"].ToString();
            int FunIndex = helpcommon.ParmPerportys.GetNumParms(Request.Form["FunIndex"].ToString());
            int UserId = userInfo.User.Id;
            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["id"].ToString());
            IDataParameter[] ipara = new IDataParameter[]{
                
                new SqlParameter("MenuId",SqlDbType.NVarChar,20),
                new SqlParameter("FunName",SqlDbType.NVarChar,500),
                new SqlParameter("FunIndex",SqlDbType.Int,4),
                new SqlParameter("UserId",SqlDbType.Int,4),
                new SqlParameter("id",SqlDbType.Int,4)
            };
            ipara[0].Value = MenuId;
            ipara[1].Value = FunName;
            ipara[2].Value = FunIndex;
            ipara[3].Value = UserId;
            ipara[4].Value = id;

            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            s = funPermissonBll.Update(ipara, "funpermissonUpdate");

            return s;
        }

    }
}
