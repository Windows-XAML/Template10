using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.App
{
    public class StateLogic : Dictionary<DateTime, BootstrapperStates>
    {
        public void Add(BootstrapperStates state)
        {
            this.DebugWriteMessage($"{DateTime.Now}, {state}");
            Add(DateTime.Now, state);
        }

        public BootstrapperStates Current => this.Any() ? this.First().Value : default(BootstrapperStates);

        public bool Exists(BootstrapperStates state) => ContainsValue(state);

        public new int Count(BootstrapperStates state) => this.Where(x => x.Value == state).Count();
    }
}
