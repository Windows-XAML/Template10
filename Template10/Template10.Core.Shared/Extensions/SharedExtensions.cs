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
        private static ISerializationService SerializationService
            => Services.Container.ContainerService.Default.Resolve<ISerializationService>();

        public static object DeserializeEx(this string parameter, ISerializationService serializer = null)
            => (serializer ?? SerializationService).Deserialize(parameter);

        public static T DeserializeEx<T>(this string parameter, ISerializationService serializer = null)
            => (serializer ?? SerializationService).Deserialize<T>(parameter);

        public static string SerializeEx(this object parameter, ISerializationService serializer = null)
            => (serializer ?? SerializationService).Serialize(parameter);

        public static bool TrySerializeEx(this object parameter, out string result, ISerializationService serializer = null)
            => (serializer ?? SerializationService).TrySerialize(parameter, out result);

        public static bool TryDeserializeEx<T>(this string parameter, out T result, ISerializationService serializer = null)
            => (serializer ?? SerializationService).TryDeserialize(parameter, out result);

        private static readonly TaskFactory _factory = new
          TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        public static T RunAsSync<T>(this Func<Task<T>> func)
            => _factory.StartNew(func).Unwrap().GetAwaiter().GetResult();

        public static void RunAsSync(this Func<Task> func)
            => _factory.StartNew(func).Unwrap().GetAwaiter().GetResult();
    }
}
