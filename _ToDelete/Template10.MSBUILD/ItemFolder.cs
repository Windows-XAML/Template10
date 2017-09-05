using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.MSBUILD
{
    /// <summary>
    /// Used to keep track of the Item folders during conversion in vstemplate
    /// </summary>
    internal class ItemFolder
    {
        internal ItemFolder()
        {

            Folders = new List<ItemFolder>();
            Items = new List<string>();
        }

        internal string FolderName { get; set; }

        internal List<ItemFolder> Folders { get; set; }

        internal List<string> Items { get; set; }

    }
}
