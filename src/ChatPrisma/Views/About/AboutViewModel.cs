using System.Collections.ObjectModel;
using ChatPrisma.Options;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.About;

public partial class AboutViewModel(IOptionsMonitor<ApplicationOptions> options) : ObservableObject
{
    [ObservableProperty]
    private string _applicationName = options.CurrentValue.ApplicationName;

    [ObservableProperty]
    private string _applicationVersion = options.CurrentValue.ApplicationVersion;

    [ObservableProperty]
    private string _commitId = options.CurrentValue.CommitId;

    [ObservableProperty]
    private string _contactName = options.CurrentValue.ContactName;

    [ObservableProperty]
    private string _contactEmailAddress = options.CurrentValue.ContactEmailAddress;

    [ObservableProperty]
    private ObservableCollection<ThirdPartyLibrary> _thirdPartyLibraries = new()
    {
        new ThirdPartyLibrary(".NET",                                 new("https://dotnet.microsoft.com/"),                                                "MIT",          new Uri("https://github.com/dotnet/core/blob/main/LICENSE.TXT")),
        new ThirdPartyLibrary("CommunityToolkit.Mvvm",                new("https://github.com/CommunityToolkit/dotnet"),                                   "MIT",          new Uri("https://github.com/CommunityToolkit/dotnet/blob/main/License.md")),
        new ThirdPartyLibrary("Azure OpenAI client library for .NET", new("https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.openai-readme"), "MIT",          new Uri("https://github.com/Azure/azure-sdk-for-net/blob/main/LICENSE.txt")),
        new ThirdPartyLibrary("FluentIcons.WPF",                      new("https://github.com/davidxuang/FluentIcons"),                                    "MIT",          new Uri("https://github.com/davidxuang/FluentIcons/blob/master/LICENSE")),
        new ThirdPartyLibrary("GlobalKeyInterceptor",                 new("https://github.com/arcanexhoax/GlobalKeyInterceptor"),                          "MIT",          new Uri("https://github.com/arcanexhoax/GlobalKeyInterceptor/blob/main/LICENSE")),
        new ThirdPartyLibrary("NLog",                                 new("https://nlog-project.org/"),                                                    "BSD 3-Clause", new Uri("https://github.com/NLog/NLog/blob/dev/LICENSE.txt")),
        new ThirdPartyLibrary("Hardcodet WPF NotifyIcon",             new("https://github.com/hardcodet/wpf-notifyicon"),                                  "CPOL",         new Uri("https://github.com/hardcodet/wpf-notifyicon/blob/develop/LICENSE")),
        new ThirdPartyLibrary("Emoji.Wpf",                            new("https://github.com/samhocevar/emoji.wpf"),                                      "WTFPL",        new Uri("https://github.com/samhocevar/emoji.wpf/blob/main/COPYING")),
        new ThirdPartyLibrary("DevExpress.Mvvm",                      new("https://github.com/DevExpress/DevExpress.Mvvm.Free"),                           "MIT",          new Uri("https://github.com/DevExpress/DevExpress.Mvvm.Free/blob/main/LICENSE")),
        new ThirdPartyLibrary("Nerdbank.GitVersioning",               new("https://github.com/dotnet/Nerdbank.GitVersioning"),                             "MIT",          new Uri("https://github.com/dotnet/Nerdbank.GitVersioning/blob/main/LICENSE")),
        new ThirdPartyLibrary("SingleInstanceCore",                   new("https://github.com/soheilkd/SingleInstanceCore"),                               "MIT",          new Uri("https://github.com/soheilkd/SingleInstanceCore/blob/master/LICENSE")),
        new ThirdPartyLibrary("Onova",                                new("https://github.com/Tyrrrz/Onova"),                                              "MIT",          new Uri("https://github.com/Tyrrrz/Onova/blob/master/License.txt")),
    };
}

public record ThirdPartyLibrary(string Name, Uri HomepageUrl, string LicenseName, Uri LicenseUrl);
