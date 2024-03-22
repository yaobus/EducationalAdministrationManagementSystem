using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Shapes;
using EducationalAdministrationManagementSystem.EncryptionDecryptionFunction;
using EducationalAdministrationManagementSystem.UserPlan;
using GloableVariable;
using EducationalAdministrationManagementSystem.ViewMode;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.Data;

namespace EducationalAdministrationManagementSystem
{
    /// <summary>
    /// StudentEdit.xaml 的交互逻辑
    /// </summary>
    public partial class StudentEdit : Window
    {
        public StudentEdit()
        {
            InitializeComponent();

            GlobalFunction.FileClass.AnalysisDatabaseConfig();//解析数据库配置
            LoadComboBoxData();
            DatePicker.Text = DateTime.Now.ToString();
            //BirthDay.Text = DateTime.Now.ToString();


            if (GlobalVariable.TextualResearchId != 0)
            {
               
                LoadEditComboBoxData();//加载被编辑的数据
            }
            else
            {

                LoadNewComboBoxSelect();//加载新数据
            }



        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            GlobalVariable.PhotoModified = 0;
            this.Close();
        }



        /// <summary>
        /// 加载各个ComboBox的数据
        /// </summary>
        private void LoadComboBoxData()
        {
            //加载性别数据
            List<string> sexList = new List<string>()
            {
                {"男"},
                {"女"},

            };
            GenderCombobox.ItemsSource = sexList;


            //加载考生类型
            List<string> studentModeList = new List<string>()
            {
                {"应用型"},
                {"非应用型"},

            };
            StudentLevelCombobox.ItemsSource = studentModeList;




            //加载系部数据
            LoadSystemData();
            SystemCombobox.ItemsSource = systemList;



            //加载证件类型
            List<string> certificateList = new List<string>()
            {
                {"身份证" },
                {"居住证" },
                {"护照" },
                {"港澳通行证" },
                {"其他" },
            };
            CertificateCombobox.ItemsSource = certificateList;



            //加载专业层次
            List<string> majorTypeList = new List<string>()
            {
                {"本科"},
                {"专科"},
            };
            MajorComboBox.ItemsSource = majorTypeList;




            //加载专业名称
            //LoadMajorData();
            //MajorNameComboBox.ItemsSource = majorList;





            //加载政治面貌
            List<string> PoliticsList = new List<string>()
            {
                {"群众"},
                {"共青团员"},
                {"中共党员"},
                {"中共预备党员"},
                {"民革党员"},
                {"民盟盟员"},
                {"民建会员"},
                {"民进会员"},
                {"农工党党员"},
                {"致公党党员"},
                {"九三学社社员"},
                {"台盟盟员"},
                {"无党派人士"},
            };
            PoliticsCombobox.ItemsSource = PoliticsList;



            //加载民族数据
            List<string> nationList = new List<string>()
            {
                {"汉族" },
                {"蒙古族"},
                {"回族"},
                {"藏族"},
                {"维吾尔族"},
                {"苗族"},
                {"彝族"},
                {"壮族"},
                {"布依族"},
                {"朝鲜族"},
                {"满族"},
                {"侗族"},
                {"瑶族"},
                {"白族"},
                {"土家族"},
                {"哈尼族"},
                {"哈萨克族"},
                {"傣族"},
                {"黎族"},
                {"僳僳族"},
                {"畲族"},
                {"高山族"},
                {"拉祜族"},
                {"水族"},
                {"东乡族"},
                {"纳西族"},
                {"景颇族"},
                {"柯尔克孜族"},
                {"土族"},
                {"达斡尔族"},
                {"仫佬族"},
                {"羌族"},
                {"布朗族"},
                {"撒拉族"},
                {"毛南族"},
                {"仡佬族"},
                {"锡伯族"},
                {"阿昌族"},
                {"普米族"},
                {"塔吉克族"},
                {"怒族"},
                {"乌孜别克族"},
                {"俄罗斯族"},
                {"鄂温克族"},
                {"德昂族"},
                {"保安族"},
                {"裕固族"},
                {"京族"},
                {"塔塔尔族"},
                {"独龙族"},
                {"鄂伦春族"},
                {"赫哲族"},
                {"门巴族"},
                {"珞巴族"},
                {"基诺族"},



            };
            NationCombobox.ItemsSource = nationList;



            //加载户籍地信息
            List<string> censusList = new List<string>()
            {
                {"城市户口"},

                {"农村户口"},
            };
            CensusCombobox.ItemsSource = censusList;



            //加载考生类型信息
            List<string> studentTypeList = new List<string>()
            {
                {"一类"},
                {"二类"},
                {"三类"},
                };
            StudentTypeCombobox.ItemsSource = studentTypeList;


            //加载教师数据
            LoadTeacherData();
        }

        /// <summary>
        /// 加载各个选择框的数据
        /// </summary>
        private void LoadNewComboBoxSelect()
        {


            //加载性别数据

            GenderCombobox.SelectedIndex = 0;

            //加载考生类型

            StudentLevelCombobox.SelectedIndex = 0;





            //加载证件类型

            CertificateCombobox.SelectedIndex = 0;


            //加载专业层次

            MajorComboBox.SelectedIndex = 0;



            //加载专业名称
            //LoadMajorData();
            //MajorNameComboBox.ItemsSource = majorList;





            //加载政治面貌

            PoliticsCombobox.SelectedIndex = 0;


            //加载民族数据

            NationCombobox.SelectedIndex = 0;


            //加载户籍地信息

            CensusCombobox.SelectedIndex = 0;


            //加载考生类型信息
            StudentTypeCombobox.SelectedIndex = 0;
        }




        #region 加载专业信息




        /// <summary>
        /// 加载专业信息
        /// </summary>
        private void LoadMajorData(string systemId = null)
        {

            //清空专业列表
            MajorList.Clear();
            majorList.Clear();

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

                string sql;
                sql = string.Format("SELECT * FROM MAJOR_INFO WHERE DELSTATUS != 1 AND BELONG_SYSTEM='{0}'", systemId);
                Console.WriteLine(sql);



                //第二步,查询系部信息

                MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);







                int index = 0;
                while (reader.Read())
                {
                    index += 1;
                    string id = reader.GetString("MAJOR_INDEX").ToString();
                    string name = Base64Conver.Base64Str2Text(reader.GetString("MAJOR_NAME"));
                    string num = reader.GetString("MAJOR_NUMBER").ToString();
                    string note = Base64Conver.Base64Str2Text(reader.GetString("MAJOR_NOTE"));
                    string belong = reader.GetString("BELONG_SYSTEM");
                    string delStatus = reader.GetString("DELSTATUS");





                    MajorList.Add(new ViewMode.ViewMode.MajorInfoViewMode(index, id, name, num, note, belong, delStatus));

                    majorList.Add(name);



                }

                DbConnect.MySqlConnection.Close();

                MajorNameComboBox.ItemsSource = majorList;

            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }





        }

        
        public List<string> majorList = new List<string>();

        public ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode> MajorList = new ObservableCollection<ViewMode.ViewMode.MajorInfoViewMode>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };


        #endregion

        #region 加载系部数据
        /// <summary>
        /// 加载系部数据
        /// </summary>
        private void LoadSystemData()
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
                string sql = string.Format("SELECT * FROM SYSTEM_INFO WHERE DELSTATUS != 1 ");

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


                    SystemList.Add(new ViewMode.ViewMode.SystemInfoViewMode(index,id,name,note,belong,delStatus));

                    systemList.Add(name);





                }

                DbConnect.MySqlConnection.Close();



            }
            else
            {
                MessageBox.Show("数据库连接信息有误");
            }








        }


        public List<string> systemList = new List<string>();

        public ObservableCollection<ViewMode.ViewMode.SystemInfoViewMode> SystemList = new ObservableCollection<ViewMode.ViewMode.SystemInfoViewMode>()
        {
            //new DepartmentViewMode.DepartmentInfo("1","TEXT","3","4"),
        };




        #endregion



        /// <summary>
        /// 学籍数据列表
        /// </summary>
        public ObservableCollection<ViewMode.ViewMode.StudentTextualViewMode> StudentTextualList =
            new ObservableCollection<ViewMode.ViewMode.StudentTextualViewMode>();



        /// <summary>
        /// 加载被编辑的数据
        /// </summary>
        private void LoadEditComboBoxData()
        {
           

            Image.Source = GlobalFunction.ImageClass.BytesToBitmapImage(GlobalVariable.ImageBytes);
            ViewMode.ViewMode.StudentTextualViewMode v = GlobalVariable.SelectTextual;


            NameTextBox.Text = v.Name;
            GenderCombobox.Text = v.Sex;
            NumberTextBox.Text = v.Number;
            SystemCombobox.Text = v.System;
            CertificateCombobox.Text = v.Certificate;
            CertificateNumberTextBox.Text = v.CertificateNumber;
            AddressTextBox.Text = v.Address;
            PhoneNumberTextBox.Text = v.Phone;
            DatePicker.Text = v.Date;
            SchoolNameTextBox.Text = v.SchoolName;
            CompanyTextBox.Text = v.Company;
            MajorComboBox.Text = v.MajorLevel;
            MajorTypeTextBox.Text = v.MajorType;
            MajorNameComboBox.Text = v.MajorName;
            MajorNumberTextBox.Text = v.MajorNum;
            EmailTextBox.Text = v.EMail;
            StudentTypeCombobox.Text = v.StudentType;
            StudentLevelCombobox.Text = v.StudentLevel;
            StudentEduTextBox.Text = v.StudentEdu;
            PoliticsCombobox.Text = v.Politics;
            NationCombobox.Text = v.Nation;
            WorkCompanyTextBox.Text = v.WorkCompany;
            HealthTextBox.Text = v.Health;
            CensusCombobox.Text = v.Census;
            PostNumberTextBox.Text = v.PostNum;
            EnglishNameTextBox.Text = v.EngName;
            BirthDay.Text = v.BirthDay;
            CodeTextBox.Text = v.Code;
            GraduationTextBox.Text = v.Graduation;
            SignTextBox.Text = v.Sign;
            SignTypeTextBox.Text = v.SignType;
            TeacherCombobox.Text = v.Teacher;
        }


        /// <summary>
        /// 显示对应专业的编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MajorNameComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            try
            {
                MajorNumberTextBox.Text = MajorList[MajorNameComboBox.SelectedIndex].Num;
                Console.WriteLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                
            }
        }





        /// <summary>
        /// 保存学籍档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {

            #region 字段

            string id, //学生全局唯一ID
                name, //姓名
                sex, //性别
                number, //准考证号
                system, //系
                certificate, //证件类型
                certificateNumber, //证件号码
                address, //地址
                phone, //电话
                date, //报名日期
                schoolName, //学校名称
                company, //助学单位
                majorLevel, //专业层次
                majorType, //专业类型
                majorName, //专业名称
                majorNum, //专业编号
                eMail, //电子邮箱
                studentType, //学生类别一类、二类、三类
                studentLevel, //学生类型
                studentEdu, //学生学历
                politics, //政治面貌
                nation, //民族
                occupation, //职业
                workCompany, //工作单位
                health, //身体状况
                census, //户籍地
                postNum, //邮编
                engName, //英文名
                birthDay, //生日
                code, //集体代码
                graduation, //延期毕业
                sign, //发卡标志
                signType, //报名形式
                teacher;  //班主任

            #endregion


            //检查是否有必填项未填写
            CheckEmpty(NameTextBox, NumberTextBox, CertificateNumberTextBox, AddressTextBox, PhoneNumberTextBox, MajorNumberTextBox);

            //生日级报名日期未填写
            if (BirthDay.Text != null || DatePicker.Text == null)
            {


                //计算唯一用户ID
                id = MD5EncryptionDecryption.MyTextMD5(CertificateNumberTextBox.Text.Trim(), 8);
                name = NameTextBox.Text.Trim();
                sex = GenderCombobox.Text;
                number = NumberTextBox.Text;
                system = SystemCombobox.Text;
                certificate = CertificateCombobox.Text;
                certificateNumber = CertificateNumberTextBox.Text;
                address = AddressTextBox.Text;
                phone = PhoneNumberTextBox.Text;
                date = DatePicker.Text;
                schoolName = SchoolNameTextBox.Text;
                company = CompanyTextBox.Text;
                majorLevel = MajorComboBox.Text;
                majorType = MajorTypeTextBox.Text;
                majorName = MajorNameComboBox.Text;
                majorNum = MajorNumberTextBox.Text;
                eMail = EmailTextBox.Text;
                studentType = StudentTypeCombobox.Text;
                studentLevel = StudentLevelCombobox.Text;
                studentEdu = StudentEduTextBox.Text;
                politics = PoliticsCombobox.Text;
                nation = NationCombobox.Text;
                occupation = OccupationTextBox.Text;
                workCompany = WorkCompanyTextBox.Text;
                health = HealthTextBox.Text;
                census = CensusCombobox.Text;
                postNum = PostNumberTextBox.Text;
                engName = EnglishNameTextBox.Text;
                birthDay = BirthDay.Text;
                code = CodeTextBox.Text;
                graduation = GraduationTextBox.Text;
                sign = SignTextBox.Text;
                signType = SignTypeTextBox.Text;
                teacher = TeacherCombobox.Text;
                string sql;

                //修改档案
                if (GlobalVariable.TextualResearchId != 0)
                {
                    id = GlobalVariable.TextualStudentId;

                    sql = string.Format(
                        "UPDATE STUDENT_INFO SET STUDENT_NAME='{0}',SEX='{1}',NUMBER='{2}',SYSTEM_NAME='{3}',CERTIFICATE='{4}',CERTIFICATENUMBER='{5}',ADDRESS='{6}',PHONE='{7}',DATE='{8}',SCHOOLNAME='{9}',COMPANY='{10}',MAJORLEVEL='{11}',MAJORTYPE='{12}',MAJORNAME='{13}',MAJORNUM='{14}',EMAIL='{15}',STUDENTTYPE='{16}',STUDENTLEVEL='{17}',STUDENTEDU='{18}',POLITICS='{29}',NATION='{20}',OCCUPATION='{21}',WORKCOMPANY='{22}',HEALTH='{23}',CENSUS='{24}',POSTNUM='{25}',ENGNAME='{26}',BIRTHDAY='{27}',COLLECTIVE_CODE='{28}',GRADUATION='{29}',SIGN='{30}',SIGNTYPE='{31}', TEACHER='{32}' WHERE ID='{33}'",  name, sex, number, system, certificate, certificateNumber, address, phone, date, schoolName, company, majorLevel, majorType, majorName, majorNum, eMail, studentType, studentLevel, studentEdu, politics, nation, occupation, workCompany, health, census, postNum, engName, birthDay, code, graduation, sign, signType,teacher, id);
                    Console.WriteLine(sql);
                  //文本信息存储
                    DbConnect.ModifySql(sql);
                }
                //新建档案
                else
                {
                    sql = string.Format("SELECT * FROM student_info WHERE NUMBER = '{0}'", number);

                    if (DbConnect.CountDataNumber(sql) < 1)
                    {

                        sql = string.Format(
                            "INSERT INTO STUDENT_INFO  VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','0')",
                            id, name, sex, number, system, certificate, certificateNumber, address, phone, date,
                            schoolName, company, majorLevel, majorType, majorName, majorNum, eMail, studentType,
                            studentLevel, studentEdu, politics, nation, occupation, workCompany, health, census,
                            postNum, engName, birthDay, code, graduation, sign, signType, teacher);
                        Console.WriteLine(sql);
                        //文本信息存储
                        DbConnect.ModifySql(sql);

                    }
                    else
                    {
                        MessageBox.Show("此学号已存在！");
                        return;
                    }


                }



                sql = string.Format("SELECT * FROM portrait_img WHERE STUDENT_ID = '{0}'", id);

                //照片不存在则写入照片
                if (DbConnect.CountDataNumber(sql) < 1)
                {
                    if (GlobalVariable.PhotoModified == 1 && Image.Source != null)
                    {


                        //照片存储

                        sql = string.Format("INSERT INTO portrait_img  VALUES ('{0}',@file)", id);
                        //DbConnect.MySqlConnection.Open(); //连接数据库

                        DbConnect.MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);


                        DbConnect.MySqlConnection.Open(); //连接数据库

                        MySqlCommand SqlCommand = new MySqlCommand(sql, DbConnect.MySqlConnection);

                        SqlCommand.Parameters.Add("@file", MySqlDbType.Binary, GlobalVariable.ImageBytes.Length);
                        SqlCommand.Parameters["@file"].Value = GlobalVariable.ImageBytes;
                        SqlCommand.ExecuteNonQuery();
                        DbConnect.MySqlConnection.Dispose();

                    }

                }
                else//照片存在则更新照片
                {
                    if (GlobalVariable.PhotoModified == 1 && Image.Source != null)
                    {

                        //照片存储

                        sql = string.Format("UPDATE  portrait_img SET IMG=@file WHERE STUDENT_ID='{0}'", id);
                        //DbConnect.MySqlConnection.Open(); //连接数据库



                        DbConnect.MySqlConnection = DbConnect.ConnectionMysql(GlobalVariable.DbConnectInfo);


                        DbConnect.MySqlConnection.Open(); //连接数据库

                        MySqlCommand SqlCommand = new MySqlCommand(sql, DbConnect.MySqlConnection);

                        SqlCommand.Parameters.Add("@file", MySqlDbType.Binary, GlobalVariable.ImageBytes.Length);
                        SqlCommand.Parameters["@file"].Value = GlobalVariable.ImageBytes;
                        SqlCommand.ExecuteNonQuery();
                        DbConnect.MySqlConnection.Dispose();



                    }
                }

                GlobalVariable.PhotoModified = 0;
                this.Close();

            }
            else
            {
                MessageBox.Show("生日或报名日期未填写！");
            }

        }



        /// <summary>
        /// 浏览证件照图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseImgButton_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "请选择一寸证件照图片(*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg"
            };
            if ((bool)openFileDialog.ShowDialog())
            {
                //获取图片字节流
                byte[] bytes = GlobalFunction.ImageClass.FileToBytes(openFileDialog.FileName);
                try
                {
                    int x = bytes.Length;
                    if (x > 30 * 1024 || x == 0)
                    {
                        string str = string.Format("照片不得超过30kb\r\n" + "当前照片为：{0}kb", x / 1024);
                        MessageBox.Show(str, "照片不达标！");
                    }
                    else
                    {
                        //图片显示到头像框
                        Image.Source = GlobalFunction.ImageClass.BytesToBitmapImage(bytes);

                        //照片存放到缓存
                        GlobalVariable.ImageBytes = bytes;
                    }
                    
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                }
            }
            GlobalVariable.PhotoModified = 1;
        }




        /// <summary>
        /// 判断是否有必填项未填写
        /// </summary>
        /// <param name="text"></param>
        private static void CheckEmpty(params TextBox[] text)
        {
           

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Text.Trim().Length == 0)
                {
                    MessageBox.Show("该选项为必填项！");
                    text[i].Focus();
                    break;
                }
            }
            
        }



        /// <summary>
        /// 当前选中的系部ID，联动专业数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MajorNameComboBox.ItemsSource = null;
            
            LoadMajorData(SystemList[SystemCombobox.SelectedIndex].Id);

            if (majorList.Count > 0)
            {
                MajorNameComboBox.SelectedIndex = 0;
            }
            else
            {
                MajorNameComboBox.SelectedIndex = -1;
            }


        }




        private void LoadTeacherData()
        {
            List<string> teacherList = new List<string>();

            if (DbConnect.MySqlConnection.State == ConnectionState.Closed)
            {
                DbConnect.MySqlConnection.Open(); //连接数据库
            }
            string sql = "SELECT * FROM teacher_info";
            MySqlDataReader reader = DbConnect.CarrySqlCmd(sql);

            while (reader.Read())
            {
                string name = reader.GetString("TEACHER_NAME");
                teacherList.Add(name);
            }

            DbConnect.MySqlConnection.Close();
            TeacherCombobox.ItemsSource = teacherList;


        }

        private void SaveButton2_OnClick(object sender, RoutedEventArgs e)
        {
            
                          string  sql = string.Format("INSERT INTO portrait_img (STUDENT_ID,IMG) VALUES ('{0}',@file)", "f33470eb");
                DbConnect.MySqlConnection.Open(); //连接数据库

                MySqlCommand SqlCommand = new MySqlCommand(sql, DbConnect.MySqlConnection);

                SqlCommand.Parameters.Add("@file", MySqlDbType.Binary, GlobalVariable.ImageBytes.Length);
                SqlCommand.Parameters["@file"].Value = GlobalVariable.ImageBytes;
                SqlCommand.ExecuteNonQuery();
               // DbConnect.MySqlConnection.Close();


        }
    }


}
