using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using pbxdata.idal;
using System.Data.SqlClient;
namespace pbxdata.dal
{
    public class AttributeDal : dataoperating
    {
        /// <summary>
        /// 取得相应属性--商品类别
        /// </summary>
        /// <returns></returns>


        public string connctionstring
        {
            get { return PubConstant.ConnectionString; }
        }
        Errorlogdal dal = new Errorlogdal();
        public List<model.AttributeModel> GetAttributeList()
        {
            List<model.AttributeModel> list = new List<model.AttributeModel>();
            DataTable dt = SelectAttribute();
            list = DataTableToList(dt.Rows);
            return list;
        }
        /// <summary>
        /// 取得相应属性--根据商品类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetAttributeList(string TypeNo)
        {
            List<model.AttributeModel> list = new List<model.AttributeModel>();
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.productPorperty
                    join a in context.producttype on c.TypeId equals a.Id
                    into aa
                    from aaa in aa.DefaultIfEmpty()
                    join b in context.TBProducttype on aaa.Def1 equals b.cid
                    into bb
                    from bbb in bb.DefaultIfEmpty()
                    select new
                    {
                        Id = c.Id,
                        TypeNo = aaa.TypeNo,
                        PropertyName = c.PropertyName,
                        PorpertyIndex = c.PorpertyIndex,
                        Def1 = c.Def1,
                        cid = bbb.cid,
                    };
            q = q.Where(a => a.TypeNo == TypeNo);
            dt = LinqToDataTable.LINQToDataTable(q.OrderBy(a => a.Id));
            //IDataParameter[] ipr = new IDataParameter[]{
            //new SqlParameter("TypeNo",TypeNo)
            //};
            // dt = Select(ipr, "GetproductPorpertyDT");
            //list = DataTableToListAttr(dt.Rows);
            return dt;
        }
        /// <summary>
        /// 获取所有类别--商品类别
        /// </summary>
        /// <returns></returns>
        private DataTable SelectAttribute()
        {
            //Add();
            DataSet ds = DbHelperSQL.Query("select *,b.bigtypeName as BigTypeName from producttype as a left join productbigtype as b on a.BigId=b.Id ");
            return ds.Tables[0];
        }
        /// <summary>
        /// 搜索获取类别--商品类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetBigTypeList(string Name, int Page, int Pagecount, out string counts)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.productbigtype select c;
            if (Name != "")//通过大类别名称查询
            {
                q = q.Where(a => a.bigtypeName == Name);
            }
            counts = q.Count().ToString();//计算数量
            q = q.OrderBy(a => a.bigtypeIndex);//通过index排序
            dt = LinqToDataTable.LINQToDataTable(q.Skip((Page - 1) * Pagecount).Take(Pagecount));//翻页

            return dt;
        }
        /// <summary>
        /// 编辑商品属性
        /// </summary>
        /// <returns></returns>
        public int UpdateAttribute(string Id, string PropertyName, int UserId)
        {
            pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);

            try
            {
                var info = pdc.productPorperty.Where(a => a.Id == Convert.ToInt32(Id));//通过id更新商品属性
                //var check = info.Where();

                foreach (var item in info)
                {
                    item.PropertyName = PropertyName;//商品属性名
                }
                pdc.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateAttribute()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "修改属性名称->" + Id + "->" + PropertyName,
                    UserId = UserId,
                });
                return 1;
            }
            catch (Exception ex)
            {

                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateAttribute()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return -1;
            }
        }
        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <returns></returns>
        public int InsertAttribute(string TypeNo, string PropertyName, string UserId)
        {
            try
            {
                int TypeId = -1;
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                //var test = from t in pdc.producttype where t.TypeNo == TypeNo  select t;
                var info = context.producttype.Where(a => a.TypeNo == TypeNo);//通过类编编号查询类别id

                foreach (var temp in info)
                {
                    TypeId = temp.Id;
                }
                var check = context.productPorperty.Where(a => a.TypeId == TypeId && a.PropertyName == PropertyName);//通过类别id以及属性名称查询来判断
                if (check.Count() == 0)//判断商品属性是否存在 不存在就insert
                {

                    if (TypeId != -1)
                    {
                        model.productPorperty pp = new model.productPorperty()
                        {
                            TypeId = TypeId,
                            PropertyName = PropertyName,
                            PorpertyIndex = 1,
                            UserId = Convert.ToInt32(UserId),
                        };
                        context.productPorperty.InsertOnSubmit(pp);
                        context.SubmitChanges();
                        //      IDataParameter[] ipr = new IDataParameter[]{
                        //  new SqlParameter("TypeId",TypeId),
                        //  new SqlParameter("PropertyName",PropertyName),
                        //  new SqlParameter("PorpertyIndex","1"),
                        //  new SqlParameter("UserId",UserId),
                        //};
                        //      string result = Add(ipr, "InsertproductPorperty");//Insert 商品属性
                        // if (result == "添加成功")
                        //{
                        dal.InsertErrorlog(new model.errorlog()//日志
                        {
                            errorSrc = "pbxdata.dal->AttributeDal->InsertAttribute()",
                            ErrorMsg = "添加",
                            errorTime = DateTime.Now,
                            operation = 1,
                            errorMsgDetails = "添加属性->" + TypeId + "->" + PropertyName,
                            UserId = Convert.ToInt32(UserId),
                        });
                        return 1;
                        //}
                        //else
                        //{
                        //    return -1;
                        //}
                    }
                }
                return -1;

            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->InsertAttribute()",
                    ErrorMsg = "添加",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = Convert.ToInt32(UserId),
                });
                return -1;
            }
        }
        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <returns></returns>
        public int DeleteAttribute(string Id, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.productPorperty.Where(a => a.Id == Convert.ToInt32(Id));
                foreach (var i in q)
                {
                    context.productPorperty.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//正常日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->DeleteAttribute()",
                    ErrorMsg = "删除->" + Id,
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "删除属性",
                    UserId = UserId,
                });
                return 1;
               
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->DeleteAttribute()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return -1;
            }
        }
        /// <summary>
        /// 修改类别对应税号表
        /// </summary>
        /// <returns></returns>
        public bool UpdateTypeIdToTariffNo(string TypeNo, string TariffNo, int UserId)
        {

            try
            {
                pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
                var info = pdc.TypeIdToTariffNo.Where(a => a.TypeNo == TypeNo);//查询该类别是否已判断税号编码
                int i = info.Count();
                if (info.Count() == 1)
                {
                    foreach (var temp in info)
                    {
                        temp.TariffNo = TariffNo;//税号编码
                    }
                    pdc.SubmitChanges();//更新类别对应税号编码
                    dal.InsertErrorlog(new model.errorlog()//日志
                    {
                        errorSrc = "pbxdata.dal->AttributeDal->UpdateTypeIdToTariffNo()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "修改税号->" + TypeNo,
                        UserId = UserId,
                    });

                }
                else
                {
                    TypeIdToTariffNo mm = new TypeIdToTariffNo()
                    {
                        TypeNo = TypeNo,
                        TariffNo = TariffNo,
                        UserId = Convert.ToInt32(UserId),
                    };
                    pdc.TypeIdToTariffNo.InsertOnSubmit(mm);//insert 类别对应税号编码
                    pdc.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()//日志
                    {
                        errorSrc = "pbxdata.dal->AttributeDal->UpdateTypeIdToTariffNo()",
                        ErrorMsg = "添加",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "给类别添加税号->" + TypeNo,
                        UserId = UserId,
                    });

                }
                return true;
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateTypeIdToTariffNo()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return false;
            }
        }
        /// <summary>
        /// 修改类别对应淘宝编码
        /// </summary>
        /// <returns></returns>
        public bool UploadTBType(string TypeNo, string cid, int UserId)
        {

            try
            {
                pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
                var info = pdc.producttype.Where(a => a.TypeNo == TypeNo);//通过类别编码查询
                int i = info.Count();
                if (info.Count() == 1)//判断是否存在
                {
                    foreach (var temp in info)
                    {
                        temp.Def1 = cid;//淘宝编码
                    }
                    pdc.SubmitChanges();
                    //dal.InsertErrorlog(new model.errorlog()
                    //{
                    //    errorSrc = "pbxdata.dal->AttributeDal->UpdateTypeIdToTariffNo()",
                    //    ErrorMsg = "修改",
                    //    errorTime = DateTime.Now,
                    //    operation = 2,
                    //    errorMsgDetails = "修改税号->" + TypeNo,
                    //    UserId = UserId,
                    //});

                }
                return true;
            }
            catch (Exception ex)
            {
                //dal.InsertErrorlog(new model.errorlog()
                //{
                //    errorSrc = "pbxdata.dal->AttributeDal->UpdateTypeIdToTariffNo()",
                //    ErrorMsg = "修改",
                //    errorTime = DateTime.Now,
                //    operation = 1,
                //    errorMsgDetails = ex.Message,
                //    UserId = UserId,
                //});
                return false;
            }
        }
        /// <summary>
        /// 将打datatable转换为list集合--商品类别
        /// </summary>
        /// <param name="dataRow">datatable rows</param>
        /// <returns></returns>
        private List<model.AttributeModel> DataTableToList(DataRowCollection dataRow)
        {
            List<model.AttributeModel> list = new List<model.AttributeModel>();
            foreach (DataRow row in dataRow)
            {
                model.AttributeModel pro = new model.AttributeModel();
                if (row["Id"] != null)
                {
                    pro.Id = int.Parse(row["Id"].ToString());
                }
                if (row["BigId"] != null)
                {
                    pro.BigId = int.Parse(row["BigId"].ToString());
                }
                if (row["BigTypeName"] != null)
                {
                    pro.BigTypeName = row["BigTypeName"].ToString();
                }
                if (row["TypeName"] != null)
                {
                    pro.TypeName = row["TypeName"].ToString();
                }
                if (row["TypeNo"] != null)
                {
                    pro.TypeNo = row["TypeNo"].ToString();
                }
                if (row["TypeIndex"] != null)
                {
                    pro.TypeIndex = int.Parse(row["TypeIndex"].ToString());
                }
                if (row["UserId"] != null)
                {
                    pro.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["Def1"] != null)
                {
                    pro.Def1 = row["Def1"].ToString();
                }
                if (row["Def2"] != null)
                {
                    pro.Def2 = row["Def2"].ToString();
                }
                if (row["Def3"] != null)
                {
                    pro.Def3 = row["Def3"].ToString();
                }
                if (row["Def4"] != null)
                {
                    pro.Def4 = row["Def4"].ToString();
                }
                if (row["Def5"] != null)
                {
                    pro.Def5 = row["Def5"].ToString();
                }

                list.Add(pro);
            }
            return list;
        }
        ///// <summary>
        ///// 获取所有类别--大类别
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetBigTypeList(int Page, int Pagecount)
        //{
        //    DataTable dt = new DataTable();

        //    model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
        //    //var q=
        //    IDataParameter[] ipr = new IDataParameter[]
        //    {
        //        new SqlParameter("Page",Page),
        //        new SqlParameter("Pagecount",Pagecount),
        //    };
        //    dt = Select(ipr, "ShowProductbigtype");//存储过程查询所有大类别列表
        //    return dt;
        //}
        /// <summary>
        /// 将打datatable转换为list集合--大类别
        /// </summary>
        /// <param name="dataRow">datatable rows</param>
        /// <returns></returns>
        private List<model.AttributeModel> DataTableToList1(DataRowCollection dataRow)
        {
            List<model.AttributeModel> list = new List<model.AttributeModel>();
            foreach (DataRow row in dataRow)
            {
                model.AttributeModel pro = new model.AttributeModel();
                if (row["Id"] != null)
                {
                    pro.Id = int.Parse(row["Id"].ToString());
                }
                if (row["BigTypeName"] != null)
                {
                    pro.BigTypeName = row["BigTypeName"].ToString();
                }
                if (row["bigtypeIndex"] != null)
                {
                    pro.bigtypeIndex = int.Parse(row["bigtypeIndex"].ToString());
                }
                if (row["UserId"] != null)
                {
                    pro.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["Def1"] != null)
                {
                    pro.Def1 = row["Def1"].ToString();
                }
                if (row["Def2"] != null)
                {
                    pro.Def2 = row["Def2"].ToString();
                }
                if (row["Def3"] != null)
                {
                    pro.Def3 = row["Def3"].ToString();
                }
                if (row["Def4"] != null)
                {
                    pro.Def4 = row["Def4"].ToString();
                }
                if (row["Def5"] != null)
                {
                    pro.Def5 = row["Def5"].ToString();
                }

                list.Add(pro);
            }
            return list;
        }
        /// <summary>
        /// 添加类别
        /// </summary>
        /// <returns></returns>
        public bool ProTypeInsert(model.AttributeModel mm)
        {
            pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
            try
            {
                model.producttype am = new model.producttype()
                {
                    BigId = mm.BigId,//大类别id
                    TypeName = mm.TypeName,//类别名称
                    TypeNo = mm.TypeNo,//类别编码
                    typeIndex = mm.TypeIndex,//类别排序
                    UserId = mm.UserId,//操作人
                };
                pdc.producttype.InsertOnSubmit(am);
                pdc.SubmitChanges();

                dal.InsertErrorlog(new model.errorlog()//操作日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->ProTypeInsert()",
                    ErrorMsg = "添加",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "添加类别->" + mm.TypeNo + "->" + mm.TypeName,
                    UserId = mm.UserId,
                });
                return true;
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateAttribute()",
                    ErrorMsg = "添加",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = mm.UserId,
                });
                return false;
            }

        }
        /// <summary>
        /// 删除类别
        /// </summary>
        /// <returns></returns>
        public bool ProTypeDelete(string TypeNo, int UserId)
        {
            try
            {
                pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
                var info = pdc.producttype.Where(a => a.TypeNo == TypeNo);//通过类别编号删除类别

                foreach (var temp in info)
                {
                    pdc.producttype.DeleteOnSubmit(temp);
                }
                pdc.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//操作日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->ProTypeDelete()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "删除类别->" + TypeNo,
                    UserId = UserId,
                });
                return true;
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->ProTypeDelete()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return false;
            }

        }
        /// <summary>
        /// 更新类别
        /// </summary>
        /// <returns></returns>
        public bool UpdateType(model.AttributeModel mm)
        {
            pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
            try
            {
                var info = pdc.producttype.Where(a => a.Id == mm.Id);//通过id更新类别

                foreach (var temp in info)
                {
                    temp.BigId = mm.BigId;//大类别Id
                    temp.TypeName = mm.TypeName;//类别名
                    temp.TypeNo = mm.TypeNo;//类别编号
                }
                pdc.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//操作日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateType()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "修改类别->" + mm.TypeNo + "->" + mm.TypeName,
                    UserId = mm.UserId,
                });
                return true;
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateType()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = mm.UserId,
                });
                return false;
            }
        }
        /// <summary>
        /// 添加大类别
        /// </summary>
        /// <returns></returns>
        public bool ProBigTypeInsert(model.AttributeModel mm)
        {
            pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
            try
            {
                model.productbigtype am = new model.productbigtype()
                {
                    bigtypeName = mm.BigTypeName,//大类别名称
                    bigtypeIndex = mm.bigtypeIndex,//大类别排序
                    UserId = mm.UserId,//操作人
                };
                pdc.productbigtype.InsertOnSubmit(am);//insert 打类别
                pdc.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//操作日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->ProBigTypeInsert()",
                    ErrorMsg = "添加",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "添加大类别->" + mm.BigTypeName,
                    UserId = mm.UserId,
                });
                return true;
            }
            catch (Exception ex)
            {

                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->ProBigTypeInsert()",
                    ErrorMsg = "添加",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = mm.UserId,
                });
                return false;
            }

        }
        /// <summary>
        /// 删除大类别
        /// </summary>
        /// <returns></returns>
        public bool ProBigTypeDelete(string Id, int UserId)
        {
            try
            {
                pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
                var info = pdc.productbigtype.Where(a => a.Id == Convert.ToInt32(Id));//通过id删除大类别
                foreach (var temp in info)
                {
                    pdc.productbigtype.DeleteOnSubmit(temp);
                }
                pdc.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->ProBigTypeDelete()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "删除大类别->" + Id,
                    UserId = UserId,
                });
                return true;
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->ProBigTypeDelete()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return false;
            }

        }
        /// <summary>
        /// 更新大类别
        /// </summary>
        /// <returns></returns>
        public string UpdateBigType(model.AttributeModel mm)
        {
            pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
            try
            {
                var check = pdc.productbigtype.Where(a => a.bigtypeName == mm.BigTypeName);
                var info = pdc.productbigtype.Where(a => a.Id == mm.Id && a.bigtypeName == mm.BigTypeName);

                if (info.Count() == 1)//更新时候通过id以及大类别名查询存在的话则update
                {
                    foreach (var temp in info)
                    {
                        temp.bigtypeName = mm.BigTypeName;//大类别名
                        temp.bigtypeIndex = mm.bigtypeIndex;//排序
                        temp.UserId = mm.UserId;//操作人
                    }
                    pdc.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->AttributeDal->UpdateBigType()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "修改大类别->" + mm.BigTypeName,
                        UserId = mm.UserId,
                    });
                    return "修改成功";
                }
                else
                {
                    if (check.Count() > 0)//更新的时候判断更新是否和原有是否冲突
                    {
                        return "存在";
                    }
                    else
                    {
                        info = pdc.productbigtype.Where(a => a.Id == mm.Id);//通过id更新
                        foreach (var temp in info)
                        {
                            temp.bigtypeName = mm.BigTypeName;//大类别名
                            temp.bigtypeIndex = mm.bigtypeIndex;//排序
                            temp.UserId = mm.UserId;//操作人
                        }
                        pdc.SubmitChanges();
                        dal.InsertErrorlog(new model.errorlog()//日志
                        {
                            errorSrc = "pbxdata.dal->AttributeDal->UpdateBigType()",
                            ErrorMsg = "修改",
                            errorTime = DateTime.Now,
                            operation = 2,
                            errorMsgDetails = "修改大类别->" + mm.BigTypeName,
                            UserId = mm.UserId,
                        });
                        return "修改成功";
                    }
                }

            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateBigType()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = mm.UserId,
                });
                return "修改失败";
            }
        }
        ///// <summary>
        ///// 返回所有字段名称集合
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <returns></returns>
        //public string[] getDataName(string sql)
        //{
        //    List<model.users> list = new List<model.users>();
        //    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        //    string[] ss = new string[dt.Columns.Count];

        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        ss[i] = dt.Columns[i].ColumnName;
        //    }

        //    return ss;
        //}
        /// <summary>
        /// 搜索条件商品类别信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSearchAttrTypeList(Dictionary<string, string> Dic, int page, int pages, out string counts)
        {
            List<model.product> list = new List<model.product>();
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.producttype
                    join a in context.productbigtype on c.BigId equals a.Id
                    into aa
                    from aaa in aa.DefaultIfEmpty()
                    join b in context.TypeIdToTariffNo on c.TypeNo equals b.TypeNo
                    into bb
                    from bbb in bb.DefaultIfEmpty()
                    join d in context.Tarifftab on bbb.TariffNo equals d.TariffNo
                    into dd
                    from ddd in dd.DefaultIfEmpty()
                    join e in context.TBProducttype on c.Def1 equals e.cid
                    into ee
                    from eee in ee.DefaultIfEmpty()
                    select new
                    {
                        Id = c.Id,
                        BigId = c.BigId,//打类别Id
                        TypeName = c.TypeName,//类别名称
                        TypeNo = c.TypeNo,//类别编码
                        typeIndex = c.typeIndex,//排序
                        UserId = c.UserId,//操作人
                        Def1 = c.Def1,//淘宝编码
                        Def2 = c.Def2,//默认2
                        Def3 = c.Def3,//默认3
                        Def4 = c.Def4,//默认4
                        Def5 = c.Def5,//默认5
                        bigtypeName = aaa.bigtypeName,//大类别名称
                        TBtypeName = eee.TBtypeName,//淘宝类别名称
                        TariffName = ddd.TariffName,//关税名称
                    };
            if (Dic["txtSeachName"] != "")//通过类别名查询
            {
                q = q.Where(a => a.TypeName.Contains(Dic["txtSeachName"]));
            }
            if (Dic["txtSeachBigName"] != "")//通过大类别查询
            {
                q = q.Where(a => a.bigtypeName.Contains(Dic["txtSeachBigName"]));
            }
            if (Dic["txtSeachTypeNo"] != "")//通过类别编号查询
            {
                q = q.Where(a => a.TypeNo.Contains(Dic["txtSeachTypeNo"]));
            }
            q = q.OrderBy(a => a.BigId);//通过打类别Id排序
            counts = q.Count().ToString();
            dt = LinqToDataTable.LINQToDataTable(q.Skip(pages * (page - 1)).Take(pages));//翻页
            //string MinNid = (pages * (page - 1)).ToString();
            //string MaxNid = (pages * page).ToString();
            //IDataParameter[] ipr = new IDataParameter[]
            //{
            //    new SqlParameter("TypeName",Dic["txtSeachName"]),
            //    new SqlParameter("bigtypeName",Dic["txtSeachBigName"]),
            //    new SqlParameter("TypeNo",Dic["txtSeachTypeNo"]),
            //    new SqlParameter("MinNid",MinNid),
            //    new SqlParameter("MaxNid",MaxNid),
            //    new SqlParameter("sql",""),
            //};
            //IDataParameter[] iprc = new IDataParameter[]
            //{
            //    new SqlParameter("TypeName",Dic["txtSeachName"]),
            //    new SqlParameter("bigtypeName",Dic["txtSeachBigName"]),
            //    new SqlParameter("TypeNo",Dic["txtSeachTypeNo"]),
            //    new SqlParameter("sql",""),
            //};
            //dt = Select(ipr, "Selectproducttype");
            //counts = Select(iprc, "SelectproducttypeCount").Rows[0][0].ToString();

            return dt;
        }
        /// <summary>
        /// 修改productPorperty表
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private List<model.AttributeModel> DataTableToListAttr(DataRowCollection dataRow)
        {
            List<model.AttributeModel> list = new List<model.AttributeModel>();
            foreach (DataRow row in dataRow)
            {
                model.AttributeModel pro = new model.AttributeModel();
                if (row["Id"] != null)
                {
                    pro.Id = int.Parse(row["Id"].ToString());
                }
                if (row["TypeId"] != null)
                {
                    pro.TypeName = row["TypeId"].ToString();
                }
                if (row["PropertyName"] != null)
                {
                    pro.PropertyName = row["PropertyName"].ToString();
                }
                if (row["PorpertyIndex"] != null)
                {
                    pro.PorpertyIndex = int.Parse(row["PorpertyIndex"].ToString());
                }
                if (row["UserId"] != null)
                {
                    pro.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["Def1"] != null)
                {
                    pro.Def1 = row["Def1"].ToString();
                }
                if (row["Def2"] != null)
                {
                    pro.Def2 = row["Def2"].ToString();
                }
                if (row["Def3"] != null)
                {
                    pro.Def3 = row["Def3"].ToString();
                }
                if (row["Def4"] != null)
                {
                    pro.Def4 = row["Def4"].ToString();
                }
                if (row["Def5"] != null)
                {
                    pro.Def5 = row["Def5"].ToString();
                }

                list.Add(pro);
            }
            return list;
        }
        /// <summary>
        /// 根据权限获取类别下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeDDlist(string CustomerId)
        {

            DataTable dt = new DataTable();//根据个人id显示可查询类别权限
            string sql = @"select TypeName,TypeNo from producttype a left join PersonaTypeConfit b on a.TypeNo=b.TypeId where b.CustomerId=" + CustomerId + " order by TypeName";
            dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 得到所有类别下拉
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeDDlist()
        {
            string sql = "select * from producttype";//查询所有类别
            DataSet ds = DbHelperSQL.Query(sql);
            DataTable dt = ds.Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据权限获取品牌下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetCatDDlist(string CustomerId)
        {
            DataTable dt = new DataTable();//根据个人id显示查询品牌权限
            string sql = @"select a.BrandName,BrandAbridge from brand a left join BrandConfigPersion b on a.BrandAbridge=b.BrandId where b.CustomerId=" + CustomerId + " order by a.BrandName";
            dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 得到所有品牌下拉
        /// </summary>
        /// <returns></returns>
        public DataTable GetCatDDlist()
        {
            string sql = "select * from brand";//查询所有品牌
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 根据权限获取品牌表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchBrand(string Name, string page, string Selpages, out string counts)
        {
            DataTable dt1 = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            //pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
            dt1 = null;
            var q = from c in context.brand
                    join t in context.TBBrand
                    on c.Def1 equals t.vid
                    into temp
                    from tt in temp.DefaultIfEmpty()
                    select new
                    {
                        Id = c.Id,//id
                        BrandName = c.BrandName,//品牌名
                        BrandAbridge = c.BrandAbridge,//品牌缩写
                        Def1 = c.Def1,//淘宝编码
                        Def2 = c.Def2,//品牌归属地
                        TBBrandName = tt.TBBrandName,//淘宝品牌名称
                    };
            if (Name != "")//根据品牌名和品牌缩写模糊查询
            {
                q = q.Where(a => a.BrandName.Contains(Name) || a.BrandAbridge.Contains(Name));
            }
            counts = q.ToList().Count.ToString();//返回数量
            q = q.Skip((Convert.ToInt32(page) - 1) * Convert.ToInt32(Selpages)).Take(Convert.ToInt32(Selpages));//翻页
            dt1 = LinqToDataTable.LINQToDataTable(q);
            return dt1;
        }
        /// <summary>
        /// 获取淘宝品牌表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchTBBrand(string Name, string page, string Selpages, out string counts)
        {
            DataTable dt1 = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            //pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
            dt1 = null;
            var q = from c in context.TBBrand select c;
            if (Name != "")//模糊查询淘宝名牌
            {
                q = q.Where(a => a.TBBrandName.Contains(Name));
            }
            counts = q.ToList().Count.ToString();//返回查询数量
            q = q.OrderBy(s => s.TBBrandName).Skip((Convert.ToInt32(page) - 1) * Convert.ToInt32(Selpages)).Take(Convert.ToInt32(Selpages));//翻页
            dt1 = LinqToDataTable.LINQToDataTable(q);
            return dt1;
        }
        /// <summary>
        /// 获取淘宝类别表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchTBType(string Name, string page, string Selpages, out string counts)
        {
            DataTable dt1 = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            //pbxdata.model.pbxdatasourceDataContext pdc = new model.pbxdatasourceDataContext(connctionstring);
            dt1 = null;
            var q = from c in context.TBProducttype select c;
            if (Name != "")//模糊查询淘宝类别名
            {
                q = q.Where(a => a.TBtypeName.Contains(Name));
            }
            counts = q.ToList().Count.ToString();//返回查询数量
            q = q.OrderBy(s => s.TBtypeName).Skip((Convert.ToInt32(page) - 1) * Convert.ToInt32(Selpages)).Take(Convert.ToInt32(Selpages));//翻页
            dt1 = LinqToDataTable.LINQToDataTable(q);
            return dt1;
        }
        /// <summary>
        /// 搜索条件供应商类别表信息
        /// </summary>
        /// <returns></returns>
        public DataTable VenProducttypeSearch(Dictionary<string, string> Dic, int page, int pages, out string counts)
        {
            List<model.product> list = new List<model.product>();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            var q = from c in context.ItalyPorductStock
                    group c by new { c.Cat2, c.Vencode } into cc
                    join a in context.producttypeVen on new { Cat2 = cc.Key.Cat2, Vencode = cc.Key.Vencode } equals new { Cat2 = a.TypeNameVen, Vencode = a.Vencode }
                    into aa
                    from aaa in aa.DefaultIfEmpty()
                    join b in context.productbigtype on aaa.BigId equals b.Id
                    into bb
                    from bbb in bb.DefaultIfEmpty()
                    join d in context.productsource on cc.Key.Vencode equals d.SourceCode
                    into dd
                    from ddd in dd.DefaultIfEmpty()
                    select new
                    {
                        Cat2 = cc.Key.Cat2,//--供应商类别名
                        Vencode2 = cc.Key.Vencode,//--供应商编码
                        Id = aaa.Id == null ? 0 : aaa.Id,
                        TypeName = aaa.TypeName,//--类别名称
                        TypeNo = aaa.TypeNo,//--类别编号
                        BigId = aaa.BigId,//--大类别Id
                        bigtypeName = bbb.bigtypeName,//--大类别名称
                        sourceName = ddd.sourceName,//--供应商名称
                    };
            if (Dic["SearchName"] != "")
            {
                q = q.Where(a => a.TypeName.Contains(Dic["SearchName"]));
            }
            if (Dic["SearchVenName"] != "")
            {
                q = q.Where(a => a.Cat2.Contains(Dic["SearchVenName"]));
            }
            if (Dic["SearchVencode"] != "")
            {
                q = q.Where(a => a.Vencode2 == Dic["SearchVencode"]);
            }
            if (Dic["bangd"] == "0")
            {
                q = q.Where(a => a.TypeName != "");
            }
            else if (Dic["bangd"] == "1")
            {
                q = q.Where(a => a.TypeName == null);
            }
            counts = q.Count().ToString();
            dt = LinqToDataTable.LINQToDataTable(q.Skip(pages * (page - 1)).Take(pages));



            //string MinNid = (pages * (page - 1)).ToString();
            //string MaxNid = (pages * page).ToString();
            //IDataParameter[] ipr = new IDataParameter[]
            //{
            //    new SqlParameter("TypeName",Dic["SearchName"]),
            //    new SqlParameter("TypeNameVen",Dic["SearchVenName"]),
            //    new SqlParameter("Vencode",Dic["SearchVencode"]),
            //    new SqlParameter("bangd",Dic["bangd"]),
            //    new SqlParameter("MinNid",MinNid),
            //    new SqlParameter("MaxNid",MaxNid),
            //    new SqlParameter("sql",""),
            //};
            //IDataParameter[] iprc = new IDataParameter[]
            //{
            //    new SqlParameter("TypeName",Dic["SearchName"]),
            //    new SqlParameter("TypeNameVen",Dic["SearchVenName"]),
            //    new SqlParameter("Vencode",Dic["SearchVencode"]),
            //    new SqlParameter("bangd",Dic["bangd"]),
            //    new SqlParameter("sql",""),
            //};
            //dt = Select(ipr, "SelectProducttypeVen");
            //counts = Select(iprc, "SelectProducttypeVenCount").Rows[0][0].ToString();

            return dt;
        }
        /// <summary>
        /// 修改供应商类别表
        /// </summary>
        /// <returns></returns>
        public string UpdateProducttypeVen(string Id, string TypeNameVen, string Vencode, string TypeNo, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = from c in context.producttype where c.TypeNo == TypeNo select c;
                DataTable dt = LinqToDataTable.LINQToDataTable(q);
                if (Id == "0")//判断供应商类别是否存在 0为不存在则insert 其他为存在则update
                {
                    var q2 = context.producttypeVen.Where(a => a.TypeNameVen == TypeNameVen).Where(a => a.Vencode == Vencode).ToList();
                    if (q2.Count == 0)
                    {
                        model.producttypeVen PV = new model.producttypeVen()
                        {
                            BigId = Convert.ToInt32(dt.Rows[0]["BigId"].ToString()),//大类别Id
                            TypeName = dt.Rows[0]["TypeName"].ToString(),//类别名称
                            TypeNo = TypeNo,//类别编号
                            TypeNameVen = TypeNameVen,//供应商类别名
                            Vencode = Vencode,//供应商
                            UserId = UserId,//操作人

                        };
                        context.producttypeVen.InsertOnSubmit(PV);
                        context.SubmitChanges();

                        dal.InsertErrorlog(new model.errorlog()//日志
                        {
                            errorSrc = "pbxdata.dal->AttributeDal->UpdateProducttypeVen()",
                            ErrorMsg = "添加",
                            errorTime = DateTime.Now,
                            operation = 2,
                            errorMsgDetails = "添加供应商类别表->" + TypeNameVen,
                            UserId = UserId,
                        });

                    }
                    else
                    {
                        return "该类别存在!";
                    }


                }
                else
                {
                    var q1 = context.producttypeVen.Where(a => a.Id == Convert.ToInt32(Id));
                    foreach (var i in q1)//更新供应商类别
                    {
                        i.BigId = Convert.ToInt32(dt.Rows[0]["BigId"].ToString());//大类别Id
                        i.TypeName = dt.Rows[0]["TypeName"].ToString();//类别名称
                        i.TypeNo = TypeNo;//类别编号
                        i.TypeNameVen = TypeNameVen;//供应商类别名
                        i.Vencode = Vencode;//供应商
                        i.UserId = UserId;//操作人
                    }
                    context.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()//日志
                    {
                        errorSrc = "pbxdata.dal->AttributeDal->UpdateProducttypeVen()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "修改供应商类别表->" + Id + "->" + TypeNameVen,
                        UserId = UserId,
                    });
                }
                return "修改成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateProducttypeVen()",
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
        public string deleteProducttypeVen(string Id, int UserId)
        {
            try
            {
                string item = "";
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.producttypeVen.Where(a => a.Id == Convert.ToInt32(Id));
                foreach (var i in q)//通过Id删除供应商类别
                {
                    item = i.TypeNameVen;//日志获取被删除供应商类别
                    context.producttypeVen.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->deleteProducttypeVen()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "删除供应商类别表->" + item,
                    UserId = UserId,
                });
                return "删除成功!";

            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->deleteProducttypeVen()",
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
        /// 类别搜索
        /// </summary>
        public DataTable SearchProducttype(string TypeName)
        {
            model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();//模糊查询类别
            var q = context.producttype.Where(a => a.TypeName.Contains(TypeName) || a.TypeNo.Contains(TypeName));
            return LinqToDataTable.LINQToDataTable(q);

        }
        /// <summary>
        /// 更新淘宝类别
        /// </summary>
        public string UpdateTBType(string TypeName, string cid, string Id, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.TBProducttype.Where(a => a.Id == Convert.ToInt32(Id));//通过Id查询
                foreach (var i in q)
                {
                    i.TBtypeName = TypeName;//淘宝类别名
                    i.cid = cid;//淘宝编码
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateTBType()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "更新淘宝类别->" + TypeName,
                    UserId = UserId,
                });
                return "修改成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateTBType()",
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
        /// 添加淘宝类别
        /// </summary>
        public string AddTBType(string TypeName, string cid, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var check = context.TBProducttype.Where(a => a.TBtypeName == TypeName & a.cid == cid).ToList();
                if (check.Count > 0)//添加淘宝类别时判断是否存在
                {
                    return "该类别已存在!";
                }
                else
                {
                    model.TBProducttype tb = new model.TBProducttype()
                    {
                        TBtypeName = TypeName,//淘宝类别名
                        cid = cid,//淘宝编码
                    };
                    context.TBProducttype.InsertOnSubmit(tb);
                    context.SubmitChanges();
                    dal.InsertErrorlog(new model.errorlog()//日志
                    {
                        errorSrc = "pbxdata.dal->AttributeDal->AddTBType()",
                        ErrorMsg = "修改",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "添加淘宝类别->" + TypeName,
                        UserId = UserId,
                    });
                    return "添加成功!";
                }
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->AddTBType()",
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
        /// 更新淘宝属性
        /// </summary>
        public string UpdateTBProperty(string PropertyName, string vid, string Id, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.TBProductProperty.Where(a => a.Id == Convert.ToInt32(Id));//通过Id查询
                foreach (var i in q)
                {
                    i.TBPropertyName = PropertyName;//淘宝属性名称
                    i.vid = vid;//淘宝属性编码
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateTBProperty()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "更新淘宝属性->" + PropertyName,
                    UserId = UserId,
                });
                return "";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//错误日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->UpdateTBProperty()",
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
        /// 添加淘宝属性
        /// </summary>
        public string AddTBProperty(string TBPropertyName, string vid, string parent_cid, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                model.TBProductProperty tb = new model.TBProductProperty()
                {
                    TBPropertyName = TBPropertyName,//淘宝属性名称
                    vid = vid,//淘宝属性编码
                    parent_cid = parent_cid,//该属性名称所属类别
                };
                context.TBProductProperty.InsertOnSubmit(tb);//insert淘宝属性
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->AddTBProperty()",
                    ErrorMsg = "修改",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "添加淘宝属性->" + TBPropertyName,
                    UserId = UserId,
                });
                return "添加成功!";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->AddTBProperty()",
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
        /// 删除淘宝属性
        /// </summary>
        public string DeleteTBProperty(string Id, int UserId)
        {
            try
            {
                string item = "";
                model.pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.TBProductProperty.Where(a => a.Id == Convert.ToInt32(Id));//通过id删除淘宝属性
                foreach (var i in q)
                {
                    item = i.TBPropertyName;
                    context.TBProductProperty.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->DeleteTBProperty()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 2,
                    errorMsgDetails = "删除淘宝属性->" + item,
                    UserId = UserId,
                });
                return "";
            }
            catch (Exception ex)
            {
                dal.InsertErrorlog(new model.errorlog()//日志
                {
                    errorSrc = "pbxdata.dal->AttributeDal->DeleteTBProperty()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = ex.Message,
                    UserId = UserId,
                });
                return "删除失败!";
            }
        }
        //**********7.6
        /// <summary>
        /// 添加与系统类别对应的类别
        /// </summary>
        /// <param name="typeno"></param>
        /// <param name="typename"></param>
        /// <param name="typenameven"></param>
        /// <returns></returns>
        public string InsertProducttypeVenExcel(string typeno, string typename, string typenameven, string vencode)
        {
            pbxdatasourceDataContext pbc = new pbxdatasourceDataContext(connctionstring);
            List<producttypeVen> list = pbc.producttypeVen.Where(a => a.TypeNameVen == typenameven && a.Vencode == vencode).ToList();//判断当前数据源当前类别是否已存在
            if (list.Count > 0)
            {
                return "当前数据源类别已存在";
            }
            else
            {
                try
                {
                    IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("TypeName",typename),
                    new SqlParameter("TypeNo",typeno),
                    new SqlParameter("TypeNameVen",typenameven),
                    new SqlParameter("Vencode",vencode)
                };
                    return Add(ipr, "InsertProducttypeVenExcel");
                }
                catch (Exception)
                {
                    return "添加失败！";
                    throw;
                }
            }
        }
        /// <summary>
        /// ---得到所有与excel表格关联的类别
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetProductTypeExcel(string[] str, string minid, string maxid)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("TypeName",str[0]),
                    new SqlParameter("TypeNameVen",str[1]),
                    new SqlParameter("minId",minid),
                    new SqlParameter("maxId",maxid),
                };
                DataTable dt = Select(ipr, "GetProductTypeExcel");
                return dt;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        /// <summary>
        /// ---得到所有与excel表格关联的类别个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetProductTypeExcelCount(string[] str)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("sql",""),
                    new SqlParameter("sqlbody",""),
                    new SqlParameter("TypeName",str[0]),
                    new SqlParameter("TypeNameVen",str[1]),
                };
                DataTable dt = Select(ipr, "GetProductTypeExcelCount");
                int count = 0;
                if (dt.Rows.Count > 0)
                {
                    count = int.Parse(dt.Rows[0][0].ToString());
                }
                else
                {
                    count = 0;
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
        /// 删除类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteTypeExcel(string id)
        {
            try
            {
                IDataParameter[] ipr = new IDataParameter[] 
                {
                    new SqlParameter("Id",id)
                };
                return Delete(ipr, "DeleteTypeExcel");
            }
            catch (Exception)
            {
                return "删除失败！";
                throw;
            }

        }




        public string mjj(OrderDetails od)
        {
            pbxdatasourceDataContext pbdc = new pbxdatasourceDataContext(connctionstring);
            var info = pbdc.OrderDetails;
            foreach (OrderDetails temp in info)
            {
                if (od.Def3 != null)
                    temp.Def3 = od.Def3;

            }
            return "";
        }
    }
}
