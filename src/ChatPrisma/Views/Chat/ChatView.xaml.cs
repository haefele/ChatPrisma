using System.Windows;
using System.Windows.Threading;
using ChatPrisma.Common;

namespace ChatPrisma.Views.Chat;

public partial class ChatView
{
    private void ChatView_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.InstructionTextBox.Focus();

        // Ensure we are scrolled to the bottom
        this.Dispatcher.BeginInvoke(DispatcherPriority.Render, this.ScrollToBottom);

        WindowPlacementHelper.CenteredInFront(this);
    }

    private void ChatItemsControl_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Keep the ScrollViewer scrolled to the bottom while the text is generating
        this.ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        this.TextScrollViewer.ScrollToVerticalOffset(this.TextScrollViewer.ExtentHeight);
    }
}
