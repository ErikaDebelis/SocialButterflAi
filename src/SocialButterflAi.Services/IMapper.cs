namespace SocialButterflAi.Services
{
    public interface IMapper<TDto, TEntity>
    {
        TEntity MapToEntity(TDto dto);
        TDto MapToDto(TEntity entity);
    }
}