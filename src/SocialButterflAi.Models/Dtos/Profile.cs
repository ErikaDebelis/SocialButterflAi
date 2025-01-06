using System;
using System.Collections.Generic;

namespace SocialButterflAi.Models.Dtos
{
    public class Profile
    {
        public Guid Id { get; set; }
        public Guid IdentityId { get; set; }

        public string Name { get; set; }
        public ProfileStatus Status { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? PreferredName { get; set; }
        public Title Title { get; set; }
        public Gender Gender { get; set; }
        public IEnumerable<Pronoun>? Pronouns { get; set; }
    }
}