using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface ICustom
    {
        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDate(Dictionary<string, string> Dic, int page, int Selpages, out string counts);

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDate(string Id);

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <returns></returns>
        string AddCustomer(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 保存编辑客户信息
        /// </summary>
        /// <returns></returns>
        string SaveEdit(Dictionary<string, string> Dic, int UserId);
        /// <summary>
        /// 显示客户地址信息
        /// </summary>
        /// <returns></returns>
        DataTable CustomerAddress(string CustomerId);

        /// <summary>
        /// 添加客户地址
        /// </summary>
        /// <returns></returns>
        string SaveAddAddress(Dictionary<string, string> Dic, int UserId);
        /// <summary>
        /// 编辑客户地址
        /// </summary>
        /// <returns></returns>
        string SaveEditAddress(Dictionary<string, string> Dic, int UserId);
        /// <summary>
        /// 删除客户地址
        /// </summary>
        /// <returns></returns>
        string DeleteAddress(string Id, int UserId);
        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <returns></returns>
        string DeleteCustomer(string Id, int UserId);

        /// <summary>
        /// 查看客户订单信息
        /// </summary>
        /// <returns></returns>
        DataTable CheckOrder(string CustomerId);
    }
}
