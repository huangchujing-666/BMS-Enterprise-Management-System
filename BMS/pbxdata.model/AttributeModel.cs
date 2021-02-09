using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.model
{
    public class AttributeModel
    {
        /// <summary>
        /// 主键自增 //--商品类别
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 大类别Id //
        /// </summary>
        public int BigId { get; set; }
        /// <summary>
        /// 大类别名称 //
        /// </summary>
        public string BigTypeName { get; set; }
        /// <summary>
        /// 类别名称 //
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类别号 //
        /// </summary>
        public string TypeNo { get; set; }
        /// <summary>
        /// 类别索引 //
        /// </summary>
        public int TypeIndex { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性显示顺序
        /// </summary>
        public int PorpertyIndex { get; set; }
        /// <summary>
        /// 使用者Id //
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 默认1 //
        /// </summary>
        public string Def1 { get; set; }
        /// <summary>
        /// 默认2 //
        /// </summary>
        public string Def2 { get; set; }
        /// <summary>
        /// 默认3 //
        /// </summary>
        public string Def3 { get; set; }
        /// <summary>
        /// 默认4 //
        /// </summary>
        public string Def4 { get; set; }
        /// <summary>
        /// 默认5 //
        /// </summary>
        public string Def5 { get; set; }
        /// <summary>
        /// 大类别名称
        /// </summary>
        public int bigtypeIndex { get; set; }
    }
}
