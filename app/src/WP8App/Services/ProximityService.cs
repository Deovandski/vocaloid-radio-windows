using WPAppStudio.Localization;
using WPAppStudio.Services.Interfaces;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Windows.Networking.Proximity;

namespace WPAppStudio.Services
{
    /// <summary>
    /// Implementation of the Proximity service.
    /// </summary>
    public class ProximityService : IProximityService
    {
        private ProximityDevice _device = null;

        public ProximityService()
        {
            _device = ProximityDevice.GetDefault();
        }

        public bool IsProximityAvailable
        {
            get { return _device != null; }
        }

        /// <summary>
        /// Executes the Proximity service.
        /// </summary>
        /// <param name="title">The title proximityd.</param>
        /// <param name="message">The message proximityd.</param>
        /// <param name="link">The link proximityd.</param>
        /// <param name="type">The image proximityd.</param>
        public void ShareUri(string uriString)
        {
            var cancelPublishUriMessageDialog = GetCancelTapSendMesssage();

            // Make sure NFC is supported
            if (_device != null)
            {
                var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
                long Id = _device.PublishUriMessage(uri, (sender, messageId) => 
                {
                    _device.StopPublishingMessage(messageId);
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        cancelPublishUriMessageDialog.Dismiss();
                    });
                });
                cancelPublishUriMessageDialog.Dismissed += (sender, e) =>
                {
                    _device.StopPublishingMessage(Id);
                };
                cancelPublishUriMessageDialog.Show();
            }
        }

        private CustomMessageBox GetCancelTapSendMesssage()
        {
            var tapSendImage = new Image
            {
                Source = new BitmapImage(new Uri("Images/tap+send.png", UriKind.Relative)),
                Stretch = System.Windows.Media.Stretch.None
            };
            var cancelPublishUriMessageDialog = new CustomMessageBox
            {
                Title = AppResources.TapSend.ToUpper(),
                Message = AppResources.TapSendMessage,
                Content = tapSendImage,
                IsLeftButtonEnabled = true,
                LeftButtonContent = "cancel",
                IsRightButtonEnabled = false,
                IsFullScreen = true
            };
            return cancelPublishUriMessageDialog;
        }
    }
}
