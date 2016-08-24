using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Caliburn.Micro;
using Sightstone.Core;
using Sightstone.ViewHelper;
using Sightstone.ViewModels.Controls;
using Sightstone.Views;
using Sightstone.Views.Controls;
using Timer = System.Timers.Timer;

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

        private object _moveContainer;
        public object MoveContainer
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
            var t = new Timer(100);
            t.Elapsed += (o, e) =>
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    SendNotification(new TopSlideUserViewModel
                    {
                        InfoLabel = new Label { Content = "This is a test message" },
                        ProgressBar = new ProgressBar { IsIndeterminate = true }
                    });
                }));
                t.Stop();
            };
            t.Start();
        }

        public void SendNotification(TopSlideUserViewModel viewModel)
        {
            _moveContainer = viewModel;
            ViewHelpers.OnSendNotficationSlideEvent();
        }
    }
}
