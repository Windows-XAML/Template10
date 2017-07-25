namespace Template10.StartArgs
{
    public static class Template10StartArgsFactory
    {
        public static ITemplate10StartArgs Create(object eventArgs, Template10.Template10StartArgs.StartKinds kind)
            => new Template10StartArgs(eventArgs, kind);
    }
}
