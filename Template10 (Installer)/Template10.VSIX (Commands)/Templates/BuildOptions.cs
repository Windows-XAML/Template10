using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.VSIX.Commands.Templates
{
    internal class BuildOptions
    {
        public string SolutionNamespace { get; set; }
        public string PageName { get; set; }
        public bool HasModel { get; set; }
    }
}
