using Maticsoft.DBUtility;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class BrandController : BaseController
    {
        //
        // GET: /Brand/
        bll.Attributebll attrbll = new bll.Attributebll();
        bll.BrandBLL brandbll = new bll.BrandBLL();

        public ActionResult Index()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;          

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }
        /// <summary>
        /// 品牌表查询
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="menuId"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string SearchBrand()//string menuId,string Name,string page, string Selpages
        {
            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Name = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}","");
            string counts = string.Empty;
            DataTable dt = new DataTable();
            dt = attrbll.SearchBrand(Name, pageIndex, pageSize, out counts);
            int pageCount = Convert.ToInt32(counts) %Convert.ToInt32(pageSize) == 0 ? Convert.ToInt32(counts) / Convert.ToInt32(pageSize) : Convert.ToInt32(counts) / Convert.ToInt32(pageSize)+1;
            return getDataBrand(dt, menuId) + "-----"+pageCount.ToString()+ "-----"+ counts;
        }
        /// <summary>
        /// 品牌表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataBrand(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            s.Append("<th>品牌名</th>");
            s.Append("<th>品牌缩写</th>");
            s.Append("<th>淘宝品牌</th>");
            s.Append("<th>淘宝Vid</th>");
            s.Append("<th>品牌归属地</th>");
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                s.Append("<th>编辑</th>");
            }
            if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            {
                s.Append("<th>删除</th>");
            }


            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr><td>" + n + "</td>");
                    s.Append("<td>" + dt.Rows[i]["BrandName"] + "</td>");
                    s.Append("<td>" + dt.Rows[i]["BrandAbridge"] + "</td>");
                    s.Append("<td>" + dt.Rows[i]["TBBrandName"] + "</td>");
                    s.Append("<td>" + dt.Rows[i]["Def1"] + "</td>");
                    s.Append("<td>" + dt.Rows[i]["Def2"] + "</td>");

                    #region 编辑
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<td><a href='#' onclick='Edit(\"" + dt.Rows[i]["BrandName"] + "\",\"" + dt.Rows[i]["BrandAbridge"] + "\",\"" + dt.Rows[i]["Def1"] + "\",\"" + dt.Rows[i]["Def2"] + "\")'>编辑</a></td>");
                    }
                    #endregion

                    #region 删除

                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        s.Append("<td><a href='#' onclick='Delete(\"" + dt.Rows[i]["BrandName"] + "\",\"" + dt.Rows[i]["BrandAbridge"] + "\")'>删除</a></td>");
                    }
                    #endregion


                    s.Append("</tr>");
                }
            }
            
            #endregion


            return s.ToString();
        }
        /// <summary>
        /// 获取淘宝品牌下拉表
        /// </summary>
        public string GetTBbrandlist()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string TBBrandName = Request.QueryString["TBBrandName"];
            string vid = Request.QueryString["vid"];
            dt = brandbll.GetTBbrandlist(TBBrandName);
            if (dt.Rows.Count != 0)
            {
                s.Append("<option value=''>请选择</option>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["vid"].ToString() == vid)
                    {
                        s.Append("<option value='" + dt.Rows[i]["vid"] + "' selected='selected'>" + dt.Rows[i]["TBBrandName"] + "</option>");
                    }
                    else
                    {
                        s.Append("<option value='" + dt.Rows[i]["vid"] + "'>" + dt.Rows[i]["TBBrandName"] + "</option>");
                    }
                }
            }

            return s.ToString();
        }
        /// <summary>
        /// 获取没被选择的淘宝品牌下拉表
        /// </summary>
        public string GetTBBrand()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            string vid = Request.QueryString["vid"];
            string sql = @"  select * from TBBrand where vid not in (select Def1 as vid from brand where Def1 is not null and Def1 !='' group by Def1)";
            dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count != 0)
            {
                s.Append("<option value=''>请选择</option>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["vid"].ToString() == vid)
                    {
                        s.Append("<option value='" + dt.Rows[i]["TBBrandName"] + "' selected='selected'>" + dt.Rows[i]["TBBrandName"] + "</option>");
                    }
                    else
                    {
                        s.Append("<option value='" + dt.Rows[i]["TBBrandName"] + "'>" + dt.Rows[i]["TBBrandName"] + "</option>");
                    }
                }
            }

            return s.ToString();
        }
        /// <summary>
        /// 更新品牌对应的淘宝编号
        /// </summary>
        public string UpdateBrand()
        {
            string s = "";
            string BrandName = Request.QueryString["BrandName"];
            string BrandAbridge = Request.QueryString["BrandAbridge"];
            string Vid = Request.QueryString["Vid"];
            string Def2 = Request.QueryString["Def2"];
            return brandbll.UpdateBrand(BrandName, BrandAbridge, Vid, Def2, userInfo.User.Id);
        }
        /// <summary>
        /// 添加品牌
        /// </summary>
        public string AddBrand()
        {
            string BrandName = Request.QueryString["BrandName"];
            string BrandAbridge = Request.QueryString["BrandAbridge"];
            string Def2 = Request.QueryString["Def2"];
            return brandbll.AddBrand(BrandName, BrandAbridge,Def2, userInfo.User.Id);
        }
        /// <summary>
        /// 删除品牌
        /// </summary>
        public string DeleteBrand()
        {
            //string s = "";
            string BrandName = Request.QueryString["BrandName"];
            string BrandAbridge = Request.QueryString["BrandAbridge"];
            return brandbll.DeleteBrand(BrandName, BrandAbridge,userInfo.User.Id);
            //string sql = @"delete from brand where BrandName='" + BrandName + "' and BrandAbridge='" + BrandAbridge + "'";
            //if (DbHelperSQL.ExecuteSql(sql) == -1)
            //{
            //    s = "删除失败!";
            //}
            //return s;
        }
        /// <summary>
        /// 显示淘宝品牌
        /// </summary>
        public ActionResult TBBrandShow()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }
        /// <summary>
        /// 淘宝品牌表查询
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="menuId"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string SearchTBBrand()//string menuId, string Name, string page, string Selpages
        {
            string paramss = Request.Form["params"] ?? string.Empty;
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string pageIndex = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageIndex"]);
            string pageSize = helpcommon.ParmPerportys.GetStrParms(Request.Form["pageSize"]);
            string[] ss = helpcommon.StrSplit.StrSplitData(paramss, ',');
            string Name = helpcommon.StrSplit.StrSplitData(ss[0], ':')[1].Replace("'", "").Replace("}", "");
            string counts = string.Empty;
            DataTable dt = new DataTable();
            dt = attrbll.SearchTBBrand(Name, pageIndex,pageSize, out counts);
            int pageCount=Convert.ToInt32(counts)%Convert.ToInt32(pageSize)==0?Convert.ToInt32(counts)/Convert.ToInt32(pageSize):Convert.ToInt32(counts)/Convert.ToInt32(pageSize);
            return getDataTBBrand(dt, menuId) + "-----" + pageCount + "-----" + counts;
        }
        /// <summary>
        /// 显示淘宝品牌表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataTBBrand(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            s.Append("<th>淘宝品牌</th>");
            s.Append("<th>淘宝编号</th>");
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                s.Append("<th>编辑</th>");
            }
            if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            {
                s.Append("<th>删除</th>");
            }
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr><td>" + n + "</td>");
                    s.Append("<td>" + dt.Rows[i]["TBBrandName"] + "</td>");
                    s.Append("<td>" + dt.Rows[i]["vid"] + "</td>");

                    #region 编辑
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<td><a href='#' onclick='Edit(\"" + dt.Rows[i]["TBBrandName"] + "\",\"" + dt.Rows[i]["vid"] + "\",\"" + dt.Rows[i]["Id"] + "\")'>编辑</a></td>");
                    }
                    #endregion

                    #region 删除
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        s.Append("<td><a href='#' onclick='Delete(\"" + dt.Rows[i]["TBBrandName"] + "\",\"" + dt.Rows[i]["vid"] + "\",\"" + dt.Rows[i]["Id"] + "\")'>删除</a></td>");
                    }
                    #endregion


                    s.Append("</tr>");
                }
            }
            
            #endregion


            return s.ToString();
        }
        /// <summary>
        /// 更新淘宝品牌
        /// </summary>
        public string UpdateTBBrand()
        {
            string TBBrandName = Request.QueryString["TBBrandName"];
            string vid = Request.QueryString["vid"];
            string Id = Request.QueryString["Id"];
            return brandbll.UpdateTBBrand(TBBrandName, vid, Id, userInfo.User.Id);
        }
        /// <summary>
        /// 添加淘宝品牌
        /// </summary>
        public string AddTBBrand()
        {
            string TBBrandName = Request.QueryString["BrandName"];
            string vid = Request.QueryString["vid"];
            return brandbll.AddTBBrand(TBBrandName, vid, userInfo.User.Id);
        }
        /// <summary>
        /// 更新淘宝品牌
        /// </summary>
        public string DeleteTBBrand()
        {
            string TBBrandName = Request.QueryString["TBBrandName"];
            string vid = Request.QueryString["vid"];
            string Id = Request.QueryString["Id"];
            string s = "";
            string Deletesql = @"delete from TBBrand where TBBrandName='" + TBBrandName + "' and vid='" + vid + "'";
            if (Convert.ToInt32(DbHelperSQL.GetSingle(Deletesql)) == -1)
            {
                s = "删除失败!";
            }
            return s;
        }
        /// <summary>
        /// 供应商品牌
        /// </summary>
        /// <returns></returns>
        public ActionResult BrandVen()
        {
            try
            {

                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                PublicHelpController ph = new PublicHelpController();


                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                {
                    return View("../NoPermisson/Index");
                }
                #endregion


                ViewData["myMenuId"] = menuId;

                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }
        /// <summary>
        /// 供应商品牌搜索
        /// </summary>
        public string SearchBrandVen(string lists, string menuId, string page, string Selpages)
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                DataTable dt = new DataTable();
                PublicHelpController ph = new PublicHelpController();
                string[] SearchInfo = lists.Split(',');
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("BrandName", SearchInfo[0]);  //--本地类别
                Dic.Add("BrandNameVen", SearchInfo[1]); //--供应商类别
                Dic.Add("Vencode", SearchInfo[2]);//--供应商
                Dic.Add("bangd", SearchInfo[3]);//--是否绑定
                string counts;
                dt = brandbll.SearchBrandVen(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                return GetTabBrandVen(dt, menuId) + "-*-" + counts; ;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
        /// <summary>
        /// 供应商品牌搜索
        /// </summary>
        public string GetTabBrandVen(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            //string sql = @"select * from(select cat2 from ItalyPorductStock group by cat2) a left join producttypeVen b on a.Cat2=b.TypeNo";
            //string[] ssName = attrbll.getDataName(sql);
            string[] ssName = { "Cat", "Vencode2", "BrandName", "BrandAbridge" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            //DataTable dt = DbHelperSQL.Query(strsql).Tables[0];
            PublicHelpController ph = new PublicHelpController();
            //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            string[] ss = { "Cat", "Vencode2", "BrandName", "BrandAbridge" };
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "Cat")
                        s.Append("供应品牌名称");
                    if (ssName[z] == "Vencode2")
                        s.Append("供应商");
                    if (ssName[z] == "BrandName")
                        s.Append("品牌名称");
                    if (ssName[z] == "BrandAbridge")
                        s.Append("品牌缩写");
                    s.Append("</th>");
                }
            }
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                s.Append("<th>编辑</th>");
            };
            #region 删除
            if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            {
                s.Append("<th>删除</th>");
            }
            #endregion
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count<=0||dt==null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; text-align:center; color:red;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr><td>" + n + "</td>");
                    for (int j = 0; j < ssName.Length; j++)
                    {
                        if (ss.Contains(ssName[j]))
                        {
                            if (ssName[j] == "Vencode2")
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i]["sourceName"].ToString());
                                s.Append("</td>");
                            }
                            else
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i][ssName[j]].ToString());
                                s.Append("</td>");
                            }
                        }
                    }

                    #region 编辑
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        string itemId = dt.Rows[i]["Id"].ToString() == null || dt.Rows[i]["Id"].ToString() == "" ? "0" : dt.Rows[i]["Id"].ToString();
                        //string Cat = string.Empty;
                        //if (dt.Rows[i]["Cat"].ToString().Contains("'"))
                        //{
                        //    Cat = ;
                        //}
                        //s.Append("<td><a href='#' onclick='EditAttribute(\"" + dt.Rows[i]["Id"].ToString() + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\",\"" + dt.Rows[i]["TypeNameVen"].ToString() + "\",\"" + dt.Rows[i]["Vencode"].ToString() + "\")'>编辑</a></td>");
                        s.Append("<td><a href='#' onclick='EditAttribute(\"" + itemId + "\",\"" + dt.Rows[i]["BrandName"].ToString() + "\",\"" + dt.Rows[i]["Cat"].ToString().Replace("'", "**") + "\",\"" + dt.Rows[i]["Vencode2"].ToString() + "\")'>编辑</a></td>");
                    }
                    #endregion

                    #region 删除
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        string itemId = dt.Rows[i]["Id"].ToString() == null || dt.Rows[i]["Id"].ToString() == "" ? "0" : dt.Rows[i]["Id"].ToString();
                        //s.Append("<td><a href='#' onclick='DeleteType(\"" + dt.Rows[i]["Id"].ToString() + "\")'>删除</a></td>");
                        if (itemId == "0")
                        {
                            s.Append("<td></td>");
                        }
                        else
                        {
                            s.Append("<td><a href='#' onclick='DeleteType(\"" + itemId + "\")'>删除</a></td>");
                        }

                    }
                    #endregion

                    s.Append("</tr>");
                }
            }
            
            #endregion
            return s.ToString();
        }
        /// <summary>
        /// 供应商类别搜索
        /// </summary>
        public string SearchBrandDDlist()
        {
            string BrandName = Request.QueryString["BrandName"].ToString();
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            dt = brandbll.SearchBrandDDlist(BrandName);
            if (dt.Rows.Count > 0)
            {
                s.Append("<option value=''>请选择</option>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (BrandName == dt.Rows[i]["BrandName"].ToString())
                    {
                        s.Append("<option value='" + dt.Rows[i]["BrandAbridge"] + "' title='" + dt.Rows[i]["BrandName"].ToString().Replace("'", "**") + "' selected='selected' >" + dt.Rows[i]["BrandName"] + "-" + dt.Rows[i]["BrandAbridge"] + "</option>");
                    }
                    else
                    {
                        s.Append("<option value='" + dt.Rows[i]["BrandAbridge"] + "' title='" + dt.Rows[i]["BrandName"].ToString().Replace("'", "**") + "' >" + dt.Rows[i]["BrandName"] + "-" + dt.Rows[i]["BrandAbridge"] + "</option>");
                    }

                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 编辑供应商品牌
        /// </summary>
        public string UpdateBrandVen()
        {
            string Id = Request.Form["Id"].ToString();
            string BrandNameVen = Request.Form["BrandNameVen"].ToString();
            string Vencode = Request.Form["Vencode"].ToString();
            string BrandName = Request.Form["BrandName"].ToString();
            string BrandAbridge = Request.Form["BrandAbridge"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", Id);
            Dic.Add("BrandNameVen", BrandNameVen);
            Dic.Add("Vencode", Vencode);
            Dic.Add("BrandName", BrandName);
            Dic.Add("BrandAbridge", BrandAbridge);
            DataTable dt = new DataTable();
            //return attrbll.UpdateProducttypeVen(Id, TypeNameVen, Vencode, TypeNo, userInfo.User.Id);
            return brandbll.UpdateBrandVen(Dic, userInfo.User.Id);
        }
        /// <summary>
        /// 删除供应商类别
        /// </summary>
        public string deleteBrandVen()
        {
            string Id = Request.QueryString["Id"].ToString();
            return brandbll.deleteBrandVen(Id, userInfo.User.Id);
        }
        /// <summary>
        /// 绑定品牌下拉
        /// </summary>
        /// <returns></returns>
        public string Brand()
        {
            ProductHelper pdh = new ProductHelper();
            List<SelectListItem> drplisttype = new List<SelectListItem>();//类别下拉框
            DataTable listType = pdh.ProductStyleDDlist(userInfo.User.Id.ToString());
            StringBuilder str = new StringBuilder();
            str.Append("<option value=''>请选择</option>");
            for (int i = 0; i < listType.Rows.Count; i++)
            {
                str.Append("<option value='" + listType.Rows[i]["BrandAbridge"] + "'>" + listType.Rows[i]["BrandName"] + "</option>");
            }
            return str.ToString();
        }
        /// <summary>
        /// 搜索品牌
        /// </summary>
        /// <returns></returns>
        public string ChageBrand() 
        {
            string brand = Request.Form["Brand"] == null ? "" : Request.Form["Brand"].ToString();
            ProductHelper pdh = new ProductHelper();
            List<SelectListItem> drplisttype = new List<SelectListItem>();//类别下拉框
            DataTable listType = pdh.ProductStyleDDlist(userInfo.User.Id.ToString());
            StringBuilder str = new StringBuilder();
            str.Append("<option value=''>请选择</option>");

            if (brand != "") 
            {
                DataRow[] dr = listType.Select("BrandName LIKE '%"+brand+"%'");
                for (int i = 0; i < dr.Length; i++)
                {
                    str.Append("<option value='" + dr[i][1] + "'>" + dr[i][0] + "</option>");
                }
                return str.ToString();
            }
            else
            {
                for (int i = 0; i < listType.Rows.Count; i++)
                {
                    str.Append("<option value='" + listType.Rows[i]["BrandAbridge"] + "'>" + listType.Rows[i]["BrandName"] + "</option>");
                }
                return str.ToString();
            }
        }
        /// <summary>
        /// 确认添加对应品牌
        /// </summary>
        /// <returns></returns>
        public string InsertBrand() 
        {
            string[] str = new string[4];
            str[0] = Request.Form["BrandName"] == null ? "" : Request.Form["BrandName"].ToString();
            str[1] = Request.Form["BrandAbridge"] == null ? "" : Request.Form["BrandAbridge"].ToString();
            str[2] = Request.Form["BrandVenName"] == null ? "" : Request.Form["BrandVenName"].ToString();
            str[3] = Request.Form["Vencode"] == null ? "" : Request.Form["Vencode"].ToString();
            return brandbll.InsertBrand(str);
        }
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public static int BrandPage = 0;
        public string GetBrandExcel() 
        {
            string customer = userInfo.User.Id.ToString();
            
            string[] str = new string[2];
            str[0] = Request.Form["BrandName"] == null ? "" : Request.Form["BrandName"].ToString();
            str[1] = Request.Form["BrandNameVen"] == null ? "" : Request.Form["BrandNameVen"].ToString();
            int page = Request.Form["page"] == null ? 10 : int.Parse(Request.Form["page"].ToString());
            int count = brandbll.GetBrandExcelCount(str);
            int PageSum = count % page > 0 ? count / page + 1 : count / page;//页码
            bool IsPage = Request.Form["isPage"] == null ? false : true;//为false 表示加载  为true则表示翻页
            int Index = Request.Form["Index"] == null ? 0 : int.Parse(Request.Form["Index"].ToString());
            if (IsPage == true)
            {
                #region   分页
                switch (Index)
                {
                    case 0:        //首页
                        BrandPage = 0;
                        break;
                    case 1:         //上一页
                        if (BrandPage - 1 >= 0)
                        {
                            BrandPage = BrandPage - 1;
                        }
                        else
                        {
                            BrandPage = 0;
                        }
                        break;
                    case 2:         //跳页 
                        int ReturnPage = Request.Form["PageReturn"] == null ? 0 : int.Parse(Request.Form["PageReturn"].ToString());
                        BrandPage = ReturnPage - 1;
                        break;
                    case 3:        //下页
                        if (BrandPage + 1 <= PageSum - 1)
                        {
                            BrandPage = BrandPage + 1;
                        }
                        else
                        {
                            BrandPage = PageSum - 1;
                        }
                        break;
                    case 4:        //末页
                        BrandPage = PageSum - 1;
                        break;
                }
                #endregion
            }
            else
            {
                BrandPage = 0;
            }
            int minid = BrandPage * page;
            int maxid = BrandPage * page + page;
            DataTable dt = brandbll.GetBrandExcel(str, minid, maxid);
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            //int menuId = OrderDetailsSaveMenuId;
            PublicHelpController ph = new PublicHelpController();
            //usersbll usb = new usersbll();
            //string[] allTableName = usb.getDataName("productstockOrder");
            //string[] s = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder Alltable = new StringBuilder();
            Alltable.Append("<table id='StcokTable' class='mytable' style='font-size:12px;'><tr style='text-align:center;'>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ToString() != "Id")
                {
                    if (dt.Columns[i].ToString() == "RowId")
                        Alltable.Append("<th>序号</th>");
                    if (dt.Columns[i].ToString() == "BrandName")
                        Alltable.Append("<th>品牌名称</th>");
                    if (dt.Columns[i].ToString() == "BrandAbridge")
                        Alltable.Append("<th>品牌缩写</th>");
                    if (dt.Columns[i].ToString() == "BrandNameVen")
                        Alltable.Append("<th>供应商品牌名称</th>");
                    if (dt.Columns[i].ToString() == "Vencode")
                        Alltable.Append("<th>来源</th>");
                }
            } 
            Alltable.Append("<th>操作</th>");
            Alltable.Append("</tr>");
            if (dt.Rows.Count<=0||dt==null)
            {
                Alltable.Append("<tr><td colspan='50' style='font-size:12px; text-align:center; color:red;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Alltable.Append("<tr>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ToString() != "Id")
                        {
                            Alltable.Append("<td>" + dt.Rows[i][j] + "</td>");
                        }
                    }
                    Alltable.Append("<td><a href='#' onclick='DeleteBrand(" + dt.Rows[i]["Id"] + ")'>删除</a></td>");
                    Alltable.Append("</tr>");
                }
            }
            Alltable.Append("</table>");
            int thispage = 0;
            if (IsPage == true)
            {
                thispage = BrandPage + 1;
            }
            else
            {
                thispage = 1;
            }
            return Alltable.ToString() + "❤" + PageSum + "❤" + count + "❤" + thispage;
        }
        /// <summary>
        /// 删除对应品牌
        /// </summary>
        /// <returns></returns>
        public string DeleteBrandExcel() 
        {
            string id = Request.Form["Id"] == null ? "" : Request.Form["Id"].ToString();
            return brandbll.DeleteBrandExcel(id);
        }
    }
}
