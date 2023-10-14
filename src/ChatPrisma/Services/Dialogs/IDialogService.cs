using System.Windows;
using ChatPrisma.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.Dialogs;

public interface IDialogService
{
    void ShowWindow(object viewModel);
}

public class DialogService(IServiceProvider serviceProvider, IOptions<ApplicationOptions> applicationOptions) : IDialogService
{
    public void ShowWindow(object viewModel)
    {
        var viewType = this.TryResolveViewType(viewModel);

        if (viewType is null)
            throw new PrismaException();

        var view = (FrameworkElement)ActivatorUtilities.CreateInstance(serviceProvider, viewType);
        view.DataContext = viewModel;

        var window = new Window
        {
            Content = view,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode = ResizeMode.NoResize,
            Title = applicationOptions.Value.ApplicationName,
        };
        window.Show();
    }

    private Type? TryResolveViewType(object viewModel)
    {
        var viewTypeFullName = viewModel.GetType().FullName?.Replace("ViewModel", "View");
        return viewModel.GetType().Assembly.GetType(viewTypeFullName ?? string.Empty);
    }
}