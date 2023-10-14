using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ChatPrisma.Options;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;
using FluentIcons.WPF;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Host;

public class TrayIconLifetime(IOptions<TrayIconLifetimeOptions> trayIconLifetimeOptions, IOptions<ApplicationOptions> applicationOptions, Application app, IServiceProvider serviceProvider) : IHostLifetime
{
    private TaskbarIcon? _icon;

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        this._icon = new TaskbarIcon();
        this._icon.ToolTipText = applicationOptions.Value.ApplicationName;
        this._icon.ContextMenu = trayIconLifetimeOptions.Value.ContextMenuFactory?.Invoke(serviceProvider) ?? new ContextMenu();
        this._icon.TrayMouseDoubleClick += this.IconOnTrayMouseDoubleClick;
        this._icon.IconSource = new BitmapImage(new Uri("pack://application:,,,/Themes/Images/AppIcon.ico"));

        if (trayIconLifetimeOptions.Value.AppShutdownEnabled)
        {
            if (this._icon.ContextMenu.Items.Count > 0) 
                this._icon.ContextMenu.Items.Add(new Separator());

            this._icon.ContextMenu.Items.Add(new MenuItem
            {
                Icon = new SymbolIcon { Symbol = Symbol.Battery0 },
                Header = trayIconLifetimeOptions.Value.AppShutdownHeader,
                Command = new RelayCommand(app.Shutdown)
            });
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (this._icon is not null)
        {
            this._icon.TrayMouseDoubleClick -= this.IconOnTrayMouseDoubleClick;
            this._icon?.Dispose();
            this._icon = null;
        }

        return Task.CompletedTask;
    }

    private void IconOnTrayMouseDoubleClick(object sender, RoutedEventArgs e)
    {
        trayIconLifetimeOptions.Value.MouseDoubleClickAction?.Invoke(serviceProvider);
    }
}