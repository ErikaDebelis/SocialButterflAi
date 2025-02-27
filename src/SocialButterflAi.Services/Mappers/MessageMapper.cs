using System;
using MessageDto = SocialButterflAi.Models.Dtos.Message;
using MessageEntity = SocialButterflAi.Data.Chat.Entities.Message;
using SocialButterflAi.Models.Dtos;

namespace SocialButterflAi.Services.Mappers
{
    //Todo: finish implementing the mapper
    public class MessageMapper : IMapper<MessageDto, MessageEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MessageDto"></param>
        /// <returns></returns>
        public MessageEntity MapToEntity(
            MessageDto msgDto
        )
        => new MessageEntity
            {
                Id = msgDto.Id ?? Guid.NewGuid(),
                ChatId = msgDto.ChatId,
                FromIdentityId = msgDto.FromIdentityId,
                ToIdentityId = msgDto.ToIdentityId ?? default,
                MessageType = Enum.Parse<Data.Chat.Entities.MessageType>($"{msgDto.MessageType}"),
                Metadata = msgDto.Metadata,
                CreatedBy = $"{msgDto.FromIdentityId}",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = $"{msgDto.FromIdentityId}",
                ModifiedOn = DateTime.UtcNow
            };

        /// <summary>
        ///
        /// </summary>
        /// <param name="MessageEntity"></param>
        /// <returns></returns>
        public MessageDto MapToDto(
            MessageEntity msgEntity
        )
        => new MessageDto
            {
                // Map properties from entity to dto
                Id = msgEntity.Id,
                ChatId = msgEntity.ChatId,
                FromIdentityId = msgEntity.FromIdentityId,
                ToIdentityId = msgEntity.ToIdentityId,
                MessageType = Enum.Parse<MessageType>($"{msgEntity.MessageType}"),
                Metadata = msgEntity.Metadata,
            };
    }
}