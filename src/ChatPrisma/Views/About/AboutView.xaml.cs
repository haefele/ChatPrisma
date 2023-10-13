using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace ChatPrisma.Views.About;

public partial class AboutView
{
    public AboutView()
    {
        this.InitializeComponent();
    }

    private void Contact_EmailHyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("mailto:haefele@xemio.net") { UseShellExecute = true });
    }
    
    private void ThirdPartyLibrary_HomepageHyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        var hyperlink = (Hyperlink)sender;
        var thirdPartyLibrary = (ThirdPartyLibrary)hyperlink.DataContext;
        
        Process.Start(new ProcessStartInfo(thirdPartyLibrary.HomepageUrl) { UseShellExecute = true });
    }

    private void ThirdPartyLibrary_LicenseHyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        var hyperlink = (Hyperlink)sender;
        var thirdPartyLibrary = (ThirdPartyLibrary)hyperlink.DataContext;
        
        Process.Start(new ProcessStartInfo(thirdPartyLibrary.LicenseUrl) { UseShellExecute = true });
    }
}