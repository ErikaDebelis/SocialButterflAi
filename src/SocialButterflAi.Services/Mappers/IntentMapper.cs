using System;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Data.Analysis.Entities;
using IntentDto = SocialButterflAi.Models.Analysis.Intent;
using IntentEntity = SocialButterflAi.Data.Analysis.Entities.Intent;

namespace SocialButterflAi.Services.Mappers
{
    public class IntentMapper : IMapper<IntentDto, IntentEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntentData"></param>
        /// <returns></returns>
        public IntentEntity MapToEntity(
            IntentDto IntentDto
        )
        => new IntentEntity
            {
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntentEntity"></param>
        /// <returns></returns>
        public IntentDto MapToDto(
            IntentEntity IntentEntity
        )
        => new IntentDto
            {
                // Map properties from entity to dto
                Id = IntentEntity.Id,
            };
    }
}