using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace ChatPrisma.Common;

public static class WindowPlacementHelper
{
    public static void CenteredInFront(FrameworkElement view)
    {
        var window = Window.GetWindow(view)!;

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

            view.Width = currentScreenWorkingAreaDpiAdjusted.Width switch
            {
                > 1400 => 800,
                _ => 600
            };

            view.MinHeight = 200;

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
            window.Left = currentScreenWorkingAreaDpiAdjusted.X + Math.Max((currentScreenWorkingAreaDpiAdjusted.Width - view.Width) / 2, 0);
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
}
