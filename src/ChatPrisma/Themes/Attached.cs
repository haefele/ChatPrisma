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

    public static readonly DependencyProperty GroupBoxOpacityProperty = DependencyProperty.RegisterAttached(
        "GroupBoxOpacity", typeof(double), typeof(Attached), new PropertyMetadata(default(double)));

    public static void SetGroupBoxOpacity(DependencyObject element, double value)
    {
        element.SetValue(GroupBoxOpacityProperty, value);
    }
    public static double GetGroupBoxOpacity(DependencyObject element)
    {
        return (double)element.GetValue(GroupBoxOpacityProperty);
    }

    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached(
        "Placeholder", typeof(string), typeof(Attached), new PropertyMetadata(default(string)));

    public static void SetPlaceholder(DependencyObject element, string value)
    {
        element.SetValue(PlaceholderProperty, value);
    }
    public static string GetPlaceholder(DependencyObject element)
    {
        return (string)element.GetValue(PlaceholderProperty);
    }
}
