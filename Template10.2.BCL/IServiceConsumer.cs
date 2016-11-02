using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.BCL;

namespace Template10.BCL
{
    public interface IService
    {
    }

    public interface IServiceHost<T> where T: IService
    {
        T Instance { get; set; }
    }
}

namespace Template10
{
    public static class Extensions
    {
        public static T GetService<T>(this IServiceHost<T> accessor) where T : IService
        {
            return accessor.Instance;
        }

        public static void SetService<T>(this IServiceHost<T> accessor, T value) where T : IService
        {
            accessor.Instance = value;
        }
    }
}