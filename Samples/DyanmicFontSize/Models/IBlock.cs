using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFontSize.Models
{
    public interface IBlock
    {
        string Text { get; set; }
        Windows.UI.Xaml.Documents.Run ToRun();
    }
}
