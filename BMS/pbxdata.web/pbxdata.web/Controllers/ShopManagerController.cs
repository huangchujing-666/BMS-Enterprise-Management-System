using Maticsoft.DBUtility;
using pbxdata.bll;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Top.Api.Domain;
using pbxdata.helpcommon;
using pbxdata.tmall;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace pbxdata.web.Controllers
{
    public class ShopManagerController : BaseController
    {
        public static int Savemenuid = 0;
        public static int PageSum = 0;//保存当前页数
        ShopTypeBLL stb = new ShopTypeBLL();
        ProductHelper phb = new ProductHelper();
        shopbll sbl = new shopbll();


        #region 店铺类型
        /// <summary>
        /// 页面加载显示
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowShopType()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
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
                Savemenuid = menuId;
            }
            #region 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["menuId"] = Savemenuid;
            #endregion
            ViewData["PageSum"] = phb.GetPageDDlist();

            return View();
        }
        /// <summary>
        /// 店铺类型翻页
        /// </summary>
        /// <returns></returns>
        public string ShopTypePageIndex()
        {
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();

            #region 查询条件
            string shopTypeName = Request.Form["params"] == null ? "" : Request.Form["params"].ToString();

            string[] orderParamss = helpcommon.StrSplit.StrSplitData(shopTypeName, ','); //参数集合

            string TypeName = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号

            TypeName = TypeName == "\'\'" ? string.Empty : TypeName;

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("shoptype");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段
            #endregion

            int count = 0;
            stb.SelectAllShopType(0, pageSize, TypeName, out count);
            StringBuilder AllTable = new StringBuilder();
            int pageCount = 0;
            pageCount = count % pageSize > 0 ? count / pageSize + 1 : count / pageSize;

            int MinId = (pageIndex - 1) * pageSize;
            int MaxId = (pageIndex - 1) * pageSize + pageSize;
            DataTable dt = stb.SelectAllShopType(MinId, MaxId, TypeName, out count);

            AllTable.Append("<table id='mytable' class='mytable' rules='all'><tr>");
            AllTable.Append("<tr>");

            #region   判断表头
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case "Id":
                        AllTable.Append("<th>店铺类型编号</th>");
                        break;
                    case "ShoptypeName":
                        AllTable.Append("<th>店铺类型名称</th>");
                        break;
                    case "ShoptypeIndex":
                        AllTable.Append("<th>店铺排序</th>");
                        break;
                    case "UserId":
                        AllTable.Append("<th>操作人</th>");
                        break;
                    case "Def1":
                        AllTable.Append("<th>默认1</th>");
                        break;
                    case "Def2":
                        AllTable.Append("<th>默认2</th>");
                        break;
                    case "Def3":
                        AllTable.Append("<th>默认3</th>");
                        break;
                    case "Def4":
                        AllTable.Append("<th>默认4</th>");
                        break;
                    case "Def5":
                        AllTable.Append("<th>默认5</th>");
                        break;
                }
            }
            AllTable.Append("<th>操作</th>");
            AllTable.Append("</tr>");
            #endregion

            #region  判断内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AllTable.Append("<tr>");
                for (int j = 0; j < s.Length; j++)
                {

                    AllTable.Append("<td>");
                    AllTable.Append(dt.Rows[i][s[j]]);
                    AllTable.Append("</td>");

                }
                AllTable.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    AllTable.Append("<a href='#' onclick='UpdateShopType(" + dt.Rows[i]["Id"] + ")'>编辑</a>||||");
                }
                else
                {
                    AllTable.Append("<a href='#'>无编辑权限</a>||||");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    AllTable.Append("<a href='#' onclick='DeleteType(" + dt.Rows[i]["Id"] + ")' >删除</a>");
                }
                else
                {
                    AllTable.Append("<a href='#'>无删除权限</a>");
                }
                AllTable.Append("</td>");
                AllTable.Append("</tr>");
            }
            AllTable.Append("</table>");
            #endregion

            #region 分页
            AllTable.Append("-----");
            AllTable.Append(pageCount + "-----" + count);
            #endregion
            return AllTable.ToString();
        }
        /// <summary>
        /// 添加店铺类型名称
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertTypeName()
        {
            string typename = Request.Form["typename"].ToString();
            if (stb.SelectIsIn(typename))
            {
                return Json(stb.InsertTypeName(typename));
            }
            else
            {
                return Json("店铺名称已经存在！");
            }
        }
        /// <summary>
        /// 删除店铺类型
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteType()
        {
            string shopId = Request.Form["shopname"].ToString();
            if (stb.ProductIsIn(shopId))
            {
                return Json(stb.DeleteShopName(shopId));
            }
            else
            {
                return Json("该类型下存在店铺,不能删除！");
            }
        }
        /// <summary>
        /// 编辑字段权限页面
        /// </summary>
        /// <returns></returns>
        public string UpdatePage()
        {
            string shopId = Request.Form["Id"].ToString();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = int.Parse(Request.Form["MenuId"].ToString());
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();
            DataTable dt = stb.SelectAllById(shopId);
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.updateName);//判断编辑的权限字段
            string[] allTableName = usb.getDataName("shoptype");
            StringBuilder AllTable = new StringBuilder();
            AllTable.Append("<span style='text-align:center'>修改店铺信息</span><br />");
            AllTable.Append("<br /><table  style='margin:auto;font-size:12px;'>");
            for (int i = 0; i < s.Length; i++)
            {
                if (allTableName.Contains(s[i]))
                {
                    AllTable.Append("<tr id='UpdateCondition' value='" + dt.Rows[0]["Id"] + "'>");
                    if (s[i] == "Id")
                    {
                        AllTable.Append("<td>店铺类型编号:</td>");
                        AllTable.Append("<td><input type='text' id='Id' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "ShoptypeName")
                    {
                        AllTable.Append("<td>店铺类型名称</td>");
                        AllTable.Append("<td><input type='text' id='ShoptypeName' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "ShoptypeIndex")
                    {
                        AllTable.Append("<td>店铺排序</td>");
                        AllTable.Append("<td><input type='text' id='ShoptypeIndex' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "UserId")
                    {
                        AllTable.Append("<td>操作人</td>");
                        AllTable.Append("<td><input type='text' id='UserId' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def1")
                    {
                        AllTable.Append("<td>默认1</td>");
                        AllTable.Append("<td><input type='text' id='Def1' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def2")
                    {
                        AllTable.Append("<td>默认2</td>");
                        AllTable.Append("<td><input type='text' id='Def2' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def3")
                    {
                        AllTable.Append("<td>默认3</td>");
                        AllTable.Append("<td><input type='text' id='Def3' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def4")
                    {
                        AllTable.Append("<td>默认4</td>");
                        AllTable.Append("<td><input type='text' id='Def4' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def5")
                    {
                        AllTable.Append("<td>默认5</td>");
                        AllTable.Append("<td><input type='text' id='Def5' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    AllTable.Append("</tr>");
                }
            }
            AllTable.Append("</table><br /><br/><input type='button' onclick='UpdateShopName()'  value='确认编辑'/><input style='margin-left:20px;' type='button' onclick='Close()' value='退出编辑' />");
            return AllTable.ToString();
        }
        /// <summary>
        /// 修改店铺类型名称
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateByShopName()
        {
            string id = Request.Form["Id"].ToString();
            string shopname = Request.Form["shopname"].ToString();
            if (stb.SelectIsIn(shopname))
            {
                return Json(stb.UpdateShopNameById(id, shopname));
            }
            else
            {
                return Json("店铺类型名称已存在!");
            }
        }
        #endregion



        #region 店铺列表

        /// <summary>
        /// 加载店铺页面
        /// </summary>
        /// <returns></returns>
        public ActionResult StoreManager()
        {

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
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
            #region 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            #endregion
            ViewData["ShopType"] = phb.GetShopTypeDDlist();
            ViewData["User"] = phb.UserDDlist();
            ViewData["PageSum"] = phb.GetPageDDlist();
            return View();
        }
        /// <summary>
        /// 翻页
        /// </summary>
        /// <returns></returns>
        public string ShopPageIndex()
        {
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();
            #region 查询条件
            string Params = Request.Form["params"] == null ? "" : Request.Form["params"].ToString();

            string[] ShopParamss = helpcommon.StrSplit.StrSplitData(Params, ','); //参数集合

            string ShopName = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string ShopType = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string ManagerName = helpcommon.StrSplit.StrSplitData(ShopParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号

            ShopName = ShopName == "\'\'" ? string.Empty : ShopName;
            ShopType = ShopType == "\'\'" ? string.Empty : ShopType;
            ManagerName = ManagerName == "\'\'" ? string.Empty : ManagerName;

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("shop");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段
            #endregion
            int count = 0;
            sbl.SelectAllShop(0, pageSize, ShopName, ShopType, ManagerName, out count);

            int MinId = pageSize * (pageIndex - 1);
            int MaxId = pageSize * (pageIndex - 1) + pageSize;
            DataTable dt = sbl.SelectAllShop(MinId, MaxId, ShopName, ShopType, ManagerName, out count);
            int PageCount = count % pageSize > 0 ? count / pageSize + 1 : count / pageSize;
            StringBuilder AllTable = new StringBuilder();
            //if (ph.isFunPermisson(roleId, menuId, funName.addName))
            //{
            //    AllTable.Append("<a href='#' onclick='AddShop()'>添加店铺</a>-----");
            //}
            AllTable.Append("<table id='StcokTable' class='mytable' rules='all' style='font-size:12px;'>");
            AllTable.Append("<tr>");
            #region   判断表头
            for (int i = 0; i < allTableName.Length; i++)
            {
                if (s.Contains(allTableName[i]))
                {
                    AllTable.Append("<th>");
                    if (s[i] == "Id")
                        AllTable.Append("店铺编号");
                    if (s[i] == "ShopName")
                        AllTable.Append("店铺名称");
                    if (s[i] == "ShoptypeId")
                        AllTable.Append("店铺类型");
                    if (s[i] == "ShopState")
                        AllTable.Append("店铺状态");
                    if (s[i] == "ShopManageId")
                        AllTable.Append("店铺管理人");
                    if (s[i] == "ShopIdex")
                        AllTable.Append("排序");
                    if (s[i] == "UserId")
                        AllTable.Append("操作人");
                    if (s[i] == "Def1")
                        AllTable.Append("默认1");
                    if (s[i] == "Def2")
                        AllTable.Append("默认2");
                    if (s[i] == "Def3")
                        AllTable.Append("默认3");
                    if (s[i] == "Def4")
                        AllTable.Append("默认4");
                    if (s[i] == "Def5")
                        AllTable.Append("默认5");
                    AllTable.Append("</th>");
                }
            }
            AllTable.Append("<th>操作</th>");
            #endregion
            #region  判断内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AllTable.Append("<tr>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {
                        AllTable.Append("<td>");
                        AllTable.Append(dt.Rows[i][allTableName[j]]);
                        AllTable.Append("</td>");
                    }
                }
                AllTable.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    AllTable.Append("<a href='#' onclick='UpdateShop(" + dt.Rows[i]["Id"] + ")'>编辑</a>||||");
                }
                else
                {
                    AllTable.Append("<a href='#'>无编辑权限</a>||||");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    AllTable.Append("<a href='#' onclick='DeleteShop(" + dt.Rows[i]["Id"] + ")' >删除</a>||||");
                }
                else
                {
                    AllTable.Append("<a href='#'>无删除权限||||</a>");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.Enabled))
                {
                    AllTable.Append("<a href='#' onclick='Enabled(" + dt.Rows[i]["Id"] + ")' >启用</a>||||");
                }
                else
                {
                    AllTable.Append("<a href='#'>无启用权限</a>||||");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.disable))
                {
                    AllTable.Append("<a href='#' onclick='disable(" + dt.Rows[i]["Id"] + ")' >停用</a>");
                }
                else
                {
                    AllTable.Append("<a href='#'>无停用权限</a>");
                }
                AllTable.Append("</td>");
                AllTable.Append("</tr>");
            }
            AllTable.Append("</table>");
            #endregion

            #region 分页
            AllTable.Append("-----");
            AllTable.Append(PageCount + "-----" + count);
            #endregion
            return AllTable.ToString();
        }
        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteShop()
        {
            string Id = Request.Form["Id"].ToString();
            if (sbl.SelectShopIdIsIn(Id))
            {
                return Json(sbl.DeleteShop(Id));
            }
            else
            {
                return Json("店铺中还存在商品！不能删除！");
            }
        }
        public static string SaveShopName = "";
        /// <summary>
        /// 修改店铺
        /// </summary>
        /// <returns></returns>
        public string UpdateShop()
        {
            string shopId = Request.Form["Id"].ToString();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = int.Parse(Request.Form["MenuId"].ToString());
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();
            DataTable dt = stb.SelectShopById(shopId);
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.updateName);//判断编辑的权限字段
            string[] allTableName = usb.getDataName("shop");
            List<shoptype> list = sbl.BindShopType();//绑定店铺类型
            string shoptype = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id.ToString() == dt.Rows[0]["ShoptypeId"].ToString())
                {
                    shoptype += "<option value='" + list[i].Id + "' selected='selected' >" + list[i].ShoptypeName + "</option>";
                }
                else
                {
                    shoptype += "<option value='" + list[i].Id + "'>" + list[i].ShoptypeName + "</option>";
                }

            }
            List<users> list1 = sbl.BindUserName();//绑定店铺管理人
            string manager = "";
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i].Id.ToString() == dt.Rows[0]["ShopManageId"].ToString())
                {
                    manager += "<option value='" + list1[i].Id + "'  selected='selected'>" + list1[i].userRealName + "</option>";
                }
                else
                {
                    manager += "<option value='" + list1[i].Id + "' >" + list1[i].userRealName + "</option>";
                }
            }
            StringBuilder AllTable = new StringBuilder();
            AllTable.Append("<span style='text-align:center'>修改店铺信息</span><br />");
            AllTable.Append("<br /><table  style='margin:auto;font-size:12px;'>");
            for (int i = 0; i < s.Length; i++)
            {
                if (allTableName.Contains(s[i]))
                {
                    AllTable.Append("<tr id='UpdateCondition'  value='" + dt.Rows[0]["Id"] + "'>");
                    if (s[i] == "Id")
                    {
                        AllTable.Append("<td>店铺编号:</td>");
                        AllTable.Append("<td><input type='text' id='Id' disabled='disabled' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "ShopName")
                    {
                        AllTable.Append("<td>店铺名称</td>");
                        AllTable.Append("<td><input type='text' id='ShopName' value='" + dt.Rows[0][s[i]] + "'/><td>");
                        SaveShopName = dt.Rows[0][s[i]].ToString();
                    }
                    if (s[i] == "ShoptypeId")
                    {
                        AllTable.Append("<td>店铺类别</td>");
                        AllTable.Append("<td><select id='ShopType'>" + shoptype + "</select><td>");
                    }
                    if (s[i] == "ShopManageId")
                    {
                        AllTable.Append("<td>店铺管理人</td>");
                        AllTable.Append("<td><select id='Manager'>" + manager + "</select><td>");
                    }
                    if (s[i] == "ShopState")
                    {
                        AllTable.Append("<td>店铺状态</td>");
                        if (dt.Rows[0]["ShopState"].ToString() != "0")
                        {
                            AllTable.Append("<td><input type='radio' id='Enable' checked='checked' name='ChooseName' value='1' />启用<input type='radio' id='disable' name='ChooseName' value='0' />暂不启用<td>");
                        }
                        if (dt.Rows[0]["ShopState"].ToString() != "1")
                        {
                            AllTable.Append("<td><input type='radio' id='Enable'  name='ChooseName' value='1' />启用<input type='radio' id='disable' name='ChooseName' checked='checked' value='0' />暂不启用<td>");
                        }
                    }
                    if (s[i] == "ShopIdex")
                    {
                        AllTable.Append("<td>排序</td>");
                        AllTable.Append("<td><input type='text' id='ShopIdex' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def1")
                    {
                        AllTable.Append("<td>默认1</td>");
                        AllTable.Append("<td><input type='text' id='Def1' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def2")
                    {
                        AllTable.Append("<td>默认2</td>");
                        AllTable.Append("<td><input type='text' id='Def2' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def3")
                    {
                        AllTable.Append("<td>默认3</td>");
                        AllTable.Append("<td><input type='text' id='Def3' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def4")
                    {
                        AllTable.Append("<td>默认4</td>");
                        AllTable.Append("<td><input type='text' id='Def4' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    if (s[i] == "Def5")
                    {
                        AllTable.Append("<td>默认5</td>");
                        AllTable.Append("<td><input type='text' id='Def5' value='" + dt.Rows[0][s[i]] + "'/><td>");
                    }
                    AllTable.Append("</tr>");
                }
            }
            AllTable.Append("</table><br /><br/><input type='button' onclick='UpdateShopName()'  value='确认编辑'/><input style='margin-left:20px;' type='button' onclick='Close()' value='退出编辑' />");
            return AllTable.ToString();
        }
        /// <summary>
        /// 确认编辑
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateShopOk()
        {

            string Id = Request.Form["Id"].ToString();
            string ShoptypeId = Request.Form["shoptype"].ToString();
            string ShopState = Request.Form["shopstate"].ToString();
            string ShopManageId = Request.Form["shopmanager"].ToString();
            string shopname = Request.Form["shopname"].ToString();
            if (SaveShopName != shopname)
            {
                if (sbl.ShopNameIsOn(shopname))
                {
                    return Json(sbl.UpdateShop(Id, shopname, ShoptypeId, ShopManageId, ShopState));
                }
                else
                {
                    return Json("店铺名称已存在！");
                }
            }
            else
            {
                return Json(sbl.UpdateShop(Id, shopname, ShoptypeId, ShopManageId, ShopState));
            }

        }
        /// <summary>
        /// 启用店铺
        /// </summary>
        /// <returns></returns>
        public ActionResult EnabledShop()
        {
            string id = Request.Form["Id"].ToString();
            if (sbl.ShopState(id) == 1)
            {
                return Json("当前店铺已经启用！");
            }
            else
            {
                sbl.UpdateShopState("1", id);
                return Json("启用成功！");
            }
        }
        /// <summary>
        /// 停用店铺
        /// </summary>
        /// <returns></returns>
        public ActionResult disableShop()
        {
            string id = Request.Form["Id"].ToString();
            if (sbl.ShopState(id) == 0)
            {
                return Json("当前店铺已经启停用！");
            }
            else
            {
                sbl.UpdateShopState("0", id);
                return Json("停用成功！");
            }
        }
        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertShop()
        {
            string ShopState = Request.Form["ShopState"].ToString();
            string shopname = Request.Form["shopname"].ToString();
            string ShopType = Request.Form["ShopType"].ToString();
            string UserId = Request.Form["UserId"].ToString();
            string Telphone = Request.Form["Telphone"].ToString();
            if (sbl.ShopNameIsOn(shopname))
            {
                return Json(sbl.InsertShop(shopname, ShopType, ShopState, UserId, Telphone));
            }
            return Json("店铺名称已存在");
        }

        #endregion



        #region 合作店铺
        public static int CollaborationSaveMenuId = 0;
        /// <summary>
        /// 加载页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Collaboration()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
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
            CollaborationSaveMenuId = menuId;
            #region 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            return View();
            #endregion
        }
        /// <summary>
        /// 页面第一次加载
        /// </summary>
        /// <returns></returns>
        public string CollaborationShopShow()
        {
            DataTable dt = sbl.SelectCollaborationShop(userInfo.User.Id.ToString());
            StringBuilder alltable = new StringBuilder();
            alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'>");
            alltable.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                alltable.Append("<th>");
                if (dt.Columns[i].ColumnName == "row_nb")
                    alltable.Append("序号");
                if (dt.Columns[i].ColumnName == "Id")
                    alltable.Append("店铺编号");
                if (dt.Columns[i].ColumnName == "ShopName")
                    alltable.Append("店铺名称");
                if (dt.Columns[i].ColumnName == "ShopState")
                    alltable.Append("店铺状态");
                if (dt.Columns[i].ColumnName == "Def1")
                    alltable.Append("联系电话");
                if (dt.Columns[i].ColumnName == "ShoptypeName")
                    alltable.Append("店铺类型");
                if (dt.Columns[i].ColumnName == "userRealName")
                    alltable.Append("店铺负责人");
                alltable.Append("</th>");
            }
            alltable.Append("<th>操作</th>");
            alltable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                alltable.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName == "ShopState")
                    {
                        alltable.Append("<td>");
                        if (dt.Rows[i]["ShopState"].ToString() == "1")
                        {
                            alltable.Append("已启用");
                        }
                        else
                        {
                            alltable.Append("未启用");

                        }
                        alltable.Append("</td>");
                    }
                    else
                    {
                        alltable.Append("<td>");
                        alltable.Append(dt.Rows[i][dt.Columns[j].ColumnName]);
                        alltable.Append("</td>");
                    }
                }

                alltable.Append("<td><a href='#' onclick='Instruction(\"" + dt.Rows[i]["Id"] + "\",\"" + dt.Rows[i]["ShopName"] + "\")'>仓库</a></td>");
                alltable.Append("</tr>");
            }
            alltable.Append("</table>");
            return alltable.ToString();
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <returns></returns>
        public static string ShopName;
        public ActionResult Warehouse()
        {
            ShopName = Request.QueryString["shopname"].ToString();
            ViewData["shopname"] = ShopName;
            ViewData["shopId"] = Request.QueryString["shopid"].ToString();
            ViewData["Page"] = phb.GetPageDDlist();
            return View();
        }
        /// <summary>
        /// 跳转仓库
        /// </summary>
        /// <returns></returns>
        public string WarehouseShopShow()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = CollaborationSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("shopproduct");
            StringBuilder alltable = new StringBuilder();
            #region    首栏搜索条件
            if (s.Contains("Style"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>款号：</span><input class='spanPropertyValue' type='text' id='StyleTop' style='width: 70px;'/></span>");
            }
            if (s.Contains("Pricea"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最小价格：</span><input class='spanPropertyValue' type='text' id='priceMin' style='width: 70px;'/></span>");
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最大价格：</span><input class='spanPropertyValue' type='text' id='priceMax' style='width: 70px;'/></span>");
            }
            if (s.Contains("Cat2"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>类别：</span>");
                alltable.Append("<input type='text' id='Cat2' class='spanPropertyValue' onclick='GetDropToBrand(this.id)'/>");
                alltable.Append("</span>");
            }
            if (s.Contains("Balance1"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最小库存：</span><input class='spanPropertyValue' type='text' id='BalanceMin' style='width: 70px;'/></span>");
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>最大库存：</span><input class='spanPropertyValue' type='text' id='BalanceMax' style='width: 70px;'/></span>");
            }
            if (s.Contains("Cat"))
            {

                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>品牌：</span>");
                alltable.Append("<input type='text' id='Cat' class='spanPropertyValue' onclick='GetDropToType(this.id)'/>");
                alltable.Append("</span>");
            }
            if (s.Contains("Imagefile"))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>图片状态：</span><select id='Imagefile' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>有</option><option value='0'>无</option></select></span>");
            }
            if (ph.isFunPermisson(roleId, menuId, funName.SalesState))
            {
                alltable.Append("<span class='spanProperty'><span class='spanPropertyName'>销售状态：</span><select id='salesState' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>已开放</option><option value='0'>未开放</option><option value='2'>已选货</option></select></span>");
            }
            alltable.Append("&nbsp;&nbsp;<input type='button' value='查询' class='spanPropertySearch' onclick='Search()'/>");
            if (ph.isFunPermisson(roleId, menuId, funName.SalesState))
            {

                alltable.Append("<input type='button' class='spanPropertySearch' value='开放销售' onclick='OpenSale()'/><input class='spanPropertySearch' type='button'  value='取消销售' onclick='CancelSale()'/>");
                alltable.Append("<input type='button' value='批量删除' class='spanPropertySearch' onclick='DeleteAll()'/>");
            }

            #endregion
            #region  下栏搜索条件
            StringBuilder alltableDown = new StringBuilder();
            if (s.Contains("Style"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>款号：</span><input class='spanPropertyValue' type='text' id='Style' style='width: 70px;'/></span>");
            }
            if (s.Contains("Scode"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>货号:</span><input class='spanPropertyValue' type='text' id='ScodeDown' style='width: 70px;'/></span>");
            }
            if (s.Contains("Cat"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>品牌：</span>");
                alltableDown.Append("<input id='CatDown' type='text' onclick='GetDropToBrand(this.id)' class='spanPropertyValue'/>"); ;
                alltableDown.Append("</span>");
            }
            if (s.Contains("Cat2"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>类别：</span>");
                alltableDown.Append("<input id='Cat2Down' type='text' onclick='GetDropToType(this.id)' class='spanPropertyValue'/>");
                alltableDown.Append("</span>");
            }
            if (s.Contains("Pricea"))
            {

                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最小价格：</span><input class='spanPropertyValue' type='text' id='PriceMinDown'/></span>");
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最大价格：</span><input class='spanPropertyValue' type='text' id='PriceMaxDown'/></span>");
            }
            if (s.Contains("Balance1"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最小库存：</span><input class='spanPropertyValue' type='text' id='StcokMinDown' style='width: 70px;'/></span>");
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>最大库存：</span><input class='spanPropertyValue' type='text' id='StcokMaxDown' style='width: 70px;'/></span>");
            }
            if (s.Contains("Lastgrnd"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>开始时间：</span><input type='text'  id='MinTimeDown' class='spanPropertyValue' onfocus=\"WdatePicker({isShowWeek:true,onpicked:function() {$dp.$(\'d122_1\').value=$dp.cal.getP(\'W\',\'W\');$dp.$(\'d122_2\').value=$dp.cal.getP(\'W\',\'WW\');}})\" /></span>");
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>结束时间：</span><input type='text'  id='MaxTimeDown' class='spanPropertyValue' onfocus=\"WdatePicker({isShowWeek:true,onpicked:function() {$dp.$(\'d122_1\').value=$dp.cal.getP(\'W\',\'W\');$dp.$(\'d122_2\').value=$dp.cal.getP(\'W\',\'WW\');}})\" /></span>");
            }
            if (s.Contains("Imagefile"))
            {
                alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>图片状态：</span><select id='Imagefiledown' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>有</option><option value='0'>无</option></select></span>");
            }
            alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'>销售状态：</span><select id='salesStatedown' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>已开放</option><option value='0'>未开放</option><option value='2'>已选货</option><option value='3'>已改价格</option><option value='5'>未改价格</option><option value='4'>已改库存</option><option value='6'>未改库存</option></select></span>");
            alltableDown.Append("<span class='spanProperty'><span class='spanPropertyName'><input type='button' value='查询 ' class='spanPropertySearch' onclick='SearchDown()'/></span></span>");
            #endregion
            return alltable.ToString() + "❤" + alltableDown.ToString();
        }
        /// <summary>
        /// 款号查询 
        /// </summary>
        /// <returns></returns>
        public static int SearchPageSum;
        public string SearchPage()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = CollaborationSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            #region 查询条件
            string[] str = new string[20];
            str[0] = Request.Form["style"] == null ? "" : Request.Form["style"].ToString().Trim();
            str[1] = Request.Form["priceMin"] == null ? "" : Request.Form["priceMin"].ToString().Trim();
            str[2] = Request.Form["priceMax"] == null ? "" : Request.Form["priceMax"].ToString().Trim();
            str[3] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString().Trim();
            str[4] = Request.Form["BalanceMin"] == null ? "" : Request.Form["BalanceMin"].ToString().Trim();
            str[5] = Request.Form["BalanceMax"] == null ? "" : Request.Form["BalanceMax"].ToString().Trim();
            str[6] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString().Trim();
            str[7] = Request.Form["Imagefile"] == null ? "" : Request.Form["Imagefile"].ToString().Trim();
            str[8] = Request.Form["salesState"] == null ? "" : Request.Form["salesState"].ToString().Trim();
            str[9] = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            #endregion
            int PageCount = Request.Form["Page"] == null ? 10 : int.Parse(Request.Form["Page"].ToString());
            int Index = int.Parse(Request.Form["Index"].ToString());
            StringBuilder alltable = new StringBuilder();
            int Count = sbl.SelectWarehouseByShopCount(str);
            int PageSum = Count % 10 > 0 ? Count / 10 + 1 : Count / 10;
            #region  分页判断
            switch (Index)
            {


                case 0:          //首页
                    SearchPageSum = 0;
                    break;
                case 1:          //上页 
                    if (SearchPageSum - 1 >= 0)
                    {
                        SearchPageSum = SearchPageSum - 1;
                    }
                    else
                    {
                        SearchPageSum = 0;
                    }
                    break;
                case 2:          //跳页
                    int tiaopage = int.Parse(Request.Form["PageReturn"].ToString());
                    if (tiaopage > PageSum - 1)
                    {
                        SearchPageSum = PageSum - 1;
                    }
                    else
                    {
                        if (tiaopage == 0)
                        {
                            SearchPageSum = 0;
                        }
                        else
                        {
                            SearchPageSum = tiaopage - 1;
                        }
                    }
                    break;
                case 3:           //下页
                    if (SearchPageSum + 1 <= PageSum - 1)
                    {
                        SearchPageSum = SearchPageSum + 1;
                    }
                    else
                    {
                        SearchPageSum = PageSum - 1;
                    }
                    break;
                case 4:           //末页
                    SearchPageSum = PageSum - 1;
                    break;

            }
            #endregion
            DataTable dt = sbl.SelectWarehouseByShop(str, SearchPageSum * PageCount, SearchPageSum * PageCount + PageCount);
            alltable.Append("<table id='Warehouse' class='mytable' rules='all'>");
            alltable.Append("<tr>");
            alltable.Append("<th><input type='checkbox' id='Checkall' onchange='CheckAll()'/></th>");
            #region   Table行标题
            dt.Columns.Remove("Pricea");
            dt.Columns.Remove("Cat2");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                alltable.Append("<th>");
                if (dt.Columns[i].ColumnName == "nid")
                    alltable.Append("序号");
                if (dt.Columns[i].ColumnName == "style")
                    alltable.Append("款号");
                if (dt.Columns[i].ColumnName == "Cat")
                    alltable.Append("品牌");
                if (dt.Columns[i].ColumnName == "TypeName")
                    alltable.Append("类别");
                if (dt.Columns[i].ColumnName == "Rolevel")
                    alltable.Append("预警库存");
                if (dt.Columns[i].ColumnName == "Balance")
                    alltable.Append("店铺库存");
                if (dt.Columns[i].ColumnName == "stylePicSrc")
                    alltable.Append("缩略图");
                if (dt.Columns[i].ColumnName == "ShowState")
                    alltable.Append("销售状态");
                alltable.Append("</th>");
            }
            alltable.Append("<th>操作</th>");
            alltable.Append("</tr>");
            #endregion
            #region Table内容
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    alltable.Append("<tr>");
                    alltable.Append("<td><input type='checkbox' id='Choose' styleName='" + dt.Rows[i]["Style"] + "' shopid='" + str[9] + "'/></td>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "ShowState")
                        {
                            #region  判断开放状态    0 已选货  1  未开放 2 已开放
                            if (dt.Rows[i]["ShowState"].ToString().Equals("0"))
                            {
                                alltable.Append("<td style='color:Green;'>");
                                alltable.Append("已选货");
                                alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("1"))
                            {
                                alltable.Append("<td style='color:red;'>");
                                alltable.Append("未开放");
                                alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("2"))
                            {
                                alltable.Append("<td style='color:blue;'>");
                                alltable.Append("已开放");
                                alltable.Append("</td>");
                            }
                            #endregion
                        }
                        else if (dt.Columns[j].ColumnName == "stylePicSrc")
                        {
                            #region 判断是否有图  若无图则不加载img标签
                            string image = dt.Rows[i][dt.Columns[j].ColumnName] == null ? "" : dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                            if (image == "")
                            {
                                alltable.Append("<td>");
                                alltable.Append("");
                                alltable.Append("</td>");
                            }
                            else
                            {

                                alltable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + image + "' width='50' height='50'/></td>");
                            }
                            #endregion
                        }
                        else
                        {
                            alltable.Append("<td>");
                            alltable.Append(dt.Rows[i][dt.Columns[j].ColumnName]);
                            alltable.Append("</td>");
                        }
                    }
                    alltable.Append("<td>");
                    #region 判断用户操作权限
                    if (ph.isFunPermisson(roleId, menuId, funName.LookPice))
                    {
                        alltable.Append("<a href='#' onclick='LookAttr(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\")'>查看图片</a>&nbsp;&nbsp;");
                    }
                    if (ph.isFunPermisson(roleId, menuId, funName.LookShop))
                    {
                        alltable.Append("<a href='#' onclick='LookShop(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\")'>查看商品</a>&nbsp;&nbsp;");
                    }
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        alltable.Append("<a href='#' onclick='Delete(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\")'>删除</a>&nbsp;&nbsp;");
                    }
                    if (dt.Rows[i]["ShowState"].ToString().Equals("1"))
                    {
                        alltable.Append("<a href='#' onclick='LoadStyleImage(\"" + dt.Rows[i]["Style"] + "\")'>下载图片</a>");
                    }
                    #endregion
                    alltable.Append("</td>");
                    alltable.Append("</tr>");
                }
            }
            #endregion
            alltable.Append("</table>");
            int thisPage = SearchPageSum + 1;
            return alltable.ToString() + "❤" + Count + "❤" + PageSum + "❤" + thisPage;
        }


        #region  商品图片处理
        ///  <summary>
        /// 显示配图
        /// </summary>
        public string ShowPeiPic()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string style = Request.QueryString["style"];
            string sql = @"select * from stylepic where style='" + style + "' and Left(Def2,1)='2' Order by RIGHT(Def2,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        /// <summary>
        /// 显示主图-款号表
        /// </summary>
        /// <returns></returns>
        public string ShowMainPicStyle()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string style = Request.QueryString["style"];
            string sql = @"select * from stylepic where style='" + style + "' and Left(Def1,1)='2' Order by RIGHT(Def1,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        /// <summary>
        /// 显示主图-货号表
        /// </summary>
        /// <returns></returns>
        public string ShowMainPic()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string scode = Request.QueryString["scode"];
            string sql = @"select * from scodepic where scode='" + scode + "' and Left(Def1,1)='2' Order by RIGHT(Def1,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["scodePicSrc"].ToString() + "@10q." + dt1.Rows[i]["scodePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        ///  <summary>
        /// 显示细节图
        /// </summary>
        public string ShowDetailPic()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string style = Request.QueryString["style"];
            string sql = @"select * from stylepic where style='" + style + "' and Left(Def3,1)='2'  Order by RIGHT(Def3,1)";
            dt1 = DbHelperSQL.Query(sql).Tables[0];

            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    temp = dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[3];
                    s += temp + "-*-";
                }
            }
            return s;
        }
        #endregion


        /// <summary>
        /// 货号分页
        /// </summary>
        /// <returns></returns>
        public static int WarehouseShopShowDownPageSum;
        public string WarehouseShopShowDownPage()
        {
            #region 加载查询条件
            string[] str = new string[20];
            str[1] = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();//货号
            str[2] = Request.Form["style"] == null ? "" : Request.Form["style"].ToString();//款号
            str[3] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();//品牌
            str[5] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();//类别
            str[6] = Request.Form["BalanceMin"] == null ? "" : Request.Form["BalanceMin"].ToString();//最小库存
            str[7] = Request.Form["BalanceMax"] == null ? "" : Request.Form["BalanceMax"].ToString();//最大库存 
            str[8] = Request.Form["priceMin"] == null ? "" : Request.Form["priceMin"].ToString();//最小价格
            str[9] = Request.Form["priceMax"] == null ? "" : Request.Form["priceMax"].ToString();//最大价格
            str[10] = Request.Form["minTime"] == null ? "" : Request.Form["minTime"].ToString();//最小时间
            str[11] = Request.Form["maxTime"] == null ? "" : Request.Form["maxTime"].ToString();//最大时间
            str[13] = Request.Form["salesState"] == null ? "" : Request["salesState"].ToString();//销售状态
            str[14] = Request.Form["Imagefiledown"] == null ? "" : Request.Form["Imagefiledown"].ToString();
            str[12] = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim(); ;//店铺编号
            #endregion
            int PageCount = Request.Form["Page"] == null ? 10 : int.Parse(Request.Form["Page"].ToString());
            int Index = int.Parse(Request.Form["Index"].ToString());
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int Count = sbl.SelectWarehouseByScodeCount(str);//总数量
            int PageSum = Count % 10 > 0 ? Count / 10 + 1 : Count / 10;//总页数
            #region  分页判断
            switch (Index)
            {
                case 0:          //首页
                    WarehouseShopShowDownPageSum = 0;
                    break;
                case 1:          //上页 
                    if (WarehouseShopShowDownPageSum - 1 >= 0)
                    {
                        WarehouseShopShowDownPageSum = WarehouseShopShowDownPageSum - 1;
                    }
                    else
                    {
                        WarehouseShopShowDownPageSum = 0;
                    }
                    break;
                case 2:          //跳页
                    int tiaopage = int.Parse(Request.Form["PageReturnD"].ToString());
                    if (tiaopage > PageSum - 1)
                    {
                        WarehouseShopShowDownPageSum = PageSum - 1;
                    }
                    else
                    {
                        if (tiaopage == 0)
                        {
                            WarehouseShopShowDownPageSum = 0;
                        }
                        else
                        {
                            WarehouseShopShowDownPageSum = tiaopage - 1;
                        }
                    }
                    break;
                case 3:           //下页
                    if (WarehouseShopShowDownPageSum + 1 <= PageSum - 1)
                    {
                        WarehouseShopShowDownPageSum = WarehouseShopShowDownPageSum + 1;
                    }
                    else
                    {
                        WarehouseShopShowDownPageSum = PageSum - 1;
                    }
                    break;
                case 4:           //末页
                    WarehouseShopShowDownPageSum = PageSum - 1;
                    break;
            }
            #endregion
            int menuId = CollaborationSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("outsideProduct");
            StringBuilder Alltable = new StringBuilder();
            DataTable dt = sbl.SelectWarehouseByScode(str, WarehouseShopShowDownPageSum * PageCount, WarehouseShopShowDownPageSum * PageCount + PageCount);
            #region 表头排序
            if (s.Contains("Imagefile"))
            {
                string temp;
                temp = s[0];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Imagefile")
                    {
                        s[i] = temp;
                        s[0] = "Imagefile";
                    }
                }
            }
            if (s.Contains("Style"))
            {
                string temp;
                temp = s[1];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Style")
                    {
                        s[i] = temp;
                        s[1] = "Style";
                    }
                }
            }
            if (s.Contains("Scode"))
            {
                string temp;
                temp = s[2];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Scode")
                    {
                        s[i] = temp;
                        s[2] = "Scode";
                    }
                }
            }
            if (s.Contains("Descript"))
            {
                string temp;
                temp = s[3];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Descript")
                    {
                        s[i] = temp;
                        s[3] = "Descript";
                    }
                }
            }
            if (s.Contains("Cdescript"))
            {
                string temp;
                temp = s[4];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Cdescript")
                    {
                        s[i] = temp;
                        s[4] = "Cdescript";
                    }
                }
            }
            #endregion
            string RoleType = sbl.GetUserTypeByPersonaId(roleId.ToString());
            Alltable.Append("<table id='Warehouse' class='mytable' rules='all'>");
            Alltable.Append("<tr>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                #region//判断头
                if (allTableName.Contains(s[i]))
                {
                    Alltable.Append("<th>");
                    if (s[i] == "Id")
                        Alltable.Append("编号");
                    if (s[i] == "Scode")
                        Alltable.Append("货号");
                    if (s[i] == "Bcode")
                        Alltable.Append("条码1");
                    if (s[i] == "Bcode2")
                        Alltable.Append("条码2");
                    if (s[i] == "Descript")
                        Alltable.Append("英文描述");
                    if (s[i] == "Cdescript")
                        Alltable.Append("中文描述");
                    if (s[i] == "Unit")
                        Alltable.Append("单位");
                    if (s[i] == "Currency")
                        Alltable.Append("货币");
                    if (s[i] == "Cat")
                        Alltable.Append("品牌");
                    if (s[i] == "Cat1")
                        Alltable.Append("季节");
                    if (s[i] == "Cat2")
                        Alltable.Append("类别");
                    if (s[i] == "Clolor")
                        Alltable.Append("颜色");
                    if (s[i] == "Size")
                        Alltable.Append("尺寸");
                    if (s[i] == "Style")
                        Alltable.Append("款号");
                    if (s[i] == "Pricea")
                        Alltable.Append("吊牌价");
                    if (s[i] == "Priceb")
                        Alltable.Append("零售价");
                    if (s[i] == "Pricec")
                        Alltable.Append("供货价");
                    if (s[i] == "Priced")
                        Alltable.Append("批发价");
                    if (s[i] == "Pricee")
                        Alltable.Append("成本价");
                    if (s[i] == "Disca")
                        Alltable.Append("折扣1");
                    if (s[i] == "Discb")
                        Alltable.Append("折扣2");
                    if (s[i] == "Discc")
                        Alltable.Append("折扣3");
                    if (s[i] == "Discd")
                        Alltable.Append("折扣4");
                    if (s[i] == "Disce")
                        Alltable.Append("折扣5");
                    if (s[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (s[i] == "Model")
                        Alltable.Append("型号");
                    if (s[i] == "Rolevel")
                        Alltable.Append("预警库存");
                    if (s[i] == "Roamt")
                        Alltable.Append("最少订货量");
                    if (s[i] == "Stopsales")
                        Alltable.Append("停售库存");
                    if (s[i] == "Loc")
                        Alltable.Append("店铺");
                    if (s[i] == "Balance")
                        Alltable.Append("店铺库存");
                    if (s[i] == "Balance2")
                        Alltable.Append("销售库存");
                    if (s[i] == "Balance3")
                        Alltable.Append("退货库存");
                    if (s[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (s[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (s[i] == "UserId")
                        Alltable.Append("操作人");
                    if (s[i] == "ShowState")
                        Alltable.Append("销售状态");
                    if (s[i] == "Def2")
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("退货库存");
                    if (s[i] == "Def5")
                        Alltable.Append("操作人");
                    if (s[i] == "Def6")
                        Alltable.Append("操作时间");
                    if (s[i] == "Def7")
                        Alltable.Append("价格状态");
                    if (s[i] == "Def8")
                        Alltable.Append("库存状态");
                    if (s[i] == "Def9")
                        Alltable.Append("默认9");
                    if (s[i] == "Def10")
                        Alltable.Append("默认10");
                    if (s[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
                #endregion
            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region 判断内容
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["row_nb"] + "</td>");
                for (int j = 0; j < s.Length; j++)
                {
                    string showState = sbl.GetShowStateByScodeAndLoc(dt.Rows[i]["Scode"].ToString(), str[12]);
                    if (allTableName.Contains(s[j]))
                    {
                        if (s[j] == "Def5")//判断用户字段查询出用户名
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(sbl.GetUserRealNameByUserId(dt.Rows[i]["Def5"].ToString()));
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Cat")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["BrandName"]);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Cat2")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["TypeName"]);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Vencode")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["sourceName"]);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "ShowState")
                        {
                            #region 判断开放状态    0 已选货  1 未开放  2 已开放
                            if (dt.Rows[i]["ShowState"].ToString().Equals("0"))
                            {
                                Alltable.Append("<td style='color:green;'>");
                                Alltable.Append("已选货");
                                Alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("1"))
                            {
                                Alltable.Append("<td style='color:red;'>");
                                Alltable.Append("未开放");
                                Alltable.Append("</td>");
                            }
                            else if (dt.Rows[i]["ShowState"].ToString().Equals("2"))
                            {
                                Alltable.Append("<td style='color:blue;'>");
                                Alltable.Append("已开放");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("</td>");
                            }
                            #endregion

                        }
                        else if (s[j] == "Imagefile")
                        {
                            #region 判断是否有图  若无图则不加载img标签
                            string image = dt.Rows[i]["ImagePath"] == null ? "" : dt.Rows[i]["ImagePath"].ToString();
                            if (image == "")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["ImagePath"] + "' width='50' height='50'/></td>");
                            }
                            #endregion
                        }
                        else if (s[j] == "Loc")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(ShopName);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Def7")
                        {
                            if (dt.Rows[i]["Def7"].ToString() == "1")
                            {
                                Alltable.Append("<td style='color:green;'>");
                                Alltable.Append("已改");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td style='color:red;'>");
                                Alltable.Append("未改");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Def8")
                        {
                            if (dt.Rows[i]["Def8"].ToString() == "1")
                            {
                                Alltable.Append("<td style='color:green;'>");
                                Alltable.Append("已改");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td style='color:red;'>");
                                Alltable.Append("未改");
                                Alltable.Append("</td>");
                            }
                        }
                        #region  判断价格是否显示  如果不为合作客户  或者显示状态为1 则可显示
                        else if (s[j] == "Pricea")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append("*****");
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Priceb")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Pricec")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Priced")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Pricee")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else if (s[j] == "Balance")
                        {
                            if (showState == "1" || RoleType != "Customer")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i][s[j]]);
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("*****");
                                Alltable.Append("</td>");
                            }
                        }
                        else
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i][s[j]]);
                            Alltable.Append("</td>");
                        }
                    }
                        #endregion

                }
                Alltable.Append("<td>");
                #region 判断用户操作权限
                if (ph.isFunPermisson(roleId, menuId, funName.LookAttr))
                {
                    Alltable.Append("<a href='#' onclick='LookAtrrbutt(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Vencode"].ToString().Trim() + "\")'>属性</a>&nbsp;");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    Alltable.Append("<a href='#' onclick='DeleteScode(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\")'>删除</a>&nbsp;&nbsp;");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.SubtractBalance))
                {
                    Alltable.Append("<a href='#' onclick='Subtract(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Vencode"].ToString().Trim() + "\")'>库存</a>&nbsp;&nbsp;");
                }
                if (ph.isFunPermisson(roleId, menuId, funName.UpdatePrice))
                {
                    Alltable.Append("<a href='#' onclick='UpdatePricea(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\",\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\")'>价格</a>&nbsp;&nbsp;");
                }
                if (dt.Rows[i]["ShowState"].ToString().Equals("1"))
                {
                    Alltable.Append("<a href='#' onclick='LoadScodeImage(\"" + dt.Rows[i]["Scode"] + "\")'>下载图片</a>");
                }
                #endregion
                Alltable.Append("</td>");
                Alltable.Append("</tr>");
                #endregion
            }
            int thisPage = WarehouseShopShowDownPageSum + 1;
            Alltable.Append("</table>");

            return Alltable.ToString() + "❤" + Count + "❤" + PageSum + "❤" + thisPage;
        }
        /// <summary>
        /// 开放销售
        /// </summary>
        /// <returns></returns>
        public string OpenSale()
        {
            string styles = Request.Form["styles"] == null ? "" : Request.Form["styles"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            string[] str = styles.Split('❤');
            string Scode = "";
            for (int i = 0; i < str.Length - 1; i++)
            {

                DataTable dt = sbl.GetOutSideByStyleAndShopId(str[i].ToString(), shopid);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string state = "0";
                    if (dt.Rows[j]["ShowState"].ToString() == "1") //判断当前货号是否为未开放状态
                    {
                        state = "2";  //是 则改为开放
                        sbl.OpenSalesByStyle(dt.Rows[j]["Scode"].ToString(), shopid, state, DateTime.Now.ToString(), userInfo.User.Id.ToString());
                    }
                    else
                    {
                        if (dt.Rows[j]["ShowState"].ToString() != "1")
                        {
                            Scode += dt.Rows[j]["Scode"].ToString() + "\n";
                            if (i == str.Length - 1)
                            {
                                Scode = "开放失败！";
                            }
                        }
                    }
                }

            }
            return Scode.ToString();
        }
        /// <summary>
        /// 取消销售
        /// </summary>
        /// <returns></returns>
        public string CancelSale()
        {
            string styles = Request.Form["styles"] == null ? "" : Request.Form["styles"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            string[] str = styles.Split('❤');
            string Scode = "";
            for (int i = 0; i < str.Length - 1; i++)
            {

                DataTable dt = sbl.GetOutSideByStyleAndShopId(str[i].ToString(), shopid);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string state = "0";

                    if (dt.Rows[j]["ShowState"].ToString() == "2") //判断当前货号是否为开放状态
                    {
                        state = "1";  //是 则改为开放
                        sbl.OpenSalesByStyle(dt.Rows[j]["Scode"].ToString(), shopid, state, DateTime.Now.ToString(), userInfo.User.Id.ToString());
                    }
                    else
                    {
                        if (dt.Rows[j]["ShowState"].ToString() != "2")
                        {
                            Scode += dt.Rows[j]["Scode"].ToString() + "\n";
                            if (i == str.Length - 1)
                            {
                                Scode = "取消失败！";
                            }
                        }
                    }
                }

            }
            return Scode.ToString();
        }
        /// <summary>
        /// 删除款号
        /// </summary>
        /// <returns></returns>
        public string DeleteByStyleAndShopid()
        {
            string style = Request.Form["styles"] == null ? "" : Request.Form["styles"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            return sbl.DeleteByStyleAndShopid(style, shopid);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public string DeleteByStyleAndShopidBatch()
        {
            string styles = Request.Form["styles"] == null ? "" : Request.Form["styles"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            string[] str = styles.Split('❤');
            for (int i = 0; i < str.Length - 1; i++)
            {
                sbl.DeleteByStyleAndShopid(str[i], shopid);
            }
            return "删除成功！";
        }
        /// <summary>
        /// 通过货号删除
        /// </summary>
        /// <returns></returns>
        public string DeleteScode()
        {
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            return sbl.DeletebyScodeAndShopid(scode, shopid);
        }
        /// <summary>
        /// 修改当前数据源 当前店铺 对应货号的 库存
        /// </summary>
        /// <returns></returns>
        public string UpdateBalanceByAndShopIdVencode()
        {
            ProductStockBLL psb = new ProductStockBLL();
            ShopProductBll spb = new ShopProductBll();
            int balance = Request.Form["Balance"] == null ? 0 : int.Parse(Request.Form["Balance"].ToString());
            string vencode = Request.Form["Vencod"] == null ? "" : Request.Form["Vencod"].ToString();
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            int countBalance = sbl.GetBalanceByScodeAndShopid(scode, shopid, vencode);
            string thisState = sbl.GetShowStateByScodeAndLoc(scode, shopid);
            string count = psb.CountByScode(scode, vencode).ToString();//当前货号的总库存
            int sum = spb.BalanceByScode(scode);//当前货号已分配的库存  
            int SurplusBalance = int.Parse(count.ToString()) - sum;//剩余库存
            DataTable dt = sbl.GetThisScodeState(scode, shopid);//查询当前店铺该货号的状态 
            int thisBalance = int.Parse(dt.Rows[0]["Balance"].ToString());
            int KxGkc = sum - thisBalance;//出当前店铺当前货号 以外的库存
            if (balance + KxGkc > int.Parse(count.ToString()))
            {
                return "货号" + scode + "剩余库存不足:" + balance + "件";
            }
            else
            {
                sbl.UpdateBalanceByAndShopIdVencode(scode, shopid, vencode, balance.ToString(), "1", DateTime.Now.ToString(), userInfo.User.Id.ToString());//修改价格  时将状态改为以修改价格  
                DataTable dtState = sbl.GetThisScodeState(scode, shopid);//查询当前店铺该货号的状态 
                if (dtState.Rows.Count > 0)//不管有没有修改成功 都判断
                {
                    string def7 = dtState.Rows[0]["Def7"] == null ? "" : dtState.Rows[0]["Def7"].ToString();
                    string def8 = dtState.Rows[0]["Def8"] == null ? "" : dtState.Rows[0]["Def8"].ToString();
                    if (def7 == "1" && def8 == "1")//如果两个都已经修改 则修改为未开放
                    {
                        if (dtState.Rows[0]["ShowState"].ToString() != "2")
                        {
                            sbl.OpenSalesByStyle(scode, shopid, "1", DateTime.Now.ToString(), userInfo.User.Id.ToString());
                        }
                        return "修改成功！";
                    }
                }
                else
                {
                    return "修改成功！";
                }
                return "修改成功！";
            }

        }
        /// <summary>
        /// 打开库存修改页面
        /// </summary>
        /// <returns></returns>
        public string PageShowRk()
        {

            ProductStockBLL psb = new ProductStockBLL();
            ShopProductBll spb = new ShopProductBll();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            string ShopName = spb.GetShopNameByShopId(shopid);
            string Scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string venName = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string vencode = venName;
            int sum = spb.BalanceByScode(Scode);//当前货号已分配的库存  
            string count = psb.CountByScode(Scode, vencode).ToString();//当前货号的总库存
            int SurplusBalance = int.Parse(count.ToString()) - sum;//剩余库存
            int countBalance = int.Parse(count.ToString());
            DataTable dt = spb.BalanceAndShopShow(0, 10, vencode, Scode);
            int countShopBalance = spb.BalanceAndShopShowCount(vencode, Scode);//共多少条记录
            int PageSum = countShopBalance % 10 == 0 ? countShopBalance / 10 : countShopBalance / 10 + 1;//共多少页
            StringBuilder alltable = new StringBuilder();
            alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'>");
            alltable.Append("<tr style='text-align:center;'>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName != "Loc")
                {
                    alltable.Append("<th>");
                    if (dt.Columns[i].ColumnName == "Id")
                        alltable.Append("序号");
                    if (dt.Columns[i].ColumnName == "ShopName")
                        alltable.Append("店铺名称");
                    if (dt.Columns[i].ColumnName == "Balance")
                        alltable.Append("库存");
                    if (dt.Columns[i].ColumnName == "userRealName")
                        alltable.Append("操作人");
                    if (dt.Columns[i].ColumnName == "Def6")
                        alltable.Append("操作时间");
                    if (dt.Columns[i].ColumnName == "Scode")
                        alltable.Append("货号");
                    alltable.Append("</th>");
                }
            }
            alltable.Append("</tr>");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ShopName"].ToString().Equals(ShopName))
                    {
                        alltable.Append("<tr style='background-color:gray;font-weight:bold;'>");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName != "Loc")
                            {
                                alltable.Append("<td>");
                                alltable.Append(dt.Rows[i][j]);
                                alltable.Append("</td>");
                            }

                        }
                        alltable.Append("</tr>");
                    }
                    else
                    {
                        alltable.Append("<tr>");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName != "Loc")
                            {
                                alltable.Append("<td>");
                                alltable.Append(dt.Rows[i][j]);
                                alltable.Append("</td>");
                            }

                        }
                        alltable.Append("</tr>");
                    }

                }
            }
            alltable.Append("</table>");
            return Scode + "❤" + psb.GetSourname(venName).ToString() + "❤" + count + "❤" + alltable + "❤" + countShopBalance + "❤" + PageSum + "❤" + SurplusBalance;
        }
        /// <summary>
        /// 显示价格
        /// </summary>
        /// <returns></returns>
        public string GetPriceByScodeandShopid()
        {
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            DataTable dt = sbl.GetPriceByScodeandShopid(scode, shopid);
            string str = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                str += dt.Rows[0][i].ToString() + "❤";
            }
            return str;
        }
        ///修改价格
        ////
        public string UpdatePricecByStyleAndPrice()
        {
            string price = Request.Form["pricec"] == null ? "" : Request.Form["pricec"].ToString();
            string Updateprice = Request.Form["UpatePricec"] == null ? "" : Request.Form["UpatePricec"].ToString();
            string style = Request.Form["style"] == null ? "" : Request.Form["style"].ToString();
            string Scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            string State = sbl.GetShowStateByScodeAndLoc(Scode, shopid);
            string reslut = sbl.UpdatePricecByStyleAndPrice(style, price, Updateprice, shopid, DateTime.Now.ToString(), userInfo.User.Id.ToString());
            if (reslut == "修改成功")
            {
                DataTable dt = sbl.GetThisScodeState(Scode, shopid);//查询当前店铺该货号的状态 
                if (dt.Rows.Count > 0)
                {
                    string def7 = dt.Rows[0]["Def7"] == null ? "" : dt.Rows[0]["Def7"].ToString();
                    string def8 = dt.Rows[0]["Def8"] == null ? "" : dt.Rows[0]["Def8"].ToString();
                    if (def7 == "1" && def8 == "1")//如果两个都已经修改 则修改为未开放
                    {
                        if (dt.Rows[0]["ShowState"].ToString() != "2")
                        {
                            sbl.OpenSalesByStyle(Scode, shopid, "1", DateTime.Now.ToString(), userInfo.User.Id.ToString());
                        }
                    }
                }
                return "修改成功";
            }
            else
            {
                return "修改失败";
            }

        }
        /// <summary>
        /// 查看属性
        /// </summary>
        /// <returns></returns>
        public string GetShopProductByScodeAndShopIdVencode()
        {
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            string RoleType = sbl.GetUserTypeByPersonaId(roleId.ToString());
            string showState = sbl.GetShowStateByScodeAndLoc(scode, shopid);
            DataTable dt = sbl.GetProductstockByScodeAndVencode(scode, vencode, shopid);
            string property = GetTypeNo(scode, dt.Rows[0]["TypeNo"].ToString());
            string s = string.Format(@"<table id='tabProperty'>
            <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><img src='{11}' style='width:120px;' /></td></tr>
            <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' disabled='disabled' type='text' value='{2}' /></td></tr>
            <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                <td>季节：<input type='text' value='{4}' disabled='disabled' /></td></tr>
            <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
            <tr><td>预警库存：<input id='txtRolevelE' disabled='disabled' type='text' value='{9}' /></td><td>类别：<input type='text' disabled='disabled' id='hidId'' value='{5}' /></td></tr>
        </table>", dt.Rows[0]["Scode"].ToString()
                             , dt.Rows[0]["Descript"].ToString()
                             , dt.Rows[0]["Cdescript"].ToString()
                             , dt.Rows[0]["Cat"].ToString()
                             , dt.Rows[0]["Cat1"].ToString()
                             , dt.Rows[0]["Cat2"].ToString()
                             , dt.Rows[0]["Clolor"].ToString()
                             , dt.Rows[0]["Style"].ToString()
                             , dt.Rows[0]["Size"].ToString()
                             , dt.Rows[0]["Rolevel"].ToString()
                             , dt.Rows[0]["Id"].ToString()
                             , dt.Rows[0]["ImagePath"].ToString()

                        );
            #region
            StringBuilder alltableprice = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            int menuId = CollaborationSaveMenuId;
            string[] Spermission = ph.getFiledPermisson(roleId, menuId, funName.selectName);

            #endregion
            return s.ToString() + "❤" + alltableprice.ToString() + "❤" + property;
        }
        /// <summary>
        /// 查看商品
        /// </summary>
        /// <returns></returns>
        public string LookShop()
        {
            string[] str = new string[20];
            str[1] = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();//货号
            str[2] = Request.Form["style"] == null ? "" : Request.Form["style"].ToString();//款号
            str[3] = Request.Form["Cat2"] == null ? "" : Request.Form["Cat2"].ToString();//品牌
            str[5] = Request.Form["Cat"] == null ? "" : Request.Form["Cat"].ToString();//类别
            str[6] = Request.Form["BalanceMin"] == null ? "" : Request.Form["BalanceMin"].ToString();//最小库存
            str[7] = Request.Form["BalanceMax"] == null ? "" : Request.Form["BalanceMax"].ToString();//最大库存
            str[8] = Request.Form["priceMin"] == null ? "" : Request.Form["priceMin"].ToString();//最小价格
            str[9] = Request.Form["priceMax"] == null ? "" : Request.Form["priceMax"].ToString();//最大价格
            str[10] = Request.Form["minTime"] == null ? "" : Request.Form["minTime"].ToString();//最小时间
            str[11] = Request.Form["maxTime"] == null ? "" : Request.Form["maxTime"].ToString();//最大时间
            str[12] = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim(); //shopid;//店铺编号
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int Count = sbl.SelectWarehouseByScodeCount(str);//总数量
            int PageSum = Count % 10 > 0 ? Count / 10 + 1 : Count / 10;//总页数
            int menuId = CollaborationSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            usersbll usb = new usersbll();
            string[] allTableName = usb.getDataName("outsideProduct");
            StringBuilder Alltable = new StringBuilder();
            DataTable dt = sbl.SelectWarehouseByScode(str, 0, 10);
            if (s.Contains("Imagefile"))
            {
                string temp;
                temp = s[2];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Imagefile")
                    {
                        s[i] = temp;
                        s[2] = "Imagefile";
                    }
                }
            }
            if (s.Contains("Style"))
            {
                string temp;
                temp = s[3];
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == "Style")
                    {
                        s[i] = temp;
                        s[3] = "Style";
                    }
                }
            }
            s[2] = "Style";
            s[3] = "Imagefile";
            Alltable.Append("<table id='Warehouse' class='mytable' style='font-size:12px;'>");
            Alltable.Append("<tr>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                #region//判断头
                if (allTableName.Contains(s[i]))
                {
                    Alltable.Append("<th>");
                    if (s[i] == "Id")
                        Alltable.Append("编号");
                    if (s[i] == "Scode")
                        Alltable.Append("货号");
                    if (s[i] == "Bcode")
                        Alltable.Append("条码1");
                    if (s[i] == "Bcode2")
                        Alltable.Append("条码2");
                    if (s[i] == "Descript")
                        Alltable.Append("英文描述");
                    if (s[i] == "Cdescript")
                        Alltable.Append("中文描述");
                    if (s[i] == "Unit")
                        Alltable.Append("单位");
                    if (s[i] == "Currency")
                        Alltable.Append("货币");
                    if (s[i] == "Cat")
                        Alltable.Append("品牌");
                    if (s[i] == "Cat1")
                        Alltable.Append("季节");
                    if (s[i] == "Cat2")
                        Alltable.Append("类别");
                    if (s[i] == "Clolor")
                        Alltable.Append("颜色");
                    if (s[i] == "Size")
                        Alltable.Append("尺寸");
                    if (s[i] == "Style")
                        Alltable.Append("款号");
                    if (s[i] == "Pricea")
                        Alltable.Append("吊牌价");
                    if (s[i] == "Priceb")
                        Alltable.Append("零售价");
                    if (s[i] == "Pricec")
                        Alltable.Append("供货价");
                    if (s[i] == "Priced")
                        Alltable.Append("批发价");
                    if (s[i] == "Pricee")
                        Alltable.Append("成本价");
                    if (s[i] == "Disca")
                        Alltable.Append("折扣1");
                    if (s[i] == "Discb")
                        Alltable.Append("折扣2");
                    if (s[i] == "Discc")
                        Alltable.Append("折扣3");
                    if (s[i] == "Discd")
                        Alltable.Append("折扣4");
                    if (s[i] == "Disce")
                        Alltable.Append("折扣5");
                    if (s[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (s[i] == "Model")
                        Alltable.Append("型号");
                    if (s[i] == "Rolevel")
                        Alltable.Append("预警库存");
                    if (s[i] == "Roamt")
                        Alltable.Append("最少订货量");
                    if (s[i] == "Stopsales")
                        Alltable.Append("停售库存");
                    if (s[i] == "Loc")
                        Alltable.Append("店铺");
                    if (s[i] == "Balance1")
                        Alltable.Append("店铺库存");
                    if (s[i] == "Balance2")
                        Alltable.Append("销售库存");
                    if (s[i] == "Balance3")
                        Alltable.Append("退货库存");
                    if (s[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (s[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (s[i] == "UserId")
                        Alltable.Append("操作人");
                    if (s[i] == "Def1")
                        Alltable.Append("操作时间");
                    if (s[i] == "Def2")
                        Alltable.Append("供应商");
                    if (s[i] == "Def3")
                        Alltable.Append("销售状态");
                    if (s[i] == "Def4")
                        Alltable.Append("默认4");
                    if (s[i] == "Def5")
                        Alltable.Append("默认5");
                    if (s[i] == "Def6")
                        Alltable.Append("默认6");
                    if (s[i] == "Def7")
                        Alltable.Append("默认7");
                    if (s[i] == "Def8")
                        Alltable.Append("默认8");
                    if (s[i] == "Def9")
                        Alltable.Append("默认9");
                    if (s[i] == "Def10")
                        Alltable.Append("默认10");
                    if (s[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
                #endregion
            }
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region  内容
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + dt.Rows[i]["row_nb"] + "</td>");
                for (int j = 0; j < s.Length; j++)
                {

                    if (allTableName.Contains(s[j]))
                    {
                        if (s[j] == "UserId")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(sbl.GetUserRealNameByUserId(dt.Rows[i]["UserId"].ToString()));
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Cat")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["BrandName"]);
                            Alltable.Append("</td>");

                        }
                        else if (s[j] == "Cat2")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["TypeName"]);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Def3")
                        {
                            if (dt.Rows[i]["Def3"].ToString().Equals("0"))
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("未开放");
                                Alltable.Append("</td>");
                            }
                            else
                            {
                                Alltable.Append("<td>");
                                Alltable.Append("以开放");
                                Alltable.Append("</td>");
                            }
                        }
                        else
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i][s[j]]);
                            Alltable.Append("</td>");
                        }
                    }

                }
                Alltable.Append("<td><a href='#' onclick='LookAtrrbutt(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Def2"].ToString().Trim() + "\")'>查看属性</a>|||<a href='#' onclick='DeleteScode(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\")'>删除</a>|||<a href='#' onclick='Subtract(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Def2"].ToString().Trim() + "\")'>减去库存</a>|||<a href='#' onclick='UpdatePricea(\"" + dt.Rows[i]["Style"].ToString().Trim() + "\",\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\")'>修改价格</a></td>");
                Alltable.Append("</tr>");
                #endregion
            }
            Alltable.Append("</table>");

            return Alltable.ToString() + "❤" + Count + "❤" + PageSum;
        }
        /// <summary>
        /// 获取拉列表商品类别显示相应属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTypeNo(string scode, string typeno)
        {
            ProductBll pro = new ProductBll();
            string s = string.Empty;
            DataTable dt = new DataTable();
            dt = pro.GetPropertyByTypeNo(scode, typeno);
            if (dt.Rows.Count > 0)
            {
                s += "<table style='text-align:right;'>";//--hidUpdateType判断是Insert 还是Update
                if (dt.Rows.Count % 3 == 0)
                {
                    s += "<tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += "<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\" title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>";
                        if ((i + 1) % 3 == 0)
                            s += "</tr>";
                    }
                }
                else if (dt.Rows.Count % 3 == 1)
                {
                    s += "<tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += "<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\"  title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>";
                        if ((i + 1) % 3 == 0)
                            s += "</tr>";
                    }
                    s += "<td style='width:237px'></td><td style='width:238px'></td></tr>";
                }
                else
                {
                    s += "<tr>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += "<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\"  title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>";
                        if ((i + 1) % 3 == 0)
                            s += "</tr>";
                    }
                    s += "<td style='width:238px'></td></tr>";
                }
                s += "</table>";
            }
            return s;
        }
        #endregion



        #region 淘宝店铺
        /// <summary>
        /// 页面首次加载
        /// </summary>
        /// <returns></returns>
        public ActionResult TaoBaoPage()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
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
            // 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            DataTable dt = tpmb.GetShopName();
            StringBuilder shopName = new StringBuilder();
            shopName.Append("<option value=''>请选择</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                shopName.Append("<option value='" + dt.Rows[i]["ProductShopName"] + "'>" + dt.Rows[i]["ProductShopName"] + "</option>");
            }
            ViewData["PageSum"] = phb.GetPageDDlist();
            ViewData["Brand"] = phb.GetBrandDDlist(userInfo.User.Id);
            ViewData["Type"] = phb.GetTypeDDlist(userInfo.User.Id);
            ViewData["ShopName"] = shopName;
            return View();
        }
        /// <summary>
        /// 显示淘宝店铺
        /// </summary>
        /// <returns></returns>
        tbProductReMarkBll tpmb = new tbProductReMarkBll();
        public string GetProductReMarkTb()
        {
            PublicHelpController ph = new PublicHelpController();
            ProductHelper proh = new ProductHelper();
            usersbll usb = new usersbll();
            #region 查询条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件

            string ProductId = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string MinBalance = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//淘宝最小库存
            string MaxBalance = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//淘宝最大库存
            string HkBalancemin = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//香港最小库存
            string HkBalancemax = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//香港最大库存
            string Pricemin = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小价格
            string Pricemax = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大价格
            string ShopName = helpcommon.StrSplit.StrSplitData(orderParamss[8], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//店铺名称
            string Brand = helpcommon.StrSplit.StrSplitData(orderParamss[9], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//品牌
            string Type = helpcommon.StrSplit.StrSplitData(orderParamss[10], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别
            string Cat1 = helpcommon.StrSplit.StrSplitData(orderParamss[11], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//季节
            string SaleState = helpcommon.StrSplit.StrSplitData(orderParamss[12], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//销售状态
            string HkBalance = helpcommon.StrSplit.StrSplitData(orderParamss[13], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//香港库存比价


            ProductId = ProductId == "\'\'" ? string.Empty : ProductId;
            Style = Style == "\'\'" ? string.Empty : Style;
            MinBalance = MinBalance == "\'\'" ? string.Empty : MinBalance;
            MaxBalance = MaxBalance == "\'\'" ? string.Empty : MaxBalance;
            HkBalancemin = HkBalancemin == "\'\'" ? string.Empty : HkBalancemin;
            HkBalancemax = HkBalancemax == "\'\'" ? string.Empty : HkBalancemax;
            Pricemin = Pricemin == "\'\'" ? string.Empty : Pricemin;
            Pricemax = Pricemax == "\'\'" ? string.Empty : Pricemax;
            ShopName = ShopName == "\'\'" ? string.Empty : ShopName;
            Type = Type == "\'\'" ? string.Empty : Type;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            Cat1 = Cat1 == "\'\'" ? string.Empty : Cat1;
            SaleState = SaleState == "\'\'" ? string.Empty : SaleState;
            HkBalance = HkBalance == "\'\'" ? string.Empty : HkBalance;

            dic.Add("ProductId", ProductId);
            dic.Add("Style", Style);
            dic.Add("MinBalance", MinBalance);
            dic.Add("MaxBalance", MaxBalance);
            dic.Add("HkBalancemin", HkBalancemin);
            dic.Add("HkBalancemax", HkBalancemax);
            dic.Add("Pricemin", Pricemin);
            dic.Add("Pricemax", Pricemax);
            dic.Add("ShopName", ShopName);
            dic.Add("Cat2", Type);
            dic.Add("Cat", Brand);
            dic.Add("Cat1", Cat1);
            dic.Add("SaleState", SaleState);
            dic.Add("HkBalance", HkBalance);

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);//页码
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);//每页显示个数

            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段

            int Count = tpmb.GetProductReMarkTbCount(dic);//数据总个数
            int PageCount = Count % pageSize > 0 ? Count / pageSize + 1 : Count / pageSize;//数据总页数

            #endregion

            int MinId = (pageIndex - 1) * pageSize;
            int MaxId = (pageIndex - 1) * pageSize + pageSize;
            DataTable dt = tpmb.GetProductReMarkTb(dic, MinId, MaxId);
            StringBuilder AllTable = new StringBuilder();


            #region  表头排序

            s = proh.TableHeader(s, 0, "ProductReMarkId");
            s = proh.TableHeader(s, 1, "ProductStyle");
            s = proh.TableHeader(s, 2, "ProductTitle");
            s = proh.TableHeader(s, 3, "ProductImg");
            s = proh.TableHeader(s, 4, "ProductTaoBaoPrice");
            s = proh.TableHeader(s, 5, "Pricea");
            s = proh.TableHeader(s, 6, "Priceb");
            s = proh.TableHeader(s, 7, "Pricec");
            s = proh.TableHeader(s, 8, "Priced");
            s = proh.TableHeader(s, 9, "Pricee");

            #endregion

            #region Table表头
            AllTable.Append("<table id='mytable' class='mytable' rules='all'><tr>");
            AllTable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case "ProductReMarkId":
                        AllTable.Append("<th>淘宝商品编号</th>");
                        break;
                    case "ProductStyle":
                        AllTable.Append("<th>款号</th>");
                        break;
                    case "ProductTitle":
                        AllTable.Append("<th>商品标题</th>");
                        break;
                    case "ProductImg":
                        AllTable.Append("<th>商品图片</th>");
                        break;
                    case "ProductTaoBaoPrice":
                        AllTable.Append("<th>淘宝价格</th>");
                        break;
                    case "ProductYJStock":
                        AllTable.Append("<th>预警库存</th>");
                        break;
                    case "ProductSJ":
                        AllTable.Append("<th>上架库存</th>");
                        break;
                    case "ProductSJTime1":
                        AllTable.Append("<th>上架时间</th>");
                        break;
                    case "ProductXJTime2":
                        AllTable.Append("<th>下架时间</th>");
                        break;
                    case "ProductState":
                        AllTable.Append("<th>状态</th>");
                        break;
                    case "ProductShopName":
                        AllTable.Append("<th>淘宝店铺</th>");
                        break;
                    case "ProductReMarkShopCar":
                        AllTable.Append("<th>购物车</th>");
                        break;
                    case "ProductReMarkActivity":
                        AllTable.Append("<th>活动</th>");
                        break;
                    case "ProductReMarkKeep":
                        AllTable.Append("<th>收藏</th>");
                        break;
                    case "ProductReMarkStockA":
                        AllTable.Append("<th>库存A</th>");
                        break;
                    case "ProductReMarkStockB":
                        AllTable.Append("<th>库存B</th>");
                        break;
                    case "ProductReMark1":
                        AllTable.Append("<th>备注</th>");
                        break;
                    case "ProductOther1":
                        AllTable.Append("<th>其他1</th>");
                        break;
                    case "ProductOther2":
                        AllTable.Append("<th>其他2</th>");
                        break;
                    case "ProductOther3":
                        AllTable.Append("<th>其他3</th>");
                        break;
                    case "ProductOther4":
                        AllTable.Append("<th>其他4</th>");
                        break;
                    case "Def1":
                        AllTable.Append("<th>默认1</th>");
                        break;
                    case "Def2":
                        AllTable.Append("<th>默认2</th>");
                        break;
                    case "Def3":
                        AllTable.Append("<th>默认3</th>");
                        break;
                    case "Def4":
                        AllTable.Append("<th>默认4</th>");
                        break;
                    case "Def5":
                        AllTable.Append("<th>默认5</th>");
                        break;
                    case "Def6":
                        AllTable.Append("<th>默认6</th>");
                        break;
                    case "Def7":
                        AllTable.Append("<th>默认7</th>");
                        break;
                    case "Def8":
                        AllTable.Append("<th>默认8</th>");
                        break;
                    case "Def9":
                        AllTable.Append("<th>默认9</th>");
                        break;
                    case "Def10":
                        AllTable.Append("<th>默认10</th>");
                        break;
                    case "Pricea":
                        AllTable.Append("<th>吊牌价<th>");
                        break;
                    case "Priceb":
                        AllTable.Append("<th>零售价</th>");
                        break;
                    case "Pricec":
                        AllTable.Append("<th>VIP价格</th>");
                        break;
                    case "Priced":
                        AllTable.Append("<th>批发价</th>");
                        break;
                    case "Pricee":
                        AllTable.Append("<th>成本价</th>");
                        break;
                    case "Cat":
                        AllTable.Append("<th>品牌</th>");
                        break;
                    case "Cat2":
                        AllTable.Append("<th>类别</th>");
                        break;
                    case "Cat1":
                        AllTable.Append("<th>季节</th>");
                        break;
                    case "Balance":
                        AllTable.Append("<th>实库存</th>");
                        break;
                }
            }
            AllTable.Append("<th>操作</th>");
            AllTable.Append("</tr>");
            #endregion

            #region Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AllTable.Append("<tr>");
                AllTable.Append("<td>");
                AllTable.Append(dt.Rows[i]["row_nb"]);
                AllTable.Append("</td>");
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j] == "ProductState")
                    {
                        if (dt.Rows[i]["ProductState"].ToString() == "onsale")
                        {
                            AllTable.Append("<td>");
                            AllTable.Append("销售中");
                            AllTable.Append("</td>");
                        }
                        else
                        {
                            AllTable.Append("<td>");
                            AllTable.Append("仓库中");
                            AllTable.Append("</td>");
                        }
                    }
                    else if (s[j] == "ProductTitle")
                    {
                        AllTable.Append("<td style='width:120px;'>");
                        AllTable.Append("<a href=http://item.taobao.com/item.htm?id=" + dt.Rows[i]["ProductReMarkId"] + " target='_bank' >" + dt.Rows[i]["ProductTitle"] + "</a>");
                        AllTable.Append("</td>");
                    }
                    else if (s[j] == "ProductImg")
                    {

                        AllTable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["ProductImg"] + "' width='50' height='50'/></td>");
                    }
                    else if (s[j] == "Cat")
                    {
                        AllTable.Append("<td>" + dt.Rows[i]["BrandName"] + "</td>");
                    }
                    else if (s[j] == "Cat2")
                    {
                        AllTable.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                    }
                    else
                    {
                        AllTable.Append("<td>");
                        AllTable.Append(dt.Rows[i][s[j]]);
                        AllTable.Append("</td>");
                    }

                }
                AllTable.Append("<td><a href='#' onclick='LookDetailed(\"" + dt.Rows[i]["ProductReMarkId"] + "\",\"" + dt.Rows[i]["ProductStyle"] + "\")'>查看详细</a></td>");
                AllTable.Append("</tr>");
            }
            AllTable.Append("</table>");
            #endregion

            #region 分页
            AllTable.Append("-----");
            AllTable.Append(PageCount + "-----" + Count);
            #endregion


            return AllTable.ToString();
        }
        /// <summary>
        /// 根据款号获取商品品牌和类别
        /// </summary>
        /// <returns></returns>
        public string GetProductBrandCat2(string style)
        {
            string s = string.Empty;

            DataTable dt = tpmb.GetList(" style='" + style + "'").Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    s += dt.Rows[0]["Cat"] + "," + dt.Rows[0]["Cat2"];
                }
                else
                {
                    s += ",";
                }

            }
            return s;
        }
        /// <summary>
        /// 添加-获取淘宝商品列表数据(淘宝在售-仓库中的数据)
        /// </summary>
        /// <returns></returns>
        public string AddProductData()
        {
            string s = string.Empty;

            HttpCookie cookie = Request.Cookies["userInfo"];
            if (cookie == null)
            {
                return null;
            }
            productSales tmallapp = new productSales();
            List<Item> itemList = new List<Item>();


            for (int i = 0; i < userInfo.TaoAppUserList.Count; i++)
            {
                tmallapp.Sessionkey = userInfo.TaoAppUserList[i].refreshToken;
                #region 获取上架商品
                int temp = tmallapp.GetTaoShopOnSaleItems();    //攀比轩店铺  444件    //弗罗伦斯店铺   656 
                if (temp > 0)
                {
                    int myOnSaleNum = temp % 200;
                    int myOnSaleNum1 = temp / 200;
                    if (temp % 200 != 0)
                    {
                        myOnSaleNum1 += 1;
                    }

                    for (int m = 1; m <= myOnSaleNum1; m++)
                    {
                        itemList.AddRange(tmallapp.GetTaoShopOnSaleItems(m, 200));
                    }
                }
                #endregion
                #region 获取仓库商品
                int temp1 = tmallapp.GetInventoryTaoShop();
                if (temp1 > 0)
                {
                    int myOnSaleNum = temp1 % 200;
                    int myOnSaleNum1 = temp1 / 200;
                    if (temp1 % 200 != 0)
                    {
                        myOnSaleNum1 += 1;
                    }

                    for (int m = 1; m <= myOnSaleNum1; m++)
                    {
                        itemList.AddRange(tmallapp.GetInventoryTaoShop(m, 200));
                    }

                }
                #endregion
                for (int j = 0; j < itemList.Count; j++)
                {
                    string style = ParmPerportys.GetStrParms(itemList[j].OuterId);
                    string[] BrandCat = StrSplit.StrSplitData(GetProductBrandCat2(style), ',');
                    if (BrandCat.Length > 0)
                    {
                        itemList[j].InputPids = BrandCat[0] == null ? "" : BrandCat[0].ToString();  //品牌
                    }
                    else
                    {
                        itemList[j].InputPids = "";
                    }
                    if (BrandCat.Length > 1)
                    {
                        itemList[j].InputStr = BrandCat[1] == null ? "" : BrandCat[1].ToString();  //类别
                    }
                    else
                    {
                        itemList[j].InputStr = "";
                    }
                }
            }
            ////-******************************
            string[] strcc = new string[itemList.Count];
            for (int i = 0; i < itemList.Count; i++)
            {
                strcc[i] = itemList[i].NumIid.ToString();
            }
            string[] strcj = tpmb.GetAll();
            string listccc = "";
            for (int j = 0; j < strcj.Length; j++)
            {
                if (!strcc.Contains(strcj[j]))
                {
                    listccc += ",'" + strcj[j] + "'";

                }
            }
            listccc = listccc.Trim(',');
            ////-******************************
            List<CommandInfo> list = new List<CommandInfo>();
            tbProductReMarkBll bll = new tbProductReMarkBll();
            foreach (var item in itemList)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@ProductReMarkShopId", SqlDbType.BigInt,8),
                    new SqlParameter("@ProductStyle", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductImg", SqlDbType.NVarChar,500),
                    new SqlParameter("@ProductTaoBaoPrice", SqlDbType.Money,15),
                    new SqlParameter("@ProductYJStock", SqlDbType.Int,4),
                    new SqlParameter("@ProductSJ", SqlDbType.Int,4),
                    new SqlParameter("@ProductSJTime1", SqlDbType.NVarChar,500),
                    new SqlParameter("@ProductXJTime2", SqlDbType.NVarChar,500),
                    new SqlParameter("@ProductState", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductShopName", SqlDbType.NVarChar,50)};
                parameters[0].Value = item.NumIid;  //商品ID
                parameters[1].Value = item.OuterId; //款号
                parameters[2].Value = item.Title; //标题
                parameters[3].Value = item.PicUrl; //图片路径
                parameters[4].Value = item.Price; //淘宝价
                parameters[5].Value = 1; //预警库存
                parameters[6].Value = item.Num; //上架数
                parameters[7].Value = item.ListTime == null ? "" : DateTime.Parse(item.ListTime).ToString("yyyy-MM-dd HH:mm"); //上架时间
                parameters[8].Value = item.DelistTime == null ? "" : DateTime.Parse(item.DelistTime).ToString("yyyy-MM-dd HH:mm"); //下架时间
                parameters[9].Value = item.ApproveStatus; //状态
                parameters[10].Value = item.Nick; //店铺名称


                string sqlText = string.Empty;
                long shopId = item.NumIid;
                int orderNum = bll.GetProductReMarkList(" ProductReMarkId ='" + shopId + "'").Tables[0].Rows.Count;
                if (orderNum <= 0)
                {
                    sqlText = "insert into tbProductReMark(ProductReMarkId,ProductStyle,ProductTitle,ProductImg,ProductTaoBaoPrice,ProductYJStock,ProductSJ,ProductSJTime1,ProductXJTime2,ProductState,ProductShopName) values (@ProductReMarkShopId,@ProductStyle,@ProductTitle,@ProductImg,@ProductTaoBaoPrice,@ProductYJStock,@ProductSJ,@ProductSJTime1,@ProductXJTime2,@ProductState,@ProductShopName)";
                }
                else
                {
                    sqlText = @"update tbProductReMark set 
                                    ProductReMarkId=@ProductReMarkShopId,
                                    ProductStyle=@ProductStyle,
                                    ProductTitle=@ProductTitle,
                                    ProductImg=@ProductImg,
                                    ProductTaoBaoPrice = @ProductTaoBaoPrice,
                                    ProductSJ=@ProductSJ,
                                    ProductSJTime1=@ProductSJTime1,
                                    ProductXJTime2=@ProductXJTime2,
                                    ProductState=@ProductState,
                                    ProductShopName=@ProductShopName
                                where ProductReMarkId=@ProductReMarkShopId";
                }
                CommandInfo c = new CommandInfo(sqlText, parameters, EffentNextType.ExcuteEffectRows);
                list.Add(c);
            }

            s = bll.Add(list) == true ? "更新成功" : "更新失败";

            bll = null;
            list = null;

            return s;
        }
        #endregion



        #region 货品挑选
        public ActionResult CheckProduct()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
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
            // 查询  判断查询权限
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            #region   显示当前登陆人
            ProductTypeBLL ptb = new ProductTypeBLL();
            string userName = userInfo.User.userName;
            DataTable dtPersion = ptb.GetPersonaIdByUserName(userName);//通过当前用户查找当前用户所属角色
            string Name = dtPersion.Rows[0][1].ToString();//用户名
            ViewData["UserName"] = Name;
            #endregion
            #region  显示当前登陆人的店铺
            ViewData["ShopName"] = phb.GetShopByUserDDlist(userInfo.User.Id);
            #endregion
            #region   页码下拉框
            ViewData["PageSum"] = phb.GetPageDDlist();
            ViewData["Type"] = phb.GetTypeDDlist(userInfo.User.Id);
            ViewData["Brand"] = phb.GetBrandDDlist(userInfo.User.Id);
            #endregion
            return View();
        }
        /// <summary>
        /// 显示所有可挑选商品
        /// </summary>
        /// <returns></returns>
        public string ShowCheckProduct()
        {
            PublicHelpController ph = new PublicHelpController();
            ProductStockBLL psb = new ProductStockBLL();
            usersbll usb = new usersbll();

            #region 查询条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件

            string Scode = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//货号
            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string PriceMin = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小价格
            string PriceMax = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大价格
            string Season = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//季节
            string Brand = helpcommon.StrSplit.StrSplitData(orderParamss[5], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//品牌
            string StockMin = helpcommon.StrSplit.StrSplitData(orderParamss[6], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小库存
            string StockMax = helpcommon.StrSplit.StrSplitData(orderParamss[7], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大库存
            string TimeMin = helpcommon.StrSplit.StrSplitData(orderParamss[8], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最小时间
            string TimeMax = helpcommon.StrSplit.StrSplitData(orderParamss[9], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//最大时间
            string Type = helpcommon.StrSplit.StrSplitData(orderParamss[10], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别
            string Imagefiel = helpcommon.StrSplit.StrSplitData(orderParamss[11], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//有图/无图


            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            PriceMax = PriceMax == "\'\'" ? string.Empty : PriceMax;
            PriceMin = PriceMin == "\'\'" ? string.Empty : PriceMin;
            Season = Season == "\'\'" ? string.Empty : Season;
            Brand = Brand == "\'\'" ? string.Empty : Brand;
            StockMax = StockMax == "\'\'" ? string.Empty : StockMax;
            StockMin = StockMin == "\'\'" ? string.Empty : StockMin;
            TimeMin = TimeMin == "\'\'" ? string.Empty : TimeMin;
            TimeMax = TimeMax == "\'\'" ? string.Empty : TimeMax;
            Type = Type == "\'\'" ? string.Empty : Type;
            Imagefiel = Imagefiel == "\'\'" ? string.Empty : Imagefiel;

            dic.Add("Scode", Scode);
            dic.Add("Style", Style);
            dic.Add("PriceMax", PriceMax);
            dic.Add("PriceMin", PriceMin);
            dic.Add("Season", Season);
            dic.Add("Brand", Brand);
            dic.Add("StockMax", StockMax);
            dic.Add("StockMin", StockMin);
            dic.Add("TimeMin", TimeMin);
            dic.Add("TimeMax", TimeMax);
            dic.Add("Type", Type);
            dic.Add("Vencode", "");
            dic.Add("Imagefile", Imagefiel);
            dic.Add("Descript", "");
            dic.Add("ShopId", "");//店铺Id 选货处使用
            dic.Add("isCheckProduct", "true");//是否为选货查询 选货处使用
            dic.Add("CustomerId", userInfo.User.Id.ToString());//用户Id 
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("productstock");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段
            bool Storage = ph.isFunPermisson(roleId, menuId, funName.Storage); //入库权限
            bool Marker = ph.isFunPermisson(roleId, menuId, funName.Marker);//商品标记权限（备注）

            int Count = 0;//数据总个数
            psb.SearchShowProductStock(dic, out Count);
            int PageCount = Count % pageSize > 0 ? Count / pageSize + 1 : Count / pageSize;//数据总页数

            int MinId = pageSize;
            int MaxId = pageSize * (pageIndex - 1);

            DataTable dt = psb.SearchShowProductStock(dic, MinId, MaxId);




            #endregion

            StringBuilder Alltable = new StringBuilder();
            #region 表头排序
            #endregion
            #region Table表头
            Alltable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            Alltable.Append("<th>序号</th>");
            for (int i = 0; i < s.Length; i++)
            {
                if (allTableName.Contains(s[i]))
                {
                    Alltable.Append("<th>");
                    if (s[i] == "Id")
                        Alltable.Append("编号");
                    if (s[i] == "Scode")
                        Alltable.Append("货号");
                    if (s[i] == "Bcode")
                        Alltable.Append("条码1");
                    if (s[i] == "Bcode2")
                        Alltable.Append("条码2");
                    if (s[i] == "Descript")
                        Alltable.Append("英文描述");
                    if (s[i] == "Cdescript")
                        Alltable.Append("中文描述");
                    if (s[i] == "Unit")
                        Alltable.Append("单位");
                    if (s[i] == "Currency")
                        Alltable.Append("货币");
                    if (s[i] == "Cat")
                        Alltable.Append("品牌");
                    if (s[i] == "Cat1")
                        Alltable.Append("季节");
                    if (s[i] == "Cat2")
                        Alltable.Append("类别");
                    if (s[i] == "Clolor")
                        Alltable.Append("颜色");
                    if (s[i] == "Size")
                        Alltable.Append("尺寸");
                    if (s[i] == "Style")
                        Alltable.Append("款号");
                    if (s[i] == "Pricea")
                        Alltable.Append("吊牌价");
                    if (s[i] == "Priceb")
                        Alltable.Append("零售价");
                    if (s[i] == "Pricec")
                        Alltable.Append("供货价");
                    if (s[i] == "Priced")
                        Alltable.Append("批发价");
                    if (s[i] == "Pricee")
                        Alltable.Append("成本价");
                    if (s[i] == "Disca")
                        Alltable.Append("折扣1");
                    if (s[i] == "Discb")
                        Alltable.Append("折扣2");
                    if (s[i] == "Discc")
                        Alltable.Append("折扣3");
                    if (s[i] == "Discd")
                        Alltable.Append("折扣4");
                    if (s[i] == "Disce")
                        Alltable.Append("折扣5");
                    if (s[i] == "Vencode")
                        Alltable.Append("供应商");
                    if (s[i] == "Model")
                        Alltable.Append("型号");
                    if (s[i] == "Rolevel")
                        Alltable.Append("预警库存");
                    if (s[i] == "Roamt")
                        Alltable.Append("最少订货量");
                    if (s[i] == "Stopsales")
                        Alltable.Append("停售库存");
                    if (s[i] == "Loc")
                        Alltable.Append("店铺");
                    if (s[i] == "Balance")
                        Alltable.Append("供应商库存");
                    if (s[i] == "Lastgrnd")
                        Alltable.Append("交货日期");
                    if (s[i] == "Imagefile")
                        Alltable.Append("缩略图");
                    if (s[i] == "UserId")
                        Alltable.Append("操作人");
                    if (s[i] == "PrevStock")
                        Alltable.Append("上一次库存");
                    if (s[i] == "Def2")
                        Alltable.Append("默认2");
                    if (s[i] == "Def3")
                        Alltable.Append("默认3");
                    if (s[i] == "Def4")
                        Alltable.Append("默认4");
                    if (s[i] == "Def5")
                        Alltable.Append("默认5");
                    if (s[i] == "Def6")
                        Alltable.Append("默认6");
                    if (s[i] == "Def7")
                        Alltable.Append("默认7");
                    if (s[i] == "Def8")
                        Alltable.Append("默认8");
                    if (s[i] == "Def9")
                        Alltable.Append("默认9");
                    if (s[i] == "Def10")
                        Alltable.Append("默认10");
                    if (s[i] == "Def11")
                        Alltable.Append("默认11");
                    Alltable.Append("</th>");
                }
            }
            Alltable.Append("<th>店铺</th>");
            Alltable.Append("<th>操作</th>");
            if (ph.isFunPermisson(roleId, menuId, funName.CheckProduct))
            {
                Alltable.Append("<th>选货<input type='checkbox' id='CheckAll' onchange='CheckAll()'></th>");
            }
            Alltable.Append("</tr>");
            #endregion
            #region Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + (i + (pageIndex - 1) * pageSize + 1) + "</td>");
                for (int j = 0; j < s.Length; j++)
                {

                    if (allTableName.Contains(s[j]))
                    {
                        if (s[j] == "Imagefile")
                        {
                            if (dt.Rows[i]["ImagePath"].ToString() == "")
                            {
                                Alltable.Append("<td>");
                                Alltable.Append(dt.Rows[i]["ImagePath"].ToString());
                                Alltable.Append("</td>");
                            }
                            else
                            {

                                Alltable.Append("<td><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["ImagePath"] + "' width='50' height='50'/></td>");
                            }
                        }
                        else if (s[j] == "Vencode")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["sourceName"]);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Cat")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["BrandName"]);
                            Alltable.Append("</td>");
                        }
                        else if (s[j] == "Cat2")
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i]["TypeName"]);
                            Alltable.Append("</td>");
                        }
                        else
                        {
                            Alltable.Append("<td>");
                            Alltable.Append(dt.Rows[i][s[j]]);
                            Alltable.Append("</td>");
                        }
                    }
                }
                DataTable dtShopName = sbl.GetShopByScode(dt.Rows[i]["Scode"].ToString(), userInfo.User.Id.ToString());
                string shopname = "";
                if (dtShopName.Rows.Count > 0)
                {
                    for (int zc = 0; zc < dtShopName.Rows.Count; zc++)
                    {
                        shopname += "," + dtShopName.Rows[zc]["ShopName"].ToString();
                    }
                    shopname = shopname.Trim(',');
                }
                Alltable.Append("<td ><label style='color:blue;'>" + shopname + "</label></td>");
                Alltable.Append("<td><a href='#' onclick='LookAtrrbutt(\"" + dt.Rows[i]["Scode"].ToString().Trim() + "\",\"" + dt.Rows[i]["Vencode"].ToString().Trim() + "\")'>属性</a></td>");
                if (ph.isFunPermisson(roleId, menuId, funName.CheckProduct))
                {
                    Alltable.Append("<td><label style='width:100%;height:100%;display:block;padding-top:15px;'><input type='checkbox' class='Check' Scode='" + dt.Rows[i]["Scode"] + "' Vencode='" + dt.Rows[i]["Vencode"] + "'></label></td>");
                }
                Alltable.Append("</tr>");
            }
            Alltable.Append("</table>");
            #endregion
            #region 分页
            Alltable.Append("-----");
            Alltable.Append(PageCount + "-----" + Count);
            #endregion
            return Alltable.ToString();
        }
        /// <summary>
        /// 保存选货
        /// </summary>
        /// <returns></returns>
        public string SaveCheckProduct()
        {
            ShopProductBll spb = new ShopProductBll();
            ProductStockBLL psb = new ProductStockBLL();
            ProductTypeBLL ptb = new ProductTypeBLL();
            string result = "";//返回成功结果
            string sbResult = "";//返回失败结果
            int resultCount = 0;//成功的数量
            int SbCount = 0;//失败的数量
            string str = Request.Form["Scodes"] == null ? "" : Request.Form["Scodes"].ToString().Trim(',');
            string str1 = Request.Form["Vencodes"] == null ? "" : Request.Form["Vencodes"].ToString().Trim(',');
            string[] Scode = str.Split(',');
            string[] Vencode = str1.Split(',');
            string StrNoInsertScode = "";
            string shopid = Request.Form["ShopId"] == null ? "" : Request.Form["ShopId"].ToString();//得到店铺
            if (Scode.Length > 0)//如果有选中的才做操作
            {
                for (int i = 0; i < Scode.Length; i++)
                {
                    if (spb.DataByScodeAndShopId(Scode[i].ToString(), shopid))//查询当前货号  在当前店铺是否已经分配库存
                    {
                        StrNoInsertScode += "," + Scode[i].ToString();
                        sbResult = "当前货品已存在您的店铺当中：" + StrNoInsertScode;
                    }
                    else //为分配库存则添加
                    {
                        DataTable dt = psb.SelectDataByScodeAndVencode(Vencode[i].ToString(), Scode[i].ToString());
                        dt.Columns.Add("UpdateDatetime");
                        dt.Columns.Add("Balance1");
                        dt.Columns.Add("UserId");
                        dt.Columns.Add("ShopId");
                        dt.Columns.Add("SaleState");
                        dt.Rows[0]["UpdateDatetime"] = DateTime.Now.ToString();
                        string UserId = userInfo.User.Id.ToString();//当前角色的Id
                        dt.Rows[0]["UserId"] = int.Parse(UserId);
                        dt.Rows[0]["ShopId"] = shopid;
                        dt.Rows[0]["SaleState"] = 2;//销售状态  默认为 不开放销售   0  未开放  1 开放 2  选货状态
                        dt.Rows[0]["Balance1"] = 0;
                        if (spb.InsertBalance(dt))
                        {
                            resultCount++;
                            result = "有" + resultCount + "条货品选货成功！";
                        }
                        else
                        {
                            SbCount++;
                            result = "有" + SbCount + "条货品选货失败！";
                        }
                    }
                }
            }
            else
            {
                result = "您还未选中货品！";
            }
            return result + sbResult;
        }
        /// <summary>
        /// 查看属性   ---现货挑选
        /// </summary>
        /// <returns></returns>
        public string LookAttrByCheck()
        {
            string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
            string scode = Request.Form["scode"] == null ? "" : Request.Form["scode"].ToString();
            string shopid = Request.Form["shopid"] == null ? "" : Request.Form["shopid"].ToString().Trim();
            int menuId = int.Parse(Request.Form["MenuId"].ToString());
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            string RoleType = sbl.GetUserTypeByPersonaId(roleId.ToString());//是否是合作客户
            string showState = sbl.GetShowStateByScodeAndLoc(scode, shopid);//得到当前货号的状态
            DataTable dt = sbl.LookAttrByCheck(scode, vencode);
            string property = GetTypeNo(scode, dt.Rows[0]["TypeNo"].ToString());
            string s = string.Format(@"<table id='tabProperty'>
            <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><img src='{11}' style='width:120px;' /></td></tr>
            <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' disabled='disabled' type='text' value='{2}' /></td></tr>
            <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                <td>季节：<input type='text' value='{4}' disabled='disabled' /></td></tr>
            <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
            <tr><td>预警库存：<input id='txtRolevelE' disabled='disabled' type='text' value='{9}' /></td><td>类别：<input type='text' disabled='disabled' id='hidId'' value='{5}' /></td></tr>
        </table>", dt.Rows[0]["Scode"].ToString()
                             , dt.Rows[0]["Descript"].ToString()
                             , dt.Rows[0]["Cdescript"].ToString()
                             , dt.Rows[0]["Cat"].ToString()
                             , dt.Rows[0]["Cat1"].ToString()
                             , dt.Rows[0]["Cat2"].ToString()
                             , dt.Rows[0]["Clolor"].ToString()
                             , dt.Rows[0]["Style"].ToString()
                             , dt.Rows[0]["Size"].ToString()
                             , dt.Rows[0]["Rolevel"].ToString()
                             , dt.Rows[0]["Id"].ToString()
                             , dt.Rows[0]["ImagePath"].ToString()
                        );
            StringBuilder alltableprice = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            string[] Spermission = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            alltableprice.Append("<table class='mytable' style='font-size:12px;min-width:300px;'>");
            alltableprice.Append("<tr>");
            alltableprice.Append("<th>来源</th>");
            if (Spermission.Contains("Pricea"))
            {
                alltableprice.Append("<th>吊牌价</th>");
            }
            if (Spermission.Contains("Priceb"))
            {
                alltableprice.Append("<th>零售价</th>");
            }
            if (Spermission.Contains("Pricec"))
            {
                alltableprice.Append("<th>供货价</th>");
            }
            if (Spermission.Contains("Priced"))
            {
                alltableprice.Append("<th>批发价</th>");
            }
            if (Spermission.Contains("Pricee"))
            {
                alltableprice.Append("<th>成本价</th>");
            }
            alltableprice.Append("<th>库存</th>");
            alltableprice.Append("</tr>");
            alltableprice.Append("<tr>");
            alltableprice.Append("<td>" + dt.Rows[0]["sourceName"] + "</td>");
            if (Spermission.Contains("Pricea"))
            {
                alltableprice.Append("<td>");
                alltableprice.Append(dt.Rows[0]["Pricea"]);
                alltableprice.Append("</td>");

            }
            if (Spermission.Contains("Priceb"))
            {
                if (showState == "1" || RoleType != "Customer")
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Priceb"]);
                    alltableprice.Append("</td>");
                }
                else
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append("*****");
                    alltableprice.Append("</td>");
                }
            }
            if (Spermission.Contains("Pricec"))
            {
                if (showState == "1" || RoleType != "Customer")
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Pricec"]);
                    alltableprice.Append("</td>");
                }
                else
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append("*****");
                    alltableprice.Append("</td>");
                }
            }
            if (Spermission.Contains("Priced"))
            {
                if (showState == "1" || RoleType != "Customer")
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Priced"]);
                    alltableprice.Append("</td>");
                }
                else
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append("*****");
                    alltableprice.Append("</td>");
                }
            }
            if (Spermission.Contains("Pricee"))
            {
                if (showState == "1" || RoleType != "Customer")
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Pricee"]);
                    alltableprice.Append("</td>");
                }
                else
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append("*****");
                    alltableprice.Append("</td>");
                }
            }
            if (showState == "1" || RoleType != "Customer")
            {
                if (Spermission.Contains("Balance"))
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append(dt.Rows[0]["Balance"]);
                    alltableprice.Append("</td>");
                }
                else
                {
                    alltableprice.Append("<td>");
                    alltableprice.Append("*****");
                    alltableprice.Append("</td>");
                }
            }
            else
            {
                alltableprice.Append("<td>");
                alltableprice.Append("*****");
                alltableprice.Append("</td>");
            }
            alltableprice.Append("</tr>");
            alltableprice.Append("</table>");
            return s.ToString() + "❤" + alltableprice.ToString() + "❤" + property;
        }
        #endregion



        #region 修改淘宝数据
        public static string remarkId;
        public static string Style;
        public static int TbMenuId;
        public ActionResult TaoBaoUpdate()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
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
            TbMenuId = menuId;
            remarkId = Request.QueryString["RemarkId"] == null ? "" : Request.QueryString["RemarkId"].ToString();
            Style = Request.QueryString["Style"] == null ? "" : Request.QueryString["Style"].ToString();
            DataTable dt = sbl.GetTbProductByRemarkId(remarkId);
            ViewBag.Title = dt.Rows[0]["ProductTitle"].ToString();
            return View();
        }
        /// <summary>
        /// 更新淘宝数据
        /// </summary>
        /// <returns></returns>
        public static List<Top.Api.Domain.Sku> Sku = new List<Top.Api.Domain.Sku>();
        public static DataTable dtProduct = new DataTable();
        public static string sessionkey;
        public string TaoBaoUpdateShow()
        {

            productSales taoapp = new productSales();
            DataTable dt = sbl.GetTbProductByRemarkId(remarkId);
            if (dt.Rows[0]["ProductShopName"].ToString() == "攀比轩旗舰店")
            {
                taoapp.Sessionkey = "61007278ecd41be4ead2e3859a0f4169a1d18d3002122dc1969004137";
                sessionkey = "61007278ecd41be4ead2e3859a0f4169a1d18d3002122dc1969004137";
            }
            else
            {
                taoapp.Sessionkey = "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799";
                sessionkey = "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799";
            }
            long nid = long.Parse(remarkId.ToString());
            Item it = taoapp.GetItem(nid);
            Sku = it.Skus;
            dtProduct = sbl.GetProductByTbStyle(Style, "");
            StringBuilder Alltable = new StringBuilder();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            PublicHelpController ph = new PublicHelpController();
            #region    淘宝数据
            Alltable.Append("<table id='mytable' class='mytable' style='font-size:12px;'><tr>");
            Alltable.Append("<th>商品SKUID</th>");
            Alltable.Append("<th>商品图片</th>");
            Alltable.Append("<th>货号</th>");
            Alltable.Append("<th>颜色分类</th>");
            Alltable.Append("<th>库存</th>");
            Alltable.Append("<th style='background-color:#80FFFF;'>系统库存</th>");
            Alltable.Append("<th>价格</th>");
            Alltable.Append("<th>创建时间</th>");
            Alltable.Append("<th>修改</th>");
            Alltable.Append("</tr>");
            string[] scodes = new string[Sku.Count];
            for (int i = 0; i < Sku.Count; i++)
            {
                scodes[i] = Sku[i].OuterId;
                Alltable.Append("<tr>");
                Alltable.Append("<td>" + Sku[i].SkuId + "</td>");
                if (it.PropImgs.Count > 0)
                {
                    if (i < it.PropImgs.Count)
                    {
                        string imge = it.PropImgs[i].Url == null ? "" : it.PropImgs[i].Url.ToString();
                        if (imge != "")
                        {
                            Alltable.Append("<td><img src='" + imge + "'/ style='width:50px;height:50px;' ></td>");
                        }
                        else
                        {
                            Alltable.Append("<td></td>");
                        }
                    }
                    else
                    {
                        Alltable.Append("<td></td>");
                    }
                }
                else
                {
                    Alltable.Append("<td></td>");
                }
                Alltable.Append("<td>" + Sku[i].OuterId + "</td>");
                Alltable.Append("<td>" + Sku[i].PropertiesName.Split(':')[3] + "</td>");
                Alltable.Append("<td>" + Sku[i].Quantity + "</td>");
                string Balance = "";
                for (int j = 0; j < dtProduct.Rows.Count; j++)
                {
                    string OuterId = Sku[i].OuterId == null ? "" : Sku[i].OuterId.ToString();
                    if (dtProduct.Rows[j]["Scode"].ToString() == OuterId)
                    {
                        Balance = dtProduct.Rows[j]["Balance"].ToString();
                    }
                }
                Alltable.Append("<td style='background-color:#80FFFF;font-weight:bold;'>" + Balance + "</td>");
                Alltable.Append("<td>" + Sku[i].Price + "</td>");
                Alltable.Append("<td>" + Sku[i].Created + "</td>");

                Alltable.Append("<td>");
                if (!ph.isFunPermisson(roleId, TbMenuId, funName.UpdateBalanceTb))
                {
                    Alltable.Append("<a href='#' onclick='Balance(\"" + Sku[i].SkuId + "\")'>库存</a>丨丨");
                }
                if (!ph.isFunPermisson(roleId, TbMenuId, funName.UpdatePriceTb))
                {
                    Alltable.Append("<a href='#' onclick='Price(\"" + Sku[i].SkuId + "\")'>价格</a>丨丨");
                }
                if (!ph.isFunPermisson(roleId, TbMenuId, funName.UpdateScodeTb))
                {
                    Alltable.Append("<a href='#' onclick='Scode(\"" + Sku[i].SkuId + "\")'>货号</a>");
                }
                Alltable.Append("</td>");
                Alltable.Append("</tr>");
            }
            Alltable.Append("</table>");
            #endregion
            #region  服务器数据
            StringBuilder ProductTable = new StringBuilder();
            ProductTable.Append("<table id='mytable' class='mytable' style='font-size:12px;'><tr>");
            ProductTable.Append("<th>货号</th>");
            ProductTable.Append("<th>商品图片</th>");
            ProductTable.Append("<th>颜色</th>");
            ProductTable.Append("<th>颜色分类</th>");
            ProductTable.Append("<th>尺码</th>");
            ProductTable.Append("<th>系统库存</th>");
            ProductTable.Append("<th>价格</th>");
            ProductTable.Append("<th>操作</th>");
            ProductTable.Append("</tr>");
            if (dtProduct.Rows.Count > 0)
            {
                string colorcode = dtProduct.Rows[0]["Def1"] == null ? "50012010" : dtProduct.Rows[0]["Def1"].ToString();
                colorcode = colorcode == "" ? "50012010" : colorcode;
                string json = JsonConvert.SerializeObject(taoapp.ItempropsGetRequest(long.Parse(colorcode)));
                JObject jsonObj = JObject.Parse(json);
                string color = GetColorlist(jsonObj);
                for (int i = 0; i < dtProduct.Rows.Count; i++)
                {
                    ProductTable.Append("<tr>");
                    ProductTable.Append("<td>" + dtProduct.Rows[i]["Scode"] + "</td>");
                    if (dtProduct.Rows[i]["ImagePath"].ToString() != "")
                    {
                        ProductTable.Append("<td><img src='" + dtProduct.Rows[i]["ImagePath"] + "'/ style='width:50px;height:50px;' ></td>");
                    }
                    else
                    {
                        ProductTable.Append("<td></td>");
                    }
                    ProductTable.Append("<td id=\"" + dtProduct.Rows[i]["Scode"] + "\">" + color + "</td>");
                    ProductTable.Append("<td>" + dtProduct.Rows[i]["Clolor"] + "</td>");
                    ProductTable.Append("<td>" + dtProduct.Rows[i]["Size"] + "</td>");
                    ProductTable.Append("<td>" + dtProduct.Rows[i]["Balance"] + "</td>");
                    ProductTable.Append("<td>" + dtProduct.Rows[i]["Pricea"] + "</td>");
                    if (scodes.Contains(dtProduct.Rows[i]["Scode"]))
                    {
                        ProductTable.Append("<td>已上架</td>");
                    }
                    else
                    {
                        if (!ph.isFunPermisson(roleId, TbMenuId, funName.HeadBlock))
                        {
                            ProductTable.Append("<td><a href='#' onclick='HeadBlock(\"" + dtProduct.Rows[i]["Scode"] + "\")'>上架</a></td>");
                        }
                        else
                        {
                            ProductTable.Append("<td></td>");
                        }
                    }
                    ProductTable.Append("</tr>");
                }
            }
            #endregion
            return Alltable.ToString() + "❤" + ProductTable;
        }
        /// <summary>
        /// 修改库存
        /// </summary>
        /// <returns></returns>
        public string TaoBaoBalance()
        {
            string skuId = Request.Form["SkuId"] == null ? "" : Request.Form["SkuId"].ToString();
            string balance = "";
            string scode = "";
            for (int i = 0; i < Sku.Count; i++)
            {
                if (Sku[i].SkuId.ToString() == skuId)
                {
                    balance = Sku[i].Quantity.ToString();
                    scode = Sku[i].OuterId == null ? "" : Sku[i].OuterId.ToString();
                }
            }
            return scode + "❤" + balance;
        }
        /// <summary>
        /// 确认修改库存
        /// </summary>
        public string UpdateBalance()
        {
            Top.Api.Domain.Sku skuModel = new Top.Api.Domain.Sku();
            string skuId = Request.Form["SkuId"] == null ? "" : Request.Form["SkuId"].ToString();
            long Balance = Request.Form["Balance"] == null ? long.Parse("") : long.Parse(Request.Form["Balance"].ToString());
            for (int i = 0; i < Sku.Count; i++)
            {
                if (Sku[i].SkuId.ToString() == skuId)
                {
                    skuModel = Sku[i];
                }
            }
            productSales ps = new productSales();
            ps.Sessionkey = sessionkey;
            if (ps.UpdateTao(skuId, Balance, long.Parse(remarkId), skuModel.OuterId, skuModel.Price, skuModel.Properties))
            {
                return "修改成功！";
            }
            else
            {
                return "修改失败！";
            }
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <returns></returns>
        public string TaoBaoPrice()
        {
            string skuId = Request.Form["SkuId"] == null ? "" : Request.Form["SkuId"].ToString();
            string Price = "";
            string scode = "";
            for (int i = 0; i < Sku.Count; i++)
            {
                if (Sku[i].SkuId.ToString() == skuId)
                {
                    Price = Sku[i].Price.ToString();
                    scode = Sku[i].OuterId == null ? "" : Sku[i].OuterId.ToString();
                }
            }
            return scode + "❤" + Price;
        }
        /// <summary>
        /// 确认修改价格
        /// </summary>
        public string UpdatePrice()
        {
            Top.Api.Domain.Sku skuModel = new Top.Api.Domain.Sku();
            string skuId = Request.Form["SkuId"] == null ? "" : Request.Form["SkuId"].ToString();
            string Price = Request.Form["Price"] == null ? "" : Request.Form["Price"].ToString();
            for (int i = 0; i < Sku.Count; i++)
            {
                if (Sku[i].SkuId.ToString() == skuId)
                {
                    skuModel = Sku[i];
                }
            }
            productSales ps = new productSales();
            ps.Sessionkey = sessionkey;
            if (ps.UpdateTao(skuId, skuModel.Quantity, long.Parse(remarkId), skuModel.OuterId, Price, skuModel.Properties))
            {
                return "修改成功！";
            }
            else
            {
                return "修改失败！";
            }
        }
        /// <summary>
        /// 修改货号
        /// </summary>
        /// <returns></returns>
        public string TaoBaoScode()
        {
            string skuId = Request.Form["SkuId"] == null ? "" : Request.Form["SkuId"].ToString();
            string scode = "";
            for (int i = 0; i < Sku.Count; i++)
            {
                if (Sku[i].SkuId.ToString() == skuId)
                {
                    scode = Sku[i].OuterId == null ? "" : Sku[i].OuterId.ToString();
                }
            }
            return scode;
        }
        /// <summary>
        /// 确认修改货号
        /// </summary>
        public string UpdateScode()
        {
            Top.Api.Domain.Sku skuModel = new Top.Api.Domain.Sku();
            string skuId = Request.Form["SkuId"] == null ? "" : Request.Form["SkuId"].ToString();
            string Scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            for (int i = 0; i < Sku.Count; i++)
            {
                if (Sku[i].SkuId.ToString() == skuId)
                {
                    skuModel = Sku[i];
                }
            }
            productSales ps = new productSales();
            ps.Sessionkey = sessionkey;
            if (ps.UpdateTao(skuId, skuModel.Quantity, long.Parse(remarkId), Scode, skuModel.Price, skuModel.Properties))
            {
                return "修改成功！";
            }
            else
            {
                return "修改失败！";
            }
        }
        /// <summary>
        /// 上架
        /// </summary>
        /// <returns></returns>
        public string HeadBlock()
        {
            string color = Request.Form["Color"] == null ? "" : Request.Form["Color"].ToString();
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            productSales ps = new productSales();
            ps.Sessionkey = sessionkey;
            long Balance = 0;
            string price = "";
            for (int i = 0; i < dtProduct.Rows.Count; i++)
            {
                if (dtProduct.Rows[i]["Scode"].ToString() == scode)
                {
                    Balance = long.Parse(dtProduct.Rows[i]["Balance"].ToString());
                    price = dtProduct.Rows[i]["Pricea"].ToString();
                }
            }
            return ps.ItemSkuAddRequest(long.Parse(remarkId), color, Balance, price, scode);
        }
        /// <summary>
        /// 获取淘宝颜色
        /// </summary>
        public string GetColorlist(JObject jsonObj)
        {
            string s = "";
            s += "<select class='selColor'>";
            int a = jsonObj["ItemProps"].Count();
            for (int i = 0; i < jsonObj["ItemProps"].Count(); i++)
            {
                if (jsonObj["ItemProps"][i]["Pid"].ToString() == "1627207")
                {
                    for (int j = 0; j < jsonObj["ItemProps"][i]["PropValues"].Count(); j++)
                    {
                        s += "<option value='" + jsonObj["ItemProps"][i]["PropValues"][j]["Vid"] + "'>" + jsonObj["ItemProps"][i]["PropValues"][j]["Name"] + "</option>";
                    }
                    break;
                }
            }
            s += "</select>";
            return s;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public string SearchByTbStyle()
        {
            string Style = Request.Form["Style"] == null ? "" : Request.Form["Style"].ToString();
            string Scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            shopbll sbl = new shopbll();
            DataTable dt = sbl.GetProductByTbStyle(Style, Scode);
            StringBuilder ProductTable = new StringBuilder();
            ProductTable.Append("<table id='mytable' class='mytable' style='font-size:12px;'><tr>");
            ProductTable.Append("<th>货号</th>");
            ProductTable.Append("<th>商品图片</th>");
            ProductTable.Append("<th>颜色</th>");
            ProductTable.Append("<th>颜色分类</th>");
            ProductTable.Append("<th>尺码</th>");
            ProductTable.Append("<th>系统库存</th>");
            ProductTable.Append("<th>价格</th>");
            ProductTable.Append("<th>操作</th>");
            ProductTable.Append("</tr>");
            string[] scodes = new string[Sku.Count];
            for (int i = 0; i < Sku.Count; i++)
            {
                scodes[i] = Sku[i].OuterId;
            }
            productSales taoapp = new productSales();
            taoapp.Sessionkey = sessionkey;
            if (dt.Rows.Count > 0)
            {
                string json = JsonConvert.SerializeObject(taoapp.ItempropsGetRequest(long.Parse(dt.Rows[0]["Def1"].ToString())));
                JObject jsonObj = JObject.Parse(json);
                string color = GetColorlist(jsonObj);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ProductTable.Append("<tr>");
                    ProductTable.Append("<td>" + dt.Rows[i]["Scode"] + "</td>");
                    if (dt.Rows[i]["ImagePath"].ToString() != "")
                    {
                        ProductTable.Append("<td><img src='" + dt.Rows[i]["ImagePath"] + "'/ style='width:50px;height:50px;' ></td>");
                    }
                    else
                    {
                        ProductTable.Append("<td></td>");
                    }
                    ProductTable.Append("<td id=\"" + dt.Rows[i]["Scode"] + "\">" + color + "</td>");
                    ProductTable.Append("<td>" + dt.Rows[i]["Clolor"] + "</td>");
                    ProductTable.Append("<td>" + dt.Rows[i]["Size"] + "</td>");
                    ProductTable.Append("<td>" + dt.Rows[i]["Balance"] + "</td>");
                    ProductTable.Append("<td>" + dt.Rows[i]["Pricea"] + "</td>");
                    if (scodes.Contains(dt.Rows[i]["Scode"]))
                    {
                        ProductTable.Append("<td>已上架</td>");
                    }
                    else
                    {
                        ProductTable.Append("<td><a href='#' onclick='HeadBlock(\"" + dt.Rows[i]["Scode"] + "\")'>上架</a></td>");
                    }
                    ProductTable.Append("</tr>");
                }
            }
            return ProductTable.ToString();
        }
        /// <summary>
        /// 修改完成后进行单条更新
        /// </summary>
        public string OneUpdate()
        {
            string s = string.Empty;

            HttpCookie cookie = Request.Cookies["userInfo"];
            if (cookie == null)
            {
                return null;
            }
            productSales tmallapp = new productSales();
            List<Item> itemList = new List<Item>();
            tmallapp.Sessionkey = sessionkey;
            shopbll sbl = new shopbll();
            DataTable dt = sbl.GetTbProductByRemarkId(remarkId);
            #region 获取上架商品
            int temp = tmallapp.GetTaoShopOnSaleItems(dt.Rows[0]["ProductTitle"].ToString());    //攀比轩店铺  444件    //弗罗伦斯店铺   656 
            if (temp > 0)
            {
                int myOnSaleNum = temp % 200;
                int myOnSaleNum1 = temp / 200;
                if (temp % 200 != 0)
                {
                    myOnSaleNum1 += 1;
                }

                for (int m = 1; m <= myOnSaleNum1; m++)
                {
                    itemList.AddRange(tmallapp.GetTaoShopOnSaleItems(m, 200, dt.Rows[0]["ProductTitle"].ToString()));
                }
            }
            #endregion
            #region 获取仓库商品
            int temp1 = tmallapp.GetInventoryTaoShop(dt.Rows[0]["ProductTitle"].ToString());
            if (temp1 > 0)
            {
                int myOnSaleNum = temp1 % 200;
                int myOnSaleNum1 = temp1 / 200;
                if (temp1 % 200 != 0)
                {
                    myOnSaleNum1 += 1;
                }

                for (int m = 1; m <= myOnSaleNum1; m++)
                {
                    itemList.AddRange(tmallapp.GetInventoryTaoShop(m, 200, dt.Rows[0]["ProductTitle"].ToString()));
                }

            }
            #endregion
            for (int j = 0; j < itemList.Count; j++)
            {
                string style = ParmPerportys.GetStrParms(itemList[j].OuterId);
                string[] BrandCat = StrSplit.StrSplitData(GetProductBrandCat2(style), ',');
                itemList[j].InputPids = BrandCat[0];  //品牌
                itemList[j].InputStr = BrandCat[1];  //类别
            }
            List<CommandInfo> list = new List<CommandInfo>();
            tbProductReMarkBll bll = new tbProductReMarkBll();
            foreach (var item in itemList)
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@ProductReMarkShopId", SqlDbType.BigInt,8),
                    new SqlParameter("@ProductStyle", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductImg", SqlDbType.NVarChar,500),
                    new SqlParameter("@ProductTaoBaoPrice", SqlDbType.Money,8),
                    new SqlParameter("@ProductYJStock", SqlDbType.Int,4),
                    new SqlParameter("@ProductSJ", SqlDbType.Int,4),
                    new SqlParameter("@ProductSJTime1", SqlDbType.NVarChar,500),
                    new SqlParameter("@ProductXJTime2", SqlDbType.NVarChar,500),
                    new SqlParameter("@ProductState", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductShopName", SqlDbType.NVarChar,50)};
                parameters[0].Value = item.NumIid;  //商品ID
                parameters[1].Value = item.OuterId; //款号
                parameters[2].Value = item.Title; //标题
                parameters[3].Value = item.PicUrl; //图片路径
                parameters[4].Value = item.Price; //淘宝价
                parameters[5].Value = 1; //预警库存
                parameters[6].Value = item.Num; //上架数
                parameters[7].Value = item.ListTime == null ? "" : DateTime.Parse(item.ListTime).ToString("yyyy-MM-dd HH:mm"); //上架时间
                parameters[8].Value = item.DelistTime == null ? "" : DateTime.Parse(item.DelistTime).ToString("yyyy-MM-dd HH:mm"); //下架时间
                parameters[9].Value = item.ApproveStatus; //状态
                parameters[10].Value = item.Nick; //店铺名称


                string sqlText = string.Empty;
                long shopId = item.NumIid;
                int orderNum = bll.GetProductReMarkList(" ProductReMarkId ='" + shopId + "'").Tables[0].Rows.Count;
                if (orderNum <= 0)
                {
                    sqlText = "insert into tbProductReMark(ProductReMarkId,ProductStyle,ProductTitle,ProductImg,ProductTaoBaoPrice,ProductYJStock,ProductSJ,ProductSJTime1,ProductXJTime2,ProductState,ProductShopName) values (@ProductReMarkShopId,@ProductStyle,@ProductTitle,@ProductImg,@ProductTaoBaoPrice,@ProductYJStock,@ProductSJ,@ProductSJTime1,@ProductXJTime2,@ProductState,@ProductShopName)";
                }
                else
                {
                    sqlText = @"update tbProductReMark set 
                                    ProductReMarkId=@ProductReMarkShopId,
                                    ProductStyle=@ProductStyle,
                                    ProductTitle=@ProductTitle,
                                    ProductImg=@ProductImg,
                                    ProductTaoBaoPrice = @ProductTaoBaoPrice,
                                    ProductSJ=@ProductSJ,
                                    ProductSJTime1=@ProductSJTime1,
                                    ProductXJTime2=@ProductXJTime2,
                                    ProductState=@ProductState,
                                    ProductShopName=@ProductShopName
                                where ProductReMarkId=@ProductReMarkShopId";
                }
                CommandInfo c = new CommandInfo(sqlText, parameters, EffentNextType.ExcuteEffectRows);
                list.Add(c);
            }
            s = bll.Add(list) == true ? "更新成功" : "更新失败";
            bll = null;
            list = null;
            return s;
        }
        /// <summary>
        /// 得到某个货号的图
        /// </summary>
        /// <returns></returns>
        public string GetImageFile()
        {
            ProductStockBLL psd = new ProductStockBLL();
            string scode = Request.Form["Scode"] == null ? "" : Request.Form["Scode"].ToString();
            List<model.scodepic> list = psd.GetImageFile(scode);
            string str = "";
            str = "<ul id='ImageUl'>";
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str += "<li><img src=\"" + list[i].scodePicSrc + "\" /><div><a href=\"" + list[i].scodePicSrc + "\" target='_blank'>下载</a></div></li>";
                }
            }
            else
            {
                str = "暂无图片";
            }
            str += "</ul>";
            return str;
        }
        /// <summary>
        /// 得到款号的图
        /// </summary>
        /// <returns></returns>
        public string GetStyleImageFile()
        {
            ProductStockBLL psd = new ProductStockBLL();
            string scode = Request.Form["Style"] == null ? "" : Request.Form["Style"].ToString();
            List<model.stylepic> list = psd.GetStyleImageFile(scode);
            string str = "";
            str = "<ul id='ImageUl'>";
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    str += "<li><img src=\"" + list[i].stylePicSrc + "\" /><div><a href=\"" + list[i].stylePicSrc + "\" target='_blank'>下载</a></div></li>";
                }
            }

            else
            {
                str = "暂无图片";
            }
            str += "</ul>";
            return str;
        }
        #endregion



        #region 淘宝对应SKU
        /// <summary>
        /// 淘宝对应SKU货号
        /// </summary>
        /// <returns></returns>
        public static int SkuMenuId;
        public ActionResult TbSkuScode()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]);
            ViewData["myMenuId"] = menuId;
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
            #region   页码下拉框
            ViewData["PageSum"] = phb.GetPageDDlist();
            #endregion
            SkuMenuId = menuId;
            return View();
        }
        public string LoadSku()
        {
            List<Top.Api.Domain.Sku> tbSku = new List<Top.Api.Domain.Sku>();
            shopbll sbl = new shopbll();
            List<string> listProductRemarkId = sbl.GetTbProductRemarkId();
            productSales taoapp = new productSales();
            bool result = true;
            for (int i = 0; i < listProductRemarkId.Count; i++)
            {
                if (result == true)
                {
                    DataTable dt = sbl.GetTbProductByRemarkId(listProductRemarkId[i]);
                    if (dt.Rows[0]["ProductShopName"].ToString() == "攀比轩旗舰店")
                    {
                        taoapp.Sessionkey = "61007278ecd41be4ead2e3859a0f4169a1d18d3002122dc1969004137";
                        sessionkey = "61007278ecd41be4ead2e3859a0f4169a1d18d3002122dc1969004137";
                    }
                    else
                    {
                        taoapp.Sessionkey = "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799";
                        sessionkey = "61018063b06cee193a796d291600c416f5e73b4d565b01f1954282799";
                    }
                    long nid = long.Parse(listProductRemarkId[i]);
                    Item it = taoapp.GetItem(nid);
                    string imgefile = "";
                    for (int j = 0; j < it.PropImgs.Count; j++)
                    {
                        if (j < it.PropImgs.Count)
                        {
                            string imge = it.PropImgs[j].Url == null ? "" : it.PropImgs[j].Url.ToString();
                            if (imge != "")
                            {
                                imgefile = imge;
                            }
                            else
                            {
                                imgefile = "";
                            }
                        }
                        else
                        {
                            imgefile = "";
                        }
                    }
                    tbSku = it.Skus;
                    for (int j = 0; j < tbSku.Count; j++)
                    {
                        model.TbSkuScode tb = new TbSkuScode()
                        {
                            TbStyle = dt.Rows[0]["ProductStyle"].ToString(),
                            SaleStatus = it.ApproveStatus,
                            Scode = tbSku[j].OuterId,
                            Color = tbSku[j].PropertiesName.Split(':')[3],
                            TbImage = imgefile,
                            TbRemarkId = listProductRemarkId[i],
                            TbSkuId = tbSku[j].SkuId.ToString(),
                            CreateTime = tbSku[j].Created,
                            Price = tbSku[j].Price,
                            Balance = tbSku[j].Quantity.ToString()

                        };
                        result = sbl.InserttbSkuScode(tb);
                    }
                }
                else
                {
                    return "更新中断！";
                }
            }
            return "";
        }
        /// <summary>
        /// 淘宝商品SKUiD
        /// </summary>
        /// <returns></returns>
        public string selectSkuScode()
        {
            shopbll sbl = new shopbll();
            usersbll usb = new usersbll();
            PublicHelpController ph = new PublicHelpController();

            #region 查询条件
            string orderParams = Request.Form["params"] ?? string.Empty; //参数
            string[] orderParamss = helpcommon.StrSplit.StrSplitData(orderParams, ','); //参数集合
            Dictionary<string, string> dic = new Dictionary<string, string>();//搜索条件

            string Style = helpcommon.StrSplit.StrSplitData(orderParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//款号
            string Scode = helpcommon.StrSplit.StrSplitData(orderParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//货号
            string tbRemarkId = helpcommon.StrSplit.StrSplitData(orderParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//商品编号
            string SkuId = helpcommon.StrSplit.StrSplitData(orderParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//SKU编号
            string SaleStatus = helpcommon.StrSplit.StrSplitData(orderParamss[4], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//销售状态

            Scode = Scode == "\'\'" ? string.Empty : Scode;
            Style = Style == "\'\'" ? string.Empty : Style;
            tbRemarkId = tbRemarkId == "\'\'" ? string.Empty : tbRemarkId;
            SkuId = SkuId == "\'\'" ? string.Empty : SkuId;
            SaleStatus = SaleStatus == "\'\'" ? string.Empty : SaleStatus;

            string[] str = new string[5];
            str[0] = Style;
            str[1] = Scode;
            str[2] = tbRemarkId;
            str[3] = SkuId;
            str[4] = SaleStatus;

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            int menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]); //菜单ID

            int pageIndex = Request.Form["pageIndex"] == null ? 0 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = Request.Form["pageSize"] == null ? 10 : helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] allTableName = usb.getDataName("TbSkuScode");//当前列表所有字段
            string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);//获得当前权限字段

            int Count = sbl.SelectSkuId(str);//数据总个数
            int PageCount = Count % pageSize > 0 ? Count / pageSize + 1 : Count / pageSize;//数据总页数

            int MinId = pageSize * (pageIndex - 1);
            int MaxId = pageSize * (pageIndex - 1) + pageSize;

            DataTable dt = sbl.SelectSkuId(str, MinId, MaxId);

            StringBuilder allTable = new StringBuilder();
            #endregion

            #region Table表头
            allTable.Append("<table id='StcokTable' class='mytable' rules='all'><tr style='text-align:center;'>");
            allTable.Append("<tr>");
            allTable.Append("<th>序号</th>");
            for (int i = 0; i < allTableName.Length; i++)
            {


                if (s.Contains(allTableName[i]))
                {
                    allTable.Append("<th>");
                    if (allTableName[i] == "Id")
                        allTable.Append("编号");
                    if (allTableName[i] == "TbRemarkId")
                        allTable.Append("淘宝商品编号");
                    if (allTableName[i] == "TbStyle")
                        allTable.Append("淘宝商品款号");
                    if (allTableName[i] == "TbSkuId")
                        allTable.Append("商品SKUID");
                    if (allTableName[i] == "TbImage")
                        allTable.Append("商品图片");
                    if (allTableName[i] == "Scode")
                        allTable.Append("货号");
                    if (allTableName[i] == "Color")
                        allTable.Append("颜色");
                    if (allTableName[i] == "Balance")
                        allTable.Append("库存");
                    if (allTableName[i] == "Price")
                        allTable.Append("价格");
                    if (allTableName[i] == "SaleStatus")
                        allTable.Append("销售状态");
                    if (allTableName[i] == "CreateTime")
                        allTable.Append("上架时间");
                    if (allTableName[i] == "Def1")
                        allTable.Append("默认1");
                    if (allTableName[i] == "Def2")
                        allTable.Append("默认2");
                    if (allTableName[i] == "Def3")
                        allTable.Append("默认3");
                    if (allTableName[i] == "Def4")
                        allTable.Append("默认4");
                    if (allTableName[i] == "Def5")
                        allTable.Append("默认5");
                    if (allTableName[i] == "Def6")
                        allTable.Append("默认6");
                    allTable.Append("</th>");
                }

            }
            allTable.Append("</tr>");
            #endregion

            #region Table内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                allTable.Append("<tr>");
                allTable.Append("<td>" + dt.Rows[i]["rowId"] + "</td>");
                for (int j = 0; j < allTableName.Length; j++)
                {
                    if (s.Contains(allTableName[j]))
                    {

                        if (allTableName[j] == "TbImage")
                        {
                            if (dt.Rows[i]["TbImage"].Equals(""))
                            {
                                allTable.Append("<td></td>");
                            }
                            else
                            {
                                allTable.Append("<td ><img onmouseover=showPic(this) onmouseout='HidePic()' src='" + dt.Rows[i]["TbImage"] + "' width='50' height='50'/></td>");
                            }
                        }
                        else if (allTableName[j] == "SaleStatus")
                        {
                            string states = dt.Rows[i]["SaleStatus"].ToString() == "onsale" ? "销售中" : "仓库中";
                            allTable.Append("<td>" + states + "</td>");
                        }
                        else
                        {
                            allTable.Append("<td>" + dt.Rows[i][allTableName[j]] + "</td>");
                        }
                    }
                }
                allTable.Append("</tr>");

            }
            allTable.Append("</table>");
            #endregion

            #region 分页
            allTable.Append("-----");
            allTable.Append(PageCount + "-----" + Count);
            #endregion
            return allTable.ToString();
        }

        #endregion




    }
}
 