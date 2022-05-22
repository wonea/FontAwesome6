using Avalonia.Data.Converters;
using Avalonia.Media;

using System;
using System.Globalization;

namespace FontAwesome.Avalonia.Converters
{
    //[ValueConversion(typeof(SolidColorBrush), typeof(string))]
    public class HexColorStringToSolidColorBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                return brush.Color.ToString();
            }


            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                var converter = new BrushConverter();
                var obj = converter.ConvertFromString(str);
                return obj as SolidColorBrush;
            }

            return null;
        }
    }
}
