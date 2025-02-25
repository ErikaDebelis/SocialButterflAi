using System;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Data.Analysis.Entities;
using AudioDto = SocialButterflAi.Models.Analysis.Audio;
using AudioEntity = SocialButterflAi.Data.Analysis.Entities.Audio;

namespace SocialButterflAi.Services.Mappers
{
    public class AudioMapper : IMapper<AudioDto, AudioEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AudioDto"></param>
        /// <returns></returns>
        public AudioEntity MapToEntity(
            AudioDto audioDto
        )
        => new AudioEntity
            {
                // Map properties from dto to entity
                Id = audioDto.Id,
                MessageId = null,
                Base64 = null,
                CreatedBy = $"{audioDto.UploaderIdentityId}",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = $"{audioDto.UploaderIdentityId}",
                ModifiedOn = DateTime.UtcNow
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audioEntity"></param>
        /// <returns></returns>
        public AudioDto MapToDto(
            AudioEntity audioEntity
        )
        => new AudioDto
            {
                // Map properties from entity to dto
                Id = default,
                UploaderIdentityId = default,
                MessageId = null,
                Base64 = null
            };
    }
}