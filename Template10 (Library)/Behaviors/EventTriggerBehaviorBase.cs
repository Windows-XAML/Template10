using System;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

namespace Template10.Behaviors
{
    /// <summary>
    /// A behavior that listens for a specified event on its source and executes its actions when that event is fired.
    /// </summary>
    [ContentPropertyAttribute(Name = "Actions")]
    public class EventTriggerBehaviorBase : Behavior
    {
        /// <summary>
        /// Identifies the <seealso cref="Actions"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(
            "Actions",
            typeof(ActionCollection),
            typeof(EventTriggerBehaviorBase),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <seealso cref="EventName"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
            "EventName",
            typeof(string),
            typeof(EventTriggerBehaviorBase),
            new PropertyMetadata("Loaded", new PropertyChangedCallback(EventTriggerBehaviorBase.OnEventNameChanged)));

        /// <summary>
        /// Identifies the <seealso cref="SourceObject"/> dependency property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly DependencyProperty SourceObjectProperty = DependencyProperty.Register(
            "SourceObject",
            typeof(object),
            typeof(EventTriggerBehaviorBase),
            new PropertyMetadata(null, new PropertyChangedCallback(EventTriggerBehaviorBase.OnSourceObjectChanged)));

        private object resolvedSource;
        private Delegate eventHandler;
        private bool isLoadedEventRegistered;
        private bool isWindowsRuntimeEvent;
        private Func<Delegate, EventRegistrationToken> addEventHandlerMethod;
        private Action<EventRegistrationToken> removeEventHandlerMethod;

        /// <summary>
        /// Resolved source.
        /// </summary>
        protected object ResolvedSource => resolvedSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTriggerBehaviorBase"/> class.
        /// </summary>
        public EventTriggerBehaviorBase()
        {
        }

        /// <summary>
        /// Gets the collection of actions associated with the behavior. This is a dependency property.
        /// </summary>
        public ActionCollection Actions
        {
            get
            {
                ActionCollection actionCollection = (ActionCollection)this.GetValue(EventTriggerBehaviorBase.ActionsProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    this.SetValue(EventTriggerBehaviorBase.ActionsProperty, actionCollection);
                }

                return actionCollection;
            }
        }

        /// <summary>
        /// Gets or sets the name of the event to listen for. This is a dependency property.
        /// </summary>
        public string EventName
        {
            get
            {
                return (string)this.GetValue(EventTriggerBehaviorBase.EventNameProperty);
            }

            set
            {
                this.SetValue(EventTriggerBehaviorBase.EventNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the source object from which this behavior listens for events.
        /// If <seealso cref="SourceObject"/> is not set, the source will default to <seealso cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>. This is a dependency property.
        /// </summary>
        public object SourceObject
        {
            get
            {
                return (object)this.GetValue(EventTriggerBehaviorBase.SourceObjectProperty);
            }

            set
            {
                this.SetValue(EventTriggerBehaviorBase.SourceObjectProperty, value);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.SetResolvedSource(this.ComputeResolvedSource());
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.SetResolvedSource(null);
        }

        private void SetResolvedSource(object newSource)
        {
            if (this.AssociatedObject == null || this.resolvedSource == newSource)
            {
                return;
            }

            if (this.resolvedSource != null)
            {
                this.UnregisterEvent(this.EventName);
            }

            this.resolvedSource = newSource;

            if (this.resolvedSource != null)
            {
                this.RegisterEvent(this.EventName);
            }
        }

        private object ComputeResolvedSource()
        {
            // If the SourceObject property is set at all, we want to use it. It is possible that it is data
            // bound and bindings haven't been evaluated yet. Plus, this makes the API more predictable.
            if (this.ReadLocalValue(EventTriggerBehaviorBase.SourceObjectProperty) != DependencyProperty.UnsetValue)
            {
                return this.SourceObject;
            }

            return this.AssociatedObject;
        }

        private void RegisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName != "Loaded")
            {
                Type sourceObjectType = this.resolvedSource.GetType();
                EventInfo info = sourceObjectType.GetRuntimeEvent(eventName);
                if (info == null)
                {
                    throw new ArgumentException(string.Format(                        
                        "Can not find event {0} for the class {1}",
                        this.EventName,
                        sourceObjectType.Name));
                }

                MethodInfo methodInfo = typeof(EventTriggerBehaviorBase).GetTypeInfo().GetDeclaredMethod("OnEvent");
                this.eventHandler = methodInfo.CreateDelegate(info.EventHandlerType, this);

                this.isWindowsRuntimeEvent = EventTriggerBehaviorBase.IsWindowsRuntimeEvent(info);
                if (this.isWindowsRuntimeEvent)
                {
                    this.addEventHandlerMethod = add => (EventRegistrationToken)info.AddMethod.Invoke(this.resolvedSource, new object[] { add });
                    this.removeEventHandlerMethod = token => info.RemoveMethod.Invoke(this.resolvedSource, new object[] { token });

                    WindowsRuntimeMarshal.AddEventHandler(this.addEventHandlerMethod, this.removeEventHandlerMethod, this.eventHandler);
                }
                else
                {
                    info.AddEventHandler(this.resolvedSource, this.eventHandler);
                }
            }
            else if (!this.isLoadedEventRegistered)
            {
                FrameworkElement element = this.resolvedSource as FrameworkElement;
                if (element != null && !EventTriggerBehaviorBase.IsElementLoaded(element))
                {
                    this.isLoadedEventRegistered = true;
                    element.Loaded += this.OnEvent;
                }
            }
        }

        private void UnregisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName != "Loaded")
            {
                if (this.eventHandler == null)
                {
                    return;
                }

                EventInfo info = this.resolvedSource.GetType().GetRuntimeEvent(eventName);
                if (this.isWindowsRuntimeEvent)
                {
                    WindowsRuntimeMarshal.RemoveEventHandler(this.removeEventHandlerMethod, this.eventHandler);
                }
                else
                {
                    info.RemoveEventHandler(this.resolvedSource, this.eventHandler);
                }

                this.eventHandler = null;
            }
            else if (this.isLoadedEventRegistered)
            {
                this.isLoadedEventRegistered = false;
                FrameworkElement element = (FrameworkElement)this.resolvedSource;
                element.Loaded -= this.OnEvent;
            }
        }

        /// <summary>
        /// Actions to be done on event triggering. In default implementation list of actions is executed immediately.
        /// This is a main class extension point if some additional actions should be done on event trigger.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="eventArgs">Event argument.</param>
        protected virtual void OnEvent(object sender, object eventArgs)
        {
            Interaction.ExecuteActions(this.resolvedSource, this.Actions, eventArgs);
        }

        private static void OnSourceObjectChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            EventTriggerBehaviorBase behavior = (EventTriggerBehaviorBase)dependencyObject;
            behavior.SetResolvedSource(behavior.ComputeResolvedSource());
        }

        private static void OnEventNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            EventTriggerBehaviorBase behavior = (EventTriggerBehaviorBase)dependencyObject;
            if (behavior.AssociatedObject == null || behavior.resolvedSource == null)
            {
                return;
            }

            string oldEventName = (string)args.OldValue;
            string newEventName = (string)args.NewValue;

            behavior.UnregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }

        internal static bool IsElementLoaded(FrameworkElement element)
        {
            if (element == null)
            {
                return false;
            }

            UIElement rootVisual = Window.Current.Content;
            DependencyObject parent = element.Parent;
            if (parent == null)
            {
                // If the element is the child of a ControlTemplate it will have a null parent even when it is loaded.
                // To catch that scenario, also check it's parent in the visual tree.
                parent = VisualTreeHelper.GetParent(element);
            }

            return (parent != null || (rootVisual != null && element == rootVisual));
        }

        private static bool IsWindowsRuntimeEvent(EventInfo eventInfo)
        {
            return eventInfo != null &&
                EventTriggerBehaviorBase.IsWindowsRuntimeType(eventInfo.EventHandlerType) &&
                EventTriggerBehaviorBase.IsWindowsRuntimeType(eventInfo.DeclaringType);
        }

        private static bool IsWindowsRuntimeType(Type type)
        {
            if (type != null)
            {
                return type.AssemblyQualifiedName.EndsWith("ContentType=WindowsRuntime", StringComparison.Ordinal);
            }

            return false;
        }
    }
}