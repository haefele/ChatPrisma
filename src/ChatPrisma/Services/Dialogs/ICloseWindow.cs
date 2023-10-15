namespace ChatPrisma.Services.Dialogs;

public interface ICloseWindow
{
    event EventHandler<CloseDialogEventArgs>? Close;
}

public class CloseDialogEventArgs(bool? dialogResult) : EventArgs
{
    public bool? DialogResult { get; set; } = dialogResult;
}
