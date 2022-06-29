// using System.Collections.Generic;
// using UnityEngine;
// using System;

// namespace Common.DB
// {
//     public class Copyable<T>
//     {
//         public T Copy()
//         {
//             return (T)this.MemberwiseClone();
//         }
//     }

//     public class Storage<K, V> 
//     {
//         private Dictionary<K, V> dict = new Dictionary<K, V>();
//         private string tableName = "";
//         private string keyName = "";
//         protected Storage(string _tableName, string _keyName)
//         {
//             tableName = _tableName;
//             keyName = _keyName;
//         }
//         public V this[K key]
//         {
//             get
//             {
//                 try
//                 {
//                     return dict[key];
//                 }
//                 catch (KeyNotFoundException)
//                 {
//                     var results = Query.Instance.SelectFrom<V>(tableName, $"{keyName}={key}").results;
//                     if (results != null && results.Length > 0)
//                     {
//                         dict[key] = results[0];
//                         return dict[key];
//                     }
//                     else
//                     {
//                         return default;
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     Debug.LogError(e);
//                     return default;
//                 }
//             }
//         }
//     }
// }