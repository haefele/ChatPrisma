using System.Windows;
using System.Windows.Controls;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.ViewModels;
using ChatPrisma.Views.Settings;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;
using FluentIcons.WPF;
using Microsoft.Extensions.DependencyInjection;

namespace ChatPrisma;

public static class WindowsTray
{
    public static void HandleDoubleClick(IServiceProvider serviceProvider)
    {
        ShowAbout(serviceProvider);
    }
    
    public static ContextMenu CreateContextMenu(IServiceProvider serviceProvider)
    {
        return new ContextMenu
        {
            Items =
            {
                new MenuItem
                {
                    Icon = new SymbolIcon { Symbol = Symbol.Settings }, 
                    Header = "Einstellungen",
                    Command = new RelayCommand(() => ShowSettings(serviceProvider))
                },
                new MenuItem
                {
                    Icon = new SymbolIcon { Symbol = Symbol.Info },
                    Header = "Über",
                    Command = new RelayCommand(() => ShowAbout(serviceProvider))
                }
            }
        };
    }
    
    private static void ShowSettings(IServiceProvider serviceProvider)
    {
        var viewModelFactory = serviceProvider.GetRequiredService<IViewModelFactory>();
        var dialogService = serviceProvider.GetRequiredService<IDialogService>();
        
        var app = viewModelFactory.CreateSettingsViewModel();
        dialogService.ShowDialog(app);
    }

    private static void ShowAbout(IServiceProvider serviceProvider)
    {
        var viewModelFactory = serviceProvider.GetRequiredService<IViewModelFactory>();
        var dialogService = serviceProvider.GetRequiredService<IDialogService>();
        
        var app = viewModelFactory.CreateAboutViewModel();
        dialogService.ShowDialog(app);
    }
}