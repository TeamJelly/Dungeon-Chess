using UnityEngine;
using System.Collections;
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
}