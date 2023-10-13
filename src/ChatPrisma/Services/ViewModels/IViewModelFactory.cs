using ChatPrisma.Views.Settings;
using ChatPrisma.Views.About;

namespace ChatPrisma.Services.ViewModels;

public interface IViewModelFactory
{
    AboutViewModel CreateAboutViewModel();
    SettingsViewModel CreateSettingsViewModel();
}