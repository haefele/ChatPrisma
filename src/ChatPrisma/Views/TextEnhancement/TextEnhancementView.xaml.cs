using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementView
{
    private void TextEnhancementView_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.InstructionTextBox.Focus();

        // Place window slightly to the top
        var window = Window.GetWindow(this)!;
        window.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
        {
            var helper = new WindowInteropHelper(window);
            var currentScreen = Screen.FromHandle(helper.Handle);
            var currentScreenHeight = currentScreen.Bounds.Height;

            // Place the window a bit moved to the top, so it is perfectly centered if we reach this.MaxHeight
            window.Top = Math.Max((currentScreenHeight - this.MaxHeight) / 2, 0);
        }));
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

    private void TextTextBlock_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Keep the ScrollViewer scrolled to the bottom while the text is generating
        this.TextScrollViewer.ScrollToVerticalOffset(this.TextScrollViewer.ExtentHeight);
    }
}
