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
    public partial class BrandDAL : dataoperating, iBrandDAL
    {
        /// <summary>
        /// 查询品牌表所有信息，进行数据绑定
        /// </summary>
        /// <returns></returns>
        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }
        Errorlogdal dal = new Errorlogdal();
        /// <summary>
        /// 查出所有的品牌信息  用于绑定下拉列表
        /// </summary>
        /// <returns></returns>
        public List<BrandModel> SelectBrand()
        {
            pbxdatasourceDataContext pdc = new pbxdatasourceDataContext(connectionString);
            List<BrandModel> list = new List<BrandModel>();
            var info = pdc.brand;
            BrandModel bml = new BrandModel();
            bml.BrandAbridge = "";
            bml.BrandName = "请选择";
            list.Add(bml);
            foreach (var temp in info)
            {
                BrandModel bm = new BrandModel();
                bm.BrandAbridge = temp.BrandAbridge;
                bm.BrandName = temp.BrandName;
                list.Add(bm);
            }
            return list;
        }
        /// <summary>
        /// 查询所有品牌
        /// </summary>
        /// <returns></returns>
        public List<BrandModel> SelectAllBrand()
        {
            pbxdatasourceDataContext pdc = new pbxdatasourceDataContext(connectionString);
            List<BrandModel> list = new List<BrandModel>();
            var info = pdc.brand;
            foreach (var temp in info)
            {
                BrandModel bm = new BrandModel();
                bm.BrandAbridge = temp.BrandAbridge;
                bm.BrandName = temp.BrandName;
                list.Add(bm);
            }
            return list;
        }
        /// <summary>
        /// 通过首字母查询
        /// </summary>
        /// <returns></returns>
        public DataTable SelectBrandByChar(string fristchar)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("char",fristchar)
            };
            return Select(ipr, "GetBarndByCharFrist");
        }
        /// <summary>
        /// 首字母不为A-Z的品牌
        /// </summary>
        /// <returns></returns>
        public DataTable GetBrandEles()
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql","")
            };
            return Select(ipr, "GetBrandElse");
        }
        /// <summary>
        /// 查询当前角色是否已有权限
        /// </summary>
        /// <param name="PersonaId"></param>
        /// <returns></returns>
        public bool BrandConfigIsExist(string PersonaId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("PersonalId",PersonaId)
            };
            DataTable dt = Select(ipr, "BrandConfigIsExist");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 如果当前角色已有权限 则修改权限
        /// </summary>
        /// <param name="personaId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool UpdateBrandConfig(string personaId, string BrandId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("BrandId",BrandId),
                new SqlParameter("PersonaId",personaId)
            };
            string result = Update(ipr, "UpdateBrandConfig");
            if (result == "修改成功")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 如果当前角色没有拥有权限则添加权限
        /// </summary>
        /// <param name="personaId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool InsertBrandConfig(string personaId, string BrandId,string brandname,string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("BrandId",BrandId),
                new SqlParameter("PersonaId",personaId),
                new SqlParameter("BrandName",brandname),
                new SqlParameter("Vencode",vencode)
            };
            string result = Add(ipr, "InsertBrandConfig");
            if (result == "添加成功")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 通过角色信息查查找角色权限
        /// </summary>
        /// <param name="PersionId"></param>
        /// <returns></returns>
        public DataTable GetQxByPersionId(string PersionId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("PersonalId",PersionId)
            };
            return Select(ipr, "BrandConfigIsExist");
        }
        /// <summary>
        /// 查询当前用户是否已有配置
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public bool UserPerssionIsIn(string CustomerId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("CustomerId",@CustomerId)
            };
            DataTable dt = Select(ipr, "UserPerssionIsIn");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 如果已有配置 则进行修改
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool UpdateUserPerssion(string CustomerId, string BrandId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("CustomerId",CustomerId),
                new SqlParameter("BrandId",BrandId)
            };
            if (Update(ipr, "UpdateUserPerssion") == "修改成功")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 如果没有配置 则添加配置
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="BrandId"></param>
        /// <returns></returns>
        public bool InsertUserPerssion(string CustomerId, string BrandId,string vencode,string brandName)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("CustomerId",CustomerId),
                new SqlParameter("BrandId",BrandId),
                new SqlParameter("Vencode",vencode),
                new SqlParameter("BrandName",brandName)
            };
            if (Add(ipr, "InsertUserPerssion") == "添加成功")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据用户ID查找用户拥有的权限
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public DataTable UesrPerssionByUserId(string CustomerId)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("CustomerId",@CustomerId)
            };
            DataTable dt = Select(ipr, "UserPerssionIsIn");
            return dt;
        }
        /***********品牌权限 新增************/
        /// <summary>
        /// ---------清除当前角色的品牌权限
        /// </summary>
        /// <param name="uerid"></param>
        public void ClearBrandConfigByPersonaId(string userid,string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("PersonaId",userid),
                new SqlParameter("Vencode",vencode)
            };
            try
            {
                Delete(ipr, "ClearBrandConfigByPersonaId");
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                throw;
            }

        }
        /// <summary>
        /// ---------获得 当前角色的权限
        /// </summary>
        /// <returns></returns>
        public string[] GetBrandConfigByPersonaId(string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("personal",userid)
            };
            DataTable dt = Select(ipr, "GetBrandConfigByPersonaId");
            int cj = dt.Rows.Count;
            string[] str = new string[cj];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["BrandAbridge"].ToString();
                }
                return str;
            }
            else
            {
                return str;
            }
        }
        /// <summary>
        /// -------获得当前用户的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string[] GetBrandConfigPersionByUserId(string userid)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userid",userid),
            };
            DataTable dt = Select(ipr, "GetBrandConfigPersionByUserId");
            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["BrandAbridge"].ToString();
                }
                return str;
            }
            else
            {
                return str;
            }


        }
        /// <summary>
        /// ------清除当前用户的权限
        /// </summary>
        /// <param name="userid"></param>
        public void ClearBrandConfigPersionByUserId(string userid,string vencode)
        {
            IDataParameter[] ipr = new IDataParameter[] 
            {
                new SqlParameter("sql",""),
                new SqlParameter("userid",userid),
                new SqlParameter("Vencode",vencode)
            };
            Delete(ipr, "ClearBrandConfigPersionByUserId");
        }
        /// <summary>
        /// 获取供应商品牌表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchBrandVen(Dictionary<string, string> Dic, int page, int pages, out string counts)
        {
            List<model.product> list = new List<model.product>();
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = from c in context.ItalyPorductStock
                    group c by new { c.Cat, c.Vencode } into cc
                    join a in context.BrandVen on new{Cat=cc.Key.Cat,Vencode=cc.Key.Vencode} equals new{Cat=a.BrandNameVen,Vencode=a.Vencode}
                    into aa
                    from aaa in aa.DefaultIfEmpty()
                    join d in context.productsource on cc.Key.Vencode equals d.SourceCode
                    into dd
                    from ddd in dd.DefaultIfEmpty()
                    select new
                    {
                        Cat = cc.Key.Cat,//--供应商类别名
                        Vencode2 = cc.Key.Vencode,//--供应商编码
                        Id = aaa.Id == null ? 0 : aaa.Id,
                        BrandName = aaa.BrandName,//--名牌名称
                        BrandAbridge = aaa.BrandAbridge,//--名牌缩写
                        sourceName = ddd.sourceName,//--供应商名称
                    };
            if (Dic["BrandName"] != "")
            {
                q = q.Where(a => a.BrandName.Contains(Dic["BrandName"]));
            }
            if (Dic["BrandNameVen"] != "")
            {
                q = q.Where(a => a.Cat.Contains(Dic["BrandNameVen"]));
            }
            if (Dic["Vencode"] != "")
            {
                q = q.Where(a => a.Vencode2 == Dic["Vencode"]);
            }
            if (Dic["bangd"] == "0")
            {
                q = q.Where(a => a.BrandName != "");
            }
            else if (Dic["bangd"] == "1")
            {
                q = q.Where(a => a.BrandName == null);
            }
            counts = q.Count().ToString();
            dt = LinqToDataTable.LINQToDataTable(q.Skip(pages * (page - 1)).Take(pages));
            //string MinNid = (pages * (page - 1)).ToString();
            //string MaxNid = (pages * page).ToString();
            //IDataParameter[] ipr = new IDataParameter[]
            //{
            //    new SqlParameter("BrandName",Dic["BrandName"]),
            //    new SqlParameter("BrandNameVen",Dic["BrandNameVen"]),
            //    new SqlParameter("Vencode",Dic["Vencode"]),
            //    new SqlParameter("bangd",Dic["bangd"]),
            //    new SqlParameter("MinNid",MinNid),
            //    new SqlParameter("MaxNid",MaxNid),
            //    new SqlParameter("sql",""),
            //};
            //IDataParameter[] iprc = new IDataParameter[]
            //{
            //    new SqlParameter("BrandName",Dic["BrandName"]),
            //    new SqlParameter("BrandNameVen",Dic["BrandNameVen"]),
            //    new SqlParameter("Vencode",Dic["Vencode"]),
            //    new SqlParameter("bangd",Dic["bangd"]),
            //    new SqlParameter("sql",""),
            //};
            //dt = Select(ipr, "SelectBrandVen");
            //counts = Select(iprc, "SelectBrandVenCount").Rows[0][0].ToString();

            return dt;
        }
        /// <summary>
        /// 更新供应商品牌表
        /// </summary>
        /// <returns></returns>
        public string UpdateBrandVen(Dictionary<string, string> dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                if (dic["Id"] == "0")//--判断是否存在 存在则更新不存在插入
                {
                    var q = context.BrandVen.Where(a => a.BrandNameVen == dic["BrandNameVen"]).Where(a => a.Vencode == dic["Vencode"]).ToList();
                    if (q.Count == 0)
                    {
                        model.BrandVen BV = new model.BrandVen()
                   {
                       BrandName = dic["BrandName"],
                       BrandAbridge = dic["BrandAbridge"],
                       BrandNameVen = dic["BrandNameVen"],
                       Vencode = dic["Vencode"],
                   };
                        context.BrandVen.InsertOnSubmit(BV);
                        context.SubmitChanges();
                        dal.InsertErrorlog(new model.errorlog()
                        {
                            errorSrc = "pbxdata.dal->BrandDal->UpdateProducttypeVen()",
                            ErrorMsg = "修改",
                            errorTime = DateTime.Now,
                            operation = 2,
                            errorMsgDetails = "添加供应商品牌表->" + dic["BrandNameVen"],
                            UserId = UserId,
                        });
                    }
                    else
                    {
                        return "该品牌已存在!";
                    }

                }
                else
                {
                    var q = from c in context.BrandVen where c.Id == Convert.ToInt32(dic["Id"].ToString()) select c;
                    foreach (var i in q)
                    {
                        i.BrandName = dic["BrandName"];
                        i.BrandAbridge = dic["BrandAbridge"];
                        i.BrandNameVen = dic["BrandNameVen"];
                        i.Vencode = dic["Vencode"];
                    }
                    context.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->BrandDal->UpdateProducttypeVen()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "修改供应商品牌表->" + dic["BrandNameVen"],
                        UserId = UserId,
                    });
                }
                return "修改成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->UpdateProducttypeVen()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "修改失败!";
            }

        }
        /// <summary>
        /// 删除供应商类别
        /// </summary>
        public string deleteBrandVen(string Id, int UserId)
        {
            try
            {
                string item = "";
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.BrandVen.Where(a => a.Id == Convert.ToInt32(Id));//通过id删除
                foreach (var i in q)
                {
                    item = i.BrandNameVen;
                    context.BrandVen.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//--操作日志
                {
                    errorSrc = "pbxdata.dal->BrandDal->deleteBrandVen()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "修改供应商品牌表->" + item,
                    UserId = UserId,
                });
                return "删除成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->deleteBrandVen()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "删除失败!";
            }
        }
        /// <summary>
        /// 更新品牌对应的淘宝编号
        /// </summary>
        public string UpdateBrand(string BrandName, string BrandAbridge, string Vid,string Def2, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.brand.Where(a => a.BrandAbridge == BrandAbridge);//--根据品牌缩写Update
                foreach (var i in q)
                {
                    i.BrandName = BrandName;
                    i.Def1 = Vid;
                    i.Def2 = Def2;
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//--操作日志
                {
                    errorSrc = "pbxdata.dal->BrandDal->UpdateBrand()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "更新品牌对应的淘宝编号->" + BrandName,
                    UserId = UserId,
                });
                return "更新成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->UpdateBrand()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "更新失败!";
            }

        }
        /// <summary>
        /// 添加品牌
        /// </summary>
        public string AddBrand(string BrandName, string BrandAbridge,string Def2, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var check = context.brand.Where(a => a.BrandAbridge == BrandAbridge).Where(a => a.BrandName == BrandName).ToList();
                if (check.Count > 0)//--根据品牌缩写判断是否存在
                {
                    return "品牌名或缩写重复!";
                }
                else
                {
                    model.brand brand = new model.brand()
                    {
                        BrandName = BrandName,
                        BrandAbridge = BrandAbridge,
                        Def2=Def2,
                    };
                    context.brand.InsertOnSubmit(brand);
                    context.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->BrandDal->AddBrand()",
                        ErrorMsg = "添加",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "添加品牌->" + BrandName,
                        UserId = UserId,
                    });
                    return "添加成功!";
                }
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->AddBrand()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "修改失败!";
            }
        }
        /// <summary>
        /// 删除品牌
        /// </summary>
        public string DeleteBrand(string BrandName, string BrandAbridge, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.brand.Where(a => a.BrandName == BrandName & a.BrandAbridge == BrandAbridge);
                foreach (var i in q)//--根据品牌名和品牌缩写删除
                {
                    context.brand.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->DeleteBrand()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "删除品牌->" + BrandName,
                    UserId = UserId,
                });
                return "";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->DeleteBrand()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "删除失败!";
            }
            
        }
        /// <summary>
        /// 获取淘宝品牌下拉表
        /// </summary>
        public DataTable GetTBbrandlist(string TBBrandName)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = from c in context.TBBrand select c;
            if (TBBrandName != "")
            {
                q = q.Where(a => a.TBBrandName.Contains(TBBrandName));
            }
            return LinqToDataTable.LINQToDataTable(q);
        }
        /// <summary>
        /// 更新淘宝品牌
        /// </summary>
        public string UpdateTBBrand(string TBBrandName, string vid, string Id, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.TBBrand.Where(a => a.Id == Convert.ToInt32(Id));
                foreach (var i in q)
                {
                    i.TBBrandName = TBBrandName;//淘宝品牌名
                    i.vid = vid;//淘宝编码
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->DeleteBrand()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "更新淘宝品牌->" + TBBrandName,
                    UserId = UserId,
                });
                return "修改成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->UpdateTBBrand()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "修改失败!";
            }
        }
        /// <summary>
        /// 添加淘宝品牌
        /// </summary>
        public string AddTBBrand(string TBBrandName, string vid, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var check = context.TBBrand.Where(a => a.TBBrandName == TBBrandName && a.vid == vid).ToList();
                if (check.Count > 0)
                {
                    return "该品牌已存在!";
                }
                else
                {
                    model.TBBrand tb = new model.TBBrand()
                    {
                        TBBrandName = TBBrandName,//淘宝品牌名
                        vid = vid,//淘宝编码
                    };
                    context.TBBrand.InsertOnSubmit(tb);
                    context.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->BrandDal->AddTBBrand()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "添加淘宝品牌->" + TBBrandName,
                        UserId = UserId,
                    });
                    return "添加成功!";
                }
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->BrandDal->AddTBBrand()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "添加失败!";
            }
        }
        /// <summary>
        /// 供应商类别搜索
        /// </summary>
        public DataTable SearchBrandDDlist(string BrandName)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = context.brand.Where(a => a.BrandName.Contains(BrandName) || a.BrandAbridge.Contains(BrandName));
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }
        //******************7.6
        /// <summary>
        /// 根据excel内容添加对应的品牌
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string InsertBrand(string [] str) 
        {
            pbxdatasourceDataContext pdc = new pbxdatasourceDataContext(connectionString);

            List<BrandVen> info = pdc.BrandVen.Where(a => a.Vencode == str[3] && a.BrandNameVen == str[2]).ToList();//查询当前添加的品牌是否已经存在
            if (info.Count > 0)
            {
                return "当前数据源已存在此品牌！";
            }
            else 
            {
                try
                {
                    IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("BrandName",str[0]),
                    new SqlParameter("BrandAbridge",str[1]),
                    new SqlParameter("BrandNameVen",str[2]),
                    new SqlParameter("Vencode",str[3]),
                };
                    return Add(ipr, "InsertBrandVencode");
                }
                catch (Exception)
                {
                    return "添加失败！";
                    throw;
                }
            }
            
        }
        /// <summary>
        /// 得到所有excel的对应品牌数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetBrandExcel(string[] str, int minid, int maxid) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("BrandName",str[0]),
                    new SqlParameter("BrandVenName",str[1]),
                    new SqlParameter("minid",minid),
                    new SqlParameter("maxid",maxid)
                };
                DataTable dt = Select(ipr, "GetBrandExcel");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到所有excel的对应品牌数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetBrandExcelCount(string[] str)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("BrandName",str[0]),
                    new SqlParameter("BrandVenName",str[1]),
                };
                DataTable dt = Select(ipr, "GetBrandExcelCount");
                int count = 0;
                if (dt.Rows.Count > 0) 
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                return count;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        /// <summary>
        /// ---删除EXCEL对应关系的品牌
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string DeleteBrandExcel(string Id) 
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("Id",Id)
                };
                return Delete(ipr, "DeleteBrandExcel");
            }
            catch (Exception)
            {
                return "删除失败！";
                throw;
            }
        }
        /***********品牌权限   新修*********/
        /// <summary>
        /// 跟据品牌缩写和数据源得到当前数据源全称
        /// </summary>
        /// <param name="BrandSx"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string GetBrandNameVencode(string BrandSx, string vencode) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<BrandVen> list = pddc.BrandVen.Where(a => a.Vencode == vencode && a.BrandAbridge == BrandSx).ToList();
                return list[0].BrandNameVen.ToString().Replace("'","''");
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }
        /// <summary>
        /// 得到角色当前数据源的权限品牌
        /// </summary>
        /// <param name="Rrole"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetUserBrandName(string Role, string vencode,string ZfChar) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<BrandConfig> list = pddc.BrandConfig.Where(a => a.Def1 == vencode && a.PersonaId == Role && a.BrandName.StartsWith(ZfChar)).ToList();
                string[] str = new string[list.Count];
                for (int i = 0; i < list.Count; i++) 
                {
                    str[i] = list[i].BrandId;
                }
                return str;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到角色当前数据源的其他（首字母在A-Z之外）品牌
        /// </summary>
        /// <param name="Role"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetUserBrandName(string Role, string vencode) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<BrandConfig> list = pddc.BrandConfig.Where(a => a.Def1 == vencode && a.PersonaId == Role).ToList();
                string[] strall = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                for (int i = 0; i < strall.Length; i++)
                {
                    list = list.Where(a => a.BrandName.StartsWith(strall[i]) == false).ToList();//排除首字母A-Z
                }
                string[] str = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    str[i] = list[i].BrandId;
                }

                return str;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到角色权限
        /// </summary>
        /// <param name="role">角色Id</param>
        /// <param name="vencode">数据源编号</param>
        /// <param name="ZfChar">A-Z首字母</param>
        /// <returns></returns>
        public List<BrandConfig> GetAllUserBrand(string role, string vencode, string ZfChar)
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<BrandConfig> list = pddc.BrandConfig.Where(a => a.Def1 == vencode && a.PersonaId == role && a.BrandName.StartsWith(ZfChar)).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到角色权限  所有品牌
        /// </summary>
        /// <param name="role"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<BrandConfig> GetAllUserBrand(string role, string vencode) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<BrandConfig> list = pddc.BrandConfig.Where(a => a.Def1 == vencode && a.PersonaId == role).ToList();
                return list;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// 得到用户已配置权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string[] GetUserBrandSx(string userId,string vencode) 
        {
            pbxdatasourceDataContext pddc = new pbxdatasourceDataContext(connectionString);
            try
            {
                List<BrandConfigPersion> list = pddc.BrandConfigPersion.Where(a => a.Def1 == vencode && a.CustomerId == userId).ToList();
                string[] str = new string[list.Count];
                for (int i = 0; i < list.Count; i++) 
                {
                    str[i] = list[i].BrandId;
                }
                return str;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}
