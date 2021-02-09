using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pbxdata.helpcommon
{
    /// <summary>
    /// 参数类
    /// </summary>
    public class ParmPerportys
    {

        /// <summary>
        /// 获取字符串参数进行过滤
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static string GetStrParms(object parms)
        {
            string s = string.Empty;
            if (parms != null)
                s = parms.ToString().Trim();
            else
                s = string.Empty;
            return s;
        }

        /// <summary>
        /// 获取数字参数进行过滤(默认值为-1)
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static int GetNumParms(object parms)
        {
            int num = -1;
            if (parms != null)
                int.TryParse(parms.ToString().Trim(), out num);
            return num;
        }


        /// <summary>
        /// 获取数字参数进行过滤(默认值为0)
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static int GetNumParms(object parms,bool flag)
        {
            int num = 0;
            if (parms != null)
            {
                if (parms!="")
                {
                    int.TryParse(parms.ToString().Trim(), out num);
                }
            }

            return num;
        }


        /// <summary>
        /// 获取long参数进行过滤
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static long GetLongParms(object parms)
        {
            long num = -1;
            if (parms != null)
                long.TryParse(parms.ToString().Trim(), out num);
            return num;
        }

        /// <summary>
        /// 获取decimal参数进行过滤
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static decimal GetDecimalParms(object parms)
        {
            decimal num = 0;
            if (parms != null)
            {
                if (!string.IsNullOrWhiteSpace(parms.ToString()))
                    num = Convert.ToDecimal(parms.ToString());
            }
            return num;
        }

        /// <summary>
        /// 获取Guid参数进行过滤
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static Guid GetGuidParms(object parms)
        {
            Guid guid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(parms.ToString()))
                Guid.TryParse(parms.ToString(),out guid);
            return guid;
        }

        /// <summary>
        /// 获取DateTime参数进行过滤
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static DateTime GetDateTimeParms(object parms)
        {
            DateTime time = new DateTime(0);
            if (parms != null)
                DateTime.TryParse(parms.ToString().Trim(), out time);
            return time;
        }

        /// <summary>
        /// 获取DateTime参数进行过滤(如果为空，返回当前时间)
        /// </summary>
        /// <param name="parms">参数</param>
        /// <returns></returns>
        public static DateTime GetDateTimeNowParms(string parms)
        {
            DateTime time = new DateTime(0);
            if (parms != null)
            {
                if (!string.IsNullOrWhiteSpace(parms))
                {
                    DateTime.TryParse(parms.ToString().Trim(), out time);
                }
                else
                {
                    time = DateTime.Now;
                }
                
            }   
            return time;
        }

        /// <summary>
        /// 获取byte[]参数进行过滤
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static byte[] GetByteParms(object parms)
        {
            byte[] bt = new byte[65536];;
            if (parms==null || parms.ToString()=="")
            {
                bt = null;
            }
            else { bt = (byte[])parms as byte[]; }
            return bt;
        }

        /// <summary>
        /// 转换小写形式(取出空格)
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static string stringToLower(object parms)
        {
            string s = string.Empty;
            if (parms != null)
                s = parms.ToString().Trim().ToLower();
            else
                s = string.Empty;
            return s;
        }

    }
}
