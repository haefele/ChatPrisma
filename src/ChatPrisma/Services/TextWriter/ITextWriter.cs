namespace ChatPrisma.Services.TextWriter;

public interface ITextWriter
{
    Task WriteTextAsync(IAsyncEnumerable<string> text);
}