using System.Windows.Controls;
using System.Windows;

namespace ChatPrisma.Themes;

#pragma warning disable CA1010 // Generic interface should also be implemented
public partial class ContextMenuStyles
#pragma warning restore CA1010 // Generic interface should also be implemented
{
}

public class ContextMenuItemStyleSelector : StyleSelector
{
    public Style? SeparatorStyle { get; set; }

    public override Style? SelectStyle(object item, DependencyObject container)
    {
        if (item is Separator)
        {
            return this.SeparatorStyle;
        }

        // Fall back to default style
        return null;
    }
}
