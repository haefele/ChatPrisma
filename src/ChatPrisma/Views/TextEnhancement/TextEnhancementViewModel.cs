using System.Collections.ObjectModel;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.TextWriter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementViewModel(string inputText, ITextWriter textWriter, IChatBotService chatBotService) : ObservableObject, ICloseWindow
{
    private List<PrismaChatMessage> _allMessages = new()
    {
        new PrismaChatMessage(PrismaChatRole.System, ChatPrompts.System(inputText))
    };
    
    [ObservableProperty] 
    private string _currentText = inputText;

    [ObservableProperty] 
    private string _instruction = string.Empty;

    [RelayCommand]
    private async Task ApplyInstruction()
    {
        this._allMessages.Add(new PrismaChatMessage(PrismaChatRole.User, this.Instruction));

        var response = chatBotService.GetResponse(this._allMessages);

        this.CurrentText = string.Empty;
        await foreach (var part in response)
        {
            this.CurrentText += part;
        }
        
        this._allMessages.Add(new PrismaChatMessage(PrismaChatRole.Assistant, this.CurrentText));
        this.Instruction = string.Empty;
    }

    [RelayCommand]
    private async Task AcceptText()
    {
        this.Close?.Invoke();
        
        await textWriter.WriteTextAsync(this.CurrentText);
    }

    public event Action? Close;
}