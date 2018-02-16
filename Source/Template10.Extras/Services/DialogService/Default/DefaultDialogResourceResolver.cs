using System;

namespace Template10.Services.Dialog
{

    public class DefaultResourceResolver : IDialogResourceResolver
    {
        public DefaultResourceResolver() => Resolve = DefaultResolve;

        public Func<ResourceTypes, string> Resolve { get; set; }

        private string DefaultResolve(ResourceTypes resource)
        {
            switch (resource)
            {
                case ResourceTypes.Ok: return "Ok";
                case ResourceTypes.Yes: return "Yes";
                case ResourceTypes.No: return "No";
                case ResourceTypes.Cancel: return "Cancel";
                default: throw new NotSupportedException($"Resource:{resource}");
            }
        }
    }
}
