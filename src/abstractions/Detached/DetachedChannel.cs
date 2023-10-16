using Faactory.Channels.Buffers;
using Faactory.Collections;

namespace Faactory.Channels;

/// <summary>
/// A detached channel instance to be used outside a Channels context
/// </summary>
public sealed class DetachedChannel : IChannel
{
    public string Id { get; } = Guid.NewGuid().ToString( "N" );

    public IMetadata Data { get; } = new Metadata();

    public DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? LastReceived { get; }

    public DateTimeOffset? LastSent { get; }

    public IByteBuffer Buffer => throw new NotSupportedException();

    public bool IsClosed => false;

    public IEnumerable<IChannelService> Services { get; } = Enumerable.Empty<IChannelService>();

    public Task CloseAsync()
    {
        return Task.CompletedTask;
    }

    public Task WriteAsync( object data )
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    { }
}
