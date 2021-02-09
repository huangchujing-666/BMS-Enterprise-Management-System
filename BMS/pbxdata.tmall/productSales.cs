/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       productSales
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.tmall
    * 文 件 名：       productSales
    * 创建时间：       2015-05-14 15:41:47
    * 作    者：       fj
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using Jayrock.Json.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using System.Runtime.Serialization.Json;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

namespace pbxdata.tmall
{
    public class productSales : tmallapp
    {
        /// <summary>
        /// 获取淘宝店铺销售中的商品
        /// </summary>
        /// <param name="page">淘宝的分页序号</param>
        /// <param name="pageSize">淘宝分页每页显示的数量</param>
        /// <returns>返回店铺销售中的商品</returns>
        public List<Top.Api.Domain.Item> GetTaoShopOnSaleItems(int page, int pageSize)
        {

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
            req.Fields = "approve_status,num_iid,title,nick,type,cid,pic_url,num,props,valid_thru,list_time,price,has_discount,has_invoice,has_warranty,has_showcase,modified,delist_time,postage_id,seller_cids,outer_id";
            //淘宝的分页序号
            req.PageNo = page;
            //淘宝分页每页显示的数量
            req.PageSize = pageSize;
            ItemsOnsaleGetResponse response = client.Execute(req, Sessionkey);
            List<Top.Api.Domain.Item> saleList = response.Items;

            return saleList;
        }
        /// <summary>
        /// 获取淘宝店铺销售中的商品
        /// </summary>
        /// <param name="page">淘宝的分页序号</param>
        /// <param name="pageSize">淘宝分页每页显示的数量</param>
        /// <returns>返回店铺销售中的商品</returns>
        public List<Top.Api.Domain.Item> GetTaoShopOnSaleItems(int page, int pageSize,string title)
        {

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
            req.Fields = "approve_status,num_iid,title,nick,type,cid,pic_url,num,props,valid_thru,list_time,price,has_discount,has_invoice,has_warranty,has_showcase,modified,delist_time,postage_id,seller_cids,outer_id";
            //淘宝的分页序号
            req.PageNo = page;
            //淘宝分页每页显示的数量
            req.PageSize = pageSize;
            req.Q = title;
            ItemsOnsaleGetResponse response = client.Execute(req, Sessionkey);
            List<Top.Api.Domain.Item> saleList = response.Items;

            return saleList;
        }
        /// <summary>
        /// 获取淘宝店铺销售中的商品
        /// </summary>
        /// <returns>返回店铺销售中的商品数量</returns>
        public int GetTaoShopOnSaleItems()
        {

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
            req.Fields = "num_iid";
            ItemsOnsaleGetResponse response = client.Execute(req, Sessionkey);
            int temp = GetNumParms(response.TotalResults);
            return temp;
        }
        /// <summary>
        /// 获取淘宝店铺销售中的商品
        /// </summary>
        /// <returns>返回店铺销售中的商品数量</returns>
        public int GetTaoShopOnSaleItems(string title)
        {

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
            req.Fields = "num_iid";
            req.Q = title;
            ItemsOnsaleGetResponse response = client.Execute(req, Sessionkey);
            int temp = GetNumParms(response.TotalResults);
            return temp;
        }
        public int GetNumParms(object parms)
        {
            int num = -1;
            if (parms != null)
                int.TryParse(parms.ToString().Trim(), out num);
            return num;
        }
        /// <summary>
        /// 获取淘宝店铺库存商品
        /// </summary>
        /// <param name="page">淘宝的分页序号</param>
        /// <param name="pageSize">淘宝分页每页显示的数量</param>
        /// <returns>返回店铺仓库中的商品</returns>
        public List<Top.Api.Domain.Item> GetInventoryTaoShop(int page, int pageSize)
        {

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsInventoryGetRequest req = new ItemsInventoryGetRequest();
            req.Fields = "approve_status,num_iid,title,nick,type,cid,pic_url,num,props,valid_thru,list_time,price,has_discount,has_invoice,has_warranty,has_showcase,modified,delist_time,postage_id,seller_cids,outer_id";
            req.PageNo = page;
            req.PageSize = pageSize;
            ItemsInventoryGetResponse response = client.Execute(req, Sessionkey);
            List<Top.Api.Domain.Item> saleList = response.Items;
            return saleList;

        }
        /// <summary>
        /// 获取淘宝店铺库存商品
        /// </summary>
        /// <param name="page">淘宝的分页序号</param>
        /// <param name="pageSize">淘宝分页每页显示的数量</param>
        /// <returns>返回店铺仓库中的商品</returns>
        public List<Top.Api.Domain.Item> GetInventoryTaoShop(int page, int pageSize, string title)
        {

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsInventoryGetRequest req = new ItemsInventoryGetRequest();
            req.Fields = "approve_status,num_iid,title,nick,type,cid,pic_url,num,props,valid_thru,list_time,price,has_discount,has_invoice,has_warranty,has_showcase,modified,delist_time,postage_id,seller_cids,outer_id";
            req.PageNo = page;
            req.PageSize = pageSize;
            req.Q = title;
            ItemsInventoryGetResponse response = client.Execute(req, Sessionkey);
            List<Top.Api.Domain.Item> saleList = response.Items;
            return saleList;

        }
        /// <summary>
        /// 获取淘宝店铺库存商品
        /// </summary>
        /// <returns>返回店铺仓库中的商品数量</returns>
        public int GetInventoryTaoShop()
        {
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsInventoryGetRequest req = new ItemsInventoryGetRequest();
            req.Fields = "num_iid";
            ItemsInventoryGetResponse response = client.Execute(req, Sessionkey);
            int temp = GetNumParms(response.TotalResults);
            return temp;

        }
        /// <summary>
        /// 获取淘宝店铺库存商品
        /// </summary>
        /// <returns>返回店铺仓库中的商品数量</returns>
        public int GetInventoryTaoShop(string title)
        {
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemsInventoryGetRequest req = new ItemsInventoryGetRequest();
            req.Fields = "num_iid";
            req.Q = title;
            ItemsInventoryGetResponse response = client.Execute(req, Sessionkey);
            int temp = GetNumParms(response.TotalResults);
            return temp;

        }
        /// <summary>
        /// 获取淘宝商品的SKU
        /// </summary>
        /// <param name="numiids">淘宝商品ID</param>
        /// <returns>返回淘宝商品SKU信息</returns>
        public List<Top.Api.Domain.Sku> GetTaoSKU(string numiids)
        {

            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemSkusGetRequest req = new ItemSkusGetRequest();
            req.Fields = "sku_id,num_iid,properties,quantity,price,outer_id,created,modified";
            req.NumIids = numiids;
            ItemSkusGetResponse response = client.Execute(req, Sessionkey);
            List<Top.Api.Domain.Sku> SkuList = response.Skus;
            return SkuList;

        }
        /// <summary>
        /// 获得单个淘宝商品信息
        /// </summary>
        /// <param name="numIid">淘宝商品ID</param>
        /// <returns>淘宝Item</returns>
        public Top.Api.Domain.Item GetItem(long numIid)
        {
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            //ItemGetRequest req = new ItemGetRequest();
            ItemSellerGetRequest req = new ItemSellerGetRequest();
            req.Fields = "detail_url,num_iid,title,nick,type,cid,seller_cids,props,input_pids,input_str,desc,pic_url,num,valid_thru,list_time,delist_time,stuff_status,location,price,post_fee,express_fee,ems_fee,has_discount,freight_payer,has_invoice,has_warranty,has_showcase,modified,increment,approve_status,postage_id,product_id,auction_point,property_alias,item_img,prop_img,sku,video,outer_id,is_virtual";
            req.NumIid = numIid;
            ItemSellerGetResponse response = client.Execute(req, Sessionkey);
            return response.Item;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skuCode">商品SKUid   商品SKU条形码</param>
        /// <param name="balance">需要修改的库存</param>
        /// <param name="num_id">淘宝商品编号 .</param>
        /// <param name="Scode">商品Scode  需要改成</param>
        /// <param name="Price">需要修改目标价格 </param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public bool UpdateTao(string skuCode, long balance, long num_id, string Scode, string Price, string properties) 
        {
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemSkuUpdateRequest req = new ItemSkuUpdateRequest();
            
            req.NumIid = num_id;             //淘宝商品编号 必填
            req.Properties = properties;   //SKU属性  必填
            req.Quantity = balance;        //库存  
            req.Barcode = skuCode;         //SKUiD  必填 
            req.ItemPrice = Price;       //价格
            req.OuterId = Scode;        //货号
            ItemSkuUpdateResponse response = client.Execute(req, Sessionkey);
            return true;
        }
        /// <summary>
        /// 获取标准商品类目属性
        /// </summary>
        /// <param name="RemarkId">商品编号</param>
        /// <param name="ProductTypeId">类别编号</param>
        /// <returns></returns>
        public object ItempropsGetRequest(long ProductTypeId) 
        {
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItempropsGetRequest req = new ItempropsGetRequest();
            req.Fields = "pid,name,must,multi,prop_values";
            req.Cid = ProductTypeId;
            req.Pid=1627207;
            req.Type=2;
            ItempropsGetResponse response = client.Execute(req);
            return response;
        }
        /// <summary>
        /// 商品SKU上架
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="properties"></param>
        /// <param name="Balance"></param>
        /// <param name="price"></param>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string ItemSkuAddRequest(long remarkId, string properties,long Balance,string price,string scode) 
        {
            ITopClient client = new DefaultTopClient(Url, Appkey, Appsecret);
            ItemSkuAddRequest req = new ItemSkuAddRequest();
            req.NumIid = remarkId;
            req.Properties = "1627207:" + properties;
            req.Quantity = Balance;
            req.Price = price;
            req.OuterId = scode;
            ItemSkuAddResponse response = client.Execute(req, Sessionkey);
            if (response.IsError == true)
            {
                return response.SubErrMsg;
            }
            else 
            {
                return "上架成功！";
            }
        }
    }
}
