using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
//


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
public class IntMyClassDictionary : SerializableDictionary<int, MyClass> { }
[Serializable]
public class QuaternionListStorage : SerializableDictionary.Storage<List<MyClass>> { }
[Serializable]
public class QuaternionMyClassDictionary : SerializableDictionary<Quaternion, List<MyClass>, QuaternionListStorage> { }