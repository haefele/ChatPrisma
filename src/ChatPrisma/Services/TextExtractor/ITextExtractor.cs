namespace ChatPrisma.Services.TextExtractor;

public interface ITextExtractor
{
    Task<string?> GetCurrentTextAsync();
}