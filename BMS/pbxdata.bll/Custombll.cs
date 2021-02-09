using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using pbxdata.dal;
using pbxdata.idal;
using System.Data;
namespace pbxdata.bll
{
    public class Custombll
    {
        ICustom dal = pbxdata.dalfactory.ReflectFactory.CreateIDataOperatingByReflect("CustomDal") as ICustom;
        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(Dictionary<string, string> Dic, int page, int Selpages, out string counts)
        {
            return dal.GetDate(Dic, page, Selpages, out counts);
        }
         /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(string Id)
        {
            return dal.GetDate(Id);
        }
        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <returns></returns>
        public string AddCustomer(Dictionary<string, string> Dic, int UserId)
        {
            return dal.AddCustomer(Dic, UserId);
        }

        /// <summary>
        /// 保存编辑客户信息
        /// </summary>
        /// <returns></returns>
        public string SaveEdit(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveEdit(Dic, UserId);
        }
        /// <summary>
        /// 显示客户地址信息
        /// </summary>
        /// <returns></returns>
        public DataTable CustomerAddress(string CustomerId)
        {
            return dal.CustomerAddress(CustomerId);
        }

        /// <summary>
        /// 添加客户地址
        /// </summary>
        /// <returns></returns>
        public string SaveAddAddress(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveAddAddress(Dic, UserId);
        }

        /// <summary>
        /// 编辑客户地址
        /// </summary>
        /// <returns></returns>
        public string SaveEditAddress(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveEditAddress(Dic, UserId);
        }
        /// <summary>
        /// 删除客户地址
        /// </summary>
        /// <returns></returns>
        public string DeleteAddress(string Id, int UserId)
        {
            return dal.DeleteAddress(Id, UserId);
        }
        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <returns></returns>
        public string DeleteCustomer(string Id,int UserId)
        {
            return dal.DeleteCustomer(Id,UserId);
        }

         /// <summary>
        /// 查看客户订单信息
        /// </summary>
        /// <returns></returns>
        public DataTable CheckOrder(string CustomerId)
        {
            return dal.CheckOrder(CustomerId);
        }
    }
}
