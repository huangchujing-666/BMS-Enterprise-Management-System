using Maticsoft.DBUtility;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class CustomPropertybll
    {
        ICustomProperty dal = dalfactory.ReflectFactory.CreateIDataOperatingByReflect("CustomPropertydal") as idal.ICustomProperty;
        public CustomPropertybll()
        {
        }
        /// <summary>
        /// 修改客户属性表
        /// </summary>
        /// <param name="customPro">客户属性实体</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool UpdateCustomProperty(model.CustomProperty customPro, out string mess)
        {
            return dal.UpdateCustomProperty(customPro, out mess);
        }
        public bool DeleteCustomProperty(int id, out string mess)
        {
            return dal.DeleteCustomProperty(id, out mess);
        }
        public bool InsertCustomProperty(model.CustomProperty customPro, out string mess)
        {
            return dal.InsertCustomProperty(customPro, out mess);
        }
        public List<model.CustomProperty> GetCustomPropertyList(int skip, int take, out int listCount)
        {
            return dal.GetCustomPropertyList(skip, take, out listCount);

        }
        public List<model.CustomProperty> GetCustomPropertyList(Dictionary<string, string> dic, int skip, int take, out int listCount)
        {
            return dal.GetCustomPropertyList(dic, skip, take, out listCount);
        }
        public void insert()
        {
            dal.insert();
        }
    }
}
