using System.Collections.ObjectModel;
using System.Windows;
using ChatPrisma.Options;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.TextWriter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.Chat;

public partial class ChatViewModel(IChatBotService chatBotService, IOptionsMonitor<TextEnhancementOptions> textEnhancementOptions) : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<PrismaChatMessage> _messages = new();

    [ObservableProperty]
    private string _nextMessage = string.Empty;

    [ObservableProperty]
    private int _textSize = textEnhancementOptions.CurrentValue.TextSize;

    [RelayCommand]
    private async Task SendMessage()
    {
        this.Messages.Add(new PrismaChatMessage(PrismaChatRole.User, this.NextMessage));
        this.NextMessage = string.Empty;

        var response = chatBotService.GetResponse(this.Messages.ToList());

        var responseMessage = string.Empty;
        await foreach (var part in response)
        {
            responseMessage += part;
        }

        this.Messages.Add(new PrismaChatMessage(PrismaChatRole.Assistant, responseMessage));
    }
}
