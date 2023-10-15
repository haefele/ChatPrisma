using ChatPrisma.Views.Settings;
using ChatPrisma.Views.About;
using ChatPrisma.Views.TextEnhancement;
using ChatPrisma.Views.Update;
using Onova.Models;

namespace ChatPrisma.Services.ViewModels;

public interface IViewModelFactory
{
    AboutViewModel CreateAboutViewModel();
    SettingsViewModel CreateSettingsViewModel();
    TextEnhancementViewModel CreateTextEnhancementViewModel(string text);
    UpdateViewModel CreateUpdateViewModel(CheckForUpdatesResult? updatesResult = null);
}
