using System;
using System.Windows;
using System.Windows.Controls;
using EducationalAdministrationManagementSystem.UserPlan;

namespace EducationalAdministrationManagementSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口任意移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                DragMove();
            }
        }





        /// <summary>
        /// 左上角菜单功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewMain_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            int index = ListViewMain.SelectedIndex;


            switch (index)
            {
                case 0:


                    break;

                case 1:


                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new DepartmentPlan());


                    break;


                case 2:
                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new AllCurriculum());

                    break;

                case 3:
                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new CurriculumPlan());

                    break;

                case 4:

                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new TeacherPlan());
                    break;
                case 5:

                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new StudentInfoPlan());

                    break;
                case 6:

                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new AchievementPlan());


                    break;

                case 7:

                        ChlidPlan.Children.Clear();
                        ChlidPlan.Children.Add(new TextualResearchPlan());


                    break;


                case 8:

                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new RewardPlan());


                    break;

                case 9:

                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new CostPlan());


                    break;

                case 10:

                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new PayPlan());


                    break;

                default:
                     break;
                
            }


        }



        /// <summary>
        /// 左下角功能列表实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewClient_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewClient.SelectedIndex;


            switch (index)
            {
                //索引为0的时候加载设置页面
                case 0:



                    break;

                //索引为1的时候加载用户管理页面
                case 1:



                    break;

                //索引为2的时候加载关于页面
                case 2:

                    ChlidPlan.Children.Clear();
                    ChlidPlan.Children.Add(new AboutPlan());
                    ListViewClient.SelectedIndex = -1;


                    break;
                    
                //索引为3的时候结束程序
                case 3:


                    Application.Current.Shutdown();
                    break;


            }
        }
    }
}
