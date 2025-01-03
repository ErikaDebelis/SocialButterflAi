﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Data.Analysis.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Analysis: BaseEntity
    {
        /// <summary>
        /// Navigation properties
        /// </summary>
        public Guid? CaptionId { get; set; }
        public EnhancedCaption? Caption { get; set; }

        public Guid? VideoId { get; set; }
        public Video? Video { get; set; }

        public Guid? ImageId { get; set; }
        public Image? Image { get; set; }

        public Guid? AudioId { get; set; }
        public Audio? Audio { get; set; }

        public double Certainty { get; set; }
        public string EnhancedDescription { get; set; }
        public string EmotionalContext { get; set; }
        public string? NonVerbalCues { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}