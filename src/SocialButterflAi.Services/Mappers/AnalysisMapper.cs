using System;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Data.Analysis.Entities;
using AnalysisEntity = SocialButterflAi.Data.Analysis.Entities.Analysis;

namespace SocialButterflAi.Services.Mappers
{
    public class AnalysisMapper : IMapper<AnalysisData, AnalysisEntity>
    {
        public ToneMapper ToneMapper = new ToneMapper();
        public IntentMapper IntentMapper = new IntentMapper();
        public CaptionMapper CaptionMapper = new CaptionMapper();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisData"></param>
        /// <returns></returns>
        public AnalysisEntity MapToEntity(
            AnalysisData analysisDto
        )
        => new AnalysisEntity
            {
                Id = analysisDto.Id ?? Guid.NewGuid(),
                CaptionId = null,
                Caption = CaptionMapper.MapToEntity(analysisDto.Caption),
                Type = Enum.Parse<Data.Analysis.Entities.MediaType>($"{analysisDto.Type}"),
                Certainty = 0,
                EnhancedDescription = null,
                Tone = ToneMapper.MapToEntity(analysisDto.Tone),
                Intent = IntentMapper.MapToEntity(analysisDto.Intent),
                Metadata = null,
                CreatedBy = $"{analysisDto.IdentityId}",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = $"{analysisDto.IdentityId}",
                ModifiedOn = DateTime.UtcNow
            };

        /// <summary>
        ///
        /// </summary>
        /// <param name="analysisEntity"></param>
        /// <returns></returns>
        public AnalysisData MapToDto(
            AnalysisEntity analysisEntity
        )
        => new AnalysisData
            {
                // Map properties from entity to dto
                Id = analysisEntity.Id,
            };
    }
}