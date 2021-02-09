using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.model
{
    public class TableXmlModel
    {
        /// <summary>
        /// 数据源表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string TextName { get; set; }
        /// <summary>
        /// 每次更新多久
        /// </summary>
        public string SetTime { get; set; }
        /// <summary>
        /// 更新状态（是否在更新）
        /// </summary>
        public string UpdateState { get; set; }
        /// <summary>
        /// 开始更新时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 正在更新的时间
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
