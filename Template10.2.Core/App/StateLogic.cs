using System;
using System.Collections.Generic;
using System.Linq;

namespace Template10.App
{
    public class StateLogic : Dictionary<DateTime, BootstrapperStates>
    {
        public void Add(BootstrapperStates state)
        {
            this.DebugWriteInfo($"{DateTime.Now}, {state}");
            Add(DateTime.Now, state);
        }

        public BootstrapperStates Current => this.Any() ? this.First().Value : default(BootstrapperStates);

        public bool Exists(BootstrapperStates state) => ContainsValue(state);

        public new int Count(BootstrapperStates state) => this.Where(x => x.Value == state).Count();
    }
}
