using System.Windows;
using Microsoft.Extensions.Hosting;
using Onova;

namespace ChatPrisma.HostedServices;

public class UpdaterHostedService(IUpdateManager updateManager, Application app) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var result = await updateManager.CheckForUpdatesAsync(cancellationToken);
        if (result is { CanUpdate: true, LastVersion: not null })
        {
            await updateManager.PrepareUpdateAsync(result.LastVersion, cancellationToken: cancellationToken);

            var updateResult = MessageBox.Show("Update available!, Wanna update now?", "Title", MessageBoxButton.YesNo);
            if (updateResult == MessageBoxResult.Yes)
            {
                updateManager.LaunchUpdater(result.LastVersion);
                app.Shutdown();
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
