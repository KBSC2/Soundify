using System;
using System.Globalization;
using System.Windows.Data;

namespace View.Components
{
    class ID3DurationPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var duration = value is TimeSpan span ? span : default;

            return duration.ToString(@"hh\:mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
