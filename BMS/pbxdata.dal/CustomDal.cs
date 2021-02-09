using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
using Maticsoft.DBUtility;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data;


namespace pbxdata.dal
{
    public class CustomDal : pbxdata.idal.ICustom
    {
        //数据库连接字符串(web.config来配置)，多数据库可使用DbHelperSQLP来实现.

        public string connectionString
        {
            get { return PubConstant.ConnectionString; }
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(Dictionary<string, string> Dic, int page, int Selpages, out string counts)
        {
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            DataTable dt = new DataTable();
            var q = from c in context.custom select c;
            if (Dic["CustomerId"] != "")//客户Id
            {
                q = q.Where(a => a.CustomerId == Dic["CustomerId"]);
            }
            if (Dic["Shop"] != "")//客户来源 
            {
                q = q.Where(a => a.Shop == Dic["Shop"]);
            }
            if (Dic["Sex"] != "")//性别
            {
                q = q.Where(a => a.Sex == Convert.ToInt32(Dic["Sex"]));
            }
            if (Dic["CustomerLevel"] != "")//客户等级
            {
                q = q.Where(a => a.CustomerLevel == Dic["CustomerLevel"]);
            }
            counts = q.Count().ToString();//返回查询数量

            dt = LinqToDataTable.LINQToDataTable(q.Skip((page - 1) * Selpages).Take(Selpages));//翻页
            return dt;
        }
        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(string Id)
        {
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            DataTable dt = new DataTable();
            var q = context.custom.Where(a => a.Id == Convert.ToInt32(Id));//通过id查询客户信息
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <returns></returns>
        public string AddCustomer(Dictionary<string, string> Dic, int UserId)
        {
            string s = string.Empty;
            try
            {
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                int check = context.custom.Where(a => a.CustomerId == Dic["CustomerId"]).Count();
                int? item = null;
                if (check > 0)
                {
                    s = "该客户信息已存在!";
                }
                else
                {
                    model.custom mc = new model.custom()
                    {
                        CustomerId = Dic["CustomerId"],//客户Id
                        Shop = Dic["Shop"],//来源
                        Contactperson = Dic["Contactperson"],//联系人
                        Sex = Dic["Sex"] == "" ? item : Convert.ToInt32(Dic["Sex"]),//性别
                        Age = Dic["Age"] == "" ? item : Convert.ToInt32(Dic["Age"]),//年龄
                        Birthday = Dic["Birthday"],//出生日期
                        IDNumber = Dic["IDNumber"],//身份证
                        Telephone = Dic["Telephone"],//手机
                        Phone = Dic["Phone"],//电话
                        Weixin = Dic["Weixin"],//微信
                        QQNo = Dic["QQNo"],//QQ
                        CustomerLevel = Dic["CustomerLevel"],//客户等级
                        Remark = Dic["Remark"],//备注
                        CustomerServiceId = UserId,//处理客服
                    };
                    context.custom.InsertOnSubmit(mc);
                    context.SubmitChanges();
                    s = "添加客户信息成功!";
                }
            }
            catch (Exception ex)
            {
                s = "添加失败!";
            }

            return s;
        }
        /// <summary>
        /// 保存编辑客户信息
        /// </summary>
        /// <returns></returns>
        public string SaveEdit(Dictionary<string, string> Dic, int UserId)
        {
            string s = string.Empty;
            int? item = null;
            try
            {
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.custom.Where(a => a.Id == Convert.ToInt32(Dic["Id"]));
                foreach (var i in q)
                {
                    //i.CustomerId = Dic["CustomerId"];
                    i.Shop = Dic["Shop"];
                    i.Contactperson = Dic["Contactperson"];
                    i.Sex = Dic["Sex"] == "" ? item : Convert.ToInt32(Dic["Sex"]);
                    i.Age = Dic["Age"] == "" ? item : Convert.ToInt32(Dic["Age"]);
                    i.Birthday = Dic["Birthday"];
                    i.IDNumber = Dic["IDNumber"];
                    i.Telephone = Dic["Telephone"];
                    i.Phone = Dic["Phone"];
                    i.Weixin = Dic["Weixin"];
                    i.QQNo = Dic["QQNo"];
                    i.CustomerLevel = Dic["CustomerLevel"];
                    i.Remark = Dic["Remark"];
                    i.CustomerServiceId = UserId;
                }
                context.SubmitChanges();
                s = "修改成功!";
            }
            catch (Exception ex)
            {
                s = "修改失败!";
            }
            return s;
        }

        /// <summary>
        /// 显示客户地址信息
        /// </summary>
        /// <returns></returns>
        public DataTable CustomerAddress(string CustomerId)
        {
            pbxdatasourceDataContext context = new pbxdatasourceDataContext();
            DataTable dt = new DataTable();
            var q = context.customAddress.Where(a => a.CustomerId == CustomerId);
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;

        }

        /// <summary>
        /// 添加客户地址
        /// </summary>
        /// <returns></returns>
        public string SaveAddAddress(Dictionary<string, string> Dic, int UserId)
        {
            string s = string.Empty;
            try
            {
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                model.customAddress mc = new model.customAddress()
                {
                    CustomerId = Dic["CustomerId"],//客户Id
                    Provinces = Dic["Provinces"],//所在省
                    City = Dic["City"],//所在城市
                    District = Dic["District"],//所在地区
                    CusAddress = Dic["CusAddress"],//具体地址
                    CustomerServiceId = UserId,//处理客服
                    UserId = UserId,//操作人
                };
                context.customAddress.InsertOnSubmit(mc);
                context.SubmitChanges();
                s = "添加成功!";
            }
            catch (Exception ex)
            {
                s = "添加失败!";
            }
            return s;
        }

        /// <summary>
        /// 编辑客户地址
        /// </summary>
        /// <returns></returns>
        public string SaveEditAddress(Dictionary<string, string> Dic, int UserId)
        {
            string s = string.Empty;
            try
            {
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.customAddress.Where(a => a.Id == Convert.ToInt32(Dic["Id"]));
                foreach (var i in q)
                {
                    i.Provinces = Dic["Provinces"];
                    i.City = Dic["City"];
                    i.District = Dic["District"];
                    i.CusAddress = Dic["CusAddress"];
                    i.CustomerServiceId = UserId;
                    i.UserId = UserId;
                }
                context.SubmitChanges();
                s = "保存成功!";
            }
            catch (Exception ex)
            {
                s = "编辑失败!";
            }
            return s;
        }

        /// <summary>
        /// 删除客户地址
        /// </summary>
        /// <returns></returns>
        public string DeleteAddress(string Id, int UserId)
        {
            string s = string.Empty;
            try
            {
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.customAddress.Where(a => a.Id == Convert.ToInt32(Id));//通过Id删除客户地址
                foreach (var i in q)
                {
                    context.customAddress.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                s = "";
            }
            catch (Exception ex)
            {
                s = "删除失败!";
            }
            return s;
        }
        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <returns></returns>
        public string DeleteCustomer(string Id, int UserId)
        {
            string s = string.Empty;
            try
            {
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = context.custom.Where(a => a.Id == Convert.ToInt32(Id));//通过Id删除客户信息
                foreach (var i in q)
                {
                    context.custom.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                s = "";
            }
            catch (Exception ex)
            {
                s = "删除失败!";
            }
            return s;
        }

        /// <summary>
        /// 查看客户订单信息
        /// </summary>
        /// <returns></returns>
        public DataTable CheckOrder(string CustomerId)
        {
            DataTable dt = new DataTable();
            try
            {
                pbxdatasourceDataContext context = new pbxdatasourceDataContext();
                var q = from c in context.ProductInfo
                        join a in context.CustomerInfo on c.OrderId equals a.OrderId
                        into aa
                        from aaa in aa.DefaultIfEmpty()
                        join b in context.product on c.Scode equals b.Scode
                        into bb 
                        from bbb in bb.DefaultIfEmpty()
                        join c in context.brand on bbb.Cat equals c.BrandAbridge
                        into cc
                        from ccc in cc.DefaultIfEmpty()
                        join d in context.producttype on bbb.Cat2 equals d.TypeNo
                        into dd
                        from ddd in dd.DefaultIfEmpty()
                        select new
                        {
                            CustomerId = aaa.CustomerId,//客户Id
                            OrderId = c.OrderId,//订单Id
                            Scode = c.Scode,//货号
                            Brand = bbb.Cat,//品牌缩写
                            BrandName = ccc.BrandName,//品牌名
                            Color = bbb.Clolor,//颜色
                            TypeNo = bbb.Cat2,//类别编号
                            TypeName = ddd.TypeName,//类别名称
                            Size = bbb.Size,//尺寸
                            Imagefile = bbb.Imagefile,//缩略图
                            SellPrice = c.SellPrice,//交易价格
                            Number = c.Number,//交易数量
                            Def1 = c.Def1,//发货状态
                        };
                q = q.Where(a => a.CustomerId == CustomerId);
                dt = LinqToDataTable.LINQToDataTable(q);
            }
            catch (Exception ex)
            {
               
            }
            return dt;
        }

    }
}
