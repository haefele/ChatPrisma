using ChatPrisma.Options;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Onova;

namespace ChatPrisma.HostedServices;

public class UpdaterHostedService(IUpdateManager updateManager, IViewModelFactory viewModelFactory, IDialogService dialogService, IOptionsMonitor<UpdaterOptions> updaterOptions) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            if (updaterOptions.CurrentValue.CheckForUpdatesInBackground)
            {
                var result = await updateManager.CheckForUpdatesAsync(stoppingToken);
                if (result is { CanUpdate: true, LastVersion: not null })
                {
                    var viewModel = viewModelFactory.CreateUpdateViewModel(result);
                    await dialogService.ShowDialog(viewModel);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(updaterOptions.CurrentValue.MinutesBetweenUpdateChecks), stoppingToken);
        }
    }
}
