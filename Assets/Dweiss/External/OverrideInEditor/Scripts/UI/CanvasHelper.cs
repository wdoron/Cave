using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHelper : MonoBehaviour {

    public bool showAll = false;

    [SerializeField] private Button showHideButton;
    [SerializeField] private Button next;
    [SerializeField] private Button prev;
    [SerializeField] private int numberOfButtonsInGroup;

    private Vector3 startPos;

    private List<GameObject> buttons = new List<GameObject>();

    private int firstButtonIndex = 0;

    void Awake () {
        //showHideButton.onClick.AddListener(FlipShow);
        //next.onClick.AddListener(NextGroup);
        //prev.onClick.AddListener(PrevGroup);

        foreach (Transform t in transform.parent) if (t != transform) buttons.Add(t.gameObject);

        for (int i = 0; i < buttons.Count; ++i) { buttons[i].SetActive(true); buttons[i].SetActive(false); }
    }

    private void Start()
    {
        showAll = !showAll;
        FlipShow();
    }


    public void FlipShow()
    {
        showAll = !showAll;

        next.gameObject.SetActive(showAll);
        prev.gameObject.SetActive(showAll);

        if (showAll)
        {
            UpdateButtons();
        }
        else
        {
            for (int i = 0; i < buttons.Count; ++i) buttons[i].SetActive(false);
        }
    }

    public void NextGroup()
    {
        firstButtonIndex = Mathf.Min(buttons.Count/ numberOfButtonsInGroup, firstButtonIndex + numberOfButtonsInGroup);
        UpdateButtons();
    }


    public void PrevGroup()
    {
        firstButtonIndex = Mathf.Max(0, firstButtonIndex - numberOfButtonsInGroup);
        UpdateButtons();
    }

    // Update is called once per frame
    void UpdateButtons() {
        for (int i = 0; i < buttons.Count; ++i) buttons[i].SetActive(false);

        var sizeD = showHideButton.GetComponent<RectTransform>().sizeDelta;

        int buttonsCount = 0;
        for (int i = firstButtonIndex; i < buttons.Count && i < numberOfButtonsInGroup; ++i)
        {
            buttons[i].SetActive(true);
            var p = new Vector2(sizeD.x / 2 * 1.1f, -sizeD.y * (.5f * 1.1f + 1) - sizeD.y * (buttonsCount++));
            buttons[i].GetComponent<RectTransform>().anchoredPosition = p;
        }
    }
}
