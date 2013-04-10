using System;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ControlLibrary
{
    /// <summary>
    /// Wraps basic routed events like Loaded and Unloaded in virtual methods and expose common properties like IsLoaded and IsFocused.
    /// </summary>
    public abstract class MatControl : Control
    {
        /// <summary>
        /// Defines the IsFocused property.
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register("IsFocused", typeof(bool), typeof(MatControl), new PropertyMetadata(false, OnIsFocusedChanged));

        internal const char VisualStateDelimiter = ',';
        internal bool changePropertySilently;

        // used to listen for changes in the DataContext property
        private static readonly DependencyProperty InnerDataContextProperty =
            DependencyProperty.Register("InnerDataContext", typeof(object), typeof(MatControl), new PropertyMetadata(null,OnDataContextChanged));

        private bool isFocused;
        private string currentVisualState;
        private bool isLoaded;
        private bool isTemplateApplied;
        private byte visualStateUpdateLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadControl"/> class.
        /// </summary>
        public MatControl()
        {
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;

            this.currentVisualState = string.Empty;

            // add an empty binding to listen to the DataContextChanged event
            this.SetBinding(InnerDataContextProperty, new Binding());
        }

        /// <summary>
        /// Gets the current visual state of the control.
        /// </summary>
        public string CurrentVisualState
        {
            get
            {
                return this.currentVisualState;
            }
        }

        /// <summary>
        /// Determines whether the control is currently loaded.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return this.isLoaded;
            }
        }

        /// <summary>
        /// Determines whether the <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/> method has been called for this instance.
        /// </summary>
        public bool IsTemplateApplied
        {
            get
            {
                return this.isTemplateApplied;
            }
        }

        /// <summary>
        /// Determines whether the control is currently Focused (has the keyboard input).
        /// </summary>
        public bool IsFocused
        {
            get
            {
                return this.isFocused;
            }
            protected set
            {
                this.SetValue(IsFocusedProperty, value);
            }
        }
		
		/// <summary>
		/// Gets a value indicating whether this control is properly templated.
		/// </summary>
		/// <value>
		/// 	<c>True</c> if this instance is properly templated; otherwise, <c>false</c>.
		/// </value>
		protected internal virtual bool IsProperlyTemplated
		{
			get
			{
				return this.isTemplateApplied;
			}
		}

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.isTemplateApplied = true;
        }

        internal void ChangePropertySilently(DependencyProperty property, object value)
        {
            this.changePropertySilently = true;
            this.SetValue(property, value);
            this.changePropertySilently = false;
        }

        /// <summary>
        /// Locks any visual state update. Useful when performing batch operations.
        /// </summary>
        protected internal void BeginVisualStateUpdate()
        {
            this.visualStateUpdateLock++;
        }

        /// <summary>
        /// Resumes visual state update and optionally re-evaluates the current visual state.
        /// </summary>
        /// <param name="update">True to re-evaluate the current visual state, false otherwise.</param>
        /// <param name="animate">True to use animations when updating visual state, false otherwise.</param>
        protected internal void EndVisualStateUpdate(bool update, bool animate)
        {
            if (this.visualStateUpdateLock > 0)
            {
                this.visualStateUpdateLock--;
            }

            if (update && this.visualStateUpdateLock == 0)
            {
                this.UpdateVisualState(animate);
            }
        }

        /// <summary>
        /// Re-evaluates the current visual state for the control and updates it if necessary.
        /// </summary>
        /// <param name="animate">True to use transitions during state update, false otherwise.</param>
        protected internal void UpdateVisualState(bool animate)
        {
            if (!this.CanUpdateVisualState())
            {
                return;
            }

            string state = this.ComposeVisualStateName();
            if (state != this.currentVisualState)
            {
                this.currentVisualState = state;
                this.SetVisualState(state, animate);
            }
        }

        /// <summary>
        /// Applies the specified visual state as current.
        /// </summary>
        /// <param name="state">The new visual state.</param>
        /// <param name="animate">True to use transitions, false otherwise.</param>
        protected virtual void SetVisualState(string state, bool animate)
        {
            string[] states = state.Split(MatControl.VisualStateDelimiter);
            foreach (string visualState in states)
            {
                VisualStateManager.GoToState(this, visualState, animate);
            }
        }

        /// <summary>
        /// Determines whether the current visual state may be updated.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanUpdateVisualState()
        {
            return this.isTemplateApplied && this.visualStateUpdateLock == 0;
        }

        /// <summary>
        /// Occurs when this object is no longer connected to the main object tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = false;
        }

        /// <summary>
        /// Occurs when a System.Windows.FrameworkElement has been constructed and added to the object tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = true;
        }

        /// <summary>
        /// Occurs when the DataContext property has changed.
        /// </summary>
        protected virtual void OnDataContextChanged()
        {

        }

        /// <summary>
        /// Builds the current visual state for this instance.
        /// </summary>
        /// <returns></returns>
        protected virtual string ComposeVisualStateName()
        {
            return string.Empty;
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.GotFocus"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            this.IsFocused = true;
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.LostFocus"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            this.IsFocused = false;
        }

        private static void OnIsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MatControl control = d as MatControl;
            control.isFocused = (bool)e.NewValue;
            control.UpdateVisualState(control.isLoaded);
        }

        private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MatControl).OnDataContextChanged();
        }
    }
}
