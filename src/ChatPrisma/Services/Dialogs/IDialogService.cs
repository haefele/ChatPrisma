namespace ChatPrisma.Services.Dialogs;

public interface IDialogService
{
    Task<bool?> ShowDialog(object viewModel);
}
