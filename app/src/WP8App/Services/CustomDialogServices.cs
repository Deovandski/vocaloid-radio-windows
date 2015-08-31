using Microsoft.Phone.Controls;
using Test_Zoom.Services.Interfaces;

namespace WPAppStudio.Services
{
    /// <summary>
    /// Implementation of a dialog service.
    /// </summary>
    public class CustomDialogService : ICustomDialogService
    {
        /// <summary>
        /// Shows a message with a caption in the dialog.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        public void Show(string title, object content)
        {
            var customMessageBox = new CustomMessageBox
            {
                Title = title,
                Content = content,
                IsLeftButtonEnabled = false,
                RightButtonContent = "Close",
                IsFullScreen = true
            };

            customMessageBox.Show();
        }
    }
}
