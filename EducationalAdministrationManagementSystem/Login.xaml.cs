using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EducationalAdministrationManagementSystem.GlobalFunction;
using EducationalAdministrationManagementSystem.EncryptionDecryptionFunction;
using GloableVariable;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace EducationalAdministrationManagementSystem
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            LoadUserType();
        }


        /// <summary>
        /// 登陆的用户类别
        /// </summary>
        private void LoadUserType()
        {

            //加载专业层次
            List<string> userTypeList = new List<string>()
            {
                {"学生"},
                {"教师"},
                {"管理员"},
            };
            UserTypeCombobox.ItemsSource = userTypeList;

            UserTypeCombobox.SelectedIndex = 2;



        }



        /// <summary>
        /// 程序启动后加载数据库配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_OnLoaded(object sender, RoutedEventArgs e)
        {

            GlobalFunction.FileClass.AnalysisDatabaseConfig();//解析数据库配置
                                                             

        }




        /// <summary>
        /// 允许程序界面任意拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }


        /// <summary>
        /// 打开设置页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalVariable.ServerSetPlanShow == false)
            {
                FunctionPlan.Children.Clear();
                FunctionPlan.Children.Add(new ServerSettingPlan());
                GlobalVariable.ServerSetPlanShow = true;
            }
            else
            {
                FunctionPlan.Children.Clear();
                GlobalVariable.ServerSetPlanShow = false;
            }
        }


        /// <summary>
        /// 结束程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }




        /// <summary>
        /// 登陆验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProgressBar.IsIndeterminate = true;
            
            if (GlobalVariable.ServerSetPlanShow == true)
            {
                GlobalVariable.ServerSetPlanShow = false;
                FunctionPlan.Children.Clear();
            }


            GlobalFunction.FileClass.AnalysisDatabaseConfig();//解析数据库配置
            

            //第一步,连接数据库

            if (GlobalVariable.DbConnectInfo != null)
            {
                try
                {
                    DbConnect.MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);
                    DbConnect.MySqlConnection.Open(); //连接数据库

                }
                catch (Exception)
                {
                    MessageBox.Show("数据库连接失败!请检查数据库配置");
                }

                //第二步,查询比对管理员登录信息
                string sql = string.Format("SELECT * FROM ADMIN_INFO WHERE NICK_NAME='{0}'", UserNameBox.Text);

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);
                if (reader.Read())
                {

                    string password = reader.GetString("PASSWORD");

                    if (password == MD5EncryptionDecryption.PasswordMD5(UserPasswordBox.Password))//密码正确
                    {
                        //释放连接资源
                        reader.Dispose();

                        Window mainWindow = new MainWindow();

                        var window = Window.GetWindow(this);//关闭父窗体
                        window?.Close();

                        //打开新窗口
                        mainWindow.Show();
                        




                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误!");
                    }






                }
                else
                {
                    MessageBox.Show("用户名或密码错误!");
                }

            }


        }

        private void LanguageSelect_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (LanguageSelect.SelectedIndex ==1)
            {
                GlobalFunction.PublicClass.SetLanguage("en-US");
            }
            else
            {
                GlobalFunction.PublicClass.SetLanguage("zh-CN");
            }



        }
    }
}
