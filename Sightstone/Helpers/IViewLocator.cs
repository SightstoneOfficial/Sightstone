using System;
using System.Windows;

namespace SightStone.Helpers
{
    public interface IViewLocator
    {
        UIElement GetOrCreateViewType(Type viewType);
    }
}
