using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoolButton : MonoBehaviour {
    [SerializeField] private Button callBackButton;
    [SerializeField] private Button boolValueButton;

    [SerializeField] private Text txtName;

    private Image boolValueImage;

    public bool flipValueOnClick;

    private bool booleanVal;

    public System.Action<bool> onValueChange;

    public void SetName(string newName)
    {
        txtName.text = newName;
    }

    public void ChangeValue(bool b)
    {
        booleanVal = b;
        SetColor(b);
    }

    private void SetColor(bool b)
    {
        boolValueImage.color = b ? Color.green : Color.red;
    }

    void Awake() {
        //var wasActive = boolValueButton.gameObject.activeSelf;
        //if (wasActive == false) boolValueButton.gameObject.SetActive(true);
        boolValueImage = boolValueButton.GetComponentInChildren<Image>();
        //if (wasActive == false) boolValueButton.gameObject.SetActive(false);

        boolValueButton.onClick.AddListener(() => ChangeValue(!booleanVal));
        callBackButton.onClick.AddListener(() => {
            if (onValueChange != null) onValueChange(booleanVal);
            else Debug.Log(name + " Button pressed");
            if (flipValueOnClick) ChangeValue(!booleanVal);
        }
        );
        //Debug.Log(name + " Setup " + boolValueImage);
    }
	
}
