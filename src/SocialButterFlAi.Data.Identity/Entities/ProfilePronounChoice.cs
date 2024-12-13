using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Data.Identity.Entities
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