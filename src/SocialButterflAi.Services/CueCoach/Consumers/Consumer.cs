using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

using Serilog;
using Serilog.Core;
using SocialButterflAi.Models.CueCoach;
using SocialButterflAi.Models.CueCoach.Contracts;
using SocialButterflAi.Services.CueCoach;
namespace CueCoach.Consumers
{
    /// <summary>
    /// The consumer
    /// </summary>
    public class Consumer : IConsumer<MessageContract>
    {

    #region Private Variables
        public ICueCoachService CueCoachService;
        private readonly IBus Bus;
        private readonly IDistributedCache DistributedCache;
        private readonly Serilog.ILogger SeriLogger;

    #endregion

    #region Constructors

        public Consumer(
            ICueCoachService cueCoachService,
            IBus bus,
            IDistributedCache distributedCache
        )
        {
            CueCoachService = cueCoachService;
            Bus = bus;
            DistributedCache = distributedCache;
            SeriLogger = Serilog.Log.Logger;
        }

    #endregion

    #region Methods

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
	        // Only one consumption of this type allowed (Regardless of node). Pick a wait interval...
            // todo: there probably is a way to do this cleaner...
            var randomTo1k = new Random().Next(1000, 3000);
            Thread.Sleep(randomTo1k);

            var lockKey = $"MessageContract:{messageContractContext.MessageId}:Lock";
            var lockValue = await DistributedCache.GetStringAsync(lockKey) ?? string.Empty;
            if (lockValue == "Locked")
            {
                SeriLogger.Information($"Another server is taking care of this contract. All done.");
                return;
            }

            await DistributedCache.SetStringAsync(lockKey, "Locked");

            if (string.IsNullOrWhiteSpace($"{messageContractContext.Message.TransactionId}"))
            {
                messageContractContext.Message.TransactionId = Guid.NewGuid();
            }

            var transactionId = messageContractContext.Message.TransactionId;
            var msg = JsonSerializer.Deserialize<Message>(messageContractContext.Message.Body);
            try
            {
                //continue here
                var response = await CueCoachService.ProcessMessageAsync(
                    msg,
                    transactionId,
                    true
                );
            }
            catch (Exception ex)
            {
                SeriLogger.Fatal($" failed to process Contract message in Consumer. {ex.Message}");
            }

            await DistributedCache.RemoveAsync(lockKey);
        }

    #endregion
    }
}