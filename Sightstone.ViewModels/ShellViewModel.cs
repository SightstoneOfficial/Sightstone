using System;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Sightstone.Views;

namespace Sightstone.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<ShellView>
    {
        [ImportingConstructor]
        public ShellViewModel(IWindowManager windowManager)
        {
            
        }
        public async override void CanClose(Action<bool> callback)
        {
            var progressClose = await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Quit", "Are you sure you want to quit?", MessageDialogStyle.AffirmativeAndNegative);
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
        }
    }
}
