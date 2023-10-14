using System.Windows;

namespace ChatPrisma.Themes;

public static class Attached
{
    public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached(
        "Icon", typeof(FluentIcons.Common.Symbol), typeof(Attached), new PropertyMetadata(default(FluentIcons.Common.Symbol)));

    public static void SetIcon(DependencyObject element, FluentIcons.Common.Symbol value)
    {
        element.SetValue(IconProperty, value);
    }
    public static FluentIcons.Common.Symbol GetIcon(DependencyObject element)
    {
        return (FluentIcons.Common.Symbol)element.GetValue(IconProperty);
    }

    public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.RegisterAttached(
        "WindowTitle", typeof(string), typeof(Attached), new PropertyMetadata(default(string)));

    public static void SetWindowTitle(DependencyObject element, string value)
    {
        element.SetValue(WindowTitleProperty, value);
    }
    public static string GetWindowTitle(DependencyObject element)
    {
        return (string)element.GetValue(WindowTitleProperty);
    }
}