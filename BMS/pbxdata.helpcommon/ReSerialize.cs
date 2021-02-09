/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ReSerialize
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.helpcommon
    * 文 件 名：       ReSerialize
    * 创建时间：       2015-07-01 15:24:14
    * 作    者：       lcg
    * 说    明：       字符串反序列化到实体类
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;

namespace pbxdata.helpcommon
{
    public class ReSerialize
    {
        /// <summary>
        /// 反序列化到实体类
        /// </summary>
        public static appOrderMsg ReserializeMethod(string orderResult)
        {
            appOrderMsg msg = JsonConvert.DeserializeObject(orderResult, typeof(appOrderMsg)) as appOrderMsg;
            return msg;
        }
    }

    [Serializable]
    public class appOrderMsg
    {
        /// <summary>
        /// 返回错误信息
        /// </summary>
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 返回结果
        /// </summary>
        private string success;

        public string Success
        {
            get { return success; }
            set { success = value; }
        }

    }
}
