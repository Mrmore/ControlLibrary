using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

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
    }
}
