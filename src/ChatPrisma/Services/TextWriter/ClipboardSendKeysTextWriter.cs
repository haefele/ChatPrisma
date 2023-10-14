using System.Windows.Forms;

namespace ChatPrisma.Services.TextWriter;

public class ClipboardSendKeysTextWriter : ITextWriter
{
    public async Task WriteTextAsync(string text)
    {
        // Give the user some time to take their hands off the keyboard
        await Task.Delay(TimeSpan.FromMilliseconds(200));
        
        Clipboard.SetText(text);
        
        SendKeys.SendWait("^v");
        await Task.CompletedTask;
    }
}