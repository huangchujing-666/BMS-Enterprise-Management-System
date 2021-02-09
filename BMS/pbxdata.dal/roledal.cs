/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       roledal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       roledal
    * 创建时间：       2015/2/11 17:20:59
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
    public class roledal : dataoperating, irole
    {
        public List<model.persona> getRole(IDataParameter[] ipara, string procName)
        {
            List<model.persona> list = new List<model.persona>();
            DataTable dt = Select(ipara, procName);
            list = DataRowToModel(dt.Rows);
            return list;
        }

        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public model.persona getRoleName(int id)
        {
            model.persona mdPersona = new model.persona();
            //model.ConnectionString md = new model.ConnectionString();
            //var s = from c in md.persona where c.Id==id select c;
            //foreach (var item in s)
            //{
            //    mdPersona = item;
            //}
            //return mdPersona;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var s = from c in context.persona where c.Id == id select c;
            foreach (var item in s)
            {
                mdPersona = item;
            }
            return mdPersona;
            
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<model.persona> DataRowToModel(DataRowCollection rowCollection)
        {
            List<model.persona> list = new List<model.persona>();

            foreach (DataRow row in rowCollection)
            {
                model.persona modelMd = new model.persona();
                if (row != null)
                {
                    if (row["Id"] != null && row["Id"].ToString() != "")
                    {
                        modelMd.Id = int.Parse(row["Id"].ToString());
                    }
                    if (row["PersonaName"] != null)
                    {
                        modelMd.PersonaName = row["PersonaName"].ToString();
                    }
                    if (row["PersonaIndex"] != null && row["PersonaIndex"].ToString() != "")
                    {
                        modelMd.PersonaIndex = int.Parse(row["PersonaIndex"].ToString());
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
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public DataTable getRole()
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            DataTable dt = new DataTable();
            var p = from c in context.persona select c;
            dt = LinqToDataTable.LINQToDataTable<model.persona>(p);
            return dt;
        }
        /// <summary>
        /// 复制角色权限
        /// </summary>
        public string CopyUsers(string Id, string PersonaId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                string sql = string.Empty;
                DataTable dt = new DataTable();
                
                var q = from c in context.personapermisson where c.personaId == Convert.ToInt32(Id) select c;
                foreach (var item in q)
                {
                    
                    var person = new model.personapermisson()
                    {
                        personaId = Convert.ToInt32(PersonaId),
                        MemuId=item.MemuId,
                        FunId=item.FunId,
                        FieldId=item.FieldId
                    };
                    context.personapermisson.InsertOnSubmit(person);
                    //sql = @"insert into personapermisson(personaId,MemuId,FunId,FieldId) values(" + Id + "," + item.MemuId + "," + Convert.ToInt32(item.FunId == null ? 0 : item.FunId) + "," + Convert.ToInt32(item.FieldId == null ? 0 : item.FieldId) + ")";
                    //DbHelperSQL.ExecuteSql(sql);
                }
                context.SubmitChanges();
                return "复制成功!";
            }
            catch(Exception ex)
            {
                return "复制失败!";
            }
            
        }
    }
}
