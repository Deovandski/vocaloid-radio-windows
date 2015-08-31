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
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    streamStatus.Text = "Playing Radio | Buffer is " + BackgroundAudioPlayer.Instance.BufferingProgress.ToString() + "s";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Green);
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

            }
        }

        // Check any change in playstate | Connected to Page Loaded Event Handler
        void Instance_PlayStateChanged()
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                  //  if (streamStatus.Text == "Playing Radio | Buffer is 0s")
                  //  {
                   //     streamStatus.Text = "Buffering Issues...";
                  //      streamStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                  //  }
                    if (BackgroundAudioPlayer.Instance.BufferingProgress == 1)
                    {
                        streamStatus.Text = "Playing Radio | Buffer is " + BackgroundAudioPlayer.Instance.BufferingProgress.ToString() + "s";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Green);
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

            }
        }

        // Stops all audio streaming

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BackgroundAudioPlayer.Instance.Stop();
        }

        // Start Media Streaming | Check for conditions such as MediaState.playing and stopped

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Paused) { BackgroundAudioPlayer.Instance.Play(); }
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Stopped) { BackgroundAudioPlayer.Instance.Play(); }
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Shutdown) { BackgroundAudioPlayer.Instance.Play();  }
            if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Unknown) { BackgroundAudioPlayer.Instance.Play(); }
            
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
    }
}