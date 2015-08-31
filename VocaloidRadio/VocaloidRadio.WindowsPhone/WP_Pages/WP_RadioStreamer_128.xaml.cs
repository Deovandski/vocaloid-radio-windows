using System;
using Windows.Media.Playback;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.Media;
using System.Diagnostics;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;

namespace VocaloidRadio.WP_Pages
{

    public sealed partial class WP_RadioStreamer_128 : Page
    {
        // Variables
        private bool InternetAvailable;

        // Constructor
        public WP_RadioStreamer_128()
        {

            this.InitializeComponent();

            // Source for WebControl || XAML
            ImageStreamer.Source = new Uri("ms-appx-web:///HTML code/SongInformation_128.html");
            recentSongs.Source = new Uri("ms-appx-web:///HTML code/PreviousSongs.html");
            
            //Add handlers for MediaPlayer
            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;

            // Back Button controller
            this.NavigationCacheMode = NavigationCacheMode.Required;
            InternetTest.NavigationCompleted += InternetTest_NavigationCompleted;
        }

        // Verify Internet Connectivity
        private void InternetTest_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                InternetAvailable = true;

                networkStatus.Text = "128 Server is On!";
                networkStatus.Foreground = new SolidColorBrush(Colors.Green);
                InternetTest.Stop();

            }
            else
            {
                InternetAvailable = false;
            }

        }

        // Handles any state change within the Background Process. All code Must follow:
        // --
        // -async Method
        //               await + within clause statement.
        // --

        async void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => networkStatus.Text = "Verifying Internet");
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => networkStatus.Foreground = new SolidColorBrush(Colors.Yellow));
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => InternetTest.Navigate(new Uri("http://curiosity.shoutca.st:8019/stream")));

            if (InternetAvailable == true)
            {
                try
                {
                    if (BackgroundMediaPlayer.Current == null) { throw new Exception(); }
                    switch (sender.CurrentState)
                    {
                        case MediaPlayerState.Playing: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                            {
                                networkStatus.Text = "Radio is Streaming!";
                                networkStatus.Foreground = new SolidColorBrush(Colors.Green);
                                try
                                {
                                    if (BackgroundMediaPlayer.Current.BufferingProgress > 0.5 && BackgroundMediaPlayer.Current.BufferingProgress < 0.8)
                                    {
                                        streamStatus.Text = "50%";
                                        streamStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                                    }
                                    if (BackgroundMediaPlayer.Current.BufferingProgress > 0.8)
                                    {
                                        streamStatus.Text = "100%";
                                        streamStatus.Foreground = new SolidColorBrush(Colors.Green);
                                    }
                                    if (BackgroundMediaPlayer.Current.BufferingProgress == 0)
                                    {
                                        streamStatus.Text = "Buffering Issue...";
                                        streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                    }
                                }
                                catch (Exception)
                                {
                                    InternetAvailable = false;
                                }
                            }
                            else
                            {
                                networkStatus.Text = "Error Ocurred! (Ref: WP_101)";
                                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                            }

                        }
                            );
                            break;
                        case MediaPlayerState.Paused: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                            {
                                streamStatus.Text = "Radio is Paused";
                                streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                            }
                            else
                            {
                                networkStatus.Text = "Error Ocurred! (Ref: WP_102)";
                                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                            }
                        }
                            );
                            break;
                        case MediaPlayerState.Buffering: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                            {
                                networkStatus.Text = "Network detected";
                                networkStatus.Foreground = new SolidColorBrush(Colors.GreenYellow);
                                if (BackgroundMediaPlayer.Current.BufferingProgress > 0.5 && BackgroundMediaPlayer.Current.BufferingProgress < 0.8)
                                {
                                    streamStatus.Text = "50%";
                                    streamStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                                }
                                if (BackgroundMediaPlayer.Current.BufferingProgress > 0.8)
                                {
                                    streamStatus.Text = "100%";
                                    streamStatus.Foreground = new SolidColorBrush(Colors.Green);
                                }
                                if (BackgroundMediaPlayer.Current.BufferingProgress == 0)
                                {
                                    streamStatus.Text = "Buffering Issue...";
                                    streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                }
                            }
                            else
                            {
                                networkStatus.Text = "Error Ocurred! (Ref: WP_103)";
                                streamStatus.Text = "...";
                                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                            }
                        }
                            );
                            break;
                        case MediaPlayerState.Stopped: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                            {
                                streamStatus.Text = "Paused";
                                streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                                networkStatus.Text = "Radio is Paused";
                                networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                            }
                            else
                            {
                                networkStatus.Text = "Error Ocurred! (Ref: WP_104)";
                                streamStatus.Text = "...";
                                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                            }
                        }
                            );
                            break;
                        case MediaPlayerState.Opening: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                            {
                                streamStatus.Text = "Waiting for Buffer...";
                                streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                networkStatus.Text = "Radio is Opening";
                                networkStatus.Foreground = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                networkStatus.Text = "Error Ocurred! (Ref: WP_105)";
                                streamStatus.Text = "...";
                                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                            }
                        }
                           );
                            break;
                    }
                }
                catch (Exception)
                {
                    if (BackgroundMediaPlayer.Current == null)
                    {
                        networkStatus.Text = "Internal Error!";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = "(Ref: WP_106)";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);

                    }
                    else
                    {
                        networkStatus.Text = "Error Ocurred! (Ref: WP_107)";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = "...";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }
            else // If Internet Is Not Available
            {
                // Threading not available
            }
        }

        #region XAML_CodeControl


        // Stops all audio streaming
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                BackgroundMediaPlayer.Current.Pause();
                streamStatus.Text = "Radio is Off. Press Play!";
                streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                networkStatus.Text = "...";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                networkStatus.Text = "Fatal Error... (Ref: WP_2)";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        // Method for activating Radio
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            networkStatus.Text = "Verifying Internet";
            networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
            InternetTest.Navigate(new Uri("http://curiosity.shoutca.st:8019/128"));

            if (InternetAvailable == true)
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                {
                    streamStatus.Text = "Please Wait for Radio Setup. 10% Complete";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    networkStatus.Text = "...";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Green);
                    try
                    {
                        streamStatus.Text = "Please Wait for Radio Setup. 30% Complete";

                        if (MediaPlayerState.Playing != BackgroundMediaPlayer.Current.CurrentState)
                        {
                            BackgroundAudioTask.MyBackgroundAudioTask backgroundAccess = new BackgroundAudioTask.MyBackgroundAudioTask();
                            streamStatus.Text = "Please Wait for Radio Setup. 35% Complete";
                            var message = new ValueSet();
                            streamStatus.Text = "Please Wait for Radio Setup. 45% Complete";
                            ApplicationSettingsHelper.SaveSettingsValue(Constants.CurrentTrack, "128");
                            streamStatus.Text = "Please Wait for Radio Setup. 55% Complete";
                            message.Add(Constants.StartPlayback, "0");
                            streamStatus.Text = "Please Wait for Radio Setup. 75% Complete";
                            BackgroundMediaPlayer.SendMessageToBackground(message);
                        }

                        streamStatus.Text = "...";
                    }
                    catch (Exception)
                    {
                        networkStatus.Text = "Stream/BackgroundAudio Error";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                        {
                            streamStatus.Text = "(Ref: WP_3)";
                        }
                        else
                        {
                            try
                            {
                                BackgroundMediaPlayer.Current.Play();
                            }
                            catch (Exception) { }
                        }
                    }
                }
                else
                {
                    networkStatus.Text = "Not Connected to any Network!";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                    streamStatus.Text = "Radio is Off";
                    streamStatus.Foreground = new SolidColorBrush(Colors.Red);
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
        #endregion

        // Invoked when this page is about to be displayed in a Frame.
        // Event data that describes how this page was reached.
        // This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            networkStatus.Text = "Verifying Internet";
            networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
            InternetTest.Navigate(new Uri("http://curiosity.shoutca.st:8019/128"));

            if (InternetAvailable == true)
            {
                try
                {
                    if (BackgroundMediaPlayer.Current == null) { throw new Exception(); }
                    switch (BackgroundMediaPlayer.Current.CurrentState)
                    {
                        case MediaPlayerState.Playing:
                            {
                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                                {
                                    networkStatus.Text = "Radio is Streaming!";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Green);
                                    try
                                    {
                                        if (BackgroundMediaPlayer.Current.BufferingProgress > 0.5 && BackgroundMediaPlayer.Current.BufferingProgress < 0.8)
                                        {
                                            streamStatus.Text = "50%";
                                            streamStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                                        }
                                        if (BackgroundMediaPlayer.Current.BufferingProgress > 0.8)
                                        {
                                            streamStatus.Text = "100%";
                                            streamStatus.Foreground = new SolidColorBrush(Colors.Green);
                                        }
                                        if (BackgroundMediaPlayer.Current.BufferingProgress == 0)
                                        {
                                            streamStatus.Text = "Buffering Issue...";
                                            streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        InternetAvailable = false;
                                    }
                                }
                                else
                                {
                                    networkStatus.Text = "Error Ocurred! (Ref: WP_101)";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                }

                            }
                            break;
                        case MediaPlayerState.Paused:
                            {
                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                                {
                                    streamStatus.Text = "Radio is Paused";
                                    streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                                }
                                else
                                {
                                    networkStatus.Text = "Error Ocurred! (Ref: WP_102)";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                }
                            }
                            break;
                        case MediaPlayerState.Buffering:
                            {
                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                                {
                                    networkStatus.Text = "Network detected";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.GreenYellow);
                                    if (BackgroundMediaPlayer.Current.BufferingProgress > 0.5 && BackgroundMediaPlayer.Current.BufferingProgress < 0.8)
                                    {
                                        streamStatus.Text = "50%";
                                        streamStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                                    }
                                    if (BackgroundMediaPlayer.Current.BufferingProgress > 0.8)
                                    {
                                        streamStatus.Text = "100%";
                                        streamStatus.Foreground = new SolidColorBrush(Colors.Green);
                                    }
                                    if (BackgroundMediaPlayer.Current.BufferingProgress == 0)
                                    {
                                        streamStatus.Text = "Buffering Issue...";
                                        streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                    }
                                }
                                else
                                {
                                    networkStatus.Text = "Error Ocurred! (Ref: WP_103)";
                                    streamStatus.Text = "...";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                    streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                }
                            }
                            break;
                        case MediaPlayerState.Stopped:
                            {
                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                                {
                                    streamStatus.Text = "Paused";
                                    streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                                    networkStatus.Text = "Radio is Paused";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                                }
                                else
                                {
                                    networkStatus.Text = "Error Ocurred! (Ref: WP_104)";
                                    streamStatus.Text = "...";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                    streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                }
                            }
                            break;
                        case MediaPlayerState.Opening:
                            {
                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                                {
                                    streamStatus.Text = "Waiting for Buffer...";
                                    streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                    networkStatus.Text = "Radio is Opening";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.White);
                                }
                                else
                                {
                                    networkStatus.Text = "Error Ocurred! (Ref: WP_105)";
                                    streamStatus.Text = "...";
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                    streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                }
                            }
                            break;
                    }
                }
                catch (Exception)
                {
                    if (BackgroundMediaPlayer.Current == null)
                    {
                        networkStatus.Text = "Internal Error!";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = "(Ref: WP_106)";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);

                    }
                    else
                    {
                        networkStatus.Text = "Error Ocurred! (Ref: WP_107)";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = "...";
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }
            else // If Internet Is Not Available
            {
                // Threading not available
            }
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_RadioStreamer_Selection));
        }
    }
}

