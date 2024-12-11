using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterFlAi.Data.Db.Basics;
using SocialButterFlAi.Data.Db.Identity.Entities;
using System.Collections.Generic;

namespace SocialButterFlAi.Data.Db.Chat.Entities
{
    [PrimaryKey(nameof(Id))]
    [Table("Chat")]
    public class Chat: BaseEntity
    {
        public string Name { get; set; }
        public ChatStatus ChatStatus { get; set; }
        public List<Message> Messages { get; set; }
    }
}