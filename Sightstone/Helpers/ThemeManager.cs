using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace SightStone.Helpers
{
    [Export(typeof(IThemeManager))]
    public class ThemeManager : IThemeManager
    {
        private readonly ResourceDictionary _themeResources;

        public ThemeManager()
        {
            _themeResources = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Resources/Theme1.xaml")
            };
        }

        public ResourceDictionary GetThemeResources()
        {
            return _themeResources;
        }
    }
}
