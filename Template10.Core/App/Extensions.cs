using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Template10.Helpers;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Template10.App;

namespace System
{
    internal static class Extensions
    {
        public static void DebugWriteMessage(this object item, string text = null, [Runtime.CompilerServices.CallerMemberName]string caller = null, [Runtime.CompilerServices.CallerLineNumber]int line = -1, [Runtime.CompilerServices.CallerFilePath]string path = null)
        {
            Diagnostics.Debug.WriteLine($"MSG {DateTime.Now} {text} caller: {caller} line: {line} path: {path}");
        }

        public static void DebugWriteError(this object item, string text = null, [Runtime.CompilerServices.CallerMemberName]string caller = null, [Runtime.CompilerServices.CallerLineNumber]int line = -1, [Runtime.CompilerServices.CallerFilePath]string path = null)
        {
            Diagnostics.Debug.WriteLine($"ERR {DateTime.Now} {text} caller: {caller} line: {line} path: {path}");
        }

        public static int Occurances(this Dictionary<DateTime, LoadStates> list, LoadStates state) 
            => list.Count(x => x.Value.Equals(state));
    }
}