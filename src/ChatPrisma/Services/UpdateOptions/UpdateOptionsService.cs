using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using ChatPrisma.Options;
using Microsoft.Extensions.Hosting;

namespace ChatPrisma.Services.UpdateOptions;

public class UpdateOptionsService(IHostEnvironment hostEnvironment) : IUpdateOptionsService
{
    public async Task Update(OpenAIOptions options)
    {
        await this.UpdateSettings(settings =>
        {
            var newJson = JsonSerializer.Serialize(options);
            settings[OpenAIOptions.Section] = JsonNode.Parse(newJson);
        });
    }

    private async Task UpdateSettings(Action<JsonNode> updateAction)
    {
        var path = Path.Combine(AppContext.BaseDirectory, $"appsettings.{hostEnvironment.EnvironmentName}.json");

        string settingsContent;
        try
        {
            settingsContent = await File.ReadAllTextAsync(path);
        }
        catch (FileNotFoundException)
        {
            settingsContent = "{}";
        }

        var json = JsonNode.Parse(settingsContent) ?? new JsonObject();

        updateAction(json);

        await using var writer = File.CreateText(path);
        await writer.WriteAsync(json.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }
}
