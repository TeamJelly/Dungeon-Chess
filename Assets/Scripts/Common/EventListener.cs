using System.Collections.Generic;
using System;

namespace Common
{
    public class EventListener<T>
    {
        public delegate T MyAction(T value);

        private List<MyAction> myActions = new List<MyAction>();

        //public Action<T> actions = null;
        //private MyAction refActions = null;

        public void AddListener(MyAction callback) { myActions.Add(callback); }
        public void RemoveListener(MyAction callback) { myActions.Remove(callback); }
        public void RemoveAllListeners() { myActions.Clear(); }

        //public void AddRefListener(MyAction callback) { refActions += callback; }
        //public void RemoveRefListener(MyAction callback) { refActions -= callback; }
        //public void RemoveAllRefListeners() { refActions = null; }

        public T Invoke(T value)
        {
            T _return = value;

            for (int i = myActions.Count - 1; i >= 0; i--)
                _return = myActions[i].Invoke(_return);

            return _return;
        }

        //public void RefInvoke(ref T value)
        //{
        //    refActions?.Invoke(ref value);
        //}
    }

    //public class UpAndDownEventListener<T>
    //{
    //    public EventListener<T> changed = new EventListener<T>();
    //    public EventListener<T> up = new EventListener<T>();
    //    public EventListener<T> down = new EventListener<T>();
    //}

    public class TimeEventListener<T>
    {
        public EventListener<T> before = new EventListener<T>();
        public EventListener<T> after = new EventListener<T>();
    }
}