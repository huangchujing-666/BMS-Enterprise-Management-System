using Aliyun.OpenServices.OpenStorageService;
using Maticsoft.DBUtility;
using Newtonsoft.Json;
using pbxdata.bll;
using pbxdata.dal;
using pbxdata.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace pbxdata.web.Controllers
{
    public class ProductController : BaseController
    {
        ProductHelper phb = new ProductHelper();
        aliyun.ControlAliyun CAliyun = new aliyun.ControlAliyun();
        bll.ProductBll pro = new bll.ProductBll();
        bll.Attributebll attrbll = new bll.Attributebll();
        model.AttributeModel Attrmm = new AttributeModel();
        model.product Promm = new product();
        PublicHelpController ph = new PublicHelpController();
        static List<model.product> list = null;
        static List<AttributeModel> list1 = null;
        DataTable dt = new DataTable();
        static int pageIndex = 1;
        static int pageCount = 0;
        int count = 20;


        #region ProductShow
        /// <summary>
        /// 是否存在查看菜单的权限
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult ProductShow()
        {
            try
            {
                PublicHelpController ph = new PublicHelpController();
                int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
                int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
                #region 查询
                if (!ph.isFunPermisson(roleId, menuId, funName.selectName))                       //获取是否存在查询权限
                {
                    return View("../NoPermisson/Index");
                }
                #endregion
                string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);           //获取字段权限（能查看哪些字段）
                ViewBag.ShowSearch1 = SearchList1(ss);
                ViewBag.ShowSearch = SearchList(ss, menuId);
                ViewData["myMenuId"] = menuId;
                DeleteDir("123");
                return View();
            }
            catch
            {
                return View("../ErrorMsg/Index");
            }
        }


        /// <summary>
        /// 判断搜索条件--根据权限显示查询--款号表
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public string SearchList1(string[] ss)
        {
            StringBuilder s = new StringBuilder(200);
            foreach (var temp in ss)
            {
                if (temp == "Style")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>款号：</span><input class='spanPropertyValue' id='txtStyle1' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>品牌：</span><input class='spanPropertyValue' onclick='GetDropToBrand(this.id)' id='txtCat11' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat2")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>类别：</span><input class='spanPropertyValue' onclick='GetDropToType(this.id)' id='txtCat21' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Balance")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>库存：</span><input type='text' id='txtBalance11'  class='spanPropertyValue'  style='width: 50px' />-<input type='text' id='txtBalance21'  class='spanPropertyValue'  style='width: 50px' /></span>");
                }
            };
            s.Append("<span class='spanProperty'><span class='spanPropertyName'>图片：</span><select id='selPicExist' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>有</option><option value='0'>无</option></select></span>");
            tbProductReMarkBll tpmb = new tbProductReMarkBll();
            DataTable dt = tpmb.GetShopName();
            s.Append("<span class='spanProperty'><span class='spanPropertyName'>店铺：</span><select id='ShopName' class='spanPropertyValue'><option value=''>请选择</option>");
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                //value值以及选项都是tb店铺名称
                s.Append("<option value='" + dt.Rows[j]["ProductShopName"] + "'>" + dt.Rows[j]["ProductShopName"] + "</option>");
            }
            s.Append("<option value='0'>已上架</option>");
            s.Append("<option value='1'>未上架</option>");
            s.Append("</select></span>");
            s.Append("<div class='clearfix'></div>");
            s.Append("<input type='button' value=' 查  询 ' class='spanPropertySearch' onclick='SearchbyStyle()' />");
            return s.ToString();

        }

        /// <summary>
        /// 判断搜索条件--根据权限显示查询--货号表
        /// </summary>
        /// <param name="Id">条件</param>
        /// <returns></returns>
        public string SearchList(string[] ss, int menuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            StringBuilder s = new StringBuilder(200);
            foreach (var temp in ss)
            {
                if (temp == "Style")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>款号：</span><input class='spanPropertyValue' id='txtStyle' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Scode")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>货号：</span><input class='spanPropertyValue' id='txtScode' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Descript" && temp == "Cdescript")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>名称：</span><input class='spanPropertyValue' id='txtName' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat1")
                {

                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>季节：</span><input class='spanPropertyValue' id='txtCat1' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Clolor")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>颜色：</span><input class='spanPropertyValue' id='txtColor' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>品牌：</span><input class='spanPropertyValue' onclick='GetDropToBrand(this.id)' id='txtCat' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Cat2")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>类别：</span><input class='spanPropertyValue' onclick='GetDropToType(this.id)' id='txtCat2' type='text'></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Balance")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>库存：</span><input type='text' id='txtBalance'  class='spanPropertyValue'  style='width: 50px' />-<input type='text' id='txtBalance2'  class='spanPropertyValue'  style='width: 50px' /></span>");
                }
            };

            foreach (var temp in ss)
            {
                if (temp == "Lastgrnd")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>时间：</span><input type='date' id='txtLastgrnd1'  class='spanPropertyValue' style='width:110px; padding:0 0;' />-<input type='date' id='txtLastgrnd2'  class='spanPropertyValue' style='width:110px; padding:0 0;'/></span>");
                }
            };
            s.Append("<span class='spanProperty'><span class='spanPropertyName'>属性：</span><select id='selAttrExistScode' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>有</option><option value='0'>无</option></select></span>");
            s.Append("<span class='spanProperty'><span class='spanPropertyName'>图片：</span><select id='selPicExistScode' class='spanPropertyValue'><option value=''>请选择</option><option value='1'>有</option><option value='0'>无</option></select></span>");

            foreach (var temp in ss)
            {

                if (temp == "Def1")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>图片时间：</span><input type='date' id='txtPictime'  class='spanPropertyValue' style='width:110px; padding:0 0;' />-<input type='date' id='txtPictime2'  class='spanPropertyValue' style='width:110px; padding:0 0;' /></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "userRealName")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>图片操作人：</span><select id='txtUserName' class='spanPropertyValue'>" + UsersNameDDlist() + "</select></span>");
                }
            };
            foreach (var temp in ss)
            {
                if (temp == "Def9")
                {
                    s.Append("<span class='spanProperty'><span class='spanPropertyName'>图片规范：</span><select id='selPicCheck' class='spanPropertyValue'><option value='' selected='selected' >请选择</option><option value='0'>未检查</option><option  value='1'>合格</option><option value='2'>不合格</option></select></span>");
                }
            };
            s.Append("<span class='spanProperty'><span class='spanPropertyName'>商品信息图：</span><select id='selSciqProductId' class='spanPropertyValue'><option value=''>请选择</option><option value='3'>不合格</option><option value='1'>合格</option><option value='2'>未标记</option></select></span>");
            s.Append("<div class='clearfix'></div>");
            s.Append(" <input type='button' value=' 查  询 ' class='spanPropertySearch' onclick='SearchbyScode()' />");
            //s.Append(" <input type='button' value='导出数据' class='spanPropertySearch' onclick='OutPutExcel()' />");
            return s.ToString();
        }

        /// <summary>
        /// 刚进入的适合删除上传到服务器上缓存文件
        /// </summary>
        public void DeleteDir(string aimPath)
        {
            try
            {

                aimPath = Server.MapPath("~/UploadImage/");
                long a = 0;


                if (Directory.Exists(aimPath))
                {
                    DirectoryInfo di = new DirectoryInfo(aimPath);
                    // 检查目标目录存放文件达到100M就删除所有图片
                    foreach (var i in di.GetFiles())
                    {
                        a += i.Length;
                    }
                    if (a > 100000000)
                    {
                        // 检查目标目录是否以目录分割字符结束如果不是则添加之
                        if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                            aimPath += Path.DirectorySeparatorChar;
                        // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                        // 如果你指向Delete目标文件下面的文件而不包含目录请使用下面的方法
                        // string[] fileList = Directory.GetFiles(aimPath);
                        string[] fileList = Directory.GetFileSystemEntries(aimPath);
                        // 遍历所有的文件和目录
                        foreach (string file in fileList)
                        {

                            // 先当作目录处理如果存在这个目录就递归Delete该目录下面的文件
                            if (Directory.Exists(file))
                            {
                                DeleteDir(aimPath + Path.GetFileName(file));
                            }
                            //否则直接Delete文件
                            else
                            {
                                System.IO.File.Delete(aimPath + Path.GetFileName(file));
                            }
                        }
                    }


                }
                else
                {
                    Directory.CreateDirectory(aimPath);
                }

            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        /// <summary>
        /// 条件搜索主货号下表
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="menuId"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string OnSeach(string lists, string menuId, string page, string Selpages)
        {
            #region 参数
            string[] SearchInfo = lists.Split(',');
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Style", SearchInfo[0]);
            Dic.Add("Scode", SearchInfo[1]);
            Dic.Add("Name", SearchInfo[2]);
            Dic.Add("Cat", SearchInfo[3]);
            Dic.Add("Cat1", SearchInfo[4]);
            Dic.Add("Cat2", SearchInfo[5]);
            Dic.Add("MinBalance", SearchInfo[6]);
            Dic.Add("MaxBalance", SearchInfo[7]);
            Dic.Add("MinPricea", SearchInfo[8]);
            Dic.Add("MaxPricea", SearchInfo[9]);
            Dic.Add("MinLastgrnd", SearchInfo[10]);
            Dic.Add("MaxLastgrnd", SearchInfo[11]);
            Dic.Add("AttrState", SearchInfo[12]);
            Dic.Add("PicState", SearchInfo[13]);
            Dic.Add("MinPictime", SearchInfo[14]);
            Dic.Add("MaxPictime", SearchInfo[15]);
            Dic.Add("CustomerId", userInfo.User.Id.ToString());
            Dic.Add("UserId", SearchInfo[16]);
            Dic.Add("Def9", SearchInfo[17]);
            Dic.Add("Color", SearchInfo[18]);
            Dic.Add("Def11", SearchInfo[19]);
            Dic.Add("ciqProductId", SearchInfo[20]);

            int isReset = helpcommon.ParmPerportys.GetNumParms(SearchInfo[21]);                                                                //查看本次操作是重新查询还是翻页
            #endregion


            string counts, Balcounts;//--返回总数以及库存
            if (page == "0")//通过传入的page判断查询(分页)
            {
                dt = pro.SearchProduct(Dic, 1, Convert.ToInt32(Selpages), out counts, out Balcounts, isReset);
            }
            else
            {
                dt = pro.SearchProduct(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts, out Balcounts, isReset);
            }
            return getDataProduct(dt, menuId) + "-*-" + counts + "-*-" + Balcounts;
        }
        /// <summary>
        /// 条件搜索主款号上表
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="menuId"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string OnSeach1(string lists, string menuId, string page, string Selpages)
        {
            #region 参数
            string[] SearchInfo = lists.Split(',');
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Style", SearchInfo[0]);
            Dic.Add("Cat", SearchInfo[1]);
            Dic.Add("Cat2", SearchInfo[2]);
            Dic.Add("MinBalance", SearchInfo[3]);
            Dic.Add("MaxBalance", SearchInfo[4]);
            Dic.Add("MinPricea", SearchInfo[5]);
            Dic.Add("MaxPricea", SearchInfo[6]);
            Dic.Add("PicExist", SearchInfo[7]);
            Dic.Add("CustomerId", userInfo.User.Id.ToString());
            Dic.Add("ShopName", SearchInfo[8]);

            int isReset = helpcommon.ParmPerportys.GetNumParms(SearchInfo[9]);                                                                //查看本次操作是重新查询还是翻页
            #endregion

            string counts, Balcounts;                                                                                                         //商品数据总条数和库存总量
            if (page == "0")
            {
                dt = pro.SearchProduct1(Dic, 1, Convert.ToInt32(Selpages), out counts, out Balcounts, isReset);
            }
            else
            {
                dt = pro.SearchProduct1(Dic, Convert.ToInt32(page), Convert.ToInt32(Selpages), out counts, out Balcounts, isReset);
            }
            string s = getDataProduct1(dt, menuId);                                                                                          //table字符串（商品数据信息）


            return s + "-*-" + counts + "-*-" + Balcounts;
        }


        #region 初始化下拉框数据
        /// <summary>
        /// 品牌下拉列表
        /// </summary>
        /// <returns></returns>
        public string ProductStyleDDlist()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetCatDDlist(userInfo.User.Id.ToString());
            s.Append("<option value=''>请选择</option>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                //value为品牌缩写，选项为品牌名称
                s.Append("<option value='" + dt1.Rows[i][1].ToString() + "'>" + dt1.Rows[i][0].ToString() + "</option>");
            }
            return s.ToString();
        }
        /// <summary>
        /// 类别下拉列表
        /// </summary>
        /// <returns></returns>
        public string ProductTypeDDlist()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetTypeDDlist(userInfo.User.Id.ToString());
            s.Append("<option value=''>请选择</option>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                //value存类别编码        选项存类别名称
                s.Append("<option value='" + dt1.Rows[i][1].ToString() + "'>" + dt1.Rows[i][0].ToString() + "</option>");
            }
            return s.ToString();
        }
        /// <summary>
        /// 操作下拉列表
        /// </summary>
        /// <returns></returns>
        public string UsersNameDDlist()
        {
            StringBuilder s = new StringBuilder(200);
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query(@"select * from users where personaId in(1,3,4)").Tables[0];
            s.Append("<option value=''>请选择</option>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                //用户id                  用户名称
                s.Append("<option value='" + dt1.Rows[i]["Id"].ToString() + "'>" + dt1.Rows[i]["userRealName"].ToString() + "</option>");
            }
            return s.ToString();
        }
        #endregion



        /// <summary>
        /// Product界面上表主款号
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataProduct1(DataTable dt, string FmenuId)
        {
            StringBuilder s = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);                                                                         //角色编号
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);        //菜单编号

            string[] ssName = { "stylePicSrc", "Style", "Cat", "Cat2", "Balance" };
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);                                                                             //获取有权限的字段

            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            s.Append("<th>缩略图</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "Style")
                        s.Append("款号");
                    if (ssName[z] == "Cat")
                        s.Append("品牌");
                    if (ssName[z] == "Cat2")
                        s.Append("类别");
                    if (ssName[z] == "Balance")
                        s.Append("总库存");
                    if (ssName[z] == "Pricea")
                        s.Append("价格");
                    s.Append("</th>");
                }
            }
            s.Append("<th>上架店铺</th>");
            s.Append("<th>操作</th>");
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count <= 0 || dt == null)
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
                        if (ssName[j] == "stylePicSrc")
                        {
                            if (dt.Rows[i][ssName[j]].ToString() != "")
                            {
                                s.Append("<td><img src='" + dt.Rows[i][ssName[j]].ToString() + "' onerror='errorImg(this)' style='width:60px;height:60px'/></td>");
                            }
                            else
                            {
                                s.Append("<td></td>");
                            }
                        }
                        if (ss.Contains(ssName[j]))
                        {

                            if (ssName[j] == "Cat2")
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i]["TypeName"].ToString());
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
                    DataTable dtShop = pro.GetShopNameByStyle(dt.Rows[i]["Style"].ToString());
                    string shop = "";
                    for (int cc = 0; cc < dtShop.Rows.Count; cc++)
                    {
                        if (!shop.Contains(dtShop.Rows[cc]["ProductShopName"].ToString()))
                        {
                            shop += ",<a href=http://item.taobao.com/item.htm?id=" + dtShop.Rows[cc]["ProductReMarkId"] + " target='_bank'>" + dtShop.Rows[cc]["ProductShopName"].ToString() + "</a>";
                        }
                    }
                    s.Append("<td>" + shop.Trim(',') + "</td>");


                    #region 编辑
                    s.Append("<td>");

                    s.Append("<a href='#' onclick='ScodeInfo(\"" + dt.Rows[i]["Style"].ToString() + "\")'>查看</a>");
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))//--判断是可编辑还是查看功能
                    {
                        s.Append("<a href='#' onclick='styledescript(\"" + dt.Rows[i]["Style"].ToString() + "\",\"edit\")'>编辑</a>");
                    }
                    else
                    {
                        s.Append("<a href='#' onclick='styledescript(\"" + dt.Rows[i]["Style"].ToString() + "\",\"check\")'>编辑</a>");
                    }

                    #endregion
                    #region 上货
                    if (ph.isFunPermisson(roleId, menuId, funName.ShowGoods))
                    {
                        s.Append("<a href='#' onclick='ShowShop(\"" + dt.Rows[i]["Style"].ToString() + "\")'>上货</a>");
                    }
                    #endregion

                    s.Append("</td>");
                    s.Append("</tr>");
                }
            }

            #endregion
            return s.ToString();
        }
        /// <summary>
        /// Product界面下表主货号
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataProduct(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            PublicHelpController ph = new PublicHelpController();
            //bll.usersbll usersBll = new bll.usersbll();
            //string[] ssName = usersBll.getDataName("product");
            string[] ssName = { "Imagefile", "Style", "Model", "Scode", "Descript", "Cdescript", "Cat", "Cat1", "Cat2", "Clolor", "Size", "Balance", "Lastgrnd", "Def1", "userRealName", "Def9", "Def10", "ciqProductId" };
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {

                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");

                    if (ssName[z] == "Imagefile")
                        s.Append("缩略图");
                    if (ssName[z] == "ciqProductId")
                        s.Append("商品信息图");
                    if (ssName[z] == "Style")
                        s.Append("款号");
                    if (ssName[z] == "Model")
                        s.Append("型号");
                    if (ssName[z] == "Scode")
                        s.Append("货品编号");
                    if (ssName[z] == "Descript")
                        s.Append("英文名称");
                    if (ssName[z] == "Cdescript")
                        s.Append("中文名称");
                    if (ssName[z] == "Cat")
                        s.Append("品牌");
                    if (ssName[z] == "Cat1")
                        s.Append("季节");
                    if (ssName[z] == "Cat2")
                        s.Append("类别");
                    if (ssName[z] == "Clolor")
                        s.Append("颜色");
                    if (ssName[z] == "Size")
                        s.Append("尺寸");
                    //if (ssName[z] == "Pricea")
                    //    s.Append("吊牌价");
                    //if (ssName[z] == "Priceb")
                    //    s.Append("零售价");
                    //if (ssName[z] == "Pricec")
                    //    s.Append("VIP价");
                    //if (ssName[z] == "Priced")
                    //    s.Append("批发价");
                    //if (ssName[z] == "Pricee")
                    //    s.Append("成本价");
                    if (ssName[z] == "Loc")
                        s.Append("店铺");
                    if (ssName[z] == "Balance")
                        s.Append("库存");
                    if (ssName[z] == "Lastgrnd")
                        s.Append("收货日期");
                    if (ssName[z] == "Def1")
                        s.Append("图片日期");
                    if (ssName[z] == "userRealName")
                        s.Append("图片操作人");
                    if (ssName[z] == "Def9")
                        s.Append("图片规范");
                    if (ssName[z] == "Def10")
                        s.Append("条形码");
                    //if (ssName[z] == "Def1")
                    //    s.Append("属性状态");
                    //if (ssName[z] == "Def2")
                    //    s.Append("图片状态");
                    s.Append("</th>");
                }
            }
            //s.Append("<th>商检</th>");
            s.Append("<th>操作</th>");
            #endregion
            s.Append("</tr>");


            #region TABLE内容
            if (dt.Rows.Count <= 0 || dt == null)
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
                            if (ssName[j] == "Cat2")
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i]["TypeName"].ToString());
                                s.Append("</td>");
                            }
                            else if (ssName[j] == "Imagefile")
                            {
                                if (dt.Rows[i][ssName[j]].ToString() != "")
                                {
                                    s.Append("<td><img src='" + dt.Rows[i][ssName[j]].ToString() + "' onerror='errorImg(this)' style='width:60px;height:60px'/></td>");
                                }
                                else
                                {
                                    s.Append("<td></td>");
                                }
                            }
                            else if (ssName[j] == "Def9")
                            {
                                string item = dt.Rows[i]["Def9"].ToString() == "0" ? "未检查" : dt.Rows[i]["Def9"].ToString() == "1" ? "合格" : dt.Rows[i]["Def9"].ToString() == "2" ? "不合格" : "无图";
                                s.Append("<td>" + item + "</td>");
                            }
                            else if (ssName[j] == "ciqProductId")
                            {
                                string item = dt.Rows[i]["ciqProductId"].ToString() == "3" ? "不合格" : dt.Rows[i]["ciqProductId"].ToString() == "1" ? "合格" : "未标记";
                                s.Append("<td>" + item + "</td>");
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
                    s.Append("<td>");
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<a href='#' onclick='EditProduct(\"" + dt.Rows[i]["Scode"].ToString() + "\",\"edit\")'>编辑</a>");
                        s.Append("<a href='#' onclick='EditPicture(\"" + dt.Rows[i]["Scode"].ToString() + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\")'>编辑图片</a>");
                    }
                    else
                    {
                        s.Append("<a href='#' onclick='EditProduct(\"" + dt.Rows[i]["Scode"].ToString() + "\",\"check\")'>查看</a>");
                    }

                    #endregion

                    s.Append("</td>");
                    s.Append("</tr>");
                }
            }

            #endregion
            return s.ToString();
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        /// type //--判断是否可编辑
        /// unitNo //--单位编号
        /// <returns></returns>
        public string apiUnitDDlist(string unitNo, string type)
        {
            StringBuilder s = new StringBuilder(200);
            //string s = string.Empty;
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query(@"select * from apiUnit").Tables[0];
            s.Append("<select id='selapiUnit'  style='width:150px'>");

            if (type == "edit")
            {
                s.Append("<option value=''>请选择</option>");
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (unitNo == dt1.Rows[i]["unitNo"].ToString())
                    {
                        s.Append("<option value='" + dt1.Rows[i]["unitNo"].ToString() + "' selected='selected' >" + dt1.Rows[i]["unitName"].ToString() + "</option>");
                    }
                    else
                    {
                        s.Append("<option value='" + dt1.Rows[i]["unitNo"].ToString() + "'>" + dt1.Rows[i]["unitName"].ToString() + "</option>");
                    }
                }
            }
            else
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (unitNo == dt1.Rows[i]["unitNo"].ToString())
                    {
                        s.Append("<option value='" + dt1.Rows[i]["unitNo"].ToString() + "' selected='selected' >" + dt1.Rows[i]["unitName"].ToString() + "</option>");
                        break;
                    }
                }
            }
            s.Append("</select>");
            return s.ToString();

        }
        /// <summary>
        /// 获取图片是否合格下拉框
        /// </summary>
        /// 
        public string PicCheckDDlList(string state)
        {
            string s = "";
            if (state == "0")
            {
                s = "<select id='PicChecklist'><option value=''>请选择</option><option value='0' selected='selected' >未检查</option><option  value='1'>合格</option><option value='2'>不合格</option></select>";
            }
            else if (state == "1")
            {
                s = "<select id='PicChecklist'><option value=''>请选择</option><option value='0'>未检查</option><option  value='1' selected='selected' >合格</option><option value='2'>不合格</option></select>";
            }
            else if (state == "2")
            {
                s = "<select id='PicChecklist'><option value=''>请选择</option><option value='0'>未检查</option><option  value='1'>合格</option><option value='2' selected='selected' >不合格</option></select>";
            }
            else
            {
                s = "<select id='PicChecklist'><option value='' selected='selected' >请选择</option><option value='0'>未检查</option><option  value='1'>合格</option><option value='2'>不合格</option></select>";
            }
            return s;
        }
        /// <summary>
        /// 获取指定商品信息进行编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string EditProduct()
        {
            //ViewBag.asas = id;

            //string id = Request.QueryString["id"];
            string scode = Request.QueryString["scode"];
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            string strsql = @"select * from product where scode='" + scode + "'";
            dt1 = DbHelperSQL.Query(@"select * from scodepic where scode='" + scode + "' and Left(Def1,1)='2' and Def4='1' order by Left(Def1,2)").Tables[0];//获取当前货号的Def1字段第一个字符为2（主图）  同事Def4为1正常图片
            dt = DbHelperSQL.Query(strsql).Tables[0];
            string a = dt.Columns[0].ToString();
            string s = "";

            if (Request.QueryString["type"].ToString() == "edit")//--判断是否可编辑
            {
                s = string.Format(@"<table id='tabProperty'>
                    <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><div style='position:relative'><div id='divThumbnail' onclick='DeleteThumbnail()'>×</div><img src='{11}' style='width:120px;' /></div></td></tr>
                    <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' type='text' value='{2}' /></td></tr>
                    <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                        <td>季节：<input id='txtCat1E' type='text' value='{4}' /></td></tr>
                    <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                        <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
                    <tr><td>预警库存：<input id='txtRolevelE' type='text' value='{9}' /></td>
                        <td>类别：{5}<input type='hidden' id='hidId'' value='{10}' /></td></tr>
                    <tr><td>商品信息图：<select id='txtciqProductId'  style='width:150px'>{12}</select></td>
                        <td>规格型号：<input id='txtciqSpec' type='text' value='{13}' disabled='disabled'  /></td>
                        <td>图片是否合格:{17}</td></tr>
                    <tr><td>商品HS编码：<input id='txtciqHSNo' type='text' value='{14}' disabled='disabled' /></td>
                        <td><div style='position:relative'>原产国/地区：<input type='text' id='txtciqAssemCountry' value='{22}' disabled='disabled' ><select id='selciqAssemCountry' style='width:150px' >{15}</selected><div></td>
                        <td>商品序号：<input type='text' value='{16}' disabled='disabled' /></td></tr>
                   <tr><td>条形码：<input id='txtDef10' type='text' value='{18}' disabled='disabled'  /></td>
                       <td>计量单位: {19}</td>
                       <td>毛重：<input id='txtDef15' type='text' value='{20}' disabled='disabled'  /></td></tr>
                   <tr><td>净重：<input id='txtDef16' type='text' value='{21}'  disabled='disabled' /></td>
                       <td></td>
                       <td></td></tr>
           </table>", dt.Rows[0]["Scode"].ToString()
                , dt.Rows[0]["Descript"].ToString()
                , dt.Rows[0]["Cdescript"].ToString()
                , dt.Rows[0]["Cat"].ToString()
                , dt.Rows[0]["Cat1"].ToString()
                , GetDDlist(dt.Rows[0]["Cat2"].ToString())
                , dt.Rows[0]["Clolor"].ToString()
                , dt.Rows[0]["Style"].ToString()
                , dt.Rows[0]["Size"].ToString()
                , dt.Rows[0]["Rolevel"].ToString()
                , "1"
                , dt.Rows[0]["Imagefile"].ToString()
                , ProductInfoPic(dt.Rows[0]["ciqProductId"].ToString())
                , dt.Rows[0]["ciqSpec"].ToString()
                , dt.Rows[0]["ciqHSNo"].ToString()
                , GetCountrylist("", dt.Rows[0]["ciqAssemCountry"].ToString())
                , dt.Rows[0]["ciqProductNo"].ToString()
                , PicCheckDDlList(dt.Rows[0]["Def9"].ToString())
                , dt.Rows[0]["Def10"].ToString()
                , apiUnitDDlist(dt.Rows[0]["QtyUnit"].ToString(), "1")
                , dt.Rows[0]["Def15"].ToString()
                , dt.Rows[0]["Def16"].ToString()
                , dt.Rows[0]["ciqAssemCountry"].ToString()
             );
            }
            else
            {
                s = string.Format(@"<table id='tabProperty'>
            <tr><td>货品编号：<input id='txtScodeE' type='text' value='{0}' disabled='disabled' /></td><td>款号：<input type='text' value='{7}' disabled='disabled' /></td><td rowspan='5'><img src='{11}' style='width:120px;' /></td></tr>
            <tr><td>英文名称：<input id='Descript'  type='text' value='{1}' disabled='disabled' /></td><td>中文名称：<input id='txtCdescriptE' type='text' value='{2}' /></td></tr>
            <tr><td>品牌：<input type='text' value='{3}' disabled='disabled' /></td>
                <td>季节：<input type='text' value='{4}' disabled='disabled' /></td></tr>
            <tr><td>颜色：<input type='text' value='{6}' disabled='disabled' /></td>
                <td>尺寸：<input type='text' value='{8}' disabled='disabled' /></td></tr>
            <tr><td>预警库存：<input id='txtRolevelE' type='text' value='{9}' /></td><td>类别：<input type='text' value='{5}' disabled='disabled' /><input type='hidden' id='hidId'' value='{10}' /></td></tr>
            <tr><td>商品申请编号：<input type='text' value='{12}' disabled='disabled' /></td><td>规格型号：<input type='text' value='{13}' disabled='disabled' /></td><td></td></td></tr>
            <tr><td>商品HS编码：<input type='text' value='{14}' disabled='disabled' /></td><td>原产国/地区：<input type='text' value='{15}' disabled='disabled' /></td><td>商品序号：<input type='text' value='{16}' disabled='disabled' /></td></tr>
            <tr><td>条形码：<input type='text' value='{17}' disabled='disabled'  /></td>
                <td>计量单位: {18}</td>
                <td>毛重：<input id='txtDef15' type='text' value='{19}' disabled='disabled' /></td></tr>
            <tr><td>净重：<input id='txtDef16' type='text' value='{20}' disabled='disabled' /></td>
                <td></td>
                <td></td></tr>
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
               , "1"
               , dt.Rows[0]["Imagefile"].ToString()
               , dt.Rows[0]["ciqProductId"].ToString()
               , dt.Rows[0]["ciqSpec"].ToString()
               , dt.Rows[0]["ciqHSNo"].ToString()
               , dt.Rows[0]["ciqAssemCountry"].ToString()
               , dt.Rows[0]["ciqProductNo"].ToString()
               , dt.Rows[0]["Def10"].ToString()
               , apiUnitDDlist(dt.Rows[0]["QtyUnit"].ToString(), "1")
               , dt.Rows[0]["Def15"].ToString()
               , dt.Rows[0]["Def16"].ToString()
          );
                s += "<div id='divProductInfo'></div>";
                string src = string.Empty;
                if (dt1.Rows.Count > 0)//--显示主图
                {
                    s += "<div class='divStyleContainer'><div>主图</div>";
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        src = dt1.Rows[i]["scodePicSrc"].ToString() + "@10q." + dt1.Rows[i]["scodePicSrc"].ToString().Split('.')[dt1.Rows[i]["scodePicSrc"].ToString().Split('.').Length - 1];
                        s += "<img src=\"" + src + "\" />";
                    }
                    s += "</div>";
                }
            }
            //string json = JsonConvert.SerializeObject(dt);
            return s;

        }
        /// <summary>
        /// 根据货号更新商品信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateProduct()
        {
            string ComAttrvalues = Request.QueryString["ComAttrvalues"];
            string AttrId = Request.QueryString["AttrId"];
            string PropertyId = Request.QueryString["PropertyId"];
            Promm.Scode = Request.QueryString["Scode"];
            Promm.Cdescript = Request.QueryString["Cdescript"];
            Promm.Rolevel = Convert.ToInt32(Request.QueryString["Rolevel"]);
            Promm.Cat2 = Request.QueryString["Cat2"];
            Promm.Cat1 = Request.QueryString["Cat1"];
            Promm.ciqProductId = Request.QueryString["ciqProductId"];
            Promm.ciqHSNo = Request.QueryString["ciqHSNo"];
            Promm.ciqSpec = Request.QueryString["ciqSpec"];
            Promm.ciqAssemCountry = Request.QueryString["ciqAssemCountry"];
            Promm.Def9 = Request.QueryString["Def9"];
            Promm.Def10 = Request.QueryString["Def10"];
            Promm.Imagefile = Request.QueryString["Imagefile"].ToString();
            Promm.QtyUnit = Request.QueryString["QtyUnit"].ToString();
            Promm.Def15 = Request.QueryString["Def15"].ToString();
            Promm.Def16 = Request.QueryString["Def16"].ToString();

            return pro.UpdateProduct(Promm, AttrId, ComAttrvalues, PropertyId, userInfo.User.Id);
        }
        #endregion


        #region AttributeManagement
        /// <summary>
        /// 获取所以商品类别列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult AttributeManagement()
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

            //return View(list1.Take(count).ToList());
        }


        /// <summary>
        /// 获取商品类别并构造下拉列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDDlist(string TypeNo)
        {
            StringBuilder s = new StringBuilder(200);
            //string s = string.Empty;
            DataTable dt = new DataTable();
            dt = attrbll.GetTypeDDlist(userInfo.User.Id.ToString());
            s.Append("<select id='ddlType' style='width:150px' onchange='ddl(this[selectedIndex].value)'>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString() == TypeNo)
                {
                    s.Append("<option value='" + dt.Rows[i][1].ToString() + "' selected='selected' >" + dt.Rows[i][0].ToString() + "</option>");
                }
                else
                {
                    s.Append("<option value='" + dt.Rows[i][1].ToString() + "'>" + dt.Rows[i][0].ToString() + "</option>");
                }
            }
            s.Append("</select>");
            return s.ToString();
        }
        /// <summary>
        /// 获取拉列表商品类别显示相应属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTypeNo()
        {
            StringBuilder s = new StringBuilder(200);
            //string s = string.Empty;
            DataTable dt = new DataTable();
            string TypeNo = Request.QueryString["TypeNo"];
            string Scode = Request.QueryString["Scode"];
            dt = pro.GetPropertyByTypeNo(Scode, TypeNo);//--通过类别显示属性
            if (dt.Rows.Count > 0)
            {
                s.Append("<table>");//--hidUpdateType判断是Insert 还是Update
                if (dt.Rows.Count % 3 == 0)//--通过个数拼接td的个数
                {
                    s.Append("<tr>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s.Append("<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\" title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>");
                        if ((i + 1) % 3 == 0)
                            s.Append("</tr>");
                    }
                }
                else if (dt.Rows.Count % 3 == 1)
                {
                    s.Append("<tr>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s.Append("<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\"  title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>");
                        if ((i + 1) % 3 == 0)
                            s.Append("</tr>");
                    }
                    s.Append("<td style='width:237px'></td><td style='width:238px'></td></tr>");
                }
                else
                {
                    s.Append("<tr>");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s.Append("<td style='width:237px'>" + dt.Rows[i]["PropertyName"].ToString() + "：<input type='text' name=\"" + dt.Rows[i]["PropertyId"].ToString() + "\"  title=\"" + dt.Rows[i]["Id"].ToString() + "\" value='" + dt.Rows[i]["PropertyValue"].ToString() + "' /></td>");
                        if ((i + 1) % 3 == 0)
                            s.Append("</tr>");
                    }
                    s.Append("<td style='width:238px'></td></tr>");
                }
                s.Append("</table>");
            }
            return s.ToString();
        }

        /// <summary>
        /// 修改商品类别
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool UpdateType()
        {
            Attrmm.Id = Convert.ToInt32(Request.QueryString["Id"]);
            Attrmm.TypeName = Request.QueryString["TypeName"];
            Attrmm.TypeNo = Request.QueryString["TypeNo"];
            Attrmm.BigId = Convert.ToInt32(Request.QueryString["Bigid"]);
            Attrmm.UserId = userInfo.User.Id;
            return attrbll.UpdateType(Attrmm);
        }
        /// <summary>
        /// 删除商品类别
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteType()
        {
            string TypeNo = Request.QueryString["TypeNo"];
            return attrbll.DeleteType(TypeNo, userInfo.User.Id); ;
        }
        /// <summary>
        /// 添加商品类别
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool AddType()
        {
            string Bigid = Request.QueryString["Bigid"];
            string TypeName = Request.QueryString["TypeName"];
            string TypeNo = Request.QueryString["TypeNo"];
            Attrmm.BigId = Convert.ToInt32(Bigid);
            Attrmm.TypeName = TypeName;
            Attrmm.TypeNo = TypeNo;
            Attrmm.bigtypeIndex = 1;
            Attrmm.UserId = Convert.ToInt32(userInfo.User.Id);
            return attrbll.InsertType(Attrmm);
        }
        /// <summary>
        /// 商品修改界面的大类别下拉列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string Getddl()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            string BigTypeName = Request.QueryString["BigTypeName"];
            string strsql = @"select * from productbigtype ";
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(strsql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["bigtypeName"].ToString() == BigTypeName)
                {
                    s.Append("<option value='" + dt.Rows[i]["id"].ToString() + "' selected='selected' >" + dt.Rows[i]["bigtypeName"].ToString() + "</option>");
                }
                else
                {
                    s.Append("<option value='" + dt.Rows[i]["id"].ToString() + "' >" + dt.Rows[i]["bigtypeName"].ToString() + "</option>");
                }

            }
            return s.ToString();
        }
        /// <summary>
        /// 条件搜索
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string AttributeManagementSearch()//string lists string menuId, string page, string Selpages
        {
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]).ToString();
            int pageIndex = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);
            string paramss = Request.Form["params"] ?? string.Empty;
            string[] paramssArr = helpcommon.StrSplit.StrSplitData(paramss, ',');

            string SeachName = helpcommon.StrSplit.StrSplitData(paramssArr[0], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别名称
            string SeachBigName = helpcommon.StrSplit.StrSplitData(paramssArr[1], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//大类别
            string SeachTypeNo = helpcommon.StrSplit.StrSplitData(paramssArr[2], ':')[1].Replace("'", "").Replace("}", "") ?? string.Empty;//类别编号
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("txtSeachName", SeachName);
            Dic.Add("txtSeachBigName", SeachBigName);
            Dic.Add("txtSeachTypeNo", SeachTypeNo);
            //string[] paramsArr;
            //helpcommon.StrSplit.StrSplitData
            // string[] SearchInfo = lists.Split(',');

            //Dic.Add("txtSeachName", SearchInfo[0]);
            //Dic.Add("txtSeachBigName", SearchInfo[1]);
            //Dic.Add("txtSeachTypeNo", SearchInfo[2]);
            string counts;
            //if (page == "0")
            //{
            //    dt = attrbll.SearchAttrType(Dic, 1, Convert.ToInt32(Selpages), out counts);
            //}
            //else
            //{
            dt = attrbll.SearchAttrType(Dic, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), out counts);
            int pageCount = Convert.ToInt32(counts) % pageSize == 0 ? Convert.ToInt32(counts) / pageSize : Convert.ToInt32(counts) / pageSize + 1;
            // }
            return getDataType(dt, menuId) + "-----" + pageCount + "-----" + counts;

        }

        /// <summary>
        /// 商品类别表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataType(DataTable dt, string FmenuId)
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            StringBuilder s = new StringBuilder();
            string[] ssName = { "Id", "BigId", "TypeName", "TypeNo", "TBtypeName", "Def1", "TariffName" };
            //string[] ssName = { "Id", "BigId", "TypeNo", "TypeName", "bigtypeName" };
            //DataTable dt = DbHelperSQL.Query(strsql).Tables[0];
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);
            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "Id")
                        s.Append("类别Id");
                    if (ssName[z] == "TypeNo")
                        s.Append("类编编号");
                    if (ssName[z] == "TypeName")
                        s.Append("类别名称");
                    if (ssName[z] == "BigId")
                        s.Append("大类别名称");
                    if (ssName[z] == "TBtypeName")
                        s.Append("淘宝类别");
                    if (ssName[z] == "Def1")
                        s.Append("淘宝编号");
                    if (ssName[z] == "TariffName")
                        s.Append("海关类别");


                    s.Append("</th>");
                }
            }
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                s.Append("<th>编辑</th>");
            }
            s.Append("<th>查看属性</th>");
            s.Append("<th>淘宝类别</th>");
            s.Append("<th>海关类别</th>");
            #region 删除
            if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            {
                s.Append("<th>删除</th>");
            }
            #endregion
            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count <= 0 || dt == null)
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
                            if (ssName[j] == "BigId")
                            {
                                s.Append("<td>");
                                s.Append(dt.Rows[i]["bigtypeName"].ToString());
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
                        s.Append("<td><a href='#' onclick='EditAttribute(\"" + dt.Rows[i]["Id"].ToString() + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\",\"" + dt.Rows[i]["TypeNo"].ToString() + "\",\"" + dt.Rows[i]["BigTypeName"].ToString() + "\")'>编辑</a></td>");
                    }
                    #endregion
                    #region 查看属性
                    s.Append("<td>");
                    s.Append("<a href='#' onclick='checkAttribute(\"" + dt.Rows[i]["TypeNo"].ToString() + "\")'>查看属性</a>");
                    s.Append("</td>");
                    #endregion
                    #region 淘宝类别
                    s.Append("<td>");
                    s.Append("<a href='#' onclick='checkTBtype(\"" + dt.Rows[i]["TypeNo"].ToString() + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\",\"" + dt.Rows[i]["Def1"].ToString() + "\",\"" + dt.Rows[i]["TBtypeName"].ToString() + "\")'>修改</a>");
                    s.Append("</td>");
                    #endregion
                    #region 海关类别
                    s.Append("<td>");
                    s.Append("<a href='#' onclick='checkTariff(\"" + dt.Rows[i]["TypeNo"].ToString() + "\",\"" + dt.Rows[i]["TypeName"].ToString() + "\")'>修改</a>");
                    s.Append("</td>");
                    #endregion
                    #region 删除
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        s.Append("<td><a href='#' onclick='DeleteType(\"" + dt.Rows[i]["TypeNo"].ToString() + "\")'>删除</a></td>");
                    }
                    #endregion


                    s.Append("</tr>");
                }
            }
            #endregion


            return s.ToString();
        }

        /// <summary>
        /// 跳到更新商品属性界面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateAttribute()
        {
            string TypeNo = Request.QueryString["TypeNo"];
            ViewData["TypeNo"] = TypeNo;
            return View();
        }
        /// <summary>
        /// 跳到更新商品属性界面
        /// </summary>
        /// <returns></returns>
        public string GetAttribute()
        {
            string TypeNo = Request.QueryString["TypeNo"];
            StringBuilder s = new StringBuilder();
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetAttributeList(TypeNo);
            string[] ssName = { "PropertyName", "Def1", "TBPropertyName", "PorpertyIndex" };
            s.Append("<tr><th>编号</th>");
            s.Append("<th>属性名称</th>");
            s.Append("<th>淘宝编号</th>");
            s.Append("<th>淘宝属性列表</th>");
            s.Append("<th>顺序</th>");
            s.Append("<th>操作</th></tr>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                int num = i + 1;
                s.Append("<tr><td>" + num + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (ssName[j] == "TBPropertyName")
                    {
                        s.Append("<td>" + GetTBProducttypeDDlist(dt1.Rows[i]["cid"].ToString(), dt1.Rows[i]["Def1"].ToString()) + "</td>");
                    }
                    else if (ssName[j] == "Def1")
                    {
                        s.Append("<td class='tdvid'>" + dt1.Rows[i][ssName[j]] + "</td>");
                    }
                    else
                    {
                        s.Append("<td>" + dt1.Rows[i][ssName[j]] + "</td>");
                    }

                }
                s.Append("<td><a href='#' onclick='Edit(\"" + dt1.Rows[i]["Id"] + "\",\"" + dt1.Rows[i]["PropertyName"] + "\")'>编辑</a><a href='#' onclick='Delete(\"" + dt1.Rows[i]["Id"] + "\")'>删除</a><a class='SaveTBcid' href='#' name='" + dt1.Rows[i]["Id"] + "' title='' >绑定</a></td>");
                s.Append("</tr>");
            }

            return s.ToString();
        }
        /// <summary>
        /// 获取淘宝属性表
        /// </summary>
        /// <returns></returns>
        public string GetTBProductPro()
        {
            StringBuilder s = new StringBuilder();
            DataTable dt1 = new DataTable();
            string[] ssName = { "TBPropertyName", "vid", "PropertyName" };
            string TypeNo = Request.QueryString["TypeNo"];
            string sql = @"select a.*,b.PropertyName from TBProductProperty a 
 left join productPorperty b on a.vid=b.Def1
 left join producttype as c on b.TypeId=c.Id 
 where a.parent_cid=(select Def1 from producttype where TypeNo='" + TypeNo + "')";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            s.Append("<tr><th>编号</th>");
            s.Append("<th>淘宝属性</th>");
            s.Append("<th>淘宝编号</th>");
            s.Append("<th>本地属性</th></tr>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                int num = i + 1;
                s.Append("<tr><td>" + num + "</td>");
                for (int j = 0; j < ssName.Length; j++)
                {
                    if (dt1.Rows[i][ssName[j]].ToString() == "")
                    {
                        s.Append("<td><span style='color:red;font-weight:bold'>未绑定</span></td>");
                    }
                    else
                    {
                        s.Append("<td>" + dt1.Rows[i][ssName[j]].ToString() + "</td>");
                    }

                }
                s.Append("</tr>");
            }
            return s.ToString();
        }
        /// <summary>
        /// 淘宝属性下拉列表
        /// </summary>
        /// <returns></returns>
        public string GetTBProducttypeDDlist(string parentid, string Def1)
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query("select TBPropertyName,vid from TBProductProperty where parent_cid='" + parentid + "'").Tables[0];
            s.Append("<select class='selTBProducttype'><option value=''>请选择</option>");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (Def1 == dt1.Rows[i]["vid"].ToString())
                {
                    s.Append("<option value='" + dt1.Rows[i]["vid"] + "' selected='selected'>" + dt1.Rows[i]["TBPropertyName"] + "</option>");
                }
                else
                {
                    s.Append("<option value='" + dt1.Rows[i]["vid"] + "'>" + dt1.Rows[i]["TBPropertyName"] + "</option>");
                }

            }
            s.Append("</select>");
            return s.ToString();
        }
        /// <summary>
        /// 保存淘宝属性到商品属性表
        /// </summary>
        /// <returns></returns>
        public string SaveTBVid()
        {
            string s = "";
            string Id = Request.QueryString["Id"];
            string vid = Request.QueryString["vid"];
            string TypeNo = Request.QueryString["TypeNo"];
            string checksql = @"select COUNT(*) from productPorperty a left join producttype b on a.TypeId=b.Id where TypeNo='" + TypeNo + "' and a.Def1='" + vid + "'";
            if (DbHelperSQL.GetSingle(checksql).ToString() != "0")
            {
                return "该属性已被绑定";
            }
            string Updatesql = @"update productPorperty set Def1='" + vid + "' where Id='" + Id + "'";
            int i = DbHelperSQL.ExecuteSql(Updatesql);
            if (i == -1)
            {
                s = "绑定失败!";
            }

            return s;
        }
        /// <summary>
        /// 编辑商品属性
        /// </summary>
        /// <returns></returns>
        public int EditAttribute()
        {
            string Id = Request.QueryString["Id"];
            string PropertyName = Request.QueryString["PropertyName"];
            int n = attrbll.UpdateAttribute(Id, PropertyName, userInfo.User.Id);
            return n;

        }
        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <returns></returns>
        public int AddAttribute()
        {
            string TypeNo = Request.QueryString["TypeNo"];
            string PropertyName = Request.QueryString["PropertyName"];
            string UserId = userInfo.User.Id.ToString();
            int n = attrbll.InsertAttribute(TypeNo, PropertyName, UserId);
            return n;

        }
        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <returns></returns>
        public int DeleteAttribute()
        {
            string Id = Request.QueryString["id"];
            int n = attrbll.DeleteAttribute(Id, userInfo.User.Id);
            return n;

        }
        #endregion


        #region BigTypeManagement
        //-----------BigTypeManagement
        /// <summary>
        /// 获取所以大类别类别列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult BigTypeManagement()
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
        /// 修改大类别名称
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string UpdateBigType()
        {
            Attrmm.Id = Convert.ToInt32(Request.QueryString["Id"]);
            Attrmm.BigTypeName = Request.QueryString["BigtypeName"];
            Attrmm.bigtypeIndex = Convert.ToInt32(Request.QueryString["bigtypeIndex"]);
            Attrmm.UserId = Convert.ToInt32(userInfo.User.Id);
            return attrbll.UpdateBigType(Attrmm);
        }
        /// <summary>DeleteBigType
        /// 添加大类别
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool AddBigType()
        {
            Attrmm.BigTypeName = Request.QueryString["BigtypeName"];
            Attrmm.bigtypeIndex = 1;
            Attrmm.UserId = Convert.ToInt32(userInfo.User.Id);
            return attrbll.InsertBigType(Attrmm);
        }
        /// <summary>DeleteBigType
        ///  删除大类别
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteBigType()
        {
            string Id = Request.QueryString["Id"];
            return attrbll.DeleteBigType(Id, userInfo.User.Id);
        }

        /// <summary>
        /// 大类别表查询
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="menuId"></param>
        /// <param name="page"></param>
        /// <param name="Selpages"></param>
        /// <returns></returns>
        public string SearchBigType()//string Name, string menuId, string page, string Selpages
        {
            string menuId = helpcommon.ParmPerportys.GetStrParms(Request.Form["menuId"]);
            string paramsstr = Request.Form["params"] ?? string.Empty;
            int pageIndex = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageIndex"]);
            int pageSize = helpcommon.ParmPerportys.GetNumParms(Request.Form["pageSize"]);

            string[] paramsArr = helpcommon.StrSplit.StrSplitData(paramsstr, ',');
            string Name = helpcommon.StrSplit.StrSplitData(paramsArr[0], ':')[1].Replace("'", "").Replace("}", "");
            string counts;
            dt = attrbll.SearchGetBigTypeList(Name, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), out counts);
            int pageCount = Convert.ToInt32(counts) % pageSize == 0 ? Convert.ToInt32(counts) / pageSize : Convert.ToInt32(counts) / pageSize + 1;
            return getDataBigType(dt, menuId) + "-----" + pageCount + "-----" + counts;
        }

        /// <summary>
        /// 大类别表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FmenuId"></param>
        /// <returns></returns>
        public string getDataBigType(DataTable dt, string FmenuId)
        {

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = FmenuId == null ? helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"]) : helpcommon.ParmPerportys.GetNumParms(FmenuId);
            int pagecount = Convert.ToInt32(Request.Form["selPages"]);
            int page = Convert.ToInt32(Request.Form["Page"]);
            List<model.users> list = new List<model.users>();
            StringBuilder s = new StringBuilder();
            //string sql = @"select * from productbigtype";//---获取权限字段
            string[] ssName = { "Id", "bigtypeName" };

            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectName);


            #region TABLE表头
            s.Append("<tr><th>编号</th>");
            for (int z = 0; z < ssName.Length; z++)
            {
                if (ss.Contains(ssName[z]))
                {
                    s.Append("<th>");
                    if (ssName[z] == "Id")
                        s.Append("大类别ID");
                    if (ssName[z] == "bigtypeName")
                        s.Append("名称");

                    s.Append("</th>");
                }
            }

            #region 编辑
            if (ph.isFunPermisson(roleId, menuId, funName.updateName))
            {
                s.Append("<th>编辑</th>");
            }
            #endregion

            #region 删除
            if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
            {
                s.Append("<th>删除</th>");
            }
            #endregion


            s.Append("</tr>");
            #endregion

            #region TABLE内容
            if (dt.Rows.Count <= 0 || dt == null)
            {
                s.Append("<tr><td colspan='50' style='font-size:12px; color:red; text-align:center;'>本次搜索暂无数据！</td></tr>");
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = i + 1;
                    s.Append("<tr><td>" + n + "</td>");
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
                    if (ph.isFunPermisson(roleId, menuId, funName.updateName))
                    {
                        s.Append("<td><a href='#' onclick='EditBigType(\"" + dt.Rows[i]["Id"].ToString() + "\",\"" + dt.Rows[i]["BigTypeName"].ToString() + "\",\"" + dt.Rows[i]["bigtypeIndex"].ToString() + "\")'>编辑</a></td>");
                    }
                    #endregion

                    #region 删除
                    if (ph.isFunPermisson(roleId, menuId, funName.deleteName))
                    {
                        s.Append("<td><a href='#' onclick='DeleteBigType(" + dt.Rows[i]["Id"].ToString() + ")'>删除</a></td>");
                    }
                    #endregion


                    s.Append("</tr>");
                }
            }

            #endregion


            return s.ToString();
        }


        #endregion


        /// <summary>
        /// 上传文件保存到本地UploadXls文件夹中
        /// </summary>
        /// <param name="FileUpload"></param>
        public void UpLoadXls(HttpPostedFileBase FileUpload)
        {
            string result = string.Empty;
            string fliename = FileUpload.FileName;
            string uploadPath = Server.MapPath("~/UploadXls/");
            string url = "/UploadXls/" + fliename;
            FileUpload.SaveAs(uploadPath + fliename);
        }
        /// <summary>
        /// 获得款号描述
        /// </summary>
        /// <returns></returns>
        public string Getstyledescript()
        {
            string s = "";
            string style = Request.QueryString["style"];
            string strsql = @"select * from styledescript where style='" + style + "' ";
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query(strsql).Tables[0];
            if (dt1.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                s = dt1.Rows[0]["productDescript"].ToString();
                return s;
            }

        }
        /// <summary>
        /// 获得查看款号属性
        /// </summary>
        /// <returns></returns>
        public string Checkstyledescript()
        {
            //string s = "";
            StringBuilder s = new StringBuilder(200);
            string style = Request.QueryString["style"];
            string src = "";
            string strsql = @"select a.*,b.productDescript from stylepic a left join styledescript b on a.style=b.style where a.style='" + style + "' and a.Def4='1'";
            DataTable dt1 = new DataTable();
            dt1 = DbHelperSQL.Query(strsql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                if (dt1.Rows[0]["productDescript"].ToString() != "")
                {
                    s.Append(dt1.Rows[0]["productDescript"].ToString());
                }
                s.Append("-*-");
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i]["Def1"].ToString().Substring(0, 1) == "2")
                    {
                        src = dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[dt1.Rows[i]["stylePicSrc"].ToString().Split('.').Length - 1];
                        s.Append("<img src=\"" + src + "\" />");
                    }
                }
                s.Append("-*-");
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (dt1.Rows[j]["Def2"].ToString().Substring(0, 1) == "2")
                    {
                        src = dt1.Rows[j]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[j]["stylePicSrc"].ToString().Split('.')[dt1.Rows[j]["stylePicSrc"].ToString().Split('.').Length - 1];
                        s.Append("<img src=\"" + src + "\" />");
                    }
                }
                s.Append("-*-");
                for (int n = 0; n < dt1.Rows.Count; n++)
                {
                    if (dt1.Rows[n]["Def3"].ToString().Substring(0, 1) == "2")
                    {
                        src = dt1.Rows[n]["stylePicSrc"].ToString() + "@10q." + dt1.Rows[n]["stylePicSrc"].ToString().Split('.')[dt1.Rows[n]["stylePicSrc"].ToString().Split('.').Length - 1];
                        s.Append("<img src=\"" + src + "\" />");
                    }
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 存储编辑信息 Product主款号表
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public int Updatestyledescript()
        {
            DataTable dt1 = new DataTable();
            string style = Request.QueryString["style"];
            string productDescript = Request.QueryString["productDescript"];
            return pro.Updatestyledescript(style, productDescript, userInfo.User.Id);
        }
        /// <summary>
        /// 根据货号显示商品价格
        /// </summary>
        public string ProductInfoByScode()
        {
            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);
            int menuId = Request.QueryString["menuId"] != null ? helpcommon.ParmPerportys.GetNumParms(Request.QueryString["menuId"]) : 0;
            PublicHelpController ph = new PublicHelpController();
            string[] ss = ph.getFiledPermisson(roleId, menuId, funName.selectProductPrice);
            DataTable dt1 = new DataTable();
            StringBuilder s = new StringBuilder();
            string scode = Request.QueryString["scode"];
            string[] ssName = { "Vencode", "Pricea", "Priceb", "Pricec", "Priced", "Pricee", "Balance" };
            string sql = @"select a.*,b.sourceName from productstock a 
left join productsource b on a.Vencode=b.SourceCode 
left join PersonaTypeConfit c on a.Vencode=c.Def1 and a.Cat2=c.TypeId 
left join BrandConfigPersion d on a.Vencode=d.Def1 and a.Cat=d.BrandId 
where Scode='" + scode + "' and c.CustomerId=" + userInfo.User.Id + " and d.CustomerId=" + userInfo.User.Id + "";
            dt1 = DbHelperSQL.Query(sql).Tables[0];//--查询价格以及库存

            if (dt1.Rows.Count > 0)
            {
                s.Append("<table class='tabPriceInfo'>");
                s.Append("<tr>");
                for (int z = 0; z < ssName.Length; z++)
                {
                    if (ss.Contains(ssName[z]))
                    {
                        s.Append("<th>");
                        if (ssName[z] == "Vencode")
                            s.Append("供应商");
                        if (ssName[z] == "Pricea")
                            s.Append("吊牌价");
                        if (ssName[z] == "Priceb")
                            s.Append("零售价");
                        if (ssName[z] == "Pricec")
                            s.Append("VIP价");
                        if (ssName[z] == "Priced")
                            s.Append("批发价");
                        if (ssName[z] == "Pricee")
                            s.Append("成本价");
                        if (ssName[z] == "Balance")
                            s.Append("库存");
                        s.Append("</th>");
                    }
                }
                s.Append("</tr>");
            }
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < ssName.Length; j++)
                {

                    if (ss.Contains(ssName[j]))
                    {
                        s.Append("<td>");
                        if (ssName[j] == "Vencode")
                        {
                            s.Append(dt1.Rows[i]["sourceName"].ToString());

                        }
                        else
                        {
                            s.Append(dt1.Rows[i][ssName[j]].ToString());
                        }
                        s.Append("</td>");
                    }


                }
                s.Append("</tr>");
            }
            s.Append("</table>");
            return s.ToString();
        }
        /// <summary>
        /// 修改类别对应税号
        /// </summary>
        public bool UploadTariff()
        {
            DataTable dt1 = new DataTable();
            bool flag = false;
            try
            {
                string TypeNo = Request.QueryString["TypeNo"];
                string TariffNo = Request.QueryString["TariffNo"];
                int UserId = Convert.ToInt32(userInfo.User.Id);
                flag = attrbll.UpdateTypeIdToTariffNo(TypeNo, TariffNo, UserId);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 模糊查询税名
        /// </summary>
        public string SelectTariff()
        {
            DataTable dt1 = new DataTable();
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            string TariffName = Request.QueryString["TariffName"];
            string sql = @"select * from Tarifftab where TariffName like '%" + TariffName + "%'";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    s.Append("<option value='" + dt1.Rows[i]["TariffNo"] + "'>" + dt1.Rows[i]["TariffName"] + "</option>");
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 获得税名列表
        /// </summary>
        public string GetTariffDDL()
        {
            DataTable dt1 = new DataTable();
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            string TypeNo = Request.QueryString["TypeNo"];
            string sql = @"select * from Tarifftab ";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    s.Append("<option value='" + dt1.Rows[i]["TariffNo"] + "'>" + dt1.Rows[i]["TariffName"] + "</option>");
                }
            }
            string TypeNosql = @"select b.* from TypeIdToTariffNo as a left join Tarifftab as b on a.TariffNo=b.TariffNo where a.TypeNo='" + TypeNo + "'";
            dt1 = DbHelperSQL.Query(TypeNosql).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                s.Append("-*-" + dt1.Rows[0]["TariffNo"] + "-*-" + dt1.Rows[0]["TariffName"]);
            }
            return s.ToString();
        }
        /// <summary>
        /// 模糊查询淘宝类别
        /// </summary>
        public string SelectTBtype()
        {
            DataTable dt1 = new DataTable();
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            string TBtypeName = Request.QueryString["TBtype"];
            string sql = @"select * from TBProducttype where TBtypeName like '%" + TBtypeName + "%'";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            s.Append("<option value=''>请选择</option>");
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    s.Append("<option value='" + dt1.Rows[i]["cid"] + "'>" + dt1.Rows[i]["TBtypeName"] + "</option>");
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 修改类别对应淘宝编号
        /// </summary>
        public bool UploadTBType()
        {
            DataTable dt1 = new DataTable();
            bool flag = false;
            try
            {
                string TypeNo = Request.QueryString["TypeNo"];
                string cid = Request.QueryString["cid"];
                int UserId = Convert.ToInt32(userInfo.User.Id);
                flag = attrbll.UploadTBType(TypeNo, cid, UserId);
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        ///  <summary>
        /// HS信息查询界面
        /// </summary>
        public ActionResult HSNumberShow()
        {
            return View();
        }

        ///  <summary>
        /// --HS信息查询
        /// </summary>
        public string SearchHSinfo()
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            string HSNumber = Request.Form["HSNumber"].ToString();
            string TypeName = Request.Form["TypeName"].ToString();
            DataTable dt1 = new DataTable();
            dt1 = pro.SearchHSinfo(HSNumber, TypeName);
            s.Append("<tr><th>商品编码</th><th>商品名称</th><th>最惠国税率</th><th>操作</th></tr>");
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    s.Append("<tr><td>" + dt1.Rows[i]["HSNumber"] + "</td><td>" + dt1.Rows[i]["TypeName"] + "</td><td>" + dt1.Rows[i]["tariff"] + "</td><td><a href='#' onclick='SaveHSinfo(\"" + dt1.Rows[i]["HSNumber"].ToString() + "\")'>绑定</a></td></tr>");
                }
            }
            return s.ToString();
        }

        /// <summary>
        /// 获取国家下拉列表
        /// </summary>
        /// <param name="countryName"></param>
        /// <param name="Country"></param>
        /// <returns></returns>
        public string GetCountrylist(string countryName, string Country)
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            s.Append("<option value=''>请选择</option>");
            if (countryName != "")
            {
                //var q = from c in context.customsCountry where c.countryName.Contains(countryName) || c.countryNo.Contains(countryName) select c;
                var q = from c in context.customsCountry where c.countryName.Contains(countryName) || c.countryNo.Contains(countryName) || context.fun_getPY(c.countryName).Contains(countryName) select c;
                dt = LinqToDataTable.LINQToDataTable(q.OrderBy(a => a.countryNo));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s.Append("<option value='" + dt.Rows[i]["countryNo"] + "'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>");
                }
            }
            else
            {
                dt = DbHelperSQL.Query(@"select * from customsCountry order by countryNo").Tables[0];
                if (Country == "")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s.Append("<option value='" + dt.Rows[i]["countryNo"] + "'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>");
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Country.Contains(dt.Rows[i]["countryNo"].ToString()))
                        {
                            s.Append("<option value='" + dt.Rows[i]["countryNo"] + "' selected='selected'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>");
                        }
                        else
                        {
                            s.Append("<option value='" + dt.Rows[i]["countryNo"] + "'>" + dt.Rows[i]["countryName"] + "-" + dt.Rows[i]["countryNo"] + "</option>");
                        }

                    }
                }
            }
            return s.ToString();
        }
        /// <summary>
        /// 商品信息图
        /// </summary>
        /// <returns></returns>
        public string ProductInfoPic(string item)
        {
            //string s = string.Empty;
            StringBuilder s = new StringBuilder(200);
            if (item == "")
            {
                s.Append("<option value='' selected='selected'>请选择</option>");
            }
            else
            {
                s.Append("<option value=''>请选择</option>");
            }
            if (item == "1")
            {
                s.Append("<option value='1' selected='selected'>合格</option>");
            }
            else
            {
                s.Append("<option value='1'>合格</option>");
            }
            if (item == "3")
            {
                s.Append("<option value='3' selected='selected'>不合格</option>");
            }
            else
            {
                s.Append("<option value='3'>不合格</option>");
            }
            return s.ToString();
        }



        #region  类别 品牌下拉列表

        public string Brand()
        {
            string sql = Request.Form["sql"].ToString();
            string data = Request.Form["value"].ToString();
            string tsql;
            if (sql == "1")
            {
                tsql = "select a.BrandName,BrandAbridge from brand a left join BrandConfigPersion b on a.BrandAbridge=b.BrandId where b.CustomerId=" + userInfo.User.Id + " order by a.BrandName";
            }
            else
            {
                tsql = "select a.BrandName,BrandAbridge from brand a left join BrandConfigPersion b on a.BrandAbridge=b.BrandId where b.CustomerId=" + userInfo.User.Id + " and  a.BrandName like '%" + data + "%' order by a.BrandName";
            }
       
            return phb.GetDropList(tsql);
        }

        public string Type()
        {
            string sql = Request.Form["sql"].ToString();
            string data = Request.Form["value"].ToString();
            string tsql;
            if (sql == "1")
            {
                tsql = "select TypeName,TypeNo from producttype a left join PersonaTypeConfit b on a.TypeNo=b.TypeId where b.CustomerId=" + userInfo.User.Id + " order by TypeName";
            }
            else
            {
                tsql = "select TypeName,TypeNo from producttype a left join PersonaTypeConfit b on a.TypeNo=b.TypeId where b.CustomerId=" + userInfo.User.Id + "  and a.TypeName like '%" + data + "%' order by TypeName";
            }
            return phb.GetDropList1(tsql);
        }

        #endregion


        #region   导出EXCEL文档

        public string OutPutExcel()
        {
            int MenuId = int.Parse(Request.Form["MenuId"].ToString());
            string List = Request.Form["list"].ToString();
            string[] SearchInfo = List.ToString().Split(',');

            Dictionary<string, string> Dic = new Dictionary<string, string>();
            Dic.Add("Style", SearchInfo[0]);
            Dic.Add("Scode", SearchInfo[1]);
            Dic.Add("Name", SearchInfo[2]);
            Dic.Add("Cat", SearchInfo[3]);
            Dic.Add("Cat1", SearchInfo[4]);
            Dic.Add("Cat2", SearchInfo[5]);
            Dic.Add("MinBalance", SearchInfo[6]);
            Dic.Add("MaxBalance", SearchInfo[7]);
            Dic.Add("MinPricea", SearchInfo[8]);
            Dic.Add("MaxPricea", SearchInfo[9]);
            Dic.Add("MinLastgrnd", SearchInfo[10]);
            Dic.Add("MaxLastgrnd", SearchInfo[11]);
            Dic.Add("AttrState", SearchInfo[12]);
            Dic.Add("PicState", SearchInfo[13]);
            Dic.Add("MinPictime", SearchInfo[14]);
            Dic.Add("MaxPictime", SearchInfo[15]);
            Dic.Add("CustomerId", userInfo.User.Id.ToString());
            Dic.Add("UserId", SearchInfo[16]);
            Dic.Add("Def9", SearchInfo[17]);
            Dic.Add("Color", SearchInfo[18]);
            Dic.Add("Def11", SearchInfo[19]);
            Dic.Add("ciqProductId", SearchInfo[20]);

            string counts;
            string Balcounts;
            DataTable dt = new DataTable();
            int isReset = 1;
            pro.SearchProduct(Dic, 1, 10, out counts, out Balcounts, isReset);
            dt = pro.SearchProduct(Dic, 1, int.Parse(counts.ToString()), out counts, out Balcounts, isReset);

            int roleId = helpcommon.ParmPerportys.GetNumParms(userInfo.User.personaId);  //角色ID
            string[] s = ph.getFiledPermisson(roleId, MenuId, funName.selectName);//获得当前权限字段

            List<pbxdata.web.Controllers.ProductHelper.KeyValues> list = new List<pbxdata.web.Controllers.ProductHelper.KeyValues>();

            for (int i = 0; i < s.Length; i++)
            {
                #region   Excel列表字段
                switch (s[i])
                {
                    case "Scode":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Scode", Value = "货号" });
                        break;
                    case "Bcode":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Bcode", Value = "条形码" });
                        break;
                    case "Bcode2":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Bcode2", Value = "条形码2" });
                        break;
                    case "Descript":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Descript", Value = "英文描述" });
                        break;
                    case "Cdescript":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Cdescript", Value = "中文描述" });
                        break;
                    case "Unit":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Unit", Value = "单位" });
                        break;
                    case "Currency":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Currency", Value = "货币" });
                        break;
                    case "Cat":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Cat", Value = "品牌" });
                        break;
                    case "Cat1":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Cat1", Value = "季节" });
                        break;
                    case "Cat2":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "TypeName", Value = "类别" });
                        break;
                    case "Clolor":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Clolor", Value = "颜色" });
                        break;
                    case "Size":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Size", Value = "尺码" });
                        break;
                    case "Style":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Style", Value = "款号" });
                        break;
                    case "Pricea":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Pricea", Value = "吊牌价" });
                        break;
                    case "Priceb":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Priceb", Value = "零售价" });
                        break;
                    case "Pricec":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Pricec", Value = "VIP价" });
                        break;
                    case "Priced":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Priced", Value = "批发价" });
                        break;
                    case "Pricee":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Pricee", Value = "成本价" });
                        break;
                    case "Disca":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Disca", Value = "折扣1" });
                        break;
                    case "Discb":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Discb", Value = "折扣2" });
                        break;
                    case "Discc":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Discc", Value = "折扣3" });
                        break;
                    case "Discd":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Discd", Value = "折扣4" });
                        break;
                    case "Disce":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Disce", Value = "折扣5" });
                        break;
                    case "Vencode":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Vencode", Value = "数据源" });
                        break;
                    case "Model":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Model", Value = "型号" });
                        break;
                    case "Rolevel":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Rolevel", Value = "预警库存" });
                        break;
                    case "Roamt":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Roamt", Value = "最少订货量" });
                        break;
                    case "Stopsales":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Stopsales", Value = "停售库存" });
                        break;
                    case "Loc":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Loc", Value = "店铺" });
                        break;
                    case "Balance":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Balance", Value = "库存" });
                        break;
                    case "Lastgrnd":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Lastgrnd", Value = "交货日期" });
                        break;
                    case "Imagefile":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Imagefile", Value = "图片路径" });
                        break;
                    case "Def1":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def1", Value = "图片上传时间" });
                        break;
                    case "Def2":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def2", Value = "默认2" });
                        break;
                    case "Def3":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def3", Value = "销售库存" });
                        break;
                    case "ciqProductId":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "ciqProductId", Value = "是否合格" });
                        break;
                    case "ciqSpec":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "ciqSpec", Value = "规格型号" });
                        break;
                    case "ciqAssemCountry":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "ciqAssemCountry", Value = "原产国/地区" });
                        break;
                    case "ciqHSNo":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "ciqHSNo", Value = "商品HS编码" });
                        break;
                    case "ciqProductNo":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "ciqProductNo", Value = "商品序号" });
                        break;
                    case "Def9":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def9", Value = "图片合格" });
                        break;
                    case "Def10":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def10", Value = "条形码" });
                        break;
                    case "Def11":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def11", Value = "商品海关备案编号" });
                        break;
                    case "Def12":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def12", Value = "商检商品信息报备" });
                        break;
                    case "Def13":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def13", Value = "联邦商品信息报备" });
                        break;
                    case "QtyUnit":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "QtyUnit", Value = "计量单位" });
                        break;
                    case "Def15":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def15", Value = "毛重" });
                        break;
                    case "Def16":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def16", Value = "净重" });
                        break;
                    case "Def17":
                        list.Add(new pbxdata.web.Controllers.ProductHelper.KeyValues { Key = "Def17", Value = "产品链接" });
                        break;
                }
                #endregion
            }

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + dt.Rows.Count;
            fileName += ".xls";
            ProductHelper phelper = new ProductHelper();
            try
            {
                //string savaPath = "~/Uploadxls/T1_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //Server.MapPath(savaPath);
                phelper.dataTableToCsv(dt, Server.MapPath("~/Uploadxls/" + fileName + ""), list);
                string UploadPath = "http://" + Request.Url.Authority + "/" + ("~/Uploadxls/" + fileName + "").Replace("~/", "");
                return UploadPath;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
                throw;
            }
        }
        #endregion
    }
}
