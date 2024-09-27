using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityFramework.CustomDictionary;

[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
[CustomPropertyDrawer(typeof(QuaternionMyClassDictionary))]
[CustomPropertyDrawer(typeof(IntMyClassDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
[CustomPropertyDrawer(typeof(QuaternionListStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }
