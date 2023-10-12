using System.Windows;
using System.Windows.Threading;
using ChatPrisma.Configuration;
using ChatPrisma.Services;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.KeyboardHooks;
using ChatPrisma.Services.TextExtractor;
using ChatPrisma.Services.TextWriter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Hosting;
using MessageBox = System.Windows.MessageBox;

namespace ChatPrisma;

public partial class App
{
    private IHost? _host;
    public IHost Host => this._host ?? throw new PrismaException();
    public IServiceProvider Services => this.Host.Services;

    public new static App Current => (App)System.Windows.Application.Current;

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {        
        try
        {
            this._host = this.CreateHostBuilder(e.Args).Build();
            await this._host.StartAsync();

            var logger = this.Services.GetRequiredService<ILogger<App>>();
            var environment = this.Services.GetRequiredService<IHostEnvironment>();
            
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
        .CreateDefaultBuilder()
        .ConfigureHostConfiguration(c =>
        {
            c.SetBasePath(System.IO.Path.GetDirectoryName(AppContext.BaseDirectory) ?? throw new PrismaException());
        })
        .ConfigureAppConfiguration(c =>
        {
            c.SetBasePath(System.IO.Path.GetDirectoryName(AppContext.BaseDirectory) ?? throw new PrismaException());
        })
        .ConfigureServices((context, services) =>
        {
            // Config
            services.Configure<ConsoleLifetimeOptions>(o => o.SuppressStatusMessages = true);
            services.Configure<OpenAIConfig>(context.Configuration.GetSection("OpenAI"));
            
            // Services
            services.AddHostedService<GlobalKeyInterceptorHostedService>();
            services.AddSingleton<GlobalKeyInterceptorKeyboardHooks>();
            services.AddSingleton<IKeyboardHooks>(s => s.GetRequiredService<GlobalKeyInterceptorKeyboardHooks>());

            services.AddSingleton<ITextExtractor, ClipboardTextExtractor>();

            services.AddSingleton<ITextWriter, SendKeysTextWriter>();
            
            services.AddSingleton<IChatBotService, OpenAIChatBotService>();

            services.AddHostedService<PrismaHostedService>();
        })
        .UseNLog();
}