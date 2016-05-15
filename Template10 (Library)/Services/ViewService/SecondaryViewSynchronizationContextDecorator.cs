using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logging=Template10.Services.LoggingService.LoggingService;

namespace Template10.Services.ViewService
{
    class SecondaryViewSynchronizationContextDecorator : SynchronizationContext
    {
        private readonly ViewLifetimeControl control;
        private readonly SynchronizationContext context;

        public SecondaryViewSynchronizationContextDecorator(ViewLifetimeControl control, SynchronizationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            this.control = control;
            this.context = context;
        }


        public override void OperationStarted()
        {

            try
            {
                var count = control.StartViewInUse();
                Logging.WriteLine("SecondaryViewSynchronizationContextDecorator : OperationStarted: " + count);
                context.OperationStarted();
            }
            catch (ViewLifetimeControl.ViewLifeTimeException)
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
                Logging.WriteLine("SecondaryViewSynchronizationContextDecorator : OperationCompleted: " + count);
            }
            catch (ViewLifetimeControl.ViewLifeTimeException)
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
