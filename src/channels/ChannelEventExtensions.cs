using Microsoft.Extensions.DependencyInjection;

namespace Faactory.Channels;

internal static class ChannelEventExtensions
{
    public static void NotifyChannelCreated( this Channel channel )
        => channel.GetEventServices()
            .InvokeAll( x => x.ChannelCreated( channel.Info ) );

    public static void NotifyChannelClosed( this Channel channel )
        => channel.GetEventServices()
            .InvokeAll( x => x.ChannelClosed( channel.Info ) );

    public static void NotifyDataReceived( this Channel channel, byte[] data )
        => channel.GetEventServices()
            .InvokeAll( x => x.DataReceived( channel.Info, data ) );

    public static void NotifyDataSent( this Channel channel, int sent )
        => channel.GetEventServices()
            .InvokeAll( x => x.DataSent( channel.Info, sent ) );

    private static void InvokeAll( this IEnumerable<IChannelEvents> services, Action<IChannelEvents> invoke )
    {
        foreach ( var service in services )
        {
            invoke( service );
        }
    }

    private static IEnumerable<IChannelEvents> GetEventServices( this Channel channel )
        => channel.ServiceProvider.GetServices<IChannelEvents>();
}
