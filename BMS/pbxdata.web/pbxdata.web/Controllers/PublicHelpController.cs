using Maticsoft.DBUtility;
using pbxdata.bll;
using pbxdata.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class PublicHelpController : BaseController
    {
        //
        // GET: /PublicHelp/1
        

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public string getMenu()
        {
           
            string s = "{\"menu\":[";
            bll.menubll menuBll = new bll.menubll();
            IDataParameter[] ipara = new IDataParameter[]{
                //new SqlParameter("menuName",SqlDbType.NVarChar,20)
            };

            List<model.menu> list = menuBll.getMenu(ipara, "menuChild");
            foreach (var item in list)
            {
                s += "{\"menuid\":\"" + item.Id + "\",\"menuname\":\"" + item.menuName + "\"},";
            }

            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            s += "]}";

            menuBll = null;
            return s;
        }


        #region 菜单-权限管理-顶部导航条
        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <returns></returns>
        public string getRoleName()
        {
            bll.rolebll roleBll = new bll.rolebll();
            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["roleId"].ToString());

            model.persona mdPersona = roleBll.getRoleName(id);

            return mdPersona.PersonaName;
        }


        /// <summary>
        /// 获取菜单名称
        /// </summary>
        /// <returns></returns>
        public string getMenuName()
        {
            bll.menubll menuBll = new bll.menubll();
            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["menuId"].ToString());

            model.menu mdMenu = menuBll.getMenuName(id);

            return mdMenu.menuName;
        }


        /// <summary>
        /// 获取功能名称
        /// </summary>
        /// <returns></returns>
        public string getFunName1()
        {
            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            int id = helpcommon.ParmPerportys.GetNumParms(Request.Form["funId"].ToString());

            model.funpermisson mdFunpermisson = funPermissonBll.getFunName(id);

            return mdFunpermisson.FunName;
        }

        /// <summary>
        /// 获取功能名称
        /// </summary>
        /// <param name="funId">功能ID</param>
        /// <returns></returns>
        public string getFunName(int funId)
        {
            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            model.funpermisson mdFunpermisson = funPermissonBll.getFunName(funId);
            return mdFunpermisson.FunName;
        }

        #endregion


        #region 菜单权限管理

        /// <summary>
        /// 主菜单权限控制
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> getUserMenuF1(int roleId)
        {
            IDataParameter[] iparaMenu = new IDataParameter[] { 
                                    new SqlParameter("roleId",SqlDbType.Int,4),
                                };
            iparaMenu[0].Value = roleId;
            List<int> listMenuId = new List<int>(); //目录ID
            model.menu menuMd = new model.menu();
            bll.menubll menuBll = new bll.menubll();


            List<model.menu> listMenu = menuBll.getMenu(iparaMenu, "menuSelectFF");//主菜单
            foreach (var item in listMenu)
            {
                listMenuId.Add(item.Id);
            }
            if (listMenu.Count == 0)
            {
                listMenuId.Add(9999);
            }

            menuBll = null;
            menuMd = null;

            return listMenuId;
        }

        /// <summary>
        /// 子菜单权限控制
        /// </summary>
        /// <param name="listMenuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> getUserMenuC1(List<int> listMenuId, int roleId)
        {
            List<int> listMenuIdC = new List<int>(); //菜单ID
            string s = string.Empty;
            foreach (int item in listMenuId)
            {
                s += "'" + item + "',";
            }
            s = s.Length > 0 ? s.Remove(s.Length - 1, 1) : s;
            IDataParameter[] iparaMenuC = new IDataParameter[] { 
                                    new SqlParameter("roleId",SqlDbType.Int,4),
                                    new SqlParameter("menuId",SqlDbType.NVarChar,500)
                                };
            iparaMenuC[0].Value = roleId;
            iparaMenuC[1].Value = s;
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

            menuBllC = null;
            menuMdC = null;

            return listMenuIdC;
        }
        #endregion


        #region 功能权限管理

        /// <summary>
        /// 是否存在功能管理权限
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <param name="permissonName">权限-功能名称</param>
        /// <returns></returns>
        public bool isFunPermisson(int roleId,int menuId, string permissonName)
        {
            bool flag = false;
            string s = string.Empty;
            List<int> list1 = new List<int>();
            list1.Add(menuId);
            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            List<int> list = getUserFun1(list1,roleId);
            foreach (int item in list)
            {
                string funName = getFunName(item);
                if (!string.IsNullOrWhiteSpace(funName))
                {
                    bool flg1 = funName.Contains(permissonName.Trim());
                    if (funName.Contains(permissonName))
                    {
                        flag = true;
                    }
                }
                
            }

            personapermissonBll = null;
            
            return flag;
        }


        /// <summary>
        /// 功能权限控制
        /// </summary>
        /// <param name="listMenuIdC"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> getUserFun1(List<int> listMenuIdC,int roleId)
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

            iparaFun[0].Value = roleId;
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


            funBll = null;
            funMd = null;

            return listFunId;
        }


        /// <summary>
        /// 功能权限控制
        /// </summary>
        /// <param name="listMenuIdC"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> getUserFiled(int roleId,int menuId,int funId)
        {
            string s = string.Empty;
            IDataParameter[] iparaFiled = new IDataParameter[] { 
                                    new SqlParameter("roleId",SqlDbType.Int,4),
                                    new SqlParameter("menuId",SqlDbType.Int,4),
                                    new SqlParameter("funId",SqlDbType.Int,4)
                                };
            iparaFiled[0].Value = roleId;
            iparaFiled[1].Value = menuId;
            iparaFiled[2].Value = funId;

            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            List<model.tableFiledPerssion> list = tableFiledPerssionBll.getTable(iparaFiled, "tableFiledSelect");
            List<int> listFiledId = new List<int>(); //功能ID

            foreach (var item in list)
            {
                listFiledId.Add(item.Id);
            }
            if (listFiledId.Count == 0)
            {
                listFiledId.Add(9999);
            }

            return listFiledId;
        }
        #endregion


        #region 字段权限管理

        #endregion



        #region 日志处理

        //public void AddLog(errorlog log)
        //{
        //    string s = string.Empty;
        //    bll.Errorlogbll errorBll = new bll.Errorlogbll(out s);
        //    log.ErrorMsg = s;
        //    errorBll.InsertErrorlog(log);
        //    errorBll = null;
        //}
        #endregion


        #region 返回有权限的字段(sql语句或字段集合)
        /// <summary>
        /// 获取有权限的字段
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <param name="funid">功能ID</param>
        /// <returns>返回sql语句（包含有权限的字段）</returns>
        public string getFiledPermissonSQL(int roleid, int menuid, int funid,string tableName)
        {
            string sql = string.Empty;
            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            int[] temp = personapermissonBll.selectFiledPersonaPermisson(roleid, menuid, funid);

            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            sql = tableFiledPerssionBll.getFiledPermissionSQL(temp, tableName);

            personapermissonBll = null;
            tableFiledPerssionBll = null;

            return sql;
        }


        /// <summary>
        /// 获取有权限的字段
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="funName">功能操作</param>
        /// <returns>返回sql语句（包含有权限的字段）</returns>
        public string getFiledPermissonSQL(int roleId, int menuId,string funName,string tableName)
        {
            string sql = string.Empty;
            PublicHelpController pub = new PublicHelpController();
            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            int[] funIds = personapermissonBll.selectFunPersonaPermisson(roleId, menuId);
            int funId = 0;
            if (funIds.Length > 0)
            {
                for (int i = 0; i < funIds.Length; i++)
                {
                    int myFunId = funPermissonBll.getFunName(helpcommon.ParmPerportys.GetNumParms(funIds[i])).Id;
                    string myFunName = funPermissonBll.getFunName(helpcommon.ParmPerportys.GetNumParms(funIds[i])).FunName;
                    if (!string.IsNullOrWhiteSpace(myFunName))
                    {
                        if (myFunName.Contains(funName))
                        {
                            funId = myFunId;
                        }
                    }
                }
            }
            sql = pub.getFiledPermissonSQL(roleId, menuId, funId,tableName);


            funPermissonBll = null;
            personapermissonBll = null;
            return sql;
        }


        /// <summary>
        /// 获取有权限的字段
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <param name="menuid">菜单ID</param>
        /// <param name="funid">功能ID</param>
        /// <returns>返回有权限的字段集合</returns>
        public string[] getFiledPermisson(int roleid, int menuid, string funName)
        {
            bll.funPermissonbll funPermissonBll = new bll.funPermissonbll();
            bll.personapermissonbll personapermissonBll = new bll.personapermissonbll();
            int[] funIds = personapermissonBll.selectFunPersonaPermisson(roleid, menuid);
            int funid = 0;
            if (funIds.Length > 0)
            {
                for (int i = 0; i < funIds.Length; i++)
                {
                    int myFunId = funPermissonBll.getFunName(helpcommon.ParmPerportys.GetNumParms(funIds[i])).Id;
                    string myFunName = funPermissonBll.getFunName(helpcommon.ParmPerportys.GetNumParms(funIds[i])).FunName;
                    if (!string.IsNullOrWhiteSpace(myFunName))
                    {
                        if (myFunName.Contains(funName))
                        {
                            funid = myFunId;
                        }
                    }
                }
            }

            string[] filed = new string[] { };

            int[] temp = personapermissonBll.selectFiledPersonaPermisson(roleid, menuid, funid);

            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            filed = tableFiledPerssionBll.getFiledName(temp);

            personapermissonBll = null;
            tableFiledPerssionBll = null;

            return filed;
        }
        #endregion
       
    }
    public class funName
    {
        /// <summary>
        /// 查询
        /// </summary>
        public static string selectName = "查询";
        /// <summary>
        /// 货号合并
        /// </summary>
        public static string MergeselectName = "货号合并";
        /// <summary>
        /// 编辑
        /// </summary>
        public static string updateName = "编辑";
        /// <summary>
        /// 添加
        /// </summary>
        public static string addName = "添加";
        /// <summary>
        /// 删除
        /// </summary>
        public static string deleteName = "删除";
        /// <summary>
        /// 启用
        /// </summary>
        public static string Enabled = "启用";
        /// <summary>
        /// 停用
        /// </summary>
        public static string disable = "停用";
        /// <summary>
        /// 更新库存
        /// </summary>
        public static string StartUpdate = "更新库存";
        /// <summary>
        /// 更新商品
        /// </summary>
        public static string StartBalance = "更新商品";
        /// <summary>
        /// 商品销售 
        /// </summary>
        public static string SalesState = "商品销售";
        /// <summary>
        /// 入库
        /// </summary>
        public static string Storage = "入库";
        /// <summary>
        /// 查看商品
        /// </summary>
        public static string LookShop = "查看商品";
        /// <summary>
        /// 查看图片
        /// </summary>
        public static string LookPice = "查看图片";
        /// <summary>
        /// 商品标记
        /// </summary>
        public static string Marker = "商品标记";
        /// <summary>
        /// 查看属性
        /// </summary>
        public static string LookAttr = "查看属性";
        /// <summary>
        /// 减去库存
        /// </summary>
        public static string SubtractBalance = "减去库存";
        /// <summary>
        /// 修改价格
        /// </summary>
        public static string UpdatePrice = "修改价格";
        /// <summary>
        /// 选货
        /// </summary>
        public static string CheckProduct = "选货";
        /// <summary>
        /// 现货下单
        /// </summary>
        public static string PlaceAnOrder = "现货下单";
        /// <summary>
        /// 更新淘宝
        /// </summary>
        public static string UpdateTb = "更新淘宝";
        /// <summary>
        /// 客户确认
        /// </summary>
        public static string CustomerOk = "客户确认";
        /// <summary>
        /// 供应商确认
        /// </summary>
        public static string VencodeOk = "供应商确认";
        /// <summary>
        /// 结算
        /// </summary>
        public static string Js="结算";
        /// <summary>
        /// 导入数据
        /// </summary>
        public static string ImportData = "导入数据";
        /// <summary>
        /// 修改淘宝价格
        /// </summary>
        public static string UpdatePriceTb = "修改价格";
        /// <summary>
        /// 修改淘宝库存
        /// </summary>
        public static string UpdateBalanceTb = "修改库存";
        /// <summary>
        /// 修改淘宝货号
        /// </summary>
        public static string UpdateScodeTb = "修改货号";
        /// <summary>
        /// 淘宝上架
        /// </summary>
        public static string HeadBlock = "上架";
        /// <summary>
        /// 增加商品   销售计划中
        /// </summary>
        public static string AddProduct = "增加商品";
        #region 权限管理
        /// <summary>
        /// 菜单分配权限
        /// </summary>
        public static string menuFPQX = "菜单分配权限";

        /// <summary>
        /// 功能分配权限
        /// </summary>
        public static string funFPQX = "功能分配权限";

        /// <summary>
        /// 表格分配权限
        /// </summary>
        public static string tableFPQX = "表格分配权限";


        /// <summary>
        /// 字段分配权限
        /// </summary>
        public static string fieldFPQX = "字段分配权限";


        /// <summary>
        /// 菜单查看权限
        /// </summary>
        public static string menuCKQX = "菜单查看权限";


        /// <summary>
        /// 功能查看权限
        /// </summary>
        public static string funCKQX = "功能查看权限";


        /// <summary>
        /// 表格查看权限
        /// </summary>
        public static string tableCKQX = "表格查看权限";


        /// <summary>
        /// 字段查看权限
        /// </summary>
        public static string fieldCKQX = "字段查看权限";


        /// <summary>
        /// 菜单列表
        /// </summary>
        public static string menuList = "菜单列表";


        /// <summary>
        /// 功能列表
        /// </summary>
        public static string funList = "功能列表";


        /// <summary>
        /// 表格列表
        /// </summary>
        public static string tableList = "表格列表";


        /// <summary>
        /// 字段列表
        /// </summary>
        public static string filedList = "字段列表";

        /// <summary>
        /// 用户管理
        /// </summary>
        public static string userList = "用户管理";


        ///// <summary>
        ///// 字段列表
        ///// </summary>
        //public static string filedList = "字段列表";

        #endregion

        #region 数据源管理
        public static string start = "启动";
        /// <summary>
        /// 
        /// </summary>
        public static string Log = "该数据源更新日志";
        /// <summary>
        /// 删除该数据源下日志功能
        /// </summary>
        public static string DeleteLog = "日志删除";
        #endregion

        #region 日志管理
        /// <summary>
        /// 数据备份操作
        /// </summary>
        public static string backups = "数据备份";
        /// <summary>
        /// 数据恢复操作
        /// </summary>
        public static string recover = "数据恢复";
        #endregion

        /// <summary>
        /// 查看价格
        /// </summary>
        public static string selectProductPrice = "查看价格";
        /// <summary>
        /// 子订单
        /// </summary>
        public static string selectChildOrder = "子订单";
        /// <summary>
        /// 拆单
        /// </summary>
        public static string splitSingle = "拆单";
        /// <summary>
        /// 审核订单(是否开放给供应商查看,0为未开放，1为开放)
        /// </summary>
        public static string checkOrderShow = "审核";
        /// <summary>
        /// 确认订单状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        public static string sendOrderState = "确认发货";
        /// <summary>
        /// 取消订单(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
        /// </summary>
        public static string cancelOrder = "取消订单";
        /// <summary>
        /// 订单报关
        /// </summary>
        public static string ciqOrder = "订单报关";
        /// <summary>
        /// 上货权限
        /// </summary>
        public static string ShowGoods = "上货";
        /// <summary>
        /// 商品检测
        /// </summary>
        public static string HSCheck = "商品检测";
        /// <summary>
        /// 商检编辑
        /// </summary>
        public static string EditProductCheck = "商检编辑";
        
    }
    public class RoleHelperController:Controller
    {

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public string getRoleData()
        {
            StringBuilder s = new StringBuilder();

            s.Append("<option value='0'>请选择</option>");
            bll.rolebll roleBll = new bll.rolebll();
            DataTable dt = roleBll.getRole();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s.Append("<option value=");
                    s.Append(dt.Rows[i]["Id"].ToString());
                    s.Append(">");
                    s.Append(dt.Rows[i]["PersonaName"].ToString());
                    s.Append("</option>");
                }
            }

            return s.ToString();
        }
    }


    /// <summary>
    /// 订单帮助类（订单状态：1 待确认 2 确认 3 待发货 4 发货 5 收货（交易成功） 11 退单 12 取消订单）
    /// </summary>
    public class OrderHelper
    {
        /// <summary>
        /// 拆单，分配订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="scode">商品货号</param>
        /// <returns></returns>
        public string getBalance(string orderId, string[] scode)
        {
            string s = string.Empty;
            string splitResult = string.Empty;                                     //分配订单返回的结果
            string sqlText = string.Empty;
            string cancelOrder = string.Empty;                                     //取消订单返回结果

            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            DataTable dtTime = apiorderBll.getOrderDetailsMsg(orderId);            //获取原始订单时间

            for (int f = 0; f < scode.Length; f++)                                 //如果一个订单存在多个商品，根据订单ID和货号进行遍历分配供应商
            {
                DataTable dt = new DataTable();                                    //返回当前货号所在订单中的商品购买数量
                DataTable dt1 = new DataTable();                                   //返回当前商品(scode)的供应商，价格，库存3个属性值集合

                string detailsOrderId = string.Empty;                              //子订单ID
                string detailsColor = string.Empty;                                //商品颜色
                string detailsImg = string.Empty;                                  //商品图片
                string detailsTime = string.Empty;                                 //订单插入时间
                string minusBanlaceResult = string.Empty;                          //销售库存减去本次销售数量结果（成功、失败）
                string orderScode = scode[f].ToString();                           //当前商品的货号(scode)
                int orderScodeNum = 0;                                             //子订单商品购买数量
                int totalBalance = 0;                                              //当前商品（scode）的总库存(所有供应商的库存和)
                int saleBalanceDef3 = 0;                                           //当前商品(scode)已售数量(销售字段Def3的值)

                #region 根据orderId,scode获取订单商品购买数量
                //商品购买数量
                if (!string.IsNullOrWhiteSpace(orderId) && !string.IsNullOrWhiteSpace(orderScode))
                {
                    dt = apiorderBll.getOrderMsg(orderId, orderScode);                                                                         //根据订单号和货号返回此scode货号所在订单中的商品数量。
                    orderScodeNum = helpcommon.ParmPerportys.GetNumParms(dt.Rows[0]["detailsSaleCount"].ToString());                           //返回此scode货号所在订单中的商品数量。
                    detailsOrderId = dt.Rows[0]["detailsOrderId"].ToString();                                                                  //子订单ID
                    detailsColor = dt.Rows[0]["detailsColor"].ToString();                                                                      //商品颜色
                    detailsImg = dt.Rows[0]["detailsImg"].ToString();                                                                          //商品图片
                    detailsTime = dtTime.Rows[0]["detailsTime"].ToString();                                                                    //订单插入时间
                }
                #endregion

                #region 当前货号的3个信息值：1.供应商，2.价格，3.库存(因product表中只存在总库存数量，所以应该去productstock表中查询这3个属性值)
                Dictionary<string, decimal> dicPrice = new Dictionary<string, decimal>();                                                      //存放库存数量，价格
                Dictionary<string, int> dicLevel = new Dictionary<string, int>();                                                              //存放数据源NO，库存数量

                Dictionary<string, decimal> dicPrice1 = new Dictionary<string, decimal>();                                                     //存放库存数量，价格
                Dictionary<string, int> dicLevel1 = new Dictionary<string, int>();                                                             //存放数据源NO，库存数量

                ProductStockBLL ProductStockBll = new ProductStockBLL();
                dt1 = ProductStockBll.getScodeBalance(orderScode, false);                                      //返回供应商，价格，库存（当前货号的3个信息值：1.供应商，2.价格，3.库存）


                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    totalBalance += helpcommon.ParmPerportys.GetNumParms(dt1.Rows[i]["balance"].ToString());   //当前商品（scode）的总库存(所有供应商的库存和)
                }

                var m = dt1.AsEnumerable().ToArray();                                                          //dt1转换为数组
                var p = (from c in m orderby c["vencode"].ToString(), c["balance"].ToString(), c["price"].ToString() descending select c).ToList();               //已按供应商，库存，价格排序                          
                saleBalanceDef3 = ProductStockBll.getScodeBalanceSales(orderScode);                            //此scode已售数量
                #endregion

                #region 总库存<=已售数量-1
                if (totalBalance <= saleBalanceDef3 - 1)//总库存<=已售数量-1（因为在bms接收app订单的时候，product表中的def3销售字段就会加上相应的订单商品购买数量）
                {
                    string cancelMsg = "库存不足,系统自动取消，";
                    //update
                    string orderResult = MD5DAL.AppAPIHelper.ChangeOrderStatus(orderId, 3);                    //给app发送取消消息
                    helpcommon.appOrderMsg msg = helpcommon.ReSerialize.ReserializeMethod(orderResult);
                    if (msg != null)
                    {
                        if (msg.Code == "0")   //code：0表示取消订单成功
                        {
                            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                            context.Connection.Open();
                            context.Transaction = context.Connection.BeginTransaction();

                            bll.sourceorderbll sourceorderBll = new bll.sourceorderbll();
                            ProductBll Productbll = new ProductBll();
                            cancelOrder += sourceorderBll.cancelOrder(orderId, context);                        //bms取消订单（修改相关数据表状态为12）
                            minusBanlaceResult = Productbll.minusBanlace(orderScode, orderScodeNum, context);   //减掉此次购买商品数量  helpcommon.ParmPerportys.GetNumParms(dt1.Rows[i]["balance"].ToString());

                            if (cancelOrder.Contains("成功") && minusBanlaceResult.Contains("成功"))
                            {
                                context.Transaction.Commit();
                            }
                            else
                            {
                                context.Transaction.Rollback();
                            }
                            sourceorderBll = null;
                            Productbll = null;
                        }
                        else
                        {
                            cancelMsg += "(" + msg.Message + ")给app推送取消订单信息失败";
                        }
                    }
                    return cancelMsg + orderResult;                                                            // "库存不足，请取消此订单";
                }
                #endregion

                #region 分配供应商(一个订单多件商品，可能会被拆分到几个供应商。这里指同款商品多个或多款商品多个)
                int restNum = orderScodeNum;    //分配到几个供应商，依次分配后，还剩下多少需要分配
                int state = 0;      //
                for (int i = 0; i < p.Count; i++)
                {
                    if (state > 0)
                    {
                        continue;
                    }

                    #region 检测是否存在分配成功却又取消分配的订单（需重新分配）
                    DataTable dtSendSource = new bll.sourceorderbll().getOrderSendData(orderId, orderScode);                 //根据订单ID在apisendorder源头表中检测是否存在分配成功却又取消分配的订单（需重新分配）
                    sqlText += @"update apiorderdetails set detailsEditTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "'  where orderId='" + orderId + "' and detailsscode='" + orderScode + "';";   //子订单编辑修改的时间
                    #endregion

                    if (helpcommon.ParmPerportys.GetNumParms(p[i]["balance"].ToString()) >= orderScodeNum)          //判断哪些数据源中的商品库存量>客户所购商品数量（供应商的库存已倒序）
                    {
                        #region 某个供应商存在足够多的库存，直接分配完成
                        //如果是取消过的订单
                        //update  p[i]["vencode"].ToString()
                        //比较vencode，取    取消订单的供应商的下一个
                        //select * from apiSendOrder where orderId = dt.Rows[0]["orderId"].ToString()

                        if (dtSendSource.Rows.Count == 1)    //存在分配成功却又取消分配的订单（需重新分配）【分配成功却又取消分配的订单只处理分配到一个供应商的订单，多个供应商由人工分配】
                        {
                            #region 存在分配成功却又取消分配的订单（需重新分配），因为前面已分配过，所以无需新增，修改即可
                            if (p[i]["vencode"].ToString() == dtSendSource.Rows[0]["sendSource"].ToString())
                            {
                                if (i + 1 < p.Count)
                                {
                                    //根据订单号、货号更改订单数量，编辑时间，供应商
                                    sqlText += @"update apiSendOrder set  newSaleCount='" + orderScodeNum + "',editTime='" + DateTime.Now.ToString("yyyy-MM-dd") + "',sendSource = '" + p[i + 1]["vencode"].ToString() + "' where orderId='" + orderId + "' and newscode='" + orderScode + "';";
                                    state++;
                                }
                                else
                                {
                                    return "订单(" + orderId + ")无供应商可分配，请取消订单";
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 不存在分配成功却又取消分配的订单（新增记录）
                            //否则insert
                            var sendOrderId = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999).ToString();
                            sqlText += @"insert into apiSendOrder(orderId,detailsOrderId,newOrderId,newScode,newColor,newSize,newImg,newSaleCount,
newStatus,createTime,editTime,showStatus,sendSource) values('"
                                        + orderId + "','"
                                        + detailsOrderId + "','"
                                        + sendOrderId + "','"
                                        + orderScode + "','"
                                        + detailsColor + "','"
                                        + "','"
                                        + detailsImg + "',"
                                        + orderScodeNum + ","
                                        + 1 + ",'" //新订单状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
                                        + detailsTime + "','"
                                        + DateTime.Now.ToString("yyyy-MM-dd") + "',"
                                        + 0 + ",'"//审核开放(是否开放给供应商查看,0为未开放，1为开放)
                                        + p[i]["vencode"].ToString() + "');";
                            state++;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region 一个供应商库存不足，需几个供应商加在一起

                        if (dtSendSource.Rows.Count > 1)    //存在分配成功却又取消分配的订单（需重新分配）
                        {
                            return "人工处理一个订单分配到几个供应商的订单";
                        }
                        else
                        {
                            sqlText += @"insert into apiSendOrder(orderId,detailsOrderId,newOrderId,newScode,newColor,newSize,newImg,newSaleCount,
newStatus,createTime,editTime,showStatus,sendSource) values('"
                                    + orderId + "','"
                                    + detailsOrderId + "','"
                                    + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999) + "','"
                                    + orderScode + "','"
                                    + detailsColor + "','"
                                    + "','"
                                    + detailsImg + "',"
                                    + helpcommon.ParmPerportys.GetNumParms(p[i]["balance"].ToString()) + ","
                                    + 1 + ",'" //新订单状态(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
                                    + detailsTime + "','"
                                    + DateTime.Now.ToString("yyyy-MM-dd") + "',"
                                    + 0 + ",'"//审核开放(是否开放给供应商查看,0为未开放，1为开放)
                                    + p[i]["vencode"].ToString() + "');";

                            if (helpcommon.ParmPerportys.GetNumParms(p[i]["balance"].ToString()) >= restNum)                            //当分配完成后
                            {
                                restNum = restNum - helpcommon.ParmPerportys.GetNumParms(p[i]["balance"].ToString());                   //剩余购买数量
                                state++;
                            }
                        }

                        #endregion
                    }
                }
                #endregion

            }

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
            con.Open();
            SqlTransaction st = con.BeginTransaction();
            SqlCommand command = new SqlCommand();
            try
            {
                //def2   是否已分配(0未分配，1分配)
                sqlText += @"update apiorder set def2 = 1 where orderId='" + orderId + "';";

                IDataParameter[] ipara = new IDataParameter[] { new SqlParameter("@sqlText", SqlDbType.NVarChar, 4000) };
                ipara[0].Value = sqlText;
                command.Connection = con;
                command.Parameters.AddRange(ipara);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sendSource";
                command.Transaction = st;
                command.ExecuteNonQuery();

                st.Commit();
                splitResult = "订单(" + orderId + ")分配成功";
            }
            catch (Exception ex) { st.Rollback(); splitResult = "分配失败，程序错误" + ex.Message + sqlText; }

            return splitResult;
        }

        /// <summary>
        /// 判断是否存在此账号
        /// </summary>
        /// <returns></returns>
        public string GetUserId(string username,string password)
        {
            string s = string.Empty;

            #region shop表查找是否存在

            #endregion

            return s;
        }
    
    }
    /// <summary>
    /// 库存帮助类
    /// </summary>
    public class ProductHelper :BaseController
    {
        shopbll sbl = new shopbll();
        /// <summary>
        /// 品牌下拉列表
        /// </summary>
        /// <returns></returns>
        public DataTable ProductStyleDDlist(string Courstomer)
        {
            bll.Attributebll attrbll = new bll.Attributebll();
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetCatDDlist(Courstomer);
            return dt1;
        }
        /// <summary>
        /// 得到所有品牌下拉
        /// </summary>
        /// <returns></returns>
        public DataTable ProductStyleDDlist() 
        {
            bll.Attributebll attrbll = new bll.Attributebll();
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetCatDDlist();
            return dt1;
        }
        /// <summary>
        /// 类别下拉列表
        /// </summary>
        /// <returns></returns>
        public DataTable ProductTypeDDlist(string Courstomer)
        {
            bll.Attributebll attrbll = new bll.Attributebll();
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetTypeDDlist(Courstomer);
            return dt1;
        }
        /// <summary>
        /// 得到所有类别下拉列表
        /// </summary>
        /// <returns></returns>
        public DataTable ProductTypeDDlist()
        {
            bll.Attributebll attrbll = new bll.Attributebll();
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetTypeDDlist();
            return dt1;
        }
        /// <summary>
        /// 店铺下拉列表  根据用户
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        public DataTable ProductLocDDlist(string Customer) 
        {

            bll.ActivityBLL ab = new ActivityBLL();
            DataTable dt = new DataTable();
            dt = ab.GetShopAllocationDDlist(Customer);
            return dt;
        }
        /// <summary>
        /// 品牌下拉列表
        /// </summary>
        /// <returns></returns>
        public string GetBrandDDlist(int UserId)
        {
            string s = string.Empty;
            
            bll.Attributebll attrbll = new bll.Attributebll();
            DataTable dt1 = new DataTable();
            dt1 = attrbll.GetCatDDlist(UserId.ToString());
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s += "<option value='" + dt1.Rows[i][1].ToString() + "'>" + dt1.Rows[i][0].ToString() + "</option>";
            }
            return s;
        }
        

        /// <summary>
        /// 类别下拉列表
        /// </summary>
        /// <returns></returns>
        public string GetTypeDDlist(int UserId)
        {
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            bll.Attributebll attrbll = new bll.Attributebll();
            dt1 = attrbll.GetTypeDDlist(UserId.ToString());

            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s += "<option value='" + dt1.Rows[i][1].ToString() + "'>" + dt1.Rows[i][0].ToString() + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 显示页码数量列表
        /// </summary>
        /// <returns></returns>
        public string GetPageDDlist()
        {
            string s = string.Empty;
            string[] ss = { "5", "10", "20", "50" };
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < ss.Length; i++)
            {
                s += "<option value='" + ss[i] + "'>" + ss[i] + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 显示数据源列表
        /// </summary>
        /// <returns></returns>
        public string GetVencodeDDlist()
        {
            string s = string.Empty;
            DataTable dt1 = new DataTable();
            bll.Attributebll attrbll = new bll.Attributebll();
            dt1 = DbHelperSQL.Query(@"select * from productsource").Tables[0];
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                s += "<option value='" + dt1.Rows[i]["SourceCode"].ToString() + "'>" + dt1.Rows[i]["sourceName"].ToString() + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 店铺下拉列表
        /// </summary>
        /// <returns></returns>
        public string GetShopDDlist() 
        {
            shopbll sp = new shopbll();
            DataTable dt = sp.DropListShop();
            string s = string.Empty;
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                s += "<option value='" + dt.Rows[i]["Id"].ToString() + "'>" + dt.Rows[i]["ShopName"].ToString() + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 用户下拉列表
        /// </summary>
        /// <returns></returns>
        public string UserDDlist()
        {
            List<users> list1 = sbl.BindUserName();
            string s = string.Empty;
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < list1.Count; i++)
            {
                s += "<option value='" + list1[i].Id+ "'>" + list1[i].userRealName + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 店铺类型列表
        /// </summary>
        /// <returns></returns>
        public string GetShopTypeDDlist() 
        {
            List<shoptype> list = sbl.BindShopType();
            string s = string.Empty;
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < list.Count; i++)
            {
                s += "<option value='" + list[i].Id + "'>" + list[i].ShoptypeName + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 得到某个客户所拥有的店铺
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetShopByUserDDlist(int userId) 
        {
            DataTable dt = sbl.GetShopNameByUserId(userId.ToString());
            string s = string.Empty;
            s += "<option value=''>请选择</option>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += "<option value='" + dt.Rows[i]["Id"].ToString() + "'>" + dt.Rows[i]["ShopName"].ToString() + "</option>";
            }
            return s;
        }
        /// <summary>
        /// 判断分页
        /// </summary>
        /// <param name="ThisPageCount">当前页码</param>
        /// <param name="PageCount">最大页码</param>
        /// <param name="Index">分页操作   0首页 1 上页 2跳页 3 下页 4末页 </param>
        /// <returns>当前需要的页码</returns>
        public int PageIndex(int ThisPageCount, int PageCount, int Index) 
        {
            switch (Index)
            {
                case 0:        //首页
                    ThisPageCount = 0;
                    break;
                case 1:         //上一页
                    if (ThisPageCount - 1 >= 0)
                    {
                        ThisPageCount = ThisPageCount - 1;
                    }
                    break;
                case 2:         //跳页 
                    ThisPageCount = ThisPageCount+0;
                    break;
                case 3:        //下页
                    if (ThisPageCount + 1 <= PageCount - 1)
                    {
                        ThisPageCount = ThisPageCount + 1;
                    }
                    break;
                case 4:        //末页
                    ThisPageCount = PageCount - 1;
                    break;
            }
            return ThisPageCount;
        }
        /// <summary>
        /// 表头字段排序
        /// </summary>
        /// <param name="str">权限字段数组</param>
        /// <param name="Index">当前字段需要放置的位置  从0开始</param>
        /// <param name="Name">需要排序字段名称</param>
        /// <returns>排序后的数组</returns>
        public string[] TableHeader(string[] str, int Index, string Name)
        {
            if (str.Contains(Name))
            {
                string temp = str[Index];
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == Name)
                    {
                        str[i] = temp;
                        str[Index] = Name;
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 绑定显示内容
        /// </summary>
        public class KeyValues
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }


        /// <summary>
        /// Excel文档
        /// </summary>
        /// <param name="table"></param>
        /// <param name="file"></param>
        public void dataTableToCsv(DataTable table, string file, List<KeyValues> List)
        {
            string title = "";
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.UTF8);
            for (int i = 0; i < List.Count; i++)
            {
                title += List[i].Value + "\t";
            }
            string[] strColumName = new string[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                strColumName[i] = table.Columns[i].ColumnName;
            }
            title = title.Substring(0, title.Length - 1) + "\n";
            sw.Write(title);
            for (int j = 0; j < table.Rows.Count; j++)
            {
                string line = "";
                for (int i = 0; i < List.Count; i++)
                {
                    if (strColumName.Contains(List[i].Key))
                    {
                        line += "" + table.Rows[j][List[i].Key].ToString().Trim() + "\t";
                    }
                    else
                    {
                        line += "\t";
                    }
                }
                line = line.Substring(0, line.Length - 1) + "\n";
                sw.Write(line);
            }
            sw.Close();
            fs.Close();
            //dataTableToCsv(dt, @"c:\1.xls"); //调用函数

            //System.Diagnostics.Process.Start(@"c:\1.xls");  //打开excel文件
        }


        /// <summary>
        /// 品牌搜索下拉
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetDropList(string sql)
        {
            DataTable dt1 = DbHelperSQL.Query(sql).Tables[0];
            List<object> list = new List<object>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                var family = new { BrandName = dt1.Rows[i]["BrandName"], BrandAbridge = dt1.Rows[i]["BrandAbridge"] };

                list.Add(family);
            };
            var js = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list);
            return js;
        }
        /// <summary>
        /// 类别搜索下拉
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string GetDropList1(string sql)
        {


            DataTable dt1 = DbHelperSQL.Query(sql).Tables[0];
            List<object> list = new List<object>();

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                var family = new { TypeName = dt1.Rows[i]["TypeName"], TypeNo = dt1.Rows[i]["TypeNo"] };

                list.Add(family);
            };
            var js = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(list);
            return js;
        }



    }




}
