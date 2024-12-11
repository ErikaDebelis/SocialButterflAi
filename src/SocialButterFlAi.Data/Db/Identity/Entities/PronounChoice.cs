
using System.ComponentModel.DataAnnotations.Schema;
using SocialButterFlAi.Data.Db.Basics;

namespace SocialButterFlAi.Data.Db.Identity.Entities
{
	[Table("PronounChoice")]
    public class PronounChoice : BaseEntity
    {
        public Pronoun Pronoun { get; set; }
    }
}