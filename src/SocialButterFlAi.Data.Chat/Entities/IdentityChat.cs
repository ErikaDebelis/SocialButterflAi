using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SocialButterflAi.Data.Identity.Entities;

namespace SocialButterflAi.Data.Chat.Entities
{
    /// <summary>
    /// join table for 
    /// </summary>
    public class IdentityChat
    {
        public Guid IdentityId { get; set; }
        public Identity.Entities.Identity Identity { get; set; }

        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}