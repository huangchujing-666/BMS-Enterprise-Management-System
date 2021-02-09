using Maticsoft.DBUtility;
using pbxdata.bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class UsersController : BaseController
    {
        //
        // GET: /Users/




        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 判断权限
        /// </summary>
        /// <returns></returns>
        public ActionResult getUsers()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                int id = helpcommon.ParmPerportys.GetNumParms(Request.Url.LocalPath.Replace("/Users/getUsers/", "").ToString());
                PublicHelpController ph = new PublicHelpController();

                #region
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
                    #region 查询
                    if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                    {
                        return View("../NoPermisson/Index");
                    }
                    #endregion
                }

                #endregion
                ViewData["id"] = id;
                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }


        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public string getData()
        {
            string roleJumpId = string.Empty; //外部跳转roleId
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (Request.Form["id"] != null && Request.Form["id"]!="0")
            {
                roleJumpId = Request.Form["id"].ToString();
                dic.Add("roleId", roleJumpId);
            }

            ///查询参数
            

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);
            List<model.users> list = new List<model.users>();
            StringBuilder s = new StringBuilder();
            bll.usersbll usersBll = new bll.usersbll();
            string[] ssName = usersBll.getDataName("users");
            //DataTable dt = new DataTable();
            //DataTable dt = usersBll.getData();
            DataTable dt = usersBll.getData(dic);

            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);

            #region TABLE添加
            s.Append("<tr>");
            s.Append("<th colspan='50' class='mytableadd'>");
            s.Append("<div style='padding-top: 20px;'>");
            if (ph.isFunPermisson(roleId, menuId, funName.addName))
            {
                s.Append("<a href='#'  onclick='javascript: showDiv()' >添加</a>");
            }
            s.Append("</div>");
            s.Append("</th>");
            s.Append("<tr>");
            #endregion

            #region TABLE表头
            s.Append("<tr>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<td>");
                    if (ssName[z] == "personaId")
                        s.Append("角色");
                    if (ssName[z] == "userName")
                        s.Append("用户名");
                    if (ssName[z] == "userPwd")
                        s.Append("密码");
                    if (ssName[z] == "userRealName")
                        s.Append("姓名");
                    if (ssName[z] == "userSex")
                        s.Append("性别");
                    if (ssName[z] == "UserPhone")
                        s.Append("电话");
                    if (ssName[z] == "UserAddress")
                        s.Append("地址");
                    if (ssName[z] == "UserEmail")
                        s.Append("邮箱");
                    if (ssName[z] == "userIndex")
                        s.Append("排序");
                    if (ssName[z] == "UserManage")
                        s.Append("管理");
                    if (ssName[z] == "UserId")
                        s.Append("操作人");
                    if (ssName[z] == "Def1")
                        s.Append("默认1");
                    if (ssName[z] == "Def2")
                        s.Append("默认2");
                    if (ssName[z] == "Def3")
                        s.Append("默认3");
                    if (ssName[z] == "Def4")
                        s.Append("默认4");
                    if (ssName[z] == "Def5")
                        s.Append("默认5");

                    s.Append("</td>");
                }
            }
            s.Append("<td>编辑</td><td>删除</td>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < ss.Length; j++)
                {
                    if (ss[j].ToLower() == "id")
                    {
                        s.Append("<td>");
                        s.Append("<label id='lblId'>" + dt.Rows[i][ss[j]].ToString() + "</label>");
                        s.Append("</td>");
                    }
                    else
                    {
                        s.Append("<td>");
                        s.Append(dt.Rows[i][ss[j]].ToString());
                        s.Append("</td>");
                    }
                }

                #region 编辑
                s.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    s.Append("<a href='#' onclick='javascript: showDivEdit();userEdit(" + dt.Rows[i][0].ToString() + ")'>编辑</a>");
                }
                else
                {
                    s.Append("<a href='#'>无编辑权限</a>");
                }
                s.Append("</td>");
                #endregion

                #region 删除
                s.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='del(" + dt.Rows[i][0].ToString() + ")'>删除</a>");
                }
                else
                {
                    s.Append("<a href='#'>无删除权限</a>");
                }

                s.Append("</td>");
                #endregion


                s.Append("</tr>");
            }
            #endregion

            usersBll = null;

            return s.ToString();
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        public string addUsers()
        {
            string s = string.Empty;
            string usersName = Request.Form["usersName"];
            string usersPwd = Request.Form["usersPwd"];
            string usersRealName = Request.Form["usersRealName"];
            string usersSex = Request.Form["usersSex"];
            string usersPhone = Request.Form["usersPhone"];
            string usersAddress = Request.Form["usersAddress"];
            string usersEmail = Request.Form["usersEmail"];
            string usersIndex = Request.Form["usersIndex"];
            string usersManage = Request.Form["usersManage"];
            string usersRole = Request.Form["usersRole"];
            int usersId = userInfo.User.Id;

            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("usersName",SqlDbType.NVarChar,20),
                new SqlParameter("usersPwd",SqlDbType.NVarChar,20),
                new SqlParameter("usersRealName",SqlDbType.NVarChar,20),
                new SqlParameter("usersSex",SqlDbType.NVarChar,20),
                new SqlParameter("usersPhone",SqlDbType.NVarChar,20),
                new SqlParameter("usersAddress",SqlDbType.NVarChar,20),
                new SqlParameter("usersEmail",SqlDbType.NVarChar,20),
                new SqlParameter("usersIndex",SqlDbType.NVarChar,20),
                new SqlParameter("usersManage",SqlDbType.NVarChar,20),
                new SqlParameter("usersRole",SqlDbType.NVarChar,20),
                new SqlParameter("usersId",SqlDbType.NVarChar,20)
            };
            ipara[0].Value = usersName;
            ipara[1].Value = usersPwd;
            ipara[2].Value = usersRealName;
            ipara[3].Value = usersSex;
            ipara[4].Value = usersPhone;
            ipara[5].Value = usersAddress;
            ipara[6].Value = usersEmail;
            ipara[7].Value = usersIndex;
            ipara[8].Value = usersManage;
            ipara[9].Value = usersRole;
            ipara[10].Value = usersId;
            bll.usersbll usersBll = new bll.usersbll();
            s = usersBll.Add(ipara, "addUsers");

            usersBll = null;
            return s;
        }



        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        public string editUsers()
        {
            

            StringBuilder s = new StringBuilder();
            List<model.users> list = new List<model.users>();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);

            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["id"]);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);


            bll.usersbll usersBll = new bll.usersbll();
            DataTable dt = usersBll.getDataEdit(id);
            string[] ssName = usersBll.getDataName("users");


            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.updateName);

            s.Append("<div style='float:left'>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        s.Append("<div style='width:250px;float:left;'>");

                        if (ssName[j] == "Id")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>编号:</span>");
                        if (ssName[j] == "personaId")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>角色:</span>");
                        if (ssName[j] == "userName")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>用户名:</span>");
                        if (ssName[j] == "userPwd")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>密码:</span>");
                        if (ssName[j] == "userRealName")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>姓名:</span>");
                        if (ssName[j] == "userSex")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>性别:</span>");
                        if (ssName[j] == "UserPhone")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>电话:</span>");
                        if (ssName[j] == "UserAddress")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>地址:</span>");
                        if (ssName[j] == "UserEmail")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>邮箱:</span>");
                        if (ssName[j] == "userIndex")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>排序:</span>");
                        if (ssName[j] == "UserManage")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>管理:</span>");
                        if (ssName[j] == "UserId")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>操作人:</span>");
                        if (ssName[j] == "Def1")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>默认1:</span>");
                        if (ssName[j] == "Def2")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>默认2:</span>");
                        if (ssName[j] == "Def3")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>默认3:</span>");
                        if (ssName[j] == "Def4")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>默认4:</span>");
                        if (ssName[j] == "Def5")
                            s.Append("<span style='float:left;text-align:right;width: 80px;'>默认5:</span>");

                        if (ssName[j] == "userSex")
                        {
                            #region 性别
                            s.Append("<span style='width:135px;display:-moz-inline-box; display:inline-block;'>");
                            if (dt.Rows[i][ss[j]].ToString() == "0")
                            {
                                s.Append("<input style='width:18px;' id='usersSex' name='radio1' type='radio' value='0' checked='checked' />男");
                                s.Append("<input style='width:18px;' id='usersSex1' name='radio1' type='radio' value='1' />女");
                            }
                            else
                            {
                                s.Append("<input style='width:18px;' id='usersSex' name='radio1' type='radio' value='0' />男");
                                s.Append("<input style='width:18px;' id='usersSex1' name='radio1' type='radio' value='1' checked='checked' />女");
                            }
                            s.Append("</span>");
                            #endregion
                        }
                        else if (ssName[j] == "personaId")
                        {
                            #region 角色
                            RoleHelperController RH = new RoleHelperController();
                            
                            s.Append("<select id='usersRoleEdit'  title='" + dt.Rows[i][ss[j]].ToString() + "'>"+RH.getRoleData()+"</select>");
                            #endregion
                        }
                        else if (ssName[j] == "userPwd")
                        {
                            #region 密码
                            s.Append("<input type='password' value='" + dt.Rows[i][ss[j]] + "' id='" + ssName[j] + "' />");
                            #endregion
                        }
                        else
                        {
                            s.Append("<input type='text' value='" + dt.Rows[i][ss[j]] + "' id='" + ssName[j] + "' />");
                        }


                        s.Append("</div>");
                    }
                }
            }
            shopbll sbl = new shopbll();
            DataTable dtShop = sbl.GetAllShop();
            string[] dtPerssionshop = usersBll.GetShopAllocation(id.ToString());
            s.Append("<div style='width:450px;margin:auto'>");
            for (int i = 0; i < dtShop.Rows.Count; i++)
            {
                if (dtPerssionshop.Contains(dtShop.Rows[i]["Id"].ToString()))
                {
                    s.Append("<label style='width: 150px;float:left;text-align:left'><input class='Check' style='width:20px;' type='checkbox' checked='checked' shopid='" + dtShop.Rows[i]["Id"] + "' />" + dtShop.Rows[i]["ShopName"] + "</label>");
                }
                else
                {
                    s.Append("<label style='width: 150px;float:left;text-align:left'><input class='Check' style='width:20px;' type='checkbox' shopid='" + dtShop.Rows[i]["Id"] + "' />" + dtShop.Rows[i]["ShopName"] + "</label>");
                }
            }
            s.Append("</div>");
            s.Append("</div>");
            s.Append("<div><button id='btnSave' value='保存' onclick='userSave()'>保存</button></div>");


            usersBll = null;
            return s.ToString();
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        public string updateUsers()
        {
            string s = string.Empty;
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]);

            #region 密码加密
            string pwd = helpcommon.PasswordHelp.encrypt(Request.Form["userPwd"]);
            pwd = helpcommon.PasswordHelp.encrypt(pwd);
            #endregion

            var Id = helpcommon.ParmPerportys.GetNumParms(Request.Form["Id"]);
            var personaId = helpcommon.ParmPerportys.GetNumParms(Request.Form["personaId"]);
            var userName = Request.Form["userName"];
            //var userPwd = Request.Form["userPwd"];//密码
            var userPwd = pwd;//密码
            var userRealName = Request.Form["userRealName"];
            var userSex = helpcommon.ParmPerportys.GetNumParms(Request.Form["userSex"]);
            var UserPhone = Request.Form["UserPhone"];
            var UserAddress = Request.Form["UserAddress"];
            var UserEmail = Request.Form["UserEmail"];
            var userIndex = helpcommon.ParmPerportys.GetNumParms(Request.Form["userIndex"]);
            var UserManage = helpcommon.ParmPerportys.GetNumParms(Request.Form["UserManage"]);
            //var UserId = helpcommon.ParmPerportys.GetNumParms(Request.Form["UserId"]);
            var Def1 = Request.Form["Def1"];
            var Def2 = Request.Form["Def2"];
            var Def3 = Request.Form["Def3"];
            var Def4 = Request.Form["Def4"];
            var Def5 = Request.Form["Def5"];
            int UserId = userInfo.User.Id;

            PublicHelpController ph = new PublicHelpController();
            bll.usersbll usersBll = new bll.usersbll();

            DataTable mytable = usersBll.getDataEdit(Id);
            string[] ssName = usersBll.getDataName("users");
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.updateName);

            IDataParameter[] ipara = new IDataParameter[]{
                    new SqlParameter("id",SqlDbType.Int,4),
                    new SqlParameter("personaId",SqlDbType.Int,4),
                    new SqlParameter("usersName",SqlDbType.NVarChar,20),
                    new SqlParameter("usersPwd",SqlDbType.NVarChar,100),
                    new SqlParameter("usersRealName",SqlDbType.NVarChar,20),
                    new SqlParameter("usersSex",SqlDbType.Int,4),
                    new SqlParameter("usersPhone",SqlDbType.NVarChar,20),
                    new SqlParameter("usersAddress",SqlDbType.NVarChar,200),
                    new SqlParameter("usersEmail",SqlDbType.NVarChar,20),
                    new SqlParameter("usersIndex",SqlDbType.Int,4),
                    new SqlParameter("usersManage",SqlDbType.Int,4),
                    new SqlParameter("userId",SqlDbType.Int,4),
                    new SqlParameter("Def1",SqlDbType.NVarChar,50),
                    new SqlParameter("Def2",SqlDbType.NVarChar,50),
                    new SqlParameter("Def3",SqlDbType.NVarChar,50),
                    new SqlParameter("Def4",SqlDbType.NVarChar,50),
                    new SqlParameter("Def5",SqlDbType.NVarChar,50)
            };
            for (int i = 0; i < ssName.Length; i++)
            {
                if (ss.Contains(ssName[i]))
                {
                    if (ssName[i] == "Id")
                        ipara[i].Value = Id;
                    if (ssName[i] == "personaId")
                        ipara[i].Value = personaId;
                    if (ssName[i] == "userName")
                        ipara[i].Value = userName;
                    if (ssName[i] == "userPwd")
                        ipara[i].Value = userPwd;
                    if (ssName[i] == "userRealName")
                        ipara[i].Value = userRealName;
                    if (ssName[i] == "userSex")
                        ipara[i].Value = userSex;
                    if (ssName[i] == "UserPhone")
                        ipara[i].Value = UserPhone;
                    if (ssName[i] == "UserAddress")
                        ipara[i].Value = UserAddress;
                    if (ssName[i] == "UserEmail")
                        ipara[i].Value = UserEmail;
                    if (ssName[i] == "userIndex")
                        ipara[i].Value = userIndex;
                    if (ssName[i] == "UserManage")
                        ipara[i].Value = UserManage;
                    if (ssName[i] == "UserId")
                        ipara[i].Value = UserId;
                    if (ssName[i] == "Def1")
                        ipara[i].Value = Def1;
                    if (ssName[i] == "Def2")
                        ipara[i].Value = Def2;
                    if (ssName[i] == "Def3")
                        ipara[i].Value = Def3;
                    if (ssName[i] == "Def4")
                        ipara[i].Value = Def4;
                    if (ssName[i] == "Def5")
                        ipara[i].Value = Def5;

                }
                else
                {
                    ipara[i].Value = mytable.Rows[0][mytable.Columns[i].ColumnName].ToString();
                }
            }


            s = usersBll.Update(ipara, "updateUsers");
            usersBll = null;

            return s;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public string delUsers()
        {
            string s = string.Empty;

            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["id"]);
            bll.usersbll usersBll = new bll.usersbll();
            s = usersBll.del(id);
            usersBll = null;

            return s;
        }

        /// <summary>
        /// 用户分配店铺
        /// </summary>
        /// <returns></returns>
        public string Allocation()
        {
            string ShopId = Request.Form["ShopId"] == null ? "" : Request.Form["ShopId"].ToString().Trim(',');
            string Id = Request.Form["UserId"] == null ? "" : Request.Form["UserId"].ToString();
            string[] strShopId = ShopId.Split(',');
            shopbll sb = new shopbll();
            if (sb.ClearShopAllocationByManagerId(Id))
            {
                if (strShopId.Length > 0)
                {
                    usersbll usb = new usersbll();
                    for (int i = 0; i < strShopId.Length; i++)
                    {
                        if (usb.ShopIsAllocation(strShopId[i].ToString(), Id))
                        {
                            usb.InsertShopAllocation(strShopId[i].ToString(), Id);
                        }
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// 用户修改自己信息
        /// </summary>
        public ActionResult ChangeUsers()
        {
            ViewData["UserId"] = userInfo.User.Id;
            return View();
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        public string GetUserInfo()
        {
            int UserId = userInfo.User.Id;
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            string sql = @"select userName,userPwd,userRealName,userSex,UserPhone,UserEmail,UserAddress from Users where Id=" + UserId;
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt1.Columns.Count; i++)
            {
                s += dt1.Rows[0][i].ToString() == null ? "" : dt1.Rows[0][i].ToString() + "*";
            }

            return s.Trim('*');
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        public string UpdateUserInfo()
        {
            #region 新密码加密
            string password = helpcommon.PasswordHelp.encrypt(Request.Form["Pwd"].ToString());
            password = helpcommon.PasswordHelp.encrypt(password);
            #endregion

            #region 判断老密码是否正确,先把老密码加密
            string oldpassword = helpcommon.PasswordHelp.encrypt(Request.Form["oldPwd"].ToString());
            oldpassword = helpcommon.PasswordHelp.encrypt(oldpassword);
            #endregion

            Dictionary<string, string> Dic = new Dictionary<string, string>();
            bll.usersbll usersBll = new bll.usersbll();
            Dic.Add("UserName", Request.Form["UserName"].ToString());
            //Dic.Add("oldPwd", Request.Form["oldPwd"].ToString());
            //Dic.Add("Pwd", Request.Form["Pwd"].ToString());
            Dic.Add("oldPwd", oldpassword);
            Dic.Add("Pwd", password);
            Dic.Add("RealName", Request.Form["RealName"].ToString());
            Dic.Add("sex", Request.Form["sex"].ToString());
            Dic.Add("Phone", Request.Form["Phone"].ToString());
            Dic.Add("Email", Request.Form["Email"].ToString());
            Dic.Add("Address", Request.Form["Address"].ToString());
            Dic.Add("UserId", userInfo.User.Id.ToString());
            return usersBll.UpdateUserInfo(Dic);
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        public ActionResult ClientUsers()
        {
            return View();
        }
        /// <summary>
        /// 获取客户端用户信息
        /// </summary>
        public string GetClientUsersTable()
        {
            string UserName = Request.Form["UserName"].ToString();
            string Vencode = Request.Form["Vencode"].ToString();
            string counts = string.Empty;
            int Page = Convert.ToInt32(Request.Form["Page"].ToString());
            int selpages = Convert.ToInt32(Request.Form["selpages"].ToString());
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            bll.usersbll usersBll = new bll.usersbll();
            dt1 = usersBll.GetClientUsersTable(UserName, Vencode, Page, selpages, out counts);
            #region 表头
            s += "<tr><th>编号</th><th>用户名</th><th>密码</th><th>数据源</th><th>邮箱</th><th>编辑</th><th>删除</th></tr>";
            #endregion

            #region 表内容
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                int n = Convert.ToInt32(i * 1 + 1);
                s += "<tr><td>" + n + "</td><td>" + dt1.Rows[i]["UserName"] + "</td><td>" + dt1.Rows[i]["PassWord"] + "</td><td>" + dt1.Rows[i]["sourceName"] + "</td><td>" + dt1.Rows[i]["Def1"] + "</td>";
                s += "<td><a href='#' onclick='EditClientUsers(\"" + dt1.Rows[i]["UserName"] + "\",\"" + dt1.Rows[i]["PassWord"] + "\",\"" + dt1.Rows[i]["Vencode"] + "\",\"" + dt1.Rows[i]["Def1"] + "\")'>编辑</a></td>";
                s += "<td><a href='#' onclick='DeleteClientUsers(\"" + dt1.Rows[i]["UserName"] + "\")'>删除</a></td>";
                s += "</tr>";
            }
            #endregion

            return s.ToString() + "-*-" + counts;
        }
        /// <summary>
        /// 修改客户端用户信息
        /// </summary>
        public string UpdateClientUsers()
        {
            string UserName = Request.Form["UserName"].ToString();
            string PassWord = Request.Form["PassWord"].ToString();
            string Vencode = Request.Form["Vencode"].ToString();
            string Email = Request.Form["Email"].ToString();
            bll.usersbll usersBll = new bll.usersbll();
            return usersBll.UpdateClientUsers(UserName, PassWord, Vencode, Email);
        }
        /// <summary>
        /// 添加客户端用户信息
        /// </summary>
        public string AddClientUsers()
        {
            string UserName = Request.Form["UserName"].ToString();
            string PassWord = Request.Form["PassWord"].ToString();
            string Vencode = Request.Form["Vencode"].ToString();
            string Email = Request.Form["Email"].ToString();
            bll.usersbll usersBll = new bll.usersbll();
            return usersBll.AddClientUsers(UserName, PassWord, Vencode, Email);
        }
        /// <summary>
        /// 删除客户端用户
        /// </summary>
        public string DeleteClientUsers()
        {
            string UserName = Request.Form["UserName"].ToString();
            bll.usersbll usersBll = new bll.usersbll();
            return usersBll.DeleteClientUsers(UserName);
        }
        
    }
}
