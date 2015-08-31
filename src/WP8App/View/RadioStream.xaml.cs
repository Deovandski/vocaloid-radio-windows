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
            System.Diagnostics.Debug.WriteLine("Ad Error : ({0}) {1}", e.ErrorCode, e.Error); 
        }
    }
}