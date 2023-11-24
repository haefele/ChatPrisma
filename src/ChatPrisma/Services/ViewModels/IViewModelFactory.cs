using ChatPrisma.Views.Settings;
using ChatPrisma.Views.About;
using ChatPrisma.Views.Chat;
using ChatPrisma.Views.TextEnhancement;
using ChatPrisma.Views.Update;
using Onova.Models;
using ChatPrisma.Views.OpenSource;

namespace ChatPrisma.Services.ViewModels;

public interface IViewModelFactory
{
    AboutViewModel CreateAboutViewModel();
    SettingsViewModel CreateSettingsViewModel();
    TextEnhancementViewModel CreateTextEnhancementViewModel(string text);
    ChatViewModel CreateChatViewModel();
    UpdateViewModel CreateUpdateViewModel(CheckForUpdatesResult? updatesResult = null);
    OpenSourceViewModel CreateOpenSourceViewModel();
}
