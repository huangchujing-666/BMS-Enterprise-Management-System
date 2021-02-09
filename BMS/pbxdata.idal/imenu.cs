/*************************************************************************************
    * CLR版本：       4.0.30319.18063
    * 类 名 称：       imenu1
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       imenu
    * 创建时间：       2015/2/2 11:31:01
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
    public interface imenu
    {
        List<model.menu> getMenu(IDataParameter[] ipara, string procName);

        model.menu getMenuName(int id);


        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        string del(int id);
    }
}
