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
        keyboardHooks.TextEnhancementHotkeyPressed += this.KeyboardHooksOnTextEnhancementHotkeyPressed;
        keyboardHooks.ChatHotkeyPressed += this.KeyboardHooksOnChatHotkeyPressed;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        keyboardHooks.TextEnhancementHotkeyPressed -= this.KeyboardHooksOnTextEnhancementHotkeyPressed;
        keyboardHooks.ChatHotkeyPressed -= this.KeyboardHooksOnChatHotkeyPressed;
        return Task.CompletedTask;
    }

    private async void KeyboardHooksOnTextEnhancementHotkeyPressed(object? sender, EventArgs e)
    {
        var text = await textExtractor.GetCurrentTextAsync();
        if (text is null)
            return;

        var textEnhancementViewModel = viewModelFactory.CreateTextEnhancementViewModel(text);
        await dialogService.ShowDialog(textEnhancementViewModel);
    }

    private async void KeyboardHooksOnChatHotkeyPressed(object? sender, EventArgs e)
    {
        var chatViewModel = viewModelFactory.CreateChatViewModel();
        await dialogService.ShowDialog(chatViewModel);
    }
}
