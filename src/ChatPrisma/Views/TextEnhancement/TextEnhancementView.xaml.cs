using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementView
{
    private void TextEnhancementView_OnLoaded(object sender, RoutedEventArgs e)
    {
        this.InstructionTextBox.Focus();

        var window = Window.GetWindow(this)!;

        // Ensure we are scrolled to the bottom
        window.Dispatcher.BeginInvoke(DispatcherPriority.Render, this.ScrollToBottom);

        // Setup dimensions and starting position
        window.Dispatcher.BeginInvoke(DispatcherPriority.Render, () =>
        {
            var helper = new WindowInteropHelper(window);
            var currentScreen = Screen.FromHandle(helper.Handle);
            var dpi = VisualTreeHelper.GetDpi(window);
            var currentScreenWorkingAreaDpiAdjusted = new Rectangle(
                (int)(currentScreen.WorkingArea.X / dpi.DpiScaleX),
                (int)(currentScreen.WorkingArea.Y / dpi.DpiScaleY),
                (int)(currentScreen.WorkingArea.Width / dpi.DpiScaleX),
                (int)(currentScreen.WorkingArea.Height / dpi.DpiScaleY));

            this.Width = currentScreenWorkingAreaDpiAdjusted.Width switch
            {
                > 1400 => 800,
                _ => 600
            };

            this.MinHeight = 200;

            // Set window max-height, so we don't have to think about the window-shell when calculating the starting position
            window.MaxHeight = currentScreenWorkingAreaDpiAdjusted.Height switch
            {
                > 1200 => 1000,
                > 1000 => 800,
                > 700 => 600,
                _ => 400,
            };

            // Place the window a bit moved to the top, so it is perfectly centered if we reach window.MaxHeight
            window.Top = currentScreenWorkingAreaDpiAdjusted.Y + Math.Max((currentScreenWorkingAreaDpiAdjusted.Height - window.MaxHeight) / 2, 0);
            // And horizontally perfectly centered, because we have a fixed width
            window.Left = currentScreenWorkingAreaDpiAdjusted.X + Math.Max((currentScreenWorkingAreaDpiAdjusted.Width - this.Width) / 2, 0);
        });

        // Hide the window until the previous call has positioned it correctly
        // Don't use Visibility here, as that will not just hide the window, but also deactivate it and make it "not be a dialog anymore"
        window.Opacity = 0;
        window.Dispatcher.BeginInvoke(DispatcherPriority.Render, () =>
        {
            window.Opacity = 1;
        });

        // Ensure window is shown above all other windows
        window.Activate();
        window.Topmost = true;
        window.Topmost = false;
        window.Focus();
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
