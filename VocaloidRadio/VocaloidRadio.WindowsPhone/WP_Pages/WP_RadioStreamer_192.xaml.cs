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
using Windows.Foundation.Collections;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VocaloidRadio.WP_Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WP_RadioStreamer_192 : Page
    {   
        // Constructor
        public WP_RadioStreamer_192()
        {
            this.InitializeComponent();

            // Source for WebControl || XAML
            ImageStreamer.Source = new Uri("ms-appx-web:///HTML code/SongInformation_128.html");
            recentSongs.Source = new Uri("ms-appx-web:///HTML code/PreviousSongs.html");

            //Add handlers for MediaPlayer
            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;

            // Back Button controller
            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        // Handles any state change within the Background Process. All code Must follow:
        // --
        // -async Method
        //               await + within clause statement.
        // --

        async void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            switch (sender.CurrentState)
            {
                case MediaPlayerState.Playing: await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
                    {
                        networkStatus.Text = "Radio is Streaming!";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Green);
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
                        streamStatus.Text = "Background Task Awaiting";
                        streamStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
                        networkStatus.Text = "Radio is Paused";
                        networkStatus.Foreground = new SolidColorBrush(Colors.DarkGray);
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
                        networkStatus.Text = "Radio is Streaming!";
                        networkStatus.Foreground = new SolidColorBrush(Colors.Green);
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
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
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
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
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
                        networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
                   );
                    break;
            }
        }

        #region XAML_CodeControl


        // Stops all audio streaming
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                BackgroundMediaPlayer.Current.Pause();
                streamStatus.Text = "100% Complete";
                streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
            }
            catch (Exception)
            {
                networkStatus.Text = "Fatal Error... Please restart the App. (Ref: WP_2)";
                networkStatus.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        // Method for activating Radio and Disabling IdleDetectionMode.
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == true)
            {
                streamStatus.Text = "Please Wait for Radio Setup. 10% Complete";
                streamStatus.Foreground = new SolidColorBrush(Colors.Gray);
                networkStatus.Text = "Network is Available";
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
                        ApplicationSettingsHelper.SaveSettingsValue(Constants.CurrentTrack, "192");
                        streamStatus.Text = "Please Wait for Radio Setup. 55% Complete";
                        message.Add(Constants.StartPlayback, "0");
                        streamStatus.Text = "Please Wait for Radio Setup. 75% Complete";
                        BackgroundMediaPlayer.SendMessageToBackground(message);
                    }

                    streamStatus.Text = "100% Complete";
                }
                catch (Exception)
                {
                    networkStatus.Text = "Stream/BackgroundAudio Error";
                    networkStatus.Foreground = new SolidColorBrush(Colors.Red);
                    if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                    {
                        streamStatus.Text = "Somwthing Wrong Happened... (Ref: WP_3)";
                    }
                    else
                    {
                        BackgroundMediaPlayer.Current.Play();
                    }
                }
            }
            else
            {
                networkStatus.Text = "No Connection";
                networkStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                streamStatus.Text = "Radio is Off";
                streamStatus.Foreground = new SolidColorBrush(Colors.Red);
            }

        }
        #endregion

        // Invoked when this page is about to be displayed in a Frame.
        // Event data that describes how this page was reached.
        // This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        // Back Button Event Handler
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            // Go Back to Radio Selection
            Frame.Navigate(typeof(WP_Pages.WP_RadioStreamer_Selection));
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_RadioStreamer_Selection));
        }
       
    }
}
