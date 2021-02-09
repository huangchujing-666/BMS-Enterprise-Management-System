using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace pbxdata.idal
{
    public interface iSeason
    {
        /// <summary>
        /// 获取供应商季节
        /// </summary>
        /// <returns></returns>
        DataTable SearchSeasonVen(Dictionary<string, string> Dic, int page, int pages, out string counts);


        /// <summary>
        /// 更新供应商季节
        /// </summary>
        /// <returns></returns>
        string UpdateSeasonVen(Dictionary<string, string> dic, int UserId);

        /// <summary>
        /// 删除供应商季节
        /// </summary>
        string DeleteSeasonVen(string Id, int UserId);
        /// <summary>
        /// 根据季节名称查询季节表
        /// </summary>
        /// <returns></returns>
        DataTable GetData(string Cat1);

        /// <summary>
        /// 获取季节表
        /// </summary>
        /// <returns></returns>
        DataTable OnSearch(Dictionary<string, string> dic, int page, int Selpages, out string counts);

        /// <summary>
        /// 添加季节名称
        /// </summary>
        /// <returns></returns>
        string AddSeason(string Cat1, int UserId);

        /// <summary>
        /// 更新季节
        /// </summary>
        /// <returns></returns>
        string UpdateSeason(string Id, string Cat1, int UserId);

        /// <summary>
        /// 删除季节
        /// </summary>
        /// <returns></returns>
        string DeleteSeason(string Cat1, int UserId);

    }
}
