using ChatPrisma.Options;
using ChatPrisma.Services.AutoStart;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.UpdateOptions;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.Settings;

public partial class SettingsViewModel(IAutoStartService autoStartService, IUpdateOptionsService updateOptionsService, IOptionsMonitor<ApplicationOptions> applicationOptions, IOptionsMonitor<OpenAIOptions> openAIOptions) : ObservableObject, IInitialize, IFinalize
{
    [ObservableProperty]
    private bool _isAutoStartActive;

    [ObservableProperty]
    private string _applicationName = applicationOptions.CurrentValue.ApplicationName;

    [ObservableProperty]
    private string? _model = openAIOptions.CurrentValue.Model;

    [ObservableProperty]
    private string? _apiKey = openAIOptions.CurrentValue.ApiKey;

    public async Task InitializeAsync()
    {
        this.IsAutoStartActive = await autoStartService.IsInAutoStart();
    }

    public async Task FinalizeAsync()
    {
        await autoStartService.SetAutoStart(this.IsAutoStartActive);

        await updateOptionsService.Update(openAIOptions.CurrentValue with
        {
            Model = this.Model,
            ApiKey = this.ApiKey
        });
    }
}
