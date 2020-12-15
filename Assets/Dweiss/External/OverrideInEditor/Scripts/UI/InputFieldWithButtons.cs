using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldWithButtons : MonoBehaviour {
    [SerializeField] private Button increase;
    [SerializeField] private Button decrease;
    [SerializeField] private InputField input;
    [SerializeField] private Text txtName;

    public float valueChangeOnPress;

    [SerializeField] private string format = "###,##0.##";

    [SerializeField] private bool eventOnlyOnEdit;


    public System.Action<float> onValueChange;



    private void Reset()
    {
        input = GetComponentInChildren<InputField>();
        txtName = GetComponentInChildren<Text>();

    }

    public void SetName(string newName)
    {
        txtName.text = newName;
    }

    public void ChangeValue(float num)
    {
        input.text = num.ToString(format);
    }


    void Start () {
        increase.onClick.AddListener(IncreaseButton);
        decrease.onClick.AddListener(DecreaseButton);

        input.onValueChanged.AddListener(ValueChanged);
        input.onEndEdit.AddListener(EditChanged);

        //onValueChange += (v) => Debug.Log("New value "+ v); 
    }

    private void EditChanged(string arg0)
    {
        if (eventOnlyOnEdit && onValueChange != null) onValueChange(float.Parse(arg0));
    }

    private void ValueChanged(string arg0)
    {
        if (eventOnlyOnEdit == false && onValueChange != null) onValueChange(float.Parse(arg0));
    }

    private void IncreaseButton()
    {
        // Debug.Log("value change " + float.Parse(input.text) + " with " + valueChangeOnPress);
        var newValue = float.Parse(input.text) + valueChangeOnPress;
        input.text = newValue.ToString(format);
       // if (onValueChange != null) onValueChange(newValue);
    }

    private void DecreaseButton()
    {
        var newValue = float.Parse(input.text) - valueChangeOnPress;
        input.text = newValue.ToString(format);
       // if (onValueChange != null) onValueChange(newValue);
    }

   

}
