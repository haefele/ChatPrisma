namespace ChatPrisma.Services.TextExtractor;

public interface ITextExtractor
{
    Task<string?> GetPreviousTextAsync();

    Task<string?> GetCurrentTextAsync();
}
