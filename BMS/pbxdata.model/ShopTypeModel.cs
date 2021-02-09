using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.model
{
    public class ShopTypeModel
    {
        /// <summary>
        /// 编号自增
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 店铺类别名称
        /// </summary>
        public string ShopTypeName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int ShoptypeIndex { get; set; }
        /// <summary>
        /// 操作人 外键
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 默认1
        /// </summary>
        public string Def1 { get; set; }
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
    }
}
