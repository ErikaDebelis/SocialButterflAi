using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterFlAi.Data.Identity;

namespace SocialButterFlAi.Data.Identity.Entities
{
    [PrimaryKey(nameof(Id))]
    [Table("Identity")]
    public class Identity : BaseEntity
    {
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