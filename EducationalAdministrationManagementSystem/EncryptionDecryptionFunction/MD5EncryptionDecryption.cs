using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EducationalAdministrationManagementSystem.EncryptionDecryptionFunction
{
    public class MD5EncryptionDecryption
    {

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string MyTextMD5(string str, int num)
        {
            MD5CryptoServiceProvider md5hash = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5hash.ComputeHash(Encoding.GetEncoding("GB2312").GetBytes(str));
            StringBuilder tmpBuilder = new StringBuilder();
            foreach (byte i in hashBytes)
            {
                tmpBuilder.Append(i.ToString("x2"));

            }

            if (num >= 8 && num <= 32)
            {
                return tmpBuilder.ToString().Substring((32 - num) / 2, num);
            }
            else
            {
                return tmpBuilder.ToString().Substring(12, 8);//默认返回8位
            }
        }





        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        /// <param name="str">密码字符串</param>
        /// <param name="num">16</param>
        /// <returns></returns>
        public static string PasswordMD5(string str, int num = 16)
        {
            MD5CryptoServiceProvider md5hash = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5hash.ComputeHash(Encoding.GetEncoding("GB2312").GetBytes(str + "Okra-Remote"));
            StringBuilder tmpBuilder = new StringBuilder();
            foreach (byte i in hashBytes)
            {
                tmpBuilder.Append(i.ToString("x2"));

            }

            if (num >= 8 && num <= 32)
            {
                return tmpBuilder.ToString().Substring((32 - num) / 2, num).ToUpper();
            }
            else
            {
                return tmpBuilder.ToString().Substring(12, 16).ToUpper();//默认返回16位
            }
        }

    }
}
