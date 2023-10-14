using System.Windows;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.TextWriter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementViewModel(string inputText, IClipboardTextWriter clipboardTextWriter, IChatBotService chatBotService) : ObservableObject, ICloseWindow, IConfigureWindow
{
    private List<PrismaChatMessage> _allMessages = new()
    {
        new PrismaChatMessage(PrismaChatRole.System, ChatPrompts.System(inputText))
    };

    [ObservableProperty]
    private bool _windowDeactivated = false;
    
    [ObservableProperty] 
    private string _currentText = inputText;

    [ObservableProperty] 
    private string _instruction = string.Empty;

    public event EventHandler? ApplyInstructionCancelled;

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task ApplyInstruction(CancellationToken token)
    {
        // Remember those, if the user cancels the operation we can reset the UI
        string previousInstruction = this.Instruction;
        string previousText = this.CurrentText;

        try
        {
            this._allMessages.Add(new PrismaChatMessage(PrismaChatRole.User, this.Instruction));
            this.Instruction = string.Empty;

            var response = chatBotService.GetResponse(this._allMessages, token);

            this.CurrentText = string.Empty;
            await foreach (var part in response)
            {
                this.CurrentText += part;
            }
            
            this._allMessages.Add(new PrismaChatMessage(PrismaChatRole.Assistant, this.CurrentText));
        }
        catch (OperationCanceledException)
        {
            // The user cancelled the operation, so reset everything
            this._allMessages.Remove(this._allMessages[^1]);
            this.Instruction = previousInstruction;
            this.CurrentText = previousText;
            
            this.ApplyInstructionCancelled?.Invoke(this, EventArgs.Empty);
        }
    }

    [RelayCommand]
    private async Task AcceptText()
    {
        // Read it before we close the window, because that will deactivate the window and set it to true
        bool windowDeactivated = this.WindowDeactivated;
        
        this.Close?.Invoke();

        await clipboardTextWriter.CopyTextAsync(this.CurrentText, autoPaste: windowDeactivated is false);
    }

    public event Action? Close;
    public void Configure(Window window)
    {
        window.Deactivated += this.WindowOnDeactivated;
    }

    private void WindowOnDeactivated(object? sender, EventArgs e)
    {
        this.WindowDeactivated = true;
    }
}