using System.Runtime.CompilerServices;
using Azure.AI.OpenAI;
using ChatPrisma.Options;
using ChatPrisma.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.ChatBot;

public class OpenAIChatBotService(IOptionsMonitor<OpenAIOptions> openAiConfig, ILogger<OpenAIChatBotService> logger) : IChatBotService
{
    public async IAsyncEnumerable<string> GetResponse(List<PrismaChatMessage> messages, [EnumeratorCancellation] CancellationToken token = default)
    {
        var client = this.GetClient();
        if (client is null)
        {
            yield return Strings.ApiKeyMissing;
            yield break;
        }

        if (string.IsNullOrWhiteSpace(openAiConfig.CurrentValue.Model))
        {
            yield return Strings.ModelMissing;
            yield break;
        }

        var chatCompletionsOptions = new ChatCompletionsOptions();
        foreach (var message in messages)
        {
            chatCompletionsOptions.Messages.Add(this.ConvertChatMessage(message));
        }

        logger.LogInformation("Calling ChatGPT model {Model}", openAiConfig.CurrentValue.Model);

        var response = await client.GetChatCompletionsStreamingAsync(openAiConfig.CurrentValue.Model, chatCompletionsOptions, token);

        using var completions = response.Value;

        await foreach (var choice in completions.GetChoicesStreaming(token))
        {
            await foreach (var message in choice.GetMessageStreaming(token))
            {
                if (message.Content is not null)
                    yield return message.Content;
            }
        }
    }

    private ChatMessage ConvertChatMessage(PrismaChatMessage message)
    {
        var openAiChatRole = message.Role switch
        {
            PrismaChatRole.System => ChatRole.System,
            PrismaChatRole.User => ChatRole.User,
            PrismaChatRole.Assistant => ChatRole.Assistant,
            _ => throw new PrismaException($"Unexpected {nameof(PrismaChatRole)} value of {message.Role}."),
        };

        return new ChatMessage(openAiChatRole, message.Content);
    }

    private (OpenAIClient Client, string ApiKey)? _lastClient;
    private OpenAIClient? GetClient()
    {
        if (this._lastClient is null || this._lastClient.Value.ApiKey != openAiConfig.CurrentValue.ApiKey)
        {
            _lastClient = string.IsNullOrWhiteSpace(openAiConfig.CurrentValue.ApiKey) is false
                ? (new OpenAIClient(openAiConfig.CurrentValue.ApiKey), openAiConfig.CurrentValue.ApiKey)
                : null;
        }

        return _lastClient?.Client;
    }
}
