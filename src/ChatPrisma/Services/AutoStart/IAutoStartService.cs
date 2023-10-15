namespace ChatPrisma.Services.AutoStart;

public interface IAutoStartService
{
    Task<bool> IsInAutoStart();
    Task SetAutoStart(bool enabled);
}
