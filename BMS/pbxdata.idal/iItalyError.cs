using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    /// <summary>
    /// 意大利数据源更新异常信息类
    /// </summary>
    public interface iItalyError
    {
        /// <summary>
        /// 根据条件获取异常信息
        /// </summary>
        /// <param name="error">搜索实体</param>
        /// <param name="skip">跳过多少条数据</param>
        /// <param name="take">去多少条数据</param>
        /// <returns></returns>
         DataTable GetItaly(string ItalyCode, DateTime? createTime, DateTime? createTimeEnd, int skip, int take,out int pageRowsCount);
    }
}
