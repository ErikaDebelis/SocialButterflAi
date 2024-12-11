using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterFlAi.Data.Db.Basics;
using SocialButterFlAi.Data.Db.Chat.Entities;
using SocialButterFlAi.Data.Db.Identity.Entities;

namespace SocialButterFlAi.Data.Db.Analysis.Entities
{
    [PrimaryKey(nameof(Id))]
    [Table("Video")]
    public class Video: BaseEntity
    {
        /// <summary>
        /// Navigation property
        /// </summary>
        public Guid IdentityId { get; set; }
        public Identity.Entities.Identity Identity { get; set; }

        public Guid ChatId { get; set; }
        public Chat.Entities.Chat Chat { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public VideoType VideoType { get; set; }
        public TimeSpan Duration { get; set; }
        public List<EnhancedCaption> Captions { get; set; }
    }
}