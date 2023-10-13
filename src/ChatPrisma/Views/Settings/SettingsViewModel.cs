using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatPrisma.Views.Settings;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _test;
}