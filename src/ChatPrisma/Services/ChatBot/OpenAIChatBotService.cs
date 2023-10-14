using System.Runtime.CompilerServices;
using Azure.AI.OpenAI;
using ChatPrisma.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.ChatBot;

public class OpenAIChatBotService(IOptions<OpenAIOptions> openAiConfig, ILogger<OpenAIChatBotService> logger) : IChatBotService
{
    private readonly OpenAIClient _client = new(openAiConfig.Value.ApiKey ?? throw new PrismaException("OpenAI API Key is missing"));

    public async IAsyncEnumerable<string> GetResponse(List<PrismaChatMessage> messages, [EnumeratorCancellation] CancellationToken token = default)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions();
        foreach (var message in messages)
        {
            chatCompletionsOptions.Messages.Add(this.ConvertChatMessage(message));
        }

        logger.LogInformation("Calling ChatGPT model {Model}", openAiConfig.Value.Model);

        var response = await this._client.GetChatCompletionsStreamingAsync(openAiConfig.Value.Model, chatCompletionsOptions, token);
        
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
            _ => throw new ArgumentOutOfRangeException(nameof(message.Role), message.Role, null)
        };
        
        return new ChatMessage(openAiChatRole, message.Content);
    }
}