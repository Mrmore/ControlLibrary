using System;
using System.Collections.Generic;
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
    }
}
