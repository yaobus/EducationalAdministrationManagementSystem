using EducationalAdministrationManagementSystem.EncryptionDecryptionFunction;
using MySql.Data.MySqlClient;
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

namespace EducationalAdministrationManagementSystem.UserPlan
{
    /// <summary>
    /// CostPlan.xaml 的交互逻辑
    /// </summary>
    public partial class CostPlan : UserControl
    {
        public CostPlan()
        {
            InitializeComponent();
            LoadDepartmentData();
            List<string> costTyPe = new List<string>();
            costTyPe.Add("缴纳");
            costTyPe.Add("退费");
            CostTypeCombobox.ItemsSource = costTyPe;
            DatePicker.Text = DateTime.Now.ToString();
            CostShouldListView.ItemsSource = CostShouldList;
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



        public ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode> MajorInfoList =
            new ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode>()
            {
                //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
            };


        private void DepartmentCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            SystemCombobox.SelectedIndex = -1;
            CostShouldList.Clear();
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

        private void SystemCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MajorCombobox.SelectedIndex = -1;
            CostShouldList.Clear();
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

        private void MajorCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CostShouldList.Clear();
            try
            {
                LoadCostShould(MajorInfoList[MajorCombobox.SelectedIndex].Num);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

            }


        }

        /// <summary>
        /// 加载缴费信息
        /// </summary>
        /// <param name="majorId"></param>
        private void LoadCostShould(string majorIndex)
        {
            CostShouldList.Clear();

            if (majorIndex != null)
            {
                string sql = String.Format("SELECT * FROM cost_should_info WHERE MAJOR_ID='{0}'", majorIndex);

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

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);
                double sum = 0;
                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string costIndex = reader.GetString("COST_INDEX");
                    string majorId = reader.GetString("MAJOR_ID");
                    string costName = reader.GetString("COST_NAME");
                    string costType = reader.GetString("COST_TYPE");
                    float costNumber = reader.GetFloat("COST_NUMBER");
                    string costNote = reader.GetString("COST_NOTE");
                    string date = reader.GetString("SHOULD_DATE");
                    sum += costNumber;
                    CostShouldList.Add(new ViewMode.ViewMode.CostShouldViewMode(index, costIndex, majorId, costName,
                        costType, costNumber, costNote, date));

                }


                DbConnect.MySqlConnection.Close();
                SumTextBox.Text = sum.ToString();

            }
        }

        public ObservableCollection<ViewMode.ViewMode.CostShouldViewMode> CostShouldList =
            new ObservableCollection<ViewMode.ViewMode.CostShouldViewMode>()
            {
                //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
            };


        /// <summary>
        /// 添加费用信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            string majorId = MajorInfoList[MajorCombobox.SelectedIndex].Num;
            string costIndex;
            string costName = CostNameTextBox.Text.Trim();
            string costType = CostTypeCombobox.Text;
            double costNumber = Math.Abs(Convert.ToDouble(CostTextBox.Text.Trim()));
            string costNote = CostNoteTextBox.Text.Trim();
            string date = DatePicker.Text;

            if (CostTypeCombobox.SelectedIndex == 1)
            {
                costNumber *= -1;
            }

            if (majorId != null)
            {

                if (costName != null && costType != null && costNumber != 0 && date != null)
                {
                    costIndex =
                        EncryptionDecryptionFunction.MD5EncryptionDecryption.MyTextMD5(majorId + costName + date, 8);

                    string sql =
                        string.Format("INSERT INTO cost_should_info VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                            costIndex, majorId, costName, costType, costNumber,costNote, date);
                    Console.WriteLine("CostPlan.AddButton_OnClick" + sql);
                    DbConnect.ModifySql(sql);
                    LoadCostShould(majorId);
                    ClearDate();
                }
                else
                {
                    MessageBox.Show("费用信息不完整,请检查！");
                }

            }
            else
            {
                MessageBox.Show("请先选择一个专业!");
            }


        }

        /// <summary>
        /// 清空编辑框
        /// </summary>
        private void ClearDate()
        {
            CostNameTextBox.Text = null;
            CostTypeCombobox.SelectedIndex = -1;
            CostTextBox.Text = null;
            CostNoteTextBox.Text = null;
            DatePicker.Text = null;
        }



        //TODO 加载选中费用信息
        /// <summary>
        /// 加载选中的费用信息到编辑框（费用信息一旦生成，无法再次编辑及删除）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CostShouldListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewMode.ViewMode.CostShouldViewMode costShould = CostShouldListView.SelectedItem as ViewMode.ViewMode.CostShouldViewMode;
            
            if (costShould!=null)
            {
                CostNameTextBox.Text = costShould.CostName;
                CostTypeCombobox.Text = costShould.CostType;
                CostTextBox.Text = costShould.CostNumber.ToString();
                CostNoteTextBox.Text = costShould.CostNote;
                DatePicker.Text = costShould.ShouldDate;
            }


        }
    }
}