/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       productcustomresultbll
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.bll
    * 文 件 名：       productcustomresultbll
    * 创建时间：       2015-06-29 11:22:33
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.dal;
using pbxdata.dalfactory;
using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class productcustomresultbll : dataoperatingbll
    {

        iproductcustomsresult dal = (iproductcustomsresult)ReflectFactory.CreateIDataOperatingByReflect("productcustomsresultdal");


        /// <summary>
        /// 根据类别获取海关备案号
        /// </summary>
        /// <param name="cat2"></param>
        /// <returns></returns>
        public string getCustomsTariffNo(string cat2)
        {
            return dal.getCustomsTariffNo(cat2);
        }


        /// <summary>
        /// 根据货号获取产品商检备案号
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string getCustomsProductNO(string scode)
        {
            return dal.getCustomsProductNO(scode);
        }

        /// <summary>
        /// 根据ciqProductNo修改下载文件夹名
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string getCustomsProductPath(string ciqProductNo)
        {
            return dal.getCustomsProductPath(ciqProductNo);
        }

        /// <summary>
        /// 根据货号获取回执文件路径
        /// </summary>
        /// <param name="ciqProductNo"></param>
        /// <returns></returns>
        public string getCustomsProductPath1(string scode)
        {
            return dal.getCustomsProductPath1(scode);
        }

        /// <summary>
        /// 根据货号获取商检审核是否成功
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        public string getUploadCheck(string scode)
        {
            return dal.getUploadCheck(scode);
        }

        /// <summary>
        /// 上传报文状态(插入数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string addUploadStatus(model.productCustomsResult resultStatus)
        {
            return dal.addUploadStatus(resultStatus);
        }


        /// <summary>
        /// 更新报文名称
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="name">报文名称</param>
        /// <returns></returns>
        public string updateUploadName(string scode, string name)
        {
            return dal.updateUploadName(scode, name);
        }


        /// <summary>
        /// 判断是否上传过此商品
        /// </summary>
        /// <param name="scodes">货号集合</param>
        /// <returns></returns>
        public Dictionary<string, string> getUploadStatus(string scodes)
        {
            return dal.getUploadStatus(scodes);
        }

        /// <summary>
        /// 获取此商品是否上传成功
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        public Dictionary<string, string> getUploadStatus1(string scode)
        {
            return dal.getUploadStatus1(scode);
        }


        /// <summary>
        /// 上传报文状态(更新数据表)
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        public string updateUploadStatus(string scode)
        {
            return dal.updateUploadStatus(scode);
        }
       

        /// <summary>
        /// 更新报文状态(商检--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus(model.productCustomsResult resultStatus)
        {
            return dal.updateUploadStatus(resultStatus);
        }

        /// <summary>
        /// 更新报文状态(商检审核--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus1(model.productCustomsResult resultStatus)
        {
            return dal.updateUploadStatus1(resultStatus);
        }

        /// <summary>
        /// 更新报文状态(联邦--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        public string updateUploadStatus2(model.productCustomsResult resultStatus)
        {
            return dal.updateUploadStatus2(resultStatus);
        }
    }
}
