using System;
using System.Collections.Generic;

namespace SocialButterflAi.Models.CueCoach.Dtos
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public ChatStatus ChatStatus { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Guid> MemberIdentityIds { get; set; }
    }
}