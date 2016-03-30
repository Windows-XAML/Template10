using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace DynamicFontSize.Models
{
    public class Body : IBlock
    {
        public string Text { get; set; }
        public Run ToRun() => new Run { Text = Text };
    }
}
