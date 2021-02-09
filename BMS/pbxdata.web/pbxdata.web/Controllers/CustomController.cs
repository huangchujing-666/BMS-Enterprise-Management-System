using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pbxdata.model;
using pbxdata.bll;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.OleDb;
using Maticsoft.DBUtility;

namespace pbxdata.web.Controllers
{
    public class CustomController : BaseController
    {

        bll.Custombll bll = new bll.Custombll();
        PublicHelpController ph = new PublicHelpController();
        /// <summary>
        /// 客户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowCustom()
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
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
        /// 判断搜索条件
        /// </summary>
        /// <param name="Id">条件</param>
        /// <returns></returns>
        public string SearchList(int menuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            StringBuilder s = new StringBuilder(200);
            foreach (var temp in ss)
            {
                if (temp == "CustomerId")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>客户ID：</span><input class='spanPropertyValue' id='txtSearchCustomerId' type='text'></span>");
                }
                else if (temp == "Shop")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>店铺/平台：</span><input class='spanPropertyValue' id='txtSearchShop' type='text'></span>");
                }
                else if (temp == "Sex")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>性别：</span><select id='selSearchSex' class='spanPropertyValue'><option value=''>请选择</option><option value='0'>女</option><option value='1'>男</option></select></span>");
                }
                else if (temp == "CustomerLevel")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>用户等级：</span><input class='spanPropertyValue' id='txtSearchCustomerLevel' type='text'></span>");
                }
            };
            s.Append("<br />");
            s.Append("<div class='clearfix'></div>");
            s.Append("<input type='button' value=' 查  询 ' id='btnSearch' class='spanPropertySearch' onclick='serarchCustom()' />");
            s.Append("<input type='button' value=' 添  加 ' class='spanPropertySearch' onclick='AddClick()' />");
            return s.ToString(); ;

        }
        public string OnSearch()
        {
            string customerParams=Request.Form["params"] ?? string.Empty;//参数
            string[] customerParamss = helpcommon.StrSplit.StrSplitData(customerParams, ',');//得到参数集合
            string menuId=helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]).ToString();
            int page = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int Selpages = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string CustomerId = helpcommon.StrSplit.StrSplitData(customerParamss[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//客户ID
            string Shop = helpcommon.StrSplit.StrSplitData(customerParamss[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//店铺/平台
            string Sex = helpcommon.StrSplit.StrSplitData(customerParamss[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//性别
            string CustomerLevel = helpcommon.StrSplit.StrSplitData(customerParamss[3], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//客户等级

            Dictionary<string, string> Dic = new Dictionary<string, string>();
            CustomerId = CustomerId == "\'\'" ? string.Empty : CustomerId;
            Shop = Shop == "\'\'" ? string.Empty : Shop;
            Sex = Sex == "\'\'" ? string.Empty : Sex;
            CustomerLevel = CustomerLevel == "\'\'" ? string.Empty : CustomerLevel;

            DataTable dt = new DataTable();

            Dic.Add("CustomerId", CustomerId);
            Dic.Add("Shop", Shop);
            Dic.Add("Sex", Sex);
            Dic.Add("CustomerLevel", CustomerLevel);
            string counts;
            dt = bll.GetDate(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts);
            int pagecount=int.Parse(counts) % Selpages == 0 ? int.Parse(counts) / Selpages : int.Parse(counts) / Selpages + 1;
            return GetDate(dt, menuId) + "-----" + pagecount.ToString() + "-----" + counts;
        }
        public string GetDate(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            string[] ssName = { "CustomerId", "Shop", "Contactperson", "Sex", "Age", "Birthday", "IDNumber", "Telephone", "Phone", "Weixin", "QQNo", "CustomerLevel", "Remark" };
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {

                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");

                    if (ssName[z] == "CustomerId")
                        s.Append("客户ID");
                    if (ssName[z] == "Shop")
                        s.Append("店铺/平台");
                    if (ssName[z] == "Contactperson")
                        s.Append("联系人");
                    if (ssName[z] == "Sex")
                        s.Append("性别");
                    if (ssName[z] == "Age")
                        s.Append("年龄");
                    if (ssName[z] == "Birthday")
                        s.Append("生日");
                    if (ssName[z] == "IDNumber")
                        s.Append("身份证");
                    if (ssName[z] == "Telephone")
                        s.Append("电话");
                    if (ssName[z] == "Phone")
                        s.Append("手机号");
                    if (ssName[z] == "Weixin")
                        s.Append("微信");
                    if (ssName[z] == "QQNo")
                        s.Append("QQ");
                    if (ssName[z] == "CustomerLevel")
                        s.Append("用户等级");
                    if (ssName[z] == "Remark")
                        s.Append("备注");
                    //if (ssName[z] == "CustomerServiceId")
                    //    s.Append("库存");
                    //if (ssName[z] == "UserId")
                    //    s.Append("商检");
                    s.Append("</th>");
                }
            }
            s.Append("<th>操作</th>");

            #endregion
            s.Append("</tr>");


            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int n = i + 1;
                s.Append("<tr><td>" + n + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        s.Append("<td>");
                        if (ssName[j].ToLower() == "sex")
                        {
                            string sex = dt.Rows[i][ssName[j]].ToString() == "0" ? "女" : dt.Rows[i][ssName[j]].ToString() == "1" ? "男" : "";
                            s.Append(sex);
                        }
                        else
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                        }

                        s.Append("</td>");
                    }
                }
                #region 编辑
                s.Append("<td>");
                if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                {
                    s.Append("<a href='#' onclick='EditCustomer(\"" + dt.Rows[i]["Id"].ToString() + "\")'>编辑</a>");
                    s.Append("<a href='#' onclick='CheckOrder(\"" + dt.Rows[i]["CustomerId"].ToString() + "\")'>查看</a>");
                }



                if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                {
                    s.Append("<a href='#' onclick='DeleteCustomer(\"" + dt.Rows[i]["Id"].ToString() + "\")''>删除</a>");
                }


                s.Append("</td>");
                #endregion

                s.Append("</tr>");
            }
            #endregion
            return s.ToString();
        }

        /// <summary>
        /// 添加新客户信息
        /// </summary>
        /// <returns></returns>
        public string AddCustomer()
        {
            string s = string.Empty;
            string[] list = Request.Form["list[]"].ToString().Split(',');
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("CustomerId", list[0]);
            Dic.Add("Shop", list[1]);
            Dic.Add("Contactperson", list[2]);
            Dic.Add("Sex", list[3]);
            Dic.Add("Age", list[4]);
            Dic.Add("Birthday", list[5]);
            Dic.Add("IDNumber", list[6]);
            Dic.Add("Telephone", list[7]);
            Dic.Add("Phone", list[8]);
            Dic.Add("Weixin", list[9]);
            Dic.Add("QQNo", list[10]);
            Dic.Add("CustomerLevel", list[11]);
            Dic.Add("Remark", list[12]);
            return bll.AddCustomer(Dic, userInfo.User.Id);
        }
        /// <summary>
        /// 弹出编辑客户信息
        /// </summary>
        /// <returns></returns>
        public string EditCustomer()
        {
            string s = string.Empty;
            string Id = Request.Form["Id"].ToString();
            DataTable dt = new DataTable();
            dt = bll.GetDate(Id);
            s += "<ul>";
            s += "<li>客户ID:<input type='text' id='txtEditCustomerId' value='" + dt.Rows[0]["CustomerId"].ToString() + "' title='" + dt.Rows[0]["Id"].ToString() + "' disabled='disabled' /></li>";
            s += "<li>店铺/平台:<input type='text' id='txtEditShop'  value='" + dt.Rows[0]["Shop"].ToString() + "' /></li>";
            s += "<li>联系人:<input type='text' id='txtEditContactperson'  value='" + dt.Rows[0]["Contactperson"].ToString() + "' /></li>";
            if (dt.Rows[0]["Sex"].ToString() == "0")
            {
                s += "<li>性别:<select id='selEditSex'><option value=''>请选择</option><option value='0' selected='selected'>女</option><option value='1'>男</option></select></li>";
            }
            else if (dt.Rows[0]["Sex"].ToString() == "1")
            {
                s += "<li>性别:<select id='selEditSex'><option value=''>请选择</option><option value='0'>女</option><option value='1' selected='selected'>男</option></select></li>";
            }
            else
            {
                s += "<li>性别:<select id='selEditSex'><option value=''  selected='selected'>请选择</option><option value='0'>女</option><option value='1'>男</option></select></li>";
            }
            s += "<li>年龄:<input type='text' id='txtEditAge'  value='" + dt.Rows[0]["Age"].ToString() + "' /></li>";
            if (dt.Rows[0]["Birthday"].ToString() == "" || dt.Rows[0]["Birthday"].ToString() == null)
            {
                s += " <li>生日:<input type='date' id='txtEditBirthday' /></li>";
            }
            else
            {
                s += " <li>生日:<input type='date' id='txtEditBirthday'  value='" + Convert.ToDateTime(dt.Rows[0]["Birthday"].ToString()).ToString("yyyy-MM-dd") + "' /></li>";
            }

            s += "<li>身份证:<input type='text' id='txtEditIDNumber'  value='" + dt.Rows[0]["IDNumber"].ToString() + "' /></li>";
            s += "<li>电话:<input type='text' id='txtEditTelephone'  value='" + dt.Rows[0]["Telephone"].ToString() + "' /></li>";
            s += "<li>手机号:<input type='text' id='txtEditPhone'  value='" + dt.Rows[0]["Phone"].ToString() + "' /></li>";
            s += "<li>微信:<input type='text' id='txtEditWeixin'  value='" + dt.Rows[0]["Weixin"].ToString() + "' /></li>";
            s += "<li>QQ:<input type='text' id='txtEditQQNo'  value='" + dt.Rows[0]["QQNo"].ToString() + "' /></li>";
            s += "<li>用户等级:<input type='text' id='txtEditCustomerLevel'  value='" + dt.Rows[0]["CustomerLevel"].ToString() + "' /></li>";
            s += "<li class='liRemark'>备注:<textarea id='txtEditRemark'>" + dt.Rows[0]["Remark"].ToString() + "</textarea></li>";
            s += "</ul>";
            return s;
        }
        /// <summary>
        /// 保存编辑客户信息
        /// </summary>
        /// <returns></returns>
        public string SaveEdit()
        {
            string s = string.Empty;
            string[] list = Request.Form["list[]"].ToString().Split(',');
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", list[0]);
            Dic.Add("CustomerId", list[1]);
            Dic.Add("Shop", list[2]);
            Dic.Add("Contactperson", list[3]);
            Dic.Add("Sex", list[4]);
            Dic.Add("Age", list[5]);
            Dic.Add("Birthday", list[6]);
            Dic.Add("IDNumber", list[7]);
            Dic.Add("Telephone", list[8]);
            Dic.Add("Phone", list[9]);
            Dic.Add("Weixin", list[10]);
            Dic.Add("QQNo", list[11]);
            Dic.Add("CustomerLevel", list[12]);
            Dic.Add("Remark", list[13]);
            return bll.SaveEdit(Dic, userInfo.User.Id);
        }
        /// <summary>
        /// 显示客户地址信息
        /// </summary>
        /// <returns></returns>
        public string CustomerAddress()
        {
            string s = string.Empty;
            DataTable dt = new DataTable();
            string CustomerId = Request.Form["CustomerId"].ToString();
            dt = bll.CustomerAddress(CustomerId);

            s += "<table id='TBAddress' class='mytable'>";
            s += "<tr><th>编号</th><th>省</th><th>市</th><th>区/县</th><th>客户地址</th><th>操作</th></tr>";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += "<tr>";
                    s += "<td>" + Convert.ToInt32(i + 1) + "</td>";
                    s += "<td class='tdProvinces'>" + dt.Rows[i]["Provinces"] + "</td>";
                    s += "<td class='tdCity'>" + dt.Rows[i]["City"] + "</td>";
                    s += "<td class='tdDistrict'>" + dt.Rows[i]["District"] + "</td>";
                    s += "<td class='tdCusAddress'>" + dt.Rows[i]["CusAddress"] + "</td>";
                    s += "<td title='" + dt.Rows[i]["Id"] + "'><a href='#' onclick='EditAddress(this)' >编辑</a><a href='#' onclick='DeleteAddress(\"" + dt.Rows[i]["Id"] + "\")'>删除</a></td>";
                    s += "</tr>";
                }
            }
            s += "<tr class='trAddAddress'><td colspan='99'><a href='#' onclick='AddAddress()'>添加新地址</a></td></tr>";
            s += "</table>";

            return s;
        }
        /// <summary>
        /// 添加客户地址
        /// </summary>
        /// <returns></returns>
        public string SaveAddAddress()
        {
            string s = string.Empty;
            string[] list = Request.Form["list[]"].ToString().Split(',');
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("CustomerId", list[0]);
            Dic.Add("Provinces", list[1]);
            Dic.Add("City", list[2]);
            Dic.Add("District", list[3]);
            Dic.Add("CusAddress", list[4]);
            return bll.SaveAddAddress(Dic, userInfo.User.Id);
        }
        /// <summary>
        /// 修改客户地址 
        /// </summary>
        /// <returns></returns>
        public string SaveEditAddress()
        {
            string s = string.Empty;
            string[] list = Request.Form["list[]"].ToString().Split(',');
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Id", list[0]);
            Dic.Add("Provinces", list[1]);
            Dic.Add("City", list[2]);
            Dic.Add("District", list[3]);
            Dic.Add("CusAddress", list[4]);
            return bll.SaveEditAddress(Dic, userInfo.User.Id);
        }
        /// <summary>
        /// 删除客户地址
        /// </summary>
        /// <returns></returns>
        public string DeleteAddress()
        {
            string s = string.Empty;
            string Id = Request.Form["Id"].ToString();
            return bll.DeleteAddress(Id, userInfo.User.Id);
        }
        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <returns></returns>
        public string DeleteCustomer()
        {
            string s = string.Empty;
            string Id = Request.Form["Id"].ToString();
            return bll.DeleteCustomer(Id, userInfo.User.Id);
        }


        /// <summary>
        /// 查看客户订单信息
        /// </summary>
        /// <returns></returns>
        public string CheckOrder()
        {
            StringBuilder s = new StringBuilder();
            string CustomerId = Request.Form["CustomerId"].ToString();
            DataTable dt = bll.CheckOrder(CustomerId);
            string[] ssName = { "OrderId", "Scode", "BrandName", "Color", "TypeName", "Size", "Imagefile", "Number", "SellPrice", "Def1" };
            string[] ss = { "OrderId", "Scode", "BrandName", "Color", "TypeName", "Size", "Imagefile", "Number", "SellPrice", "Def1" };
            s.Append("<tr>");
            for (int z = 0; z < ssName.Length; z++)
            {

                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");

                    if (ssName[z] == "OrderId")
                        s.Append("订单编号");
                    if (ssName[z] == "Scode")
                        s.Append("货品编号");
                    if (ssName[z] == "Brand")
                        s.Append("品牌");
                    if (ssName[z] == "TypeName")
                        s.Append("类别");
                    if (ssName[z] == "Color")
                        s.Append("颜色");
                    if (ssName[z] == "Size")
                        s.Append("尺码");
                    if (ssName[z] == "Imagefile")
                        s.Append("商品示图");
                    if (ssName[z] == "Number")
                        s.Append("数量");
                    if (ssName[z] == "SellPrice")
                        s.Append("成交金额");
                    if (ssName[z] == "Def1")
                        s.Append("发货状态");
                    s.Append("</th>");
                }
            }
            s.Append("</tr>");

            #region TABLE内容
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ss.Contains(ssName[j]))
                    {
                        s.Append("<td>");
                        if (ssName[j].ToLower() == "def1")
                        {
                            string item = dt.Rows[i][ssName[j]].ToString();
                            string Def1 = item == "1" ? "未发货" : item == "2" ? "已发货" : item == "3" ? "退货中" : item == "4" ? "退货完成" : item == "5" ? "退款中" : item == "6" ? "退款完成" : item == "7" ? "交易成功" : "";
                            s.Append(Def1);
                        }
                        else if (ssName[j].ToLower() == "imagefile")
                        {
                            s.Append("<img src=\"" + dt.Rows[i][ssName[j]].ToString() + "\" onerror='errorImg(this)' />");
                        }
                        else
                        {
                            s.Append(dt.Rows[i][ssName[j]].ToString());
                        }

                        s.Append("</td>");
                    }
                }
                s.Append("</tr>");
            }
            #endregion

            return s.ToString();
        }

        /// <summary>
        /// 导入客户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult InsertOrUpdateView()
        {
            try
            {
                //int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                //int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;

                //#region 查询
                //if (!ph.isFunPermisson(roleId, menuId, funName.selectName))
                //{
                //    return View("../NoPermisson/Index");
                //}
                //#endregion
                //ViewData["myMenuId"] = menuId;
                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }


        /// <summary>
        /// 导入excell中的客户信息到custom表
        /// </summary>
        /// <returns></returns>
        public string UploadExcel()
        {
            try
            {
                string s = "导入成功!";
                DataTable dt = new DataTable();
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                HttpPostedFileBase file1 = Request.Files[0];
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss");
                string filePath = newFile + file1.FileName;//生成新名字
                file1.SaveAs(Server.MapPath("~/Uploadxls/" + filePath));//存到本地
                dt = GetExcellData(Server.MapPath("~/Uploadxls/" + filePath), "Sheet1$").Tables[0];//把excel转换成datatable
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() != "")
                    {
                        
                        int check = context.custom.Where(a => a.CustomerId == dt.Rows[i][0].ToString()).Count();//查询是否存在

                        if (check > 0)
                        {
                            s += dt.Rows[i][0] + ":已存在<br />";
                        }
                        else
                        {
                            custom cu = new custom()
                            {
                                CustomerId = dt.Rows[i][0].ToString(),
                                Shop = dt.Rows[i][1].ToString(),
                                Contactperson = dt.Rows[i][2].ToString(),
                                Sex = dt.Rows[i][3].ToString() == "女" ? 0 : 1,
                                Age = Convert.ToInt32(dt.Rows[i][4].ToString()),
                                Birthday = Convert.ToDateTime(dt.Rows[i][5].ToString()).ToString("yyyy-MM-dd"),
                                IDNumber = dt.Rows[i][6].ToString(),
                                Telephone = dt.Rows[i][7].ToString(),
                                Phone = dt.Rows[i][8].ToString(),
                                Weixin = dt.Rows[i][9].ToString(),
                                QQNo = dt.Rows[i][10].ToString(),
                                CustomerLevel = dt.Rows[i][11].ToString(),
                                Remark = dt.Rows[i][12].ToString(),
                                CustomerServiceId = Convert.ToInt32(dt.Rows[i][13].ToString()),
                            };
                            context.custom.InsertOnSubmit(cu);
                            context.SubmitChanges();
                        }
                    }
                }
                return s;
            }
            catch(Exception ex)
            {
                return "导入失败!";
            }
          
        }
        /// <summary>
        /// 获取excell中的数据 转换成dataset
        /// </summary>
        /// <param name="filename"></param>//文件路径
        /// <param name="workTableName"></param>//excel表格
        /// <returns></returns>
        public DataSet GetExcellData(string filename, string workTableName)
        {
            DataSet ds;
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Extended Properties=Excel 8.0;" + "data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = " SELECT * FROM [" + workTableName + "]";
            try
            {
                Open(myConn);//打开数据库
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                ds = new DataSet();
                myCommand.Fill(ds);
                myCommand.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Close(myConn);
            }
        }
        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="con"></param>
        private void Open(OleDbConnection con)
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <param name="con"></param>
        private void Close(OleDbConnection con)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public ActionResult CustomPropertyView()
        {
            try
            {
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
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


    }
}
