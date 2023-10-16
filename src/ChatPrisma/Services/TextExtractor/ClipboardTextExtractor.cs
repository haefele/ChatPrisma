using System.Windows.Forms;
using ChatPrisma.Options;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.TextExtractor;

public class ClipboardTextExtractor(IOptionsMonitor<HotkeyOptions> hotkeyOptions) : ITextExtractor
{
    public async Task<string?> GetCurrentTextAsync()
    {
        // Give the user a bit of time to release the keyboard keys,
        // otherwise CTRL+C will not work
        await Task.Delay(TimeSpan.FromMilliseconds(hotkeyOptions.CurrentValue.HotkeyDelayInMilliseconds));

        // Need to clear the clipboard, or otherwise we might get some previously copied text
        Clipboard.Clear();
        SendKeys.SendWait("^c");

        var selectedText = Clipboard.GetText();

        if (string.IsNullOrWhiteSpace(selectedText))
            return null;

        return selectedText;
    }
}
