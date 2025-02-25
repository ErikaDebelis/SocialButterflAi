using System;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Data.Analysis.Entities;
using CaptionDto = SocialButterflAi.Models.Analysis.EnhancedCaption;
using CaptionEntity = SocialButterflAi.Data.Analysis.Entities.EnhancedCaption;

namespace SocialButterflAi.Services.Mappers
{
    public class CaptionMapper : IMapper<CaptionDto, CaptionEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CaptionData"></param>
        /// <returns></returns>
        public CaptionEntity MapToEntity(
            CaptionDto CaptionDto
        )
        => new CaptionEntity
            {
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CaptionEntity"></param>
        /// <returns></returns>
        public CaptionDto MapToDto(
            CaptionEntity CaptionEntity
        )
        => new CaptionDto
            {
                // Map properties from entity to dto
                Id = CaptionEntity.Id,
            };
    }
}