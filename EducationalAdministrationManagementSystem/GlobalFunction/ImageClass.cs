using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace EducationalAdministrationManagementSystem.GlobalFunction
{
    class ImageClass
    {



        /// <summary>
        /// bitmapImage转Bytes
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapImageToBytes(BitmapImage bitmap)
        {
            byte[] bytes = null;

            try
            {
                Stream bitmapStream = bitmap.StreamSource;
                if (bitmapStream != null && bitmapStream.Length > 0)
                {
                    bitmapStream.Position = 0;

                    using (BinaryReader binary = new BinaryReader(bitmapStream))
                    {
                        bytes = binary.ReadBytes((int)bitmapStream.Length);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return bytes;

        }




        /// <summary>
        /// byte[]转BitmapImage
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static BitmapImage BytesToBitmapImage(byte[] bytes)
        {
            BitmapImage bitmap = null;

            try
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(bytes);
                bitmap.EndInit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
            return bitmap;
            }



        /// <summary>
        /// 文件到byte数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] FileToBytes(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            byte[] bytes = new byte[fileInfo.Length];
            FileStream fileStream = fileInfo.OpenRead();
            fileStream.Read(bytes, 0, Convert.ToInt32(fileStream.Length));
            fileStream.Close();
            return bytes;
            }





        /// <summary>
        /// 比较图片的字节尺寸，比设定值大就返回1，比设定值小就返回0，图片为空或比较失败则返回-1
        /// </summary>
        /// <param name="bitmap">待比较的Bitmap</param>
        /// <param name="space">设定的比较字节尺寸,默认是30kb</param>
        /// <returns></returns>
        public static int CompareImageSpace(BitmapImage bitmap, int space = 30720)
        {



            try
            {

                Stream bitmapStream = bitmap.StreamSource;
                bitmapStream.Position = 0;
                long size = bitmapStream.Length;


                if (bitmapStream != null)
                {
                    return -1;
                }
                else
                {
                    if (size >= space)
                    {
                        return 1;

                    }
                    else
                    {
                        return 0;
                    }

                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }




        }


    }
}
