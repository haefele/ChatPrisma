namespace ChatPrisma.Services.ChatBot;

public interface IChatBotService
{
    IAsyncEnumerable<string> GetResponse(string newMessage, List<PrismaChatMessage>? previousMessages);
}