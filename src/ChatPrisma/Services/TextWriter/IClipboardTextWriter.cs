namespace ChatPrisma.Services.TextWriter;

public interface IClipboardTextWriter
{
    Task CopyTextAsync(string text, bool autoPaste);
}