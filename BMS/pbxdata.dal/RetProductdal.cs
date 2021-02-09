using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class RetProductdal : dataoperating, iRetProduct
    {
        /// <summary>
        /// 返回查询交易信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();


            var q = from c in context.TradeInfo
                    join s in context.CustomerInfo on c.OrderId equals s.OrderId
                    into tt
                    from t in tt.DefaultIfEmpty()
                    join u in context.users on Convert.ToInt32(c.ServiceId) equals u.Id
                    into ut
                    from tu in ut.DefaultIfEmpty()
                    select new
                        {
                            Id = c.Id,
                            OrderId = c.OrderId,                    //订单编号
                            //SystemId = c.SystemId,                  //系统单号
                            SellPrice = c.SellPrice,                //成交总金额
                            SaleStates = c.SaleStates,              //交易是否成功
                            Evaluate = c.Evaluate,                  //是否评价
                            ServiceId = c.ServiceId,                //接待客服Id
                            ServiceName = tu.userRealName,                //接待客服名字
                            ServiceRemark = c.ServiceRemark,        //客服备注
                            OrderTime = c.OrderTime,                //落单日期

                            CustomerId = t.CustomerId,              //客户ID
                            Shop = t.Shop,                          //店铺/平台
                            Contactperson = t.Contactperson,        //联系人
                            Telephone = t.Telephone,                //电话
                            Phone = t.Phone,                        //手机号
                            Weixin = t.Weixin,                      //微信
                            QQNo = t.QQNo,                          //QQ
                            Provinces = t.Provinces,                //省
                            City = t.City,                          //市
                            CusAddress = t.CusAddress,              //发货地址
                            Payment = t.Payment,                    //支付平台
                            Account = t.Account,                    //支付账号


                        };
            if (Dic["OrderId"] != "")
            {
                q = q.Where(a => a.OrderId == Dic["OrderId"]);
            }
            //if (Dic["SystemId"] != "")
            //{
            //    q = q.Where(a => a.SystemId == Dic["SystemId"]);
            //}
            //if (Dic["Heidui"] != "")
            //{
            //    q = q.Where(a => a.Heidui == Dic["Heidui"]);
            //}
            //if (Dic["ExBalance"] != "")
            //{
            //    q = q.Where(a => a.ExBalance == Dic["ExBalance"]);
            //}
            //if (Dic["SellPrice"] != "")
            //{
            //    q = q.Where(a => a.SellPrice == Dic["SellPrice"]);
            //}
            //if (Dic["Express"] != "")
            //{
            //    q = q.Where(a => a.Express == Dic["Express"]);
            //}
            if (Dic["ServiceId"] != "")
            {
                q = q.Where(a => a.ServiceId == Dic["ServiceId"]);
            }
            if (Dic["SaleStates"] != "")
            {
                q = q.Where(a => a.SaleStates == Dic["SaleStates"]);
            }
            if (Dic["OrderTime"] != "")
            {
                if (Dic["OrderTime1"] != "")
                {
                    q = q.Where(a => a.OrderTime >= Convert.ToDateTime(Dic["OrderTime"]) && a.OrderTime <= Convert.ToDateTime(Dic["OrderTime1"]));
                }
                else
                {
                    q = q.Where(a => a.OrderTime >= Convert.ToDateTime(Dic["OrderTime"]));
                }
            }
            else
            {
                if (Dic["OrderTime1"] != "")
                {
                    q = q.Where(a => a.OrderTime <= Convert.ToDateTime(Dic["OrderTime1"]));
                }
            }
            if (Dic["CustomerId"] != "")
            {
                q = q.Where(a => a.CustomerId == Dic["CustomerId"]);
            }
            if (Dic["Shop"] != "")
            {
                q = q.Where(a => a.Shop == Dic["Shop"]);
            }
            //q = q.Where(a => a.OrderId == "0");

            q = q.OrderByDescending(a => a.Id);
            DataTable dt1 = dt = LinqToDataTable.LINQToDataTable(q);
            decimal sum = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt1.Rows[i]["SellPrice"].ToString() != null && dt1.Rows[i]["SellPrice"].ToString() != "")
                {
                    sum += Convert.ToDecimal(dt.Rows[i]["SellPrice"]);
                }
            }
            if (pageIndex == 0)
            {
                dt = LinqToDataTable.LINQToDataTable(q.Take(pageSize));
            }
            else
            {
                dt = LinqToDataTable.LINQToDataTable(q.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            }
           
            counts = q.ToList().Count.ToString() + "-" + sum.ToString("f2");
            return dt;
        }

        /// <summary>
        /// 返回查询交易信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(string OrderId)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.TradeInfo
                    join s in context.CustomerInfo on c.OrderId equals s.OrderId
                    into tt
                    from t in tt.DefaultIfEmpty()
                    join u in context.users on Convert.ToInt32(c.ServiceId) equals u.Id
                    into ut
                    from tu in ut.DefaultIfEmpty()
                    select new
                    {
                        Id = c.Id,
                        OrderId = c.OrderId,                    //订单编号
                        //SystemId = c.SystemId,                  //系统单号
                        SellPrice = c.SellPrice,                //成交总金额
                        SaleStates = c.SaleStates,              //交易是否成功
                        Evaluate = c.Evaluate,                  //是否评价
                        ServiceId = c.ServiceId,                //接待客服Id
                        ServiceName = tu.userRealName,                //接待客服名字
                        ServiceRemark = c.ServiceRemark,        //客服备注
                        OrderTime = c.OrderTime,                //落单日期

                        CustomerId = t.CustomerId,              //客户ID
                        Shop = t.Shop,                          //店铺/平台
                        Contactperson = t.Contactperson,        //联系人
                        Telephone = t.Telephone,                //电话
                        Phone = t.Phone,                        //手机号
                        Weixin = t.Weixin,                      //微信
                        QQNo = t.QQNo,                          //QQ
                        Provinces = t.Provinces,                //省
                        City = t.City,                          //市
                        CusAddress = t.CusAddress,              //发货地址
                        Payment = t.Payment,                    //支付平台
                        Account = t.Account,                    //支付账号
                    };
            q = q.Where(a => a.OrderId == OrderId);
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }

        /// <summary>
        /// 添加交易信息
        /// </summary>
        /// <returns></returns>
        public string AddTradeInfo(Dictionary<string, string> Dic, int UserId)
        {
            string s = "";
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.TradeInfo where c.OrderId == Dic["OrderId"] select c;
            var q1 = from c in context.CustomerInfo where c.OrderId == Dic["OrderId"] select c;
            if (q.ToList().Count > 0)
            {
                s = "交易信息表订单已存在!";
            }
            else
            {
                if (q1.ToList().Count > 0)
                {
                    s = "客户信息表订单已存在!";
                }
                else
                {
                    model.TradeInfo t = new model.TradeInfo()
                    {
                        OrderId = Dic["OrderId"],//订单编号
                        //SystemId = Dic["SystemId"],
                        SaleStates = "1",//交易是否成功 1.已付款 2.未付款 3.交易成功 4.交易关闭
                        Evaluate = Dic["Evaluate"],//是否评价
                        ServiceId = UserId.ToString(),//客服id
                        ServiceRemark = Dic["ServiceRemark"],//客服备注
                        OrderTime = Convert.ToDateTime(Dic["OrderTime"]),//下单日期
                        UserId = UserId,//操作人
                    };
                    context.TradeInfo.InsertOnSubmit(t);
                    context.SubmitChanges();

                    model.CustomerInfo tt = new model.CustomerInfo()
                    {
                        OrderId = Dic["OrderId"],//订单编号
                        CustomerId = Dic["CustomerId"],//客户Id
                        Shop = Dic["Shop"],//店铺/平台
                        Contactperson = Dic["Contactperson"],//联系人
                        Telephone = Dic["Telephone"],//电话
                        Phone = Dic["Phone"],//手机号
                        Weixin = Dic["Weixin"],//微信
                        QQNo = Dic["QQNo"],//QQ
                        Provinces = Dic["Provinces"],//省
                        City = Dic["City"],//城市
                        CusAddress = Dic["CusAddress"],//地址
                        Payment = Dic["Payment"],//支付平台
                        Account = Dic["Account"],//支付账号
                    };
                    context.CustomerInfo.InsertOnSubmit(tt);
                    context.SubmitChanges();
                    s = "添加成功!";
                    OperaRecord(new model.OperationRecord()
                    {
                        OrderId = Dic["OrderId"],
                        OperaTable = "交易信息",
                        OperaType = "添加",
                        UserId = UserId,
                    });
                }
            }
            return s;
        }

        /// <summary>
        /// 编辑交易信息
        /// </summary>
        /// <returns></returns>
        public string SaveTradeEdit(Dictionary<string, string> Dic, int UserId)
        {
            string s = "";
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = from c in context.TradeInfo where c.OrderId == Dic["OrderId"] select c;//交易信息
                var q1 = from c in context.CustomerInfo where c.OrderId == Dic["OrderId"] select c;//交易客户信息
                if (Dic["Shop"] == "app")
                {
                    foreach (var i in q)
                    {
                        //i.SystemId = Dic["SystemId"];
                        //i.SaleStates = "1";
                        i.ServiceId = UserId.ToString();//客服Id
                        i.ServiceRemark = Dic["ServiceRemark"];//客服备注
                        i.UserId = UserId;//操作人
                    }
                    foreach (var i in q1)
                    {
                        i.CustomerId = Dic["CustomerId"];//客户Id
                        i.Phone = Dic["Phone"];//手机
                        i.Weixin = Dic["Weixin"];//
                        i.QQNo = Dic["QQNo"];

                    }
                }
                else
                {
                    foreach (var i in q)
                    {
                        //i.SystemId = Dic["SystemId"];
                        //i.SaleStates = "1";
                        i.Evaluate = Dic["Evaluate"];
                        i.ServiceId = UserId.ToString();
                        i.ServiceRemark = Dic["ServiceRemark"];
                        i.OrderTime = Convert.ToDateTime(Dic["OrderTime"]);
                        i.UserId = UserId;
                    }
                    foreach (var i in q1)
                    {
                        i.CustomerId = Dic["CustomerId"];
                        i.Shop = Dic["Shop"];
                        i.Contactperson = Dic["Contactperson"];
                        i.Telephone = Dic["Telephone"];
                        i.Phone = Dic["Phone"];
                        i.Weixin = Dic["Weixin"];
                        i.QQNo = Dic["QQNo"];
                        i.Provinces = Dic["Provinces"];
                        i.City = Dic["City"];
                        i.CusAddress = Dic["CusAddress"];
                        i.Payment = Dic["Payment"];
                        i.Account = Dic["Account"];
                    }
                }

                context.SubmitChanges();
                s = "修改成功!";
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "交易信息",
                    OperaType = "编辑",
                    UserId = UserId,
                });
            }
            catch (Exception ex)
            {
                s = "修改失败!";
            }

            return s;
        }

        /// <summary>
        /// 删除交易信息
        /// </summary>通过订单编号删除交易信息
        /// <returns></returns>
        public string DeleteTrade(string OrderId, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = from c in context.TradeInfo where c.OrderId == OrderId select c;
                foreach (var i in q)
                {
                    context.TradeInfo.DeleteOnSubmit(i);
                }
                var q1 = from c in context.CustomerInfo where c.OrderId == OrderId select c;
                foreach (var i in q1)
                {
                    context.CustomerInfo.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = OrderId,
                    OperaTable = "交易信息",
                    OperaType = "删除",
                    UserId = UserId,
                });
                return "";

            }
            catch (Exception ex)
            {
                return "删除失败!";
            }

        }

        /// <summary>
        /// 添加退货\退款信息
        /// </summary>
        /// <returns></returns>
        public string ReturnGoods(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                //var check = context.RetProductInfo.Where(a => a.OrderId == Dic["OrderId"]&&a.Def2==Dic["Scode"]);
                //if (check.Count() > 0)
                //{
                //    var check2 = context.ProductInfo.Where(a => a.OrderId == Dic["OrderId"] && a.Scode == Dic["Scode"]).FirstOrDefault().Def1.ToString();
                //    if (check2 == "5" || check2 == "7")
                //    {
                //        var item = check.FirstOrDefault().RetType.ToString() == "5" ? "申请退货" : "申请退款";
                //        return "该商品已有" + item + "信息!";
                //    }

                //}

                model.RetProductInfo rp = new model.RetProductInfo()
                {
                    OrderId = Dic["OrderId"],//订单编号
                    RetPrice = Dic["RetPrice"],//退款金额
                    //Express = Dic["Express"],
                    //ExpressNo = Dic["ExpressNo"],
                    RetDetails = Dic["RetDetails"],//退货说明
                    RetType = Dic["RetType"],//退换货类型 1.退款 2.退货 5.退货(未发货)
                    //Receiver = Dic["Receiver"],
                    RetAccount = Dic["RetAccount"],//数量
                    ServiceId = UserId.ToString(),//处理人
                    UserId = UserId,//操作人
                    Def1 = Dic["RetType"],//退货状态 1.申请退款 2.申请退货3.退款成功 4.退货成功 5.申请退货(未发货) 6.退货已收
                    Def2 = Dic["Scode"],//货品编号
                    Def3 = Dic["Reason"],//退货理由
                };
                context.RetProductInfo.InsertOnSubmit(rp);
                var q = context.ProductInfo.Where(a => a.OrderId == Dic["OrderId"] && a.Scode == Dic["Scode"]);
                if (Dic["RetType"].ToString() == "1")//1.退款 2.退货  通过退货理由判断交易商品信息的状态
                {
                    foreach (var i in q)
                    {
                        i.Def1 = "5";//发货状态1.未发货 2.已发货  3.退货中  4.退货完成  5.退款中  6.退款完成   7.交易成功
                    }
                }
                else if (Dic["RetType"].ToString() == "2" || Dic["RetType"].ToString() == "5")
                {
                    foreach (var i in q)
                    {
                        i.Def1 = "3";
                    }

                }
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "退货信息",
                    OperaType = Dic["RetType"].ToString() == "1" ? "添加退款" : Dic["RetType"].ToString() == "2" ? "添加退货" : "",
                    UserId = UserId,
                });
                context.SubmitChanges();

                return "添加成功!";
            }
            catch (Exception ex)
            {
                return "添加失败!";
            }
        }

        /// <summary>
        /// 添加发货信息
        /// </summary>
        /// <returns></returns>
        public string ExchangeGoods(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var check = context.ShipmentRecord.Where(a => a.OrderId == Dic["OrderId"] && a.Def2 == Dic["Scode"] && a.SendType == "1");
                if (check.Count() > 0)
                {
                    return "该订商品已有发货信息!";
                }
                model.ShipmentRecord sr = new model.ShipmentRecord()
                {
                    OrderId = Dic["OrderId"],//订单编号
                    ExPrice = Dic["ExPrice"],//换货金额
                    SendTime = Convert.ToDateTime(Dic["SendTime"]),//发货时间
                    Express = Dic["Express"],//快递公司
                    ExpressNo = Dic["ExpressNo"],//快递单号
                    YFHKD = Dic["YFHKD"],//运费HKD
                    YFRMB = Dic["YFRMB"],//运费RMB
                    RetRemark = Dic["RetRemark"],//退货说明
                    SendPerson = Dic["SendPerson"],//发货人
                    SendType = Dic["SendType"],//出货类型
                    SendStatus = Dic["SendType"],//发货状态
                    Def2 = Dic["Scode"],//货品编号
                    UserId = UserId,//操作人
                };
                context.ShipmentRecord.InsertOnSubmit(sr);
                var q = context.ProductInfo.Where(a => a.OrderId == Dic["OrderId"] && a.Scode == Dic["Scode"]);
                if (Dic["SendType"].ToString() == "1")//1.新订单 2.订单关闭
                {
                    foreach (var i in q)
                    {
                        i.Def1 = "2";
                    }
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "出货记录",
                    OperaType = "添加",
                    UserId = UserId,
                });
                return "添加成功!";
            }
            catch (Exception ex)
            {
                return "添加失败!";
            }

        }


        /// <summary>
        /// 取消状态
        /// </summary> 
        /// <returns></returns>
        public string CancelStatus(string OrderId, string Scode, string type, int UserId)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                var q = context.ProductInfo.Where(a => a.OrderId == OrderId && a.Scode == Scode);//改变商品信息的状态
                var q1 = context.RetProductInfo.Where(a => a.OrderId == OrderId && a.Def2 == Scode);//删除退货信息
                foreach (var i in q1)
                {
                    context.RetProductInfo.DeleteOnSubmit(i);
                }

                context.SubmitChanges();
                if (context.ShipmentRecord.Where(a => a.OrderId == OrderId && a.Def2 == Scode && a.SendStatus == "1").Count() == 0)//通过出货记录中的发货状态是否已发货来判断商品信息状态
                {
                    foreach (var i in q)//未发货
                    {
                        i.Def1 = "1";
                    }
                    context.SubmitChanges();
                }
                else
                {
                    foreach (var i in q)//已发货
                    {
                        i.Def1 = "2";
                    }
                    context.SubmitChanges();
                }
                s = "取消成功!";
                //if (type == "3")
                //{
                //    var q1 = context.RetProductInfo.Where(a => a.OrderId == OrderId && a.Def2 == Scode);

                //    foreach (var i in q1)
                //    {
                //        context.RetProductInfo.DeleteOnSubmit(i);
                //    }
                //    context.SubmitChanges();
                //    foreach (var i in q)
                //    {
                //        i.Def1 = "2";
                //    }
                //    context.SubmitChanges();
                //    s = "取消成功!";
                //}
                //else if (type == "4")
                //{
                //    var q1 = context.RetProductInfo.Where(a => a.OrderId == OrderId && a.Def2 == Scode);
                //    foreach (var i in q1)
                //    {
                //        context.RetProductInfo.DeleteOnSubmit(i);
                //    }

                //    context.SubmitChanges();
                //    if (context.ShipmentRecord.Where(a => a.OrderId == OrderId && a.Def2 == Scode && a.SendStatus == "1").Count() == 0)
                //    {
                //        foreach (var i in q)
                //        {
                //            i.Def1 = "1";
                //        }
                //        context.SubmitChanges();
                //    }
                //    else
                //    {
                //        foreach (var i in q)
                //        {
                //            i.Def1 = "2";
                //        }
                //        context.SubmitChanges();
                //    }
                //    s = "取消成功!";
                //}
                //else if (type == "6")
                //{
                //    foreach (var i in q)
                //    {
                //        i.Def1 = "3";
                //    }
                //    context.SubmitChanges();
                //    s = "";
                //}
                //else
                //{
                //    s = "取消失败!";
                //}
            }
            catch (Exception ex)
            {
                s = "取消失败!";
            }

            return s;
        }
        /// <summary>
        /// 返回查询商品信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDateProInfo(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.ProductInfo
                    join p in context.product on c.Scode equals p.Scode
                    into pt
                    from pp in pt.DefaultIfEmpty()
                    join s in context.producttype on pp.Cat2 equals s.TypeNo
                    into tt
                    from t in tt.DefaultIfEmpty()
                    select new
                    {
                        Id = c.Id,
                        OrderId = c.OrderId,
                        Scode = c.Scode,
                        //Brand = c.Brand,
                        //Color = c.Color,
                        //Imagefile = c.Imagefile,
                        //Size = c.Size,
                        TypeNo = t.TypeName,

                        Number = c.Number,//数量
                        ProDetails = c.ProDetails,//货品描述
                        ProLink = c.ProLink,//商品链接
                        DeliveryAttri = c.DeliveryAttri,//发货属性
                        LastOrderId = c.LastOrderId,//上次订单号
                        SellPrice = c.SellPrice,//成交金额
                        Warehouse = c.Warehouse,//出货仓
                        Def1 = c.Def1,//发货状态
                        Def2 = c.Def2,//系统单号
                        Def3 = c.Def3,//最后操作时间
                        Def4 = c.Def4,
                        Def5 = c.Def5,

                        Brand = pp.Cat,
                        Color = pp.Clolor,
                        Imagefile = pp.Imagefile,
                        Size = pp.Size,

                        TypeNo1 = c.TypeNo,
                    };
            if (Dic["OrderId"] != "")
            {
                q = q.Where(a => a.OrderId == Dic["OrderId"]);
            }
            if (Dic["Scode"] != "")
            {
                q = q.Where(a => a.Scode == Dic["Scode"]);
            }
            if (Dic["Brand"] != "")
            {
                q = q.Where(a => a.Brand == Dic["Brand"]);
            }
            if (Dic["Color"] != "")
            {
                q = q.Where(a => a.Color == Dic["Color"]);
            }
            if (Dic["TypeNo"] != "")
            {
                q = q.Where(a => a.TypeNo1 == Dic["TypeNo"]);
            }
            if (Dic["Size"] != "")
            {
                q = q.Where(a => a.Size == Dic["Size"]);
            }
            if (Dic["Def1"] != "")
            {
                q = q.Where(a => a.Def1 == Dic["Def1"]);
            }
            //if (Dic["LastOrderId"] != "")
            //{
            //    q = q.Where(a => a.LastOrderId == Dic["LastOrderId"]);
            //}
            //if (Dic["Warehouse"] != "")
            //{
            //    q = q.Where(a => a.Warehouse == Dic["Warehouse"]);
            //}
            counts = q.ToList().Count;
            q = q.OrderBy(a => a.OrderId);
            if (pageIndex == 0)
            {
                dt = LinqToDataTable.LINQToDataTable(q.Take(pageSize));
            }
            else
            {
                dt = LinqToDataTable.LINQToDataTable(q.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            }
            return dt;
        }

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <returns></returns>
        public string AddProductInfo(Dictionary<string, string> Dic, int UserId)
        {
            string s = "";
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q1 = context.product.Where(a => a.Scode == Dic["Scode"]).SingleOrDefault();
                model.ProductInfo mp = new model.ProductInfo()
                {
                    OrderId = Dic["OrderId"],
                    Scode = Dic["Scode"],
                    //Brand = q1.Cat,
                    //Color = q1.Clolor,
                    //TypeNo = q1.Cat2,
                    //Imagefile = q1.Imagefile,
                    //Size = q1.Size,
                    Number = Dic["Number"],
                    ProDetails = Dic["ProDetails"],
                    ProLink = Dic["ProLink"],
                    DeliveryAttri = Dic["DeliveryAttri"],
                    LastOrderId = Dic["LastOrderId"],
                    SellPrice = Dic["SellPrice"],
                    Warehouse = Dic["Warehouse"],
                    Def1 = "1",//新添加的商品信息默认为未发货
                    Def2 = Dic["SystemId"],//系统单号
                };
                context.ProductInfo.InsertOnSubmit(mp);
                context.SubmitChanges();
                s = "添加成功!";

                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "交易信息",
                    OperaType = "添加",
                    UserId = UserId,
                });
            }
            catch (Exception ex)
            {
                s = "添加失败!";
            }
            return s;
        }

        /// <summary>
        /// 查询编辑商品信息
        /// </summary>
        /// <returns></returns>
        public DataTable EditProductInfo(string Id)
        {

            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            //var q = context.ProductInfo.Where(a => a.Id == Convert.ToInt32(Id));
            var q = from c in context.ProductInfo
                    join p in context.product on c.Scode equals p.Scode
                    into pt
                    from pp in pt.DefaultIfEmpty()
                    join s in context.producttype on pp.Cat2 equals s.TypeNo
                    into tt
                    from t in tt.DefaultIfEmpty()
                    where c.Id == Convert.ToInt32(Id)
                    select new
                    {
                        Id = c.Id,
                        OrderId = c.OrderId,
                        Scode = c.Scode,
                        Number = c.Number,
                        ProDetails = c.ProDetails,
                        ProLink = c.ProLink,
                        DeliveryAttri = c.DeliveryAttri,
                        LastOrderId = c.LastOrderId,
                        SellPrice = c.SellPrice,
                        Warehouse = c.Warehouse,
                        TypeNo1 = c.TypeNo,
                        Def2 = c.Def2,//系统单号

                        Brand = pp.Cat,
                        Color = pp.Clolor,
                        Imagefile = pp.Imagefile,
                        Size = pp.Size,

                        TypeNo = t.TypeName,
                    };
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;


        }

        /// <summary>
        /// 保存编辑商品信息
        /// </summary>
        /// <returns></returns>
        public string SaveProductInfoEdit(Dictionary<string, string> Dic, int UserId)
        {
            string s = "";
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.ProductInfo.Where(a => a.Id == Convert.ToInt32(Dic["Id"]));
                var q1 = context.product.Where(a => a.Scode == Dic["Scode"]).SingleOrDefault();
                foreach (var i in q)
                {
                    i.OrderId = Dic["OrderId"];
                    i.Scode = Dic["Scode"];
                    i.Brand = q1.Cat;
                    i.Color = q1.Clolor;
                    i.TypeNo = q1.Cat2;
                    i.Imagefile = q1.Imagefile;
                    i.Size = q1.Size;
                    i.Number = Dic["Number"];
                    i.ProDetails = Dic["ProDetails"];
                    i.ProLink = Dic["ProLink"];
                    i.DeliveryAttri = Dic["DeliveryAttri"];
                    i.LastOrderId = Dic["LastOrderId"];
                    i.SellPrice = Dic["SellPrice"];
                    i.Warehouse = Dic["Warehouse"];
                    i.Def2 = Dic["SystemId"];
                }
                context.SubmitChanges();
                s = "修改成功!";
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "交易信息",
                    OperaType = "编辑",
                    UserId = UserId,
                });
            }
            catch (Exception ex)
            {
                s = "修改失败!";
            }
            return s;
        }

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <returns></returns>
        public string DeleteProductInfo(string Id, int UserId)
        {
            string s = "";
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.ProductInfo.Where(a => a.Id == Convert.ToInt32(Id));//通过Id删除商品信息
                string OrderId = q.SingleOrDefault().OrderId.ToString();
                string Scode = q.SingleOrDefault().Scode.ToString();
                foreach (var i in q)
                {
                    context.ProductInfo.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = OrderId + "->" + Scode,
                    OperaTable = "交易信息",
                    OperaType = "删除",
                    UserId = UserId,
                });
            }
            catch (Exception ex)
            {
            }

            return s;
        }

        /// <summary>
        /// 返回退货信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate1(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.RetProductInfo select c;
            //join t in context.TradeInfo on c.OrderId equals t.OrderId
            //into s
            //from g in s.DefaultIfEmpty()
            //select new
            //{
            //    Id = c.Id,
            //    OrderId = c.OrderId,
            //    RetPrice = c.RetPrice,
            //    Express = c.Express,
            //    ExpressNo = c.ExpressNo,
            //    RetDetails = c.RetDetails,
            //    RetType = c.RetType,
            //    Receiver = c.Receiver,
            //    ServiceId = c.ServiceId,
            //    RetAccount = c.RetAccount,

            //    SaleStates = g.SaleStates,
            //};
            if (Dic["OrderId"] != "")
            {
                q = q.Where(a => a.OrderId == Dic["OrderId"]);
            }
            if (Dic["RetPrice"] != "")
            {
                q = q.Where(a => a.RetPrice == Dic["RetPrice"]);
            }
            //if (Dic["QuaLevel"] != "")
            //{
            //    q = q.Where(a => a.QuaLevel == Dic["QuaLevel"]);
            //}
            //if (Dic["RetTime"] != "")
            //{
            //    if (Dic["RetTime1"] != "")
            //    {
            //        q = q.Where(a => a.RetTime >= Convert.ToDateTime(Dic["RetTime"]) && a.RetTime <= Convert.ToDateTime(Dic["RetTime1"]));
            //    }
            //    else
            //    {
            //        q = q.Where(a => a.RetTime >= Convert.ToDateTime(Dic["RetTime"]));
            //    }
            //}
            //else
            //{
            //    if (Dic["RetTime1"] != "")
            //    {
            //        q = q.Where(a => a.RetTime <= Convert.ToDateTime(Dic["RetTime1"]));
            //    }
            //}
            if (Dic["Express"] != "")
            {
                q = q.Where(a => a.Express == Dic["Express"]);
            }
            if (Dic["ExpressNo"] != "")
            {
                q = q.Where(a => a.ExpressNo == Dic["ExpressNo"]);
            }
            if (Dic["RetType"] != "")
            {
                q = q.Where(a => a.RetType == Dic["RetType"]);
            }
            if (Dic["Receiver"] != "")
            {
                q = q.Where(a => a.Receiver == Dic["Receiver"]);
            }
            if (Dic["ServiceId"] != "")
            {
                q = q.Where(a => a.ServiceId == Dic["ServiceId"]);
            }
            if (Dic["RetAccount"] != "")
            {
                q = q.Where(a => a.RetAccount == Dic["RetAccount"]);
            }
            if (Dic["Def1"] != "")
            {
                q = q.Where(a => a.Def1 == Dic["Def1"]);
            }
            if (Dic["Def3"] != "")
            {
                q = q.Where(a => a.Def3 == Dic["Def3"]);
            }
            counts = q.ToList().Count;
            dt = LinqToDataTable.LINQToDataTable(q.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            return dt;
        }

        /// <summary>
        /// 返回退货信息
        /// </summary>//通过id查询退货信息--编辑页面
        /// <returns></returns>
        public DataTable GetDate1(string Id)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.RetProductInfo select c;
            q = q.Where(a => a.Id == Convert.ToInt32(Id));
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }

        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <returns></returns>
        public string AddRetProInfo(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.RetProductInfo mr = new model.RetProductInfo()
                {
                    OrderId = Dic["OrderId"],
                    RetPrice = Dic["RetPrice"],
                    //QuaLevel = Dic["QuaLevel"],
                    //RetTime = Convert.ToDateTime(Dic["RetTime"]),
                    Express = Dic["Express"],
                    ExpressNo = Dic["ExpressNo"],
                    RetDetails = Dic["RetDetails"],//退货说明
                    RetType = Dic["RetType"],//退换货类型 1.退款 2.退货 5.退货(未发货)
                    Receiver = Dic["Receiver"],//收货人	
                    ServiceId = Dic["ServiceId"],
                    RetAccount = Dic["RetAccount"],
                    Def1 = Dic["RetType"],//退货状态 1.申请退款 2.申请退货3.退款成功 4.退货成功 5.申请退货(未发货) 6.退货已收
                };
                context.RetProductInfo.InsertOnSubmit(mr);
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "退货信息",
                    OperaType = "添加",
                    UserId = UserId,
                });
                return "添加成功!";

            }
            catch (Exception ex)
            {
                return "添加失败!";
            }
        }

        /// <summary>
        /// 修改退货信息
        /// </summary>
        /// <returns></returns>
        public string SaveEditRetProInfo(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.RetProductInfo.Where(a => a.Id == Convert.ToInt32(Dic["Id"]));
                DateTime RetTime = DateTime.Now;
                if (q.SingleOrDefault().Def1 == "2")//通过判断退货状态是 2.申请退货(已发货) 还是 5.申请退货(未发货) 来确定 退货状态的改变
                {
                    foreach (var i in q)
                    {
                        i.OrderId = Dic["OrderId"];
                        i.RetPrice = Dic["RetPrice"];
                        //i.QuaLevel = Dic["QuaLevel"];
                        //i.RetTime = Convert.ToDateTime(Dic["RetTime"]);
                        i.Express = Dic["Express"];
                        i.ExpressNo = Dic["ExpressNo"];
                        i.RetDetails = Dic["RetDetails"];
                        //i.RetType = Dic["RetType"];
                        i.Receiver = Dic["Receiver"];
                        i.ServiceId = Dic["ServiceId"];
                        i.RetAccount = Dic["RetAccount"];
                        i.Def1 = "6";
                    }
                    model.RetBalance rb = new model.RetBalance()//如果申请退货(已发货) 接收货物时候添加退货库存信息
                    {
                        OrderId = q.SingleOrDefault().OrderId,
                        Scode = q.SingleOrDefault().Def2,
                        Price = q.SingleOrDefault().RetPrice,
                        SellPrice = q.SingleOrDefault().RetPrice,
                        RetTime = RetTime
                    };
                    context.RetBalance.InsertOnSubmit(rb);

                }
                else
                {
                    foreach (var i in q)
                    {
                        i.OrderId = Dic["OrderId"];
                        i.RetPrice = Dic["RetPrice"];
                        //i.QuaLevel = Dic["QuaLevel"];
                        //i.RetTime = Convert.ToDateTime(Dic["RetTime"]);
                        i.Express = Dic["Express"];
                        i.ExpressNo = Dic["ExpressNo"];
                        i.RetDetails = Dic["RetDetails"];
                        //i.RetType = Dic["RetType"];
                        i.Receiver = Dic["Receiver"];
                        i.ServiceId = Dic["ServiceId"];
                        i.RetAccount = Dic["RetAccount"];
                        //i.Def1="";
                    }
                }

                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "退货信息",
                    OperaType = "编辑",
                    UserId = UserId,
                });
                return "修改成功!";

            }
            catch (Exception ex)
            {
                return "修改失败!";
            }
        }

        /// <summary>
        /// 删除退货信息
        /// </summary>通过Id删除退货信息
        /// <returns></returns>
        public string DeleteRetProInfo(string Id, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.RetProductInfo.Where(a => a.Id == Convert.ToInt32(Id));
                string OrderId = q.SingleOrDefault().OrderId.ToString();
                foreach (var i in q)
                {
                    context.RetProductInfo.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = OrderId,
                    OperaTable = "退货信息",
                    OperaType = "删除",
                    UserId = UserId,
                });
                return "";
            }
            catch (Exception ex)
            {
                return "删除失败!";
            }

        }

        /// <summary>
        /// 返回出货记录信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate2(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.ShipmentRecord select c;
            //join t in context.TradeInfo on c.OrderId equals t.OrderId
            //into s
            //from g in s.DefaultIfEmpty()
            //select new
            //{
            //    Id = c.Id,
            //    OrderId = c.OrderId,
            //    ExPrice = c.ExPrice,
            //    SendTime = c.SendTime,
            //    Express = c.Express,
            //    ExpressNo = c.ExpressNo,
            //    YFHKD = c.YFHKD,
            //    YFRMB = c.YFRMB,
            //    RetRemark = c.RetRemark,
            //    SendPerson = c.SendPerson,
            //    SendType = c.SendType,
            //    SendStatus = c.SendStatus,

            //    SaleStates = g.SaleStates,//交易状态
            //};
            if (Dic["OrderId"] != "")
            {
                q = q.Where(a => a.OrderId == Dic["OrderId"]);
            }
            if (Dic["ExPrice"] != "")
            {
                q = q.Where(a => a.ExPrice == Dic["ExPrice"]);
            }
            //if (Dic["QuaLevel"] != "")
            //{
            //    q = q.Where(a => a.QuaLevel == Dic["QuaLevel"]);
            //}
            if (Dic["SendTime"] != "")
            {
                if (Dic["SendTime"] != "")
                {
                    q = q.Where(a => a.SendTime >= Convert.ToDateTime(Dic["SendTime"]) && a.SendTime <= Convert.ToDateTime(Dic["SendTime1"]));
                }
                else
                {
                    q = q.Where(a => a.SendTime >= Convert.ToDateTime(Dic["SendTime"]));
                }
            }
            else
            {
                if (Dic["SendTime1"] != "")
                {
                    q = q.Where(a => a.SendTime <= Convert.ToDateTime(Dic["SendTime1"]));
                }
            }
            if (Dic["Express"] != "")
            {
                q = q.Where(a => a.Express == Dic["Express"]);
            }
            if (Dic["ExpressNo"] != "")
            {
                q = q.Where(a => a.ExpressNo == Dic["ExpressNo"]);
            }
            //if (Dic["YFHKD"] != "")
            //{
            //    q = q.Where(a => a.YFHKD == Dic["YFHKD"]);
            //}
            //if (Dic["YFRMB"] != "")
            //{
            //    q = q.Where(a => a.YFRMB == Dic["YFRMB"]);
            //}
            //if (Dic["RetRemark"] != "")
            //{
            //    q = q.Where(a => a.RetRemark == Dic["RetRemark"]);
            //}
            if (Dic["SendPerson"] != "")
            {
                q = q.Where(a => a.RetRemark == Dic["SendPerson"]);
            }
            if (Dic["SendType"] != "")
            {
                q = q.Where(a => a.RetRemark == Dic["SendType"]);
            }
            if (Dic["SendStatus"] != "")
            {
                q = q.Where(a => a.SendStatus == Dic["SendStatus"]);
            }
            counts = q.ToList().Count;
            dt = LinqToDataTable.LINQToDataTable(q.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            return dt;
        }

        /// <summary>
        /// 返回出货记录信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate2(string Id)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.ShipmentRecord select c;
            q = q.Where(a => a.Id == Convert.ToInt32(Id));
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }


        /// <summary>
        /// 添加出货记录
        /// </summary>
        /// <returns></returns>
        public string AddRecord(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                model.ShipmentRecord mr = new model.ShipmentRecord()
                {
                    OrderId = Dic["OrderId"],
                    ExPrice = Dic["ExPrice"],//换货金额
                    //QuaLevel = Dic["QuaLevel"],
                    SendTime = Convert.ToDateTime(Dic["SendTime"]),//发货时间
                    Express = Dic["Express"],
                    ExpressNo = Dic["ExpressNo"],
                    YFHKD = Dic["YFHKD"],
                    YFRMB = Dic["YFRMB"],
                    RetRemark = Dic["RetRemark"],
                    SendPerson = Dic["SendPerson"],//发货人
                    SendType = Dic["SendType"],//出货类型 1.新订单 
                    SendStatus = Dic["SendStatus"],//发货状态 1.已发货 2.订单关闭

                };
                context.ShipmentRecord.InsertOnSubmit(mr);
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "出货记录",
                    OperaType = "添加",
                    UserId = UserId,
                });
                return "添加成功!";

            }
            catch (Exception ex)
            {
                return "添加失败!";
            }
        }

        /// <summary>
        /// 修改出货记录
        /// </summary>
        /// <returns></returns>
        public string SaveEditRecord(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.ShipmentRecord.Where(a => a.Id == Convert.ToInt32(Dic["Id"]));
                foreach (var i in q)
                {
                    i.OrderId = Dic["OrderId"];
                    i.ExPrice = Dic["ExPrice"];
                    //i.QuaLevel = Dic["QuaLevel"];
                    i.SendTime = Convert.ToDateTime(Dic["SendTime"]);
                    i.Express = Dic["Express"];
                    i.ExpressNo = Dic["ExpressNo"];
                    i.YFHKD = Dic["YFHKD"];
                    i.YFRMB = Dic["YFRMB"];
                    i.RetRemark = Dic["RetRemark"];
                    i.SendPerson = Dic["SendPerson"];
                    //i.SendType = Dic["SendType"];
                    //i.SendStatus = Dic["SendStatus"];
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "出货记录",
                    OperaType = "编辑",
                    UserId = UserId,
                });
                return "修改成功!";

            }
            catch (Exception ex)
            {
                return "修改失败!";
            }
        }

        /// <summary>
        /// 删除出货记录
        /// </summary>
        /// <returns></returns>
        public string DeleteRecord(string Id, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.ShipmentRecord.Where(a => a.Id == Convert.ToInt32(Id));
                string OrderId = q.SingleOrDefault().OrderId.ToString();
                foreach (var i in q)
                {
                    context.ShipmentRecord.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = OrderId,
                    OperaTable = "出货记录",
                    OperaType = "删除",
                    UserId = UserId,
                });
                return "";
            }
            catch (Exception ex)
            {
                return "删除失败!";
            }

        }
        /// <summary>
        /// 返回退货库存
        /// </summary>通过条件查询退货库存信息
        /// <returns></returns>
        public DataTable GetDate3(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.RetBalance
                    join ps in context.product on c.Scode equals ps.Scode
                    into pt
                    from pp in pt.DefaultIfEmpty()
                    join p in context.producttype on pp.Cat2 equals p.TypeNo
                    into t
                    from s in t.DefaultIfEmpty()
                    select new
                    {
                        Id = c.Id,
                        OrderId = c.OrderId,
                        Scode = c.Scode,
                        //Brand = c.Brand,
                        //Color = c.Color,
                        //Imagefile = c.Imagefile,
                        //Size = c.Size,
                        Price = c.Price,
                        Number = c.Number,
                        QuaLevel = c.QuaLevel,
                        RetTime = c.RetTime,
                        ProDetails = c.ProDetails,
                        ProLink = c.ProLink,
                        RetNum = c.RetNum,
                        LastOrderId = c.LastOrderId,
                        SellPrice = c.SellPrice,
                        Heidui = c.Heidui,
                        ExBalance = c.ExBalance,
                        Imagefile = c.Imagefile,

                        TypeNo = s.TypeName,

                        Brand = pp.Cat,
                        Color = pp.Clolor,
                        
                        Size = pp.Size,

                        TypeNo1 = c.TypeNo,
                    };
            if (Dic["OrderId"] != "")
            {
                q = q.Where(a => a.OrderId == Dic["OrderId"]);
            }
            if (Dic["Scode"] != "")
            {
                q = q.Where(a => a.Scode == Dic["Scode"]);
            }
            if (Dic["Brand"] != "")
            {
                q = q.Where(a => a.Brand == Dic["Brand"]);
            }
            if (Dic["Color"] != "")
            {
                q = q.Where(a => a.Color == Dic["Color"]);
            }
            if (Dic["TypeNo"] != "")
            {
                q = q.Where(a => a.TypeNo1 == Dic["TypeNo"]);
            }
            if (Dic["Size"] != "")
            {
                q = q.Where(a => a.Size == Dic["Size"]);
            }
            if (Dic["LastOrderId"] != "")
            {
                q = q.Where(a => a.LastOrderId == Dic["LastOrderId"]);
            }
            //if (Dic["QuaLevel"] != "")
            //{
            //    q = q.Where(a => a.QuaLevel == Dic["QuaLevel"]);
            //}
            if (Dic["RetTime"] != "")
            {
                if (Dic["RetTime1"] != "")
                {
                    q = q.Where(a => a.RetTime >= Convert.ToDateTime(Dic["RetTime"]) && a.RetTime <= Convert.ToDateTime(Dic["RetTime1"]));
                }
                else
                {
                    q = q.Where(a => a.RetTime >= Convert.ToDateTime(Dic["RetTime"]));
                }
            }
            else
            {
                if (Dic["RetTime1"] != "")
                {
                    q = q.Where(a => a.RetTime <= Convert.ToDateTime(Dic["RetTime1"]));
                }
            }
            counts = q.ToList().Count;
            dt = LinqToDataTable.LINQToDataTable(q.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            return dt;
        }

        /// <summary>
        /// 返回返回退货库存信息
        /// </summary>通过Id查询单条退货库存信息--用于编辑
        /// <returns></returns>
        public DataTable GetDate3(string Id)
        {
            DataTable dt = new DataTable();
            dt = null;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.RetBalance
                    join ps in context.product on c.Scode equals ps.Scode
                    into pt
                    from pp in pt.DefaultIfEmpty()
                    join p in context.producttype on pp.Cat2 equals p.TypeNo
                    into t
                    from s in t.DefaultIfEmpty()
                    select new
                    {
                        Id = c.Id,
                        OrderId = c.OrderId,
                        Scode = c.Scode,
                        //Brand = c.Brand,
                        //Color = c.Color,
                        //Imagefile = c.Imagefile,
                        //Size = c.Size,
                        Price = c.Price,
                        Number = c.Number,
                        QuaLevel = c.QuaLevel,
                        RetTime = c.RetTime,
                        ProDetails = c.ProDetails,
                        ProLink = c.ProLink,
                        RetNum = c.RetNum,
                        LastOrderId = c.LastOrderId,
                        SellPrice = c.SellPrice,
                        Heidui = c.Heidui,
                        ExBalance = c.ExBalance,
                        Imagefile = c.Imagefile,

                        TypeNo = s.TypeName,

                        Brand = pp.Cat,
                        Color = pp.Clolor,
                        Size = pp.Size,

                        TypeNo1 = c.TypeNo,
                    };
            q = q.Where(a => a.Id == Convert.ToInt32(Id));
            dt = LinqToDataTable.LINQToDataTable(q);
            return dt;
        }
        /// <summary>
        /// 添加退货库存
        /// </summary>
        /// <returns></returns>
        public string AddRetBalance(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q1 = context.product.Where(a => a.Scode == Dic["Scode"]).SingleOrDefault();
                model.RetBalance mr = new model.RetBalance()
                {
                    OrderId = Dic["OrderId"],
                    Scode = Dic["Scode"],
                    Brand = q1.Cat,
                    Color = q1.Clolor,
                    TypeNo = q1.Cat2,
                    Imagefile = q1.Imagefile,
                    Size = q1.Size,
                    Price = Dic["Price"],
                    Number = Dic["Number"],
                    QuaLevel = Dic["QuaLevel"],
                    RetTime = Convert.ToDateTime(Dic["RetTime"]),
                    ProDetails = Dic["ProDetails"],
                    ProLink = Dic["ProLink"],
                    RetNum = Dic["RetNum"],
                    LastOrderId = Dic["LastOrderId"],
                    SellPrice = Dic["SellPrice"],
                    Heidui = Dic["Heidui"],
                    ExBalance = Dic["ExBalance"],

                };
                context.RetBalance.InsertOnSubmit(mr);
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "退货库存",
                    OperaType = "添加",
                    UserId = UserId,
                });
                return "添加成功!";

            }
            catch (Exception ex)
            {
                return "添加失败!";
            }
        }

        /// <summary>
        /// 修改退货库存
        /// </summary>
        /// <returns></returns>
        public string SaveEditRetBalance(Dictionary<string, string> Dic, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.RetBalance.Where(a => a.Id == Convert.ToInt32(Dic["Id"]));
                var q1 = context.product.Where(a => a.Scode == Dic["Scode"]).SingleOrDefault();
                foreach (var i in q)
                {
                    i.OrderId = Dic["OrderId"];
                    i.Scode = Dic["Scode"];
                    i.Brand = q1.Cat;
                    i.Color = q1.Clolor;
                    i.TypeNo = q1.Cat2;
                    i.Imagefile = q1.Imagefile;
                    i.Size = q1.Size;
                    i.Price = Dic["Price"];
                    i.Number = Dic["Number"];
                    i.QuaLevel = Dic["QuaLevel"];
                    i.RetTime = Convert.ToDateTime(Dic["RetTime"]);
                    i.ProDetails = Dic["ProDetails"];
                    i.ProLink = Dic["ProLink"];
                    i.RetNum = Dic["RetNum"];
                    i.LastOrderId = Dic["LastOrderId"];
                    i.SellPrice = Dic["SellPrice"];
                    i.Heidui = Dic["Heidui"];
                    i.ExBalance = Dic["ExBalance"];
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = Dic["OrderId"],
                    OperaTable = "退货库存",
                    OperaType = "编辑",
                    UserId = UserId,
                });
                return "修改成功!";

            }
            catch (Exception ex)
            {
                return "修改失败!";
            }
        }

        /// <summary>
        /// 删除退货库存
        /// </summary>
        /// <returns></returns>
        public string DeleteRetBalance(string Id, int UserId)
        {
            try
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q = context.RetBalance.Where(a => a.Id == Convert.ToInt32(Id));
                string OrderId = q.SingleOrDefault().OrderId.ToString();
                foreach (var i in q)
                {
                    context.RetBalance.DeleteOnSubmit(i);
                }
                context.SubmitChanges();
                OperaRecord(new model.OperationRecord()
                {
                    OrderId = OrderId,
                    OperaTable = "退货库存",
                    OperaType = "删除",
                    UserId = UserId,
                });
                return "";
            }
            catch (Exception ex)
            {
                return "删除失败!";
            }
        }

        /// <summary>
        /// 查询操作记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate4(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            DataTable dt = new DataTable();
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.OperationRecord
                    join u in context.users on c.UserId equals u.Id
                    into s
                    from g in s.DefaultIfEmpty()
                    select new
                    {
                        Id=c.Id,
                        OrderId = c.OrderId,//订单编号
                        OperaTable = c.OperaTable,//操作表格
                        OperaType = c.OperaType,//操作类型
                        OperaTime = c.OperaTime,//操作时间
                        UserId1 = c.UserId,//操作人Id
                        UserId = g.userName,//操作人名称
                    };
            if (Dic["OrderId"] != "")
            {
                q = q.Where(a => a.OrderId == Dic["OrderId"]);
            }
            if (Dic["OperaTable"] != "")
            {
                q = q.Where(a => a.OperaTable == Dic["OperaTable"]);
            }
            if (Dic["OperaType"] != "")
            {
                q = q.Where(a => a.OperaType == Dic["OperaType"]);
            }
            if (Dic["OperaTime"] != "")
            {
                if (Dic["OperaTime1"] != "")
                {
                    q = q.Where(a => a.OperaTime >= Convert.ToDateTime(Dic["OperaTime"]) && a.OperaTime <= Convert.ToDateTime(Dic["OperaTime1"]));
                }
                else
                {
                    q = q.Where(a => a.OperaTime >= Convert.ToDateTime(Dic["OperaTime"]));
                }
            }
            else
            {
                if (Dic["OperaTime1"] != "")
                {
                    q = q.Where(a => a.OperaTime <= Convert.ToDateTime(Dic["OperaTime1"]));
                }
            }



            counts = q.Count();
            q = q.OrderByDescending(a => a.OperaTime);
            dt = LinqToDataTable.LINQToDataTable(q.Skip((pageIndex - 1) * pageSize).Take(pageSize));
            return dt;
        }

        /// <summary>
        /// 更新交易状态
        /// </summary>
        /// <param name="OrderId">存储了OrderId以及Socde 通过-分割</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string UpdateSalesState(string Id, int UserId)
        {

            try
            {
                string s = string.Empty;
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                var q1 = context.RetProductInfo.Where(a => a.Id == Convert.ToInt32(Id));
                string OrderId = q1.SingleOrDefault().OrderId;
                string Scode = q1.SingleOrDefault().Def2;
                var q = context.ProductInfo.Where(a => a.OrderId == OrderId && a.Scode == Scode);
                string SaleStates = q.SingleOrDefault().Def1.ToString();
                if (SaleStates == "3")//退货信息 退货 根据发货状态 来修改状态  3.退货中 对应 4.退货完成     5.退款中 对应 6.退款完成 
                {
                    foreach (var i in q)
                    {
                        i.Def1 = "4";//退货成功
                        i.Def3 = DateTime.Now;
                    }

                    foreach (var i in q1)//根据ProductInfo表的发货状态来修改RetProductInfo的退货状态
                    {
                        i.Def1 = "4";//退货成功
                        i.Def4 = DateTime.Now;
                    }
                    OperaRecord(new model.OperationRecord()
                    {
                        OrderId = OrderId,
                        OperaTable = "退货信息",
                        OperaType = "确定退货",
                        UserId = UserId,
                       
                    });
                    s = "退货成功!";
                }
                else if (SaleStates == "5")//退货信息 退款
                {
                    foreach (var i in q)
                    {
                        i.Def1 = "6";//退款成功
                        i.Def3 = DateTime.Now;
                    }
                    foreach (var i in q1)
                    {
                        i.Def1 = "3";//退款成功
                        i.Def4 = DateTime.Now;
                    }
                    OperaRecord(new model.OperationRecord()
                    {
                        OrderId = OrderId,
                        OperaTable = "退货信息",
                        OperaType = "确定退款",
                        UserId = UserId,
                    });
                    s = "退款成功!";
                }
                //else if (SaleStates == "3")//出货记录 换货
                //{

                //    foreach (var i in q)
                //    {
                //        i.Def1 = "4";
                //    }
                //    var q1 = context.ShipmentRecord.Where(a => a.OrderId == OrderId1 && a.Def2 == Scode);
                //    foreach (var i in q1)
                //    {
                //        i.SendStatus = "3";
                //    }
                //    OperaRecord(new model.OperationRecord()
                //    {
                //        OrderId = OrderId,
                //        OperaTable = "出货记录",
                //        OperaType = "确定换货",
                //        UserId = UserId,
                //    });
                //    s = "换货成功!";
                //}
                context.SubmitChanges();
                return s;
            }
            catch (Exception ex)
            {
                return "操作失败!";
            }

        }

        /// <summary>
        /// 添加操作记录
        /// </summary>
        /// <param name="mm"></param>
        public void OperaRecord(model.OperationRecord mm)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            mm.OperaTime = DateTime.Now;
            context.OperationRecord.InsertOnSubmit(mm);
            context.SubmitChanges();
        }



    }
}
