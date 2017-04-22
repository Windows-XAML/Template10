using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.MSBUILD
{
    /// <summary>
    /// Used in matching and replacing.
    /// </summary>
    internal class FindReplaceItem
    {
        internal string Pattern { get; set; }
        internal string Replacement { get; set; }
    }
}
