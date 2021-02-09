using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public partial class SystemConfigManagerbll : dataoperatingbll
    {
        /// <summary>
        /// 获取所有数据源（供应商）
        /// </summary>
        /// <returns></returns>
        public DataTable getSource()
        {
            return dal.getSource();
        }
    }
}
