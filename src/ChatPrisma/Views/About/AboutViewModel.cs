using System.Collections.ObjectModel;
using ChatPrisma.Options;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.About;

public partial class AboutViewModel(IOptionsMonitor<ApplicationOptions> options, IDialogService dialogService, IViewModelFactory viewModelFactory) : ObservableObject
{
    [ObservableProperty]
    private string _applicationName = options.CurrentValue.ApplicationName;

    [ObservableProperty]
    private string _applicationVersion = options.CurrentValue.ApplicationVersion;

    [ObservableProperty]
    private string _commitId = options.CurrentValue.CommitId;

    [ObservableProperty]
    private bool _isPublicVersion = options.CurrentValue.IsPublicVersion;

    [ObservableProperty]
    private string _contactName = options.CurrentValue.ContactName;

    [ObservableProperty]
    private string _contactEmailAddress = options.CurrentValue.ContactEmailAddress;

    [ObservableProperty]
    private string _gitHubLink = options.CurrentValue.GitHubLink;

    [RelayCommand]
    private async Task ShowOpenSourceLibraries()
    {
        var viewModel = viewModelFactory.CreateOpenSourceViewModel();
        await dialogService.ShowDialog(viewModel);
    }
}
