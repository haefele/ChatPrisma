using System.Windows;

namespace ChatPrisma.Themes
{
#pragma warning disable CA1010 // Generic interface should also be implemented
    public partial class WindowStyles
#pragma warning restore CA1010 // Generic interface should also be implemented
    {
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow((System.Windows.Controls.Button)sender)?.Close();
        }
    }
}
