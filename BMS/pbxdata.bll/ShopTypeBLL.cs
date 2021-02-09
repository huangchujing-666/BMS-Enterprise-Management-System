using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.dal;
using pbxdata.idal;
using System.Data;
namespace pbxdata.bll
{
     
    public class ShopTypeBLL : dataoperatingbll
    {
        ShopTypeDAL std = new ShopTypeDAL();
        /// <summary>
        /// 显示所有店铺类型信息
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAllShopType(int minid,int maxid,string shoptypename,out int count) 
        {
            return std.SelectAllShopType(minid,maxid,shoptypename,out count);
        }
        /// <summary>
        /// 添加店铺类型名称
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public string InsertTypeName(string typename) 
        {
            return std.InsertTypeName(typename);
        }
        /// <summary>
        /// 判断店铺名称是否存在
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public bool SelectIsIn(string typename) 
        {
            return std.SelectIsIn(typename);
        }
        /// <summary>
        /// 判断店铺中是否存在商品
        /// </summary>
        /// <param name="typename">店铺类型Id</param>
        /// <returns></returns>
        public bool ProductIsIn(string typename) 
        {
            return std.ProductIsIn(typename);
        }
        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteShopName(string Id) 
        {
            return std.DeleteShopName(Id);
        }
        /// <summary>
        /// 查找需要编辑的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataTable SelectAllById(string Id) 
        {
            return std.SelectAllById(Id);
        }
        /// <summary>
        /// 根据Id查出店铺所有信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SelectShopById(string id) 
        {
            return std.SelectShopById(id);
        }
        /// <summary>
        /// 根据Id修改店铺名称
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="shopname"></param>
        /// <returns></returns>
        public string UpdateShopNameById(string Id, string shopname) 
        {
            return std.UpdateShopNameById(Id,shopname);
        }
    }
}
