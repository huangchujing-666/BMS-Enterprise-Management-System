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
        /// 获取所有数据源（供应商）
        /// </summary>
        /// <returns></returns>
        DataTable getSource();
    }
}
