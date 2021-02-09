using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using System.Data;
namespace pbxdata.idal
{
    public interface iShopTypeDAL
    {

        /// <summary>
        /// 获取所有的店铺类型信息
        /// </summary>
        /// <returns></returns>
        DataTable SelectAllShopType(int minid, int maxid,string shoptypename,out int count);
        /// <summary>
        /// 添加店铺类型名称
        /// </summary>
        /// <param name="typename">店铺类型名称</param>
        /// <returns></returns>
        string InsertTypeName(string typename);
        /// <summary>
        /// 判断店铺类型是否存在
        /// </summary>
        /// <param name="typename">店铺类型名称</param>
        /// <returns></returns>
        bool SelectIsIn(string typename);
        /// <summary>
        /// 判断店铺中是否存在商品
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool ProductIsIn(string Id);
        /// <summary>
        /// 删除店铺（店铺中无商品）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string DeleteShopName(string Id);
        /// <summary>
        /// 得到当前选中行的所有信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        DataTable SelectAllById(string Id);
        /// <summary>
        /// 根据Id查出店铺所有信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable SelectShopById(string id);
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="shopname"></param>
        /// <returns></returns>
        string UpdateShopNameById(string Id,string shopname);
    }
}
