using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestDemoApp.Model
{
    public class Person : INotifyPropertyChanged
    {
        private bool isHappy;
        private string name;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsHappy
        {
            get { return this.isHappy; }
            set
            {
                if (this.isHappy != value)
                {
                    this.isHappy = value;
                    this.OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
