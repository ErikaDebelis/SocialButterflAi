using System;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Data.Analysis.Entities;
using VideoDto = SocialButterflAi.Models.Analysis.Video;
using VideoEntity = SocialButterflAi.Data.Analysis.Entities.Video;

namespace SocialButterflAi.Services.Mappers
{
    public class VideoMapper : IMapper<VideoDto, VideoEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoDto"></param>
        /// <returns></returns>
        public VideoEntity MapToEntity(
            VideoDto videoDto
        )
        => new VideoEntity
            {
                // Map properties from dto to entity
                Id = videoDto.Id ?? Guid.NewGuid(),
                IdentityId = videoDto.UploaderIdentityId,
                //todo:fix this
                // MessageId = ,
                Title = videoDto.Title,
                Description = videoDto.Description,
                Path = videoDto.Url,
                VideoType = Enum.Parse<VideoType>($"{videoDto.Format}"),
                Base64 = videoDto.Base64,
                Duration = videoDto.Duration.TimeSpan,
                Captions = null,
                CreatedBy = $"{videoDto.UploaderIdentityId}",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = $"{videoDto.UploaderIdentityId}",
                ModifiedOn = DateTime.UtcNow
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoEntity"></param>
        /// <returns></returns>
        public VideoDto MapToDto(
            VideoEntity videoEntity
        )
        => new VideoDto
            {
                // Map properties from entity to dto
                Id = videoEntity.Id,
                UploaderIdentityId = videoEntity.IdentityId,
                //todo:fix this
                // MessageId = ,
                Title = videoEntity.Title,
                Description = videoEntity.Description,
                Url = videoEntity.Path,
                Format = Enum.Parse<VideoFormat>($"{videoEntity.VideoType}"),
                Base64 = videoEntity.Base64,
                Duration = new DurationData
                {
                    StartTime = "00:00:00",
                    EndTime = $"{videoEntity.Duration.Hours}:{videoEntity.Duration.Minutes}:{videoEntity.Duration.Seconds}",
                    TimeSpan = videoEntity.Duration
                },
                FileStream = null
            };
    }
}