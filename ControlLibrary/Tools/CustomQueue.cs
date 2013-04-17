using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Tools
{
    public class CustomQueue<T>
    {
        private T[] _Array;
        private int current, setter;

        public CustomQueue(int size)
        {
            if (size <= 0)
                throw new IndexOutOfRangeException();
            _Array = new T[size];
            current = 0;
            setter = 0;
        }

        public void Enqueue(T item)
        {
            _Array[setter] = item;
            setter++;
            if (setter == _Array.Length)
                setter = 0;
            if (setter == current)
                current++;
            if (current == _Array.Length)
                current = 0;
        }

        public T Dequeue()
        {
            if (current == setter)
                return default(T);
            T item = _Array[current];
            _Array[current] = default(T);
            current++;
            if (current == _Array.Length)
                current = 0;
            return item;
        }
    }
}
