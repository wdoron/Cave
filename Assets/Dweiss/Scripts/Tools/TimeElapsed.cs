using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeElapsed : MonoBehaviour {


    public class Schedule {
        public float startTime;
        public float endTime;

        public float ElapsedTime
        {
            get { return endTime - startTime; }
        }
    }

    public Dictionary<string, Schedule> elapsedtimers = new Dictionary<string, Schedule>();

    [System.Serializable]
    public class TimerEvent : UnityEngine.Events.UnityEvent<string, float> { }

    public TimerEvent onTimeElapesd;

    private string lastActive;
    private int counter = 0;


    public void StartTime()
    {
        StartTime((++counter).ToString());
    }
    public void StartTime(string id)
    {
        lastActive = id;
        elapsedtimers[id] = new Schedule() { startTime = Time.timeSinceLevelLoad };
    }

    public void StopTime(string id)
    {
        elapsedtimers[id].endTime = Time.timeSinceLevelLoad;
        onTimeElapesd.Invoke(id, elapsedtimers[id].ElapsedTime);

    }

    public void StopTime()
    {
        StopTime(lastActive);
    }

    

}
