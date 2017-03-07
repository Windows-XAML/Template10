using System;

namespace Template10.Services.KeyboardService
{
    public interface IKeyboardService
    {
        Action AfterBackGesture { get; set; }
        Action AfterControlEGesture { get; set; }
        Action AfterForwardGesture { get; set; }
        Action AfterMenuGesture { get; set; }
    }
}