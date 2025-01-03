using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

using SocialButterflAi.Data.Identity;
using System.Collections.Generic;
namespace SocialButterflAi.Models.CueCoach
{
    [PrimaryKey(nameof(Id))]
    [Table("Chat")]
    public class Chat
    {
        public string Name { get; set; }
        public ChatStatus ChatStatus { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<object> Members { get; set; }
    }
}