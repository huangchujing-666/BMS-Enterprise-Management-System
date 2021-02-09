using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.dal;
using System.Data;
using Maticsoft.DBUtility;
namespace pbxdata.bll
{
    public class tbProductReMarkBll
    {
        tbProductReMarkDal tpd = new tbProductReMarkDal();
        /// <summary>
        /// 按照条件查找所有数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetProductReMarkTb(Dictionary<string,string> dic, int minid, int maxid) 
        {
            return tpd.GetProductReMarkTb(dic,minid,maxid);
        }
        /// <summary>
        /// 查找出符合当前条件的数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetProductReMarkTbCount(Dictionary<string, string> dic)
        {
            return tpd.GetProductReMarkTbCount(dic);
        }
        /// <summary>
        /// 得到店铺名称
        /// </summary>
        /// <returns></returns>
        public DataTable GetShopName()
        {
            return tpd.GetShopName();
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return tpd.GetList(strWhere);
        }
        /// <summary>
        /// 添加-获取淘宝商品列表数据(淘宝在售-仓库中的数据)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Add(List<CommandInfo> list)
        {
            return tpd.Add(list);
        }
        /// <summary>
        /// 查找相同的编号
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public DataSet GetProductReMarkList(string strwhere)
        {
            return tpd.GetProductReMarkList(strwhere);
        }
        public string[] GetAll()
        {
            return tpd.GetAll();
        }
    }
}
