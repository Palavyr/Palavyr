using System.Collections.Generic;
using System.Linq;

namespace Test.Common.ExtensionsMethods
{
    public static class EnumerableExtensionMethods
    {
        // https://stackoverflow.com/questions/47815660/does-c-sharp-7-have-array-enumerable-destructuring
        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out IEnumerable<T> rest)
        {
            var enumerable = seq as T[] ?? seq.ToArray();
            first = enumerable.FirstOrDefault();
            rest = enumerable.Skip(1);
        }

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out IEnumerable<T> rest)
            => (first, (second, rest)) = seq;

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out IEnumerable<T> rest)
            => (first, second, (third, rest)) = seq;

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out IEnumerable<T> rest)
            => (first, second, third, (fourth, rest)) = seq;

        public static void Deconstruct<T>(this IEnumerable<T> seq, out T first, out T second, out T third, out T fourth, out T fifth, out IEnumerable<T> rest)
            => (first, second, third, fourth, (fifth, rest)) = seq;
        
        // If you need to deconstruct more things... just add more deconstructors here
    }
}