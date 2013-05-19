using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestDemoApp.Model;

namespace TestDemoApp.ViewModel
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Input;

    public class PersonViewModel : INotifyPropertyChanged
    {
        private Person person;
        private bool allowFaceToggling;
        private string lastClickEvent;

        private int nextChangeCount;

        private int changeCount;

        public event PropertyChangedEventHandler PropertyChanged;

        public PersonViewModel()
        {
            this.AllowFaceToggling = true;
            this.MakeHappy = new ActionCommand(() => this.Person.IsHappy = true);
            this.MakeSad = new ActionCommand(() => this.Person.IsHappy = false);
            this.ToggleHappySad = new ActionCommand(OnHappySadToggled);
            this.Person = new Person { Name = "Joe Bloggs", IsHappy = true };
            this.NextChangeCount = 1;
        }

        private void OnHappySadToggled(object obj)
        {
            this.Person.IsHappy = !this.Person.IsHappy;

            var eventData = obj as TappedRoutedEventArgs;
            if (eventData != null)
            {
                this.LastClickEvent = eventData.GetPosition(eventData.OriginalSource as UIElement).ToString();
            }
        }

        public string LastClickEvent
        {
            get { return this.lastClickEvent; }
            set
            {
                this.lastClickEvent = value;
                this.OnPropertyChanged();
            }
        }

        public Person Person
        {
            get { return this.person; }
            set
            {
                this.person = value;
                this.OnPropertyChanged();
            }
        }

        public bool AllowFaceToggling
        {
            get { return this.allowFaceToggling; }
            set
            {
                this.allowFaceToggling = value;
                this.OnPropertyChanged();
            }
        }

        public int NextChangeCount
        {
            get { return this.nextChangeCount; }
            set
            {
                this.nextChangeCount = value;
                this.OnPropertyChanged();
            }
        }

        public int ChangeCount
        {
            get { return this.changeCount; }
            set
            {
                this.NextChangeCount = value + 1;
                this.changeCount = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand MakeHappy
        {
            get;
            private set;
        }

        public ICommand MakeSad
        {
            get;
            private set;
        }

        public ICommand ToggleHappySad
        {
            get;
            private set;
        }

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
