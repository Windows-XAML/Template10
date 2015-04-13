using System;

namespace Template10.Mvvm.Container
{
    public interface IContainer
    {
        T Resolve<T>(T type) 
            where T : System.Type;

        string Register<T, C>(Lifetimes lifetime = Lifetimes.Singleton)
            where T : System.Type
            where C : T;

        string Register<T>(T instance)
            where T : System.Type;

        string Register<T>(Func<T> factory, Lifetimes lifetime = Lifetimes.Singleton) 
            where T : System.Type;

        bool IsRegistered<T>()
            where T : System.Type;

        void Unregister<T>() 
            where T : System.Type;

        void Unregister<T>(string key)
            where T : System.Type;

        void Unregister<T>(T instance)
            where T : System.Type;
    }
}