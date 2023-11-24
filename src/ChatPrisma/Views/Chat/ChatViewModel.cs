using System.Collections.ObjectModel;
using ChatPrisma.Options;
using ChatPrisma.Services.ChatBot;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.Chat;

public partial class ChatViewModel(IChatBotService chatBotService, IOptionsMonitor<TextEnhancementOptions> textEnhancementOptions) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<MessageViewModel> _messages = new();

    [ObservableProperty]
    private string _nextMessage = string.Empty;

    [ObservableProperty]
    private int _textSize = textEnhancementOptions.CurrentValue.TextSize;

    [RelayCommand]
    private async Task SendMessage()
    {
        this.Messages.Add(new MessageViewModel(PrismaChatRole.User, this.NextMessage));
        this.NextMessage = string.Empty;

        var response = chatBotService.GetResponse(this.Messages.Select(f => new PrismaChatMessage(f.Role, f.Content)).ToList());
        var responseMessage = new MessageViewModel(response);
        this.Messages.Add(responseMessage);

        await responseMessage.WaitUntilFinished();
    }
}

public partial class MessageViewModel : ObservableObject
{
    private readonly IAsyncEnumerable<string>? _contentEnumerable;

    [ObservableProperty]
    private PrismaChatRole _role;
    [ObservableProperty]
    private string _content;

    public MessageViewModel(PrismaChatRole role, string content)
    {
        this.Role = role;
        this.Content = content;
    }

    public MessageViewModel(IAsyncEnumerable<string> response)
    {
        this.Role = PrismaChatRole.Assistant;
        this.Content = string.Empty;

        this._contentEnumerable = response;
    }

    public async Task WaitUntilFinished()
    {
        if (this._contentEnumerable is null)
            return;

        await foreach (var part in this._contentEnumerable)
        {
            this.Content += part;
        }
    }
}
