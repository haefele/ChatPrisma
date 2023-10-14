namespace ChatPrisma.Services.ChatBot;

public interface IChatBotService
{
    IAsyncEnumerable<string> GetResponse(List<PrismaChatMessage> messages);
}