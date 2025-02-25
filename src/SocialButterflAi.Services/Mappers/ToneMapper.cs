using System;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Data.Analysis.Entities;
using ToneDto = SocialButterflAi.Models.Analysis.Tone;
using ToneEntity = SocialButterflAi.Data.Analysis.Entities.Tone;

namespace SocialButterflAi.Services.Mappers
{
    public class ToneMapper : IMapper<ToneDto, ToneEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToneData"></param>
        /// <returns></returns>
        public ToneEntity MapToEntity(
            ToneDto ToneDto
        )
        => new ToneEntity
            {
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToneEntity"></param>
        /// <returns></returns>
        public ToneDto MapToDto(
            ToneEntity ToneEntity
        )
        => new ToneDto
            {
                // Map properties from entity to dto
                Id = ToneEntity.Id,
            };
    }
}