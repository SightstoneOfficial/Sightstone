using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace Sightstone.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<object>
    {
        public override void CanClose(Action<bool> callback)
        {
            
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
