using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ChatPrisma.Views.OpenSource;

public partial class OpenSourceView : UserControl
{
    private void ThirdPartyLibrary_HomepageHyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        var hyperlink = (Hyperlink)sender;
        var thirdPartyLibrary = (ThirdPartyLibrary)hyperlink.DataContext;

        Process.Start(new ProcessStartInfo(thirdPartyLibrary.HomepageUrl.ToString()) { UseShellExecute = true });
    }

    private void ThirdPartyLibrary_LicenseHyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        var hyperlink = (Hyperlink)sender;
        var thirdPartyLibrary = (ThirdPartyLibrary)hyperlink.DataContext;

        Process.Start(new ProcessStartInfo(thirdPartyLibrary.LicenseUrl.ToString()) { UseShellExecute = true });
    }
}
