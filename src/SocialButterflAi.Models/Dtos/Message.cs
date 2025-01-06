using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SocialButterflAi.Models.Dtos
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public Guid? ToIdentityId { get; set; }
        public Guid FromIdentityId { get; set; }

        /// <summary>
        /// The text of the message
        /// </summary>
        public string Text { get; set; }

        public MessageType MessageType { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}