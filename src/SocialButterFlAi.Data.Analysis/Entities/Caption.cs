using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Data.Analysis.Entities
{
    [PrimaryKey(nameof(Id))]
    public class EnhancedCaption: BaseEntity
    {
        /// <summary>
        /// Navigation property
        /// </summary>
        public Guid VideoId { get; set; }
        public Video Video { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string StandardText { get; set; }
        public List<Analysis> Analyses { get; set; }
        public string? BackgroundContext { get; set; }
        public string? SoundEffects { get; set; }
    }
}