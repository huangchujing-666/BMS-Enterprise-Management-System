using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class ExcelOperationbll
    {
        dal.ExcelOperation dal = new dal.ExcelOperation();
        public DataTable GetTablesName(string filename,out Exception ex)
        {
            return  dal.GetTablesName(filename,out ex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">excel文件完全限定路径</param>
        /// <param name="workTableName">excel文件工作表名称</param>
        /// <returns></returns>
        public DataSet GetData(string filename, string workTableName, out Exception exx)
        {
          return dal.GetData(filename,workTableName,out exx);
        }
    }
}
