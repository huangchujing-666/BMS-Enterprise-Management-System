
using pbxdata.bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class LayoutController :BaseController
    {
        //
        // GET: /Layout/

        //public ActionResult Index()
        //{
        //    return View();
        //}


        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public string getdatamenuF()
        {
            string s = "{\"menu\":[";
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("roleId",SqlDbType.Int,4)
            };
            ipara[0].Value = userInfo.User.personaId;
            
            List<model.menu> list = menuBll.getMenu(ipara, "menuSelectF");
            foreach (var item in list)
            {
                s += "{\"menuid\":\"" + item.Id + "\",\"menuname\":\"" + item.menuName + "\"},";
            }
            
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            s += "]}";
            return s;
        }
        /// <summary>
        /// 获取子菜单列表
        /// </summary>
        /// <returns></returns>
        public string getdatamenuC(int menuid)
        {
            string s = "{\"menu\":[";
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("roleId",SqlDbType.Int,4),
                new SqlParameter("menuid",SqlDbType.Int,4)
            };
            ipara[0].Value = userInfo.User.personaId;
            ipara[1].Value = menuid;

            List<model.menu> list = menuBll.getMenu(ipara, "menuSelectC");
            foreach (var item in list)
            {
                s += "{\"menuid\":\"" + item.Id + "\",\"menuname\":\"" + item.menuName + "\",\"menusrc\":\"" + item.MenuSrc + "\"},";
            }
            if (list.Count == 0)
            {
                s += "{\"menuid\":\"" + 0 + "\",\"menuname\":\"" + 0 + "\"},";
            }
            
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            s += "]}";

            
            return s;
        }
        /// <summary>
        /// 显示当前用户
        /// </summary>
        /// <returns></returns>
        public string UserShow() 
        {
            ProductTypeBLL ptb = new ProductTypeBLL();
            string userName = userInfo.User.userName; 
            DataTable dtPersion = ptb.GetPersonaIdByUserName(userName);//通过当前用户查找当前用户所属角色
            string Name = dtPersion.Rows[0][1].ToString();//用户名
            string persionName = ptb.GetPersionNameByid(dtPersion.Rows[0][0].ToString());
            int thistime = DateTime.Now.Hour;
            string greeting = "";
            if (thistime > 0 && thistime < 12)
            {
                greeting = "上午好！@_@";
            }
            else 
            {
                greeting = "下午好！@_@";
            }
            return userName + "❤" + Name + "❤" + persionName + "❤" + greeting;
        }
        
        /// <summary>
        /// 跳转到登录界面
        /// </summary>
        /// <returns></returns>
        public string Logoff() 
        {
            Session["UserMsg"] = null;
            return "/Login/Index";
        }

    }
}
