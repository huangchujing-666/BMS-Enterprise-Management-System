using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.idal;
using Maticsoft.DBUtility;
using System.Data;
using System.Data.SqlClient;
using pbxdata.model;
using System.ComponentModel.DataAnnotations;
namespace pbxdata.dal
{
    public class Shopdal : dataoperating, Shopidal
    {
        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }

        /// <summary>
        /// 查看所有店铺信息
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAllShop(int minid, int maxid, string shopname, string shoptypeid, string managerid, out int count)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
               new SqlParameter("sql",""),
               new SqlParameter("minid",minid),
               new SqlParameter("maxid",maxid),
               new SqlParameter("shopName",shopname),
               new SqlParameter("shopManagerId",managerid),
               new SqlParameter("ShopTypeId",shoptypeid),
               new SqlParameter("Condition","")
            };
            DataTable dt = Select(ipr, "SelectAllShop");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ShopState"].ToString() == "1")
                {
                    dt.Rows[i]["ShopState"] = "已启用";
                }
                else
                {
                    dt.Rows[i]["ShopState"] = "未启用";
                }
            }
            IDataParameter[] ipr1 = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("shopName",shopname),
                new SqlParameter("shopManagerId",managerid),
                new SqlParameter("ShopTypeId",shoptypeid),
                new SqlParameter("Condition","")
            };
            DataTable ds = Select(ipr1, "SelectAllShopCount");
            count = int.Parse(ds.Rows[0][0].ToString());
            return dt;
        }
        /// <summary>
        /// 查询店铺中是否有商品
        /// </summary>
        /// <returns></returns>
        public bool SelectShopIdIsIn(string id)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("shopId",id)
            };
            DataTable dt = Select(ipr, "SelectIsIdIn");
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <returns></returns>
        public string DeleteShop(string id)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("shopId",id)
            };
            return Delete(ipr, "DeleteShop");
        }
        /// <summary>
        /// 绑定下拉列表店铺类型
        /// </summary>
        /// <returns></returns>
        public List<shoptype> BindShopType()
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            var info = pddc.shoptype;
            List<shoptype> list = new List<shoptype>();
            shoptype sp = new shoptype();
            sp.Id = 0;
            sp.ShoptypeName = "请选择";
            list.Add(sp);
            foreach (var temp in info)
            {
                shoptype shop = new shoptype();
                shop.Id = temp.Id;
                shop.ShoptypeName = temp.ShoptypeName;
                list.Add(shop);
            }

            return list;
        }
        /// <summary>
        /// 绑定下拉列表店铺管理人
        /// </summary>
        /// <returns></returns>
        public List<users> BindUserName()
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            var info = pddc.users;
            List<users> list = new List<users>();
            users user = new users();
            user.Id = 0;
            user.userRealName = "请选择";
            list.Add(user);
            foreach (var temp in info)
            {
                users us = new users();
                us.Id = temp.Id;
                us.userRealName = temp.userRealName;
                list.Add(us);
            }
            return list;
        }
        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <returns></returns>
        public string InsertShop(string shopname, string shoptypeid, string shopstate, string shopmanager, string telphone)
        {
            IDataParameter[] ipr = new IDataParameter[] 
           {
               new SqlParameter("sql",""),
               new SqlParameter("shopname",shopname),
               new SqlParameter("shoptypeid",shoptypeid),
               new SqlParameter("shopstate",shopstate),
               new SqlParameter("shopmanageId",shopmanager),
               new SqlParameter("Telphone",telphone)
           };
            return Add(ipr, "InsertShop");
        }
        /// <summary>
        /// 店铺名称是否已存在
        /// </summary>
        /// <param name="shopname"></param>
        /// <returns></returns>
        public bool ShopNameIsOn(string shopname)
        {
            IDataParameter[] ipr = new IDataParameter[] 
           {
               new SqlParameter("sql",""),
               new SqlParameter("shopname",shopname)
           };
            DataTable dt = Select(ipr, "ShopNameIsOn");
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 店铺当前状态
        /// </summary>
        /// <param name="id">店铺编号</param>
        /// <returns></returns>
        public int ShopState(string id)
        {
            IDataParameter[] ipr = new IDataParameter[] {
               new SqlParameter("sql",""),
               new SqlParameter("Id",id)
           };
            DataTable dt = Select(ipr, "StateIs");
            return int.Parse(dt.Rows[0]["ShopState"].ToString());
        }
        /// <summary>
        /// 修改店铺状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string UpdateShopState(string state, string id)
        {
            IDataParameter[] ipr = new IDataParameter[] 
           {
               new SqlParameter("sql",""),
               new SqlParameter("state",state),
               new SqlParameter("Id",id)
           };
            return Update(ipr, "UpdateState");
        }
        /// <summary>
        /// 修改店铺信息
        /// </summary>
        /// <param name="shopname"></param>
        /// <param name="shoptyppe"></param>
        /// <param name="shopmanager"></param>
        /// <param name="shopstate"></param>
        /// <returns></returns>
        public string UpdateShop(string id, string shopname, string shoptyppe, string shopmanager, string shopstate)
        {
            IDataParameter[] ipr = new IDataParameter[] 
           {
               new SqlParameter("sql",""),
               new SqlParameter("Id",id),
               new SqlParameter("shopname",shopname),
               new SqlParameter("shoptype",shoptyppe),
               new SqlParameter("shopstate",shopstate),
               new SqlParameter("ShopManageId",shopmanager),
           };
            return Update(ipr, "UpdateShop");
        }
        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        /// <returns></returns>
        public DataTable DropListShop()
        {
            IDataParameter[] ipr = new IDataParameter[] 
           {
               new SqlParameter("sql","")
           };
            DataTable dt = Select(ipr, "DropListShop");
            return dt;
        }


        /*------------------------------------------外部店铺----------------------------------------------*/
        /// <summary>
        /// 查询所有合作店铺
        /// </summary>
        /// <returns></returns>
        public DataTable SelectCollaborationShop(string userId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
           {
               new SqlParameter("sql",""),
               new SqlParameter("userId",userId)
           };
            return Select(ipr, "SelectCollaborationShop");
        }
        /// <summary>
        /// 店铺仓库库存
        /// </summary>
        /// <returns></returns>
        public DataTable SelectWarehouseByShop(string[] str, int minid, int maxid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("style",str[0]),
                new SqlParameter("priceMin",str[1]),
                new SqlParameter("priceMax",str[2]),
                new SqlParameter("cat",str[3]),
                new SqlParameter("BalanceMin",str[4]),
                new SqlParameter("BalanceMax",str[5]),
                new SqlParameter("Cat2",str[6]),
                new SqlParameter("sqlbody",""),
                new SqlParameter("sql",""),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("shopid",str[9]),
                new SqlParameter("Imagefile",str[7]),
                new SqlParameter("Def3",str[8]),
                new SqlParameter("imgsql","")
            };
            try
            {
                return Select(ipr, "SelectWarehouseByShop");
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }
        /// <summary>
        /// 获取当前店铺所有款号总数
        /// 
        /// </summary>
        /// <returns></returns>
        public int SelectWarehouseByShopCount(string[] str)
        {

            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("style",str[0]),
                new SqlParameter("priceMin",str[1]),
                new SqlParameter("priceMax",str[2]),
                new SqlParameter("cat",str[3]),
                new SqlParameter("BalanceMin",str[4]),
                new SqlParameter("BalanceMax",str[5]),
                new SqlParameter("Cat2",str[6]),
                new SqlParameter("sqlbody",""),
                new SqlParameter("sql",""),
                new SqlParameter("shopid",str[9]),
                new SqlParameter("Imagefile",str[7]),
                new SqlParameter("Def3",str[8]),
            };
            try
            {
                DataTable dt = Select(ipr, "SelectWarehouseByShopCount");
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 店铺仓库库存  按照货号
        /// </summary>
        /// <param name="str">查询条件</param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable SelectWarehouseByScode(string[] str, int minid, int maxid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("Scode",str[1]),
                new SqlParameter("Style",str[2]),
                new SqlParameter("Brand",str[3]),
                new SqlParameter("Type",str[5]),
                new SqlParameter("minBalance",str[6]),
                new SqlParameter("maxBalance",str[7]),  
                new SqlParameter("minPricea",str[8]),
                new SqlParameter("maxPricea",str[9]),
                new SqlParameter("minTime",str[10]),
                new SqlParameter("maxTime",str[11]),
                new SqlParameter("ShopId",str[12]),
                new SqlParameter("ShowState",str[13]),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("Imagefile",str[14])
            };
            try
            {
                DataTable dt = Select(ipr, "SelectWarehouseByScode");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        /// <summary>
        /// 得到当前店铺当前货号的 是否已经修改价格或者库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetThisScodeState(string scode, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                 new SqlParameter("sql",""),
                 new SqlParameter("Scode",scode),
                 new SqlParameter("shopid",shopid)
            };
            try
            {
                DataTable dt = Select(ipr, "GetThisScodeState");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 获取当前店铺所有商品总数
        /// </summary>
        /// <param name="str">查询条件</param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int SelectWarehouseByScodeCount(string[] str)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("Scode",str[1]),
                new SqlParameter("Style",str[2]),
                new SqlParameter("Brand",str[3]),
                new SqlParameter("Type",str[5]),
                new SqlParameter("minBalance",str[6]),
                new SqlParameter("maxBalance",str[7]),  
                new SqlParameter("minPricea",str[8]),
                new SqlParameter("maxPricea",str[9]),
                new SqlParameter("minTime",str[10]),
                new SqlParameter("maxTime",str[11]),
                new SqlParameter("ShowState",str[13]),
                new SqlParameter("ShopId",str[12]),
                new SqlParameter("Imagefile",str[14])
            };
            try
            {
                DataTable dt = Select(ipr, "SelectWarehouseByScodeCount");
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 通过用户Id查找出用户名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetUserRealNameByUserId(string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userid",userid)
            };
            try
            {
                DataTable dt = Select(ipr, "GetUserRealNameByUserId");
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }
        /// <summary>
        /// 开放销售
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string OpenSalesByStyle(string style, string shopid, string showState, string time, string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("style",style),
                new SqlParameter("shopid",shopid),
                new SqlParameter("showState",showState),
                new SqlParameter("time",time),
                new SqlParameter("userid",userid)
            };
            return Update(ipr, "OpenSalesByStyle");
        }
        /// <summary>
        /// 取消销售
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string CancelSalesByStyle(string style, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("style",style),
                new SqlParameter("shopid",shopid)
            };
            return Update(ipr, "CloseSalesByStyle");
        }
        /// <summary>
        /// 当前店铺   通过款号删除
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string DeleteByStyleAndShopid(string style, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("style",style),
                new SqlParameter("shopid",shopid)
            };
            return Delete(ipr, "DeleteByStyleAndShopid");
        }
        /// <summary>
        /// 当前店铺   通过货号删除
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string DeletebyScodeAndShopid(string scode, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("shopid",shopid)
            };
            return Delete(ipr, "DeletebyScodeAndShopid");
        }
        /// <summary>
        /// 查找当前店铺当前货号的库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public int GetBalanceByScodeAndShopid(string scode, string shopid, string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("shopid",shopid),
                new SqlParameter("vencode",vencode)
            };
            try
            {
                DataTable dt = Select(ipr, "GetBalanceByScodeAndShopid");
                int balance = int.Parse(dt.Rows[0][0].ToString());
                return balance;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }

        }
        /// <summary>
        /// 修改当前店铺库存
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="shopid">店铺编号</param>
        /// <param name="vencode">数据源编号</param>
        /// <param name="balance">库存</param>
        /// <returns></returns>
        public string UpdateBalanceByAndShopIdVencode(string scode, string shopid, string vencode, string balance, string ShowState, string time, string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("shopid",shopid),
                new SqlParameter("vencode",vencode),
                new SqlParameter("balance",balance),
                new SqlParameter("ShowState",ShowState),
                new SqlParameter("time",time),
                new SqlParameter("userid",userid)
            };
            try
            {
                return Update(ipr, "UpdateBalanceByAndShopIdVencode");
            }
            catch (Exception)
            {
                return "修改失败";
                throw;
            }

        }
        /// <summary>
        /// 查找当前店铺下某货号的价格
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetPriceByScodeandShopid(string scode, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("shopid",shopid),
                new SqlParameter("scode",scode),
            };
            try
            {
                DataTable dt = Select(ipr, "GetPriceByScodeandShopid");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }
        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="style"></param>
        /// <param name="price"></param>
        /// <param name="updataprie"></param>
        /// <returns></returns>
        public string UpdatePricecByStyleAndPrice(string style, string price, string updataprie, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("style",style),
                new SqlParameter("pricee",price),
                new SqlParameter("shopid",shopid),
                new SqlParameter("updateprice",updataprie)
            };
            try
            {
                return Update(ipr, "UpdatePricecByStyleAndPrice");
            }
            catch (Exception)
            {
                return "修改失败！";
                throw;
            }
        }
        /// <summary>
        /// ---通过修改价格时修改状态 - --将当前的货号是选货状态改为未开放状态
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string UpdateShowStateByUpdatePrice(string Scode, string shopid, string time, string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode),
                new SqlParameter("ShopId",shopid),
                new SqlParameter("time",time),
                new SqlParameter("userid",userid)
            };
            try
            {
                return Update(ipr, "UpdateShowStateByUpdatePrice");

            }
            catch (Exception)
            {
                return "修改错误！";
                throw;
            }
        }
        /// <summary>
        /// //---通过款号和店铺查询当前款号有多少条货号
        /// </summary>
        /// <param name="style"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public DataTable GetOutSideByStyleAndShopId(string style, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("style",style),
                new SqlParameter("shopid",shopid)
            };
            try
            {
                DataTable dt = Select(ipr, "GetOutSideByStyleAndShopId");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 查找该条货号属性
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="shopid">店铺编号</param>
        /// <param name="vencode">数据源</param>
        /// <returns></returns>
        public DataTable GetShopProductByScodeAndShopIdVencode(string scode, string shopid, string vencode)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("shopid",shopid),
                new SqlParameter("scode",scode),
                new SqlParameter("vencode",vencode),
            };
            try
            {
                DataTable dt = Select(ipr, "GetShopProductByScodeAndShopIdVencode");
                dt.Columns.Add("TypeNo");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["TypeNo"] = dt.Rows[i]["Cat2"];
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 查看货号属性   非选货
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable GetProductstockByScodeAndVencode(string scode, string vencode,string shopid)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("Vencode",vencode),
                new SqlParameter("shopId",shopid)
            };
            try
            {
                DataTable dt = Select(ipr, "GetProductstockByScodeAndVencode");
                dt.Columns.Add("TypeNo");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["TypeNo"] = dt.Rows[i]["Cat2"];
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public DataTable GetdistinctProductStockByScod(string scode) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
            };
            try
            {
                DataTable dt = Select(ipr, "GetProductstockByScodeAndVencode");
                dt.Columns.Add("TypeNo");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["TypeNo"] = dt.Rows[i]["Cat2"];
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        /// <summary>
        /// 查看货号属性  选货
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public DataTable LookAttrByCheck(string scode, string vencode)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("scode",scode),
                new SqlParameter("Vencode",vencode),
            };
            try
            {
                DataTable dt = Select(ipr, "GetLookAttrByCheck");
                dt.Columns.Add("TypeNo");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["TypeNo"] = dt.Rows[i]["Cat2"];
                    var info = pddc.producttype.Where(a => a.TypeNo == dt.Rows[i]["Cat2"].ToString());
                    foreach (var temp in info)
                    {
                        dt.Rows[i]["Cat2"] = temp.TypeName;
                    }
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到当前店铺该货号的状态
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopid"></param>
        /// <returns></returns>
        public string GetShowStateByScodeAndLoc(string scode, string shopid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",scode),
                new SqlParameter("Loc",shopid),
            };
            try
            {
                DataTable dt = Select(ipr, "GetShowStateByScodeAndLoc");
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "0";
                throw;
            }


        }
        /// <summary>
        /// 通过角色Id获得当前角色的标识
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public string GetUserTypeByPersonaId(string RoleId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Id",RoleId)
            };
            try
            {
                DataTable dt = Select(ipr, "GetUserTypeByPersonaId");
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }

        /***********************选货******************/
        public DataTable GetShopNameByUserId(string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userId",userid),
            };
            return Select(ipr, "GetShopNameByUserId");
        }
        /// <summary>
        /// ---------选货  通过货号查询出当前货号已存在的店铺
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable GetShopByScode(string Scode, string user)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Scode",Scode),
                new SqlParameter("ShopUser",user)
            };
            return Select(ipr, "GetShopByScode");
        }
        /*************************分配店铺*********************/
        /// <summary>
        /// 得到所有店铺
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllShop()
        {
            IDataParameter[] ipr = new IDataParameter[] {
            
                new SqlParameter("sql","")
            };
            return Select(ipr, "GetAllShop");
        }
        /// <summary>
        /// 得到所有用户
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllUsers()
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql","")
            };
            return Select(ipr, "GetAllUsers");
        }


        //*************************6.1
        /// <summary>
        /// ---清除店铺权限
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public bool ClearShopAllocationByManagerId(string managerId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("ManagerId",managerId)
            };
            try
            {
                string result = Delete(ipr, "ClearShopAllocationByManagerId");
                if (result == "删除成功")
                {
                    return true;
                }
                else
                {
                    IDataParameter[] ipr1 = new IDataParameter[] 
                    {
                        new SqlParameter("sql",""),
                        new SqlParameter("ManagerId",managerId)
                    };
                    DataTable dt = Select(ipr1, "SelectShopAllocationIsUserId");
                    if (dt.Rows.Count > 0)
                    {
                        return false;
                    }
                    else 
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        //********************6.5
        /// <summary>
        /// ---通过商品编号获淘宝数据信息
        /// </summary>
        /// <param name="RemarkId"></param>
        /// <returns></returns>
        public DataTable GetTbProductByRemarkId(string RemarkId) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("RemarkId",RemarkId)
            };
            try
            {
                DataTable dt = Select(ipr, "GetTbProductByRemarkId");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        /// <summary>
        /// 通过淘宝款号获得本地数据
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public DataTable GetProductByTbStyle(string style,string Scode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("Style",style),
                new SqlParameter("Scode",Scode)
            };
            try
            {
                DataTable dt = Select(ipr, "GetProductByTbStyle");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        /********更新淘宝SKU   2015年8月27日 10:13:15********/
        /// <summary>
        /// 得到所有淘宝商品编号
        /// </summary>
        /// <returns></returns>
        public List<string> GetTbProductRemarkId() 
        {
            try
            {
                pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
                var info = pddc.tbProductReMark;
                List<string> list = new List<string>();
                foreach (tbProductReMark temp in info) 
                {
                    list.Add(temp.ProductReMarkId);
                }
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 插入SKU信息
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool InserttbSkuScode(TbSkuScode ts) 
        {
            try
            {
                string sql = "insert into TbSkuScode(TbRemarkId,TbSkuId,Scode,Balance,Color,TbImage,CreateTime,Price,TbStyle,SaleStatus)"
                +"values('"+ts.TbRemarkId+"','"+ts.TbSkuId+"','"+ts.Scode+"','"+ts.Balance+"','"+ts.Color+"','"+ts.TbImage+"','"+ts.CreateTime+"','"+ts.Price +"','"+ts.TbStyle+"','"+ts.SaleStatus+"')";
                int i= DbHelperSQL.ExecuteSql(sql);
                if (i > 0)
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        /// <summary>
        /// skuId个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int SelectSkuId(string[] str) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("TbRemarkId",str[2]),
                    new SqlParameter("TbStyle",str[0]),
                    new SqlParameter("TbSkuId",str[3]),
                    new SqlParameter("Scode",str[1]),
                    new SqlParameter("SaleStatus",str[4]),
                };
                DataTable dt = Select(ipr, "GetTbSkuScodeCount");
                int count = 0;
                if (dt.Rows.Count > 0) 
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// 商品SKU信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable SelectSkuId(string[] str, int minid, int maxid) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("TbRemarkId",str[2]),
                    new SqlParameter("TbStyle",str[0]),
                    new SqlParameter("TbSkuId",str[3]),
                    new SqlParameter("Scode",str[1]),
                    new SqlParameter("SaleStatus",str[4]),
                    new SqlParameter("minId",minid),
                    new SqlParameter("maxId",maxid)
                };
                DataTable dt = Select(ipr, "GetTbSkuScode");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }

}
