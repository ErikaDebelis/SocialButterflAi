using System;

using SocialButterflAi.Data.Identity;

using SocialButterflAi.Models;

namespace SocialButterflAi.Services.Mappers
{
    public interface IMapper<TDto, TEntity> where TDto : BaseDto where TEntity : BaseEntity
    {
        TEntity MapToEntity(TDto dto);

        TDto MapToDto(TEntity entity);
    }
}