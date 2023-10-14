using System.Windows.Forms;

namespace ChatPrisma.Services.TextWriter;

public class SendKeysClipboardTextWriter : IClipboardTextWriter
{
    public async Task CopyTextAsync(string text, bool autoPaste)
    {
        Clipboard.SetText(text);

        if (autoPaste)
        {
            SendKeys.SendWait("^v");
        }
        
        await Task.CompletedTask;
    }
}