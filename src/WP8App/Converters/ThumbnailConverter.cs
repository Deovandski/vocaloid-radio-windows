using System;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPAppStudio.Converters
{
    public class ThumbnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
				if (String.IsNullOrEmpty(value.ToString()) || (!value.ToString().Contains(".jpg") && !value.ToString().Contains(".png"))) return new BitmapImage(new Uri("../Images/NoImage.png", UriKind.Relative));
				
				var bm = new BitmapImage(new Uri(value.ToString(), UriKind.RelativeOrAbsolute))
                {
					CreateOptions = BitmapCreateOptions.BackgroundCreation,
                    DecodePixelHeight = System.Convert.ToInt32(parameter)
                };

				return bm;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
			
			return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
