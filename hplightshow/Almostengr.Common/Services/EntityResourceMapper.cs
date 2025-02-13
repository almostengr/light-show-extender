using Almostengr.Common.Repositories;

namespace Almostengr.Common.Services;

public static class EntityResourceMapper<TEntity, TResource> where TEntity : BaseEntity, new() where TResource : BaseResource, new()
{
    public static TResource ToResource(TEntity entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new TResource
        {
            ReferenceId = entity.ReferenceId
        };
    }

    // public static TEntity ToEntity(TResource resource)
    // {
    //     if (resource == null)
    //     {
    //         return null;

    //     }

    //     return TEntity.Create(resource.ReferenceId);
    // }
}