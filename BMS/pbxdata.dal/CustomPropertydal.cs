using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class CustomPropertydal:idal.ICustomProperty
    {
        model.pbxdatasourceDataContext pbx = null;
        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }
        public CustomPropertydal()
        {
          
        }
        /// <summary>
        /// 修改客户属性表
        /// </summary>
        /// <param name="customPro">客户属性实体</param>
        /// <param name="mess"></param>
        /// <returns></returns>
        public bool UpdateCustomProperty(model.CustomProperty customPro, out string mess)
        {
            mess = "";
            try
            {
                pbx = new model.pbxdatasourceDataContext(connectionString);
                pbx.Connection.Open();
                var  temp = from c in pbx.CustomProperty where customPro.CustomId == c.CustomId select c;
                if (temp.ToList().Count == 0)//判断改信息是否还存在
                {
                    foreach (var t in temp)
                    {
                        t.custom = customPro.custom;
                        t.CustomBuyAmount = customPro.CustomBuyAmount;
                        t.CustomBuyCount = customPro.CustomBuyCount;
                        t.CustomId = customPro.CustomId;
                        t.CustomLevel = customPro.CustomLevel;
                        t.CustomLoveBrand = customPro.CustomLoveBrand;
                        t.CustomServiceComment = customPro.CustomServiceComment;
                    }
                }
                else
                {
                    mess = "该信息已经不存在！";
                    return false;
                }
                pbx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                return false;
            }
            finally
            {
                pbx.Connection.Close();
            }
        }
        public bool DeleteCustomProperty(int id, out string mess)
        {
            mess = "";
            try
            {
                pbx = new model.pbxdatasourceDataContext(connectionString);
                pbx.Connection.Open();
                var temp = from c in pbx.CustomProperty where id == c.Id select c;
                if (temp.ToList().Count > 0)
                {
                    pbx.CustomProperty.DeleteAllOnSubmit(temp);
                    pbx.SubmitChanges();
                    return true;
                }
                else
                {
                    mess = "该信息已经不存在！";
                    return false;
                }
                
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                return false;
            }
            finally
            {
                pbx.Connection.Close();
            }
        }
        public void insert()
        {
            pbx = new model.pbxdatasourceDataContext(connectionString);
            pbx.Connection.Open();
            for (int i = 0; i < 1000; i++)
            {
                model.CustomProperty customPro = new model.CustomProperty();
                Random ran=new Random();
                customPro.CustomId=ran.Next(19,22);
                customPro.CustomLevel=ran.Next(0,5).ToString();
                customPro.CustomBuyCount=ran.Next(1000);
                customPro.CustomBuyAmount=ran.Next(1000000);
                customPro.CustomLoveBrand="阿道夫"+i+"打"+i;
                customPro.CustomServiceComment="好︿(￣︶￣)︿︿(￣︶￣)︿";
                pbx.CustomProperty.InsertOnSubmit(customPro);
            }
            pbx.SubmitChanges();
        }
        public bool InsertCustomProperty(model.CustomProperty customPro, out string mess)
        {
            mess = "";
            try
            {
                pbx = new model.pbxdatasourceDataContext(connectionString);
                pbx.Connection.Open();
                var temp = from c in pbx.CustomProperty where customPro.CustomId == c.CustomId select c;
                if (temp.ToList().Count > 0)
                {
                    foreach (model.CustomProperty cus in temp)
                    {
                        cus.custom = customPro.custom;
                        cus.CustomBuyAmount = customPro.CustomBuyAmount;
                        cus.CustomBuyCount = customPro.CustomBuyCount;
                        cus.CustomId = customPro.CustomId;
                        cus.CustomLevel = customPro.CustomLevel;
                        cus.CustomLoveBrand = customPro.CustomLoveBrand;
                        cus.CustomServiceComment = customPro.CustomServiceComment;
                    }
                }
                else
                {
                    pbx.CustomProperty.InsertOnSubmit(customPro);
                }
                pbx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                return false;
            }
            finally
            {
                pbx.Connection.Close();
            }   

        }
        public List<model.CustomProperty> GetCustomPropertyList(int skip, int take, out int listCount)
        {
            try
            {
                pbx = new model.pbxdatasourceDataContext(connectionString);
                pbx.Connection.Open();
                var customPro = (from c in pbx.CustomProperty select c).ToList();
                List<model.CustomProperty> list = new List<model.CustomProperty>();
                listCount = customPro.Count;
                list = customPro.Skip(skip).Take(take).ToList();
                pbx.Connection.Close();

                return list;
            }
            catch (Exception ex)
            {
                listCount = 0;
                return null;
            }
        }
        public List<model.CustomProperty> GetCustomPropertyList(Dictionary<string, string> dic, int skip, int take, out int listCount)
        {
            try
            {
                pbx.Connection.Open();
                List<model.CustomProperty> list = new List<model.CustomProperty>();
                list = (from c in pbx.CustomProperty select c).ToList();
                /**
                 * CustomName
                 * CustomLevel
                 * CustomBuyCount1
                 * CustomBuyCount2
                 * CustomBuyAmount1
                 * CustomBuyAmount2
                 * CustomLoveBrand
                 * **/
                if (!string.IsNullOrWhiteSpace(dic["CustomName"]))
                {
                    list = list.Where(a => a.custom.customName.Contains(dic["CustomName"])).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dic["CustomLevel"]))
                {
                    list = list.Where(a => a.CustomLevel == dic["CustomLevel"]).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dic["CustomBuyCount1"]))
                {
                    list = list.Where(a => a.CustomBuyCount >= int.Parse(dic["CustomBuyCount1"])).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dic["CustomBuyCount2"]))
                {
                    list = list.Where(a => a.CustomBuyCount <= int.Parse(dic["CustomBuyCount2"])).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dic["CustomBuyAmount1"]))
                {
                    list = list.Where(a => a.CustomBuyCount >= int.Parse(dic["CustomBuyAmount1"])).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dic["CustomBuyAmount2"]))
                {
                    list = list.Where(a =>
                    {
                        if (a.CustomBuyCount == null)
                            return false;
                        else
                            return a.CustomBuyCount <= int.Parse(dic["CustomBuyAmount2"]);
                    }).ToList();
                }
                if (!string.IsNullOrWhiteSpace(dic["CustomLoveBrand"]))
                {
                    list = list.Where(a => a.CustomLoveBrand.Contains(dic["CustomLoveBrand"])).ToList();
                }
                listCount = list.Count;
                pbx.Connection.Close();
                return list.Skip(skip).Take(take).ToList();   
            }
            catch (Exception ex)
            {
                listCount = 0;
                return null;
            }
           
        }
    }
}
