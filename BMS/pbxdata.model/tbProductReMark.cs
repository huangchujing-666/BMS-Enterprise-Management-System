/**  版本信息模板在安装目录下，可自行修改。
* tbProductReMark.cs
*
* 功 能： N/A
* 类 名： tbProductReMark
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/10/9 17:24:10   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace PbxData.Model
{
    /// <summary>
    /// tbProductReMark:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class tbProductReMark
    {
        public tbProductReMark()
        { }
        #region Model
        private int _productremarkid;
        private long? _productremarkshopid;
        private string _productstyle;
        private string _producttitle;
        private string _productimg;
        private decimal? _producttaobaoprice;
        private int? _productyjstock;
        private int? _productsj;
        private DateTime? _productsjtime1;
        private DateTime? _productxjtime2;
        private string _productstate;
        private string _productshopname;
        private int? _productremarkstocka;
        private int? _productremarkstockb;
        private int? _productremarkshopcar;
        private int? _productremarkkeep;
        private string _productremarkactivity;
        private string _productremark1;
        private string _productother1;
        private string _productother2;
        private string _productother3;
        private string _productother4;
        /// <summary>
        /// 
        /// </summary>
        public int ProductReMarkId
        {
            set { _productremarkid = value; }
            get { return _productremarkid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long? ProductReMarkShopId
        {
            set { _productremarkshopid = value; }
            get { return _productremarkshopid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductStyle
        {
            set { _productstyle = value; }
            get { return _productstyle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductTitle
        {
            set { _producttitle = value; }
            get { return _producttitle; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductImg
        {
            set { _productimg = value; }
            get { return _productimg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ProductTaoBaoPrice
        {
            set { _producttaobaoprice = value; }
            get { return _producttaobaoprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductYJStock
        {
            set { _productyjstock = value; }
            get { return _productyjstock; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductSJ
        {
            set { _productsj = value; }
            get { return _productsj; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ProductSJTime1
        {
            set { _productsjtime1 = value; }
            get { return _productsjtime1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ProductXJTime2
        {
            set { _productxjtime2 = value; }
            get { return _productxjtime2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductState
        {
            set { _productstate = value; }
            get { return _productstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductShopName
        {
            set { _productshopname = value; }
            get { return _productshopname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductReMarkStockA
        {
            set { _productremarkstocka = value; }
            get { return _productremarkstocka; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductReMarkStockB
        {
            set { _productremarkstockb = value; }
            get { return _productremarkstockb; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductReMarkShopCar
        {
            set { _productremarkshopcar = value; }
            get { return _productremarkshopcar; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductReMarkKeep
        {
            set { _productremarkkeep = value; }
            get { return _productremarkkeep; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductReMarkActivity
        {
            set { _productremarkactivity = value; }
            get { return _productremarkactivity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductReMark1
        {
            set { _productremark1 = value; }
            get { return _productremark1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductOther1
        {
            set { _productother1 = value; }
            get { return _productother1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductOther2
        {
            set { _productother2 = value; }
            get { return _productother2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductOther3
        {
            set { _productother3 = value; }
            get { return _productother3; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductOther4
        {
            set { _productother4 = value; }
            get { return _productother4; }
        }
        #endregion Model

    }
}

