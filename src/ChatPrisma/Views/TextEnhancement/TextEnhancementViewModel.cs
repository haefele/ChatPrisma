using System.Collections.ObjectModel;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.TextWriter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementViewModel : ObservableObject, ICloseWindow
{
    private readonly ITextWriter _textWriter;

    public TextEnhancementViewModel(string inputText, ITextWriter textWriter)
    {
        _textWriter = textWriter;
        
        this._messages.Add(new MessageViewModel
        {
            Message = inputText
        });
    }

    [ObservableProperty] 
    private ObservableCollection<MessageViewModel> _messages = new();

    [ObservableProperty] 
    private string _instruction = string.Empty;

    [RelayCommand]
    private async Task ApplyInstruction()
    {
        await Task.CompletedTask;
        
        this.Messages.Add(new MessageViewModel
        {
            Message = this.Instruction,
        });
        
        this.Instruction = string.Empty;
    }

    [RelayCommand]
    private async Task AcceptText()
    {
        this.Close?.Invoke();

        // Give the user some time to take their hands off the keyboard
        await Task.Delay(TimeSpan.FromMilliseconds(200));
        
        await this._textWriter.WriteTextAsync(this.Get());
    }

    private async IAsyncEnumerable<string> Get()
    {
        await Task.CompletedTask;
        
        yield return this.Messages.Last().Message;
    }

    public event Action? Close;
}

public partial class MessageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _message;
}