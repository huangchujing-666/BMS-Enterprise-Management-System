using Maticsoft.DBUtility;
using pbxdata.bll;
using pbxdata.dal;
using pbxdata.model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class ReaderExcelController : BaseController
    {
        //
        // GET: /ReaderExcel/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReadExcel()
        {
            return View();
        }
        public ActionResult HomePage()
        {
            return View();
        }
        public ActionResult InsertPorpertyValue()
        {
            return View();
        }
        /// <summary>
        /// 上传excel文件到服务器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UploadExcel()
        {
            if (Request.Files.Count == 0)
            {
                //Request.Files.Count 文件数为0上传不成功
                // Response.Output.WriteLine("<script language='javascript'>parent.UploadCallback('fase|请选择文件！')</script>");
                return "请选择文件！";

            }
            if (!Request.Files[0].FileName.Contains(".xls"))
            {
                // Response.Output.WriteLine("<script language='javascript'>parent.UploadCallback('fase|请选择文件！')</script>");
                return "请选择excel文件！";

            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                //Response.Output.WriteLine("<script language='javascript'>parent.UploadCallback('fase|上传文件失败！')</script>");
                return "上传文件失败！";
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase file1 = Request.Files[0];
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                //服务器上的UpLoadFile文件夹必须有读写权限　　　
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss");
                string filePath = @"D:\Files\" + newFile + file1.FileName;
                file.SaveAs(filePath);
                string check = @"select * from excelFilePath where UserId=" + userInfo.User.Id + "";
                string sql = string.Empty;
                if (DbHelperSQL.Query(check).Tables[0].Rows.Count > 0)
                {
                    sql = @"update excelFilePath set FilePath='" + filePath + "' where UserId='" + userInfo.User.Id + "'";
                }
                else
                {
                    sql = @"insert into excelFilePath (FilePath,UserId) values ('" + filePath + "','" + userInfo.User.Id + "')";
                }
                DbHelperSQL.ExecuteSql(sql);
                ////Response.Output.WriteLine("<script language='javascript'>parent.UploadCallback('true|" + filePath + "')</script>");
                ////判断是否存在该文件
                //bool bol = System.IO.File.Exists(@"D:\ExcelPath" + userInfo.User.Id + ".txt");
                //if (bol)
                //{
                //    //存在则删除
                //    System.IO.File.Delete(@"D:\ExcelPath" + userInfo.User.Id + ".txt");
                //}
                //FileInfo f = new FileInfo(@"D:\ExcelPath" + userInfo.User.Id + ".txt");
                //FileStream stream = f.Open(FileMode.OpenOrCreate);
                //StreamWriter w = new StreamWriter(stream, System.Text.Encoding.UTF8);
                //w.Write(filePath);
                //w.Close();
                //stream.Close();
                return "<script>alert('上传成功！');</script>";
            }


        }
        /// <summary>
        ///得到当前excel文件的工作表名
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult GetFilePath(string Id)
        {
            Thread.Sleep(3000);
            //FileInfo file = new FileInfo(@"D:\ExcelPath" + userInfo.User.Id + ".txt");
            //bll.productPorpertybll bll = new bll.productPorpertybll();
            //productPorpertybll.SetOnAddProValAndOnAddProValDefeat(AddValError, AddProValDefeat);
            try
            {
                //Stream stream = file.Open(FileMode.OpenOrCreate);
                //StreamReader r = new StreamReader(stream, System.Text.Encoding.UTF8);
                //string filePath = r.ReadToEnd();
                //r.Close();
                //stream.Close();
                //file.Delete();
                //取得excel工作表名

                string filePath = @"" + rtnPath() + "";
                ExcelOperationbll excel = new ExcelOperationbll();
                Exception ex;
                DataTable dt = excel.GetTablesName(filePath, out ex);
                string str = "";
                foreach (DataRow dr in dt.Rows)
                {
                    str += dr["Table_Name"] != null ? dr["Table_Name"].ToString() : "";
                    str += "!";
                }
                str = str.Remove(str.Length - 1);
                return Content(str);
            }
            catch (Exception ex)
            {
                return Content("false|" + ex.Message);
            }
        }
        /// <summary>
        /// 打开单个工作表
        /// </summary>
        /// <param name="Id">工作表名</param>
        /// <returns></returns>
        public ActionResult btnOpen_Click(string Id)
        {
            Thread.Sleep(3000);
            if (Id != null)
            {
                //取得excel文件路径
                //FileInfo file = new FileInfo(@"D:\ExcelPath" + userInfo.User.Id + ".txt");
                //Stream stream = file.Open(FileMode.Open);
                //StreamReader r = new StreamReader(stream);
                //string filePath = r.ReadToEnd();
                //r.Close();
                //stream.Close();

                ExcelOperationbll excel = new ExcelOperationbll();
                Exception ex;
                DataSet ds = excel.GetData(@"" + rtnPath() + "", Id, out ex);

                if (ds != null)
                {

                    //dataGridView1.DataSource = ds.Tables[0];
                    //foreach (DataColumn v in ds.Tables[0].Columns)
                    //{
                    //    textBox3.Text += v.ColumnName + "\r\n";
                    //}
                    string html = "<table id='content'>";
                    List<string> list = new List<string>();
                    html += "<tr>";
                    foreach (DataColumn c in ds.Tables[0].Columns)
                    {
                        if (c.ColumnName != "")
                        {
                            list.Add(c.ColumnName);
                            html += "<td>" + c.ColumnName + "</td>";
                        }
                    }
                    html += "</tr>";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        
                        html += "<tr>";
                        foreach (string c in list)
                        {
                            html += "<td>" + (dr[c] == null ? "" : dr[c].ToString()) + "</td>";
                        }
                        html += "</tr>";

                    }
                    html += "</table>";
                    return Content(html);
                }
                else
                {
                    return Content("false|" + ex.Message);
                }
            }
            else
            {
                return Content("未传入工作表名！");
            }
        }

        /// <summary>
        /// 单个工作表数据
        /// </summary>
        public ActionResult btnYes_Click(string Id)
        {


            AddDefeatScode.Clear();
            string[] temp = Id.Split('!');
            //textBox3.Clear();
            //if (textBox2.Text == "")
            //{
            //    MessageBox.Show("请输入类别编号！");
            //    return;
            //}
            //添加属性调用
            //取得excel文件路径
            //FileInfo file = new FileInfo(@"D:\ExcelPath" + userInfo.User.Id + ".txt");
            //Stream stream = file.Open(FileMode.Open);
            //StreamReader r = new StreamReader(stream);
            //string filePath = r.ReadToEnd();

            //r.Close();
            //stream.Close();
            string filePath = @"" + rtnPath() + "";
            if (temp[0] == "1")
            {
                //取得
                if (temp[1] != "")
                {

                    //OnBtnEnabled(false);
                    //MSG msg = obj as MSG;
                    ExcelOperationbll excel = new ExcelOperationbll();
                    Exception ex;
                    productPorpertybll bll = new productPorpertybll();
                    productPorpertybll.SetOnAddProValAndOnAddProValDefeat(AddValError, AddProValDefeat);

                    DataSet ds = excel.GetData(filePath, temp[1], out ex);
                    List<PorpertyModel> list = new List<PorpertyModel>();
                    if (ds != null)
                    {
                        //------------把海关表excel信息插入数据库
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)  
                        //{
                        //    string sql = @"insert into Tarifftab values('" + ds.Tables[0].Rows[i][0] + "','" + ds.Tables[0].Rows[i][1] + "','" + ds.Tables[0].Rows[i][2] + "','" + ds.Tables[0].Rows[i][3] + "','" + ds.Tables[0].Rows[i][4] + "','" + ds.Tables[0].Rows[i][5] + "','" + ds.Tables[0].Rows[i][6] + "')";
                        //    DbHelperSQL.ExecuteSql(sql);
                        //}
                        //------------


                        //dataGridView1.DataSource = ds.Tables[0];
                        foreach (DataColumn v in ds.Tables[0].Columns)
                        {
                            if (v.ColumnName != "货品编号" && v.ColumnName != "货号" && v.ColumnName != "SCODE")
                            {
                                PorpertyModel p = new PorpertyModel();
                                p.PropertyName = v.ColumnName;
                                if (temp[2] != "")
                                {
                                    int typeId = bll.SeletTypeId(temp[2].Trim());
                                    if (typeId > 0)
                                    {
                                        p.TypeId = typeId;
                                        list.Add(p);
                                    }
                                }
                            }
                        }
                    }
                    Dictionary<int, string> li;
                    bool bol = bll.Insert(list, out li);
                    if (bol)
                    {
                        return Content("true");
                    }
                    //OnBtnEnabled(true);
                    return Content("false");

                }
                else
                    return Content("true");
            }
            //添加属性值调用
            else if (temp[0] == "2")
            {
                if (temp[1] != "")
                {
                    ExcelOperationbll excel = new ExcelOperationbll();
                    Exception ex;
                    productPorpertybll dal = new productPorpertybll();
                    DataSet ds = excel.GetData(filePath, temp[1], out ex);
                    productPorpertybll bll = new productPorpertybll();
                    productPorpertybll.SetOnAddProValAndOnAddProValDefeat(AddValError, AddProValDefeat);
                    //return Content(ds.Tables[0].Rows[0][0].ToString());
                    if (ds != null)
                    {
                        bool bol = dal.InsertPropertyValue(ds.Tables[0], out ex);
                        if (bol)
                        {
                            string addErr = "";
                            foreach (string s in AddDefeatScode)
                            {
                                addErr += "|" + s + "添加失败！\r\n";
                                //if (OnAddText != null)
                                //{
                                //    OnAddText(s + "添加失败！");
                                //    this.textBox3.SelectionStart = this.textBox3.Text.Length;
                                //    this.textBox3.ScrollToCaret();
                                //}
                            }
                            if (addErr == "")
                                return Content("true");
                            else
                            {
                                return Content("true" + addErr);
                            }
                        }
                        else
                        {

                            return Content("false|" + ex.Message);
                        }
                    }

                }
                else
                    return Content("请选择类别！");
            }
            return Content("No！");
        }
        /// <summary>
        /// 添加税号表
        /// </summary>
        public ActionResult btnYes_Click2(string Id)
        {
            AddDefeatScode.Clear();
            string[] temp = Id.Split('!');
            //textBox3.Clear();
            //if (textBox2.Text == "")
            //{
            //    MessageBox.Show("请输入类别编号！");
            //    return;
            //}
            //添加属性调用
            //取得excel文件路径
            //FileInfo file = new FileInfo(@"D:\ExcelPath" + userInfo.User.Id + ".txt");
            //Stream stream = file.Open(FileMode.Open);
            //StreamReader r = new StreamReader(stream);
            //string filePath = r.ReadToEnd();
            //r.Close();
            //stream.Close();
            string filePath = @"" + rtnPath() + "";
            if (temp[0] == "1")
            {
                //取得
                if (temp[1] != "")
                {

                    //OnBtnEnabled(false);
                    //MSG msg = obj as MSG;
                    ExcelOperationbll excel = new ExcelOperationbll();
                    Exception ex;
                    productPorpertybll bll = new productPorpertybll();
                    productPorpertybll.SetOnAddProValAndOnAddProValDefeat(AddValError, AddProValDefeat);

                    DataSet ds = excel.GetData(filePath, temp[1], out ex);
                    List<PorpertyModel> list = new List<PorpertyModel>();
                    if (ds != null)
                    {
                        //------------把海关表excel信息插入数据库
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string sql = @"insert into Tarifftab (TariffNo,TariffName,Unit,DutiableValue,Tariff,ConditionOne,ConditionTwo) values('" + ds.Tables[0].Rows[i][0] + "','" + ds.Tables[0].Rows[i][1] + "','" + ds.Tables[0].Rows[i][2] + "','" + ds.Tables[0].Rows[i][3] + "','" + ds.Tables[0].Rows[i][4] + "','" + ds.Tables[0].Rows[i][5] + "','" + ds.Tables[0].Rows[i][6] + "')";
                            DbHelperSQL.ExecuteSql(sql);
                        }
                        //------------


                        //dataGridView1.DataSource = ds.Tables[0];

                    }
                    Dictionary<int, string> li;
                    bool bol = bll.Insert(list, out li);
                    if (bol)
                    {
                        return Content("true");
                    }
                    //OnBtnEnabled(true);
                    return Content("false");

                }
                else
                    return Content("true");
            }
            //添加属性值调用
            else if (temp[0] == "2")
            {
                if (temp[1] != "")
                {
                    ExcelOperationbll excel = new ExcelOperationbll();
                    Exception ex;
                    productPorpertybll dal = new productPorpertybll();
                    DataSet ds = excel.GetData(filePath, temp[1], out ex);
                    if (ds != null)
                    {
                        bool bol = dal.InsertPropertyValue(ds.Tables[0], out ex);
                        if (bol)
                        {
                            string addErr = "";
                            foreach (string s in AddDefeatScode)
                            {
                                addErr += "|" + s + "添加失败！\r\n";
                                //if (OnAddText != null)
                                //{
                                //    OnAddText(s + "添加失败！");
                                //    this.textBox3.SelectionStart = this.textBox3.Text.Length;
                                //    this.textBox3.ScrollToCaret();
                                //}
                            }
                            if (addErr == "")
                                return Content("true");
                            else
                            {
                                return Content("true" + addErr);
                            }
                        }
                        else
                        {

                            return Content("false|" + ex.Message);
                        }
                    }

                }
                else
                    return Content("请选择类别！");
            }
            return Content("No！");
        }
        /// <summary>
        /// 添加属性值成功事件调用方法
        /// </summary>
        /// <param name="str"></param>
        public void AddValError(string str)
        {
            //textBox3.Text += str + "\r\n";
            //this.textBox3.SelectionStart = this.textBox3.Text.Length;
            //this.textBox3.ScrollToCaret();
        }
        /// <summary>
        /// 保存添加失败编号
        /// </summary>
        static List<string> AddDefeatScode = new List<string>();
        /// <summary>
        /// 添加属性值失败事件调用方法
        /// </summary>
        /// <param name="str"></param>
        public void AddProValDefeat(string Scode)
        {
            if (!AddDefeatScode.Contains(Scode))
                AddDefeatScode.Add(Scode);
        }

        public string rtntest()
        {
            string s = string.Empty;
            string[] Scode = { "123", "05V32RJ50F", "111" };
            foreach (var temp in Scode)
            {
                string path = @"E:\工作图片\2015年新款阿玛尼\" + temp; //文件夹路径

                if (Directory.Exists(path))
                {
                    string[] paths = Directory.GetFiles(path); //获取文件夹下全部文件路径
                    List<FileInfo> files = new List<FileInfo>();
                    foreach (string filepath in paths)
                    {
                        FileInfo file = new FileInfo(filepath); //获取单个文件
                        files.Add(file);
                        s += file.Name + ",";
                    }
                }
            }
            return s;
        }
        public string rtnPath()
        {
            string Pathsql = @"select * from excelFilePath where UserId='" + userInfo.User.Id + "'";
            string Path = DbHelperSQL.Query(Pathsql).Tables[0].Rows[0]["FilePath"].ToString();
            return Path;
        }
    }
    class MSG
    {
        public string txtText { get; set; }
        public string cmbText { get; set; }
    }
}
