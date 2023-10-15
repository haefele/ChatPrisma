using ChatPrisma.Options;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

namespace ChatPrisma.Services.AutoStart;

public class RegistryAutoStartService(IOptions<ApplicationOptions> applicationOptions) : IAutoStartService
{
    public async Task<bool> IsInAutoStart()
    {
        await Task.CompletedTask;

        var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: false);
        var result = key?.GetValue(applicationOptions.Value.ApplicationName) is not null;

        // If auto-start is enabled, ensure that we have the correct application path in the registry
        // We do that by just enabling auto-start again
        if (result is true)
        {
            await this.SetAutoStart(true);
        }

        return result;
    }

    public async Task SetAutoStart(bool enabled)
    {
        await Task.CompletedTask;

        var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true) ??
                  Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

        if (enabled)
        {
            key.SetValue(applicationOptions.Value.ApplicationName, $"\"{Environment.ProcessPath}\"");
        }
        else
        {
            key.DeleteValue(applicationOptions.Value.ApplicationName, throwOnMissingValue: false);
        }
    }
}
