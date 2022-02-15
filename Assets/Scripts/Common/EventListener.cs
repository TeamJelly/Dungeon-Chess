using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Common
{
    public class EventListener<T>
    {
        public List<Func<T, Task<T>>> myActions = new List<Func<T, Task<T>>>();

        public void AddListener(Func<T, Task<T>> callback) { myActions.Add(callback); }
        public void RemoveListener(Func<T, Task<T>> callback) { myActions.Remove(callback); }
        public void RemoveAllListeners() { myActions.Clear(); }

        public async Task<T> Invoke(T value)
        {
            T _return = value;

            for (int i = myActions.Count - 1; i >= 0; i--)
                _return = await myActions[i].Invoke(_return);

            return _return;
        }
    }

    public class TimeEventListener<T>
    {
        public EventListener<T> before = new EventListener<T>();
        public EventListener<T> after = new EventListener<T>();
    }
}