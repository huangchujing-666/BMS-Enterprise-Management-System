using pbxdata.dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pbxdata.model;
namespace pbxdata.bll
{
    public partial class ProductStockBLL : dataoperatingbll
    {
        ProductStockDAL psd = new ProductStockDAL();
        //***查询和分页***///
        /// <summary>
        /// 多条件查询分页
        /// </summary>
        /// <param name="str">多条件查询数据</param>
        /// <param name="i">页码</param>
        /// <returns></returns>
        public DataTable SearchShowProductStock(Dictionary<string,string> dic, int minid, int maxid)
        {
            return psd.SerachShowProductStock(dic, minid, maxid);
        }
        /// <summary>
        /// 多条件查询
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable SearchShowProductStock(Dictionary<string, string> dic, out int count)
        {
            return psd.SerachShowProductStock(dic, out count);
        }
        /// <summary>
        /// 按照款号查询
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable ProductStcokByStyle(string[] str, int minid, int maxid)
        {
            return psd.ProductStcokByStyle(str, minid, maxid);
        }
        /// <summary>
        /// 按照款号查询分页
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable ProductStcokByStyle(string[] str, out int count)
        {
            return psd.ProductStcokByStyle(str, out count);
        }
        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable Excel(string[] str)
        {
            return psd.Excel(str);
        }
        /// <summary>
        /// 按照货号和数据源查出当前货号的总库存
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public int CountByScode(string scode, string vencode)
        {
            return psd.CountByScode(scode, vencode);
        }
        /// <summary>
        /// 通过数据源ID找出数据源名称
        /// </summary>
        /// <param name="vencdoe"></param>
        /// <returns></returns>
        public string SelectVenNameByVencode(string vencode)
        {
            return psd.SelectVenNameByVencode(vencode);
        }
        /// <summary>
        /// 通过货号和数据源查找出当前数据的信息
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="socde"></param>
        /// <returns></returns>
        public DataTable SelectDataByScodeAndVencode(string vencode, string scode)
        {
            return psd.SelectDataByScodeAndVencode(vencode, scode);
        }
        /// <summary>
        /// 根据数据源名获得数据源名字
        /// </summary>
        /// <param name="sourname"></param>
        /// <returns></returns>
        public string GetSourcode(string sourname)
        {
            return psd.GetSourcode(sourname);
        }
        /// <summary>
        /// 根据数据源code获得数据源名字
        /// </summary>
        /// <param name="sourname"></param>
        /// <returns></returns>
        public string GetSourname(string sourname)
        {
            return psd.GetSourname(sourname);
        }
        //****************************7.2订单管理
        public bool InsertOrder(string[] str)
        {
            return psd.InsertOrder(str);
        }
        /// <summary>
        /// 得到所有订单信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetOrderGoods(Dictionary<string,string> dic, int minid, int maxid)
        {
            return psd.GetOrderGoods(dic, minid, maxid);
        }
        /// <summary>
        /// 得到所有订单个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetOrderGoodsCount(Dictionary<string,string> dic)
        {
            return psd.GetOrderGoodsCount(dic);
        }

        /// <summary>
        /// 导入当前订单的数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool InsertProductStockOrder(DataTable dt, string OrderId, string Vencode)
        {
            return psd.InsertProductStockOrder(dt, OrderId, Vencode);
        }

        /// <summary>
        /// 当前数据是否存在
        /// </summary>
        /// <param name="Scode"></param>
        /// <param name="time"></param>
        /// <param name="Vencode"></param>
        /// <returns></returns>
        public bool IsProductStockOrder(string Scode, string OrderNo)
        {
            return psd.IsProductStockOrder(Scode, OrderNo);
        }
        /// <summary>
        /// --------导入数据时将将需要重复数据进行修改
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="time"></param>
        /// <param name="vencode"></param>
        /// <param name="balance"></param>
        /// <returns></returns>
        public bool UpdateProductStockOrder(string scode, string balance, string OrderNo, string Pricea, string Pricee)
        {
            return psd.UpdateProductStockOrder(scode, balance, OrderNo, Pricea, Pricee);
        }
        /// <summary>
        /// 上传成功 修改上传时间
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public bool UpdateOrderGoodsImportTime(string OrderNo, string Time, string countstyle, string countbalance)
        {
            return psd.UpdateOrderGoodsImportTime(OrderNo, Time, countstyle, countbalance);
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public DataTable ProductStockOrderPage(Dictionary<string,string> Dic, string customer)
        {
            return psd.ProductStockOrderPage(Dic, customer);
        }
        /// <summary>
        /// 订单详情  个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int ProductStockOrderPageCount(Dictionary<string,string> Dic, string customer)
        {
            return psd.ProductStockOrderPageCount(Dic, customer);
        }
        /// <summary>
        /// -----统计当前单号有多少款 总公共多少件货
        /// </summary>
        /// <returns></returns>
        public DataTable ProductStockOrderStatistics(string orderid)
        {
            return psd.ProductStockOrderStatistics(orderid);
        }
        //////*********************************7.22
        /// <summary>
        /// 标记当前商品是否为残次品
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string Marker(string scode, string vencode)
        {
            return psd.Marker(scode, vencode);
        }
        /// <summary>
        /// 此货号当前残次品的数量
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public int MarkerCount(string scode, string vencode)
        {
            return psd.MarkerCount(scode, vencode);
        }
        /// <summary>
        /// 查出库存表中当前数据的信息
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<productstock> SelectProduct(string scode, string vencode)
        {
            return psd.SelectProduct(scode, vencode);
        }
        /// <summary>
        /// 将标记的残次品加入到数据库中
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string InsertDefectiveRemark(List<productstock> list, string ProductRemark, string ScodeMarKer, int sum)
        {
            return psd.InsertDefectiveRemark(list, ProductRemark, ScodeMarKer, sum);
        }
        /// <summary>
        /// 查询所有标记货品
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetDefectiveRemark(string[] str, int minid, int maxid)
        {
            return psd.GetDefectiveRemark(str, minid, maxid);
        }
        /// <summary>
        /// 查询所有标记货品个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetDefectiveRemarkCount(string[] str)
        {
            return psd.GetDefectiveRemarkCount(str);
        }
        /// <summary>
        /// ---得到合并数据
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetMergeDefectiveRemark(string[] str, int minid, int maxid)
        {
            return psd.GetMergeDefectiveRemark(str, minid, maxid);
        }
        /// <summary>
        /// ---得到合并数据个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetMergeDefectiveRemarkCount(string[] str)
        {
            return psd.GetMergeDefectiveRemarkCount(str);
        }

        /// <summary>
        /// 通过标记编号获得数据
        /// </summary>
        /// <returns></returns>
        public List<DefectiveRemark> GetDefectiveRemarkByScodeMarker(string scodemarker)
        {
            return psd.GetDefectiveRemarkByScodeMarker(scodemarker);
        }
        /// <summary>
        /// 修改标记品的价格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string UpdatePrice(decimal[] str, string scodemarker)
        {
            return psd.UpdatePrice(str, scodemarker);
        }
        /// <summary>
        /// -得到当前货号被标记的总库存
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="scode"></param>
        /// <returns></returns>
        public int GetDefctiveRemarkBalanceCount(string vencode, string scode)
        {
            return psd.GetDefctiveRemarkBalanceCount(vencode, scode);
        }
        /// <summary>
        /// 得到所有数据源
        /// </summary>
        /// <returns></returns>
        public List<productsource> GetVencodeProduct()
        {
            return psd.GetVencodeProduct();
        }
        /// <summary>
        /// 查询当前标记编号是否存在
        /// </summary>
        /// <param name="ScodeMarker"></param>
        /// <returns></returns>
        public bool IsExistScodeMarker(string ScodeMarker)
        {
            return psd.IsExistScodeMarker(ScodeMarker);
        }
        /// <summary>
        /// 通过货号的到当前货号的最大标记号
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public string GetMaxScodeMarker(string Scode)
        {
            return psd.GetMaxScodeMarker(Scode);
        }
        /// <summary>
        /// 取消当前标记
        /// </summary>
        /// <param name="ScodeMarker"></param>
        /// <returns></returns>
        public string DeleteScodeMarker(string ScodeMarker)
        {
            return psd.DeleteScodeMarker(ScodeMarker);
        }
        /// <summary>
        /// 得到货号的所有图片
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public List<scodepic> GetImageFile(string scode)
        {
            return psd.GetImageFile(scode);
        }
        /// <summary>
        /// 得到款号图
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public List<stylepic> GetStyleImageFile(string style)
        {
            return psd.GetStyleImageFile(style);
        }

        /// <summary>
        /// 暂停开放某个数据源的某件货
        /// </summary>
        /// <param name="scode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public string CloseStock(string scode, string vencode, string state)
        {
            return psd.CloseStock(scode, vencode, state);
        }
        /// <summary>
        /// ---得到需要开放或者关闭的货号以及数据源
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable GetProductOpenScode(string[] str)
        {
            return psd.GetProductOpenScode(str);
        }
        /// <summary>
        /// ---开放或者暂停 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="state">开放或者暂停   暂停  1   开放  2</param>
        /// <returns></returns>
        public string ProductOpenScode(DataTable dt, string state)
        {
            return psd.ProductOpenScode(dt, state);
        }


        ////////8.11
        /// <summary>
        /// 得到供应商品牌
        /// </summary>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<BrandVen> GetBrandByVencode(string vencode)
        {
            return psd.GetBrandByVencode(vencode);
        }
        /// <summary>
        /// 得到供应商类别
        /// </summary>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public List<producttypeVen> GetProductTypeVencode(string Vencode)
        {
            return psd.GetProductTypeVencode(Vencode);
        }
        /// <summary>
        /// 得到供应商类别  供应商+大类别编号
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="TypeBigId"></param>
        /// <returns></returns>
        public List<producttypeVen> GetProductTypeVencode(string Vencode, int TypeBigId)
        {
            return psd.GetProductTypeVencode(Vencode, TypeBigId);
        }
        /// <summary>
        /// 得到所有大类别
        /// </summary>
        /// <returns></returns>
        public List<productbigtype> GetProductBigTypeVencode()
        {
            return psd.GetProductBigTypeVencode();
        }
        /// <summary>
        /// 打开或者暂停库存  品牌
        /// </summary>
        /// <returns></returns>
        public bool OpenPorudctBrand(string[] str, string openstate, string vencode)
        {
            return psd.OpenPorudctBrand(str, openstate, vencode);
        }
        /// <summary>
        /// 打开或者暂停库存 类别
        /// </summary>
        /// <returns></returns>
        public bool OpenPorudctType(string[] str, string openstate, string vencode)
        {
            return psd.OpenPorudctType(str, openstate, vencode);
        }
        /// <summary>
        /// 得到所有数据源
        /// </summary>
        /// <returns></returns>
        public List<productsource> GetProductSourceAll()
        {
            return psd.GetProductSourceAll();
        }
        /// <summary>
        /// 得到数据源中开放或者未开放的货物数量
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="vencode"></param>
        /// <returns></returns>
        public int GetProductStockOpenCount(string vencode, string Open)
        {
            return psd.GetProductStockOpenCount(vencode, Open);
        }
        /// <summary>
        /// 开放或者暂停整个数据源
        /// </summary>
        /// <param name="vencode"></param>
        /// <param name="openstate"></param>
        /// <returns></returns>
        public bool OpenAll(string vencode, string openstate)
        {
            return psd.OpenAll(vencode, openstate);
        }
                /// <summary>
        /// 导出数据库存按照款号导出
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public DataTable OutPutExcelStyle(string[] str) 
        {
            return psd.OutPutExcelStyle(str);
        }
    }
}
