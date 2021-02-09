using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.dal;
using System.Data;
namespace pbxdata.bll
{
    public class ShopProductBll
    {
        ShopProductDal spd = new ShopProductDal();
        /// <summary>
        /// 当前货号已分配的库存总和
        /// </summary>
        /// <returns></returns>
        public int BalanceByScode(string Scode)
        {
            return spd.BalanceByScode(Scode);
        }
        /// <summary>
        /// 通过店铺编号获得店铺信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetShopNameByShopId(string id)
        {
            return spd.GetShopNameByShopId(id);
        }
        /// <summary>
        /// 按照货号和店铺Id查找当前数据是否存在
        /// </summary>
        /// <param name="Scode">货号</param>
        /// <param name="ShopId">店铺编号</param>
        /// <returns>true  表示存在该条记录  false 表示不存在</returns>
        public bool DataByScodeAndShopId(string Scode, string ShopId)
        {
            return spd.DataByScodeAndShopId(Scode,ShopId);
        }
        /// <summary>
        /// 如果存在当前店铺存在该货号库存则修改
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopId"></param>
        /// <param name="Balance"></param>
        /// <returns></returns>
        public string UpdateBalanceByShopidAndScode(string scode, string shopId, string Balance, string updatetime, string vencode)
        {
            return spd.UpdateBalanceByShopidAndScode(scode, shopId, Balance, updatetime,vencode);
        }
        /// <summary>
        /// 如果当前店铺不存在该货号则添加
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertBalance(DataTable dt)
        {
            return spd.InsertBalance(dt);
        }
        /// <summary>
        /// 取出外部库存
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public DataTable BalanceAndShopShow(int skip, int take,string vencode,string scode)
        {
           return spd.BalanceAndShopShow(skip,take,vencode,scode);
        }
        /// <summary>
        /// 取出外部库存
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public int BalanceAndShopShowCount(string vencode, string scode)
        {
            return spd.BalanceAndShopShowCount(vencode,scode);
        }
        /// <summary>
        /// 修改库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="shopId"></param>
        /// <param name="Balance"></param>
        /// <param name="updatetime"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string UpdateBalanceByShopid(string scode, string shopId, string Balance, string updatetime, string userid, string vencode)
        {
            return spd.UpdateBalanceByShopid(scode,shopId,Balance,updatetime,userid,vencode);
        }

    }
}
