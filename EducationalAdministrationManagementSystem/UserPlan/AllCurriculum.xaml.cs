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
    /// AllCurriculum.xaml 的交互逻辑
    /// </summary>
    public partial class AllCurriculum : UserControl
    {
        public AllCurriculum()
        {
            InitializeComponent();
            LoadCurriculumData();
        }

        /// <summary>
        /// 加载课程池
        /// </summary>
        private void LoadCurriculumData()
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
                    CurriculumList.Add(new ViewMode.ViewMode.CurriculumViewMode(index, id, name, teachBook, bookAuthor, bookPress, note, delStatus));
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


            CurriculumListView.ItemsSource = CurriculumList;

        }



        public ObservableCollection<ViewMode.ViewMode.CurriculumViewMode> CurriculumList = new ObservableCollection<ViewMode.ViewMode.CurriculumViewMode>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };




        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurriculumNameTextBox.Text.Trim() != "" && BookNameTextBox.Text.Trim() != "")
            {
                string id = CurriculumNumTextBox.Text.Trim();

                string name = CurriculumNameTextBox.Text.Trim();

                string teachBook = BookNameTextBox.Text.Trim();

                string bookAuthor = BookAuthorTextBox.Text.Trim();

                string bookPress = BookPressTextBox.Text.Trim();

                string note = BookNoteTextBox.Text.Trim();

                string sql;


                //存储修改后的数据
                if (GlobalVariable.SelectCurriculumId != null)
                {
                    sql = string.Format("UPDATE course_pool SET COURSE_NAME='{0}', TEACH_BOOK='{1}',BOOK_AUTHOR='{2}',BOOK_PRESS='{3}',NOTE='{4}' WHERE COURSE_ID='{5}'", name, teachBook, bookAuthor, bookPress, note, GlobalVariable.SelectCurriculumId);
                    DbConnect.ModifySql(sql);
                    LoadCurriculumData();

                }
                //存储新建的数据
                else
                {
                    sql = string.Format("SELECT * FROM course_pool WHERE COURSE_ID = '{0}'", id);

                    if (DbConnect.CountDataNumber(sql) < 1)
                    {

                        sql = string.Format("INSERT INTO course_pool  VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", id, name, teachBook, bookAuthor, bookPress, note, "0");

                        Console.WriteLine(sql);

                        DbConnect.ModifySql(sql);
                        LoadCurriculumData();
                    }
                    else
                    {
                        MessageBox.Show("课程已存在！");
                    }





                }


                //清空选择，防止误操作
                GlobalVariable.SelectDepartmentId = null;
                NewButton_OnClick(null,null);

            }
            else
            {
                MessageBox.Show("课程信息不完整不得为空");

            }









        }

        private void CurriculumListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode.ViewMode.CurriculumViewMode CurriculumInfo = CurriculumListView.SelectedItem as ViewMode.ViewMode.CurriculumViewMode;



            //清空系部编辑框
            NewButton_OnClick(null, null);


            if (CurriculumInfo != null)
            {
                //记录当前选择的院部ID
                GlobalVariable.SelectCurriculumId = CurriculumInfo.CourseId;

                //加载院部数据到编辑框
                CurriculumNameTextBox.Text = CurriculumInfo.CourseName;
                CurriculumNumTextBox.Text = CurriculumInfo.CourseId;
                BookNameTextBox.Text = CurriculumInfo.TeachBook;
                BookAuthorTextBox.Text = CurriculumInfo.BookAuthor;
                BookPressTextBox.Text = CurriculumInfo.BookPress;
                BookNoteTextBox.Text = CurriculumInfo.Note;

            }
        }

        /// <summary>
        /// 新建课程信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            CurriculumNameTextBox.Text = "";
            CurriculumNumTextBox.Text = "";
            BookNameTextBox.Text = "";
            BookAuthorTextBox.Text = "";
            BookPressTextBox.Text = "";
            BookNoteTextBox.Text = "";
            GlobalVariable.SelectCurriculumId = null;
        }



        private void DelButton_OnClick(object sender, RoutedEventArgs e)
        {
            string sql = string.Format("UPDATE course_pool SET DELSTATUS='1' WHERE COURSE_ID='{0}'", GlobalVariable.SelectCurriculumId);
            DbConnect.ModifySql(sql);

            NewButton_OnClick(null, null);
            LoadCurriculumData();
        }
    }
}
