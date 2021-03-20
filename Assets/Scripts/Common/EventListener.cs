using UnityEngine;
using System.Collections;
using System;

namespace Common
{
    public class EventListener
    {
        private Action invoke = null;
        public Action Invoke => invoke;
        public void AddListener(Action callback) { invoke += callback; }
        public void RemoveListener(Action callback) { invoke -= callback; }
        public void RemoveAllListener(Action callback) { invoke = null; }

    }
}