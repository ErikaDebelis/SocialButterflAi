using System;
using System.Collections.Generic;
using SocialButterflAi.Models;

namespace SocialButterflAi.Models.Dtos
{
    public class Chat: BaseDto
    {
        public override Guid? Id { get; set; }

        public string Name { get; set; }
        public ChatStatus ChatStatus { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<Guid> MemberIdentityIds { get; set; }
        public Guid CreatorId { get; set; }
    }
}