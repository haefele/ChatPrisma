using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChatPrisma.Options;
using ChatPrisma.Services.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Onova;
using Onova.Models;

namespace ChatPrisma.Views.Update
{
    public partial class UpdateViewModel : ObservableObject, IInitialize
    {
        private readonly Application _app;
        private readonly IUpdateManager _updateManager;
        private CheckForUpdatesResult? _updatesResult;

        public UpdateViewModel(IOptionsMonitor<ApplicationOptions> applicationOptions, Application app, IUpdateManager updateManager)
        {
            this._app = app;
            this._updateManager = updateManager;
            this.CurrentVersion = applicationOptions.CurrentValue.ApplicationVersion;
            this.ApplicationName = applicationOptions.CurrentValue.ApplicationName;
        }
        public UpdateViewModel(CheckForUpdatesResult? updatesResult, IOptionsMonitor<ApplicationOptions> applicationOptions, Application app, IUpdateManager updateManager)
            : this(applicationOptions, app, updateManager)
        {
            this.UseUpdateResult(updatesResult);
        }

        public async Task InitializeAsync()
        {
            if (this._updatesResult is null)
            {
                var updateResult = await this._updateManager.CheckForUpdatesAsync();
                this.UseUpdateResult(updateResult);
            }
        }

        [ObservableProperty]
        private string _currentVersion;

        [ObservableProperty]
        private string _applicationName;

        [ObservableProperty]
        private bool _updateAvailable = false;

        [ObservableProperty]
        private string? _updateVersion;


        [RelayCommand]
        private async Task DownloadAndInstallUpdate(CancellationToken cancellationToken)
        {
            if (this._updatesResult?.LastVersion is null)
                return;

            await this._updateManager.PrepareUpdateAsync(this._updatesResult.LastVersion, cancellationToken: cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            this._updateManager.LaunchUpdater(this._updatesResult.LastVersion!);
            this._app.Shutdown();
        }

        private void UseUpdateResult(CheckForUpdatesResult? updatesResult)
        {
            this._updatesResult = updatesResult;
            this.UpdateAvailable = updatesResult?.CanUpdate ?? false;
            this.UpdateVersion = updatesResult?.LastVersion?.ToString(3);
        }
    }
}
