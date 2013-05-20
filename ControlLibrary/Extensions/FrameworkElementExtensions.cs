using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary.Extensions
{
    /// <summary>
    /// Extensions for the FrameworkElement class.
    /// </summary>
    public static class FrameworkElementExtensions
    {
        #region ClipToBounds
        /// <summary>
        /// ClipToBounds Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ClipToBoundsProperty =
            DependencyProperty.RegisterAttached(
                "ClipToBounds",
                typeof(bool),
                typeof(FrameworkElementExtensions),
                new PropertyMetadata(false, OnClipToBoundsChanged));

        /// <summary>
        /// Gets the ClipToBounds property. This dependency property 
        /// indicates whether the element should be clipped to its bounds.
        /// </summary>
        public static bool GetClipToBounds(DependencyObject d)
        {
            return (bool)d.GetValue(ClipToBoundsProperty);
        }

        /// <summary>
        /// Sets the ClipToBounds property. This dependency property 
        /// indicates whether the element should be clipped to its bounds.
        /// </summary>
        public static void SetClipToBounds(DependencyObject d, bool value)
        {
            d.SetValue(ClipToBoundsProperty, value);
        }

        /// <summary>
        /// Handles changes to the ClipToBounds property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnClipToBoundsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldClipToBounds = (bool)e.OldValue;
            bool newClipToBounds = (bool)d.GetValue(ClipToBoundsProperty);

            if (newClipToBounds)
                SetClipToBoundsHandler(d, new ClipToBoundsHandler());
            else
                SetClipToBoundsHandler(d, null);
        }
        #endregion

        #region ClipToBoundsHandler
        /// <summary>
        /// ClipToBoundsHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ClipToBoundsHandlerProperty =
            DependencyProperty.RegisterAttached(
                "ClipToBoundsHandler",
                typeof(ClipToBoundsHandler),
                typeof(FrameworkElementExtensions),
                new PropertyMetadata(null, OnClipToBoundsHandlerChanged));

        /// <summary>
        /// Gets the ClipToBoundsHandler property. This dependency property 
        /// indicates the handler that handles the updates to the clipping geometry when ClipToBounds is set to true.
        /// </summary>
        public static ClipToBoundsHandler GetClipToBoundsHandler(DependencyObject d)
        {
            return (ClipToBoundsHandler)d.GetValue(ClipToBoundsHandlerProperty);
        }

        /// <summary>
        /// Sets the ClipToBoundsHandler property. This dependency property 
        /// indicates the handler that handles the updates to the clipping geometry when ClipToBounds is set to true.
        /// </summary>
        public static void SetClipToBoundsHandler(DependencyObject d, ClipToBoundsHandler value)
        {
            d.SetValue(ClipToBoundsHandlerProperty, value);
        }

        /// <summary>
        /// Handles changes to the ClipToBoundsHandler property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnClipToBoundsHandlerChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ClipToBoundsHandler oldClipToBoundsHandler = (ClipToBoundsHandler)e.OldValue;
            ClipToBoundsHandler newClipToBoundsHandler = (ClipToBoundsHandler)d.GetValue(ClipToBoundsHandlerProperty);

            if (oldClipToBoundsHandler != null)
                oldClipToBoundsHandler.Detach();
            if (newClipToBoundsHandler != null)
                newClipToBoundsHandler.Attach((FrameworkElement)d);
        }
        #endregion

        #region Cursor
        /// <summary>
        /// Cursor Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CursorProperty =
            DependencyProperty.RegisterAttached(
                "Cursor",
                typeof(CoreCursor),
                typeof(FrameworkElementExtensions),
                new PropertyMetadata(null, OnCursorChanged));

        /// <summary>
        /// Gets the Cursor property. This dependency property 
        /// indicates the cursor to use when a mouse cursor is moved over the control.
        /// </summary>
        public static CoreCursor GetCursor(DependencyObject d)
        {
            return (CoreCursor)d.GetValue(CursorProperty);
        }

        /// <summary>
        /// Sets the Cursor property. This dependency property 
        /// indicates the cursor to use when a mouse cursor is moved over the control.
        /// </summary>
        public static void SetCursor(DependencyObject d, CoreCursor value)
        {
            d.SetValue(CursorProperty, value);
        }

        /// <summary>
        /// Handles changes to the Cursor property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCursorChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoreCursor oldCursor = (CoreCursor)e.OldValue;
            CoreCursor newCursor = (CoreCursor)d.GetValue(CursorProperty);

            if (oldCursor == null)
            {
                var handler = new CursorDisplayHandler();
                handler.Attach((FrameworkElement)d);
                SetCursorDisplayHandler(d, handler);
            }
            else
            {
                var handler = GetCursorDisplayHandler(d);

                if (newCursor == null)
                {
                    handler.Detach();
                    SetCursorDisplayHandler(d, null);
                }
                else
                {
                    handler.UpdateCursor();
                }
            }
        }
        #endregion

        #region SystemCursor
        /// <summary>
        /// SystemCursor Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SystemCursorProperty =
            DependencyProperty.RegisterAttached(
                "SystemCursor",
                typeof(CoreCursorType),
                typeof(FrameworkElementExtensions),
                new PropertyMetadata(CoreCursorType.Arrow, OnSystemCursorChanged));

        /// <summary>
        /// Gets the SystemCursor property. This dependency property 
        /// indicates the system CoreCursorType to use for the control's mouse cursor.
        /// </summary>
        public static CoreCursorType GetSystemCursor(DependencyObject d)
        {
            return (CoreCursorType)d.GetValue(SystemCursorProperty);
        }

        /// <summary>
        /// Sets the SystemCursor property. This dependency property 
        /// indicates the system CoreCursorType to use for the control's mouse cursor.
        /// </summary>
        public static void SetSystemCursor(DependencyObject d, CoreCursorType value)
        {
            d.SetValue(SystemCursorProperty, value);
        }

        /// <summary>
        /// Handles changes to the SystemCursor property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnSystemCursorChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoreCursorType oldSystemCursor = (CoreCursorType)e.OldValue;
            CoreCursorType newSystemCursor = (CoreCursorType)d.GetValue(SystemCursorProperty);

            //if (newSystemCursor.HasValue)
            {
                SetCursor(d, new CoreCursor(newSystemCursor, 1));
            }
            //else
            //{
            //    SetCursor(d, null);
            //}
        }
        #endregion

        #region CursorDisplayHandler
        /// <summary>
        /// CursorDisplayHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty CursorDisplayHandlerProperty =
            DependencyProperty.RegisterAttached(
                "CursorDisplayHandler",
                typeof(CursorDisplayHandler),
                typeof(FrameworkElementExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the CursorDisplayHandler property. This dependency property 
        /// indicates the handler for displaying the Cursor when a mouse is moved over the control.
        /// </summary>
        public static CursorDisplayHandler GetCursorDisplayHandler(DependencyObject d)
        {
            return (CursorDisplayHandler)d.GetValue(CursorDisplayHandlerProperty);
        }

        /// <summary>
        /// Sets the CursorDisplayHandler property. This dependency property 
        /// indicates the handler for displaying the Cursor when a mouse is moved over the control.
        /// </summary>
        public static void SetCursorDisplayHandler(DependencyObject d, CursorDisplayHandler value)
        {
            d.SetValue(CursorDisplayHandlerProperty, value);
        }
        #endregion


        #region Class to help animations from code using the CompositeTransform
        /// <summary>
        /// Finds the composite transform either direct
        /// or as part of a TransformGroup
        /// </summary>
        /// <param name="fe">FrameworkElement</param>
        /// <returns></returns>
        public static CompositeTransform GetCompositeTransform(this FrameworkElement fe)
        {
            if (fe.RenderTransform != null)
            {
                var tt = fe.RenderTransform as CompositeTransform;
                if (tt != null) return tt;

                var tg = fe.RenderTransform as TransformGroup;
                if (tg != null)
                {
                    return tg.Children.OfType<CompositeTransform>().FirstOrDefault();
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the point to where FrameworkElement is translated
        /// </summary>
        /// <param name="fe">FrameworkElement</param>
        /// <returns>Translation point</returns>
        public static Point GetTranslatePoint(this FrameworkElement fe)
        {
            var translate = fe.GetCompositeTransform();

            if (translate == null) throw new ArgumentNullException("CompositeTransform");

            return new Point(
                (double)translate.GetValue(CompositeTransform.TranslateXProperty),
                (double)translate.GetValue(CompositeTransform.TranslateYProperty));

        }

        /// <summary>
        /// Translates a FrameworkElement to a new location
        /// </summary>
        /// <param name="fe">FrameworkElement</param>
        /// <param name="p">the new location</param>
        public static void SetTranslatePoint(this FrameworkElement fe, Point p)
        {
            var translate = fe.GetCompositeTransform();
            if (translate == null) throw new ArgumentNullException("CompositeTransform");

            translate.SetValue(CompositeTransform.TranslateXProperty, p.X);
            translate.SetValue(CompositeTransform.TranslateYProperty, p.Y);
        }

        /// <summary>
        /// Translates a FrameworkElement to a new location
        /// </summary>
        /// <param name="fe">FrameworkElement</param>
        /// <param name="x">X coordinate of the new location</param>
        /// <param name="y">Ycoordinate of the new location</param>
        public static void SetTranslatePoint(this FrameworkElement fe, double x, double y)
        {
            fe.SetTranslatePoint(new Point(x, y));
        }

        public static FrameworkElement GetElementToAnimate(this FrameworkElement fe)
        {
            var parent = fe.GetVisualParent();
            return parent is ContentPresenter ? parent : fe;
        }

        public static bool IsPortrait(this FrameworkElement elem)
        {
            return elem.ActualHeight > elem.ActualWidth;
        }
        #endregion
    }

    public class ClipToBoundsHandler
    {
        private FrameworkElement _fe;

        public void Attach(FrameworkElement fe)
        {
            _fe = fe;
            UpdateClipGeometry();
            fe.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (_fe == null)
                return;

            UpdateClipGeometry();
        }

        private void UpdateClipGeometry()
        {
            _fe.Clip =
                new RectangleGeometry
                {
                    Rect = new Rect(0, 0, _fe.ActualWidth, _fe.ActualHeight)
                };
        }

        public void Detach()
        {
            if (_fe == null)
                return;

            _fe.SizeChanged -= OnSizeChanged;
            _fe = null;
        }
    }

    public class CursorDisplayHandler
    {
        private FrameworkElement _control;
        private bool _isHovering;

        #region DefaultCursor
        private static CoreCursor _defaultCursor;
        private static CoreCursor DefaultCursor
        {
            get
            {
                return _defaultCursor ?? (_defaultCursor = Window.Current.CoreWindow.PointerCursor);
            }
        }
        #endregion

        public void Attach(FrameworkElement c)
        {
            _control = c;
            _control.PointerEntered += OnPointerEntered;
            _control.PointerExited += OnPointerExited;
            _control.Unloaded += OnControlUnloaded;
        }

        private void OnControlUnloaded(object sender, RoutedEventArgs e)
        {
            Detach();
        }

        public void Detach()
        {
            _control.PointerEntered -= OnPointerEntered;
            _control.PointerExited -= OnPointerExited;
            _control.Unloaded -= OnControlUnloaded;

            if (_isHovering)
            {
                Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            }
        }

        private void OnPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                _isHovering = true;
                UpdateCursor();
            }
        }

        private void OnPointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                _isHovering = false;
                Window.Current.CoreWindow.PointerCursor = DefaultCursor;
            }
        }

        internal void UpdateCursor()
        {
            if (_defaultCursor == null)
            {
                _defaultCursor = Window.Current.CoreWindow.PointerCursor;
            }

            var cursor = FrameworkElementExtensions.GetCursor(_control);

            if (_isHovering)
            {
                if (cursor != null)
                {
                    Window.Current.CoreWindow.PointerCursor = cursor;
                }
                else
                {
                    Window.Current.CoreWindow.PointerCursor = DefaultCursor;
                }
            }
        }
    }
}
