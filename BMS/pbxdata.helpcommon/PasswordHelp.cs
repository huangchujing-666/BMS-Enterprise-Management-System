using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace pbxdata.helpcommon
{
    public class PasswordHelp
    {

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inputString">需要加密的字符串</param>
        /// <returns>加密字符串</returns>
        public static string encrypt(string inputString)
        {

            MD5 md5 = MD5.Create();
            byte[] inputByte = md5.ComputeHash(Encoding.Default.GetBytes(inputString));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < inputByte.Length; i++)
            {
                sBuilder.Append(inputByte[i].ToString("x2"));

            }




            return sBuilder.ToString(); ;
        }


        /// <summary>
        /// 验证MD5
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <param name="hash">验证值</param>
        /// <returns></returns>
        public static bool verifyMd5(string input, string hash)
        {
            string hashOfInput = encrypt(input);
            StringComparer compare = StringComparer.OrdinalIgnoreCase;
            if (0 == compare.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// MD5 16位加密 加密后密码为小写
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string To16Md5(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");

            t2 = t2.ToLower();

            return t2;
        }



        /// <summary>
        /// MD5　32位加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string To32Md5(string str)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
