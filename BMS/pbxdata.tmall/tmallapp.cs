/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       tmallapp
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.tmall
    * 文 件 名：       tmallapp
    * 创建时间：       2015-04-01 10:46:38
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
using System.Runtime.Serialization.Json;
namespace pbxdata.tmall
{
    public class tmallapp
    {
        //public tmallapp(string appkey,string appsecret)
        //{
        //    this._appkey = appkey;
        //    this._appsecret = appsecret;
        //}

        #region porperty

        /// <summary>
        /// appkey
        /// </summary>
        private string _appkey = "21738994";

        public string Appkey
        {
            get { return _appkey; }
            set { _appkey = value; }
        }

        /// <summary>
        /// appsecret
        /// </summary>
        private string _appsecret = "94bbf1b464e4983726c97b4461dfb448";

        public string Appsecret
        {
            get { return _appsecret; }
            set { _appsecret = value; }
        }

        /// <summary>
        /// session
        /// </summary>
        private string _sessionkey;

        public string Sessionkey
        {
            get { return _sessionkey; }
            set { _sessionkey = value; }
        }

        private string _url = "http://gw.api.taobao.com/router/rest";

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }


        private string _appShopName;

        public string AppShopName
        {
            get { return _appShopName; }
            set { _appShopName = value; }
        }

        #endregion




    }
}
