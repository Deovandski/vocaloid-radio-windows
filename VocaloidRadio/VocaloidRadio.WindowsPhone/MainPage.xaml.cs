using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Playback;
using Windows.UI.Core;
using System.Diagnostics;

namespace VocaloidRadio
{
    
    public sealed partial class MainPage : Page
    {
        // Variables
        private AutoResetEvent SererInitialized;
        private bool isMyBackgroundTaskRunning = false;
        
        public MainPage()
        {
            this.InitializeComponent();

            SererInitialized = new AutoResetEvent(false);

            // This is a static public property that will allow downstream pages to get 
            // a handle to the MainPage instance in order to call methods that are in this class.
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += OnBackPressed;
        }

        private void OnBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null || !frame.CanGoBack) return;
            e.Handled = true;
            frame.GoBack();
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Adding App suspension handlers here so that we can unsubscripted handlers 
            //that access to BackgroundMediaPlayer events
            App.Current.Suspending += ForegroundApp_Suspending;
            App.Current.Resuming += ForegroundApp_Resuming;
            ApplicationSettingsHelper.SaveSettingsValue(Constants.AppState, Constants.ForegroundAppActive);
        }
        

        // Vocaloid Website Control
        private void imageButton_VocaloidWebsite_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_VocaloidWebsite));
        }
        // Developer Blog Control

        private void imageButton_DeveloperBlog_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_DeveloperBlog));
        }

        // Radio Stream Control
        private void imageButton_RadioStream_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_RadioStreamer_Selection));
        }

        // Request Song Control
        private void imageButton_RequestSong_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_RequestSong));
        }

        // Donate Control
        private void imageButton_Donate_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_Donate));
        }

        // Featured Control
        private void imageButton_FeaturedPages_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_FeaturedPages));
        }

        // Help Control
        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_Help));
        }

        // License Control
        private void licenseButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_LicenseInformation));
        }

        // Network Control
        private void networkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_NetworkInformation));
        }

        // Privacy Control
        private void privacyButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_PrivacyInformation));
        }

        // Technical Control
        private void technicalButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WP_Pages.WP_TechnicalInformation));
        }
        

        #region BackgroundAudioTask Properties

        /// <summary>
        /// Gets the information about background task is running or not by reading the setting saved by background task
        /// </summary>
        private bool IsMyBackgroundTaskRunning
        {
            get
            {
                if (isMyBackgroundTaskRunning)
                    return true;

                object value = ApplicationSettingsHelper.ReadResetSettingsValue(Constants.BackgroundTaskState);
                if (value == null)
                {
                    return false;
                }
                else
                {
                    isMyBackgroundTaskRunning = ((String)value).Equals(Constants.BackgroundTaskRunning);
                    return isMyBackgroundTaskRunning;
                }
            }
        }
        #endregion


        #region Foreground App Lifecycle Handlers
        /// <summary>
        /// Sends message to background informing app has resumed
        /// Subscribe to MediaPlayer events
        /// </summary>
        void ForegroundApp_Resuming(object sender, object e)
        {
            ApplicationSettingsHelper.SaveSettingsValue(Constants.AppState, Constants.ForegroundAppActive);

            // Verify if the task was running before
            if (IsMyBackgroundTaskRunning)
            {
                //if yes, reconnect to media play handlers
                AddMediaPlayerEventHandlers();

                //send message to background task that app is resumed, so it can start sending notifications
                ValueSet messageDictionary = new ValueSet();
                messageDictionary.Add(Constants.AppResumed, DateTime.Now.ToString());
                BackgroundMediaPlayer.SendMessageToBackground(messageDictionary);

                if (BackgroundMediaPlayer.Current.CurrentState == MediaPlayerState.Playing)
                {
                }
                else
                {
                }
            }
            else
            {
            }

        }

        /// <summary>
        /// Send message to Background process that app is to be suspended
        /// Stop clock and slider when suspending
        /// Unsubscribe handlers for MediaPlayer events
        /// </summary>
        void ForegroundApp_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            ValueSet messageDictionary = new ValueSet();
            messageDictionary.Add(Constants.AppSuspended, DateTime.Now.ToString());
            BackgroundMediaPlayer.SendMessageToBackground(messageDictionary);
            RemoveMediaPlayerEventHandlers();
            ApplicationSettingsHelper.SaveSettingsValue(Constants.AppState, Constants.ForegroundAppSuspended);
            deferral.Complete();
        }
        #endregion

        #region Background MediaPlayer Event handlers
        /// <summary>
        /// MediaPlayer state changed event handlers. 
        /// Note that we can subscribe to events even if Media Player is playing media in background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            switch (sender.CurrentState)
            {
                case MediaPlayerState.Playing:
                    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                     }
                        );

                    break;
            }
        }

        /// <summary>
        /// This event fired when a message is recieved from Background Process
        /// </summary>
        async void BackgroundMediaPlayer_MessageReceivedFromBackground(object sender, MediaPlayerDataReceivedEventArgs e)
        {
            foreach (string key in e.Data.Keys)
            {
                switch (key)
                {
                    case Constants.Trackchanged:
                        //When foreground app is active change track based on background message
                        await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                           }
                        );
                        break;
                    case Constants.BackgroundTaskStarted:
                        //Wait for Background Task to be initialized before starting playback
                        Debug.WriteLine("Background Task started");
                        SererInitialized.Set();
                        break;
                }
            }
        }

        #endregion

        #region Button Click Event Handlers

        /// <summary>
        /// If the task is already running, it will just play/pause MediaPlayer Instance
        /// Otherwise, initializes MediaPlayer Handlers and starts playback
        /// track or to pause if we're already playing.
        /// </summary>
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Play button pressed from App");
            if (IsMyBackgroundTaskRunning)
            {
                if (MediaPlayerState.Playing == BackgroundMediaPlayer.Current.CurrentState)
                {
                    BackgroundMediaPlayer.Current.Pause();
                }
                else if (MediaPlayerState.Paused == BackgroundMediaPlayer.Current.CurrentState)
                {
                    BackgroundMediaPlayer.Current.Play();
                }
                else if (MediaPlayerState.Closed == BackgroundMediaPlayer.Current.CurrentState)
                {
                    StartBackgroundAudioTask();
                }
            }
            else
            {
                StartBackgroundAudioTask();
            }
        }

        #endregion Button Click Event Handlers

        #region Media Playback Helper methods
        /// <summary>
        /// Unsubscribes to MediaPlayer events. Should run only on suspend
        /// </summary>
        private void RemoveMediaPlayerEventHandlers()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged -= this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground -= this.BackgroundMediaPlayer_MessageReceivedFromBackground;
        }

        /// <summary>
        /// Subscribes to MediaPlayer events
        /// </summary>
        private void AddMediaPlayerEventHandlers()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged += this.MediaPlayer_CurrentStateChanged;
            BackgroundMediaPlayer.MessageReceivedFromBackground += this.BackgroundMediaPlayer_MessageReceivedFromBackground;
        }

        /// <summary>
        /// Initialize Background Media Player Handlers and starts playback
        /// </summary>
        private void StartBackgroundAudioTask()
        {
            AddMediaPlayerEventHandlers();
            var backgroundtaskinitializationresult = this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                bool result = SererInitialized.WaitOne(2000);
                //Send message to initiate playback
                if (result == true)
                {
                    var message = new ValueSet();
                    message.Add(Constants.StartPlayback, "0");
                    BackgroundMediaPlayer.SendMessageToBackground(message);
                }
                else
                {
                    throw new Exception("Background Audio Task didn't start in expected time");
                }
            }
            );
            backgroundtaskinitializationresult.Completed = new AsyncActionCompletedHandler(BackgroundTaskInitializationCompleted);
        }

        private void BackgroundTaskInitializationCompleted(IAsyncAction action, AsyncStatus status)
        {
            if (status == AsyncStatus.Completed)
            {
                Debug.WriteLine("Background Audio Task initialized");
            }
            else if (status == AsyncStatus.Error)
            {
                Debug.WriteLine("Background Audio Task could not initialized due to an error ::" + action.ErrorCode.ToString());
            }
        }
        #endregion

    }
}
