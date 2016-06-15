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
    public class MultiViewModel : Screen, IShell
    {
        public ContentControl MainContainer { get; set; }
        public ContentControl ChatContainer { get; set; }
        public ContentControl StatusContainer { get; set; }
        public ContentControl NotificationContainer { get; set; }
        public MultiViewModel()
        {

        }
    }
}
