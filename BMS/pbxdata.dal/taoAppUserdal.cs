/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       taoAppUserdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       taoAppUserdal
    * 创建时间：       2015/2/10 9:57:30
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
    public class taoAppUserdal:dataoperating,itaoAppUser
    {
        public List<model.taoAppUser> GetModelList(IDataParameter[] ipara, string procName)
        {
            List<model.taoAppUser> list = new List<model.taoAppUser>();
            DataTable dt = Select(ipara, procName);
            list = DataRowToModel(dt.Rows);

            return list;
        }


        public model.taoAppUser GetModelByNick(IDataParameter[] ipara, string procName)
        {
            List<model.taoAppUser> list = new List<model.taoAppUser>();
            DataTable dt = Select(ipara, procName);
            list = DataRowToModel(dt.Rows);

            return list[0];
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public List<model.taoAppUser> DataRowToModel(DataRowCollection rowCollection)
        {
            List<model.taoAppUser> list = new List<model.taoAppUser>();

            foreach (DataRow row in rowCollection)
            {
                model.taoAppUser modelMd = new model.taoAppUser();
                if (row != null)
                {
                    if (row["Id"] != null && row["Id"].ToString() != "")
                    {
                        modelMd.Id = int.Parse(row["Id"].ToString());
                    }
                    if (row["tbUsserId"] != null)
                    {
                        modelMd.tbUsserId = row["tbUsserId"].ToString();
                    }
                    if (row["tbUserNick"] != null)
                    {
                        modelMd.tbUserNick = row["tbUserNick"].ToString();
                    }
                    if (row["accessToken"] != null)
                    {
                        modelMd.accessToken = row["accessToken"].ToString();
                    }
                    if (row["refreshToken"] != null)
                    {
                        modelMd.refreshToken = row["refreshToken"].ToString();
                    }
                    if (row["userId1"] != null && row["userId1"].ToString() != "")
                    {
                        modelMd.userId1 = int.Parse(row["userId1"].ToString());
                    }
                    if (row["userId"] != null && row["userId"].ToString() != "")
                    {
                        modelMd.userId = int.Parse(row["userId"].ToString());
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
    }
}
