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
using EducationalAdministrationManagementSystem.EncryptionDecryptionFunction;
using GloableVariable;
using MySql.Data.MySqlClient;

namespace EducationalAdministrationManagementSystem.UserPlan
{
    /// <summary>
    /// RewardPlan.xaml 的交互逻辑
    /// </summary>
    public partial class RewardPlan : UserControl
    {
        public RewardPlan()
        {
            InitializeComponent();
            LoadDefaultImage();
            Image.Source = DefaultBitmapImage;
            LoadDepartmentData();
            StudentListView.ItemsSource = StudentList;
            RewardListView.ItemsSource = RewardList;
            List<string> reawardType = new List<string>();
            reawardType.Add("奖励");
            reawardType.Add("惩罚");
            RewardComboBox.ItemsSource = reawardType;
            RewardComboBox.SelectedIndex = -1;

        }




        /// <summary>
        /// 加载学院数据
        /// </summary>
        public void LoadDepartmentData()
        {
            List<string> departList = new List<string>();


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
                string sql = "SELECT * FROM DEPARTMENT_INFO WHERE DELSTATUS != 1";

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);



                DepartmentList.Clear();
                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string id = reader.GetString("DEPARTMENT_INDEX").ToString();
                    string name = Base64Conver.Base64Str2Text(reader.GetString("DEPARTMENT_NAME").ToString());
                    string note = Base64Conver.Base64Str2Text(reader.GetString("DEPARTMENT_NOTE").ToString());
                    string delStatus = reader.GetString("DELSTATUS").ToString();
                    DepartmentList.Add(new ViewMode.ViewMode.DepartmentInfo(index, id, name, note, delStatus));
                    departList.Add(name);
                }

                DepartmentCombobox.ItemsSource = departList;
                DepartmentCombobox.SelectedIndex = 0;
                DbConnect.MySqlConnection.Close();





            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }

        }

        public ObservableCollection<ViewMode.ViewMode.DepartmentInfo> DepartmentList =
            new ObservableCollection<ViewMode.ViewMode.DepartmentInfo>()
            {
                //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
            };


        /// <summary>
        /// 加载系部数据
        /// </summary>
        private void LoadSystemData(string departmentId)
        {

            List<string> systemNameList = new List<string>();

            SystemList.Clear();
            if (GlobalVariable.DbConnectInfo != null)
            {
                try
                {
                    DbConnect.MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);

                    DbConnect.MySqlConnection.Open(); //连接数据库

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                //第二步,查询系部信息
                string sql =
                    string.Format("SELECT * FROM SYSTEM_INFO WHERE DELSTATUS != 1 AND BELONG_DEPARTMENT = '{0}'",
                        departmentId);

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);



                //   SystemList.Clear();
                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string id = reader.GetString("SYSTEM_INDEX").ToString();
                    string name = Base64Conver.Base64Str2Text(reader.GetString("SYSTEM_NAME").ToString());
                    string note = Base64Conver.Base64Str2Text(reader.GetString("SYSTEM_NOTE").ToString());
                    string belong = reader.GetString("BELONG_DEPARTMENT");
                    string delStatus = reader.GetString("DELSTATUS").ToString();
                    systemNameList.Add(name);
                    SystemList.Add(new ViewMode.ViewMode.SystemInfoViewMode(index, id, name, note, belong, delStatus));

                }

                SystemCombobox.ItemsSource = systemNameList;
                //SystemCombobox.SelectedIndex = 0;
                DbConnect.MySqlConnection.Close();



            }
            else
            {
                Console.WriteLine("CurriculumPlan.LoadSystemData检查数据库连接信息！");
            }




        }

        public ObservableCollection<ViewMode.ViewMode.SystemInfoViewMode> SystemList =
            new ObservableCollection<ViewMode.ViewMode.SystemInfoViewMode>()
            {
                //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
            };


        /// <summary>
        /// 加载专业信息
        /// </summary>
        /// <param name="systemId"></param>
        private void LoadMajor(string systemId)
        {
            List<string> majorNameList = new List<string>();

            //清空专业列表
            MajorInfoList.Clear();

            if (GlobalVariable.DbConnectInfo != null)
            {
                try
                {
                    DbConnect.MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);

                    DbConnect.MySqlConnection.Open(); //连接数据库



                    //第二步,查询系部信息
                    string sql =
                        string.Format("SELECT * FROM MAJOR_INFO WHERE DELSTATUS != 1 AND BELONG_SYSTEM = '{0}'",
                            systemId);

                    MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);



                    //   SystemList.Clear();
                    int index = 0;
                    while (reader.Read())
                    {
                        index += 1;
                        string id = reader.GetString("MAJOR_INDEX").ToString();
                        string name = Base64Conver.Base64Str2Text(reader.GetString("MAJOR_NAME").ToString());
                        string num = reader.GetString("MAJOR_NUMBER").ToString();
                        string note = Base64Conver.Base64Str2Text(reader.GetString("MAJOR_NOTE").ToString());
                        string belong = reader.GetString("BELONG_SYSTEM");
                        string delStatus = reader.GetString("DELSTATUS").ToString();

                        majorNameList.Add(name);
                        MajorInfoList.Add(new ViewMode.ViewMode.MajorInfoViewMode(index, id, name, num, note, belong,
                            delStatus));
                        Console.WriteLine(name);






                    }



                    MajorCombobox.ItemsSource = majorNameList;
                    MajorCombobox.SelectedIndex = -1;

                    DbConnect.MySqlConnection.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            else
            {
                Console.WriteLine("CurriculumPlan.Loadmajor检查数据库连接信息！");
            }







        }



        public ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode> MajorInfoList =
            new ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode>()
            {
                //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
            };






        /// <summary>
        /// 选择学院
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DepartmentCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SystemCombobox.SelectedIndex = -1;
            MajorCombobox.SelectedIndex = -1;
            SystemList.Clear();
            MajorInfoList.Clear();
            StudentList.Clear();

            if (DepartmentList != null)
            {
                //记录当前选择的院部ID
                string departmentInfoId = DepartmentList[DepartmentCombobox.SelectedIndex].Id;


                try
                {
                    //加载系部数据到系部列表
                    LoadSystemData(departmentInfoId);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                }


            }
        }


        /// <summary>
        /// 选择系部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MajorCombobox.SelectedIndex = -1;
            MajorInfoList.Clear();
            StudentList.Clear();

            if (SystemList != null)
            {

                try
                {
                    //记录当前选择的院部ID
                    string systemInfoId = SystemList[SystemCombobox.SelectedIndex].Id;

                    //加载系专业数据
                    LoadMajor(systemInfoId);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                }

            }

        }

        /// <summary>
        /// 选择专业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MajorCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StudentList.Clear();


            try
            {
                string majorId = MajorInfoList[MajorCombobox.SelectedIndex].Num;
                LoadMajorStudent(majorId);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }
        }

        /// <summary>
        /// 加载该专业学生
        /// </summary>
        /// <param name="majorId">专业ID</param>
        private void LoadMajorStudent(string majorId)
        {
            StudentList.Clear();

            try
            {
                DbConnect.MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);

                DbConnect.MySqlConnection.Open(); //连接数据库

            }
            catch (Exception)
            {
                MessageBox.Show("数据库连接失败!请检查数据库配置");
            }

            if (majorId != null)
            {
                string sql = string.Format("SELECT * FROM student_info WHERE MAJORNUM  = '{0}'", majorId);

                Console.WriteLine("RewardPlan.LoadMajorStudent" + sql);

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);


                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string studentName = reader.GetString("STUDENT_NAME");
                    string studentId = reader.GetString("NUMBER"); //学号
                    string studentIndex = reader.GetString("ID"); //学生索引号



                    StudentList.Add(
                        new ViewMode.ViewMode.StudentInfoViewMode(index, studentId, studentName, studentIndex));



                }



                StudentListView.SelectedIndex = -1;


                DbConnect.MySqlConnection.Close();


            }




        }


        public ObservableCollection<ViewMode.ViewMode.StudentInfoViewMode> StudentList =
            new ObservableCollection<ViewMode.ViewMode.StudentInfoViewMode>()
                { };

        /// <summary>
        /// 选中学生后加载课程和分数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GlobalVariable.RewardIndex = null;
            string majorId;
            try
            {
                majorId = MajorInfoList[MajorCombobox.SelectedIndex].Num;

                var student = StudentListView.SelectedItem as ViewMode.ViewMode.StudentInfoViewMode;

                if (student != null)
                {

                    StudentName.Text = student.Name;
                    StudentId.Text = student.Id;

                    //加载该学生的头像
                    LoadImg(StudentList[StudentListView.SelectedIndex].StudentIndex);

                    //加载该学生的奖惩信息
                    LoadRewardData(StudentList[StudentListView.SelectedIndex].Id);
                }



            }
            catch (Exception)
            {
                majorId = null;
                Console.WriteLine(e.ToString());

            }



        }


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
        /// 加载学生头像
        /// </summary>
        private void LoadImg(string studentId)
        {
            GlobalVariable.ImageBytes = null;
            Image.Source = DefaultBitmapImage;

            Console.WriteLine(studentId);

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

        public BitmapImage DefaultBitmapImage = new BitmapImage();




        /// <summary>
        /// 加载奖惩信息
        /// </summary>
        private void LoadRewardData(string studentId)
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
                string sql = string.Format("SELECT * FROM reward_info WHERE STUDENT_ID = '{0}' AND DELSTATUS !='1'", studentId);

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);

                RewardList.Clear();
                int index = 0;
                while (reader.Read())
                {
                    index += 1;

                    string rewardIndex = reader.GetString("REWARD_INDEX");
                    string id = reader.GetString("STUDENT_ID");
                    string studentName = reader.GetString("STUDENT_NAME");
                    string date = reader.GetString("DATE");
                    string rewardType = reader.GetString("TYPE");
                    string note = reader.GetString("REWARD_NOTE");
                    string delStatus = reader.GetString("DELSTATUS");

                    RewardList.Add(new ViewMode.ViewMode.RewardViewMode(index, rewardIndex, id, studentName, date, rewardType, note, delStatus));
                }

                DbConnect.MySqlConnection.Close();

                //清空院部选择项
                GlobalVariable.SelectCurriculumId = null;

                //清空院部编辑框


            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }


           


        }

        public ObservableCollection<ViewMode.ViewMode.RewardViewMode> RewardList = new ObservableCollection<ViewMode.ViewMode.RewardViewMode>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };




        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {

            string name = StudentName.Text.Trim();
            string studentId = StudentId.Text.Trim();
            string rewardIndex= GlobalVariable.RewardIndex;
            string sql;

            if (name != "" && studentId != "" && RewardComboBox.SelectedIndex != -1 && DatePicker.Text.ToString() != null)
            {
                //更新记录
                if (rewardIndex != null)
                {
                    sql = string.Format("UPDATE reward_info SET TYPE='{0}',DATE='{1}',REWARD_NOTE='{2}' WHERE REWARD_INDEX='{3}'",
                        RewardComboBox.Text, DatePicker.Text, NoteTextBox.Text, rewardIndex);

                    DbConnect.ModifySql(sql);
                    LoadRewardData(studentId);
                    //清空输入框
                    ClearData();
                }
                else//新建记录
                {
                    rewardIndex =
                        EncryptionDecryptionFunction.MD5EncryptionDecryption.MyTextMD5(studentId + DateTime.Now, 8);
                    sql = string.Format("INSERT INTO reward_info VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','0')",rewardIndex,studentId,name,DatePicker.Text,RewardComboBox.Text,NoteTextBox.Text);

                    DbConnect.ModifySql(sql);
                    LoadRewardData(studentId);
                    ClearData();
                }

            }
            else
            {
                MessageBox.Show("信息不完整");
            }


        }


        private void ClearData()
        {

            RewardComboBox.SelectedIndex = -1;
            DatePicker.Text = null;
            NoteTextBox.Text = "";
            GlobalVariable.RewardIndex = null;



        }

        private void RewardListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var rewardInfo = RewardListView.SelectedItem as ViewMode.ViewMode.RewardViewMode;

            if (rewardInfo!=null)
            {
                RewardComboBox.Text = rewardInfo.RewardType;
                DatePicker.Text = rewardInfo.Date;
                NoteTextBox.Text = rewardInfo.Note;
                GlobalVariable.RewardIndex = rewardInfo.RewardIndex;
            }
            

        }

        private void DelButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalVariable.RewardIndex != null)
            {
                string sql = string.Format("UPDATE reward_info SET DELSTATUS='1' WHERE REWARD_INDEX='{0}'",
                    GlobalVariable.RewardIndex);
                DbConnect.ModifySql(sql);
                LoadRewardData(StudentId.Text);
                ClearData();



            }



        }
    }
}
