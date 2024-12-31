using System;
using System.Collections.Generic;

namespace SocialButterflAi.Models.CueCoach.Contracts
{
    /// <summary>
    ///
    /// </summary>
    public class MessageContract
    {
        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Guid IdentityId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The TransactionId of the request.
        /// </summary>
        public Guid TransactionId { get; set; }

        /// <summary>
        /// The body of the request. The actual Message text sent to the endpoint.
        /// </summary>
        public string Body { get; set; }
    }
}