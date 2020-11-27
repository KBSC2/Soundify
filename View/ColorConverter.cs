using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace View
{
    public class ColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var colorCode = value as string;
            if(colorCode == null) return new SolidColorBrush(Colors.Black);
            return (SolidColorBrush)new BrushConverter().ConvertFromString(colorCode);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}