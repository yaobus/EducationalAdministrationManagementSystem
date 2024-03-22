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
using MaterialDesignColors;
using MySql.Data.MySqlClient;

namespace EducationalAdministrationManagementSystem.UserPlan
{
    /// <summary>
    /// PayPlan.xaml 的交互逻辑
    /// </summary>
    public partial class PayPlan : UserControl
    {
        public PayPlan()
        {
            InitializeComponent();

            LoadDepartmentData();
            LoadDefaultImage(); //加载默认头像图像
            DataBinding();
            Image.Source = DefaultBitmapImage;
        }



        /// <summary>
        /// 数据绑定
        /// </summary>
        public void DataBinding()
        {
            StudentListView.ItemsSource = StudentList;
            //CurriculumScoreListView.ItemsSource = CurriculumScoreList;
            CostShouldListView.ItemsSource = CostPayList;

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
            Image.Source = DefaultBitmapImage;
            SystemList.Clear();
            MajorInfoList.Clear();
            StudentList.Clear();
            ClearData();
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
            Image.Source = DefaultBitmapImage;
            MajorInfoList.Clear();
            StudentList.Clear();
            ClearData();
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
            Image.Source = DefaultBitmapImage;
            StudentList.Clear();
            ClearData();

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

                Console.WriteLine(sql);
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
        /// 选中学生后加载课程和分数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearData();
            //清空专业列表
            CostPayList.Clear();
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

                    //加载该学生的费用信息
                    LoadCostPayData(majorId);
                }



            }
            catch (Exception)
            {
                majorId = null;
                Console.WriteLine(e.ToString());

            }

        }


        /// <summary>
        ///加载已选专业费用信息
        /// </summary>
        private void LoadCostPayData(string majorId)
        {


            CostPayList.Clear();


            if (GlobalVariable.DbConnectInfo != null)
            {
                if (DbConnect.MySqlConnection.State == ConnectionState.Closed)
                {
                    DbConnect.MySqlConnection.Open(); //连接数据库
                    
                }
                if (majorId != null)
                {
                    string sql = string.Format("SELECT * FROM cost_should_info WHERE MAJOR_ID  = '{0}'", majorId);

                    MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);


                    List<string> indexList = new List<string>();
                    double sum = 0;
                    double sumPay = 0;
                    int index = 0;
                    while (reader.Read())
                    {
                        index += 1;
                        string costIndex = reader.GetString("COST_INDEX");
                        string id = reader.GetString("MAJOR_ID");
                        string costName = reader.GetString("COST_NAME");
                        string costType = reader.GetString("COST_TYPE");
                        double costNumber = Convert.ToDouble(reader.GetString("COST_NUMBER"));
                        string costNote = reader.GetString("COST_NOTE");
                        DateTime costDate = Convert.ToDateTime(reader.GetString("SHOULD_DATE"));
                        sum += costNumber;
                        //生成支付索引
                        string payIndex =
                            EncryptionDecryptionFunction.MD5EncryptionDecryption.MyTextMD5(
                                StudentId.Text + costIndex, 8);

                        indexList.Add(payIndex);

                        double costPay = 0;

                        string payNote = "";

                        string payDate = "";


                        CostPayList.Add(new ViewMode.ViewMode.CostPayViewMode(index, costIndex,
                            id, costName, costType, costNumber, costNote, costDate.ToString("D"), costPay, payNote, payDate));

                    }

                    CostTextBox.Text = sum.ToString();

                    reader.Close();




                    for (int i = 0; i < indexList.Count(); i++)
                    {

                        //加载该学生对应的科目成绩

                        string sql2 = string.Format("SELECT * FROM cost_pay_info WHERE PAY_INDEX = '{0}'",
                            indexList[i]);
                        Console.WriteLine("PayPlan.LoadCostPayData" + sql2);

                        //记录不存在则先增加记录并显示
                        if (DbConnect.CountDataNumber(sql2) < 1)
                        {


                            sql2 = string.Format(
                                "INSERT INTO cost_pay_info  VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",
                                indexList[i], CostPayList[i].CostIndex, StudentId.Text, 0, "", null);
                            Console.WriteLine(sql2);
                            DbConnect.ModifySql(sql2);

                        }
                        else //记录存在则加载数据
                        {

                            MySqlDataReader reader2 = DbConnect.CarrySqlCmd(sql2);

                            if (reader2.Read())
                            {
                                double costPay = Convert.ToDouble(reader2.GetString("COST_PAY"));
                                string payNote = reader2.GetString("PAY_NOTE");
                                string payDate = reader2.GetString("PAY_DATE");
                                sumPay += costPay;
                                CostPayList[i].CostPay = costPay;
                                CostPayList[i].PayNote = payNote;
                                CostPayList[i].PayDate = payDate;
                            }

                            reader2.Close();
                            PayTextBox.Text = sumPay.ToString();
                        }

                    }



                    DbConnect.MySqlConnection.Close();



                    if (sumPay < sum)
                    {
                        PayTextBox.Foreground = Brushes.IndianRed;
                    }
                    else
                    {
                        if (sumPay > sum)
                        {
                            PayTextBox.Foreground = Brushes.Orange;
                        }
                        else
                        {
                            PayTextBox.Foreground = Brushes.Green;
                        }


                    }



                }
                else
                {
                    Console.WriteLine("CurriculumPlan.LoadSelectCourse 检查数据库连接信息！");
                }


            }
        }

        public ObservableCollection<ViewMode.ViewMode.CostPayViewMode> CostPayList =
            new ObservableCollection<ViewMode.ViewMode.CostPayViewMode>()
            { };



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




        /// <summary>
        /// 清除页面数据
        /// </summary>
        private void ClearData()
        {
            GlobalVariable.CostPayIndex = null;
            CostTypeTextBox.Text = "";
            PayNumberTextBox.Text = "";
            PayNoteTextBox.Text = "";
            DatePicker.Text = "";
            CostTextBox.Text = "";
            PayTextBox.Text = "";
            PayTextBox.Foreground = Brushes.Gray;
            DatePicker.Text = DateTime.Now.ToString();
        }

        /// <summary>
        /// 选中费用信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CostShouldListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var costPay = CostShouldListView.SelectedItem as ViewMode.ViewMode.CostPayViewMode;

            if (costPay != null)
            {

                GlobalVariable.CostPayIndex = costPay.CostIndex;
                CostNumberTextBox.Text = costPay.CostNumber.ToString();
                CostTypeTextBox.Text = costPay.CostType;
                PayNumberTextBox.Text = costPay.CostPay.ToString();
                PayNoteTextBox.Text = costPay.PayNote;
                DatePicker.Text = costPay.PayDate;


            }
        }



        /// <summary>
        /// 保存缴费数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            double costPay = 0;
            string payNote = "";
            string payDate = DatePicker.Text;
            double payNumber = 0;





            try
            {
                payNumber = Math.Abs(Convert.ToDouble(CostNumberTextBox.Text));
                costPay = Math.Abs(Convert.ToDouble(PayNumberTextBox.Text.Trim()));
                payNote = PayNoteTextBox.Text;



            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }








            if (costPay <= 0)
            {
                MessageBox.Show("缴费信息不完整");
            }
            else
            {

                if (costPay <= payNumber)
                {
                    Console.WriteLine("缴纳"+costPay+"需缴纳"+payNumber);
                    string index = EncryptionDecryptionFunction.MD5EncryptionDecryption.MyTextMD5(
                            StudentId.Text + GlobalVariable.CostPayIndex, 8);

                    if (CostTypeTextBox.Text == "退费")
                    {
                        costPay *= -1;
                    }



                    string sql = string.Format("UPDATE cost_pay_info SET  COST_PAY='{0}',PAY_NOTE='{1}',PAY_DATE='{2}' WHERE PAY_INDEX='{3}'", costPay, payNote, payDate, index);
                    Console.WriteLine(sql);
                    DbConnect.ModifySql(sql);
                    //加载该学生的缴费信息

                    string majorId = MajorInfoList[MajorCombobox.SelectedIndex].Num;

                    LoadCostPayData(majorId);
                    
                }
                else
                {
                    MessageBox.Show("执行金额不得大于目标金额");
                }




            }


        }


    }
}
