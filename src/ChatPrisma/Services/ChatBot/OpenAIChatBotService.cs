using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;

namespace ChatPrisma.Services.ChatBot;

public class OpenAIChatBotService(IOptions<OpenAIConfig> openAiConfig) : IChatBotService
{
    private readonly OpenAIClient _client = new(openAiConfig.Value.ApiKey ?? throw new PrismaException("OpenAI API Key is missing"));

    public async IAsyncEnumerable<string> GetResponse(string newMessage, List<PrismaChatMessage>? previousMessages)
    {
        var chatCompletionsOptions = previousMessages is null
            ? this.GetChatCompletionOptionsForNewChat(newMessage)
            : this.GetChatCompletionOptionsForExistingChat(newMessage, previousMessages);

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

    private ChatCompletionsOptions GetChatCompletionOptionsForNewChat(string newMessage)
    {
        return new ChatCompletionsOptions
        {
            Messages =
            {
                new(ChatRole.System, "You are a helpful assistant that helps people write better texts. " +
                                     "You modify the given text exactly how the user requested. " +
                                     "Respond with just the modified text and nothing else."),
                
                new(ChatRole.User, $"Please improve this text:{Environment.NewLine}{Environment.NewLine}{newMessage}"),
            }
        };
    }
    
    private ChatCompletionsOptions GetChatCompletionOptionsForExistingChat(string newMessage, List<PrismaChatMessage> previousMessages)
    {
        var result = new ChatCompletionsOptions();

        foreach (var previousMessage in previousMessages)
        {
            result.Messages.Add(ConvertChatMessage(previousMessage));
        }

        result.Messages.Add(new(ChatRole.User, newMessage));
        
        return result;
        
        ChatMessage ConvertChatMessage(PrismaChatMessage message)
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
}