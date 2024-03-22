using System;
using System.Windows;
using GloableVariable;
using MySql.Data.MySqlClient;


namespace EducationalAdministrationManagementSystem
{
    public class DbConnect //数据库连接操作库
    {


        public static MySqlConnection MySqlConnection;

        //连接MYSQL数据库
        public static MySqlConnection ConnectionMysql(string DbConnectInfo)
        {
            MySqlConnection = new MySqlConnection(DbConnectInfo);

            return MySqlConnection;
        }



        /// <summary>
        /// 在指定的mysql连接上执行sql语句,返回MySqlDataReader 
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public static MySqlDataReader CarrySqlCmd(string sql)
        {
            using (MySqlCommand cmdCommand = new MySqlCommand(sql, MySqlConnection))
            {
                var reader = cmdCommand.ExecuteReader();
                return reader;
            }

        }


        /// <summary>
        /// 执行mysql修改,插入数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回受影响的行数,为int值</returns>
        public static int ModifySql(string sql)
        {
            try
            {
                MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);
                MySqlConnection.Open(); //连接数据库
                MySqlCommand SqlCommand = new MySqlCommand(sql, MySqlConnection);
                return SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }

           
        }


        /// <summary>
        /// 查询数据记录数量，避免数据重复
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回数字0或1</returns>
        public static int CountDataNumber(string sql)
        {

            MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);

            MySqlConnection.Open(); //连接数据库

            MySqlDataReader reader = CarrySqlCmd(sql);


            if (reader.Read())
            {
                reader.Close();
             return 1;
            }
            else
            {
                reader.Close();
                return 0;  
            }


     
            










        }






        #region 数据库操作

        //CREATE TABLE student_info
        //(
        //    ID varchar(8),
        //STUDENT_NAME varchar(16),
        //SEX varchar(2),
        //NUMBER varchar(20),
        //SYSTEM_NAME varchar(64),
        //CERTIFICATE varchar(8),
        //CERTIFICATE_NUMBER varchar(20),
        //ADDRESS varchar(128),
        //PHONE varchar(11),
        //DATE date,
        //    SCHOOL_NAME  varchar(64),
        //COMPANY varchar(64),
        //MAJOR_LEVEL varchar(4),
        //MAJOR_TYPE varchar(16),
        //MAJOR_NAME varchar(64),
        //MAJOR_NUM varchar(8),
        //EMAIL varchar(32),
        //STUDENT_TYPE varchar(8),
        //STUDENT_LEVEL varchar(8),
        //STUDENT_EDU varchar(8),
        //POLITICS varchar(16),
        //NATION varchar(16),
        //OCCUPATION varchar(16),
        //WORK_COMPANY varchar(64),
        //HEALTH varchar(8),
        //CENSUS varchar(8),
        //POST_NUM varchar(6),
        //ENG_NAME varchar(16),
        //BIRTHDAY date,
        //    COLLECTIVE_CODE  varchar(16),
        //GRADUATION varchar(8),
        //SIGN varchar(8),
        //SIGN_TYPE varchar(8)

        //    )

        #endregion
    }


}
