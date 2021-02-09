/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       FtpHelp
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.helpcommon
    * 文 件 名：       FtpHelp
    * 创建时间：       2015-05-11 10:36:39
    * 作    者：       lcg
    * 说    明：       FTP上传下载帮助
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Configuration;
using System.Globalization;

namespace pbxdata.helpcommon
{
    public class FtpHelp
    {

        public static FtpStatusCode UploadFileInFTP(string filename)
        {
            Stream requestStream = null;
            FileStream fileStream = null;
            FtpWebResponse uploadResponse = null;
            FtpWebRequest uploadRequest = null;
            string serverIP;
            string userName;
            string password;
            string uploadurl;

            try
            {
                
                serverIP = System.Configuration.ConfigurationManager.AppSettings["FTPServerIP"];
                userName = System.Configuration.ConfigurationManager.AppSettings["SJUserName"];
                password = System.Configuration.ConfigurationManager.AppSettings["SJPassword"];
                uploadurl = "ftp://" + serverIP + "/" + Path.GetFileName(filename);
                uploadRequest = (FtpWebRequest)WebRequest.Create(uploadurl);
                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
                uploadRequest.Proxy = null;
                NetworkCredential nc = new NetworkCredential();
                nc.UserName = userName;
                nc.Password = password;
                uploadRequest.Credentials = nc;
                requestStream = uploadRequest.GetRequestStream();
                fileStream = File.Open(filename, FileMode.Open);

                byte[] buffer = new byte[1024];
                int bytesRead;

                while (true)
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    requestStream.Write(buffer, 0, bytesRead);
                }

                requestStream.Close();
                uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();
                return uploadResponse.StatusCode;
            }
            catch (Exception e)
            {
                SystemLog.logger(e.InnerException.Message);
            }
            finally
            {
                if (uploadResponse != null)
                {
                    uploadResponse.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                if (requestStream != null)
                {
                    requestStream.Close();
                }

            }
            return FtpStatusCode.Undefined;
        }


        /// <summary>
        /// FTP上传（现使用）
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <returns></returns>
        public static int UploadFtp(string serverIP, string userName, string password, string filename)
        {
            FtpWebRequest reqFTP = null;
            string url;

            try
            {
                FileInfo fileInf = new FileInfo(filename);
                url = serverIP + Path.GetFileName(filename);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Credentials = new NetworkCredential(userName, password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                strm.Close();
                fs.Close();
                return 0;
            }
            catch (Exception ex)
            {
                if (reqFTP != null)
                {
                    reqFTP.Abort();
                }
                SystemLog.logger(ex.InnerException.Message);
                return -2;
            }
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="serverIP">FTP地址</param>
        /// <param name="userName">FTP账号</param>
        /// <param name="password">FTP密码</param>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static int DownloadFtp(string serverIP, string userName, string password, string filename)
        {
            FtpWebRequest reqFTP;
            string url;

            try
            {
                url = serverIP + Path.GetFileName(filename);

                //FileStream outputStream = new FileStream(@"D:\CustomsFTPFile\sjProduct\out\"+filename, FileMode.Create);
                FileStream outputStream = new FileStream(filename, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(userName, password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
                
                return 0;
            }
            catch (Exception ex)
            {
                SystemLog.logger(ex.InnerException.Message);
                return -2;
            }
        }


        public class SystemLog
        {

            public static bool logger(string message)
            {
                try
                {
                    DateTime timeNow = DateTime.Now;
                    string filename = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
                    string logSwitch = System.Configuration.ConfigurationManager.AppSettings["LogSwitch"];
                    if (logSwitch == "1")
                    {
                        Encoding encoding = Encoding.GetEncoding("gb2312");
                        byte[] info = encoding.GetBytes("[ " + timeNow.ToString("yyyy-MM-dd HH:mm:ss") + " ] " + message + "\n"); //转换编码成字节串  

                        if (!filename.EndsWith(Path.DirectorySeparatorChar.ToString()))
                        {
                            filename = filename + Path.DirectorySeparatorChar.ToString();
                        }

                        using (FileStream fs = System.IO.File.Open(filename + timeNow.ToString("yyyy_MM_dd") + ".txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                        {
                            fs.Write(info, 0, info.Length);
                            //以ASCII方式编写  
                            using (StreamWriter w = new StreamWriter(fs, Encoding.ASCII))
                            {
                                w.Flush();
                                w.Close();
                            }

                            fs.Close();
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }



        /// <summary>  
        /// 获取文件列表    
        /// </summary>  
        /// <param name="ftpServerIP">服务器地址</param>  
        /// <param name="ftpUserID">FTP用户名</param>  
        /// <param name="ftpPassword">FTP密码</param>  
        /// <returns></returns>  
        public static string[] GetFileList(string ftpServerIP, string ftpUserID, string ftpPassword)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                // 根据uri创建FtpWebRequest对象     
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpServerIP));

                // 指定数据传输类型    
                reqFTP.UseBinary = true;

                // ftp用户名和密码    
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // 指定执行什么命令    
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();

                //获取文件流  
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();

                //如果有文件就将文件名添加到文件列表  
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                //result.Remove(result.ToString().LastIndexOf('\n'), 1);
                if (result.Length>0)
                {
                    result.Remove(result.ToString().LastIndexOf('\n'), 1);
                }
                else
                {
                    //关闭流  
                    reader.Close();
                    response.Close();
                    downloadFiles = null;
                    return downloadFiles;
                }

                //关闭流  
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ftpPath">ftp路径（如：ftp://192.168.0.124/out/ ）</param>
        /// <param name="username">FTP账号</param>
        /// <param name="password">FTP密码</param>
        /// <param name="ftpName">ftp上的文件名称</param>
        public static bool fileDelete(string ftpPath,string username,string password,string ftpName)
        {
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                //string uri = "ftp://192.168.0.124/out/" + ftpName;
                string uri = ftpPath + ftpName;
                //string url = ftpPath + ftpName;
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(username, password);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (ftpResponseStream != null)
                {
                    ftpResponseStream.Close();
                }
                if (ftpWebResponse != null)
                {
                    ftpWebResponse.Close();
                }
            }
            return success;
        }

    }
}

