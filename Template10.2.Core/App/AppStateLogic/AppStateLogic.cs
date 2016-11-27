using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template10.BCL;

namespace Template10.App
{
    public interface IAppStateLogic : ILogic
    {
        IReadOnlyDictionary<DateTime, AppStates> History { get; }
        void Add(AppStates state, DateTime? date = null);
        bool Exists(DateTime date);
        bool Exists(AppStates state);
    }

    public class AppStateLogic : IAppStateLogic
    {
        public static AppStateLogic Instance { get; } = new AppStateLogic();
        private AppStateLogic()
        {
            // private
        }

        public IReadOnlyDictionary<DateTime, AppStates> History { get; private set; } = new ReadOnlyDictionary<DateTime, AppStates>(null);

        public void Add(AppStates state, DateTime? date = null)
        {
            this.LogInfo($"{DateTime.Now}, {state}");
            var key = date.HasValue ? date.Value : DateTime.Now;
            History = History.Add(key, state);
        }

        public AppStates Current => History.Any() ? History.First().Value : default(AppStates);

        public bool Exists(DateTime date) => History.ContainsKey(date);

        public bool Exists(AppStates state) => History.Any(x => x.Value == state);
    }

    public static partial class Extensions
    {
        public static IReadOnlyDictionary<K, V> Add<K, V>(this IReadOnlyDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary;
            }
            var item = new KeyValuePair<K, V>(key, value);
            var array = new[] { item };
            return array.Union(dictionary).ToDictionary(x => x.Key, y => y.Value);
        }
    }
}
