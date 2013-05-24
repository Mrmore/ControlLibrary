using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary.Extensions
{
    public static class ExtensionMethods
    {
        public static FrameworkElement FindVisualChild(this FrameworkElement root, string name)
        {
            if (root != null)
            {
                FrameworkElement temp = root.FindName(name) as FrameworkElement;
                if (temp != null)
                    return temp;

                foreach (FrameworkElement element in root.GetVisualDescendents())
                {
                    temp = element.FindName(name) as FrameworkElement;
                    if (temp != null)
                        return temp;
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the visual parent of the element
        /// </summary>
        /// <param name="node">The element to check</param>
        /// <returns>The visual parent</returns>
        public static FrameworkElement GetVisualParent(this FrameworkElement node)
        {
            return VisualTreeHelper.GetParent(node) as FrameworkElement;
        }

        public static IEnumerable<FrameworkElement> GetVisualDescendents(this FrameworkElement root)
        {
            Queue<IEnumerable<FrameworkElement>> toDo = new Queue<IEnumerable<FrameworkElement>>();

            toDo.Enqueue(root.GetVisualChildren());
            while (toDo.Count > 0)
            {
                IEnumerable<FrameworkElement> children = toDo.Dequeue();
                foreach (FrameworkElement child in children)
                {
                    yield return child;
                    toDo.Enqueue(child.GetVisualChildren());
                }
            }
        }

        public static IEnumerable<FrameworkElement> GetVisualChildren(this FrameworkElement root)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                yield return VisualTreeHelper.GetChild(root, i) as FrameworkElement;
        }

        /// <summary>
        /// Gets a visual child of the element
        /// </summary>
        /// <param name="node">The element to check</param>
        /// <param name="index">The index of the child</param>
        /// <returns>The found child</returns>
        public static FrameworkElement GetVisualChild(this FrameworkElement node, int index)
        {
            return VisualTreeHelper.GetChild(node, index) as FrameworkElement;
        }

        /// <summary>
        /// Gets the ancestors of the element, up to the root
        /// </summary>
        /// <param name="node">The element to start from</param>
        /// <returns>An enumerator of the ancestors</returns>
        public static IEnumerable<FrameworkElement> GetVisualAncestors(this FrameworkElement node)
        {
            FrameworkElement parent = node.GetVisualParent();
            while (parent != null)
            {
                yield return parent;
                parent = parent.GetVisualParent();
            }
        }

        /// <summary>
        /// Gets the VisualStateGroup with the given name, looking up the visual tree
        /// </summary>
        /// <param name="root">Element to start from</param>
        /// <param name="groupName">Name of the group to look for</param>
        /// <param name="searchAncestors">Whether or not to look up the tree</param>
        /// <returns>The group, if found</returns>
        public static VisualStateGroup GetVisualStateGroup(this FrameworkElement root, string groupName, bool searchAncestors)
        {
            // Changed from IList to var - LocalJoost
            var groups = VisualStateManager.GetVisualStateGroups(root);
            foreach (object o in groups)
            {
                VisualStateGroup group = o as VisualStateGroup;
                if (group != null && group.Name == groupName)
                    return group;
            }

            if (searchAncestors)
            {
                FrameworkElement parent = root.GetVisualParent();
                if (parent != null)
                    return parent.GetVisualStateGroup(groupName, true);
            }

            return null;
        }

        /// <summary>
        /// Finds the VisualStateGroup with the given name
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static VisualStateGroup FindVisualState(this FrameworkElement root, string name)
        {
            if (root == null)
                return null;

            // Changed from IList to var - LocalJoost
            var groups = VisualStateManager.GetVisualStateGroups(root);
            return groups.Cast<VisualStateGroup>().FirstOrDefault(group => group.Name == name);
        }

        /// <summary>
        /// Finding the ScrollViewer or ScrollBar (All Con)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root"></param>
        /// <returns></returns>
        public static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        public static T FindFirstElementInVisualTree<T>(DependencyObject parentElement) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parentElement);
            if (count == 0)
                return null;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parentElement, i);

                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var result = FindFirstElementInVisualTree<T>(child);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static Task RunAsync(this Storyboard storyboard)
        {
            TaskCompletionSource<Object> tcTaskCompletionSource = new TaskCompletionSource<object>();
            EventHandler<Object> eventHandler = null;
            eventHandler = (sender, o) =>
            {
                storyboard.Completed -= eventHandler;
                tcTaskCompletionSource.TrySetResult(null);
            };
            storyboard.Completed += eventHandler;
            storyboard.Begin();
            return tcTaskCompletionSource.Task;
        }

        /// <summary>
        /// Subsribe to a Dependency Property Changed Event
        /// </summary>
        public static void SubscribePropertyChanged(this FrameworkElement element, string property, PropertyChangedCallback propertyChangedCallback)
        {
            Binding b = new Binding { Path = new PropertyPath(property), Source = element };
            var prop = DependencyProperty.RegisterAttached(property, typeof(object), typeof(Control), new PropertyMetadata(null, propertyChangedCallback));
            element.SetBinding(prop, b);
        }

        internal static UIElement GetClosest(FrameworkElement itemsControl, IEnumerable<DependencyObject> items,
                                    Point position, Orientation searchDirection, UIElement selectedItem, bool searchDown = true)
        {
            UIElement closest = null;
            double closestDistance = Double.MaxValue;

            var arrayItems = items as DependencyObject[] ?? items.ToArray();

            for (int cpt = 0; cpt < arrayItems.Count(); cpt++)
            {
                UIElement uiElement = arrayItems[cpt] as UIElement;

                if (uiElement == null) continue;

                Rect rect2 = new Rect();
                Rect rect = uiElement.TransformToVisual(itemsControl).TransformBounds(rect2);

                if (position.Y <= rect.Y + uiElement.RenderSize.Height && position.Y >= rect.Y)
                    return uiElement;
            }

            // this code is not used. Just for reminder :)
            if (searchDown)
            {

                for (int cpt = 0; cpt < arrayItems.Count(); cpt++)
                {
                    UIElement uiElement = arrayItems[cpt] as UIElement;

                    if (uiElement != null)
                    {
                        //if (selectedItem != null &&  uiElement == selectedItem)
                        //    return uiElement;

                        Point p = uiElement.TransformToVisual(itemsControl).TransformPoint(new Point(0, 0));

                        Rect rect = uiElement.TransformToVisual(itemsControl).TransformBounds(new Rect());


                        // Get Distance. Must be neagative
                        double distance = GetDistance(itemsControl, position, p, searchDirection);

                        // if distance is positive, and it's the first item, must select it (we are at the top
                        if (distance > 0 && cpt == 0)
                            return uiElement;

                        if (distance > 0)
                            break;

                        // Get absolute
                        distance = Math.Abs(distance);

                        if (distance > closestDistance) continue;

                        // If the closest item is the item itself (and it's not the first one !)
                        // I can break
                        if (uiElement == selectedItem && cpt > 0) break;

                        closest = uiElement;
                        closestDistance = distance;
                    }

                    // if UIElement is null then i scroll uo but im at the first item which is above, so i select it
                    if (closest == null)
                        closest = arrayItems[0] as UIElement;

                }
            }
            else
            {

                for (int cpt = arrayItems.Count() - 1; cpt >= 0; cpt--)
                {
                    UIElement uiElement = arrayItems[cpt] as UIElement;


                    if (uiElement != null)
                    {
                        //if (selectedItem != null && uiElement == selectedItem)
                        //    return uiElement;

                        Point p = uiElement.TransformToVisual(itemsControl).TransformPoint(new Point(0, 0));

                        double distance = GetDistance(itemsControl, position, p, searchDirection);

                        if (distance < 0) continue;
                        if (!(distance <= closestDistance)) continue;

                        closest = uiElement;
                        closestDistance = distance;
                    }

                }

                // if UIElement is null then i scroll down but im at the last item which is above, so i select it
                if (closest == null)
                    closest = arrayItems[arrayItems.Count() - 1] as UIElement;
            }

            return closest;
        }

        private static double GetDistance(FrameworkElement itemsControl, Point position1, Point position2, Orientation searchDirection)
        {
            // Point p = uiElement.TransformToVisual(itemsControl).TransformPoint(new Point(0, 0));
            double distance = Double.MaxValue;

            switch (searchDirection)
            {
                case Orientation.Horizontal:
                    distance = position2.X - position1.X;
                    break;
                case Orientation.Vertical:
                    distance = position2.Y - position1.Y;
                    break;
            }
            return distance;
        }

        internal static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            // Search immediate children first (breadth-first)   
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;

                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        internal static T GetVisualAncestor<T>(this DependencyObject d) where T : class
        {
            DependencyObject item = VisualTreeHelper.GetParent(d);

            while (item != null)
            {
                T itemAsT = item as T;
                if (itemAsT != null) return itemAsT;
                item = VisualTreeHelper.GetParent(item);
            }

            return null;
        }

        public static DependencyObject GetVisualAncestor(this DependencyObject d, Type type)
        {
            DependencyObject item = VisualTreeHelper.GetParent(d);

            while (item != null)
            {
                if (item.GetType() == type)
                    return item;
                item = VisualTreeHelper.GetParent(item);
            }

            return null;
        }

        internal static T GetVisualDescendent<T>(this DependencyObject d) where T : DependencyObject
        {
            return d.GetVisualDescendents<T>().FirstOrDefault();
        }

        internal static IEnumerable<T> GetVisualDescendents<T>(this DependencyObject d) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(d);

            for (int n = 0; n < childCount; n++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(d, n);

                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (T match in GetVisualDescendents<T>(child))
                {
                    yield return match;
                }
            }

            yield break;
        }
    }
}
