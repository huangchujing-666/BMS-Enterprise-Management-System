/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       ApiOrderController
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.web.Controllers
    * 文 件 名：       ApiOrderController
    * 创建时间：       2015/06/01 16:22:40
    * 作    者：       lcg
    * 说    明：       海关商品报备(报备文件保存：http://112.74.74.98:8085/661105_20150519151619390.xml)    
    *                   (订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
    * 修改时间：
    * 修 改 人：
*************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace pbxdata.web.Controllers
{
    public class CustomsController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        #region 商检、海关报文名称前缀
        /// <summary>
        /// 海关
        /// </summary>
        public enum msgType
        {
            /// <summary>
            /// 订单备案报文
            /// </summary>
            p1 = 880020
        }


        /// <summary>
        /// 商检
        /// </summary>
        public enum ciqMsgType
        {
            /// <summary>
            /// 商品备案报文
            /// </summary>
            p1 = 661105,
            /// <summary>
            /// 电商交易订单报文
            /// </summary>
            p2 = 661101,
            /// <summary>
            /// 支付信息上报报文
            /// </summary>
            p3 = 661107,
            /// <summary>
            /// 物流信息上报报文
            /// </summary>
            p4 = 661108
        }
        #endregion


        #region 商品商检报备
        /// <summary>
        /// 生成商品报文
        /// </summary>
        /// <returns></returns>
        public string createSJProductReport()
        {

            bll.ProductBll Productbll = new bll.ProductBll();
            bll.BrandBLL brandbll = new bll.BrandBLL();
            bll.productcustomresultbll productcustomresultBll = new bll.productcustomresultbll();
            model.product MdProduct = new model.product();
            string s = string.Empty;
            string fileName = string.Empty;
            string fileLocalUrl = string.Empty;
            string brandFullName = string.Empty;
            string scode = string.Empty;
            string success = string.Empty; //数据表更新成功
            string fail = string.Empty; //数据表更新失败

            #region 如果文件夹存在报文，则删除这些报文
            try
            {
                //string[] ss = Directory.GetFiles(@"D:\CustomsFTPFile\sjProduct\in\");                                               //(本地)       
                string[] ss = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/sjProduct/in/", "fj", "123456");                   //(ftp，负载均衡)

                if (ss != null && ss.Length > 0)
                {
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(ss[i]))
                        {
                            FileInfo info = new FileInfo(ss[i]);
                            info.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #endregion


            scode = Request.Form["scode"].ToString();
            scode = scode.Trim(',');
            string[] scodes = helpcommon.StrSplit.StrSplitData(scode, ','); //处理批量scode

            #region 判断是否上传过此商品
            //string[] scodeExists = productcustomresultBll.getUploadStatus(scode);
            Dictionary<string, string> dic = productcustomresultBll.getUploadStatus(scode);
            string[] scodeExists = new string[dic.Count];
            int u = 0;
            foreach (var item in dic)
            {
                scodeExists[u] = item.Key;
                u++;
            }
            #endregion


            for (int h = 0; h < scodes.Length; h++)
            {
                ///如果货号中包含yy，那么说明此货号为前期报备错误的货号，需去掉yy重新报备
                string newScode = scodes[h].Substring(scodes[h].Length - 2, 2).Contains("yy") == true ? scodes[h].Replace("yy", "") : scodes[h];

                //MdProduct = Productbll.getProductScode(scodes[h]);
                MdProduct = Productbll.getProductScode(newScode);
                brandFullName = brandbll.getBrandFullName(MdProduct.Cat);
                if (MdProduct == null)
                {
                    return "商品信息有误";
                }

                XmlDocument doc = new XmlDocument();
                //加入XML的声明段落
                XmlNode node = doc.CreateXmlDeclaration("1.0", "gb2312", null);
                doc.AppendChild(node);

                //创建根目录
                XmlElement el = doc.CreateElement("Root");
                doc.AppendChild(el);

                #region 报文头
                XmlNode xnode = doc.CreateElement(string.Empty, "Head", string.Empty);
                el.AppendChild(xnode);

                XmlElement xnode1 = doc.CreateElement(string.Empty, "MessageID", string.Empty);
                //xnode1.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqProductNo);//报文编号
                xnode1.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.Scode);//报文编号
                xnode.AppendChild(xnode1);
                XmlElement xnode2 = doc.CreateElement(string.Empty, "MessageType", string.Empty);
                xnode2.InnerText = "661105";//报文类型
                xnode.AppendChild(xnode2);
                XmlElement xnode3 = doc.CreateElement(string.Empty, "Sender", string.Empty);
                //xnode3.InnerText = "IE150212852866";//报文发送者标识
                xnode3.InnerText = "1000000992";//报文发送者标识
                xnode.AppendChild(xnode3);
                XmlElement xnode4 = doc.CreateElement(string.Empty, "Receiver", string.Empty);
                xnode4.InnerText = "CBE_CBBPPS_000001";//报文接收人标识
                xnode.AppendChild(xnode4);
                XmlElement xnode5 = doc.CreateElement(string.Empty, "SendTime", string.Empty);
                xnode5.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");//发送时间
                xnode.AppendChild(xnode5);
                XmlElement xnode6 = doc.CreateElement(string.Empty, "FunctionCode", string.Empty);
                //xnode6.InnerText = "";//业务类型
                xnode.AppendChild(xnode6);
                XmlElement xnode7 = doc.CreateElement(string.Empty, "Version", string.Empty);
                xnode7.InnerText = "1.0";//版本号
                xnode.AppendChild(xnode7);
                #endregion

                #region 报文体
                XmlNode nodeContent = doc.CreateElement(string.Empty, "Body", string.Empty);
                el.AppendChild(nodeContent);

                XmlNode nodeContent1 = doc.CreateElement(string.Empty, "GOODSRECORD", string.Empty);
                nodeContent.AppendChild(nodeContent1);

                XmlNode nodeContent2 = doc.CreateElement(string.Empty, "Record", string.Empty);
                nodeContent1.AppendChild(nodeContent2);

                XmlNode Content1 = doc.CreateElement(string.Empty, "CargoBcode", string.Empty);
                Content1.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqProductNo);//商品申请编号
                nodeContent2.AppendChild(Content1);
                XmlNode Content2 = doc.CreateElement(string.Empty, "Ciqbcode", string.Empty);
                Content2.InnerText = "000067";//检验检疫机构代码
                nodeContent2.AppendChild(Content2);
                XmlNode Content3 = doc.CreateElement(string.Empty, "CbeComcode", string.Empty);
                Content3.InnerText = "1000000992";//跨境电商企业备案编号
                nodeContent2.AppendChild(Content3);
                XmlNode Content4 = doc.CreateElement(string.Empty, "Remark", string.Empty);
                //Content4.InnerText = "CUST000000000000000000038143";//备注
                nodeContent2.AppendChild(Content4);
                XmlNode Content411 = doc.CreateElement(string.Empty, "Editccode", string.Empty);
                Content411.InnerText = "1000000992";//制单企业编号
                nodeContent2.AppendChild(Content411);
                XmlNode Content5 = doc.CreateElement(string.Empty, "ApplyDate", string.Empty);
                Content5.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");//申请日期
                nodeContent2.AppendChild(Content5);
                XmlNode Content6 = doc.CreateElement(string.Empty, "CARGOLIST", string.Empty);
                nodeContent2.AppendChild(Content6);
                XmlNode Content61 = doc.CreateElement(string.Empty, "Record", string.Empty);
                Content6.AppendChild(Content61);

                XmlNode c1 = doc.CreateElement(string.Empty, "Gcode", string.Empty);
                //c1.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.Scode);   //货号
                c1.InnerText = helpcommon.ParmPerportys.GetStrParms(scodes[h].ToString());   //货号
                Content61.AppendChild(c1);
                XmlNode c2 = doc.CreateElement(string.Empty, "Gname", string.Empty);
                c2.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.Cdescript);//商品名称
                Content61.AppendChild(c2);
                XmlNode c3 = doc.CreateElement(string.Empty, "Spec", string.Empty);
                c3.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqSpec);//规格型号(module)
                Content61.AppendChild(c3);
                XmlNode c4 = doc.CreateElement(string.Empty, "Hscode", string.Empty);
                c4.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqHSNo);//商品HS编码
                Content61.AppendChild(c4);
                XmlNode c5 = doc.CreateElement(string.Empty, "Unit", string.Empty);
                c5.InnerText = MdProduct.QtyUnit; //计量单位(最小)
                Content61.AppendChild(c5);
                XmlNode c6 = doc.CreateElement(string.Empty, "GoodsBarcode", string.Empty);
                //c6.InnerText = "";//商品条形码
                Content61.AppendChild(c6);
                XmlNode c7 = doc.CreateElement(string.Empty, "GoodsDesc", string.Empty);
                //c7.InnerText = "";//商品描述
                Content61.AppendChild(c7);
                XmlNode c8 = doc.CreateElement(string.Empty, "Remark", string.Empty);
                //c8.InnerText = "";//备注
                Content61.AppendChild(c8);
                XmlNode c9 = doc.CreateElement(string.Empty, "ComName", string.Empty);
                //c9.InnerText = "";//生产企业名称
                Content61.AppendChild(c9);
                XmlNode c10 = doc.CreateElement(string.Empty, "Brand", string.Empty);
                c10.InnerText = helpcommon.ParmPerportys.GetStrParms(brandFullName);//品牌
                Content61.AppendChild(c10);
                XmlNode c11 = doc.CreateElement(string.Empty, "AssemCountry", string.Empty);
                c11.InnerText = MdProduct.ciqAssemCountry.Contains("-") ? helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqAssemCountry.Split(new char[] { '-' })[0].ToString()) : string.Empty;//原产国/地区(属性表)ciqAssemCountry
                Content61.AppendChild(c11);
                XmlNode c12 = doc.CreateElement(string.Empty, "Ingredient", string.Empty);
                //c12.InnerText = "";
                Content61.AppendChild(c12);
                XmlNode c13 = doc.CreateElement(string.Empty, "Additiveflag", string.Empty);
                //c13.InnerText = "";
                Content61.AppendChild(c13);
                XmlNode c14 = doc.CreateElement(string.Empty, "Poisonflag", string.Empty);
                //c14.InnerText = "";
                Content61.AppendChild(c14);
                #endregion

                try
                {
                    fileName = ciqMsgType.p1.GetHashCode().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);// +"-" + scodes[h];
                    fileLocalUrl = "D:\\CustomsFTPFile\\sjProduct\\in\\" + fileName + ".xml";
                    doc.Save(fileLocalUrl);                                                                                   //(本地)

                    helpcommon.FtpHelp.UploadFtp(@"ftp://192.168.1.124/sjProduct/in/", "fj", "123456", fileLocalUrl);         //(ftp，负载均衡,本地生成，然后上传FTP)
                    s += scodes[h] + ",";

                    if (!scodeExists.Contains(scodes[h].ToLower()))
                    {
                        model.productCustomsResult mdProductCustomsResult = new model.productCustomsResult();
                        mdProductCustomsResult.productScode = scodes[h];
                        mdProductCustomsResult.def1 = fileName; //报文名称（无后缀）

                        string result = productcustomresultBll.addUploadStatus(mdProductCustomsResult);
                        if (result.Contains("成功"))
                        {
                            success += scodes[h] + ",";
                        }
                        else
                        {
                            fail += scodes[h] + ",";
                        }
                    }
                    else
                    {
                        string result = productcustomresultBll.updateUploadName(scodes[h], fileName);
                        if (result.Contains("成功"))
                        {
                            success += scodes[h] + ",";
                        }
                        else
                        {
                            fail += scodes[h] + ",";
                        }
                    }
                }
                catch (Exception ex)
                {
                    s += "执行到商品（" + scodes[h] + "）出现异常;error:" + ex.Message;
                }

            }
            if (!string.IsNullOrWhiteSpace(s))
            {
                s += "报文生成成功;";
            }
            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "报文生成成功，数据表更新成功;";
            }
            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "报文生成成功，数据表更新失败;";
            }
            MdProduct = null;
            brandbll = null;
            Productbll = null;
            productcustomresultBll = null;

            return s + success + fail;
        }


        /// <summary>
        /// 上传商检商品报备
        /// </summary>
        /// <returns></returns>
        public string upLoadSJProductReport()
        {
            string success = string.Empty; //上传成功商品
            string fail = string.Empty; //上传失败商品
            string successStatus = string.Empty; //上传成功商品，状态修改是否成功
            string scode = string.Empty;
            string scode1 = string.Empty;
            string name = string.Empty; //报文名称
            string serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPSJIP"];
            string userName = System.Configuration.ConfigurationManager.AppSettings["SJUserName"];
            string password = System.Configuration.ConfigurationManager.AppSettings["SJPassword"];
            bll.productcustomresultbll productcustomresultBll = new bll.productcustomresultbll();

            scode = Request.Form["scode"].ToString();
            scode = scode.Trim(',');

            #region 判断是否生成过报文
            Dictionary<string, string> scodes = productcustomresultBll.getUploadStatus(scode); //判断是否生成过报文
            #endregion

            serverIP += "/4200.IMPBA.SWBCARGOBACK.REPORT/in/";

            //string[] files = Directory.GetFiles("D:\\CustomsFTPFile\\sjProduct\\in\\"); //获取所有生成的文件报表
            string[] files = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/sjProduct/in/", "fj", "123456");                   //(ftp，负载均衡); //获取所有生成的文件报表
            for (int h = 0; h < files.Length; h++)
            {
                try
                {
                    int i = 0;
                    //name = files[h].Replace("D:\\CustomsFTPFile\\sjProduct\\in\\", "").Replace(".xml", "").ToString();             //本地
                    name = files[h].Replace("D:\\CustomsFTPFile\\sjProduct\\in\\", "").Replace(".xml", "").ToString();               //(ftp，负载均衡);

                    #region 读取生成的商品报文，查取其报文货号
                    scode = (from c in scodes where c.Value == name select c.Key).SingleOrDefault();
                    if (string.IsNullOrWhiteSpace(scode))
                    {
                        continue;
                    }
                    #endregion

                    #region 根据货号读取商检审核状态，如果不为10，则继续上传报文，否则不予上传
                    Dictionary<string, string> dic = productcustomresultBll.getUploadStatus1(scode); //判断报文是否报关成功
                    #endregion

                    string sjState = string.IsNullOrWhiteSpace(dic[scode]) ? string.Empty : dic[scode].ToString();
                    if (sjState == "10")
                    {
                        continue;
                    }
                    else
                    {
                        //i = helpcommon.FtpHelp.UploadFtp(serverIP, userName, password, files[h]);
                        i = helpcommon.FtpHelp.UploadFtp(serverIP, userName, password, files[h]);

                        if (i == 0)
                        {
                            success += scode + ",";

                            string result = productcustomresultBll.updateUploadStatus(scode);
                            if (!result.Contains("成功"))
                            {
                                successStatus += scode + ",";
                            }
                        }
                        else
                        {
                            fail += scode + ",";
                        }
                    }
                }
                catch (Exception ex)
                {
                    success = "执行到商品（" + scode + "）出现异常;error:" + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(success) && !success.Contains("error"))
            {
            success += "上传成功;";
            }
            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "上传失败;";
            }
            if (!string.IsNullOrWhiteSpace(successStatus))
            {
                successStatus += "上传成功，状态修改失败";
            }
            if (string.IsNullOrWhiteSpace(success) && string.IsNullOrWhiteSpace(fail) && string.IsNullOrWhiteSpace(successStatus))
            {
                return "所有选中产品上传重复，请勿重复上传";
            }

            return success + fail + successStatus;
        }


        /// <summary>
        /// (商检)读取商品回执
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string readSJProductReport()
        {
            bll.productcustomresultbll productcustomresultBll = new bll.productcustomresultbll();
            string scode = string.Empty;
            string s = string.Empty;
            string name = string.Empty;
            string success = string.Empty; //数据库更新成功
            string fail = string.Empty;    //数据库更新失败
            string filesuccess = string.Empty;  //报文成功
            string filefail = string.Empty;     //报文失败
            string dbStatus = string.Empty;     //数据库商品商检状态（10：已接收（等待审核）20：申报失败）

            string messageId1 = string.Empty;//报文编号
            string sendtime1 = string.Empty;//回执时间
            string status1 = string.Empty; //申报状态
            string notes1 = string.Empty; //申报反馈信息
            //string[] ss = Directory.GetFiles(@"D:\CustomsFTPFile\sjProduct\out\");                                               //本地
            string[] ss = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/sjProduct/out/", "fj", "123456");                   //(ftp，负载均衡)
            if (ss.Length < 1)
            {
                return "暂无商品报文回执";
            }

            for (int i = 0; i < ss.Length; i++)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load(ss[i].ToString());
                doc.Load("ftp://192.168.1.124/sjProduct/out/" + ss[i].ToString());

                XmlNode node = doc.DocumentElement;
                XmlNodeList nodeList = node.ChildNodes;

                #region 遍历xml节点
                foreach (XmlNode item in nodeList)
                {
                    XmlNodeList nodeList2 = item.ChildNodes;
                    foreach (XmlNode item2 in nodeList2)
                    {
                        string item2Value = helpcommon.ParmPerportys.stringToLower(item2.InnerText);
                        if (item2.Name == "OrgMessageID")
                        {
                            messageId1 = item2Value;
                        }

                        if (item2.Name == "SendTime")  //回执时间
                        {
                            sendtime1 = item2Value;
                        }
                        if (item2.Name == "Status")  //申报状态
                        {
                            status1 = item2Value;
                        }
                        if (item2.Name == "Notes")  //申报反馈信息
                        {
                            notes1 = item2Value;
                        }
                    }
                }
                #endregion

                if (status1 == "10") //10：已接收（等待审核）20：申报失败
                {
                    filesuccess += messageId1 + ",";
                }
                else
                {
                    filefail += "(" + messageId1 + ")" + notes1 + ";";
                }

                #region 判断商品是否已报备成功
                dbStatus = productcustomresultBll.getUploadCheck(messageId1);
                if (dbStatus == "10")
                {
                    continue;
                }
                #endregion

                try
                {
                    model.productCustomsResult mdorderCustomsResult = new model.productCustomsResult();
                    //mdorderCustomsResult.SJOrgReturnTime = helpcommon.ParmPerportys.GetDateTimeParms(sendtime1);
                    mdorderCustomsResult.SJOrgStatus = status1;
                    mdorderCustomsResult.SJOrgNotes = notes1;
                    mdorderCustomsResult.SJstatus = status1 == "10" ? "1" : "0";//订单状态(0失败，1成功，2未上传)
                    mdorderCustomsResult.productScode = messageId1;
                    string result = productcustomresultBll.updateUploadStatus(mdorderCustomsResult);
                    if (result.Contains("成功"))
                    {
                        success += messageId1 + ",";

                        //删除本地商检订单回执
                        FileInfo info = new FileInfo(ss[i]);
                        info.Delete();
                    }
                    else
                    {
                        fail += messageId1 + ",";
                    }

                }
                catch (Exception ex)
                {
                    s += "商品" + messageId1 + "报关成功" + "(" + ex.Message + ")";
                }
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success = success.Trim(',') + "数据库更新成功;" + "\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail = fail.Trim(',') + "数据库更新失败;" + "\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filesuccess))
            {
                filesuccess = "\r\n" + filesuccess.Trim(',') + "报关成功;" + "\r\n";
            }

            productcustomresultBll = null;

            return s + filesuccess + filefail + success + fail;
        }


        /// <summary>
        /// (商检)读取商品审核回执(D:\CustomsFTPFile\sjProductAudit\out\)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string readSJCheckProductReport()
        {
            string s = string.Empty;
            string scode = string.Empty;
            scode = helpcommon.ParmPerportys.stringToLower(scode);
            string success = string.Empty; //成功
            string fail = string.Empty;     //失败
            string filesuccess = string.Empty; //数据库更新成功
            string filefail = string.Empty; //数据库更新失败
            string messageId1 = string.Empty;//报文编号
            string ciqgoodsno = string.Empty;//商品备案号
            string status1 = string.Empty; //申报状态
            string notes1 = string.Empty; //申报反馈信息
            string scode1 = string.Empty; //货号
            string dbStatus = string.Empty; //审核状态
            bll.productcustomresultbll productcustomresultBll = new bll.productcustomresultbll();

            //string[] ss = Directory.GetFiles(@"D:\CustomsFTPFile\sjProductAudit\out\");                                          //本地
            string[] ss = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/sjProduct/out/", "fj", "123456");                   //(ftp，负载均衡)

            if (ss.Length < 1)
            {
                return "暂无商品报文审核回执";
            }

            for (int i = 0; i < ss.Length; i++)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load(ss[i]);                                                                   //本地
                doc.Load("ftp://192.168.1.124/sjProduct/out/" + ss[i].ToString());                   //(ftp，负载均衡)

                XmlNodeList nodeListRecord = doc.GetElementsByTagName("Record");

                #region 遍历xml节点
                foreach (XmlNode item2 in nodeListRecord)
                {
                    XmlNodeList nodeList2 = item2.ChildNodes;

                    foreach (XmlNode item in nodeList2)
                    {
                        string item2Value = helpcommon.ParmPerportys.stringToLower(item.InnerText);

                        if (item.Name == "CargoBcode")
                        {
                            messageId1 = item2Value;
                        }
                        if (item.Name == "CIQGoodsNO")//最大50字符,商品审核通过时给的一个商品备案号。
                        {
                            ciqgoodsno = item2Value;
                        }
                        if (item.Name == "RegStatus")  //申报状态(最大4字符10:通过;20:不通过)
                        {
                            status1 = item2Value;
                        }
                        if (item.Name == "RegNotes")  //申报反馈信息
                        {
                            notes1 = item2Value;
                        }
                        if (item.Name == "Gcode")  //货号
                        {
                            scode1 = item2Value;
                        }
                    }
                }
                #endregion

                if (status1 == "10") //10：已接收（等待审核）20：申报失败
                {
                    filesuccess += messageId1 + ",";
                }
                else
                {
                    filefail += "(" + messageId1 + ")" + notes1 + ";";
                }

                #region 判断商品是否已报备成功
                dbStatus = productcustomresultBll.getUploadCheck(messageId1);
                if (dbStatus == "10")
                {
                    continue;
                }
                #endregion

                try
                {
                    model.productCustomsResult mdorderCustomsResult = new model.productCustomsResult();
                    mdorderCustomsResult.CIQGoodsNO = ciqgoodsno;
                    mdorderCustomsResult.RegStatus = status1;
                    mdorderCustomsResult.RegNotes = notes1;
                    mdorderCustomsResult.productScode = scode1;
                    
                    string result = productcustomresultBll.updateUploadStatus1(mdorderCustomsResult);

                    if (result.Contains("成功"))
                    {
                        success += scode1 + ",";

                        //删除本地商检订单回执
                        FileInfo info = new FileInfo(ss[i]);
                        info.Delete();
                    }
                    else
                    {
                        fail += scode1 + ",";
                    }
                }
                catch (Exception ex)
                {
                    s += "商品" + scode1 + "报关审核成功(" + ex.Message + ")";
                }
                
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success = success.Trim(',') + "数据库更新成功;" + "\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail = fail.Trim(',') + "数据库更新失败;" + "\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filesuccess))
            {
                filesuccess = "\r\n" + filesuccess.Trim(',') + "报关成功;" + "\r\n";
            }

            productcustomresultBll = null;
            return s + filesuccess + filefail + success + fail;
        }


        /// <summary>
        /// 获取商检商检回执,并下载到本地(D:\CustomsFTPFile\sjProduct\out\)
        /// </summary>
        /// <returns></returns>
        public string downLoadSJProductReport()
        {
            bll.productcustomresultbll productcustomresultBll = new bll.productcustomresultbll();
            string s = "下载成功";

            string serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPSJIP"]; //ftp地址
            string userName = System.Configuration.ConfigurationManager.AppSettings["SJUserName"]; //ftp用户名
            string password = System.Configuration.ConfigurationManager.AppSettings["SJPassword"]; //ftp密码

            serverIP += "/4200.IMPBA.SWBCARGOBACK.REPORT/out/";
            try
            {
                string[] ss = helpcommon.FtpHelp.GetFileList(serverIP, userName, password);
                if (ss == null)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }
                if (ss.Length <= 0)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }

                for (int i = 0; i < ss.Length; i++)
                {
                    string localUrl = @"D:\CustomsFTPFile\sjProduct\out\" + ss[i];
                    int m = helpcommon.FtpHelp.DownloadFtp(serverIP, userName, password, localUrl);
                    if (m == 0)
                    {
                        #region (ftp，负载均衡,本地生成，然后上传FTP)
                        int k = helpcommon.FtpHelp.UploadFtp(@"ftp://192.168.1.124/sjProduct/out/", "fj", "123456", localUrl);         //(ftp，负载均衡,本地生成，然后上传FTP)

                        if (k==0)
                        {
                            //下载成功，删除此xml
                            helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                        }
                        #endregion

                        #region 本地
                        ////下载成功，删除此xml
                        //helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                        #endregion
                    }
                    else
                    {
                        s = "部分商检商品回执下载完成";
                    }
                }
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            System.Threading.Thread.Sleep(2000); //延时2秒

            return s;
        }


        /// <summary>
        /// 获取商品商检审核回执,并下载到本地
        /// </summary>
        /// <returns></returns>
        public string downLoadSJCheckProductReport()
        {
            string s = "下载成功";

            string serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPSJIP"]; //ftp地址
            string userName = System.Configuration.ConfigurationManager.AppSettings["SJUserName"]; //ftp用户名
            string password = System.Configuration.ConfigurationManager.AppSettings["SJPassword"]; //ftp密码

            serverIP += "/4200.IMPBA.SWBCARGOBACK.AUDIT/out/";
            try
            {
                string[] ss = helpcommon.FtpHelp.GetFileList(serverIP, userName, password);
                if (ss == null)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }
                if (ss.Length <= 0)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }

                for (int i = 0; i < ss.Length; i++)
                {
                    string localUrl = @"D:\CustomsFTPFile\sjProductAudit\out\" + ss[i];
                    int m = helpcommon.FtpHelp.DownloadFtp(serverIP, userName, password, localUrl);
                    if (m == 0)
                    {
                        #region (ftp，负载均衡,本地生成，然后上传FTP)
                        int k = helpcommon.FtpHelp.UploadFtp(@"ftp://192.168.1.124/sjProduct/out/", "fj", "123456", localUrl);         //(ftp，负载均衡,本地生成，然后上传FTP)

                        if (k == 0)
                        {
                            //下载成功，删除此xml
                            helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                        }
                        #endregion

                        //下载成功，删除此xml
                        helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                    }
                    else
                    {
                        s = "部分商检商品回执下载完成";
                    }
                }
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            System.Threading.Thread.Sleep(2000); //延时2秒

            return s;
        }
        #endregion


        #region 订单商检报备

        /*数据库中为什么要村报文后缀（因为在上传报文的时候需知道哪些货号上传成功，1、要么读取报文中xml，提取货号。2、要么把报文名称存入数据库，然后比对报文名称得到货号）*/ 
        

        /// <summary>
        /// (商检)生成订单报文 
        /// </summary>
        /// <returns></returns>
        public string createSJOrderReport()
        {
            string success = string.Empty; //数据表更新成功
            string fail = string.Empty;    //数据表更新失败
            string filesuccess = string.Empty; //报文创建成功
            string filefail = string.Empty;    //报文创建失败
            string orderId = string.Empty; //订单ID
            string s = string.Empty;
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            model.apiOrder MdApiOrder = new model.apiOrder();
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            bll.apiorderpaydetailsbll apiorderpaydetailsBll = new bll.apiorderpaydetailsbll();
            List<model.apiOrderDetails> MdApiOrderDetails = new List<model.apiOrderDetails>();

            #region 如果文件夹存在报文，则删除这些报文
            try
            {
                //string[] ss = Directory.GetFiles(@"D:\CustomsFTPFile\sjOrder\in\");
                string[] ss = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/sjOrder/in/", "fj", "123456");                   //(ftp，负载均衡)

                if (ss != null && ss.Length > 0)
                {
                    for (int i = 0; i < ss.Length; i++)
                    {
                        FileInfo info = new FileInfo(ss[i]);
                        info.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #endregion

            orderId = Request.Form["orderId"].ToString();
            orderId = orderId.Trim(',');
            string[] orderIds = helpcommon.StrSplit.StrSplitData(orderId, ','); //处理批量orderId


            for (int h = 0; h < orderIds.Length; h++)
            {
                string[] orderIdPayId = helpcommon.StrSplit.StrSplitData(orderIds[h], '-');
                string oId = orderIdPayId[0].ToString();
                string pId = orderIdPayId[1].ToString();//支付单号（这里以pId为订单号报关海关，商检，支付，物流）

                MdApiOrder = apiorderBll.getOrderMsg(oId);
                if (MdApiOrder == null)
                {
                    return "订单信息有误";
                }

                MdApiOrderDetails = apiorderBll.getOrderDetailsMsg(oId, "");
                if (MdApiOrderDetails == null)
                {
                    return "订单信息有误";
                }
                
                XmlDocument doc = new XmlDocument();
                //加入XML的声明段落
                XmlNode node = doc.CreateXmlDeclaration("1.0", "gb2312", null);
                doc.AppendChild(node);

                //创建根目录
                XmlElement el = doc.CreateElement("ROOT");
                doc.AppendChild(el);

                #region 报文头
                XmlNode xnode = doc.CreateElement(string.Empty, "Head", string.Empty);
                el.AppendChild(xnode);

                XmlElement xnode1 = doc.CreateElement(string.Empty, "MessageID", string.Empty);
                //xnode1.InnerText = helpcommon.ParmPerportys.GetStrParms(MdApiOrder.orderId);//报文编号(报文唯一编号，最大50字符)
                xnode1.InnerText = helpcommon.ParmPerportys.GetStrParms(pId);//报文编号(报文唯一编号，最大50字符)
                xnode.AppendChild(xnode1);
                XmlElement xnode2 = doc.CreateElement(string.Empty, "MessageType", string.Empty);
                xnode2.InnerText = "661101";//报文类型
                xnode.AppendChild(xnode2);
                XmlElement xnode3 = doc.CreateElement(string.Empty, "Sender", string.Empty);
                //xnode3.InnerText = "IE150212852866";//报文发送者标识   1000000992
                xnode3.InnerText = "1000000992";//报文发送者标识   1000000992
                xnode.AppendChild(xnode3);
                XmlElement xnode4 = doc.CreateElement(string.Empty, "Receiver", string.Empty);
                xnode4.InnerText = "CBE_CBBPPS_000001";//报文接收人标识
                xnode.AppendChild(xnode4);
                XmlElement xnode5 = doc.CreateElement(string.Empty, "SendTime", string.Empty);
                xnode5.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");//发送时间
                xnode.AppendChild(xnode5);
                XmlElement xnode6 = doc.CreateElement(string.Empty, "FunctionCode", string.Empty);
                //xnode6.InnerText = "";//业务类型
                xnode.AppendChild(xnode6);
                XmlElement xnode7 = doc.CreateElement(string.Empty, "Version", string.Empty);
                xnode7.InnerText = "1.0";//版本号
                xnode.AppendChild(xnode7);
                #endregion

                #region 报文体
                XmlNode nodeContent = doc.CreateElement(string.Empty, "Body", string.Empty);
                el.AppendChild(nodeContent);

                XmlNode nodeContent1 = doc.CreateElement(string.Empty, "swbebtrade", string.Empty);
                nodeContent.AppendChild(nodeContent1);

                XmlNode nodeContent2 = doc.CreateElement(string.Empty, "Record", string.Empty);
                nodeContent1.AppendChild(nodeContent2);

                XmlNode Content1 = doc.CreateElement(string.Empty, "EntInsideNo", string.Empty);
                //Content1.InnerText = helpcommon.ParmPerportys.GetStrParms(MdApiOrder.orderId);//企业内部编号(订单编号？)
                Content1.InnerText = helpcommon.ParmPerportys.GetStrParms(pId);//企业内部编号(订单编号？)
                nodeContent2.AppendChild(Content1);
                XmlNode Content2 = doc.CreateElement(string.Empty, "Ciqbcode", string.Empty);
                Content2.InnerText = "000067";//检验检疫机构代码
                nodeContent2.AppendChild(Content2);
                XmlNode Content3 = doc.CreateElement(string.Empty, "CbeComcode", string.Empty);
                Content3.InnerText = "1000000992";//跨境电商企业备案编号
                nodeContent2.AppendChild(Content3);
                XmlNode Content4 = doc.CreateElement(string.Empty, "CbepComcode", string.Empty);
                Content4.InnerText = "1000000992";//电商平台企业备案编号
                nodeContent2.AppendChild(Content4);
                XmlNode Content5 = doc.CreateElement(string.Empty, "OrderStatus", string.Empty);
                //Content5.InnerText = helpcommon.ParmPerportys.GetStrParms(MdApiOrder.orderStatus);//订单状态（？）S-订单新增，C-订单取消；（目前要求为预付方式，预付成功后订单才提交，如未发货可取消，已发货则不可取消）
                if (MdApiOrder.orderStatus == 4)//(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
                {
                    Content5.InnerText = "S";//订单状态（？）
                }
                else if (MdApiOrder.orderStatus == 12)
                {
                    Content5.InnerText = "C";//订单状态（？）
                }
                nodeContent2.AppendChild(Content5);
                XmlNode Content6 = doc.CreateElement(string.Empty, "ReceiveName", string.Empty);
                Content6.InnerText = helpcommon.ParmPerportys.GetStrParms(MdApiOrder.realName);//收件人姓名
                nodeContent2.AppendChild(Content6);
                XmlNode Content7 = doc.CreateElement(string.Empty, "ReceiveAddr", string.Empty);
                Content7.InnerText = helpcommon.ParmPerportys.GetStrParms(MdApiOrder.buyNameAddress);//收件人地址
                nodeContent2.AppendChild(Content7);
                XmlNode Content8 = doc.CreateElement(string.Empty, "ReceiveNo", string.Empty);
                Content8.InnerText = MdApiOrder.def1;//收件人证件号
                nodeContent2.AppendChild(Content8);
                XmlNode Content9 = doc.CreateElement(string.Empty, "ReceivePhone", string.Empty);
                Content9.InnerText = helpcommon.ParmPerportys.GetStrParms(MdApiOrder.phone);//收件人电话
                nodeContent2.AppendChild(Content9);
                XmlNode Content10 = doc.CreateElement(string.Empty, "FCY", string.Empty);
                Content10.InnerText = double.Parse(MdApiOrder.itemPrice.ToString()).ToString("F2");//总货款
                nodeContent2.AppendChild(Content10);
                XmlNode Content11 = doc.CreateElement(string.Empty, "Fcode", string.Empty);
                Content11.InnerText = "CNY";//币种
                nodeContent2.AppendChild(Content11);
                XmlNode Content12 = doc.CreateElement(string.Empty, "Editccode", string.Empty);
                Content12.InnerText = "1000000992";//制单企业编号
                nodeContent2.AppendChild(Content12);
                XmlNode Content13 = doc.CreateElement(string.Empty, "DrDate", string.Empty);
                Content13.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");//下单日期
                nodeContent2.AppendChild(Content13);


                foreach (var item in MdApiOrderDetails)
                {
                    #region 获取商品信息
                    ///如果货号中包含yy，那么说明此货号为前期报备错误的货号，需去掉yy重新报备
                    string newScode = item.detailsScode.Substring(item.detailsScode.Length - 2, 2).Contains("yy") == true ? item.detailsScode.Replace("yy", "") : item.detailsScode;

                    bll.ProductBll Productbll = new bll.ProductBll();
                    //model.product MdProduct = Productbll.getProductScode(item.detailsScode); //货号
                    model.product MdProduct = Productbll.getProductScode(newScode); //货号
                    #endregion


                    XmlNode Content131 = doc.CreateElement(string.Empty, "swbebtradeg", string.Empty);
                    nodeContent2.AppendChild(Content131);
                    XmlNode Content1311 = doc.CreateElement(string.Empty, "Record", string.Empty);
                    Content131.AppendChild(Content1311);

                    //Gcode	商品货号
                    //Hscode	HS编码(海关编码)

                    XmlNode c1 = doc.CreateElement(string.Empty, "EntGoodsNo", string.Empty);
                    c1.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqProductNo);//商品序号
                    Content1311.AppendChild(c1);
                    XmlNode c98 = doc.CreateElement(string.Empty, "Gcode", string.Empty);  //商品货号
                    //c98.InnerText = MdProduct.Scode;
                    c98.InnerText = item.detailsScode;
                    Content1311.AppendChild(c98);
                    XmlNode c99 = doc.CreateElement(string.Empty, "Hscode", string.Empty);  //HS编码(海关编码)
                    c99.InnerText = MdProduct.ciqHSNo;
                    Content1311.AppendChild(c99);
                    XmlNode c2 = doc.CreateElement(string.Empty, "CiqGoodsNo", string.Empty);
                    //c2.InnerText = "";//商品备案编
                    Content1311.AppendChild(c2);
                    XmlNode c3 = doc.CreateElement(string.Empty, "CopGName", string.Empty);
                    c3.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.Cdescript);//商品名称
                    Content1311.AppendChild(c3);
                    XmlNode c4 = doc.CreateElement(string.Empty, "Brand", string.Empty);
                    c4.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.Cat);//品牌
                    Content1311.AppendChild(c4);
                    XmlNode c5 = doc.CreateElement(string.Empty, "Spec", string.Empty);
                    c5.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqSpec);//规格型号
                    Content1311.AppendChild(c5);
                    XmlNode c6 = doc.CreateElement(string.Empty, "Origin", string.Empty);
                    c6.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqAssemCountry);//产地
                    Content1311.AppendChild(c6);
                    XmlNode c7 = doc.CreateElement(string.Empty, "Qty", string.Empty);
                    c7.InnerText = helpcommon.ParmPerportys.GetStrParms(item.detailsSaleCount);//缺省：整数，可输入四位小数；商品购买数/重量
                    Content1311.AppendChild(c7);
                    XmlNode c8 = doc.CreateElement(string.Empty, "QtyUnit", string.Empty);
                    c8.InnerText = helpcommon.ParmPerportys.GetStrParms(MdProduct.QtyUnit);//商品购买数/重量的计量单位
                    Content1311.AppendChild(c8);
                    XmlNode c9 = doc.CreateElement(string.Empty, "DecPrice", string.Empty);
                    c9.InnerText = helpcommon.ParmPerportys.GetStrParms(double.Parse(item.detailsItemPrice.ToString()).ToString("F2"));//2位小数；该项商品的单价；单位：RMB；
                    Content1311.AppendChild(c9);
                    XmlNode c10 = doc.CreateElement(string.Empty, "DecTotal", string.Empty);
                    c10.InnerText = helpcommon.ParmPerportys.GetStrParms(double.Parse((item.detailsItemPrice * item.detailsSaleCount).ToString()).ToString("F2"));//该项商品的总价，二位小数； 单位：RMB；(含税)(商品总价+快递费用-优惠费用+税费)
                    //c10.InnerText = helpcommon.ParmPerportys.GetStrParms(double.Parse(((item.detailsItemPrice * item.detailsSaleCount) + item.detailsTaxPrice + item.detailsDeliveryPrice - MdApiOrder.favorablePrice).ToString()).ToString("F2"));//该项商品的总价，二位小数； 单位：RMB；(含税)(商品总价+快递费用-优惠费用+税费)
                    Content1311.AppendChild(c10);
                    XmlNode c11 = doc.CreateElement(string.Empty, "SellWebSite", string.Empty);
                    c11.InnerText = helpcommon.ParmPerportys.GetStrParms(item.def1);//销售网址//h5.bstapp.cn
                    Content1311.AppendChild(c11);
                    XmlNode c12 = doc.CreateElement(string.Empty, "Nots", string.Empty);
                    //c12.InnerText = " ";//商品的备注信息
                    Content1311.AppendChild(c12);
                }
                #endregion


                string fileName = ciqMsgType.p2.GetHashCode().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);
                string fileLocalUrl = @"D:\CustomsFTPFile\sjOrder\in\" + fileName + ".xml";

                #region 创建报文
                try
                {
                    doc.Save(fileLocalUrl);
                    helpcommon.FtpHelp.UploadFtp(@"ftp://192.168.1.124/sjOrder/in/", "fj", "123456", fileLocalUrl);         //(ftp，负载均衡,本地生成，然后上传FTP)

                    filesuccess += oId.ToString() + ","; //文件创建成功
                }
                catch (Exception ex)
                {
                    filefail += oId.ToString() + ex.Message + ","; //文件创建失败
                }
                #endregion

                #region 判断是否生成过此商品报文
                Dictionary<string, string> dic = ordercustomsresultBll.getUploadStatus(pId);
                string[] scodeExists = new string[dic.Count];
                int u = 0;
                foreach (var item in dic)
                {
                    scodeExists[u] = item.Key;
                    u++;
                }
                #endregion

                #region 更新数据库
                try
                {
                    //if (!scodeExists.Contains(oId.ToLower())) //判断是否生成过此商品报文
                    if (!scodeExists.Contains(pId)) //判断是否生成过此商品报文
                    {
                        //insert
                        model.orderCustomsResult MdorderCustomsResult = new model.orderCustomsResult();
                        //MdorderCustomsResult.SJOrgOrderChildId = oId.ToString();
                        MdorderCustomsResult.SJOrgOrderChildId = pId;
                        MdorderCustomsResult.SJstatus = "2";////商检订单状态(0失败，1成功，2未上传)
                        MdorderCustomsResult.HGstatus = "2";////海关订单状态(0失败，1成功，2未上传)
                        MdorderCustomsResult.BBCstatus = "2";////联邦订单状态(0失败，1成功，2未上传)
                        MdorderCustomsResult.def1 = fileName; //商检报文名称（无后缀）

                        string result = ordercustomsresultBll.addUploadStatus(MdorderCustomsResult);
                        if (result.Contains("成功"))
                        {
                            success += oId + ",";
                        }
                        else
                        {
                            fail += oId + ",";
                        }
                    }
                    else
                    {
                        //update
                        //string result = ordercustomsresultBll.updateUploadName(oId.ToString(), fileName);
                        string result = ordercustomsresultBll.updateUploadName(pId, fileName);
                        if (result.Contains("成功"))
                        {
                            success += oId + ",";
                        }
                        else
                        {
                            fail += oId + ",";
                        }
                    }
                }
                catch (Exception ex)
                {
                    s += "执行到订单(" + oId + "）出现异常;error:" + ex.Message;
                }
                #endregion

            }

            if (!string.IsNullOrWhiteSpace(filesuccess))
            {
                filesuccess += "报文创建成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filefail))
            {
                filefail += "报文创建失败;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "数据表更新成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "数据表更新失败;\r\n";
            }

            apiorderBll = null;
            ordercustomsresultBll = null;
            MdApiOrderDetails = null;
            MdApiOrder = null;

            return filesuccess + filefail + success + fail + s;
        }


        /// <summary>
        /// (商检)上传订单报备
        /// </summary>
        /// <returns></returns>
        public string upLoadSJOrderReport()
        {
            string orderId = string.Empty;
            string success = string.Empty;        //上传成功商品
            string fail = string.Empty;           //上传失败商品
            string s = string.Empty;  //异常信息
            string name = string.Empty;           //报文名称
            //string payId = string.Empty; //支付号
            string payId = string.Empty;//（这里以payId为订单号报关海关，商检，支付，物流）
            
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            bll.apiorderpaydetailsbll apiorderpaydetailsBll = new bll.apiorderpaydetailsbll();
            string serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPSJIP"];
            string userName = System.Configuration.ConfigurationManager.AppSettings["SJUserName"];
            string password = System.Configuration.ConfigurationManager.AppSettings["SJPassword"];
            
            orderId = Request.Form["orderId"].ToString();
            orderId = orderId.Trim(',');
            string[] ss = helpcommon.StrSplit.StrSplitData(orderId,',');


            string pId = string.Empty;
            for (int i = 0; i < ss.Length; i++)
            {
                string[] orderIdPayId = helpcommon.StrSplit.StrSplitData(ss[i].ToString(),'-');
                string oId = orderIdPayId[0].ToString();
                pId += orderIdPayId[1].ToString()+",";
            }
            pId = pId.Trim(',');
            
            #region 判断是否生成过报文
            Dictionary<string, string> orderIds = ordercustomsresultBll.getUploadStatus(pId); //判断是否生成过报文
            #endregion
            serverIP += "/4200.IMPBA.SWBEBTRADE.REPORT/in/";

            //string[] files = Directory.GetFiles("D:\\CustomsFTPFile\\sjOrder\\in\\"); //获取所有生成的文件报表
            string[] files = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/sjProduct/in/", "fj", "123456");                   //(ftp，负载均衡); //获取所有生成的文件报表
            for (int h = 0; h < files.Length; h++)
            {
                try
                {
                    int i = 0;
                    //name = files[h].Replace("D:\\CustomsFTPFile\\sjOrder\\in\\", "").Replace(".xml", "").ToString();
                    name = files[h].Replace("D:\\CustomsFTPFile\\sjOrder\\in\\", "").Replace(".xml", "").ToString();               //(ftp，负载均衡);

                    #region 读取生成的商品报文，查取其报文订单号(用报文名称和数据库存储的报文名称对比，获取订单号)
                    orderId = (from c in orderIds where c.Value == name select c.Key).SingleOrDefault();
                    if (string.IsNullOrWhiteSpace(orderId))
                    {
                        continue;
                    }
                    #endregion

                    #region 根据货号读取商检审核状态，如果不为1，则继续上传报文，否则不予上传
                    Dictionary<string, string> dic = ordercustomsresultBll.getUploadStatus1(orderId); //判断报文是否报关成功
                    #endregion

                    string sjState = string.IsNullOrWhiteSpace(dic[orderId]) ? string.Empty : dic[orderId].ToString();
                    if (sjState == "1")
                    {
                        s += orderId + "已上传成功，请勿重复上传;";
                        continue;
                    }
                    else
                    {
                        i = helpcommon.FtpHelp.UploadFtp(serverIP, userName, password, files[h]);

                        if (i == 0)
                        {
                            success += orderId + ",";
                        }
                        else
                        {
                            fail += orderId + ",";
                        }
                    }

                }
                catch (Exception ex)
                {
                    s += "执行到订单（" + orderId + "）出现异常;error:" + ex.Message + "\r\n";
                }
            }
            //if (!string.IsNullOrWhiteSpace(success) && !success.Contains("error"))
            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "上传成功;\r\n";
            }
            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "上传失败;\r\n";
            }

            return success + fail + s;
        }


        /// <summary>
        /// (商检)更新订单回执
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string readSJOrderReport()
        {
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            string orderId = string.Empty;
            string s = string.Empty;
            string name = string.Empty;
            string success = string.Empty;      //数据库更新成功
            string fail = string.Empty;         //数据库更新失败
            string filesuccess = string.Empty;  //报文成功
            string filefail = string.Empty;     //报文失败

            string messageId1 = string.Empty;//报文编号
            string sendtime1 = string.Empty;//回执时间
            string status1 = string.Empty; //申报状态（10：申报成功 20：申报失败）
            string notes1 = string.Empty; //申报反馈信息

            orderId = helpcommon.ParmPerportys.stringToLower(orderId);
            //string[] ss = Directory.GetFiles(@"D:\CustomsFTPFile\sjOrder\out\");
            string[] ss = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/sjProduct/out/", "fj", "123456");                   //(ftp，负载均衡)
            if (ss.Length < 1)
            {
                return "暂无订单报文回执";
            }

            for (int i = 0; i < ss.Length; i++)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load(ss[i]);
                doc.Load("ftp://192.168.1.124/sjProduct/out/" + ss[i].ToString());                   //(ftp，负载均衡)
                XmlNode node = doc.DocumentElement;
                XmlNodeList nodeList = node.ChildNodes;

                #region 遍历xml节点
                foreach (XmlNode item in nodeList)
                {
                    XmlNodeList nodeList2 = item.ChildNodes;
                    foreach (XmlNode item2 in nodeList2)
                    {
                        string item2Value = helpcommon.ParmPerportys.stringToLower(item2.InnerText);
                        if (item2.Name == "OrgMessageID")
                        {
                            messageId1 = item2Value;
                        }

                        if (item2.Name == "SendTime")  //回执时间
                        {
                            sendtime1 = item2Value;
                        }
                        if (item2.Name == "Status")  //申报状态
                        {
                            status1 = item2Value;
                        }
                        if (item2.Name == "Notes")  //申报反馈信息
                        {
                            notes1 = item2Value;
                        }
                    }
                }
                #endregion

                if (status1 == "10") //10：申报成功 20：申报失败
                {
                    filesuccess += messageId1 + ",";
                }
                else
                {
                    filefail += "(" + messageId1 + ")" + notes1 + ";";
                }

                try
                {
                    model.orderCustomsResult mdorderCustomsResult = new model.orderCustomsResult();
                    mdorderCustomsResult.SJOrgReturnTime = helpcommon.ParmPerportys.GetDateTimeParms(sendtime1);
                    mdorderCustomsResult.SJOrgStatus = status1;
                    mdorderCustomsResult.SJOrgNotes = notes1;
                    mdorderCustomsResult.SJstatus = status1 == "10" ? "1" : "0";//订单状态(0失败，1成功，2未上传,)
                    mdorderCustomsResult.SJOrgOrderChildId = messageId1;

                    string result = ordercustomsresultBll.updateUploadStatus(mdorderCustomsResult);
                    if (result.Contains("成功"))
                    {
                        //s = "订单" + messageId1 + "报关成功";
                        success += messageId1 + ",";

                        //删除本地商检订单回执
                        FileInfo info = new FileInfo(ss[i]);
                        info.Delete();
                    }
                    else
                    {
                        //s = "订单" + messageId1 + "报关成功,状态修改失败;" + result;
                        fail += messageId1 + ",";
                    }
                }
                catch (Exception ex)
                {
                    s = "执行到订单" + messageId1 + "出现异常，error:" + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success = success.Trim(',') + "数据库更新成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail = fail.Trim(',') + "数据库更新失败;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filesuccess))
            {
                filesuccess = filesuccess.Trim(',') + "报关成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filefail))
            {
                filefail = filefail.Trim(',') + "报关失败;\r\n";
            }

            ordercustomsresultBll = null;

            return filesuccess + filefail + success + fail + s;
        }

        /// <summary>
        /// 获取商检订单回执,并下载到本地
        /// </summary>
        /// <returns></returns>
        public string downLoadSJOrderReport()
        {
            string s = "下载成功";

            string serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPSJIP"]; //ftp地址
            string userName = System.Configuration.ConfigurationManager.AppSettings["SJUserName"]; //ftp用户名
            string password = System.Configuration.ConfigurationManager.AppSettings["SJPassword"]; //ftp密码

            serverIP += "/4200.IMPBA.SWBEBTRADE.REPORT/out/";
            try
            {
                string[] ss = helpcommon.FtpHelp.GetFileList(serverIP, userName, password);
                if (ss == null)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }
                if (ss.Length <= 0)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }

                for (int i = 0; i < ss.Length; i++)
                {
                    string localUrl = @"D:\CustomsFTPFile\sjOrder\out\" + ss[i];
                    int m = helpcommon.FtpHelp.DownloadFtp(serverIP, userName, password, localUrl);
                    if (m == 0)
                    {
                        #region (ftp，负载均衡,本地生成，然后上传FTP)
                        int k = helpcommon.FtpHelp.UploadFtp(@"ftp://192.168.1.124/sjOrder/out/", "fj", "123456", localUrl);         //(ftp，负载均衡,本地生成，然后上传FTP)

                        if (k == 0)
                        {
                            //下载成功，删除此xml
                            helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                        }
                        #endregion


                        #region 本地
                        ////下载成功，删除此xml
                        //helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                        #endregion
                    }
                    else
                    {
                        s = "部分商检订单回执下载完成";
                    }
                }
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            System.Threading.Thread.Sleep(2000); //延时2秒

            return s;
        }
        #endregion


        #region 订单海关报备
        /// <summary>
        /// 生成订单报文
        /// </summary>
        /// <returns></returns>
        public string createHGOrderReport()
        {
            string success = string.Empty; //数据表更新成功
            string fail = string.Empty;    //数据表更新失败
            string filesuccess = string.Empty; //报文创建成功
            string filefail = string.Empty;    //报文创建失败
            string s = string.Empty;
            string orderId = string.Empty;
            string ciqGoodsNo = string.Empty;//海关备案号
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            bll.productcustomresultbll productcustomresultBll = new bll.productcustomresultbll();
            bll.apiorderpaydetailsbll apiorderpaydetailsBll = new bll.apiorderpaydetailsbll();
            model.apiOrder MdApiOrder = new model.apiOrder();
            List<model.apiOrderDetails> MdApiOrderDetails = new List<model.apiOrderDetails>();

            #region 如果文件夹存在报文，则删除这些报文
            try
            {
                //string[] ss = Directory.GetFiles(@"D:\CustomsFTPFile\hgOrder\in\");
                string[] ss = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/hgOrder/in/", "fj", "123456");                   //(ftp，负载均衡)
                if (ss != null && ss.Length > 0)
                {
                    for (int i = 0; i < ss.Length; i++)
                    {
                        FileInfo info = new FileInfo(ss[i]);
                        info.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            #endregion


            orderId = Request.Form["orderId"].ToString();
            orderId = orderId.Trim(',');
            string[] orderIds = helpcommon.StrSplit.StrSplitData(orderId, ','); //处理批量orderId

            for (int h = 0; h < orderIds.Length; h++)
            {
                string[] orderIdPayId = helpcommon.StrSplit.StrSplitData(orderIds[h], '-');
                string oId = orderIdPayId[0].ToString();
                string pId = orderIdPayId[1].ToString();//支付单号（这里以pId为订单号报关海关，商检，支付，物流）

                MdApiOrder = apiorderBll.getOrderMsg(oId);
                if (MdApiOrder == null)
                {
                    return "订单信息有误";
                }
                MdApiOrderDetails = apiorderBll.getOrderDetailsMsg(oId, "");
                if (MdApiOrderDetails == null)
                {
                    return "订单信息有误";
                }

                XmlDocument doc = new XmlDocument();
                //加入XML的声明段落
                //XmlNode node = doc.CreateXmlDeclaration("1.0", "gb2312", null);
                XmlNode node = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(node);

                //创建根目录
                XmlElement el = doc.CreateElement("Manifest");
                doc.AppendChild(el);

                #region 报文头
                XmlNode xnode = doc.CreateElement(string.Empty, "Head", string.Empty);
                el.AppendChild(xnode);

                XmlElement xnode1 = doc.CreateElement(string.Empty, "MessageID", string.Empty);
                //xnode1.InnerText = MdApiOrder.orderId + "S"; //报文唯一编号，最大50字符
                xnode1.InnerText = pId + "S"; //报文唯一编号，最大50字符
                xnode.AppendChild(xnode1);
                XmlElement xnode2 = doc.CreateElement(string.Empty, "MessageType", string.Empty);
                xnode2.InnerText = "880020"; //固定为880020
                xnode.AppendChild(xnode2);
                XmlElement xnode3 = doc.CreateElement(string.Empty, "SenderID", string.Empty);
                xnode3.InnerText = "IE150311113719"; //报文发送者标识
                xnode.AppendChild(xnode3);
                XmlElement xnode5 = doc.CreateElement(string.Empty, "SendTime", string.Empty);
                xnode5.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//发送时间
                xnode.AppendChild(xnode5);
                XmlElement xnode7 = doc.CreateElement(string.Empty, "Version", string.Empty);
                xnode7.InnerText = "1.0";//最大10字符，缺省值：1.0
                xnode.AppendChild(xnode7);
                #endregion

                #region 报文体
                XmlNode nodeContent = doc.CreateElement(string.Empty, "Declaration", string.Empty);
                el.AppendChild(nodeContent);

                XmlNode nodeContent2 = doc.CreateElement(string.Empty, "EOrder", string.Empty);
                nodeContent.AppendChild(nodeContent2);

                XmlNode Content1 = doc.CreateElement(string.Empty, "OrderId", string.Empty);
                //Content1.InnerText = MdApiOrder.orderId; //订单编号
                Content1.InnerText = pId; //订单编号
                nodeContent2.AppendChild(Content1);
                XmlNode Content2 = doc.CreateElement(string.Empty, "IEFlag", string.Empty);
                Content2.InnerText = "I"; //进出口标识(I:进口；E:出口；)
                nodeContent2.AppendChild(Content2);
                XmlNode Content3 = doc.CreateElement(string.Empty, "OrderStatus", string.Empty);
                if (MdApiOrder.orderStatus == 4)//(订单当前状态：1为待确认，2为确认，3为待发货，4为发货，5交易成功，6通关异常，7，通关成功，11退货，12取消)
                {
                    Content3.InnerText = "S";//订单状态（？）
                }
                else if (MdApiOrder.orderStatus == 12)
                {
                    Content3.InnerText = "C";//订单状态（？）
                }
                //Content3.InnerText = "S"; //订单状态(S-订单新增，C-订单取消；)ps：手动修改订单为取消订单报关时，修改apiorder的状态为12就可以了，这里读取的是apiorder表中的状态和其他表状态没有关系。
                nodeContent2.AppendChild(Content3);
                XmlNode Content4 = doc.CreateElement(string.Empty, "EntRecordNo", string.Empty);
                Content4.InnerText = "IE150311113719"; //电商平台企业备案号（代码）
                nodeContent2.AppendChild(Content4);
                XmlNode Content5 = doc.CreateElement(string.Empty, "EntRecordName", string.Empty);
                Content5.InnerText = "深圳市亚平宁环球科技有限公司"; //电商平台企业名称
                nodeContent2.AppendChild(Content5);
                XmlNode Content6 = doc.CreateElement(string.Empty, "OrderName", string.Empty);
                Content6.InnerText = MdApiOrder.realName; //订单人姓名
                nodeContent2.AppendChild(Content6);
                XmlNode Content7 = doc.CreateElement(string.Empty, "OrderDocType", string.Empty);
                Content7.InnerText = "01"; //订单人证件类型(01:身份证、02:护照、03:其他)
                nodeContent2.AppendChild(Content7);
                XmlNode Content8 = doc.CreateElement(string.Empty, "OrderDocId", string.Empty);
                Content8.InnerText = MdApiOrder.def1; //订单人证件号
                nodeContent2.AppendChild(Content8);
                XmlNode Content9 = doc.CreateElement(string.Empty, "OrderPhone", string.Empty);
                Content9.InnerText = MdApiOrder.phone; //订单人电话
                nodeContent2.AppendChild(Content9);
                XmlNode Content10 = doc.CreateElement(string.Empty, "OrderGoodTotal", string.Empty);
                //Content10.InnerText = double.Parse(MdApiOrder.itemPrice.ToString()).ToString("F2"); //订单商品总额 //（订单优惠价格favorablePrice）（订单金额=商品总价+快递费用-优惠费用+税费orderPrice）
                Content10.InnerText = double.Parse(MdApiOrder.orderPrice.ToString()).ToString("F2"); //订单商品总额 //（订单优惠价格favorablePrice）（订单金额=商品总价+快递费用-优惠费用+税费orderPrice）
                //helpcommon.ParmPerportys.GetStrParms(double.Parse(((item.detailsItemPrice * item.detailsSaleCount) + item.detailsTaxPrice + item.detailsDeliveryPrice - MdApiOrder.favorablePrice).ToString()).ToString("F2"));//该项商品的总价，二位小数； 单位：RMB；(含税)(商品总价+快递费用-优惠费用+税费)
                nodeContent2.AppendChild(Content10);
                XmlNode Content11 = doc.CreateElement(string.Empty, "OrderGoodTotalCurr", string.Empty);
                Content11.InnerText = "142"; //订单商品总额币制
                nodeContent2.AppendChild(Content11);
                XmlNode Content12 = doc.CreateElement(string.Empty, "Freight", string.Empty);
                Content12.InnerText = double.Parse(MdApiOrder.deliveryPrice.ToString()).ToString("F2"); //运费
                nodeContent2.AppendChild(Content12);
                XmlNode Content13 = doc.CreateElement(string.Empty, "FreightCurr", string.Empty);
                Content13.InnerText = "142"; //运费币制
                nodeContent2.AppendChild(Content13);
                XmlNode Content14 = doc.CreateElement(string.Empty, "Tax", string.Empty);
                Content14.InnerText = double.Parse(MdApiOrder.taxPrice.ToString()).ToString("F2"); //税款
                nodeContent2.AppendChild(Content14);
                XmlNode Content15 = doc.CreateElement(string.Empty, "TaxCurr", string.Empty);
                Content15.InnerText = "142"; //税款币制
                nodeContent2.AppendChild(Content15);
                XmlNode Content16 = doc.CreateElement(string.Empty, "Note", string.Empty);
                Content16.InnerText = "备注"; //备注
                nodeContent2.AppendChild(Content16);
                XmlNode Content17 = doc.CreateElement(string.Empty, "OrderDate", string.Empty);
                Content17.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //订单日期
                nodeContent2.AppendChild(Content17);

                int pp = 0;
                foreach (var item in MdApiOrderDetails)
                {
                    #region 获取商品信息
                    ///如果货号中包含yy，那么说明此货号为前期报备错误的货号，需去掉yy重新报备
                    string newScode = item.detailsScode.Substring(item.detailsScode.Length - 2, 2).Contains("yy") == true ? item.detailsScode.Replace("yy", "") : item.detailsScode;

                    bll.ProductBll Productbll = new bll.ProductBll();
                    //model.product MdProduct = Productbll.getProductScode(item.detailsScode); //货号
                    model.product MdProduct = Productbll.getProductScode(newScode); //货号
                    ciqGoodsNo = productcustomresultBll.getCustomsTariffNo(MdProduct.Cat2);  //海关备案号
                    #endregion

                    XmlNode nodeContent3 = doc.CreateElement(string.Empty, "EOrderGoods", string.Empty);
                    nodeContent.AppendChild(nodeContent3);

                    XmlNode nodeContent31 = doc.CreateElement(string.Empty, "EOrderGood", string.Empty);
                    nodeContent3.AppendChild(nodeContent31);

                    pp++;
                    XmlNode c1 = doc.CreateElement(string.Empty, "GNo", string.Empty);
                    c1.InnerText = pp.ToString();//商品序号
                    nodeContent31.AppendChild(c1);
                    XmlNode c2 = doc.CreateElement(string.Empty, "ChildOrderNo", string.Empty);
                    //c2.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");//子订单编号
                    //c2.InnerText = MdApiOrder.orderId;//子订单编号(此处填写主订单ID)
                    c2.InnerText = pId;
                    nodeContent31.AppendChild(c2);
                    XmlNode c3 = doc.CreateElement(string.Empty, "StoreRecordNo", string.Empty);
                    c3.InnerText = "IE150311113719";//电商商户企业备案号
                    nodeContent31.AppendChild(c3);
                    XmlNode c4 = doc.CreateElement(string.Empty, "StoreRecordName", string.Empty);
                    c4.InnerText = "深圳市亚平宁环球科技有限公司";//电商商户企业名称
                    nodeContent31.AppendChild(c4);
                    XmlNode c5 = doc.CreateElement(string.Empty, "CustomsListNO", string.Empty);
                    c5.InnerText = ciqGoodsNo;//商品海关备案号(行邮税税号)
                    nodeContent31.AppendChild(c5);
                    XmlNode c6 = doc.CreateElement(string.Empty, "DecPrice", string.Empty);
                    c6.InnerText = helpcommon.ParmPerportys.GetStrParms(double.Parse(item.detailsItemPrice.ToString()).ToString("F2"));//商品单价
                    nodeContent31.AppendChild(c6);
                    XmlNode c7 = doc.CreateElement(string.Empty, "Unit", string.Empty);
                    c7.InnerText = MdProduct.QtyUnit;//计量单位
                    nodeContent31.AppendChild(c7);
                    XmlNode c8 = doc.CreateElement(string.Empty, "GQty", string.Empty);
                    c8.InnerText = helpcommon.ParmPerportys.GetStrParms(item.detailsSaleCount);//商品数量
                    nodeContent31.AppendChild(c8);
                    XmlNode c9 = doc.CreateElement(string.Empty, "DeclTotal", string.Empty);
                    c9.InnerText = helpcommon.ParmPerportys.GetStrParms(double.Parse((item.detailsItemPrice * item.detailsSaleCount).ToString()).ToString("F2"));//商品总价
                    nodeContent31.AppendChild(c9);
                    XmlNode c10 = doc.CreateElement(string.Empty, "Notes", string.Empty);
                    c10.InnerText = "备注";//备注
                    nodeContent31.AppendChild(c10);
                }
                #endregion


                string fileName = msgType.p1.GetHashCode().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
                string fileLocalUrl = "D:\\CustomsFTPFile\\hgOrder\\in\\" + fileName + ".xml";
                try
                {
                    doc.Save(fileLocalUrl);
                    //加密
                    string ss2 = AESHelper.Encrypt(System.IO.File.ReadAllText(fileLocalUrl));
                    System.IO.File.WriteAllText("D:\\CustomsFTPFile\\hgOrder\\in\\" + fileName + ".xml", ss2);

                    helpcommon.FtpHelp.UploadFtp(@"ftp://192.168.1.124/hgOrder/in/", "fj", "123456", fileLocalUrl);         //(ftp，负载均衡,本地生成，然后上传FTP)

                    filesuccess += oId.ToString() + ",";
                }
                catch (Exception ex)
                {
                    filefail += oId.ToString() + "," + ex.Message;
                }

                try
                {
                    //update
                    string result = ordercustomsresultBll.updateUploadName(pId, fileName);
                    if (result.Contains("成功"))
                    {
                        success += oId.ToString() + ",";
                    }
                    else
                    {
                        fail += oId.ToString() + ",";
                    }
                }
                catch (Exception ex)
                {
                    s += "执行到订单（" + oId.ToString() + "）出现异常;error:" + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(filesuccess))
            {
                filesuccess += "报文创建成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filefail))
            {
                filefail += "报文创建失败;\r\n";
            }


            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "数据表更新成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "数据表更新失败;\r\n";
            }

            apiorderBll = null;
            ordercustomsresultBll = null;
            productcustomresultBll = null;
            MdApiOrder = null;
            MdApiOrderDetails = null;

            return filesuccess + filefail + success + fail + s;
        }


        //ftp://YPNH:dY6318Cd@210.21.48.7:2312/  账号：YPNH    密码：dY6318Cd   //正式账号
        /// <summary>
        /// 上传海关订单报文
        /// </summary>
        /// <returns></returns>
        public string upLoadHGOrderReport()
        {
            string orderId = string.Empty;
            string success = string.Empty; //上传成功商品
            string fail = string.Empty;    //上传失败商品
            string s = string.Empty;       //异常信息
            string name = string.Empty; //报文名称
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            string serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPHGIP"];
            string userName = System.Configuration.ConfigurationManager.AppSettings["HGUserName"];
            string password = System.Configuration.ConfigurationManager.AppSettings["HGPassword"];

            orderId = Request.Form["orderId"].ToString();
            orderId = orderId.Trim(',');
            string[] ss = helpcommon.StrSplit.StrSplitData(orderId, ',');


            string pId = string.Empty;//（这里以pId为订单号报关海关，商检，支付，物流）
            for (int i = 0; i < ss.Length; i++)
            {
                string[] orderIdPayId = helpcommon.StrSplit.StrSplitData(ss[i].ToString(), '-');
                string oId = orderIdPayId[0].ToString();
                pId += orderIdPayId[1].ToString() + ",";
            }
            pId = pId.Trim(',');

            #region 判断是否生成过报文
            //Dictionary<string, string> orderIds = ordercustomsresultBll.getUploadStatus(orderId); //判断是否生成过报文
            Dictionary<string, string> orderIds = ordercustomsresultBll.getUploadStatus(pId); //判断是否生成过报文
            #endregion
            serverIP += "/UPLOAD/";

            //string[] files = Directory.GetFiles("D:\\CustomsFTPFile\\hgOrder\\in\\"); //获取所有生成的文件报表
            string[] files = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/hgOrder/in/", "fj", "123456");                   //(ftp，负载均衡); //获取所有生成的文件报表

            try
            {
                for (int h = 0; h < files.Length; h++)
                {
                    int i = 0;
                    name = files[h].Replace("D:\\CustomsFTPFile\\hgOrder\\in\\", "").Replace(".xml", "").ToString();
                    #region 读取生成的商品报文，查取其报文货号
                    orderId = (from c in orderIds where c.Value == name select c.Key).SingleOrDefault();
                    if (string.IsNullOrWhiteSpace(orderId))
                    {
                        continue;
                    }
                    #endregion

                    #region 根据货号读取海关状态，如果不为10，则继续上传报文，否则不予上传
                    Dictionary<string, string> dic = ordercustomsresultBll.getUploadStatus2(orderId); //判断报文是否报关成功
                    #endregion

                    string hgState = string.IsNullOrWhiteSpace(dic[orderId]) ? string.Empty : dic[orderId].ToString();

                    if (hgState == "1")
                    {
                        s += orderId + "已上传成功，请勿重复上传;";
                        continue;
                    }
                    else
                    {
                        i = helpcommon.FtpHelp.UploadFtp(serverIP, userName, password, files[h]);

                        if (i == 0)
                        {
                            success += orderId + ",";
                        }
                        else
                        {
                            fail += orderId + ",";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                s = "执行到商品（" + orderId + "）出现异常;error:" + ex.Message;
            }

            if (!string.IsNullOrWhiteSpace(success) && !success.Contains("error"))
            {
                success += "上传成功;\r\n";
            }
            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "上传失败;\r\n";
            }

            apiorderBll = null;
            ordercustomsresultBll = null;

            return success + fail + s;
        }


        /// <summary>
        /// (海关)更新订单回执
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public string readHGOrderReport()
        {
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            string s = string.Empty;
            string orderId = string.Empty;
            string name = string.Empty;
            string success = string.Empty; //数据库更新成功
            string fail = string.Empty;    //数据库更新失败
            string filesuccess = string.Empty;  //报文成功
            string filefail = string.Empty;     //报文失败
            string dbStatus = string.Empty;     //数据库订单海关状态

            string messageId1 = string.Empty;//报文编号 DocumentNo
            string sendtime1 = string.Empty;//回执时间 HGReturnDate
            string status1 = string.Empty; //处理状态 HGReturnCode (C01:入库成功)
            string notes1 = string.Empty; //回执说明 HGReturnInfo
            string attachedflag1 = string.Empty; //AttachedFlag 附件标识(0—无数据附件、1—有数据附件；=0—无数据附件：回执报文中，无业务数据；=1—有数据附件：回执报文中，有业务数据；)

            orderId = helpcommon.ParmPerportys.stringToLower(orderId);
            //string[] ss = Directory.GetFiles(@"D:\CustomsFTPFile\hgOrder\out\");
            string[] ss = helpcommon.FtpHelp.GetFileList(@"ftp://192.168.1.124/hgOrder/out/", "fj", "123456");                   //(ftp，负载均衡)
            if (ss.Length < 1)
            {
                return "暂无订单报文审核回执";
            }

            for (int i = 0; i < ss.Length; i++)
            {
                XmlDocument doc = new XmlDocument();
                //doc.Load(ss[i]);
                doc.Load("ftp://192.168.1.124/hgOrder/out/" + ss[i].ToString());                   //(ftp，负载均衡)
                XmlNode node = doc.DocumentElement;
                XmlNodeList nodeList = node.ChildNodes;

                #region
                foreach (XmlNode item in nodeList)
                {
                    XmlNodeList nodeList2 = item.ChildNodes;
                    foreach (XmlNode item2 in nodeList2)
                    {
                        XmlNodeList nodeList3 = item2.ChildNodes;
                        if (nodeList3.Count > 1)
                        {
                            foreach (XmlNode item3 in nodeList3)
                            {
                                string item2Value = helpcommon.ParmPerportys.stringToLower(item3.InnerText);
                                if (item3.Name == "DocumentNo")
                                {
                                    messageId1 = item2Value; //订单ID
                                }

                                if (item3.Name == "ReturnDate")  //回执时间
                                {
                                    sendtime1 = item2Value;
                                }
                                if (item3.Name == "ReturnCode")  //处理状态 HGReturnCode (C01:入库成功)
                                {
                                    status1 = item2Value;
                                }
                                if (item3.Name == "ReturnInfo")  //回执说明 HGReturnInfo
                                {
                                    notes1 = item2Value;
                                }
                                if (item3.Name == "AttachedFlag")  //附件标识 AttachedFlag
                                {
                                    attachedflag1 = item2Value;
                                }
                            }
                        }
                        else
                        {
                            string item2Value = helpcommon.ParmPerportys.stringToLower(item2.InnerText);
                            if (item2.Name == "DocumentNo")
                            {
                                messageId1 = item2Value; //订单ID
                            }

                            if (item2.Name == "ReturnDate")  //回执时间
                            {
                                sendtime1 = item2Value;
                            }
                            if (item2.Name == "ReturnCode")  //处理状态 HGReturnCode (C01:入库成功)
                            {
                                status1 = item2Value;
                            }
                            if (item2.Name == "ReturnInfo")  //回执说明 HGReturnInfo
                            {
                                notes1 = item2Value;
                            }
                            if (item2.Name == "AttachedFlag")  //附件标识 AttachedFlag
                            {
                                attachedflag1 = item2Value;
                            }
                        }
                    }
                }
                #endregion

                //报文资格检查
                //A01	缺少接入企业备案号	海关收到报文经过合法性检查后，不通过，不保存到预处理库；
                //A02	电商接入企业未备案或失效	海关收到报文经过合法性检查后，不通过，不保存到预处理库；

                //A10	缺少电商企业备案号。	海关收到报文经过合法性检查后，不通过，不保存到预处理库；
                //A11	电商企业未备案或失效；	海关收到报文经过合法性检查后，不通过，不保存到预处理库；
                //A14	退回修改	指报文中的字段不满足要求，退回修改；
                //异常业务处理状态	
                //B10	报文类型不正确。	海关收到报文经过合法性检查后，不通过，不保存到预处理库；
                //B11	业务字段异常	海关收到报文经过合法性检查后，不通过，不保存到预处理库；
                //B12	重复报文	海关数据处理时，发现重复，不再处理；
                //B13	重复记录	预处理时，发现重复的记录；
                //正常业务处理状态	
                //C01	入库成功	海关收到报文经过合法性检查后，报文数据入预处理库成功；


                if (status1.ToLower() == "c01") //c01：申报成功 其他：申报失败
                {
                    filesuccess += messageId1 + ",";
                }
                else
                {
                    filefail += "(" + messageId1 + ")" + notes1 + ";";
                }

                try
                {
                    model.orderCustomsResult mdorderCustomsResult = new model.orderCustomsResult();
                    mdorderCustomsResult.HGReturnDate = DateTime.Now; //helpcommon.ParmPerportys.GetDateTimeNowParms(sendtime1);
                    mdorderCustomsResult.HGReturnCode = status1;
                    mdorderCustomsResult.HGReturnInfo = notes1;
                    mdorderCustomsResult.HGstatus = status1.ToLower() == "c01" ? "1" : "0";//订单状态(0失败，1成功，2未上传,)
                    //mdorderCustomsResult.SJOrgOrderChildId = orderId;
                    mdorderCustomsResult.SJOrgOrderChildId = messageId1;

                    string result = ordercustomsresultBll.updateUploadStatus1(mdorderCustomsResult);
                    if (result.Contains("成功"))
                    {
                        //s = "商品" + scode + "报关审核成功";
                        success += messageId1 + ",";

                        //删除本地海关订单回执
                        FileInfo info = new FileInfo(ss[i]);
                        info.Delete();
                    }
                    else
                    {
                        //s = "商品" + scode + "报关审核成功,状态修改失败;" + result;
                        fail += messageId1 + ",";
                    }

                }
                catch (Exception ex)
                {
                    s = "执行到订单" + messageId1 + "出现异常，error:" + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "数据库更新成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail +="数据库更新失败;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filesuccess))
            {
                filesuccess += "报关成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(filefail))
            {
                filefail += "报关失败;\r\n";
            }

            ordercustomsresultBll = null;
            return s + filesuccess + filefail + success + fail + s;
        }


        /// <summary>
        /// 获取海关订单回执,并下载到本地
        /// </summary>
        /// <returns></returns>
        public string downLoadHGOrderReport()
        {
            string s = "下载成功";

            string serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPHGIP"]; //ftp地址
            string userName = System.Configuration.ConfigurationManager.AppSettings["HGUserName"]; //ftp用户名
            string password = System.Configuration.ConfigurationManager.AppSettings["HGPassword"]; //ftp密码
            serverIP += "/DOWNLOAD/";

            try
            {
                string[] ss = helpcommon.FtpHelp.GetFileList(serverIP, userName, password);
                if (ss == null)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }
                if (ss.Length <= 0)
                {
                    return "暂无文件下载，请等待几分钟重新下载";
                }

                for (int i = 0; i < ss.Length; i++)
                {
                    string localUrl = @"D:\CustomsFTPFile\hgOrder\out\" + ss[i];
                    int m = helpcommon.FtpHelp.DownloadFtp(serverIP, userName, password, localUrl);
                    if (m == 0)
                    {
                        //解密
                        System.IO.File.WriteAllText(localUrl, AESHelper.Decrypt(System.IO.File.ReadAllText(localUrl)));

                        #region (ftp，负载均衡,本地生成，然后上传FTP)
                        int k = helpcommon.FtpHelp.UploadFtp(@"ftp://192.168.1.124/hgOrder/out/", "fj", "123456", localUrl);         //(ftp，负载均衡,本地生成，然后上传FTP)

                        if (k == 0)
                        {
                            //下载成功，删除此xml
                            helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                        }
                        #endregion


                        ////下载成功，删除此xml
                        //helpcommon.FtpHelp.fileDelete(serverIP, userName, password, ss[i]);
                        ////"HDvqi10LCBr5E/zf7Ls3mMc423JJ8Nua/i3gzAYTK7RSTPZ1xcJOf/I+wJcVUnhdK3KOu0cJpIaVsO3jpHwNKzRNpszBZED+VzmD4jAcJuVRoNYf2BMwuBAf3Z+rCK8emJxfrW9x5cpv5Z5NbTTJspdNgWgwNF9PH4QEsyxg22BGteLx3LYqazbmlA6Mxl/gTnVXMg6aC8551HusZlT1tQo1xdB4BhmOQD05MSZfXMplItPy7aHTSTNzaWadcSJmGoR6x0+wTX5N1Sa9ih9UB4ozZxLVganhUebkzKcjPbv9JipNzaoZwYzclekIeuB7B7swk2pKC/U1+Bz0ZRiGu9mSYzC+sfWV2BqVEkTgmG7hsg97fDNL7yRss3C/fbnXrnDSgI8au98bRVARMtGBLCFPzq8W+B/W7p+raw3YBV3RzUbq4CLao9eJySQqp81bYkezwoh0mMOn/Fav7VGdYUstNXZ70lseFCIRyGhCC9y/JxBSj1AxMNQHSBDAgRV7+kuJClXJdH/u+XAXrB1raz/1RLtCs0sWHoJ5hI+Hv/JvBADW4KiQwdFaktIGFgAioTLGTdu1MNJvr4G/U8Li0HZpdkBnDNdAXegniiX2rROp46IhdRKgN2ASqZlmBy5lfx/n64L9tn0hPXOUihlsFgutUUvtO/URuRzJSBDe34sD+xWFSCBHYtOF2K+V6U2iHYl5hq8sPeMPCSXE8/Qq86Y7yJ0BMlhqpSEtKSY99suYEVkYTVOZWKAdt2gFWmg6YZrGawSGHZLI+0yY8pPlPg=="
                        ////"HDvqi10LCBr5E/zf7Ls3mMc423JJ8Nua/i3gzAYTK7RSTPZ1xcJOf/I+wJcVUnhdK3KOu0cJpIaVsO3jpHwNKzRNpszBZED+VzmD4jAcJuVRoNYf2BMwuBAf3Z+rCK8emJxfrW9x5cpv5Z5NbTTJspdNgWgwNF9PH4QEsyxg22BGteLx3LYqazbmlA6Mxl/gTnVXMg6aC8551HusZlT1tQo1xdB4BhmOQD05MSZfXMplItPy7aHTSTNzaWadcSJmGoR6x0+wTX5N1Sa9ih9UB4ozZxLVganhUebkzKcjPbv9JipNzaoZwYzclekIeuB7B7swk2pKC/U1+Bz0ZRiGu9mSYzC+sfWV2BqVEkTgmG7hsg97fDNL7yRss3C/fbnXrnDSgI8au98bRVARMtGBLCFPzq8W+B/W7p+raw3YBV3RzUbq4CLao9eJySQqp81bYkezwoh0mMOn/Fav7VGdYUstNXZ70lseFCIRyGhCC9y/JxBSj1AxMNQHSBDAgRV7+kuJClXJdH/u+XAXrB1raz/1RLtCs0sWHoJ5hI+Hv/JvBADW4KiQwdFaktIGFgAioTLGTdu1MNJvr4G/U8Li0HZpdkBnDNdAXegniiX2rROp46IhdRKgN2ASqZlmBy5lfx/n64L9tn0hPXOUihlsFgutUUvtO/URuRzJSBDe34sD+xWFSCBHYtOF2K+V6U2iHYl5hq8sPeMPCSXE8/Qq86Y7yJ0BMlhqpSEtKSY99suYEVkYTVOZWKAdt2gFWmg6YZrGawSGHZLI+0yY8pPlPg=="
                    }
                    else
                    {
                        s = "部分商检订单回执下载完成";
                    }
                }
            }
            catch (Exception ex)
            {
                s = ex.Message;
            }
            System.Threading.Thread.Sleep(2000); //延时2秒

            return s;
        }
        #endregion


        #region 订单海关支付报备
        //1.正确待签名：
        //_input_charset=utf-8&amount=300&customs_place=GUANGZHOU&merchant_customs_code=IE150311113719&merchant_customs_name=深圳亚平宁环球科技有限公司&out_request_no=20150805114700218&partner=2088021109441402&service=alipay.acquire.customs&trade_no=2015080521001003940047317600eg0n5nomuptk76gk885dyqn6r20y2fui

        //2.MD5加密待签名字符串：

        //3.把sign_type和sign一同加上申请支付报关
        //https://mapi.alipay.com/gateway.do?_input_charset=utf-8&amount=360&customs_place=GUANGZHOU&merchant_customs_code=IE150311113719&merchant_customs_name=深圳亚平宁环球科技有限公司&out_request_no=20150805105920259&partner=2088021109441402&service=alipay.acquire.customs&trade_no=2015080521001003940047343644&sign_type=MD5&sign=e10fe1693928d2fb033516344f9510e6


        #region 创建支付报文
        /// <summary>
        /// 创建支付报文
        /// </summary>
        /// <returns></returns>
        public string createHGPayReport()
        {
            bll.apiorderpaydetailsbll apiorderpaydetailsBll = new bll.apiorderpaydetailsbll();
            string s = string.Empty;
            string error = string.Empty; //错误信息
            string fileFail = string.Empty; //报关失败
            string fileSuccess = string.Empty; //报关成功
            string success = string.Empty; //数据更新成功
            string fail = string.Empty; //数据更新失败
            string orderId = string.Empty;
            string sign_type = "MD5"; //签名方式(DSA、RSA、MD5 三个值可选，必须大写。)
            string sign = "";//签名(请参见“7 签名机制”。)

            orderId = Request.Form["orderId"].ToString();
            orderId = orderId.Trim(',');
            string[] orderIds = helpcommon.StrSplit.StrSplitData(orderId, ','); //处理批量orderId


            #region 参数
            #region 基本参数
            string partner = "2088021109441402"; //合作者身份ID(签约的支付宝账号对应的支付宝唯一用户号。以 2088 开头的 16 位纯数字组成。)
            string _input_charset = "utf-8";//参数编码字符集(商户网站使用的编码格式，如 utf-8、gbk、gb2312 等)
            string key = "eg0n5nomuptk76gk885dyqn6r20y2fui";
            #endregion
            //eg0n5nomuptk76gk885dyqn6r20y2fui
            //eg0n5nomuptk76gk885dyqn6r20y2fui
            #region 业务参数
            string out_request_no = ""; //报关流水号(商户生成的用于唯一标识一次报关操作的业务编号。建议生成规则 yyyymmmdd型八位日期拼接 24 位序列号。根据此流水号做幂等)
            string trade_no = "";//支付宝交易号(该交易在支付宝系统中的交易流水号。最长 64 位。)
            string merchant_customs_code = "IE150311113719"; //商户海关备案编号(电商平台编号)
            string merchant_customs_name = "深圳亚平宁环球科技有限公司"; //商户海关备案名称(商户海关备案名称)
            string amount = ""; //报关金额(报关金额)
            string customs_place = "GUANGZHOU"; //海关编号(海关编号，详见 4.3)
            //杭州海关：HANGZHOU
            //郑州海关：ZHENGZHOU
            //广州海关：GUANGZHOU
            //重庆海关：CHONGQING
            //宁波海关：NINGBO
            //深圳海关：SHENZHEN
            #endregion
            #endregion

            string sign_world = string.Empty;   //待签名字符串
            string get_url = string.Empty; //支付报关链接
            for (int i = 0; i < orderIds.Length; i++)
            {
                try
                {
                    string[] ss = apiorderpaydetailsBll.getPayDetails(orderIds[i]);
                    //out_request_no = ss[1];
                    //trade_no = ss[0];

                    out_request_no = ss[0]; //支付宝流水
                    trade_no = ss[1]; //系统流水
                    amount = float.Parse(ss[2]).ToString("F0"); //支付金额

                    #region 待签名字符串
                    sign_world = "_input_charset=utf-8&amount=" + amount + "&customs_place=GUANGZHOU&merchant_customs_code=IE150311113719&merchant_customs_name=深圳亚平宁环球科技有限公司&out_request_no=" + out_request_no + "&partner=2088021109441402&service=alipay.acquire.customs&trade_no=" + trade_no + key;
                    sign_world = sign_world.Trim();
                    sign = helpcommon.PasswordHelp.To32Md5(sign_world.ToString().Trim()).ToLower();
                    #endregion

                    #region 申请支付报关
                    get_url = "https://mapi.alipay.com/gateway.do?_input_charset=utf-8&amount=" + amount + "&customs_place=GUANGZHOU&merchant_customs_code=IE150311113719&merchant_customs_name=深圳亚平宁环球科技有限公司&out_request_no=" + out_request_no + "&partner=2088021109441402&service=alipay.acquire.customs&trade_no=" + trade_no + "&sign_type=MD5&sign=" + sign;
                    get_url = get_url.Trim();
                    s = get_url;
                    #endregion

                    WebRequest request = (WebRequest)HttpWebRequest.Create(s);
                    request.ContentType = "text/xml";
                    request.Method = "get";
                    WebResponse response = request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    StreamReader read = new StreamReader(stream, Encoding.UTF8);
                    s = read.ReadToEnd();
                    //System.IO.File.WriteAllText(@"D:\CustomsFTPFile\hgPay\out\" + DateTime.Now.ToString() + new Random().Next(100, 999) + ".xml", s, Encoding.UTF8);


                    model.payCustomsResult MdpayCustomsResult = new model.payCustomsResult();
                    //MdpayCustomsResult.OrderChildId = orderIds[i].ToString();
                    MdpayCustomsResult.OrderChildId = ss[0]; //支付宝流水(支付报文：以支付宝流水为订单号报关)
                    if (s.Contains("<is_success>T</is_success>"))
                    {
                        fileSuccess += orderIds[i] + ",";

                        string trade_no_result = Regex.Match(s, "<trade_no>[\\s\\S]*?</trade_no>").Groups[0].Value.Replace("<trade_no>", "").Replace("</trade_no>", "");
                        string alipay_declare_no_result = Regex.Match(s, "<alipay_declare_no>[\\s\\S]*?</alipay_declare_no>").Groups[0].Value.Replace("<alipay_declare_no>", "").Replace("</alipay_declare_no>", "");
                        string result_code_result = Regex.Match(s, "<result_code>[\\s\\S]*?</result_code>").Groups[0].Value.Replace("<result_code>", "").Replace("</result_code>", "");

                        MdpayCustomsResult.trade_no = trade_no_result;//支付宝交易号(该交易在支付宝系统中的交易流水号。最长 64 位。)
                        MdpayCustomsResult.alipay_declare_no = alipay_declare_no_result;//支付宝报关流水号(支付宝报关流水号)
                        MdpayCustomsResult.result_code = result_code_result;//响应码(处理结果响应码。SUCCESS：成功,FAIL：失败)
                        MdpayCustomsResult.isSuccess = "T";//请求是否成功请求是否成功。请求成功不代表业务处理成功。(T代表成功,F代表失败)
                    }
                    else
                    {
                        fileFail += orderIds[i] + ",";

                        MdpayCustomsResult.isSuccess = "F";//请求是否成功请求是否成功。请求成功不代表业务处理成功。(T代表成功,F代表失败)
                        MdpayCustomsResult.error = "报关失败"; //错误信息

                    }



                    bll.paycustomsresultbll paycustomsresultBll = new bll.paycustomsresultbll();
                    string exist_result = paycustomsresultBll.existPayCustoms(orderIds[i]); //是否存在订单支付报关数据
                    if (exist_result.Contains("已存在")) // "已存在" : "不存在";
                    {
                        //update
                        string p1 = paycustomsresultBll.updatePayCustoms(MdpayCustomsResult);
                        if (p1.Contains("成功"))
                        {
                            success += orderIds[i].ToString() + ",";
                        }
                        else
                        {
                            fail += orderIds[i].ToString() + ",";
                        }
                    }
                    else
                    {
                        string p1 = paycustomsresultBll.addPayCustoms(MdpayCustomsResult);
                        if (p1.Contains("成功"))
                        {
                            success += orderIds[i].ToString() + ",";
                        }
                        else
                        {
                            fail += orderIds[i].ToString() + ",";
                        }
                    }
                }
                catch (Exception ex)
                {
                    //error += "执行到" + orderIds[i].ToString() + "出现异常:(" + ex.Message + ")";
                    return "执行到" + orderIds[i].ToString() + "出现异常:(" + ex.Message + ")";
                }
            }

            if (!string.IsNullOrWhiteSpace(fileFail))
            {
                fileFail += "订单支付报关失败\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fileSuccess))
            {
                fileSuccess += "订单支付报关成功\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "支付数据更新失败\r\n";
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "支付数据更新成功\r\n";
            }

            s = string.Empty;
            s += fileFail + fileSuccess + fail + success;

            apiorderpaydetailsBll = null;

            return s;
        }
        #endregion

        #region 上传支付报文
        /// <summary>
        /// 上传支付报文
        /// </summary>
        /// <returns></returns>
        public string upLoadHGPayReport()
        {
            string s = string.Empty;

            return s;
        }
        #endregion

        #region 下载支付报文
        public string downLoadHGPayReport()
        {
            string s = string.Empty;

            return s;
        }
        #endregion

        #region 读取支付报文
        public string readHGPayReport()
        {
            string s = string.Empty;

            return s;
        }
        #endregion

        #endregion


        #region 商品联邦报备
        /// <summary>
        /// 联邦商品报备  jqy@panbixuan.com  123456
        /// </summary>
        /// <returns></returns>
        public string BBCProductReport()
        {
            bll.productcustomresultbll productcustomresultBll = new bll.productcustomresultbll();
            bll.ProductBll Productbll = new bll.ProductBll();
            bll.hscodebll hscodeBll = new bll.hscodebll();
            bll.ProductStockBLL myProductStockBLL = new bll.ProductStockBLL();
            bll.BrandBLL brandbll = new bll.BrandBLL();
            model.product MdProduct = new model.product();
            string message = string.Empty;
            string ciqGoodsNo = string.Empty; //海关备案号
            string sjCiqGoodsNo = string.Empty; //商检备案号
            string s = string.Empty;
            string scode = string.Empty;
            string ciqHsName = string.Empty;
            string priceA = string.Empty;
            string brandFullName = string.Empty;
            string errorMsg = string.Empty;
            string success = string.Empty;  //数据库更新成功
            string fail = string.Empty;     //数据库更新失败
            string filesuccess = string.Empty; //报关成功

            scode = Request.Form["scode"].ToString();
            scode = scode.Trim(',');
            string[] scodes = helpcommon.StrSplit.StrSplitData(scode, ','); //处理批量scode


            for (int h = 0; h < scodes.Length; h++)
            {
                ///如果货号中包含yy，那么说明此货号为前期报备错误的货号，需去掉yy重新报备
                string newScode = scodes[h].Substring(scodes[h].Length-2,2).Contains("yy") == true ? scodes[h].Replace("yy", "") : scodes[h];

                //MdProduct = Productbll.getProductScode(scodes[h]);
                MdProduct = Productbll.getProductScode(newScode);
                ciqHsName = string.IsNullOrWhiteSpace(MdProduct.ciqHSNo) ? "" : hscodeBll.getHscodeProductName(MdProduct.ciqHSNo);
                //priceA = myProductStockBLL.getPriceA(scodes[h]);
                priceA = myProductStockBLL.getPriceA(newScode);
                brandFullName = brandbll.getBrandFullName(MdProduct.Cat);
                ciqGoodsNo = productcustomresultBll.getCustomsTariffNo(MdProduct.Cat2);  //海关备案号
                sjCiqGoodsNo = productcustomresultBll.getCustomsProductNO(MdProduct.Scode);  //商检备案号
                //ciqGoodsNo = "06010200";


                ProductReport.HeaderRequest hdRequest = new ProductReport.HeaderRequest();
                //测试
                //hdRequest.appKey = "4f2e13b1569911a45cd742e9219c4fd1";
                //hdRequest.appToken = "8AC50A04A094A0C5";
                //hdRequest.customerCode = "C0146";

                //正式
                hdRequest.appKey = "695a2742bbd6acc11f5f183539938944";
                hdRequest.appToken = "7F811285C6E5BC38";
                hdRequest.customerCode = "C0139";

                try
                {
                    ProductReport.ProductInfo infoRequest = new ProductReport.ProductInfo();
                    infoRequest.applyEnterpriseCode = ciqGoodsNo;//产品海关备案号
                    infoRequest.applyEnterpriseCodeCIQ = sjCiqGoodsNo; //产品商检备案号
                    infoRequest.barcodeType = 1;//条码类型0:默认条码1:自定义条码2:序列号;
                    infoRequest.brand = helpcommon.ParmPerportys.GetStrParms(brandFullName);//品牌
                    //infoRequest.heightSpecified = "";
                    infoRequest.hsCode = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqHSNo);//海关编码HS_CODE
                    infoRequest.hsGoodsName = ciqHsName;//海关品名
                    float netWt = (float.Parse(MdProduct.Def16) / 1000); //净重
                    infoRequest.netWeight = netWt;//产品净重(kg)
                    infoRequest.originCountry = MdProduct.ciqAssemCountry.Contains("-") ? helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqAssemCountry.Split(new char[] { '-' })[1].ToString()) : string.Empty;//原产国 国家二字码对应关系参见附件三
                    //infoRequest.originCountry = helpcommon.ParmPerportys.GetStrParms("CN");//原产国 国家二字码对应关系参见附件三
                    infoRequest.productDeclaredValue = float.Parse(priceA);//申报价值
                    infoRequest.productLink = "http://h5.bstapp.cn/item/sku/" + MdProduct.Scode;//产品链接
                    infoRequest.skuCategorySpecified = true;
                    infoRequest.skuCategory = helpcommon.ParmPerportys.GetNumParms(10);//产品分类1:玩具,文体用品2:家妆用品3:衣服,鞋子和珠宝4:汽摩及配件5:书籍光盘6:电子产品7:食物与饮料8:家具家电9:保健护理用品10:其他
                    infoRequest.skuEnName = helpcommon.ParmPerportys.GetStrParms(MdProduct.Descript);//产品英文名称
                    infoRequest.skuName = helpcommon.ParmPerportys.GetStrParms(MdProduct.Cdescript);//产品名称
                    infoRequest.skuNo = helpcommon.ParmPerportys.GetStrParms(scodes[h].ToString()); //产品Sku代码
                    infoRequest.specificationsAndModels = helpcommon.ParmPerportys.GetStrParms(MdProduct.ciqSpec);//规格型号
                    infoRequest.UOM = MdProduct.QtyUnit;//产品计量单位代码，比如：007（个） 计量单位参见附件二
                    float produtWeight = (float.Parse(MdProduct.Def16) / 1000); //毛重
                    infoRequest.weight = produtWeight;//产品毛重 (kg)

                    if (netWt>produtWeight)
                    {
                        return "执行到商品(" + scodes[h] + ")时净重大于毛重,停止后续执行操作";
                    }

                    ProductReport.errorType[] error = new ProductReport.errorType[1];
                    ProductReport.ServiceForProductClient spc = new ProductReport.ServiceForProductClient();
                    string bbcAsk = spc.createProduct(hdRequest, infoRequest, out message, out error);

                    for (int i = 0; i < error.Length; i++)
                    {
                        if (error[i].errorMessage != null)
                            errorMsg += error[i].errorMessage.ToString();
                    }
                    s = message;
                    if (message.Contains("成功"))
                    {
                        filesuccess += scodes[h] + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(s) || error.Length > 1)
                    {
                        if (!s.Contains("成功"))
                        {
                            return "执行到商品(" + scodes[h] + ")时出现数据错误,停止后续操作," + message + "(" + errorMsg + ")";
                        }
                    }

                    model.productCustomsResult mdorderCustomsResult = new model.productCustomsResult();
                    mdorderCustomsResult.BBCask = bbcAsk;
                    mdorderCustomsResult.BBCmessage = message; //返回信息
                    mdorderCustomsResult.BBCskuNo = scodes[h];
                    mdorderCustomsResult.BBCerrorMessage = errorMsg;//返回错误信息

                    mdorderCustomsResult.productScode = scodes[h];
                    string result = productcustomresultBll.updateUploadStatus2(mdorderCustomsResult);
                    if (result.Contains("成功"))
                    {
                        success += scodes[h] + ",";
                    }
                    else
                    {
                        fail += scodes[h] + ",";
                    }
                }
                catch (Exception ex)
                {
                    s += "执行到商品(" + scodes[h] + ")时出现异常,error:" + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(filesuccess))
            {
                filesuccess += "联邦报关成功\r\n";
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "数据库更新成功\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "数据库更新失败;";
            }

            hscodeBll = null;
            myProductStockBLL = null;
            productcustomresultBll = null;
            Productbll = null;
            brandbll = null;
            MdProduct = null;

            return s + filesuccess + success + fail;
        }
        #endregion


        #region 订单联邦报备
        /// <summary>
        /// 联邦订单报备
        /// </summary>
        /// <returns></returns>
        public string BBCOrderPeport()
        {
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            bll.apiorderexpressbll apiorderexpressBll = new bll.apiorderexpressbll();
            bll.apiorderpaydetailsbll apiorderpaydetailsBll = new bll.apiorderpaydetailsbll();
            model.apiOrder MdApiOrder = new model.apiOrder();
            List<model.apiOrderDetails> MdApiOrderDetails = new List<model.apiOrderDetails>();

            OrderReport.errorType[] error1 = new OrderReport.errorType[1];
            OrderReport.errorType[] error2 = new OrderReport.errorType[1];
            string message = string.Empty;
            string orderCode = string.Empty; //联邦返回的系统订单交易号（联邦的订单号，非下单的订单号）
            string orderId = string.Empty;
            string s = string.Empty;
            string expressNo = string.Empty; //获取运单号
            string success = string.Empty;  //成功
            string fail = string.Empty;     //失败
            string errorMsg = string.Empty; //联邦返回错误信息

            orderId = Request.Form["orderId"].ToString();
            //orderId = "377,";
            orderId = orderId.Trim(',');
            string[] orderIds = helpcommon.StrSplit.StrSplitData(orderId, ','); //处理批量订单

            for (int h = 0; h < orderIds.Length; h++)
            {
                string payId = string.Empty;//（这里以payId为订单号报关海关，商检，支付，物流）
                payId = apiorderpaydetailsBll.getPayId(orderIds[h]); //获取payId
                //payId = "3";
                if (string.IsNullOrWhiteSpace(payId))
                {
                    return "订单未存在支付信息";
                }

                #region 检测联邦订单报备是否成功
                if (true)
                {
                    //报备过且成功，跳出本次循环
                }
                else
                {
                    //报备过但是失败，继续执行
                }
                #endregion

                MdApiOrder = apiorderBll.getOrderMsg(orderIds[h]);
                //expressNo = apiorderexpressBll.expressNo(orderIds[h]); //获取运单号
                expressNo = "1221312313"; //获取运单号
                MdApiOrderDetails = apiorderBll.getOrderDetailsMsg(orderIds[h], "");

                OrderReport.createOrderRequest cor = new OrderReport.createOrderRequest();
                OrderReport.HeaderRequest hdRequest = new OrderReport.HeaderRequest();
                //测试
                //hdRequest.appKey = "4f2e13b1569911a45cd742e9219c4fd1";
                //hdRequest.appToken = "8AC50A04A094A0C5";
                //hdRequest.customerCode = "C0146";

                //正式
                hdRequest.appKey = "695a2742bbd6acc11f5f183539938944";
                hdRequest.appToken = "7F811285C6E5BC38";
                hdRequest.customerCode = "C0139";
                try
                {
                    #region 详情
                    OrderReport.productDeatilType[] details = new OrderReport.productDeatilType[MdApiOrderDetails.Count];
                    int det1 = 0;
                    double produtWeight = 0; //毛重
                    foreach (var item in MdApiOrderDetails)
                    {
                        bll.ProductBll Productbll = new bll.ProductBll();

                        ///如果货号中包含yy，那么说明此货号为前期报备错误的货号，需去掉yy重新报备
                        //string newScode = item.detailsScode.Substring(item.detailsScode.Length - 2, 2).Contains("yy") == true ? item.detailsScode.Replace("yy", "") : item.detailsScode;
                        string newScode = item.detailsScode.ToString();
                        //string newScode = string.Empty; //把最后带有yy的同货号替换过来(如果存在同货号商品，且以yy结尾，那将使用yy结尾的相同货号报关)
                        newScode += "yy";
                        newScode = Productbll.getYyScode(newScode) == true ? newScode : item.detailsScode.ToString(); //是否存在以yy结尾的相同货号，如果存在，使用其报关，不存在，使用原货号报关）

                        //string newScode = "K15027C21BLKF";
                        float[] price = { float.Parse(item.detailsItemPrice.ToString()) };
                        float[] allPrice = { float.Parse((item.detailsItemPrice * item.detailsSaleCount).ToString()) };
                        details[det1] = new OrderReport.productDeatilType()
                        {
                            dealPrice = price,
                            opQuantity = helpcommon.ParmPerportys.GetNumParms(item.detailsSaleCount),
                            //productSku = item.detailsScode, 
                            productSku = newScode,
                            transactionPrice = allPrice
                        };
                        bll.ProductBll myProductBll = new bll.ProductBll();
                        model.product mdProduct = myProductBll.getProductScode(item.detailsScode);
                        //model.product mdProduct = myProductBll.getProductScode(newScode);
                        produtWeight += (float.Parse(mdProduct.Def15) / 1000);
                        det1++;
                    }
                    #endregion

                    OrderReport.OrderInfo infoOrder = new OrderReport.OrderInfo();
                    infoOrder.currencyCode = "RMB";//币种 默认 RMB
                    //infoOrder.deliveryFee = "";
                    infoOrder.grossWt = produtWeight.ToString();//毛重 单位 kg
                    infoOrder.idNumber = MdApiOrder.def1;//证件号码
                    infoOrder.idType = "身份证";//证件类型（传以下汉字之一）:身份证,护照,其它
                    //infoOrder.netWt = "";//净重 单位 kg
                    //infoOrder.oabCity = "深圳";// MdApiOrder.cityId;//收件人城市
                    infoOrder.oabCity = MdApiOrder.cityId;//收件人城市
                    //infoOrder.oabCompany = "";//收件人公司名
                    infoOrder.oabCounty = "CN";//收件人国家二字码 国家二字码对照表见附件 3 //MdProduct.ciqAssemCountry.Split(new char[]{'-'})[0].ToString()
                    //infoOrder.oabEmail = "";//电子邮件
                    infoOrder.oabName = MdApiOrder.realName;//收件人姓名
                    infoOrder.oabPhone = MdApiOrder.phone;//收件人电话
                    infoOrder.oabPostcode = MdApiOrder.postcode;//收件人邮编
                    //infoOrder.oabStateName = "广东";//MdApiOrder.provinceId;//收件人省份 省份名称对照表见附件 1
                    infoOrder.oabStateName = MdApiOrder.provinceId;//收件人省份 省份名称对照表见附件 1
                    infoOrder.oabStreetAddress1 = MdApiOrder.provinceId + MdApiOrder.cityId + MdApiOrder.district + MdApiOrder.buyNameAddress;//收件人地址 1
                    //infoOrder.oabStreetAddress2 = "";
                    infoOrder.orderProduct = details;//订单产品详情
                    infoOrder.orderStatus = "2";//提交订单状态 1：草稿2：确认
                    //infoOrder.referenceNo = MdApiOrder.orderId;//交易订单号
                    infoOrder.referenceNo = payId;//交易订单号
                    //infoOrder.remark = "";//备注
                    infoOrder.smCode = "FEDEX";//运输方式代码，固定填FEDEX
                    infoOrder.trackingNumber = expressNo;//国际运单号

                    //infoOrder.trackingNumberIn = expressNo;//国内运单号
                    infoOrder.serviceType = "1";//服务类型：1、一票到底，2、两段式

                    //success = "1";
                    OrderReport.ServiceForOrderClient sfc = new OrderReport.ServiceForOrderClient();
                    //string bbcAsk = string.Empty;
                    string bbcAsk = sfc.createOrder(hdRequest, infoOrder, out message, out orderCode, out error1, out error2);
                    if (error1 != null && error1.Length > 0)
                    {
                        for (int i = 0; i < error1.Length; i++)
                        {
                            if (error1[i]!=null)
                            {
                                errorMsg += error1[i].errorMessage ?? string.Empty;
                            }
                        }
                    }

                    string s1 = message;
                    if (!s1.Contains("成功"))
                    {
                        s += "(" + payId + ")message:" + message + ",errorMsg:" + errorMsg + "\r\n";
                    }
                    else
                    {
                        success += payId + ",";
                    }


                    model.orderCustomsResult mdorderCustomsResult = new model.orderCustomsResult();
                    mdorderCustomsResult.BBCask = int.Parse(bbcAsk);
                    mdorderCustomsResult.BBCmessage = message; //返回信息
                    mdorderCustomsResult.BBCorderCode = orderCode;
                    mdorderCustomsResult.BBCerrorMessage = errorMsg;//返回错误信息
                    mdorderCustomsResult.BBCstatus = bbcAsk;
                    mdorderCustomsResult.BBCReturnData = DateTime.Now;

                    mdorderCustomsResult.SJOrgOrderChildId = payId;
                    string result = ordercustomsresultBll.updateUploadStatus2(mdorderCustomsResult);
                    if (result.Contains("成功"))
                    {
                        success += payId + ",";
                    }
                    else
                    {
                        if (result.Contains("信息记录"))
                        {
                            fail += payId + "(" + result + ")" + ",";
                        }
                        else
                        {
                            fail += payId + ",";
                        }
                    }
                }
                catch (Exception ex)
                {
                    s += "执行到订单(" + payId + ")时出现异常,error:" + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "联邦报关成功;\r\n";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "失败;\r\n";
            }

            ordercustomsresultBll = null;
            apiorderBll = null;
            MdApiOrder = null;
            MdApiOrderDetails = null;

            return success + fail + s;
        }


        /// <summary>
        /// 更新联邦订单报备
        /// </summary>
        /// <returns></returns>
        public string BBCUpdateOrderPeport()
        {
            bll.ordercustomsresultbll ordercustomsresultBll = new bll.ordercustomsresultbll();
            bll.apiorderbll apiorderBll = new bll.apiorderbll();
            bll.apiorderexpressbll apiorderexpressBll = new bll.apiorderexpressbll();
            model.apiOrder MdApiOrder = new model.apiOrder();
            List<model.apiOrderDetails> MdApiOrderDetails = new List<model.apiOrderDetails>();

            
            OrderReport.errorType[] error1 = new OrderReport.errorType[1];
            OrderReport.errorType[] error2 = new OrderReport.errorType[1];
            string message = string.Empty;
            string orderCode = "SOC01390000011"; //联邦返回的系统订单交易号（联邦的订单号，非下单的订单号）
            //string orderCode = string.Empty; //联邦返回的系统订单交易号（联邦的订单号，非下单的订单号）
            string orderId = string.Empty;
            string s = string.Empty;
            string expressNo = string.Empty; //获取运单号
            string success = string.Empty;  //成功
            string fail = string.Empty;     //失败
            string errorMsg = string.Empty; //联邦返回错误信息

            orderId = Request.Form["orderId"].ToString();
            //orderId = "380,";
            orderId = orderId.Trim(',');
            string[] orderIds = helpcommon.StrSplit.StrSplitData(orderId, ','); //处理批量订单

            for (int h = 0; h < orderIds.Length; h++)
            {
                MdApiOrder = apiorderBll.getOrderMsg(orderIds[h]);
                expressNo = apiorderexpressBll.expressNo(orderIds[h]); //获取运单号
                MdApiOrderDetails = apiorderBll.getOrderDetailsMsg(orderId, "");

                OrderReport.createOrderRequest cor = new OrderReport.createOrderRequest();
                OrderReport.HeaderRequest hdRequest = new OrderReport.HeaderRequest();
                //测试
                //hdRequest.appKey = "4f2e13b1569911a45cd742e9219c4fd1";
                //hdRequest.appToken = "8AC50A04A094A0C5";
                //hdRequest.customerCode = "C0146";

                //正式
                hdRequest.appKey = "695a2742bbd6acc11f5f183539938944";
                hdRequest.appToken = "7F811285C6E5BC38";
                hdRequest.customerCode = "C0139";
                try
                {
                    #region 详情
                    OrderReport.productDeatilType[] details = new OrderReport.productDeatilType[MdApiOrderDetails.Count];
                    int det1 = 0;
                    double produtWeight = 0; //毛重
                    foreach (var item in MdApiOrderDetails)
                    {
                        ///如果货号中包含yy，那么说明此货号为前期报备错误的货号，需去掉yy重新报备
                        string newScode = item.detailsScode.Substring(item.detailsScode.Length - 2, 2).Contains("yy") == true ? item.detailsScode.Replace("yy", "") : item.detailsScode;
                        //string newScode = "K15027C21BLKF";
                        float[] price = { float.Parse(item.detailsItemPrice.ToString()) };
                        float[] allPrice = { float.Parse((item.detailsItemPrice * item.detailsSaleCount).ToString()) };
                        details[det1] = new OrderReport.productDeatilType()
                        {
                            dealPrice = price,
                            opQuantity = helpcommon.ParmPerportys.GetNumParms(item.detailsSaleCount),
                            //productSku = item.detailsScode, 
                            productSku = newScode,
                            transactionPrice = allPrice
                        };
                        bll.ProductBll myProductBll = new bll.ProductBll();
                        //model.product mdProduct = myProductBll.getProductScode(item.detailsScode);
                        model.product mdProduct = myProductBll.getProductScode(newScode);
                        produtWeight += (float.Parse(mdProduct.Def15) / 1000);

                        det1++;
                    }
                    #endregion

                    OrderReport.UpdateOrderInfo infoOrder = new OrderReport.UpdateOrderInfo();
                    infoOrder.orderCode = orderCode;
                    infoOrder.currencyCode = "RMB";//币种 默认 RMB
                    //infoOrder.deliveryFee = "";
                    infoOrder.grossWt = produtWeight.ToString();//毛重 单位 kg
                    infoOrder.idNumber = MdApiOrder.def1;//证件号码
                    infoOrder.idType = "身份证";//证件类型（传以下汉字之一）:身份证,护照,其它
                    //infoOrder.netWt = "";//净重 单位 kg
                    //infoOrder.oabCity = "深圳";// MdApiOrder.cityId;//收件人城市
                    infoOrder.oabCity = MdApiOrder.cityId;//收件人城市
                    //infoOrder.oabCompany = "";//收件人公司名
                    infoOrder.oabCountry = "CN";//收件人国家二字码 国家二字码对照表见附件 3 //MdProduct.ciqAssemCountry.Split(new char[]{'-'})[0].ToString()
                    //infoOrder.oabEmail = "";//电子邮件
                    infoOrder.oabName = MdApiOrder.realName;//收件人姓名
                    infoOrder.oabPhone = MdApiOrder.phone;//收件人电话
                    infoOrder.oabPostcode = MdApiOrder.postcode;//收件人邮编
                    //infoOrder.oabStateName = "广东";//MdApiOrder.provinceId;//收件人省份 省份名称对照表见附件 1
                    infoOrder.oabStateName = MdApiOrder.provinceId;//收件人省份 省份名称对照表见附件 1
                    infoOrder.oabStreetAddress1 = MdApiOrder.provinceId + MdApiOrder.cityId + MdApiOrder.district + MdApiOrder.buyNameAddress;//收件人地址 1
                    //infoOrder.oabStreetAddress2 = "";
                    infoOrder.orderProduct = details;//订单产品详情
                    infoOrder.orderStatus = 2;//提交订单状态 1：草稿2：确认
                    infoOrder.referenceNo = MdApiOrder.orderId;//交易订单号
                    //infoOrder.remark = "";//备注
                    infoOrder.smCode = "FEDEX";//运输方式代码，固定填FEDEX
                    //infoOrder.trackingNumber = expressNo;//运单号
                    
                    infoOrder.trackingNumberIn = expressNo;//国内运单号
                    infoOrder.serviceType = "1";//服务类型：1、一票到底，2、两段式


                    OrderReport.ServiceForOrderClient sfc = new OrderReport.ServiceForOrderClient();
                    
                    string bbcAsk = sfc.updateOrder(hdRequest, infoOrder, out message, out orderCode, out error1);
                    //string bbcAsk = sfc.updateOrder(hdRequest, infoOrder, out message, out orderCode, out error1, out error2);
                    if (error1 != null && error1.Length > 0)
                    {
                        for (int i = 0; i < error1.Length; i++)
                        {
                            errorMsg += error1[i].errorMessage ?? string.Empty;
                        }

                    }
                    s = message;
                    if (!string.IsNullOrWhiteSpace(s) || error1!=null)
                    {
                        if (error1.Length > 1 && !string.IsNullOrWhiteSpace(error1[0].errorMessage))
                        {
                            return "执行到订单(" + orderIds[h] + ")时出现数据错误,停止后续操作," + message + "(" + errorMsg + ")";
                        }
                    }

                    model.orderCustomsResult mdorderCustomsResult = new model.orderCustomsResult();
                    mdorderCustomsResult.BBCask = int.Parse(bbcAsk);
                    mdorderCustomsResult.BBCmessage = message; //返回信息
                    mdorderCustomsResult.BBCorderCode = orderCode;
                    mdorderCustomsResult.BBCerrorMessage = errorMsg;//返回错误信息
                    mdorderCustomsResult.BBCstatus = bbcAsk;
                    mdorderCustomsResult.BBCReturnData = DateTime.Now;

                    mdorderCustomsResult.SJOrgOrderChildId = orderIds[h].ToString();
                    string result = ordercustomsresultBll.updateUploadStatus2(mdorderCustomsResult);
                    if (result.Contains("成功"))
                    {
                        //s = "订单" + orderIds[h].ToString() + "报关成功";
                        success += orderIds[h].ToString() + ",";
                    }
                    else
                    {
                        //s = "订单" + orderIds[h].ToString() + "报关成功,状态修改失败;" + result;
                        fail += orderIds[h].ToString() + ",";
                    }
                }
                catch (Exception ex)
                {
                    return "执行到订单(" + orderIds[h] + ")时出现异常,error:" + ex.Message;
                }
            }

            if (!string.IsNullOrWhiteSpace(success))
            {
                success += "联邦报关成功;";
            }

            if (!string.IsNullOrWhiteSpace(fail))
            {
                fail += "联邦报关成功，更改状态失败;";
            }

            ordercustomsresultBll = null;
            apiorderBll = null;
            MdApiOrder = null;
            MdApiOrderDetails = null;

            return s + success + fail;
        }


        /// <summary>
        /// 修改联邦订单状态
        /// </summary>
        /// <returns></returns>
        public string BBCOrderUpdateStatus()
        {
            string s = string.Empty;
            string success = string.Empty; //修改状态成功
            string fail = string.Empty; //修改状态失败
            string errorMsg = string.Empty; //错误信息
            string orderId = string.Empty;

            string orderCode = string.Empty; //返回的订单号
            int orderStatus = 2; //修改订单状态(0：删除,    1：草稿,   2：确认)
            string[] message = new string[1]; //返回信息
            OrderReport.errorType[] error1 = new OrderReport.errorType[1]; //错误信息
            
            orderId = Request.Form["orderId"].ToString();
            orderId = orderId.Trim(',');
            string[] orderIds = helpcommon.StrSplit.StrSplitData(orderId, ','); //处理批量orderId

            OrderReport.HeaderRequest hdRequest = new OrderReport.HeaderRequest();
            //测试
            //hdRequest.appKey = "4f2e13b1569911a45cd742e9219c4fd1";
            //hdRequest.appToken = "8AC50A04A094A0C5";
            //hdRequest.customerCode = "C0146";

            //正式
            hdRequest.appKey = "695a2742bbd6acc11f5f183539938944";
            hdRequest.appToken = "7F811285C6E5BC38";
            hdRequest.customerCode = "C0139";

            for (int j = 0; j < orderIds.Length; j++)
            {
                //orderCode = orderIds[j].ToString();
                orderCode = "SOC01390000011";
                OrderReport.ServiceForOrderClient sfc = new OrderReport.ServiceForOrderClient();
                string[] s2 = sfc.updateOrderStatus(hdRequest, ref orderCode, orderStatus, out message, out error1);
                

                for (int i = 0; i < error1.Length; i++)
                {
                    errorMsg += error1[i].errorMessage;
                }

                //s = "订单" + orderCode + "更改状态" + ss + "(" + ss1[0] + ")" + errorMsg + "\r\n";
            }
            return s;
        }
        #endregion



    }


    /// <summary>
    /// 海关报备加密
    /// </summary>
    public static class AESHelper
    {
        /// <summary>
        ///  密钥
        /// </summary>
        private static String Key = "MYgGnQE2+DAS973vd1DFHg==";

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            System.Security.Cryptography.ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
            //return Encoding.GetEncoding("gb2312").GetString(resultArray);// Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        ///  加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            System.Security.Cryptography.RijndaelManaged rm = new System.Security.Cryptography.RijndaelManaged
            {
                Key = Convert.FromBase64String(Key),
                Mode = System.Security.Cryptography.CipherMode.ECB,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }

}
