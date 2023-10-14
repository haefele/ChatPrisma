using ChatPrisma.Views.Settings;
using ChatPrisma.Views.About;
using ChatPrisma.Views.TextEnhancement;

namespace ChatPrisma.Services.ViewModels;

public interface IViewModelFactory
{
    AboutViewModel CreateAboutViewModel();
    SettingsViewModel CreateSettingsViewModel();
    TextEnhancementViewModel CreateTextEnhancementViewModel(string text);
}