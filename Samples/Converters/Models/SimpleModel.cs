using System;

namespace Template10.Samples.ConvertersSample.Models
{
    public class SimpleModel
    {
        static Random r = new Random();
        public SimpleModel()
        {
            IntProperty = r.Next(-int.MaxValue, +int.MaxValue);
            DateProperty = DateTime.Today.AddDays(r.Next(-365, 365));
        }

        public int IntProperty { get; set; }
        public DateTime DateProperty { get; set; }
    }
}
