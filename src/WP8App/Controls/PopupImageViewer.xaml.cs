using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;

namespace WPAppStudio.Controls
{
    /// <summary>
    /// PopupImageViewer Control
    /// </summary>
	public partial class PopupImageViewer : UserControl
    {
        public PopupImageViewer()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(PopupImageViewer),
            new PropertyMetadata(string.Empty)
        );

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
            "Message",
            typeof(string),
            typeof(PopupImageViewer),
            new PropertyMetadata(string.Empty)
        );

		public Image Image
        {
            get { return (Image)GetValue(ImageProperty); }
            set
            {
                SetValue(ImageProperty, value);
            }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
            "Image",
            typeof(Image),
            typeof(PopupImageViewer),
            new PropertyMetadata(null, OnImageChanged)
        );

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = (PopupImageViewer)d;
            selector.SetImage(e);
        }

        private void SetImage(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            BtnImageContainer.Content = e.NewValue as Image;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var zoomPanel = new ZoomPanel
            {
                Image = new Image() { Source = Image.Source }, 
                Height = Application.Current.RootVisual.RenderSize.Height,
                Width = Application.Current.RootVisual.RenderSize.Width,
            };
            
            var customMessageBox = new CustomMessageBox
            {
                Title = Title,
                Message = Message,
                Content = zoomPanel,
                IsLeftButtonEnabled = false,
				IsRightButtonEnabled = false,
                IsFullScreen = true
            };

            customMessageBox.Show();
        }
    }
}