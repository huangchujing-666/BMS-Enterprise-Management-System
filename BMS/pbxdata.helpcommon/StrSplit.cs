using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pbxdata.helpcommon
{
    public class StrSplit
    {
        /// <summary>
        /// 字符串分割成为string[]数组
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <param name="c">分割符号</param>
        /// <returns></returns>
        public static string[] StrSplitData(string str,char c)
        { 
            char value =c;
            List<string> list = new List<string>();

            if (!str.Contains(c))
            {
                string[] strs = new string[] { str };
                if (string.IsNullOrWhiteSpace(strs[0]))
                {
                    strs = new string[0];
                }
                return strs;
            }

            list = str.Split(value).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
            string[] s = list.ToArray();
            return s;
        }

    }
}
