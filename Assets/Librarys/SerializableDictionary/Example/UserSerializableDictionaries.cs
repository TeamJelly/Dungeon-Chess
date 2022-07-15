using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Model;


[Serializable]
public class StringIntDictionary : SerializableDictionary<string, int> { }

[Serializable]
public class StringStateEffectDictionary : SerializableDictionary<string, StateEffect> { }

[Serializable]
public class StringUnitDictionary : SerializableDictionary<string, Unit> { }

[Serializable]
public class StringArtifactDictionary : SerializableDictionary<string, Artifact> { }

[Serializable]
public class StringSkillDictionary : SerializableDictionary<string, Skill> { }

[Serializable]
public class StringSpriteDictionary : SerializableDictionary<string, Sprite> { }

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string> { }

[Serializable]
public class ObjectColorDictionary : SerializableDictionary<UnityEngine.Object, Color> { }

[Serializable]
public class ColorArrayStorage : SerializableDictionary.Storage<Color[]> { }

[Serializable]
public class StringColorArrayDictionary : SerializableDictionary<string, Color[], ColorArrayStorage> { }

[Serializable]
public class MyClass
{
    public int i;
    public string str;
}

[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, MyClass> { }