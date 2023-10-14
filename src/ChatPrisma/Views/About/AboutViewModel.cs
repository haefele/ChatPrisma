using System.Collections.ObjectModel;
using ChatPrisma.Options;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.About;

public partial class AboutViewModel(IOptions<ApplicationOptions> options) : ObservableObject
{
    [ObservableProperty] 
    private string _applicationName = options.Value.ApplicationName;

    [ObservableProperty]
    private string _applicationVersion = options.Value.ApplicationVersion;

    [ObservableProperty]
    private string _contactName = options.Value.ContactName;

    [ObservableProperty]
    private string _contactEmailAddress = options.Value.ContactEmailAddress;

    [ObservableProperty] 
    private ObservableCollection<ThirdPartyLibrary> _thirdPartyLibraries = new()
    {
        new ThirdPartyLibrary(".NET",                                 "https://dotnet.microsoft.com/",                                                "MIT",          "https://github.com/dotnet/core/blob/main/LICENSE.TXT"),
        new ThirdPartyLibrary("CommunityToolkit.Mvvm",                "https://github.com/CommunityToolkit/dotnet",                                   "MIT",          "https://github.com/CommunityToolkit/dotnet/blob/main/License.md"),
        new ThirdPartyLibrary("Azure OpenAI client library for .NET", "https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.openai-readme", "MIT",          "https://github.com/Azure/azure-sdk-for-net/blob/main/LICENSE.txt"),
        new ThirdPartyLibrary("FluentIcons.WPF",                      "https://github.com/davidxuang/FluentIcons",                                    "MIT",          "https://github.com/davidxuang/FluentIcons/blob/master/LICENSE"),
        new ThirdPartyLibrary("GlobalKeyInterceptor",                 "https://github.com/arcanexhoax/GlobalKeyInterceptor",                          "MIT",          "https://github.com/arcanexhoax/GlobalKeyInterceptor/blob/main/LICENSE"),
        new ThirdPartyLibrary("NLog",                                 "https://nlog-project.org/",                                                    "BSD 3-Clause", "https://github.com/NLog/NLog/blob/dev/LICENSE.txt"),
        new ThirdPartyLibrary("Hardcodet WPF NotifyIcon",             "https://github.com/hardcodet/wpf-notifyicon",                                  "CPOL",         "https://github.com/hardcodet/wpf-notifyicon/blob/develop/LICENSE"),
        new ThirdPartyLibrary("Emoji.Wpf",                            "https://github.com/samhocevar/emoji.wpf",                                      "WTFPL",        "https://github.com/samhocevar/emoji.wpf/blob/main/COPYING"),
        
    };
}

public record ThirdPartyLibrary(string Name, string HomepageUrl, string LicenseName, string LicenseUrl);