using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Sightstone.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell
    {
        public void Close()
        {
            this.TryClose();
        }
        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.DisplayName = "Sightstone";
        }
    }
}
