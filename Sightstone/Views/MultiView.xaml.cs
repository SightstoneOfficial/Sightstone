using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Caliburn.Micro;
using Sightstone.ViewHelper;
using Sightstone.Views.Controls;

namespace Sightstone.Views
{
    /// <summary>
    /// Interaction logic for MultiView.xaml
    /// </summary>
    public partial class MultiView
    {
        public MultiView()
        {
            ViewHelpers.SendNotficationSlideEvent += DoMoveGridAnimation;
            InitializeComponent();
        }

        public void DoMoveGridAnimation()
        {
            var thickness = MoveContainer.Margin;
            Dispatcher.Invoke(() =>
            {
                var moveAnimation = new ThicknessAnimation(new Thickness(thickness.Left, 5, thickness.Right, 0), TimeSpan.FromSeconds(2.25));
                MoveContainer.BeginAnimation(MarginProperty, moveAnimation);
            });

            var timer = new Timer { Interval = 20000 };
            timer.Elapsed += (o, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var moveAnimation = new ThicknessAnimation(thickness, TimeSpan.FromSeconds(2.25));
                    MoveContainer.BeginAnimation(MarginProperty, moveAnimation);
                });
            };
            timer.Start();
        }
    }
}
