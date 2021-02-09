using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface iRetProduct
    {
        /// <summary>
        /// 返回查询交易信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDate(Dictionary<string, string> Dic, int pageIndex, int pageSize, out string counts);

        /// <summary>
        /// 返回查询交易信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDate(string OrderId);

        /// <summary>
        /// 添加交易信息
        /// </summary>
        /// <returns></returns>
        string AddTradeInfo(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 编辑交易信息
        /// </summary>
        /// <returns></returns>SaveTradeEdit
        string SaveTradeEdit(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 删除交易信息
        /// </summary>
        /// <returns></returns>
        string DeleteTrade(string OrderId, int UserId);

        /// <summary>
        /// 退货\退款
        /// </summary>
        /// <returns></returns>
        string ReturnGoods(Dictionary<string,string> Dic,  int UserId);
      

        /// <summary>
        /// 换货\发货
        /// </summary>
        /// <returns></returns>
        string ExchangeGoods(Dictionary<string,string> Dic,  int UserId);

        /// <summary>
        /// 取消状态
        /// </summary>
        /// <returns></returns>
        string CancelStatus(string OrderId, string Scode, string type, int UserId);

        /// <summary>
        /// 返回查询交易信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDateProInfo(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts);

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <returns></returns>
        string AddProductInfo(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <returns></returns>
        DataTable EditProductInfo(string Id);

        /// <summary>
        /// 保存编辑商品信息
        /// </summary>
        /// <returns></returns>
        string SaveProductInfoEdit(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 删除商品信息
        /// </summary>
        /// <returns></returns>
        string DeleteProductInfo(string Id, int UserId);

        /// <summary>
        /// 返回退货信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDate1(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts);

        /// <summary>
        /// 返回退货信息
        /// </summary>
        /// <returns></returns>
        DataTable GetDate1(string Id);

        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <returns></returns>
        string AddRetProInfo(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <returns></returns>
        string SaveEditRetProInfo(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 删除退货信息
        /// </summary>
        /// <returns></returns>
        string DeleteRetProInfo(string Id, int UserId);

        /// <summary>
        /// 返回出货记录
        /// </summary>
        /// <returns></returns>
        DataTable GetDate2(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts);

        /// <summary>
        /// 返回出货记录
        /// </summary>
        /// <returns></returns>
        DataTable GetDate2(string Id);

        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <returns></returns>
        string AddRecord(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <returns></returns>
        string SaveEditRecord(Dictionary<string, string> Dic, int UserId);

        /// <summary>
        /// 删除退货信息
        /// </summary>
        /// <returns></returns>
        string DeleteRecord(string Id, int UserId);

        /// <summary>
        /// 返回退货库存
        /// </summary>
        /// <returns></returns>
        DataTable GetDate3(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts);

        /// <summary>
        /// 返回退货库存
        /// </summary>
        /// <returns></returns>
        DataTable GetDate3(string Id);

        /// <summary>
        /// 添加退货库存
        /// </summary>
        /// <returns></returns>
        string AddRetBalance(Dictionary<string, string> Dic, int UserId);
        

        /// <summary>
        /// 修改退货库存
        /// </summary>
        /// <returns></returns>
        string SaveEditRetBalance(Dictionary<string, string> Dic, int UserId);
        

        /// <summary>
        /// 删除退货库存
        /// </summary>
        /// <returns></returns>
        string DeleteRetBalance(string Id, int UserId);

        /// <summary>
        /// 查询操作记录
        /// </summary>
        /// <returns></returns>
        DataTable GetDate4(Dictionary<string, string> Dic, int pageIndex, int pageSize, out int counts);

        /// <summary>
        /// 更新交易状态
        /// </summary>
        /// <returns></returns>
        string UpdateSalesState(string Id, int UserId);

    }
}
