using System.Diagnostics;
using System.Windows.Forms;
using ChatPrisma.Common;
using ChatPrisma.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.TextExtractor;

public class ClipboardTextExtractor(IOptionsMonitor<HotkeyOptions> hotkeyOptions, ILogger<ClipboardTextExtractor> logger) : ITextExtractor
{
    public async Task<string?> GetPreviousTextAsync()
    {
        // Send ALT-TAB to switch to the previous window
        SendKeys.SendWait("%{TAB}");

        // A little bit of mini-delay is needed here, otherwise the text is not copied
        await Task.Delay(TimeSpan.FromMilliseconds(20));

        return await this.GetCurrentTextAsync();
    }

    public async Task<string?> GetCurrentTextAsync()
    {
        // We gotta wait until no keys are pressed anymore, otherwise CTRL+C will not work
        await this.WaitUntilNoKeyPressed();

        // Need to clear the clipboard, or otherwise we might get some previously copied text
        Clipboard.Clear();
        SendKeys.SendWait("^c");

        // It can take a while until the text is in the clipboard
        var selectedText = await this.WaitUntilClipboardTextIsAvailable();

        if (string.IsNullOrWhiteSpace(selectedText))
            return null;

        return selectedText;
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

    private async Task<string?> WaitUntilClipboardTextIsAvailable()
    {
        var watch = Stopwatch.StartNew();

        var (success, text) = await TaskHelper.WaitUntil(ClipboardHasText, hotkeyOptions.CurrentValue.ClipboardDelayInMilliseconds);
        if (success)
        {
            logger.LogInformation("Early exit from WaitUntilClipboardIsFilled because we got some text from the clipboard (after {Time} ms)", watch.Elapsed.TotalMilliseconds);
        }
        else
        {
            logger.LogInformation("Sadly no text available in clipboard (after {Time} ms)", watch.Elapsed.TotalMilliseconds);
        }

        return text;

        (bool, string?) ClipboardHasText()
        {
            try
            {
                var dataObject = Clipboard.GetDataObject();
                return dataObject?.GetData(DataFormats.Text) is string s
                    ? (true, s)
                    : (false, null);
            }
#pragma warning disable CA1031
            catch (Exception e)
#pragma warning restore CA1031
            {
                logger.LogError(e, "An error occurred when accessing the clipboard contents.");
                return (false, null);
            }
        }
    }
}
