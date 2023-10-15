using GlobalKeyInterceptor;
using Microsoft.Extensions.Logging;
using Shortcut = GlobalKeyInterceptor.Shortcut;

namespace ChatPrisma.Services.KeyboardHooks;

public class GlobalKeyInterceptorKeyboardHooks(ILogger<GlobalKeyInterceptorKeyboardHooks> logger) : IKeyboardHooks, IDisposable
{
    public event EventHandler? CombinationPressed;

    private KeyInterceptor? _interceptor;
    public Task StartAsync()
    {
        var shortcuts = new[]
        {
            new Shortcut(Key.Y, KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt, "CTRL + SHIFT + ALT + Y"),
        };

        this._interceptor = new KeyInterceptor(shortcuts);
        this._interceptor.ShortcutPressed += OnShortcutPressed;

        logger.LogInformation("Started listening for keyboard shortcuts.");

        return Task.CompletedTask;
    }
    public Task StopAsync()
    {
        this._interceptor?.Dispose();
        this._interceptor = null;

        logger.LogInformation("Stopped listening for keyboard shortcuts.");

        return Task.CompletedTask;
    }

    private void OnShortcutPressed(object? sender, ShortcutPressedEventArgs e)
    {
        logger.LogTrace("Shortcut pressed: {Shortcut}", e.Shortcut.Name);
        this.CombinationPressed?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        this._interceptor?.Dispose();
        this._interceptor = null;
    }
}
