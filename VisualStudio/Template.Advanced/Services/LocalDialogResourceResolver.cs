using System;
using Template10.Services.Dialog;
using Template10.Services.Resources;

namespace Sample.Services
{
    public class LocalDialogResourceResolver : IDialogResourceResolver
    {
        protected IResourceService _resourceService;

        public LocalDialogResourceResolver(IResourceService resources)
        {
            _resourceService = resources;
            Resolve = ResolveImplmenetation;
        }

        private string ResolveImplmenetation(ResourceTypes arg)
        {
            switch (arg)
            {
                case ResourceTypes.Yes: return _resourceService.GetLocalizedString("AreYouSure_Button1Text");
                case ResourceTypes.No: return _resourceService.GetLocalizedString("AreYouSure_Button2Text");
                default: throw new NotImplementedException();
            }
        }

        public Func<ResourceTypes, string> Resolve { get; set; }
    }
}
