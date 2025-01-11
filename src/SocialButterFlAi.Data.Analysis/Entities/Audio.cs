using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;
using SocialButterflAi.Data.Chat.Entities;
using SocialButterflAi.Data.Identity.Entities;

namespace SocialButterflAi.Data.Analysis.Entities
{
    [PrimaryKey(nameof(Id))]
    [Table("Audio")]
    public class Audio: BaseEntity
    {
        /// <summary>
        /// Navigation property
        /// </summary>
        public Guid IdentityId { get; set; }
        public Identity.Entities.Identity Identity { get; set; }

        public Guid? MessageId { get; set; }
        public Chat.Entities.Message? Message { get; set; }

        public AudioType Type { get; set; }
        public string Path { get; set; }
        public string Base64 { get; set; }
        public List<EnhancedCaption>? Captions { get; set; }
    }
}