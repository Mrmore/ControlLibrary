using ControlLibrary.Tools.Interactivity;
using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace ControlLibrary.Behaviors
{
  /// <summary>
  /// A behavior to imitate an EventToCommand trigger
  /// </summary>
  public class EventToBoundCommandBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      var evt = AssociatedObject.GetType().GetRuntimeEvent(Event);
      if (evt != null)
      {
        Observable.FromEventPattern<RoutedEventArgs>(AssociatedObject, Event)
          .Subscribe(se => FireCommand(se.EventArgs));
      }
      base.OnAttached();
    }

    private void FireCommand(RoutedEventArgs routedEventArgs)
    {
      if (Command != null && Command.CanExecute(CommandParameter))
      {
          if (PassEventArgsToCommand && CommandParameter == null)
          {
              Command.Execute(routedEventArgs);
          }
          else
          {
              Command.Execute(CommandParameter);
          }
      }
    }

    public bool PassEventArgsToCommand { get; set; }

    #region Event

    /// <summary>
    /// Event Property name
    /// </summary>
    public const string EventPropertyName = "Event";

    public string Event
    {
      get { return (string)GetValue(EventProperty); }
      set { SetValue(EventProperty, value); }
    }

    /// <summary>
    /// Event Property definition
    /// </summary>
    public static readonly DependencyProperty EventProperty = DependencyProperty.Register(
        EventPropertyName,
        typeof(string),
        typeof(EventToBoundCommandBehavior),
        new PropertyMetadata(default(string)));

    #endregion

    #region Command

    /// <summary>
    /// Command Property name
    /// </summary>
    public const string CommandPropertyName = "Command";

    public ICommand Command
    {
      get { return (ICommand)GetValue(CommandProperty); }
      set { SetValue(CommandProperty, value); }
    }

    /// <summary>
    /// Command Property definition
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        CommandPropertyName,
        typeof(ICommand),
        typeof(EventToBoundCommandBehavior),
        new PropertyMetadata(default(object)));

    #endregion

    #region CommandParameter

    /// <summary>
    /// CommandParameter Property name
    /// </summary>
    public const string CommandParameterPropertyName = "CommandParameter";

    public object CommandParameter
    {
      get { return (object)GetValue(CommandParameterProperty); }
      set { SetValue(CommandParameterProperty, value); }
    }

    /// <summary>
    /// CommandParameter Property definition
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        CommandParameterPropertyName,
        typeof(object),
        typeof(EventToBoundCommandBehavior),
        new PropertyMetadata(default(object)));

    #endregion
  }
}
