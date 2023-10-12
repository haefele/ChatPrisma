using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.KeyboardHooks;
using ChatPrisma.Services.TextExtractor;
using ChatPrisma.Services.TextWriter;
using Microsoft.Extensions.Hosting;

namespace ChatPrisma.Services;

public class PrismaHostedService(IKeyboardHooks keyboardHooks, ITextExtractor textExtractor, ITextWriter textWriter, IChatBotService chatBotService) : IHostedService
{
    public  Task StartAsync(CancellationToken cancellationToken)
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

        // TODO: Show Chat window and let user improve on the text

        var response = chatBotService.GetResponse(text, previousMessages: null);
        await textWriter.WriteTextAsync(response);
    }
}