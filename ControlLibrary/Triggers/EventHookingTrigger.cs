namespace ControlLibrary.Triggers
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices.WindowsRuntime;

    using Windows.UI.Xaml;

    /// <summary>
    /// A trigger that needs to hook into an even on an associated object, which when fired will
    /// invoke the trigging process.
    /// </summary>
    public abstract class EventHookingTrigger : Trigger
    {
        /// <summary>
        /// A helper class capable of wrapping an arbitrary event handler on a <see cref="FrameworkElement"/>.
        /// </summary>
        /// <remarks>
        /// This is a heavily modified version of some code found in http://www.codeproject.com/Articles/319855/Attached-Command-for-Windows-8-Metro-Style-in-C
        /// Kudos for coming up with the approach in the first place - that's sometimes the hardest part.
        /// </remarks>
        protected sealed class EventHooker
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="EventHooker"/> class. 
            /// Initializes the instance of the class
            /// </summary>
            /// <param name="binding">
            /// The <see cref="EventTrigger"/> that owns this instance.
            /// </param>
            public EventHooker(EventHookingTrigger binding)
            {
                this.Binding = binding;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets the <see cref="EventTrigger"/> that owns this instance
            /// </summary>
            private EventHookingTrigger Binding { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// Adds a hooks into the associated object for the event with the given name.
            /// </summary>
            /// <param name="obj">
            /// The object to attach the event to.
            /// </param>
            /// <param name="eventName">
            /// The name of the event to hook into.
            /// </param>
            public void HookEvent(object obj, string eventName)
            {
                var eventInfo = GetEventInfo(obj.GetType(), eventName);

                if (eventInfo != null)
                {
                    var handler = this.GetEventHandler(eventInfo);

                    WindowsRuntimeMarshal.AddEventHandler(
                        e => (EventRegistrationToken)eventInfo.AddMethod.Invoke(obj, new object[] { e }),
                        e => eventInfo.RemoveMethod.Invoke(obj, new object[] { e }),
                        handler);
                }
            }

            /// <summary>
            /// Removes a hook from the associated object for the event with the given name.
            /// </summary>
            /// <param name="obj">
            /// The object to attach the event to.
            /// </param>
            /// <param name="eventName">
            /// The name of the event to remove the hook for.
            /// </param>
            public void UnhookEvent(object obj, string eventName)
            {
                var eventInfo = GetEventInfo(obj.GetType(), eventName);
                var handler = this.GetEventHandler(eventInfo);

                WindowsRuntimeMarshal.RemoveEventHandler(
                    e => eventInfo.RemoveMethod.Invoke(obj, new object[] { e }),
                    handler);
            }

            #endregion

            #region Methods

            /// <summary>
            /// Gets <see cref="EventInfo"/> information about the specified event name of the provided type.
            /// </summary>
            /// <param name="type">
            /// The type that the event should be found on.
            /// </param>
            /// <param name="eventName">
            /// The name of the event to get the information for.
            /// </param>
            /// <returns>
            /// Instance of <see cref="EventInfo"/> representing the named event.
            /// </returns>
            private static EventInfo GetEventInfo(Type type, string eventName)
            {
                EventInfo eventInfo;

                // GetDeclaredEvent doesn't get event info for events that are declared higher up in the
                // type hierarchy, so keep working up the base types until we find it.
                do
                {
                    var typeInfo = type.GetTypeInfo();
                    eventInfo = typeInfo.GetDeclaredEvent(eventName);
                    type = typeInfo.BaseType;
                }
                while (eventInfo == null && type != null);

                return eventInfo;
            }

            /// <summary>
            /// Create an instance of a delegate able to handle the provided event
            /// </summary>
            /// <param name="eventInfo">
            /// Information about the event to create the delegate
            /// </param>
            /// <returns>
            /// The <see cref="Delegate"/>.
            /// </returns>
            private Delegate GetEventHandler(EventInfo eventInfo)
            {
                if (eventInfo == null)
                {
                    throw new ArgumentNullException("eventInfo");
                }

                if (eventInfo.EventHandlerType == null)
                {
                    throw new ArgumentException("eventInfo.EventHandlerType must be set");
                }

                var eventRaisedMethod = this.GetType().GetTypeInfo().GetDeclaredMethod("OnEventRaised");
                return eventRaisedMethod.CreateDelegate(eventInfo.EventHandlerType, this);
            }

            /// <summary>
            /// This method is hooked, via reflection, to a named event. It is responsible for 
            /// triggering the 
            /// </summary>
            /// <param name="sender">
            /// source of the event
            /// </param>
            /// <param name="e">
            /// arguments of the event
            /// </param>
            // ReSharper disable UnusedMember.Local
            // ReSharper disable UnusedParameter.Local
            private void OnEventRaised(object sender, object e)
                // ReSharper restore UnusedParameter.Local
                // ReSharper restore UnusedMember.Local
            {
                this.Binding.OnTriggered(e);
            }

            #endregion
        }
    }
}