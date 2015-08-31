using WPAppStudio.Entities.Base;
using WPAppStudio.Localization;
using WPAppStudio.Services.Interfaces;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Resources;

namespace WPAppStudio.Services
{
    /// <summary>
    /// Implementation of the Share service.
    /// </summary>
    public class ShareService : IShareService
    {
		private IProximityService _proximityService;

		private enum ShareTypeEnum
        { 
            TapSend,
            ShareLink,
            ShareImage,
            ShareStatus,
            ShareByEmail
        }

		public const string IMAGES = "Images/";

        public ShareService(IProximityService proximityService)
        {
            _proximityService = proximityService;
        }

        /// <summary>
        /// Executes the Share service.
        /// </summary>
        /// <param name="title">The title shared.</param>
        /// <param name="message">The message shared.</param>
        /// <param name="link">The link shared.</param>
		/// <param name="type">The image shared.</param>
        public void Share(string title, string message, string link = "", string image = "")
        {
            var availableShareTypes = GetAvailableShareTypes(title, message, link, image);
            OpenShareTypeSelector(availableShareTypes, title, message, link, image);
        }

        /// <summary>
        /// Check if current app exist in marketplace.
        /// </summary>
        public bool AppExistInMarketPlace()
        {
            var result = false;
            try
            {
                var link = Windows.ApplicationModel.Store.CurrentApp.LinkUri;
                WebClient client = new WebClient();
                client.OpenReadCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        result = true;
                    }
                };
                client.OpenReadAsync(link);
            }
            catch { }

            return result;
        }
        
        private List<ShareTypeEnum> GetAvailableShareTypes(string title, string message, string link = "", string image = "")
        {
            var result = new List<ShareTypeEnum>();

            if (Uri.IsWellFormedUriString(link, UriKind.Absolute)
                && _proximityService.IsProximityAvailable)
                result.Add(ShareTypeEnum.TapSend);

            if (!string.IsNullOrEmpty(title)
                && !string.IsNullOrEmpty(message)
                && Uri.IsWellFormedUriString(link, UriKind.Absolute))
                result.Add(ShareTypeEnum.ShareLink);

            if (!string.IsNullOrEmpty(image)
                && IsValidMedia(image))
                result.Add(ShareTypeEnum.ShareImage);

            if (!string.IsNullOrEmpty(title)
                || !string.IsNullOrEmpty(message))
                result.Add(ShareTypeEnum.ShareStatus);

            if (!string.IsNullOrEmpty(title)
                && !string.IsNullOrEmpty(message))
                result.Add(ShareTypeEnum.ShareByEmail);

            return result;
        }

        private void OpenShareTypeSelector(List<ShareTypeEnum> availableShareTypes, string title, string message, string link = "", string image = "")
        {
            var listSelector = new System.Windows.Controls.ListBox
            {
                ItemsSource = availableShareTypes,
                ItemTemplate = App.Current.Resources["SelectorItemTemplate"] as DataTemplate
            };

            listSelector.SelectionChanged += (sender, e) =>
            {
                var selectedItem = (ShareTypeEnum)e.AddedItems[0];
                switch (selectedItem)
                {
                    case ShareTypeEnum.TapSend:
                        _proximityService.ShareUri(link);
                        break;
                    case ShareTypeEnum.ShareLink:
                        ShareLink(title, message, link);
                        break;
                    case ShareTypeEnum.ShareImage:
                        ShareMedia(image);
                        break;
                    case ShareTypeEnum.ShareStatus:
                        ShareStatus(string.IsNullOrEmpty(message) ? title : message);
                        break;
                    case ShareTypeEnum.ShareByEmail:
                        ShareByEmail(title, message, link);
                        break;
                    default:
                        break;
                }
            };

            var customMessageBox = new CustomMessageBox
            {
                Message = AppResources.ShareMessage,
                Content = listSelector,
                IsLeftButtonEnabled = false,
                IsRightButtonEnabled = false,
                IsFullScreen = true
            };

            customMessageBox.Show();
        }

        /// <summary>
        /// Executes the Share Link service.
        /// </summary>
        /// <param name="title">The title shared.</param>
        /// <param name="message">The message shared.</param>
        /// <param name="link">The link shared.</param>
        private void ShareLink(string title, string message, string link = "")
        {
            title = string.IsNullOrEmpty(title) ? string.Empty : HtmlUtil.CleanHtml(title);
            message = string.IsNullOrEmpty(message) ? string.Empty : HtmlUtil.CleanHtml(message);
            var linkUri = string.IsNullOrEmpty(link) ? new System.Uri(AppResources.HomeUrl) : new System.Uri(link, System.UriKind.Absolute);
            var shareLinkTask = new ShareLinkTask
            {
                Title = title,
                Message = message,
                LinkUri = linkUri
            };

            shareLinkTask.Show();
        }

		/// <summary>
        /// Executes the Share Status service.
        /// </summary>
        /// <param name="status">The status to share.</param>
        private void ShareStatus(string status)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
			shareStatusTask.Status = status;

			shareStatusTask.Show();
        }
		
		/// <summary>
        /// Executes the Compose Mail service.
        /// </summary>
        /// <param name="subject">The message subject.</param>
		/// <param name="body">The message body.</param>
		/// <param name="to">The email of the recipient.</param>
        private void ShareByEmail(string subject, string body, string link = "")
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

			emailComposeTask.Subject = subject;
			
            if (string.IsNullOrEmpty(link))
                emailComposeTask.Body = body;
            else
                emailComposeTask.Body = string.Format("{0}{1}{1}{2}", body, Environment.NewLine, link);

			emailComposeTask.Show();
        }
		
		/// <summary>
        /// Executes the Share Media service.
        /// </summary>
        /// <param name="media">The media to share.</param>
        private void ShareMedia(string media)
        {
            if (media.StartsWith("http"))
                DownloadToMediaLibraryAndShare(media);
            else
                SaveToMediaLibraryAndShare(media);
        }

        private bool IsValidMedia(string media)
        {
            return media.StartsWith("http") || GetLocalResource(media) != null;
        }

        private StreamResourceInfo GetLocalResource(string media)
        {
            var uri = new Uri(IMAGES + Path.GetFileName(media), UriKind.RelativeOrAbsolute);
            return App.GetResourceStream(uri);
        }

        private void DownloadToMediaLibraryAndShare(string media)
        {
            try
            {
                WebClient client = new WebClient();
                client.OpenReadCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        var picture = SaveToMediaLibrary(media, e.Result);
                        Share(picture.GetPath());
                    }
                };
                client.OpenReadAsync(new Uri(media, UriKind.Absolute));
            }
            catch (Exception ex)
            {
				Debug.WriteLine("{0} {1}", AppResources.Error, ex.ToString());
            }
        }

        private void SaveToMediaLibraryAndShare(string media)
        {
            try
            {
                var resource = GetLocalResource(media);
                if (resource != null)
                {
                    using (var stream = resource.Stream)
                    {
                        var picture = SaveToMediaLibrary(media, stream);
                        Share(picture.GetPath());
                    }
                }
            }
            catch (Exception ex)
            {
				Debug.WriteLine("{0} {1}", AppResources.Error, ex.ToString());
            }
        }

        private Picture SaveToMediaLibrary(string media, Stream stream)
        {
            MediaLibrary library = new MediaLibrary();
            var picture = library.SavePicture(Path.GetFileName(media), stream);
            return picture;
        }

        private void Share(string media)
        {
            ShareMediaTask mediaTask = new ShareMediaTask();
            mediaTask.FilePath = media;

            mediaTask.Show();
        }
    }
}
