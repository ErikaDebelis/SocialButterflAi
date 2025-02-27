using System;
using System.Linq;

using SocialButterflAi.Models.Dtos;
using ChatDto = SocialButterflAi.Models.Dtos.Chat;
using ChatEntity = SocialButterflAi.Data.Chat.Entities.Chat;

namespace SocialButterflAi.Services.Mappers
{
    //Todo: finish implementing the mapper
    public class ChatMapper : IMapper<ChatDto, ChatEntity>
    {
        public MessageMapper MessageMapper = new MessageMapper();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatDto"></param>
        /// <returns></returns>
        public ChatEntity MapToEntity(
            ChatDto chatDto
        )
        => new ChatEntity
            {
                Id = chatDto.Id ?? Guid.NewGuid(),
                Name = chatDto.Name,
                ChatStatus = Enum.Parse<Data.Chat.Entities.ChatStatus>($"{chatDto.ChatStatus}"),
                Messages = chatDto.Messages.Select(m => MessageMapper.MapToEntity(m)).ToList(),
                // Members = ,
                // CreatedBy = $"{chatDto.IdentityId}",
                CreatedOn = DateTime.UtcNow,
                // ModifiedBy = $"{chatDto.IdentityId}",
                ModifiedOn = DateTime.UtcNow
            };

        /// <summary>
        ///
        /// </summary>
        /// <param name="chatEntity"></param>
        /// <returns></returns>
        public ChatDto MapToDto(
            ChatEntity chatEntity
        )
        => new ChatDto
            {
                // Map properties from entity to dto
                Id = chatEntity.Id,
                Name = chatEntity.Name,
                ChatStatus = Enum.Parse<ChatStatus>($"{chatEntity.ChatStatus}"),
                Messages = chatEntity.Messages.Select(m => MessageMapper.MapToDto(m)).ToList(),
                MemberIdentityIds = chatEntity.Members.Select(m => m.Id).ToList(),
            };
    }
}