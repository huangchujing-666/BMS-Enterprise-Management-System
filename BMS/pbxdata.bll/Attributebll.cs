using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.bll
{
    public class Attributebll
    {
        dal.AttributeDal dal = new dal.AttributeDal();
        /// <summary>
        /// 获取商品属性
        /// </summary>
        /// <returns></returns>
        public List<model.AttributeModel> GetAttributeList()
        {

            return dal.GetAttributeList(); ;
        }
        ///// <summary>
        ///// 获取所有大类别列表
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetBigTypeList(int page,int pagecount)
        //{

        //    return dal.GetBigTypeList(page, pagecount); 
        //}
        /// <summary>
        /// 搜索获取大类别列表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchGetBigTypeList(string Name, int page, int pages, out string counts)
        {

            return dal.GetBigTypeList(Name, page, pages, out counts);
        }
        /// <summary>
        /// 删除商品类别
        /// </summary>
        /// <returns></returns>
        public bool DeleteType(string TypeNo, int UserId)
        {

            return dal.ProTypeDelete(TypeNo, UserId);
        }
        /// <summary>
        /// 添加商品类别
        /// </summary>
        /// <returns></returns>
        public bool InsertType(model.AttributeModel mm)
        {

            return dal.ProTypeInsert(mm);
        }
        /// <summary>
        /// 修改商品类别
        /// </summary>
        /// <returns></returns>
        public bool UpdateType(model.AttributeModel mm)
        {

            return dal.UpdateType(mm);
        }
        /// <summary>
        /// 删除大类别
        /// </summary>
        /// <returns></returns>
        public bool DeleteBigType(string a, int UserId)
        {

            return dal.ProBigTypeDelete(a, UserId);
        }
        /// <summary>
        /// 添加大类别
        /// </summary>
        /// <returns></returns>
        public bool InsertBigType(model.AttributeModel mm)
        {

            return dal.ProBigTypeInsert(mm);
        }
        /// <summary>
        /// 修改大类别
        /// </summary>
        /// <returns></returns>
        public string UpdateBigType(model.AttributeModel mm)
        {

            return dal.UpdateBigType(mm);
        }
        /// <summary>
        /// 返回所有字段名称集合
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string[] getDataName(string sql)
        {
            string[] ss = dal.getDataName(sql);
            return ss;
        }
        /// <summary>
        /// 商品属性界面搜索
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="page"></param>
        /// <param name="pages"></param>
        /// <param name="counts"></param>
        /// <returns></returns>
        public DataTable SearchAttrType(Dictionary<string, string> dic, int page, int pages, out string counts)
        {
            return dal.GetSearchAttrTypeList(dic, page, pages, out counts);
        }
        /// <summary>
        /// 更新商品类别
        /// </summary>
        /// <returns></returns>
        public DataTable GetAttributeList(string TypeNo)
        {
            return dal.GetAttributeList(TypeNo);
        }
        /// <summary>
        /// 编辑商品属性
        /// </summary>
        /// <returns></returns>
        public int UpdateAttribute(string Id, string PropertyName, int UserId)
        {
            return dal.UpdateAttribute(Id, PropertyName, UserId);
        }
        /// <summary>
        /// 添加商品属性
        /// </summary>
        /// <returns></returns>
        public int InsertAttribute(string TypeNo, string PropertyName, string UserId)
        {
            return dal.InsertAttribute(TypeNo, PropertyName, UserId);
        }
        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <returns></returns>
        public int DeleteAttribute(string Id, int UserId)
        {
            return dal.DeleteAttribute(Id, UserId);
        }
        /// <summary>
        /// 操作类别对应税号表
        /// </summary>
        /// <returns></returns>
        public bool UpdateTypeIdToTariffNo(string TypeNo, string TariffNo, int UserId)
        {
            return dal.UpdateTypeIdToTariffNo(TypeNo, TariffNo, UserId);
        }
        /// <summary>
        /// 操作类别对应淘宝类别
        /// </summary>
        /// <returns></returns>
        public bool UploadTBType(string TypeNo, string cid, int UserId)
        {
            return dal.UploadTBType(TypeNo, cid, UserId);
        }
        /// <summary>
        /// 根据权限获取类别下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeDDlist(string CustomerId)
        {
            return dal.GetTypeDDlist(CustomerId);
        }
        /// <summary>
        /// 得到所有类别下拉
        ///
        /// </summary>
        /// <returns></returns>
        public DataTable GetTypeDDlist() 
        {
            return dal.GetTypeDDlist();
        }
        /// <summary>
        /// 根据权限获取品牌下拉框
        /// </summary>
        /// <returns></returns>
        public DataTable GetCatDDlist(string CustomerId)
        {
            return dal.GetCatDDlist(CustomerId);
        }
        /// <summary>
        /// 得到所有品牌下拉
        /// </summary>
        /// <returns></returns>
        public DataTable GetCatDDlist() 
        {
            return dal.GetCatDDlist();
        }
        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchBrand(string Name, string page, string Selpages, out string counts)
        {
            return dal.SearchBrand(Name, page, Selpages, out counts);
        }
        /// <summary>
        /// 获取淘宝品牌列表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchTBBrand(string Name, string page, string Selpages, out string counts)
        {
            return dal.SearchTBBrand(Name, page, Selpages, out counts);
        }
        /// <summary>
        /// 获取淘宝类别列表
        /// </summary>
        /// <returns></returns>
        public DataTable SearchTBType(string Name, string page, string Selpages, out string counts)
        {
            return dal.SearchTBType(Name, page, Selpages, out counts);
        }
        /// <summary>
        /// 获取供应商类别表
        /// </summary>
        /// <returns></returns>
        public DataTable VenProducttypeSearch(Dictionary<string, string> dic, int page, int Selpages, out string counts)
        {
            return dal.VenProducttypeSearch(dic, page, Selpages, out counts);
        }

        /// <summary>
        /// 修改供应商类别表
        /// </summary>
        /// <returns></returns>
        public string UpdateProducttypeVen(string Id, string TypeNameVen, string Vencode, string TypeNo, int UserId)
        {
            return dal.UpdateProducttypeVen(Id, TypeNameVen, Vencode, TypeNo, UserId);
        }
        /// <summary>
        /// 删除供应商类别
        /// </summary>
        public string deleteProducttypeVen(string Id, int UserId)
        {
            return dal.deleteProducttypeVen(Id, UserId);
        }
        /// <summary>
        /// 类别搜索
        /// </summary>
        public DataTable SearchProducttype(string TypeName)
        {
            return dal.SearchProducttype(TypeName);
        }
        /// <summary>
        /// 更新淘宝类别
        /// </summary>
        public string UpdateTBType(string TypeName, string cid, string Id, int UserId)
        {
            return dal.UpdateTBType(TypeName, cid, Id, UserId);
        }
        /// <summary>
        /// 添加淘宝类别
        /// </summary>
        public string AddTBType(string TypeName, string cid, int UserId)
        {
            return dal.AddTBType(TypeName, cid, UserId);
        }
        /// <summary>
        /// 更新淘宝属性
        /// </summary>
        public string UpdateTBProperty(string PropertyName, string vid, string Id, int UserId)
        {
            return dal.UpdateTBProperty(PropertyName, vid, Id, UserId);
        }
        /// <summary>
        /// 添加淘宝属性
        /// </summary>
        public string AddTBProperty(string TBPropertyName, string vid, string parent_cid, int UserId)
        {
            return dal.AddTBProperty(TBPropertyName, vid, parent_cid, UserId);
        }
        /// <summary>
        /// 删除淘宝属性
        /// </summary>
        public string DeleteTBProperty(string Id, int UserId)
        {
            return dal.DeleteTBProperty(Id, UserId);
        }
        //**********7.6
        /// <summary>
        /// 添加与系统类别对应的类别
        /// </summary>
        /// <param name="typeno"></param>
        /// <param name="typename"></param>
        /// <param name="typenameven"></param>
        /// <returns></returns>
        public string InsertProducttypeVenExcel(string typeno, string typename, string typenameven,string vencode) 
        {
            return dal.InsertProducttypeVenExcel(typeno, typename, typenameven, vencode);
        }
        /// <summary>
        /// ---得到所有与excel表格关联的类别
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public DataTable GetProductTypeExcel(string[] str, string minid, string maxid)
        {
            return dal.GetProductTypeExcel(str, minid, maxid);
        }
        /// <summary>
        /// ---得到所有与excel表格关联的类别个数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minid"></param>
        /// <param name="maxid"></param>
        /// <returns></returns>
        public int GetProductTypeExcelCount(string[] str)
        {
            return dal.GetProductTypeExcelCount(str);
        }
        /// <summary>
        /// 删除类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteTypeExcel(string id)
        {
            return dal.DeleteTypeExcel(id);
        }
    }
}
