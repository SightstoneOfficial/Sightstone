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
using Sightstone.Views;

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

        private Grid _moveGrid;
        public Grid MoveGrid
        {
            get { return _moveGrid; }
            set
            {
                _moveGrid = value;
                NotifyOfPropertyChange(() => _moveGrid);
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
            var moveAnimation = new ThicknessAnimation(new Thickness(100, 5, 100, 0), TimeSpan.FromSeconds(2.25));
            MoveGrid.BeginAnimation(FrameworkElement.MarginProperty, moveAnimation);

            var timer = new Timer {Interval = TimeSpan.FromSeconds(20).Milliseconds};
            timer.Elapsed += (o, e) =>
            {
                moveAnimation = new ThicknessAnimation(new Thickness(100, -55, 100, 0), TimeSpan.FromSeconds(2.25));
                MoveGrid.BeginAnimation(FrameworkElement.MarginProperty, moveAnimation);
            };
        }
    }
}
