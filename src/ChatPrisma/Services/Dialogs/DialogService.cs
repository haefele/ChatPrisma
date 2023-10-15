using System.Windows;
using System.Windows.Data;
using ChatPrisma.Options;
using ChatPrisma.Themes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.Dialogs;

public class DialogService(IServiceProvider serviceProvider, IOptions<ApplicationOptions> applicationOptions) : IDialogService
{
    public async Task<bool?> ShowDialog(object viewModel)
    {
        var viewType = this.ResolveViewType(viewModel);

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
            closeWindow.Close += (_, e) =>
            {
                // This automatically closes the window
                window.DialogResult = e.DialogResult;
            };
        }

        if (viewModel is IConfigureWindow configureWindow)
        {
            configureWindow.Configure(window);
        }

        if (viewModel is IInitialize initialize)
        {
            await initialize.InitializeAsync();
        }

        return window.ShowDialog();
    }

    private Type ResolveViewType(object viewModel)
    {
        var viewTypeFullName = viewModel.GetType().FullName?.Replace("ViewModel", "View", StringComparison.OrdinalIgnoreCase);
        var viewType = viewModel.GetType().Assembly.GetType(viewTypeFullName ?? string.Empty);

        return viewType is not null
            ? viewType
            : throw new PrismaException($"Couldn't find view for view-model {viewModel.GetType().Name}");
    }

    private void InvokeInitializeComponents(FrameworkElement view)
    {
        var initializeComponent = view.GetType().GetMethod("InitializeComponent");
        initializeComponent?.Invoke(view, null);
    }
}
