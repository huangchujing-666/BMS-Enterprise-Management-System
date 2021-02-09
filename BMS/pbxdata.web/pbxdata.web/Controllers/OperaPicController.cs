using Aliyun.OpenServices.OpenStorageService;
using Maticsoft.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class OperaPicController : BaseController
    {
        

        aliyun.ControlAliyun CAliyun = new aliyun.ControlAliyun();
        bll.ProductBll pro = new bll.ProductBll();
        bll.Attributebll attrbll = new bll.Attributebll();
        model.AttributeModel Attrmm = new model.AttributeModel();
        model.product Promm = new model.product();
        PublicHelpController ph = new PublicHelpController();
        //string rootName = "TestImage/";//调试用
        string rootName = "images/";//正式用
        //
        // GET: /Product/
        const string accessId = "X9foTnzzxHCk6gK7";
        const string accessKey = "ArQYcpKLbaGweM8p1LQDq5kG1VIMuz";
        const string endpoint = "http://oss-cn-shenzhen.aliyuncs.com";
        string bucketName = "best-bms";


        public ActionResult PeituExchange()
        {
            ViewData["style"] = Request.QueryString["style"].ToString();
            return View();
        }
        /// <summary>
        /// 获取配图
        /// </summary>
        /// <returns></returns>
        public string GetPeituPic()
        {
            string s = string.Empty;
            string style = Request.Form["style"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def2,1)='2' and Def4='1' order by CONVERT(int,substring(def2,3,LEN(def2)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["stylePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存配图顺序
        /// </summary>
        /// <returns></returns>
        public string SavePeitu()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update stylepic set def2='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除配图
        /// </summary>
        /// <returns></returns>
        public string DeletePeituPic()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update stylepic set Def2='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }


        public ActionResult XiJieExchange()
        {
            ViewData["style"] = Request.QueryString["style"].ToString();
            return View();
        }

        /// <summary>
        /// 获取细节图
        /// </summary>
        /// <returns></returns>
        public string GetXiJiePic()
        {
            string s = string.Empty;
            string style = Request.Form["style"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def3,1)='2' and Def4='1' order by CONVERT(int,substring(def3,3,LEN(def3)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["stylePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存细节图顺序
        /// </summary>
        /// <returns></returns>
        public string SaveXiJietu()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update stylepic set def3='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除细节图
        /// </summary>
        /// <returns></returns>
        public string DeleteXiJiePic()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update stylepic set Def3='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }


        public ActionResult ZhutuExchange()
        {
            ViewData["style"] = Request.QueryString["style"].ToString();
            return View();
        }

        /// <summary>
        /// 获取主图--款号表
        /// </summary>
        /// <returns></returns>
        public string GetZhutuPic()
        {
            string s = string.Empty;
            string style = Request.Form["style"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def1,1)='2' and Def4='1' order by CONVERT(int,substring(def1,3,LEN(def1)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["stylePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存主图顺序--款号表
        /// </summary>
        /// <returns></returns>
        public string SaveZhutu()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update stylepic set def1='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除主图--款号表
        /// </summary>
        /// <returns></returns>
        public string DeleteZhutuPic()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update stylepic set Def1='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }

        /// <summary>
        /// 获取主图--货号表
        /// </summary>
        /// <returns></returns>
        public string GetZhutuPic1()
        {
            string s = string.Empty;
            string scode = Request.Form["scode"].ToString();
            DataTable dt = new DataTable();
            dt = DbHelperSQL.Query(@"select * from scodepic where scode='" + scode + "' and Left(Def1,1)='2' and Def4='1' order by CONVERT(int,substring(def1,3,LEN(def1)-2))").Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s += dt.Rows[i]["scodePicSrc"].ToString() + "@10q.jpg" + "***" + dt.Rows[i]["Id"] + "-*-";
            }

            return s.ToString();
        }

        /// <summary>
        /// 保存主图顺序--货号表
        /// </summary>
        /// <returns></returns>
        public string SaveZhutu1()
        {
            string Ids = Request.Form["Ids[]"].ToString();
            for (int i = 0; i < Ids.Split(',').Length; i++)
            {
                int n = i + 1;
                DbHelperSQL.ExecuteSql(@"Update scodepic set def1='2-" + n + "' where Id='" + Ids.Split(',')[i] + "'");
            }
            return "";
        }

        /// <summary>
        /// 删除主图--货号表
        /// </summary>
        /// <returns></returns>
        public string DeleteZhutuPic1()
        {
            string s = "";
            string Id = Request.Form["Id"].ToString();
            string sql = @"Update scodepic set Def1='1' where Id=" + Id + "";
            if (DbHelperSQL.ExecuteSql(sql) == -1)
            {
                s = "删除失败";
            }
            return s;
        }
        ///  <summary>
        /// 选择配图
        /// </summary>
        public bool SelectPeiPic()
        {
            bool flag = false;
            try
            {
                DataTable dt1 = new DataTable();
                string Ids = Request.QueryString["arryId"];
                string style = Request.QueryString["style"];
                string[] arryId;
                int n = 1;
                arryId = Ids.Split(',');
                string updatesql1 = string.Empty;
                dt1 = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def2,1)='2' and Def4='1' order by CONVERT(int,substring(def2,3,LEN(def2)-2))").Tables[0];
                if (dt1.Rows.Count > 0)//--进行重新排序
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        int item = Convert.ToInt32(i + 1);
                        updatesql1 = @"update stylepic set Def2='2-" + item + "' where Id='" + dt1.Rows[i]["Id"] + "'";
                        DbHelperSQL.ExecuteSql(updatesql1);
                    }
                    n = Convert.ToInt32(dt1.Rows.Count) + 1;
                }
                foreach (var item in arryId)
                {
                    string checksql = @"select * from stylepic where Id='" + item + "' and left(Def2,1)='2'";
                    if (DbHelperSQL.Query(checksql).Tables[0].Rows.Count == 0)
                    {
                        string Updatesql = @"update stylepic set Def2='2-" + n + "' where Id='" + item + "' ";
                        DbHelperSQL.ExecuteSql(Updatesql);
                        n++;
                    }

                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 选择主图-款号表
        /// </summary>
        /// <returns></returns>
        public bool SelectMainPicStyle()
        {
            bool flag = false;

            try
            {
                string style = Request.QueryString["style"];
                string srcs = Request.QueryString["Arrysrc"];
                string[] arrysrc;
                arrysrc = srcs.Split(',');
                int n = 1;
                //DataTable dt1 = new DataTable();
                //dt1 = DbHelperSQL.Query(@"select count(*) from stylepic where style='" + style + "' and Left(Def1,1)='2' and Def4='1'").Tables[0];
                //if (dt1.Rows[0][0].ToString() != "")
                //{
                //    n = Convert.ToInt32(dt1.Rows[0][0].ToString()) + 1;
                //}
                foreach (var item in arrysrc)
                {
                    string Updatesql = @"update stylepic set Def1='2-" + n + "' where style='" + style + "' and stylePicSrc='" + item.Split('@')[0] + "' ";
                    DbHelperSQL.ExecuteSql(Updatesql);
                    n++;
                }

            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;

        }
        ///  <summary>
        /// 选择细节图
        /// </summary>
        public bool SelectDetailPic()
        {
            bool flag = false;
            try
            {
                DataTable dt1 = new DataTable();
                string Ids = Request.QueryString["arryId"];
                string style = Request.QueryString["style"];
                string[] arryId;
                arryId = Ids.Split(',');
                int n = 1;
                string updatesql1 = string.Empty;
                dt1 = DbHelperSQL.Query(@"select * from stylepic where style='" + style + "' and Left(Def3,1)='2' and Def4='1' order by CONVERT(int,substring(def3,3,LEN(def3)-2))").Tables[0];
                if (dt1.Rows.Count > 0)//--进行重新排序
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        int item = Convert.ToInt32(i + 1);
                        updatesql1 = @"update stylepic set Def3='2-" + item + "' where Id='" + dt1.Rows[i]["Id"] + "'";
                        DbHelperSQL.ExecuteSql(updatesql1);
                    }
                    n = Convert.ToInt32(dt1.Rows.Count) + 1;
                }

                foreach (var item in arryId)
                {
                    string checksql = @"select * from stylepic where Id='" + item + "' and left(Def3,1)='2'";
                    if (DbHelperSQL.Query(checksql).Tables[0].Rows.Count == 0)
                    {
                        string Updatesql = @"update stylepic set Def3='2-" + n + "' where Id='" + item + "' ";
                        DbHelperSQL.ExecuteSql(Updatesql);
                        n++;
                    }

                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        //上传图片保存到服务器
        #region
        public string UploadminImg(FormContext from)
        {
            var file = Request.Files["Filedata"];
            var style = Request.QueryString["style"];

            bool blean = false;
            string uploadPath = Server.MapPath("~/UploadImage/");
            string url = "/UploadImage/" + file.FileName.Replace("+", "-");
            if (file != null)
            {
                file.SaveAs(uploadPath + file.FileName.Replace("+", "-"));
                return url;
            }
            return blean.ToString();
        }
        #endregion

        /// <summary>
        /// 图片存到阿里云--主款号表
        /// </summary>
        /// <param name="filename"></param>//本地图片路径
        /// <param name="objkey"></param>//储存到云名称
        public void PutObject(string filename, string objkey)
        {
            OssClient client = new OssClient(endpoint, accessId, accessKey);

            //定义文件流
            var objStream = new System.IO.FileStream(Server.MapPath("~/UploadImage/") + filename, System.IO.FileMode.OpenOrCreate);
            //定义 object 描述
            var objMetadata = new ObjectMetadata();
            //执行 put 请求，并且返回对象的MD5摘要。
            var putResult = client.PutObject(bucketName, objkey.Replace("*", "_"), objStream, objMetadata);
        }
        ////鼠标指到相应款号上显示缩略图
        //public string retImgPath()
        //{
        //    string style = Request.QueryString["style"];
        //    string sql = @"select stylePicSrc from styledescript where style='" + style + "' ";
        //    string path = "";
        //    if (DbHelperSQL.Query(sql).Tables[0].Rows.Count != 0)
        //    {
        //        path = DbHelperSQL.Query(sql).Tables[0].Rows[0][0].ToString();
        //    }
        //    return path;
        //}

        /// <summary>
        /// 图片存到阿里云--主货号表
        /// </summary>
        public bool UploadScodePic()
        {
            bool flag = false;
            try
            {
                int i = 1;
                DataTable dtpath = new DataTable();
                DataTable dtcount = new DataTable();
                string srcs = Request.QueryString["Arrysrc"];
                string scode = Request.QueryString["scode"];
                string[] arrysrc;
                string filename = "";
                arrysrc = srcs.Split(',');
                string Picurl;
                string path = CAliyun.GetObjectlistScode(scode.Replace("*", "_"));//得到货号文件夹路径  
                string pathsql = @"select * from product where Scode='" + scode + "'";
                dtpath = DbHelperSQL.Query(pathsql).Tables[0];
                //得到货号文件夹路径  rootName+品牌缩写+类别+款号+货号+文件名
                string newpath = rootName + dtpath.Rows[0]["Cat"] + "/" + dtpath.Rows[0]["Cat2"] + "/" + dtpath.Rows[0]["Style"].ToString() + "/" + dtpath.Rows[0]["Scode"].ToString() + "/";
                dtcount = DbHelperSQL.Query(@"select count(*) from scodepic where scode='" + scode + "' ").Tables[0];
                i = Convert.ToInt32(dtcount.Rows[0][0]) + 1;//判断数据中对应货号存放图片的个数
                //string num = dtcount.Rows[dtcount.Rows.Count - 1]["scodePicSrc"].ToString();
                //i = Convert.ToInt32(num.Substring(num.LastIndexOf('/') + 1).Split('_')[0]) + 1;//判断数据中对应货号存放图片的个数
                if (CAliyun.ScodeExist(scode.Replace("*", "_")))//判断该货号文件夹是否存在
                {
                    foreach (var temp in arrysrc)
                    {
                        filename = scode.Replace(".", "-") + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[temp.Split('.').Length - 1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.pbxluxury.com/" + path + i + "_" + filename;

                        string sql = @"insert into scodepic values ('" + scode + "','" + Picurl.Replace("*", "_") + "','1','1','1','" + userInfo.User.Id + "','1','1','1','1','" + getFile.Substring(0, getFile.LastIndexOf('.')) + "')";
                        int n = DbHelperSQL.ExecuteSql(sql);
                        if (n != -1)
                        {
                            PutObject(getFile, path + i + "_" + filename);//存入新上传图片
                        }
                        i++;

                    }
                }
                else
                {
                    newpath = newpath.Replace("*", "_");
                    CAliyun.CreateEmptyFolder(newpath);//创建款号文件夹
                    foreach (var temp in arrysrc)
                    {
                        filename = scode.Replace(".", "-") + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[temp.Split('.').Length - 1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.pbxluxury.com/" + newpath + i + "_" + filename;
                        string sql = @"insert into scodepic values ('" + scode + "','" + Picurl.Replace("*", "_") + "','1','1','1','" + userInfo.User.Id + "','1','1','1','1','" + getFile.Substring(0, getFile.LastIndexOf('.')) + "')";
                        int n = DbHelperSQL.ExecuteSql(sql);
                        if (n != -1)
                        {
                            PutObject(getFile, newpath + i + "_" + filename);//存入新上传图片

                        }
                        i++;
                    }
                }
                flag = true;
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string updatetime = @"update product set Def1='" + date + "',Def9='0' where Scode='" + scode + "'";
                DbHelperSQL.ExecuteSql(updatetime);

            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 图片存到阿里云--主款号表
        /// </summary>
        public bool UploadScodePicStyle()
        {
            bool flag = false;
            try
            {
                int i = 1;
                DataTable dtpath = new DataTable();
                DataTable dtcount = new DataTable();
                string srcs = Request.QueryString["Arrysrc"];
                string Style = Request.QueryString["Style"];
                string[] arrysrc;
                string filename = "";
                arrysrc = srcs.Split(',');
                string Picurl;
                string path = CAliyun.GetObjectlistStyle(Style);//得到款号文件夹路径
                string pathsql = @"select * from product where Style='" + Style + "'";
                dtpath = DbHelperSQL.Query(pathsql).Tables[0];
                string newpath = rootName + dtpath.Rows[0]["Cat"] + "/" + dtpath.Rows[0]["Cat2"] + "/" + dtpath.Rows[0]["Style"] + "/" + dtpath.Rows[0]["Style"] + "/";
                dtcount = DbHelperSQL.Query(@"select count(*) from stylepic where style='" + Style + "' ").Tables[0];
                i = Convert.ToInt32(dtcount.Rows[0][0]) + 1;//判断数据中对应款号存放图片的个数
                if (CAliyun.ScodeExist(Style.Replace("*", "_")))//判断该款号文件夹是否存在
                {
                    foreach (var temp in arrysrc)
                    {
                        filename = Style + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[temp.Split('.').Length - 1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.pbxluxury.com/" + path + i + "_" + filename;
                        string sql = @"insert into stylepic values ('" + Style + "','" + Picurl.Replace("*", "_") + "','1','1','" + userInfo.User.Id + "','1','1','1','1','" + getFile.Substring(0, getFile.LastIndexOf('.')) + "')";
                        int n = DbHelperSQL.ExecuteSql(sql);
                        if (n != -1)
                        {
                            PutObject(getFile, path + i + "_" + filename);//存入新上传图片
                        }
                        i++;

                    }
                }
                else
                {
                    newpath = newpath.Replace("*", "_");
                    CAliyun.CreateEmptyFolder(newpath);//创建款号文件夹
                    foreach (var temp in arrysrc)
                    {
                        filename = Style + "." + temp.Substring(temp.LastIndexOf('/') + 1).Split('.')[temp.Split('.').Length - 1];
                        string getFile = temp.Substring(temp.LastIndexOf('/') + 1);
                        Picurl = "http://best-bms.pbxluxury.com/" + newpath + i + "_" + filename;
                        string sql = @"insert into stylepic values ('" + Style + "','" + Picurl.Replace("*", "_") + "','1','1','" + userInfo.User.Id + "','1','1','1','1','" + getFile.Substring(0, getFile.LastIndexOf('.')) + "')";
                        int n = DbHelperSQL.ExecuteSql(sql);
                        if (n != -1)
                        {
                            PutObject(getFile, newpath + i + "_" + filename);//存入新上传图片
                        }
                        i++;
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 返回所有通过货号上传的图片-货号表
        /// </summary>
        /// <returns></returns>
        public string retAllpicByScode()
        {
            string rtn = "";
            string src = "";
            DataTable dt1 = new DataTable();
            string scode = Request.QueryString["scode"];
            string strsql = @"select * from scodepic where scode='" + scode + "' and Def4='1'";
            //dt1 = DbHelperSQL.Query(strsql).Tables[0];
            //if (dt1.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt1.Rows.Count; i++)
            //    {
            //        rtn += dt1.Rows[i]["scodePicSrc"] + "-*-";
            //    }
            //}
            //return rtn;
            string checkMainPic = @"select * from product where scode='" + scode + "'";//查询缩略图
            string MainPic = "noPicture";
            if (DbHelperSQL.Query(checkMainPic).Tables[0].Rows.Count > 0)
            {
                MainPic = DbHelperSQL.Query(checkMainPic).Tables[0].Rows[0]["Imagefile"].ToString();//查询缩略图得到url
            }
            dt1 = DbHelperSQL.Query(strsql).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    //string State = dt1.Rows[i]["scodePicSrc"].ToString().Split('.')[dt1.Rows[i]["scodePicSrc"].ToString().Split('.').Length - 1];
                    string State = dt1.Rows[i]["scodePicSrc"].ToString().Substring(dt1.Rows[i]["scodePicSrc"].ToString().LastIndexOf('.') + 1);
                    if (MainPic == dt1.Rows[i]["scodePicSrc"].ToString() + "@10q." + State)//判断是否是缩略图
                    {
                        src = dt1.Rows[i]["scodePicSrc"] + "@10q." + State;

                        rtn += "<div style='float:left;text-align:center;margin-left:10px;width:100px;height:135px;'><img src=\"" + src + "\" title=\"" + dt1.Rows[i]["Def5"].ToString() + "\"  /><div style='clear: both;;height: 28px;overflow: hidden;'>" + dt1.Rows[i]["Def5"].ToString() + "</div><div><a href='#'>缩略图</a>&nbsp;<input type='checkbox' value=\"" + dt1.Rows[i]["Id"].ToString() + "\"/></div></div>";
                        //rtn += dt1.Rows[i]["scodePicSrc"] + "@10q." + State + "-*-";
                    }
                    else
                    {
                        src = dt1.Rows[i]["scodePicSrc"] + "@10q." + State;
                        rtn += "<div style='float:left;text-align:center;margin-left:10px;width:100px;height:135px;'><img src=\"" + src + "\" title=\"" + dt1.Rows[i]["Def5"].ToString() + "\"  /><div style='clear: both;;height: 28px;overflow: hidden;'>" + dt1.Rows[i]["Def5"].ToString() + "</div><div><a href='#'   onclick='UploadMinPic(\"" + src + "\")'>缩</a>&nbsp;<input type='checkbox' value=\"" + dt1.Rows[i]["Id"].ToString() + "\" /></div></div>";
                    }

                }
            }
            return rtn;

        }

        /// <summary>
        /// 返回所有通过货号上传的图片-款号表
        /// </summary>
        /// <returns></returns>
        public string retAllpicByScodeStyle()
        {
            StringBuilder rtn = new StringBuilder(200);
            string src = "";
            DataTable dt1 = new DataTable();
            string Style = Request.QueryString["Style"];
            string strsql = @"select * from stylepic where Style='" + Style + "' and Def4='1'";
            string checkMainPic = @"select stylePicSrc from styledescript where style='" + Style + "'";
            string MainPic = "noPicture";
            if (DbHelperSQL.Query(checkMainPic).Tables[0].Rows.Count > 0)
            {
                MainPic = DbHelperSQL.Query(checkMainPic).Tables[0].Rows[0][0].ToString();
            }
            dt1 = DbHelperSQL.Query(strsql).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    //string State = dt1.Rows[i]["stylePicSrc"].ToString().Split('.')[dt1.Rows[i]["stylePicSrc"].ToString().Length - 1];
                    string State = dt1.Rows[i]["stylePicSrc"].ToString().Substring(dt1.Rows[i]["stylePicSrc"].ToString().LastIndexOf('.') + 1);
                    if (MainPic == dt1.Rows[i]["stylePicSrc"].ToString() + "@10q." + State)
                    {
                        //rtn += "thisisselectpic" + "-*-";
                        //rtn += dt1.Rows[i]["stylePicSrc"] + "@10q." + State + "-*-";
                        src=dt1.Rows[i]["stylePicSrc"] + "@10q." + State;
                        rtn.Append("<div style='float:left;text-align:center;margin-left:10px;width:100px;height:135px;'><img src=\"" + src.ToString() + "\" title=\"" + dt1.Rows[i]["Def5"].ToString() + "\"  /><div style='clear: both;;height: 28px;overflow: hidden;'>" + dt1.Rows[i]["Def5"].ToString() + "</div><div><a href='#'>缩略图</a>&nbsp;<input type='checkbox' value=\"" + dt1.Rows[i]["Id"].ToString() + "\" /></div></div>");

                    }
                    else
                    {
                        //rtn += dt1.Rows[i]["stylePicSrc"] + "@10q." + State + "-*-";
                        src=dt1.Rows[i]["stylePicSrc"] + "@10q." + State;
                        rtn.Append("<div style='float:left;text-align:center;margin-left:10px;width:100px;height:135px;'><img src=\"" + src.ToString() + "\" title=\"" + dt1.Rows[i]["Def5"].ToString() + "\"  /><div style='clear: both;;height: 28px;overflow: hidden;'>" + dt1.Rows[i]["Def5"].ToString() + "</div><div><a href='#'   onclick='UploadMinPicStyle(\"" + src.ToString() + "\")'>缩</a>&nbsp;<input type='checkbox' value=\"" + dt1.Rows[i]["Id"].ToString() + "\" /></div></div>");

                    }

                }
            }
            return rtn.ToString();

        }

        /// <summary>
        /// 选择主图-货号表
        /// </summary>
        /// <returns></returns>
        public bool SelectMainPic()
        {
            bool flag = false;

            try
            {
                string scode = Request.QueryString["scode"];
                string srcs = Request.QueryString["Arrysrc"];
                string[] arrysrc;
                arrysrc = srcs.Split(',');
                int n = 1;
                //DataTable dt1 = new DataTable();
                //dt1 = DbHelperSQL.Query(@"select MAX(RIGHT(Def1,1)) from scodepic where scode='" + scode + "' and Left(Def1,1)='2' and Def4='1'").Tables[0];
                //if (dt1.Rows[0][0].ToString() != "")
                //{
                //    n = Convert.ToInt32(dt1.Rows[0][0].ToString()) + 1;
                //}
                foreach (var item in arrysrc)
                {
                    string Updatesql = @"update scodepic set Def1='2-" + n + "' where scode='" + scode + "' and scodePicSrc='" + item.Split('@')[0] + "' ";
                    DbHelperSQL.ExecuteSql(Updatesql);
                    n++;
                }

            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;


        }


        /// <summary>
        /// 显示主图-货号表
        /// </summary>
        /// <returns></returns>
        public string ShowMainPic()
        {
            string s = string.Empty;
            string temp = string.Empty;
            DataTable dt1 = new DataTable();
            string scode = Request.QueryString["scode"];
            string sql = @"select * from scodepic where scode='" + scode + "' and Left(Def1,1)='2' and Def4='1' Order by RIGHT(Def1,1) ";
            dt1 = DbHelperSQL.Query(sql).Tables[0];
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {


                    temp = dt1.Rows[i]["scodePicSrc"].ToString() + "@10q." + dt1.Rows[i]["scodePicSrc"].ToString().Substring(dt1.Rows[i]["scodePicSrc"].ToString().LastIndexOf('.') + 1);
                    s += temp + "-*-";
                }
            }
            return s;
        }
        /// <summary>
        /// 选择缩略图-货号表
        /// </summary>
        public bool UploadMinPic()
        {
            string scode = Request.QueryString["scode"];
            string Imagefile = Request.QueryString["Imagefile"];
            bool bean = false;
            try
            {
                string sql = @"update product set Imagefile='" + Imagefile + "' where scode='" + scode + "'";
                int n = DbHelperSQL.ExecuteSql(sql);
                if (n != -1)
                {
                    bean = true;
                }
            }
            catch (Exception ex)
            {
                bean = false;
            }
            return bean;
        }
        /// <summary>
        /// 选择缩略图-款号表
        /// </summary>
        public bool UploadMinPicStyle()
        {
            string style = Request.QueryString["Style"];
            string Imagefile = Request.QueryString["Imagefile"];
            bool bean = false;
            try
            {
                int n;
                string checkstyle = @"select * from styledescript where style='" + style + "'";
                if (DbHelperSQL.Query(checkstyle).Tables[0].Rows.Count > 0)
                {
                    string sql = @"update styledescript set stylePicSrc='" + Imagefile + "' where style='" + style + "'";
                    n = DbHelperSQL.ExecuteSql(sql);
                }
                else
                {
                    string sql = @"insert into styledescript values ('" + style + "','','" + Imagefile + "','1','1','','','','','')";
                    n = DbHelperSQL.ExecuteSql(sql);
                }

                if (n != -1)
                {
                    bean = true;
                }
            }
            catch (Exception ex)
            {
                bean = false;
            }
            return bean;
        }
        /// <summary>
        /// 删除图片 款号表
        /// </summary>
        public bool DeleteAllPicByStyle()
        {
            bool flag = false;

            int n = -1;
            try
            {

                OssClient client = new OssClient(endpoint, accessId, accessKey);

                string Ids = Request.QueryString["arryId"];
                string style = Request.QueryString["style"];
                string[] arryId;
                string src = "";
                arryId = Ids.Split(',');
                foreach (var item in arryId)
                {
                    string delsql = @"update stylepic set Def4='2',Def1='1',Def2='1',Def3='1',UserId='" + userInfo.User.Id + "' where Id='" + item + "' ";
                    src = DbHelperSQL.Query(@"select * from stylepic where Id='" + item + "'").Tables[0].Rows[0]["stylePicSrc"].ToString();
                    DbHelperSQL.ExecuteSql(@"update scodepic set Def4='2',Def1='1',Def2='1',Def3='1',UserId='" + userInfo.User.Id + "' where scodePicSrc='" + src + "' ");
                    n = DbHelperSQL.ExecuteSql(delsql);
                    client.DeleteObject(bucketName, src.Replace(@"http://best-bms.pbxluxury.com/", ""));
                }
                if (n != -1)
                {
                    flag = true;
                }

            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        ///  <summary>
        /// 删除图片 货号表
        /// </summary>
        public bool DeleteAllPicByScode()
        {
            bool flag = false;
            int n = -1;
            try
            {
                OssClient client = new OssClient(endpoint, accessId, accessKey);

                string Ids = Request.QueryString["arryId"];
                string scode = Request.QueryString["scode"];
                string src = "";
                string[] arryId;
                arryId = Ids.Split(',');
                foreach (var item in arryId)
                {
                    string delsql = @"update scodepic set Def4='2',Def1='1',Def2='1',Def3='1',UserId='" + userInfo.User.Id + "' where Id='" + item + "' ";
                    src = DbHelperSQL.Query(@"select * from scodepic where Id='" + item + "'").Tables[0].Rows[0]["scodePicSrc"].ToString();
                    DbHelperSQL.ExecuteSql(@"update stylepic set Def4='2',Def1='1',Def2='1',Def3='1',UserId='" + userInfo.User.Id + "' where stylePicSrc='" + src + "' ");
                    n = DbHelperSQL.ExecuteSql(delsql);
                    client.DeleteObject(bucketName, src.Replace(@"http://best-bms.pbxluxury.com/", ""));
                }
                if (n != -1)
                {
                    flag = true;
                }


            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        ///  <summary>
        /// 根据把该款号图片全复制给该货号
        /// </summary>
        public bool CopystylepicToscode()
        {
            bool flag = false;
            DataTable dt1 = new DataTable();
            try
            {
                string scode = Request.QueryString["scode"];
                string sql = @"select * from stylepic where style in(select Style from product where Scode='" + scode + "') and stylePicSrc not in(select scodePicSrc as stylePicSrc from scodepic where scode='" + scode + "') and Def4='1'";
                dt1 = DbHelperSQL.Query(sql).Tables[0];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string insertsql = @"insert into scodepic values('" + scode + "','" + dt1.Rows[i]["stylePicSrc"].ToString() + "','1','1','1','" + userInfo.User.Id + "','1','1','1','1','" + dt1.Rows[i]["Def5"].ToString() + "')";
                    DbHelperSQL.GetSingle(insertsql);
                }
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        ///  <summary>
        /// 根据货号把该款图片全复制给该款号
        /// </summary>
        public bool CopysocdepicTostyle()
        {
            bool flag = false;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            try
            {
                string Ids = Request.QueryString["arryId"];
                string scode = Request.QueryString["scode"];
                string[] arryId;
                arryId = Ids.Split(',');
                string style = DbHelperSQL.Query(@"select style from product where scode='" + scode + "'").Tables[0].Rows[0]["style"].ToString();

                for (int i = 0; i < arryId.Length; i++)
                {
                    string check = @"select * from stylepic where stylePicSrc=(select scodePicSrc from scodepic where Id='" + arryId[i] + "') ";
                    dt1 = DbHelperSQL.Query(check).Tables[0];
                    if (dt1.Rows.Count == 0)
                    {
                        dt2 = DbHelperSQL.Query(@"select * from scodepic where Id='" + arryId[i] + "'").Tables[0];
                        string insertsql = @"insert into stylepic values('" + style + "','" + dt2.Rows[0]["scodePicSrc"] + "','1','1','" + userInfo.User.Id + "','1','1','1','1','" + dt2.Rows[0]["Def5"] + "')";
                        DbHelperSQL.GetSingle(insertsql);
                    }

                }
                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
        
        ///// <summary>
        ///// 对调图片顺序
        ///// </summary>
        ///// <returns></returns>
        //public string Exchange()
        //{
        //    string handle = Request.Form["handle"].ToString();
        //    string oNear = Request.Form["oNear"].ToString();
        //    string type = Request.Form["type"].ToString();
        //    DataTable dt = new DataTable();
        //    string Item = DbHelperSQL.Query(@"select * from stylepic where Id='" + handle + "'").Tables[0].Rows[0][type].ToString();
        //    string Item2 = DbHelperSQL.Query(@"select * from stylepic where Id='" + oNear + "'").Tables[0].Rows[0][type].ToString();
        //    DbHelperSQL.ExecuteSql(@"update stylepic set " + type + "='" + Item2 + "' where Id='" + handle + "';update stylepic set " + type + "='" + Item + "' where Id='" + oNear + "';");
        //    return "";
        //}

       
    }
}
