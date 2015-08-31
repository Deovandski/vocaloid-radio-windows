using WPAppStudio.Localization;
using System;
using System.Windows.Data;

namespace WPAppStudio.Converters
{
    public class LocalizedResourcesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var resourceName = value.ToString() as string;
            return AppResources.ResourceManager.GetString(resourceName, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
