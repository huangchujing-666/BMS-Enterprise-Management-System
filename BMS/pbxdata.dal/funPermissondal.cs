/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       funPermissondal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       funPermissondal
    * 创建时间：       2015/2/10 15:26:52
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class funPermissondal : dataoperating, ifunPermisson
    {

        public List<model.funpermisson> getFun(IDataParameter[] ipara, string procName)
        {
            List<model.funpermisson> list = new List<model.funpermisson>();
            DataTable dt = Select(ipara, procName);
            list = DataRowToModel(dt.Rows);
            return list;
        }
        /// <summary>
        /// 获取功能名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public model.funpermisson getFunName(int id)
        {
            model.funpermisson mdFunpermisson = new model.funpermisson();
            //model.ConnectionString md = new model.ConnectionString();
            //var s = from c in md.funpermisson where c.Id == id select c;
            //foreach (var item in s)
            //{
            //    mdFunpermisson = item;
            //}
            //return mdFunpermisson;

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var s = from c in context.funpermisson where c.Id == id select c;
            foreach (var item in s)
            {
                mdFunpermisson = item;
            }
            return mdFunpermisson;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<model.funpermisson> DataRowToModel(DataRowCollection rowCollection)
        {
            List<model.funpermisson> list = new List<model.funpermisson>();

            foreach (DataRow row in rowCollection)
            {
                model.funpermisson modelMd = new model.funpermisson();
                if (row != null)
                {
                    if (row["Id"] != null && row["Id"].ToString() != "")
                    {
                        modelMd.Id = int.Parse(row["Id"].ToString());
                    }
                    if (row["MenuId"] != null && row["MenuId"].ToString() != "")
                    {
                        modelMd.MenuId = int.Parse(row["MenuId"].ToString());
                    }
                    if (row["FunName"] != null)
                    {
                        modelMd.FunName = row["FunName"].ToString();
                    }
                    if (row["FunIndex"] != null && row["FunIndex"].ToString() != "")
                    {
                        modelMd.FunIndex = int.Parse(row["FunIndex"].ToString());
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
        /// 删除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string del(int id)
        {
            string s = string.Empty;
            try
            { 
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.funpermisson fun = (from c in context.funpermisson where c.Id == id select c).FirstOrDefault();
                context.funpermisson.DeleteOnSubmit(fun);
                context.SubmitChanges();
                s = "删除成功！";
            }
            catch (Exception ex)
            {
                s = "删除失败！";
            }
            return s;

        }
    }
}
