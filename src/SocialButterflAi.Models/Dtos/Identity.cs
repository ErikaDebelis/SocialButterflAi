using System;
using System.Collections.Generic;

namespace SocialButterflAi.Models.Dtos
{
    public class Identity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// first and last
        /// </summary>
        public string Name { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// hashed password
        /// </summary>
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? StopDate { get; set; }
    }
}