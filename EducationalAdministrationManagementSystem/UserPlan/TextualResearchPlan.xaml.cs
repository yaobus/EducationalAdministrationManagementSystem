using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EducationalAdministrationManagementSystem.EncryptionDecryptionFunction;
using GloableVariable;
using MySql.Data.MySqlClient;

namespace EducationalAdministrationManagementSystem.UserPlan
{
    /// <summary>
    /// TextualResearchPlan.xaml 的交互逻辑
    /// </summary>
    public partial class TextualResearchPlan : UserControl
    {
        public TextualResearchPlan()
        {
            
            InitializeComponent();
            
            LoadTextualData();
            
            LoadDefaultImage();
            StudentListView.ItemsSource = TextualList;
        }


        public BitmapImage DefaultBitmapImage = new BitmapImage();

        private void LoadDefaultImage()
        {
            string path = string.Format(@"/Resources/Contact.png");
            Uri pathUri = new Uri(path,UriKind.RelativeOrAbsolute);
            BitmapImage DefaultBitmap = new BitmapImage();
            DefaultBitmap.BeginInit();
            DefaultBitmap.UriSource = pathUri;
            DefaultBitmap.EndInit();
            DefaultBitmapImage = DefaultBitmap;
        }



        /// <summary>
        /// 加载学籍信息到列表框
        /// </summary>
        public void LoadTextualData()
        {
            GlobalFunction.FileClass.AnalysisDatabaseConfig();//解析数据库配置

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

                //第二步,查询部门信息
                string sql = "SELECT * FROM STUDENT_INFO WHERE DELSTATUS != '1'";

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);

                Console.WriteLine(sql);

                TextualList.Clear();
                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string id = reader.GetString("ID");
                    string name = reader.GetString("STUDENT_NAME");
                    string sex = reader.GetString("SEX");
                    string number = reader.GetString("NUMBER");
                    string system = reader.GetString("SYSTEM_NAME");
                    string certificate = reader.GetString("CERTIFICATE");
                    string certificateNumber = reader.GetString("CERTIFICATENUMBER");
                    string address = reader.GetString("ADDRESS");
                    string phone = reader.GetString("PHONE");
                    string date = reader.GetString("DATE");
                    date = Convert.ToDateTime(date).ToLongDateString().ToString();

                    string schoolName = reader.GetString("SCHOOLNAME");
                    string company = reader.GetString("COMPANY");
                    string majorLevel = reader.GetString("MAJORLEVEL");
                    string majorType = reader.GetString("MAJORTYPE");
                    string majorName = reader.GetString("MAJORNAME");
                    string majorNum = reader.GetString("MAJORNUM");
                    string eMail = reader.GetString("EMAIL");
                    string studentType = reader.GetString("STUDENTTYPE");
                    string studentLevel = reader.GetString("STUDENTLEVEL");
                    string studentEdu = reader.GetString("STUDENTEDU");
                    string politics = reader.GetString("POLITICS");
                    string nation = reader.GetString("NATION");
                    string occupation = reader.GetString("OCCUPATION");
                    string workCompany = reader.GetString("WORKCOMPANY");
                    string health = reader.GetString("HEALTH");
                    string census = reader.GetString("CENSUS");
                    string postNum = reader.GetString("POSTNUM");
                    string engName = reader.GetString("ENGNAME");

                    string birthDay = reader.GetString("BIRTHDAY");
                    birthDay = Convert.ToDateTime(birthDay).ToLongDateString().ToString();


                    string code = reader.GetString("COLLECTIVE_CODE");
                    string graduation = reader.GetString("GRADUATION");
                    string sign = reader.GetString("SIGN");
                    string signType = reader.GetString("SIGNTYPE");
                    string teacher = reader.GetString("TEACHER");
                    string delstatus = reader.GetString("DELSTATUS");



                    TextualList.Add(new ViewMode.ViewMode.StudentTextualViewMode(index, id, name, sex, number, system, certificate, certificateNumber, address, phone, date, schoolName, company, majorLevel, majorType, majorName, majorNum, eMail, studentType, studentLevel, studentEdu, politics, nation, occupation, workCompany, health, census, postNum, engName, birthDay, code, graduation, sign, signType, teacher, delstatus));



                }

                DbConnect.MySqlConnection.Close();



            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }










        }




        /// <summary>
        /// 创建动态列表
        /// </summary>
        public ObservableCollection<ViewMode.ViewMode.StudentTextualViewMode> TextualList = new ObservableCollection<ViewMode.ViewMode.StudentTextualViewMode>()
        {

        };





        /// <summary>
        /// 弹出新建窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            GlobalVariable.TextualResearchId = 0;
            Window studentEditWindow = new StudentEdit();
            studentEditWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            studentEditWindow.ShowDialog();
        }


        /// <summary>
        /// 选择学生子项后加载相关信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GlobalVariable.ImageBytes = null;
            Image.Source = DefaultBitmapImage;




            //存储当前选择的学籍数据到缓存
            GlobalVariable.SelectTextual = StudentListView.SelectedItem as ViewMode.ViewMode.StudentTextualViewMode;
            if (GlobalVariable.SelectTextual != null)
            {
                GlobalVariable.TextualStudentId = GlobalVariable.SelectTextual.Id;


                //加载照片
                LoadImg(GlobalVariable.SelectTextual.Id);
            }
        }

        /// <summary>
        /// 加载学生头像
        /// </summary>
        private void LoadImg(string studentId)
        {


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

                //第二步,查询系部信息
                string sql = string.Format("SELECT * FROM portrait_img WHERE  STUDENT_ID = '{0}'", studentId);

                // MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);

                try
                {
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql, DbConnect.MySqlConnection);
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    DbConnect.MySqlConnection.Close();
                    byte[] imgBytes = (Byte[])dataSet.Tables[0].Rows[0]["IMG"];
                    Image.Source = GlobalFunction.ImageClass.BytesToBitmapImage(imgBytes);
                    GlobalVariable.ImageBytes = imgBytes;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }



            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }







        }





        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            GlobalVariable.TextualResearchId = 1;
            
            if (StudentListView.SelectedIndex != -1)
            {

                try
                {
                    Window editWindow = new StudentEdit();
                    editWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    editWindow.ShowDialog();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    //throw;
                }
            }
            else
            {
                MessageBox.Show("请先选择一条数据再进行编辑操作");
            }


        }

        private void DelButton_OnClick(object sender, RoutedEventArgs e)
        {

            string sql = string.Format("UPDATE STUDENT_INFO SET DELSTATUS='1' WHERE ID='{0}'", GlobalVariable.TextualStudentId);

            DbConnect.ModifySql(sql);

            LoadTextualData();
        }


        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoadTextualData();
        }
    }
}

