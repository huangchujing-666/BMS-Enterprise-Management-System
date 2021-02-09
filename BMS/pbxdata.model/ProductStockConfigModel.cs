using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.model
{
    public class ProductStockConfigModel
    {
        public int Id { get; set; }
        public string SourceCode { get; set; }
        public string DataSources { get; set; }
        public string TableName { get; set; }
        public string UpdateState { get; set; }
        public string StartTime { get; set; }
        public string SetTime { get; set; }
        public string Def1 { get; set; }
        public string Def2 { get; set; }
        public string Def3 { get; set; }
        public string Def4 { get; set; }
        public string Def5 { get; set; }
        
    }
}
