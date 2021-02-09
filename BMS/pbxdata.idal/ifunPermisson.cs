/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ifunPermisson
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       ifunPermisson
    * 创建时间：       2015/2/10 15:24:21
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface ifunPermisson : IDataOperating
    {
        List<model.funpermisson> getFun(IDataParameter[] ipara, string procName);

        model.funpermisson getFunName(int id);


        /// <summary>
        /// 根据功能id删除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string del(int id);
    }
}
