using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GloableVariable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EducationalAdministrationManagementSystem.GlobalFunction
{
    class FileClass
    {
        /// <summary>
        /// 读取数据库连接配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <returns></returns>
        public static JObject GetDbConfig(string path)
        {

            if (!File.Exists(path))
            {
                JObject jObject = null;
                return jObject;
            }
            else
            {

                try
                {
                    StreamReader streamReader = File.OpenText(path);
                    JsonTextReader jsonTextReader = new JsonTextReader(streamReader);
                    JObject jObject = (JObject)JToken.ReadFrom(jsonTextReader);
                    streamReader.Close();
                    return jObject;
                }
                catch (Exception e)
                {

                    File.Delete(path);
                    MessageBox.Show("配置文件损坏，请重新配置!"+"\r\n"+e.ToString());

                    return null;
                }


            }

        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        public static void  ReadConfig()
        {

            GlobalVariable.JObject = GetDbConfig(GlobalVariable.DbPath);//从指定配置文件读取配置信息,返回JObject对象
           
        }


        /// <summary>
        /// 解析数据库配置文件
        /// </summary>
        public static void AnalysisDatabaseConfig()
        {
            ReadConfig();
            #region 解析配置文件

            //读取数据库配置,并连接数据库
            JObject jObject = GlobalVariable.JObject; //难得改下面的变量



            if (jObject != null)
            {
                try
                {
                    GlobalVariable.DbConnectInfo = "server=" + jObject["ServerAddress"].ToString() + ";port=" +
                                                   jObject["ServerPort"].ToString() + ";user=" + jObject["DbUser"].ToString() +
                                                   ";password=" + jObject["DbPassword"].ToString() + ";database=" +
                                                   jObject["DbName"].ToString() + ";";


                }
                catch (Exception)
                {
                    //输出数据库配置信息载入错误信息!
                  
                    GlobalVariable.DbConnectInfo = null;
                }
            }
            else
            {
                //输出数据库配置信息载入错误信息!
               
                GlobalVariable.DbConnectInfo = null;
            }

            #endregion


        }
    }
}
