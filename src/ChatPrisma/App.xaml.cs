using System.Windows;
using System.Windows.Threading;
using ChatPrisma.Host;
using ChatPrisma.HostedServices;
using ChatPrisma.Options;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.KeyboardHooks;
using ChatPrisma.Services.TextExtractor;
using ChatPrisma.Services.TextWriter;
using ChatPrisma.Services.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;

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
#pragma warning disable CA1031
        catch (Exception exception)
#pragma warning restore CA1031
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

            // Options
            services.AddOptions<TrayIconLifetimeOptions>()
                .Configure(o =>
                {
                    o.MouseDoubleClickAction = WindowsTray.HandleDoubleClick;
                    o.ContextMenuFactory = WindowsTray.CreateContextMenu;
                    o.AppShutdownEnabled = true;
                    o.AppShutdownHeader = "Beenden";
                });
            services.AddOptions<OpenAIOptions>()
                .Bind(context.Configuration.GetSection("OpenAI"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<ApplicationOptions>()
                .Configure(o =>
                {
                    o.ApplicationName = "Chat Prisma";
                    o.ApplicationVersion = ThisAssembly.AssemblyInformationalVersion;
                    o.ContactName = "Daniel Häfele";
                    o.ContactEmailAddress = "haefele@xemio.net";
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // Services
            services.AddSingleton<IKeyboardHooks, GlobalKeyInterceptorKeyboardHooks>();
            services.AddSingleton<ITextExtractor, ClipboardTextExtractor>();
            services.AddSingleton<IClipboardTextWriter, SendKeysClipboardTextWriter>();
            services.AddSingleton<IChatBotService, OpenAIChatBotService>();
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<IDialogService, DialogService>();

            // Hosted Services
            services.AddHostedService<StartKeyboardHooksHostedService>();
            services.AddHostedService<PrismaHostedService>();
        })
        .UseNLog();
}
