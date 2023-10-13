using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChatPrisma.Views.About;

public partial class AboutViewModel : ObservableObject
{
    [ObservableProperty] 
    private string _applicationName = "Heyo";

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
        
    };
}

public record ThirdPartyLibrary(string Name, string HomepageUrl, string LicenseName, string LicenseUrl);