using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public partial class SystemConfigManagerdal : pbxdata.idal.ISystemConfig
    {

        /// <summary>
        /// 获取所有数据源（供应商）
        /// </summary>
        /// <returns></returns>
        public DataTable getSource()
        {
            DataTable dt = new DataTable();
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var p = from c in context.productsource orderby c.SourceLevel descending select c.SourceLevel ;
            dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }
    }
}
