using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sightstone.Core;

namespace Sightstone.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell
    {
        [ImportingConstructor]
        public ShellViewModel(IWindowManager windowManager)
        {
            WindowData.WindowManager = windowManager;
        }
        public async override void CanClose(Action<bool> callback)
        {
            var progressClose = await WindowData.MainWindow.ShowMessageAsync("Quit", "Are you sure you want to quit?", MessageDialogStyle.AffirmativeAndNegative);
            if (progressClose == MessageDialogResult.Affirmative)
                callback(true);
            callback(false);
        }
        public void Close()
        {
            TryClose();
        }
        protected override void OnInitialize()
        {
            base.OnInitialize();
            DisplayName = "Sightstone";
            WindowData.MainWindow = (Application.Current.MainWindow as MetroWindow);
            EnsureItem(new LoginViewModel());
            ActivateItem(new LoginViewModel());
            
        }
    }
}
