namespace ChatPrisma.Services.KeyboardHooks;

public interface IKeyboardHooks
{
    public event EventHandler TextEnhancementHotkeyPressed;

    Task StartAsync();
    Task StopAsync();
}
