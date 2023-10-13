using ChatPrisma.Services.KeyboardHooks;
using Microsoft.Extensions.Hosting;

namespace ChatPrisma.HostedServices;

public class StartKeyboardHooksHostedService(IKeyboardHooks keyboardHooks) : IHostedLifecycleService
{
    public async Task StartingAsync(CancellationToken cancellationToken)
    {
        await keyboardHooks.StartAsync();
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task StartedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task StoppingAsync(CancellationToken cancellationToken)
    {
        await keyboardHooks.StopAsync();
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