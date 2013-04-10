using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ControlLibrary
{
    //internal interface IWeakEventListener
    //{
    //    void ReceiveEvent(object sender, object e);
    //}

    //internal class WeakEventHandler<TArgs> where TArgs : EventArgs
    //{
    //    private WeakReference eventListener;
    //    private WeakReference eventSender;
    //    private string eventName;
    //    private EventInfo eventInfo;
    //    private Delegate eventDelegate;

    //    public WeakEventHandler(object sender, IWeakEventListener listener, string eventName)
    //    {
    //        this.eventListener = new WeakReference(listener);
    //        this.eventSender = new WeakReference(sender);
    //        this.eventName = eventName;

    //        this.Update(sender, true);
    //    }

    //    public void Unsubscribe()
    //    {
    //        if (this.eventSender == null || !this.eventSender.IsAlive)
    //        {
    //            return;
    //        }

    //        this.Update(this.eventSender.Target, false);
    //        this.eventSender = null;
    //        this.eventInfo = null;
    //        this.eventDelegate = null;
    //    }

    //    private void Update(object sender, bool subscribe)
    //    {
    //        switch (this.eventName)
    //        {
    //            case MatListSource.CollectionChangedEventName:
    //                if (subscribe)
    //                {
    //                    (sender as INotifyCollectionChanged).CollectionChanged += this.OnCollectionChanged;
    //                }
    //                else
    //                {
    //                    (sender as INotifyCollectionChanged).CollectionChanged -= this.OnCollectionChanged;
    //                }
    //                break;
    //            case MatListSource.CurrentChangedEventName:
    //                if (subscribe)
    //                {
    //                    (sender as ICollectionView).CurrentChanged += this.OnCurrentChanged;
    //                }
    //                else
    //                {
    //                    (sender as ICollectionView).CurrentChanged -= this.OnCurrentChanged;
    //                }
    //                break;
    //            case MatListSource.PropertyChangedEventName:
    //                if (subscribe)
    //                {
    //                    (sender as INotifyPropertyChanged).PropertyChanged += this.OnPropertyChanged;
    //                }
    //                else
    //                {
    //                    (sender as INotifyPropertyChanged).PropertyChanged -= this.OnPropertyChanged;
    //                }
    //                break;
    //            default:
    //                if (subscribe)
    //                {
    //                    //this.eventInfo = sender.GetType().GetEvent(this.eventName);
    //                    this.eventInfo = sender.GetType().GetTypeInfo().GetDeclaredEvent(this.eventName);
    //                    if (this.eventInfo != null)
    //                    {
    //                        //this.eventDelegate = Delegate.CreateDelegate(this.eventInfo.EventHandlerType, this, "OnEvent");
    //                        //MethodInfo.CreateDelegate
    //                        this.eventDelegate = sender.GetType().GetTypeInfo().GetDeclaredMethod(this.eventName).CreateDelegate(this.eventInfo.EventHandlerType, "OnEvent");
    //                        this.eventInfo.AddEventHandler(sender, this.eventDelegate);
    //                    }
    //                }
    //                else if (this.eventInfo != null)
    //                {
    //                    this.eventInfo.RemoveEventHandler(sender, this.eventDelegate);
    //                }
    //                break;
    //        }
    //    }

    //    private void OnEvent(object sender, TArgs e)
    //    {
    //        this.ProcessEvent(sender, e);
    //    }

    //    private void ProcessEvent(object sender, object e)
    //    {
    //        if (this.eventListener.IsAlive)
    //        {
    //            (this.eventListener.Target as IWeakEventListener).ReceiveEvent(sender, e);
    //        }
    //        else
    //        {
    //            this.Unsubscribe();
    //        }
    //    }

    //    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        this.ProcessEvent(sender, e);
    //    }

    //    private void OnCurrentChanged(object sender, object e)
    //    {
    //        this.ProcessEvent(sender, e);
    //    }

    //    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        this.ProcessEvent(sender, e);
    //    }
    //}

    internal interface IWeakEventListener
    {
        void ReceiveEvent(object sender, EventArgs e);
    }

    internal class WeakEventHandler<TArgs> where TArgs : EventArgs
    {
        private WeakReference eventListener;
        private WeakReference eventSender;
        private string eventName;
        private EventInfo eventInfo;
        private Delegate eventDelegate;

        public WeakEventHandler(object sender, IWeakEventListener listener, string eventName)
        {
            this.eventListener = new WeakReference(listener);
            this.eventSender = new WeakReference(sender);
            this.eventName = eventName;

            this.Update(sender, true);
        }

        public void Unsubscribe()
        {
            if (this.eventSender == null || !this.eventSender.IsAlive)
            {
                return;
            }

            this.Update(this.eventSender.Target, false);
            this.eventSender = null;
            this.eventInfo = null;
            this.eventDelegate = null;
        }

        private void Update(object sender, bool subscribe)
        {
            switch (this.eventName)
            {
                case MatListSource.CollectionChangedEventName:
                    if (subscribe)
                    {
                        (sender as INotifyCollectionChanged).CollectionChanged += this.OnCollectionChanged;
                    }
                    else
                    {
                        (sender as INotifyCollectionChanged).CollectionChanged -= this.OnCollectionChanged;
                    }
                    break;
                case MatListSource.CurrentChangedEventName:
                    if (subscribe)
                    {
                        (sender as ICollectionView).CurrentChanged += this.OnCurrentChanged;
                    }
                    else
                    {
                        (sender as ICollectionView).CurrentChanged -= this.OnCurrentChanged;
                    }
                    break;
                case MatListSource.PropertyChangedEventName:
                    if (subscribe)
                    {
                        (sender as INotifyPropertyChanged).PropertyChanged += this.OnPropertyChanged;
                    }
                    else
                    {
                        (sender as INotifyPropertyChanged).PropertyChanged -= this.OnPropertyChanged;
                    }
                    break;
                default:
                    if (subscribe)
                    {
                        //this.eventInfo = sender.GetType().GetEvent(this.eventName);
                        this.eventInfo = sender.GetType().GetTypeInfo().GetDeclaredEvent(this.eventName);
                        if (this.eventInfo != null)
                        {
                            //this.eventDelegate = Delegate.CreateDelegate(this.eventInfo.EventHandlerType, this, "OnEvent");
                            //MethodInfo.CreateDelegate
                            this.eventDelegate = sender.GetType().GetTypeInfo().GetDeclaredMethod(this.eventName).CreateDelegate(this.eventInfo.EventHandlerType, "OnEvent");
                            this.eventInfo.AddEventHandler(sender, this.eventDelegate);
                        }
                    }
                    else if (this.eventInfo != null)
                    {
                        this.eventInfo.RemoveEventHandler(sender, this.eventDelegate);
                    }
                    break;
            }
        }

        private void OnEvent(object sender, TArgs e)
        {
            this.ProcessEvent(sender, e);
        }

        private void ProcessEvent(object sender, EventArgs e)
        {
            if (this.eventListener.IsAlive)
            {
                (this.eventListener.Target as IWeakEventListener).ReceiveEvent(sender, e);
            }
            else
            {
                this.Unsubscribe();
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ProcessEvent(sender, e);
        }

        private void OnCurrentChanged(object sender, object e)
        {
            this.ProcessEvent(sender, e as EventArgs);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ProcessEvent(sender, e);
        }
    }
}
