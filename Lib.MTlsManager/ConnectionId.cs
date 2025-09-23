namespace Lib.MTlsManager;

public readonly struct ConnectionId(byte[] bytes)
{
    public static implicit operator ConnectionId(ReadOnlyMemory<byte> bytes) => new(bytes.ToArray());
    public override string ToString() => string.Join(" ", bytes.Select(b => b.ToString("X2")));
}