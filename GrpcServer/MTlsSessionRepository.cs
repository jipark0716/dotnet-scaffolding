using Lib.DI;
using Lib.MTlsManager;

namespace GrpcServer;

public class MTlsSessionRepository: IMTlsSessionRepository, IScoped<IMTlsSessionRepository>
{
    public async Task<IMTlsSessionContext> CreateContextAsync(ConnectionId connectionId)
    {
        return new MTlsSessionContext(connectionId, "UserId");
    }
}