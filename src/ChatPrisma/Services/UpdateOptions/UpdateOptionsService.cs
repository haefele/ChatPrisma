using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using ChatPrisma.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ChatPrisma.Services.UpdateOptions;

public class UpdateOptionsService(IHostEnvironment hostEnvironment, ILogger<UpdateOptionsService> logger) : IUpdateOptionsService
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
        logger.LogInformation("Updating config at {Path}", path);

        string settingsContent;
        try
        {
            settingsContent = await File.ReadAllTextAsync(path);
        }
        catch (FileNotFoundException exception)
        {
            logger.LogError(exception, "Could not read existing config file from {Path}, starting with a new empty one", path);
            settingsContent = "{}";
        }

        try
        {
            var json = JsonNode.Parse(settingsContent, documentOptions: new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip }) ?? new JsonObject();

            updateAction(json);

            await using var writer = File.CreateText(path);
            await writer.WriteAsync(json.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            logger.LogInformation("Updated config at {Path} successfully!", path);
        }
        catch (Exception e)
        {
            logger.LogError(e, "An unexpected error occurred while updating the config at {Path}", path);
            throw new PrismaException($"Ein unerwarteter Fehler ist beim Speichern der Einstellungen aufgetreten.", e);
        }
    }
}
