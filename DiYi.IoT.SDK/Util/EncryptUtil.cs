using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DiYi.IoT.SDK.Util
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public class EncryptUtil
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64Encode(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base64Decode(string value)
        {
            var bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 16位MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(value)), 4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
    }
}
