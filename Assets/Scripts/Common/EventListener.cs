using System.Collections.Generic;
using System;

namespace Common
{
    public class EventListener<T>
    {
        public delegate void MyAction(ref T value);

        public Action<T> actions = null;

        private MyAction refActions = null;

        public void AddListener(Action<T> callback) { actions += callback; }
        public void RemoveListener(Action<T> callback) { actions -= callback; }
        public void RemoveAllListeners() { actions = null; }

        public void AddRefListener(MyAction callback) { refActions += callback; }
        public void RemoveRefListener(MyAction callback) { refActions -= callback; }
        public void RemoveAllRefListeners() { refActions = null; }

        public void Invoke(T value)
        {
            actions?.Invoke(value);
        }

        public void RefInvoke(ref T value)
        {
            refActions?.Invoke(ref value);
        }
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