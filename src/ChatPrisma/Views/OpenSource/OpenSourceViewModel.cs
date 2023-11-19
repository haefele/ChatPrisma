using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatPrisma.Options;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Views.OpenSource;

public partial class OpenSourceViewModel(IOptionsMonitor<ApplicationOptions> applicationOptions) : ObservableObject
{
    [ObservableProperty]
    private string _applicationName = applicationOptions.CurrentValue.ApplicationName;

    [ObservableProperty]
    private ObservableCollection<ThirdPartyLibrary> _thirdPartyLibraries = new()
    {
        new ThirdPartyLibrary(".NET",                                 new("https://dotnet.microsoft.com/"),                                                "MIT",            new Uri("https://github.com/dotnet/core/blob/main/LICENSE.TXT")),
        new ThirdPartyLibrary("CommunityToolkit.Mvvm",                new("https://github.com/CommunityToolkit/dotnet"),                                   "MIT",            new Uri("https://github.com/CommunityToolkit/dotnet/blob/main/License.md")),
        new ThirdPartyLibrary("Azure OpenAI client library for .NET", new("https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.openai-readme"), "MIT",            new Uri("https://github.com/Azure/azure-sdk-for-net/blob/main/LICENSE.txt")),
        new ThirdPartyLibrary("FluentIcons.WPF",                      new("https://github.com/davidxuang/FluentIcons"),                                    "MIT",            new Uri("https://github.com/davidxuang/FluentIcons/blob/master/LICENSE")),
        new ThirdPartyLibrary("GlobalKeyInterceptor",                 new("https://github.com/arcanexhoax/GlobalKeyInterceptor"),                          "MIT",            new Uri("https://github.com/arcanexhoax/GlobalKeyInterceptor/blob/main/LICENSE")),
        new ThirdPartyLibrary("NLog",                                 new("https://nlog-project.org/"),                                                    "BSD 3-Clause",   new Uri("https://github.com/NLog/NLog/blob/dev/LICENSE.txt")),
        new ThirdPartyLibrary("Hardcodet WPF NotifyIcon",             new("https://github.com/hardcodet/wpf-notifyicon"),                                  "CPOL",           new Uri("https://github.com/hardcodet/wpf-notifyicon/blob/develop/LICENSE")),
        new ThirdPartyLibrary("DevExpress.Mvvm",                      new("https://github.com/DevExpress/DevExpress.Mvvm.Free"),                           "MIT",            new Uri("https://github.com/DevExpress/DevExpress.Mvvm.Free/blob/main/LICENSE")),
        new ThirdPartyLibrary("Nerdbank.GitVersioning",               new("https://github.com/dotnet/Nerdbank.GitVersioning"),                             "MIT",            new Uri("https://github.com/dotnet/Nerdbank.GitVersioning/blob/main/LICENSE")),
        new ThirdPartyLibrary("SingleInstanceCore",                   new("https://github.com/soheilkd/SingleInstanceCore"),                               "MIT",            new Uri("https://github.com/soheilkd/SingleInstanceCore/blob/master/LICENSE")),
        new ThirdPartyLibrary("Onova",                                new("https://github.com/Tyrrrz/Onova"),                                              "MIT",            new Uri("https://github.com/Tyrrrz/Onova/blob/master/License.txt")),
        new ThirdPartyLibrary("SharpVectors",                         new("https://elinamllc.github.io/SharpVectors/"),                                    "BSD 3-Clause",   new Uri("https://github.com/ElinamLLC/SharpVectors/blob/master/License.md")),
        new ThirdPartyLibrary("unDraw",                               new("https://undraw.co/"),                                                           "unDraw License", new Uri("https://undraw.co/license")),
    };
}

public record ThirdPartyLibrary(string Name, Uri HomepageUrl, string LicenseName, Uri LicenseUrl);
