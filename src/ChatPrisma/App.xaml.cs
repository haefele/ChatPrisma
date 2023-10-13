using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ChatPrisma.Host;
using ChatPrisma.HostedServices;
using ChatPrisma.Services;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.KeyboardHooks;
using ChatPrisma.Services.TextExtractor;
using ChatPrisma.Services.TextWriter;
using ChatPrisma.Services.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace ChatPrisma;

public partial class App
{
    private IHost? _host;

    public new static App Current => (App)System.Windows.Application.Current;

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {        
        try
        {
            this._host = this.CreateHostBuilder(e.Args).Build();
            await this._host.StartAsync();

            var logger = this._host.Services.GetRequiredService<ILogger<App>>();
            var environment = this._host.Services.GetRequiredService<IHostEnvironment>();
            
            logger.LogInformation("Application started! Environment: {Environment}", environment.EnvironmentName);
        }
        catch (Exception exception)
        {
            // TODO: Improve
            MessageBox.Show("Etwas ist schief gelaufen. Anwendung konnte nicht gestartet werden." + Environment.NewLine + exception.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Shutdown();
        }
    }

    private async void App_OnExit(object sender, ExitEventArgs e)
    {
        if (this._host is not null)
        {
            await this._host.StopAsync();
            this._host.Dispose();
        }
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var logger = this._host?.Services.GetRequiredService<ILogger<App>>();
        logger?.LogError(e.Exception, "An unhandled exception occurred.");
    }
    
    private IHostBuilder CreateHostBuilder(string[] args) => Microsoft.Extensions.Hosting.Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            // Host
            services.AddSingleton<Application>(this);
            services.AddSingleton<IHostLifetime, TrayIconLifetime>();
            
            // Config
            services.Configure<TrayIconLifetimeOptions>(o =>
            {
                o.ToolTipText = "Chat Prisma";
                o.MouseDoubleClickAction = WindowsTray.HandleDoubleClick;
                o.ContextMenuFactory = WindowsTray.CreateContextMenu;
            });
            services.Configure<OpenAIConfig>(context.Configuration.GetSection("OpenAI"));
            
            // Services
            services.AddSingleton<IKeyboardHooks, GlobalKeyInterceptorKeyboardHooks>();
            services.AddSingleton<ITextExtractor, ClipboardTextExtractor>();
            services.AddSingleton<ITextWriter, SendKeysTextWriter>();
            services.AddSingleton<IChatBotService, OpenAIChatBotService>();
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<IDialogService, DialogService>();

            // Hosted Services
            services.AddHostedService<StartKeyboardHooksHostedService>();
            services.AddHostedService<PrismaHostedService>();
        })
        .UseNLog();
}