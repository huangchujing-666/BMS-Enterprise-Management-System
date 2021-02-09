using pbxdata.bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{

    public class LoginController : Controller
    {
        //
        // GET: /Home/
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult Index(FormCollection collention)

        public ActionResult Index(string userName, string password)
        {
            IDataParameter[] ipara = new IDataParameter[] { 
                new SqlParameter("userName",SqlDbType.NVarChar,20),
                new SqlParameter("userPwd",SqlDbType.NVarChar,50)
            };
            

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // 密码加密
                password = helpcommon.PasswordHelp.encrypt(password);
                //密码二次加密
                password = helpcommon.PasswordHelp.encrypt(password);
                ipara[0].Value = userName;
                ipara[1].Value = password;

                pbxdata.bll.usersbll userBll = new bll.usersbll();
                //是否存在
                List<model.users> listUserMd = userBll.GetModel(ipara, "usersSelect");
                pbxdata.model.users userMd = null;
                if (listUserMd.Count > 0)
                {
                    userMd = listUserMd[0];
                }

                if (userMd != null)
                {
                    //设置cookie
                    pbxdata.bll.taoAppUserbll taoAppUserBll = new bll.taoAppUserbll();

                    #region
                    IDataParameter[] iparaTaoAppUser = new IDataParameter[] { 
                new SqlParameter("userId1",SqlDbType.Int,4),
                    };
                    iparaTaoAppUser[0].Value = userMd.Id;

                    #endregion

                    List<pbxdata.model.taoAppUser> taoShopList = taoAppUserBll.GetModelList(iparaTaoAppUser, "taoAppUsersSelect");
                    StringBuilder taoNames = new StringBuilder();
                    StringBuilder access_tokens = new StringBuilder();
                    foreach (pbxdata.model.taoAppUser t in taoShopList)
                    {
                        taoNames.Append(t.refreshToken + ",");
                        access_tokens.Append(t.accessToken + ",");
                    }

                    string TaoBaoNames = taoNames.ToString();
                    if (!string.IsNullOrEmpty(TaoBaoNames))
                    {
                        TaoBaoNames = TaoBaoNames.Substring(0, TaoBaoNames.Length - 1);
                    }
                    string AccessTokens = access_tokens.ToString();
                    if (!string.IsNullOrEmpty(AccessTokens))
                    {
                        AccessTokens = AccessTokens.Substring(0, AccessTokens.Length - 1);
                    }

                    HttpCookie cookie = new HttpCookie("userInfo");
                    cookie.Values["userName"] = userMd.userName;
                    cookie.Values["ID"] = userMd.Id.ToString();
                    cookie.Values["nick"] = HttpUtility.UrlEncode(TaoBaoNames);
                    cookie.Values["accessToken"] = AccessTokens;


                    string[] st = new string[] { userMd.userName, userMd.Id.ToString(), HttpUtility.UrlEncode(TaoBaoNames), AccessTokens, userMd.personaId.ToString() };
                    Session["UserMsg"] = st;
                    Response.Cookies.Add(cookie);

                    this.Response.Clear();//这里是关键，清除在返回前已经设置好的标头信息，这样后面的跳转才不会报错
                    this.Response.BufferOutput = true;//设置输出缓冲
                    if (!this.Response.IsRequestBeingRedirected)//在跳转之前做判断,防止重复
                    {
                        return RedirectToAction("../Home/Index");
                    }

                }

            }

            return View();

        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public string UserRegistered()
        {
            Dictionary<string, string> Dic = new Dictionary<string, string>();
            if (Request.QueryString["CheckCode"] != Session["Code"].ToString())
            {
                return "验证码错误!";
            }

            //helpcommon.PasswordHelp.encrypt(password)
            //密码：
            string password = helpcommon.PasswordHelp.encrypt(Request.QueryString["Password"]);
            password = helpcommon.PasswordHelp.encrypt(password);

            Dic.Add("UserName", Request.QueryString["UserName"]);
            Dic.Add("Password", password);
            Dic.Add("RealName", Request.QueryString["RealName"]);
            Dic.Add("Sex", Request.QueryString["Sex"]); //0为女生，1为男生
            Dic.Add("txtReTelphone", Request.QueryString["txtReTelphone"]);
            Dic.Add("txtReAddress", Request.QueryString["txtReAddress"]);
            Dic.Add("txtReEmail", Request.QueryString["txtReEmail"]);
            usersbll bll = new usersbll();

            return bll.UserRegistered(Dic);
        }


        /// <summary>
        /// 检测用户是否已存在
        /// </summary>
        /// <returns></returns>
        public string CheckUserName()
        {
            string UserName = Request.QueryString["UserName"];
            usersbll bll = new usersbll();
            return bll.CheckUserName(UserName);
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public string CheckCode()
        {
            string s = string.Empty;
            Random ran = new Random();

            s = Convert.ToInt32(ran.Next(0, 9)).ToString() + Convert.ToInt32(ran.Next(0, 9)).ToString() + Convert.ToInt32(ran.Next(0, 9)).ToString() + Convert.ToInt32(ran.Next(0, 9)).ToString();
            Session["Code"] = s;
            return s;
        }

        /// <summary>
        /// 检测验证码是否正确
        /// </summary>
        /// <returns></returns>
        public string Check()
        {
            string flag = "false";
            string a = Session["Code"].ToString();
            string code = Request.Form["code"];
            if (code == Session["Code"].ToString())
            {
                flag = "true";

            }
            return flag;
        }
        public ActionResult Signup()
        {
            return View();
        }
    }
}
