using System.Globalization;
using System.Windows.Data;

namespace ChatPrisma.Themes.Converters;

public class NumberWidthPlaceholderTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int intValue)
            return string.Empty;

        // Use character "4" as a placeholder, because it seems to be even wider than 0
        return new string('4', intValue.ToString().Length);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
