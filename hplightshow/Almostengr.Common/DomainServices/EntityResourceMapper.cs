using Almostengr.Common.Domain;

namespace Almostengr.Common.DomainServices;

public static class EntityResourceMapper
{
    public static TResource ToResource<TEntity, TResource>(this TEntity entity) where TResource : BaseResource, new() where TEntity : BaseEntity
    {
        if (entity == null)
        {
            return null;
        }

        return new TResource
        {
            Guid = entity.Guid
        };
    }

    public static TEntity ToEntity<TEntity, TResource>(this TResource resource) where TResource : BaseResource where TEntity : BaseEntity, new()
    {
        if (resource == null)
        {
            return null;
        }

        return new TEntity();
    }
}
