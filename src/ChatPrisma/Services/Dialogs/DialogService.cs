using System.Windows;
using System.Windows.Data;
using ChatPrisma.Options;
using ChatPrisma.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.Dialogs;

public class DialogService(IServiceProvider serviceProvider, IOptions<ApplicationOptions> applicationOptions) : IDialogService
{
    public bool? ShowDialog(object viewModel)
    {
        var viewType = this.TryResolveViewType(viewModel);

        if (viewType is null)
            throw new PrismaException();

        var view = (FrameworkElement)ActivatorUtilities.CreateInstance(serviceProvider, viewType);
        this.InvokeInitializeComponents(view);
        view.DataContext = viewModel;

        var window = new Window
        {
            Content = view,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            SizeToContent = SizeToContent.WidthAndHeight,
            ResizeMode = ResizeMode.NoResize,
        };

        window.SetBinding(Window.TitleProperty, new Binding
        {
            Path = new PropertyPath(Attached.WindowTitleProperty),
            Source = view,
            FallbackValue = applicationOptions.Value.ApplicationName,
        });

        if (viewModel is ICloseWindow closeWindow)
        {
            closeWindow.Close += dialogResult =>
            {
                // This automatically closes the window
                window.DialogResult = dialogResult;
            };
        }

        if (viewModel is IConfigureWindow configureWindow)
        {
            configureWindow.Configure(window);
        }
        
        return window.ShowDialog();
    }

    private Type? TryResolveViewType(object viewModel)
    {
        var viewTypeFullName = viewModel.GetType().FullName?.Replace("ViewModel", "View");
        return viewModel.GetType().Assembly.GetType(viewTypeFullName ?? string.Empty);
    }

    private void InvokeInitializeComponents(FrameworkElement view)
    {
        var initializeComponent = view.GetType().GetMethod("InitializeComponent");
        initializeComponent?.Invoke(view, null);
    }
}