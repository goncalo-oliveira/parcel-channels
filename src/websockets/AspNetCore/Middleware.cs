using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Faactory.Channels.WebSockets;

internal static class WebSocketChannelMiddleware
{
    public static async Task InvokeAsync( HttpContext httpContext, IWebSocketChannelFactory channelFactory, [FromServices] IChannelPipelineBuilder? pipelineBuilder = null )
    {
            // make sure this is a WebSocket request
            if ( !httpContext.WebSockets.IsWebSocketRequest )
            {
                httpContext.Response.StatusCode = 400;

                return;
            }

            // accept the WebSocket connection
            using var ws = await httpContext.WebSockets.AcceptWebSocketAsync();

            // create a WebSocket channel using the factory
            var channel = await channelFactory.CreateChannelAsync( ws, pipelineBuilder );

            /*
            this creates a linked token source that will cancel when either of the following happens:
            - the HTTP request is cancelled
            - the application is gracefully stopping

            if we used only the HTTP request token, the application would take longer to stop
            since the channel wouldn't receive the stop signal, leading to a delay in the application shutdown
            and a forced (non-graceful) termination of the channel after the graceful shutdown timeout.
            */
            var cts = CancellationTokenSource.CreateLinkedTokenSource(
                [
                    httpContext.RequestAborted,
                    httpContext.RequestServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping
                ]
            );

            // wait until the channel is closed
            try
            {
                await channel.WaitAsync( cts.Token );
            }
            catch ( OperationCanceledException )
            { }

            // close the channel and release resources
            await channel.CloseAsync();
    }
}
