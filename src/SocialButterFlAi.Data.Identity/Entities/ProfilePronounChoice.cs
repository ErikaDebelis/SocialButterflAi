using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SocialButterFlAi.Data.Identity;

namespace SocialButterFlAi.Data.Identity.Entities
{
    /// <summary>
    /// join table for Profile and PronounChoice
    /// </summary>
    public class ProfilePronounChoice
    {
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }

        public Guid PronounChoiceId { get; set; }
        public PronounChoice PronounChoice { get; set; }
    }
}