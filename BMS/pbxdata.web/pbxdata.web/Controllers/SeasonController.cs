using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class SeasonController : BaseController
    {
        //
        // GET: /Season/

        bll.Seasonbll bll = new bll.Seasonbll();

        /// <summary>
        /// 供应商季节
        /// </summary>
        /// <returns></returns>
        public ActionResult VenSeason()
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
        /// 供应商季节搜索
        /// </summary>
        public string SearchSeasonVen()
        {
            try
            {
                //{SeachName:'" + SeachName + "',SeachVenName:'" + SeachVenName + "',Vencode:'" + Vencode + "',bangd:'" + bangd + "'}
                string customerParams = Request.Form["params"] ?? string.Empty;//参数
                string[] customerParamss = helpcommon.StrSplit.StrSplitData(customerParams, ',');//得到参数集合
                string menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]).ToString();
                int page = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
                int Selpages = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

                string SeachName = helpcommon.StrSplit.StrSplitData(customerParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//季节名称
                string SeachVenName = helpcommon.StrSplit.StrSplitData(customerParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//供应季节名称
                string Vencode = helpcommon.StrSplit.StrSplitData(customerParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//供应商
                string bangd = helpcommon.StrSplit.StrSplitData(customerParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//是否绑定
                //Dictionary<string, string> Dic = new Dictionary<string, string>();
                SeachName = SeachName == "\'\'" ? string.Empty : SeachName;
                SeachVenName = SeachVenName == "\'\'" ? string.Empty : SeachVenName;
                Vencode = Vencode == "\'\'" ? string.Empty : Vencode;
                bangd = bangd == "\'\'" ? string.Empty : bangd;
                DataTable dt = new DataTable();
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                Dic.Add("Cat1", SeachName);  //--本地类别
                Dic.Add("Cat1Ven", SeachVenName); //--供应商类别
                Dic.Add("Vencode", Vencode);//--供应商
                Dic.Add("bangd", bangd);//--是否绑定
                string counts;
                dt = bll.SearchSeasonVen(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                int pagecount = int.Parse(counts) % Selpages == 0 ? int.Parse(counts) / Selpages : int.Parse(counts) / Selpages + 1;
                return GetTabSeasonVen(dt, menuId) + "-----" + pagecount.ToString() + "-----" + counts; 

                //int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                //DataTable dt = new DataTable();
                //PublicHelpController ph = new PublicHelpController();
                //string[] SearchInfo = lists.Split(',');
                //Dictionary<string, string> Dic = new Dictionary<string, string>();
                //Dic.Add("Cat1", SeachName);  //--本地类别
                //Dic.Add("Cat1Ven", SeachVenName); //--供应商类别
                //Dic.Add("Vencode", Vencode);//--供应商
                //Dic.Add("bangd", bangd);//--是否绑定
                //string counts;
                //dt = bll.SearchSeasonVen(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                //return GetTabSeasonVen(dt, menuId) + "-*-" + counts; 
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
        /// <summary>
        /// 供应商季节table拼接
        /// </summary>
        public string GetTabSeasonVen(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            //string sql = @"select * from(select cat2 from ItalyPorductStock group by cat2) a left join producttypeVen b on a.Cat2=b.TypeNo";
            //string[] ssName = attrbll.getDataName(sql);
            string[] ssName = {"Cat1", "VenCat1", "Vencode2"};
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            //DataTable dt = DbHelperSQL.Query(strsql).Tables[0];
            PublicHelpController ph = new PublicHelpController();
            //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            string[] ss = { "Cat1", "VenCat1", "Vencode2" };
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "Cat1")
                        s.Append("季节名称");
                    if (ssName[z] == "VenCat1")
                        s.Append("供应季节名称");
                    if (ssName[z] == "Vencode2")
                        s.Append("供应商");
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
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
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
                        s.Append("<td><a href='#' onclick='EditAttribute(\"" + itemId + "\",\"" + dt.Rows[i]["Cat1"].ToString() + "\",\"" + dt.Rows[i]["VenCat1"].ToString() + "\",\"" + dt.Rows[i]["Vencode2"].ToString() + "\")'>编辑</a></td>");
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
                            s.Append("<td><a href='#' onclick='DeleteSeasonVen(\"" + itemId + "\")'>删除</a></td>");
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
        /// 季节下拉列表
        /// </summary>
        public string SearchSeasonDDlist()
        {
            //string s = string.Empty;\
            StringBuilder s = new StringBuilder(200);
            string Cat1 = Request.QueryString["Cat1"].ToString();
            DataTable dt = new DataTable();
            dt = bll.GetData(Cat1);
            if (dt.Rows.Count > 0)
            {
                s.Append("<option value=''>请选择</option>");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Cat1 == dt.Rows[i]["Cat1"].ToString())
                    {
                        s.Append("<option value='" + dt.Rows[i]["Cat1"] + "'  selected='selected' >" + dt.Rows[i]["Cat1"]+"</option>");
                    }
                    else
                    {
                        s.Append("<option value='" + dt.Rows[i]["Cat1"] + "' >" + dt.Rows[i]["Cat1"] + "</option>");
                    }

                }
            }
            return s.ToString(); ;
        }
        /// <summary>
        /// 编辑供应商季节
        /// </summary>
        public string UpdateSeasonVen()
        {
            string Id = Request.Form["Id"].ToString();
            string SeasonVen = Request.Form["SeasonVen"].ToString();
            string Vencode = Request.Form["Vencode"].ToString();
            string Season = Request.Form["Season"].ToString();
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", Id);
            Dic.Add("Cat1Ven", SeasonVen);
            Dic.Add("Vencode", Vencode);
            Dic.Add("Cat1", Season);
            DataTable dt = new DataTable();
            return bll.UpdateSeasonVen(Dic, userInfo.User.Id);
        }
        /// <summary>
        /// 删除供应商类别
        /// </summary>
        public string DeleteSeasonVen()
        {
            string Id = Request.QueryString["Id"].ToString();
            return bll.DeleteSeasonVen(Id, userInfo.User.Id);
        }

        /// <summary>
        /// 季节视图
        /// </summary>
        /// <returns></returns>
        public ActionResult SeasonShow()
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
        /// 查询季节表
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="menuId"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string OnSearch()
        { 
            try
            {
                string customerParams = Request.Form["params"] ?? string.Empty;//参数
                string[] customerParamss = helpcommon.StrSplit.StrSplitData(customerParams, ',');//得到参数集合
                string menuId = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]).ToString();
                int page = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
                int Selpages = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

                string SerarchCat = helpcommon.StrSplit.StrSplitData(customerParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//季节名称
                Dictionary<string, string> Dic = new Dictionary<string, string>();
                SerarchCat = SerarchCat == "\'\'" ? string.Empty : SerarchCat;
                DataTable dt = new DataTable();
                Dic.Add("Cat1", SerarchCat);  //季节名称
                string counts;
                dt = bll.OnSearch(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
                int pagecount = int.Parse(counts) % Selpages == 0 ? int.Parse(counts) / Selpages : int.Parse(counts) / Selpages + 1;
                return GetData(dt, menuId) + "-----" + pagecount.ToString() + "-----" + counts; 

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// 季节table拼接
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string GetData(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            //string sql = @"select * from(select cat2 from ItalyPorductStock group by cat2) a left join producttypeVen b on a.Cat2=b.TypeNo";
            //string[] ssName = attrbll.getDataName(sql);
            string[] ssName = { "Cat1" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            //DataTable dt = DbHelperSQL.Query(strsql).Tables[0];
            PublicHelpController ph = new PublicHelpController();
            //string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            string[] ss = { "Cat1" };
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "Cat1")
                        s.Append("季节名称");
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
                //s.Append("<tr id='noData'>");
                //s.Append("<td></td>");
                //s.Append("</tr>");
                s.Append("***NoData***");
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
                            s.Append("<td>");
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                            s.Append("</td>");
                        }
                    }

                    #region 编辑
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<td><a href='#' onclick='EditSeason(\"" + dt.Rows[i]["Id"].ToString() + "\",\"" + dt.Rows[i]["Cat1"].ToString() + "\")'>编辑</a></td>");
                    }
                    #endregion

                    #region 删除
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        s.Append("<td><a href='#' onclick='DeleteSeason(\"" + dt.Rows[i]["Cat1"].ToString() + "\")'>删除</a></td>");
                    }
                    #endregion

                    s.Append("</tr>");
                }
            } 
            #endregion
            return s.ToString();
        }
        /// <summary>
        /// 添加季节名称
        /// </summary>
        /// <returns></returns>
        public string AddSeason()
        {
            string s = string.Empty;
            string Cat1 = Request.Form["Cat1"].ToString();
            return bll.AddSeason(Cat1, userInfo.User.Id);
        }
        /// <summary>
        /// 更新季节
        /// </summary>
        /// <returns></returns>
        public string UpdateSeason()
        {
            string s = string.Empty;
            string Id = Request.Form["Id"].ToString();
            string Cat1 = Request.Form["Cat1"].ToString();
            return bll.UpdateSeason(Id,Cat1,userInfo.User.Id);
        }
        /// <summary>
        /// 删除季节
        /// </summary>
        /// <returns></returns>
        public string DeleteSeason()
        {
            string s = string.Empty;
            string Cat1 = Request.Form["Cat1"].ToString();
            return bll.DeleteSeason(Cat1, userInfo.User.Id);
        }

    
    }
}
