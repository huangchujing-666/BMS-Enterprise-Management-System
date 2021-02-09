/*************************************************************************************
    * CLR版本：        4.0.30319.18063
    * 类 名 称：       mytest_Ldal
    * 机器名称：       WIN-9KCN5GHKKM8
    * 命名空间：       pbxdata.dal
    * 文 件 名：       mytest_Ldal
    * 创建时间：       2015-08-11 10:20:35
    * 作    者：       lcg
    * 说    明：
    * 修改时间：
    * 修 改 人：
*************************************************************************************/


using pbxdata.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pbxdata.dal
{
    public class mytest_Ldal : imytest_L
    {
        public string insert_L(object o)
        {
            string s = string.Empty;
            model.mytest_L MDmytest = new model.mytest_L();
            model.pbxdatasourceDataContext context = (model.pbxdatasourceDataContext)o;
            ///model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();



            var p = (from c in context.apiOrder where c.orderId == "212" select c).FirstOrDefault().def3;

            try
            {
                MDmytest.uname = "张三";
                MDmytest.upwd = "123456";
                MDmytest.usex = "男";

                context.mytest_L.InsertOnSubmit(MDmytest);
                context.SubmitChanges();
                s = "添加成功";
            }
            catch (Exception ex)
            {
                s = "添加失败(" + ex.Message + ")";
            }
            return s;
        }

        public string delete_L(object o)
        {

            model.pbxdatasourceDataContext context1 = (model.pbxdatasourceDataContext)o;
            string s = string.Empty;
            model.mytest_L MDmytest = new model.mytest_L();
            //model.pbxdatasourceDataContext context1 = new model.pbxdatasourceDataContext();

            try
            {
                MDmytest = (from c in context1.mytest_L where c.id == 22 select c).SingleOrDefault();
                context1.mytest_L.DeleteOnSubmit(MDmytest);
                context1.SubmitChanges();
                
                s = "删除成功";
            }catch(Exception ex)
            {
                s = "删除失败(" + ex.Message + ")";
            }

            return s;
        }


        public string get_L(string tableName)
        {
            string s = string.Empty;


            return s;
        }
    }
}
