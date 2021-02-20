using System.Collections.Generic;
using UnityEngine;
using System;

namespace Common.DB
{
    public class Storage<K, V> 
    {
        private Dictionary<K, V> dict = new Dictionary<K, V>();
        protected Storage() { }
        public V this[K key]
        {
            get
            {
                try
                {
                    return dict[key];
                }
                catch (KeyNotFoundException)
                {
                    return default;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return default;
                }
            }
            set => dict[key] = value;
        }
    }
}