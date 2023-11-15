namespace ChatPrisma.Common;

public static class TaskHelper
{
    public static async Task<bool> WaitUntil(Func<bool> condition, int timeoutInMilliseconds)
    {
        var result = await WaitUntil(() => (condition(), string.Empty), timeoutInMilliseconds);
        return result.Success;
    }
    public static async Task<(bool Success, T? Data)> WaitUntil<T>(Func<(bool, T)> condition, int timeoutInMilliseconds)
    {
        using var cancellationTokenSource = new CancellationTokenSource(timeoutInMilliseconds);
        var cancellationToken = cancellationTokenSource.Token;

        try
        {
            // Wait until we reach the timeout or condition is true
            while (true)
            {
                var (success, data) = condition();
                if (success)
                    return (true, data);

                // This delay is cancellable and will throw an exception if the token is cancelled.
                await Task.Delay(10, cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            return (Success: false, Data: default);
        }
    }
}
