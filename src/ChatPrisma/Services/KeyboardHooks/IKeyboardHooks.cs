namespace ChatPrisma.Services.KeyboardHooks;

public interface IKeyboardHooks
{
    public event EventHandler CombinationPressed;

    Task StartAsync();
    Task StopAsync();
}