namespace ObjectLibrary.Common
{
    public interface IBaseEntity<EntityType>
    {
        EntityType Id { get; set; }
    }
}