using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Sightstone.Views.Controls;
using System.Windows.Controls;

namespace Sightstone.ViewModels.Controls
{
    public class TopSlideUserViewModel : Screen, IShell
    {
        private Label _infoLabel;
        public Label InfoLabel
        {
            get { return _infoLabel; }
            set
            {
                _infoLabel = value;
                NotifyOfPropertyChange(() => _infoLabel);
            }
        }


        private ProgressBar _progressBar;
        public ProgressBar ProgressBar
        {
            get { return _progressBar; }
            set
            {
                _progressBar = value;
                NotifyOfPropertyChange(() => _progressBar);
            }
        }
    }
}
