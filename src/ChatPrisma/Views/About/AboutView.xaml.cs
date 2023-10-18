using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace ChatPrisma.Views.About;

public partial class AboutView
{
    public AboutViewModel ViewModel => (AboutViewModel)this.DataContext;

    private void Contact_EmailHyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("mailto:" + this.ViewModel.ContactEmailAddress) { UseShellExecute = true });
    }

    private void Contact_GitHubHyperlink_OnClick(object sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo(this.ViewModel.GitHubLink) { UseShellExecute = true });
    }
}
