using System;
using SocialButterflAi.Models.Analysis;
using SocialButterflAi.Data.Analysis.Entities;
using ImageDto = SocialButterflAi.Models.Analysis.Image;
using ImageEntity = SocialButterflAi.Data.Analysis.Entities.Image;

namespace SocialButterflAi.Services.Mappers
{
    public class ImageMapper : IMapper<ImageDto, ImageEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageDto"></param>
        /// <returns></returns>
        public ImageEntity MapToEntity(
            ImageDto imageDto
        )
        => new ImageEntity
            {
                // Map properties from dto to entity
                Id = imageDto.Id,
                IdentityId = imageDto.UploaderIdentityId,
                //todo:fix this
                // MessageId = ,
                Title = imageDto.Title,
                Description = imageDto.Description,
                CreatedBy = $"{imageDto.UploaderIdentityId}",
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = $"{imageDto.UploaderIdentityId}",
                ModifiedOn = DateTime.UtcNow
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageEntity"></param>
        /// <returns></returns>
        public ImageDto MapToDto(
            ImageEntity imageEntity
        )
        => new ImageDto
            {
                // Map properties from entity to dto
                Id = imageEntity.Id,
                UploaderIdentityId = imageEntity.IdentityId,
                //todo:fix this
                // MessageId = ,
                Title = imageEntity.Title,
                Description = imageEntity.Description
            };
    }
}