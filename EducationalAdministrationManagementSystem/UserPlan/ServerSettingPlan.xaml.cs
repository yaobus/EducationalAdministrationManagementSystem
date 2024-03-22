using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EducationalAdministrationManagementSystem.GlobalFunction;
using GloableVariable;
using Newtonsoft.Json.Linq;

namespace EducationalAdministrationManagementSystem
{
    /// <summary>
    /// ServerSettingPlan.xaml 的交互逻辑
    /// </summary>
    public partial class ServerSettingPlan : UserControl
    {
        public ServerSettingPlan()
        {
            InitializeComponent();
            LoadConfig();
        }


        /// <summary>
        /// 加载配置信息到编辑框
        /// </summary>
        private void LoadConfig()
        {
            FileClass.ReadConfig();

            //数据库信息显示开始，避免出现仅显示第一条信息的BUG
            GloableVariable.GlobalVariable.DatabaseInfoShowStatus = true;

            if (GlobalVariable.JObject != null)
            {


                try
                {
                    ServerIpBox.Text = GlobalVariable.JObject["ServerAddress"].ToString();
                    ServerPortBox.Text = GlobalVariable.JObject["ServerPort"].ToString();
                    DbUserBox.Text = GlobalVariable.JObject["DbUser"].ToString();
                    DbPasswordBox.Password = GlobalVariable.JObject["DbPassword"].ToString();
                    DbNameBox.Text = GlobalVariable.JObject["DbName"].ToString();

                }
                catch (Exception)
                {

                }



            }
            else
            {
                MessageBox.Show("NUll database info");
            }

            //数据库信息显示完毕
            GlobalVariable.DatabaseInfoShowStatus =false;

        }



        /// <summary>
        /// 隐藏面板按钮被单击,隐藏面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HideSettingPlan_OnClick(object sender, RoutedEventArgs e)
        {
            //SaveConfig();
            ServerSettingPlan2.Visibility = Visibility.Hidden;
            ServerSettingPlan2.Background = new SolidColorBrush(Colors.AliceBlue);
            GlobalVariable.ServerSetPlanShow = false;

        }


        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            //程序不处于信息加载状态时才存储信息，避免正常显示数据的时候后续数据被清空
            if (GlobalVariable.DatabaseInfoShowStatus==false)
            {

                JObject jObject = new JObject();

                jObject.Add("ServerAddress", ServerIpBox.Text);
                jObject.Add("ServerPort", ServerPortBox.Text);
                jObject.Add("DbUser", DbUserBox.Text);
                jObject.Add("DbPassword", DbPasswordBox.Password);
                jObject.Add("DbName", DbNameBox.Text);


                //获取程序启动目录
                string path = System.IO.Directory.GetCurrentDirectory() + @"\config\db_config.json";
                string outstr =
                    Newtonsoft.Json.JsonConvert.SerializeObject(jObject, Newtonsoft.Json.Formatting.Indented);


                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\config"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\config");
                }
                else
                {
                    if (!File.Exists(path))
                    {
                        StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8);

                        streamWriter.WriteLine(outstr);

                        streamWriter.Close();

                        // MessageBox.Show("保存成功!");

                    }
                    else
                    {
                        StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8);

                        streamWriter.WriteLine(outstr);

                        streamWriter.Close();

                        // MessageBox.Show("保存成功!");
                    }
                }




                GlobalVariable.JObject = jObject;



            }







        }


    /// <summary>
    /// Ip发生改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ServerIpBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SaveConfig();
    }

    /// <summary>
    /// 端口改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ServerPortBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SaveConfig();
    }


    /// <summary>
    /// 用户名发生改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DbUserBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SaveConfig();
    }


    /// <summary>
    /// 密码发生改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DbPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        SaveConfig();
    }


    /// <summary>
    /// 数据库名称发生改变
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DbNameBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SaveConfig();
    }
}
}
