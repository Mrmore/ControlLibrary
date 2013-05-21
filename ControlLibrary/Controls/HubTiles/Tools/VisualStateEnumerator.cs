using System.Collections;
using System.Collections.Generic;

namespace ControlLibrary
{
    internal class VisualStateEnumerator : IEnumerator<string>
    {
        private string[] states;
        private IEnumerator enumerator;

        public VisualStateEnumerator(string[] states)
        {
            this.enumerator = states.GetEnumerator();
            this.States = states;
        }

        public string[] States
        {
            get
            {
                return this.states;
            }

            set
            {
                this.states = value;
                this.enumerator = this.states.GetEnumerator();
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.enumerator.Current;
            }
        }

        public string Current
        {
            get
            {
                return (string)this.enumerator.Current;
            }
        }

        public void Dispose()
        {
        }
        
        public void MoveTo(string state)
        {
            do
            {
                this.MoveNext();
            }
            while (this.Current != state);
        }

        public bool MoveNext()
        {
            if (!this.enumerator.MoveNext())
            {
                this.enumerator = this.states.GetEnumerator();
                this.enumerator.MoveNext();
            }

            return true;
        }

        public void Reset()
        {
            this.enumerator.Reset();
        }
    }
}
