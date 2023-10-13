using System.Windows.Controls;

namespace ChatPrisma.Host;

public class TrayIconLifetimeOptions
{
    public string? ToolTipText { get; set; }
    public Action<IServiceProvider>? MouseDoubleClickAction { get; set; }

    public Func<IServiceProvider, ContextMenu>? ContextMenuFactory { get; set; }

    public bool AppShutdownEnabled { get; set; } = true;
    public string AppShutdownHeader { get; set; } = "Beenden";
}