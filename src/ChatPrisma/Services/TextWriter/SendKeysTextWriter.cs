using System.Text;
using System.Windows.Forms;

namespace ChatPrisma.Services.TextWriter;

public class SendKeysTextWriter : ITextWriter
{
    public async Task WriteTextAsync(IAsyncEnumerable<string> text)
    {
        var builder = new StringBuilder(128);
        
        await foreach (var part in text)
        {
            ReplaceSpecialCharacters(part, builder);
            SendKeys.SendWait(builder.ToString());

            // Reset the builder for the next line
            builder.Clear();
        }
    }
    
    private static void ReplaceSpecialCharacters(string text, StringBuilder result)
    {
        // See: https://docs.microsoft.com/en-US/dotnet/api/system.windows.forms.sendkeys.send?view=netframework-4.7.2#remarks

        foreach (var c in text)
        {
            switch (c)
            {
                // Braces
                case '{':
                    result.Append("{{}");
                    break;
                case '}':
                    result.Append("{}}");
                    break;
                // Parentheses
                case '(':
                    result.Append("{(}");
                    break;
                case ')':
                    result.Append("{)}");
                    break;
                // Brackets
                case '[':
                    result.Append("{[}");
                    break;
                case ']':
                    result.Append("{]}");
                    break;
                // Special Characters
                case '+':
                    result.Append("{+}");
                    break;
                case '^':
                    result.Append("{^}");
                    break;
                case '%':
                    result.Append("{%}");
                    break;
                case '~':
                    result.Append("{~}");
                    break;
                // Every other character
                default:
                    result.Append(c);
                    break;
            }
        }
    }
}