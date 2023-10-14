using ChatPrisma.Views.About;
using ChatPrisma.Views.Settings;
using ChatPrisma.Views.TextEnhancement;
using Microsoft.Extensions.DependencyInjection;

namespace ChatPrisma.Services.ViewModels;

public class ViewModelFactory(IServiceProvider serviceProvider) : IViewModelFactory
{
    public AboutViewModel CreateAboutViewModel() => ActivatorUtilities.CreateInstance<AboutViewModel>(serviceProvider);

    public SettingsViewModel CreateSettingsViewModel() => ActivatorUtilities.CreateInstance<SettingsViewModel>(serviceProvider);

    public TextEnhancementViewModel CreateTextEnhancementViewModel(string text) => ActivatorUtilities.CreateInstance<TextEnhancementViewModel>(serviceProvider, text);
}