using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;

namespace SightStone.Helpers
{
    [Export(typeof(IViewLocator))]
    public class ViewLocator : IViewLocator
    {
        private readonly IThemeManager _themeManager;

        [ImportingConstructor]
        public ViewLocator(IThemeManager themeManager)
        {
            _themeManager = themeManager;
        }

        public UIElement GetOrCreateViewType(Type viewType)
        {
            var cached = IoC.GetAllInstances(viewType).OfType<UIElement>().FirstOrDefault();
            if (cached != null)
            {
                Caliburn.Micro.ViewLocator.InitializeComponent(cached);
                return cached;
            }

            if (viewType.IsInterface || viewType.IsAbstract || !typeof(UIElement).IsAssignableFrom(viewType))
            {
                return new TextBlock { Text = $"Cannot create {viewType.FullName}."};
            }

            var newInstance = (UIElement)Activator.CreateInstance(viewType);
            var frameworkElement = newInstance as FrameworkElement;
            frameworkElement?.Resources.MergedDictionaries.Add(_themeManager.GetThemeResources());

            Caliburn.Micro.ViewLocator.InitializeComponent(newInstance);
            return newInstance;
        }
    }
}
