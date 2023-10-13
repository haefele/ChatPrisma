using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;
using FluentIcons.Common;
using FluentIcons.WPF;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Host;

public class TrayIconLifetime(IOptions<TrayIconLifetimeOptions> options, Application app, IServiceProvider serviceProvider) : IHostLifetime
{
    private TaskbarIcon? _icon;

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        this._icon = new TaskbarIcon();
        this._icon.ToolTipText = options.Value.ToolTipText;
        this._icon.ContextMenu = options.Value.ContextMenuFactory?.Invoke(serviceProvider) ?? new ContextMenu();
        this._icon.TrayMouseDoubleClick += this.IconOnTrayMouseDoubleClick;

        if (options.Value.AppShutdownEnabled)
        {
            if (this._icon.ContextMenu.Items.Count > 0) 
                this._icon.ContextMenu.Items.Add(new Separator());

            this._icon.ContextMenu.Items.Add(new MenuItem
            {
                Icon = new SymbolIcon { Symbol = Symbol.ErrorCircle },
                Header = options.Value.AppShutdownHeader,
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
        options.Value.MouseDoubleClickAction?.Invoke(serviceProvider);
    }
}