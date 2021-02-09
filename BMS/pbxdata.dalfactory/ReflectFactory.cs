/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       menubll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dalfactory
    * 文 件 名：       ReflectFactory
    * 创建时间：       2015/2/2 13:55:09
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pbxdata.dalfactory
{
    public class ReflectFactory
    {
        public static object CreateIDataOperatingByReflect(string typeName)
        {
            string namespaceStr = "pbxdata.dal." + typeName;
            object objType = null; ;

            try
            {
                objType = Assembly.Load("pbxdata.dal").CreateInstance(namespaceStr);//反射创建
            }
            catch
            { }

            return objType;
        }
        //public static object CreateIDataOperatingByReflect(string typeName)
        //{
        //    string namespaceStr = "pbxdata.dal." + typeName;
        //    object objType = null; ;

        //    try
        //    {
        //        objType = Assembly.Load("pbxdata.dal").CreateInstance(namespaceStr,true, BindingFlags.CreateInstance,null,,,);//反射创建
        //    }
        //    catch
        //    { }

        //    return objType;
        //}
    }
}
