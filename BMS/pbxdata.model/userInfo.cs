/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       userInfo
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.model
    * 文 件 名：       userInfo
    * 创建时间：       2015/2/9 15:21:12
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

namespace pbxdata.model
{
    public class userInfo
    {

        public userInfo() { }


        private users _userTable;
        private string _roleId;
        private List<taoAppUser> _taoAppUserList;
        private List<int> _menuIdF;
        private List<int> _menuIdC;
        private List<int> _funId;
        private List<int> _filedId;


        /// <summary>
        /// 用户
        /// </summary>
        public users User
        {
            get { return _userTable; }
            set { _userTable = value; }
        }
        /// <summary>
        /// 用户淘宝店授权信息
        /// </summary>
        public List<taoAppUser> TaoAppUserList
        {
            get { return _taoAppUserList; }
            set { _taoAppUserList = value; }
        }


        /// <summary>
        /// 角色ID(应当从此处分割【如果存在多个角色】)---暂不处理多角色
        /// </summary>
        public string RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        /// <summary>
        /// 菜单ID(读取该用户存在访问权限的菜单列表)
        /// </summary>
        public List<int> MenuIdF
        {
            get { return _menuIdF; }
            set { _menuIdF = value; }
        }

        /// <summary>
        /// 菜单ID(读取该用户存在访问权限的菜单列表)
        /// </summary>
        public List<int> MenuIdC
        {
            get { return _menuIdC; }
            set { _menuIdC = value; }
        }

        /// <summary>
        /// 功能ID(读取该用户存在访问权限的功能列表)
        /// </summary>
        public List<int> FunId
        {
            get { return _funId; }
            set { _funId = value; }
        }

        /// <summary>
        /// 字段ID(读取该用户存在访问权限的字段列表)
        /// </summary>
        public List<int> FiledId
        {
            get { return _filedId; }
            set { _filedId = value; }
        }

    }
}
