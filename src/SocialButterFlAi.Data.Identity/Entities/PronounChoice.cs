
using System.ComponentModel.DataAnnotations.Schema;
using SocialButterflAi.Data.Identity;

namespace SocialButterflAi.Data.Identity.Entities
{
	[Table("PronounChoice")]
    public class PronounChoice : BaseEntity
    {
        public Pronoun Pronoun { get; set; }
    }
}