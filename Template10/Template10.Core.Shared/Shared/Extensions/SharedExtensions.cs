using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Services.Serialization;

namespace Template10.Extensions
{
    public static class SharedExtensions
    {
        [Obsolete("Ensure service locally")]
        public static object DeserializeEx(this string parameter, ISerializationService serializer = null)
            => Central.Serialization.Deserialize(parameter);

        [Obsolete("Ensure service locally")]
        public static T DeserializeEx<T>(this string parameter, ISerializationService serializer = null)
            => Central.Serialization.Deserialize<T>(parameter);

        [Obsolete("Ensure service locally")]
        public static string SerializeEx(this object parameter, ISerializationService serializer = null)
            => Central.Serialization.Serialize(parameter);

        [Obsolete("Ensure service locally")]
        public static bool TrySerializeEx(this object parameter, out string result, ISerializationService serializer = null)
            => Central.Serialization.TrySerialize(parameter, out result);

        [Obsolete("Ensure service locally")]
        public static bool TryDeserializeEx<T>(this string parameter, out T result, ISerializationService serializer = null)
            => Central.Serialization.TryDeserialize(parameter, out result);

        private static readonly TaskFactory _factory = new
          TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        [Obsolete("Use .Result")]
        public static T RunAsSync<T>(this Func<Task<T>> func)
            => _factory.StartNew(func).Unwrap().GetAwaiter().GetResult();

        [Obsolete("Use .Result")]
        public static void RunAsSync(this Func<Task> func)
            => _factory.StartNew(func).Unwrap().GetAwaiter().GetResult();
    }
}
