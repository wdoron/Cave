using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cheats : MonoBehaviour {

    public bool runOnlyInEditor = true;

    [System.Serializable]
    public class ClickCheat
    {
        public KeyCode key;
        public UnityEvent onClick;
        public float runAfter = -1;
    }
    public ClickCheat[] cheats;

    void Awake()
    {
        if (enabled == false) return;

#if UNITY_EDITOR == false
        //if (runOnlyInEditor) return;
        if (runOnlyInEditor) Destroy(this);
#else

#endif
        
        foreach (var cheat in cheats)
        {
            if (cheat.runAfter == -9) cheat.onClick.Invoke();

        }
    }


    public void PressButton(GameObject go)
    {
        UnityEngine.EventSystems.ExecuteEvents.Execute(
            go, new UnityEngine.EventSystems.BaseEventData(
                GameObject.FindObjectOfType<UnityEngine.EventSystems.EventSystem>()),
            UnityEngine.EventSystems.ExecuteEvents.submitHandler);
    }

    void Start () {
#if UNITY_EDITOR == false
        if (runOnlyInEditor) return;
#endif
        foreach (var cheat in cheats) {
            if (cheat.runAfter == 0) cheat.onClick.Invoke();
            
        }
        StartCoroutine(RunCheats());
	}
	
    IEnumerator RunCheats()
    {
        var sorted = cheats.ToList();
        sorted.Sort((a, b) => a.runAfter.CompareTo(b.runAfter));
        cheats = sorted.ToArray();

        float waitedAlready = 0;
        for (int i=0; i < cheats.Length; ++i)
        {
            if (cheats[i].runAfter < 0) continue;
            var wait = cheats[i].runAfter - waitedAlready;
            if (wait > 0)
            {
                yield return new WaitForSecondsRealtime(wait);
                waitedAlready += wait;
            }
            Debug.Log(Time.time + " Invoking cheats " + i);
            cheats[i].onClick.Invoke();

        }
    }


    void Update () {

#if UNITY_EDITOR == false
        if (runOnlyInEditor) return;
#endif
        foreach (var cheat in cheats) if (Input.GetKeyDown(cheat.key)) cheat.onClick.Invoke();

	}
}
