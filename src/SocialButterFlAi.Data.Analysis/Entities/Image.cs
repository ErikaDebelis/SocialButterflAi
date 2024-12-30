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
    [Table("Image")]
    public class Image: BaseEntity
    {
        /// <summary>
        /// Navigation property
        /// </summary>
        public Guid IdentityId { get; set; }
        public Identity.Entities.Identity Identity { get; set; }

        public Guid? ChatId { get; set; }
        public Chat.Entities.Chat? Chat { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Base64 { get; set; }
        public ImageType ImageType { get; set; }
        public List<Analysis>? Analyses { get; set; }
    }
}