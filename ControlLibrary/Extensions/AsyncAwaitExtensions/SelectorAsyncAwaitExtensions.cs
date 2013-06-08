using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace ControlLibrary.Extensions
{
    /// <summary>
    /// Contains an extension method for awaiting selection changes on a Selector control, such as a ListBox, GridView, ListView
    /// </summary>
    public static class SelectorAsyncAwaitExtensions
    {
        /// <summary>
        /// Waits for a selection change event in a Selector (eg. a ListBox, a GridView, a ListView).
        /// </summary>
        public static async Task<SelectionChangedEventArgs> WaitForSelectionChangedAsync(this Selector selector)
        {
            var tcs = new TaskCompletionSource<SelectionChangedEventArgs>();

            // Need to set it to noll so that the compiler does not
            // complain about use of unassigned local variable.
            SelectionChangedEventHandler sceh = null;

            sceh = (s, e) =>
            {
                selector.SelectionChanged -= sceh;
                tcs.SetResult(e);
            };

            selector.SelectionChanged += sceh;
            return await tcs.Task;
        }
    }
}
