using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    [System.Serializable]
    public class EventSimple : UnityEvent { }

    [System.Serializable]
    public class EventString : UnityEvent<string> { }

    [System.Serializable]
    public class EventBool : UnityEvent<bool> { }

    [System.Serializable]
    public class EventFloat : UnityEvent<float> { }

    [System.Serializable]
    public class EventInt : UnityEvent<int> { }

    [System.Serializable]
    public class EventVector3 : UnityEvent<Vector3> { }

    [System.Serializable]
    public class EventScriptableObject : UnityEvent<ScriptableObject> { }

    [System.Serializable]
    public class EventMonoBehaviour : UnityEvent<UnityEngine.MonoBehaviour> { }


}