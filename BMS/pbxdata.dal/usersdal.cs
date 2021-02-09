/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       users
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       users
    * 创建时间：       2015/2/9 16:22:23
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using Maticsoft.DBUtility;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class usersdal : dataoperating, iusers
    {


        Errorlogdal errordal = new Errorlogdal();
        public List<model.users> GetModel(IDataParameter[] ipara, string procName)
        {
            List<model.users> list = new List<model.users>();
            DataTable dt = Select(ipara, procName);
            list = DataRowToModel(dt.Rows);
            return list;
        }


        //public List<model.users> getUsers(IDataParameter[] ipara, string procName)
        //{
        //    List<model.users> list = new List<model.users>();
        //    DataTable dt = Select(ipara, procName);
        //    list = DataRowToModel(dt.Rows);
        //    return list;
        //}


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<model.users> DataRowToModel(DataRowCollection rowCollection)
        {
            List<model.users> list = new List<model.users>();

            foreach (DataRow row in rowCollection)
            {
                model.users modelMd = new model.users();
                //foreach (var item in modelMd.GetType().GetProperties())
                //{
                //    string name = item.Name;   
                //}
                if (row != null)
                {
                    if (row["Id"] != null && row["Id"].ToString() != "")
                    {
                        modelMd.Id = int.Parse(row["Id"].ToString());
                    }
                    if (row["personaId"] != null && row["personaId"].ToString() != "")
                    {
                        modelMd.personaId = int.Parse(row["personaId"].ToString());
                    }
                    if (row["userName"] != null)
                    {
                        modelMd.userName = row["userName"].ToString();
                    }
                    if (row["userPwd"] != null)
                    {
                        modelMd.userPwd = row["userPwd"].ToString();
                    }
                    if (row["userRealName"] != null)
                    {
                        modelMd.userRealName = row["userRealName"].ToString();
                    }
                    if (row["userSex"] != null && row["userSex"].ToString() != "")
                    {
                        modelMd.userSex = int.Parse(row["userSex"].ToString());
                    }
                    if (row["UserPhone"] != null)
                    {
                        modelMd.UserPhone = row["UserPhone"].ToString();
                    }
                    if (row["UserAddress"] != null)
                    {
                        modelMd.UserAddress = row["UserAddress"].ToString();
                    }
                    if (row["UserEmail"] != null)
                    {
                        modelMd.UserEmail = row["UserEmail"].ToString();
                    }
                    if (row["userIndex"] != null && row["userIndex"].ToString() != "")
                    {
                        modelMd.userIndex = int.Parse(row["userIndex"].ToString());
                    }
                    if (row["UserManage"] != null && row["UserManage"].ToString() != "")
                    {
                        modelMd.UserManage = int.Parse(row["UserManage"].ToString());
                    }
                    if (row["UserId"] != null && row["UserId"].ToString() != "")
                    {
                        modelMd.UserId = int.Parse(row["UserId"].ToString());
                    }
                    if (row["Def1"] != null)
                    {
                        modelMd.Def1 = row["Def1"].ToString();
                    }
                    if (row["Def2"] != null)
                    {
                        modelMd.Def2 = row["Def2"].ToString();
                    }
                    if (row["Def3"] != null)
                    {
                        modelMd.Def3 = row["Def3"].ToString();
                    }
                    if (row["Def4"] != null)
                    {
                        modelMd.Def4 = row["Def4"].ToString();
                    }
                    if (row["Def5"] != null)
                    {
                        modelMd.Def5 = row["Def5"].ToString();
                    }
                }

                list.Add(modelMd);
            }

            return list;
        }


        /// <summary>
        /// 根据权限字段组成sql语句返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<model.users> getUserData(string sql)
        {
            List<model.users> list = new List<model.users>();
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];

            list = DataRowToModel(dt.Rows);
            return list;
        }


        /// <summary>
        /// 返回所有字段集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable getData()
        {
            List<model.users> list = new List<model.users>();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.users select c;
            DataTable dt = LinqToDataTable.LINQToDataTable<model.users>(p);

            return dt;
        }


        /// <summary>
        /// 根据权限字段组成sql语句返回集合(可带条件查询)
        /// </summary>
        /// <param name="dic">查询参赛</param>
        /// <returns></returns>
        public DataTable getData(Dictionary<string, string> dic = null)
        {
            List<model.users> list = new List<model.users>();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.users select c;

            foreach (var item in dic)
            {
                string ikey = item.Key;      //名称

                int ivalue = 0; //值
                int.TryParse(item.Value,out ivalue);  

                if (item.Key == "roleId") //角色
                {
                    p = p.Where(c => c.personaId ==  ivalue);
                }
            }

            DataTable dt = LinqToDataTable.LINQToDataTable<model.users>(p);

            return dt;
        }



        /// <summary>
        /// 编辑-根据ID获取信息
        /// </summary>
        /// <returns></returns>
        public DataTable getDataEdit(int id)
        {
            List<model.users> list = new List<model.users>();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.users where c.Id == id select c;
            DataTable dt = LinqToDataTable.LINQToDataTable<model.users>(p);

            return dt;
        }



        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public string del(int id)
        {
            string s = string.Empty;
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.users p = (from c in context.users where c.Id == id select c).FirstOrDefault();
                //context.users.DeleteOnSubmit(p);
                context.users.DeleteOnSubmit(p);
                context.SubmitChanges();
                s = "删除成功！";
            }
            catch (Exception ex)
            {
                s = "删除失败！";
            }
            return s;
        }
        /// <summary>
        /// 注册用户
        /// </summary>
        /// <returns></returns>
        public string UserRegistered(Dictionary<string, string> Dic)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string sql = "insert users(personaId,userName,userPwd,userRealName,userSex,UserPhone,UserAddress,UserEmail)"+
            "values('10','" + Dic["UserName"] + "','" + Dic["Password"] + "','" + Dic["RealName"] + "','" + Convert.ToInt32(Dic["Sex"]) + "','" + Dic["txtReTelphone"] + "','" + Dic["txtReAddress"] + "','" + Dic["txtReEmail"] + "')";
            try
            {
                var info = context.users.Where(a => a.userName == Dic["UserName"]);
                if (info.Count() > 0)
                {
                    return "该帐户已注册!";
                }
                else
                {
                    int d = DbHelperSQL.ExecuteSql(sql);
                    //model.users user = new model.users()
                    //{
                    //    personaId = 10,
                    //    userName = Dic["UserName"],
                    //    userPwd = Dic["Password"],
                    //    userRealName = Dic["RealName"],
                    //    userSex = Convert.ToInt32(Dic["Sex"]),
                    //    UserPhone = Dic["txtReTelphone"],
                    //    UserAddress = Dic["txtReAddress"],
                    //    UserEmail = Dic["txtReEmail"],
                    //};
                    //context.users.InsertOnSubmit(user);
                    //context.SubmitChanges();
                    //return "注册成功!";
                    if (d > 0)
                    {
                        return "注册成功！";
                    }
                    else 
                    {
                        return "注册失败！";
                    }
                }
            }
            catch
            {
                return "注册失败!";
            }


        }
        /// <summary>
        /// 检测用户
        /// </summary>
        /// <returns></returns>
        public string CheckUserName(string UserName)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var info = context.users.Where(a => a.userName == UserName);
                if (info.Count() > 0)
                {
                    return "该帐户已注册!";
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "注册失败!";
            }
        }
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <returns></returns>
        public string UpdateUserInfo(Dictionary<string, string> Dic)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = from c in context.users where c.userName == Dic["UserName"] && c.userPwd == Dic["oldPwd"] select c;
                if (q.FirstOrDefault() != null)
                {
                    foreach (var item in q)
                    {
                        item.userRealName = Dic["RealName"];
                        item.userSex = Convert.ToInt32(Dic["sex"]);
                        item.userPwd = Dic["Pwd"];
                        item.UserPhone = Dic["Phone"];
                        item.UserEmail = Dic["Email"];
                        item.UserAddress = Dic["Address"];
                    }
                    context.SubmitChanges();
                    errordal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->usersdal->UpdateUserInfo()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "修改个人信息->" + Dic["UserName"],
                        UserId = Convert.ToInt32(Dic["UserId"].ToString())
                    });
                    return "修改成功!";
                }
                else
                {
                    return "密码错误!";
                }

            }
            catch (Exception ex)
            {
                errordal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->usersdal->UpdateUserInfo()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = Convert.ToInt32(Dic["UserId"].ToString())
                });
                return "修改失败!";
            }

        }
        /// <summary>
        /// 分配店铺
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string InsertShopAllocation(string shopid, string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("UserId",userid),
                new SqlParameter("shopId",shopid)
            };
            return Add(ipr, "InsertShopAllocation");
        }
        /// <summary>
        /// 当前用户是否已经分配当前店铺
        /// </summary>
        /// <param name="shopid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool ShopIsAllocation(string shopid, string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userId",userid),
                new SqlParameter("shopId",shopid)
            };
            DataTable dt = Select(ipr, "ShopIsAllocation");
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 得到当前用户拥有的店铺的数组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string[] GetShopAllocation(string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userid",userid),
            };
            DataTable dt = Select(ipr, "GetShopAllocation");
            string[] s = new string[5000];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s[i] = dt.Rows[i]["ShopId"].ToString();
            }
            return s;
        }
        /// <summary>
        /// 获取客户端用户信息
        /// </summary>
        public DataTable GetClientUsersTable(string UserName, string Vencode, int page, int Selpages, out string counts)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.ClientLogin
                    join t in context.productsource
                    on Convert.ToString(c.sourceId) equals t.SourceCode
                    into temp
                    from tt in temp.DefaultIfEmpty()
                    select new
                    {
                        Id=c.Id,
                        UserName=c.userName,
                        PassWord=c.userPwd,
                        sourceName = tt.sourceName,
                        Vencode=c.sourceId,
                        Def1=c.Def1 //邮箱

                    };
            if (UserName != "")
            {
                q = q.Where(a => a.UserName.Contains(UserName));
            };
            if (Vencode != "")
            {
                q = q.Where(a => a.Vencode == Convert.ToInt32(Vencode)).OrderByDescending(a=>a.Id);
            };
            counts = q.ToList().Count.ToString();
            q = q.Skip((Convert.ToInt32(page) - 1) * Convert.ToInt32(Selpages)).Take(Convert.ToInt32(Selpages));
            return LinqToDataTable.LINQToDataTable(q);
        }
        /// <summary>
        /// 修改客户端用户信息
        /// </summary>
        public string UpdateClientUsers(string UserName, string PassWord, string Vencode, string Email)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.ClientLogin.Where(a => a.userName == UserName);
                foreach (var i in q)
                {
                    i.userPwd = PassWord;
                    i.sourceId = Convert.ToInt32(Vencode);
                    i.Def1 = Email;
                }
                context.SubmitChanges();
                return "修改成功!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 添加客户端用户信息
        /// </summary>
        public string AddClientUsers(string UserName, string PassWord, string Vencode, string Email)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.ClientLogin.Where(a => a.userName == UserName);
                if (q.ToList().Count > 0)
                {
                    return "该用户已存在!";
                }
                model.ClientLogin CL = new model.ClientLogin()
                {
                    userName=UserName,
                    userPwd=PassWord,
                    sourceId=Convert.ToInt32(Vencode),
                    Def1=Email,
                };
                context.ClientLogin.InsertOnSubmit(CL);
                context.SubmitChanges();
                return "添加成功!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 删除客户端用户
        /// </summary>
        public string DeleteClientUsers(string UserName)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.ClientLogin.Where(a => a.userName == UserName);
                foreach (var i in q)
                {
                    context.ClientLogin.DeleteOnSubmit(i); 
                }
                context.SubmitChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           
        }
      
    }
}
