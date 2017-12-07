using System;
using System.Threading;
using Template10.Extensions;
using Template10.Services.Logging;

namespace Template10.Common
{
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
                this.Log("SecondaryViewSynchronizationContextDecorator : OperationStarted: " + count);
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
                this.Log("SecondaryViewSynchronizationContextDecorator : OperationCompleted: " + count);
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
