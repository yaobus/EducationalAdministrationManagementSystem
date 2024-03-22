using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalAdministrationManagementSystem.ViewMode
{
   public class ViewMode
    {



        #region 自定义院部数据类型
        /// <summary>
        /// 自定义院部数据类型
        /// </summary>
        public class DepartmentInfo
        {
            public int Index { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Note { get; set; }

            public string DelStatus { get; set; }


            public DepartmentInfo(int index, string id, string name, string note, string delStatus)
            {
                Index = index;
                Id = id;
                Name = name;
                Note = note;
                DelStatus = delStatus;

            }



        }

        #endregion

        #region 自定义系部数据类型
        /// <summary>
        /// 自定义系部数据类型
        /// </summary>
        public class SystemInfoViewMode
        {
            public int Index { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Note { get; set; }

            public string BeLong { get; set; }

            public string DelStatus { get; set; }


            public SystemInfoViewMode(int index, string id, string name, string note,string beLong ,string delStatus)
            {
                Index = index;
                Id = id;
                Name = name;
                Note = note;
                BeLong = beLong;
                DelStatus = delStatus;

            }



        }

        #endregion


        /// <summary>
        /// 自定义专业数据类型
        /// </summary>
        public class MajorInfoViewMode
        {
            public int Index { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Num { get; set; }

            public string Note { get; set; }

            public string Belong { get; set; }

            public string DelStatus { get; set; }


            public MajorInfoViewMode(int index, string id, string name, string num, string note, string belong, string delStatus)
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




        /// <summary>
        /// 自定义学生学籍数据类型
        /// </summary>
        public class StudentTextualViewMode
        {
            public int Index { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Sex { get; set; }
            public string Number { get; set; }
            public string System { get; set; }
            public string Certificate { get; set; }
            public string CertificateNumber { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Date { get; set; }
            public string SchoolName { get; set; }
            public string Company { get; set; }
            public string MajorLevel { get; set; }
            public string MajorType { get; set; }
            public string MajorName { get; set; }
            public string MajorNum { get; set; }
            public string EMail { get; set; }
            public string StudentType { get; set; }
            public string StudentLevel { get; set; }
            public string StudentEdu { get; set; }
            public string Politics { get; set; }
            public string Nation { get; set; }
            public string Occupation { get; set; }
            public string WorkCompany { get; set; }
            public string Health { get; set; }
            public string Census { get; set; }
            public string PostNum { get; set; }
            public string EngName { get; set; }
            public string BirthDay { get; set; }
            public string Code { get; set; }
            public string Graduation { get; set; }
            public string Sign { get; set; }
            public string SignType { get; set; }
            public string Teacher { get; set; }
            public string DelStatus { get; set; }



            public StudentTextualViewMode(int index, string id, string name, string sex, string number, string system, string certificate, string certificateNumber, string address, string phone, string date, string schoolName, string company, string majorLevel, string majorType, string majorName, string majorNum, string eMail, string studentType, string studentLevel, string studentEdu, string politics, string nation, string occupation, string workCompany, string health, string census, string postNum, string engName, string birthDay, string code, string graduation, string sign, string signType, string teacher, string delStatus)
            {
                Index = index;
                Id = id;
                Name = name;
                Sex = sex;
                Number = number;
                System = system;
                Certificate = certificate;
                CertificateNumber = certificateNumber;
                Address = address;
                Phone = phone;
                Date = date;
                SchoolName = schoolName;
                Company = company;
                MajorLevel = majorLevel;
                MajorType = majorType;
                MajorName = majorName;
                MajorNum = majorNum;
                EMail = eMail;
                StudentType = studentType;
                StudentLevel = studentLevel;
                StudentEdu = studentEdu;
                Politics = politics;
                Nation = nation;
                Occupation = occupation;
                WorkCompany = workCompany;
                Health = health;
                Census = census;
                PostNum = postNum;
                EngName = engName;
                BirthDay = birthDay;
                Code = code;
                Graduation = graduation;
                Sign = sign;
                SignType = signType;
                Teacher = teacher;
                DelStatus = delStatus;






            }




        }


        /// <summary>
        /// 自定义学生基础信息数据类型，用于成绩加载页面
        /// </summary>
        public class StudentInfoViewMode
        {
            public int Index { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
           
            public string StudentIndex { get; set; }


            public StudentInfoViewMode(int index, string id, string name,string studentIndex)
            {
                Index = index;
                Id = id;
                Name = name;
                StudentIndex = studentIndex;
            }




        }


        /// <summary>
        /// 自定义课程数据类型
        /// </summary>
        public class CurriculumViewMode
        {
            public int Index { get; set; }
            public string CourseId { get; set; }
            public string CourseName { get; set; }
            public string TeachBook { get; set; }
            public string BookAuthor { get; set; }
            public string BookPress { get; set; }
            public string Note { get; set; }
            public string DelStatus { get; set; }


            public CurriculumViewMode(int index, string courseId, string courseName, string teachBook,
                string bookAuthor, string bookPress, string note, string delStatus)
            {
                Index = index;
                CourseId = courseId;
                CourseName = courseName;
                TeachBook = teachBook;
                BookAuthor = bookAuthor;
                BookPress = bookPress;
                Note = note;
                DelStatus = delStatus;


            }
        }

        /// <summary>
        /// 已选择课程数据类型
        /// </summary>
        public class SelectCurriculumViewMode
        {
            public  int Index { get; set; }

            public string CourseIndex { get; set; }
            public string MajorId { get; set; }

            public string CourseId { get; set; }

            public string CourseName { get; set; }

            public int Score { get; set; }


            public SelectCurriculumViewMode(int index,string courseIndex ,string majorId,string courseId,string courseName,int score)
            {
                Index = index;
                CourseIndex = courseIndex;
                MajorId = majorId;
                CourseId = courseId;
                CourseName = courseName;
                Score = score;

            }



        }



        /// <summary>
        /// 已选择课程及分数数据类型
        /// </summary>
        public class CurriculumScoreViewMode
        {
            public int Index { get; set; }

            public string CourseIndex { get; set; }

            public string MajorId { get; set; }

            public string CourseId { get; set; }

            public string CourseName { get; set; }

            public string Score { get; set; }

            public string DailyScore { get; set; }

            public string LastScore { get; set; }

            public string CreditScore { get; set; }



            public CurriculumScoreViewMode(int index, string courseIndex, string majorId, string courseId, string courseName, string score,string dailyScore,string lastScore,string creditScore)
            {
                Index = index;
                CourseIndex = courseIndex;
                MajorId = majorId;
                CourseId = courseId;
                CourseName = courseName;
                Score = score;
                DailyScore = dailyScore;
                LastScore = lastScore;
                CreditScore = creditScore;

            }



        }








        /// <summary>
        /// 自定义教师数据类型
        /// </summary>
        public class TeacherViewMode
        {
            public int Index { get; set; }
            public string TeacherId { get; set; }
            public string TeacherName { get; set; }
            public string TeacherSex { get; set; }
            public string TeacherNote { get; set; }
            public string Password { get; set; }
            public string DelStatus { get; set; }


            public TeacherViewMode(int index, string teacherId, string teacherName, string teacherSex,
                string teacherNote, string password, string delStatus)
            {
                Index = index;
                TeacherId = teacherId;
                TeacherName = teacherName;
                TeacherSex = teacherSex;
                TeacherNote = teacherNote;
                Password = password;
               
                DelStatus = delStatus;


            }
        }


        /// <summary>
        /// 自定义教师数据类型
        /// </summary>
        public class RewardViewMode
        {
            public int Index { get; set; }
            public string RewardIndex { get; set; }
            public string StudentId { get; set; }
            public string StudentName { get; set; }
            public string Date { get; set; }
            public string RewardType { get; set; }
            public string Note { get; set; }
            public string DelStatus { get; set; }


            public RewardViewMode(int index, string rewardIndex, string studentId, string studentName,
                string date, string rewardType, string note, string delStatus)
            {
                Index = index;
                RewardIndex = rewardIndex;
                StudentId = studentId;
                StudentName = studentName;
                Date = date;
                RewardType = rewardType;
                Note = note;
                DelStatus = delStatus;


            }
        }

        /// <summary>
        /// 费用规划数据类型
        /// </summary>
        public class CostShouldViewMode
        {
            public int Index { get; set; }

            public string CostIndex { get; set; }

            public string MajorId { get; set; }

            public string CostName { get; set; }

            public string CostType { get; set; }

            public double CostNumber { get; set; }

            public string CostNote { get; set; }

            public  string ShouldDate { get; set; }


            public CostShouldViewMode(int index, string costIndex, string majorId, string costName, string costType, double costNumber, string costNote,string date)
            {
                Index = index;
                CostIndex = costIndex;
                MajorId = majorId;
                CostName = costName;
                CostType = costType;
                CostNumber = costNumber;
                CostNote = costNote;
                ShouldDate = date;
            }



        }


        /// <summary>
        /// 费用支付数据类型
        /// </summary>
        public class CostPayViewMode
        {
            public int Index { get; set; }

            public string CostIndex { get; set; }

            public string MajorId { get; set; }

            public string CostName { get; set; }

            public string CostType { get; set; }

            public  double CostNumber { get; set; }

            public string CostNote { get; set; }

            public string ShouldDate { get; set; }

            public double CostPay { get; set; }

            public string PayNote { get; set; }
            public string PayDate { get; set; }

            public CostPayViewMode(int index, string costIndex, string majorId, string costName, string costType, double costNumber, string costNote,string shouldDate, double costPay,string payNote,string payDate)
            {
                Index = index;
                CostIndex = costIndex;
                MajorId = majorId;
                CostName = costName;
                CostType = costType;
                CostNumber = costNumber;
                CostNote = costNote;
                ShouldDate = shouldDate;
                CostPay = costPay;
                PayNote = payNote;
                PayDate = payNote;
            }



        }

    }


}
