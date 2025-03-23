using System.Collections.Generic;
using System.Linq;

namespace Runtime.Backend
{
    public static class LinqExtensions
    {
        public static IEnumerable<(T1, T2, T3)> ZipThree<T1, T2, T3>(
            IEnumerable<T1> first, IEnumerable<T2> second, IEnumerable<T3> third)
        {
            return first.Zip(second, (f, s) => (f, s))
                .Zip(third, (fs, t) => (fs.f, fs.s, t));
        }
    }
}