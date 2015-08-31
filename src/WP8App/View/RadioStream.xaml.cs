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
using System.Windows.Media;
using Microsoft.Phone.Net.NetworkInformation;

// Developed by Deovandski Skibinski Junior
// Updated on 2/21/2014

namespace Vocaloid_Radio
{

    public partial class RadioStream : PhoneApplicationPage
    {
        public RadioStream()
        {
            InitializeComponent();
            BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);
        }

        // Check any change in playstate | Connected to Event Handler
        void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            Instance_PlayStateChanged();
        }

        // Check any change in playstate | Connected to Page Loaded Event Handler
        void Instance_PlayStateChanged()
        {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                {
                networkStatus.Text = "Network is Available";
                networkStatus.Foreground = new SolidColorBrush(Colors.Green);
              switch (BackgroundAudioPlayer.Instance.PlayerState)
             {
                case PlayState.Playing:
                    if (BackgroundAudioPlayer.Instance.BufferingProgress == 1)
                    {
                        streamStatus.Text = "Streaming Radio || Buffer OK";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    if (BackgroundAudioPlayer.Instance.BufferingProgress == 0)
                    {
                        streamStatus.Text = "Streaming Radio || Slow Buffer";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                    }
                    break;

                case PlayState.Paused:
                    streamStatus.Text = "Radio is on Hold";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    playButton.Content = "Pause";
                    break;

                case PlayState.Stopped:
                    streamStatus.Text = "Radio is Off";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    break;

                case PlayState.BufferingStarted:
                    streamStatus.Text = "Buffering Radio";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Brown);
                    break;

                case PlayState.BufferingStopped:
                    streamStatus.Text = "Radio is not buffering";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Orange);
                    playButton.Content = "Pause";
                    break;

                case PlayState.Shutdown:
                    streamStatus.Text = "Radio is Off";
                    streamStatus.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case PlayState.Error:
                    streamStatus.Text = "An Error Occurred";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Orange);
                    // catch error and try to fix it
                    BackgroundAudioPlayer.Instance.Stop();
                    break;

                case PlayState.Unknown:
                    streamStatus.Text = "Unknown State";
                    streamStatus.Foreground = new SolidColorBrush(Colors.LightGray);
                    // catch any unexpected situation and try to fix it
                    BackgroundAudioPlayer.Instance.Stop();
                    break;
             } // end switch
            } // end if
            else
            {
                networkStatus.Text = "No Connection";
                networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                streamStatus.Text = "Radio is Off";
            }
          } // end Instance_PlayStateChanged method

        // Stops all audio streaming
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.Stop();
            PhoneApplicationService phoneAppService = PhoneApplicationService.Current;
            phoneAppService.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        // Method for activating Radio and Disabling IdleDetectionMode
        private void playRadio()
        {
              if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                networkStatus.Text = "Network is Available";
                networkStatus.Foreground = new SolidColorBrush(Colors.Green);
                BackgroundAudioPlayer.Instance.Play();
                PhoneApplicationService phoneAppService = PhoneApplicationService.Current;
                phoneAppService.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
            else 
            {
                networkStatus.Text = "No Connection";
                networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                streamStatus.Text = "Radio is Off";
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        // Start Media Streaming | Check for conditions such as MediaState.playing and stopped
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Paused) { this.playRadio(); }
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Stopped) { this.playRadio(); }
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Shutdown) { this.playRadio(); }
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Unknown) { this.playRadio(); }

        }
        // Debug Diagnostics for Ad
        private void Ad1_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
           // System.Diagnostics.Debug.WriteLine("Ad Error : ({0}) {1}", e.ErrorCode, e.Error);
        }

        // Troubleshooting Button + make sure to clean BackgroundPlayerAgent so that MediaElement can use the resources.
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.Close();
            NavigationService.Navigate(new Uri("/View/TroubleShootingPage.xaml", UriKind.Relative));
        }

        // Check and update textblock stream Status
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Instance_PlayStateChanged();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml", UriKind.Relative));
        }
    }
}