using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.DependencyInjection
{
    public interface IDependencyService : IContainerConsumer, IContainerBuilder
    {
        // empty
    }
}
