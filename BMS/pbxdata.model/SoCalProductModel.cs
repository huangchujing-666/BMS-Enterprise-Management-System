using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.model
{
    /// <summary>
    /// 现货实体
    /// </summary>
    public class SoCalProductModel
    {
        /// <summary>
        /// 主键自增 //
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 货号
        /// </summary>
        public string Scode { get; set; }
        /// <summary>
        /// 条码1
        /// </summary>
        public string Bcode { get; set; }
        /// <summary>
        /// 条码2
        /// </summary>
        public string Bcode2 { get; set; }
        /// <summary>
        /// 英文描述
        /// </summary>
        public string Descript { get; set; }
        /// <summary>
        /// 中文描述
        /// </summary>
        public string Cdescript { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 货币
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string Cat { get; set; }
        /// <summary>
        /// 季节
        /// </summary>
        public string Cat1 { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Cat2 { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string Cat2Name { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string Clolor { get; set; }
        /// <summary>
        /// 尺寸
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 款号
        /// </summary>
        public string Style { get; set; }
        /// <summary>
        /// 价格1
        /// </summary>
        public decimal Pricea { get; set; }
        /// <summary>
        /// 价格2
        /// </summary>
        public decimal Priceb { get; set; }
        /// <summary>
        /// 价格3
        /// </summary>
        public decimal Pricec { get; set; }
        /// <summary>
        /// 价格4
        /// </summary>
        public decimal Priced { get; set; }
        /// <summary>
        /// 价格5
        /// </summary>
        public decimal Pricee { get; set; }
        /// <summary>
        /// 折扣1
        /// </summary>
        public decimal Disca { get; set; }
        /// <summary>
        /// 折扣2
        /// </summary>
        public decimal Discb { get; set; }
        /// <summary>
        /// 折扣3
        /// </summary>
        public decimal Discc { get; set; }
        /// <summary>
        /// 折扣4
        /// </summary>
        public decimal Discd { get; set; }
        /// <summary>
        /// 折扣5
        /// </summary>
        public decimal Disce { get; set; }
        /// <summary>
        /// 供应商编号（外键）
        /// </summary>
        public string Vencode { get; set; }
        /// <summary>
        /// 供应商名称（外键关联查询）
        /// </summary>
        public string VencodeName { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        ///预警库存
        /// </summary>
        public int Rolevel { get; set; }
        /// <summary>
        /// 最少订货量
        /// </summary>
        public int Roamt { get; set; }
        /// <summary>
        /// 停售库存
        /// </summary>
        public int Stopsales { get; set; }
        /// <summary>
        /// 店铺
        /// </summary>
        public string Loc { get; set; }
        /// <summary>
        /// 供应商库存
        /// </summary>
        public int Balance { get; set; }
        /// <summary>
        /// 退货库存（从订单表中得到）
        /// </summary>
        public int ReturnBalance { get; set; }
        /// <summary>
        /// 销售库存（从订单表中得到）
        /// </summary>
        public int SalesBalance { get; set; }
        /// <summary>
        /// 收货日期
        /// </summary>
        public string Lastgrnd { get; set; }
        /// <summary>
        /// 货品图片（暂时为空）
        /// </summary>
        public string Imagefile { get; set; }
        /// <summary>
        /// 操作人编号（外键，暂时为空）
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 上一次库存
        /// </summary>
        public string PrevStock { get; set; }
        /// <summary>
        /// 默认2
        /// </summary>
        public string Def2 { get; set; }
        /// <summary>
        /// 默认3
        /// </summary>
        public string Def3 { get; set; }
        /// <summary>
        /// 默认4
        /// </summary>
        public string Def4 { get; set; }
        /// <summary>
        /// 默认5
        /// </summary>
        public string Def5 { get; set; }
        /// <summary>
        /// 默认6
        /// </summary>
        public string Def6 { get; set; }
        /// <summary>
        /// 默认7
        /// </summary>
        public string Def7 { get; set; }
        /// <summary>
        /// 默认8
        /// </summary>
        public string Def8 { get; set; }
        /// <summary>
        /// 默认9
        /// </summary>
        public string Def9 { get; set; }
        /// <summary>
        /// 默认10
        /// </summary>
        public string Def10 { get; set; }
        /// <summary>
        /// 默认11
        /// </summary>
        public string Def11 { get; set; }
    }
}
