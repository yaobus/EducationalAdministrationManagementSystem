using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalAdministrationManagementSystem.EncryptionDecryptionFunction
{
    class Base64Conver
    {


        /// <summary>
        /// 文本转BASE64文本
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Text2Base64Str(string str)
        {
            byte[] a = Encoding.Default.GetBytes(str);

            return Convert.ToBase64String(a);
        }




        /// <summary>
        /// BASE64文本转普通文本
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Str2Text(string str)
        {
            byte[] a = Convert.FromBase64String(str);
            return Encoding.Default.GetString(a);
        }
    }
}
