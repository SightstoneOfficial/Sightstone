using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        public MultiViewModel()
        {
            //MainContainer = new AboutViewModel();
        }
    }
}
