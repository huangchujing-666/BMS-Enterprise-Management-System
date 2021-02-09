/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       menubll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       menudal
    * 创建时间：       2015/2/2 13:55:09
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
    public class menudal : dataoperating, imenu
    {

        public List<model.menu> getMenu(IDataParameter[] ipara, string procName)
        {
            List<model.menu> list = new List<model.menu>();
            DataTable dt = Select(ipara, procName);
            list = DataRowToModel(dt.Rows);
            return list;
        }

        public model.menu getMenuName(int id)
        {
            model.menu mdMenu = new model.menu();

            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var s = from c in context.menu where c.Id == id select c;
            foreach (var item in s)
            {
                mdMenu = item;
            }
            return mdMenu;

        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<model.menu> DataRowToModel(DataRowCollection rowCollection)
        {
            List<model.menu> list = new List<model.menu>();

            foreach (DataRow row in rowCollection)
            {
                model.menu modelMd = new model.menu();
                if (row != null)
                {
                    if (row["Id"].ToString() != null && row["Id"].ToString() != "")
                    {
                        modelMd.Id = int.Parse(row["Id"].ToString());
                    }
                    if (row["menuName"] != null)
                    {
                        modelMd.menuName = row["menuName"].ToString();
                    }
                    if (row["MenuSrc"] != null)
                    {
                        modelMd.MenuSrc = row["MenuSrc"].ToString();
                    }
                    if (row["MenuLevel"] != null && row["MenuLevel"].ToString() != "")
                    {
                        modelMd.MenuLevel = int.Parse(row["MenuLevel"].ToString());
                    }
                    if (row["MenuIndex"] != null && row["MenuIndex"].ToString() != "")
                    {
                        modelMd.MenuIndex = int.Parse(row["MenuIndex"].ToString());
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
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public string del(int id)
        {
            string s = string.Empty;
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.menu p = (from c in context.menu where c.Id == id select c).FirstOrDefault();
                context.menu.DeleteOnSubmit(p);
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
