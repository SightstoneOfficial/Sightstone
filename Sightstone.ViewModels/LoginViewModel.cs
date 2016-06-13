using System;
using Caliburn.Micro;
using Sightstone.Views;

namespace Sightstone.ViewModels
{
    public class LoginViewModel : Conductor<LoginView>, IShell, IScreen
    {
        bool _LoginGridIsVisible;
        public bool LoginGridIsVisible {
            get
            {
                return _LoginGridIsVisible;
            }
            set
            {
                if (_LoginGridIsVisible != value)
                {
                    _LoginGridIsVisible = value;
                    NotifyOfPropertyChange(() => LoginGridIsVisible);
                }
            }

        }
        public void Unlock(EventArgs args)
        {
            
        }

        public void AddAccount(EventArgs args)
        {
            LoginGridIsVisible = true;
        }
    }
}
