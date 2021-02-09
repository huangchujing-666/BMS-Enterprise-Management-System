/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       iproductcustomsresult
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.idal
    * 文 件 名：       iproductcustomsresult
    * 创建时间：       2015-06-29 11:10:14
    * 作    者：       lcg
    * 说    明：       海关商品报备
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.idal
{
    public interface iproductcustomsresult
    {
        /// <summary>
        /// 根据类别获取海关备案号
        /// </summary>
        /// <param name="cat2"></param>
        /// <returns></returns>
        string getCustomsTariffNo(string cat2);


        /// <summary>
        /// 根据货号获取产品商检备案号
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        string getCustomsProductNO(string scode);


        /// <summary>
        /// 根据ciqProductNo修改下载文件夹名
        /// </summary>
        /// <param name="ciqProductNo"></param>
        /// <returns></returns>
        string getCustomsProductPath(string ciqProductNo);

        /// <summary>
        /// 根据货号获取回执文件路径
        /// </summary>
        /// <param name="ciqProductNo"></param>
        /// <returns></returns>
        string getCustomsProductPath1(string scode);


        /// <summary>
        /// 更新报文名称
        /// </summary>
        /// <param name="scode">货号</param>
        /// <param name="name">报文名称</param>
        /// <returns></returns>
        string updateUploadName(string scode, string name);


        /// <summary>
        /// 判断是否上传过此商品
        /// </summary>
        /// <param name="scodes">货号集合</param>
        /// <returns></returns>
        Dictionary<string, string> getUploadStatus(string scodes);

        /// <summary>
        /// 获取此商品是否上传成功
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        Dictionary<string, string> getUploadStatus1(string scode);

        /// <summary>
        /// 根据货号获取商检审核是否成功
        /// </summary>
        /// <param name="scode"></param>
        /// <returns></returns>
        string getUploadCheck(string scode);

        /// <summary>
        /// 上传报文状态(更新数据表)
        /// </summary>
        /// <param name="scode">货号</param>
        /// <returns></returns>
        string updateUploadStatus(string scode);

        /// <summary>
        /// 上传报文状态(插入数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        string addUploadStatus(model.productCustomsResult resultStatus);

        /// <summary>
        /// 更新报文状态(商检--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        string updateUploadStatus(model.productCustomsResult resultStatus);

        /// <summary>
        /// 更新报文状态(商检审核--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        string updateUploadStatus1(model.productCustomsResult resultStatus);

        /// <summary>
        /// 更新报文状态(联邦--更新数据表)
        /// </summary>
        /// <param name="resultStatus"></param>
        /// <returns></returns>
        string updateUploadStatus2(model.productCustomsResult resultStatus);


    }
}
