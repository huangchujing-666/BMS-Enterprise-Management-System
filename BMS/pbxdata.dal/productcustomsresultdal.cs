/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       productcustomsresultdal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       productcustomsresultdal
    * 创建时间：       2015-06-29 11:14:14
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal 
{
    public class productcustomsresultdal : dataoperating, iproductcustomsresult
    {
        /// <summary>
        /// 根据类别获取海关备案号
        /// </summary>
        /// <param name="cat2"></param>
        /// <returns></returns>
        public string getCustomsTariffNo(string cat2)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            s = (from c in context.TypeIdToTariffNo where c.TypeNo == cat2 select c).SingleOrDefault().TariffNo;
            return s;        
        }


        /// <summary>
        /// 根据货号获取产品商检备案号
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string getCustomsProductNO(string scode)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            s = (from c in context.productCustomsResult where c.productScode == scode select c).SingleOrDefault().CIQGoodsNO;
            return s;
        }

        /// <summary>
        /// 根据ciqProductNo修改下载文件夹名
        /// </summary>
        /// <param name="ciqProductNo"></param>
        /// <returns></returns>
        public string getCustomsProductPath(string ciqProductNo)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            s = (from c in context.product where c.ciqProductNo == Convert.ToInt32(ciqProductNo) select c).SingleOrDefault().Scode;
            return s;
        }

        /// <summary>
        /// 根据货号获取回执文件路径
        /// </summary>
        /// <param name="ciqProductNo"></param>
        /// <returns></returns>
        public string getCustomsProductPath1(string scode)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var q = from c in context.productCustomsResult where c.productScode == scode select c;
            if (q.Count() > 0)
            {
                s = (from c in context.productCustomsResult where c.productScode == scode select c).SingleOrDefault().def1;
            }
            return s;
        }


        /// <summary>
        /// 判断是否上传过此商品
        /// </summary>
        /// <param name="scodes">货号集合</param>
        /// <returns></returns>
        public Dictionary<string, string> getUploadStatus(string scodes)
        {
            string[] s = scodes.Split(',');
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            List<model.productCustomsResult> list = (from c in context.productCustomsResult where s.Contains(c.productScode) select c).ToList();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dic.Add(item.productScode.ToLower(), item.def1);
            }
            
            return dic;            
        }

        /// <summary>
        /// 获取此商品是否上传成功
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        public Dictionary<string, string> getUploadStatus1(string scode)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            List<model.productCustomsResult> list = (from c in context.productCustomsResult where c.productScode == scode select c).ToList();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dic.Add(item.productScode.ToLower(), item.RegStatus);
            }

            return dic;
        }

        /// <summary>
        /// 根据货号获取商检审核是否成功
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string getUploadCheck(string scode)
        {
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            string p = (from c in context.productCustomsResult where c.productScode == scode select c.RegStatus).SingleOrDefault();
            //string p = from c in context.productCustomsResult where c.productScode == scode select c.RegStatus;
            return p;
        }


        /// <summary>
        /// 更新报文名称
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="name">报文名称</param>
        /// <returns></returns>
        public string updateUploadName(string scode, string name)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context
                 = new model.pbxdatasourceDataContext();
            var p = (from c in context.productCustomsResult where c.productScode == scode select c).SingleOrDefault();
            p.def1 = name;
            p.productScode = scode;
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
        public string addUploadStatus(model.productCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            try
            {
                context.productCustomsResult.InsertOnSubmit(resultStatus);
                context.SubmitChanges();

                s = "添加成功";
            }
            catch
            {
                s = "添加失败";
            }
            return s;
        }

        /// <summary>
        /// 上传报文状态(更新数据表)
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        public string updateUploadStatus(string scode) 
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.productCustomsResult where c.productScode == scode select c).SingleOrDefault();
            //p.productScode = scode;
            p.SJstatus = "3";
            p.SJOrgSendTime = DateTime.Now;

            try
            {
                context.SubmitChanges();
                s = "更新商检成功";
            }
            catch
            {
                s = "更新商检失败";
            }
            return s;
        }


        /// <summary>
        /// 更新报文状态(商检--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus(model.productCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.productCustomsResult where c.productScode == resultStatus.productScode select c).SingleOrDefault();
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
            catch
            {
                s = "更新商检失败";
            }
            return s;
        }

        /// <summary>
        /// 更新报文状态(商检审核--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus1(model.productCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.productCustomsResult where c.productScode == resultStatus.productScode select c).SingleOrDefault();
            if (p==null)
            {
                return "货号为" + resultStatus.productScode + "的商品未生成过报文";
            }
            p.CIQGoodsNO = resultStatus.CIQGoodsNO;
            p.RegStatus = resultStatus.RegStatus;
            p.RegNotes = resultStatus.RegNotes;

            try
            {
                context.SubmitChanges();
                s = "更新商检成功";
            }
            catch(Exception ex)
            {
                s = ex.Message;
                //s = "更新商检失败";
            }
            return s;
        }

        /// <summary>
        /// 更新报文状态(联邦--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus2(model.productCustomsResult resultStatus)
        {
            string s = string.Empty;
            model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
            var p = (from c in context.productCustomsResult where c.productScode == resultStatus.productScode select c).SingleOrDefault();
            p.BBCask = resultStatus.BBCask;
            p.BBCerrorMessage = resultStatus.BBCerrorMessage;
            p.BBCmessage = resultStatus.BBCmessage;
            p.BBCReturnData = resultStatus.BBCReturnData;
            p.BBCskuNo = resultStatus.BBCskuNo;

            try
            {
                context.SubmitChanges();
                s = "更新商检成功";
            }
            catch
            {
                s = "更新商检失败";
            }
            return s;
        }
    }
}
