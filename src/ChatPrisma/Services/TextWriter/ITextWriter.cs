namespace ChatPrisma.Services.TextWriter;

public interface ITextWriter
{
    Task WriteTextAsync(string text);
}