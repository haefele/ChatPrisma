using System.Windows.Forms;

namespace ChatPrisma.Services.TextExtractor;

public class ClipboardTextExtractor : ITextExtractor
{
    public async Task<string?> GetCurrentTextAsync()
    {
        // Give the user a bit of time to release the keyboard keys,
        // otherwise CTRL+C will not work
        await Task.Delay(TimeSpan.FromMilliseconds(200));
        
        // Need to clear the clipboard, or otherwise we might get some previously copied text
        Clipboard.Clear();
        SendKeys.SendWait("^c");

        var selectedText = Clipboard.GetText();
        
        if (string.IsNullOrWhiteSpace(selectedText))
            return null;
        
        return selectedText;
    }
}