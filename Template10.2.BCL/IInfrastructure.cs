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

    public interface IInfrastructure<T> where T: IService
    {
        T Instance { get; set; }
    }
}

namespace Template10
{
    public static class Extensions
    {
        public static T GetInfrastructure<T>(this IInfrastructure<T> accessor) where T : IService
        {
            return accessor.Instance;
        }

        public static void SetInfrastructure<T>(this IInfrastructure<T> accessor, T value) where T : IService
        {
            accessor.Instance = value;
        }
    }
}