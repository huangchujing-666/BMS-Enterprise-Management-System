/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       tableFiledPerssiondal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       tableFiledPerssiondal
    * 创建时间：       2015/2/11 11:10:45
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace pbxdata.dal
{
    public class tableFiledPerssiondal : dataoperating, itableFiledPerssion
    {
        Errorlogdal errordal = new Errorlogdal();
        public List<model.tableFiledPerssion> getTable(IDataParameter[] ipara, string procName)
        {
            List<model.tableFiledPerssion> list = new List<model.tableFiledPerssion>();
            DataTable dt = Select(ipara, procName);
            list = DataRowToModel(dt.Rows);
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<model.tableFiledPerssion> DataRowToModel(DataRowCollection rowCollection)
        {
            List<model.tableFiledPerssion> list = new List<model.tableFiledPerssion>();

            foreach (DataRow row in rowCollection)
            {
                model.tableFiledPerssion modelMd = new model.tableFiledPerssion();
                if (row != null)
                {
                    if (row["Id"] != null && row["Id"].ToString() != "")
                    {
                        modelMd.Id = int.Parse(row["Id"].ToString());
                    }
                    if (row["tableName"] != null)
                    {
                        modelMd.tableName = row["tableName"].ToString();
                    }
                    if (row["tableFiled"] != null)
                    {
                        modelMd.tableFiled = row["tableFiled"].ToString();
                    }
                    if (row["tableLevel"] != null && row["tableLevel"].ToString() != "")
                    {
                        modelMd.tableLevel = int.Parse(row["tableLevel"].ToString());
                    }
                    if (row["tableNameState"] != null && row["tableNameState"].ToString() != "")
                    {
                        modelMd.tableNameState = int.Parse(row["tableNameState"].ToString());
                    }
                    if (row["tableFiledState"] != null && row["tableFiledState"].ToString() != "")
                    {
                        modelMd.tableFiledState = int.Parse(row["tableFiledState"].ToString());
                    }
                    if (row["tableIndex"] != null && row["tableIndex"].ToString() != "")
                    {
                        modelMd.tableIndex = int.Parse(row["tableIndex"].ToString());
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
        /// 根据字段ID返回字段名称
        /// </summary>
        /// <param name="filedid"></param>
        /// <returns></returns>
        public string getFiledName(int filedid)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.tableFiledPerssion where c.Id == filedid select c.tableName;
            s = p.ToList().Count > 0 ? p.ToList()[0].ToString() : string.Empty; ;
            return s;
        }

        /// <summary>
        /// 根据字段ID集合返回字段名称集合
        /// </summary>
        /// <param name="filedids"></param>
        /// <returns></returns>
        public string[] getFiledName(int[] filedids)
        {
            string[] ss;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.tableFiledPerssion where (filedids).Contains(c.Id) select c.tableName).ToList();
            ss = new string[p.Count];
            for (int i = 0; i < p.Count; i++)
            {
                ss[i] = p[i];
            }

            return ss;
        }

        /// <summary>
        /// 根据字段ID集合返回有权限的字段,并组合成sql语句
        /// </summary>
        /// <param name="filedids"></param>
        /// <param name="tableName">表名称</param>
        /// <returns></returns>
        public string getFiledPermissionSQL(int[] filedids, string tableName)
        {
            StringBuilder s = new StringBuilder();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.tableFiledPerssion where (filedids).Contains(c.Id) select c.tableName).ToList();
            s.Append("select ");
            for (int i = 0; i < p.Count; i++)
            {
                s.Append(p[i]);
                s.Append(",");
            }
            s.Remove(s.Length - 1, 1);
            s.Append(" from ");
            s.Append(tableName);

            return s.ToString();
        }

        /// <summary>
        ///添加表
        /// </summary>
        /// <returns></returns>
        public string AddTabName(string tableName, int UserId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {

                string check = @"select count(*)  from syscolumns where id=object_id('" + tableName + "')";
                if (DbHelperSQL.Query(check).Tables[0].Rows[0][0].ToString() == "0")
                {
                    return "该表不存在!";
                }
                else
                {
                    var p = (from c in context.tableFiledPerssion where c.tableName == tableName select c.tableName).FirstOrDefault();
                    if (p == null)
                    {
                        model.tableFiledPerssion tt = new model.tableFiledPerssion()
                        {
                            tableName = tableName,
                            tableLevel = 0,
                            tableNameState = 1,
                            tableFiledState = 1,
                        };
                        context.tableFiledPerssion.InsertOnSubmit(tt);
                        context.SubmitChanges();
                        string sql = @"insert into tableFiledPerssion(tableName,tableLevel) select name,(select id from tableFiledPerssion where tableName='" + tableName + "') from sys.syscolumns where id =object_id('" + tableName + "')";
                        DbHelperSQL.ExecuteSql(sql);
                        errordal.InsertErrorlog(new model.errorlog()
                        {
                            errorSrc = "pbxdata.dal->tablefiledPerssiondal->AddTabName()",
                            ErrorMsg = "添加",
                            errorTime = DateTime.Now,
                            operation = 2,
                            errorMsgDetails = "添加表->" + tableName,
                            UserId = UserId,
                        });
                    }
                    else
                    {
                        return "该表已存在!";
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                errordal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->tablefiledPerssiondal->AddTabName()",
                    ErrorMsg = "添加",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = "添加表失败->" + tableName+"->"+ex.Message,
                    UserId = UserId,
                });
                return "添加失败!";
            }

        }
        /// <summary>
        ///添加列
        /// </summary>
        /// <returns></returns>
        public string AddFiledName(string tableName, string tableLevel, int UserId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                int tableLevels = Convert.ToInt32(tableLevel);
                var p = (from c in context.tableFiledPerssion where c.tableName == tableName && c.tableLevel == tableLevels select c.tableName).FirstOrDefault();
                if (p == null)
                {
                    model.tableFiledPerssion tt = new model.tableFiledPerssion()
                    {
                        tableName = tableName,
                        tableLevel = tableLevels,
                        tableNameState = 1,
                        tableFiledState = 1,
                    };
                    context.tableFiledPerssion.InsertOnSubmit(tt);
                    context.SubmitChanges();
                    errordal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->tablefiledPerssiondal->AddFiledName()",
                        ErrorMsg = "添加",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "添加列->" + tableName,
                        UserId = UserId,
                    });
                    return "";
                }
                else
                {
                    return "该列已存在!";
                }

            }
            catch (Exception ex)
            {
                return "添加失败!";
            }

        }

        /// <summary>
        /// 删除表,列
        /// </summary>
        public string DeletetableFiled(string Id, int UserId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                int Ids = Convert.ToInt32(Id);
                var p = (from c in context.tableFiledPerssion where c.Id == Ids select c).First();
                var cp = (from c in context.tableFiledPerssion where c.Id == Ids select c).FirstOrDefault();
                if (cp.tableLevel == 0)
                {
                    var ps = from c in context.tableFiledPerssion where c.tableLevel == cp.Id select c;
                    
                    foreach (var item in ps)
                    {
                        context.tableFiledPerssion.DeleteOnSubmit(item);
                    }
                    context.tableFiledPerssion.DeleteOnSubmit(p);
                    context.SubmitChanges();
                    errordal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->tablefiledPerssiondal->DeletetableFiled()",
                        ErrorMsg = "删除",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "删除表->" + cp.tableName,
                        UserId = UserId,
                    });
                    return "";
                }
                else
                {
                    context.tableFiledPerssion.DeleteOnSubmit(p);
                    context.SubmitChanges();
                    errordal.InsertErrorlog(new model.errorlog()
                    {
                        errorSrc = "pbxdata.dal->tablefiledPerssiondal->DeletetableFiled()",
                        ErrorMsg = "删除",
                        errorTime = DateTime.Now,
                        operation = 2,
                        errorMsgDetails = "删除列->" + p.tableName,
                        UserId = UserId,
                    });
                    return "";
                }
                
            }
            catch (Exception ex)
            {
                errordal.InsertErrorlog(new model.errorlog()
                {
                    errorSrc = "pbxdata.dal->tablefiledPerssiondal->DeletetableFiled()",
                    ErrorMsg = "删除",
                    errorTime = DateTime.Now,
                    operation = 1,
                    errorMsgDetails = "删除表或列->" + ex.Message,
                    UserId = UserId,
                });
                return "删除失败!";
            }
        }
        /// <summary>
        /// 添加字段描述,添加表描述tableFiledPerssion表
        /// </summary>
        public string AddDescript(string Id, string Descript)
        {
            try
            {
                int nid = Convert.ToInt32(Id);
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var p = (from c in context.tableFiledPerssion where c.Id == nid select c);
                foreach (var item in p)
                {
                    item.Def1 = Descript;
                }
                context.SubmitChanges();
                return "";
            }
            catch (Exception ex)
            {
                return "修改失败!";
            }
        }
        /// <summary>
        /// 修改表名,列名
        /// </summary>
        public string UpdateTabName(string Id, string TabName)
        {
            try
            {
                int nid = Convert.ToInt32(Id);
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var p = (from c in context.tableFiledPerssion where c.Id == nid select c);
                foreach (var item in p)
                {
                    item.tableName = TabName;
                }
                context.SubmitChanges();
                return "";
            }
            catch (Exception ex)
            {
                return "修改失败!";
            }
        }
    }
}
