using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace pbxdata.model
{
    public class taoAppUserModel
    {

        public taoAppUserModel(string appkey, string appSecret) 
        {
            this.tbUsserId = appkey;
            this.accessToken = appSecret;
        }
        private string apiUrl = "http://gw.api.taobao.com/router/rest ";
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 淘宝账号
        /// </summary>
        public string tbUsserId { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string tbUserNick { get; set; }
        /// <summary>
        /// 店铺密钥
        /// </summary>
        public string accessToken { get; set; }
        /// <summary>
        /// 店铺Session
        /// </summary>
        public string refreshToken { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int userId1 { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int userId { get; set; }
        public string Def1 { get; set; }
        public string Def2 { get; set; }
        public string Def3 { get; set; }
        public string Def4 { get; set; }
        public string Def5 { get; set; }

    }
}
