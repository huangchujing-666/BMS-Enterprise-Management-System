using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class ItalyErrordal:idal.iItalyError
    {
        model.pbxdatasourceDataContext pbx = new model.pbxdatasourceDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);
        /// <summary>
        /// 根据条件获取异常信息
        /// </summary>
        /// <param name="error">搜索实体</param>
        /// <param name="skip">跳过多少条数据</param>
        /// <param name="take">去多少条数据</param>
        /// <returns></returns>
        public DataTable GetItaly(string ItalyCode,DateTime? createTime,DateTime? createTimeEnd, int skip, int take,out int pageRowsCount)
        {
            try
            {
                DataTable dt = new DataTable();
                var temp = from c in pbx.ItalyUpdateError select c;
                if (!string.IsNullOrWhiteSpace(ItalyCode))
                {
                    temp = temp.Where(a => a.ItalyCode.Contains(ItalyCode));
                }
                if (createTime!=null)
                {
                    temp = temp.Where(a => a.createTime >= createTime);
                }
                if (createTimeEnd != null)
                {
                    temp = temp.Where(a => a.createTime <= createTimeEnd);
                }
                pageRowsCount = temp.ToList().Count;
                temp = temp.OrderByDescending(a=>a.createTime).Skip(skip).Take(take);
                ItalyUpdateError stock = new ItalyUpdateError();
                List<string> list = new List<string>();
                foreach (var s in stock.GetType().GetProperties())
                {
                    DataColumn cl = new DataColumn();
                    cl.ColumnName = s.Name;
                    list.Add(s.Name);
                    // cl.DataType = s.GetType();
                    dt.Columns.Add(cl);
                }
                foreach (ItalyUpdateError i in temp)
                {
                    DataRow row = dt.NewRow();
                    foreach (string s in list)
                    {
                        //给row赋值
                        row[s] = i.GetType().GetProperty(s).GetValue(i);
                    }
                    dt.Rows.Add(row);
                }
                return dt;
            }
            catch (Exception ex)
            {
                pageRowsCount = 0;
                Errorlogdal err = new Errorlogdal();
                err.InsertErrorlog(new errorlog() { operation = 3, errorTime = DateTime.Now, errorSrc = "ItalyErrordal->GetItaly()", ErrorMsg=ex.Source, errorMsgDetails=ex.Message });
                return null;
            }
            
        }
    }
}
