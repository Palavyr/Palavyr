using System.Buffers;
using Newtonsoft.Json;

namespace Palavyr.API.CustomMiddleware
{
    public class ArrayPool : IArrayPool<char>
    {
        public char[] Rent(int minimumLength)
        {
            return ArrayPool<char>.Shared.Rent(minimumLength);
        }

        public void Return(char[] array)
        {
            ArrayPool<char>.Shared.Return(array);
        }
    }
}