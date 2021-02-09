using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public partial class SystemConfigManagerbll : dataoperatingbll
    {
        pbxdata.idal.ISystemConfig dal = pbxdata.dalfactory.ReflectFactory.CreateIDataOperatingByReflect("SystemConfigManagerdal") as pbxdata.idal.ISystemConfig;
        pbxdata.idal.IErrorlog error = pbxdata.dalfactory.ReflectFactory.CreateIDataOperatingByReflect("Errorlogdal") as pbxdata.idal.IErrorlog;
        Exception ex;
        string mess;
        /// <summary>
        /// 查找供应商信息
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<pbxdata.model.productsource> GetProductsource(int? userId)
        {
            //dal = new pbxdata.dal.SystemConfigManagerdal();
            List<pbxdata.model.productsource> list= dal.GetProductsource( out ex);
            if (list == null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->GetProductsource()--->pbxdata.dal.SystemConfigManagerdll->GetProductsource()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = userId;
                error.InsertErrorlog(err);
            }
            return list;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Code">删除的供应商编号</param>
        /// <param name="userId">操作人</param>
        /// <returns></returns>
        public bool DeleteProductsources(string Code, int? userId)
        {
            Exception ex;
            bool bol = dal.DeleteProductsources(Code, out ex);
            if (!bol)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->DeleteProductsources()--->pbxdata.dal.SystemConfigManagerdll->DeleteProductsources()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                 err.UserId = userId;
                error.InsertErrorlog(err);
            }
            else
            {
                model.errorlog err = new model.errorlog();
                err.operation = 2;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->DeleteProductsources()--->pbxdata.dal.SystemConfigManagerdll->DeleteProductsources()";
                err.ErrorMsg = "删除";
                err.errorMsgDetails = "删除一条数据源信息";
                err.UserId = userId;
                error.InsertErrorlog(err);
            }
            return bol;
        }
         /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <param name="pro">供应商实体类</param>
        /// <param name="IsExistCode">该供应商是否存在</param>
        /// <returns></returns>
        public bool InsertProductsources(model.productsource pro, out bool IsExistCode)
        {
            Exception ex;
            bool bol = dal.InsertProductsources(pro, out ex);
            if (!bol)
            {
                if (ex.Message.Equals("该供应商编号已经存在"))
                {
                    IsExistCode = true;
                    return false;
                }
                else
                {
                    model.errorlog err = new model.errorlog();
                    err.operation = 1;
                    err.errorTime = DateTime.Now;
                    err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->DeleteProductsources()--->pbxdata.dal.SystemConfigManagerdll->DeleteProductsources()";
                    err.ErrorMsg = ex.Source;
                    err.errorMsgDetails = ex.Message;
                    err.UserId = pro.UserId;
                    error.InsertErrorlog(err);
                    IsExistCode = false;
                }
            }
            else
            {
                model.errorlog err = new model.errorlog();
                err.operation = 2;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->DeleteProductsources()--->pbxdata.dal.SystemConfigManagerdll->DeleteProductsources()";
                err.ErrorMsg = " 添加";
                err.errorMsgDetails = "添加一条供应商信息";
                err.UserId = pro.UserId;
                error.InsertErrorlog(err);
                IsExistCode = false;
            }
            IsExistCode = false;
            return bol;
        }
        /// <summary>
        /// 查找供应商
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public model.productsource GetProductsourece(string Id)
        {
            Exception ex;
            model.productsource pro = dal.GetProductsource(Id, out ex);
            if (pro == null && ex != null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->DeleteProductsources()--->pbxdata.dal.SystemConfigManagerdll->DeleteProductsources()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = pro.UserId;
                error.InsertErrorlog(err);
                return null;
            }
            return pro;
        }
         /// <summary>
        /// 获取所有数据源信息集合
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetProductsourceConfig(int? UserId)
        {
            Exception ex;
            DataTable dt= dal.GetProductsourceConfig(out ex);
            if (dt == null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->GetProductsourceConfig()--->pbxdata.dal.SystemConfigManagerdll->GetProductsourceConfig()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            return dt;
        }
        /// <summary>
        /// 删除数据源信息
        /// </summary>
        /// <param name="Id">数据源编号</param>
        /// <param name="UserId">当前用户</param>
        /// <returns></returns>
        public bool DeleteProductsourceConfig(int Id, int? UserId)
        {
            Exception ex;
            bool bol = dal.DeleteProductsourceConfig(Id, out ex);
            if (ex != null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->DeleteProductsourceConfig()--->pbxdata.dal.SystemConfigManagerdll->DeleteProductsourceConfig()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            else
            {
                model.errorlog err = new model.errorlog();
                err.operation = 2;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->DeleteProductsourceConfig()--->pbxdata.dal.SystemConfigManagerdll->DeleteProductsourceConfig()";
                err.ErrorMsg = "删除";
                err.errorMsgDetails = "删除一条数据源信息";
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            return bol;
        }
        /// <summary>
        ///修改数据源信息
        /// </summary>
        /// <param name="pro">数据源实体</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool UpdateProducesourceConfig(model.productsourceConfig pro,int? UserId)
        {
            Exception ex;
            bool bol = dal.UpdateProductsourceConfig(pro, out ex);
            if (ex != null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->UpdateProducesourceConfig()--->pbxdata.dal.SystemConfigManagerdll->UpdateProducesourceConfig()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            else
            {
                model.errorlog err = new model.errorlog();
                err.operation = 2;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->UpdateProducesourceConfig()--->pbxdata.dal.SystemConfigManagerdll->UpdateProducesourceConfig()";
                err.ErrorMsg = "修改";
                err.errorMsgDetails = "修改一条数据源信息";
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            return bol;
        }
       /// <summary>
        /// 查询指定数据源信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public model.productsourceConfig GetProductsourceConfig(int Id)
        {
            Exception ex;
            model.productsourceConfig pro = dal.GetProductsourceConfig(Id,out ex);
            return pro;
        }
        /// <summary>
        /// 添加数据源信息
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool InsertProductsourceConfig(model.productsourceConfig pro,int? UserId,out bool isExist)
        {
            Exception ex;
            bool bol = dal.InsertProductsourceConfig(pro, out ex);
            if (ex != null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->InsertProductsourceConfig()--->pbxdata.dal.SystemConfigManagerdll->InsertProductsourceConfig()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = UserId;
                error.InsertErrorlog(err);
                isExist = false;
            }
            else if (bol)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 2;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->InsertProductsourceConfig()--->pbxdata.dal.SystemConfigManagerdll->InsertProductsourceConfig()";
                err.ErrorMsg = "添加";
                err.errorMsgDetails = "添加一条数据源信息";
                err.UserId = UserId;
                error.InsertErrorlog(err);
                isExist = false;
            }
            else
            {
                isExist = true;
            }
            return bol;
        }
          /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="exx"></param>
        /// <returns></returns>
        public bool UpdateProductsource(model.productsource pro,int? UserId)
        {
            Exception ex;
            bool bol = dal.UpdateProductsource(pro, out ex);
            if (ex != null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->UpdateProductsource()--->pbxdata.dal.SystemConfigManagerdll->UpdateProductsource()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            else
            {
                model.errorlog err = new model.errorlog();
                err.operation = 2;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->UpdateProductsource()--->pbxdata.dal.SystemConfigManagerdll->InsertProductsourceConfig()";
                err.ErrorMsg = "修改";
                err.errorMsgDetails = "修改一条供应商信息";
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            return bol;
        }
        /// <summary>
        ///获取所有供应商名称编号
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetProductsource1(int? UserId)
        {
            Exception ex;
            Dictionary<string,string> dic= dal.GetProductsource1(out ex);
            if (ex != null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->GetProductsource()--->pbxdata.dal.SystemConfigManagerdll->GetProductsource()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = UserId;
                error.InsertErrorlog(err);
            }
            return dic;
        }
        /// <summary>
        /// 查询所有数据源更新日志信息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public DataTable GetProGetProductsourceUpdateLog(int skip,int take,string log,string beginTime,string endTime, out int count, int? userId)
        {
            Exception ex;
            DataTable dt = dal.GetProGetProductsourceUpdateLog(skip, take, log, beginTime, endTime, out count, out ex);
            if (ex != null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->GetProGetProductsourceUpdateLog()--->pbxdata.dal.SystemConfigManagerdll->GetProGetProductsourceUpdateLog()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = userId;
                error.InsertErrorlog(err);
            }
            return dt;
        }
        /// <summary>
        /// 查询所有数据源更新日志信息
        /// </summary>
        /// <param name="SourceCode">数据源编号</param>
        /// <param name="log">日志类型</param>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public DataTable GetProGetProductsourceUpdateLog(int skip, int take, string SourceCode,string log,string beginTime,string endTime, out int count, int? userId)
        {
            Exception ex;
            DataTable dt = dal.GetProGetProductsourceUpdateLog(skip, take,SourceCode,log ,beginTime,endTime, out count, out ex);
            if (dt == null)
            {
                model.errorlog err = new model.errorlog();
                err.operation = 1;
                err.errorTime = DateTime.Now;
                err.errorSrc = "pbxdata.bll.SystemConfigManagerbll->GetProGetProductsourceUpdateLog()--->pbxdata.dal.SystemConfigManagerdll->GetProGetProductsourceUpdateLog()";
                err.ErrorMsg = ex.Source;
                err.errorMsgDetails = ex.Message;
                err.UserId = userId;
                error.InsertErrorlog(err);
            }
            return dt;
        }
    }
}
