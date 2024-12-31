using System;
using Serilog;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

using Microsoft.AspNetCore.SignalR;

using SocialButterflAi.Models.CueCoach.Contracts;

namespace SocialButterflAi.Services.CueCoach
{
    public class ChatMessageHub : Hub
    {
        private IDistributedCache Cache;
        private ILogger<ChatMessageHub> Logger;
        private Serilog.ILogger SeriLogger;

		public ChatMessageHub(
			IDistributedCache cache,
			ILogger<ChatMessageHub> logger
		)
		{
			Cache = cache;
			Logger = logger;
            SeriLogger = Serilog.Log.Logger;
		}

        /// <remarks></remarks>
        /// <summary>
        ///
        ///</summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(
            MessageContract message
        )
        {
            try
            {
                // Get the token from the cache based on the identityId
                var token = await Cache.GetStringAsync($"ChatMessages:Identity:{message.IdentityId}");
                if(string.IsNullOrWhiteSpace(token))
                {
                    Logger.LogError($"Token not found for {message.IdentityId}");
                    SeriLogger.Error($"Token not found for {message.IdentityId}");
                    return;
                }
                // Get the connectionId from the cache based on the token
                var connectionId = await Cache.GetStringAsync($"ChatMessages:TokenCId:{token}");

                if(string.IsNullOrWhiteSpace(connectionId))
                {
                    Logger.LogError($"ConnectionId not found for {token}");
                    SeriLogger.Error($"ConnectionId not found for {token}");
                    return;
                }

                // Send the message to the connectionId
                await Clients
                    .Client(connectionId)
                    .SendAsync( "Message", message);

                Logger.LogInformation($"Message sent to {message.IdentityId}");
                SeriLogger.Information($"Message sent to {message.IdentityId}");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error sending", ex);
                SeriLogger.Error(ex, "Error sending");
            }
        }

        /// <summary>
        /// Register the connection
        /// </summary>
        /// <param name="identityId"></param>
        /// <param name="bearerToken"></param>
        /// <returns></returns>
        public async Task RegisterConnectionAsync(
            Guid identityId,
            string bearerToken
        )
        {
            try
            {
                await Cache.SetStringAsync($"ChatMessages:Identity:{identityId}", bearerToken);
                await Cache.SetStringAsync($"ChatMessages:CIdToken:{Context.ConnectionId}", bearerToken);
                await Cache.SetStringAsync($"ChatMessages:TokenCId:{bearerToken}", Context.ConnectionId);
                await Cache.SetStringAsync($"ChatMessages:TokenIdentity:{bearerToken}", $"{identityId}");

                await Clients
                    .Client(Context.ConnectionId)
                    .SendAsync("Registered", new { });

                Logger.LogInformation($"Registered {identityId}");
                SeriLogger.Information($"Registered {identityId}");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error registering", ex);
                SeriLogger.Error(ex, "Error registering");
            }
        }
    }
}