using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.BCL;

namespace Template10.BCL
{
    public interface ILogic
    {
    }

    public interface ILogicHost<T> where T: ILogic
    {
        T Instance { get; set; }
    }
}

namespace Template10
{
    public static class Extensions
    {
        public static T GetProvider<T>(this ILogicHost<T> host) where T : ILogic
        {
            return host.Instance;
        }

        public static void SetProvider<T>(this ILogicHost<T> host, T value) where T : ILogic
        {
            host.Instance = value;
        }
    }
}