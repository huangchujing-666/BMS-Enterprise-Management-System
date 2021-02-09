/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ordercustomsresultdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       ordercustomsresultdal
    * 创建时间：       2015-06-27 14:26:00
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class ordercustomsresultdal:dataoperating,iordercustomsresult
    {
        /// <summary>
        /// 判断是否生成过此商品报文
        /// </summary>
        /// <param name="orderIds">订单ID集合</param>
        /// <returns>订单ID和报文名称的键值对</returns>
        public Dictionary<string, string> getUploadStatus(string orderIds)
        {
            string[] s = orderIds.Split(',');
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            List<model.orderCustomsResult> list = (from c in context.orderCustomsResult where s.Contains(c.SJOrgOrderChildId) select c).ToList();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dic.Add(item.SJOrgOrderChildId.ToLower(), item.def1);
            }

            return dic;
        }

        /// <summary>
        /// 获取此商品是否上传成功(商检)
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>订单ID和上传状态成功失败的键值对</returns>
        public Dictionary<string, string> getUploadStatus1(string orderId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            List<model.orderCustomsResult> list = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == orderId select c).ToList();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dic.Add(item.SJOrgOrderChildId.ToLower(), item.SJstatus);
            }

            return dic;
        }

        /// <summary>
        /// 获取此商品是否上传成功(海关)
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>订单ID和上传状态成功失败的键值对</returns>
        public Dictionary<string, string> getUploadStatus2(string orderId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            List<model.orderCustomsResult> list = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == orderId select c).ToList();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dic.Add(item.SJOrgOrderChildId.ToLower(), item.HGstatus);
            }

            return dic;
        }


        /// <summary>
        /// 根据货号获取订单商检报关是否成功(商检订单)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getSJUploadCheck(string orderId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string p = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == orderId select c.SJOrgStatus).SingleOrDefault();
            //string p = from c in context.productCustomsResult where c.productScode == scode select c.RegStatus;
            return p;
        }

        /// <summary>
        /// 根据货号获取订单海关报关是否成功(海关订单)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string getHGUploadCheck(string orderId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string p = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == orderId select c.SJstatus).SingleOrDefault();
            //string p = from c in context.productCustomsResult where c.productScode == scode select c.RegStatus;
            return p;
        }

        /// <summary>
        /// 更新报文名称
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <param name="name">报文名称</param>
        /// <returns></returns>
        public string updateUploadName(string orderId, string name)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context
                 = new model.pbxdatasourceDataContext();
            var p = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == orderId select c).SingleOrDefault();
            p.def1 = name;
            p.SJOrgOrderChildId = orderId;
            try
            {
                context.SubmitChanges();
                s = "更新成功";
            }
            catch (Exception ex)
            {
                s = "更新失败";
            }

            return s;
        }



        /// <summary>
        /// 上传报文状态(插入数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string addUploadStatus(model.orderCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                context.orderCustomsResult.InsertOnSubmit(resultStatus);
                context.SubmitChanges();

                s = "添加成功";
            }
            catch {
                s = "添加失败";
            }
            return s;
        }

        /// <summary>
        /// 更新报文状态(商检--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus(model.orderCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == resultStatus.SJOrgOrderChildId select c).SingleOrDefault();
            //SJOrgReturnTime,SJOrgStatus,SJOrgNotes,SJstatus
            p.SJOrgReturnTime = resultStatus.SJOrgReturnTime;
            p.SJOrgStatus = resultStatus.SJOrgStatus;
            p.SJOrgNotes = resultStatus.SJOrgNotes;
            p.SJstatus = resultStatus.SJstatus;
            try
            {
                context.SubmitChanges();
                s = "更新商检成功";
            }
            catch {
                s = "更新商检失败";
            }
            return s;
        }

        /// <summary>
        /// 更新报文状态(海关--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus1(model.orderCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == resultStatus.SJOrgOrderChildId select c).SingleOrDefault();
            //HGReturnDate,HGReturnCode,HGReturnInfo,HGAttachedFlag,HGstatus
            p.HGReturnDate = resultStatus.HGReturnDate;
            p.HGReturnCode = resultStatus.HGReturnCode;
            p.HGReturnInfo = resultStatus.HGReturnInfo;
            p.HGAttachedFlag = resultStatus.HGAttachedFlag;
            p.HGstatus = resultStatus.HGstatus;
            try
            {
                context.SubmitChanges();
                s = "更新海关成功";
            }
            catch
            {
                s = "更新海关失败";
            }
            return s;
        }


        /// <summary>
        /// 更新报文状态(联邦--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus2(model.orderCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.orderCustomsResult where c.SJOrgOrderChildId == resultStatus.SJOrgOrderChildId select c).SingleOrDefault();
            if (p==null)
            {
                return "更新联邦失败,无订单(" + resultStatus.SJOrgOrderChildId + ")信息记录";
            }
            //BBCReturnData,BBCask,BBCmessage,BBCorderCode,BBCerrorMessage,BBCstatus
            p.BBCReturnData = resultStatus.BBCReturnData;
            p.BBCask = resultStatus.BBCask;
            p.BBCmessage = resultStatus.BBCmessage;
            p.BBCorderCode = resultStatus.BBCorderCode;
            p.BBCerrorMessage = resultStatus.BBCerrorMessage;
            p.BBCstatus = resultStatus.BBCstatus;


            try
            {
                context.SubmitChanges();
                s = "更新联邦成功";
            }
            catch
            {
                s = "更新联邦失败";
            }
            return s;
        }


        /// <summary>
        /// 查询状态
        /// </summary>
        /// <param name="childorderId">子订单</param>
        /// <returns></returns>
        public DataTable getOrderReportStatus(string childorderId)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = from c in context.orderCustomsResult where c.SJOrgOrderChildId == childorderId select c;
            DataTable dt = LinqToDataTable.LINQToDataTable(p);
            return dt;
        }
    }
}
