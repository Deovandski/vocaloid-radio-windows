using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Advertising;
using Microsoft.Phone.BackgroundAudio;

namespace Vocaloid_Radio
{
    public partial class RadioStream : PhoneApplicationPage
    {
       
        public RadioStream()
        {
            InitializeComponent();
            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);
        }

        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:

                    break;

                case PlayState.Stopped:

                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RadioStream.xaml", UriKind.Relative));
           
        }

        // Stops all audio streaming

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.Stop();
        }

        // Start Media Streaming

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.Play();
            
        }

        // Debug Diagnostics for Ad
        private void Ad1_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
           // System.Diagnostics.Debug.WriteLine("Ad Error : ({0}) {1}", e.ErrorCode, e.Error);
            TextBox.Text = e.Error.ToString();
        }

        private void ImageStreamer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void ImageStreamer_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void ImageStreamer_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void ImageStreamer_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void ImageStreamer_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void ImageStreamer_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void ImageStreamer_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void ImageStreamer_Navigating(object sender, NavigatingEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void bt1_GotFocus(object sender, RoutedEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void bt1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }

        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            // Hold user interaction. For Design porpuse to not allow the user to scroll down.
        }
    }
}