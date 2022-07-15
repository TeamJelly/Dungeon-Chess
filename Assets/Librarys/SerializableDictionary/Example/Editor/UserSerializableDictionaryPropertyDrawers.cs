using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringIntDictionary))]
[CustomPropertyDrawer(typeof(StringSkillDictionary))]
[CustomPropertyDrawer(typeof(StringSpriteDictionary))]
[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }
