using ChatPrisma.Services.AutoStart;
using ChatPrisma.Services.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatPrisma.Views.Settings;

public partial class SettingsViewModel(IAutoStartService autoStartService) : ObservableObject, IInitialize
{
    [ObservableProperty]
    private bool _isAutoStartActive;

    public async Task InitializeAsync()
    {
        this.IsAutoStartActive = await autoStartService.IsInAutoStart();
    }

    [RelayCommand]
    private async Task EnableAutoStart()
    {
        await autoStartService.SetAutoStart(true);
        this.IsAutoStartActive = true;
    }

    [RelayCommand]
    private async Task DisableAutoStart()
    {
        await autoStartService.SetAutoStart(false);
        this.IsAutoStartActive = false;
    }
}
