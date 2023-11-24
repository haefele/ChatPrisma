using ChatPrisma.Options;
using GlobalKeyInterceptor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shortcut = GlobalKeyInterceptor.Shortcut;

namespace ChatPrisma.Services.KeyboardHooks;

public class GlobalKeyInterceptorKeyboardHooks(ILogger<GlobalKeyInterceptorKeyboardHooks> logger, IOptions<HotkeyOptions> keyboardOptions) : IKeyboardHooks, IDisposable
{
    private const string TextEnhancement = "TextEnhancement";

    public event EventHandler? TextEnhancementHotkeyPressed;

    private KeyInterceptor? _interceptor;
    public Task StartAsync()
    {
        var (textEnhancementKey, textEnhancementKeyModifiers) = ParseKey(keyboardOptions.Value.TextEnhancement);

        var shortcuts = new[]
        {
            new Shortcut(textEnhancementKey, textEnhancementKeyModifiers, TextEnhancement),
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

        if (e.Shortcut.Name == TextEnhancement)
        {
            this.TextEnhancementHotkeyPressed?.Invoke(this, EventArgs.Empty);
            e.IsHandled = true;
        }
    }

    private (Key, KeyModifier) ParseKey(KeyCombination keyCombination)
    {
        try
        {
            var key = keyCombination.Key;
            var keyModifiers = keyCombination.KeyModifiers;

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
