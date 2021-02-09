using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.model
{
    public class PorpertyModel
    {
        /// <summary>
        /// 属性名
        /// </summary>
      
        private string propertyName;
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value.Replace("$_", "").Replace("$", "").Trim(); }
        }
        /// <summary>
        /// 类别id
        /// </summary>
        public int TypeId { get; set; }
    }
}
