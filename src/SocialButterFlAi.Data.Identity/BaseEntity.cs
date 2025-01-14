using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialButterflAi.Data.Identity
{
    /// <summary>
    /// the core entity that all other entities inherit from. standard audit fields and id
    /// </summary>
    [NotMapped]
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}