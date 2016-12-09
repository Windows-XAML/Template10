using System;
using System.Linq;
using Windows.System;
using Windows.UI.Core;

namespace Template10.Services.KeyboardService
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Services#keyboardservice
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

        public bool OnlyWindows => WindowsKey & !AltKey & !ControlKey & !ShiftKey;
        public bool OnlyAlt => !WindowsKey & AltKey & !ControlKey & !ShiftKey;
        public bool OnlyControl => !WindowsKey & !AltKey & ControlKey & !ShiftKey;
        public bool OnlyShift => !WindowsKey & !AltKey & !ControlKey & ShiftKey;
        public bool Combo => new[] { AltKey, ControlKey, ShiftKey }.Any(x => x) & Character.HasValue;

        public override string ToString()
        {
            return $"KeyboardEventArgs = Handled {Handled}, AltKey {AltKey}, ControlKey {ControlKey}, ShiftKey {ShiftKey}, VirtualKey {VirtualKey}, Character {Character}, WindowsKey {WindowsKey}, OnlyWindows {OnlyWindows}, OnlyAlt {OnlyAlt}, OnlyControl {OnlyControl}, OnlyShift {OnlyShift}";
        }
    }
}
