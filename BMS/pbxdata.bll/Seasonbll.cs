using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class Seasonbll:dataoperatingbll
    {
        iSeason dal = (iSeason)ReflectFactory.CreateIDataOperatingByReflect("Seasondal");
        /// <summary>
        /// 获取供应商季节表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchSeasonVen(Dictionary<string, string> dic, int page, int Selpages, out string counts)
        {
            return dal.SearchSeasonVen(dic, page, Selpages, out counts);
        }
        /// <summary>
        /// 更新供应商季节表
        /// </summary>
        /// <returns></returns>
        public string UpdateSeasonVen(Dictionary<string, string> dic, int UserId)
        {
            return dal.UpdateSeasonVen(dic, UserId);
        }
        /// <summary>
        /// 删除供应商季节
        /// </summary>
        public string DeleteSeasonVen(string Id, int UserId)
        {
            return dal.DeleteSeasonVen(Id, UserId);
        }
        /// <summary>
        /// 根据季节名称查询季节表
        /// </summary>
        /// <returns></returns>
        public DataTable GetData(string Cat1)
        {
            return dal.GetData(Cat1);
        }
        /// <summary>
        /// 获取季节表
        /// </summary>
        /// <returns></returns>
        public DataTable OnSearch(Dictionary<string, string> dic, int page, int Selpages, out string counts)
        {
            return dal.OnSearch(dic, page, Selpages, out counts);
        }
         /// <summary>
        /// 添加季节名称
        /// </summary>
        /// <returns></returns>
        public string AddSeason(string Cat1,int UserId)
        {
            return dal.AddSeason(Cat1, UserId);
        }
        /// <summary>
        /// 更新季节
        /// </summary>
        /// <returns></returns>
        public string UpdateSeason(string Id, string Cat1, int UserId)
        {
            return dal.UpdateSeason(Id,Cat1, UserId);
        }
        /// <summary>
        /// 删除季节
        /// </summary>
        /// <returns></returns>
        public string DeleteSeason(string Cat1, int UserId)
        {
            return dal.DeleteSeason(Cat1, UserId);
        }
    }
}
