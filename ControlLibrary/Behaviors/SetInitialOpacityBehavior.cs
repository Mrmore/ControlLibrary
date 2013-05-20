using ControlLibrary.Tools.Interactivity;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml;

namespace ControlLibrary.Behaviors
{
  public class SetInitialOpacityBehavior : Behavior<FrameworkElement>
  {
    protected override void OnAttached()
    {
      AssociatedObject.Opacity = InitialOpacity;
      AssociatedObject.Loaded += AssociatedObjectLoaded;
      base.OnAttached();
    }

    private async void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
    {
      AssociatedObject.Loaded -= AssociatedObjectLoaded;
      await Task.Delay(GetDelayTime(this));
      AssociatedObject.Opacity = OpacityAfterLoading;
    }

    #region InitialOpacity
    public const string InitialOpacityPropertyName = "InitialOpacity";

    public int InitialOpacity
    {
      get { return (int)GetValue(InitialOpacityProperty); }
      set { SetValue(InitialOpacityProperty, value); }
    }

    public static readonly DependencyProperty InitialOpacityProperty =
        DependencyProperty.Register(
        InitialOpacityPropertyName,
        typeof(int),
        typeof(SetInitialOpacityBehavior),
        new PropertyMetadata(0));
    #endregion

    #region OpacityAfterLoading
    public const string OpacityAfterLoadingPropertyName = "OpacityAfterLoading";

    public int OpacityAfterLoading
    {
      get { return (int)GetValue(OpacityAfterLoadingProperty); }
      set { SetValue(OpacityAfterLoadingProperty, value); }
    }

    public static readonly DependencyProperty OpacityAfterLoadingProperty =
      DependencyProperty.Register(
      OpacityAfterLoadingPropertyName,
      typeof(int),
      typeof(SetInitialOpacityBehavior),
      new PropertyMetadata(1));

    #endregion

    #region Attached Dependency Property DelayTime
    public static readonly DependencyProperty DelayTimeProperty =
         DependencyProperty.RegisterAttached("DelayTime",
         typeof(int),
         typeof(SetInitialOpacityBehavior),
         new PropertyMetadata(default(int)));

    // Called when Property is retrieved
    public static int GetDelayTime(DependencyObject obj)
    {
      return (int)obj.GetValue(DelayTimeProperty);
    }

    // Called when Property is set
    public static void SetDelayTime(
       DependencyObject obj,
       int value)
    {
      obj.SetValue(DelayTimeProperty, value);
    }
    #endregion
  }

}
