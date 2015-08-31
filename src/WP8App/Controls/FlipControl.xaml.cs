using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WPAppStudio.Controls
{
    public partial class FlipControl : UserControl
    {
        public static readonly DependencyProperty NextElementCommandProperty =
            DependencyProperty.Register("NextElementCommand", typeof(ICommand), typeof(FlipControl), new PropertyMetadata(null));

        public ICommand NextElementCommand
        {
            get { return (ICommand)GetValue(NextElementCommandProperty); }
            set { SetValue(NextElementCommandProperty, value); }
        }

        public static readonly DependencyProperty PreviousElementCommandProperty =
            DependencyProperty.Register("PreviousElementCommand", typeof(ICommand), typeof(FlipControl), new PropertyMetadata(null));

        public ICommand PreviousElementCommand
        {
            get { return (ICommand)GetValue(PreviousElementCommandProperty); }
            set { SetValue(PreviousElementCommandProperty, value); }
        }

        public static readonly DependencyProperty ShowPreviousButtonProperty =
            DependencyProperty.Register("ShowPreviousButton", typeof(bool), typeof(FlipControl), new PropertyMetadata(null));

        public bool ShowPreviousButton
        {
            get { return (bool)GetValue(ShowPreviousButtonProperty); }
            set { SetValue(ShowPreviousButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowNextButtonProperty =
            DependencyProperty.Register("ShowNextButton", typeof(bool), typeof(FlipControl), new PropertyMetadata(null));

        public bool ShowNextButton
        {
            get { return (bool)GetValue(ShowNextButtonProperty); }
            set { SetValue(ShowNextButtonProperty, value); }
        }

        public static readonly DependencyProperty InnerContentProperty =
            DependencyProperty.Register("InnerContent", typeof(object), typeof(FlipControl),
              new PropertyMetadata(null));

        public object InnerContent
        {
            get { return GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        public static readonly DependencyProperty BlockTransitionsProperty =
            DependencyProperty.Register("BlockTransitions", typeof(bool), typeof(FlipControl), new PropertyMetadata(null));

        public bool BlockTransitions
        {
            get { return (bool)GetValue(BlockTransitionsProperty); }
            set { SetValue(BlockTransitionsProperty, value); }
        }

        public FlipControl()
        {
            InitializeComponent();
            SlideLeft.Completed += SlideLeft_Completed;
            SlideRight.Completed += SlideRight_Completed;
            SlideTopLeft.Completed += SlideTopLeft_Completed;
            SlideTopRight.Completed += SlideTopRight_Completed;
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            base.OnManipulationCompleted(e);

            if(BlockTransitions) return;

            var horizontalVelocity = e.FinalVelocities.LinearVelocity.X;
            var verticalVelocity = e.FinalVelocities.LinearVelocity.Y;

            var direction = GetDirection(horizontalVelocity, verticalVelocity);

            if (direction == Orientation.Horizontal && Math.Abs(horizontalVelocity) > 200)
            {
                if (e.TotalManipulation.Translation.X < 0)
                {
                    if (ShowNextButton)
                        SlideLeft.Begin();
                    else
                        SlideTopLeft.Begin();
                }
                else
                {
                    if (ShowPreviousButton)
                        SlideRight.Begin();
                    else
                        SlideTopRight.Begin();
                }
            }
        }

        private void SlideLeft_Completed(object sender, EventArgs e)
        {
            NextElementCommand.Execute(null);
            SlideRightReset.Begin();
        }

        private void SlideRight_Completed(object sender, EventArgs e)
        {
            PreviousElementCommand.Execute(null);
            SlideLeftReset.Begin();
        }

        private void SlideTopLeft_Completed(object sender, EventArgs e)
        {
            SlideTopLeftReset.Begin();
        }

        private void SlideTopRight_Completed(object sender, EventArgs e)
        {
            SlideTopRightReset.Begin();
        }

        private void PrevButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(BlockTransitions) return;
			
            SlideRight.Begin();
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(BlockTransitions) return;
			
            SlideLeft.Begin();   
        }

        private Orientation GetDirection(double x, double y)
        {
            return Math.Abs(x) >= Math.Abs(y) ? Orientation.Horizontal : Orientation.Vertical;
        }
    }
}
