using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sightstone.ViewHelper
{
    public class ViewHelpers
    {
        public delegate void SendNotficationSlide();

        public static event SendNotficationSlide SendNotficationSlideEvent;

        public static void OnSendNotficationSlideEvent()
        {
            SendNotficationSlideEvent?.Invoke();
        }
    }
}
