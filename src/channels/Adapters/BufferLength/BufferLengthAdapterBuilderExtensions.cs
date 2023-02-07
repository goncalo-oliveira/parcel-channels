using Faactory.Channels.Adapters;

namespace Faactory.Channels;

public static class BufferLengthAdapterChannelBuilderExtensions
{
    /// <summary>
    /// Adds a buffer length adapter to the input pipeline
    /// </summary>
    public static IChannelBuilder AddBufferLengthInputAdapter( this IChannelBuilder builder, Action<BufferLengthAdapterOptions>? configure = null )
    {
        builder.AddInputAdapter<BufferLengthAdapter>();

        if ( configure != null )
        {
            builder.Configure<BufferLengthAdapterOptions>( configure );
        }

        return ( builder );
    }
}
