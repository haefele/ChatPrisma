using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.KeyboardHooks;
using ChatPrisma.Services.TextExtractor;
using ChatPrisma.Services.ViewModels;
using Microsoft.Extensions.Hosting;

namespace ChatPrisma.HostedServices;

public class PrismaHostedService(IKeyboardHooks keyboardHooks, ITextExtractor textExtractor, IDialogService dialogService, IViewModelFactory viewModelFactory) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        keyboardHooks.CombinationPressed += this.KeyboardHooksOnCombinationPressed;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        keyboardHooks.CombinationPressed -= this.KeyboardHooksOnCombinationPressed;
        return Task.CompletedTask;
    }

    private async void KeyboardHooksOnCombinationPressed(object? sender, EventArgs e)
    {
        var text = await textExtractor.GetCurrentTextAsync();
        if (text is null)
            return;

        var textEnhancementViewModel = viewModelFactory.CreateTextEnhancementViewModel(text);
        await dialogService.ShowDialog(textEnhancementViewModel);
    }
}
