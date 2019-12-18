using System;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Prism.Windows.Behaviors
{
    /// <summary>
    /// A behavior that listens for a specified event on its source and executes its actions when that event is fired.
    /// </summary>
    [ContentPropertyAttribute(Name = "Actions")]
    internal class EventTriggerBehaviorBase : Behavior
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

        private object _resolvedSource;
        private Delegate _eventHandler;
        private bool _isLoadedEventRegistered;
        private bool _isWindowsRuntimeEvent;
        private Func<Delegate, EventRegistrationToken> _addEventHandlerMethod;
        private Action<EventRegistrationToken> _removeEventHandlerMethod;

        /// <summary>
        /// Resolved source.
        /// </summary>
        protected object ResolvedSource => _resolvedSource;

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
                var actionCollection = (ActionCollection)GetValue(EventTriggerBehaviorBase.ActionsProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    SetValue(EventTriggerBehaviorBase.ActionsProperty, actionCollection);
                }

                return actionCollection;
            }
        }

        /// <summary>
        /// Gets or sets the name of the event to listen for. This is a dependency property.
        /// </summary>
        public string EventName
        {
            get => (string)GetValue(EventTriggerBehaviorBase.EventNameProperty);

            set => SetValue(EventTriggerBehaviorBase.EventNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the source object from which this behavior listens for events.
        /// If <seealso cref="SourceObject"/> is not set, the source will default to <seealso cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>. This is a dependency property.
        /// </summary>
        public object SourceObject
        {
            get => GetValue(EventTriggerBehaviorBase.SourceObjectProperty);

            set => SetValue(EventTriggerBehaviorBase.SourceObjectProperty, value);
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            SetResolvedSource(ComputeResolvedSource());
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Microsoft.Xaml.Interactivity.Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            SetResolvedSource(null);
        }

        private void SetResolvedSource(object newSource)
        {
            if (AssociatedObject == null || _resolvedSource == newSource)
            {
                return;
            }

            if (_resolvedSource != null)
            {
                UnregisterEvent(EventName);
            }

            _resolvedSource = newSource;

            if (_resolvedSource != null)
            {
                RegisterEvent(EventName);
            }
        }

        private object ComputeResolvedSource()
        {
            // If the SourceObject property is set at all, we want to use it. It is possible that it is data
            // bound and bindings haven't been evaluated yet. Plus, this makes the API more predictable.
            if (ReadLocalValue(EventTriggerBehaviorBase.SourceObjectProperty) != DependencyProperty.UnsetValue)
            {
                return SourceObject;
            }

            return AssociatedObject;
        }

        private void RegisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName != "Loaded")
            {
                var sourceObjectType = _resolvedSource.GetType();
                var info = sourceObjectType.GetRuntimeEvent(eventName);
                if (info == null)
                {
                    throw new ArgumentException(string.Format(
                        "Can not find event {0} for the class {1}",
                        EventName,
                        sourceObjectType.Name));
                }

                var methodInfo = typeof(EventTriggerBehaviorBase).GetTypeInfo().GetDeclaredMethod("OnEvent");
                _eventHandler = methodInfo.CreateDelegate(info.EventHandlerType, this);

                _isWindowsRuntimeEvent = EventTriggerBehaviorBase.IsWindowsRuntimeEvent(info);
                if (_isWindowsRuntimeEvent)
                {
                    _addEventHandlerMethod = add => (EventRegistrationToken)info.AddMethod.Invoke(_resolvedSource, new object[] { add });
                    _removeEventHandlerMethod = token => info.RemoveMethod.Invoke(_resolvedSource, new object[] { token });

                    WindowsRuntimeMarshal.AddEventHandler(_addEventHandlerMethod, _removeEventHandlerMethod, _eventHandler);
                }
                else
                {
                    info.AddEventHandler(_resolvedSource, _eventHandler);
                }
            }
            else if (!_isLoadedEventRegistered)
            {
                var element = _resolvedSource as FrameworkElement;
                if (element != null && !EventTriggerBehaviorBase.IsElementLoaded(element))
                {
                    _isLoadedEventRegistered = true;
                    element.Loaded += OnEvent;
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
                if (_eventHandler == null)
                {
                    return;
                }

                var info = _resolvedSource.GetType().GetRuntimeEvent(eventName);
                if (_isWindowsRuntimeEvent)
                {
                    WindowsRuntimeMarshal.RemoveEventHandler(_removeEventHandlerMethod, _eventHandler);
                }
                else
                {
                    info.RemoveEventHandler(_resolvedSource, _eventHandler);
                }

                _eventHandler = null;
            }
            else if (_isLoadedEventRegistered)
            {
                _isLoadedEventRegistered = false;
                var element = (FrameworkElement)_resolvedSource;
                element.Loaded -= OnEvent;
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
            Interaction.ExecuteActions(_resolvedSource, Actions, eventArgs);
        }

        private static void OnSourceObjectChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (EventTriggerBehaviorBase)dependencyObject;
            behavior.SetResolvedSource(behavior.ComputeResolvedSource());
        }

        private static void OnEventNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (EventTriggerBehaviorBase)dependencyObject;
            if (behavior.AssociatedObject == null || behavior._resolvedSource == null)
            {
                return;
            }

            var oldEventName = (string)args.OldValue;
            var newEventName = (string)args.NewValue;

            behavior.UnregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }

        internal static bool IsElementLoaded(FrameworkElement element)
        {
            if (element == null)
            {
                return false;
            }

            var rootVisual = Window.Current.Content;
            var parent = element.Parent;
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