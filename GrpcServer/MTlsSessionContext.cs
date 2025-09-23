using Lib.MTlsManager;

namespace GrpcServer;

public readonly struct MTlsSessionContext(
    ConnectionId connectionId,
    TaskCompletionSource connectionHandle,
    string userId
) : IMTlsSessionContext
{
    public ConnectionId ConnectionId { get; } = connectionId;
    public TaskCompletionSource ConnectionHandle { get; } = connectionHandle;
    public string UserId => userId;

    public MTlsSessionContext() : this(
        default,
        new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously),
        string.Empty)
    {
    }

    public MTlsSessionContext(ConnectionId connectionId, string userId) : this(
        connectionId,
        new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously),
        userId)
    {
    }
}