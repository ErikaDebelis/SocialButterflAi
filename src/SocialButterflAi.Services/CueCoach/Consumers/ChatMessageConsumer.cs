using System;
using Serilog;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SocialButterflAi.Services.CueCoach;
using Microsoft.Extensions.Caching.Distributed;
using MessageDto = SocialButterflAi.Models.Dtos.Message;
using SocialButterflAi.Models.CueCoach;
using SocialButterflAi.Models.CueCoach.Contracts;

namespace SocialButterflAi.Services.CueCoach.Consumers
{
    /// <summary>
    /// The consumer
    /// </summary>
    public class ChatMessageConsumer : IConsumer<MessageContract>
    {
        public ICueCoachService CueCoachService;
        private ILogger<ChatMessageConsumer> Logger;
        private IBus Bus;
        private IDistributedCache Cache;
        private Serilog.ILogger SeriLogger;
        private IHubContext<ChatMessageHub> ChatMessageHub;

        public ChatMessageConsumer(
            ICueCoachService cueCoachService,
            IBus bus,
            IDistributedCache cache,
            IHubContext<ChatMessageHub> chatMessageHub,
            ILogger<ChatMessageConsumer> logger
        )
        {
            CueCoachService = cueCoachService;
            ChatMessageHub = chatMessageHub;
            Bus = bus;
            Cache = cache;
            Logger = logger;
            SeriLogger = Serilog.Log.Logger;
        }

        /// <summary>
        /// This consumer will take the message and send it to the appropriate channel.
        /// </summary>
        /// <param name="messageContractContext"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Consume(
            ConsumeContext<MessageContract> messageContractContext
        )
        {
            if (string.IsNullOrWhiteSpace($"{messageContractContext.Message.TransactionId}"))
            {
                messageContractContext.Message.TransactionId = Guid.NewGuid();
            }

            var transactionId = messageContractContext.Message.TransactionId;
            var msg = JsonSerializer.Deserialize<MessageDto>(messageContractContext.Message.Body);

            try
            {
                //continue here
                var response = await CueCoachService.ProcessMessageAsync(
                    msg,
                    transactionId,
                    true
                );

                if (response == null)
                {
                    Logger.LogError($"Failed to process message in Consumer. Response is null.");
                    SeriLogger.Error($"Failed to process message in Consumer. Response is null.");
                    return;
                }

                if(!response.Success)
                {
                    Logger.LogError($"Failed to process message in Consumer. {response.Message}");
                    SeriLogger.Error($"Failed to process message in Consumer. {response.Message}");
                    return;
                }

                Logger.LogInformation($"Successfully processed message in Consumer. {response.Message}");
                SeriLogger.Information($"Successfully processed message in Consumer. {response.Message}");

                try
                {
                    var token = await Cache.GetStringAsync($"IT:{messageContractContext.Message.IdentityId}");

                    if(string.IsNullOrWhiteSpace(token))
                    {
                        Logger.LogError($"Token not found for {messageContractContext.Message.IdentityId} - need to register in cache/ w signalR");
                        SeriLogger.Error($"Token not found for {messageContractContext.Message.IdentityId} - need to register in cache/ w signalR");
                        return;
                    }

                    var connectionId = await Cache.GetStringAsync($"T:{token}");

                    if(string.IsNullOrWhiteSpace(connectionId))
                    {
                        Logger.LogError($"ConnectionId not found for {token} - need to register in cache/ w signalR");
                        SeriLogger.Error($"ConnectionId not found for {token} - need to register in cache/ w signalR");
                        return;
                    }

                    await ChatMessageHub.Clients
                        .Client(connectionId)
                        .SendAsync(
                            "ChatMessage",
                            response.Data.AnalysisData,
                            new CancellationToken()
                        );

                    Logger.LogInformation($"({messageContractContext.Message.IdentityId}) chat message sent.");
                    SeriLogger.Information($"({messageContractContext.Message.IdentityId}) chat message sent.");
                }
                catch (Exception ex)
                {
                    Logger.LogCritical($"Error sending {ex.Message}", ex);
                    SeriLogger.Fatal(ex, $"Error sending {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogCritical($" failed to process Contract message in Consumer. {ex.Message}", ex);
                SeriLogger.Fatal($" failed to process Contract message in Consumer. {ex.Message}");
            }
            // await Cache.RemoveAsync(lockKey);
        }
    }
}