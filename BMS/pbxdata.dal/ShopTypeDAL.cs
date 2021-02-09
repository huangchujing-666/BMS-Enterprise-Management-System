using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using System.Data;
using System.Data.SqlClient;
using pbxdata.idal;
namespace pbxdata.dal
{
    public class ShopTypeDAL : dataoperating, iShopTypeDAL
    {
        /// <summary>
        /// 获取所有的店铺类型信息
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAllShopType(int minid,int maxid,string shoptypename,out int count)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("minid",minid),
                new SqlParameter("maxid",maxid),
                new SqlParameter("sqlbody",""),
                new SqlParameter("shoptypename",shoptypename)
            };
            IDataParameter[] ipr1 = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("sqlbody",""),
                new SqlParameter("shoptypename",shoptypename)
            };
            DataTable dt = Select(ipr1, "SelecAllShopTypeCount");
            count = int.Parse(dt.Rows[0][0].ToString());
            return Select(ipr, "SelectAllShopType");
        }
        /// <summary>
        /// 添加店铺类型名称
        /// </summary>
        /// <param name="typename">店铺类型名称</param>
        /// <returns></returns>
        public string InsertTypeName(string typename)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("typename",typename)
            };
            return Add(ipr, "InsertTypeName");
        }
        /// <summary>
        /// 判断店铺类型是否存在
        /// </summary>
        /// <param name="typename">店铺类型名称</param>
        /// <returns></returns>
        public bool SelectIsIn(string typename)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("typename",typename)
            };
            DataTable dt = Select(ipr, "SelectTypeIsIn");
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断店铺中是否存在商品
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>true表示不存在，false表示存在</returns>
        public bool ProductIsIn(string Id)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("shopId",Id)
            };
            DataTable dt = Select(ipr, "ProuductIsIn");
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 删除店铺（店铺中无商品）
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteShopName(string Id)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Id",Id)
            };
            return Delete(ipr, "DeleteTypeName");

        }
        /// <summary>
        /// 根据ID查出需要编辑的数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DataTable SelectAllById(string Id)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("shopId",Id)
            };
            return Select(ipr, "SelectAllByShopId");
        }
        /// <summary>
        /// 根据Id查出店铺所有信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable SelectShopById(string id) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("Id",id)
            };
            return Select(ipr, "SelectShopById");
        }
        /// <summary>
        /// 根据Id修改数据
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="shopname"></param>
        /// <returns></returns>
        public string UpdateShopNameById(string Id, string shopname)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("id",Id),
                new SqlParameter("shopname",shopname)
            };
            return Update(ipr, "UpdateType");
        }
    }
}
