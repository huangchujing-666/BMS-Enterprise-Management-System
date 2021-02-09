using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace pbxdata.web.Controllers
{
    public class mytest_LController : Controller
    {
        //
        // GET: /mytest_L/

        public ActionResult Index()
        {
            return View();
        }



        public string test1()
        {
            string s = string.Empty;
            string s1 = null;
            bll.mytest_Lbll mytest_LBll = new bll.mytest_Lbll();

            using (TransactionScope scope = new TransactionScope())
            {
                model.pbxdatasourceDataContext context = new model.pbxdatasourceDataContext();
                s += mytest_LBll.insert_L(context);
                s += mytest_LBll.delete_L(context);

                //s = "11" ?? string.Empty;
                //s = s1 ?? "22";

                scope.Complete();

                
            }

            return s;
        }

            // Initialize the return value to zero and create a StringWriter to display results.
        //    int returnValue = 0;
        //    System.IO.StringWriter writer = new System.IO.StringWriter();

        //    try
        //    {
        //        // Create the TransactionScope to execute the commands, guaranteeing
        //        // that both commands can commit or roll back as a single unit of work.
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            using (SqlConnection connection1 = new SqlConnection("data source=192.168.0.124;initial catalog=pbxDB;user id=sa;password=bms123456;"))
        //            {
        //                // Opening the connection automatically enlists it in the 
        //                // TransactionScope as a lightweight transaction.
        //                connection1.Open();

        //                // Create the SqlCommand object and execute the first command.
        //                SqlCommand command1 = new SqlCommand("insert into mytest_L(uname,upwd,usex) values('李四','123456123456123456123456123456123456123456123456123456123456','女')", connection1);
        //                returnValue = command1.ExecuteNonQuery();
        //                writer.WriteLine("Rows to be affected by command1: {0}", returnValue);
        //                connection1.Close();
        //                // If you get here, this means that command1 succeeded. By nesting
        //                // the using block for connection2 inside that of connection1, you
        //                // conserve server and network resources as connection2 is opened
        //                // only when there is a chance that the transaction can commit.   
        //                using (SqlConnection connection2 = new SqlConnection("data source=192.168.0.124;initial catalog=pbxDB;user id=sa;password=bms123456;"))
        //                {
        //                    // The transaction is escalated to a full distributed
        //                    // transaction when connection2 is opened.
        //                    connection2.Open();

        //                    // Execute the second command in the second database.
        //                    returnValue = 0;
        //                    SqlCommand command2 = new SqlCommand("delete from mytest_L where id = 24", connection2);
        //                    returnValue = command2.ExecuteNonQuery();
        //                    writer.WriteLine("Rows to be affected by command2: {0}", returnValue);
        //                }
        //            }

        //            // The Complete method commits the transaction. If an exception has been thrown,
        //            // Complete is not  called and the transaction is rolled back.
        //            scope.Complete();

        //        }

        //    }
        //    catch (TransactionAbortedException ex)
        //    {
        //        writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        writer.WriteLine("ApplicationException Message: {0}", ex.Message);
        //    }

        //    // Display messages.
        //    Console.WriteLine(writer.ToString());

        //    return returnValue.ToString();
        //}

    }
}
