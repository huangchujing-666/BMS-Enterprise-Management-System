/**  版本信息模板在安装目录下，可自行修改。
* GoodsType.cs
*
* 功 能： N/A
* 类 名： GoodsType
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/2/18 11:08:24   N/A    初版
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
	/// 商品类型表
	/// </summary>
	[Serializable]
	public partial class GoodsType
	{
		public GoodsType()
		{}
		#region Model
		private int _typeid;
		private string _atrributevalue;
		private string _typeno;
		private int? _del;
		private int? _isstate=0;
        private string _bigtypename;
		/// <summary>
		/// 主键ID自增列
		/// </summary>
		public int TypeID
		{
			set{ _typeid=value;}
			get{return _typeid;}
		}
		/// <summary>
		/// 类别名称
		/// </summary>
		public string AtrributeValue
		{
			set{ _atrributevalue=value;}
			get{return _atrributevalue;}
		}
		/// <summary>
		/// 类别编号
		/// </summary>
		public string TypeNO
		{
			set{ _typeno=value;}
			get{return _typeno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? del
		{
			set{ _del=value;}
			get{return _del;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsState
		{
			set{ _isstate=value;}
			get{return _isstate;}
		}

        /// <summary>
        /// 大类别名称
        /// </summary>
        public string BigTypeName
        {
            set { _bigtypename = value; }
            get { return _bigtypename; }
        }
		#endregion Model

	}
}

