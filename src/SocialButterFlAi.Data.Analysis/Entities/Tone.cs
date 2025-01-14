using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Data.Analysis.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Tone: BaseEntity
    {
        /// <summary>
        /// Navigation properties
        /// </summary>
        public Guid AnalysisId { get; set; }
        public Analysis Analysis { get; set; }

        public string PrimaryEmotion { get; set; } // "hurt"
        public Dictionary<string, double> EmotionalSpectrum { get; set; } // { "anger": 0.5, "joy": 0.3, "sadness": 0.2 }
        public string EmotionalContext { get; set; } // "their voice quivered"
        public string NonVerbalCues { get; set; } // "they were frowning"
        public double IntensityScore { get; set; } // 0.8
    }
}