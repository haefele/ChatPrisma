using Azure.AI.OpenAI;
using ChatPrisma.Options;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.ChatBot;

public class OpenAIChatBotService(IOptions<OpenAIOptions> openAiConfig) : IChatBotService
{
    private readonly OpenAIClient _client = new(openAiConfig.Value.ApiKey ?? throw new PrismaException("OpenAI API Key is missing"));

    public async IAsyncEnumerable<string> GetResponse(List<PrismaChatMessage> messages)
    {
        var chatCompletionsOptions = new ChatCompletionsOptions();
        foreach (var message in messages)
        {
            chatCompletionsOptions.Messages.Add(this.ConvertChatMessage(message));
        }

        var response = await this._client.GetChatCompletionsStreamingAsync(openAiConfig.Value.Model, chatCompletionsOptions);

        using var completions = response.Value;

        await foreach (var choice in completions.GetChoicesStreaming())
        {
            await foreach (var message in choice.GetMessageStreaming())
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