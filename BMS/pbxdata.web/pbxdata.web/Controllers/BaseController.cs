using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/

        //public ActionResult Index()
        //{
        //    return View();
        //}
        model.userInfo us = new model.userInfo();
        public model.userInfo userInfo { get { return us; } set { us = value; } }//用户完整信息(包涵用户资料，用户菜单、功能权限，用户淘宝店授权信息)


        //Action方法执行之前执行此方法
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //校验用户是否已经登录
            if (filterContext.HttpContext.Session["UserMsg"] == null)
            {
                //跳转到登陆页
                filterContext.HttpContext.Response.Redirect("/Login");
            }
            else
            {
                string[] strMsg = Session["UserMsg"] as string[];
                //List<int> listMenuId;
                //List<int> listMenuIdC;

                #region 用户资料信息
                getUserMsg(strMsg);
                #endregion

                #region 角色权限


                //#region 主菜单
                //getUserMenuF(out listMenuId);
                //#endregion

                //#region 子菜单
                //getUserMenuC(listMenuId, out listMenuIdC);
                //#endregion

                //#region 功能
                //List<int> listFunId = getUserFun(listMenuIdC);
                //#endregion

                //#region 字段
                //getUserFiled(listMenuIdC, listFunId);
                //#endregion


                #endregion

                #region 用户淘宝店授权信息
                //getUserTaobao(strMsg);
                #endregion
            }
        }

        /// <summary>
        /// 用户淘宝店授权信息(用户获取淘宝的权限)
        /// </summary>
        /// <param name="strMsg"></param>
        public void getUserTaobao(string[] strMsg)
        {
            List<model.taoAppUser> taoAppUserMdList = new List<model.taoAppUser>(); //用户淘宝店授权信息
            string access_tokens = strMsg[3].ToString();
            string nicks = HttpUtility.UrlDecode(strMsg[2].ToString());
            if (!string.IsNullOrEmpty(access_tokens) && !string.IsNullOrEmpty(nicks))
            {
                string[] Access_tokens = access_tokens.Split(',');
                string[] Nicks = nicks.Split(',');

                for (int i = 0; i < Access_tokens.Length; i++)
                {
                    bll.taoAppUserbll taoAppUserBll = new bll.taoAppUserbll();

                    #region
                    IDataParameter[] iparaNick = new IDataParameter[] { 
                                    new SqlParameter("tbUserNick",SqlDbType.NVarChar,200),
                                };
                    iparaNick[0].Value = Nicks[i];
                    #endregion
                    //model.taoAppUser taoAppUserMd = taoAppUserBll.GetModelByNick(Nicks[i]);
                    model.taoAppUser taoAppUserMd = taoAppUserBll.GetModelByNick(iparaNick, "taoAppUsersNickSelect");
                    if (taoAppUserMd == null)
                    { taoAppUserMd = new model.taoAppUser(); }
                    taoAppUserMd.accessToken = Access_tokens[i];
                    taoAppUserMd.tbUserNick = Nicks[i];

                    taoAppUserMdList.Add(taoAppUserMd);
                }
            }
            us.TaoAppUserList = taoAppUserMdList;
        }

        public void getUserFiled(List<int> listMenuIdC, List<int> listFunId)
        {
            string s = string.Empty;
            foreach (int item in listMenuIdC)
            {
                s += item + ",";
            }
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;

            string s1 = string.Empty;
            foreach (int item in listFunId)
            {
                s1 += item + ",";
            }
            s1 = s1.Length > 0 ? s1.Remove(s1.Length - 1, 1) : s1;
            IDataParameter[] iparaFiled = new IDataParameter[] { 
                                    new SqlParameter("roleId",SqlDbType.Int,4),
                                    new SqlParameter("menuId",SqlDbType.NVarChar,5000),
                                    new SqlParameter("funId",SqlDbType.NVarChar,5000),
                                };
            iparaFiled[0].Value = us.User.personaId;
            iparaFiled[1].Value = s;
            iparaFiled[2].Value = s1;
            List<int> listFiledId = new List<int>();
            model.tableFiledPerssion filedMd = new model.tableFiledPerssion();
            bll.tableFiledPerssionbll filedBll = new bll.tableFiledPerssionbll();

            List<model.tableFiledPerssion> listFiled = filedBll.getTable(iparaFiled, "tableFiledSelect");
            foreach (var item in listFiled)
            {
                listFiledId.Add(item.Id);
            }
            if (listFiled.Count == 0)
            {
                listFiledId.Add(9999);
            }
            us.FiledId = listFiledId;
        }

        public List<int> getUserFun(List<int> listMenuIdC)
        {
            string s = string.Empty;
            foreach (int item in listMenuIdC)
            {
                s += item + ",";
            }
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            IDataParameter[] iparaFun = new IDataParameter[] { 
                                    new SqlParameter("roleId",SqlDbType.Int,4),
                                    new SqlParameter("menuId",SqlDbType.NVarChar,500)
                                };
            iparaFun[0].Value = us.User.personaId;
            iparaFun[1].Value = s;
            List<int> listFunId = new List<int>(); //功能ID
            model.funpermisson funMd = new model.funpermisson();
            bll.funPermissonbll funBll = new bll.funPermissonbll();


            List<model.funpermisson> listFun = funBll.getFun(iparaFun, "funSelect");
            foreach (var item in listFun)
            {
                listFunId.Add(item.Id);
            }
            if (listFun.Count == 0)
            {
                listFunId.Add(9999);
            }
            us.FunId = listFunId;
            return listFunId;
        }

        public void getUserMenuC(List<int> listMenuId, out List<int> listMenuIdC)
        {
            string s = string.Empty;
            foreach (int item in listMenuId)
            {
                s += "'"+item + "',";
            }
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            IDataParameter[] iparaMenuC = new IDataParameter[] { 
                                    new SqlParameter("roleId",SqlDbType.Int,4),
                                    new SqlParameter("menuId",SqlDbType.NVarChar,500)
                                };
            iparaMenuC[0].Value = us.User.personaId;
            iparaMenuC[1].Value = s;
            listMenuIdC = new List<int>(); //目录ID
            model.menu menuMdC = new model.menu();
            bll.menubll menuBllC = new bll.menubll();


            List<model.menu> listMenuC = menuBllC.getMenu(iparaMenuC, "menuSelectCC");//子菜单
            foreach (var item in listMenuC)
            {
                listMenuIdC.Add(item.Id);
            }
            if (listMenuC.Count == 0)
            {
                listMenuIdC.Add(9999);
            }
            us.MenuIdC = listMenuIdC;
        }

        public void getUserMenuF(out List<int> listMenuId)
        {
            IDataParameter[] iparaMenu = new IDataParameter[] { 
                                    new SqlParameter("roleId",SqlDbType.Int,4),
                                };
            iparaMenu[0].Value = us.User.personaId;
            listMenuId = new List<int>(); //目录ID
            model.menu menuMd = new model.menu();
            bll.menubll menuBll = new bll.menubll();


            List<model.menu> listMenu = menuBll.getMenu(iparaMenu, "menuSelectFF");//主菜单
            foreach (var item in listMenu)
            {
                listMenuId.Add(item.Id);
            }
            if (listMenu.Count==0)
            {
                listMenuId.Add(9999);
            }
            us.MenuIdF = listMenuId;
        }

        public void getUserMsg(string[] strMsg)
        {
            model.users userMd = new model.users(); //用户资料信息
            userMd.userName = strMsg[0].ToString(); //用户名称
            userMd.Id = Convert.ToInt32(strMsg[1].ToString()); //用户ID
            userMd.personaId = Convert.ToInt32(strMsg[4].ToString()); //角色ID

            us.User = userMd;
        }

    }
}
