using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
#if UNITY_EDITOR
    [System.Serializable]
    public class EditorInfoDictionaryStrFloat : EditorInfoDictionary<string, float> { }

    [System.Serializable]
    public class EditorInfoDictionaryStrInt : EditorInfoDictionary<string, int> { }

    [System.Serializable]
    public class EditorInfoDictionaryStrBool : EditorInfoDictionary<string, bool> { }
#else

    [System.Serializable]
    public class EditorInfoDictionaryStrFloat : Dictionary<string, float> { }

    [System.Serializable]
    public class EditorInfoDictionaryStrInt : Dictionary<string, int> { }

    [System.Serializable]
    public class EditorInfoDictionaryStrBool : Dictionary<string, bool> { }
#endif

#if UNITY_EDITOR

    [System.Serializable]
    public class EditorInfoDictionary<K, V> : IEnumerable, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IDictionary<K, V>
    {
        private Dictionary<K, int> keyToIndexDic = new Dictionary<K, int>();

        [SerializeField]//[Dweiss.ArrayElementTitle("Count")]
        private List<K> keysList = new List<K>();
        [SerializeField]
        private List<V> valuesList = new List<V>();

        public Dictionary<K,V> Dic
        {
            get
            {
                var ret = new Dictionary<K, V>();
                for(int i=0; i < keysList.Count; ++i)
                {
                    ret[keysList[i]] = valuesList[i];
                }
                return ret;
            }
        }
        public Dictionary<K, V>.KeyCollection Keys
        {
            get { return new Dictionary<K, V>.KeyCollection(Dic); }
        }

        public Dictionary<K, V>.ValueCollection Values
        {
            get { return new Dictionary<K, V>.ValueCollection(Dic); }
        }

        ICollection<K> IDictionary<K, V>.Keys
        {
            get
            {
                return Keys;
            }
        }

        ICollection<V> IDictionary<K, V>.Values
        {
            get { return Values; }
        }

        public V this[K key]
        {
            get {
                return valuesList[keyToIndexDic[key]];
            }
            set
            {
                int index = 0;
                if(keyToIndexDic.TryGetValue( key, out index))
                {
                    keysList[index] = key;
                    valuesList[index] = value;
                }
                else
                {
                    keyToIndexDic[key] = keysList.Count;
                    keysList.Add(key);
                    valuesList.Add(value);
                }
            }
        }

        public IEqualityComparer<K> Comparer {
            get { return EqualityComparer<K>.Default; }

        }
        public IEqualityComparer<V> ComparerValues
        {
            get { return EqualityComparer<V>.Default; }

        }
        //public KeyCollection Keys { get; }
        //public ValueCollection Values { get; }
        public int Count { get { return keysList.Count; } }

        public bool IsReadOnly
        {
            get { return false; }
        }

       

        public void Add(K key, V value)
        {
            keyToIndexDic.Add(key, keysList.Count);
            keysList.Add(key);
            valuesList.Add(value);
        }
        public void Clear()
        {
            keyToIndexDic.Clear();
            keysList.Clear();
            valuesList.Clear();
        }
        public bool ContainsKey(K key)
        {
            return keyToIndexDic.ContainsKey(key);
        }
        public bool ContainsValue(V value)
        {
            return valuesList.Contains(value);
        }
        public bool Remove(K key)
        {
            int foundKeyIndex = -1;
            for(int i=0; i < keysList.Count; ++i)
            {
                if(Comparer.Equals(keysList[i], key))
                {
                    foundKeyIndex = i;
                    keysList.RemoveAt(i);
                    valuesList.RemoveAt(i);
                    keyToIndexDic.Remove(key);
                    break;
                }
            }


            if(foundKeyIndex > -1)
            {
                for (int i = foundKeyIndex; i < keysList.Count; ++i)
                {
                    keyToIndexDic[keysList[i]] = i;
                }
                return true;
            }
            return false;

        }
        public bool TryGetValue(K key, out V value)
        {
            int index = 0;
            if(keyToIndexDic.TryGetValue(key, out index))
            {
                value = valuesList[index];
                return true;
            }
            value = default(V);
            return false;
        }

        //public Dictionary<K, V>.Enumerator GetEnumerator()
        //{
        //    return Dic.GetEnumerator();
        //}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dic.GetEnumerator();
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            int index;
            if (keyToIndexDic.TryGetValue(item.Key, out index))
            {
                return ComparerValues.Equals(valuesList[index], item.Value); 
            }
            return false;
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            for(int i=0; i < array.Length; ++i)
            {
                Add(array[i]);
            }
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return Dic.GetEnumerator();
        }
    }

    //    [System.Serializable]
    //    public class EditorDictionary<K, V> : IEnumerable, ICollection<KeyValuePair<K, V>>, IEnumerable<KeyValuePair<K, V>>, IDictionary<K, V>
    //    {
    //        [System.Serializable]
    //        public class TempDic : EditorInfoDictionary<K, V> { }

    //        public TempDic editorDic;

    //        public Dictionary<K, V> dic;

    //        public Dictionary<K, V>.KeyCollection Keys
    //        {
    //            get {
    //#if UNITY_EDITOR
    //                return editorDic.Keys;
    //#else
    //                return dic.Keys;
    //#endif
    //            }
    //        }

    //        public Dictionary<K, V>.ValueCollection Values
    //        {
    //            get {
    //#if UNITY_EDITOR
    //                return editorDic.Values;
    //#else
    //                return dic.Values;
    //#endif
    //            }
    //        }

    //        ICollection<K> IDictionary<K, V>.Keys
    //        {
    //            get
    //            {
    //#if UNITY_EDITOR
    //                return editorDic.Keys;
    //#else
    //                return dic.Keys;
    //#endif
    //            }
    //        }

    //        ICollection<V> IDictionary<K, V>.Values
    //        {
    //            get {
    //#if UNITY_EDITOR
    //                return editorDic.Values;
    //#else
    //                return dic.Values;
    //#endif
    //            }
    //        }

    //        public V this[K key]
    //        {
    //            get
    //            {
    //#if UNITY_EDITOR
    //                return editorDic[key];
    //#else
    //                return dic[key];
    //#endif
    //            }
    //            set
    //            {
    //#if UNITY_EDITOR
    //                editorDic[key] = value;
    //#else
    //                dic[key] = value;
    //#endif
    //            }
    //        }

    //        public IEqualityComparer<K> Comparer
    //        {
    //            get {
    //#if UNITY_EDITOR
    //                return editorDic.Comparer;
    //#else
    //                return dic.Comparer;
    //#endif
    //            }

    //        }
    //        public int Count { get {
    //#if UNITY_EDITOR
    //                return editorDic.Count;
    //#else
    //                return dic.Count;
    //#endif
    //            }
    //        }

    //        public bool IsReadOnly
    //        {
    //            get {
    //#if UNITY_EDITOR
    //                return editorDic.IsReadOnly;
    //#else
    //                return dic.IsReadOnly;
    //#endif
    //            }
    //        }



    //        public void Add(K key, V value)
    //        {
    //#if UNITY_EDITOR
    //            editorDic.Add(key,value);
    //#else
    //            dic.Add(key,value);
    //#endif
    //        }
    //        public void Clear()
    //        {
    //#if UNITY_EDITOR
    //            editorDic.Clear();
    //#else
    //            dic.Clear();
    //#endif
    //        }
    //        public bool ContainsKey(K key)
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.ContainsKey(key);
    //#else
    //                return dic.ContainsKey(key);
    //#endif
    //        }
    //        public bool ContainsValue(V value)
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.ContainsValue(value);
    //#else
    //                return dic.ContainsValue(value);
    //#endif
    //        }
    //        public bool Remove(K key)
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.Remove(key);
    //#else
    //                return dic.Remove(key);
    //#endif

    //        }
    //        public bool TryGetValue(K key, out V value)
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.TryGetValue(key, out value);
    //#else
    //                return dic.TryGetValue(key, out value);
    //#endif
    //        }

    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.GetEnumerator();
    //#else
    //                return dic.GetEnumerator();
    //#endif
    //        }

    //        public void Add(KeyValuePair<K, V> item)
    //        {
    //#if UNITY_EDITOR
    //            editorDic.Add(item);
    //#else
    //            dic.Add(item);
    //#endif
    //        }

    //        public bool Contains(KeyValuePair<K, V> item)
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.Contains(item);
    //#else
    //                return dic.Contains(item);
    //#endif
    //        }

    //        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
    //        {
    //#if UNITY_EDITOR
    //            editorDic.CopyTo(array,arrayIndex);
    //#else
    //            dic.CopyTo(array,arrayIndex);
    //#endif
    //        }

    //        public bool Remove(KeyValuePair<K, V> item)
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.Remove(item);
    //#else
    //                return dic.Remove(item);
    //#endif
    //        }

    //        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
    //        {
    //#if UNITY_EDITOR
    //            return editorDic.GetEnumerator();
    //#else
    //                return dic.GetEnumerator();
    //#endif
    //        }
    //    }

#endif
}