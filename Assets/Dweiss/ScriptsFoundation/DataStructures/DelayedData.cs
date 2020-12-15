using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public class DelayedData<T>
    {
        private Dictionary<T, float> dictionary = new Dictionary<T, float>();
        private List<T> finished = new List<T>();
        private float delay;

        public DelayedData(float delay)
        {
            this.delay = delay;
        }

        public void Add(T data)
        {
            dictionary[data] = Time.time + delay;
        }

        public bool Remove(T data)
        {
            return dictionary.Remove(data);
        }

        public List<T> GetFinishedList()
        {
            foreach(var key in dictionary.Keys)
            {
                if(Time.time <= dictionary[key])
                {
                    finished.Add(key);
                }
            }

            for(int i=0; i < finished.Count; ++i)
            {
                dictionary.Remove(finished[i]);
            }
            if(finished.Count > 0)
            {
                var ret = finished;
                finished = new List<T>();
                return ret;
            }
            return finished;
        }

    }
}