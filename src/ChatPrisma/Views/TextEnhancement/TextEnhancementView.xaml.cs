using System.Windows;
using System.Windows.Threading;
using ChatPrisma.Common;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementView
{
    private void TextEnhancementView_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.InstructionTextBox.Focus();

        // Ensure we are scrolled to the bottom
        this.Dispatcher.BeginInvoke(DispatcherPriority.Render, this.ScrollToBottom);

        WindowPlacementHelper.CenteredInFront(this);
    }

    private void TextEnhancementView_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is TextEnhancementViewModel oldViewModel)
        {
            oldViewModel.ApplyInstructionCancelled -= this.ViewModelOnApplyInstructionCancelled;
        }

        if (e.NewValue is TextEnhancementViewModel newViewModel)
        {
            newViewModel.ApplyInstructionCancelled += this.ViewModelOnApplyInstructionCancelled;
        }
    }

    private void ViewModelOnApplyInstructionCancelled(object? sender, EventArgs e)
    {
        // Set cursor at the end of the instruction textbox
        this.InstructionTextBox.Select(this.InstructionTextBox.Text.Length, 0);
    }

    private void TextTextBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Keep the ScrollViewer scrolled to the bottom while the text is generating
        this.ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        this.TextScrollViewer.ScrollToVerticalOffset(this.TextScrollViewer.ExtentHeight);
    }
}
