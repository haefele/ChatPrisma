using System.Windows.Controls;

namespace ChatPrisma.Host;

public class TrayIconLifetimeOptions
{
    public Action<IServiceProvider>? MouseDoubleClickAction { get; set; }

    public Func<IServiceProvider, ContextMenu>? ContextMenuFactory { get; set; }

    public bool AppShutdownEnabled { get; set; }
    public string? AppShutdownHeader { get; set; }
}