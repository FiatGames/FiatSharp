using System;
using System.Collections.Generic;
using System.Linq;

namespace FiatSharp.Examples
{
    public static class LinqExtensions
    {
        public static IEnumerable<HashSet<T>> Permute<T>(this List<T> source, int k) where T : IEquatable<T>
        {
            List<List<T>> _permute(IEnumerable<T> s, int r) => s.SelectMany(c =>
            {
                if (r == 1) return new List<List<T>> { new List<T> { c } };

                var ps = _permute(source.Where(x => !x.Equals(c)), r - 1);
                foreach (var p in ps) p.Add(c);
                return ps;
            }).ToList();

            return _permute(source, k).Select(c => new HashSet<T>(c)).Distinct(HashSet<T>.CreateSetComparer());

        }
    }
}