using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Input;
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

        var selectedText = Clipboard.GetText();

        if (string.IsNullOrWhiteSpace(selectedText))
            return null;

        return selectedText;
    }

    private async Task WaitUntilNoKeyPressed()
    {
        var task = Task.Delay(TimeSpan.FromMilliseconds(hotkeyOptions.CurrentValue.HotkeyDelayInMilliseconds));
        var watch = Stopwatch.StartNew();

        // Either wait until the task is completed or the user releases all keys
        while (task.IsCompleted is false)
        {
            if (AnyKeyPressed() is false)
            {
                logger.LogInformation("Early exit from WaitUntilNoKeyPressed because no key is pressed anymore (after {Time} ms)", watch.Elapsed.TotalMilliseconds);
                return;
            }

            await Task.Delay(10);
        }

        logger.LogInformation("Sadly the user did not release all keys in time (after {Time} ms)", watch.Elapsed.TotalMilliseconds);
    }

    private static readonly Key[] s_allKeys = Enum.GetValues<Key>();
    private static bool AnyKeyPressed()
    {
        foreach (var key in s_allKeys)
        {
            // Skip the None key
            if (key == Key.None)
                continue;

            if (Keyboard.IsKeyDown(key))
                return true;
        }

        return false;
    }
}
