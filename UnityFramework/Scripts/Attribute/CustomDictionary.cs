/*
* FileName:          CustomDictionary
* CompanyName:       
* Author:            relly
* Description:       
* 
*/

using System;
using System.Collections.Generic;

namespace UnityFramework
{
    public partial class CustomDictionary
    {
        [Serializable]
        public class StringStringDictionary : SerializableDictionary<string, string> { }
        [Serializable]
        public class IntStringDictionary : SerializableDictionary<int, string> { }
        [Serializable]
        public class StringIntDictionary : SerializableDictionary<string, int> { }
        [Serializable]
        public class IntIntDictionary : SerializableDictionary<int, int> { }

        [Serializable]
        public class StringListStorage : SerializableDictionary.Storage<List<string>> { }
        [Serializable]
        public class StringStringListDictionary : SerializableDictionary<string, List<string>, StringListStorage> { }

    }
}