/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       taoAppUser
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       taoAppUser
    * 创建时间：       2015/2/9 15:46:04
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
    public class taoAppUserbll:dataoperatingbll
    {
        itaoAppUser dal = (itaoAppUser)ReflectFactory.CreateIDataOperatingByReflect("taoAppUserdal");

        public List<model.taoAppUser> GetModelList(IDataParameter[] ipara, string procName)
        {
            return dal.GetModelList(ipara, procName);
        }



        public model.taoAppUser GetModelByNick(IDataParameter[] ipara, string procName)
        {
            return dal.GetModelByNick(ipara, procName);
        }
    }
}
