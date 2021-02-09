using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
namespace pbxdata.idal
{
    public partial interface iProductStock
    {
        string connectionString { get; }
        //*多条件查询和分页**///
        /// <summary> 
        ///查询并分页 已完成存储过程
        /// </summary>
        /// <param name="str">查询条件参数</param>
        /// <param name="count">跳过多少页</param>
        /// <returns></returns>
        DataTable SerachShowProductStock(Dictionary<string,string> dic, int minid,int maxid);
        /// <summary>
        /// 查询条数 已完成存储过程
        /// </summary>
        /// <param name="str"></param>
        /// <param name="skip"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        DataTable SerachShowProductStock(Dictionary<string, string> dic, out int count);
        /// <summary>
        /// 按照款号查询  已完成存储过程
        /// </summary>
        /// <param name="str">查询的字段</param>
        /// <param name="count">跳过多少条查询</param>
        /// <returns></returns>
        DataTable ProductStcokByStyle(string[] str, int minid,int maxid);
        /// <summary>
        /// 款号查找   已完成存储 过程
        /// </summary>
        /// <param name="str">条件</param>
        /// <returns></returns>
        DataTable ProductStcokByStyle(string[] str, out int count);
        /// <summary>
        /// 数据源下拉框
        /// </summary>
        /// <returns></returns>
        List<productsource> DropList();
        /// <summary>
        /// 查询需要导出的数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        DataTable Excel(string [] str);
        /// <summary>
        ///按照货号和数据源查出当前货号的总库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        int CountByScode(string scode, string vencode);
        /// <summary>
        /// 通过数据源ID找出数据源名称
        /// </summary>
        /// <param name="vencdoe"></param>
        /// <returns></returns>
        string SelectVenNameByVencode(string vencode);
        /// <summary>
        /// 通过货号和数据源查找出当前数据的信息
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="socde"></param>
        /// <returns></returns>
        DataTable SelectDataByScodeAndVencode(string vencode, string scode);
    }
}
