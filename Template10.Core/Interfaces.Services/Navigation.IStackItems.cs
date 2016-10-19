using System.Collections.Generic;

namespace Template10.Interfaces.Services.Navigation
{

    public interface IStackItems: IEnumerable<IStackItem>
    {
        void Clear();
    }

}