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
using GloableVariable;
using MySql.Data.MySqlClient;

namespace EducationalAdministrationManagementSystem.UserPlan
{
    /// <summary>
    /// TeacherPlan.xaml 的交互逻辑
    /// </summary>
    public partial class TeacherPlan : UserControl
    {
        public TeacherPlan()
        {
            InitializeComponent();
            LoadTeacherData();
        }


        /// <summary>
        /// 加载教师列表
        /// </summary>
        private void LoadTeacherData()
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
                string sql = "SELECT * FROM teacher_info WHERE DELSTATUS != 1";

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);



                TeacherList.Clear();
                int index = 0;
                while (reader.Read())
                {
                    index += 1;



                    string teacherId = reader.GetString("TEACHER_ID").ToString();
                    string teacherName = reader.GetString("TEACHER_NAME").ToString();
                    string teacherSex = reader.GetString("TEACHER_SEX").ToString();
                    string teacherNote = reader.GetString("TEACHER_NOTE").ToString();
                    string password = reader.GetString("PASSWORD").ToString();
                    string delStatus = reader.GetString("DELSTATUS").ToString();

                    TeacherList.Add(new ViewMode.ViewMode.TeacherViewMode(index, teacherId, teacherName, teacherSex, teacherNote, password, delStatus));
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


            TeacherListView.ItemsSource = TeacherList;

        }



        public ObservableCollection<ViewMode.ViewMode.TeacherViewMode> TeacherList = new ObservableCollection<ViewMode.ViewMode.TeacherViewMode>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };

        private void TeacherListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode.ViewMode.TeacherViewMode TeaccherInfo = TeacherListView.SelectedItem as ViewMode.ViewMode.TeacherViewMode;



            //清空系部编辑框
            NewButton_OnClick(null, null);


            if (TeaccherInfo != null)
            {
                //记录当前选择的院部ID
                GlobalVariable.SelectTeacherId = TeaccherInfo.TeacherId;

                //加载院部数据到编辑框
                TeacherIdTextBox.Text = TeaccherInfo.TeacherId;
                TeacherNameTextBox.Text = TeaccherInfo.TeacherName;
                TeacherSexTextBox.Text = TeaccherInfo.TeacherSex;
                TeacherNoteTextBox.Text = TeaccherInfo.TeacherNote;
              

            }
        }







        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            GlobalVariable.SelectTeacherId = null;
            TeacherIdTextBox.Text = "";
            TeacherNameTextBox.Text = "";
            TeacherSexTextBox.Text = "";
            TeacherNoteTextBox.Text = "";
            TeacherListView.SelectedIndex = -1;
        }


        private void DelButton_OnClick(object sender, RoutedEventArgs e)
        {
            string sql = string.Format("UPDATE teacher_info SET DELSTATUS='1' WHERE TEACHER_ID='{0}'", GlobalVariable.SelectTeacherId);
            DbConnect.ModifySql(sql);

            NewButton_OnClick(null, null);
            LoadTeacherData();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TeacherIdTextBox.Text.Trim() != "" && TeacherNameTextBox.Text.Trim() != "")
            {
                string id = TeacherIdTextBox.Text.Trim();

                string name = TeacherNameTextBox.Text.Trim();

                string sex = TeacherSexTextBox.Text.Trim();
                
                string note = TeacherNoteTextBox.Text.Trim();

                string password = EncryptionDecryptionFunction.MD5EncryptionDecryption.PasswordMD5("123456");

                string sql;


                //存储修改后的数据
                if (GlobalVariable.SelectTeacherId != null)
                {
                    sql = string.Format("UPDATE teacher_info SET TEACHER_NAME='{0}', TEACHER_SEX='{1}',TEACHER_NOTE='{2}' WHERE TEACHER_ID='{3}'", name, sex, note, GlobalVariable.SelectTeacherId);
                    DbConnect.ModifySql(sql);
                    LoadTeacherData();

                }
                //存储新建的数据
                else
                {
                    sql = string.Format("SELECT * FROM teacher_info WHERE TEACHER_ID = '{0}'", id);

                    if (DbConnect.CountDataNumber(sql) < 1)
                    {

                        sql = string.Format("INSERT INTO teacher_info  VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", id, name, sex, note, password, "0");

                        Console.WriteLine(sql);

                        DbConnect.ModifySql(sql);
                        LoadTeacherData();
                    }
                    else
                    {
                        MessageBox.Show("教师ID已存在！");
                    }





                }

                TeacherIdTextBox.Text = "";
                TeacherNameTextBox.Text = "";
                TeacherSexTextBox.Text = "";
                TeacherNoteTextBox.Text = "";

                //清空选择，防止误操作
                GlobalVariable.SelectDepartmentId = null;


            }
            else
            {
                MessageBox.Show("课程信息不完整不得为空");

            }





        }
    }
}
