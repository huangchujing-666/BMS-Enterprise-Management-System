/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       menubll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       menubll
    * 创建时间：       2015/2/2 13:55:09
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
    public class menubll:dataoperatingbll
    {
        imenu dal = (imenu)ReflectFactory.CreateIDataOperatingByReflect("menudal");


        public List<model.menu> getMenu(IDataParameter[] ipara, string procName)
        {
            return dal.getMenu(ipara, procName);
        }


                /// <summary>
        /// 获取菜单名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public model.menu getMenuName(int id)
        {
            return dal.getMenuName(id);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public string del(int id)
        {
            return dal.del(id);
        }

    }
}
