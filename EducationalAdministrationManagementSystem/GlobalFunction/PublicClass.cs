using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EducationalAdministrationManagementSystem.GlobalFunction
{
    class PublicClass
    {
        /// <summary>
        /// 设置程序显示语言
        /// </summary>
        /// <param name="languageTag">程序中已包含的语言字典名称，例如zh-CN</param>
        //设置语言
        public static void SetLanguage(string languageTag)
        {
            List<ResourceDictionary> dictionaryList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries)
            {
                dictionaryList.Add(dictionary);
            }

            string requestedCulture = string.Format(@"Language\{0}.xaml", languageTag);

            Uri reUri = new Uri(requestedCulture, UriKind.Relative);
            ResourceDictionary rdDictionary = (ResourceDictionary)Application.LoadComponent(reUri);
            Application.Current.Resources.MergedDictionaries.Remove(rdDictionary);
            Application.Current.Resources.MergedDictionaries.Add(rdDictionary);



        }



    }
}
