using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;
using System.Collections.Generic;
namespace SocialButterflAi.Data.Chat.Entities
{
    [PrimaryKey(nameof(Id))]
    [Table("Chat")]
    public class Chat: BaseEntity
    {
        public string Name { get; set; }
        public ChatStatus ChatStatus { get; set; }
        public List<Message> Messages { get; set; }
        public List<Identity.Entities.Identity> Members { get; set; }
    }
}