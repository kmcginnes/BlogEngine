using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogEngine
{
    public static class Database
    {
        private static IDictionary<Type, ICollection<object>> DB =
            new Dictionary<Type, ICollection<object>>();

        public static void Add<T>(T dto)
        {
            var set = GetOrCreateSet<T>();
            set.Add(dto);
        }

        public static IEnumerable<T> Get<T>()
        {
            var set = GetOrCreateSet<T>();
            return set.Cast<T>();
        }

        private static ICollection<object> GetOrCreateSet<T>()
        {
            ICollection<object> set = null;
            if (!DB.TryGetValue(typeof(T), out set))
            {
                set = new HashSet<object>();
                DB.Add(typeof(T), set);
                DB.Log().Debug("Creating set for {0}", typeof(T).Name);
            }
            return set;
        }
    }
}