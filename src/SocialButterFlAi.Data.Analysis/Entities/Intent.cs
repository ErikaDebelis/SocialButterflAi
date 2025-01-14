using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Data.Analysis.Entities
{
    [PrimaryKey(nameof(Id))]
    public class Intent: BaseEntity
    {
        /// <summary>
        /// Navigation properties
        /// </summary>
        public Guid AnalysisId { get; set; }
        public Analysis Analysis { get; set; }

        public string PrimaryIntent { get; set; } // "convince them to buy a car"
        public Dictionary<string, double> SecondaryIntents { get; set; } // { "make them feel good": 0.5, "make them feel safe": 0.3 }
        public double CertaintyScore { get; set; } // 0.8
        public string SubtextualMeaning { get; set; } // "they are working hard to make a sale"
    }
}