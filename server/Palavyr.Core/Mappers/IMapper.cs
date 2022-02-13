using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Palavyr.Core.Mappers
{
    public interface IMapToNew<in TFrom, TTo>
    {
        Task<TTo> Map(TFrom @from, CancellationToken cancellationToken);
    }

    public interface IMapToPreExisting<in TFrom, in TTo>
    {
        Task Map(TFrom @from, TTo @to, CancellationToken cancellationToken);
    }

    public static class IMapperExtensionMethods
    {
        public static async Task<IEnumerable<TTo>> MapMany<TFrom, TTo>(this IMapToNew<TFrom, TTo> mapper, IEnumerable<TFrom> sources, CancellationToken cancellationToken)
        {
            var resources = new List<TTo>();
            foreach (var source in sources)
            {
                var result = await mapper.Map(source, cancellationToken);
                resources.Add(result);
            }

            return resources;
        }

        // TODO: Upgrade to .NET 6 and upgrade the servers
        // Then we can use ToArrayAsync()

        // static async IAsyncEnumerable<TTo> MapManyToIEnumerable<TFrom, TTo>(this IMapper<TFrom, TTo> mapper, IEnumerable<TFrom> sources, [EnumeratorCancellation] CancellationToken cancellationToken)
        // {
        //     foreach (var source in sources)
        //     {
        //         var resource = await mapper.Map(source, cancellationToken);
        //         if (resource != null)
        //         {
        //             yield return resource;
        //         }
        //     }
        // }
    }
}