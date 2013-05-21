using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HttpCacheTestDemo
{
    [DataContract]
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [DataContract]
    public static class PropertyChangedBaseEx
    {
        public static void NotifyPropertyChanged<T, TProperty>(this T propertyChangedBase, Expression<Func<T, TProperty>> expression) where T : PropertyChangedBase
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                propertyChangedBase.NotifyPropertyChanged(propertyName);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

