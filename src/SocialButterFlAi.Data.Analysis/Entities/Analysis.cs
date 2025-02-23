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

        public MediaType Type { get; set; }
        public double Certainty { get; set; }
        public string EnhancedDescription { get; set; }
        public Guid ToneId { get; set; }
        public Tone Tone { get; set; }
        public Guid IntentId { get; set; }
        public Intent Intent { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}