using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace Template10.Services.KeyboardService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-KeyboardService
    public class KeyboardEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;  
        public bool AltKey { get; set; }
        public bool ControlKey { get; set; }
        public bool ShiftKey { get; set; }
        public VirtualKey VirtualKey { get; set; }
        public AcceleratorKeyEventArgs EventArgs { get; set; }
        public char? Character { get; set; }
        public bool WindowsKey { get; internal set; }

        public bool OnlyWindows => this.WindowsKey & !this.AltKey & !this.ControlKey & !this.ShiftKey;
        public bool OnlyAlt => !this.WindowsKey & this.AltKey & !this.ControlKey & !this.ShiftKey;
        public bool OnlyControl => !this.WindowsKey & !this.AltKey & this.ControlKey & !this.ShiftKey;
        public bool OnlyShift => !this.WindowsKey & !this.AltKey & !this.ControlKey & this.ShiftKey;

    }
}
