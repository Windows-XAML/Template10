using System;

namespace Template10.Interfaces.Services.Navigation
{

    public interface IStackItem
    {
        Type Page { get; set; }
        object Parameter { get; set; }
    }

}