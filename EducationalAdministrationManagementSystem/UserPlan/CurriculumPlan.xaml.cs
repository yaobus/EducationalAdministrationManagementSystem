using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// CurriculumPlan.xaml 的交互逻辑
    /// </summary>
    public partial class CurriculumPlan : UserControl
    {
        public CurriculumPlan()
        {
            InitializeComponent();
            LoadCurriculumData(); //加载课程池
            LoadDepartmentData(); //加载院部数据
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
        /// 加载课程池
        /// </summary>
        private void LoadCurriculumData()
        {
            GlobalFunction.FileClass.AnalysisDatabaseConfig(); //解析数据库配置
            DepartmentList.Clear();
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

                //第二步,查询部门信息
                string sql = "SELECT * FROM course_pool WHERE DELSTATUS != 1";

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);



                CurriculumList.Clear();
                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string id = reader.GetString("COURSE_ID");
                    string name = reader.GetString("COURSE_NAME");
                    string teachBook = reader.GetString("TEACH_BOOK");
                    string bookAuthor = reader.GetString("BOOK_AUTHOR");
                    string bookPress = reader.GetString("BOOK_PRESS");
                    string note = reader.GetString("NOTE");
                    string delStatus = reader.GetString("DELSTATUS");
                    CurriculumList.Add(new ViewMode.ViewMode.CurriculumViewMode(index, id, name, teachBook, bookAuthor,
                        bookPress, note, delStatus));
                }

                DbConnect.MySqlConnection.Close();

                //清空院部选择项
                GlobalVariable.SelectCurriculumId = null;

                //清空院部编辑框


            }
            else
            {
                Console.WriteLine("CurriculumPlan.LoadCurriculumData检查数据库连接信息！");
            }


            CurriculumListView.ItemsSource = CurriculumList;

        }



        public ObservableCollection<ViewMode.ViewMode.CurriculumViewMode> CurriculumList =
            new ObservableCollection<ViewMode.ViewMode.CurriculumViewMode>()
            {

            };




        private void DepartmentCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            SystemCombobox.SelectedIndex = -1;

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
                    string sql = string.Format("SELECT * FROM MAJOR_INFO WHERE DELSTATUS != 1 AND BELONG_SYSTEM = '{0}'", systemId);

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
                        MajorInfoList.Add(new ViewMode.ViewMode.MajorInfoViewMode(index, id, name, num, note, belong, delStatus));
                        Console.WriteLine(name);






                    }



                    MajorCombobox.ItemsSource = majorNameList;
                    //SystemCombobox.SelectedIndex = 0;
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



        public ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode> MajorInfoList = new ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };

        private void SystemCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MajorCombobox.SelectedIndex = -1;

            if (SystemList != null)
            {

                try
                {
                    //记录当前选择的院部ID
                    string systemInfoId = SystemList[SystemCombobox.SelectedIndex].Id;

                    //加载系部数据到系部列表
                    LoadMajor(systemInfoId);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    
                }

            }

           
        }


        /// <summary>
        /// 选择一个课程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurriculumListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode.ViewMode.CurriculumViewMode CurriculumInfo = CurriculumListView.SelectedItem as ViewMode.ViewMode.CurriculumViewMode;




            if (CurriculumInfo != null)
            {
                //记录当前选择的课程ID
                GlobalVariable.SelectCurriculumId = CurriculumInfo.CourseId;
                GloableVariable.GlobalVariable.SelectCourseId = null;
                //加载院部数据到编辑框
                CurriculumNameTextBox.Text = CurriculumInfo.CourseName;
                CurriculumNumTextBox.Text = CurriculumInfo.CourseId;



            }
        }


        /// <summary>
        /// 增加一个课程到当前专业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (MajorInfoList.Count > 0 && CurriculumNumTextBox.Text.Trim() != "")
            {

                string courseId = CurriculumNumTextBox.Text;
                string majorId = MajorInfoList[MajorCombobox.SelectedIndex].Num;
                string courseIndex =
                    EncryptionDecryptionFunction.MD5EncryptionDecryption.MyTextMD5(majorId + courseId, 8);

                string courseName = CurriculumNameTextBox.Text;
                int score = 0;

                if (ScoreTextBox.Text.Trim() != "")
                {
                    score = Convert.ToInt32(ScoreTextBox.Text);
                }
                else
                {
                    MessageBox.Show("请设置学分信息");
                    return;
                }



                string sql;


                sql = string.Format("SELECT * FROM major_course WHERE COURSE_INDEX = '{0}'", courseIndex);

                if (DbConnect.CountDataNumber(sql) < 1)
                {

                    sql = string.Format("INSERT INTO major_course  VALUES('{0}', '{1}', '{2}', '{3}','{4}')", courseIndex, majorId, courseId, courseName, score);

                    Console.WriteLine(sql);

                    DbConnect.ModifySql(sql);

                }
                else
                {
                    Console.WriteLine(sql);
                    MessageBox.Show("该专业的此课程已存在！");
                }



                LoadSelectCourse();
            }
            else
            {
                MessageBox.Show("专业或课程信息缺失");
            }


        }




        /// <summary>
        ///加载已选课程
        /// </summary>
        private void LoadSelectCourse()
        {



            //清空专业列表
            SelectCourseList.Clear();

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

                //第二步,查询专业课程信息
                string getMajorId;
                try
                {
                    getMajorId = MajorInfoList[MajorCombobox.SelectedIndex].Num;
                }
                catch (Exception e)
                {
                    getMajorId = null;
                    Console.WriteLine(e);

                }

                if (getMajorId != null)
                {
                    string sql = string.Format("SELECT * FROM major_course WHERE MAJOR_ID  = '{0}'", getMajorId);

                    MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);



                    //   SystemList.Clear();
                    int index = 0;
                    while (reader.Read())
                    {
                        index += 1;
                        string courseIndex = reader.GetString("COURSE_INDEX");
                        string majorId = reader.GetString("MAJOR_ID");
                        string courseId = reader.GetString("COURSE_ID");
                        string courseName = reader.GetString("COURSE_NAME");
                        int score = Convert.ToInt32(reader.GetString("SCORE"));



                        SelectCourseList.Add(new ViewMode.ViewMode.SelectCurriculumViewMode(index, courseIndex, majorId, courseId, courseName, score));



                    }

                    DbConnect.MySqlConnection.Close();

                    SelectCurriculumListView.ItemsSource = SelectCourseList;

                }
                else
                {
                    Console.WriteLine("CurriculumPlan.LoadSelectCourse检查数据库连接信息！");
                }





            }










        }

        public ObservableCollection<ViewMode.ViewMode.SelectCurriculumViewMode> SelectCourseList =
            new ObservableCollection<ViewMode.ViewMode.SelectCurriculumViewMode>()
            { };


        private void MajorCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadSelectCourse();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                
            }
            
            
        }


        private void SelectCurriculumListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode.ViewMode.SelectCurriculumViewMode selectedCourse = SelectCurriculumListView.SelectedItem as ViewMode.ViewMode.SelectCurriculumViewMode;


            if (selectedCourse != null)
            {
                //记录当前选择的课程ID
                GlobalVariable.SelectCourseId = selectedCourse.CourseIndex;

                //加载院部数据到编辑框
                CurriculumNameTextBox.Text = selectedCourse.CourseName;
                CurriculumNumTextBox.Text = selectedCourse.CourseId;
                ScoreTextBox.Text = selectedCourse.Score.ToString();


            }
            else
            {
                Console.WriteLine("空信息");
            }
        }


        private void DelButton_OnClick(object sender, RoutedEventArgs e)
        {
            string CourseIndex = GlobalVariable.SelectCourseId;



            if (CourseIndex != null)
            {
                string sql = string.Format("DELETE FROM major_course WHERE COURSE_INDEX = '{0}'", CourseIndex);

                DbConnect.ModifySql(sql);
                LoadSelectCourse();
            }
            else
            {

                MessageBox.Show("请先选择所在专业对应的课程");

                return;
            }

            GlobalVariable.SelectCourseId = null;


        }



        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            string courseIndex = GlobalVariable.SelectCourseId;

            if (MajorInfoList.Count > 0 && courseIndex != "")
            {

                int score = 0;

                if (ScoreTextBox.Text.Trim() != "")
                {
                    score = Convert.ToInt32(ScoreTextBox.Text);
                }
                else
                {
                    MessageBox.Show("请设置学分信息");
                    return;
                }



                string sql = string.Format("UPDATE major_course SET SCORE= '{0}' WHERE COURSE_INDEX= '{1}'", score, courseIndex);

                Console.WriteLine(sql);

                DbConnect.ModifySql(sql);




                LoadSelectCourse();
            }
            else
            {
                MessageBox.Show("专业或课程信息缺失");
            }


        }
    }
}