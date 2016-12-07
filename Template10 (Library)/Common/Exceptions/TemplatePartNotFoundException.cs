using System;

namespace Template10.Common
{
    public class TemplatePartNotFoundException : Exception
    {
        public TemplatePartNotFoundException(string message) : base(message) { }
    }
}