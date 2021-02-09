using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public partial interface ISystemConfig
    {
        /// <summary>
        /// 获取所有供应商信息
        /// </summary>
        /// <param name="exx">异常信息</param>
        /// <returns></returns>
        List<pbxdata.model.productsource> GetProductsource( out Exception exx);
        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        bool DeleteProductsources(string Code, out Exception exx);
        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <param name="pro">供应商实体</param>
        /// <param name="exx">错误对象</param>
        /// <returns></returns>
        bool InsertProductsources(model.productsource pro,out Exception exx);
        /// <summary>
        /// 查找指定数据源信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        model.productsource GetProductsource(string Id, out Exception exx);
        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        bool UpdateProductsource(model.productsource pro, out Exception exx);
       
        /// <summary>
        /// 获取所有数据源信息集合
        /// </summary>
        /// <returns></returns>
        DataTable GetProductsourceConfig(out Exception exx);
        /// <summary>
        /// 删除数据源信息
        /// </summary>
        /// <param name="Id">数据源编号</param>
        /// <param name="exx">错误信息</param>
        /// <returns></returns>
        bool DeleteProductsourceConfig(int Id,out Exception exx);
        /// <summary>
        /// 添加数据源信息
        /// </summary>
        /// <param name="pro">数据源实体</param>
        /// <param name="exx">错误信息</param>
        /// <returns></returns>
        bool InsertProductsourceConfig(model.productsourceConfig pro, out Exception exx);
        /// <summary>
        /// 修改数据源信息
        /// </summary>
        /// <param name="pro">数据源实体</param>
        /// <param name="exx">错误信息</param>
        /// <returns></returns>
        bool UpdateProductsourceConfig(model.productsourceConfig pro, out Exception exx);
        /// <summary>
        /// 查询指定数据源信息
        /// </summary>
        /// <param name="Id">数据源编号</param>
        /// <param name="exx">错误信息</param>
        /// <returns></returns>
        model.productsourceConfig GetProductsourceConfig(int Id, out Exception exx);
        /// <summary>
        /// 获取所有供应商编号名称
        /// </summary>
        /// <param name="exx"></param>
        /// <returns></returns>
        Dictionary<string, string> GetProductsource1( out Exception exx);
        /// <summary>
        /// 获取所有数据源更新日志信息
        /// </summary>
        /// <param name="exx"></param>
        /// <returns></returns>
        DataTable GetProGetProductsourceUpdateLog(int skip, int take,string log,string beginTime,string endTime,out int count, out Exception exx);
        /// <summary>
        /// 获取所有数据源更新日志信息指定供应商
        /// </summary>
        /// <returns></returns>
        DataTable GetProGetProductsourceUpdateLog(int skip, int take, string SourceCode,string log,string beginTime,string endTime,out int count, out Exception exx);
    }
}
