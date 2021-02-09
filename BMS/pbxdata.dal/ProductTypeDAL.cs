using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using Maticsoft.DBUtility;
using pbxdata.idal;
using System.Data;
using System.Data.SqlClient;
namespace pbxdata.dal
{
    public class ProductTypeDAL : dataoperating,iProductTypeDAL
    {
        /// <summary>
        /// 通过配置获取连接字符串
        /// </summary>
        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }
        /// <summary>
        /// 类别信息
        /// </summary>
        /// <returns></returns>
        public List<ProductTypeModel> GetProductType() 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            List<ProductTypeModel> list = new List<ProductTypeModel>();
            var info = pddc.producttype;
            ProductTypeModel ptm1 = new ProductTypeModel();
            ptm1.TypeName = "请选择";
            ptm1.TypeNo = "";
            list.Add(ptm1);
            foreach (var temp in info) 
            {
                ProductTypeModel ptm = new ProductTypeModel();
                ptm.TypeName = temp.TypeName;
                ptm.TypeNo = temp.TypeNo;
                list.Add(ptm);
            }
            return list;
        }
        /// <summary>
        /// 将类别替换
        /// </summary>
        /// <returns></returns>
        public List<ProductTypeModel> GetProductTypeReplace()
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            List<ProductTypeModel> list = new List<ProductTypeModel>();
            var info = pddc.producttype;
            foreach (var temp in info)
            {
                ProductTypeModel ptm = new ProductTypeModel();
                ptm.TypeName = temp.TypeName;
                ptm.TypeNo = temp.TypeNo;
                list.Add(ptm);
            }
            return list;
        }
        /// <summary>
        /// 得到所有的大类型   -----------fj
        /// </summary>
        /// <returns></returns>
        public DataTable GetBigType() 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql","")
            };
            return Select(ipr, "GetBigType");
        }
        /// <summary>
        /// 通过大类型Id获得小类型   -----------fj
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeByBigId(int BigId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("BigId",BigId)
            };
            return Select(ipr, "GetTypeByBigId");
        }

        /// <summary>
        /// 获得所有的角色
        /// </summary>
        /// <returns></returns>
        public DataTable GetPersona() 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
            };
            return Select(ipr, "GetPersona");
        }
        /// <summary>
        /// 通过用户名查找当前用户所属角色
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public DataTable GetPersonaIdByUserName(string UserName)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("UserName",UserName)
            };
            return Select(ipr, "GetPersonaIdByUserName");
        }
        /// <summary>
        /// 如果当前角色并无权限则添加
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="PersionId"></param>
        /// <returns></returns>
        public bool InsertPersion(string typeId, string PersionId,string BigType,string vencode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("PersionId",PersionId),
                new SqlParameter("TypeId",typeId),
                new SqlParameter("BigTypeId",BigType),
                new SqlParameter("Vencode",vencode)
            };
            if (Add(ipr, "InsertPersion") == "添加成功")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        
        /// <summary>
        /// 通过当前角色编号获得权限
        /// </summary>
        /// <param name="PersionId"></param>
        /// <returns></returns>
        public DataTable GetQxByPersonaId(string PersionId) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("personaId",PersionId)
            };
            return Select(ipr, "GetQxByPersonaId");
        }
        
        /// <summary>
        /// 如果还没有配置 则添加配置
        /// </summary>
        /// <param name="personalId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public bool InsertPersonaTypeConfit(string personalId, string typeId,string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("personalId",personalId),
                new SqlParameter("TypeId",typeId),
                new SqlParameter("Vencode",vencode)
            };
            if (Add(ipr, "InsertPersonaTypeConfit") == "添加成功")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        
        /// <summary>
        /// 通过用户名查找用户的编号
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public int SelectUserIdByUserName(string userName) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userName",userName)
            };
            DataTable dt = Select(ipr, "SelectUserIdByUserName");
            int UserId = int.Parse(dt.Rows[0][0].ToString());
            return UserId;
        }
        /// <summary>
        /// 通过用户Id 查询出当前用户想看的权限
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable SelectConfigByUserId(string userName) {

            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userId",userName)
            };
            return Select(ipr, "SelectConfigByUserId");
        }
        /// <summary>
        /// 通过BigId找出当前的所有信息
        /// </summary>
        /// <param name="bigid"></param>
        /// <returns></returns>
        public DataTable GetBigIdAllByBigId(string bigid) 
        {
            IDataParameter[] irp = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("bigId",bigid)
            };
            DataTable dt = Select(irp, "GetBigAllByBigId");
            return dt;
        }
        
        //通过小类型编号  获得小类型信息
        public DataTable GetTypeIdByTypeNo(string typeno) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("typeno",typeno)
            };
            return Select(ipr, "GetTypeIdByTypeNo");
        }
        /// <summary>
        /// //----------通过角色Id查找角色名称
        /// </summary>
        /// <returns></returns>
        public string GetPersionNameByid(string persionId) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("persionId",persionId)
            };
            DataTable dt = Select(ipr, "GetPersionNameByid");
            return dt.Rows[0][0].ToString();
        }

        //***********类别配置 新增************//
        public bool ClearTypeConfigByUserId(string persionid,string table,string  vencode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("PersionId",persionid),
                new SqlParameter("table",table),
                new SqlParameter("Vencode",vencode)
            };
            if (Delete(ipr, "ClearTypeConfigByUserId") == "删除成功")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        /// <summary>
        /// 通过类别名称找到大类别编号
        /// </summary>
        /// <param name="TypeNo"></param>
        /// <returns></returns>
        public string GetBigByTypeNo(string TypeNo) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("typeNo",TypeNo)
            };
            DataTable dt = Select(ipr, "GetBigByTypeNo");
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else 
            {
                return "";
            }
        }
        /// <summary>
        /// 当前角色的所有大类别权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string[] GetPerssionBigId(string userid) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userid",userid)
            };
            DataTable dt = Select(ipr, "GetPerssionBigId");
            string[] str = new string[5000];
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                str[i] = dt.Rows[i][0].ToString();
            }
            return str;
        }
        //获得当前角色的小类别权限
        public DataTable GetPerssionTypeNo(string userid,string bigid) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userid",userid),
                new SqlParameter("bigid",bigid)
            };
            DataTable dt = Select(ipr, "GetPerssionTypeNo");
            return dt;
        }
        /// <summary>
        /// -------当前用户是否已经给自己配置权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable ThisUserIsPersion(string userid) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("coustomer",userid)
            };
            DataTable dt = Select(ipr, "ThisUserIsPersion");
            return dt;
        }
        /// <summary>
        /// 通过大权限ID 获得小权限Id
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="bigid"></param>
        /// <returns></returns>
        public DataTable GetQxTypeIdByQxBigId(string userid, string bigid) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("BigId",bigid),
                new SqlParameter("PersionId",userid)
            };
            try
            {
                return Select(ipr, "GetQxTypeIdByQxBigId");
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /////---------获得当前用户的小类别权限
        /// </summary>
        public string[] GetThisUserTypeId(string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("PersionId",userid)
            };
            DataTable dt = Select(ipr, "GetThisUserTypeId");
            string [] str=new string [50000];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["TypeId"].ToString();
                }
                return str;
            }
            else 
            {
                str[0] = "";
                return str;
            }
            
        }
        /// <summary>
        /// 清除用户的个人配置
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool ClearUserTypeByUserId(string userid,string vencode) 
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userid",userid),
                new SqlParameter("Vencode",vencode)
            };
            if (Delete(ipr, "ClearUserTypeByUserId") == "删除成功")
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        //******************类别 新增    2015.8.24******************///
        /// <summary>
        /// 得到当前角色在某个供应商的类别权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="vencode">供应商编号</param>
        /// <param name="BigType">out 返回大类别数据</param>
        /// <returns>返回小类别数组</returns>
        public string[] GetTypePerssoin(string roleId, string vencode,out string [] BigType) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            List<model.TypeConfig> list = pddc.TypeConfig.Where(a => a.PersonaId == roleId && a.Def1 == vencode).ToList();
            string[] Type = new string[list.Count];
            BigType=new string [list.Count];
            for (int i = 0; i < list.Count; i++) 
            {
                Type[i] = list[i].TypeId;
                BigType[i] = list[i].BigTypeId;
            }
            return Type;
        }
        /// <summary>
        /// 得到用户自己配置的类别权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetTypeUser(string userId, string vencode) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            List<model.PersonaTypeConfit> list = pddc.PersonaTypeConfit.Where(a => a.CustomerId == userId && a.Def1 == vencode).ToList();
            string[] Type = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                Type[i] = list[i].TypeId;
            }
            return Type;
        }

        /// <summary>
        /// 得到角色类别
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string[] GetBrandRole(string vencode,string roleId) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            List<BrandConfig> list = pddc.BrandConfig.Where(a => a.Def1 == vencode && a.PersonaId == roleId).ToList();
            string [] str=new string [list.Count];
            for (int i = 0; i < list.Count; i++) 
            {
                str[i] = list[i].BrandId.ToString();
            }
            return str;
        }
        /// <summary>
        /// 得到以某个字母开头的权限品牌个数
        /// </summary>
        /// <param name="BeginChar"></param>
        /// <returns></returns>
        public int GetBrandBeginCount(string BeginChar,string vencode,string roleId) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            List<BrandConfig> list = pddc.BrandConfig.Where(a => a.Def1 == vencode && a.PersonaId == roleId).ToList();
            return list.Count;
        }
    }
}
