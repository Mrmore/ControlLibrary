using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace ControlLibrary.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// method to strip HTML tags from a string
        /// </summary>
        /// <param name="str">the string to strip</param>
        /// <param name="decodeFirst">if set to <c>true</c> [decode first].</param>
        /// <returns></returns>
        public static string StripHtml(this string str, bool decodeFirst = true)
        {
            try
            {
                while (((str.IndexOf("<") > -1) && (str.IndexOf(">") > -1) && (str.IndexOf("<") < str.IndexOf(">"))))
                {
                    var start = str.IndexOf("<");
                    var end = str.IndexOf(">");
                    var count = end - start + 1;
                    str = str.Remove(start, count);
                }
                str = str.Replace(" ", " ");
                str = str.Replace(">", "");
                str = str.Replace("\r\n", "");
                return str.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// string to MD5String
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ComputeMD5(this string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        /// <summary>
        /// string to TimeString
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertStringToTimeString(this string str)
        {
            try
            {
                DateTime time = Convert.ToDateTime(str, CultureInfo.InvariantCulture); // Converts only the time
                string strTime = time.ToString("HH:mm:ss");
                return strTime;
            }
            catch (Exception)
            {
                //第3个参数IFormatProvider provider 可以为null
                return DateTime.ParseExact(str, "HH:mm:ss", CultureInfo.InvariantCulture).ToString();
            }
        }

        /// <summary>
        /// int to TimeString
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertIntToTimeString(this string str)
        {
            string temp = string.Empty;
            DateTime time = new DateTime().Add(TimeSpan.FromSeconds(System.Convert.ToInt32(str)));
            if (System.Convert.ToInt32(str) >= 3600)
            {
                temp = time.ToString("HH:mm:ss");
            }
            else
            {
                temp = time.ToString("mm:ss");
            }
            return temp;
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Reverse(this string s)
        {
            return new string(s.ReverseChar());
        }

        public static char[] ReverseChar(this string s)
        {
            return s.ToCharArray().Reverse().ToArray();
        }

        public static string Slice(this string source, int start)
        {
            return new string(source.ToCharArray().Slice(start));
        }
    }
}
