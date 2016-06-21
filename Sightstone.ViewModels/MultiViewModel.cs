using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Caliburn.Micro;
using Sightstone.Core;
using Sightstone.ViewModels.Controls;
using Sightstone.Views;
using Sightstone.Views.Controls;

namespace Sightstone.ViewModels
{
    public class MultiViewModel : Conductor<MultiView>, IShell
    {
        private object _mainContainer;
        public object MainContainer
        {
            get { return _mainContainer; }
            set
            {
                _mainContainer = value;
                NotifyOfPropertyChange(() => _mainContainer);
            }
        }

        private object _chatContainer;
        public object ChatContainer
        {
            get { return _chatContainer; }
            set
            {
                _chatContainer = value;
                NotifyOfPropertyChange(() => _chatContainer);
            }
        }
        

        private object _statusContainer;
        public object StatusContainer
        {
            get { return _statusContainer; }
            set
            {
                _statusContainer = value;
                NotifyOfPropertyChange(() => _statusContainer);
            }
        }

        private object _notificationContainer;
        public object NotificationContainer
        {
            get { return _notificationContainer; }
            set
            {
                _notificationContainer = value;
                NotifyOfPropertyChange(() => _notificationContainer);
            }
        }

        private Grid _moveContainer;
        public Grid MoveContainer
        {
            get { return _moveContainer; }
            set
            {
                _moveContainer = value;
                NotifyOfPropertyChange(() => _moveContainer);
            }
        }
        public MultiViewModel()
        {
            MainContainer = new AboutViewModel();
            MainContainer = new SettingsViewModel();
            SendNotification();
        }

        public void SendNotification()
        {
            MoveContainer = new Grid();
            MoveContainer.Children.Add(new ContentControl {Content = new TopSlideUserView()});
            WindowData.RunOnUIThread(() =>
            {
                var moveAnimation = new ThicknessAnimation(new Thickness(100, 5, 100, 0), TimeSpan.FromSeconds(2.25));
                MoveContainer.BeginAnimation(FrameworkElement.MarginProperty, moveAnimation);
            });

            var timer = new Timer {Interval = 20000};
            timer.Elapsed += (o, e) =>
            {
                WindowData.RunOnUIThread(() =>
                {
                    var moveAnimation = new ThicknessAnimation(new Thickness(100, -55, 100, 0), TimeSpan.FromSeconds(2.25));
                    MoveContainer.BeginAnimation(FrameworkElement.MarginProperty, moveAnimation);
                });
            };
            timer.Start();
        }
    }
}
