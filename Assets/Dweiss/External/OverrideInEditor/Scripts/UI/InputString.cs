using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputString : MonoBehaviour {

    [SerializeField] private InputField input;
    [SerializeField] private Text txtName;

    //[SerializeField] private bool fireEventOnEnterKey;

    public System.Action<string> onValueChange;



    private void Reset()
    {
        input = GetComponentInChildren<InputField>();
        txtName = GetComponentInChildren<Text>();

    }

    public void SetName(string newName)
    {
        txtName.text = newName;
    }

    public void ChangeValue(string newValue)
    {
        input.text = newValue;
    }


    void Start()
    {

        input.onValueChanged.AddListener(ValueChanged);
        input.onEndEdit.AddListener(EditChanged);

    }
    private float editTime = 0;
    public float delayedAction = 1;
    private Coroutine coroutine;
    private string argument;
    private IEnumerator DelayStart()
    {
        while (Time.realtimeSinceStartup - editTime < delayedAction) yield return new WaitForSecondsRealtime(0);
        try
        {

            if(onValueChange != null) onValueChange(argument);
        }
        catch (System.Exception e) { Debug.LogError("Error on call: " + e); }
        coroutine = null;
    }

    private void EditChanged(string arg0)
    {
        if (coroutine != null)
        {
            StopAllCoroutines();
            coroutine = null;
        }
        if (onValueChange != null) onValueChange(arg0);
    }

    private void ValueChanged(string arg0)
    {
        editTime = Time.realtimeSinceStartup;
        argument = arg0;
        if (coroutine == null) coroutine = StartCoroutine("DelayStart", arg0);
        //if (eventOnlyOnEdit == false && onValueChange != null) onValueChange(arg0);
    }

   


}
