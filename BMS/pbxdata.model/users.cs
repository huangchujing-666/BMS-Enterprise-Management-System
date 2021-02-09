/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       users
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.model
    * 文 件 名：       users
    * 创建时间：       2015-03-19 10:16:55
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
    public partial class users: System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
    {
        private int _state;

        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}
