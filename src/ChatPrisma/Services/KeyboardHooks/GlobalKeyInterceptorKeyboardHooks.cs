using ChatPrisma.Options;
using GlobalKeyInterceptor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shortcut = GlobalKeyInterceptor.Shortcut;

namespace ChatPrisma.Services.KeyboardHooks;

public class GlobalKeyInterceptorKeyboardHooks(ILogger<GlobalKeyInterceptorKeyboardHooks> logger, IOptions<HotkeyOptions> keyboardOptions) : IKeyboardHooks, IDisposable
{
    public event EventHandler? CombinationPressed;

    private KeyInterceptor? _interceptor;
    public Task StartAsync()
    {
        var (key, keyModifiers) = ParseKey();

        var shortcuts = new[]
        {
            new Shortcut(key, keyModifiers, "Prisma Shortcut"),
        };

        this._interceptor = new KeyInterceptor(shortcuts);
        this._interceptor.ShortcutPressed += this.OnShortcutPressed;

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

        e.IsHandled = true;
    }

    private (Key, KeyModifier) ParseKey()
    {
        try
        {
            var key = keyboardOptions.Value.Key;
            var keyModifiers = keyboardOptions.Value.KeyModifiers;

            var parsedKey = Enum.Parse<Key>(key, ignoreCase: true);

            var parsedKeyModifiers = keyModifiers
                .Split('+')
                .Select(f => f.Trim())
                .Select(f => Enum.Parse<KeyModifier>(f, ignoreCase: true))
                .Aggregate((x, y) => x | y);

            return (parsedKey, parsedKeyModifiers);
        }
        catch (Exception exception)
        {
            throw new PrismaException("Could not parse keyboard shortcut.", exception);
        }
    }

    public void Dispose()
    {
        this._interceptor?.Dispose();
        this._interceptor = null;
    }
}
