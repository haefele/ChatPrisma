namespace ChatPrisma.Services.KeyboardHooks;

public interface IKeyboardHooks
{
    public event EventHandler TextEnhancementHotkeyPressed;
    public event EventHandler ChatHotkeyPressed;

    Task StartAsync();
    Task StopAsync();
}
