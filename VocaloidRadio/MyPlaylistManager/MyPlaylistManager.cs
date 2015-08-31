/*
 * (c) Copyright Microsoft Corporation.
This source is subject to the Microsoft Public License (Ms-PL).
All other rights reserved.
*/

using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Media.Playback;

/*
 * This file implements a sample implementation of playlist management.
 * Make sure to note that objects of this class are to be initialized and 
 * run in background context. Using these in foreground context will lead 
 * music to stop or MediaPlayer to throw exception once the foreground app 
 * is suspended. 
 */
namespace BackgroundAudio.MyPlaylistManager
{
    /// <summary>
    /// Manage playlist information. For simplicity of this sample, we allow only one playlist
    /// </summary>
    public sealed class MyPlaylistManager
    {
        #region Private members
        private static MyPlaylist instance; 
        #endregion

        #region Playlist management methods/properties
        public MyPlaylist Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new MyPlaylist();
                }
                return instance;
            }
        }

        /// <summary>
        /// Clears playlist for re-initialization
        /// </summary>
        public void ClearPlaylist()
        {
            instance = null;
        } 
        #endregion
    }

    /// <summary>
    /// Implement a playlist of tracks. 
    /// If instantiated in background task, it will keep on playing once app is suspended
    /// </summary>
    public sealed class MyPlaylist
    {
        #region Private members
        static String[] tracks = { "http://curiosity.shoutca.st:8019/128", 
                                   "http://curiosity.shoutca.st:8019/stream"
                                };
        int CurrentTrackId = -1;
        private MediaPlayer mediaPlayer;
        internal MyPlaylist()
        {
            mediaPlayer = BackgroundMediaPlayer.Current;
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.CurrentStateChanged += mediaPlayer_CurrentStateChanged;
            mediaPlayer.MediaFailed += mediaPlayer_MediaFailed;
        }

        #endregion

        #region Public properties, events and handlers
        /// <summary>
        /// Get the current track name
        /// </summary>
        public string CurrentTrackName
        {
            get
            {
                if (CurrentTrackId == -1)
                {
                    return String.Empty;
                }
                if (CurrentTrackId < tracks.Length)
                {
                    string fullUrl = tracks[CurrentTrackId];

                    if (fullUrl == "http://curiosity.shoutca.st:8019/128") 
                    {
                        fullUrl = "128"; 
                        return fullUrl;
                    }
                    else
                    {
                        fullUrl = "192";
                        return fullUrl;
                    }
                }
                else
                    throw new ArgumentOutOfRangeException("Track Id is higher than total number of tracks");
            }
        }
        /// <summary>
        /// Invoked when the media player is ready to move to next track
        /// </summary>
        public event TypedEventHandler<MyPlaylist, object> TrackChanged;
        #endregion

        #region MediaPlayer Handlers
        /// <summary>
        /// Handler for state changed event of Media Player
        /// </summary>
        void mediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            if (sender.CurrentState == MediaPlayerState.Playing)
            {
                sender.Volume = 1.0;
                sender.PlaybackMediaMarkers.Clear();
            }
        }

        /// <summary>
        /// Fired when MediaPlayer is ready to play the track
        /// </summary>
        void MediaPlayer_MediaOpened(MediaPlayer sender, object args)
        {
            // wait for media to be ready
            sender.Play();
            Debug.WriteLine("Track Change! New Track is: " + this.CurrentTrackName);
            TrackChanged.Invoke(this, "Vocaloid Radio");
        }

        private void mediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            Debug.WriteLine("Failed with error code " + args.ExtendedErrorCode.ToString());
        }
        #endregion

        #region Playlist command handlers

        /// <summary>
        /// Starts a given track by finding its name
        /// </summary>
        public void StartSelectedStream(string stream)
        {
            if (stream == "128")
            {

                string source = tracks[0];
                CurrentTrackId = 0;
                mediaPlayer.AutoPlay = false;
                mediaPlayer.SetUriSource(new Uri(source));
            }
            else
            {
                string source = tracks[1];
                CurrentTrackId = 1;
                mediaPlayer.AutoPlay = false;
                mediaPlayer.SetUriSource(new Uri(source));
            }
        }
        
        #endregion
        
    }
}
