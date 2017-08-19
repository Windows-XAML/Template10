using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Container
{
    public interface IContainerService 
    {
        T Resolve<T>() where T : class;
        void Register<F, T>() where F : class where T : class, F;
        void Register<F>(F instance) where F : class;
    }
}
