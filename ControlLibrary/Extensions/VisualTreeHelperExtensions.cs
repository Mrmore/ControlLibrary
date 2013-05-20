using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary.Extensions
{
    /// <summary>
    /// Extension methods for DependencyObjects
    /// used for walking the visual tree with
    /// LINQ expressions.
    /// These simplify using VisualTreeHelper to one line calls.
    /// </summary>
    public static class VisualTreeHelperExtensions
    {
        /// <summary>
        /// Gets the first descendant that is of the given type.
        /// </summary>
        /// <remarks>
        /// Returns null if not found.
        /// </remarks>
        /// <typeparam name="T">Type of descendant to look for.</typeparam>
        /// <param name="start">The start object.</param>
        /// <returns></returns>
        public static T GetFirstDescendantOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendantsOfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the descendants of the given type.
        /// </summary>
        /// <typeparam name="T">Type of descendants to return.</typeparam>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendants().OfType<T>();
        }

        /// <summary>
        /// Gets the descendants.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
        {
            var queue = new Queue<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(start);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(start, i);
                yield return child;
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();
                var count2 = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count2; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// Gets the child elements.
        /// </summary>
        /// <param name="parent">The parent element.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject parent)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;
            }
        }

        /// <summary>
        /// Gets the child elements sorted in render order (by ZIndex first, declaration order second).
        /// </summary>
        /// <param name="parent">The parent element.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetChildrenByZIndex(
            this DependencyObject parent)
        {
            int i = 0;
            var indexedChildren =
                parent.GetChildren().Cast<FrameworkElement>().Select(
                child => new { Index = i++, ZIndex = Canvas.GetZIndex(child), Child = child });

            return
                from indexedChild in indexedChildren
                orderby indexedChild.ZIndex, indexedChild.Index
                select indexedChild.Child;
        }

        /// <summary>
        /// Gets the first ancestor that is of the given type.
        /// </summary>
        /// <remarks>
        /// Returns null if not found.
        /// </remarks>
        /// <typeparam name="T">Type of ancestor to look for.</typeparam>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static T GetFirstAncestorOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetAncestorsOfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the the ancestors of a given type.
        /// </summary>
        /// <typeparam name="T">Type of ancestor to look for.</typeparam>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetAncestors().OfType<T>();
        }

        /// <summary>
        /// Gets the ancestors.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject start)
        {
            var parent = VisualTreeHelper.GetParent(start);

            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        /// <summary>
        /// Determines whether the specified DependencyObject is in visual tree.
        /// </summary>
        /// <remarks>
        /// Note that this might not work as expected if the object is in a popup.
        /// </remarks>
        /// <param name="dob">The DependencyObject.</param>
        /// <returns>
        ///   <c>true</c> if the specified dob is in visual tree ; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInVisualTree(this DependencyObject dob)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return false;
            }

            //TODO: consider making it work with Popups too.
            if (Window.Current == null)
            {
                // This may happen when a picker or CameraCaptureUI etc. is open.
                return false;
            }

            return Window.Current.Content != null && dob.GetAncestors().Contains(Window.Current.Content);
        }

        /// <summary>
        /// Gets the bounding rectangle of a given element
        /// relative to a given other element or visual root
        /// if relativeTo is null or not specified.
        /// </summary>
        /// <param name="dob">The starting element.</param>
        /// <param name="relativeTo">The relative to element.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Element not in visual tree.</exception>
        public static Rect GetBoundingRect(this FrameworkElement dob, FrameworkElement relativeTo = null)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return Rect.Empty;
            }

            if (relativeTo == null)
            {
                relativeTo = Window.Current.Content as FrameworkElement;
            }

            if (relativeTo == null)
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            if (dob == relativeTo)
            {
                return new Rect(0, 0, relativeTo.ActualWidth, relativeTo.ActualHeight);
            }

            var ancestors = dob.GetAncestors().ToArray();

            if (!ancestors.Contains(relativeTo))
            {
                throw new InvalidOperationException("Element not in visual tree.");
            }

            var pos =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(new Point());
            var pos2 =
                dob
                    .TransformToVisual(relativeTo)
                    .TransformPoint(
                        new Point(
                            dob.ActualWidth,
                            dob.ActualHeight));

            return new Rect(pos, pos2);
        }

        /// <summary>
        /// Returns a render transform of the specified type from the element, creating it if necessary
        /// </summary>
        /// <typeparam name="TRequestedTransform">The type of transform (Rotate, Translate, etc)</typeparam>
        /// <param name="element">The element to check</param>
        /// <param name="mode">The mode to use for creating transforms, if not found</param>
        /// <returns>The specified transform, or null if not found and not created</returns>
        public static TRequestedTransform GetTransform<TRequestedTransform>(this UIElement element, TransformCreationMode mode) where TRequestedTransform : Transform, new()
        {
            Transform originalTransform = element.RenderTransform;
            TRequestedTransform requestedTransform = null;
            MatrixTransform matrixTransform = null;
            TransformGroup transformGroup = null;

            // Current transform is null -- create if necessary and return
            if (originalTransform == null)
            {
                if ((mode & TransformCreationMode.Create) == TransformCreationMode.Create)
                {
                    requestedTransform = new TRequestedTransform();
                    element.RenderTransform = requestedTransform;
                    return requestedTransform;
                }

                return null;
            }

            // Transform is exactly what we want -- return it
            requestedTransform = originalTransform as TRequestedTransform;
            if (requestedTransform != null)
                return requestedTransform;


            // The existing transform is matrix transform - overwrite if necessary and return
            matrixTransform = originalTransform as MatrixTransform;
            if (matrixTransform != null)
            {
                if (matrixTransform.Matrix.IsIdentity
                  && (mode & TransformCreationMode.Create) == TransformCreationMode.Create
                  && (mode & TransformCreationMode.IgnoreIdentityMatrix) == TransformCreationMode.IgnoreIdentityMatrix)
                {
                    requestedTransform = new TRequestedTransform();
                    element.RenderTransform = requestedTransform;
                    return requestedTransform;
                }

                return null;
            }

            // Transform is actually a group -- check for the requested type
            transformGroup = originalTransform as TransformGroup;
            if (transformGroup != null)
            {
                foreach (Transform child in transformGroup.Children)
                {
                    // Child is the right type -- return it
                    if (child is TRequestedTransform)
                        return child as TRequestedTransform;
                }

                // Right type was not found, but we are OK to add it
                if ((mode & TransformCreationMode.AddToGroup) == TransformCreationMode.AddToGroup)
                {
                    requestedTransform = new TRequestedTransform();
                    transformGroup.Children.Add(requestedTransform);
                    return requestedTransform;
                }

                return null;
            }

            // Current ransform is not a group and is not what we want;
            // create a new group containing the existing transform and the new one
            if ((mode & TransformCreationMode.CombineIntoGroup) == TransformCreationMode.CombineIntoGroup)
            {
                transformGroup = new TransformGroup();
                transformGroup.Children.Add(originalTransform);
                transformGroup.Children.Add(requestedTransform);
                element.RenderTransform = transformGroup;
                return requestedTransform;
            }

            Debug.Assert(false, "Shouldn't get here");
            return null;
        }

        /// <summary>
        /// Returns a string representation of a property path needed to update a Storyboard
        /// </summary>
        /// <param name="element">The element to get the path for</param>
        /// <param name="subProperty">The property of the transform</param>
        /// <typeparam name="TRequestedType">The type of transform to look fo</typeparam>
        /// <returns>A property path</returns>
        public static string GetTransformPropertyPath<TRequestedType>(this FrameworkElement element, string subProperty) where TRequestedType : Transform
        {
            Transform t = element.RenderTransform;
            if (t is TRequestedType)
                return String.Format("(RenderTransform).({0}.{1})", typeof(TRequestedType).Name, subProperty);

            else if (t is TransformGroup)
            {
                TransformGroup g = t as TransformGroup;
                for (int i = 0; i < g.Children.Count; i++)
                {
                    if (g.Children[i] is TRequestedType)
                        return String.Format("(RenderTransform).(TransformGroup.Children)[" + i + "].({0}.{1})",
                          typeof(TRequestedType).Name, subProperty);
                }
            }

            return "";
        }

        /// <summary>
        /// Returns a plane projection, creating it if necessary
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="create">Whether or not to create the projection if it doesn't already exist</param>
        /// <returns>The plane project, or null if not found / created</returns>
        public static PlaneProjection GetPlaneProjection(this UIElement element, bool create)
        {
            Projection originalProjection = element.Projection;
            PlaneProjection projection = null;

            // Projection is already a plane projection; return it
            if (originalProjection is PlaneProjection)
                return originalProjection as PlaneProjection;

            // Projection is null; create it if necessary
            if (originalProjection == null)
            {
                if (create)
                {
                    projection = new PlaneProjection();
                    element.Projection = projection;
                }
            }

            // Note that if the project is a Matrix projection, it will not be
            // changed and null will be returned.
            return projection;
        }
    }

    /// <summary>
    /// Possible modes for creating a transform
    /// </summary>
    [Flags]
    public enum TransformCreationMode
    {
        /// <summary>
        /// Don't try and create a transform if it doesn't already exist
        /// </summary>
        None = 0,

        /// <summary>
        /// Create a transform if none exists
        /// </summary>
        Create = 1,

        /// <summary>
        /// Create and add to an existing group
        /// </summary>
        AddToGroup = 2,

        /// <summary>
        /// Create a group and combine with existing transform; may break existing animations
        /// </summary>
        CombineIntoGroup = 4,

        /// <summary>
        /// Treat identity matrix as if it wasn't there; may break existing animations
        /// </summary>
        IgnoreIdentityMatrix = 8,

        /// <summary>
        /// Create a new transform or add to group
        /// </summary>
        CreateOrAddAndIgnoreMatrix = Create | AddToGroup | IgnoreIdentityMatrix,

        /// <summary>
        /// Default behaviour, equivalent to CreateOrAddAndIgnoreMatrix
        /// </summary>
        Default = CreateOrAddAndIgnoreMatrix,
    }
}
