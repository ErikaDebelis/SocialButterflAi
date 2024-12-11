
using System.ComponentModel.DataAnnotations.Schema;
using SocialButterFlAi.Data.Identity;

namespace SocialButterFlAi.Data.Identity.Entities
{
	[Table("PronounChoice")]
    public class PronounChoice : BaseEntity
    {
        public Pronoun Pronoun { get; set; }
    }
}