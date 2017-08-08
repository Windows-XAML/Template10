using System;
using System.Threading;
using Template10.Services.Logging;

namespace Template10.Core
{
    partial class SecondaryViewSynchronizationContextDecorator : ILoggable
    {
        ILoggingService ILoggable.LoggingService { get; set; } = Central.LoggingService;
        void LogThis(string text = null, Severities severity = Severities.Template10, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
            => (this as ILoggable).LogThis(text, severity, caller: $"{GetType()}.{caller}");
        void ILoggable.LogThis(string text, Severities severity, string caller)
            => (this as ILoggable).LoggingService.WriteLine(text, severity, caller: $"{GetType()}.{caller}");
    }

    partial class SecondaryViewSynchronizationContextDecorator : SynchronizationContext
    {
        private readonly IViewLifetimeControl control;
        private readonly SynchronizationContext context;

        public SecondaryViewSynchronizationContextDecorator(IViewLifetimeControl control, SynchronizationContext context)
        {
            this.control = control ?? throw new ArgumentNullException(nameof(control));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override void OperationStarted()
        {

            try
            {
                var count = control.StartViewInUse();
                LogThis("SecondaryViewSynchronizationContextDecorator : OperationStarted: " + count);
                context.OperationStarted();
            }
            catch (ViewLifeTimeException)
            {
                //Don't need to do anything, operation can't be started
            }

        }

        public override void Send(SendOrPostCallback d, object state)
        {
            context.Send(d, state);
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            context.Post(d, state);
        }

        public override void OperationCompleted()
        {
            try
            {
                context.OperationCompleted();
                var count = control.StopViewInUse();
                LogThis("SecondaryViewSynchronizationContextDecorator : OperationCompleted: " + count);
            }
            catch (ViewLifeTimeException)
            {
                //Don't need to do anything, operation can't be completed
            }
        }

        public override SynchronizationContext CreateCopy()
        {
            var copyControl = ViewLifetimeControl.GetForCurrentView();
            copyControl = copyControl != control ? copyControl : control;
            return new SecondaryViewSynchronizationContextDecorator(copyControl, context.CreateCopy());
        }
    }
}
