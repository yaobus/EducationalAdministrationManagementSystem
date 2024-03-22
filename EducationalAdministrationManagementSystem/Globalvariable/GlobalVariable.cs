using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using EducationalAdministrationManagementSystem.ViewMode;
using Newtonsoft.Json.Linq;

namespace GloableVariable
{
    public class GlobalVariable
    {
        //登陆界面的设置面板是否已经显示,默认为假
        public static bool ServerSetPlanShow = false;

        //数据库路径
        public static string DbPath = Directory.GetCurrentDirectory() + @"\config\db_config.json";//指定配置文件路径


        //数据库配置信息
        public static JObject JObject;


        //数据库配置信息显示状态,判断是否正在向配置框加载配置信息
        public static bool DatabaseInfoShowStatus = false;


        //Language配置
        public static int LanguageIndex = 0;


        

        //----------------------Database-Info-------------------------
        //Remote-Server-Core服务器ip地址
        public static IPAddress IpAddress;

        //Remote-Server-Core服务器端口
        public static Int32 ServerPort;

        //与Remote-Server-Core服务器建立的socket
        public static Socket ServerSocket;

        //定义数据接收缓存库
        public static  byte[] ResultBytes = new byte[1024 * 1024 * 64];



        //数据库连接信息
        public static string DbConnectInfo;



        //----------------------Select-Info-------------------------
        //当前选中的院部
        public static string SelectDepartmentId=null;

        //当前选中的系部
        public static string SelectSystemId=null;

        //当前选中的专业
        public static string SelectMajorId=null;

        //当前选中的课程
        public static string SelectCurriculumId= null;

        //当前选中的规划课程ID
        public static string SelectCourseId = null;

        //当前选中的规划课程ID
        public static string SelectTeacherId = null;

        //当前选中的奖惩信息ID
        public static string RewardIndex = null;

        //当前选中的缴费信息ID
        public static string CostPayIndex = null;

        //----------------------学籍信息编辑，修改-------------------------
        //当前是新建还是编辑用户，特征码(学籍ID)
        public static int TextualResearchId = 0;

        //证件照字节流
        public static byte[] ImageBytes;


        //当前选中的学生ID
        public static string TextualStudentId = null;


        //当前选中的学籍信息
        public static ViewMode.StudentTextualViewMode SelectTextual = null;


        public static int PhotoModified = 0;

    }


}
