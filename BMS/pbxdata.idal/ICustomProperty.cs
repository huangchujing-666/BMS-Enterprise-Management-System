using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface ICustomProperty
    {
        string connectionString { get; }
        /// <summary>
        /// 修改客户属性表
        /// </summary>
        /// <param name="customPro">客户属性实体</param>
        /// <param name="mess"></param>
        /// <returns></returns>
         bool UpdateCustomProperty(model.CustomProperty customPro, out string mess);
         bool DeleteCustomProperty(int id, out string mess);
         void insert();
         bool InsertCustomProperty(model.CustomProperty customPro, out string mess);
         List<model.CustomProperty> GetCustomPropertyList(int skip, int take, out int listCount);
         List<model.CustomProperty> GetCustomPropertyList(Dictionary<string, string> dic, int skip, int take, out int listCount);
    }
}
