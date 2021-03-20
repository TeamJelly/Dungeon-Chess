using System.Collections.Generic;
using System;

namespace Common
{
    public class EventListener<T>
    {
        private Action<T> invoke = null;
        public Action<T> Invoke => invoke;
        public void AddListener(Action<T> callback) { invoke += callback; }
        public void RemoveListener(Action<T> callback) { invoke -= callback; }
        public void RemoveAllListener(Action<T> callback) { invoke = null; }
    }

    public class UpAndDownEventListener<T>
    {
        public EventListener<T> changed = new EventListener<T>();
        public EventListener<T> up = new EventListener<T>();
        public EventListener<T> down = new EventListener<T>();
    }
}