using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Dweiss.Common
{
    public class ConsistantSessionSettings : MonoBehaviour
    {

        public List<Data> consistantData;

        [System.Serializable]
        public class Data
        {
            public enum DataType
            {
                Int,
                Float,
                String,
                Bool,
                //GeneralObject,
                
            }
            public string key;

            public DataType dataType;

            public int valueInt;
            public float valueFloat;
            public string valueString;
            public bool valueBool;


            public object Value
            {
                get
                {
                    switch (dataType)
                    {
                        case DataType.Bool: return valueBool;
                        case DataType.Int: return valueInt;
                        case DataType.Float: return valueFloat;
                        case DataType.String: return valueString;
                        default: throw new System.NotSupportedException("data type " + dataType + " not supported for " + key);
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("({0} ({1}),{2})", key, dataType, Value);
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

#if UNITY_EDITOR
            for(int i=0; i < consistantData.Count; ++i)
            {
                for (int j = 0; j < consistantData.Count; ++j)
                {
                    if (i == j) continue;

                    if(consistantData[i].key == consistantData[j].key)
                    {
                        Debug.LogErrorFormat("Multiple object with same keys in data: ({0}) == ({1}) {2} == {3} ", 
                            i, j, consistantData[i], consistantData[j]);
                    }
                }
            }

#endif
        }

        
    }
}