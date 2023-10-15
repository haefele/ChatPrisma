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

    private bool _alreadyShowing;

    private async void KeyboardHooksOnCombinationPressed(object? sender, EventArgs e)
    {
        // Only allow one text to be enhanced at a time
        if (this._alreadyShowing)
            return;

        var text = await textExtractor.GetCurrentTextAsync();
        if (text is null)
            return;

        this._alreadyShowing = true;
        try
        {
            var textEnhancementViewModel = viewModelFactory.CreateTextEnhancementViewModel(text);
            await dialogService.ShowDialog(textEnhancementViewModel);
        }
        finally
        {
            this._alreadyShowing = false;
        }
    }
}
