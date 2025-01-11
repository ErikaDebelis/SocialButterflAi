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
    public class Audio: Media
    {
        /// <summary>
        /// Navigation property
        /// </summary>

        public AudioType Type { get; set; }
        public override string Path { get; set; }
        public override string Base64 { get; set; }
        public List<EnhancedCaption>? Captions { get; set; }
    }
}