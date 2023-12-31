using System.Windows;
using ChatPrisma.Options;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.TextWriter;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.TextEnhancement;

public partial class TextEnhancementViewModel(string inputText, IClipboardTextWriter clipboardTextWriter, IChatBotService chatBotService, IOptionsMonitor<TextEnhancementOptions> textEnhancementOptions) : ObservableObject, ICloseWindow, IConfigureWindow
{
    private List<PrismaChatMessage> _allMessages = new()
    {
        new PrismaChatMessage(PrismaChatRole.System, ChatPrompts.System(textEnhancementOptions.CurrentValue.CustomInstructions)),
        new PrismaChatMessage(PrismaChatRole.User, inputText),
    };

    [ObservableProperty]
    private bool _autoPaste = true;

    [ObservableProperty]
    private string _currentText = inputText;

    [ObservableProperty]
    private string _instruction = string.Empty;

    [ObservableProperty]
    private int _textSize = textEnhancementOptions.CurrentValue.TextSize;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PreviousVersionCommand))]
    [NotifyCanExecuteChangedFor(nameof(NextVersionCommand))]
    private int _currentVersion = 1;

    partial void OnCurrentVersionChanged(int value)
    {
        this.CurrentText = this._allMessages[(value * 2) - 1].Content;
    }

    [RelayCommand(CanExecute = nameof(CanPreviousVersion))]
    private void PreviousVersion()
    {
        if (this.ApplyInstructionCommand.IsRunning)
            return;

        this.CurrentVersion--;
    }
    private bool CanPreviousVersion()
    {
        return this.CurrentVersion > 1;
    }

    [RelayCommand(CanExecute = nameof(CanNextVersion))]
    private void NextVersion()
    {
        if (this.ApplyInstructionCommand.IsRunning)
            return;

        this.CurrentVersion++;
    }
    private bool CanNextVersion()
    {
        return this.CurrentVersion < (this._allMessages.Count / 2);
    }

    public event EventHandler? ApplyInstructionCancelled;

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task ApplyInstruction(CancellationToken token)
    {
        // Remember those, if the user cancels the operation we can reset the UI
        var previousInstruction = this.Instruction;
        var previousText = this.CurrentText;
        var removedMessages = this._allMessages.Skip(this.CurrentVersion * 2).ToList();
        this._allMessages.RemoveRange(this.CurrentVersion * 2, removedMessages.Count);

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
            this.CurrentVersion++;
        }
        catch (OperationCanceledException)
        {
            // The user cancelled the operation, so reset everything
            this._allMessages.Remove(this._allMessages[^1]);
            this.Instruction = previousInstruction;
            this.CurrentText = previousText;
            this._allMessages.AddRange(removedMessages);

            this.ApplyInstructionCancelled?.Invoke(this, EventArgs.Empty);
        }
    }

    [RelayCommand]
    private async Task AcceptText()
    {
        // Read it before we close the window, because that will deactivate the window and set AutoPaste to false
        var autoPaste = this.AutoPaste;

        this.Close?.Invoke(this, new CloseDialogEventArgs(true));

        await clipboardTextWriter.CopyTextAsync(this.CurrentText, autoPaste);
    }

    public event EventHandler<CloseDialogEventArgs>? Close;
    public void Configure(Window window)
    {
        window.Deactivated += (_, _) =>
        {
            this.AutoPaste = false;
        };
    }
}
