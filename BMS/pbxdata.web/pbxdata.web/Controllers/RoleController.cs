using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class RoleController : BaseController
    {
        //
        // GET: /Role/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public ActionResult getRole()
        {
            #region
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            PublicHelpController ph = new PublicHelpController();
            ViewData["menuId"] = menuId;

            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }
            }
            else
            {
                #region 添加(1有权限，0无权限)
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.addName))
                {
                    ViewData["addName"] = 0;//无添加权限
                }
                else
                {
                    ViewData["addName"] = 1;//有添加权限
                }
                #endregion

                #region 编辑(1有权限，0无权限)
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.updateName))
                {
                    ViewData["updateName"] = 0;//无编辑权限
                }
                else
                {
                    ViewData["updateName"] = 1;//有编辑权限
                }
                #endregion

                #region 菜单分配权限(1有权限，0无权限)
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.menuList))
                {
                    ViewData["menuList"] = 0;//无菜单分配权限
                }
                else
                {
                    ViewData["menuList"] = 1;//有菜单分配权限
                }
                #endregion

                #region 用户管理(1有权限，0无权限)
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.userList))
                {
                    ViewData["userList"] = 0;//无用户管理权限
                }
                else
                {
                    ViewData["userList"] = 1;//有用户管理权限
                }
                #endregion

                #region 查询
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.selectName))
                {
                    List<model.persona> myList = new List<model.persona>();
                    return View("../NoPermisson/Index");
                }
                #endregion
            }
            #endregion


            bll.rolebll roleBll = new bll.rolebll();
            IDataParameter[] ipara = new IDataParameter[]{
                //new SqlParameter("menuName",SqlDbType.NVarChar,20)
            };
            //ipara[0].Value = "张三";
            List<model.persona> list = roleBll.getRole(ipara, "roleSelect");

            return View(list);
        }


        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <returns></returns>
        public ActionResult editRole(int id)
        {
            bll.rolebll roleBll = new bll.rolebll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("id",SqlDbType.Int,4)
            };
            ipara[0].Value = id;
            List<model.persona> list = roleBll.getRole(ipara, "roleEdit");
            model.persona roleMd = new model.persona();

            roleMd = list.Where(c => c.Id == id).ToList()[0];

            return View(roleMd);
        }


        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult allotMenuPermisson(int id)
        {
            ViewData["roleId"] = id;
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["menuId"] = menuId;
            PublicHelpController ph = new PublicHelpController();

            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }
            }
            else
            {

                #region 功能列表
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.funList))
                {
                    ViewData["funList"] = 0;//无功能列表权限

                }
                else
                {
                    ViewData["funList"] = 1;//有功能列表权限
                }
                #endregion

                #region 分配权限
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.menuFPQX))
                {
                    ViewData["menuFPQX"] = 0;//无分配权限

                }
                else
                {
                    ViewData["menuFPQX"] = 1;//有分配权限
                }
                #endregion

                #region 查询
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.menuCKQX))
                {
                    List<model.persona> myList = new List<model.persona>();
                    return View("../NoPermisson/Index");
                }
                #endregion
            }
            //string  menuIdC = getMenuId(helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId));
            string menuIdC = getMenuId(helpcommon.ParmPerportys.GetNumParms(id));
            ViewData["menuIdC"] = menuIdC;


            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                //new SqlParameter("menuName",SqlDbType.NVarChar,20)
            };
            //ipara[0].Value = "张三";
            List<model.menu> list = menuBll.getMenu(ipara, "menuSelectAllC");

            return View(list);
        }


        /// <summary>
        /// 获取功能列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult allotFunPermisson(int id, int roleId)
        {
            ViewData["roleId"] = roleId;
            ViewData["menuId"] = id;


            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }
            }
            else
            {
                #region 表格列表
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), id, funName.tableList))
                {
                    ViewData["tableList"] = 0;//无表格列表权限

                }
                else
                {
                    ViewData["tableList"] = 1;//有表格列表权限
                }
                #endregion

                #region 分配权限
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), id, funName.funFPQX))
                {
                    ViewData["funFPQX"] = 0;//无分配权限

                }
                else
                {
                    ViewData["funFPQX"] = 1;//有分配权限
                }
                #endregion

                #region 查询
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), id, funName.funCKQX))
                {
                    List<model.persona> myList = new List<model.persona>();
                    return View("../NoPermisson/Index");
                }
                #endregion
            }
            string funId = getFunId(roleId, id);
            ViewData["funIds"] = funId;


            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("MenuId",SqlDbType.NVarChar,20)
            };
            ipara[0].Value = id;
            List<model.funpermisson> list = funPermissonBll.getFun(ipara, "funPermissonSelect");

            return View(list);
        }


        /// <summary>
        /// 获取表格列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult allotTablePermisson(int id, int roleId, int menuId)
        {
            ViewData["roleId"] = roleId;
            ViewData["menuId"] = menuId;
            ViewData["funId"] = id;

            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            IDataParameter[] ipara = new IDataParameter[]{
                //new SqlParameter("menuName",SqlDbType.NVarChar,20)
            };
            //ipara[0].Value = "张三";
            List<model.tableFiledPerssion> list = tableFiledPerssionBll.getTable(ipara, "tableSelect");

            return View(list);
        }


        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult allotFiledPermisson(int id, int roleId, int menuId, int funId)
        {
            ViewData["id"] = id;
            ViewData["roleId"] = roleId;
            ViewData["menuId"] = menuId;
            ViewData["funId"] = funId;

            PublicHelpController ph = new PublicHelpController();
            if (userInfo.User.userName == "sa")
            {
                funName f = new funName();
                System.Reflection.MemberInfo[] properties = f.GetType().GetMembers();
                foreach (System.Reflection.MemberInfo item in properties)
                {
                    string value = item.Name;
                    ViewData[value] = 1;
                }
            }
            else
            {
                #region 分配权限
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.fieldFPQX))
                {
                    ViewData["fieldFPQX"] = 0;//无分配权限

                }
                else
                {
                    ViewData["fieldFPQX"] = 1;//有分配权限
                }
                #endregion

                #region 查询
                if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.fieldCKQX))
                {
                    List<model.persona> myList = new List<model.persona>();
                    return View("../NoPermisson/Index");
                }
                #endregion
            }
            string filed = getFiledId(roleId, menuId, funId);
            ViewData["filedIds"] = filed;


            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("tableLevel",SqlDbType.NVarChar,20)
            };
            ipara[0].Value = id;

            List<model.tableFiledPerssion> list = tableFiledPerssionBll.getTable(ipara, "filedSelect");

            return View(list);
        }



        /// <summary>
        /// 更新角色
        /// </summary>
        /// <returns></returns>
        public string updateRole()
        {
            string s = string.Empty;
            string PersonaName = Request.Form["PersonaName"].ToString();
            string PersonaIndex = Request.Form["PersonaIndex"].ToString();
            int UserId = 1;
            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["id"].ToString());
            bll.rolebll roleBll = new bll.rolebll();
            IDataParameter[] ipara = new IDataParameter[]{
                
                new SqlParameter("PersonaName",SqlDbType.NVarChar,20),
                new SqlParameter("PersonaIndex",SqlDbType.NVarChar,500),
                new SqlParameter("UserId",SqlDbType.Int,4),
                new SqlParameter("Id",SqlDbType.Int,4)
            };
            ipara[0].Value = PersonaName;
            ipara[1].Value = PersonaIndex;
            ipara[2].Value = UserId;
            ipara[3].Value = id;

            s = roleBll.Update(ipara, "roleUpdate");

            return s;
        }


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public string addRole()
        {
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            PublicHelpController ph = new PublicHelpController();
            if (!ph.isFunPermisson(int.Parse(userInfo.User.personaId.ToString()), menuId, funName.addName))
            {
                List<model.persona> myList = new List<model.persona>();
                return "您未被授权使用该功能，请重新登录或联系管理员进行处理.";
            }

            string s = string.Empty;
            string PersonaName = Request.Form["roleName"].ToString();
            string PersonaIndex = Request.Form["roleIndex"].ToString();
            int UserId = 1;
            bll.rolebll roleBll = new bll.rolebll();
            IDataParameter[] ipara = new IDataParameter[]{
                
                new SqlParameter("PersonaName",SqlDbType.NVarChar,20),
                new SqlParameter("PersonaIndex",SqlDbType.NVarChar,500),
                new SqlParameter("UserId",SqlDbType.Int,4),
            };
            ipara[0].Value = PersonaName;
            ipara[1].Value = PersonaIndex;
            ipara[2].Value = UserId;

            s = roleBll.Add(ipara, "roleAdd");

            return s;
        }


        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <returns></returns>
        public string getRoleName()
        {
            bll.rolebll roleBll = new bll.rolebll();
            model.persona mdPersona = roleBll.getRoleName(1);
            return mdPersona.PersonaName;
        }


        /// <summary>
        /// 分配菜单权限
        /// </summary>
        /// <returns></returns>
        public string allotMenu()
        {
            string roleId = Request.Form["roleId"];
            string menuId = Request.Form["menuId"];
            if (menuId == null)
            {
                return "请选择需分配菜单";
            }
            string[] menuIds = helpcommon.StrSplit.StrSplitData(menuId, ',');
            string s = string.Empty;
            List<model.personapermisson> list = new List<model.personapermisson>();

            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            for (int i = 0; i < menuIds.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(menuIds[i]))
                {
                    model.personapermisson mdPersonapermisson = new model.personapermisson();
                    mdPersonapermisson.personaId = helpcommon.ParmPerportys.GetNumParms(roleId);
                    mdPersonapermisson.MemuId = helpcommon.ParmPerportys.GetNumParms(menuIds[i]);

                    if (!personapermissonBll.selectMenuIsExists(helpcommon.ParmPerportys.GetNumParms(roleId), helpcommon.ParmPerportys.GetNumParms(menuIds[i])))
                    {
                        list.Add(mdPersonapermisson);
                    }
                }

            }
            s = personapermissonBll.AddRolePersonapermisson(list);
            personapermissonBll = null;

            return s;
        }


        /// <summary>
        /// 撤销菜单权限
        /// </summary>
        /// <returns></returns>
        public string cancelMenu()
        {
            string roleId = Request.Form["roleId"];
            string menuId = Request.Form["menuId"];
            if (menuId == null)
            {
                return "请选择需撤销菜单";
            }
            string[] menuIds = helpcommon.StrSplit.StrSplitData(menuId, ',');
            string s = string.Empty;
            List<model.personapermisson> list = new List<model.personapermisson>();

            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            for (int i = 0; i < menuIds.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(menuIds[i]))
                {
                    var p = personapermissonBll.selectMenuIsExists1(helpcommon.ParmPerportys.GetNumParms(roleId), helpcommon.ParmPerportys.GetNumParms(menuIds[i]));
                    if (p != null)
                    {
                        list.Add(p);
                    }
                }

            }
            s = personapermissonBll.DelRolePersonapermisson(list);
            personapermissonBll = null;

            return s;
        }



        /// <summary>
        /// 分配功能权限
        /// </summary>
        /// <returns></returns>
        public string allotFun()
        {
            string roleId = Request.Form["roleId"];
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string funId = Request.Form["funId"];
            if (funId == null)
            {
                return "请选择需分配功能";
            }
            string[] funIds = helpcommon.StrSplit.StrSplitData(funId, ',');
            string s = string.Empty;
            List<model.personapermisson> list = new List<model.personapermisson>();

            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            for (int i = 0; i < funIds.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(funIds[i]))
                {
                    model.personapermisson mdPersonapermisson = new model.personapermisson();
                    mdPersonapermisson.personaId = helpcommon.ParmPerportys.GetNumParms(roleId);
                    mdPersonapermisson.MemuId = menuId;
                    mdPersonapermisson.FunId = helpcommon.ParmPerportys.GetNumParms(funIds[i]);

                    if (!personapermissonBll.selectFunIsExists(helpcommon.ParmPerportys.GetNumParms(roleId), menuId, helpcommon.ParmPerportys.GetNumParms(funIds[i])))
                    {
                        list.Add(mdPersonapermisson);
                    }
                }

            }
            s = personapermissonBll.AddRolePersonapermisson(list);

            personapermissonBll = null;
            return s;
        }


        /// <summary>
        /// 撤销功能权限
        /// </summary>
        /// <returns></returns>
        public string cancelFun()
        {
            string roleId = Request.Form["roleId"];
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            string funId = Request.Form["funId"];
            if (funId == null)
            {
                return "请选择需撤销功能";
            }
            string[] funIds = helpcommon.StrSplit.StrSplitData(funId, ',');
            string s = string.Empty;
            List<model.personapermisson> list = new List<model.personapermisson>();

            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            for (int i = 0; i < funIds.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(funIds[i]))
                {
                    var p = personapermissonBll.selectFunIsExists1(helpcommon.ParmPerportys.GetNumParms(roleId), menuId, helpcommon.ParmPerportys.GetNumParms(funIds[i]));
                    if (p != null)
                    {
                        list.Add(p);
                    }
                }

            }
            s = personapermissonBll.DelRolePersonapermisson(list);

            personapermissonBll = null;
            return s;
        }



        /// <summary>
        /// 分配字段权限
        /// </summary>
        /// <returns></returns>
        public string allotFiled()
        {
            string roleId = Request.Form["roleId"];
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            int funId = helpcommon.ParmPerportys.GetNumParms(Request.Form["funId"]);
            string filedId = Request.Form["filedId"];
            if (filedId == null)
            {
                return "请选择需分配字段";
            }
            string[] filedIds = helpcommon.StrSplit.StrSplitData(filedId, ',');
            string s = string.Empty;
            List<model.personapermisson> list = new List<model.personapermisson>();

            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            for (int i = 0; i < filedIds.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(filedIds[i]))
                {
                    //string ss = userInfo.RoleId;
                    model.personapermisson mdPersonapermisson = new model.personapermisson();
                    mdPersonapermisson.personaId = helpcommon.ParmPerportys.GetNumParms(roleId);
                    mdPersonapermisson.MemuId = menuId;
                    mdPersonapermisson.FunId = funId;
                    mdPersonapermisson.FieldId = helpcommon.ParmPerportys.GetNumParms(filedIds[i]);

                    if (!personapermissonBll.selectFiledIsExists(helpcommon.ParmPerportys.GetNumParms(roleId), menuId, funId, helpcommon.ParmPerportys.GetNumParms(filedIds[i])))
                    {
                        list.Add(mdPersonapermisson);
                    }


                }

            }
            s = personapermissonBll.AddRolePersonapermisson(list);
            personapermissonBll = null;

            return s;
        }


        /// <summary>
        /// 撤销字段权限
        /// </summary>
        /// <returns></returns>
        public string cancelFiled()
        {
            string roleId = Request.Form["roleId"];
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            int funId = helpcommon.ParmPerportys.GetNumParms(Request.Form["funId"]);
            string filedId = Request.Form["filedId"];
            if (filedId == null)
            {
                return "请选择需分配字段";
            }
            string[] filedIds = helpcommon.StrSplit.StrSplitData(filedId, ',');
            string s = string.Empty;
            List<model.personapermisson> list = new List<model.personapermisson>();

            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            for (int i = 0; i < filedIds.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(filedIds[i]))
                {
                    var p = personapermissonBll.selectFiledIsExists1(helpcommon.ParmPerportys.GetNumParms(roleId), menuId, funId, helpcommon.ParmPerportys.GetNumParms(filedIds[i]));
                    if (p != null)
                    {
                        list.Add(p);
                    }
                }

            }
            s = personapermissonBll.DelRolePersonapermisson(list);
            personapermissonBll = null;

            return s;
        }






        #region 返回权限集合
        /// <summary>
        /// 返回有访问权限的子菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string getMenuId(int roleId)
        {
            string s = string.Empty;
            PublicHelpController ph = new PublicHelpController();
            List<int> listMenuId = ph.getUserMenuF1(roleId);
            List<int> listMenuIdC = ph.getUserMenuC1(listMenuId, roleId);

            foreach (int item in listMenuIdC)
            {
                s += item + ",";
            }
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            return s;
        }


        /// <summary>
        /// 返回有访问权限的菜单中的功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public string getFunId(int roleId, int menuId)
        {
            string s = string.Empty;
            List<int> menuIds = new List<int>();
            menuIds.Add(menuId);
            PublicHelpController ph = new PublicHelpController();
            List<int> listFunId = ph.getUserFun1(menuIds, roleId);
            foreach (int item in listFunId)
            {
                s += item + ",";
            }
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            return s;
        }


        /// <summary>
        /// 返回有访问权限的菜单中的功能所拥有的字段
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public string getFiledId(int roleId, int menuId, int funId)
        {
            string s = string.Empty;
            PublicHelpController ph = new PublicHelpController();
            List<int> listFiledId = ph.getUserFiled(roleId, menuId, funId);
            foreach (int item in listFiledId)
            {
                s += item + ",";
            }
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            return s;
        }

        #endregion



        /// <summary>
        /// 添加表格列表
        /// </summary>
        public string AddTable()
        {

            string TabName = Request.QueryString["TabName"].ToString();
            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();

            //return Content("123");
            return tableFiledPerssionBll.AddTabName(TabName, userInfo.User.Id);
        }

        /// <summary>
        /// 添加列
        /// </summary>
        public string AddFiled()
        {

            string tableName = Request.QueryString["tableName"].ToString();
            string tableLevel = Request.QueryString["tableLevel"].ToString();
            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();

            //return Content("123");
            return tableFiledPerssionBll.AddFiledName(tableName, tableLevel, userInfo.User.Id);
        }

        /// <summary>
        /// 添加字段描述,添加表描述tableFiledPerssion表
        /// </summary>
        public string AddDescript()
        {
            string Id = Request.QueryString["Id"].ToString();
            string Descript = Request.QueryString["Descript"].ToString();
            bll.tableFiledPerssionbll bll = new bll.tableFiledPerssionbll();
            return bll.AddDescript(Id, Descript);
        }
        /// <summary>
        /// 修改表名,列名
        /// </summary>
        public string UpdateTabName()
        {
            string Id = Request.QueryString["Id"].ToString();
            string Descript = Request.QueryString["Descript"].ToString();
            bll.tableFiledPerssionbll bll = new bll.tableFiledPerssionbll();
            return bll.UpdateTabName(Id, Descript); ;
        }
        /// <summary>
        /// 删除表,列
        /// </summary>
        public string DeletetableFiled()
        {
            string Id = Request.QueryString["Id"].ToString();
            bll.tableFiledPerssionbll bll = new bll.tableFiledPerssionbll();
            return bll.DeletetableFiled(Id, userInfo.User.Id); ;
        }
        /// <summary>
        /// 复制角色权限
        /// </summary>
        public string CopyUsers()
        {
            string Id = Request.QueryString["Id"].ToString();
            string PersonaId = Request.QueryString["PersonaId"].ToString();
            bll.rolebll bll = new bll.rolebll();
            return bll.CopyUsers(Id, PersonaId);
        }
        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        public string GetUsersList()
        {
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            bll.rolebll bll = new bll.rolebll();
            dt1 = bll.getRole();
            s += "<select><option selected='selected' value=''>请选择</option>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s += "<option value='" + dt1.Rows[i]["Id"] + "'>" + dt1.Rows[i]["PersonaName"] + "</option>";
            }
            s += "</select>";
            return s;
        }
    }
}
