namespace Palavyr.Core.Resources
{
    public class EntityResource : IEntityResource
    {
        public int Id { get; set; }
    }

    public class NullableEntityResource : INullableResource
    {
        public int? Id { get; set; }
    }

    public interface IEntityResource : IResource
    {
        public int Id { get; set; }
    }

    public interface INullableResource : IResource
    {
        public int? Id { get; set; }
    }
}