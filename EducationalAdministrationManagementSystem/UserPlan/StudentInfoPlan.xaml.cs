using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using GloableVariable;
using MySql.Data.MySqlClient;

namespace EducationalAdministrationManagementSystem.UserPlan
{
    /// <summary>
    /// StudentInfoPlan.xaml 的交互逻辑
    /// </summary>
    public partial class StudentInfoPlan : UserControl
    {
        public StudentInfoPlan()
        {
            InitializeComponent();
            LoadDataToSexBox();
            StudentListView.ItemsSource = TextualList;
            LoadTextualData();
            LoadDefaultImage();
        }

        /// <summary>
        /// 加载性别数据
        /// </summary>
        public void LoadDataToSexBox()
        {

            List<string> sexList = new List<string>()
            {
                {"男"},
                {"女"},

            };
            GenderCombobox.ItemsSource = sexList;

        }

        public BitmapImage DefaultBitmapImage = new BitmapImage();

        private void LoadDefaultImage()
        {
            string path = string.Format(@"/Resources/Contact.png");
            Uri pathUri = new Uri(path, UriKind.RelativeOrAbsolute);
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
            GlobalFunction.FileClass.AnalysisDatabaseConfig(); //解析数据库配置

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



                    TextualList.Add(new ViewMode.ViewMode.StudentTextualViewMode(index, id, name, sex, number, system,
                        certificate, certificateNumber, address, phone, date, schoolName, company, majorLevel,
                        majorType, majorName, majorNum, eMail, studentType, studentLevel, studentEdu, politics, nation,
                        occupation, workCompany, health, census, postNum, engName, birthDay, code, graduation, sign,
                        signType, teacher, delstatus));

                    Console.WriteLine(TextualList);

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
        public ObservableCollection<ViewMode.ViewMode.StudentTextualViewMode> TextualList =
            new ObservableCollection<ViewMode.ViewMode.StudentTextualViewMode>()
            {

            };


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
                StudentNameTextBox.Text = GlobalVariable.SelectTextual.Name;
                GenderCombobox.Text = GlobalVariable.SelectTextual.Sex;
                NumberTextBox.Text = GlobalVariable.SelectTextual.Number;
                SystemTextBox.Text = GlobalVariable.SelectTextual.System;
                CertificateTextBox.Text = GlobalVariable.SelectTextual.CertificateNumber;
                AddressTextBox.Text = GlobalVariable.SelectTextual.Address;
                PhoneTextBox.Text = GlobalVariable.SelectTextual.Phone;
                DatePicker.Text = GlobalVariable.SelectTextual.Date;
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
                    byte[] imgBytes = (Byte[]) dataSet.Tables[0].Rows[0]["IMG"];
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





        private void StretchButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearData();

            GlobalFunction.FileClass.AnalysisDatabaseConfig(); //解析数据库配置

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
            }

            string keyWord = StretchTextBox.Text.Trim();

            string sql =String.Format( "SELECT * FROM STUDENT_INFO WHERE STUDENT_NAME LIKE '%{0}%'",keyWord);

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



                TextualList.Add(new ViewMode.ViewMode.StudentTextualViewMode(index, id, name, sex, number, system,
                    certificate, certificateNumber, address, phone, date, schoolName, company, majorLevel,
                    majorType, majorName, majorNum, eMail, studentType, studentLevel, studentEdu, politics, nation,
                    occupation, workCompany, health, census, postNum, engName, birthDay, code, graduation, sign,
                    signType, teacher, delstatus));

                Console.WriteLine(TextualList);

            }

            DbConnect.MySqlConnection.Close();
            

        }

        private void StretchTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StretchButton_OnClick(null,null);
            }
        }


        /// <summary>
        /// 清除编辑框的内容
        /// </summary>
        private void ClearData()
        {
            StudentNameTextBox.Text = "";
            GenderCombobox.SelectedIndex = -1;
            NumberTextBox.Text = "";
            SystemTextBox.Text = "";
            CertificateTextBox.Text = "";
            AddressTextBox.Text = "";
            PhoneTextBox.Text = "";
            DatePicker.Text = "";



        }
    }
}
