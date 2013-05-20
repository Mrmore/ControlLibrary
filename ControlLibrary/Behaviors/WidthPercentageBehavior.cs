using ControlLibrary.Tools.Interactivity;
using System;
using Windows.UI.Xaml;
using ControlLibrary.Extensions;

namespace ControlLibrary.Behaviors
{
  public class WidthPercentageBehavior : Behavior<FrameworkElement>
  {
    private FrameworkElement visualParent;

    protected override void OnAttached()
    {
      AssociatedObject.Loaded += AssociatedObject_Loaded;

      base.OnAttached();
    }

    void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
      visualParent = AssociatedObject.GetVisualParent();
      if (visualParent != null)
      {
        visualParent.SizeChanged += VisualParentSizeChanged;
      }
    }

    private void VisualParentSizeChanged(object sender, SizeChangedEventArgs e)
    {
      ApplyNewWidthPercentage(WidthPercentage);
    }

    private void ApplyNewWidthPercentage(int newPercentage)
    {
      if (visualParent != null)
      {
        AssociatedObject.Width = (double) newPercentage/100*visualParent.ActualWidth;
      }
    }

    #region WidthPercentage

    /// <summary>
    /// WidthPercentage Property name
    /// </summary>
    public const string WidthPercentagePropertyName = "WidthPercentage";

    public int WidthPercentage
    {
      get { return (int)GetValue(WidthPercentageProperty); }
      set { SetValue(WidthPercentageProperty, value); }
    }

    /// <summary>
    /// WidthPercentage Property definition
    /// </summary>
    public static readonly DependencyProperty WidthPercentageProperty = DependencyProperty.Register(
        WidthPercentagePropertyName,
        typeof(int),
        typeof(WidthPercentageBehavior),
        new PropertyMetadata(100, WidthPercentageChanged));

    /// <summary>
    /// WidthPercentage property changed callback.
    /// </summary>
    /// <param name="d">The depency object (i.e. the behavior).</param>
    /// <param name="e">The property event args <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>  
    public static void WidthPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var behavior = d as WidthPercentageBehavior;
      var newValue = (int)e.NewValue;
      if (behavior != null)
      {
        behavior.ApplyNewWidthPercentage(newValue);
      }
    }

    #endregion
  }
}
