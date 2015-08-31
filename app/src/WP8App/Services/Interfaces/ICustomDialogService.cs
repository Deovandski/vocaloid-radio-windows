using System.Windows.Controls;

namespace WPAppStudio.Services.Interfaces
{
    /// <summary>
    /// Interface for dialog service.
    /// </summary>
    public interface ICustomDialogService
    {
        /// <summary>
        /// Shows a message with a caption in the dialog.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        void Show(string title, object content);
    }
}
