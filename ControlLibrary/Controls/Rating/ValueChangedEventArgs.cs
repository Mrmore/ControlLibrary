using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ControlLibrary
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
