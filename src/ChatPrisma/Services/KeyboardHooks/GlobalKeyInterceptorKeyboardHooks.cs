using GlobalKeyInterceptor;
using Microsoft.Extensions.Logging;
using Shortcut = GlobalKeyInterceptor.Shortcut;

namespace ChatPrisma.Services.KeyboardHooks;

public class GlobalKeyInterceptorKeyboardHooks(ILogger<GlobalKeyInterceptorKeyboardHooks> logger) : IKeyboardHooks
{
    public event EventHandler? CombinationPressed;
    
    private KeyInterceptor? _interceptor;
    public void Start()
    {
        var shortcuts = new[]
        {
            new Shortcut(Key.Y, KeyModifier.Ctrl | KeyModifier.Shift | KeyModifier.Alt, "CTRL + SHIFT + ALT + Y"),
        };

        this._interceptor = new KeyInterceptor(shortcuts);
        this._interceptor.ShortcutPressed += OnShortcutPressed;
        
        logger.LogInformation("Started listening for keyboard shortcuts.");
    }
    public void Stop()
    {
        this._interceptor?.Dispose();
        this._interceptor = null;
        
        logger.LogInformation("Stopped listening for keyboard shortcuts.");
    }

    private void OnShortcutPressed(object? sender, ShortcutPressedEventArgs e)
    {
        logger.LogTrace("Shortcut pressed: {Shortcut}", e.Shortcut);
        this.CombinationPressed?.Invoke(this, EventArgs.Empty);
    }
}