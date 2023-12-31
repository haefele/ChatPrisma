﻿using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ChatPrisma.Host;
using ChatPrisma.HostedServices;
using ChatPrisma.Options;
using ChatPrisma.Resources;
using ChatPrisma.Services.AutoStart;
using ChatPrisma.Services.ChatBot;
using ChatPrisma.Services.Dialogs;
using ChatPrisma.Services.KeyboardHooks;
using ChatPrisma.Services.TextExtractor;
using ChatPrisma.Services.TextWriter;
using ChatPrisma.Services.UpdateOptions;
using ChatPrisma.Services.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Hosting;
using NLog.Extensions.Logging;
using Onova;
using Onova.Models;
using Onova.Services;
using SingleInstanceCore;

namespace ChatPrisma;

public partial class App : ISingleInstance
{
    private IHost? _host;

    public App()
    {
        // Setup culture
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag)));
    }

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
        try
        {
            // Allow only a single instance of this application to run at a time
            if (SingleInstance.InitializeAsFirstInstance(this, "Chat Prisma") is false)
            {
                this.Shutdown();
                return;
            }

            // Create and start host
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

            SingleInstance.Cleanup();
        }
    }

    private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var logger = this._host?.Services.GetRequiredService<ILogger<App>>();
        logger?.LogError(e.Exception, "An unhandled exception occurred.");

        e.Handled = true;

        // TODO: Show error message
    }

    public void OnInstanceInvoked(string[] args)
    {
        // Right now we don't care about startup args
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
                    o.AppShutdownHeader = Strings.Shutdown;
                });
            services.AddOptions<OpenAIOptions>()
                .BindConfiguration(OpenAIOptions.Section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<ApplicationOptions>()
                .Configure(o =>
                {
                    o.ApplicationName = "Chat Prisma";
                    o.ApplicationVersion = Version.Parse(ThisAssembly.AssemblyVersion).ToString(3); // Remove the last 0 from the version number
                    o.CommitId = ThisAssembly.GitCommitId;
                    o.IsPublicVersion = ThisAssembly.IsPublicRelease;
                    o.ContactName = "Daniel Häfele";
                    o.ContactEmailAddress = "haefele@xemio.net";
                    o.GitHubLink = "https://github.com/haefele/ChatPrisma";
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<UpdaterOptions>()
                .Configure(o =>
                {
                    o.GitHubUsername = "haefele";
                    o.GitHubRepository = "ChatPrisma";
                    o.GitHubReleaseAssetName = "App.zip";
                    o.CheckForUpdatesInBackground = true;
                    o.MinutesBetweenUpdateChecks = 30;
                })
                .BindConfiguration(UpdaterOptions.Section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<HotkeyOptions>()
                .Configure(o =>
                {
                    o.Key = "Y";
                    o.KeyModifiers = "Ctrl+Shift+Alt";
                    o.HotkeyDelayInMilliseconds = 500;
                    o.ClipboardDelayInMilliseconds = 500;
                })
                .BindConfiguration(HotkeyOptions.Section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<TextEnhancementOptions>()
                .Configure(o =>
                {
                    o.TextSize = 12;
                })
                .BindConfiguration(TextEnhancementOptions.Section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // Services
            services.AddSingleton<IKeyboardHooks, GlobalKeyInterceptorKeyboardHooks>();
            services.AddSingleton<ITextExtractor, ClipboardTextExtractor>();
            services.AddSingleton<IClipboardTextWriter, SendKeysClipboardTextWriter>();
            services.AddSingleton<IChatBotService, OpenAIChatBotService>();
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<AssemblyMetadata>(serviceProvider =>
            {
                var applicationOptions = serviceProvider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                return new AssemblyMetadata(applicationOptions.ApplicationName, Version.Parse(applicationOptions.ApplicationVersion), Environment.ProcessPath!);
            });
            services.AddSingleton<IPackageResolver>(serviceProvider =>
            {
                var updaterOptions = serviceProvider.GetRequiredService<IOptions<UpdaterOptions>>().Value;
                return new GithubPackageResolver(updaterOptions.GitHubUsername, updaterOptions.GitHubRepository, updaterOptions.GitHubReleaseAssetName);
            });
            services.AddSingleton<IPackageExtractor, ZipPackageExtractor>();
            services.AddSingleton<IUpdateManager, UpdateManager>();
            services.AddSingleton<IAutoStartService, RegistryAutoStartService>();
            services.AddSingleton<IUpdateOptionsService, UpdateOptionsService>();

            // Hosted Services
            services.AddHostedService<StartKeyboardHooksHostedService>();
            services.AddHostedService<PrismaHostedService>();
            services.AddHostedService<UpdaterHostedService>();
        })
        .UseNLog(new NLogProviderOptions
        {
            ReplaceLoggerFactory = true
        });
}
