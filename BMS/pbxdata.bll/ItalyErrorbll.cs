using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    /// <summary>
    /// 意大利数据源更新异常信息类
    /// </summary>
    public class ItalyErrorbll
    {
        iItalyError dal = (iItalyError)ReflectFactory.CreateIDataOperatingByReflect("ItalyErrordal");
        /// <summary>
        /// 根据条件获取异常信息
        /// </summary>
        /// <param name="error">搜索实体</param>
        /// <param name="skip">跳过多少条数据</param>
        /// <param name="take">去多少条数据</param>
        /// <returns></returns>
        public DataTable GetItaly(string ItalyCode, DateTime? createTime, DateTime? createTimeEnd, int skip, int take,out int pageRowsCount)
        {
            return dal.GetItaly(ItalyCode, createTime, createTimeEnd, skip, take,out pageRowsCount);
        }
    }
}
