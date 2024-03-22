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
    /// DepartmentPlan.xaml 的交互逻辑
    /// </summary>
    public partial class DepartmentPlan : UserControl
    {
        public DepartmentPlan()
        {
            InitializeComponent();

            //数据绑定到listview
            DeListView.ItemsSource = DepartmentList;
            SyListView.ItemsSource = SystemList;
            MaListView.ItemsSource = MajorList;

            LoadDepartmentData();
        }


        #region 院部数据操作

        /// <summary>
        /// 加载学院数据
        /// </summary>
        public void LoadDepartmentData()
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
                string sql = "SELECT * FROM DEPARTMENT_INFO WHERE DELSTATUS != 1";

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);



                DepartmentList.Clear();
                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string id = reader.GetString("DEPARTMENT_INDEX");
                    string name = Base64Conver.Base64Str2Text(reader.GetString("DEPARTMENT_NAME"));
                    string note = Base64Conver.Base64Str2Text(reader.GetString("DEPARTMENT_NOTE"));
                    string delStatus = reader.GetString("DELSTATUS");
                    DepartmentList.Add(new ViewMode.ViewMode.DepartmentInfo(index, id, name, note, delStatus));
                }

                DbConnect.MySqlConnection.Close();

                //清空院部选择项
                GlobalVariable.SelectDepartmentId = null;

                //清空院部编辑框
                DpEditButton_OnClick(null, null);

            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }








        }




        /// <summary>
        /// 单击表项时加载院部数据到编辑框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode.ViewMode.DepartmentInfo departmentInfo = DeListView.SelectedItem as ViewMode.ViewMode.DepartmentInfo;

            //清空系部列表
            SystemList.Clear();

            //清空系部编辑框
            SyEditButton_OnClick(null, null);

            //清空系部选择项
            GlobalVariable.SelectSystemId = null;


            //清空专业列表
            MajorList.Clear();

            //清空专业编辑框
            MaEditButton_OnClick(null, null);

            //清空专业选择项
            GlobalVariable.SelectMajorId = null;



            if (departmentInfo != null)
            {
                //记录当前选择的院部ID
                GlobalVariable.SelectDepartmentId = departmentInfo.Id;

                //加载院部数据到编辑框
                DepartmentNameTextBox.Text = departmentInfo.Name;
                DepartmentNoteTextBox.Text = departmentInfo.Note;



                //加载系部数据到系部列表
                LoadSystemData(GlobalVariable.SelectDepartmentId);




            }


        }



        /// <summary>
        /// 删除一条院部数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpDelButton_OnClick(object sender, RoutedEventArgs e)
        {
            string sql = string.Format("UPDATE DEPARTMENT_INFO SET DELSTATUS='1' WHERE DEPARTMENT_INDEX='{0}'", GlobalVariable.SelectDepartmentId);
            DbConnect.ModifySql(sql);

            DpEditButton_OnClick(null, null);
            LoadDepartmentData();
        }



        /// <summary>
        /// 新建院部记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpEditButton_OnClick(object sender, RoutedEventArgs e)
        {
            DepartmentNameTextBox.Text = "";
            DepartmentNoteTextBox.Text = "";
            GlobalVariable.SelectDepartmentId = null;
        }


        /// <summary>
        /// 保存院部记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DpSaveButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (DepartmentNameTextBox.Text.Trim() != "")
            {
                string id = MD5EncryptionDecryption.MyTextMD5(DepartmentNameTextBox.Text.Trim(), 8);

                string name = Base64Conver.Text2Base64Str(DepartmentNameTextBox.Text.Trim());//名称文本加密
                string note = Base64Conver.Text2Base64Str(DepartmentNoteTextBox.Text);//备注文本加密

                string sql;


                //存储修改后的数据
                if (GlobalVariable.SelectDepartmentId != null)
                {
                    sql = string.Format("UPDATE DEPARTMENT_INFO SET DEPARTMENT_NAME='{0}',DEPARTMENT_NOTE='{1}' WHERE DEPARTMENT_INDEX='{2}'", name, note, GlobalVariable.SelectDepartmentId);
                    DbConnect.ModifySql(sql);
                    LoadDepartmentData();//重新加载数据

                }
                //存储新建的数据
                else
                {
                    sql = string.Format("SELECT * FROM DEPARTMENT_INFO WHERE DEPARTMENT_NAME = '{0}'", name);

                    if (DbConnect.CountDataNumber(sql) < 1)
                    {

                        sql = string.Format("INSERT INTO DEPARTMENT_INFO  VALUES('{0}', '{1}', '{2}', '{3}')", id, name, note, 0);

                        Console.WriteLine(sql);

                        DbConnect.ModifySql(sql);
                        LoadDepartmentData();
                    }
                    else
                    {
                        MessageBox.Show("院部已存在！");
                    }






                }


                //清空选择，防止误操作
                GlobalVariable.SelectDepartmentId = null;


            }
            else
            {
                MessageBox.Show("院部名称不得为空");

            }












        }


        #endregion







        #region 系部数据操作
        /// <summary>
        /// 加载系部数据
        /// </summary>
        private void LoadSystemData(string departmentId)
        {

            //清空系部列表
            SystemList.Clear();

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
                string sql = string.Format("SELECT * FROM SYSTEM_INFO WHERE DELSTATUS != 1 AND BELONG_DEPARTMENT = '{0}'", departmentId);

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


                    SystemList.Add(new majorInfo(index, id, name, note, belong, delStatus));







                }

                DbConnect.MySqlConnection.Close();



            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }





        }


        /// <summary>
        /// 系部表项被选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            majorInfo majorInfo = SyListView.SelectedItem as majorInfo;

            if (majorInfo != null)
            {
                //记录当前选择系部ID
                GlobalVariable.SelectSystemId = majorInfo.Id;

                //清空专业列表
                MajorList.Clear();

                //清空专业编辑框
                MaEditButton_OnClick(null, null);

                //清空专业选择项
                GlobalVariable.SelectMajorId = null;


                //加载系部数据到系部列表
                LoadSystemData(GlobalVariable.SelectDepartmentId);

                //加载系部数据到编辑框
                SyNameTextBox.Text = majorInfo.Name;
                SyNoteTextbox.Text = majorInfo.Note;


                //加载专业信息到专业列表
                LoadMajor(GlobalVariable.SelectSystemId);

            }

        }


        /// <summary>
        /// 删除系部数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyDelButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalVariable.SelectSystemId != null)
            {
                string sql = string.Format("UPDATE SYSTEM_INFO SET DELSTATUS='1' WHERE SYSTEM_INDEX='{0}'", GlobalVariable.SelectSystemId);
                DbConnect.ModifySql(sql);

                SyEditButton_OnClick(null, null);
                LoadSystemData(GlobalVariable.SelectDepartmentId);
            }
            else
            {
                MessageBox.Show("请先选择系部再进行删除操作！");
            }



        }



        /// <summary>
        /// 编辑系部数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SyEditButton_OnClick(object sender, RoutedEventArgs e)
        {
            SyNameTextBox.Text = null;
            SyNoteTextbox.Text = null;
            GlobalVariable.SelectSystemId = null;
        }



        /// <summary>
        /// 保存系部数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SySaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalVariable.SelectDepartmentId != null)
            {
                //DEBUG
                Console.WriteLine(GlobalVariable.SelectDepartmentId + " DEID ");


                if (SyNameTextBox.Text.Trim() != "")
                {


                    string name = Base64Conver.Text2Base64Str(SyNameTextBox.Text.Trim());//名称文本加密
                    string note = Base64Conver.Text2Base64Str(SyNoteTextbox.Text);//备注文本加密
                    string belong = GlobalVariable.SelectDepartmentId;//所属院部ID
                    string sql;




                    //存储修改后的数据
                    if (GlobalVariable.SelectSystemId != null)
                    {
                        //DEBUG
                        //Console.WriteLine(GlobalVariable.SelectSystemId + " 修改数据DEID ");




                        sql = string.Format("UPDATE SYSTEM_INFO SET SYSTEM_NAME='{0}',SYSTEM_NOTE='{1}' WHERE SYSTEM_INDEX='{2}'", name, note, GlobalVariable.SelectSystemId);



                        //DEBUG
                        //Console.WriteLine(sql + " 修改数据SQL ");



                        DbConnect.ModifySql(sql);
                        LoadSystemData(belong);//重新加载数据

                    }
                    //存储新建的数据
                    else
                    {
                        //生成新ID
                        string id = MD5EncryptionDecryption.MyTextMD5(SyNameTextBox.Text.Trim(), 8);

                        //DEBUG
                        //Console.WriteLine(GlobalVariable.SelectDepartmentId + " 新建数据DEID ");


                        sql = string.Format("SELECT * FROM SYSTEM_INFO WHERE SYSTEM_NAME = '{0}'", name);

                        if (DbConnect.CountDataNumber(sql) < 1)
                        {

                            sql = string.Format("INSERT INTO SYSTEM_INFO  VALUES('{0}', '{1}', '{2}', '{3}','{4}')", id, name, note, belong, 0);

                            //DEBUG
                            //Console.WriteLine(sql + " 新建数据SQL ");

                            DbConnect.ModifySql(sql);
                            LoadSystemData(belong);
                        }
                        else
                        {
                            MessageBox.Show("系部已存在！");
                        }

                    }



                    //清空系部编辑框
                    SyEditButton_OnClick(null, null);

                }
                else
                {
                    MessageBox.Show("系部名称不得为空");

                }
            }
            else
            {
                MessageBox.Show("请先选择一个院部，然后再添加系部");
            }













        }


        #endregion









        #region 专业数据操作
        /// <summary>
        /// 加载专业信息
        /// </summary>
        /// <param name="systemId"></param>
        private void LoadMajor(string systemId)
        {

            //清空专业列表
            MajorList.Clear();

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


                    MajorList.Add(new MajorInfo(index, id, name, num, note, belong, delStatus));







                }

                DbConnect.MySqlConnection.Close();



            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }







        }



        /// <summary>
        /// 删除当前选择的专业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaDelButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalVariable.SelectMajorId != null)
            {
                string sql = string.Format("UPDATE MAJOR_INFO SET DELSTATUS='1' WHERE MAJOR_INDEX='{0}'", GlobalVariable.SelectMajorId);
                DbConnect.ModifySql(sql);

                MaEditButton_OnClick(null, null);
                LoadMajor(GlobalVariable.SelectSystemId);
            }
            else
            {
                MessageBox.Show("请先选择专业再进行删除操作！");
            }


        }


        /// <summary>
        /// 新建一个专业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaEditButton_OnClick(object sender, RoutedEventArgs e)
        {
            MaNameTextBox.Text = null;
            MaNoteTextBox.Text = null;
            MaNumTextBox.Text = null;
            GlobalVariable.SelectMajorId = null;


        }

        /// <summary>
        /// 保存专业
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaSaveButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (GlobalVariable.SelectSystemId != null)
            {
                //DEBUG
                Console.WriteLine(GlobalVariable.SelectSystemId + " SYID ");


                if (MaNameTextBox.Text.Trim() != "" && MaNumTextBox.Text != "")
                {

                    string name = Base64Conver.Text2Base64Str(MaNameTextBox.Text.Trim());//名称文本加密
                    string num = MaNumTextBox.Text.Trim();//专业编码
                    string note = Base64Conver.Text2Base64Str(MaNoteTextBox.Text);//备注文本加密
                    string belong = GlobalVariable.SelectSystemId;//所属系部ID
                    string sql;




                    //存储修改后的数据
                    if (GlobalVariable.SelectMajorId != null)
                    {
                        //DEBUG
                        Console.WriteLine(GlobalVariable.SelectMajorId + " 修改数据SYID ");




                        sql = string.Format("UPDATE MAJOR_INFO SET MAJOR_NAME='{0}',MAJOR_NUMBER='{1}',MAJOR_NOTE='{2}' WHERE MAJOR_INDEX='{3}'", name, num, note, GlobalVariable.SelectMajorId);



                        //DEBUG
                        Console.WriteLine(sql + " 修改数据SQL ");



                        DbConnect.ModifySql(sql);

                        LoadMajor(belong);//重新加载数据

                    }
                    //存储新建的数据
                    else
                    {
                        //生成新ID
                        string id = MD5EncryptionDecryption.MyTextMD5(MaNameTextBox.Text.Trim(), 8);

                        //DEBUG
                        Console.WriteLine(GlobalVariable.SelectMajorId + " 新建数据SYID ");


                        sql = string.Format("SELECT * FROM MAJOR_INFO WHERE MAJOR_NAME = '{0}'", name);

                        if (DbConnect.CountDataNumber(sql) < 1)
                        {

                            sql = string.Format("INSERT INTO MAJOR_INFO  VALUES('{0}', '{1}', '{2}', '{3}','{4}','{5}')", id, name, num, note, belong, 0);

                            //DEBUG
                            Console.WriteLine(sql + " 新建数据SQL ");

                            DbConnect.ModifySql(sql);
                            LoadMajor(belong);
                        }
                        else
                        {
                            MessageBox.Show("专业已存在！");
                        }

                    }



                    //清空系部编辑框
                    MaEditButton_OnClick(null, null);

                }
                else
                {
                    MessageBox.Show("专业名称和编码不得为空");

                }
            }
            else
            {
                MessageBox.Show("请先选择一个系部，然后再添加专业");
            }









        }


        /// <summary>
        /// 专业表项被选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            MajorInfo majorInfo = MaListView.SelectedItem as MajorInfo;

            if (majorInfo != null)
            {

                GlobalVariable.SelectMajorId = majorInfo.Id;

                //加载系部数据到编辑框
                MaNameTextBox.Text = majorInfo.Name;
                MaNumTextBox.Text = majorInfo.Num;
                MaNoteTextBox.Text = majorInfo.Note;



            }



        }
        #endregion







        public ObservableCollection<ViewMode.ViewMode.DepartmentInfo> DepartmentList = new ObservableCollection<ViewMode.ViewMode.DepartmentInfo>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };





        #region 自定义系部数据类型

        /// <summary>
        /// 自定义专业数据类型
        /// </summary>
        public class majorInfo
        {
            public int Index { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Note { get; set; }

            public string Belong { get; set; }

            public string DelStatus { get; set; }


            public majorInfo(int index, string id, string name, string note, string belong, string delStatus)
            {
                Index = index;
                Id = id;
                Name = name;
                Note = note;
                Belong = belong;
                DelStatus = delStatus;

            }



        }


        public ObservableCollection<majorInfo> SystemList = new ObservableCollection<majorInfo>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };



        #endregion

        #region 自定义专业数据类型

        private class MajorInfo
        {
            public int Index { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Num { get; set; }

            public string Note { get; set; }

            public string Belong { get; set; }

            public string DelStatus { get; set; }


            public MajorInfo(int index, string id, string name, string num, string note, string belong, string delStatus)
            {
                Index = index;
                Id = id;
                Name = name;
                Num = num;
                Note = note;
                Belong = belong;
                DelStatus = delStatus;

            }



        }


        private ObservableCollection<MajorInfo> MajorList = new ObservableCollection<MajorInfo>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };


        #endregion


    }



}
