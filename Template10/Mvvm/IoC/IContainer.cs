using System;

namespace Template10.Mvvm.IoC
{
    public interface IContainer
    {
        bool IsRegistered<T>() 
            where T : class;

        void Register<T, C>()
            where T : class
            where C : class, T;

        void Register<T>(T instance)
            where T : class;

        T Resolve<T>();
    }
}