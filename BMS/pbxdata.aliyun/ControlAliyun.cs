using Aliyun.OpenServices.OpenStorageService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aliyun
{
    public class ControlAliyun
    {
        /// <summary>
        /// 根据款号查询文件夹位置
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public string GetObjectlist(string style)
        {
             
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            const string bucketName = "best-bms";
            ListPartsRequest lir = new ListPartsRequest(bucketName, accessKey, accessId);
            Aliyun.OpenServices.ClientConfiguration configu=new Aliyun.OpenServices.ClientConfiguration();
            OssClient client = new OssClient(endpoint, accessId, accessKey);
            string Uploadpath = "";
            DataTable tab = new DataTable();
            tab.Columns.Add("Cat", typeof(string));
            tab.Columns.Add("Cat2", typeof(string));
            tab.Columns.Add("Style", typeof(string));
            tab.Columns.Add("Scode", typeof(string));
            tab.Columns.Add("Image", typeof(string));
            try
            {
                ObjectListing list = client.ListObjects(bucketName);
                IEnumerable<OssObjectSummary> objects = list.ObjectSummaries;

                string[] arr;
                foreach (OssObjectSummary ob in objects)
                {
                    arr = ob.Key.Split('/');
                    if (arr.Length == 5)
                    {
                        tab.Rows.Add(new object[] { arr[1], arr[2], arr[3], arr[4] });
                        if (arr[3] == style)//查找style文件夹
                        {
                            Uploadpath = arr[0] + "/" + arr[1] + "/" + arr[2] + "/" + arr[3] + "/";
                        }
                    }

                }
            }
            catch (OssException ex)
            {
            }
            return Uploadpath;
        }
        public string GetObjectlistStyle(string Style)
        {

            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            const string bucketName = "best-bms";
            ListPartsRequest lir = new ListPartsRequest(bucketName, accessKey, accessId);
            OssClient client = new OssClient(endpoint, accessId, accessKey);
            string Uploadpath = "";
            DataTable tab = new DataTable();
            tab.Columns.Add("Cat", typeof(string));
            tab.Columns.Add("Cat2", typeof(string));
            tab.Columns.Add("Style", typeof(string));
            tab.Columns.Add("Scode", typeof(string));
            tab.Columns.Add("Image", typeof(string));
            try
            {
                ObjectListing list = client.ListObjects(bucketName);
                IEnumerable<OssObjectSummary> objects = list.ObjectSummaries;

                string[] arr;
                foreach (OssObjectSummary ob in objects)
                {
                    arr = ob.Key.Split('/');
                    if (arr.Length == 6)
                    {
                        tab.Rows.Add(new object[] { arr[1], arr[2], arr[3], arr[4], arr[5] });
                        if (arr[4] == Style)
                        {
                            Uploadpath = arr[0] + "/" + arr[1] + "/" + arr[2] + "/" + arr[3] + "/" + arr[4] + "/";
                        }
                    }

                }
            }
            catch (OssException ex)
            {
            }
            return Uploadpath;
        }
        /// <summary>
        /// 判断文件夹位置根据货号
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public string GetObjectlistScode(string Scode)
        {

            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            const string bucketName = "best-bms";
            ListPartsRequest lir = new ListPartsRequest(bucketName, accessKey, accessId);
            OssClient client = new OssClient(endpoint, accessId, accessKey);
            string Uploadpath = "";
            DataTable tab = new DataTable();
            tab.Columns.Add("Cat", typeof(string));
            tab.Columns.Add("Cat2", typeof(string));
            tab.Columns.Add("Style", typeof(string));
            tab.Columns.Add("Scode", typeof(string));
            tab.Columns.Add("Image", typeof(string));
            try
            {
                ObjectListing list = client.ListObjects(bucketName);
                IEnumerable<OssObjectSummary> objects = list.ObjectSummaries;

                string[] arr;
                foreach (OssObjectSummary ob in objects)
                {
                    arr = ob.Key.Split('/');
                    if (arr.Length == 6)
                    {
                        tab.Rows.Add(new object[] { arr[1], arr[2], arr[3], arr[4],arr[5] });
                        if (arr[4] == Scode)
                        {
                            Uploadpath = arr[0] + "/" + arr[1] + "/" + arr[2] + "/" + arr[3] + "/" + arr[4] + "/";
                        }
                    }

                }
            }
            catch (OssException ex)
            {
            }
            return Uploadpath;
        }
        /// <summary>
        /// 判断该款文件夹是否存在
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public bool StyleExist(string style)
        {
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            const string bucketName = "best-bms";
            OssClient client = new OssClient(endpoint, accessId, accessKey);
            bool result = false;
            List<string> listStyle = new List<string>(); ;
            try
            {
                ObjectListing list = client.ListObjects(bucketName);
                IEnumerable<OssObjectSummary> objects = list.ObjectSummaries;

                string[] arr;
                foreach (OssObjectSummary ob in objects)
                {
                    arr = ob.Key.Split('/');
                    if (arr.Length == 5)
                    {
                        if (arr[4]!="")
                        listStyle.Add(arr[4].Split('.')[0]);
                    }
                }
            }
            catch (OssException ex)
            {
                return false;
            }
            foreach (var temp in listStyle)
            {
                if (style == temp)
                    result = true;

            }
            return result;
        }
        /// <summary>
        /// 判断该货号文件夹是否存在
        /// </summary>
        /// <param name="Scode"></param>
        /// <returns></returns>
        public bool ScodeExist(string Scode)
        {
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            const string bucketName = "best-bms";
            OssClient client = new OssClient(endpoint, accessId, accessKey);
            bool result = false;
            List<string> listStyle = new List<string>(); ;
            try
            {
                ObjectListing list = client.ListObjects(bucketName);
                IEnumerable<OssObjectSummary> objects = list.ObjectSummaries;

                string[] arr;
                foreach (OssObjectSummary ob in objects)
                {
                    arr = ob.Key.Split('/');
                    if (arr.Length == 6)
                    {
                        if (arr[4] != "")
                            listStyle.Add(arr[4].Split('.')[0]);
                    }
                }
            }
            catch (OssException ex)
            {
                return false;
            }
            foreach (var temp in listStyle)
            {
                if (Scode == temp)
                    result = true;

            }
            return result;
        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="key"></param>
        public void CreateEmptyFolder(string key)
        {
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            const string bucketName = "best-bms";
            var client = new OssClient(endpoint, accessId, accessKey);
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    PutObjectResult ret = client.PutObject(bucketName, key, memStream);
                }
            }
            catch (OssException ex)
            {
                if (ex.ErrorCode == OssErrorCode.BucketAlreadyExists)
                {
                    Console.WriteLine("Bucket '{0}' already exists, please modify and recreate it.", bucketName);
                }
                else
                {
                    Console.WriteLine("CreateBucket Failed with error info: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                        ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
                }
            }
        }
        /// <summary>
        /// 进行对象存储
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        public void PutObject(string filename, string path)
        {
            //初始化 OSSClient
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            string bucketName = "best-bms";
            OssClient client = new OssClient(endpoint, accessId, accessKey);

            //定义文件流
            var objStream = new System.IO.FileStream("~/image/" + filename, System.IO.FileMode.OpenOrCreate);
            //定义 object 描述
            var objMetadata = new ObjectMetadata();
            var objKey = path + filename;//文件名

            //执行 put 请求，并且返回对象的MD5摘要。
            var putResult = client.PutObject(bucketName, objKey, objStream, objMetadata);
        }
        /// <summary>
        /// 获取一个存储对象
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="objKey">对象标识名称</param>
        public void GetObject(string bucketName, string objKey)
        {
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            OssClient client = new OssClient(endpoint, accessId, accessKey);

            //获取对象
            var obj = client.GetObject(bucketName, objKey);
            //获取Object的输入流
            var objStream = obj.Content;

            //怎么处理数据流，您看着办吧。
            //...

            //最后关闭数据流。
            objStream.Close();
        }
        /// <summary>
        /// Sample for deleting objects.
        /// </summary>
        public void DeleteObject(string key)
        {
            const string accessId = "X9foTnzzxHCk6gK7";
            const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
            const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
            const string bucketName = "best-bms";
            OssClient client = new OssClient(endpoint, accessId, accessKey);
            try
            {
                client.DeleteObject(bucketName, key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with error info: {0}", ex.Message);
            }

           
        }
    }
}
