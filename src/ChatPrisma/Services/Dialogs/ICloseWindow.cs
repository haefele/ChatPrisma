namespace ChatPrisma.Services.Dialogs;

public interface ICloseWindow
{
    event Action<bool?>? Close;
}