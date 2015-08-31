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
        private bool InternetAvailable;

        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            // Register for the window resize event
            Window.Current.SizeChanged += WindowSizeChanged;

            // Source for WebControl || XAML
            songInformation.Source = new Uri("ms-appx-web:///HTML code/SongInformation.html");
            previousSongInformation.Source = new Uri("ms-appx-web:///HTML code/PreviousSongs_Larger.html");
            MenuWebView.Navigate(new Uri("http://vocaloidradio.com/"));
            currentPage.Text = "Vocaloid Radio";

            // Hook up app to system transport controls.
            mediaControl = SystemMediaTransportControls.GetForCurrentView();
            mediaControl.ButtonPressed += medialControl_ButtonPressed;

            // Register to handle the following system transpot control buttons.
            mediaControl.IsPlayEnabled = true;
            mediaControl.IsPauseEnabled = true;
            InternetTest.NavigationCompleted += InternetTest_NavigationCompleted;
            
        }

        // Internet Test
        private void InternetTest_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            Debug.WriteLine("Web View Navigation Completed");
            if (args.IsSuccess)
            {
                try
                {
                    InternetAvailable = true;

                    networkStatus.Text = "Server is On!";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Green);

                    if (streamStatus.Text == "Check your Connection! (Ref: WP_4)" && mediaplayer.CurrentState != MediaElementState.Playing)
                    {
                        streamStatus.Text = "Press Play!";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    InternetTest.Stop();
                }
                catch (Exception)
                {
                    networkStatus.Text = "I.T Error 1";
                    streamStatus.Text = "Restart the App and notify the Developer";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                    streamStatus.Foreground = new SolidColorBrush(Colors.Red);

                }
            }
            else
            {
                InternetAvailable = false;
                networkStatus.Text = "No internet!";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                streamStatus.Text = "Check your Connection! (Ref: WP_15)";
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }

        }

        // Change Window size based on Window Width
        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // var height = Window.Current.Bounds.Height;
            var width = Window.Current.Bounds.Width;
            Debug.WriteLine("Main Page: WindowSizeChanged... width is: " + width.ToString());

            if (width <= 1000)
            {
                leftSubGrid.Visibility = Visibility.Collapsed;
                LeftColumn.Width = new GridLength(0);
            }
            if (width > 1000)
            {
                leftSubGrid.Visibility = Visibility.Visible;
                mainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
            }

        }

        // Start Playback
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            networkStatus.Text = "Verifying Internet";
            networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
            InternetTest.Navigate(new Uri("http://curiosity.shoutca.st:8019/stream"));
            
            if (InternetAvailable == true)
            {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                if (mediaplayer.CurrentState == MediaElementState.Closed)
                {
                    streamStatus.Text = "Please restart your App. Media Player error 1";
                    streamStatus.Foreground = new SolidColorBrush(Colors.OrangeRed);
                    Debug.WriteLine("Play Request. Result: Media Player is Closed");
                }
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
            else // If Internet Is Not Available
            {
                networkStatus.Text = "Could not connect to Stream...";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                streamStatus.Text = "Check your Connection! (Ref: WP_4)";
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);
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
                    try
                    {
                        PlayMedia();
                    }
                    catch (Exception) 
                    {
                        networkStatus.Text = "S.M.T.C. Error";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    try
                    {
                        PauseMedia();
                    }
                    catch (Exception)
                    {
                        networkStatus.Text = "S.M.T.C. Error";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
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
                    networkStatus.Text = "P.M. Error 102";
                    streamStatus.Text = "Restart the App and notify the Developer";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                    streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                }
            });
        }

        async void PauseMedia()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    mediaplayer.Stop();    
                    Debug.WriteLine("Player Request to Play... Result: " + mediaplayer.CurrentState.ToString());
                }
                catch (Exception)
                {
                    networkStatus.Text = "P.M. Error 103";
                    streamStatus.Text = "Restart the App and notify the Developer";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                    streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                }
            });
        }

        // Media Player State Changed Handler
        private void mediaplayer_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            switch (mediaplayer.CurrentState)
            {
                case MediaElementState.Opening:
                    streamStatus.Text = "Player Opening... If Play button does not work restart your App";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Orange);
                    Debug.WriteLine("Play Request. Result: Media Player Stuck on Opening");
                    break;
                case MediaElementState.Playing:
                    try
                    {
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
                            networkStatus.Text = "P.M. Error 104";
                            streamStatus.Text = "Restart the App and notify the Developer";
                            networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                            streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        mediaControl.PlaybackStatus = MediaPlaybackStatus.Playing;
                    }
                    catch (Exception) 
                    {
                        networkStatus.Text = "Internal Error 1";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    break;

                case MediaElementState.Paused:
                    try { 
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                    {
                        streamStatus.Text = "Stream Paused";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                        networkStatus.Text = "Player Paused";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                        Debug.WriteLine("Player Paused. Buffer: " + mediaplayer.BufferingProgress.ToString());
                    }
                    else
                    {
                        networkStatus.Text = "P.M. Error 105";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    mediaControl.PlaybackStatus = MediaPlaybackStatus.Paused;
                    }
                    catch (Exception)
                    {
                        networkStatus.Text = "Internal Error 2";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    break;

                case MediaElementState.Stopped:
                    try
                    {
                        if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                        {
                            streamStatus.Text = "Stream Stopped";
                            streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                            networkStatus.Text = "Player Stopped";
                            networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                        }
                        else
                        {
                            networkStatus.Text = "P.M. Error 105";
                            streamStatus.Text = "Restart the App and notify the Developer";
                            networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                            streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        mediaControl.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    }
                    catch (Exception) 
                    {
                        networkStatus.Text = "Internal Error 3";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    break;

                case MediaElementState.Closed:
                    try
                    {
                        streamStatus.Text = "--";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                        networkStatus.Text = "Player is Closed...";
                        networkStatus.Foreground = new SolidColorBrush(Colors.LightGray);
                        mediaControl.PlaybackStatus = MediaPlaybackStatus.Closed;
                    }
                    catch (Exception) 
                    {
                        networkStatus.Text = "Internal Error 4";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    break;

                default:
                    try
                    {
                        networkStatus.Text = "App Analyser Error...";
                        streamStatus.Text = "App is starting or an error ocurred!";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);

                    }
                    catch (Exception) 
                    {
                        networkStatus.Text = "Internal Error 5";
                        streamStatus.Text = "Restart the App and notify the Developer";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    break;
            }
        }

        private void MenuWebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Web View Navigation Error... Result: " + e.WebErrorStatus.ToString());
                networkStatus.Text = "W.V. Error 101";
                streamStatus.Text = "Restart the App and notify the Developer";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                networkStatus.Text = "Internal Error 6";
                streamStatus.Text = "Restart the App and notify the Developer";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void MenuWebView_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            try{
            Debug.WriteLine("Web View Unviewable Content Error... Result: " + args.Uri.ToString());
            networkStatus.Text = "W.V. Error 102";
            streamStatus.Text = "Restart the App and notify the Developer";
            networkStatus.Foreground = new SolidColorBrush(Colors.Red);
            streamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                networkStatus.Text = "Internal Error 7";
                streamStatus.Text = "Restart the App and notify the Developer";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        ///All code Below controls the Left SubGrid///
        
        #region LeftColumnControl

        // License Control
        private void licenseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradioapp.blogspot.com/2014/05/vocaloid-radio-legal-information.html"));
                currentPage.Text = "Legal Info";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }


        // Network Control
        private void networkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradioapp.blogspot.com/2014/05/vocaloid-radio-network-information.html"));
                currentPage.Text = "Network Info";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        // Privacy Control
        private void privacyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradioapp.blogspot.com/2014/05/all-data-collected-in-this-app-is.html"));
                currentPage.Text = "Privacy Info";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        // Technical Control
        private void technicalButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradioapp.blogspot.com/2014/06/vocaloid-radio-windows-app-technical.html"));
                currentPage.Text = "Technical Info";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        // Help Control
        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Stop();

                MenuWebView.Navigate(new Uri("http://vocaloidradioapp.blogspot.com/2014/06/vocaloid-radio-windows-app-help.html"));
                currentPage.Text = "App Help";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        // Donate Control
        private void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradio.com/donate/"));
                currentPage.Text = "Donate";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        // Featured Control
        private void FeaturedPageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradioapp.blogspot.com/2014/06/vocaloid-radio-featured-pages.html"));
                currentPage.Text = "Featured Pages";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        // Developer Blog Control
        private void AppBlogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradioapp.blogspot.com/p/windows-version.html"));
                currentPage.Text = "App Blog";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        // Vocaloid Website Control
        private void WebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuWebView.Navigate(new Uri("http://vocaloidradio.com/"));
                currentPage.Text = "Vocaloid Radio";
            }
            catch (Exception)
            {
                currentPage.Text = "Fatal Web View Error";
            }
        }

        #endregion

        private void InternetTest_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            networkStatus.Text = "Verifying Internet";
            networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
            streamStatus.Text = "Please wait 5 seconds!";
            streamStatus.Foreground = new SolidColorBrush(Colors.Yellow);
        }
    }
}
