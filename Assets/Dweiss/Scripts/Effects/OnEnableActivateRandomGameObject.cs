using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using Dweiss.ReflectionWrapper;

namespace Dweiss
{
    public class OnEnableActivateRandomGameObject : MonoBehaviour
    {
        //public UnityEngine.Events.UnityEvent u;
        public GameObject[] randomEnable;
        
        void OnEnable()
        {
            randomEnable[Random.Range(0, randomEnable.Length)].SetActive(true);

            //var type = typeof(GameObject);
            //var bf = Dweiss.ReflectionWrapper.ReflectionUtils.PrivateBindingFlags;
            //var propertyInfos = type.GetMethod(u.GetPersistentMethodName(0), bf);
           // Debug.LogFormat(">>>>>>> {0} >> {1}", u.GetPersistentMethodName(0), u.GetPersistentTarget(0));
            //ReflectionUtils.SetFunc(randomEnableAfter[0], u.GetPersistentMethodName(0), value, Dweiss.ReflectionWrapper.ReflectionUtils.PublicBindingFlags);

        }




    }
}