/*
* FileName:          CustomDictionaryDrawer
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using UnityEditor;
using static UnityFramework.CustomDictionary;

namespace UnityFramework.Editor
{
    [CustomPropertyDrawer(typeof(StringStringDictionary))]
    [CustomPropertyDrawer(typeof(IntStringDictionary))]
    [CustomPropertyDrawer(typeof(StringIntDictionary))]
    [CustomPropertyDrawer(typeof(IntIntDictionary))]
    [CustomPropertyDrawer(typeof(StringStringListDictionary))]
    public class CustomDictionaryDrawer : SerializableDictionaryPropertyDrawer { }


    [CustomPropertyDrawer(typeof(StringListStorage))]
    public class CustomDictionaryStorageDrawer : SerializableDictionaryStoragePropertyDrawer { }


}