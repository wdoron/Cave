using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLog : MonoBehaviour {

    public TextMesh txt;

    public TextMesh stackTxt;

    public string logFormat = "{0} >> {1}";
    public int lineCount = 5;
    private Queue<string> logLines = new Queue<string>();

    [System.Serializable]
    public class LogInfo : UnityEngine.Events.UnityEvent<string> { };
    [System.Serializable]
    public class FullLogInfo : UnityEngine.Events.UnityEvent<string> { };
    [System.Serializable]
    public class LogInfoAndStack : UnityEngine.Events.UnityEvent<string,string> { };


    public LogInfo onLogInfo;
    public FullLogInfo onFullLogInfo;
    public LogInfoAndStack onLogInfoAndStack;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        
    }
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        onLogInfo.Invoke(type + "-" + logString);
        onFullLogInfo.Invoke(type + "-" + logString + "\n\n" + stackTrace);
        onLogInfoAndStack.Invoke(type + "-" + logString, stackTrace);

        if (stackTxt) stackTxt.text = stackTrace;
        if (txt) AddLine(logString, type);
    }

    private void AddLine(string logString, LogType type)
    {
        while (logLines.Count > lineCount + 1) logLines.Dequeue();
        logLines.Enqueue(string.Format(logFormat, type, logString));
        
        txt.text = "";
        foreach (var q in logLines)
        {
            txt.text += q + "\n";

        }
    }
}
