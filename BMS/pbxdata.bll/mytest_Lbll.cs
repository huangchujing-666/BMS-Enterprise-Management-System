/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       mytest_Lbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       mytest_Lbll
    * 创建时间：       2015-08-11 10:21:29
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class mytest_Lbll:dataoperatingbll
    {
        imytest_L dal = (imytest_L)ReflectFactory.CreateIDataOperatingByReflect("mytest_Ldal");

        public string insert_L(object o)
        {
            return dal.insert_L(o);
        }

        public string delete_L(object o)
        {
            return dal.delete_L(o);
        }

        public string get_L(string tableName)
        {
            return dal.get_L("");
        }

    }
}
