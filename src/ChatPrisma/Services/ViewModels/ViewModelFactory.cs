using ChatPrisma.Views.About;
using ChatPrisma.Views.OpenSource;
using ChatPrisma.Views.Settings;
using ChatPrisma.Views.TextEnhancement;
using ChatPrisma.Views.Update;
using Microsoft.Extensions.DependencyInjection;
using Onova.Models;

namespace ChatPrisma.Services.ViewModels;

public class ViewModelFactory(IServiceProvider serviceProvider) : IViewModelFactory
{
    public AboutViewModel CreateAboutViewModel() => ActivatorUtilities.CreateInstance<AboutViewModel>(serviceProvider);

    public SettingsViewModel CreateSettingsViewModel() => ActivatorUtilities.CreateInstance<SettingsViewModel>(serviceProvider);

    public TextEnhancementViewModel CreateTextEnhancementViewModel(string text) => ActivatorUtilities.CreateInstance<TextEnhancementViewModel>(serviceProvider, text);

    public UpdateViewModel CreateUpdateViewModel(CheckForUpdatesResult? updatesResult = null) => updatesResult is null
        ? ActivatorUtilities.CreateInstance<UpdateViewModel>(serviceProvider)
        : ActivatorUtilities.CreateInstance<UpdateViewModel>(serviceProvider, updatesResult);

    public OpenSourceViewModel CreateOpenSourceViewModel() => ActivatorUtilities.CreateInstance<OpenSourceViewModel>(serviceProvider);
}
