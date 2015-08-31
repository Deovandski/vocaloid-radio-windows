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
            RSSFeed.Source = new Uri("ms-appx-web:///HTML code/RSSFeed.html");
           
            //Add handlers for MediaPlayer
            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;

            // Back Button controller
            this.NavigationCacheMode = NavigationCacheMode.Required;
            InternetTest.NavigationCompleted += InternetTest_NavigationCompleted;

        }

        private void InternetTest_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (args.IsSuccess)
            {
                InternetAvailable = true;

                networkStatus.Text = loader.GetString("128_CS_Text1");
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

        // Handles When the Background Player State Changes

        async void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => networkStatus.Text = loader.GetString("CS_Text2"));
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => networkStatus.Foreground = new SolidColorBrush(Colors.Yellow));
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => InternetTest.Navigate(new Uri("http://curiosity.shoutca.st:8019/128")));

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
                                networkStatus.Text = loader.GetString("CS_Text3");
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
                                        streamStatus.Text = loader.GetString("CS_Text4");
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
                                networkStatus.Text = loader.GetString("CS_Text5");
                                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                            }

                        }
                            );
                            break;
                        case MediaPlayerState.Paused: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                            {
                                streamStatus.Text = loader.GetString("CS_Text6");
                                streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                            }
                            else
                            {
                                networkStatus.Text = loader.GetString("CS_Text7");
                                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                            }
                        }
                            );
                            break;
                        case MediaPlayerState.Buffering: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                            {
                                networkStatus.Text = loader.GetString("CS_Text8");
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
                                    streamStatus.Text = loader.GetString("CS_Text4");
                                    streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                }
                            }
                            else
                            {
                                networkStatus.Text = loader.GetString("CS_Text9");
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
                                streamStatus.Text = loader.GetString("CS_Text10");
                                streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                                networkStatus.Text = loader.GetString("CS_Text6");
                                networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                            }
                            else
                            {
                                networkStatus.Text = loader.GetString("CS_Text11");
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
                                streamStatus.Text = loader.GetString("CS_Text14");
                                streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                networkStatus.Text = loader.GetString("CS_Text15");
                                networkStatus.Foreground = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                networkStatus.Text = loader.GetString("CS_Text12");
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
                        networkStatus.Text = loader.GetString("CS_Text13");
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = loader.GetString("CS_Text16");
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);

                    }
                    else
                    {
                        networkStatus.Text = loader.GetString("CS_Text17");
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
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            try
            {
                BackgroundMediaPlayer.Current.Pause();
                streamStatus.Text = loader.GetString("CS_Text18");
                streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                networkStatus.Text = "...";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
            catch (Exception)
            {
                networkStatus.Text = loader.GetString("CS_Text19");
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        // Method for activating Radio
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            networkStatus.Text = loader.GetString("CS_Text2");
            networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
            InternetTest.Navigate(new Uri("http://curiosity.shoutca.st:8019/128"));

            if (InternetAvailable == true)
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                {
                    streamStatus.Text = loader.GetString("CS_Text20");
                    streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                    networkStatus.Text = "...";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Green);
                    try
                    {
                        streamStatus.Text = loader.GetString("CS_Text21");

                        if (MediaPlayerState.Playing != BackgroundMediaPlayer.Current.CurrentState)
                        {
                            BackgroundAudioTask.MyBackgroundAudioTask backgroundAccess = new BackgroundAudioTask.MyBackgroundAudioTask();
                            streamStatus.Text = loader.GetString("CS_Text22");
                            var message = new ValueSet();
                            streamStatus.Text = loader.GetString("CS_Text23");
                            ApplicationSettingsHelper.SaveSettingsValue(Constants.CurrentTrack, "128");
                            streamStatus.Text = loader.GetString("CS_Text24");
                            message.Add(Constants.StartPlayback, "0");
                            streamStatus.Text = loader.GetString("CS_Text25");
                            BackgroundMediaPlayer.SendMessageToBackground(message);
                        }

                        streamStatus.Text = "...";
                    }
                    catch (Exception)
                    {
                        networkStatus.Text = loader.GetString("CS_Text26");
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                        {
                            streamStatus.Text = loader.GetString("CS_Text27");
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
                    networkStatus.Text = loader.GetString("CS_Text28");
                    networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                    streamStatus.Text = loader.GetString("CS_Text29");
                    streamStatus.Foreground = new SolidColorBrush(Colors.Red);
                }

            }
            else // If Internet Is Not Available
            {
                networkStatus.Text = loader.GetString("CS_Text30");
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                streamStatus.Text = loader.GetString("CS_Text31");
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);

            }
        }
        #endregion

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            networkStatus.Text = loader.GetString("CS_Text2");
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
                                    networkStatus.Text = loader.GetString("CS_Text3");
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
                                            streamStatus.Text = loader.GetString("CS_Text4");
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
                                    networkStatus.Text = loader.GetString("CS_Text5");
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                }

                            }
                            break;
                        case MediaPlayerState.Paused:
                            {
                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                                {
                                    streamStatus.Text = loader.GetString("CS_Text6");
                                    streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                                }
                                else
                                {
                                    networkStatus.Text = loader.GetString("CS_Text7");
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                                }
                            }
                            break;
                        case MediaPlayerState.Buffering:
                            {
                                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                                {
                                    networkStatus.Text = loader.GetString("CS_Text8");
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
                                        streamStatus.Text = loader.GetString("CS_Text4");
                                        streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                    }
                                }
                                else
                                {
                                    networkStatus.Text = loader.GetString("CS_Text9");
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
                                    streamStatus.Text = loader.GetString("CS_Text10");
                                    streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                                    networkStatus.Text = loader.GetString("CS_Text6");
                                    networkStatus.Foreground = new SolidColorBrush(Colors.Gray);
                                }
                                else
                                {
                                    networkStatus.Text = loader.GetString("CS_Text11");
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
                                    streamStatus.Text = loader.GetString("CS_Text14");
                                    streamStatus.Foreground = new SolidColorBrush(Colors.YellowGreen);
                                    networkStatus.Text = loader.GetString("CS_Text15");
                                    networkStatus.Foreground = new SolidColorBrush(Colors.White);
                                }
                                else
                                {
                                    networkStatus.Text = loader.GetString("CS_Text12");
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
                        networkStatus.Text = loader.GetString("CS_Text13");
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                        streamStatus.Text = loader.GetString("CS_Text16");
                        streamStatus.Foreground = new SolidColorBrush(Colors.Red);

                    }
                    else
                    {
                        networkStatus.Text = loader.GetString("CS_Text17");
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

    }
}
