using System;

namespace SocialButterflAi.Services.Mappers
{
    public interface IMapper<TDto, TEntity>
    {
        TEntity MapToEntity(TDto dto);

        TDto MapToDto(TEntity entity);
    }
}