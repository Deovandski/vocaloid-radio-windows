using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPAppStudio.Controls
{
    /// <summary>
    /// Images panel with zoom
    /// </summary>
    public partial class ZoomPanel : UserControl
    {
		const double MaxScale = 10;
        double _scale = 1.0;
        double _minScale;
        double _coercedScale;
        double _originalScale;
        Size _viewportSize;
        bool _pinching;
        Point _screenMidpoint;
        Point _relativeMidpoint;
        BitmapImage _bitmap;
		bool _initialized;

		public ZoomPanel()
        {
            InitializeComponent();
        }
		
		public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Image), typeof(ZoomPanel), new PropertyMetadata(default(Image), OnImagePropertyChanged));


        private static void OnImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var image = e.NewValue as Image;
            if (image != null)
            {
                image.Source = image.Source;
                image.RenderTransform = new ScaleTransform();
                image.RenderTransformOrigin = new Point(0, 0);
                image.CacheMode = new BitmapCache();
                var zoomPanel = d as ZoomPanel;
                if (zoomPanel == null) return;
                zoomPanel.Canvas.Children.Add(image);
            }
        }

        public Image Image
        {
            get { return (Image)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        /// <summary> 
        /// Either the user has manipulated the image or the size of the viewport has changed. We only 
        /// care about the size. 
        /// </summary> 
        void viewport_ViewportChanged(object sender, System.Windows.Controls.Primitives.ViewportChangedEventArgs e)
        {
            var newSize = new Size(Viewport.Viewport.Width, Viewport.Viewport.Height);
            if (newSize != _viewportSize)
            {
                _viewportSize = newSize;
                CoerceScale(true);
                ResizeImage(false);
            }
			if(!_initialized)
                InitializeImage();
        }

        /// <summary> 
        /// Handler for the ManipulationStarted event. Set initial state in case 
        /// it becomes a pinch later. 
        /// </summary> 
        void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _pinching = false;
            _originalScale = _scale;
        }

        /// <summary> 
        /// Handler for the ManipulationDelta event. It may or may not be a pinch. If it is not a  
        /// pinch, the ViewportControl will take care of it. 
        /// </summary> 
        /// <param name="sender"></param> 
        /// <param name="e"></param> 
        void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                e.Handled = true;

                if (!_pinching)
                {
                    _pinching = true;
                    Point center = e.PinchManipulation.Original.Center;
                    _relativeMidpoint = new Point(center.X / Image.ActualWidth, center.Y / Image.ActualHeight);

                    var xform = Image.TransformToVisual(Viewport);
                    _screenMidpoint = xform.Transform(center);
                }

                _scale = _originalScale * e.PinchManipulation.CumulativeScale;

                CoerceScale(false);
                ResizeImage(false);
            }
            else if (_pinching)
            {
                _pinching = false;
                _originalScale = _scale = _coercedScale;
            }
        }

        /// <summary> 
        /// The manipulation has completed (no touch points anymore) so reset state. 
        /// </summary> 
        void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            _pinching = false;
            _scale = _coercedScale;
        }

        /// <summary> 
        /// Set image initial scale. 
        /// </summary> 
        private void InitializeImage()
        {
            _bitmap = (BitmapImage) Image.Source;

            // Set scale to the minimum, and then save it. 
            _scale = 0;
            CoerceScale(true);
            _scale = _coercedScale;

            ResizeImage(true);
            _initialized = true;
        }

        /// <summary> 
        /// Adjust the size of the image according to the coerced scale factor. Optionally 
        /// center the image, otherwise, try to keep the original midpoint of the pinch 
        /// in the same spot on the screen regardless of the scale. 
        /// </summary> 
        /// <param name="center"></param> 
        void ResizeImage(bool center)
        {
            if (_coercedScale == 0 || _bitmap == null) return;

            double newWidth = Canvas.Width = Math.Round(_bitmap.PixelWidth * _coercedScale);
            double newHeight = Canvas.Height = Math.Round(_bitmap.PixelHeight * _coercedScale);

            var scaleTransform = Image.RenderTransform as ScaleTransform;
            if (scaleTransform != null)
                scaleTransform.ScaleX = scaleTransform.ScaleY = _coercedScale;

            Viewport.Bounds = new Rect(0, 0, newWidth, newHeight);

            if (center)
            {
                Viewport.SetViewportOrigin(
                    new Point(
                        Math.Round((newWidth - Viewport.ActualWidth) / 2),
                        Math.Round((newHeight - Viewport.ActualHeight) / 2)
                        ));
            }
            else
            {
                var newImgMid = new Point(newWidth * _relativeMidpoint.X, newHeight * _relativeMidpoint.Y);
                var origin = new Point(newImgMid.X - _screenMidpoint.X, newImgMid.Y - _screenMidpoint.Y);
                Viewport.SetViewportOrigin(origin);
            }
        }

        /// <summary> 
        /// Coerce the scale into being within the proper range. Optionally compute the constraints  
        /// on the scale so that it will always fill the entire screen and will never get too big  
        /// to be contained in a hardware surface. 
        /// </summary> 
        /// <param name="recompute">Will recompute the min max scale if true.</param> 
        void CoerceScale(bool recompute)
        {
            if (recompute && _bitmap != null && Viewport != null)
            {
                // Calculate the minimum scale to fit the viewport 
                double minX = Viewport.ActualWidth / _bitmap.PixelWidth;
                double minY = Viewport.ActualHeight / _bitmap.PixelHeight;

                _minScale = Math.Min(minX, minY);
            }

            _coercedScale = Math.Min(MaxScale, Math.Max(_scale, _minScale));
        }
    }
}
