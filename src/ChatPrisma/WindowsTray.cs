using System.Windows;
using System.Windows.Controls;
using ChatPrisma.Resources;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.TextExtractor;
using ChatPrisma.Services.ViewModels;
using ChatPrisma.Views.Settings;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;
using FluentIcons.WPF;
using Microsoft.Extensions.DependencyInjection;

namespace ChatPrisma;

public static class WindowsTray
{
    public static async void HandleDoubleClick(IServiceProvider serviceProvider)
    {
        var textExtractor = serviceProvider.GetRequiredService<ITextExtractor>();
        var text = await textExtractor.GetPreviousTextAsync();

        if (text is null)
            return;

        var viewModelFactory = serviceProvider.GetRequiredService<IViewModelFactory>();
        var dialogService = serviceProvider.GetRequiredService<IDialogService>();

        var viewModel = viewModelFactory.CreateTextEnhancementViewModel(text);
        await dialogService.ShowDialog(viewModel);
    }

    public static ContextMenu CreateContextMenu(IServiceProvider serviceProvider)
    {
        return new ContextMenu
        {
            Items =
            {
                new MenuItem
                {
                    Icon = new SymbolIcon { Symbol = Symbol.Info },
                    Header = Strings.About,
                    Command = new AsyncRelayCommand(() => ShowAbout(serviceProvider))
                },
                new MenuItem
                {
                    Icon = new SymbolIcon { Symbol = Symbol.DrawerArrowDownload },
                    Header = Strings.CheckForUpdates,
                    Command = new AsyncRelayCommand(() => ShowUpdates(serviceProvider))
                },
                new MenuItem
                {
                    Icon = new SymbolIcon { Symbol = Symbol.Settings },
                    Header = Strings.Settings,
                    Command = new AsyncRelayCommand(() => ShowSettings(serviceProvider)),
                },
            }
        };
    }

    private static async Task ShowSettings(IServiceProvider serviceProvider)
    {
        var viewModelFactory = serviceProvider.GetRequiredService<IViewModelFactory>();
        var dialogService = serviceProvider.GetRequiredService<IDialogService>();

        var app = viewModelFactory.CreateSettingsViewModel();
        await dialogService.ShowDialog(app);
    }

    private static async Task ShowUpdates(IServiceProvider serviceProvider)
    {
        var viewModelFactory = serviceProvider.GetRequiredService<IViewModelFactory>();
        var dialogService = serviceProvider.GetRequiredService<IDialogService>();

        var app = viewModelFactory.CreateUpdateViewModel();
        await dialogService.ShowDialog(app);
    }

    private static async Task ShowAbout(IServiceProvider serviceProvider)
    {
        var viewModelFactory = serviceProvider.GetRequiredService<IViewModelFactory>();
        var dialogService = serviceProvider.GetRequiredService<IDialogService>();

        var app = viewModelFactory.CreateAboutViewModel();
        await dialogService.ShowDialog(app);
    }
}
