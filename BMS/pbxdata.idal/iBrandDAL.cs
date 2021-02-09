using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface iBrandDAL
    {
        string connectionString { get; }
        List<BrandModel> SelectBrand();
    }
}
