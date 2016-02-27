using System;
using System.Threading;
using System.Windows;
using Sightstone.Core.Properties;

namespace Sightstone.Core.Languages
{
    public static class LanguageManager
    {
        public static void GetLanguage()
        {
            //Load the language resources.
            var dict = new ResourceDictionary();
            var settings = new Settings();
            if (settings.Language != null)
            {
                dict.Source = new Uri("..\\Languages\\" + settings.Language + ".xaml", UriKind.Relative);
            }
            else
            {
                var lid = Thread.CurrentThread.CurrentCulture.ToString().Contains("-")
                              ? Thread.CurrentThread.CurrentCulture.ToString().Split('-')[0].ToUpperInvariant()
                              : Thread.CurrentThread.CurrentCulture.ToString().ToUpperInvariant();
                switch (lid)
                {
                    /*
                    case "DE":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\German.xaml", UriKind.Relative);
                        break;
                    case "AR":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Arabic.xaml", UriKind.Relative);
                        break;
                    case "ES":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Spanish.xaml", UriKind.Relative);
                        break;
                    case "FR":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\French.xaml", UriKind.Relative);
                        break;
                    case "IT":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Italian.xaml", UriKind.Relative);
                        break;
                    case "KO":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Korean.xaml", UriKind.Relative);
                        break;
                    case "NL":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Dutch.xaml", UriKind.Relative);
                        break;
                    case "PL":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Polish.xaml", UriKind.Relative);
                        break;
                    case "PT":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Portuguese.xaml", UriKind.Relative);
                        break;
                    case "RO":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Romanian.xaml", UriKind.Relative);
                        break;
                    case "RU":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Russian.xaml", UriKind.Relative);
                        break;
                    case "SE":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Swedish.xaml", UriKind.Relative);
                        break;
                    case "TR":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Turkish.xaml", UriKind.Relative);
                        break;
                    case "VI":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Vietnamese.xaml", UriKind.Relative);
                        break;
                    case "ZH":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Chinese.xaml", UriKind.Relative);
                        break;
                    case "LT":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Lithuanian.xaml", UriKind.Relative);
                        break;
                    case "CZ":
                        settings.Language = "German";
                        dict.Source = new Uri("..\\Languages\\Czech.xaml", UriKind.Relative);
                        break;
                        //*/
                    default:
                        settings.Language = "English";
                        dict.Source = new Uri("..\\Languages\\English.xaml", UriKind.Relative);
                        break;
                }
                settings.Save();
            }

            WindowData.MainWindow.Resources.MergedDictionaries.Add(dict);
        }
    }
}
