using System.Windows;
using Microsoft.Extensions.Hosting;
using Onova;

namespace ChatPrisma.HostedServices;

public class UpdaterHostedService(IUpdateManager updateManager, Application app) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            var result = await updateManager.CheckForUpdatesAsync(stoppingToken);
            if (result is { CanUpdate: true, LastVersion: not null })
            {
                var updateResult = MessageBox.Show($"Update available {result.LastVersion}!, Wanna update now?", "Title", MessageBoxButton.YesNo);
                if (updateResult == MessageBoxResult.Yes)
                {
                    await updateManager.PrepareUpdateAsync(result.LastVersion, cancellationToken: stoppingToken);
                    updateManager.LaunchUpdater(result.LastVersion);

                    app.Shutdown();
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}
