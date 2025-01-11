using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Data.Analysis.Entities
{
    [NotMapped]
    public class Media: BaseEntity
    {
        /// <summary>
        /// Navigation properties
        /// </summary>
        public Guid IdentityId { get; set; }
        public Identity.Entities.Identity Identity { get; set; }

        public Guid? MessageId { get; set; }
        public Chat.Entities.Message? Message { get; set; }

        public virtual string Path { get; set; }
        public virtual string Base64 { get; set; }
    }
}