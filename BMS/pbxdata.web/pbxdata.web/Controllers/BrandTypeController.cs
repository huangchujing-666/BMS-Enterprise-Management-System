using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using pbxdata.bll;
namespace pbxdata.web.Controllers
{
    public class BrandTypeController : BaseController
    {
        //品牌类别权限控制
        // GET: /BrandType/

        public ActionResult Index()
        {
            return View();
        }
        
        /*--------------------------------类别配置----------------------------------------*/
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public string ShowCustomer()
        {
            StringBuilder DrpPersion = new StringBuilder(200);
            DataTable dt = ptb.GetPersona();
            List<SelectListItem> list = new List<SelectListItem>();
            DrpPersion.Append("<option value='0'>请选择</option>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DrpPersion.Append("<option value='" + dt.Rows[i][1] + "'>" + dt.Rows[i][0] + "</option>");
            }
            return DrpPersion.ToString();
        }
        /// <summary>
        /// 供应商列表
        /// </summary>
        /// <returns></returns>
        public string ShowVencode()
        {
            StringBuilder DroVencode = new StringBuilder(200); ;
            ProductStockBLL psb = new ProductStockBLL();
            List<model.productsource> listSource = psb.GetVencodeProduct();
            List<SelectListItem> list = new List<SelectListItem>();
            DroVencode.Append("<option value='0'>请选择</option>");
            for (int i = 0; i < listSource.Count; i++)
            {
                DroVencode.Append("<option value='" + listSource[i].SourceCode + "'>" + listSource[i].sourceName + "</option>");
            }
            return DroVencode.ToString();
        }
        public static int SvaeMenuId;
        public static string SaveUserId;
        ProductTypeBLL ptb = new ProductTypeBLL();
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <returns></returns>
        public ActionResult TypeConfiguration()
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
            SvaeMenuId = menuId;
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["RoleId"] = userInfo.User.personaId.ToString();
            return View();
        }
        /// <summary>
        /// 页面显示        
        /// </summary>
        /// <returns></returns>
        public string TypeShow()
        {
            if (userInfo.User.personaId == 1)  //如果是管理员
            {
                #region 管理员
                ProductStockBLL psb = new ProductStockBLL();
                string vencode = Request.Form["vencode"] == null ? "1" : Request.Form["vencode"].ToString();
                string roleId = Request.Form["roleId"] == null ? "0" : Request.Form["roleId"].ToString();
                StringBuilder allpage = new StringBuilder();
                List<model.productbigtype> listBigType = psb.GetProductBigTypeVencode();
                string[] BigType;
                string[] Type = ptb.GetTypePerssoin(roleId, vencode, out BigType);
                for (int i = 0; i < listBigType.Count; i++)
                {
                    if (BigType.Contains(listBigType[i].Id.ToString()))
                    {
                        allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' checked='checked' category='BigType' type='checkbox' id='" + listBigType[i].Id + "' onchange='CheckAll(\"" + listBigType[i].Id + "\")' />" + listBigType[i].bigtypeName + "</div>");
                    }
                    else
                    {
                        allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' category='BigType' type='checkbox' id='" + listBigType[i].Id + "' onchange='CheckAll(\"" + listBigType[i].Id + "\")' />" + listBigType[i].bigtypeName + "</div>");
                    }
                    List<model.producttypeVen> list = psb.GetProductTypeVencode(vencode);
                    list = list.Where(a => a.BigId == listBigType[i].Id).ToList();
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (Type.Contains(list[j].TypeNo.ToString()))
                        {
                            allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' checked='checked' category='Type' name='" + listBigType[i].Id + "' id='" + list[j].TypeNo + "'>" + list[j].TypeName + "</label></div>");
                        }
                        else
                        {
                            allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' category='Type' name='" + listBigType[i].Id + "' id='" + list[j].TypeNo + "'>" + list[j].TypeName + "</label></div>");
                        }
                    }
                    allpage.Append("</div><div style='clear:both'></div>");

                }
                return allpage.ToString();
                #endregion
            }
            else
            {
                #region 其他角色
                ProductStockBLL psb = new ProductStockBLL();
                string vencode = Request.Form["vencode"] == null ? "1" : Request.Form["vencode"].ToString();
                string roleId = Request.Form["roleId"] == null ? "0" : Request.Form["roleId"].ToString();
                StringBuilder allpage = new StringBuilder();
                List<model.productbigtype> listBigType = psb.GetProductBigTypeVencode();
                string[] BigType;//当前角色的大类别权限
                string[] Type = ptb.GetTypePerssoin(roleId, vencode, out BigType);//当前角色的权限
                string[] UserType = ptb.GetTypeUser(userInfo.User.Id.ToString(), vencode);//用户自己的权限
                for (int i = 0; i < listBigType.Count; i++)
                {
                    if (BigType.Contains(listBigType[i].Id.ToString()))
                    {
                        List<model.producttypeVen> list = psb.GetProductTypeVencode(vencode, listBigType[i].Id);//通过大类别获取小类别
                        allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' checked='checked' category='BigType' type='checkbox' id='" + listBigType[i].Id + "' onchange='CheckAll(\"" + listBigType[i].Id + "\")' />" + listBigType[i].bigtypeName + "</div>");
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (Type.Contains(list[j].TypeNo))
                            {
                                if (UserType.Contains(list[j].TypeNo))
                                {
                                    allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' checked='checked' category='Type' name='" + listBigType[i].Id + "' id='" + list[j].TypeNo + "'>" + list[j].TypeName + "</label></div>");
                                }
                                else
                                {
                                    allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' category='Type' name='" + listBigType[i].Id + "' id='" + list[j].TypeNo + "'>" + list[j].TypeName + "</label></div>");
                                }
                            }
                        }
                    }
                    allpage.Append("</div><div style='clear:both'></div>");
                }
                return allpage.ToString();
                #endregion
            }
        }
        /// <summary>
        /// 超级管理员更改权限
        /// </summary>
        /// <returns></returns>
        public string UpdatePerssion()
        {
            string typeId = Request.Form["typeId"].ToString().Trim(',');
            string PsersionId = Request.Form["PersionId"].ToString();
            string vencode = Request.Form["Vencode"] == null ? "1" : Request.Form["Vencode"].ToString();
            string[] strTypeId = typeId.Split(',');
            ptb.ClearTypeConfigByUserId(PsersionId, "TypeConfig", vencode);//清除权限
            try
            {
                for (int i = 0; i < strTypeId.Length; i++) //小类别
                {
                    ptb.InsertPersion(strTypeId[i], PsersionId, ptb.GetBigByTypeNo(strTypeId[i]), vencode);
                }
                if (PsersionId == "1")//如果是管理员则直接添加权限
                {
                    ptb.ClearUserTypeByUserId(userInfo.User.Id.ToString(), vencode);//清除权限
                    for (int i = 0; i < strTypeId.Length; i++) //小类别
                    {
                        ptb.InsertPersonaTypeConfit(userInfo.User.Id.ToString(), strTypeId[i], vencode);
                    }
                }
                return "分配成功！";
            }
            catch (Exception ex)
            {
                return "分配失败！错误信息：" + ex.Message;
            }
        }
        /// <summary>
        /// 更改个人配置
        /// </summary>
        /// <returns></returns>
        public string UpdatePersonalConfig()
        {
            string typeId = Request.Form["typeId"] == null ? "" : Request.Form["typeId"].ToString().Trim();
            string vencode = Request.Form["Vencode"] == null ? "1" : Request.Form["Vencode"].ToString();
            string PsersionId = Request.Form["PersionId"].ToString();
            typeId = typeId.Trim(',');
            string[] strTypeId = typeId.Split(',');
            ptb.ClearUserTypeByUserId(userInfo.User.Id.ToString(), vencode);//清除权限
            try
            {
                for (int i = 0; i < strTypeId.Length; i++) //小类别
                {
                    ptb.InsertPersonaTypeConfit(userInfo.User.Id.ToString(), strTypeId[i], vencode);
                }
                return "分配成功！";
            }
            catch (Exception)
            {
                return "分配失败！";
                throw;
            }

        }
        /*--------------------------------品牌配置-----------------------------------------*/
        public static string PersionId;///用于保存当前角色Id
        public static string UserId;//用于保存当前用户的Id
        BrandBLL bb = new BrandBLL();
        public ActionResult BrandShow()
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
            SvaeMenuId = menuId;
            if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
            {
                return View("../NoPermisson/Index");
            }
            ViewData["RoleId"] = userInfo.User.personaId.ToString();
            return View();
        }
        /// <summary>
        /// 显示需要选择的页面
        /// </summary>
        /// <returns></returns>
        public string BrandShowPage()
        {
            StringBuilder allpage = new StringBuilder();
            if (userInfo.User.personaId == 1)  //如果是超级管理员
            {
                #region       超级管理员
                ProductStockBLL psb = new ProductStockBLL();
                string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
                string roleId = Request.Form["roleId"] == null ? "0" : Request.Form["roleId"].ToString();
                string[] BrandUser = ptb.GetBrandRole(vencode, roleId);
                string[] str = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < str.Length; i++)
                {
                    string[] RoleBrand = bb.GetUserBrandName(roleId, vencode, str[i]);
                    if (RoleBrand.Length > 0)
                    {
                        allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' checked='checked' type='checkbox' id='" + str[i] + "' onchange='CheckAll(\"" + str[i] + "\")' />" + str[i] + "</div>");
                    }
                    else
                    {
                        allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' type='checkbox' id='" + str[i] + "' onchange='CheckAll(\"" + str[i] + "\")' />" + str[i] + "</div>");
                    }
                    List<model.BrandVen> list = psb.GetBrandByVencode(vencode);
                    list = list.Where(a => a.BrandNameVen.StartsWith(str[i])).ToList();//匹配首字母
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (RoleBrand.Contains(list[j].BrandAbridge))
                        {
                            allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' checked='checked' Genre='Brand' name='" + str[i] + "' id='" + list[j].BrandAbridge + "'>" + list[j].BrandNameVen + "</label></div>");
                        }
                        else
                        {
                            allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' name='" + str[i] + "' id='" + list[j].BrandAbridge + "'>" + list[j].BrandNameVen + "</label></div>");
                        }
                    }
                    allpage.Append("</div><div style='clear:both'></div>");
                }
                List<model.BrandVen> listall = psb.GetBrandByVencode(vencode);
                for (int i = 0; i < str.Length; i++)
                {
                    listall = listall.Where(a => a.BrandNameVen.StartsWith(str[i]) == false).ToList();//排除首字母A-Z
                }
                string[] elsrRoleBrand = bb.GetUserBrandName(roleId, vencode);
                if (elsrRoleBrand.Length > 0)
                {
                    allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' type='checkbox' checked='checked' id='rest' onchange='CheckAll(\"rest\")' />其他</div>");
                }
                else
                {
                    allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' type='checkbox' id='rest' onchange='CheckAll(\"rest\")' />其他</div>");
                }
                for (int i = 0; i < listall.Count; i++)
                {
                    if (elsrRoleBrand.Contains(listall[i].BrandAbridge))
                    {
                        allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' checked='checked' name='rest' id='" + listall[i].BrandAbridge + "'>" + listall[i].BrandNameVen + "</label></div>");
                    }
                    else
                    {
                        allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' name='rest' id='" + listall[i].BrandAbridge + "'>" + listall[i].BrandNameVen + "</label></div>");
                    }
                }
                allpage.Append("</div><div style='clear:both'></div>");
                return allpage.ToString();
                #endregion
            }
            else
            {
                #region   用户
                string vencode = Request.Form["vencode"] == null ? "" : Request.Form["vencode"].ToString();
                string roleId = Request.Form["roleId"] == null ? "0" : Request.Form["roleId"].ToString();
                string[] str = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string[] strUserBrand = bb.GetUserBrandSx(userInfo.User.Id.ToString(), vencode);
                //首字母A-Z
                for (int i = 0; i < str.Length; i++)
                {
                    #region
                    List<model.BrandConfig> list = bb.GetAllUserBrand(roleId, vencode, str[i]);
                    if (list.Count > 0)
                    {
                        allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' checked='checked' type='checkbox' id='" + str[i] + "' onchange='CheckAll(\"" + str[i] + "\")' />" + str[i] + "</div>");
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (strUserBrand.Contains(list[j].BrandId))
                            {
                                allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' checked='checked' Genre='Brand' name='" + str[i] + "' id='" + list[j].BrandId + "'>" + list[j].BrandName + "</label></div>");
                            }
                            else
                            {
                                allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' name='" + str[i] + "' id='" + list[j].BrandId + "'>" + list[j].BrandName + "</label></div>");
                            }
                        }
                    }
                    allpage.Append("</div><div style='clear:both'></div>");
                    #endregion
                }
                //其他字符开头
                List<model.BrandConfig> listAll = bb.GetAllUserBrand(roleId, vencode);
                for (int i = 0; i < str.Length; i++)
                {
                    listAll = listAll.Where(a => a.BrandName.ToUpper().StartsWith(str[i]) == false).ToList();//排除首字母A-Z
                }
                allpage.Append("<div><div style='color:blue;font-size:15px;'><input class='ckeckboxAll' type='checkbox' checked='checked' id='else' onchange='CheckAll(else)' />其他</div>");
                for (int i = 0; i < listAll.Count; i++)
                {
                    if (strUserBrand.Contains(listAll[i].BrandId))
                    {
                        allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' checked='checked' Genre='Brand' name='else' id='" + listAll[i].BrandId + "'>" + listAll[i].BrandName + "</label></div>");
                    }
                    else
                    {
                        allpage.Append("<div style='float:left;min-width:120px'><label><input type='checkbox' Genre='Brand' name='else' id='" + listAll[i].BrandId + "'>" + listAll[i].BrandName + "</label></div>");
                    }
                }
                return allpage.ToString();
                #endregion
            }

        }
        /// <summary>
        /// 确认给角色分配权限
        /// </summary>
        /// <returns></returns>
        public string OkBrandPerssion()
        {
            string brandId = Request.Form["BrandId"].ToString().Trim(',');
            string[] Brand = brandId.Split(',');
            string PesrsonalId = Request.Form["Personal"].ToString();
            string vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            bb.ClearBrandConfigByPersonaId(PesrsonalId, vencode);
            if (Brand.Length > 0)
            {
                for (int i = 0; i < Brand.Length; i++)
                {
                    bb.InsertBrandConfig(PesrsonalId, Brand[i].ToString(), bb.GetBrandNameVencode(Brand[i], vencode), vencode);
                }
                if (PesrsonalId == "1")
                {
                    bb.ClearBrandConfigPersionByUserId(userInfo.User.Id.ToString(), vencode);
                    for (int i = 0; i < Brand.Length; i++)
                    {
                        bb.InsertUserPerssion(userInfo.User.Id.ToString(), Brand[i].ToString(), vencode, bb.GetBrandNameVencode(Brand[i].ToString(), vencode));
                    }
                }
                return "分配成功！";
            }
            else
            {
                return "分配成功！";
            }
        }
        /// <summary>
        /// 用户个人配置
        /// </summary>
        /// <returns></returns>
        public string UserBrandPression()
        {
            string brandid = Request.Form["BrandId"].ToString().Trim(',');
            string vencode = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            string[] strbrand = brandid.Split(',');
            bb.ClearBrandConfigPersionByUserId(userInfo.User.Id.ToString(), vencode);
            if (strbrand.Length > 0)
            {
                for (int i = 0; i < strbrand.Length; i++)
                {
                    bb.InsertUserPerssion(userInfo.User.Id.ToString(), strbrand[i].ToString(), vencode, bb.GetBrandNameVencode(strbrand[i].ToString(), vencode));
                }
                return "分配成功！";
            }
            else
            {
                return "分配成功！";
            }
        }
    }
}
