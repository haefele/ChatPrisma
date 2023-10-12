using Microsoft.Extensions.Hosting;

namespace ChatPrisma.Services.KeyboardHooks;

public interface IKeyboardHooks
{
    public event EventHandler CombinationPressed;
}

public class GlobalKeyInterceptorHostedService(GlobalKeyInterceptorKeyboardHooks keyboardHooks) : IHostedLifecycleService
{
    public Task StartingAsync(CancellationToken cancellationToken)
    {
        keyboardHooks.Start();
        return Task.CompletedTask;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task StartedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        keyboardHooks.Stop();
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}