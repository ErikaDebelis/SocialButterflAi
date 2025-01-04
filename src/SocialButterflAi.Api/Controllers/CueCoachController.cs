using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using MessageDto = SocialButterflAi.Models.CueCoach.Dtos.Message;
using ChatDto = SocialButterflAi.Models.CueCoach.Dtos.Chat;
using SocialButterflAi.Data.Identity;
using MassTransit;
using RabbitMQ.Client;

using SocialButterflAi.Models.CueCoach;
using SocialButterflAi.Models.CueCoach.Contracts;

namespace SocialButterflAi.Api.CueCoach.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class CueCoachController : ControllerBase
    {
        private IdentityDbContext IdentityDbContext;
        private IBus Bus;
        private ILogger<CueCoachController> Logger;

        public CueCoachController(
            IBus bus,
            IdentityDbContext identityDbContext,
            ILogger<CueCoachController> logger
        )
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            IdentityDbContext = identityDbContext;
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    
        #region IncomingMessage
        ///<remarks></remarks>
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns>IActionResult</returns>
        /// <response code="404">Error:</response>
        /// <response code="500">Description here</response>
        [HttpPost]
        public async Task<IActionResult> IncomingMessage()
        {
            try
            {
                var request = HttpContext.Request;

                var incomingHeaders = HttpContext.Request.Headers.ToDictionary(x => x.Key, x => $"{x.Value}");

                //load the incoming message from the request body into a stream and string for processing
                (System.IO.Stream? Stream, string? String) incomingMessage = (null, null);
                if (request.Body != null)
                {
                    var incomingMessageStream = request.Body;
                    var memoryStream = new MemoryStream();

                    //copy the request stream to our memory stream
                    await incomingMessageStream.CopyToAsync(memoryStream);

                    // Start MemoryStream at the beginning
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    //create a new stream reader to read the stream
                    StreamReader streamReader = new StreamReader(memoryStream);

                    //read the stream as a string
                    var resultString = await streamReader.ReadToEndAsync();

                    // once we're done reading in string form... reset for JSON deserialization
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    incomingMessage = (memoryStream, resultString);
                }
                else
                {
                    throw new Exception("Body of request is null.");
                }

                var jsonSerializationOptions = new JsonSerializerOptions()
                {
	                PropertyNameCaseInsensitive = true
                };

                var deserializedMessage = await JsonSerializer.DeserializeAsync<MessageDto>(incomingMessage.Stream, jsonSerializationOptions);
                if (deserializedMessage == null)
                {
                    return StatusCode(500, "Unable to deserialize incoming message");
                }

                KeyValuePair<string, string>? headerTransaction = null;
                Func<KeyValuePair<string, string>, bool> hasTransactionHeader = pair => pair.Key.ToLower() == "transactionId";

                if(incomingHeaders.Any(hasTransactionHeader))
                {
                    headerTransaction = incomingHeaders.FirstOrDefault(hasTransactionHeader);
                }

				var contract = new MessageContract
				{
	                Body = incomingMessage.String
                };

                // publish to messaging bus
                var task = Bus.Publish(contract, (callback) =>
                {
                    callback.SetRoutingKey("Message");
                    callback.Headers.Set("TransactionId", $"{Guid.NewGuid():N}");
                });

                // run it
                await task;

                if (task.Status == TaskStatus.RanToCompletion)
                {
                    return StatusCode(200);
                }

                return StatusCode(500);
            }
            catch (Exception e)
            {
                // Something catastrophic happened if it reaches this point...
                return StatusCode(500, e);
            }
        }
    #endregion
    }
}