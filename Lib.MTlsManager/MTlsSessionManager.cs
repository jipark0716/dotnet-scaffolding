using System.Collections.Concurrent;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Features;

namespace Lib.MTlsManager;

public class MTlsSessionManager
{
    private readonly ConcurrentDictionary<ConnectionId, IMTlsSessionContext> _sessions = [];

    public async Task<IMTlsSessionContext?> ConnectAsync(IMTlsSessionRepository repository,
        ConnectionContext connectContext)
    {
        ITlsConnectionFeature? tls = connectContext.Features.Get<ITlsConnectionFeature>();
        if (tls is null) return null;

        ConnectionId connectionId = tls.ClientCertificate.SerialNumberBytes;

        if (_sessions.TryGetValue(connectionId, out IMTlsSessionContext? mTlsContext))
        {
            return mTlsContext;
        }

        mTlsContext = await repository.CreateContextAsync(connectionId);
        _sessions.TryAdd(connectionId, mTlsContext);
        return mTlsContext;
    }

    public void Abort(ConnectionId connectionId)
    {
        if (_sessions.TryGetValue(connectionId, out IMTlsSessionContext? context))
        {
            context.ConnectionHandle.TrySetResult();
        }
    }
}