using System.Collections.Generic;
using System;

namespace Common
{
    public class EventListener<T>
    {
        private Action<T> actions = null;
        public void AddListener(Action<T> callback) { actions += callback; }
        public void RemoveListener(Action<T> callback) { actions -= callback; }
        public void RemoveAllListeners() { actions = null; }

        public void Invoke(ref T value)
        {
            actions?.Invoke(value);
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
        public EventListener<T> changed = new EventListener<T>();
    }
}