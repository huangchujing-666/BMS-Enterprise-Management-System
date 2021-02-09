using pbxdata.dal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class productPorpertybll
    {

        productPorpertydal dal = new productPorpertydal();
        public bool Insert(List<PorpertyModel> list, out Dictionary<int, string> lists)
        {
            return dal.Insert(list, out lists);
        }
        /// <summary>
        /// 查询该类别typeId
        /// </summary>
        /// <param name="TypeName">类别名称</param>
        /// <returns></returns>
        public int SeletTypeId(string TypeName)
        {
            return dal.SeletTypeId(TypeName);
        }
        /// <summary>
        /// 取得当前商品的属性信息
        /// </summary>
        /// <param name="Scode">商品编号</param>
        public List<productPorpertyModel> GetProductPorpertys(string Scode)
        {
            return dal.GetProductPorpertys(Scode);
        }
        /// <summary>
        /// 添加属性值
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertPropertyValue(DataTable dt, out Exception exx)
        {
            return dal.InsertPropertyValue(dt, out exx);
        }
        /// <summary>
        /// 设置dal添加属性完和添加属性值完调用调用方法
        /// </summary>
        /// <param name="del">OnAddProVal事件委托</param>
        /// <param name="del1">OnAddProValDefeat事件委托</param>
        public static void SetOnAddProValAndOnAddProValDefeat(pbxdata.dal.productPorpertydal.AddProValHander del, pbxdata.dal.productPorpertydal.AddProValHander del1)
        {
            productPorpertydal.SetOnAddProValAndOnAddProValDefeat(del, del1);
        }
        ///// <summary>
        ///// 添加属性值信息调用
        ///// </summary>
        ///// <param name="str"></param>
        //public delegate void AddProValHander(string str);
        ///// <summary>
        ///// 添加属性值信息成功事件
        ///// </summary>
        //public static event pbxdata.dal.productPorpertydal.AddProValHander _OnAddProVal = null;
        
        ///// <summary>
        ///// 添加属性值信息失败事件
        ///// </summary>
        //public static event pbxdata.dal.productPorpertydal.AddProValHander _OnAddProValDefeat = null;

    }
}
