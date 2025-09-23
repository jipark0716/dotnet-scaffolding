namespace Lib.MTlsManager;

public interface IMTlsSessionContext
{
    ConnectionId ConnectionId { get; }
    TaskCompletionSource ConnectionHandle { get; }
}