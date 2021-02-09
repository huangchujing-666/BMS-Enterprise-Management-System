using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class TableFiledController : BaseController
    {
        //
        // GET: /TableFiled/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 获取表格信息
        /// </summary>
        /// <returns></returns>
        public ActionResult getTable()
        {
            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            IDataParameter[] ipara = new IDataParameter[]{
                //new SqlParameter("menuName",SqlDbType.NVarChar,20)
            };
            //ipara[0].Value = "张三";
            List<model.tableFiledPerssion> list = tableFiledPerssionBll.getTable(ipara, "tableSelect");

            return View(list);
        }



        /// <summary>
        ///  获取表格字段信息
        /// </summary>
        /// <returns></returns>
        public ActionResult getFiled(int id)
        {
            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("tableLevel",SqlDbType.NVarChar,20)
            };
            ipara[0].Value = id;

            List<model.tableFiledPerssion> list = tableFiledPerssionBll.getTable(ipara, "filedSelect");

            return View(list);
        }


        /// <summary>
        ///  显示字段
        /// </summary>
        /// <returns></returns>
        public string filedShow()
        {
            string filedId = Request.Form["filedId"].ToString();
            filedId = filedId.Length > 0 ? filedId.Remove(filedId.Length - 1, 1) : filedId;
            if (string.IsNullOrWhiteSpace(filedId))
            {
                return "请选择";
            }
            string s = string.Empty;
            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("tableNameState",SqlDbType.NVarChar,200)
            };
            ipara[0].Value = filedId;
            s = tableFiledPerssionBll.Update(ipara, "filedShow");

            return s;
        }


        /// <summary>
        ///  隐藏字段
        /// </summary>
        /// <returns></returns>
        public string filedHide()
        {
            string filedId = Request.Form["filedId"].ToString();
            filedId = filedId.Length > 0 ? filedId.Remove(filedId.Length - 1, 1) : filedId;
            if (string.IsNullOrWhiteSpace(filedId))
            {
                return "请选择";
            }
            string s = string.Empty;
            bll.tableFiledPerssionbll tableFiledPerssionBll = new bll.tableFiledPerssionbll();
            IDataParameter[] ipara = new IDataParameter[]{
                new SqlParameter("tableNameState",SqlDbType.NVarChar,200)
            };
            ipara[0].Value = filedId;

            s = tableFiledPerssionBll.Update(ipara, "filedHide");

            return s;
        }



    }
}
