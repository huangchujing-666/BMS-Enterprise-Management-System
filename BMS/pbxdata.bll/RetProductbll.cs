using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{

    public class RetProductbll : dataoperatingbll
    {
        iRetProduct dal = (iRetProduct)ReflectFactory.CreateIDataOperatingByReflect("RetProductdal");


        /// <summary>
        /// 返回查询交易信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts)
        {
            return dal.GetDate(Dic, pageIndex, pageSize, out counts);
        }
        
        /// <summary>
        /// 返回查询交易信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate(string OrderId)
        {
            return dal.GetDate(OrderId);
        }

        /// <summary>
        /// 添加交易信息
        /// </summary>
        /// <returns></returns>
        public string AddTradeInfo(Dictionary<string, string> Dic, int UserId)
        {
            return dal.AddTradeInfo(Dic, UserId);
        }

        /// <summary>
        /// 编辑交易信息
        /// </summary>
        /// <returns></returns>SaveTradeEdit
        public string SaveTradeEdit(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveTradeEdit(Dic, UserId);
        }

        /// <summary>
        /// 删除交易信息
        /// </summary>
        /// <returns></returns>
        public string DeleteTrade(string OrderId,int UserId)
        {
            return dal.DeleteTrade(OrderId,UserId);
        }

        /// <summary>
        /// 退货\退款\发货
        /// 1.退货\退款 2.发货
        /// </summary>
        /// <returns></returns>
        public string ReturnGoods(Dictionary<string,string> Dic,string type,int UserId)
        {
            if (type == "1")
            {
                return dal.ReturnGoods(Dic, UserId);
            }
            else if (type == "2")
            {
                return dal.ExchangeGoods(Dic, UserId);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 取消状态
        /// </summary>
        /// <returns></returns>
        public string CancelStatus(string OrderId,string Scode,string type,int UserId)
        {
            return dal.CancelStatus(OrderId, Scode, type, UserId);
        }
        /// <summary>
        /// 返回查询商品信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDateProInfo(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            return dal.GetDateProInfo(Dic, pageIndex, pageSize, out counts);
        }

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <returns></returns>
        public string AddProductInfo(Dictionary<string, string> Dic,int UserId)
        {
            return dal.AddProductInfo(Dic,UserId);
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <returns></returns>
        public DataTable EditProductInfo(string Id)
        {
            return dal.EditProductInfo(Id);
        }

        /// <summary>
        /// 保存编辑商品信息
        /// </summary>
        /// <returns></returns>
        public string SaveProductInfoEdit(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveProductInfoEdit(Dic, UserId);
        }

         /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <returns></returns>
        public string DeleteProductInfo(string Id, int UserId)
        {
            return dal.DeleteProductInfo(Id,UserId);
        }

        /// <summary>
        /// 返回退货信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate1(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            return dal.GetDate1(Dic, pageIndex, pageSize, out counts);
        }

        /// <summary>
        /// 返回退货信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate1(string Id)
        {
            return dal.GetDate1(Id);
        }

        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <returns></returns>
        public string AddRetProInfo(Dictionary<string, string> Dic,int UserId)
        {
            return dal.AddRetProInfo(Dic, UserId);
        }

        /// <summary>
        /// 修改退货信息
        /// </summary>
        /// <returns></returns>
        public string SaveEditRetProInfo(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveEditRetProInfo(Dic, UserId);
        }

        /// <summary>
        /// 删除退货信息
        /// </summary>
        /// <returns></returns>
        public string DeleteRetProInfo(string Id, int UserId)
        {
            return dal.DeleteRetProInfo(Id, UserId);
        }

        /// <summary>
        /// 返回出货记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate2(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            return dal.GetDate2(Dic, pageIndex, pageSize, out counts);
        }

        /// <summary>
        /// 返回出货记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate2(string Id)
        {
            return dal.GetDate2(Id);
        }

        /// <summary>
        /// 添加出货记录
        /// </summary>
        /// <returns></returns>
        public string AddRecord(Dictionary<string, string> Dic, int UserId)
        {
            return dal.AddRecord(Dic, UserId);
        }

        /// <summary>
        /// 修改出货记录
        /// </summary>
        /// <returns></returns>
        public string SaveEditRecord(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveEditRecord(Dic, UserId);
        }
 
        /// <summary>
        /// 删除出货记录
        /// </summary>
        /// <returns></returns>
        public string DeleteRecord(string Id, int UserId)
        {
            return dal.DeleteRecord(Id,UserId);
        }


        /// <summary>
        /// 返回退货库存
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate3(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            return dal.GetDate3(Dic, pageIndex, pageSize, out counts);
        }
        /// <summary>
        /// 返回退货库存
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate3(string Id)
        {
            return dal.GetDate3(Id);
        }

        /// <summary>
        /// 添加退货库存
        /// </summary>
        /// <returns></returns>
        public string AddRetBalance(Dictionary<string, string> Dic, int UserId)
        {
            return dal.AddRetBalance(Dic,UserId);
        }

        /// <summary>
        /// 修改退货库存
        /// </summary>
        /// <returns></returns>
        public string SaveEditRetBalance(Dictionary<string, string> Dic, int UserId)
        {
            return dal.SaveEditRetBalance(Dic,UserId);
        }

        /// <summary>
        /// 删除退货库存
        /// </summary>
        /// <returns></returns>
        public string DeleteRetBalance(string Id, int UserId)
        {
            return dal.DeleteRetBalance(Id,UserId);
        }

        /// <summary>
        /// 查询操作记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetDate4(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts)
        {
            return dal.GetDate4(Dic, pageIndex, pageSize, out counts);
        }

        /// <summary>
        /// 更新交易状态
        /// </summary>
        /// <param name="OrderId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string UpdateSalesState(string Id, int UserId)
        {
            return dal.UpdateSalesState(Id, UserId);
        }
    }
}
