namespace Lib.MTlsManager;

public interface IMTlsSessionRepository
{
    Task<IMTlsSessionContext> CreateContextAsync(ConnectionId connectionId);
}