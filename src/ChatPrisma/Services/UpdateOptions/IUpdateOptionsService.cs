using ChatPrisma.Options;

namespace ChatPrisma.Services.UpdateOptions;

public interface IUpdateOptionsService
{
    Task Update(OpenAIOptions options);
}
