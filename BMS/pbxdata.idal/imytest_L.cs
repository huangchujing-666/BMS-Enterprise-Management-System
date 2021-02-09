/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       imytest_L
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       imytest_L
    * 创建时间：       2015-08-11 10:19:16
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface imytest_L
    {
        string insert_L(object o);

        string delete_L(object o);

        string get_L(string tableName);
    }
}
