using System.Diagnostics;
using System.Windows.Forms;
using ChatPrisma.Common;
using ChatPrisma.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.TextWriter;

public class SendKeysClipboardTextWriter(IOptionsMonitor<HotkeyOptions> hotkeyOptions, ILogger<SendKeysClipboardTextWriter> logger) : IClipboardTextWriter
{
    public async Task CopyTextAsync(string text, bool autoPaste)
    {
        Clipboard.SetText(text);

        if (autoPaste)
        {
            await this.WaitUntilNoKeyPressed();
            SendKeys.SendWait("^v");
        }

        await Task.CompletedTask;
    }

    private async Task WaitUntilNoKeyPressed()
    {
        var watch = Stopwatch.StartNew();

        var success = await TaskHelper.WaitUntil(() => KeyboardHelper.AnyKeyPressed() is false, hotkeyOptions.CurrentValue.HotkeyDelayInMilliseconds);
        if (success)
        {
            logger.LogInformation("Early exit from WaitUntilNoKeyPressed because no key is pressed anymore (after {Time} ms)", watch.Elapsed.TotalMilliseconds);
        }
        else
        {
            logger.LogInformation("Sadly the user did not release all keys in time (after {Time} ms)", watch.Elapsed.TotalMilliseconds);
        }
    }
}
