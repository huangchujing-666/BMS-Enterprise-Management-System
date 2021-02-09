/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       funPermissonbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       funPermissonbll
    * 创建时间：       2015/2/10 15:27:19
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.dal;
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
    public class funPermissonbll : dataoperatingbll
    {
        ifunPermisson dal = (ifunPermisson)ReflectFactory.CreateIDataOperatingByReflect("funPermissondal");


        public List<model.funpermisson> getFun(IDataParameter[] ipara, string procName)
        {
            return dal.getFun(ipara, procName);
        }

        /// <summary>
        /// 获取功能名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public model.funpermisson getFunName(int id)
        {
            return dal.getFunName(id);
        }

        /// <summary>
        /// 删除功能 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string del(int id)
        {
            return dal.del(id);
        }
    }
}
