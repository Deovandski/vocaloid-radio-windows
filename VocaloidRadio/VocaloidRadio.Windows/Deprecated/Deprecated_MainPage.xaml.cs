using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using System.Diagnostics;
using Windows.Media;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VocaloidRadio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Variables
        SystemMediaTransportControls mediaControl;
        private String MEBufferTime = "";

        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            // Register for the window resize event
            Window.Current.SizeChanged += WindowSizeChanged;

            // Hook up app to system transport controls.
            mediaControl = SystemMediaTransportControls.GetForCurrentView();
            mediaControl.ButtonPressed += medialControl_ButtonPressed;

            // Register to handle the following system transpot control buttons.
            mediaControl.IsPlayEnabled = true;
            mediaControl.IsPauseEnabled = true;

            // Source for WebControl || XAML
            songInformation.Source = new Uri("ms-appx-web:///HTML code/SongInformation.html");
            previousSongInformation.Source = new Uri("ms-appx-web:///HTML code/PreviousSongs_Larger.html");
          
        }

        // Change Window size based on Window Width
        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // var height = Window.Current.Bounds.Height;
            var width = Window.Current.Bounds.Width;
            Debug.WriteLine("Main Page: WindowSizeChanged... width is: " + width.ToString());

            if (width <= 600)
            {
                VocaloidRadioWeb.Visibility = Visibility.Collapsed;
                VocaloidRadioAppWeb.Visibility = Visibility.Collapsed;
                previousSongInformation.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                PreviousSongText.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                RightSubGrid.Width = new GridLength(0);
                
            }
            if (width > 650)
            {
                VocaloidRadioWeb.Visibility = Visibility.Visible;
                VocaloidRadioAppWeb.Visibility = Visibility.Visible;
                previousSongInformation.Visibility = Windows.UI.Xaml.Visibility.Visible;
                PreviousSongText.Visibility = Windows.UI.Xaml.Visibility.Visible;
                MainGrid.ColumnDefinitions[1].Width = new GridLength(390);
            }
        }

        // Start Playback
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                mediaplayer.Play();
                networkStatus.Text = "Network is Available";
                networkStatus.Foreground = new SolidColorBrush(Colors.DarkGreen);
                Debug.WriteLine("Player Request to Play... Result: " + mediaplayer.CurrentState.ToString());
            }
            else
            {
                networkStatus.Text = "Error Ocurred! Check your connection! (Ref: WP_101)";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        // Stop Playback
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                mediaplayer.Stop();
                networkStatus.Text = "Player Stopped";
                networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                Debug.WriteLine("Player Request to stop... Result: " + mediaplayer.CurrentState.ToString());

            }
            catch (Exception)
            {
                networkStatus.Text = "Error Ocurred!  (Ref: WP_101)";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);

            }

        }

        // System Media Transport Control
        private void medialControl_ButtonPressed(SystemMediaTransportControls sender,
    SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    PlayMedia();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    PauseMedia();
                    break;
                default:
                    break;
            }
        }

        async void PlayMedia()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    mediaplayer.Play();
                    Debug.WriteLine("Player Request to Play... Result: " + mediaplayer.CurrentState.ToString());
                }
                catch (Exception)
                {
                    networkStatus.Text = "Error Ocurred! Check Your Connection  (Ref: WP_102)";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                
                }
            });
        }

        async void PauseMedia()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    mediaplayer.Pause();     
                    Debug.WriteLine("Player Request to Play... Result: " + mediaplayer.CurrentState.ToString());
                }
                catch (Exception)
                {
                    networkStatus.Text = "Error Ocurred! Check Your Connection  (Ref: WP_102)";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                }
            });
        }

        // Media Player State Changed Handler
        private void mediaplayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (mediaplayer.CurrentState)
            {
                case MediaElementState.Playing:
                    networkStatus.Text = "Player is Normal";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Green);     
                    MEBufferTime = mediaplayer.BufferingProgress.ToString();
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                    {
                        streamStatus.Text = "M.E. is On || Buffer is " + MEBufferTime;
                        streamStatus.Foreground = new SolidColorBrush(Colors.Green);

                    }
                    else
                    {
                        streamStatus.Text = "Check your Connection";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                        networkStatus.Text = "Player had an issue";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    mediaControl.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                    {
                        streamStatus.Text = "Stream Paused";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                        networkStatus.Text = "Player Paused";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                    else
                    {
                        networkStatus.Text = "Issue (Ref: WP_103)";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = "Stream Paused";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                    mediaControl.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaElementState.Stopped:
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                    {
                        streamStatus.Text = "Stream Stopped";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                        networkStatus.Text = "Player Stopped";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                    else
                    {
                        networkStatus.Text = "Issue (Ref: WP_104)";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = "Stream Stopped";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    }
                    mediaControl.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                case MediaElementState.Closed:
                    streamStatus.Text = "Please Restart the App...";
                    streamStatus.Foreground = new SolidColorBrush(Colors.LightGray);
                    networkStatus.Text = "Player is Closed...";
                    networkStatus.Foreground = new SolidColorBrush(Colors.LightGray);
                    mediaControl.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                default:
                    break;
            }
        }
    }
}
